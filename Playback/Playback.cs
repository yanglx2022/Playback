using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Playback {
	/// <summary>
	///     回放
	/// </summary>
	public class Playback {
		/// <summary>
		///     数据
		/// </summary>
		private readonly Dataset _dataset;

		/// <summary>
		/// 	当前回放任务通过这个取消标记来取消
		/// </summary>
		private CancellationTokenSource _cancellation;

		private int _startIndex, _endIndex;

		/// <summary>
		///     一包回放完成事件
		/// </summary>
		public Action OnPacketPlayed = null;

		/// <summary>
		///     回放实现
		/// </summary>
		public Action<byte[]> OnPlay = null;

		/// <summary>
		///     包间隔超时提醒
		/// </summary>
		public Action OnTimeout = null;

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
			_cancellation = new CancellationTokenSource();
			Task.Run(PlaybackThread, _cancellation.Token);
		}

		/// <summary>
		///     停止回放
		/// </summary>
		public void Stop() {
			_cancellation.Cancel();

			PlayedCount = StartIndex;
			OnPacketPlayed?.Invoke();
		}

		// 回放线程
		private async Task PlaybackThread() {
			try {
				PlayedCount = StartIndex;
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();

				while (PlayedCount <= EndIndex && !_cancellation.IsCancellationRequested) {
					OnPlay?.Invoke(_dataset[PlayedCount++].Parse()); // 回放数据
					OnPacketPlayed?.Invoke();                        // 回放通知

					if (PlayedCount > EndIndex) continue;

					long dt = _dataset[PlayedCount].TimeStamp - _dataset[PlayedCount - 1].TimeStamp;

					stopwatch.Stop();
					long actual = stopwatch.ElapsedMilliseconds;
					stopwatch.Restart();

					if (dt > actual)
						await Task.Delay((int) (dt - actual), _cancellation.Token);
					else if (dt < actual)
						OnTimeout?.Invoke(); // 超时通知
				}

				stopwatch.Stop();
			} catch {
				// ignore
			}
		}
	}
}
