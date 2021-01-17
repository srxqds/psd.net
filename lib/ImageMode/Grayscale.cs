/*
 * File: Assets/Editor/PSD/ImageMode/GrayScale.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/20/2015 5:34:10 AM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class Grayscale
    {
        public static void SetGrayscaleChannelsInfo(ImageData image)
        {
            if (image.channelCount == 2)
                image.channelsInfo = new ChannelInfo[] { new ChannelInfo(0), new ChannelInfo(-1) };
            else
                image.channelsInfo = new ChannelInfo[] { new ChannelInfo(0) };
        }

        public static void CombineGrayscaleChannel(ImageData image)
        {
            for (int i = 0; i < image.pixelCount; i++)
            {
                byte gray = image.imageDataRaw[i];
                byte alpha = 255;
                if (image.channelCount == 2)
                {
                    alpha = image.imageDataRaw[image.channelLength + i];

                }
				image.pixelData[4*i] = gray; 
				image.pixelData[4*i+1] = gray;
				image.pixelData[4*i+2] = gray;
				image.pixelData[4*i+3] = alpha;//gray,gray ,gray ,alpha
            }

        }

    }
}
