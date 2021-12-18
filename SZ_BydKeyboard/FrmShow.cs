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
using HalconDotNet;
using LeoBase;
using LeoMotion;
using DevComponents.AdvTree;
using System.IO;
using alg_KeyBoard_BYD;
using System.Threading;
using DevComponents.DotNetBar;

namespace SZ_BydKeyboard
{
    public partial class FrmShow : DockContent
    {
        public FrmShow()
        {
            InitializeComponent();
        }

        private void FrmShow_Load(object sender, EventArgs e)
        {
            Common.ShowDataCode = ShowData;
            Common.ShowProductType = showProductType;
            Common.SetB2B = SetB2BStyle;
            InitShowNodes();

        }

        public void ShowInitImage()
        {
            HObject ho_BackImage = new HObject();
            HOperatorSet.ReadImage(out ho_BackImage, $"{Application.StartupPath}\\SysCfg\\Init.bmp");
            displayBase1.DisplaySourceImage(ho_BackImage);
        }

        private void ShowData(string str)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Common.ShowMsg(ShowData), str);
                return;
            }
            labelX2.Text = $"当前产品DataCode：{str}";
        }

        private void showProductType(string str)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Common.ShowMsg(showProductType), str);
                return;
            }
            comboBox1.SelectedItem = str;
        }

        private void InitShowNodes()
        {
            advTree1.Nodes.Clear();
            for (int i = 0; i < Common.FaiNames.Length; i++)
            {
                advTree1.Nodes.Add(new Node(Common.FaiNames[i], new DevComponents.DotNetBar.ElementStyle(Color.White, Color.DarkGray)));
            }
        }

        public void UpdateResult(bool[] ResultList)
        {
            for (int i = 0; i < advTree1.Nodes.Count; i++)
            {
                if (ResultList[i])
                {
                    advTree1.Nodes[i].Style = new DevComponents.DotNetBar.ElementStyle(Color.White, Color.Green);
                }
                else
                {
                    advTree1.Nodes[i].Style = new DevComponents.DotNetBar.ElementStyle(Color.White, Color.Red);
                }
            }
        }


        public class InspectInfo
        {
            public int index;
            public HObject ho_Image;
            public int nfix;
        }

        public void SetB2BStyle(bool bTrue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Common.DelegateBool(SetB2BStyle), bTrue);
                return;
            }
            if (bTrue)
            {
                advTree1.Nodes[8].Style = new ElementStyle(Color.White, Color.Green);
            }
            else
            {
                advTree1.Nodes[8].Style = new ElementStyle(Color.White, Color.Red);
            }
        }

        public void InspectImage()
        {
            if (Common.QueCam1.Count > 0)
            {
                HObject ho_TempImage = Common.QueCam1.Dequeue();

                int idx = Common.ImageIdx % Common.FaiNames.Length;
                Common.ImageIdx++;
                int fix = 0;
                if (Common.ProC.bFix1)
                {
                    fix = 1;
                }
                else
                {
                    fix = 2;
                }
                ThreadPool.QueueUserWorkItem(Inspect, new InspectInfo() { index = idx, ho_Image = ho_TempImage, nfix = fix });
            }
        }

        private void Inspect(object o)
        {
            InspectInfo info = o as InspectInfo;
            if (info.index == 0)
            {
                Common.mes.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 0; i < advTree1.Nodes.Count; i++)
                {
                    advTree1.Nodes[i].Style = new DevComponents.DotNetBar.ElementStyle(Color.White, Color.DarkGray);
                }
            }
            string[] Values;
            if (info.nfix == 1)
            {
                Values = DataHelper.Values1;
            }
            else
            {
                Values = DataHelper.Values2;
            }
            //存图
            if (Common.bSaveRawImage)
            {
                if (!Directory.Exists($"D:\\RawImage\\{Common.str_CurrentDataCode}"))
                {
                    Directory.CreateDirectory($"D:\\RawImage\\{Common.str_CurrentDataCode}");
                }
                HOperatorSet.WriteImage(info.ho_Image, "bmp", 0, $"D:\\RawImage\\{Common.str_CurrentDataCode}\\{Common.FaiNames[info.index]}.bmp");
            }
            //检测
            if (Common.FaiNames[info.index] == "Power flex&Connector buckle")
            {
                HOperatorSet.RotateImage(info.ho_Image, out info.ho_Image, new HTuple(180), "constant");
            }
            else if (Common.FaiNames[info.index] == "MLB flex&Connector buckle")
            {
                HOperatorSet.RotateImage(info.ho_Image, out info.ho_Image, new HTuple(90), "constant");
            }
            else if (Common.FaiNames[info.index] == "E75 flex&Connector buckle" && Common.str_ProductName == "Flash")
            {
                HOperatorSet.RotateImage(info.ho_Image, out info.ho_Image, new HTuple(270), "constant");
            }
            displayBase1.DisplaySourceImage(info.ho_Image);
            List<StructAlgResult> result = new List<StructAlgResult>();
            lock(Common.snLockObj)
            {
                Common.InspectCode.DealImage(info.ho_Image, info.index + 1, out result);
            }

            int nResult = JudgeResult(info.ho_Image, Common.FaiNames[info.index], info.nfix, result);
            #region Region显示
            HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
            if (result.Count == 1)
            {
                #region 单个检测项
                if (Common.FaiNames[info.index] != "MLB jumper flex B2B")
                {
                    if (nResult == 0)
                    {
                        advTree1.Nodes[info.index].Style = new ElementStyle(Color.White, Color.Green);
                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                    }
                    else
                    {
                        advTree1.Nodes[info.index].Style = new ElementStyle(Color.White, Color.Red);
                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                        if (!(Directory.Exists($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\")))
                        {
                            Directory.CreateDirectory($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\");
                        }
                        HOperatorSet.DumpWindow(displayBase1.DisplayHandle, "png", $"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\{Values[0]}-{ Common.FaiNames[info.index]}.png");
                    }
                }
                else
                {
                    if (nResult == 0)
                    {
                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                    }
                    else
                    {
                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                    }
                }
                #endregion
            }
            else
            {
                #region 两个检测项
                if (nResult == 0)
                {
                    advTree1.Nodes[info.index].Style = new ElementStyle(Color.White, Color.Green);
                    displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region },
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region }
                        });
                }
                else
                {
                    if (nResult == 1)
                    {
                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region },
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region }
                        });
                    }
                    else if (nResult == 2)
                    {
                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region },
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region }
                        });
                    }
                    else if (nResult == 3)
                    {
                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } ,
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region } });
                    }
                    advTree1.Nodes[info.index].Style = new DevComponents.DotNetBar.ElementStyle(Color.White, Color.Red);
                    if (!(Directory.Exists($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\")))
                    {
                        Directory.CreateDirectory($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\");
                    }
                    HOperatorSet.DumpWindow(displayBase1.DisplayHandle, "png", $"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\{Values[0]}-{ Common.FaiNames[info.index]}.png");
                }
                #endregion
            }
            if (!(Directory.Exists($"D:\\ResultImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\{Values[0]}\\")))
            {
                Directory.CreateDirectory($"D:\\ResultImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\{Values[0]}\\");
            }
            HOperatorSet.DumpWindow(displayBase1.DisplayHandle, "png", $"D:\\ResultImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{Values[3]}\\{Values[0]}\\{ Common.FaiNames[info.index]}.png");

            #endregion

            if (nResult != 0)
            {
                Common.Data.NgCount[info.index] = Common.Data.NgCount[info.index]++;
            }

            if (Common.FaiNames[info.index] != "MLB jumper flex B2B")
            {
                Common.nResultList.Add(nResult);
            }
            if (Common.nResultList.Count == Common.FaiNames.Length)
            {
                Common.mes.stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Common.ProductSum++;
                if (Common.nResultList.Contains(1) || Common.nResultList.Contains(2) || Common.nResultList.Contains(3) || Common.nResultList.Contains(-1)) //1 第一项NG 2 第二项NG 3 两项都NG
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Result")] = "FAIL";
                    ShowResult(info.nfix, false);
                    if (info.nfix == 1)
                    {
                        Common.ProC.StationResult1 = false;
                        if (Common.RunOnline || Common.bOfflineMesUse)
                        {
                            DataHelper.SendRawData(DataHelper.Values1, DataHelper.ValueToString(DataHelper.Names, DataHelper.Values1));
                        }
                    }
                    else
                    {
                        Common.ProC.StationResult2 = false;
                        if (Common.RunOnline || Common.bOfflineMesUse)
                        {
                            DataHelper.SendRawData(DataHelper.Values2, DataHelper.ValueToString(DataHelper.Names, DataHelper.Values2));
                        }
                    }
                    Common.ProductNg++;
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Result")] = "PASS";
                    ShowResult(info.nfix, true);
                    if (info.nfix == 1)
                    {
                        if (Common.RunOnline || Common.bOfflineMesUse)
                        {
                            DataHelper.SendRawData(DataHelper.Values1, DataHelper.ValueToString(DataHelper.Names, DataHelper.Values1));
                            DataHelper.PassStation(DataHelper.Values1, DataHelper.Values1[0]);
                        }
                        Common.ProC.StationResult1 = true;
                    }
                    else
                    {
                        if (Common.RunOnline || Common.bOfflineMesUse)
                        {
                            DataHelper.SendRawData(DataHelper.Values2, DataHelper.ValueToString(DataHelper.Names, DataHelper.Values2));
                            DataHelper.PassStation(DataHelper.Values2, DataHelper.Values2[0]);
                        }
                        Common.ProC.StationResult2 = true;
                    }

                }
                Common.nResultList = new List<int>();
                Common.frmDataSum.UpdateSum(Common.ProductSum, Common.ProductNg);
                if (!Directory.Exists("D:\\RuningData\\"))
                {
                    Directory.CreateDirectory("D:\\RuningData\\");
                }
                if (info.nfix == 1)
                {
                    DataHelper.WriteToCsv(Values, $"D:\\RuningData\\{DateTime.Now.ToString("yyyy-MM-dd")}.csv");
                    Common.ProC.bFix1 = false;
                }
                else
                {
                    DataHelper.WriteToCsv(Values, $"D:\\RuningData\\{DateTime.Now.ToString("yyyy-MM-dd")}.csv");
                    Common.ProC.bFix2 = false;
                }
            }
        }

        private delegate void DelegateBool(int fix, bool bValue);

        public void ShowResult(int fix, bool bResult)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateBool(ShowResult), fix, bResult);
                return;
            }
            LabelX label = new LabelX();
            if (fix == 1)
            {
                label = labelX4;
            }
            else if (fix == 2)
            {
                label = labelX3;
            }
            if (bResult)
            {
                label.ForeColor = Color.Green;
                label.Text = "PASS";
            }
            else
            {
                label.ForeColor = Color.Red;
                label.Text = "FAIL";
            }
        }


        public double GetMult(string measureName, int fixture)
        {
            if (fixture == 1)
            {
                return Common.Data.Mult1[DataHelper.MeasureNames.ToList<string>().IndexOf(measureName)];
            }
            else
            {
                return Common.Data.Mult2[DataHelper.MeasureNames.ToList<string>().IndexOf(measureName)];
            }
        }

        public double GetAdd(string measureName, int fixture)
        {
            if (fixture == 1)
            {
                return Common.Data.Add1[DataHelper.MeasureNames.ToList<string>().IndexOf(measureName)];
            }
            else
            {
                return Common.Data.Add2[DataHelper.MeasureNames.ToList<string>().IndexOf(measureName)];
            }
        }

        private int JudgeResult(HObject ho_Image, string FaiName, int fix, List<StructAlgResult> result)
        {
            string[] strValues1, strValues2;
            double[] RawValues;
            double[] OffsetX = new double[6], OffsetY = new double[6];
            double ReviseValue1, ReviseValue2;
            string[] Values;
            if (fix == 1)
            {
                Values = DataHelper.Values1;
            }
            else
            {
                Values = DataHelper.Values2;
            }
            if ((FaiName.Contains("&") && result.Count != 2) || result.Count == 0)
            {
                return -1;
            }
            if (result.Count == 2)
            {
                strValues1 = new string[] { result[0].MeasureValue[0].ToString("0.000"), result[0].MeasureValue[1].ToString("0.0000") };
                strValues2 = new string[] { result[1].MeasureValue[0].ToString("0.000"), result[1].MeasureValue[1].ToString("0.0000") };
                //测量值 毫米值
                RawValues = new double[] { double.Parse(strValues1[0]) * 0.00345 / 0.3,
                    double.Parse(strValues1[1]) * 0.00345 / 0.3,
                    double.Parse(strValues2[0]) * 0.00345 / 0.3,
                    double.Parse(strValues2[1]) * 0.00345 / 0.3 };
            }
            else
            {
                strValues1 = new string[] { result[0].MeasureValue[0].ToString("0.000"), result[0].MeasureValue[1].ToString("0.0000") };
                //测量值 毫米值
                RawValues = new double[] {
                      double.Parse(strValues1[0]) * 0.00345 / 0.3,
                      double.Parse(strValues1[1]) * 0.00345 / 0.3 };

                if (result[0].MeasureValue.Length == 12)
                {
                    for (int i = 0; i < OffsetY.Length; i++)
                    {
                        OffsetX[i] = double.Parse(result[0].MeasureValue[i].ToString("0.000"));
                        OffsetY[i] = double.Parse(result[0].MeasureValue[i + 6].ToString("0.000"));
                    }
                }
            }

            //判定结果
            List<int> bResult = new List<int> { };
            if (FaiName == "MLB jumper flex B2B")
            {
                //测高
                if (Common.ProC.nCheckStep1 == 20)
                {
                    CheckPos pos = Common.Data.DicCheck[FaiName];
                    HeightMeasure.GenMeasurePoints(pos.Xpos, pos.Ypos, OffsetX, OffsetY,
                        out Common.HeightMeasureX, out Common.HeightMeasureY);
                }
                else if (Common.ProC.nCheckStep2 == 20)
                {
                    CheckPos pos = Common.Data.DicCheck[FaiName];
                    HeightMeasure.GenMeasurePoints(pos.Xpos + Common.Data.OffsetX2toX1, pos.Ypos + Common.Data.OffsetY2toY1, OffsetX, OffsetY
                        , out Common.HeightMeasureX, out Common.HeightMeasureY);
                }
            }
            else if (FaiName == "Power flex&Connector buckle")
            {
                //电源排线测量值
                ReviseValue1 = RawValues[1] * GetMult("Power flex_P1", fix) + GetAdd("Power flex_P1", fix);
                ReviseValue2 = RawValues[0] * GetMult("Power flex_P2", fix) + GetAdd("Power flex_P2", fix);

                Values[DataHelper.Names.ToList<string>().IndexOf("Power flex_P1")] = ReviseValue1.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("Power flex_P2")] = ReviseValue2.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("Power flex_(P1 P2)")] = Math.Abs(ReviseValue1 - ReviseValue2).ToString("0.0000");
                if (ReviseValue1 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power flex_P1")] ||
                    ReviseValue1 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power flex_P1")] ||
                    ReviseValue2 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power flex_P2")] ||
                    ReviseValue2 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power flex_P2")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power flex_(P1 P2)")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power flex_(P1 P2)")])
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Power flex")] = "NG";
                    bResult.Add(1);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Power flex")] = "OK";
                    bResult.Add(0);
                }
                //钢琴盖
                if (result[1].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Power Connector buckle")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Power Connector buckle")] = "NG";
                    bResult.Add(2);
                }
            }
            else if (FaiName == "Mesa flex&Connector buckle")
            {
                ReviseValue1 = RawValues[0] * GetMult("Mesa flex_P1", fix) + GetAdd("Mesa flex_P1", fix);
                ReviseValue2 = RawValues[1] * GetMult("Mesa flex_P2", fix) + GetAdd("Mesa flex_P2", fix);
                Values[DataHelper.Names.ToList<string>().IndexOf("Mesa flex_P1")] = ReviseValue1.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("Mesa flex_P2")] = ReviseValue2.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("Mesa flex_(P1 P2)")] = Math.Abs(ReviseValue1 - ReviseValue2).ToString("0.0000");
                if (ReviseValue1 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Mesa flex_P1")] ||
                    ReviseValue1 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Mesa flex_P1")] ||
                    ReviseValue2 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Mesa flex_P2")] ||
                    ReviseValue2 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Mesa flex_P2")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Mesa flex_(P1 P2)")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Mesa flex_(P1 P2)")])
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa flex")] = "NG";
                    bResult.Add(1);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa flex")] = "OK";
                    bResult.Add(0);
                }
                if (result[1].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa Connector buckle")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa Connector buckle")] = "NG";
                    bResult.Add(2);
                }
            }
            else if (FaiName == "E75 flex&Connector buckle")
            {
                ReviseValue1 = RawValues[0] * GetMult("E75 flex_P1", fix) + GetAdd("E75 flex_P1", fix);
                ReviseValue2 = RawValues[1] * GetMult("E75 flex_P2", fix) + GetAdd("E75 flex_P2", fix);
                Values[DataHelper.Names.ToList<string>().IndexOf("E75 flex_P1")] = ReviseValue1.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("E75 flex_P2")] = ReviseValue2.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("E75 flex_(P1 P2)")] = Math.Abs(ReviseValue1 - ReviseValue2).ToString("0.0000");
                if (ReviseValue1 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("E75 flex_P1")] ||
                    ReviseValue1 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("E75 flex_P1")] ||
                    ReviseValue2 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("E75 flex_P2")] ||
                    ReviseValue2 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("E75 flex_P2")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("E75 flex_(P1 P2)")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("E75 flex_(P1 P2)")])
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("E75 flex")] = "NG";
                    bResult.Add(1);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("E75 flex")] = "OK";
                    bResult.Add(0);
                }
                if (result[1].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("E75 Connector buckle")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("E75 Connector buckle")] = "NG";
                    bResult.Add(2);
                }
            }
            else if (FaiName == "MLB flex&Connector buckle")
            {
                ReviseValue1 = RawValues[0] * GetMult("MLB flex_P1", fix) + GetAdd("MLB flex_P1", fix);
                ReviseValue2 = RawValues[1] * GetMult("MLB flex_P2", fix) + GetAdd("MLB flex_P2", fix);
                Values[DataHelper.Names.ToList<string>().IndexOf("MLB flex_P1")] = ReviseValue1.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("MLB flex_P2")] = ReviseValue2.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("MLB flex_(P1 P2)")] = Math.Abs(ReviseValue1 - ReviseValue2).ToString("0.0000");
                if (ReviseValue1 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB flex_P1")] ||
                    ReviseValue1 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB flex_P1")] ||
                    ReviseValue2 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB flex_P2")] ||
                    ReviseValue2 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB flex_P2")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB flex_(P1 P2)")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB flex_(P1 P2)")])
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("MLB flex")] = "NG";
                    bResult.Add(1);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("MLB flex")] = "OK";
                    bResult.Add(0);
                }
                if (result[1].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("MLB Connector buckle")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("MLB Connector buckle")] = "NG";
                    bResult.Add(2);
                }
            }
            else if (FaiName == "Antenna shrapnel")
            {
                ReviseValue1 = RawValues[0] * GetMult("Antenna shrapnel_P1", fix) + GetAdd("Antenna shrapnel_P1", fix);
                ReviseValue2 = RawValues[1] * GetMult("Antenna shrapnel_P2", fix) + GetAdd("Antenna shrapnel_P2", fix);
                Values[DataHelper.Names.ToList<string>().IndexOf("Antenna shrapnel_P1")] = ReviseValue1.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("Antenna shrapnel_P2")] = ReviseValue2.ToString("0.0000");
                Values[DataHelper.Names.ToList<string>().IndexOf("Antenna shrapnel_(P1 P2)")] = Math.Abs(ReviseValue1 - ReviseValue2).ToString("0.0000");
                if (ReviseValue1 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Antenna shrapnel_P1")] ||
                    ReviseValue1 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Antenna shrapnel_P1")] ||
                    ReviseValue2 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Antenna shrapnel_P2")] ||
                    ReviseValue2 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Antenna shrapnel_P2")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Antenna shrapnel_(P1 P2)")] ||
                    Math.Abs(ReviseValue1 - ReviseValue2) > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Antenna shrapnel_(P1 P2)")])
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Antenna shrapnel")] = "NG";
                    bResult.Add(1);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Antenna shrapnel")] = "OK";
                    bResult.Add(0);
                }
            }
            else if (FaiName == "Main board screw 5")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 5")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 5")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Main board screw 4")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 4")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 4")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Main board screw 3")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 3")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 3")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Main board screw 2")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 2")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 2")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Main board screw 1")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 1")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Main board screw 1")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Switch screw 2")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Switch screw 2")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Switch screw 2")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Switch screw 1")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Switch screw 1")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Switch screw 1")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Mesa jumper flex cosmetic 1")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 1")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 1")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Mesa jumper flex cosmetic 2")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 2")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 2")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Mesa jumper flex cosmetic 3")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 3")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 3")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Mesa jumper flex cosmetic 4")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 4")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 4")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Mesa jumper flex cosmetic 5")
            {
                if (result[0].Success)
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 5")] = "OK";
                    bResult.Add(0);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Mesa jumper flex cosmetic 5")] = "NG";
                    bResult.Add(1);
                }
            }
            else if (FaiName == "Power switch")
            {
                ReviseValue1 = RawValues[0] * GetMult("Power switch_P1", fix) + GetAdd("Power switch_P1", fix);
                Values[DataHelper.Names.ToList<string>().IndexOf("Power switch_P1")] = ReviseValue1.ToString("0.0000");
                if (ReviseValue1 < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power switch_P1")] ||
                    ReviseValue1 > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("Power switch_P1")])
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Power switch")] = "NG";
                    bResult.Add(1);
                }
                else
                {
                    Values[DataHelper.Names.ToList<string>().IndexOf("Power switch")] = "OK";
                    bResult.Add(0);
                }
            }

            if (bResult.Contains(1) && bResult.Contains(2))
            {
                return 3;
            }
            else if (bResult.Contains(2))
            {
                return 2;
            }
            else if (bResult.Contains(1))
            {
                return 1;
            }
            return 0;
        }



        FolderBrowserDialog fbd = new FolderBrowserDialog();

        public void LocalImageTest()
        {
            for (int i = 0; i < advTree1.Nodes.Count; i++)
            {
                advTree1.Nodes[i].Style = new DevComponents.DotNetBar.ElementStyle(Color.White, Color.DarkGray);
            }
            fbd.SelectedPath = fbd.SelectedPath;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                fbd.SelectedPath = fbd.SelectedPath;
                HTuple hv_ImageFiles = null;

                HOperatorSet.ListFiles(fbd.SelectedPath, (new HTuple("files")).TupleConcat(
                   "follow_links"), out hv_ImageFiles);
                if (hv_ImageFiles.Length != Common.FaiNames.Length)
                {
                    MessageBox.Show("选中文件夹与检测内容不符（图片数目错误）！");
                    return;
                }
                else
                {
                    for (int i = 0; i < hv_ImageFiles.Length; i++)
                    {
                        HObject ho_Image = new HObject();
                        HOperatorSet.ReadImage(out ho_Image, hv_ImageFiles.SArr[i]);


                        for (int j = 0; j < Common.FaiNames.Length; j++)
                        {
                            if (hv_ImageFiles.SArr[i].Contains(Common.FaiNames[j]))
                            {
                                advTree1.Nodes[j].Style = new DevComponents.DotNetBar.ElementStyle(Color.White, Color.Yellow);
                                advTree1.Update();
                                if (Common.FaiNames[j] == "Power flex&Connector buckle")
                                {
                                    HOperatorSet.RotateImage(ho_Image, out ho_Image, new HTuple(180), "constant");
                                }
                                else if (Common.FaiNames[j] == "MLB flex&Connector buckle")
                                {
                                    HOperatorSet.RotateImage(ho_Image, out ho_Image, new HTuple(90), "constant");
                                }
                                else if (Common.str_ProductName == "Flash" && Common.FaiNames[j] == "E75 flex&Connector buckle")
                                {
                                    HOperatorSet.RotateImage(ho_Image, out ho_Image, new HTuple(270), "constant");
                                }

                                List<StructAlgResult> result = new List<StructAlgResult>();
                                displayBase1.DisplaySourceImage(ho_Image);
                                Common.InspectCode.DealImage(ho_Image, j + 1, out result);

                                int nResult = JudgeResult(ho_Image, Common.FaiNames[j], 1, result);
                                #region Region显示
                                HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
                                if (result.Count == 1)
                                {
                                    #region 单个检测项
                                    if (Common.FaiNames[j] != "MLB jumper flex B2B")
                                    {
                                        if (nResult == 0)
                                        {
                                            advTree1.Nodes[j].Style = new ElementStyle(Color.White, Color.Green);
                                            displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                                        }
                                        else
                                        {
                                            advTree1.Nodes[j].Style = new ElementStyle(Color.White, Color.Red);
                                            displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                                            if (!(Directory.Exists($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\")))
                                            {
                                                Directory.CreateDirectory($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\");
                                            }
                                            HOperatorSet.DumpWindow(displayBase1.DisplayHandle, "png", $"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\Test-{ Common.FaiNames[j]}.png");
                                        }
                                    }
                                    else
                                    {
                                        if (nResult == 0)
                                        {
                                            displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                                        }
                                        else
                                        {
                                            displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } });
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 两个检测项
                                    if (nResult == 0)
                                    {
                                        advTree1.Nodes[j].Style = new ElementStyle(Color.White, Color.Green);
                                        displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region },
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region }
                        });
                                    }
                                    else
                                    {
                                        if (nResult == 1)
                                        {
                                            displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region },
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region }
                        });
                                        }
                                        else if (nResult == 2)
                                        {
                                            displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.green, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region },
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region }
                        });
                                        }
                                        else if (nResult == 3)
                                        {
                                            displayBase1.AddRegion(new LeoBase.HsbRegions()
                         { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[0].Region } ,
                           new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.red, Draw = LeoBase.HsbDraw.margin, Value =  result[1].Region } });
                                        }
                                        advTree1.Nodes[j].Style = new DevComponents.DotNetBar.ElementStyle(Color.White, Color.Red);
                                        if (!(Directory.Exists($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\")))
                                        {
                                            Directory.CreateDirectory($"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\");
                                        }
                                        HOperatorSet.DumpWindow(displayBase1.DisplayHandle, "png", $"D:\\NgImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\Test-{ Common.FaiNames[j]}.png");
                                    }
                                    #endregion
                                }


                                if (!(Directory.Exists($"D:\\ResultImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\Test\\")))
                                {
                                    Directory.CreateDirectory($"D:\\ResultImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\Test\\");
                                }
                                HOperatorSet.DumpWindow(displayBase1.DisplayHandle, "png", $"D:\\ResultImage\\{DateTime.Now.ToString("yyyy-MM-dd")}\\Test\\Test\\{ Common.FaiNames[j]}.png");

                                #endregion
                                Thread.Sleep(1500);
                            }
                        }
                    }
                    if (Common.nResultList.Count == Common.FaiNames.Length)
                    {

                        Common.ProductSum++;
                        if (Common.nResultList.Contains(1) || Common.nResultList.Contains(2) || Common.nResultList.Contains(3) || Common.nResultList.Contains(-1)) //1 第一项NG 2 第二项NG 3 两项都NG
                        {
                            DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("Result")] = "FAIL";
                            ShowResult(1, false);
                            //DataHelper.SendRawData(DataHelper.ValueToString(DataHelper.Names, DataHelper.Values));
                            Common.ProductNg++;
                        }
                        else
                        {
                            DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("Result")] = "PASS";
                            ShowResult(1, true);
                            //DataHelper.PassStation(DataHelper.Values[0]);
                        }
                        Common.nResultList = new List<int>();
                        Common.frmDataSum.UpdateSum(Common.ProductSum, Common.ProductNg);
                        if (!Directory.Exists("D:\\RuningData\\"))
                        {
                            Directory.CreateDirectory("D:\\RuningData\\");
                        }
                        DataHelper.WriteToCsv(DataHelper.Values1, $"D:\\RuningData\\{DateTime.Now.ToString("yyyy-MM-dd")}.csv");

                    }
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock(Common.snLockObj)
            {
                Common.str_ProductName = comboBox1.SelectedItem.ToString();
                if (Common.str_ProductName == "Star")
                {
                    Common.FaiNames = Common.StarFais;
                }
                else if (Common.str_ProductName == "Starfire")
                {
                    Common.FaiNames = Common.StarFireFais;
                }
                else if (Common.str_ProductName == "Flash")
                {
                    Common.FaiNames = Common.FlashFais;
                }
                HOperatorSet.SetSystem("clip_region", "false");


                Common.ShowProductType(Common.str_ProductName);
                Thread.Sleep(500);
                Common.frmMain.LoadParam();
                Common.frmInspect.LoadInspectCode();
                //加载校准值
                Common.frmMain.LoadMultAndAdd();
                InitShowNodes();
                Common.frmSetting.LoadBoardAndPostionSetting(Common.str_ProductName);
                Common.frmSetting.InitCheck();
            }
        }
    }
}
