/*
 * File: Assets/Editor/PSD/LayerInfo/VectorStrokeContent.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:41:55 AM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class VectorStrokeContent : LayerInfo
    {
        public Int32 key;
        public Int32 version { get; private set; }
        public Descriptor data { get; private set; }
        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.key = reader.ReadInt32();
            this.version = reader.ReadInt32();
            this.data = new Descriptor(reader);
        }
    }
}
