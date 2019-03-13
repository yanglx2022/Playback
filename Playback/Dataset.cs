﻿using System.Collections.Generic;
using System.IO;

namespace Playback {
	/// <summary>
	///     回放数据集合
	/// </summary>
	public class Dataset {
		/// <summary>
		///     时间间隔列表
		/// </summary>
		public readonly List<int> Intervals = new List<int>();

		/// <summary>
		///     数据
		/// </summary>
		private List<Packet> _packetList;

		/// <summary>
		///     回放数据
		/// </summary>
		public Dataset(List<Packet> packets) => _packetList = packets;

		/// <summary>
		///     回放数据数量
		/// </summary>
		public int Count => _packetList.Count;

		/// <summary>
		///     回放数据
		/// </summary>
		public Packet this[int index] => _packetList[index];

		/// <summary>
		///     加载数据
		/// </summary>
		public virtual void Load(string str) => _packetList = LoadFromFile(str);

		/// <summary>
		///     从文件加载数据
		/// </summary>
		public List<Packet> LoadFromFile(string fileName) {
			var packets = new List<Packet>();
			var info    = new FileInfo(fileName);
			if (info.Exists)
				using (var reader = new StreamReader(fileName)) {
					var line = reader.ReadLine();
					while (line != null) {
						var packet = new Packet(line);
						if (packet.Checked) packets.Add(packet);

						line = reader.ReadLine();
					}

					reader.Close();
				}

			return packets;
		}
	}
}
