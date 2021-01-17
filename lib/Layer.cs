/*
 * File: Assets/Editor/PSD/Layer.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 2:06:17 PM
 */

using System.Collections.Generic;
using AdvancedInspector;
using UnityEngine;

namespace Com.Lucky.PhotoShop
{
    public enum LayerType
    {
        Folder,
        Image,
        Text,
        Divider
        
    }
    public class Layer
    {
        internal Header header { get; private set; }

        internal ColorModeData colorModeData { get; private set; }

        private Rect m_Rect;
        /// <summary>
        /// The rectangle containing the contents of the layer.
        /// </summary>
        [Inspect]
        public Rect rect { get; private set;  }

        public ChannelImage channelImage { get; private set; }

        public string blendModeKey { get; private set; }

        /// <summary>
        /// 0 = transparent ... 255 = opaque
        /// </summary>
        [Inspect]
        public byte opacity { get; private set; }

        /// <summary>
        /// false = base, true = non-base
        /// </summary>
        [Inspect]
        public bool clipping { get; private set; }

        public byte flagsByte { get; private set; }

        /// <summary>
        /// If true, the layer is visible.
        /// </summary>
        [Inspect]
        public bool visible
        {
            get { return !((this.flagsByte & (0x01 << 1)) > 0); }
        }

        /// <summary>
        /// The descriptive layer name
        /// </summary>
        public string name { get; private set; }
        [Inspect]
        public BlendingRanges blendingRangesData { get; private set; }
        [Inspect]
        public MaskInfo masks { get; private set; }
        [Inspect]
        [Bypass]
        public List<LayerInfo> adjustmentInfos { get; private set; }

        ///////////////////////////////////////////////////////////////////////////
        [Inspect]
        public LayerType layType { get { return GetLayerType(); } }

        public Layer(Header header,ColorModeData colorModeData)
        {
            this.header = header;
            this.colorModeData = colorModeData;
            rect = new Rect();
            blendModeKey = BlendMode.Normal;
            adjustmentInfos = new List<LayerInfo>();
        }

        public Layer(PsdBinaryReader reader, Header header,ColorModeData colorModeData)
            : this(header,colorModeData)
        {

            LoadRectAndChannel(reader);
            LoadBlendModes(reader);
            LoadExtraDataField(reader);
        }

        private void LoadRectAndChannel(PsdBinaryReader reader)
        {
            this.rect = reader.ReadRectangle();
            //-----------------------------------------------------------------------
            // Read channel headers.  Image data comes later, after the layer header.
            this.channelImage = new ChannelImage(reader, this, this.header);
            //-----------------------------------------------------------------------
        }

        private void LoadBlendModes(PsdBinaryReader reader)
        {
            var signature = reader.ReadAsciiChars(4);
            if (signature != "8BIM")
            {
                throw (new PsdInvalidException("Invalid signature in layer header."));
            }

            blendModeKey = reader.ReadAsciiChars(4);
            opacity = reader.ReadByte();
            clipping = reader.ReadBoolean();
            flagsByte = reader.ReadByte();

            reader.ReadByte(); //padding
        }

        private void LoadExtraDataField(PsdBinaryReader reader)
        {
            //-----------------------------------------------------------------------
            // This is the total size of the MaskData, the BlendingRangesData, the 
            // Name and the AdjustmentLayerInfo.
            var extraDataSize = reader.ReadUInt32();
            var extraDataStartPosition = reader.BaseStream.Position;

            masks = new MaskInfo(reader, this);
            blendingRangesData = new BlendingRanges(reader, this);
            //legacy_layer_name
            name = reader.ReadPascalString(4);

            //-----------------------------------------------------------------------
            // Process Additional Layer Information
            long adjustmentLayerEndPos = extraDataStartPosition + extraDataSize;

            //UnityEngine.Debug.Log(reader.BaseStream.Position +","+ adjustmentLayerEndPos);
            while (reader.BaseStream.Position < adjustmentLayerEndPos)
            {
                var layerInfo = LayerInfoFactory.Load(reader);
                adjustmentInfos.Add(layerInfo);
            }
            if (reader.BaseStream.Position != adjustmentLayerEndPos)
            {
                reader.BaseStream.Position = adjustmentLayerEndPos;
            }

            foreach (var adjustmentInfo in adjustmentInfos)
            {
                switch (adjustmentInfo.key)
                {
                    case "luni":
                        name = ((UnicodeName)adjustmentInfo).name;
                        break;
                }
            }
        }

        public void LoadChannelImage(PsdBinaryReader reader)
        {
            this.channelImage.LoadPixelData(reader);
        }
        
        //多重key，用"|"分割
        public LayerInfo GetLayerInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            string[] keys = key.Split('|');
            for (int il = 0, layerCount = adjustmentInfos.Count; il < layerCount; il++)
            {
                for (int index = 0, length = keys.Length; index < length; index++)
                {
                    if (adjustmentInfos[il].key.Equals(keys[index]))
                        return adjustmentInfos[il];
                }
            }
            return null;

        }

        private LayerType GetLayerType()
        {
            LayerSectionDivider sectionDivider = GetLayerInfoByKey(LayerInfoKey.LayerSectionDivider) as LayerSectionDivider;
            if (sectionDivider != null)
            {
                if (sectionDivider.groupStart)
                    return LayerType.Folder;
                else if (sectionDivider.groupEnd)
                    return LayerType.Divider;
            }
            else if (GetLayerInfoByKey(LayerInfoKey.TypeTool) != null)
                return LayerType.Text;
            else if (name == "</Layer group>") //防止字符流校验错误,待查找字符流校验错误原因
                return LayerType.Divider;
            else if (rect.x == 0 && rect.y == 0 && rect.width == 0 && rect.height == 0) //防止字符流校验错误,待查找字符流校验错误原因
                return LayerType.Folder;            
            return LayerType.Image;
        }
        
    }
}

