/*
 * File: Assets/Editor/PSD/ImageResouce/AlphaChannelNames.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:31:51 PM
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Com.Lucky.PhotoShop
{
    /// <summary>
    /// The names of the alpha channels
    /// </summary>
    public class AlphaChannelNames : ImageResource
    {
        public override ResourceID id
        {
            get { return ResourceID.AlphaChannelNames; }
        }

        public List<string> channelNames { get; private set; }
        

        public AlphaChannelNames(PsdBinaryReader reader, string name, int resourceDataLength)
            : base(name)
        {
            channelNames = new List<string>();

            var endPosition = reader.BaseStream.Position + resourceDataLength;

            // Alpha channel names are Pascal strings, with no padding in-between.
            while (reader.BaseStream.Position < endPosition)
            {
                var channelName = reader.ReadPascalString(1);
                channelNames.Add(channelName);
            }
        }

        
    }
}
