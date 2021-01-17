/*
 * File: Assets/Editor/PSD/LayerInfo/RawLayerInfo.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 5:11:14 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class RawLayerInfo:LayerInfo
    {
        public byte[] data { get; private set; }


        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.data = reader.ReadBytes((int)dataLength);
        }

    }
}