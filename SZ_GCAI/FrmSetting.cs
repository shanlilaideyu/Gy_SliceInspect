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
using DevComponents.DotNetBar.Charts;
using DevComponents.DotNetBar.Charts.Style;
using System.Runtime.Serialization.Formatters.Binary;

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
            SetupChart();
            showParam();
        }

        private void showParam()
        {
            textBox21.Text = Common.Data.str_SavePath;
            checkBox1.Checked = Common.Data.b_SaveImage;
            Tbox_EndPos.Text = Common.Data.endPos.ToString();
            Tbox_StartPos.Text = Common.Data.startPos.ToString();
            Tbox_Step.Text = Common.Data.stepPos.ToString();
            Tbox_BaseHeight.Text = Common.Data.Base_Height.ToString();
            Base_PlatRow.Text = Common.Data.Base_PlatRow.ToString("0.000");
            Base_PlatCol.Text = Common.Data.Base_PlatCol.ToString("0.000");
            Base_PlatDeg.Text = Common.Data.Base_PlatDeg.ToString("0.000");
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
                    case "芯片相机":
                        PlayCam = Common.Cam1;
                        break;
                    case "针卡相机":
                        PlayCam = Common.Cam2;
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
            //HOperatorSet.SetSystem("flush_graphic", "false");
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
            //HOperatorSet.SetSystem("flush_graphic", "true");
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




        public void grabImage2(HObject ho_Image)
        {
            Common.QueCam2.Enqueue(ho_Image.CopyObj(1, 1));
        }


        #region 高度对焦测试

        #region ChartSetting
        private void SetupChart()
        {
            SetupScrollBarStyles(chartControl1);
            SetupPanel(chartControl1);
            chartControl1.ChartPanel.ChartContainers.Clear();
        }

        /// <summary>
        /// Sets up the scrollbar styles.
        /// </summary>
        private void SetupScrollBarStyles(ChartControl chartControl1)
        {
            ScrollBarVisualStyle moStyle =
                chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver;

            moStyle.ArrowBackground = new Background(Color.AliceBlue);
            moStyle.ThumbBackground = new Background(Color.AliceBlue);

            ScrollBarVisualStyle smoStyle =
                chartControl1.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver;

            smoStyle.ArrowBackground = new Background(Color.White);
            smoStyle.ThumbBackground = new Background(Color.White);

            moStyle = chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver;

            moStyle.ArrowBackground = new Background(Color.AliceBlue);
            moStyle.ThumbBackground = new Background(Color.AliceBlue);

            smoStyle = chartControl1.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver;

            smoStyle.ArrowBackground = new Background(Color.White);
            smoStyle.ThumbBackground = new Background(Color.White);
        }

        private void SetupPanel(ChartControl chartControl1)
        {
            ChartPanel chartPanel = chartControl1.ChartPanel;

            // Don't have the ChartControl autosize or autofill the ChartPanel's
            // ChartMatrix, as we are going to do both of those things out==rselves.

            chartPanel.AutoSizeChartMatrix = true;
            chartPanel.AutoFillChartMatrix = true;

            // We'll define an 8x8 matrix, and set the
            // default cell size to 50x50.

            chartPanel.ChartMatrix.Size = new Size(8, 8);
            chartPanel.ChartMatrix.DefaultCellSize = new Size(50, 50);

            chartPanel.ChartMatrix.DividerLines = DividerLines.None;

            // Setup our panel Legend and then add the panel
            // Title and associated charts.

            SetupPanelLegend(chartPanel);

            if (chartControl1 == this.chartControl1)
            {
                AddRadianPanelTitle(chartPanel);
            }
            else
            {
                AddWavePanelTitle(chartPanel);
            }
        }

        private void SetupPanelLegend(ChartPanel chartPanel)
        {
            ChartLegend legend = chartPanel.Legend;

            legend.Visible = true;

            legend.ShowCheckBoxes = false;

            legend.Placement = Placement.Outside;
            legend.Alignment = Alignment.TopRight;
            legend.Direction = Direction.TopToBottom;
            legend.ItemSortDirection = SortDirection.Ascending;

            ChartLegendVisualStyle lstyle = legend.ChartLegendVisualStyles.Default;

            lstyle.BorderThickness = new Thickness(1);
            lstyle.BorderColor = new BorderColor(Color.DarkBlue);

            lstyle.Margin = new DevComponents.DotNetBar.Charts.Style.Padding(0, 0, 10, 0);
            lstyle.Padding = new DevComponents.DotNetBar.Charts.Style.Padding(4);

            lstyle.Background = new Background(Color.FromArgb(200, Color.White));

            lstyle.DropShadow.Enabled = Tbool.True;
        }

        private void AddRadianPanelTitle(ChartPanel chartPanel)
        {
            ChartTitle title = new ChartTitle();

            title.Text = "梯度(Height,Value)";
            title.XyAlignment = XyAlignment.Bottom;

            ChartTitleVisualStyle tstyle = title.ChartTitleVisualStyle;

            tstyle.Padding = new DevComponents.DotNetBar.Charts.Style.Padding(4);
            tstyle.Font = new Font("楷体", 14);
            tstyle.Alignment = Alignment.MiddleCenter;
            tstyle.TextColor = Color.DarkBlue;

            chartPanel.Titles.Add(title);
        }

        private void AddWavePanelTitle(ChartPanel chartPanel)
        {
            ChartTitle title = new ChartTitle();

            title.Text = "波浪(Cols,Height)";
            title.XyAlignment = XyAlignment.Bottom;

            ChartTitleVisualStyle tstyle = title.ChartTitleVisualStyle;

            tstyle.Padding = new DevComponents.DotNetBar.Charts.Style.Padding(4);
            tstyle.Font = new Font("楷体", 14);
            tstyle.Alignment = Alignment.MiddleCenter;
            tstyle.TextColor = Color.DarkBlue;

            chartPanel.Titles.Add(title);
        }

        private ChartXy SetRadianChart(List<double> Cols, List<double> Values, List<double> Cols1, List<double> Values1)
        {

            chartControl1.ChartPanel.ChartContainers.Clear();
            ChartXy chartXy = new ChartXy("");
            chartXy.Legend.Visible = false;
            // Set the matrix display order (higher values
            // are on top of lower values).
            chartXy.MatrixDisplayOrder = 0;

            // Set the Matrix Display Bounds (starting cell position
            // followed by the width and height (in cells).

            chartXy.MatrixDisplayBounds = new Rectangle(0, 0, 8, 6);

            // The following tells the chart control to align the start
            // and ending bounding columns with other charts that start/end
            // in the same columns.

            chartXy.MatrixAlignStartColumn = true;
            chartXy.MatrixAlignEndColumn = true;

            // Setup our Crosshair display.

            chartXy.ChartCrosshair.AxisOrientation = AxisOrientation.X;

            chartXy.ChartCrosshair.ShowValueXLine = true;
            chartXy.ChartCrosshair.ShowValueYLine = true;
            chartXy.ChartCrosshair.ShowValueYLabels = true;

            chartXy.ChartCrosshair.ShowCrosshairLabels = true;
            chartXy.ChartCrosshair.CrosshairLabelMode = CrosshairLabelMode.NearestSeries;

            // Setup various styles for the chart...

            SetupChartStyle(chartXy);
            SetupIntensityAxes(chartXy);

            // Add a chart title and associated series.

            AddChartTitle(chartXy);

            // And finally, add the chart to the ChartContainers
            // collection of chart elements.
            chartControl1.ChartPanel.ChartContainers.Add(chartXy);
            return (chartXy);
        }

        private ChartXy SetUpRadianChart(List<double> Cols, List<double> Values)
        {

            chartControl1.ChartPanel.ChartContainers.Clear();
            ChartXy chartXy = new ChartXy("");
            chartXy.Legend.Visible = false;
            // Set the matrix display order (higher values
            // are on top of lower values).
            chartXy.MatrixDisplayOrder = 0;

            // Set the Matrix Display Bounds (starting cell position
            // followed by the width and height (in cells).

            chartXy.MatrixDisplayBounds = new Rectangle(0, 0, 8, 6);

            // The following tells the chart control to align the start
            // and ending bounding columns with other charts that start/end
            // in the same columns.

            chartXy.MatrixAlignStartColumn = true;
            chartXy.MatrixAlignEndColumn = true;

            // Setup our Crosshair display.

            chartXy.ChartCrosshair.AxisOrientation = AxisOrientation.X;

            chartXy.ChartCrosshair.ShowValueXLine = true;
            chartXy.ChartCrosshair.ShowValueYLine = true;
            chartXy.ChartCrosshair.ShowValueYLabels = true;

            chartXy.ChartCrosshair.ShowCrosshairLabels = true;
            chartXy.ChartCrosshair.CrosshairLabelMode = CrosshairLabelMode.NearestSeries;

            // Setup various styles for the chart...

            SetupChartStyle(chartXy);
            SetupIntensityAxes(chartXy);
            // Add a chart title and associated series.

            AddChartTitle(chartXy);
            AddRadianSeries(chartXy, Cols, Values);

            // And finally, add the chart to the ChartContainers
            // collection of chart elements.
            chartControl1.ChartPanel.ChartContainers.Add(chartXy);
            return (chartXy);
        }
        private void SetupChartStyle(ChartXy chartXy)
        {
            ChartXyVisualStyle xystyle = chartXy.ChartVisualStyle;

            xystyle.Background = new Background(Color.White);
            xystyle.BorderThickness = new Thickness(1);
            xystyle.BorderColor = new BorderColor(Color.Black);

            xystyle.DropShadow.Enabled = Tbool.True;
        }

        private void SetupIntensityAxes(ChartXy chartXy)
        {
            // X Axis

            ChartAxis axis = chartXy.AxisX;

            axis.Visible = false;

            axis.MinorTickmarks.TickmarkCount = 0;

            axis.MajorGridLines.GridLinesVisualStyle.LineColor = Color.Gainsboro;
            axis.MinorGridLines.GridLinesVisualStyle.LineColor = Color.WhiteSmoke;
            axis.MajorTickmarks.LabelVisualStyle.TextFormat = "#0.000";

            axis.AxisMargins = 30;
            axis.GridSpacing = .5;

            // Y Axis

            axis = chartXy.AxisY;

            axis.AxisAlignment = AxisAlignment.Far;
            axis.AxisMargins = 15;
            axis.GridSpacing = 250;
            axis.MinValue = 0;
            axis.MinorTickmarks.TickmarkCount = 0;

            axis.MajorTickmarks.LabelVisualStyle.TextFormat = "#0.000";

            axis.MajorGridLines.GridLinesVisualStyle.LineColor = Color.Gainsboro;
            axis.MinorGridLines.GridLinesVisualStyle.LineColor = Color.WhiteSmoke;
        }

        private void AddChartTitle(ChartXy chartXy)
        {
            ChartTitle title = new ChartTitle();

            title.Text = chartXy.Name;
            title.XyAlignment = XyAlignment.Left;

            ChartTitleVisualStyle tstyle = title.ChartTitleVisualStyle;

            tstyle.Padding = new DevComponents.DotNetBar.Charts.Style.Padding(4);
            tstyle.Font = new Font("Arial", 14);
            tstyle.Alignment = Alignment.MiddleCenter;
            tstyle.TextColor = Color.DarkBlue;

            chartXy.Titles.Add(title);
        }

        private void AddRadianSeries(ChartXy chartXy, List<double> Cols, List<double> Values)
        {
            ChartSeries series = new ChartSeries("梯度数据1", SeriesType.Line);
            //List<string> sigmaData = ShellServices.LoadText("LinePlot_Matrix.Resources.Intensity.txt");
            for (int i = 0; i < Cols.Count; i++)
            {
                SeriesPoint sp = new SeriesPoint(Cols[i], Values[i]);
                series.SeriesPoints.Add(sp);
            }
            series.ChartSeriesVisualStyle.LineStyle.LineColor = Color.Maroon;
            series.ChartSeriesVisualStyle.LineStyle.LineWidth = 2;
            chartXy.ChartSeries.Add(series);
        }

        #endregion





        private void Btn_ZgoHome_Click(object sender, EventArgs e)
        {
            if (Common.ProC.Card1.GoHome(0, 1, 0, 0.5, 1))
            {
                Common.ShowMsgEvent($"Tips: --{DateTime.Now.ToString("yyyy:MM:dd HH:mm:ms")}--Z轴回零完成！");
            }
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            nActionStep = 0;
            nImageIndex = 0;
            Common.Data.PosList = new List<double> { };
            Common.Data.Gradient = new List<double> { };
            BackWorker.RunWorkerAsync();
            Btn_Start.Enabled = false;
            Btn_Stop.Enabled = true;
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            BackWorker.CancelAsync();
            Btn_Start.Enabled = true;
            Btn_Stop.Enabled = false;
        }

        public void ShowAndSaveImage(HObject ho_Image)
        {
            displayBase1.DisplaySourceImage(ho_Image);
            if (Common.Data.b_SaveImage)
            {
                HOperatorSet.WriteImage(ho_Image, "bmp", 0, $"{ Common.Data.str_SavePath}//{Common.ProC.Card1.LogPos[0].ToString("0.000")}.bmp");
            }
            HObject ho_ImageReduced = new HObject(); HObject ho_EdgeAmplitude = new HObject();
            HTuple hv_Mean = null, hv_Deviation = null;

            //HOperatorSet.GenRectangle1(out ho_Region, 1530, 760, 1680, 890);
            displayBase1.AddRegion(new LeoBase.HsbRegions() { new LeoBase.HsbRegion() { Value = Common.HalObject.ho_FocusRegion, Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin } });
            HOperatorSet.ReduceDomain(ho_Image, Common.HalObject.ho_FocusRegion, out ho_ImageReduced);
            HOperatorSet.SobelAmp(ho_ImageReduced, out ho_EdgeAmplitude, "sum_abs", 3);
            HOperatorSet.Intensity(ho_EdgeAmplitude, ho_EdgeAmplitude, out hv_Mean, out hv_Deviation);
            Common.Data.Gradient.Add(hv_Mean.D);
            if (targetPos == Common.Data.endPos)
            {
                SetUpRadianChart(Common.Data.PosList, Common.Data.Gradient);
                this.BeginInvoke(new ShowDelegte(ShowResult), Common.Data.PosList, Common.Data.Gradient);
            }
        }

        delegate void ShowDelegte(List<double> PosList, List<double> GradientList);
        public void ShowResult(List<double> PosList, List<double> GradientList)
        {
            SetUpRadianChart(PosList, GradientList);
            //PublicClass.AddInfoToListBox(listBox1, $"{DateTime.Now.ToString("HH:ss:ms")}---对焦最佳位置为---{pos}");
            label34.Text = $"{DateTime.Now.ToString("HH:ss:ms")}---对焦最佳位置为---[{PosList[GradientList.IndexOf(GradientList.Max())]}]";
        }

        //对焦
        double targetPos;
        public int nActionStep = -1, nImageIndex = 0;
        //标定
        public bool b_Calibra = false;
        public double dOffset = 1.0, dCenterX1 = 0, dCenterX2 = 0, dCenterY = 0;
        public int nCalibraStep = -1, iCalibraIndex = 0;
        public HTuple hv_PhysX = new HTuple();
        public HTuple hv_PhysY = new HTuple();
        public HTuple hv_ImageRows = new HTuple();
        public HTuple hv_ImageCols = new HTuple();

        //匹配
        public int nMatch = -1;
        public bool b_First = false, b_Second = false, b_FirstFinish = false, b_SecondFinish = false;
        public double offsetDeg = 0.0, offsetX = 0.0, offsetY = 0.0;
        public void AutoFocus()
        {
            if (nActionStep == 0)
            {
                targetPos = Math.Round(Common.Data.startPos + nImageIndex * Common.Data.stepPos, 4);
                Common.ProC.Card1.AbsoluteMove(0, targetPos, 0.01, 0.5, 0.5);
                nActionStep = 10;
            }
            else if (nActionStep == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - targetPos) < 0.001)
                {
                    Common.ProC.Start_T(0, 200);
                    nActionStep = 20;
                }
            }
            else if (nActionStep == 20)
            {
                if (Common.ProC.TimerSignal[0])
                {
                    Common.ProC.Reset_T(0);
                    Common.Data.PosList.Add(Common.ProC.Card1.LogPos[0]);
                    Common.Cam2.grabFinishCall = ShowAndSaveImage;
                    Common.Cam2.Snap();
                    nImageIndex++;
                    BackWorker.ReportProgress(nImageIndex);
                    Common.ProC.Start_T(1, 300);
                    nActionStep = 30;
                }
            }
            else if (nActionStep == 30)
            {
                if (Common.ProC.TimerSignal[1])
                {
                    Common.ProC.Reset_T(1);
                    if (targetPos == Common.Data.endPos)
                    {
                        nActionStep = 40;

                        //Btn_Start.Enabled = true;
                        //Btn_Stop.Enabled = false;
                    }
                    else
                    {
                        nActionStep = 0;
                    }
                }
            }
            else if (nActionStep == 40)
            {
                Common.ProC.Card1.AbsoluteMove(0, 4, 0.01, 0.5, 0.5);
                nActionStep = 50;
            }
            else if (nActionStep == 50)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - 4) < 0.001)
                {
                    nActionStep = -1;
                    BackWorker.CancelAsync();
                }
            }
        }
        public void AutoCalibra()
        {
            if (nCalibraStep == 0)
            {
                Common.ShowMsgEvent($"Tips: 开始第{iCalibraIndex}点标定。。");
                //Common.ProC.MovePlaX(hv_PhysX.DArr[iCalibraIndex], MoveType.Absolute);
                Common.ProC.Card1.AbsoluteMove(1, hv_PhysX.DArr[iCalibraIndex], 0.1, 0.5, 0.5);
                Common.ProC.Card1.AbsoluteMove(2, hv_PhysX.DArr[iCalibraIndex], 0.1, 0.5, 0.5);
                Common.ProC.Card1.AbsoluteMove(3, hv_PhysY.DArr[iCalibraIndex], 0.1, 0.5, 0.5);
                nCalibraStep = 10;
            }
            else if (nCalibraStep == 10)
            {
                if (!Common.ProC.Card1.b_Move[1] && !Common.ProC.Card1.b_Move[2] && !Common.ProC.Card1.b_Move[3] && Math.Abs(hv_PhysY.DArr[iCalibraIndex] - Common.ProC.Card1.LogPos[3]) < 0.001 && Math.Abs(hv_PhysX.DArr[iCalibraIndex] - Common.ProC.Card1.LogPos[1]) < 0.001 && Math.Abs(hv_PhysX.DArr[iCalibraIndex] - Common.ProC.Card1.LogPos[2]) < 0.001)
                {
                    Common.ShowMsgEvent($"Tips:运动完成。。");
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
                    HTuple hv_Angle = null, hv_Len1 = null, hv_Len2 = null;
                    HTuple hv_ImageRow, hv_ImageCol, hv_Deg;
                    GetPositionAndDeg(displayBase1.SourceImage, out hv_Deg, out hv_ImageRow, out hv_ImageCol);

                    Common.ShowMsgEvent($"Tips:当前图像坐标 Row:{hv_ImageRow.D.ToString("0.000")},Col:{hv_ImageCol.D.ToString("0.000")}");
                    hv_ImageRows = hv_ImageRows.TupleConcat(hv_ImageRow);
                    hv_ImageCols = hv_ImageCols.TupleConcat(hv_ImageCol);
                    if (hv_ImageRows.Length == 9)
                    {
                        Common.HalObject.hom_Plat_ImageToAxis.VectorToHomMat2d(hv_ImageRows, hv_ImageCols, hv_PhysX, hv_PhysY);
                        Common.ShowMsgEvent("Tips: 九点标定完成");

                    }
                }
            }
        }
        public void AutoMatch()
        {
            double target1 = 0.0, target2 = 0.0, target3 = 0.0;
            if (nMatch == 0)
            {
                Common.Cam1.Snap();
                nMatch = 10;
            }
            else if (nMatch == 10)
            {
                if (b_FirstFinish)
                {
                    Common.ProC.MovePlaU(offsetDeg, MoveType.Relative);
                    nMatch = 20;
                }
            }
            else if (nMatch == 20)
            {
                if (!Common.ProC.Card1.b_Move[1] && !Common.ProC.Card1.b_Move[2] && !Common.ProC.Card1.b_Move[3])
                {
                    Common.ProC.Start_T(11, 200);
                    nMatch = 30;
                }
            }
            else if (nMatch == 30)
            {
                if (Common.ProC.TimerSignal[11])
                {
                    Common.ProC.Reset_T(11);
                    Common.Cam1.Snap();
                    nMatch = 40;
                }
            }
            else if (nMatch == 40)
            {
                if (b_SecondFinish)
                {
                    Common.ProC.Reset_T(12);
                    target1 = Common.ProC.Card1.LogPos[1] + offsetX;
                    target2 = Common.ProC.Card1.LogPos[2] + offsetX;
                    target3 = Common.ProC.Card1.LogPos[2] + offsetY;
                    //这里填入限位
                    Common.ProC.MovePlaX(offsetX, MoveType.Absolute);
                    nMatch = 50;
                }
            }
            else if (nMatch == 50)
            {
                if (!Common.ProC.Card1.b_Move[1] && !Common.ProC.Card1.b_Move[2] && Math.Abs(target1 - Common.ProC.Card1.LogPos[1]) < 0.001 && Math.Abs(target2 - Common.ProC.Card1.LogPos[2]) < 0.001)
                {
                    Common.ProC.Card1.RelativeMove(3, offsetY, 0.1, 0.5, 0.5);
                    nMatch = 60;
                }
            }
            else if (nMatch == 60)
            {
                if (!Common.ProC.Card1.b_Move[3] && Math.Abs(target3 - Common.ProC.Card1.LogPos[3]) < 0.001)
                {
                    //发完成信号
                    nMatch = -1;
                }
            }


            //第一次计算角度
            if (b_First)
            {
                if (Common.QueCam1.Count > 0)
                {
                    HObject ho_Image = Common.QueCam1.Dequeue();
                    displayBase1.DisplaySourceImage(ho_Image);

                    HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
                    HObject ho_Cross = new HObject(), ho_SortedRegions = new HObject(), ho_TempImage = new HObject(), ho_Region = new HObject();
                    HTuple hv_Area = null, hv_Rows = null, hv_Cols = null, hv_Angle = null, hv_Deg = null;
                    HOperatorSet.GenEmptyObj(out ho_TempImage);
                    HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
                    HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
                    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                    HOperatorSet.GenEmptyObj(out ho_SortedRegions);
                    HOperatorSet.GenEmptyObj(out ho_Region);

                    //HOperatorSet.MultImage(displayBase1.SourceImage, displayBase1.SourceImage, out ho_TempImage, 0.03, 0);
                    //HOperatorSet.CopyImage(displayBase1.SourceImage, out ho_TempImage);
                    HOperatorSet.GenRectangle1(out ho_Region, 0, 3000, 3648, 4300);
                    HOperatorSet.ReduceDomain(displayBase1.SourceImage, ho_Region, out ho_TempImage);
                    HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 180, 255);
                    HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
                    HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(10).TupleConcat(10), new HTuple(60).TupleConcat(60));
                    HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
                    HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Rows, out hv_Cols);
                    Common.HalObject.hv_PointRows = hv_Rows;
                    Common.HalObject.hv_PointCols = hv_Cols;

                    HOperatorSet.TupleRemove(Common.HalObject.hv_PointRows, Common.Data.nThrowIndex, out Common.HalObject.hv_PointRows);
                    HOperatorSet.TupleRemove(Common.HalObject.hv_PointCols, Common.Data.nThrowIndex, out Common.HalObject.hv_PointCols);
                    //选取片段
                    HOperatorSet.TupleSelectRange(Common.HalObject.hv_PointRows, 0, Common.HalObject.hv_PinRows.Length - 1, out Common.HalObject.hv_PointRows);
                    HOperatorSet.TupleSelectRange(Common.HalObject.hv_PointCols, 0, Common.HalObject.hv_PinRows.Length - 1, out Common.HalObject.hv_PointCols);
                    ////计算中心点
                    Common.Data.Base_PlatRow = (Common.HalObject.hv_PointRows.DArr[0] + Common.HalObject.hv_PointRows.DArr[Common.HalObject.hv_PointRows.Length - 1]) / 2;
                    Common.Data.Base_PlatCol = (Common.HalObject.hv_PointCols.DArr[0] + Common.HalObject.hv_PointCols.DArr[Common.HalObject.hv_PointRows.Length - 1]) / 2;
                    //计算偏移角度
                    HOperatorSet.AngleLx(Common.HalObject.hv_PointRows.DArr[Common.HalObject.hv_PointRows.Length - 1], Common.HalObject.hv_PointCols.DArr[Common.HalObject.hv_PointRows.Length - 1],
                        Common.HalObject.hv_PointRows.DArr[0], Common.HalObject.hv_PointCols.DArr[0], out hv_Angle);
                    HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);
                    offsetDeg = hv_Deg - Common.Data.Base_PlatDeg;

                    displayBase1.DisplaySourceImage(displayBase1.SourceImage);
                    HOperatorSet.GenCrossContourXld(out ho_Cross, Common.Data.Base_PlatRow, Common.Data.Base_PlatCol, 40, hv_Angle);
                    HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
                    displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
                    b_First = false;
                    b_FirstFinish = true;
                    b_Second = true;

                }
            }

            if (b_Second)
            {
                if (Common.QueCam1.Count > 0)
                {
                    HObject ho_Image = Common.QueCam1.Dequeue();
                    displayBase1.DisplaySourceImage(ho_Image);

                    HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
                    HObject ho_Cross = new HObject(), ho_SortedRegions = new HObject(), ho_TempImage = new HObject(), ho_Region = new HObject();
                    HTuple hv_Area = null, hv_Rows = null, hv_Cols = null, hv_Angle = null, hv_Deg = null;
                    HTuple hv_TempCenterImageRow = null, hv_TempCenterImageCol = null, hv_TempX = null, hv_TempY = null, hv_BaseX = null, hv_BaseY = null;
                    HOperatorSet.GenEmptyObj(out ho_TempImage);
                    HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
                    HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
                    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                    HOperatorSet.GenEmptyObj(out ho_SortedRegions);
                    HOperatorSet.GenEmptyObj(out ho_Region);

                    //HOperatorSet.MultImage(displayBase1.SourceImage, displayBase1.SourceImage, out ho_TempImage, 0.03, 0);
                    //HOperatorSet.CopyImage(displayBase1.SourceImage, out ho_TempImage);
                    HOperatorSet.GenRectangle1(out ho_Region, 0, 3000, 3648, 4300);
                    HOperatorSet.ReduceDomain(displayBase1.SourceImage, ho_Region, out ho_TempImage);
                    HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 180, 255);
                    HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
                    HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(10).TupleConcat(10), new HTuple(60).TupleConcat(60));
                    HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
                    HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Rows, out hv_Cols);
                    Common.HalObject.hv_PointRows = hv_Rows;
                    Common.HalObject.hv_PointCols = hv_Cols;

                    HOperatorSet.TupleRemove(Common.HalObject.hv_PointRows, Common.Data.nThrowIndex, out Common.HalObject.hv_PointRows);
                    HOperatorSet.TupleRemove(Common.HalObject.hv_PointCols, Common.Data.nThrowIndex, out Common.HalObject.hv_PointCols);
                    //选取片段
                    HOperatorSet.TupleSelectRange(Common.HalObject.hv_PointRows, 0, Common.HalObject.hv_PinRows.Length - 1, out Common.HalObject.hv_PointRows);
                    HOperatorSet.TupleSelectRange(Common.HalObject.hv_PointCols, 0, Common.HalObject.hv_PinRows.Length - 1, out Common.HalObject.hv_PointCols);
                    ////计算中心点
                    hv_TempCenterImageRow = (Common.HalObject.hv_PointRows.DArr[0] + Common.HalObject.hv_PointRows.DArr[Common.HalObject.hv_PointRows.Length - 1]) / 2;
                    hv_TempCenterImageCol = (Common.HalObject.hv_PointCols.DArr[0] + Common.HalObject.hv_PointCols.DArr[Common.HalObject.hv_PointRows.Length - 1]) / 2;
                    //计算偏移
                    Common.HalObject.hom_Plat_ImageToAxis.AffineTransPixel(Common.Data.Base_PlatRow, Common.Data.Base_PlatCol, out hv_BaseX, out hv_BaseY);
                    Common.HalObject.hom_Plat_ImageToAxis.AffineTransPixel(hv_TempCenterImageRow, hv_TempCenterImageCol, out hv_TempX, out hv_TempY);
                    offsetX = hv_TempX - hv_BaseX;
                    offsetY = hv_TempY - hv_BaseY;
                    //计算偏移角度
                    displayBase1.DisplaySourceImage(displayBase1.SourceImage);
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_TempCenterImageRow, hv_TempCenterImageCol, 40, hv_Angle);
                    HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
                    displayBase1.AddRegion(new LeoBase.HsbRegions() { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
                    b_Second = false;
                    b_SecondFinish = true;
                }
            }
        }
        private void BackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(20);
                Common.ProC.Control_T();
                Common.ProC.Card1.GetCurrentInf();
                if (BackWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                //自动对焦
                AutoFocus();
                if (b_Calibra)
                {
                    //九点标定
                    AutoCalibra();
                }
                //自动对产品
                AutoMatch();
            }
        }

        private void BackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int n_totalStep = Convert.ToInt32((Common.Data.endPos - Common.Data.startPos) / Common.Data.stepPos) + 1;
            label33.Text = $"拍照进度： {e.ProgressPercentage}/{n_totalStep}";
        }




        double platDeg = 0.0;
        private void BTN_Move(object sender, EventArgs e)
        {
            ButtonX btx = sender as ButtonX;
            btx.Enabled = false;
            HTuple hv_rad = new HTuple(platStep).TupleRad();
            switch (btx.Name)
            {
                case "MOVE_PX":
                    Common.ProC.MovePlaX(platStep, MoveType.Relative);
                    break;
                case "MOVE_NX":
                    Common.ProC.MovePlaX(platStep * -1, MoveType.Relative);
                    break;
                case "MOVE_PY":
                    Common.ProC.Card1.AxisPmove(3, platStep);
                    break;
                case "MOVE_NY":
                    Common.ProC.Card1.AxisPmove(3, platStep * -1);
                    break;
                case "MOVE_PU":
                    Common.ProC.MovePlaU(platStep, MoveType.Relative);
                    platDeg += platStep;
                    TBox_Plat_Deg.Text = platDeg.ToString("0.000");
                    break;
                case "MOVE_NU":
                    Common.ProC.MovePlaU(platStep * -1, MoveType.Relative);
                    platDeg -= platStep;
                    TBox_Plat_Deg.Text = platDeg.ToString("0.000");
                    break;
                case "MOVE_PZ":
                    Common.ProC.Card1.AxisPmove(0, zStep);
                    break;
                case "MOVE_NZ":
                    Common.ProC.Card1.AxisPmove(0, zStep * -1);
                    break;
                default:
                    break;
            }
            btx.Enabled = true;
        }

        double platStep = 0.005, zStep = 0.005;

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                zStep = double.Parse(textBox2.Text);
            }
            catch (Exception)
            {
                textBox2.Text = "0.005";
            }
        }

        #region 相机标定


        private void button9_Click(object sender, EventArgs e)
        {
            HObject ho_Cross = new HObject();
            HOperatorSet.GenEmptyRegion(out ho_Cross);

            displayBase1.DisplaySourceImage(displayBase1.SourceImage);
            HOperatorSet.GenCrossContourXld(out ho_Cross, Common.HalObject.hv_PointRows, Common.HalObject.hv_PointCols, 15, 0);
            HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
            displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });

            Common.HalObject.hom_ImageBToImageA.VectorToHomMat2d(Common.HalObject.hv_PinRows, Common.HalObject.hv_PinCols, Common.HalObject.hv_PointRows, Common.HalObject.hv_PointCols);
        }

        //计算针尖
        private void button1_Click(object sender, EventArgs e)
        {
            HObject ho_Cross = new HObject();
            HOperatorSet.GenEmptyRegion(out ho_Cross);
            HTuple hv_TranRows = null, hv_TransCols = null;

            Common.HalObject.hom_ImageBToImageA.AffineTransPixel(Common.HalObject.hv_PinRows, Common.HalObject.hv_PinCols, out hv_TranRows, out hv_TransCols);
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_TranRows, hv_TransCols, 15, 0);
            HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
            displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });

        }


        public bool GetLastPin(HObject ho_Image, out HTuple hv_LastRow, out HTuple hv_LastCol)
        {
            HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
            HObject ho_Cross = new HObject(), ho_SortedRegions = new HObject(), ho_TempImage = new HObject();
            HTuple hv_Area = null, hv_Rows = null, hv_Cols = null;
            HOperatorSet.GenEmptyObj(out ho_TempImage);
            HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
            HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);

            HOperatorSet.MultImage(ho_Image, ho_Image, out ho_TempImage, 0.03, 0);
            HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 100, 255);
            HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
            HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(20).TupleConcat(20), new HTuple(60).TupleConcat(60));
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Rows, out hv_Cols, out hv_Area);

            if (hv_Rows.Length > 0)
            {
                hv_LastRow = hv_Rows.DArr[hv_Rows.Length - 1];
                hv_LastCol = hv_Cols.DArr[hv_Rows.Length - 1];
                HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_LastRow, hv_LastCol, 15, 0);
                //hv_PinRows = hv_Rows;
                //hv_PinCols = hv_Cols;
                displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
                return true;
            }
            else
            {
                hv_LastRow = null;
                hv_LastCol = null;
                return false;
            }
        }

        public bool GetFirstPin(HObject ho_Image, out HTuple hv_FirstRow, out HTuple hv_FirstCol)
        {
            HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
            HObject ho_Cross = new HObject(), ho_SortedRegions = new HObject(), ho_TempImage = new HObject();
            HTuple hv_Area = null, hv_Rows = null, hv_Cols = null;
            HOperatorSet.GenEmptyObj(out ho_TempImage);
            HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
            HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);

            HOperatorSet.MultImage(ho_Image, ho_Image, out ho_TempImage, 0.03, 0);
            HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 100, 255);
            HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
            HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(20).TupleConcat(20), new HTuple(60).TupleConcat(60));
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Rows, out hv_Cols, out hv_Area);

            if (hv_Rows.Length > 0)
            {
                hv_FirstRow = hv_Rows.DArr[0];
                hv_FirstCol = hv_Cols.DArr[0];
                HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_FirstRow, hv_FirstCol, 15, 0);
                //hv_PinRows = hv_Rows;
                //hv_PinCols = hv_Cols;
                displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
                return true;
            }
            else
            {
                hv_FirstRow = null;
                hv_FirstCol = null;
                return false;
            }
        }


        //反推焊点
        private void button4_Click(object sender, EventArgs e)
        {
            HObject ho_Cross = new HObject();
            HOperatorSet.GenEmptyRegion(out ho_Cross);
            HTuple hv_TranRows = null, hv_TransCols = null;
            HHomMat2D hom_Temp = Common.HalObject.hom_ImageBToImageA.HomMat2dInvert();
            hom_Temp.AffineTransPixel(Common.HalObject.hv_PointRows, Common.HalObject.hv_PointCols, out hv_TranRows, out hv_TransCols);
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_TranRows, hv_TransCols, 15, 0);
            HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
            displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
        }

        private void button10_Click(object sender, EventArgs e)
        {
            HObject ho_FocusRegion = new HObject();
            HOperatorSet.GenEmptyObj(out ho_FocusRegion);

            HTuple hv_Row1, hv_Col1, hv_Row2, hv_Col2;
            displayBase1.InteractiveEnable = false;
            HOperatorSet.DrawRectangle1(displayBase1.DisplayHandle, out hv_Row1, out hv_Col1, out hv_Row2, out hv_Col2);
            HOperatorSet.GenRectangle1(out ho_FocusRegion, hv_Row1, hv_Col1, hv_Row2, hv_Col2);
            HOperatorSet.DispObj(ho_FocusRegion, displayBase1.DisplayHandle);
            Common.HalObject.ho_FocusRegion = new HRegion(ho_FocusRegion);
            displayBase1.AddRegion(new LeoBase.HsbRegions() { new LeoBase.HsbRegion() { Value = ho_FocusRegion, Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin } });
            displayBase1.InteractiveEnable = true;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Common.ProC.Card1.StopRun(0, 0);
            Common.ProC.Card1.StopRun(1, 0);
            Common.ProC.Card1.StopRun(2, 0);
            Common.ProC.Card1.StopRun(3, 0);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            //去掉空点
            string[] ThrowList = textBox10.Text.Split(',');
            Common.Data.nThrowIndex = Array.ConvertAll<string, int>(ThrowList, delegate (string s) { return int.Parse(s); });
        }

        private void Tbox_BaseHeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                string result = JsonConvert.SerializeObject(Common.Data);
                if (!Directory.Exists($"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\"))
                {
                    Directory.CreateDirectory($"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\");
                }
                string fp = $"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\RunningData.json";
                if (!File.Exists(fp))  // 判断是否已有相同文件 
                {
                    FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                    fs1.Close();
                }
                File.WriteAllText(fp, result.Replace(",\"", ",\r\n\""));

                FileStream fs = new FileStream($"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\RunningData.hol", FileMode.Create, FileAccess.Write);
                using (fs)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, Common.HalObject);
                }
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败！原因为:{ex.Message}");
            }
        }

        private void MOVE_HOME_Click(object sender, EventArgs e)
        {
            Common.ProC.AllAxisGohome();
            platDeg = 0.0;
            TBox_Plat_Deg.Text = platDeg.ToString("0.000");
        }


        double[] RelOffsetX = new double[9];

        private void button3_Click(object sender, EventArgs e)
        {
            HObject ho_Cross = new HObject();
            displayBase1.DisplaySourceImage(displayBase1.SourceImage);
            HOperatorSet.GenCrossContourXld(out ho_Cross, Common.HalObject.FirstPinCalibra.ImageRows, Common.HalObject.FirstPinCalibra.ImageCols, 40, new HTuple(0));

            displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
            for (int i = 0; i < Common.HalObject.FirstPinCalibra.ImageRows.Length; i++)
            {
                HOperatorSet.SetTposition(displayBase1.DisplayHandle, Common.HalObject.FirstPinCalibra.ImageRows[i] + 20, Common.HalObject.FirstPinCalibra.ImageCols[i]);
                HOperatorSet.WriteString(displayBase1.DisplayHandle, i.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HObject ho_Cross = new HObject();
            displayBase1.DisplaySourceImage(displayBase1.SourceImage);
            HTuple hv_Rows, hv_Cols;
            double CenterX, CenterY;

            HHomMat2D hom = Common.HalObject.FirstPinCalibra.HomMat2D.HomMat2dInvert();
            hom.AffineTransPixel(Common.HalObject.FirstPinCalibra.XposList, Common.HalObject.FirstPinCalibra.YposList, out hv_Rows, out hv_Cols);
            Common.HalObject.FirstPinCalibra.ImageToAxis(Common.HalObject.FirstPinCalibra.ImageRows[4], Common.HalObject.FirstPinCalibra.ImageCols[4], out CenterX, out CenterY);
            Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->图像中心 X轴：[{CenterX.ToString("0.000")}],Y轴：[{ CenterY.ToString("0.000")}]--");

            for (int i = 0; i < Common.HalObject.FirstPinCalibra.ImageRows.Length; i++)
            {
                HOperatorSet.SetTposition(displayBase1.DisplayHandle, hv_Rows[i] + 20, hv_Cols[i]);
                HOperatorSet.WriteString(displayBase1.DisplayHandle, i.ToString());
            }

            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Rows, hv_Cols, 40, new HTuple(0));

            displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { ColorNum=12, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            HTuple hv_LastRow, hv_LastCol;
            HObject ho_Cross = new HObject();
            displayBase1.DisplaySourceImage(displayBase1.SourceImage);

            double Xpos, Ypos;
            GetLastPin(displayBase1.SourceImage, out hv_LastRow, out hv_LastCol);
            Common.HalObject.FirstPinCalibra.ImageToAxis(hv_LastRow, hv_LastCol, out Xpos, out Ypos);
            Common.Data.Base_FirstPinX = Xpos;
            Common.Data.Base_FirstPinY = Ypos;
            Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准1 X轴：[{ Common.Data.Base_FirstPinX.ToString("0.000")}],Y轴：[{ Common.Data.Base_FirstPinY.ToString("0.000")}]--");
        }

        private void button12_Click(object sender, EventArgs e)
        {

            HObject ho_Cross = new HObject();
            displayBase1.DisplaySourceImage(displayBase1.SourceImage);
            HTuple hv_FirstRow, hv_FirstCol, hv_Angle, hv_Deg;
            double Xpos, Ypos;
            GetFirstPin(displayBase1.SourceImage, out hv_FirstRow, out hv_FirstCol);
            Common.HalObject.FirstPinCalibra.ImageToAxis(hv_FirstRow, hv_FirstCol, out Xpos, out Ypos);

            //计算第二点的位置
            Common.Data.Base_LastPinX = (Common.Data.Base_Grab2X - Common.Data.Base_Grab1X) + Xpos;
            Common.Data.Base_LastPinY = Ypos + (Common.Data.Base_Grab2Y - Common.Data.Base_Grab1Y);
            Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准2 X轴：[{ Common.Data.Base_LastPinX.ToString("0.000")}],Y轴：[{ Common.Data.Base_LastPinY.ToString("0.000")}]--");

            //计算角度
            HOperatorSet.AngleLl(Common.Data.Base_LastPinX, Common.Data.Base_LastPinY, Common.Data.Base_FirstPinX, Common.Data.Base_FirstPinY,
               Common.Data.Base_Grab1X, Common.Data.Base_Grab1Y, Common.Data.Base_Grab2X, Common.Data.Base_Grab2Y, out hv_Angle);
            HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);
            Common.Data.Base_PinDeg = hv_Deg;
            Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准针卡角度：[{  Common.Data.Base_PinDeg.ToString("0.000")}]--");

        }

        private void button13_Click(object sender, EventArgs e)
        {
            HTuple hv_width, hv_height;
            HOperatorSet.GetImageSize(displayBase1.SourceImage, out hv_width, out hv_height);
            for (int i = 0; i < Common.HalObject.FirstPinCalibra.ImageRows.Length; i++)
            {
                Common.HalObject.FirstPinCalibra.ImageRows[i] += hv_height / 2;
                Common.HalObject.FirstPinCalibra.ImageCols[i] += hv_width / 2;
            }
            Common.HalObject.FirstPinCalibra.GenHomMat2D();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            if (displayBase1.SourceImage != null ? displayBase1.SourceImage.IsInitialized() : false)
            {
                HTuple hv_BaseX, hv_BaseY;
                HTuple hv_Deg2, hv_CenterRow2, hv_CenterCol12;
                GetPositionAndDeg(displayBase1.SourceImage, out hv_Deg2, out hv_CenterRow2, out hv_CenterCol12);

                Common.HalObject.hom_Plat_ImageToAxis.AffineTransPixel(hv_CenterRow2, hv_CenterCol12, out hv_BaseX, out hv_BaseY);
                Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准芯片X：[{hv_BaseX.D.ToString("0.000")}]，Y:[{hv_BaseY.D.ToString("0.000")}]--");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            HObject ho_Cross = new HObject();
            displayBase1.DisplaySourceImage(displayBase1.SourceImage);
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ImageRows, hv_ImageCols, 40, new HTuple(0));

            displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross } });
            for (int i = 0; i < Common.HalObject.FirstPinCalibra.ImageRows.Length; i++)
            {
                HOperatorSet.SetTposition(displayBase1.DisplayHandle, hv_ImageRows[i] + 20, hv_ImageCols[i]);
                HOperatorSet.WriteString(displayBase1.DisplayHandle, i.ToString());
            }
        }

        double[] RelOffsetY = new double[9];
        private void Btn_Start9Point_Click(object sender, EventArgs e)
        {
            hv_PhysX = new HTuple(-1 * dOffset, -1 * dOffset, -1 * dOffset, 0, 0, 0, dOffset, dOffset, dOffset);
            hv_PhysY = new HTuple(-1 * dOffset, 0, dOffset, dOffset, 0, -1 * dOffset, -1 * dOffset, 0, dOffset);

            hv_ImageRows = new HTuple();
            hv_ImageCols = new HTuple();
            b_Calibra = true;
            nCalibraStep = 0;
            BackWorker.RunWorkerAsync();
        }

        //private void GetPositionAndDeg(HObject ho_Image, out HTuple hv_Deg, out HTuple hv_ImageRow, out HTuple hv_ImageCol)
        //{
        //    HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
        //    HObject ho_Cross = new HObject(), ho_Rectangle2 = new HObject(), ho_TempImage = new HObject(), ho_Region = new HObject();
        //    HTuple hv_Angle = null, hv_Len1 = null, hv_Len2 = null;
        //    HOperatorSet.GenEmptyObj(out ho_TempImage);
        //    HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_Rectangle2);
        //    HOperatorSet.GenEmptyObj(out ho_Region);

        //    HOperatorSet.MultImage(ho_Image, ho_Image, out ho_TempImage, 0.4, 0);
        //    //HOperatorSet.CopyImage(displayBase1.SourceImage, out ho_TempImage);
        //    HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 200, 255);
        //    HOperatorSet.FillUp(ho_ThresholdRegion, out ho_ThresholdRegion);
        //    HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
        //    HOperatorSet.SelectShapeStd(ho_ConnectionRegion, out ho_SelectedRegions, "max_area", new HTuple(80));
        //    HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_ImageRow, out hv_ImageCol, out hv_Angle, out hv_Len1, out hv_Len2);
        //    HOperatorSet.GenRectangle2(out ho_Rectangle2, hv_ImageRow, hv_ImageCol, hv_Angle, hv_Len1, hv_Len2);
        //    HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);
        //    ////计算中心点
        //    displayBase1.DisplaySourceImage(displayBase1.SourceImage);
        //    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ImageRow, hv_ImageCol, 140, hv_Angle);
        //    HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
        //    displayBase1.AddRegion(new LeoBase.HsbRegions()
        //            { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross },
        //             new LeoBase.HsbRegion() {Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value = ho_Rectangle2  } });
        //}

        private void GetPositionAndDeg(HObject ho_Image, out HTuple hv_Deg, out HTuple hv_ImageRow, out HTuple hv_ImageCol)
        {
            // Local iconic variables 

            HObject ho_Cross, ho_Region, ho_RegionFillUp;
            HObject ho_ConnectedRegions, ho_SelectedRegions, ho_Regions1;
            HObject ho_Line1, ho_Regions2, ho_Line2, ho_Regions3, ho_Line3;
            HObject ho_Regions4, ho_Line4, ho_ObjectsConcat, ho_UnionContours;
            HObject ho_XLDTrans;

            // Local control variables 

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_CornerY = new HTuple();
            HTuple hv_CornerX = new HTuple(), hv_LineCenterY = new HTuple();
            HTuple hv_LineCenterX = new HTuple(), hv_ResultRow1 = new HTuple();
            HTuple hv_ResultColumn1 = new HTuple(), hv_LRow_11 = new HTuple();
            HTuple hv_LCol_11 = new HTuple(), hv_LRow_12 = new HTuple();
            HTuple hv_LCol_12 = new HTuple(), hv_ResultRow2 = new HTuple();
            HTuple hv_ResultColumn2 = new HTuple(), hv_LRow_21 = new HTuple();
            HTuple hv_LCol_21 = new HTuple(), hv_LRow_22 = new HTuple();
            HTuple hv_LCol_22 = new HTuple(), hv_ResultRow3 = new HTuple();
            HTuple hv_ResultColumn3 = new HTuple(), hv_LRow_31 = new HTuple();
            HTuple hv_LCol_31 = new HTuple(), hv_LRow_32 = new HTuple();
            HTuple hv_LCol_32 = new HTuple(), hv_ResultRow4 = new HTuple();
            HTuple hv_ResultColumn4 = new HTuple(), hv_LRow_41 = new HTuple();
            HTuple hv_LCol_41 = new HTuple(), hv_LRow_42 = new HTuple();
            HTuple hv_LCol_42 = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Phi1 = new HTuple();
            HTuple hv_Length11 = new HTuple(), hv_Length21 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            HOperatorSet.GenEmptyObj(out ho_Line1);
            HOperatorSet.GenEmptyObj(out ho_Regions2);
            HOperatorSet.GenEmptyObj(out ho_Line2);
            HOperatorSet.GenEmptyObj(out ho_Regions3);
            HOperatorSet.GenEmptyObj(out ho_Line3);
            HOperatorSet.GenEmptyObj(out ho_Regions4);
            HOperatorSet.GenEmptyObj(out ho_Line4);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_UnionContours);
            HOperatorSet.GenEmptyObj(out ho_XLDTrans);



            HOperatorSet.Threshold(ho_Image, out ho_Region, 128, 255);
            HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
            HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area", 70);
            HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);
            Common.get_rectangle2_points(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2, out hv_CornerY,
                 out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);

            Common.rake(ho_Image, out ho_Regions1, 50, 60, 30, 1, 20, "all", "max", hv_CornerY.TupleSelect(0), hv_CornerX.TupleSelect(0), hv_CornerY.TupleSelect(1),
                hv_CornerX.TupleSelect(1), out hv_ResultRow1, out hv_ResultColumn1);
            Common.pts_to_best_line(out ho_Line1, hv_ResultRow1, hv_ResultColumn1, 30, out hv_LRow_11, out hv_LCol_11, out hv_LRow_12, out hv_LCol_12);

            Common.rake(ho_Image, out ho_Regions2, 50, 60, 30, 1, 20, "all", "max", hv_CornerY.TupleSelect(1), hv_CornerX.TupleSelect(1), hv_CornerY.TupleSelect(2),
                hv_CornerX.TupleSelect(2), out hv_ResultRow2, out hv_ResultColumn2);
            Common.pts_to_best_line(out ho_Line2, hv_ResultRow2, hv_ResultColumn2, 30, out hv_LRow_21, out hv_LCol_21, out hv_LRow_22, out hv_LCol_22);

            Common.rake(ho_Image, out ho_Regions3, 50, 60, 30, 1, 20, "all", "max", hv_CornerY.TupleSelect(2), hv_CornerX.TupleSelect(2), hv_CornerY.TupleSelect(3),
                hv_CornerX.TupleSelect(3), out hv_ResultRow3, out hv_ResultColumn3);
            Common.pts_to_best_line(out ho_Line3, hv_ResultRow3, hv_ResultColumn3, 30, out hv_LRow_31, out hv_LCol_31, out hv_LRow_32, out hv_LCol_32);

            Common.rake(ho_Image, out ho_Regions4, 50, 60, 30, 1, 20, "all", "max", hv_CornerY.TupleSelect(3), hv_CornerX.TupleSelect(3), hv_CornerY.TupleSelect(0),
                hv_CornerX.TupleSelect(0), out hv_ResultRow4, out hv_ResultColumn4);
            Common.pts_to_best_line(out ho_Line4, hv_ResultRow4, hv_ResultColumn4, 30, out hv_LRow_41, out hv_LCol_41, out hv_LRow_42, out hv_LCol_42);


            HOperatorSet.ConcatObj(ho_Line1, ho_Line2, out ho_ObjectsConcat);
            HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Line3, out ho_ObjectsConcat);
            HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Line4, out ho_ObjectsConcat);

            HOperatorSet.UnionAdjacentContoursXld(ho_ObjectsConcat, out ho_UnionContours, 40, 5, "attr_keep");
            HOperatorSet.ShapeTransXld(ho_UnionContours, out ho_XLDTrans, "rectangle2");
            HOperatorSet.SmallestRectangle2Xld(ho_XLDTrans, out hv_ImageRow, out hv_ImageCol, out hv_Phi1, out hv_Length11, out hv_Length21);
            HOperatorSet.TupleDeg(hv_Phi1, out hv_Deg);
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ImageRow, hv_ImageCol, 140, hv_Phi1);
            HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 1);
            displayBase1.AddRegion(new LeoBase.HsbRegions()
                        {   new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross },
                            new LeoBase.HsbRegion() {Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value =ho_Line1},
                            new LeoBase.HsbRegion() {Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value =ho_Line2},
                            new LeoBase.HsbRegion() {Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value =ho_Line3},
                            new LeoBase.HsbRegion() {Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value =ho_Line4}});
            Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准芯片 长：[{(hv_Length11.D*2).ToString("0.000")}]，宽:[{(hv_Length21.D * 2).ToString("0.000")}]--");

        }

        //获取平台基准
        private void button7_Click(object sender, EventArgs e)
        {
            if (displayBase1.SourceImage != null ? displayBase1.SourceImage.IsInitialized() : false)
            {
                HTuple hv_BaseX, hv_BaseY;
                HTuple hv_Deg2, hv_CenterRow2, hv_CenterCol12;
                GetPositionAndDeg(displayBase1.SourceImage, out hv_Deg2, out hv_CenterRow2, out hv_CenterCol12);



                Common.Data.Base_PlatDeg = hv_Deg2;
                Common.Data.Base_PlatRow = hv_CenterRow2;
                Common.Data.Base_PlatCol = hv_CenterCol12;
                Base_PlatRow.Text = Common.Data.Base_PlatRow.ToString("0.000");
                Base_PlatCol.Text = Common.Data.Base_PlatCol.ToString("0.000");
                Base_PlatDeg.Text = Common.Data.Base_PlatDeg.ToString("0.000");
                Common.HalObject.hom_Plat_ImageToAxis.AffineTransPixel(Common.Data.Base_PlatRow, Common.Data.Base_PlatCol, out hv_BaseX, out hv_BaseY);
                Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准芯片X：[{hv_BaseX.D.ToString("0.000")}]，Y:[{hv_BaseY.D.ToString("0.000")}]--");
            }
        }

        //获取针尖
        private void button5_Click(object sender, EventArgs e)
        {
            if (displayBase1.SourceImage != null ? displayBase1.SourceImage.IsInitialized() : false)
            {
                HTuple hv_PinRows, hv_PinCols;
                GetPins(displayBase1.SourceImage, out hv_PinRows, out hv_PinCols);
                Common.HalObject.hv_PinRows = hv_PinRows;
                Common.HalObject.hv_PinCols = hv_PinCols;
            }
        }

        private void GetPins(HObject ho_Image, out HTuple hv_PinRows, out HTuple hv_PinCols)
        {
            HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
            HObject ho_Cross = new HObject(), ho_SortedRegions = new HObject(), ho_TempImage = new HObject();
            HTuple hv_Area = null, hv_Rows = null, hv_Cols = null;
            HOperatorSet.GenEmptyObj(out ho_TempImage);
            HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
            HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);

            HOperatorSet.MultImage(ho_Image, ho_Image, out ho_TempImage, 0.03, 0);
            HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 80, 255);
            HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
            HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "or", new HTuple(30).TupleConcat(30), new HTuple(60).TupleConcat(60));
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
            HOperatorSet.InnerCircle(ho_SortedRegions, out hv_Rows, out hv_Cols, out hv_Area);

            HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
            HOperatorSet.TupleInverse(hv_Rows, out hv_Rows);
            HOperatorSet.TupleInverse(hv_Cols, out hv_Cols);
            HOperatorSet.TupleSelectRange(hv_Rows, 0, 5, out hv_PinRows);
            HOperatorSet.TupleSelectRange(hv_Cols, 0, 5, out hv_PinCols);

            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_PinRows, hv_PinCols, 15, 0);
            //hv_PinRows = hv_Rows;
            //hv_PinCols = hv_Cols;
            displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { ColorNum =6, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross },
                      new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value = ho_SortedRegions } });
        }

        //获取焊点
        private void button8_Click(object sender, EventArgs e)
        {
            if (displayBase1.SourceImage != null ? displayBase1.SourceImage.IsInitialized() : false)
            {
                HObject ho_ThresholdRegion = new HObject(), ho_ConnectionRegion = new HObject(), ho_SelectedRegions = new HObject();
                HObject ho_Cross = new HObject(), ho_SortedRegions = new HObject(), ho_TempImage = new HObject(), ho_Region = new HObject();
                HTuple hv_Area = null, hv_Rows = null, hv_Cols = null;
                HOperatorSet.GenEmptyObj(out ho_TempImage);
                HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
                HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SortedRegions);
                HOperatorSet.GenEmptyObj(out ho_Region);

                //HOperatorSet.MultImage(displayBase1.SourceImage, displayBase1.SourceImage, out ho_TempImage, 0.03, 0);
                //HOperatorSet.CopyImage(displayBase1.SourceImage, out ho_TempImage);
                HOperatorSet.GenRectangle1(out ho_Region, 0, 3000, 3648, 4300);
                HOperatorSet.ReduceDomain(displayBase1.SourceImage, ho_Region, out ho_TempImage);
                HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 180, 255);
                HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
                HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(10).TupleConcat(10), new HTuple(60).TupleConcat(60));
                HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
                HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Rows, out hv_Cols);
                Common.HalObject.hv_PointRows = hv_Rows;
                Common.HalObject.hv_PointCols = hv_Cols;

                //HOperatorSet.TupleInverse(Common.HalObject.hv_PointRows, out Common.HalObject.hv_PointRows);
                //HOperatorSet.TupleInverse(Common.HalObject.hv_PointCols, out Common.HalObject.hv_PointCols);

                HOperatorSet.TupleRemove(Common.HalObject.hv_PointRows, Common.Data.nThrowIndex, out Common.HalObject.hv_PointRows);
                HOperatorSet.TupleRemove(Common.HalObject.hv_PointCols, Common.Data.nThrowIndex, out Common.HalObject.hv_PointCols);
                //选取片段
                HOperatorSet.TupleSelectRange(Common.HalObject.hv_PointRows, 0, Common.HalObject.hv_PinRows.Length - 1, out Common.HalObject.hv_PointRows);
                HOperatorSet.TupleSelectRange(Common.HalObject.hv_PointCols, 0, Common.HalObject.hv_PinRows.Length - 1, out Common.HalObject.hv_PointCols);

                //HOperatorSet.TupleInverse(hv_PointRows, out hv_PointRows);
                //HOperatorSet.TupleInverse(hv_PointCols, out hv_PointCols);
                HOperatorSet.GenCrossContourXld(out ho_Cross, Common.HalObject.hv_PointRows, Common.HalObject.hv_PointCols, 15, 0);
                HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
                displayBase1.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross },
                      new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value = ho_SortedRegions } });
            }
        }

        #endregion
        private void Plat_Step_TextChanged(object sender, EventArgs e)
        {
            try
            {
                platStep = double.Parse(Plat_Step.Text);
            }
            catch (Exception)
            {
                Plat_Step.Text = "0.005";
            }
        }



        private void Tbox__TextChanged(object sender, EventArgs e)
        {
            TextBox tbox = sender as TextBox;
            try
            {
                switch (tbox.Name)
                {
                    case "Tbox_StartPos":
                        Common.Data.startPos = double.Parse(tbox.Text);
                        break;
                    case "Tbox_EndPos":
                        Common.Data.endPos = double.Parse(tbox.Text);
                        break;
                    case "Tbox_Step":
                        Common.Data.stepPos = double.Parse(tbox.Text);
                        break;
                    case "Tbox_CenterX1":
                        dCenterX1 = double.Parse(tbox.Text);
                        break;
                    case "Tbox_CenterX2":
                        dCenterX2 = double.Parse(tbox.Text);
                        break;
                    case "Tbox_CenterY":
                        dCenterY = double.Parse(tbox.Text);
                        break;
                    case "Tbox_Offset":
                        dOffset = double.Parse(tbox.Text);
                        break;
                    case "Tbox_BaseHeight":
                        Common.Data.Base_Height = double.Parse(tbox.Text);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                tbox.Text = "0.0";
            }
        }


        private void button15_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择存图路径";
            fbd.ShowNewFolderButton = true;
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox21.Text = fbd.SelectedPath;
                Common.Data.str_SavePath = fbd.SelectedPath;
            }
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox21.Text))
            {
                textBox21.ForeColor = Color.Blue;
                Common.Data.str_SavePath = textBox21.Text;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(textBox21.Text);
                    Common.Data.str_SavePath = textBox21.Text;
                    textBox21.ForeColor = Color.Blue;
                }
                catch (Exception)
                {
                    textBox21.ForeColor = Color.Red;
                }
            }
        }
        #endregion
    }
}