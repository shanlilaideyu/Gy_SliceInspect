namespace LeoBase
{
    partial class DisplayBase
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayBase));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslCoord = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslGray = new System.Windows.Forms.ToolStripStatusLabel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslSize,
            this.tsslCoord,
            this.tsslGray});
            this.statusStrip1.Location = new System.Drawing.Point(0, 376);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(503, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslSize
            // 
            this.tsslSize.Image = ((System.Drawing.Image)(resources.GetObject("tsslSize.Image")));
            this.tsslSize.Name = "tsslSize";
            this.tsslSize.Size = new System.Drawing.Size(43, 17);
            this.tsslSize.Text = "0*0";
            // 
            // tsslCoord
            // 
            this.tsslCoord.Image = ((System.Drawing.Image)(resources.GetObject("tsslCoord.Image")));
            this.tsslCoord.Name = "tsslCoord";
            this.tsslCoord.Size = new System.Drawing.Size(41, 17);
            this.tsslCoord.Text = "0,0";
            // 
            // tsslGray
            // 
            this.tsslGray.Image = ((System.Drawing.Image)(resources.GetObject("tsslGray.Image")));
            this.tsslGray.Name = "tsslGray";
            this.tsslGray.Size = new System.Drawing.Size(31, 17);
            this.tsslGray.Text = "0";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(503, 376);
            this.hWindowControl1.TabIndex = 5;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(503, 376);
            this.hWindowControl1.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseMove);
            this.hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseDown);
            this.hWindowControl1.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseWheel);
            // 
            // DisplayBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hWindowControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "DisplayBase";
            this.Size = new System.Drawing.Size(503, 398);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel tsslGray;
        private System.Windows.Forms.ToolStripStatusLabel tsslCoord;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslSize;
        private HalconDotNet.HWindowControl hWindowControl1;
    }
}
