/*
 * File: Assets/Editor/PSD/LayerInfo/LayerUnicodeName.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 5:11:06 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class UnicodeName:LayerInfo
    {
        public string name { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            this.name = reader.ReadUnicodeString();

        }
    }

}