namespace SZ_BydKeyboard
{
    partial class FrmInspectSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.algSetUC1 = new alg_KeyBoard_BYD.algSetUC();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // algSetUC1
            // 
            this.algSetUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.algSetUC1.ForeColor = System.Drawing.Color.Black;
            this.algSetUC1.LiaoHao = null;
            this.algSetUC1.Location = new System.Drawing.Point(12, 12);
            this.algSetUC1.Name = "algSetUC1";
            this.algSetUC1.Size = new System.Drawing.Size(1512, 919);
            this.algSetUC1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("ø¨ÃÂ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1418, 937);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "À¢–¬";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmInspectSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1536, 1011);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.algSetUC1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmInspectSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ºÏ≤‚…Ë÷√";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInspectSetting_FormClosing);
            this.Load += new System.EventHandler(this.FrmInspectSetting_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private alg_KeyBoard_BYD.algSetUC algSetUC1;
        private System.Windows.Forms.Button button1;
    }
}