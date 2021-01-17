/*
 * File: Assets/Editor/PSD/ImageResouce/ResolutionInfo.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:32:21 PM
 */
using UnityEngine;
using System.Collections;
using System;
namespace Com.Lucky.PhotoShop
{
    /// <summary>
    /// Summary description for ResolutionInfo.
    /// </summary>
    public class ResolutionInfo : ImageResource
    {
        public override ResourceID id
        {
            get { return ResourceID.ResolutionInfo; }
        }

        /// <summary>
        /// Fixed-point decimal, with 16-bit integer and 16-bit fraction.
        /// </summary>
        public class UFixed16_16
        {
            public UInt16 Integer { get; set; }
            public UInt16 Fraction { get; set; }

            public UFixed16_16(UInt16 integer, UInt16 fraction)
            {
                Integer = integer;
                Fraction = fraction;
            }

            /// <summary>
            /// Split the high and low words of a 32-bit unsigned integer into a
            /// fixed-point number.
            /// </summary>
            public UFixed16_16(UInt32 value)
            {
                Integer = (UInt16)(value >> 16);
                Fraction = (UInt16)(value & 0x0000ffff);
            }

            public UFixed16_16(double value)
            {
                if (value >= 65536.0) throw new OverflowException();
                if (value < 0) throw new OverflowException();

                Integer = (UInt16)value;

                // Round instead of truncate, because doubles may not represent the
                // fraction exactly.
                Fraction = (UInt16)((value - Integer) * 65536 + 0.5);
            }

            public static implicit operator double(UFixed16_16 value)
            {
                return (double)value.Integer + value.Fraction / 65536.0;
            }




        }
        /// <summary>
        /// Horizontal DPI.
        /// </summary>
        public UFixed16_16 hDpi { get; set; }

        /// <summary>
        /// Vertical DPI.
        /// </summary>
        public UFixed16_16 vDpi { get; set; }

        /// <summary>
        /// 1 = pixels per inch, 2 = pixels per centimeter
        /// </summary>
        public enum ResUnit
        {
            PxPerInch = 1,
            PxPerCm = 2
        }

        /// <summary>
        /// Display units for horizontal resolution.  This only affects the
        /// user interface; the resolution is still stored in the PSD file
        /// as pixels/inch.
        /// </summary>
        public ResUnit hResDisplayUnit { get; set; }

        /// <summary>
        /// Display units for vertical resolution.
        /// </summary>
        public ResUnit vResDisplayUnit { get; set; }

        /// <summary>
        /// Physical units.
        /// </summary>
        public enum Unit
        {
            Inches = 1,
            Centimeters = 2,
            Points = 3,
            Picas = 4,
            Columns = 5
        }

        public Unit widthDisplayUnit { get; set; }

        public Unit heightDisplayUnit { get; set; }

        
        public ResolutionInfo(PsdBinaryReader reader, string name)
            : base(name)
        {
            this.hDpi = new UFixed16_16(reader.ReadUInt32());
            this.hResDisplayUnit = (ResUnit)reader.ReadInt16();
            this.widthDisplayUnit = (Unit)reader.ReadInt16();

            this.vDpi = new UFixed16_16(reader.ReadUInt32());
            this.vResDisplayUnit = (ResUnit)reader.ReadInt16();
            this.heightDisplayUnit = (Unit)reader.ReadInt16();
        }


    }
}
