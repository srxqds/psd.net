/*
 * File: Assets/Editor/PSD/LayerInfo/VectorOrigination.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:41:29 AM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class VectorOrigination : LayerInfo
    {
        public Descriptor data { get; private set; }
        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            reader.BaseStream.Position += 8;
            this.data = new Descriptor(reader);
        }
    }
}
