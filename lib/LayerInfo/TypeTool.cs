/*
 * File: Assets/Editor/PSD/LayerInfo/TypeLayerInfo.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/17/2015 5:11:28 PM
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace Com.Lucky.PhotoShop
{
    public sealed class TypeTool : LayerInfo
    {
        
        internal short version;
        internal double[] transforms = new double[6];
        internal short textVersion;
        internal int textDescriptorVersion;
        internal Descriptor textDescriptor;
        internal short wrapVersion;
        internal int wrapDescriptorVersion;
        internal Descriptor wrapDescriptor;
        internal double[] rect = new double[4];

		public EngineData engineData;

        public string text
        {
            get
            {
                return (string)this.textDescriptor["Txt "];
            }
        }
        
        
        public string fontName
        {
            get { return this.engineData.GetPropertyValue("ResourceDict>FontSet>0>Name").ToString(); }
        }

        public float fontSize
        {
			get 
            { 
                object test = GetFisrtStyleValue("FontSize");
                return Convert.ToSingle(test);
            }//this.engineData.GetValue("}
        }

        public bool isBold
        {
            get { return ParseBoolValue(GetFisrtStyleValue("FauxBold")); }
        }

        public bool isItalic
        {
            get { return ParseBoolValue(GetFisrtStyleValue("FauxItalic")); }
        }

        public bool isUnderline
        {
            get { return ParseBoolValue(GetFisrtStyleValue("Underline")); }
        }

        public float outlineWidth
        {
            get { return Convert.ToSingle(GetFisrtStyleValue("OutlineWidth")); }
        }

        public object strokeColor
        {
            get { return GetFisrtStyleValue("StrokeColor"); }
        }
        
        public Color color
        {
            get { return ArgbInt2Color((GetFisrtStyleValue("FillColor") as Dictionary<string, object>)["Values"] as IList); }
        }

        public Color[] colors
        {
            get
            {
                List<Color> colors = new List<Color>();
                foreach (var style in styles)
                {
                    if (style.ContainsKey("FillColor"))
                    {
                        Color color =
                            ArgbInt2Color(
                                (GetFisrtStyleValue("FillColor") as Dictionary<string, object>)["Values"] as IList);
                        colors.Add(color);
                    }
                }
                return colors.ToArray();
            }
        }

        //['left', 'right', 'center', 'justify']:0,1,2,3
        public int[] alignments
        {
            get
            {
                List<int> colors = new List<int>();
                foreach (var style in paragraphProperties)
                {
                    if (style.ContainsKey("Justification"))
                    {
                        int alignment = Convert.ToInt32(GetFisrtStyleValue("Justification"));
                            
                        colors.Add(alignment);
                    }
                }
                return colors.ToArray();
            }
        }



        /*
         *              /Font 0
						/FontSize 72.009
						/FauxBold false
						/FauxItalic false
						/AutoLeading true
						/Leading .01
						/HorizontalScale 1.0
						/VerticalScale 1.0
						/Tracking 0
						/AutoKerning true
						/Kerning 0
						/BaselineShift 0.0
						/FontCaps 0
						/FontBaseline 0
						/Underline false
						/Strikethrough false
						/Ligatures true
						/DLigatures false
						/BaselineDirection 2
						/Tsume 0.0
						/StyleRunAlignment 2
						/Language 0
						/NoBreak false
						/FillColor
						<<
							/Type 1
							/Values [ 1.0 0.0 0.0 0.0 ]
						>>
						/StrokeColor
						<<
							/Type 1
							/Values [ 1.0 0.0 0.0 0.0 ]
						>>
						/FillFlag true
						/StrokeFlag false
						/FillFirst false
						/YUnderline 1
						/OutlineWidth 1.0
						/CharacterDirection 0
						/HindiNumbers false
						/Kashida 1
						/DiacriticPos 2
         */
        public Dictionary<string, object>[] styles;

        public Dictionary<string, object>[] paragraphProperties;

        public Dictionary<string, object>[] resourceStyles;
        public Dictionary<string, object>[] documentStyles;

        public object GetFisrtStyleValue(string key)
        {
            if (styles == null)
                return null;
            foreach (var style in styles)
            {
                if (style.ContainsKey(key))
                    return style[key];
            }
            return null;
        }

        public object GetStyleValue(string key,int index = 0)
        {
            if (styles == null 
                || styles.Length <= index 
                || !styles[index].ContainsKey(key))
                return null;
            else
                return styles[index][key];
        }

        public object GetStyleValue(Dictionary<string,object>[] dict,string key, int index = 0)
        {
            if (dict == null
                || dict.Length <= index
                || !dict[index].ContainsKey(key))
                return null;
            else
                return dict[index][key];
        }


        public Rect area
        {
            get
            {
                Rect result = default(Rect);
                return result;
            }
        }

        private bool ParseBoolValue(object value)
        {
            if (value == null)
                return false;
            if (value is string)
                return (value as string) == "true";
            return false;
        }

        public static Color ArgbInt2Color(IList array)
        {
            if (array == null || array.Count < 4)
                return Color.white;
            return new Color(
                Convert.ToSingle(array[1]),
                Convert.ToSingle(array[2]),
                Convert.ToSingle(array[3]),
                Convert.ToSingle(array[0])
            );
        }

        protected override void ReadData(PsdBinaryReader reader, int length)
        {
            this.version = reader.ReadInt16();
            for (int i = 0; i < this.transforms.Length; i++)
            {
                this.transforms[i] = reader.ReadDouble();
            }
            this.textVersion = reader.ReadInt16();
            this.textDescriptorVersion = reader.ReadInt32();
            this.textDescriptor = new Descriptor(reader);
			this.engineData = new EngineData ((this.textDescriptor.keyItem ["EngineData"] as RawDataItem).data);

            this.styles = GetSheetSetData("EngineDict>StyleRun>RunArray", "StyleSheet>StyleSheetData");
            this.paragraphProperties = GetSheetSetData("EngineDict>ParagraphRun>RunArray", "ParagraphSheet>Properties");
            
            this.wrapVersion = reader.ReadInt16();//.getInt16(reader);
            this.wrapDescriptorVersion = reader.ReadInt32();
            this.wrapDescriptor = new Descriptor(reader);
            for (int j = 0; j < this.rect.Length; j++)
            {
                this.rect[j] = reader.ReadDouble();
            }
        }

        private Dictionary<string, object>[] GetSheetSetData(string parentPath, string childPath)
        {
            var runArrayList = this.engineData.GetPropertyValue(parentPath) as IList;
            var styles = new Dictionary<string, object>[runArrayList.Count];
            for (int i = 0; i < runArrayList.Count; i++)
            {
                styles[i] =
                    this.engineData.GetPropertyValue(parentPath + ">" + i + ">" + childPath) as Dictionary<string, object>;
            }
            return styles;
        }

    }
}
