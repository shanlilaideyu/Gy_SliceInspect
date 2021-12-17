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

namespace LeoMotion
{
    public partial class ExIOControl : UserControl
    {
        public ExIOControl()
        {
            InitializeComponent();
        }

        public ExCard _card;
        public void BindingExCard()
        {
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
        }

        private void advTree2_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            int num = e.Node.Index;
            if (e.Node.Checked)
            {
                _card.WriteOutput((ushort)num, 0);
            }
            else
            {
                _card.WriteOutput((ushort)num, 1);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _card.GetAllInput();
            for (int i = 0; i < advTree1.Nodes.Count; i++)
            {
                if (_card.InputList[i])
                {
                    advTree1.Nodes[i].ImageIndex = 1;
                }
                else
                {
                    advTree1.Nodes[i].ImageIndex = 0;
                }
            }
        }
    }
}
