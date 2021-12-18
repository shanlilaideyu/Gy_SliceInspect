using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Newtonsoft.Json;
using DevComponents.AdvTree;
using System.IO;

namespace SZ_BydKeyboard
{
    public partial class FrmDataSum : DockContent
    {
        public FrmDataSum()
        {
            InitializeComponent();
        }

        public void ShowTime(string time)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Common.ShowMsg(ShowTime), time);
                return;
            }
            labelX1.Text = $"检测时间：{time}";
        }


        private void buttonX1_Click(object sender, EventArgs e)
        {
            Common.ProductSum = 0;
            Common.ProductNg = 0;

            ResetNG();

            UpdateSum(Common.ProductSum, Common.ProductNg);
            IniHelper.Write("RuningData", "Sum", Common.ProductSum.ToString(), $"{Application.StartupPath}\\SysCfg\\System.ini");
            IniHelper.Write("RuningData", "Ng", Common.ProductNg.ToString(), $"{Application.StartupPath}\\SysCfg\\System.ini");
        }

        private void ResetNG()
        {
            for(int i=0;i<Common.Data.NgCount.Length;i++)
            {
                Common.Data.NgCount[i] = 0;
            }
            string fp = $"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\RunningData.json";
            if (File.Exists(fp))
            {
                string content = JsonConvert.SerializeObject(Common.Data,Formatting.Indented);
                File.WriteAllText(fp, content);
            }
        }

        delegate void DelegateUpdateSum(int Sum, int NG);

        List<string> fais = new List<string>();
        List<int> counts = new List<int>();
        public void UpdateSum(int Sum, int NG)
        {
            fais.Clear();
            counts.Clear();
            advTree2.Nodes.Clear();
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateUpdateSum(UpdateSum), Sum, NG);
                return;
            }
            labelX2.Text = $"检测数量：{Common.ProductSum}";
            labelX3.Text = $"NG数量：{Common.ProductNg}";
            if (Sum == 0)
            {
                Common.Data.NgCount = new int[Common.FaiNames.Length];
                labelX4.Text = $"良率：0.00%";
            }
            else
            {
                labelX4.Text = $"良率：{((Sum - NG) * 1.00 / Sum * 100.00).ToString("0.00")}%";
            }
            //NG项排名
            if (Sum != 0)
            {
                int[] SortCount = new int[Common.Data.NgCount.Length];
                Array.Copy(Common.Data.NgCount, SortCount, SortCount.Length);
                Array.Sort(SortCount);
                
                for (int i = SortCount.Length - 1; i > SortCount.Length - 6; i--)
                {
                    int fai;
                    if (i < SortCount.Length - 1 ? SortCount[i] == SortCount[i + 1] : false)
                    {
                        int fai1 = Array.IndexOf(Common.Data.NgCount, SortCount[i + 1]);
                        fai = Array.IndexOf(Common.Data.NgCount, SortCount[i], fai1 +1);
                    }
                    else
                    {
                        fai = Array.IndexOf(Common.Data.NgCount, SortCount[i]);
                    }
                    if (Common.Data.NgCount[fai] != 0)
                    {
                        fais.Add(Common.FaiNames[fai]);
                        counts.Add(Common.Data.NgCount[fai]);
                        Node node = new Node(Common.FaiNames[fai]);
                        node.Cells.Add(new Cell(Common.Data.NgCount[fai].ToString()));
                        node.Cells.Add(new Cell($"{(Common.Data.NgCount[fai] * 1.00 / Sum * 100.00).ToString("0.00")}%"));
                        advTree2.Nodes.Add(node);
                    }
                }
            }
            this.chartNG.Series[0].Points.DataBindXY(fais, counts);
        }

        private void FrmDataSum_Load(object sender, EventArgs e)
        {
            Common.ShowInspectTime = ShowTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateSum(Common.ProductSum, Common.ProductNg);
        }
    }
}
