using System.Collections.Generic;
using System.Xml;

namespace Playback {
	/// <summary>
	/// 数据格式
	/// </summary>
	public class Format {
		#region 支持的基本数据类型定义

		public enum DataType {
			Unknown,
			Bool,
			Int8,
			Int16,
			Int32,
			Int64,
			Float,
			Double,
			Uint8,
			Uint16,
			Uint32,
			Uint64,
			String
		}

		#endregion

		/// <summary>
		/// 描述
		/// </summary>
		public string Description = "";

		/// <summary>
		/// 名称
		/// </summary>
		public string Name = "";

		/// <summary>
		/// 模式列表
		/// </summary>
		public List<Pattern> PatternList = new List<Pattern>();

		/// <summary>
		/// 数据格式
		/// </summary>
		public Format() { }

		/// <summary>
		/// 类型名称索引
		/// </summary>
		public DataType this[string typename] {
			get {
				switch (typename.Trim().ToLower()) {
					case "bool":
					case "boolean":
						return DataType.Bool;
					case "char":
					case "int8":
						return DataType.Int8;
					case "short":
					case "int16":
						return DataType.Int16;
					case "int":
					case "int32":
						return DataType.Int32;
					case "long":
					case "int64":
						return DataType.Int64;
					case "float":
						return DataType.Float;
					case "double":
						return DataType.Double;
					case "uchar":
					case "uint8":
						return DataType.Uint8;
					case "ushort":
					case "uint16":
						return DataType.Uint16;
					case "uint":
					case "uint32":
						return DataType.Uint32;
					case "ulong":
					case "uint64":
						return DataType.Uint64;
					case "string":
						return DataType.String;
					default:
						return DataType.Unknown;
				}
			}
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
			/// 类型列表
			/// </summary>
			public List<string> TypeList = new List<string>();


			/// <summary>
			/// 模式
			/// </summary>
			public Pattern(XmlNode node) {
				// 嵌套分隔符
				XmlNode subNode = node;
				while (subNode.ChildNodes.Count > 0) {
					foreach (XmlAttribute attribute in subNode.Attributes) {
						if (attribute.Name == "sep") {
							NestSepartorList.Add(attribute.InnerText);
							break;
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

			/// <summary>
			/// 是否嵌套模式
			/// </summary>
			public bool Nest => NestSepartorList.Count > 0;
		}
	}
}
