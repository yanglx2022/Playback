using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Playback
{
    /// <summary>
    /// 数据格式
    /// </summary>
    public class Format
    {
        #region 支持的基本数据类型定义
        public const int TYPE_UNKNOWN = -1;
        public const int TYPE_BOOL = 0;
        public const int TYPE_INT8 = 1;
        public const int TYPE_CHAR = 1;
        public const int TYPE_INT16 = 2;
        public const int TYPE_SHORT = 2;
        public const int TYPE_INT32 = 3;
        public const int TYPE_INT = 3;
        public const int TYPE_INT64 = 4;
        public const int TYPE_LONG = 4;
        public const int TYPE_FLOAT = 5;
        public const int TYPE_DOUBLE = 6;
        public const int TYPE_UINT8 = 11;
        public const int TYPE_UCHAR = 11;
        public const int TYPE_UINT16 = 12;
        public const int TYPE_USHORT = 12;
        public const int TYPE_UINT32 = 13;
        public const int TYPE_UINT = 13;
        public const int TYPE_UINT64 = 14;
        public const int TYPE_ULONG = 14;
        public const int TYPE_STRING = 20;
        #endregion

        /// <summary>
        /// 模式
        /// </summary>
        public class Pattern
        {
            /// <summary>
            /// 类型列表
            /// </summary>
            public List<string> TypeList = new List<string>();
            /// <summary>
            /// 分隔符列表
            /// </summary>
            public List<string> SepartorList = new List<string>();
            /// <summary>
            /// 嵌套分隔符列表
            /// </summary>
            public List<string> NestSepartorList = new List<string>();

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
                            break;
                        }
                    }
                    subNode = subNode.ChildNodes[0];
                }
                // 类型与分隔符
                string typelist = subNode.InnerText.Trim();
                int left = typelist.IndexOf('[');
                int right = typelist.IndexOf(']');
                TypeList.Add(typelist.Substring(0, left >= 0 ? left : typelist.Length));
                while (left > 0 && right > 0 && left < right - 1)
                {
                    SepartorList.Add(typelist.Substring(left + 1, right - left - 1));
                    typelist = typelist.Substring(right + 1);
                    left = typelist.IndexOf('[');
                    right = typelist.IndexOf(']');
                    if (left > 0)
                    {
                        TypeList.Add(typelist.Substring(0, left));
                    }
                    else if (left < 0 && typelist.Length > 0)
                    {
                        TypeList.Add(typelist);
                    }
                }
            }
        }

        /// <summary>
        /// 类型名称索引
        /// </summary>
        public int this[string typename]
        {
            get
            {
                switch(typename.Trim().ToLower())
                {
                    case "bool":
                    case "boolean":
                        return TYPE_BOOL;
                    case "char":
                    case "int8":
                        return TYPE_INT8;
                    case "short":
                    case "int16":
                        return TYPE_INT16;
                    case "int":
                    case "int32":
                        return TYPE_INT32;
                    case "long":
                    case "int64":
                        return TYPE_INT64;
                    case "float":
                        return TYPE_FLOAT;
                    case "double":
                        return TYPE_DOUBLE;
                    case "uchar":
                    case "uint8":
                        return TYPE_UINT8;
                    case "ushort":
                    case "uint16":
                        return TYPE_UINT16;
                    case "uint":
                    case "uint32":
                        return TYPE_UINT32;
                    case "ulong":
                    case "uint64":
                        return TYPE_UINT64;
                    case "string":
                        return TYPE_STRING;
                    default:
                        return TYPE_UNKNOWN;
                }
            }
        }

        /// <summary>
        /// 模式列表
        /// </summary>
        public List<Pattern> PatternList = new List<Pattern>();
        /// <summary>
        /// 名称
        /// </summary>
        public string Name = "";
        /// <summary>
        /// 描述
        /// </summary>
        public string Description = "";

        /// <summary>
        /// 数据格式
        /// </summary>
        public Format()
        {
        }
    }
}
