/*
 * File: Assets/Editor/PSD/Layer/BlendingRange.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 5:10:38 PM
 */
using UnityEngine;
using System.Collections;
using Com.Lucky.PhotoShop;
using System.Collections.Generic;

namespace Com.Lucky.PhotoShop
{
    public class BlendingRanges
    {
        /// <summary>
        /// The layer to which this channel belongs
        /// </summary>
        public Layer layer { get; private set; }

        public RangeData grayData { get; private set; }

        public List<RangeData> channelDataList { get; private set; }

        ///////////////////////////////////////////////////////////////////////////

        

        ///////////////////////////////////////////////////////////////////////////

        public BlendingRanges(PsdBinaryReader reader, Layer layer)
        {
            //Debug.WriteLine("BlendingRanges started at " + reader.BaseStream.Position.ToString(CultureInfo.InvariantCulture));

            this.layer = layer;
            var dataLength = reader.ReadInt32();
            if (dataLength <= 0)
                return;
            var startPosition = reader.BaseStream.Position;
            var endPosition = startPosition + dataLength;

            this.grayData = new RangeData(reader);

            var channelCount = (dataLength - 8)/8;
            channelDataList = new List<RangeData>();
            for (int i = 0; i < channelCount; i++)
            {
                channelDataList.Add(new RangeData(reader));
            }

            reader.BaseStream.Position = endPosition;
        }

        public class BlackWhiteData
        {
            public byte[] black;
            public byte[] white;

            public BlackWhiteData(PsdBinaryReader reader)
            {
                this.black = new byte[2];
                this.black[0] = reader.ReadByte();
                this.black[1] = reader.ReadByte();
                this.white = new byte[2];
                this.white[0] = reader.ReadByte();
                this.white[1] = reader.ReadByte();
            }
        }

        public class RangeData
        {
            public BlackWhiteData source;
            public BlackWhiteData dest;

            public RangeData(PsdBinaryReader reader)
            {
                this.source = new BlackWhiteData(reader);
                this.dest = new BlackWhiteData(reader);
            }
        }

        

        ///////////////////////////////////////////////////////////////////////////

        
    }
}
