/*
 * File: Assets/Editor/PSD/Header.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 2:06:09 PM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public enum ColorMode
    {
        BitMap,
        Grayscale,
        IndexedColor,
        RGBColor,
        CMYKColor,
        HSLColor,
        HSBColor,
        Multichannel,
        Duotone,
        LabColor,
        Gray16,
        RGB48,
        Lab48,
        CMYK64,
        DeepMultichannel,
        Duotone16
    }

    public class Header
    {
        

        //The signature of the PSD. Should be 8BPS.
        public string signature { get; private set; }

        //The version of the PSD. Should be 1.
        public Int16 version { get; private set; }

        //The number of color channels in the PSD.
        public Int16 channelCount { get; private set; }

        //The height of the PSD. Can also be accessed with `height`.
        public Int32 height { get; private set; }//

        //The width of the PSD. Can also be accessed with `width`.
        public Int32 width { get; private set; }//

        //The bit depth of the PSD.
        public Int16 bitDepth { get; private set; }//

        //The color mode of the PSD.
        public ColorMode colorMode { get; private set; }//

        public Header(PsdBinaryReader reader)
        {
            this.signature = reader.ReadAsciiChars(4);
            if (signature != "8BPS")
                throw new PsdInvalidException("The given stream is not a valid PSD file");

            version = reader.ReadInt16();
            if (version != 1)
                throw new PsdInvalidException("The PSD file has an unknown version");

            //6 bytes reserved
            reader.BaseStream.Position += 6;

            this.channelCount = reader.ReadInt16();
            if (this.channelCount < 1 || this.channelCount > 56)
                throw new PsdInvalidException("The channel count is out of supported range(1,56)");

            this.height = reader.ReadInt32();
            if (this.height < 1 || this.height > 30000)
                throw new PsdInvalidException("The row count is out of supported range(1,30000)");

            this.width = reader.ReadInt32();
            if (this.width < 1 || this.width > 30000)
                throw new PsdInvalidException("The  column count is out of supported range(1,30000)");

            this.bitDepth = reader.ReadInt16();
            if(this.bitDepth != 1 && this.bitDepth != 8 && this.bitDepth != 16 && this.bitDepth != 32)
                throw new PsdInvalidException("The depth is not a supported value");

            this.colorMode = (ColorMode)reader.ReadInt16();

        }

        
    }
}
