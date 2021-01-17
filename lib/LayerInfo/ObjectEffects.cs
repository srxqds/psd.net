/*
 * File: Assets/Editor/PSD/LayerInfo/ObjectEffects.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:40:07 AM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class ObjectEffects : LayerInfo
    {
        public Int32 effectVersion { get; private set; }
        public Int32 descriptorVersion { get; private set; }
        public Descriptor data { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.effectVersion = reader.ReadInt32();
            this.descriptorVersion = reader.ReadInt32();
            this.data = new Descriptor(reader);
        }
    }
}
