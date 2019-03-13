using System;
using System.Collections.Generic;
using System.IO;

namespace Playback
{
    /// <summary>
    /// 回放数据集合
    /// </summary>
    public class Dataset
    {
        /// <summary>
        /// 数据
        /// </summary>
        private List<Packet> packetList;

        /// <summary>
        /// 时间间隔列表
        /// </summary>
        public readonly List<int> Intervals = new List<int>();

        /// <summary>
        /// 回放数据数量
        /// </summary>
        public int Count
        {
            get
            {
                return packetList.Count;
            }
        }

        /// <summary>
        ///  回放数据
        /// </summary>
        public Packet this[int index]
        {
            get
            {
                return packetList[index];
            }
        }

        /// <summary>
        ///  回放数据
        /// </summary>
        public Dataset(List<Packet> packets)
        {
            packetList = packets;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void Load(string str)
        {
            packetList = LoadFromFile(str);
        }
        

        /// <summary>
        /// 从文件加载数据
        /// </summary>
        public List<Packet> LoadFromFile(string fileName)
        {
            List<Packet> packets = new List<Packet>();
            FileInfo info = new FileInfo(fileName);
            if (info != null && info.Exists)
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        Packet packet = new Packet(line);
                        if (packet.Checked)
                        {
                            packets.Add(packet);
                        }
                        line = reader.ReadLine();
                    }
                    reader.Close();
                }
            }
            return packets;
        }
    }
}


