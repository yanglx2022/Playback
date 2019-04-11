using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Playback {
	/// <summary>
	///     回放
	/// </summary>
	public class Playback
    {
		/// <summary>
		///     数据
		/// </summary>
		private readonly Dataset _dataset;

		/// <summary>
		/// 	当前回放任务通过这个取消标记来取消
		/// </summary>
		private CancellationTokenSource _cancellation;

        /// <summary>
        ///     一包回放完成事件
        /// </summary>
        public Action OnPacketPlayed = null;

		/// <summary>
		///     回放实现
		/// </summary>
		public Action<Packet> OnPlay = null;

        /// <summary>
		/// 回放结束事件
		/// </summary>
		public Action OnPlayFinished = null;

        /// <summary>
        ///     包间隔超时提醒
        /// </summary>
        public Action OnTimeout = null;

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

        private int _startIndex, _endIndex;

        /// <summary>
        /// 循环播放
        /// </summary>
        public bool IsLoop = false;

        /// <summary>
        /// 数据格式
        /// </summary>
        public Format DataFormat = null;

        /// <summary>
        /// 回放状态
        /// </summary>
        public enum PlayState
        {
            STOP,   // 停止
            PAUSE,  // 暂停
            RUN     // 运行
        }

        /// <summary>
        /// 回放状态
        /// </summary>
        public PlayState State { get; private set; }

        /// <summary>
        /// 回放
        /// </summary>
		public Playback()
        {
            _dataset = new Dataset();
        }

        /// <summary>
        /// 回放
        /// </summary>
        public Playback(Dataset ds)
        {
            _dataset = ds;
        }

        /// <summary>
        ///     加载回放数据
        /// </summary>
        public void Load(string str)
        {
			_dataset.Load(str);
			StartIndex = 0;
			EndIndex   = Count - 1;
            PlayedCount = StartIndex;
        }

		/// <summary>
		///     开始回放
		/// </summary>
		public void Start()
        {
			_cancellation = new CancellationTokenSource();
            Task.Run(PlaybackThread, _cancellation.Token);
            State = PlayState.RUN;
		}

        /// <summary>
        /// 暂停/恢复回放
        /// </summary>
        /// <param name=""></param>
        public void Pause(bool pause)
        {
            State = pause ? PlayState.PAUSE : PlayState.RUN;
        }

		/// <summary>
		///     停止回放
		/// </summary>
		public void Stop()
        {
			_cancellation.Cancel();
            int timeoutCnt = 100;   // 1s超时
            while (timeoutCnt-- > 0 && State != PlayState.STOP)
            {
                Thread.Sleep(10);
            }
        }

		// 回放线程
		private async Task PlaybackThread()
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {
				PlayedCount = StartIndex;
				stopwatch.Start();
                do
                {
                    while (PlayedCount <= EndIndex && !_cancellation.IsCancellationRequested)
                    {
                        OnPlay?.Invoke(_dataset[PlayedCount++]); // 回放数据
                        OnPacketPlayed?.Invoke();                // 回放通知

                        if (PlayedCount > EndIndex) continue;

                        long dt = _dataset[PlayedCount].TimeStamp - _dataset[PlayedCount - 1].TimeStamp;
                        long actual = stopwatch.ElapsedMilliseconds;
                        stopwatch.Reset();

                        if (dt > actual)
                            await Task.Delay((int)(dt - actual), _cancellation.Token);
                        else if (dt < actual)
                            OnTimeout?.Invoke(); // 超时通知
                        // 暂停
                        while (State == PlayState.PAUSE)
                        {
                            await Task.Delay(1, _cancellation.Token);
                        }
                    }
                    PlayedCount = StartIndex;
                    OnPacketPlayed?.Invoke();
                    stopwatch.Reset();
                } while (IsLoop && !_cancellation.IsCancellationRequested);
            }
            catch
            {
				// ignore
			}
            finally
            {
                stopwatch.Stop();
                PlayedCount = StartIndex;
                OnPacketPlayed?.Invoke();
                if (State == PlayState.RUN)
                {
                    OnPlayFinished?.Invoke();
                }
                State = PlayState.STOP;
            }
		}
	}
}
