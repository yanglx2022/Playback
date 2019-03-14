using System;
using System.Collections.Generic;
using System.Linq;

namespace Playback {
	/// <summary>
	///     一次回放的数据
	/// </summary>
	public class Packet {
		/// <summary>
		///     原始文本数据
		/// </summary>
		public readonly string Text;

		/// <summary>
		///     数据时间戳
		/// </summary>
		public readonly long TimeStamp;

		private Packet(string text, long timeStamp) {
			Text      = text;
			TimeStamp = timeStamp;
		}

		/// <summary>
		/// 	尝试解析一个数据	
		/// </summary>
		/// <param name="text">行</param>
		/// <param name="packet">解析成功数据</param>
		/// <returns></returns>
		public static bool TryParse(string text, out Packet packet) {
			var head  = new string(text.TakeWhile(char.IsDigit).ToArray());
			var check = long.TryParse(head, out var value);
			packet = check ? new Packet(new string(text.Skip(head.Length + 1).ToArray()), value) : default;
			return check;
		}

		/// <summary>
		///     解析文本数据
		/// </summary>
		public virtual byte[] Parse() => new byte[] { };

		private byte[] Parse(Format.Pattern pattern) {
			List<byte>   data    = new List<byte>();
			List<string> sepList = new List<string>();
			sepList.AddRange(pattern.SepartorList);
			sepList.AddRange(pattern.NestSepartorList);
			string[] items = Text.Split(sepList.ToArray(), StringSplitOptions.None);
			for (int i = 0; i < items.Length; i++) {
				switch (pattern.TypeList[i % pattern.TypeList.Count].ToType()) {
					case Format.DataType.Unknown: break;
					case Format.DataType.Bool:    break;
					case Format.DataType.Int8:    break;
					case Format.DataType.Int16:   break;
					case Format.DataType.Int32:   break;
					case Format.DataType.Int64:   break;
					case Format.DataType.Float:   break;
					case Format.DataType.Double:  break;
					case Format.DataType.Uint8:   break;
					case Format.DataType.Uint16:  break;
					case Format.DataType.Uint32:  break;
					case Format.DataType.Uint64:  break;
					case Format.DataType.String:  break;
					default:                      throw new ArgumentOutOfRangeException();
				}

				//for(int j = 0; j < ; j++)
				//{

				//}
			}

			return data.ToArray();
		}
	}
}
