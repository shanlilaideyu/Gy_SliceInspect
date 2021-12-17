namespace LeoBase
{
    partial class DrawingBase
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingBase));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslCoord = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslGray = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.RoiOperationCbox = new System.Windows.Forms.ToolStripComboBox();
            this.Btn_DrawRectangle1 = new System.Windows.Forms.ToolStripButton();
            this.Btn_DrawRectangle2 = new System.Windows.Forms.ToolStripButton();
            this.Btn_DrawCircle = new System.Windows.Forms.ToolStripButton();
            this.Btn_DrawEllipse = new System.Windows.Forms.ToolStripButton();
            this.Btn_DrawRegion = new System.Windows.Forms.ToolStripButton();
            this.Btn_DrawLine = new System.Windows.Forms.ToolStripButton();
            this.Btn_DrawPoint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.DrawTimer = new System.Windows.Forms.Timer(this.components);
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.statusStrip1.SuspendLayout();
            this.ToolStrip.SuspendLayout();
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
            // ToolStrip
            // 
            this.ToolStrip.BackColor = System.Drawing.Color.White;
            this.ToolStrip.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RoiOperationCbox,
            this.Btn_DrawRectangle1,
            this.Btn_DrawRectangle2,
            this.Btn_DrawCircle,
            this.Btn_DrawEllipse,
            this.Btn_DrawRegion,
            this.Btn_DrawLine,
            this.Btn_DrawPoint,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripButton2});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(503, 25);
            this.ToolStrip.TabIndex = 6;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // RoiOperationCbox
            // 
            this.RoiOperationCbox.Items.AddRange(new object[] {
            "新建",
            "合并",
            "差集",
            "交集",
            "对称差"});
            this.RoiOperationCbox.Name = "RoiOperationCbox";
            this.RoiOperationCbox.Size = new System.Drawing.Size(75, 25);
            this.RoiOperationCbox.Text = "新建";
            this.RoiOperationCbox.SelectedIndexChanged += new System.EventHandler(this.RoiOperation_SelectedIndexChanged);
            // 
            // Btn_DrawRectangle1
            // 
            this.Btn_DrawRectangle1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_DrawRectangle1.Image = global::LeoBase.Properties.Resources._7;
            this.Btn_DrawRectangle1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_DrawRectangle1.Name = "Btn_DrawRectangle1";
            this.Btn_DrawRectangle1.Size = new System.Drawing.Size(23, 22);
            this.Btn_DrawRectangle1.Text = "轴平行矩形";
            this.Btn_DrawRectangle1.Click += new System.EventHandler(this.Btn_Draw_Click);
            // 
            // Btn_DrawRectangle2
            // 
            this.Btn_DrawRectangle2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_DrawRectangle2.Image = global::LeoBase.Properties.Resources._5;
            this.Btn_DrawRectangle2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_DrawRectangle2.Name = "Btn_DrawRectangle2";
            this.Btn_DrawRectangle2.Size = new System.Drawing.Size(23, 22);
            this.Btn_DrawRectangle2.Text = "仿射矩形";
            this.Btn_DrawRectangle2.Click += new System.EventHandler(this.Btn_Draw_Click);
            // 
            // Btn_DrawCircle
            // 
            this.Btn_DrawCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_DrawCircle.Image = global::LeoBase.Properties.Resources._8;
            this.Btn_DrawCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_DrawCircle.Name = "Btn_DrawCircle";
            this.Btn_DrawCircle.Size = new System.Drawing.Size(23, 22);
            this.Btn_DrawCircle.Text = "圆";
            this.Btn_DrawCircle.Click += new System.EventHandler(this.Btn_Draw_Click);
            // 
            // Btn_DrawEllipse
            // 
            this.Btn_DrawEllipse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_DrawEllipse.Image = global::LeoBase.Properties.Resources._1;
            this.Btn_DrawEllipse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_DrawEllipse.Name = "Btn_DrawEllipse";
            this.Btn_DrawEllipse.Size = new System.Drawing.Size(23, 22);
            this.Btn_DrawEllipse.Text = "椭圆";
            this.Btn_DrawEllipse.Click += new System.EventHandler(this.Btn_Draw_Click);
            // 
            // Btn_DrawRegion
            // 
            this.Btn_DrawRegion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_DrawRegion.Image = global::LeoBase.Properties.Resources._4;
            this.Btn_DrawRegion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_DrawRegion.Name = "Btn_DrawRegion";
            this.Btn_DrawRegion.Size = new System.Drawing.Size(23, 22);
            this.Btn_DrawRegion.Text = "任意区域";
            this.Btn_DrawRegion.Click += new System.EventHandler(this.Btn_Draw_Click);
            // 
            // Btn_DrawLine
            // 
            this.Btn_DrawLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_DrawLine.Image = global::LeoBase.Properties.Resources._9;
            this.Btn_DrawLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_DrawLine.Name = "Btn_DrawLine";
            this.Btn_DrawLine.Size = new System.Drawing.Size(23, 22);
            this.Btn_DrawLine.Text = "直线";
            this.Btn_DrawLine.Click += new System.EventHandler(this.Btn_Draw_Click);
            // 
            // Btn_DrawPoint
            // 
            this.Btn_DrawPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Btn_DrawPoint.Image = global::LeoBase.Properties.Resources._0;
            this.Btn_DrawPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Btn_DrawPoint.Name = "Btn_DrawPoint";
            this.Btn_DrawPoint.Size = new System.Drawing.Size(23, 22);
            this.Btn_DrawPoint.Text = "点";
            this.Btn_DrawPoint.Click += new System.EventHandler(this.Btn_Draw_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "拟合圆";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "拟合直线";
            // 
            // DrawTimer
            // 
            this.DrawTimer.Interval = 10;
            this.DrawTimer.Tick += new System.EventHandler(this.DrawTimer_Tick);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 25);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(503, 351);
            this.hWindowControl1.TabIndex = 7;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(503, 351);
            this.hWindowControl1.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseMove);
            this.hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseDown);
            this.hWindowControl1.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseUp);
            this.hWindowControl1.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseWheel);
            // 
            // DrawingBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hWindowControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ToolStrip);
            this.Name = "DrawingBase";
            this.Size = new System.Drawing.Size(503, 398);
            this.Load += new System.EventHandler(this.DrawingBase_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel tsslGray;
        private System.Windows.Forms.ToolStripStatusLabel tsslCoord;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslSize;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripComboBox RoiOperationCbox;
        private System.Windows.Forms.ToolStripButton Btn_DrawRectangle1;
        private System.Windows.Forms.ToolStripButton Btn_DrawRectangle2;
        private System.Windows.Forms.ToolStripButton Btn_DrawCircle;
        private System.Windows.Forms.ToolStripButton Btn_DrawEllipse;
        private System.Windows.Forms.ToolStripButton Btn_DrawRegion;
        private System.Windows.Forms.ToolStripButton Btn_DrawLine;
        private System.Windows.Forms.ToolStripButton Btn_DrawPoint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Timer DrawTimer;
        private HalconDotNet.HWindowControl hWindowControl1;
    }
}
