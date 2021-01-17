/*
 * File: Assets/Editor/PSD/LayerInfo/Levels.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 1:37:11 PM
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Com.Lucky.PhotoShop
{
    public class Levels : LayerInfo
    {
        public class LevelEntity
        {
            public short inputFloor { get; private set; }
            public short inputCeiling { get; private set; }
            public short outputFloor { get; private set; }
            public short outputCeiling { get; private set; }
            public float gamma { get; private set; }

            public LevelEntity(PsdBinaryReader reader)
            {
                this.inputFloor = reader.ReadInt16();
                this.inputCeiling = reader.ReadInt16();
                this.outputFloor = reader.ReadInt16();
                this.outputCeiling = reader.ReadInt16();
                this.gamma = reader.ReadInt16()/100f;
            }
        }

        public LevelEntity[] records { get; private set; }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            long startPosition = reader.Position;
            reader.Seek(2);
            List<LevelEntity> recordList = new List<LevelEntity>();
            for(int i=0;i<29;i++)
                recordList.Add(new LevelEntity(reader));

            if (reader.Position < startPosition + length - 4)
            {
                string tag = reader.ReadAsciiChars(4);
                if (tag != "Lvls")
                    throw new PsdInvalidException("Extra levels key error");
            }

            int extraCount = reader.ReadInt16() - 29;

            for (int i = 0; i < extraCount; i++)
            {
                recordList.Add(new LevelEntity(reader));
            }
            this.records = recordList.ToArray();
        }
    }
}
