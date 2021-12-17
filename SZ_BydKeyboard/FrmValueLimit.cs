using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Linq;

namespace SZ_BydKeyboard
{
    public partial class FrmValueLimit : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FrmValueLimit()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                numericUpDown2.Value = (decimal)Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf(comboBox1.SelectedItem.ToString())];
                numericUpDown1.Value = (decimal)Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf(comboBox1.SelectedItem.ToString())];
              
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value<numericUpDown2.Value)
            {
                Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf(comboBox1.SelectedItem.ToString())] = (double)numericUpDown1.Value;
            }
            else
            {
                numericUpDown1.Value = (decimal)Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf(comboBox1.SelectedItem.ToString())];
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < numericUpDown2.Value)
            {
                Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf(comboBox1.SelectedItem.ToString())] = (double)numericUpDown2.Value;
            }
            else
            {
                numericUpDown2.Value = (decimal)Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf(comboBox1.SelectedItem.ToString())];
            }
        }

        private void FrmValueLimit_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}