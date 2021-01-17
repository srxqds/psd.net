/*
 * File: Assets/Editor/PSD/LayerInfo/LayerGroup.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:35:23 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{

    public class LayerGroup :LayerInfo
    {
        public bool isFolder { get; private set; }
        public bool isHidden { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            int code = reader.ReadInt32();
            if (code == 1 || code == 2)
                this.isFolder = true;
            else if (code == 3)
                this.isHidden = true;
        }
    }
}
