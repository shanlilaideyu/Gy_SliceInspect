namespace alg_KeyBoard_BYD
{
    partial class algSetUC
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFAI = new System.Windows.Forms.ComboBox();
            this.lstXm = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSaveToFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmblLiaohao = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblprompt = new System.Windows.Forms.Label();
            this.btnCanel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGetImage = new System.Windows.Forms.Button();
            this.BtnDrawLine = new System.Windows.Forms.Button();
            this.BtnDrawRect2 = new System.Windows.Forms.Button();
            this.BtnDrawRect1 = new System.Windows.Forms.Button();
            this.picBox = new akControl.akHalWindow();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cmbVal3 = new System.Windows.Forms.ComboBox();
            this.cmbVal2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbVal1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "测量项选择:";
            // 
            // cmbFAI
            // 
            this.cmbFAI.FormattingEnabled = true;
            this.cmbFAI.Location = new System.Drawing.Point(74, 28);
            this.cmbFAI.Margin = new System.Windows.Forms.Padding(2);
            this.cmbFAI.Name = "cmbFAI";
            this.cmbFAI.Size = new System.Drawing.Size(166, 20);
            this.cmbFAI.TabIndex = 2;
            this.cmbFAI.SelectedIndexChanged += new System.EventHandler(this.cmbFAI_SelectedIndexChanged);
            // 
            // lstXm
            // 
            this.lstXm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstXm.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstXm.Font = new System.Drawing.Font("宋体", 13F);
            this.lstXm.FormattingEnabled = true;
            this.lstXm.ItemHeight = 20;
            this.lstXm.Location = new System.Drawing.Point(4, 51);
            this.lstXm.Margin = new System.Windows.Forms.Padding(2);
            this.lstXm.Name = "lstXm";
            this.lstXm.Size = new System.Drawing.Size(235, 339);
            this.lstXm.TabIndex = 4;
            this.lstXm.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstXm_DrawItem);
            this.lstXm.SelectedIndexChanged += new System.EventHandler(this.lstXm_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.btnSaveToFile);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lstXm);
            this.panel1.Controls.Add(this.cmblLiaohao);
            this.panel1.Controls.Add(this.cmbFAI);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(244, 562);
            this.panel1.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(5, 426);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(234, 103);
            this.textBox1.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 411);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "结果：";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(149, 534);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 26);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(31, 534);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(55, 26);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSaveToFile
            // 
            this.btnSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveToFile.Location = new System.Drawing.Point(90, 534);
            this.btnSaveToFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveToFile.Name = "btnSaveToFile";
            this.btnSaveToFile.Size = new System.Drawing.Size(55, 26);
            this.btnSaveToFile.TabIndex = 8;
            this.btnSaveToFile.Text = "保存";
            this.btnSaveToFile.UseVisualStyleBackColor = true;
            this.btnSaveToFile.Click += new System.EventHandler(this.btnSaveToFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "料号：";
            // 
            // cmblLiaohao
            // 
            this.cmblLiaohao.FormattingEnabled = true;
            this.cmblLiaohao.Location = new System.Drawing.Point(74, 4);
            this.cmblLiaohao.Margin = new System.Windows.Forms.Padding(2);
            this.cmblLiaohao.Name = "cmblLiaohao";
            this.cmblLiaohao.Size = new System.Drawing.Size(143, 20);
            this.cmblLiaohao.TabIndex = 2;
            this.cmblLiaohao.SelectedIndexChanged += new System.EventHandler(this.cmblLiaohao_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblprompt);
            this.panel4.Controls.Add(this.btnCanel);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.btnGetImage);
            this.panel4.Controls.Add(this.BtnDrawLine);
            this.panel4.Controls.Add(this.BtnDrawRect2);
            this.panel4.Controls.Add(this.BtnDrawRect1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(244, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(624, 40);
            this.panel4.TabIndex = 6;
            // 
            // lblprompt
            // 
            this.lblprompt.AutoSize = true;
            this.lblprompt.ForeColor = System.Drawing.Color.Red;
            this.lblprompt.Location = new System.Drawing.Point(4, 18);
            this.lblprompt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblprompt.Name = "lblprompt";
            this.lblprompt.Size = new System.Drawing.Size(167, 12);
            this.lblprompt.TabIndex = 1;
            this.lblprompt.Text = "请在图片框中绘制出标点1区域";
            // 
            // btnCanel
            // 
            this.btnCanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCanel.Location = new System.Drawing.Point(563, 10);
            this.btnCanel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCanel.Name = "btnCanel";
            this.btnCanel.Size = new System.Drawing.Size(55, 26);
            this.btnCanel.TabIndex = 5;
            this.btnCanel.Text = "取消";
            this.btnCanel.UseVisualStyleBackColor = true;
            this.btnCanel.Click += new System.EventHandler(this.btnCanel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(504, 11);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(55, 26);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetImage
            // 
            this.btnGetImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetImage.Location = new System.Drawing.Point(195, 11);
            this.btnGetImage.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetImage.Name = "btnGetImage";
            this.btnGetImage.Size = new System.Drawing.Size(64, 26);
            this.btnGetImage.TabIndex = 3;
            this.btnGetImage.Text = "打开图片";
            this.btnGetImage.UseVisualStyleBackColor = true;
            this.btnGetImage.Click += new System.EventHandler(this.btnGetImage_Click);
            // 
            // BtnDrawLine
            // 
            this.BtnDrawLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDrawLine.Enabled = false;
            this.BtnDrawLine.Location = new System.Drawing.Point(417, 10);
            this.BtnDrawLine.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDrawLine.Name = "BtnDrawLine";
            this.BtnDrawLine.Size = new System.Drawing.Size(73, 26);
            this.BtnDrawLine.TabIndex = 2;
            this.BtnDrawLine.Text = "绘制直线";
            this.BtnDrawLine.UseVisualStyleBackColor = true;
            this.BtnDrawLine.Click += new System.EventHandler(this.BtnDrawLine_Click);
            // 
            // BtnDrawRect2
            // 
            this.BtnDrawRect2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDrawRect2.Location = new System.Drawing.Point(340, 10);
            this.BtnDrawRect2.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDrawRect2.Name = "BtnDrawRect2";
            this.BtnDrawRect2.Size = new System.Drawing.Size(73, 26);
            this.BtnDrawRect2.TabIndex = 2;
            this.BtnDrawRect2.Text = "绘制矩形2";
            this.BtnDrawRect2.UseVisualStyleBackColor = true;
            this.BtnDrawRect2.Click += new System.EventHandler(this.BtnDrawRect2_Click);
            // 
            // BtnDrawRect1
            // 
            this.BtnDrawRect1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDrawRect1.Location = new System.Drawing.Point(263, 10);
            this.BtnDrawRect1.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDrawRect1.Name = "BtnDrawRect1";
            this.BtnDrawRect1.Size = new System.Drawing.Size(73, 26);
            this.BtnDrawRect1.TabIndex = 1;
            this.BtnDrawRect1.Text = "绘制矩形1";
            this.BtnDrawRect1.UseVisualStyleBackColor = true;
            this.BtnDrawRect1.Click += new System.EventHandler(this.BtnDrawRect1_Click);
            // 
            // picBox
            // 
            this.picBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBox.BackColor = System.Drawing.Color.Black;
            this.picBox.Location = new System.Drawing.Point(247, 104);
            this.picBox.Margin = new System.Windows.Forms.Padding(2);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(619, 456);
            this.picBox.TabIndex = 8;
            this.picBox.Load += new System.EventHandler(this.picBox_Load);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.cmbVal3);
            this.panel5.Controls.Add(this.cmbVal2);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.cmbVal1);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Location = new System.Drawing.Point(244, 41);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(622, 59);
            this.panel5.TabIndex = 8;
            // 
            // cmbVal3
            // 
            this.cmbVal3.FormattingEnabled = true;
            this.cmbVal3.Location = new System.Drawing.Point(392, 10);
            this.cmbVal3.Margin = new System.Windows.Forms.Padding(2);
            this.cmbVal3.Name = "cmbVal3";
            this.cmbVal3.Size = new System.Drawing.Size(53, 20);
            this.cmbVal3.TabIndex = 8;
            // 
            // cmbVal2
            // 
            this.cmbVal2.FormattingEnabled = true;
            this.cmbVal2.Location = new System.Drawing.Point(253, 10);
            this.cmbVal2.Margin = new System.Windows.Forms.Padding(2);
            this.cmbVal2.Name = "cmbVal2";
            this.cmbVal2.Size = new System.Drawing.Size(59, 20);
            this.cmbVal2.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(329, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "像素当量:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(192, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "匹配分值:";
            // 
            // cmbVal1
            // 
            this.cmbVal1.FormattingEnabled = true;
            this.cmbVal1.Location = new System.Drawing.Point(125, 10);
            this.cmbVal1.Margin = new System.Windows.Forms.Padding(2);
            this.cmbVal1.Name = "cmbVal1";
            this.cmbVal1.Size = new System.Drawing.Size(66, 20);
            this.cmbVal1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "对比度值(设0不启用):";
            // 
            // algSetUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.picBox);
            this.Name = "algSetUC";
            this.Size = new System.Drawing.Size(868, 562);
            this.Load += new System.EventHandler(this.algSetUC_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstXm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveToFile;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblprompt;
        private System.Windows.Forms.Button btnCanel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGetImage;
        private System.Windows.Forms.Button BtnDrawRect2;
        private System.Windows.Forms.Button BtnDrawRect1;
        private System.Windows.Forms.Label label3;
        public akControl.akHalWindow picBox;
        private System.Windows.Forms.Button BtnDrawLine;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox cmbVal3;
        private System.Windows.Forms.ComboBox cmbVal2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbVal1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.ComboBox cmbFAI;
        public System.Windows.Forms.ComboBox cmblLiaohao;
    }
}
