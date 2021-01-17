/*
 * File: Assets/Editor/PSD/LayerInfo/vib.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 3:05:29 PM
 */
using UnityEngine;
using System.Collections;
using Com.Lucky.PhotoShop;

namespace Com.Lucky.PhotoShop
{
    public class Vibrance:LayerInfo
    {
        public Descriptor data { get; private set; }

        public int vibrance { get { return (int)this.data["vibrance"]; } }

        public int saturation { get { return (int) this.data["Strt"]; } }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            reader.BaseStream.Position += 4;
            this.data = new Descriptor(reader);
        }
    }
}
