/*
 * File: Assets/Editor/PSD/LayerInfo/SheetColor.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:39:28 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class SheetColor : LayerInfo
    {
        public enum ColorType
        {
            NoColor,
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            Violet,
            Gray,
        } 
        public short[] data { get; private set; }

        public ColorType color { get { return (ColorType) this.data[0]; } }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            //Only the first entry is used, the rest are always 0.
            int count = 4;
            this.data = new short[4] {reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16()};
        }
    }
}
