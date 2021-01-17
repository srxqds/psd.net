/*
 * File: Assets/Editor/PSD/ImageResouce/LayerMaskInfo.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 5:05:38 PM
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace Com.Lucky.PhotoShop
{
    public class LayerMaskInfo 
    {
        public List<Layer> layerList{ get; private set; }

        public List<LayerInfo> additionalInfos { get; private set; }

        public byte[] globalLayerMaskData { get; private set; }

        public bool mergedAlpha { get; private set; }

        public Header header { get; private set; }
        public ColorModeData colorModeData { get; private set; }

        public LayerMaskInfo(PsdBinaryReader reader,Header header,ColorModeData colorModeData)
        {
			this.header = header;
			this.colorModeData = colorModeData;
            layerList = new List<Layer>();
            additionalInfos = new List<LayerInfo>();
            var layersAndMaskLength = reader.ReadUInt32();
            if (layersAndMaskLength <= 0)
                return;

            var startPosition = reader.BaseStream.Position;
            var endPosition = startPosition + layersAndMaskLength;

            LoadLayers(reader, true);
            LoadGlobalLayerMask(reader);
            LoadAdditionalLayerInfo(reader, endPosition);
            reader.BaseStream.Position = startPosition + layersAndMaskLength;

            layerList.Reverse();
        }

        private void LoadLayers(PsdBinaryReader reader, bool hasHeader)
        {
            Int32 sectionLength = 0;
            if (hasHeader)
            {
				sectionLength = reader.ReadInt32();
                sectionLength = Util.RoundUp(sectionLength, 2);
                if (sectionLength <= 0)
                    return;
            }

            var startPosition = reader.BaseStream.Position;
            var numLayers = reader.ReadInt16();

            // If numLayers < 0, then number of layers is absolute value,
            // and the first alpha channel contains the transparency data for
            // the merged result.
            if (numLayers < 0)
            {
                mergedAlpha = true;
                numLayers = Math.Abs(numLayers);
            }
            if (numLayers == 0)
                return;

            for (int i = 0; i < numLayers; i++)
            {
                var layer = new Layer(reader,this.header,this.colorModeData);
                layerList.Add(layer);
            }

            //-----------------------------------------------------------------------

            // Load image data for all channels.
            foreach (var layer in layerList)
            {
                layer.LoadChannelImage(reader);
            }

            // Length is set to 0 when called on higher bitdepth layers.
            if (sectionLength > 0)
            {
                // Layers Info section is documented to be even-padded, but Photoshop
                // actually pads to 4 bytes.
                var endPosition = startPosition + sectionLength;
                var positionOffset = reader.BaseStream.Position - endPosition;

                if (reader.BaseStream.Position < endPosition)
                    reader.BaseStream.Position = endPosition;
            }
            
        }
        
        private void LoadGlobalLayerMask(PsdBinaryReader reader)
        {
            var maskLength = reader.ReadUInt32();
            if (maskLength <= 0)
                return;
            globalLayerMaskData = reader.ReadBytes((int)maskLength);
        }

        private void LoadAdditionalLayerInfo(PsdBinaryReader reader,long endPosition)
        {
            while (reader.BaseStream.Position < endPosition)
            {
                var info = LayerInfoFactory.Load(reader);
                additionalInfos.Add(info);

                if (info is RawLayerInfo)
                {
                    var layerInfo = (RawLayerInfo)info;
                    switch (info.key)
                    {
                        case "Layr":
                        case "Lr16":
                        case "Lr32":
                            using (var memoryStream = new MemoryStream(layerInfo.data))
                            using (var memoryReader = new PsdBinaryReader(memoryStream, reader))
                            {
                                LoadLayers(memoryReader, false);
                            }
                            break;

                        case "LMsk":
                            globalLayerMaskData = layerInfo.data;
                            break;
                    }
                }
            }
        }

    }
}
