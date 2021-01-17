/*
 * File: Assets/Editor/PSD/LayerInfo/LayerId.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 8/2/2015 2:38:19 PM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class LayerId : LayerInfo
    {
        public Int32 id { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.id = reader.ReadInt32();
        }
    }
}
