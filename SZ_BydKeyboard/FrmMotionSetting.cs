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
            exIOControl1._card = Common.ProC.ExCard;
            exIOControl1.BindingExCard();
        }

        private void FrmMotionSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            cardControl1.Timer.Enabled = false;
            exIOControl1.Timer.Enabled = false;
            this.Hide();
            e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool[] MpgInput= Common.ProC.Card1.GetState(GoogolTech.StateType.MPG);
            switchButton1.Value = MpgInput[0];
            switchButton2.Value = MpgInput[1];
            switchButton3.Value = MpgInput[2];
            switchButton4.Value = MpgInput[3];
            switchButton5.Value = MpgInput[4];
            switchButton6.Value = MpgInput[5];
        }
    }
}
