namespace LeoMotion
{
    partial class CardControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardControl));
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.AxisIndex = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.AxisName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.SoftLmtEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SoftLmtN = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.SoftLmtP = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.Trans = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.StartSpeed = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.Speed = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.ASpeed = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.SmoothTime = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.advTree2 = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.ImgList = new System.Windows.Forms.ImageList(this.components);
            this.Timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowDrop = true;
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewX1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AxisIndex,
            this.AxisName,
            this.SoftLmtEnable,
            this.SoftLmtN,
            this.SoftLmtP,
            this.Trans,
            this.StartSpeed,
            this.Speed,
            this.ASpeed,
            this.SmoothTime});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.EnableHeadersVisualStyles = false;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(406, 5);
            this.dataGridViewX1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.dataGridViewX1.Name = "dataGridViewX1";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewX1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.tableLayoutPanel1.SetRowSpan(this.dataGridViewX1, 2);
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.Size = new System.Drawing.Size(1149, 650);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellValueChanged);
            this.dataGridViewX1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewX1_CurrentCellDirtyStateChanged);
            // 
            // AxisIndex
            // 
            this.AxisIndex.HeaderText = "序号";
            this.AxisIndex.Name = "AxisIndex";
            this.AxisIndex.ReadOnly = true;
            this.AxisIndex.Width = 40;
            // 
            // AxisName
            // 
            this.AxisName.HeaderText = "轴名称";
            this.AxisName.Name = "AxisName";
            this.AxisName.ReadOnly = true;
            this.AxisName.Width = 160;
            // 
            // SoftLmtEnable
            // 
            this.SoftLmtEnable.HeaderText = "限位启用";
            this.SoftLmtEnable.Name = "SoftLmtEnable";
            this.SoftLmtEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // SoftLmtN
            // 
            // 
            // 
            // 
            this.SoftLmtN.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.SoftLmtN.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.SoftLmtN.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.SoftLmtN.BackgroundStyle.TextColor = System.Drawing.Color.Black;
            this.SoftLmtN.HeaderText = "软限位-";
            this.SoftLmtN.Increment = 1D;
            this.SoftLmtN.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.SoftLmtN.Name = "SoftLmtN";
            this.SoftLmtN.Width = 120;
            // 
            // SoftLmtP
            // 
            // 
            // 
            // 
            this.SoftLmtP.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.SoftLmtP.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.SoftLmtP.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.SoftLmtP.BackgroundStyle.TextColor = System.Drawing.Color.Black;
            this.SoftLmtP.HeaderText = "软限位+";
            this.SoftLmtP.Increment = 1D;
            this.SoftLmtP.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.SoftLmtP.Name = "SoftLmtP";
            this.SoftLmtP.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SoftLmtP.Width = 120;
            // 
            // Trans
            // 
            // 
            // 
            // 
            this.Trans.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.Trans.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Trans.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Trans.BackgroundStyle.TextColor = System.Drawing.Color.Black;
            this.Trans.HeaderText = "转换比例";
            this.Trans.Increment = 1D;
            this.Trans.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.Trans.Name = "Trans";
            this.Trans.Width = 120;
            // 
            // StartSpeed
            // 
            // 
            // 
            // 
            this.StartSpeed.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.StartSpeed.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.StartSpeed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.StartSpeed.BackgroundStyle.TextColor = System.Drawing.Color.Black;
            this.StartSpeed.HeaderText = "初始速度";
            this.StartSpeed.Increment = 1D;
            this.StartSpeed.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.StartSpeed.Name = "StartSpeed";
            this.StartSpeed.Width = 120;
            // 
            // Speed
            // 
            // 
            // 
            // 
            this.Speed.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.Speed.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Speed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Speed.BackgroundStyle.TextColor = System.Drawing.Color.Black;
            this.Speed.HeaderText = "速度";
            this.Speed.Increment = 1D;
            this.Speed.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.Speed.Name = "Speed";
            // 
            // ASpeed
            // 
            // 
            // 
            // 
            this.ASpeed.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.ASpeed.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.ASpeed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ASpeed.BackgroundStyle.TextColor = System.Drawing.Color.Black;
            this.ASpeed.HeaderText = "加速度";
            this.ASpeed.Increment = 1D;
            this.ASpeed.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.ASpeed.Name = "ASpeed";
            // 
            // SmoothTime
            // 
            // 
            // 
            // 
            this.SmoothTime.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.SmoothTime.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.SmoothTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.SmoothTime.BackgroundStyle.TextColor = System.Drawing.Color.Black;
            this.SmoothTime.HeaderText = "平滑时间";
            this.SmoothTime.Increment = 1D;
            this.SmoothTime.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.SmoothTime.Name = "SmoothTime";
            this.SmoothTime.Width = 120;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.advTree1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.advTree2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewX1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1552, 660);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree1.ColumnsVisible = false;
            this.advTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTree1.Enabled = false;
            this.advTree1.GridColumnLines = false;
            this.advTree1.ImageList = this.ImgList;
            this.advTree1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.advTree1.Location = new System.Drawing.Point(3, 3);
            this.advTree1.Name = "advTree1";
            this.advTree1.NodesConnector = this.nodeConnector1;
            this.advTree1.NodeStyle = this.elementStyle1;
            this.advTree1.PathSeparator = ";";
            this.tableLayoutPanel1.SetRowSpan(this.advTree1, 2);
            this.advTree1.SelectionBox = false;
            this.advTree1.Size = new System.Drawing.Size(194, 654);
            this.advTree1.Styles.Add(this.elementStyle1);
            this.advTree1.TabIndex = 3;
            this.advTree1.Text = "advTree1";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // advTree2
            // 
            this.advTree2.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree2.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree2.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTree2.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.advTree2.Location = new System.Drawing.Point(203, 3);
            this.advTree2.Name = "advTree2";
            this.advTree2.NodesConnector = this.nodeConnector2;
            this.advTree2.NodeStyle = this.elementStyle2;
            this.advTree2.PathSeparator = ";";
            this.tableLayoutPanel1.SetRowSpan(this.advTree2, 2);
            this.advTree2.Size = new System.Drawing.Size(194, 654);
            this.advTree2.Styles.Add(this.elementStyle2);
            this.advTree2.TabIndex = 2;
            this.advTree2.Text = "advTree2";
            this.advTree2.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree2_NodeClick);
            // 
            // nodeConnector2
            // 
            this.nodeConnector2.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle2
            // 
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // ImgList
            // 
            this.ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImgList.ImageStream")));
            this.ImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImgList.Images.SetKeyName(0, "关灯 (1).png");
            this.ImgList.Images.SetKeyName(1, "开灯 (1).png");
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // CardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "CardControl";
            this.Size = new System.Drawing.Size(1552, 660);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn AxisIndex;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn AxisName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SoftLmtEnable;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn SoftLmtN;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn SoftLmtP;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Trans;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn StartSpeed;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Speed;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn ASpeed;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn SmoothTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.AdvTree.AdvTree advTree2;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private System.Windows.Forms.ImageList ImgList;
        public System.Windows.Forms.Timer Timer;
    }
}
