/*
 * File: Assets/Editor/PSD/LayerInfo/LayerNameSource.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:36:03 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{

    public class LayerNameSource : LayerInfo
    {

        public string id { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            this.id = reader.ReadAsciiChars(4);
        }
    }
}
