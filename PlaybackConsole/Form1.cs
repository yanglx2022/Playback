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

namespace PlaybackConsole
{
    public partial class Form1 : Form
    {
        string filename = "";

        Playback.Playback playback;

        public Form1()
        {
            InitializeComponent();

            //FormatConfig config = new FormatConfig();
            //config.Parse("C:\\Users\\Yang\\Desktop\\test.xml");

            playback = new Playback.Playback(new Dataset(new List<Packet>()));
            playback.OnPlay += Cast;
            playback.OnPacketPlayed += PacketPlayed;
        }

        void Cast(byte[] data)
        {
            Console.WriteLine("=====play=====");
        }

        delegate void SetValueMethod(int value);
        void PacketPlayed()
        {
            trackBar1.Invoke(new SetValueMethod( 
                value => { trackBar1.Value = value; }), playback.PlayedCount);
        }

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
                    playback.Load(filename);
                    trackBar1.SetRange(0, playback.Count);
                    trackBar1.LargeChange = 1;
                    trackBar1.Value = 0;
                    trackBar1.TickFrequency = playback.Count / 100;
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
