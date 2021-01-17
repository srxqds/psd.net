/*
 * File: Assets/Editor/PSD/ImageMode/Cmyk.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/20/2015 5:35:20 AM
 */
using UnityEngine;
using System.Collections;
using Utility;

namespace Com.Lucky.PhotoShop
{
    public class Cmyk
    {
        public static void SetCmykChannelsInfo(ImageData image)
        {
            if (image.channelCount == 5)
                image.channelsInfo = new ChannelInfo[] { new ChannelInfo(0), new ChannelInfo(1), new ChannelInfo(2),
                    new ChannelInfo(3), new ChannelInfo(-1) };
            else
                image.channelsInfo = new ChannelInfo[] { new ChannelInfo(0), new ChannelInfo(1), new ChannelInfo(2),
                    new ChannelInfo(3) };
        }

        public static void CombineCmykChannel(ImageData image)
        {
            for (int i = 0; i < image.pixelCount; i++)
            {
				byte c = 0;
				byte m = 0;
				byte y = 0;
				byte k = 0;
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
                            c = val;
                            break;
                        case 1:
                            m = val;
                            break;
                        case 2:
                            y = val;
                            break;
                        case 3:
                            k = val;
                            break;
                    }
                }
				byte[] rgb = Cmyk2Rgb((byte)(255-c), (byte)(255-m), (byte)(255-y), (byte)(255-k));
				image.pixelData[4*i] = rgb[0]; 
				image.pixelData[4*i+1] = rgb[1];
				image.pixelData[4*i+2] = rgb[2];
				image.pixelData[4*i+3] = a;//rgb ,alpha
            }

        }
		public static byte[] Cmyk2Rgb(byte c, byte m, byte y, byte k)
        {
            byte[] rgb = new byte[3];

            rgb[0] = (byte)((65535 - (c * (255 - k) + (k << 8))) >> 8).Clamp(0, 255);
            rgb[1] = (byte)((65535 - (m * (255 - k) + (k << 8))) >> 8).Clamp(0, 255);
            rgb[2] = (byte)((65535 - (y * (255 - k) + (k << 8))) >> 8).Clamp(0, 255);
			//rgb[0] = (byte)(255*(1-c/255f)*(1-k/255f));
			//rgb[1] = (byte)(255*(1-m/255f)*(1-k/255f));
			//rgb[2] = (byte)(255*(1-y/255f)*(1-k/255f));
            return rgb;
        }
    }
}
