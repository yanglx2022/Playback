using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Playback
{
    /// <summary>
    /// 回放
    /// </summary>
    public class Playback
    {
        public delegate void PlayHandler(byte[] data);
        /// <summary>
        /// 回放实现
        /// </summary>
        public PlayHandler OnPlay = null;

        public delegate void NotifyHandler();
        /// <summary>
        /// 一包回放完成事件
        /// </summary>
        public NotifyHandler OnPacketPlayed = null;
        /// <summary>
        /// 包间隔超时提醒
        /// </summary>
        public NotifyHandler OnTimeout = null;

        /// <summary>
        /// 数据
        /// </summary>
        private readonly Dataset dataset;

        /// <summary>
        /// 回放标志
        /// </summary>
        private bool playing = false;

        /// <summary>
        /// 回放线程
        /// </summary>
        private Thread playThread;

        /// <summary>
        /// 回放数据包数量
        /// </summary>
        public int Count
        {
            get
            {
                return dataset == null ? 0 : dataset.Count;
            }
        }

        /// <summary>
        /// 回放数据包计数
        /// </summary>
        public int PlayedCount { get; private set; } = 0;

        /// <summary>
        /// 回放起始位置
        /// </summary>
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
            set
            {
                startIndex = value > 0 ? (value < Count ? value : Count - 1) : 0; 
            }
        }
        private int startIndex = 0;

        /// <summary>
        /// 回放结束位置
        /// </summary>
        public int EndIndex
        {
            get
            {
                return endIndex;
            }
            set
            {
                endIndex = value > 0 ? (value < Count ? value : Count - 1) : 0;
            }
        }
        private int endIndex = 0;

        // 构造
        public Playback(Dataset ds)
        {
            dataset = ds;
        }

        /// <summary>
        /// 加载回放数据
        /// </summary>
        public void Load(string str)
        {
            PlayedCount = 0;
            dataset.Load(str);
            StartIndex = 0;
            EndIndex = Count - 1;
        }

        /// <summary>
        /// 开始回放
        /// </summary>
        public void Start()
        {
            playing = true;
            playThread = new Thread(PlaybackThread);
            playThread.IsBackground = true;
            playThread.Start();
        }

        /// <summary>
        /// 停止回放
        /// </summary>
        public void Stop()
        {
            playing = false;
            Thread.Sleep(100);
            if (playThread != null && playThread.IsAlive)
            {
                playThread.Abort();
            }
            PlayedCount = StartIndex;
            OnPacketPlayed?.Invoke();
        }

        // 回放线程
        private void PlaybackThread()
        {
            try
            {
                PlayedCount = StartIndex;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (PlayedCount <= EndIndex && playing)
                {
                    OnPlay?.Invoke(dataset[PlayedCount].Parse());// 回放数据
                    PlayedCount++;
                    OnPacketPlayed?.Invoke();                    // 回放通知
                    if (PlayedCount <= EndIndex)                 // 包间延时
                    {
                        long dt = dataset[PlayedCount].timeStamp
                            - dataset[PlayedCount - 1].timeStamp;
                        stopwatch.Stop();
                        if (dt > stopwatch.ElapsedMilliseconds)
                        {
                            Thread.Sleep((int)(dt - stopwatch.ElapsedMilliseconds));
                        }
                        stopwatch.Restart();
                        if (dt < stopwatch.ElapsedMilliseconds)
                        {
                            OnTimeout?.Invoke();                 // 超时通知
                        }
                    }
                }
                stopwatch.Stop();
            }
            catch { }
        }
    }
}
