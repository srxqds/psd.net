/*
 * File: Assets/Editor/PSD/Descriptor.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/24/2015 9:42:31 AM
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.CompilerServices;

namespace Com.Lucky.PhotoShop
{
    public class OSItemDecoder
    {
        public Int32 itemCount;
        public object[] osItems = new object[0];
        public void ParseItem(PsdBinaryReader reader)
        {
            itemCount = reader.ReadInt32();
            this.osItems = new object[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                this.osItems[i] = DecoderFactory.Load(reader);
            }
        }
    }

    public class ReferenceItem : OSItemDecoder
    {
        public ReferenceItem(PsdBinaryReader reader)
        {
            this.ParseItem(reader);
        }
    }

    public class PropertyItem
    {
        public string name;
        public string classId;
        public string keyId;
        public PropertyItem(PsdBinaryReader reader)
        {
            this.name = reader.ReadUnicodeString();
            this.classId = DecoderFactory.ReadItemKey(reader);
            this.keyId = DecoderFactory.ReadItemKey(reader);
        }
    }

    public class UnitDoubleItem 
    {
        private const string ANGLE = "#Ang"; //angle:base degree
        private const string DENSITY = "#Rsl"; //desity:base per inch
        private const string DISTANCE = "#Rlt"; //distance:base 72ppi
        private const string NONE = "#Nne"; //none:coerced
        private const string PERCENT = "#Prc"; //percent:unit value
        private const string PIXELS = "#Pxl"; //pixels:tagged unit value
        private const string MILLIMETERS = "#Mlm"; //millimeters:tagged unit value
        private const string POINTS = "#Pnt"; //points:tagged unit value
        public string key;
        public double value;

        public UnitDoubleItem(PsdBinaryReader reader)
        {
            this.key = reader.ReadAsciiChars(4);
            this.value = reader.ReadDouble();
        }
    }

    public class UnitFloatItem
    {
        private const string ANGLE = "#Ang"; //angle:base degree
        private const string DENSITY = "#Rsl"; //desity:base per inch
        private const string DISTANCE = "#Rlt"; //distance:base 72ppi
        private const string NONE = "#Nne"; //none:coerced
        private const string PERCENT = "#Prc"; //percent:unit value
        private const string PIXELS = "#Pxl"; //pixels:tagged unit value
        private const string MILLIMETERS = "#Mlm"; //millimeters:tagged unit value
        private const string POINTS = "#Pnt"; //points:tagged unit value
        public string key;
        public double value;

        public UnitFloatItem(PsdBinaryReader reader)
        {
            this.key = reader.ReadAsciiChars(4);
            this.value = reader.ReadSingle();
        }
    }

    public class ClassItem
    {
        public string name;
        public string classId;
        public ClassItem(PsdBinaryReader reader)
        {
            this.name = reader.ReadUnicodeString();
            this.classId = DecoderFactory.ReadItemKey(reader);
        }

    }

    public class EnumerateReferenceItem
    {
        public string name;
        public string classId;
        public string typeId;
        public string enumId;
        public EnumerateReferenceItem(PsdBinaryReader reader)
        {
            this.name = reader.ReadUnicodeString();
            this.classId = DecoderFactory.ReadItemKey(reader);
            this.typeId = DecoderFactory.ReadItemKey(reader);
            this.enumId = DecoderFactory.ReadItemKey(reader);
        }
    }

    public class OffsetItem
    {
        public string name;
        public string classId;
        public Int32 offset;
        public OffsetItem(PsdBinaryReader reader)
        {
            this.name = reader.ReadUnicodeString();
            this.classId = DecoderFactory.ReadItemKey(reader);
            this.offset = reader.ReadInt32();
        }
    }

    public class AliasItem
    {
        public string alias;
        public AliasItem(PsdBinaryReader reader)
        {
            int @int = reader.ReadInt32();
            this.alias = reader.ReadAsciiChars(@int);
        }
    }

    public class ListItem : OSItemDecoder
    {
        public ListItem(PsdBinaryReader reader)
        {
            this.ParseItem(reader);
        }
    }

    public class EnumerateItem 
    {
        public string type;
        public string enumName;
        public EnumerateItem(PsdBinaryReader reader)
        {
            this.type = DecoderFactory.ReadItemKey(reader);
            this.enumName = DecoderFactory.ReadItemKey(reader);
        }

    }

    public class RawDataItem
    {
        public byte[] data;
        public RawDataItem(PsdBinaryReader reader)
        {
            int @int = reader.ReadInt32();
            this.data = reader.ReadBytes(@int);
        }
    }
    
    public class UnknownOSTypeItem
    {
        public string message;
        public void Parse(PsdBinaryReader reader)
        {
            //this.message = message;
        }
    }

    public class DecoderFactory
    {
        public const string REFERENCE = "obj ";
        public const string DESCRIPTOR = "Objc";
        public const string LIST = "VlLs";
        public const string DOUBLE = "doub";
        public const string UNIT_DOUBLE = "UntF";
        public const string STRING = "TEXT";
        public const string ENUMERATED = "enum";
        public const string INTEGER = "long";
        public const string BOOLEAN = "bool";
        public const string GLOBAL_OBJECT = "GlbO";
        public const string CLASS1 = "type";
        public const string CLASS2 = "GlbC";
        public const string ALIAS = "alis";
        public const string RAW_DATA = "tdta";
        public const string PROPERTY = "prop";
        public const string CLASS = "Clss";
        public const string ENUMERATED_REFERENCE = "Enmr";
        public const string OFFSET = "rele";
        public const string IDENTIFIER = "Idnt";
        public const string INDEX = "indx";
        public const string NAME = "name";

        public const string UNIT_FLOAT = "UnFl";
        public const string LARGE_INTEGER = "comp";//not implemented
        public const string OBJECT_ARRAY = "ObAr";//not implemented

        public static object Load(PsdBinaryReader reader)
        {
            string osType = reader.ReadAsciiChars(4);
            object osItem = null;
            switch (osType)
            {
                case REFERENCE:
                    osItem = new ReferenceItem(reader);
                    break;
                case DESCRIPTOR:
                case GLOBAL_OBJECT:
                    osItem = new Descriptor(reader);
                    break;
                case LIST:
                    osItem = new ListItem(reader);
                    break;
                case DOUBLE:
                    osItem = reader.ReadDouble();
                    break;
                case UNIT_DOUBLE:
                    osItem = new UnitDoubleItem(reader);
                    break;
                case UNIT_FLOAT:
                    osItem = new UnitFloatItem(reader);
                    break;
                case STRING:
                    osItem = reader.ReadUnicodeString();
                    break;
                case ENUMERATED:
                    osItem = new EnumerateItem(reader);
                    break;
                case INTEGER:
                    osItem = reader.ReadInt32();
                    break;
                case BOOLEAN:
                    osItem = reader.ReadBoolean();
                    break;
                case CLASS://Reference
                case CLASS1:
                case CLASS2:
                    osItem = new ClassItem(reader);
                    break;
                case ALIAS:
                    osItem = new AliasItem(reader);
                    break;
                case RAW_DATA:
                    osItem = new RawDataItem(reader);
                    break;

                //Reference
                case PROPERTY:
                    osItem = new PropertyItem(reader);
                    break;
                case ENUMERATED_REFERENCE:
                    break;
                case OFFSET: //implement difference from psd.js
                    osItem = new OffsetItem(reader);
                    break;
                case IDENTIFIER:
                    osItem = reader.ReadUInt32();
                    break;
                case INDEX:
                    osItem = reader.ReadUInt32();
                    break;
                case NAME:
                    osItem = reader.ReadUnicodeString();
                    break;
				default:
				Debug.LogError(string.Format("type:{0},not implemented ", osType));
					break;
                	
            }
            return osItem;
        }

        //Descriptor
        public static string ReadItemKey(PsdBinaryReader reader)
        {
            var num = reader.ReadInt32();
            num = ((num > 0) ? num : 4);
            return reader.ReadAsciiChars(num);
        }
    }

    public class Descriptor 
    {
        public ClassItem classItem;
		public Dictionary<string,object> keyItem = new Dictionary<string,object > ();
        public Descriptor(PsdBinaryReader reader)
        {
			this.classItem = new ClassItem(reader);
			ParseKeyItem (reader);
        }

        public object this[string index]
        {
            get
            {
                if (this.keyItem.ContainsKey(index))
                    return this.keyItem[index];
                return null;
            }
        }

		private void ParseKeyItem(PsdBinaryReader reader)
		{
			int count = reader.ReadInt32 ();
			for (int i=0; i<count; i++) 
			{
				string key = DecoderFactory.ReadItemKey(reader);
				this.keyItem.Add(key,DecoderFactory.Load (reader));
			}
		}
    }
}
