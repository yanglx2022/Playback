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
	}
}
