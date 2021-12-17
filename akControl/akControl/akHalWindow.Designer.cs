namespace akControl
{
    partial class akHalWindow
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbox = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmAdpatSize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDelGraphic = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDelAllGraphic = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDelObj = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbox
            // 
            this.pbox.ContextMenuStrip = this.contextMenuStrip1;
            this.pbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbox.Location = new System.Drawing.Point(0, 0);
            this.pbox.Name = "pbox";
            this.pbox.Size = new System.Drawing.Size(433, 314);
            this.pbox.TabIndex = 0;
            this.pbox.TabStop = false;
            this.pbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbox_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAdpatSize,
            this.tsmDelGraphic,
            this.tsmDelAllGraphic,
            this.tsmDelObj});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 92);
            // 
            // tsmAdpatSize
            // 
            this.tsmAdpatSize.Name = "tsmAdpatSize";
            this.tsmAdpatSize.Size = new System.Drawing.Size(148, 22);
            this.tsmAdpatSize.Text = "自适应大小";
            this.tsmAdpatSize.Click += new System.EventHandler(this.tsmAdpatSize_Click);
            // 
            // tsmDelGraphic
            // 
            this.tsmDelGraphic.Name = "tsmDelGraphic";
            this.tsmDelGraphic.Size = new System.Drawing.Size(148, 22);
            this.tsmDelGraphic.Text = "删除(&D)";
            this.tsmDelGraphic.Click += new System.EventHandler(this.tsmDelGraphic_Click);
            // 
            // tsmDelAllGraphic
            // 
            this.tsmDelAllGraphic.Name = "tsmDelAllGraphic";
            this.tsmDelAllGraphic.Size = new System.Drawing.Size(148, 22);
            this.tsmDelAllGraphic.Text = "清除所有图形";
            this.tsmDelAllGraphic.Click += new System.EventHandler(this.tsmDelAllGraphic_Click);
            // 
            // tsmDelObj
            // 
            this.tsmDelObj.Name = "tsmDelObj";
            this.tsmDelObj.Size = new System.Drawing.Size(148, 22);
            this.tsmDelObj.Text = "清除所有对象";
            this.tsmDelObj.Click += new System.EventHandler(this.tsmDelObj_Click);
            // 
            // akHalWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbox);
            this.Name = "akHalWindow";
            this.Size = new System.Drawing.Size(433, 314);
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmAdpatSize;
        private System.Windows.Forms.ToolStripMenuItem tsmDelGraphic;
        private System.Windows.Forms.ToolStripMenuItem tsmDelAllGraphic;
        private System.Windows.Forms.ToolStripMenuItem tsmDelObj;
    }
}
