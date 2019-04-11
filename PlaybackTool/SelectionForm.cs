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
    public partial class SelectionForm : Form
    {
        /// <summary>
        /// 选中的格式
        /// </summary>
        public Format Format { get; private set; }
        
        public SelectionForm()
        {
            InitializeComponent();

            List<Format> formats = FormatConfig.LoadFromFile();
            if (formats.Count == 0)
            {
                Label label = new Label();
                label.Text = "未找到有效配置，请打开一个配置文件或点击确定退出程序";
                label.AutoSize = true;
                label.ForeColor = Color.Red;
                flowLayoutPanel2.Controls.Add(label);
            }
            else if (formats.Count == 1)
            {
                Format = formats[0];
                this.Close();
            }
            else
            {
                foreach(Format item in formats)
                {
                    RadioButton radio = new RadioButton();
                    radio.Text = item.Name + "(" + item.Description + ")";
                    radio.Tag = item;
                    radio.AutoSize = true;
                    flowLayoutPanel2.Controls.Add(radio);
                }
                ((RadioButton)flowLayoutPanel2.Controls[0]).Checked = true;
                Format = formats[0];
            }
        }

        // 确认
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel2.Controls.Count > 1)
            {
                foreach(var item in flowLayoutPanel2.Controls)
                {
                    if (((RadioButton)item).Checked)
                    {
                        Format = (Format)((RadioButton)item).Tag;
                        break;
                    }
                }
            }
            DialogResult = DialogResult.OK;
        }

        // 打开文件
        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "数据格式文件|*.xml|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                List<Format> formats = FormatConfig.LoadFromFile(filename);
                flowLayoutPanel2.Controls.Clear();
                if (formats.Count == 0)
                {
                    Label label = new Label();
                    label.Text = "无效的配置文件,请打开正确的配置文件或点击确定退出程序";
                    label.AutoSize = true;
                    label.ForeColor = Color.Red;
                    flowLayoutPanel2.Controls.Add(label);
                    Format = null;
                }
                else
                {
                    foreach (Format item in formats)
                    {
                        RadioButton radio = new RadioButton();
                        radio.Text = item.Name + "(" + item.Description + ")";
                        radio.Tag = item;
                        radio.AutoSize = true;
                        flowLayoutPanel2.Controls.Add(radio);
                    }
                    ((RadioButton)flowLayoutPanel2.Controls[0]).Checked = true;
                    Format = formats[0];
                }
            }
        }
    }
}
