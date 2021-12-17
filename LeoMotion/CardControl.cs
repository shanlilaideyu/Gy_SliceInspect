using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.AdvTree;
using System.Threading;

namespace LeoMotion
{
    public partial class CardControl : UserControl
    {
        public CardControl()
        {
            InitializeComponent();
        }

        public Card _card;
        public bool BindingCard()
        {
            try
            {
                //轴设置
                for (int i = 0; i < _card.AxisName.Length; i++)
                {
                    dataGridViewX1.Rows.Add(i, _card.AxisName[i], this._card.SoftLmtEnable[i], _card.NSoftLmt[i], _card.PSoftLmt[i], _card.PulsePerMM[i], _card.StartVList[i],
                        _card.SpeedList[i], _card.Aspeed[i], _card.TaccList[i]);
                    if (i % 2 == 1)
                    {
                        dataGridViewX1.Rows[dataGridViewX1.Rows.Count - 1].Height = 36;
                        dataGridViewX1.Rows[dataGridViewX1.Rows.Count - 1].DefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.Menu, Font = new Font("楷体", 22) };
                    }
                    else
                    {
                        dataGridViewX1.Rows[dataGridViewX1.Rows.Count - 1].Height = 36;
                        dataGridViewX1.Rows[dataGridViewX1.Rows.Count - 1].DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.White, Font = new Font("楷体", 22) };
                    }
                }
                advTree1.Nodes.Clear();
                //输入设置
                for (int j = 0; j < _card.InputNames.Length; j++)
                {
                    Node node = new Node(_card.InputNames[j]);
                    Random r = new Random();
                    int i = r.Next(0, 10);
                    System.Threading.Thread.Sleep(5);
                    if (i % 2 == 0)
                    {
                        node.ImageIndex = 1;
                    }
                    else
                    {
                        node.ImageIndex = 0;
                    }
                    advTree1.Nodes.Add(node);
                }
                advTree2.Nodes.Clear();
                //输出设置
                for (int k = 0; k < _card.OutputNames.Length; k++)
                {
                    Node node = new Node(_card.OutputNames[k]);
                    node.Checked = false;
                    node.Expanded = true;
                    node.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.CheckBox;
                    node.CheckBoxVisible = true;
                    advTree2.Nodes.Add(node);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_card != null)
            {
                int axis = e.RowIndex;
                string value = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                if (e.ColumnIndex == dataGridViewX1.Columns["SoftLmtEnable"].Index)
                {
                    _card.SoftLmtEnable[axis] = bool.Parse(value);
                    if (_card.SoftLmtEnable[axis])
                    {
                        if (_card.NSoftLmt[axis] < _card.PSoftLmt[axis])
                        {
                            _card.SetSoftLmt(axis, 1, 0, 0, _card.NSoftLmt[axis], _card.PSoftLmt[axis]);
                        }
                        else
                        {
                            dataGridViewX1.Rows[e.RowIndex].Cells["SoftLmtEnable"].Value = false;
                            dataGridViewX1.RefreshEdit();
                            MessageBox.Show("正限位必须大于负限位，请重新设置！！");
                        }
                    }
                    else
                    {
                        _card.SetSoftLmt(axis, 0, 0, 0, _card.NSoftLmt[axis], _card.PSoftLmt[axis]);
                    }
                }
                else if (e.ColumnIndex == dataGridViewX1.Columns["SoftLmtN"].Index)
                {
                    _card.NSoftLmt[axis] = double.Parse(value);
                }
                else if (e.ColumnIndex == dataGridViewX1.Columns["SoftLmtP"].Index)
                {
                    _card.PSoftLmt[axis] = double.Parse(value);
                }
                else if (e.ColumnIndex == dataGridViewX1.Columns["Trans"].Index)
                {
                    _card.PulsePerMM[axis] = double.Parse(value);
                }
                else if (e.ColumnIndex == dataGridViewX1.Columns["StartSpeed"].Index)
                {
                    _card.StartVList[axis] = double.Parse(value);
                    _card.SetSpeed(axis, _card.StartVList[axis], _card.SpeedList[axis], _card.Aspeed[axis], _card.TaccList[axis]);
                }
                else if (e.ColumnIndex == dataGridViewX1.Columns["Speed"].Index)
                {
                    _card.SpeedList[axis] = double.Parse(value);
                    _card.SetSpeed(axis, _card.StartVList[axis], _card.SpeedList[axis], _card.Aspeed[axis], _card.TaccList[axis]);
                }
                else if (e.ColumnIndex == dataGridViewX1.Columns["ASpeed"].Index)
                {
                    _card.Aspeed[axis] = double.Parse(value);
                    _card.SetSpeed(axis, _card.StartVList[axis], _card.SpeedList[axis], _card.Aspeed[axis], _card.TaccList[axis]);
                }
                else if (e.ColumnIndex == dataGridViewX1.Columns["SmoothTime"].Index)
                {
                    _card.TaccList[axis] = double.Parse(value);
                    _card.SetSpeed(axis, _card.StartVList[axis], _card.SpeedList[axis], _card.Aspeed[axis], _card.TaccList[axis]);
                }
            }
        }

        private void dataGridViewX1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewX1.IsCurrentCellDirty)
            {
                dataGridViewX1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _card.GetCurrentInf();
            for (int i = 0; i < advTree1.Nodes.Count; i++)
            {
                if (_card.Singal[i])
                {
                    advTree1.Nodes[i].ImageIndex = 1;
                }
                else
                {
                    advTree1.Nodes[i].ImageIndex = 0;
                }
            }
        }

        private void advTree2_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            int num = e.Node.Index;
            if (e.Node.Checked)
            {
                _card.WriteOutput(num, 0);
            }
            else
            {
                _card.WriteOutput(num, 1);
            }
        }
    }
}
