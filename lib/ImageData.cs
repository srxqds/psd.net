/*
 * File: Assets/Editor/PSD/ImageData.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/20/2015 4:47:56 AM
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Com.Lucky.PhotoShop
{
    public class ImageData 
    {
        public ImageCompression imageCompression { get; protected set; }

        protected Header header;

        

        public int pixelCount;

        public bool hasMask;

        public float opacity;

        //字节长度
        public int length;
        //每个通道的字节长度
        public int channelLength;
        public ChannelInfo[] channelsInfo;

        public int bytePerChannel
        {
            get { return Util.BytesFromBitDepth(this.header.bitDepth); }
        }
        /// <summary>
        /// Raw image data in compressed on-disk format.
        /// </summary>
        public byte[] imageDataRaw { get; set; }

        /// <summary>
        /// Decompressed image data . right -> left , bottom -> top
        /// </summary>
        public byte[] pixelData { get; set; }

        public ImageData()
        {
        }

        public ImageData(PsdBinaryReader reader,Header header)
        {
            this.header = header;
            this.width = this.header.width;
            this.height = this.header.height;
            this.channelCount = this.header.channelCount;
            Init();
            CalculateLength();
            this.opacity = 1.0f;
            this.hasMask = false;
            this.SetChannelsInfo();
			this.imageCompression = (ImageCompression)reader.ReadInt16 ();
			ParseImageData (reader);
			ProcessImageData ();
        }

        public int bitDepth;
        public int width;
        public int height;
        public int channelCount;
        public int chanPos;
        public ChannelInfo chan;
        protected void Init()
        {
            this.bitDepth = this.header.bitDepth;
            this.pixelCount = this.width * this.height;
            if (this.bitDepth == 16)
                this.pixelCount *= 2;
        }

        protected void CalculateLength()
        {
            switch (this.bitDepth)
            {
                case 1:
                    this.length = (this.width + 7) / 8 * this.height;
                    break;
                case 16:
                    this.length = this.width * this.height * 2;
                    break;
                default:
                    this.length = this.width * this.height;
                    break;
            }
            this.channelLength = this.length;
            this.length *= this.channelCount;
			this.imageDataRaw = new byte[this.length];
			this.pixelData = new byte[pixelCount*4];
        }
        
        public void SetChannelsInfo()
        {
            switch (this.header.colorMode)
            {
                case ColorMode.Grayscale:
                    Grayscale.SetGrayscaleChannelsInfo(this);
                    break;
                case ColorMode.RGBColor:
                    Rgb.SetRgbChannelsInfo(this);
                    break;
                case ColorMode.CMYKColor:
                    Cmyk.SetCmykChannelsInfo(this);
                    break;
            }
        }

        public virtual void ParseImageData(PsdBinaryReader reader)
        {
            switch (this.imageCompression)
            {
                case ImageCompression.Raw:
                    Raw.ParseRaw(reader,this);
                    break;
                case ImageCompression.Rle:
                    Rle.ParseRle(reader, this);
                    break;
                case ImageCompression.Zip:
                case ImageCompression.ZipPrediction:
                    ParseZip(reader, this);
                    break;

            }
        }

        protected void ParseZip(PsdBinaryReader reader, ImageData image)
        {
        }

        public void ProcessImageData()
        {
            switch (this.header.colorMode)
            {
                case ColorMode.Grayscale:
                    Grayscale.CombineGrayscaleChannel(this);
                    break;
                case ColorMode.RGBColor:
                    Rgb.CombineRgbChannel(this);
                    break;
                case ColorMode.CMYKColor:
                    Cmyk.CombineCmykChannel(this);
                    break;
            }
            
        }

        

    }
}