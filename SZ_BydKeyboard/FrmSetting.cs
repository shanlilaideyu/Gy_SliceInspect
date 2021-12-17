using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using Newtonsoft.Json;
using DevComponents.DotNetBar.Controls;
using System.Windows.Forms.VisualStyles;
using System.Threading.Tasks;
using LeoMotion;
using System.Threading;
using LeoCam;
using HalconDotNet;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DevComponents.AdvTree;

namespace SZ_BydKeyboard
{
    public partial class FrmSetting : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FrmSetting()
        {
            InitializeComponent();
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            BackWorker.WorkerSupportsCancellation = true;
            BackWorker.WorkerReportsProgress = true;

            LoadBoardAndPostionSetting(Common.str_ProductName);
            InitCheck();
        }
        public class DataGridViewDisableButtonCell : DataGridViewButtonCell
        {
            private bool enabledValue;
            public bool Enabled
            {
                get
                {
                    return enabledValue;
                }
                set
                {
                    enabledValue = value;
                }
            }

            // Override the Clone method so that the Enabled property is copied.
            public override object Clone()
            {
                DataGridViewDisableButtonCell cell =
                    (DataGridViewDisableButtonCell)base.Clone();
                cell.Enabled = this.Enabled;
                return cell;
            }

            // By default, enable the button cell.
            public DataGridViewDisableButtonCell()
            {
                this.enabledValue = true;
            }

            protected override void Paint(Graphics graphics,
                Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                DataGridViewElementStates elementState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                // The button cell is disabled, so paint the border,  
                // background, and disabled button for the cell.
                if (!this.enabledValue)
                {
                    // Draw the cell background, if specified.
                    if ((paintParts & DataGridViewPaintParts.Background) ==
                        DataGridViewPaintParts.Background)
                    {
                        SolidBrush cellBackground =
                            new SolidBrush(cellStyle.BackColor);
                        graphics.FillRectangle(cellBackground, cellBounds);
                        cellBackground.Dispose();
                    }

                    // Draw the cell borders, if specified.
                    if ((paintParts & DataGridViewPaintParts.Border) ==
                        DataGridViewPaintParts.Border)
                    {
                        PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                            advancedBorderStyle);
                    }

                    // Calculate the area in which to draw the button.
                    Rectangle buttonArea = cellBounds;
                    Rectangle buttonAdjustment =
                        this.BorderWidths(advancedBorderStyle);
                    buttonArea.X += buttonAdjustment.X;
                    buttonArea.Y += buttonAdjustment.Y;
                    buttonArea.Height -= buttonAdjustment.Height;
                    buttonArea.Width -= buttonAdjustment.Width;

                    // Draw the disabled button.                
                    ButtonRenderer.DrawButton(graphics, buttonArea,
                        PushButtonState.Disabled);

                    // Draw the disabled button text. 
                    if (this.FormattedValue is String)
                    {
                        TextRenderer.DrawText(graphics,
                            (string)this.FormattedValue,
                            this.DataGridView.Font,
                            buttonArea, SystemColors.GrayText);
                    }
                }
                else
                {
                    // The button cell is enabled, so let the base class 
                    // handle the painting.
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                        elementState, value, formattedValue, errorText,
                        cellStyle, advancedBorderStyle, paintParts);
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!BackWorker.IsBusy)
            {
                Common.ProC.Card1.GetCurrentInf();
            }
            Tbox_Pos0.Text = Common.ProC.Card1.LogPos[0].ToString();
            Tbox_Speed0.Text = Common.ProC.Card1.CurrentSpeed[0].ToString();

            Tbox_Pos1.Text = Common.ProC.Card1.LogPos[1].ToString();
            Tbox_Speed1.Text = Common.ProC.Card1.CurrentSpeed[1].ToString();

            Tbox_Pos2.Text = Common.ProC.Card1.LogPos[2].ToString();
            Tbox_Speed2.Text = Common.ProC.Card1.CurrentSpeed[2].ToString();

            Tbox_Pos3.Text = Common.ProC.Card1.LogPos[3].ToString();
            Tbox_Speed3.Text = Common.ProC.Card1.CurrentSpeed[3].ToString();
        }

        public ECamera PlayCam = new ECamera();
        private void radialMenu1_ItemClick(object sender, EventArgs e)
        {
            if (PlayCam.Status == CamStatus.Open ? PlayCam.IsStart : false)
            {
                PlayCam.Stop();
            }
            RadialMenuItem item = sender as RadialMenuItem;
            if (item != null)
            {
                switch (item.Text)
                {
                    case "CAM1":
                        PlayCam = Common.Cam1;
                        break;
                    default:
                        break;
                }
            }
            if (PlayCam.Status == CamStatus.Open)
            {
                PlayCam.grabFinishCall = CamShowImage;
                PlayCam.Start();
                buttonX12.Enabled = true;
                buttonX21.Enabled = false;
            }
        }

        bool b_showCenterLine = false;
        public void CamShowImage(HObject ho_Image)
        {
            displayBase1.DisplaySourceImage(ho_Image.Clone());
            if (b_showCenterLine)
            {
                HObject hv_Line1 = new HObject(), hv_Line2 = new HObject();
                HOperatorSet.GenEmptyObj(out hv_Line2);
                HOperatorSet.GenEmptyObj(out hv_Line1);
                HTuple hv_Width = null, hv_Height = null;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.GenRegionLine(out hv_Line1, hv_Height / 2, 0, hv_Height / 2, hv_Width);
                HOperatorSet.GenRegionLine(out hv_Line2, 0, hv_Width / 2, hv_Height, hv_Width / 2);
                displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = hv_Line1 },
                      new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value = hv_Line2 } });

            }
            //ho_Image.Dispose();
        }

        private void buttonX24_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择本地文件";
            ofd.Filter = "图像文件|*.bmp;*.jpg;*,jpeg;*.png|所有文件|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                HObject ho_Image = null;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.ReadImage(out ho_Image, ofd.FileName);
                displayBase1.DisplaySourceImage(ho_Image);
            }
        }

        private void buttonX23_Click(object sender, EventArgs e)
        {
            if (buttonX23.SymbolColor == Color.DarkTurquoise)
            {
                b_showCenterLine = true;
                buttonX23.Symbol = "\ue14c";
                buttonX23.SymbolColor = Color.PaleVioletRed;
                buttonX23.Tooltip = "关闭中心线";
            }
            else
            {
                b_showCenterLine = false;
                buttonX23.Symbol = "\ue145";
                buttonX23.SymbolColor = Color.DarkTurquoise;
                buttonX23.Tooltip = "显示中心线";
            }
        }

        private void buttonX22_Click(object sender, EventArgs e)
        {
            if (PlayCam.Status == CamStatus.Open)
            {
                PlayCam.Snap();
            }
        }

        private void buttonX21_Click(object sender, EventArgs e)
        {
            if (PlayCam.Status == CamStatus.Open)
            {
                PlayCam.Start();
                buttonX12.Enabled = true;
                buttonX21.Enabled = false;
            }
        }

        private void buttonX12_Click(object sender, EventArgs e)
        {
            if (PlayCam.Status == CamStatus.Open)
            {
                PlayCam.Stop();
                buttonX12.Enabled = false;
                buttonX21.Enabled = true;
            }
        }

        private void buttonX11_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "图像文件|*.bmp";
            sfd.Title = "存储图像";
            if (advTree2.SelectedNode != null)
            {
                sfd.FileName = advTree2.SelectedNode.FullPath;
            }
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WriteImage(displayBase1.SourceImage, "bmp", 0, sfd.FileName);
            }
        }

        private void FrmSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Timer.Enabled = false;
            this.Hide();
            e.Cancel = true;
        }

        private void BTN_Move(object sender, EventArgs e)
        {
            ButtonX btx = sender as ButtonX;
            btx.Enabled = false;
            switch (btx.Name)
            {
                case "MOVE_PX":
                    Common.ProC.Card1.AxisPmove(0, platStep);
                    break;
                case "MOVE_NX":
                    Common.ProC.Card1.AxisPmove(0, platStep * -1);
                    break;
                case "MOVE_PY1":
                    Common.ProC.Card1.AxisPmove(1, platStep);
                    break;
                case "MOVE_NY1":
                    Common.ProC.Card1.AxisPmove(1, platStep * -1);
                    break;
                case "MOVE_PY2":
                    Common.ProC.Card1.AxisPmove(2, platStep);
                    break;
                case "MOVE_NY2":
                    Common.ProC.Card1.AxisPmove(2, platStep * -1);
                    break;
                case "MOVE_PZ":
                    Common.ProC.Card1.AxisPmove(3, platStep);
                    break;
                case "MOVE_NZ":
                    Common.ProC.Card1.AxisPmove(3, platStep * -1);
                    break;
                default:
                    break;
            }
            btx.Enabled = true;
        }

        double platStep = 10;
        private void MOVE_HOME_Click(object sender, EventArgs e)
        {
            Common.ProC.AllAxisGohome();
        }


        private void slider1_ValueChanged(object sender, EventArgs e)
        {
            slider1.Text = $"A:{slider1.Value}";
            if (Common.LightControlPort.IsOpen)
            {
                Common.LightControlPort.Write($"SA{slider1.Value.ToString().PadLeft(4, '0')}#\r\n");
            }
        }

        private void slider2_ValueChanged(object sender, EventArgs e)
        {
            slider2.Text = $"B:{slider2.Value}";
            if (Common.LightControlPort.IsOpen)
            {
                Common.LightControlPort.Write($"SB{slider2.Value.ToString().PadLeft(4, '0')}#\r\n");
            }
        }

        private void Plat_Step_TextChanged(object sender, EventArgs e)
        {
            try
            {
                platStep = double.Parse(Plat_Step.Text);
            }
            catch (Exception)
            {
                Plat_Step.Text = "10";
            }
        }


        public void LoadBoardAndPostionSetting(string str_ProductName)
        {

            if (dataGridViewX1.DataSource != null)
            {
                DataTable dt = (DataTable)dataGridViewX1.DataSource;
                dt.Rows.Clear();
                dataGridViewX1.DataSource = dt;
            }
            else
            {
                dataGridViewX1.Rows.Clear();
            }
            if (Common.Data.DictAxis.Count == 0)
            {
                for (int i = 0; i < Common.str_PosList.Count; i++)
                {
                    Common.Data.DictPos.Add(Common.str_PosList[i], 5000);
                }

                for (int k = 0; k < Common.str_PosList.Count; k++)
                {
                    Common.Data.DictAxis.Add(Common.str_PosList[k], 1);
                }
            }
            for (int x = 0; x < Common.str_PosList.Count; x++)
            {
                addNewLine(Common.str_PosList[x], true);
            }
        }

        private void addNewLine(string strPositionName, bool b_CanMove)
        {
            DataGridViewRow row = new DataGridViewRow();
            //名称
            DataGridViewLabelXCell labelxcell = new DataGridViewLabelXCell();
            labelxcell.Value = strPositionName;
            row.Cells.Add(labelxcell);
            //轴号
            DataGridViewTextBoxCell textboxcell1 = new DataGridViewTextBoxCell();
            textboxcell1.Value = Common.Data.DictAxis[strPositionName].ToString();
            row.Cells.Add(textboxcell1);
            //位置
            DataGridViewTextBoxCell textboxcell2 = new DataGridViewTextBoxCell();
            textboxcell2.Value = Common.Data.DictPos[strPositionName].ToString();
            row.Cells.Add(textboxcell2);
            //按钮
            // 获取位置
            if (b_CanMove)
            {
                DataGridViewButtonCell btcel = new DataGridViewButtonCell();
                btcel.FlatStyle = FlatStyle.Flat;
                btcel.Value = "获取";
                row.Cells.Add(btcel);

                DataGridViewButtonCell btcell = new DataGridViewButtonCell();
                btcell.FlatStyle = FlatStyle.Flat;
                btcell.Value = "前往";
                row.Cells.Add(btcell);
            }
            else
            {
                DataGridViewDisableButtonCell disbtCell = new DataGridViewDisableButtonCell();
                disbtCell.Enabled = false;
                row.Cells.Add(disbtCell);
                DataGridViewDisableButtonCell disbtCell1 = new DataGridViewDisableButtonCell();
                disbtCell1.Enabled = false;
                row.Cells.Add(disbtCell1);
            }
            dataGridViewX1.Rows.Add(row);
        }

        private void Textbox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                int Col = dataGridViewX1.CurrentCell.ColumnIndex;
                try
                {
                    if (Col == 2)
                    {
                        Common.Data.DictPos[dataGridViewX1.CurrentRow.Cells["PositionName"].Value.ToString()] = double.Parse(tb.Text.ToString().Trim());
                    }
                    else if (Col == 1)
                    {
                        Common.Data.DictAxis[dataGridViewX1.CurrentRow.Cells["PositionName"].Value.ToString()] = int.Parse(tb.Text.ToString().Trim());
                    }
                }
                catch (Exception ex)
                {
                    tb.Text = "0";
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridViewX1.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    //这里可以编写你需要的任意关于按钮事件的操作~
                    switch (column.Name)
                    {
                        case "BtnMove":
                            DataGridViewButtonCell btcell = dataGridViewX1.CurrentRow.Cells["BtnMove"] as DataGridViewButtonCell;
                            if (btcell.Value != null ? btcell.Value.ToString() == "前往" : false)
                            {
                                try
                                {
                                    double n_target = double.Parse(dataGridViewX1.CurrentRow.Cells["PulsePosition"].Value.ToString());
                                    int n_axis = int.Parse(dataGridViewX1.CurrentRow.Cells["AxisNum"].Value.ToString());
                                    double n_speed = Common.ProC.Card1.SpeedList[n_axis];
                                    double n_startV = Common.ProC.Card1.StartVList[n_axis];
                                    double d_tacc = Common.ProC.Card1.TaccList[n_axis];
                                    //开始运动
                                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:ms -->") +
                                        string.Format("开始运动到->{0}<-", dataGridViewX1.CurrentRow.Cells["PositionName"].Value.ToString()));
                                    Common.ProC.Card1.AxisMove(n_axis, LeoMotion.MoveType.Absolute, n_target, n_startV, n_speed, n_speed * 2, d_tacc);
                                }
                                catch (Exception ex)
                                {
                                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:ms -->") +
                                      string.Format("Error:{0}", ex.Message));
                                }
                            }
                            break;
                        case "BtnGet":
                            DataGridViewButtonCell btcell1 = dataGridViewX1.CurrentRow.Cells["BtnGet"] as DataGridViewButtonCell;
                            if (btcell1.Value != null ? btcell1.Value.ToString() == "获取" : false)
                            {
                                Common.Data.DictPos[dataGridViewX1.CurrentRow.Cells["PositionName"].Value.ToString()] = Common.ProC.Card1.LogPos[int.Parse(dataGridViewX1.CurrentRow.Cells["AxisNum"].Value.ToString())];
                                dataGridViewX1.CurrentRow.Cells["PulsePosition"].Value = Common.ProC.Card1.LogPos[int.Parse(dataGridViewX1.CurrentRow.Cells["AxisNum"].Value.ToString())];
                                Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:ms -->") + string.Format("{0}设置为：{1}<-", dataGridViewX1.CurrentRow.Cells["PositionName"].Value.ToString(),
                                       Common.ProC.Card1.LogPos[int.Parse(dataGridViewX1.CurrentRow.Cells["AxisNum"].Value.ToString())]));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void dataGridViewX1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
            {
                TextBox tb = e.Control as TextBox;
                tb.TextChanged += Textbox_TextChanged;
            }
        }

        private void UpdateCheck()
        {
            if (advTree2.SelectedNode != null)
            {
                Node node = advTree2.SelectedNode;
                node.Cells[1].Text = Common.ProC.Card1.LogPos[0].ToString();
                node.Cells[2].Text = Common.ProC.Card1.LogPos[1].ToString();
                node.Cells[3].Text = Common.ProC.Card1.LogPos[3].ToString();
                node.Cells[4].Text = slider1.Value.ToString();
                node.Cells[5].Text = slider2.Value.ToString();
                Common.Data.DicCheck[node.FullPath] = new CheckPos()
                {
                    Xpos = Common.ProC.Card1.LogPos[0],
                    Ypos = Common.ProC.Card1.LogPos[1],
                    Zpos = Common.ProC.Card1.LogPos[3],
                    Channel1 = slider1.Value,
                    Channel2 = slider2.Value,
                };
            }
        }

        private void GotoCheck1()
        {
            if (advTree2.SelectedNode != null)
            {
                string checkName = advTree2.SelectedNode.FullPath;

                Common.ProC.Card1.AbsoluteMove(0, Common.Data.DicCheck[checkName].Xpos, 10, 200, 400, 0.5);
                Common.ProC.Card1.AbsoluteMove(1, Common.Data.DicCheck[checkName].Ypos, 10, 200, 400, 0.5);
                Common.ProC.Card1.AbsoluteMove(3, Common.Data.DicCheck[checkName].Zpos, 1, 10, 400, 0.5);
                if (Common.LightControlPort.IsOpen)
                {
                    slider1.Value = Common.Data.DicCheck[checkName].Channel1;
                    slider2.Value = Common.Data.DicCheck[checkName].Channel2;
                }
            }
        }

        private void GotoCheck2()
        {
            if (advTree2.SelectedNode != null)
            {
                string checkName = advTree2.SelectedNode.FullPath;

                Common.ProC.Card1.AbsoluteMove(0, Common.Data.DicCheck[checkName].Xpos + Common.Data.OffsetX2toX1, 10, 200, 400, 0.5);
                Common.ProC.Card1.AbsoluteMove(2, Common.Data.DicCheck[checkName].Ypos + Common.Data.OffsetY2toY1, 10, 200, 400, 0.5);
                Common.ProC.Card1.AbsoluteMove(3, Common.Data.DicCheck[checkName].Zpos + Common.Data.OffsetZ2toZ1, 1, 10, 400, 0.5);
                if (Common.LightControlPort.IsOpen)
                {
                    slider1.Value = Common.Data.DicCheck[checkName].Channel1;
                    slider2.Value = Common.Data.DicCheck[checkName].Channel2;
                }
            }
        }

        public void InitCheck()
        {
            advTree2.Nodes.Clear();
            for (int i = 0; i < Common.FaiNames.Length; i++)
            {
                Node node = new Node($"{Common.FaiNames[i]}");
                if (!Common.Data.DicCheck.ContainsKey(Common.FaiNames[i]))
                {
                    node.Cells.Add(new Cell($"{Common.ProC.Card1.LogPos[0]}"));
                    node.Cells.Add(new Cell($"{Common.ProC.Card1.LogPos[1]}"));
                    node.Cells.Add(new Cell($"{Common.ProC.Card1.LogPos[3]}"));
                    node.Cells.Add(new Cell($"{slider1.Value}"));
                    node.Cells.Add(new Cell($"{slider2.Value}"));
                    Common.Data.DicCheck.Add(Common.FaiNames[i], new CheckPos()
                    {
                        Xpos = Common.ProC.Card1.LogPos[0],
                        Ypos = Common.ProC.Card1.LogPos[1],
                        Zpos = Common.ProC.Card1.LogPos[3],
                        Channel1 = slider1.Value,
                        Channel2 = slider2.Value,
                    });
                }
                else
                {
                    node.Cells.Add(new Cell($"{Common.Data.DicCheck[Common.FaiNames[i]].Xpos}"));
                    node.Cells.Add(new Cell($"{Common.Data.DicCheck[Common.FaiNames[i]].Ypos}"));
                    node.Cells.Add(new Cell($"{Common.Data.DicCheck[Common.FaiNames[i]].Zpos}"));
                    node.Cells.Add(new Cell($"{Common.Data.DicCheck[Common.FaiNames[i]].Channel1}"));
                    node.Cells.Add(new Cell($"{Common.Data.DicCheck[Common.FaiNames[i]].Channel2}"));
                }
                advTree2.Nodes.Add(node);
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            UpdateCheck();
        }

        private void buttonX2_Click_1(object sender, EventArgs e)
        {
            GotoCheck1();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            try
            {
                string result = JsonConvert.SerializeObject(Common.Data);
                if (!Directory.Exists($"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\"))
                {
                    Directory.CreateDirectory($"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\");
                }
                string fp = $"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\RunningData.json";
                if (!File.Exists(fp))  // 判断是否已有相同文件 
                {
                    FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                    fs1.Close();
                }
                File.WriteAllText(fp, result.Replace(",\"", ",\r\n\"").Replace(":{", ":\r\n{"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败！原因为:{ex.Message}");
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Common.ProC.Card1.StopRun(0, 0);
            Common.ProC.Card1.StopRun(1, 0);
            Common.ProC.Card1.StopRun(2, 0);
            Common.ProC.Card1.StopRun(3, 0);
        }

        private void BackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (BackWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(20);

                Common.ProC.GetStatus();
                Common.ProC.Control_T();
                AutoCalibra();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Common.ProC.nCheckStep1 = 0;
            Common.ProC.nCheckStep2 = -1;
            Common.ProC.nCheckIdx = 0;
            Common.Cam1.grabFinishCall = Common.grabImage1;
            BackWorker.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Common.ImageIdx = 0;
            BackWorker.CancelAsync();
        }

        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton1.Value)
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸1前进.GetHashCode(), 0);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸1后退.GetHashCode(), 1);
            }
            else
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸1前进.GetHashCode(), 1);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸1后退.GetHashCode(), 0);
            }
        }

        private void switchButton2_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton2.Value)
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸2前进.GetHashCode(), 0);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸2后退.GetHashCode(), 1);
            }
            else
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸2前进.GetHashCode(), 1);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具1气缸2后退.GetHashCode(), 0);
            }
        }

        private void switchButton4_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton4.Value)
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸1前进.GetHashCode(), 0);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸1后退.GetHashCode(), 1);
            }
            else
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸1前进.GetHashCode(), 1);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸1后退.GetHashCode(), 0);
            }
        }

        private void switchButton3_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton3.Value)
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸2前进.GetHashCode(), 0);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸2后退.GetHashCode(), 1);
            }
            else
            {
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸2前进.GetHashCode(), 1);
                Common.ProC.Card1.WriteOutput(ProControl.Card1Output.治具2气缸2后退.GetHashCode(), 0);
            }
        }


        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (Common.HeightSensorPort.IsOpen)
            {
                Byte[] buffer = new Byte[10];
                buffer[0] = 0x02;
                buffer[1] = 0x4D;
                buffer[2] = 0x45;
                buffer[3] = 0x41;
                buffer[4] = 0x53;
                buffer[5] = 0x55;
                buffer[6] = 0x52;
                buffer[7] = 0x45;
                buffer[8] = 0x03;
                buffer[9] = 0x0a;
                Common.HeightSensorPort.Write(buffer, 0, 10);
            }
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (Common.DataReadPort1.IsOpen)
            {
                Common.DataReadPort1.Write("T\r\n");
            }
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            if (Common.DataReadPort2.IsOpen)
            {
                Common.DataReadPort2.Write("T\r\n");
            }
        }

        public int nCalibraStep = -1, iCalibraIndex = 0;
        public HTuple hv_PhysX = new HTuple();
        public HTuple hv_PhysY = new HTuple();
        public HTuple hv_ImageRows = new HTuple();
        public HTuple hv_ImageCols = new HTuple();

        public double[] CalibraX = new double[9];
        public double[] CalibraY = new double[9];
        public bool b_Calibra = false;
        private void button4_Click(object sender, EventArgs e)
        {
            hv_PhysX = new HTuple(-4, -4, -4, 0, 0, 0, 4, 4, 4);
            hv_PhysY = new HTuple(-4, 0, 4, -4, 0, 4, -4, 0, 4);
            for (int i = 0; i < CalibraX.Length; i++)
            {
                CalibraX[i] = hv_PhysX.DArr[i] + Common.ProC.Card1.LogPos[0];
                CalibraY[i] = hv_PhysY.DArr[i] + Common.ProC.Card1.LogPos[1];
            }
            hv_ImageRows = new HTuple();
            hv_ImageCols = new HTuple();
            b_Calibra = true;
            nCalibraStep = 0;
            BackWorker.RunWorkerAsync();
        }


        public void AutoCalibra()
        {
            if (nCalibraStep == 0)
            {
                Common.ShowMsgEvent($"Tips: 开始第{iCalibraIndex}点标定。。");
                //Common.ProC.MovePlaX(hv_PhysX.DArr[iCalibraIndex], MoveType.Absolute);
                Common.ProC.Card1.AbsoluteMove(1, CalibraX[iCalibraIndex], Common.ProC.Card1.StartVList[1], Common.ProC.Card1.SpeedList[1], Common.ProC.Card1.Aspeed[1], Common.ProC.Card1.TaccList[1]);
                Common.ProC.Card1.AbsoluteMove(2, CalibraY[iCalibraIndex], Common.ProC.Card1.StartVList[2], Common.ProC.Card1.SpeedList[2], Common.ProC.Card1.Aspeed[2], Common.ProC.Card1.TaccList[2]);

                nCalibraStep = 10;
            }
            else if (nCalibraStep == 10)
            {
                if (!Common.ProC.Card1.b_Move[1] && !Common.ProC.Card1.b_Move[2])
                {
                    //Common.ShowMsgEvent($"Tips:运动完成。。");
                    Common.Cam1.grabFinishCall = Common.grabImage1;
                    Common.Cam1.Snap();
                    Common.ProC.Start_T(3, 300);
                    nCalibraStep = 30;
                }
            }
            else if (nCalibraStep == 30)
            {
                if (Common.ProC.TimerSignal[3])
                {
                    Common.ProC.Reset_T(3);
                    if (iCalibraIndex < 8)
                    {
                        iCalibraIndex++;
                        nCalibraStep = 0;
                    }
                    else
                    {
                        BackWorker.CancelAsync();
                        nCalibraStep = -1;
                    }
                }
            }

            if (b_Calibra)
            {
                if (Common.QueCam1.Count > 0)
                {
                    HObject ho_Image = Common.QueCam1.Dequeue();
                    displayBase1.DisplaySourceImage(ho_Image);

                    HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
                    HObject ho_Cross = new HObject(), ho_Rectangle2 = new HObject(), ho_TempImage = new HObject(), ho_Region = new HObject();

                    HTuple hv_ImageRow, hv_ImageCol;
                    GetPositionAndDeg(displayBase1.SourceImage, out hv_ImageRow, out hv_ImageCol);

                    Common.ShowMsgEvent($"Tips:当前图像坐标 Row:{hv_ImageRow.D.ToString("0.000")},Col:{hv_ImageCol.D.ToString("0.000")}");
                    hv_ImageRows = hv_ImageRows.TupleConcat(hv_ImageRow);
                    hv_ImageCols = hv_ImageCols.TupleConcat(hv_ImageCol);
                    if (hv_ImageRows.Length == 9)
                    {
                        HHomMat2D hom = new HHomMat2D();
                        hom.VectorToHomMat2d(hv_ImageRows, hv_ImageCols, hv_PhysX, hv_PhysY);
                        HOperatorSet.WriteTuple(hom, $"{ System.Windows.Forms.Application.StartupPath}\\SysCfg\\HomMat2D.tup");
                        Common.ShowMsgEvent("Tips: 九点标定完成");
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            GotoCheck2();
        }

        private void GetPositionAndDeg(HObject sourceImage, out HTuple hv_ImageRow, out HTuple hv_ImageCol)
        {
            HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
            HObject ho_Cross = new HObject(), ho_Rectangle2 = new HObject(), ho_TempImage = new HObject(), ho_Region = new HObject();
            HTuple hv_Area;
            hv_ImageRow = null; hv_ImageCol = null;

            HOperatorSet.Threshold(sourceImage, out ho_Region, 140, 255);
            HOperatorSet.Connection(ho_Region, out ho_ConnectionRegion);
            HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"),
                new HTuple("and"), new HTuple(170).TupleConcat(170), new HTuple(220).TupleConcat(220));
            if (ho_SelectedRegions.CountObj() != 0)
            {
                HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_ImageRow, out hv_ImageCol);
            }
        }
    }
}