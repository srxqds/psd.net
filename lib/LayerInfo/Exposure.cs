/*
 * File: Assets/Editor/PSD/LayerInfo/Exposure.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:34:33 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{


    public class Exposure : LayerInfo
    {

        public double expoure { get; private set; }
        public double offset { get; private set; }
        public double gamma { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            reader.BaseStream.Position += 2;
            //# Why this shit is big endian is beyond me. Thanks Adobe.
            this.expoure = reader.ReadSingle();
            this.offset = reader.ReadSingle();
            this.gamma = reader.ReadSingle();
        }
    }
}