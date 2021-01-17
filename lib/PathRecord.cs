/*
 * File: Assets/Editor/PSD/PathRecord.cs
 * Project: TopdownStarterkit
 * Company: DreamdevStudio
 * Code Porter: D.S.Qiu
 * Create Date: 10/24/2015 3:17:35 PM
 */
using UnityEngine;
using System.Collections;

namespace Com.Lucky.PhotoShop
{

    public class PathRecord
    {
        public enum  PathType
        {
            None,
            PathRecord,
            BezierPoint,
            ClipboardRecord,
            InitialFill,
        }

        public Layer layer;

        public PathType recordType { get; private set; }

        public short pointCount { get; private set; }

        public float precedingVertical { get; private set; }
        public float precedingHorizontal { get; private set; }
        public float anchorVertical { get; private set; }
        public float anchorHorizontal { get; private set; }
        public float leavingVertical;
        public float leavingHorizontal { get; private set; }

        public float clipboardTop { get; private set; }
        public float clipboardLeft { get; private set; }
        public float clipboardBottom { get; private set; }
        public float clipboardRight { get; private set; }
        public float clipboardResolution { get; private set; }

        public short initialFill { get; private set; }

        public PathRecord(PsdBinaryReader reader)
        {
            short recordInt = reader.ReadInt16();
            switch (recordInt)
            {
                case 0:
                case 3:
                    this.recordType = PathType.PathRecord;
                    break;
                case 1:
                case 2:
                case 4:
                case 5:
                    this.recordType = PathType.BezierPoint;
                    break;
                case 7:
                    this.recordType = PathType.ClipboardRecord;
                    break;
                case 8:
                    this.recordType = PathType.InitialFill;
                    break;
                default:
                    this.recordType = PathType.None;
                   
                    break;   
            }
            ReadPath(reader);
        }

        private void ReadPath(PsdBinaryReader reader)
        {
            if (this.recordType == PathType.None)
            {
                reader.Seek(24);
            }
            else if(this.recordType == PathType.PathRecord)
            {
                this.pointCount = reader.ReadInt16();
                reader.Seek(22);
            }
            else if (this.recordType == PathType.BezierPoint)
            {
                this.precedingVertical = reader.ReadPathNumber();
                this.precedingHorizontal = reader.ReadPathNumber();
                this.anchorVertical = reader.ReadPathNumber();
                this.anchorHorizontal = reader.ReadPathNumber();
                this.leavingVertical = reader.ReadPathNumber();
                this.leavingHorizontal = reader.ReadPathNumber();
            }
            else if (this.recordType == PathType.ClipboardRecord)
            {
                this.clipboardTop = reader.ReadPathNumber();
                this.clipboardLeft = reader.ReadPathNumber();
                this.clipboardBottom = reader.ReadPathNumber();
                this.clipboardRight = reader.ReadPathNumber();
                this.clipboardResolution = reader.ReadPathNumber();
                reader.Seek(4);
            }
            else if (this.recordType == PathType.InitialFill)
            {
                this.initialFill = reader.ReadInt16();
                reader.Seek(22);
            }
        }
    }
}
