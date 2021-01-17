/*
 * File: Assets/Editor/PSD/LayerInfo/PatternFill.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:38:25 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{


    public class PatternFill : LayerInfo
    {
        public Descriptor data { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            //# Useless id/version info
            reader.BaseStream.Position += 4;
            this.data = new Descriptor(reader);
        }

    }
} 
