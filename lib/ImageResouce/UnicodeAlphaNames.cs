/*
 * File: Assets/Editor/PSD/ImageResouce/UnicodeAlphaNames.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:32:58 PM
 */
using System;
using System.Collections.Generic;

namespace Com.Lucky.PhotoShop
{
    /// <summary>
    /// The names of the alpha channels.
    /// </summary>
    public class UnicodeAlphaNames : ImageResource
    {
        public override ResourceID id
        {
            get { return ResourceID.UnicodeAlphaNames; }
        }

        public List<string> channelNames { get; private set; }

        
        public UnicodeAlphaNames(PsdBinaryReader reader, string name, int resourceDataLength)
            : base(name)
        {
            this.channelNames = new List<string>();
            var endPosition = reader.BaseStream.Position + resourceDataLength;

            while (reader.BaseStream.Position < endPosition)
            {
                var channelName = reader.ReadUnicodeString();
                channelNames.Add(channelName);
            }
        }

        
    }
}