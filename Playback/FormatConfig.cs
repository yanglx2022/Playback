using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Playback
{
    /// <summary>
    /// 数据格式配置
    /// </summary>
    public class FormatConfig
    {
        /// <summary>
        /// 从文件读取数据格式配置
        /// </summary>
        public static List<Format> LoadFromFile(string filename = "format_config.xml")
        {
            return Parse(filename);
        }

        /// <summary>
        /// 解析配置xml
        /// </summary>
        private static List<Format> Parse(string filename)
        {
            List<Format> formats = new List<Format>();
            FileInfo info = new FileInfo(filename);
            if (info.Exists)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("format");
                for(int i = 0; i < nodeList.Count; i++)
                {
                    Format format = new Format();
                    formats.Add(format);
                    foreach (XmlNode node in nodeList[i])
                    {
                        switch (node.Name)
                        {
                            case "name":
                                format.Name = node.InnerText;
                                break;
                            case "desc":
                                format.Description = node.InnerText;
                                break;
                            case "pattern":
                                format.Add(new Format.Pattern(node));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return formats;
        }
    }
}
