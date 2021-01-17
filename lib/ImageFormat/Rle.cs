/*
 * File: Assets/Editor/PSD/ImageFormat/Rle.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 8:16:08 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class Rle
    {
        public static void ParseRle(PsdBinaryReader reader, ImageData image)
        {
            //parseBytecounts
            int rowCount = image.channelCount * image.height;
            int[] byteCounts = new int[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                byteCounts[i] = reader.ReadInt16();
            }
            //end
            //parseChannelData
            image.chanPos = 0;
            int lineIndex = 0;
            for (int i = 0; i < image.channelCount; i++)
            {
                //parseRleChannel
                for (int j = 0; j < image.height; j++)
                {
                    int byteCount = byteCounts[lineIndex + j];
                    long finishPosition = reader.BaseStream.Position + byteCount;
                    while (reader.BaseStream.Position < finishPosition)
                    {
                        int len = reader.ReadByte();
                        if (len < 128)
                        {
                            len += 1;
                            for (int index = 0; index < len; index++)
                            {
                                image.imageDataRaw[image.chanPos++] = reader.ReadByte();
                            }
                        }
                        else if (len > 128)
                        {
                            len ^= 0xff;
                            len += 2;
                            var val = reader.ReadByte();
                            for (int index = 0; index < len; index++)
                            {
                                image.imageDataRaw[image.chanPos++] = val;
                            }
                        }
                    }
                }
                //end
                lineIndex += image.height;
            }
            //end
        }

    }
}
