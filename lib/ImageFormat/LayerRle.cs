/*
 * File: Assets/Editor/PSD/ImageFormat/LayerRle.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 8:15:54 PM
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Com.Lucky.PhotoShop
{
    public class LayerRle 
    {
        public static void ParseRle(PsdBinaryReader reader, ImageData image)
        {
            //parseBytecounts
            int[] byteCounts = new int[image.height];
            for (int i = 0; i < image.height; i++)
            {
                byteCounts[i] = reader.ReadInt16();
            }
            //end
            //parseChannelData
            int lineIndex = 0;
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
                            image.imageDataRaw[image.chanPos] = val;
							image.chanPos++;
                        }
                    }
                }
                //end
            }
            //end
        }
    }
}