/*
 * File: Assets/Editor/PSD/LayerInfo/FillOpacity.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:39:42 AM
 */

using System;
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class FillOpacity : LayerInfo
    {
        public byte value { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            this.value = reader.ReadByte();
        }
    }
}
