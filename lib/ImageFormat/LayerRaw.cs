/*
 * File: Assets/Editor/PSD/ImageFormat/LayerRaw.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 8:15:38 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class LayerRaw
    {
        public static void ParseRaw(PsdBinaryReader reader, ImageData image)
        {
            for (int i=image.chanPos;i<image.chanPos + image.chan.length - 2;i++)
                image.imageDataRaw[i] = reader.ReadByte();
            image.chanPos += image.chan.length - 2;
        }
    }
}
