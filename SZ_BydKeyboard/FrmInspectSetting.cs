using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using alg_KeyBoard_BYD;

namespace SZ_BydKeyboard
{
    public partial class FrmInspectSetting : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FrmInspectSetting()
        {
            InitializeComponent();
        }

        private void FrmInspectSetting_Load(object sender, EventArgs e)
        {
            algSetUC1.picBox.init();
        }

        public void LoadInspectCode()
        {
            algSetUC1.LiaoHao = Common.str_ProductName;
            Common.InspectCode = new alg_KeyBoard_BYD.DoCode();
            Common.InspectCode.initVisionParams();
            algSetUC1.objDocode = Common.InspectCode;
            //Common.InspectCode = algSetUC1.objDocode;
        }

        private void FrmInspectSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadInspectCode();
        }
    }
}