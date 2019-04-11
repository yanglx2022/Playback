using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace PlaybackTool
{
    /// <summary>
    /// 设置组播
    /// </summary>
    public partial class NetForm : Form
    {
        public string Ip = "";
        public int Port = 0;

        public NetForm(string ip, int port)
        {
            InitializeComponent();
            Ip = ip;
            Port = port;
            txtIP.Text = ip;
            txtPort.Text = port.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(txtIP.Text, out IPAddress iPAddress) && 
                int.TryParse(txtPort.Text, out Port))
            {
                Ip = txtIP.Text;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
