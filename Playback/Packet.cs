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

        Format format;

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
            if (format.PatternList)

            return data.ToArray();
        }

        private byte[] Parse(Format.Pattern pattern)
        {
            List<byte> data = new List<byte>();
            List<string> sepList = new List<string>();
            sepList.AddRange(pattern.SepartorList);
            sepList.AddRange(pattern.NestSepartorList);
            string[] items = Text.Split(sepList.ToArray(), StringSplitOptions.None);
            for(int i = 0; i < items.Length; i++)
            {
                switch(Format[pattern.TypeList[i % pattern.TypeList.Count]])

                for(int j = 0; j < ; j++)
                {

                }
            }



            if (pattern.Nest)
            {
                for(int i = 0; i < pattern.NestSepartorList.Count; i++)
                {
                    string[] items = Text.Split(pattern.NestSepartorList[i]);
                }
            }

            return data.ToArray();
        }

    }
}
