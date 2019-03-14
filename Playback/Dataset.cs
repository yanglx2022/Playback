using System.Collections.Generic;
using System.IO;
using System.Linq;

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
		public virtual void Load(string str) => _packetList = LoadFromFile(str).ToList();

		/// <summary>
		///     从文件加载数据
		/// </summary>
		private static IEnumerable<Packet> LoadFromFile(string fileName) {
			if (!new FileInfo(fileName).Exists) yield break;

			using (StreamReader reader = new StreamReader(fileName)) {
				string line = reader.ReadLine();
				while (line != null) {
					if (Packet.TryParse(line, out var packet))
						yield return packet;

					line = reader.ReadLine();
				}
			}
		}
	}
}
