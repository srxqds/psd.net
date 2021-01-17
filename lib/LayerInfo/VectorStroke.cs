/*
 * File: Assets/Editor/PSD/LayerInfo/VectorStroke.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:41:41 AM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class VectorStroke : LayerInfo
    {
        public Int32 version { get; private set; }
        public Descriptor data { get; private set; }
        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.version = reader.ReadInt32();
            this.data = new Descriptor(reader);
        }
    }
}
