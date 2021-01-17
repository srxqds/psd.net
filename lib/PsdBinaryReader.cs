/*
 * File: Assets/Editor/PSD/PsdBinaryReader.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 2:09:05 PM
 */

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Com.Lucky.PhotoShop
{
    /// <summary>
    /// Reads PSD data types in big-endian byte order.
    /// </summary>
    public class PsdBinaryReader : IDisposable
    {
        private BinaryReader reader;
        private Encoding encoding;

        public Stream BaseStream
        {
            get { return reader.BaseStream; }
        }

        public PsdBinaryReader(Stream stream, PsdBinaryReader reader)
            : this(stream, reader.encoding)
        {
        }

        public PsdBinaryReader(Stream stream, Encoding encoding)
        {
            this.encoding = encoding;
            

            // ReadPascalString and ReadUnicodeString handle encoding explicitly.
            // BinaryReader.ReadString() is never called, so it is constructed with
            // ASCII encoding to make accidental usage obvious.
            reader = new BinaryReader(stream, Encoding.ASCII);
        }

        public void Seek(int offset)
        {
            this.BaseStream.Position += offset;
        }

        public long Position { get { return this.BaseStream.Position; } }

        public void SetPosition(int position)
        {
            this.BaseStream.Position = position;
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            return reader.ReadBytes(count);
        }

        public bool ReadBoolean()
        {
            var val = reader.ReadBoolean();
            return Util.Convert(val);
        }

        public Int16 ReadInt16()
        {
            var val = reader.ReadInt16();
            return Util.Convert(val);
        }

        public Int32 ReadInt32()
        {
            var val = reader.ReadInt32();
            return Util.Convert(val);
        }

        public Int64 ReadInt64()
        {
            var val = reader.ReadInt64();
            return Util.Convert(val);
        }

        public UInt16 ReadUInt16()
        {
            var val = reader.ReadUInt16();
            return Util.Convert(val);
        }

        public UInt32 ReadUInt32()
        {
            var val = reader.ReadUInt32();
            return Util.Convert(val);
        }

        public UInt64 ReadUInt64()
        {
            var val = reader.ReadUInt64();
            return Util.Convert(val);
        }

        public double ReadSingle()
        {
            var val = reader.ReadSingle();
            return Util.Convert(val);
        }

        public double ReadDouble()
        {
            var val = reader.ReadDouble();
            return Util.Convert(val);
        }

        public float ReadPathNumber()
        {
            try
            {
                int a = ReadByte();
                byte[] arr = ReadBytes(3);
                int b1 = arr[0] << 16;
                int b2 = arr[1] << 8;
                int b3 = arr[2];
                int b = b1 | b2 | b3;

				return a + (float)(b/Math.Pow (2 , 24));
                //parseFloat(a, 10) + parseFloat(b / Math.pow(2, 24), 10)
                //return float.Parse(ReadAsciiChars(1)) + Convert.ToInt32(ReadAsciiChars(3), 2) / (float)(2 ^ 24);
            }
            catch (Exception)
            {
                return 0f;
            }
            
        }

        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Read padding to get to the byte multiple for the block.
        /// </summary>
        /// <param name="startPosition">Starting position of the padded block.</param>
        /// <param name="padMultiple">Byte multiple that the block is padded to.</param>
        public void ReadPadding(long startPosition, int padMultiple)
        {
            // Pad to specified byte multiple
            var totalLength = reader.BaseStream.Position - startPosition;
            var padBytes = Util.GetPadding((int)totalLength, padMultiple);
            ReadBytes(padBytes);
        }

        public Rect ReadRectangle()
        {
            var rect = new Rect();
            rect.y = ReadInt32();
            rect.x = ReadInt32();
            rect.height = ReadInt32() - rect.y;
            rect.width = ReadInt32() - rect.x;
            return rect;
        }

        /// <summary>
        /// Read a fixed-length ASCII string.
        /// </summary>
        public string ReadAsciiChars(int count)
        {
            var bytes = reader.ReadBytes(count); ;
            var s = Encoding.ASCII.GetString(bytes);
            return s;
        }

        /// <summary>
        /// Read a Pascal string using the specified encoding.
        /// </summary>
        /// <param name="padMultiple">Byte multiple that the Pascal string is padded to.</param>
        public string ReadPascalString(int padMultiple)
        {
            var startPosition = reader.BaseStream.Position;

            byte stringLength = ReadByte();
            var bytes = ReadBytes(stringLength);
            ReadPadding(startPosition, padMultiple);

            // Default decoder uses best-fit fallback, so it will not throw any
            // exceptions if unknown characters are encountered.
            var str = encoding.GetString(bytes);
            return str;
        }

        public string ReadUnicodeString()
        {
            var numChars = ReadInt32();
            var length = 2 * numChars;
            var data = ReadBytes(length);
            var str = Encoding.BigEndianUnicode.GetString(data, 0, length);

            return str;
        }

        //////////////////////////////////////////////////////////////////

        # region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (disposed)
                return;

            if (disposing)
            {
                if (reader != null)
                {
                    // BinaryReader.Dispose() is protected.
                    reader.Close();
                    reader = null;
                }
            }

            disposed = true;
        }

        #endregion

    }
}