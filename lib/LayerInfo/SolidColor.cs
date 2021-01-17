/*
 * File: Assets/Editor/PSD/LayerInfo/SolidColor.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:40:52 AM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class SolidColor:LayerInfo
    {
        public Int32 version { get; private set; }
        public Descriptor data { get; private set; }

        public object colorData { get { return this.data["Clr "]; } }
        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.version = reader.ReadInt32();
            this.data = new Descriptor(reader);
        }
    }
}
