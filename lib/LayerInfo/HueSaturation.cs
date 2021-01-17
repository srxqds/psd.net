/*
 * File: Assets/Editor/PSD/LayerInfo/HueSaturation.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:35:04 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
    
    public class HueSaturation : LayerInfo
    {
        public enum HueType:byte
        {
            Hue,
            Colorization,
        }

        public class HSLEntity
        {
            public short hue { get; private set; }
            public short saturation { get; private set; }
            public short light { get; private set; }

            public HSLEntity(PsdBinaryReader reader)
            {
                this.hue = reader.ReadInt16();
                this.saturation = reader.ReadInt16();
                this.light = reader.ReadInt16();
            }
        }


        public HueType type { get; private set; }
        public HSLEntity colorization { get; private set; }
        public HSLEntity master { get; private set; }

        public short[][] rangeValues = new short[6][];
        public short[][] settingValues = new short[6][];


        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            reader.BaseStream.Position += 1;
            int typeByte = reader.ReadByte();
            if(typeByte == 0)
                this.type = HueType.Hue;
            else
            {
                this.type = HueType.Colorization;
            }

            this.colorization = new HSLEntity(reader);
            this.master = new HSLEntity(reader);

            for (int i = 0; i < 6; i++)
            {
                this.rangeValues[i] = new short[4];
                for (int j = 0; j < 4; j++)
                {
                    this.rangeValues[i][j] = reader.ReadInt16();
                }
            }

            for (int i = 0; i < 6; i++)
            {
                this.settingValues[i] = new short[4];
                for (int j = 0; j < 4; j++)
                {
                    this.settingValues[i][j] = reader.ReadInt16();
                }
            }
        }
    }
}
