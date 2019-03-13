using System;
using System.Diagnostics;
using System.Threading;

namespace Playback {
	/// <summary>
	///     回放
	/// </summary>
	public class Playback {
		public delegate void NotifyHandler();

		public delegate void PlayHandler(byte[] data);

		/// <summary>
		///     数据
		/// </summary>
		private readonly Dataset _dataset;

		/// <summary>
		///     回放标志
		/// </summary>
		private bool _playing;

		/// <summary>
		///     回放线程
		/// </summary>
		private Thread _playThread;

		private int _startIndex, _endIndex;

		/// <summary>
		///     一包回放完成事件
		/// </summary>
		public NotifyHandler OnPacketPlayed = null;

		/// <summary>
		///     回放实现
		/// </summary>
		public PlayHandler OnPlay = null;

		/// <summary>
		///     包间隔超时提醒
		/// </summary>
		public NotifyHandler OnTimeout = null;

		// 构造
		public Playback(Dataset ds) => _dataset = ds;

		/// <summary>
		///     回放数据包数量
		/// </summary>
		public int Count => _dataset?.Count ?? 0;

		/// <summary>
		///     回放数据包计数
		/// </summary>
		public int PlayedCount { get; private set; }

		/// <summary>
		///     回放起始位置
		/// </summary>
		public int StartIndex {
			get => _startIndex;
			set => _startIndex = Math.Min(Math.Max(0, value), Count - 1);
		}

		/// <summary>
		///     回放结束位置
		/// </summary>
		public int EndIndex {
			get => _endIndex;
			set => _endIndex = Math.Min(Math.Max(0, value), Count - 1);
		}

		/// <summary>
		///     加载回放数据
		/// </summary>
		public void Load(string str) {
			PlayedCount = 0;
			_dataset.Load(str);
			StartIndex = 0;
			EndIndex   = Count - 1;
		}

		/// <summary>
		///     开始回放
		/// </summary>
		public void Start() {
			_playing    = true;
			_playThread = new Thread(PlaybackThread) {IsBackground = true};
			_playThread.Start();
		}

		/// <summary>
		///     停止回放
		/// </summary>
		public void Stop() {
			_playing = false;
			Thread.Sleep(100);
			if (_playThread != null && _playThread.IsAlive) _playThread.Abort();

			PlayedCount = StartIndex;
			OnPacketPlayed?.Invoke();
		}

		// 回放线程
		private void PlaybackThread() {
			try {
				PlayedCount = StartIndex;
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while (PlayedCount <= EndIndex && _playing) {
					OnPlay?.Invoke(_dataset[PlayedCount].Parse()); // 回放数据
					PlayedCount++;
					OnPacketPlayed?.Invoke();    // 回放通知
					if (PlayedCount <= EndIndex) // 包间延时
					{
						var dt = _dataset[PlayedCount].timeStamp
						       - _dataset[PlayedCount - 1].timeStamp;
						stopwatch.Stop();
						if (dt > stopwatch.ElapsedMilliseconds)
							Thread.Sleep((int) (dt - stopwatch.ElapsedMilliseconds));

						stopwatch.Restart();
						if (dt < stopwatch.ElapsedMilliseconds) OnTimeout?.Invoke(); // 超时通知
					}
				}

				stopwatch.Stop();
			} catch {
				// ignore
			}
		}
	}
}
