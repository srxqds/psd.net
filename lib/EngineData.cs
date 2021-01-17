/*
 * File: Assets/Editor/PSD/EngineData.cs
 * Project: Demo
 * Company: com.luckygz
 * Porter: D.S.Qiu
 * Create Date: 7/28/2015 3:57:02 PM
 */
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System;

namespace Com.Lucky.PhotoShop
{
    /// <summary>
    /// 解析存储为：key-value，
    /// 其中 value 有三种类型：
    /// object(string bool decimal): /ShapeType 0
    /// List<object>: /xxx [ << >> ]
    /// Dictionary<string,object>: /xxx << >>
    /// </summary>
    public class EngineData
    {
        public interface MatchDecoder
        {
            string Pattern { get; }
            bool Parse(EngineData engine, string text);
        }

        //struct
        public class HashStartMatch:MatchDecoder
        {
            public string Pattern { get { return "^<<$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text,this.Pattern);
                if (match.Success)
                    engine.PushNode(new Dictionary<string,object>());
                return match.Success;
            }
           
        }

        public class HashEndMatch : MatchDecoder
        {
            public string Pattern { get { return "^>>$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                    engine.UpdateNode();
                return match.Success;
            }
            
        }

        public class MultiLineArrayStartMatch : MatchDecoder
        {
            public string Pattern { get { return @"^/(\w+) \[$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                {
                    engine.PushKey(match.Groups[1].Value);
                    engine.PushNode(new List<object>());
                }
                return match.Success;
            }
        }

        public class MultiLineArrayEndMatch : MatchDecoder
        {
            public string Pattern { get { return @"^\]$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                    engine.UpdateNode();
                return match.Success;
            }
        }

        public class PropertyMatch : MatchDecoder
        {
            public string Pattern { get { return @"^\/([A-Z0-9a-z]+)$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                {
                    engine.PushKey(match.Groups[1].Value);
                }
                return match.Success;
            }
        }

        public class PropertyWithDataMatch : MatchDecoder
        {
            public string Pattern { get { return @"^\/([A-Z0-9a-z]+)\s((.|\r)*)$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                {
					engine.PushKey(match.Groups[1].Value);
					engine.PushNode(new object());
                    engine.Match(match.Groups[2].Value);
					engine.UpdateNode();
                }
                return match.Success;
            }
        }

        //value 
        public class SingleLineArrayMatch : MatchDecoder
        {
            public string Pattern { get { return @"^\[(.*)\]$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                {
                    engine.PushValue(new List<object>());
                    string[] tempStr = match.Groups[1].Value.Trim().Split(' ');
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        engine.Match(tempStr[i]);
                    }
                }
                return match.Success;
            }
        }

        public class BooleanMatch : MatchDecoder
        {
            public string Pattern { get { return @"^(true|false)$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if(match.Success)
                    engine.PushValue(text == "true" ? true : false);
                return match.Success;
            }
        }

        public class NumberMatch : MatchDecoder
        {
            public string Pattern { get { return @"^-?\d+$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                    engine.PushValue(decimal.Parse(text));
                return match.Success;
            }
        }

        public class NumberWithDecimalMatch : MatchDecoder
        {
            public string Pattern { get { return @"^(-?\d*)\.(\d+)$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                    engine.PushValue(decimal.Parse(text));
                return match.Success;
            }
        }

        public class StringMatch : MatchDecoder
        {
            public string Pattern { get { return @"^\(((.|\r)*)\)$"; } }
            public bool Parse(EngineData engine, string text)
            {
                Match match = Regex.Match(text, this.Pattern);
                if (match.Success)
                {
                    string txt = match.Groups[1].Value;
                    byte[] chars = new byte[txt.Length];
                    for (int i = 0; i < txt.Length; i++)
                    {
                        chars[i] = (byte)txt[i];
                    }
					//txt.ch
					char[] cha = text.ToCharArray();
					byte[] ss = Encoding.ASCII.GetBytes(txt);
					byte[] s = Encoding.BigEndianUnicode.GetBytes(txt);
					byte[] s1 = Encoding.Unicode.GetBytes(txt);
					//new UnicodeEncoding(true,true)
					engine.PushValue(new UnicodeEncoding(true,true,true).GetString(chars));
                }
                return match.Success;
            }
        }

        
        public class MatchItem
        {
            public string key;
            public object value;

        }
      
        public Stack<MatchItem> keyValue = new Stack<MatchItem>();
        public MatchItem decoderNode;

		public static List<MatchDecoder> decoderList = new List<MatchDecoder>();

        static EngineData()
        {
            decoderList.Add(new HashStartMatch());
            decoderList.Add(new HashEndMatch());
            decoderList.Add(new MultiLineArrayStartMatch());
            decoderList.Add(new MultiLineArrayEndMatch());
            decoderList.Add(new PropertyMatch());
            decoderList.Add(new PropertyWithDataMatch());
            decoderList.Add(new SingleLineArrayMatch());
            decoderList.Add(new BooleanMatch());
            decoderList.Add(new NumberMatch());
            decoderList.Add(new NumberWithDecimalMatch());
            decoderList.Add(new StringMatch());
        }
    
        public EngineData(byte[] rawData)
        {
            //clear and push root node
            keyValue.Clear();
            PushKey("EngineData");
            // ASCII   UTF-8 ???
            string engineText = Encoding.UTF7.GetString(rawData);
         //   System.IO.File.WriteAllText(EditorUtils.ProjectPath + "test.txt",engineText,Encoding.UTF8);
            string[] textSegment = engineText.Split('\n');
            string currentMatchText;
            for(int textIndex = 0, textLength = textSegment.Length; textIndex<textLength; textIndex++)
            {
                currentMatchText= textSegment[textIndex];
                Match(currentMatchText);
            }
        }

		public object GetPropertyValue(string propertyPath)
		{
            string[] keys = propertyPath.Split('>');
			int length = keys.Length;
			int index = 0;
			object temp = decoderNode.value;//  as Dictionary<string,object>;
			while (index < length) 
			{
                if (temp is IList)
                {
                    IList listValue = temp as IList;
                    int listIndex;
                    if (int.TryParse(keys[index], out listIndex) && listIndex < listValue.Count)
                    {
                        temp = listValue[listIndex];
						index++;
                    }
                    else
                    {
                        return null;
                    }
                }
				else if(temp is IDictionary)
				{
                    if ((temp as Dictionary<string, object>).TryGetValue(keys[index], out temp))
                        index++;
                    else
                        return null;
				}
				else
				{
					return null;
				}

			}
			return temp;
		}

        public void Match(string currentMatchText)
        {
			currentMatchText = Regex.Replace(currentMatchText,"^\t+","");
            if(string.IsNullOrEmpty(currentMatchText))
            {
                return;
            }
            MatchDecoder currentDecoder;
            for(int decoderIndex =0; decoderIndex<decoderList.Count; decoderIndex++)
            {
                currentDecoder = decoderList[decoderIndex];
                if(currentDecoder.Parse(this,currentMatchText))
                {
                    break;
                }
            }
        }

        public void UpdateNode()
        {
            //parse complete
			if (keyValue.Count == 0) 
			{
				return;
			}
            MatchItem node = keyValue.Pop();
            if (node.value is IList)
            {
                (node.value as IList).Add(decoderNode.value);
            }
            else if(node.value is IDictionary)
            {
				(node.value as IDictionary).Add(decoderNode.key,decoderNode.value);
            }
            decoderNode = node;
        }

        public void PushNode(object initValue)
        {
			MatchItem node = new MatchItem ();
            node.value = initValue;
			if (decoderNode != null)
				keyValue.Push (decoderNode);
            decoderNode = node;
			decoderNode.key = currentKey;
        }

		private string currentKey;
        public void PushKey(string key)
        {
			currentKey = key;
        }

        public void PushValue(object value)
        {
			var lastValue = decoderNode.value;
			if (lastValue is IList)
				(lastValue as IList).Add (value);
			else 
				decoderNode.value = value;

        }
    }
}
