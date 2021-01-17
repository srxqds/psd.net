/*
 * File: Assets/Editor/PSD/Layer/Mask.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 5:10:43 PM
 */
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;

namespace Com.Lucky.PhotoShop
{
    public class Mask
    {
        /// <summary>
        /// The layer to which this mask belongs
        /// </summary>
        public Layer layer { get; private set; }

        /// <summary>
        /// The rectangle enclosing the mask.
        /// </summary>
        public Rect rect { get; set; }

        public byte defaultColor { get; private set; }

        public byte flagsByte { get; private set; }

        public bool relative { get { return (this.flagsByte & 0x01) > 0; } }

        public bool disabled { get { return (this.flagsByte & (0x01 << 1)) > 0; } }

        public bool invert { get { return (this.flagsByte & (0x01 << 2)) > 0; } }
        
        public Mask(Layer layer, Rect rect, byte color,byte flags)
        {
            this.layer = layer;
            this.rect = rect;
            this.defaultColor = color;
            this.flagsByte = flags;
        }
    }

    /// <summary>
    /// Mask info for a layer.  Contains both the layer and user masks.
    /// </summary>
    public class MaskInfo
    {
        public Mask layerMask { get; private set; }

        public Mask userMask { get; private set; }

        public MaskInfo(PsdBinaryReader reader, Layer layer)
        {
            var maskLength = reader.ReadUInt32();
            if (maskLength <= 0)
                return;

            var startPosition = reader.BaseStream.Position;
            var endPosition = startPosition + maskLength;

            // Read layer mask
            var rectangle = reader.ReadRectangle();
            var defaultColor = reader.ReadByte();
            if ((defaultColor != 0) && (defaultColor != 255))
                throw new PsdInvalidException("Mask background must be fully-opaque or fully-transparent.");
            var flagsByte = reader.ReadByte();
            layerMask = new Mask(layer, rectangle, defaultColor, flagsByte);

            // User mask is supplied separately when there is also a vector mask.
            if (maskLength == 36)
            {
                var userFlagsByte = reader.ReadByte();
                var userBackgroundColor = reader.ReadByte();
                if ((userBackgroundColor != 0) && (userBackgroundColor != 255))
                    throw new PsdInvalidException("Mask background must be fully-opaque or fully-transparent.");
                var userRectangle = reader.ReadRectangle();
                userMask = new Mask(layer, userRectangle, userBackgroundColor,userFlagsByte);
            }

            // 20-byte mask data will end with padding.
            reader.BaseStream.Position = endPosition;
        }

        

    }
}
