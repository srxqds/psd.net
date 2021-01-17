/*
 * File: Assets/Editor/PSD/LayerInfo/Locked.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 8/2/2015 2:38:30 PM
 */
using UnityEngine;
using System.Collections;
using System;

namespace Com.Lucky.PhotoShop
{
    public class Locked : LayerInfo
    {
        public Int32 lockedByte { get; private set; }

        public bool transparencyLocked { get { return (lockedByte & (0x01 << 0)) > 0 || lockedByte == -2147483648; } }

        public bool compositeLocked { get { return (lockedByte & (0x01 << 1)) > 0 || lockedByte == -2147483648; } }

        public bool positionLocked { get { return (lockedByte & (0x01 << 2)) > 0 || lockedByte == -2147483648; } }

        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.lockedByte = reader.ReadInt32();
        }
    }
}