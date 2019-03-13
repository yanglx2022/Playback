namespace PlaybackConsole
{
    partial class Form1
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
            this.btnPlay = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.cbLoop = new System.Windows.Forms.CheckBox();
            this.cbRepeat = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(4, 13);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(112, 73);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "回放";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(145, 17);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(382, 69);
            this.trackBar1.TabIndex = 1;
            // 
            // cbLoop
            // 
            this.cbLoop.AutoSize = true;
            this.cbLoop.Location = new System.Drawing.Point(160, 79);
            this.cbLoop.Name = "cbLoop";
            this.cbLoop.Size = new System.Drawing.Size(70, 22);
            this.cbLoop.TabIndex = 2;
            this.cbLoop.Text = "循环";
            this.cbLoop.UseVisualStyleBackColor = true;
            // 
            // cbRepeat
            // 
            this.cbRepeat.AutoSize = true;
            this.cbRepeat.Location = new System.Drawing.Point(259, 79);
            this.cbRepeat.Name = "cbRepeat";
            this.cbRepeat.Size = new System.Drawing.Size(70, 22);
            this.cbRepeat.TabIndex = 3;
            this.cbRepeat.Text = "重复";
            this.cbRepeat.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 146);
            this.Controls.Add(this.cbRepeat);
            this.Controls.Add(this.cbLoop);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.btnPlay);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox cbLoop;
        private System.Windows.Forms.CheckBox cbRepeat;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

