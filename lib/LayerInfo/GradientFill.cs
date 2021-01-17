/*
 * File: Assets/Editor/PSD/LayerInfo/GradientFill.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 8/2/2015 2:37:58 PM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class GradientFill:LayerInfo
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
