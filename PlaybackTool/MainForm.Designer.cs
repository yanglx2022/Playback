namespace PlaybackTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.lbCnt = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.cbLoop = new System.Windows.Forms.CheckBox();
            this.cbRepeat = new System.Windows.Forms.CheckBox();
            this.cbSend = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnPause = new System.Windows.Forms.Button();
            this.btnNet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(12, 12);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(64, 25);
            this.btnRecord.TabIndex = 0;
            this.btnRecord.Text = "录制";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 52);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(64, 25);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "回放";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lbCnt
            // 
            this.lbCnt.AutoSize = true;
            this.lbCnt.Location = new System.Drawing.Point(95, 20);
            this.lbCnt.Name = "lbCnt";
            this.lbCnt.Size = new System.Drawing.Size(23, 12);
            this.lbCnt.TabIndex = 2;
            this.lbCnt.Text = "cnt";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(151, 15);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(269, 20);
            this.progressBar1.TabIndex = 3;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(141, 59);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(286, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // cbLoop
            // 
            this.cbLoop.AutoSize = true;
            this.cbLoop.Location = new System.Drawing.Point(93, 60);
            this.cbLoop.Name = "cbLoop";
            this.cbLoop.Size = new System.Drawing.Size(48, 16);
            this.cbLoop.TabIndex = 5;
            this.cbLoop.Text = "循环";
            this.cbLoop.UseVisualStyleBackColor = true;
            this.cbLoop.CheckedChanged += new System.EventHandler(this.cbLoop_CheckedChanged);
            // 
            // cbRepeat
            // 
            this.cbRepeat.AutoSize = true;
            this.cbRepeat.Location = new System.Drawing.Point(93, 101);
            this.cbRepeat.Name = "cbRepeat";
            this.cbRepeat.Size = new System.Drawing.Size(48, 16);
            this.cbRepeat.TabIndex = 6;
            this.cbRepeat.Text = "重复";
            this.cbRepeat.UseVisualStyleBackColor = true;
            // 
            // cbSend
            // 
            this.cbSend.AutoSize = true;
            this.cbSend.Location = new System.Drawing.Point(371, 101);
            this.cbSend.Name = "cbSend";
            this.cbSend.Size = new System.Drawing.Size(72, 16);
            this.cbSend.TabIndex = 7;
            this.cbSend.Text = "发送绘图";
            this.cbSend.UseVisualStyleBackColor = true;
            this.cbSend.CheckedChanged += new System.EventHandler(this.cbSend_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(12, 92);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(63, 25);
            this.btnPause.TabIndex = 8;
            this.btnPause.Text = "暂停";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnNet
            // 
            this.btnNet.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNet.Location = new System.Drawing.Point(425, 5);
            this.btnNet.Name = "btnNet";
            this.btnNet.Size = new System.Drawing.Size(23, 23);
            this.btnNet.TabIndex = 9;
            this.btnNet.Text = "设置网络";
            this.btnNet.UseVisualStyleBackColor = true;
            this.btnNet.Click += new System.EventHandler(this.btnNet_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 128);
            this.Controls.Add(this.btnNet);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.cbSend);
            this.Controls.Add(this.cbRepeat);
            this.Controls.Add(this.cbLoop);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbCnt);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnRecord);
            this.Name = "MainForm";
            this.Text = "数据回放工具";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label lbCnt;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox cbLoop;
        private System.Windows.Forms.CheckBox cbRepeat;
        private System.Windows.Forms.CheckBox cbSend;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnNet;
    }
}

