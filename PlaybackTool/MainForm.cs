using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Playback;


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

        public MainForm()
        {
            InitializeComponent();

            // 数据格式配置
            FormatConfig config = new FormatConfig();
            // 回放
            playback = new Playback.Playback(new Dataset(new List<Packet>()));
            playback.OnPlay += Cast;
            playback.OnPacketPlayed += PacketPlayed;
            //// 网络
            //var listen = new MulticastListener(
            //    pack => Record(pack.Payload), (byte)UdpCmd.Common);
            //var hub = new RemoteHub("PlaybackTool", additions: listen);
        }

        // 写文件
        private void Record(byte[] data)
        { }

        void Cast(byte[] data)
        {
            Console.WriteLine("=====play=====");
        }

        void PacketPlayed()
        {
            trackBar1.Invoke(new Action<int>(value => {
                trackBar1.Value = value;
            }),
                playback.PlayedCount);
        }

        private void BtnPlay_Click(object sender, EventArgs e)
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
                    playback.Load(filename);
                    trackBar1.SetRange(0, playback.Count);
                    trackBar1.LargeChange = 1;
                    trackBar1.Value = 0;
                    trackBar1.TickFrequency = playback.Count / 10;
                    playback.Start();
                    btnPlay.Text = "停止回放";
                    this.Text = "数据回放工具 - 正在回放";
                }
            }
            else
            {
                playback.Stop();
                btnPlay.Text = "回放";
                trackBar1.Value = 0;
                this.Text = "数据回放工具";
            }
        }

    }
}
