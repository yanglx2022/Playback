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
    class FormatConfig
    {
        /// <summary>
        /// 格式配置列表
        /// </summary>
        List<Format> configs = new List<Format>();

        /// <summary>
        /// 数据格式配置
        /// </summary>
        public FormatConfig()
        {
            Parse("format_config.xml");
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public void Load(string filename)
        {
            Parse(filename);
        }

        /// <summary>
        /// 解析配置xml
        /// </summary>
        private void Parse(string filename)
        {
            configs.Clear();
            FileInfo info = new FileInfo(filename);
            if (info.Exists)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("format");
                for(int i = 0; i < nodeList.Count; i++)
                {
                    Format format = new Format();
                    configs.Add(format);
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
                                format.PatternList.Add(new Format.Pattern(node));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
