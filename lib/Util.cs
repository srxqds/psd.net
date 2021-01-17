/*----------------------------------------------------------------
// Copyright (C) 2015 广州，Lucky Game
//
// 模块名：
// 创建者：D.S.Qiu
// 修改者列表：
// 创建日期：7/17/2015 2:08:06 PM
// 模块描述：
//----------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Com.Lucky.PhotoShop
{
    public static class Util
    {
        [DebuggerDisplay("Top = {Top}, Bottom = {Bottom}, Left = {Left}, Right = {Right}")]
        public struct RectanglePosition
        {
            public int Top { get; set; }
            public int Bottom { get; set; }
            public int Left { get; set; }
            public int Right { get; set; }
        }


        public static Int16 Convert(Int16 value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }
        public static Int32 Convert(Int32 value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        public static Int64 Convert(Int64 value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }
        public static UInt16 Convert(UInt16 value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }
        public static UInt32 Convert(UInt32 value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }
        public static UInt64 Convert(UInt64 value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }
        public static double Convert(double value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }
        public static bool Convert(bool value)
        {
            //大端直接返回不需要转换
            if (!BitConverter.IsLittleEndian)
                return value;
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToBoolean(bytes, 0);
        }

        public static void ConvertBytes2BigEndian(byte[] buffer)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
        }
        /////////////////////////////////////////////////////////////////////////// 
              

        /// <summary>
        /// Round the integer to a multiple.
        /// </summary>
        public static int RoundUp(int value, int multiple)
        {
            if (value == 0)
                return 0;

            if (Math.Sign(value) != Math.Sign(multiple))
                throw new ArgumentException("value and multiple cannot have opposite signs.");

            var remainder = value % multiple;
            if (remainder > 0)
            {
                value += (multiple - remainder);
            }
            return value;
        }

        /// <summary>
        /// Get number of bytes required to pad to the specified multiple.
        /// </summary>
        public static int GetPadding(int length, int padMultiple)
        {
            if ((length < 0) || (padMultiple < 0))
                throw new ArgumentException();

            var remainder = length % padMultiple;
            if (remainder == 0)
                return 0;

            var padding = padMultiple - remainder;
            return padding;
        }

        public static int BytesFromBitDepth(int depth)
        {
            switch (depth)
            {
                case 1:
                case 8:
                    return 1;
                case 16:
                    return 2;
                case 32:
                    return 4;
                default:
                    throw new ArgumentException("Invalid bit depth.");
            }
        }

    }

    


}

