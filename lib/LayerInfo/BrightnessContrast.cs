/*
 * File: Assets/Editor/PSD/LayerInfo/BrightnessContrast.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:33:22 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    public class BrightnessContrast :LayerInfo
    {
        public short brightness { get; private set; }
        public short constrast { get; private set; }
        public short meanValue { get; private set; }
        public bool labColor { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            this.brightness = reader.ReadInt16();
            this.constrast = reader.ReadInt16();
            this.meanValue = reader.ReadInt16();
            this.labColor = reader.ReadBoolean();
        }
    }
}
