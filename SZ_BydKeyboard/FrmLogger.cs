using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using WeifenLuo.WinFormsUI.Docking;
using LogRecord;
using System.Threading;

namespace SZ_BydKeyboard
{
    public partial class FrmLogger : DockContent
    {
        private int rowCount;

        public FrmLogger()
        {
            InitializeComponent();
            Common.ShowMsgEvent = JoinMsg;
        }

        public void JoinMsg(string str)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowMsg), str);
        }

        public void ShowMsg(object o)
        {
            string str = o as string;
            if (this.InvokeRequired)
            {
                this.Invoke(new Common.ShowMsg(ShowMsg), str);
                return;
            }
            Int32 TextLength = LogBox.TextLength;
            Color FontColor;
            if (str.Length > 0)
            {
                rowCount++;
                if (rowCount > 10000)
                {
                    rowCount = 0;
                    LogBox.Text = "";
                }
                string ItemType = str.Substring(0, str.IndexOf(":"));
                switch (ItemType)
                {
                    case "Tips":
                        FontColor = Color.Blue;
                        LogHelper.Debug(this.GetType(), str);
                        break;
                    case "Left":
                        FontColor = Color.Black;
                        break;
                    case "Error":
                        FontColor = Color.Red;
                        break;
                    case "Warming":
                        FontColor = Color.OrangeRed;
                        LogHelper.Error(this.GetType(), str);
                        break;
                    case "Right":
                        FontColor = Color.LimeGreen;
                        LogHelper.Info(this.GetType(), str);
                        break;
                    case "Unusual":
                        FontColor = Color.Orange;
                        break;
                    default: FontColor = Color.Black; break;
                }
                LogBox.AppendText(str + "\r\n");
                LogBox.Select(TextLength, LogBox.TextLength);
                LogBox.SelectionColor = FontColor;
                LogBox.ScrollToCaret();
            }
        }
    }
}