/*
 * File: Assets/Editor/PSD/ColorModeData.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:14:50 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class ColorModeData
    {
        public byte[] ColorData = new byte[0];
        public ColorModeData(PsdBinaryReader reader)
        {
            var paletteLength = reader.ReadUInt32();
            if (paletteLength > 0)
            {
                ColorData = reader.ReadBytes((int)paletteLength);
            }
        }
    }
}
