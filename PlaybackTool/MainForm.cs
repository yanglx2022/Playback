using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Playback;
using MechDancer.Framework.Net.Resources;
using MechDancer.Common;

namespace PlaybackTool
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 回放文件名
        /// </summary>
        string filename = "";
        /// <summary>
        /// 回放框架
        /// </summary>
        Playback.Playback playback = null;
        /// <summary>
        /// 组播
        /// </summary>
        UdpNet net = null;
        /// <summary>
        /// 组播地址
        /// </summary>
        string ip = "230.1.1.100";
        /// <summary>
        /// 组播端口
        /// </summary>
        int port = 60100;
        /// <summary>
        /// 写文件
        /// </summary>
        StreamWriter writer = null;
        /// <summary>
        /// 录制计数
        /// </summary>
        int recordCnt = 0;
        /// <summary>
        /// 时间戳起始值
        /// </summary>
        long timeStampBase = 0;

        public MainForm()
        {
            InitializeComponent();
            lbCnt.Text = "";
            btnNet.Text = "\uE713";
            timer1.Start();
        }

        // 定时器中启动选择窗体
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            // 配置选择
            SelectionForm form = new SelectionForm();
            form.ShowDialog(this);
            if (form.Format == null)
            {
                Close();
                return;
            }
            // 网络
            net = new UdpNet(ip, port);
            // 回放
            playback = new Playback.Playback();
            playback.DataFormat = form.Format;
            playback.OnPlay += UdpCast;
            playback.OnPacketPlayed += PacketPlayed;
            playback.OnPlayFinished += PlayFinished;
        }

        // 发送组播
        void UdpCast(Packet packet)
        {
            net?.Cast(packet.Parse(playback.DataFormat), packet.Type);
        }

        // 发送绘图
        void DrawSend(Packet packet)
        {
            if (packet.Type == 10)  // 超声距离
            {
                if (timeStampBase == 0)
                {
                    timeStampBase = packet.TimeStamp;
                }
                byte[] data = packet.Parse(playback.DataFormat);
                for(int i = 0; i < data.Length; i += 6)
                {
                    byte id1 = data[i];
                    byte id2 = data[i + 1];
                    int value = BitConverter.ToInt32(data, i + 2);
                    if (id1 < 90 && id2 >= 90)
                    {
                        var ms = new MemoryStream();
                        ms.WriteEnd(id2 + "-" + id1);
                        var writer = new NetworkDataWriter(ms);
                        writer.Write((double)(packet.TimeStamp - timeStampBase));
                        writer.Write((double)value);
                        net?.Cast(ms.ToArray(), (byte)UdpCmd.TopicMessage);
                    }
                }
            }
        }

        // 进度条
        void PacketPlayed()
        {
            trackBar1.Invoke(new Action<int>(value => {
                trackBar1.Value = value;
            }),
                playback.PlayedCount);
        }

        // 回放结束
        void PlayFinished()
        {
            btnPlay.Invoke(new Action<string>(value => btnPlay.Text = value), "回放");
            btnPause.Invoke(new Action<string>(value => btnPause.Text = value), "暂停");
            Invoke(new Action<string>(value => Text = value), "数据回放工具");
        }

        // 写文件
        private void Record(byte[] data, byte type)
        {
            if (writer != null)
            {
                for (int i = 0; i < playback.DataFormat.Count; i++)
                {
                    if (playback.DataFormat[i].Type == type)
                    {
                        writer.WriteLine(Packet.Pasre(data, playback.DataFormat[i]));
                        recordCnt++;
                        lbCnt.Invoke(new Action<string>(value => lbCnt.Text = value), recordCnt.ToString());
                        break;
                    }
                }

            }
        }

        // 录制按钮
        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (btnRecord.Text.Contains("停止"))
            {
                net.OnDataReceived -= Record;
                writer.Close();
                writer.Dispose();
                writer = null;
                btnRecord.Text = "录制";
                progressBar1.Style = ProgressBarStyle.Blocks;
                this.Text = "数据回放工具";
                btnPlay.Enabled = true;
            }
            else
            {
                string name = playback.DataFormat.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
                writer = new StreamWriter(name);
                net.OnDataReceived += Record;
                btnRecord.Text = "停止录制";
                progressBar1.Style = ProgressBarStyle.Marquee;
                this.Text = "数据回放工具 - 正在录制";
                btnPlay.Enabled = false;
            }
        }

        // 回放按钮
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (btnPlay.Text == "回放")
            {
                if (!cbRepeat.Checked || filename == "")
                {
                    openFileDialog1.InitialDirectory = "";
                    openFileDialog1.Filter = "数据文件|*.txt|所有文件|*.*";
                    openFileDialog1.RestoreDirectory = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        filename = openFileDialog1.FileName;
                        cbRepeat.Text = filename.Split('\\').Last();
                    }
                    else
                    {
                        return;
                    }
                }
                if (filename != "")
                {
                    if (!cbRepeat.Checked || playback.Count == 0)
                    {
                        playback.Load(filename);
                        trackBar1.SetRange(0, playback.Count);
                        trackBar1.LargeChange = 1;
                        trackBar1.TickFrequency = playback.Count / 100;
                    }
                    playback.Start();
                    btnPlay.Text = "停止回放";
                    btnPause.Text = "暂停";
                    Text = "数据回放工具 - 正在回放";
                }
            }
            else
            {
                playback.Stop();
                btnPlay.Text = "回放";
                btnPause.Text = "暂停";
                Text = "数据回放工具";
            }
        }

        // 暂停/恢复按钮
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "暂停")
            {
                playback.Pause(true);
                btnPause.Text = "恢复";
                Text = "数据回放工具 - 暂停回放";
            }
            else
            {
                playback.Pause(false);
                btnPause.Text = "暂停";
                Text = "数据回放工具 - 正在回放";
            }
        }

        // 拖动滑块
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (playback.State != Playback.Playback.PlayState.RUN)
            {
                playback.StartIndex = trackBar1.Value;
            }
        }

        // 点击循环复选框
        private void cbLoop_CheckedChanged(object sender, EventArgs e)
        {
            playback.IsLoop = cbLoop.Checked;
        }

        // 串口发送复选框
        private void cbSend_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSend.Checked)
            {
                playback.OnPlay += DrawSend;
            }
            else
            {
                playback.OnPlay -= DrawSend;
            }
        }

        // 组播设置
        private void btnNet_Click(object sender, EventArgs e)
        {
            NetForm form = new NetForm(ip, port);
            form.ShowDialog(this);
            if (!ip.Equals(form.Ip) || port != form.Port)
            {
                ip = form.Ip;
                port = form.Port;
                playback.Stop();
                net.Close();
                net = new UdpNet(ip, port);
            }
        }
    }
}
