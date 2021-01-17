/*
 * File: Assets/Editor/PSD/Layer/Channel.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 5:10:20 PM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;
using Com.Lucky.PhotoShop;

namespace Com.Lucky.PhotoShop
{
    
	public class ChannelInfo
	{
        /// <summary>
        /// Channel ID.
        /// <list type="bullet">
        /// <item>-1 = transparency mask</item>
        /// <item>-2 = user-supplied layer mask, or vector mask</item>
        /// <item>-3 = user-supplied layer mask, if channel -2 contains a vector mask</item>
        /// <item>
        /// Nonnegative channel IDs give the actual image channels, in the
        /// order defined by the colormode.  For example, 0, 1, 2 = R, G, B.
        /// </item>
        /// </list>
        /// </summary>
		public Int16 id;
		public Int32 length;
		public ChannelInfo(Int16 id,Int32 length)
		{
			this.id = id;
			this.length = length;
		}
        public ChannelInfo(Int16 id)
        {
            this.id = id;
        }

	}
    ///////////////////////////////////////////////////////////////////////////

    public class ChannelImage:ImageData
    {
        /// <summary>
        /// The layer to which this channel belongs
        /// </summary>
        public Layer layer { get; private set; }
        //////////////////////////////////////////////////////////////////

        public ChannelImage(PsdBinaryReader reader, Layer layer, Header header)
        {
            this.header = header;
            this.channelCount = reader.ReadUInt16();
            this.channelsInfo = new ChannelInfo[this.channelCount];
            for (int channel = 0; channel < channelCount; channel++)
            {
                this.channelsInfo[channel] = new ChannelInfo(reader.ReadInt16(), reader.ReadInt32());
            }
            this.layer = layer;
			this.width = (int)this.layer.rect.width;
			this.height = (int)this.layer.rect.height;
            Init();
            CalculateLength();
            this.opacity = this.layer.opacity /255.0f;
            this.hasMask = false;
            for(int i = 0;i<this.channelsInfo.Length;i++)
            {
                if(this.channelsInfo[i].id < -1)
                {
                    this.hasMask = true;
                    break;
                }
            }
        }

        //////////////////////////////////////////////////////////////////

        internal void LoadPixelData(PsdBinaryReader reader)
        {
            this.chanPos = 0;
            for (int i = 0; i < this.channelsInfo.Length; i++)
            {
				this.chanPos = channelLength*i;
                if (this.channelsInfo[i].length <= 0)
                {
                    this.imageCompression = (ImageCompression)reader.ReadInt16();
                    continue;
                }
                this.chan = channelsInfo[i];
				//mask
                if (this.chan.id < -1)
                {
                    this.width = (int)this.layer.masks.layerMask.rect.width;
                    this.height = (int)this.layer.masks.layerMask.rect.height;
                }
                else
                {
                    this.width = (int)this.layer.rect.width;
                    this.height = (int)this.layer.rect.height;
                }
                this.length = this.width * this.height;
				var start = reader.BaseStream.Position;
				var end = this.chan.length + start;
                this.ParseImageData(reader);
				if(reader.BaseStream.Position != end)
					reader.BaseStream.Position = end;
            }
            this.width = (int)this.layer.rect.width;
            this.height = (int)this.layer.rect.height;
            this.ProcessImageData();
        }

        public override void ParseImageData(PsdBinaryReader reader)
        {
			this.imageCompression = (ImageCompression)reader.ReadInt16();
            switch (this.imageCompression)
            {
                case ImageCompression.Raw:
                    LayerRaw.ParseRaw(reader, this);
                    break;
                case ImageCompression.Rle:
                    LayerRle.ParseRle(reader, this);
                    break;
                case ImageCompression.Zip:
                case ImageCompression.ZipPrediction:
                    ParseZip(reader, this);
                    break;

            }
        }

    }
}
