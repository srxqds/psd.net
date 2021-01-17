/*
 * File: Assets/Editor/PSD/ImageFormat/Raw.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 8:16:03 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class Raw
    {
        public static void ParseRaw(PsdBinaryReader reader, ImageData image)
        {
            image.imageDataRaw = reader.ReadBytes(image.length);
        }
    }
}
