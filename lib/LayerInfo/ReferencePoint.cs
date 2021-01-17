/*
 * File: Assets/Editor/PSD/LayerInfo/ReferencePoint.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:39:11 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{

    public class ReferencePoint : LayerInfo
    {

        public double x { get; private set; }
        public double y { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            this.x = reader.ReadDouble();
            this.y = reader.ReadDouble();
        }
    }
}

