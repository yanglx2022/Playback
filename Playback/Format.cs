using System;
using System.Collections.Generic;
using System.Xml;

namespace Playback {
	/// <summary>
	/// 数据格式
	/// </summary>
	public class Format
    {
        /// <summary>
        /// 支持的基本数据类型定义
        /// </summary>
		public enum DataType {
			Unknown,
			Bool,
			Int8,
			Int16,
			Int32,
			Int64,
			Float,
			Double,
			UInt8,
			UInt16,
			UInt32,
			UInt64,
			String
		}

		/// <summary>
		/// 描述
		/// </summary>
		public string Description = "";

		/// <summary>
		/// 名称
		/// </summary>
		public string Name = "";

		/// <summary>
		/// 模式
		/// </summary>
        public Pattern this[int index] => _patterns[index];

        /// <summary>
        /// 模式数量
        /// </summary>
        public int Count => _patterns.Count;

        // 模式列表
        private List<Pattern> _patterns = new List<Pattern>();

        /// <summary>
        /// 数据格式
        /// </summary>
        public Format() { }

        /// <summary>
        /// 添加模式
        /// </summary>
        public void Add(Pattern pattern)
        {
            _patterns.Add(pattern);
        }

		/// <summary>
		/// 模式
		/// </summary>
		public class Pattern {
			/// <summary>
			/// 嵌套分隔符列表
			/// </summary>
			public List<string> NestSepartorList = new List<string>();

			/// <summary>
			/// 分隔符列表
			/// </summary>
			public List<string> SepartorList = new List<string>();

			/// <summary>
			/// 数据类型列表
			/// </summary>
			public List<string> TypeList = new List<string>();

            /// <summary>
            /// 包类型
            /// </summary>
            public readonly byte Type = 0;

            /// <summary>
			/// 是否嵌套模式
			/// </summary>
			public bool Nest => NestSepartorList.Count > 0;


            /// <summary>
            /// 模式
            /// </summary>
            public Pattern(XmlNode node)
            {
				// 嵌套分隔符
				XmlNode subNode = node;
				while (subNode.ChildNodes.Count > 0)
                {
					foreach (XmlAttribute attribute in subNode.Attributes)
                    {
						if (attribute.Name == "sep")
                        {
							NestSepartorList.Add(attribute.InnerText);
						}
                        else if (attribute.Name == "type")
                        {
                            byte.TryParse(attribute.InnerText, out Type);
                        }
					}

					subNode = subNode.ChildNodes[0];
				}

				// 类型与分隔符
				string typelist = subNode.InnerText.Trim();
				int    left     = typelist.IndexOf('[');
				int    right    = typelist.IndexOf(']');
				TypeList.Add(typelist.Substring(0, left >= 0 ? left : typelist.Length));
				while (left > 0 && right > 0 && left < right - 1) {
					SepartorList.Add(typelist.Substring(left + 1, right - left - 1));
					typelist = typelist.Substring(right + 1);
					left     = typelist.IndexOf('[');
					right    = typelist.IndexOf(']');
					if (left > 0) {
						TypeList.Add(typelist.Substring(0, left));
					} else if (left < 0 && typelist.Length > 0) {
						TypeList.Add(typelist);
					}
				}
			}
		}
	}

    /// <summary>
    /// 扩展
    /// </summary>
	public static class Extensions
    {
        /// <summary>
        /// 转Format.DataType类型
        /// </summary>
        public static Format.DataType ToType(this string text) {
			switch (text.Trim().ToLower()) {
				case "bool":
				case "boolean":
					return Format.DataType.Bool;
                case "sbyte":
				case "char":
				case "int8":
					return Format.DataType.Int8;
				case "short":
				case "int16":
					return Format.DataType.Int16;
				case "int":
				case "int32":
					return Format.DataType.Int32;
				case "long":
				case "int64":
					return Format.DataType.Int64;
				case "float":
					return Format.DataType.Float;
				case "double":
					return Format.DataType.Double;
                case "byte":
                case "uchar":
				case "uint8":
					return Format.DataType.UInt8;
				case "ushort":
				case "uint16":
					return Format.DataType.UInt16;
				case "uint":
				case "uint32":
					return Format.DataType.UInt32;
				case "ulong":
				case "uint64":
					return Format.DataType.UInt64;
				case "string":
					return Format.DataType.String;
				default:
					return Format.DataType.Unknown;
			}
		}
        /// <summary>
        /// 按类型字符串转字节数组
        /// </summary>
        public static bool TryParseEx(this string text, Format.DataType type, out byte[] data)
        {
            data = new byte[] { };
            try
            {
                bool parse = true;
                switch (type)
                {
                    case Format.DataType.Bool:
                        {
                            if (parse = bool.TryParse(text, out bool value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.Int8:
                        {
                            if (parse = sbyte.TryParse(text, out sbyte value))
                            {
                                data = new byte[] { (byte)value };
                            }
                        }
                        break;
                    case Format.DataType.Int16:
                        {
                            if (parse = Int16.TryParse(text, out Int16 value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.Int32:
                        {
                            if (parse = Int32.TryParse(text, out Int32 value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.Int64:
                        {
                            if (parse = Int64.TryParse(text, out long value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.Float:
                        {
                            if (parse = float.TryParse(text, out float value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.Double:
                        {
                            if (parse = double.TryParse(text, out double value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.UInt8:
                        {
                            if (parse = byte.TryParse(text, out byte value))
                            {
                                data = new byte[] { value };
                            }
                        }
                        break;
                    case Format.DataType.UInt16:
                        {
                            if (parse = UInt16.TryParse(text, out ushort value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.UInt32:
                        {
                            if (parse = UInt32.TryParse(text, out uint value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.UInt64:
                        {
                            if (parse = UInt64.TryParse(text, out ulong value))
                            {
                                data = BitConverter.GetBytes(value);
                            }
                        }
                        break;
                    case Format.DataType.String:
                        {
                            data = System.Text.Encoding.UTF8.GetBytes(text);
                        }
                        break;
                    default:
                        parse = false;
                        break;
                }
                return parse;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 按类型字节数组转字符串
        /// </summary>
        public static int TryParseEx(this byte[] data, int startIndex, Format.DataType type, out string text)
        {
            text = "";
            try
            {
                switch (type)
                {
                    case Format.DataType.Bool:
                        text = BitConverter.ToBoolean(data, startIndex).ToString();
                        return 1;
                    case Format.DataType.Int8:
                        text = ((int)data[startIndex]).ToString();
                        return 1;
                    case Format.DataType.Int16:
                        text = BitConverter.ToInt16(data, startIndex).ToString();
                        return 2;
                    case Format.DataType.Int32:
                        text = BitConverter.ToInt32(data, startIndex).ToString();
                        return 4;
                    case Format.DataType.Int64:
                        text = BitConverter.ToInt64(data, startIndex).ToString();
                        return 8;
                    case Format.DataType.Float:
                        text = BitConverter.ToSingle(data, startIndex).ToString();
                        return 4;
                    case Format.DataType.Double:
                        text = BitConverter.ToDouble(data, startIndex).ToString();
                        return 8;
                    case Format.DataType.UInt8:
                        text = data[startIndex].ToString();
                        return 1;
                    case Format.DataType.UInt16:
                        text = BitConverter.ToUInt16(data, startIndex).ToString();
                        return 2;
                    case Format.DataType.UInt32:
                        text = BitConverter.ToUInt32(data, startIndex).ToString();
                        return 4;
                    case Format.DataType.UInt64:
                        text = BitConverter.ToUInt64(data, startIndex).ToString();
                        return 8;
                    case Format.DataType.String:
                        for(int i = startIndex; i < data.Length; i++)
                        {
                            if (data[i] == 0)
                            {
                                int len = i - startIndex + 1;
                                text = System.Text.Encoding.UTF8.GetString(data, startIndex, len);
                                return len;
                            }
                        }
                        return -1;
                    default:
                        return -1;
                }
            }
            catch { }
            return -1;
        }
	}
}
