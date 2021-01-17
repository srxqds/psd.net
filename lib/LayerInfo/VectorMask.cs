/*
 * File: Assets/Editor/PSD/LayerInfo/VectorMask.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:41:09 AM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class VectorMask : LayerInfo
    {
        public int version { get; private set; }
        public bool invert { get; private set; }
        public bool notLink { get; private set; }
        public bool disable { get; private set; }

        public PathRecord[] paths { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.version = reader.ReadInt32();
            int tag = reader.ReadInt32();
            this.invert = (tag & 0x01) == 1;
            this.notLink = (tag & (0x01 << 1)) > 0;
            this.disable = (tag & (0x01 << 2)) > 0;
            //# I haven't figured out yet why this is 10 and not 8.
            int num = (dataLength - 10)/26;
            paths = new PathRecord[num];
            for (int i = 0; i < num; i++)
            {
                paths[i] = new PathRecord(reader);
            }

        }
    }
}
