using System;
using System.Collections.Generic;
using System.Text;

namespace Playback
{
    /// <summary>
    /// 一次回放的数据
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// 数据时间戳
        /// </summary>
        public readonly long timeStamp = 0;
        /// <summary>
        /// 原始文本数据
        /// </summary>
        public readonly string text = "";

        /// <summary>
        /// 数据可用(有时间戳)
        /// </summary>
        public readonly bool Checked = false;

        /// <summary>
        /// 一次回放的数据
        /// </summary>
        public Packet(string text)
        {
            this.text = text;
            try
            {
                timeStamp = Convert.ToInt64(text.Substring(0, text.IndexOf(',')));
                Checked = true;
            }
            catch { }
        }

        /// <summary>
        /// 解析文本数据
        /// </summary>
        public virtual byte[] Parse()
        {
            return new byte[] { };
        }
    }
}
