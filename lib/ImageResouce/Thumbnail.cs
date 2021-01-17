/*
 * File: Assets/Editor/PSD/ImageResouce/Thumbnail.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:32:34 PM
 */
using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;

namespace Com.Lucky.PhotoShop
{
    /// <summary>
    /// Summary description for Thumbnail.
    /// </summary>
    public class Thumbnail : RawImageResource
    {
        public Texture2D image { get; private set; }

        public UInt32 width { get; private set; }
        public UInt32 height { get; private set; }
        public UInt32 format { get; private set; }
        

        public Thumbnail(PsdBinaryReader psdReader, ResourceID id, string name, int numBytes)
            : base(psdReader, id, name, numBytes)
        {
            using (var memoryStream = new MemoryStream(Data))
            using (var reader = new PsdBinaryReader(memoryStream, psdReader))
            {
                const int HEADER_LENGTH = 28;
                format = reader.ReadUInt32();
                width = reader.ReadUInt32();
                height = reader.ReadUInt32();
                var widthBytes = reader.ReadUInt32();
                var size = reader.ReadUInt32();
                var compressedSize = reader.ReadUInt32();
                var bitPerPixel = reader.ReadUInt16();
                var planes = reader.ReadUInt16();

                // Raw RGB bitmap
                if (format == 0)
                {
                    image = new Texture2D((int)width, (int)height, TextureFormat.RGB24, true);
                }
                // JPEG bitmap
                else if (format == 1)
                {
                    byte[] imgData = reader.ReadBytes(numBytes - HEADER_LENGTH);
                    image = new Texture2D((int)width, (int)height, TextureFormat.RGB24, true);
                    image.LoadImage(imgData);

                    //Reverse BGR pixels from old thumbnail format
                    if (id == ResourceID.ThumbnailBgr)
                    {
                        for(int y=0;y<height;y++)
                            for (int x = 0; x < width; x++)
                            {
                                Color c = image.GetPixel(x, y);
                                Color c2=new Color(c.b, c.g, c.r);
                                image.SetPixel(x, y, c2);
                            }
                    }
                }
                else
                {
                    throw new PsdInvalidException("Unknown thumbnail format.");
                }
            }
        }
    }
}
