/*
 * File: Assets/Editor/PSD/LayerInfo/BlendInteriorElements.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:39:28 AM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{

    public class BlendInteriorElements : LayerInfo
    {
        public bool enabled { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.enabled = reader.ReadBoolean();
            reader.BaseStream.Position += 3;
        }
    }

}