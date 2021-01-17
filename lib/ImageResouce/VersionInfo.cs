/*
 * File: Assets/Editor/PSD/ImageResouce/VersionInfo.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 4:32:43 PM
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Lucky.PhotoShop
{
    public class VersionInfo : ImageResource
    {
        public override ResourceID id
        {
            get { return ResourceID.VersionInfo; }
        }

        public UInt32 version { get; private set; }

        public bool hasRealMergedData { get; private set; }

        public string readerName { get; private set; }

        public string writerName { get; private set; }

        public UInt32 fileVersion { get; private set; }

                
        public VersionInfo(PsdBinaryReader reader, string name)
            : base(name)
        {
            this.version = reader.ReadUInt32();
            this.hasRealMergedData = reader.ReadBoolean();
            this.readerName = reader.ReadUnicodeString();
            this.writerName = reader.ReadUnicodeString();
            this.fileVersion = reader.ReadUInt32();
        }

        
    }
}
