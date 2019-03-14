using System;
using System.Collections.Generic;

namespace Playback {
	/// <summary>
	///     一次回放的数据
	/// </summary>
	public class Packet {
		/// <summary>
		///     数据可用(有时间戳)
		/// </summary>
		public readonly bool Checked;

		/// <summary>
		///     原始文本数据
		/// </summary>
		public readonly string Text;

		/// <summary>
		///     数据时间戳
		/// </summary>
		public readonly long TimeStamp;

        FormatConfig format;

		/// <summary>
		///     一次回放的数据
		/// </summary>
		public Packet(string text) {
			this.Text = text;
			try {
                //Text = text.Trim().TrimStart("0123456789".ToCharArray());
				TimeStamp = Convert.ToInt64(text.Substring(0, text.IndexOf(',')));
				Checked   = true;
			} catch {
				// ignored
			}
		}

        /// <summary>
        ///     解析文本数据
        /// </summary>
        public virtual byte[] Parse()
        {
            List<byte> data = new List<byte>();



            return data.ToArray();
        }
	}
}
