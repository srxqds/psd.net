/*
 * File: Assets/Editor/PSD/ImageMode/Rgb.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/20/2015 5:36:16 AM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class Rgb 
    {
        public static void SetRgbChannelsInfo(ImageData image)
        {
            if (image.channelCount == 4)
                image.channelsInfo = new ChannelInfo[] { new ChannelInfo(0), new ChannelInfo(1), new ChannelInfo(2),
                     new ChannelInfo(-1) };
            else
                image.channelsInfo = new ChannelInfo[] { new ChannelInfo(0), new ChannelInfo(1), new ChannelInfo(2) };
        }

        public static void CombineRgbChannel(ImageData image)
        {
            for (int i = 0; i < image.pixelCount; i++)
            {
				byte r = 0;
				byte g = 0;
				byte b = 0;
				byte a = 255;
                for (int index = 0; index < image.channelsInfo.Length; index++)
                {
                    ChannelInfo chan = image.channelsInfo[index];
					byte val = image.imageDataRaw[i + image.channelLength * index];
                    switch (chan.id)
                    {
                        case -1:
                            a = val;
                            break;
                        case 0:
                            r = val;
                            break;
                        case 1:
                            g = val;
                            break;
                        case 2:
                            b = val;
                            break;

                    }
                }
				image.pixelData[4*i] = r; 
				image.pixelData[4*i+1] = g;
				image.pixelData[4*i+2] = b;
				image.pixelData[4*i+3] = a;//gray,gray ,gray ,alpha
            }
        }
    }
}
