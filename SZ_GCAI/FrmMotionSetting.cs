using DevComponents.DotNetBar.Metro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeoMotion;

namespace SZ_BydKeyboard
{
    public partial class FrmMotionSetting : MetroForm
    {
        public FrmMotionSetting()
        {
            InitializeComponent();
        }

        private void FrmMotionSetting_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadMotionData()
        {
            cardControl1._card = Common.ProC.Card1;
            cardControl1.BindingCard();
        }

        private void FrmMotionSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            cardControl1.Timer.Enabled = false;
            this.Hide();
            e.Cancel = true;
        }
    }
}
