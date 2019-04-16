using System;
using System.Collections.Generic;
using System.Linq;

namespace Playback {
	/// <summary>
	///     一次回放的数据
	/// </summary>
	public class Packet {
		/// <summary>
		/// 原始文本数据(不包含时间戳)
		/// </summary>
		public readonly string Text;

        /// <summary>
        /// 二进制数据
        /// </summary>
        private byte[] Data = new byte[] { };

		/// <summary>
		/// 数据时间戳
		/// </summary>
		public readonly long TimeStamp;

        /// <summary>
        /// 包类型
        /// </summary>
        public byte Type;

        /// <summary>
        /// 数据包
        /// </summary>
		private Packet(string text, long timeStamp)
        {
			Text      = text;
			TimeStamp = timeStamp;
		}

		/// <summary>
		/// 	尝试解析一个数据	
		/// </summary>
		/// <param name="text">行</param>
		/// <param name="packet">解析成功数据</param>
		/// <returns></returns>
		public static bool TryParse(string text, out Packet packet)
        {
			var head  = new string(text.TakeWhile(char.IsDigit).ToArray());
			var check = long.TryParse(head, out var value);
			packet = check ? new Packet(new string(text.Skip(head.Length + 1).ToArray()), value) : default;
			return check;
		}

		/// <summary>
		///     解析文本数据
		/// </summary>
		public virtual byte[] Parse(Format format)
        {
            if (Data.Length > 0)
            {
                byte[] data = new byte[Data.Length];
                Buffer.BlockCopy(Data, 0, data, 0, Data.Length);
                return data;
            }
            for(int i = 0; i < format.Count; i++)
            {
                if (Parse(format[i], out byte[] data))
                {
                    Data = new byte[data.Length];
                    Buffer.BlockCopy(data, 0, Data, 0, data.Length);
                    return data;
                }
            }
            return new byte[] { };
        }

        // 按一种模式解析
		private bool Parse(Format.Pattern pattern, out byte[] data)
        {
            data = new byte[] { };
			List<byte> dataList = new List<byte>();
            List<string> sepList = new List<string>();
            foreach(string sep in pattern.SepartorList)
            {
                if (!Text.Contains(sep))
                {
                    // 数据应该包含所有基本分隔符
                    return false;
                }
            }
			sepList.AddRange(pattern.SepartorList);
			sepList.AddRange(pattern.NestSepartorList);
			string[] items = Text.Split(sepList.ToArray(), StringSplitOptions.None);
            if ((!pattern.Nest && items.Length != pattern.TypeList.Count) || 
                (pattern.Nest && ((items.Length % pattern.TypeList.Count) != 0)))
            {
                // 非嵌套模式元素数量应与类型数量相等
                // 嵌套模式元素数量应与类型数量成倍数关系
                return false;
            }
			for (int i = 0; i < items.Length; i++)
            {
                byte[] bytes = new byte[] { };
                bool parse = items[i].TryParseEx(
                    pattern.TypeList[i % pattern.TypeList.Count].ToType(), out bytes);
                if (!parse)
                {
                    return false;
                }
                foreach(byte b in bytes)
                {
                    dataList.Add(b);
                }
			}
            List<byte> list = new List<byte>();
            if (pattern.Type == 5)
            {
                list.AddRange(System.Text.Encoding.Default.GetBytes("Position"));
                list.Add(0);
            }
            list.AddRange(dataList);
            data = list.ToArray();
            Type = pattern.Type;    // 设置包类型
            return true;
		}

        /// <summary>
        /// 按格式将字节数组解析为字符串(最多一层嵌套分隔)
        /// </summary>
        public static string Pasre(byte[] data, Format.Pattern pattern)
        {
            string text = Environment.TickCount.ToString() + ",";
            int index = 0;
            while(index < data.Length)
            {
                for(int i = 0; i < pattern.TypeList.Count; i++)
                {
                    int len = 0;
                    if ((len = data.TryParseEx(index, pattern.TypeList[i].ToType(), out string val)) >= 0)
                    {
                        index += len;
                        text += val;
                        if (i < pattern.TypeList.Count - 1)
                        {
                            text += pattern.SepartorList[i];
                        }
                        else if (pattern.NestSepartorList.Count > 0)
                        {
                            text += pattern.NestSepartorList.Last();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            if (pattern.NestSepartorList.Count > 0)
            {
                text = text.Substring(0, text.Length - pattern.NestSepartorList.Last().Length);
            }
            return text;
        }
	}
}
