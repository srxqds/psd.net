/*
 * File: Assets/Editor/PSD/LayerInfo/ContentGenerator.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:33:48 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{

    public class ContentGenerator : LayerInfo
    {

        public Descriptor data { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            //skip Version
            reader.BaseStream.Position += 4;
            this.data = new Descriptor(reader);
        }

        public int brigthtness
        {
            get { return (int)this.data["Brgh"]; }
        }

        public int contrast
        {
            get { return (int)this.data["Cntr"]; }
        }

        public int meanValue
        {
            get { return (int) this.data["means"]; }
        }

        public bool labColor
        { get { return (bool) this.data["Lab "]; } }

        public bool useLegacy
        {
            get { return (bool) this.data["useLegacy"]; }
        }

        public bool auto
        { get { return (bool) this.data["Auto"]; } }

      
    }
}
