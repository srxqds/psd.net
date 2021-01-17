/*
 * File: Assets/Editor/PSD/LayerInfo/SectionDivider.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:40:40 AM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{
   
    public enum SectionType
    {
        Layer = 0,
        OpenFolder = 1,
        ClosedFolder = 2,
        SectionDivider = 3
    }

    public enum SectionSubtype
    {
        Normal = 0,
        SceneGroup = 1
    }

    /// <summary>
    /// Layer sections are known as Groups in the Photoshop UI.
    /// </summary>
    public class LayerSectionDivider : LayerInfo
    {

        public SectionType sectionType { get; private set; }

        public SectionSubtype subtype { get; private set; }

        public string blendModeKey { get; private set; }

        public bool groupOpened { get { return this.sectionType == SectionType.OpenFolder; } }

        public bool groupClosed { get { return this.sectionType == SectionType.ClosedFolder; } }

        public bool groupEnd { get { return this.sectionType == SectionType.SectionDivider; } }

        public bool groupStart {get { return this.groupClosed || this.groupOpened; } }
        
        protected override void ReadData(PsdBinaryReader reader, int dataLength)
        {
            
            sectionType = (SectionType)reader.ReadInt32();
            if (dataLength >= 12)
            {
                var signature = reader.ReadAsciiChars(4);
                if (signature != "8BIM")
                    throw new PsdInvalidException("Invalid section divider signature.");

                blendModeKey = reader.ReadAsciiChars(4);
                if (dataLength >= 16)
                {
                    subtype = (SectionSubtype)reader.ReadInt32();
                }
            }
        }
    }
}
