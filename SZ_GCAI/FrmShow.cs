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

namespace SZ_BydKeyboard
{
    public partial class FrmShow : DockContent
    {
        public FrmShow()
        {
            InitializeComponent();
        }
        //对焦
        double targetPos;
        public bool b_Focus = false;
        public int nFocusStep = 0, nImageIndex = 0;
        public double dBestHeight = 0.0;
        //平台标定
        public bool b_Calibra = false;
        public double dOffset = 1.0, dCenterX = 0, dCenterY = 0;
        public int nCalibraStep = -1, iCalibraIndex = 0;
        public HTuple hv_PhysX = new HTuple();
        public HTuple hv_PhysY = new HTuple();
        public HTuple hv_ImageRows = new HTuple();
        public HTuple hv_ImageCols = new HTuple();
        //针卡标定
        public int nPinFirstIndex = -1, nPinFirstStep = -1, nReturn = -1;
        public bool b_GetFirstPinPoints = false, b_GetFirstPinPointsFinish = false;

        public int nPinSecondIndex = -1, nPinSecondStep = -1;
        public bool b_GetSecondPinPoints = false, b_GetSecondPinPointsFinish = false;
        //匹配
        public bool b_Match = false;
        public int nMatch = -1;
        public bool b_First = false, b_Second = false, b_FirstFinish = false, b_SecondFinish = false, b_Thirdly = false, b_ThirdlyFinish = false;
        public double offsetDeg = 0.0, offsetX = 0.0, offsetY = 0.0;
        //Z轴安全
        public bool b_Zsafe = false;
        public int nZsafe = -1;

        //获取基准Base1 Base2
        public int nGetBase1 = -1, nGetBase2 = -1;
        public bool b_GetBase1 = false, b_GetBase2 = false, b_GetBase1Finish = false, b_GetBase2Finish = false;

        //获取基准Base1 Base2
        public int nGetNow1 = -1, nGetNow2 = -1;
        public bool b_GetNow1 = false, b_GetNow2 = false, b_GetNow1Finish = false, b_GetNow2Finish = false;

        public void FocusAndSaveImage()
        {
            if (Common.QueCam2.Count > 0)
            {
                HObject ho_Image = Common.QueCam2.Dequeue();
                displayBase2.DisplaySourceImage(ho_Image);
                if (Common.Data.b_SaveImage)
                {
                    HOperatorSet.WriteImage(ho_Image, "bmp", 0, $"{ Common.Data.str_SavePath}//{Common.ProC.Card1.LogPos[0].ToString("0.000")}.bmp");
                }
                HObject ho_ImageReduced = new HObject(); HObject ho_EdgeAmplitude = new HObject();
                HTuple hv_Mean = null, hv_Deviation = null;
                displayBase2.AddRegion(new LeoBase.HsbRegions() { new LeoBase.HsbRegion() { Value = Common.HalObject.ho_FocusRegion, Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin } });
                HOperatorSet.ReduceDomain(ho_Image, Common.HalObject.ho_FocusRegion, out ho_ImageReduced);
                HOperatorSet.SobelAmp(ho_ImageReduced, out ho_EdgeAmplitude, "sum_abs", 3);
                HOperatorSet.Intensity(ho_EdgeAmplitude, ho_EdgeAmplitude, out hv_Mean, out hv_Deviation);
                Common.Data.Gradient.Add(hv_Mean.D);
                if (targetPos == Common.Data.endPos)
                {
                    dBestHeight = Common.Data.PosList[Common.Data.Gradient.IndexOf(Common.Data.Gradient.Max())];
                    Common.ShowMsgEvent($"Tips:{DateTime.Now.ToString("yyyy:MM:dd HH:mm:ms")}---最佳对焦高度为[{dBestHeight}]---");
                    if (dBestHeight == Common.Data.startPos || dBestHeight == Common.Data.startPos)
                    {
                        OmronPLC.GetInstance().Send("10281", 21);
                        Common.ShowMsgEvent($"Error:{DateTime.Now.ToString("yyyy:MM:dd HH:mm:ms")}---对焦失败，请选择合适的对焦范围！");
                    }
                    else
                    {
                        OmronPLC.GetInstance().Send("10281", 22);
                        OmronPLC.GetInstance().SendFloatValue("4086", (float)(Common.Data.Base_Height - dBestHeight));
                        Common.ShowMsgEvent($"Tips:{DateTime.Now.ToString("yyyy:MM:dd HH:mm:ms")}---高度补偿:[{Common.Data.Base_Height - dBestHeight}]---");
                    }
                }
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
            HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(30).TupleConcat(30), new HTuple(60).TupleConcat(60));
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
            HOperatorSet.InnerCircle(ho_SortedRegions, out hv_Rows, out hv_Cols, out hv_Area);
            hv_PinRows = hv_Rows;
            hv_PinCols = hv_Cols;
            HOperatorSet.SetLineWidth(displayBase2.DisplayHandle, 2);

            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Rows, hv_Cols, 15, 0);
            //hv_PinRows = hv_Rows;
            //hv_PinCols = hv_Cols;
            displayBase2.AddRegion(new LeoBase.HsbRegions()
                    { new LeoBase.HsbRegion() { ColorNum =6, Draw = LeoBase.HsbDraw.fill, Value = ho_Cross },
                      new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value = ho_SortedRegions } });
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

            HOperatorSet.MultImage(ho_Image, ho_Image, out ho_TempImage, 0.06, 0);
            HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 200, 255);
            HOperatorSet.ClosingCircle(ho_ThresholdRegion, out ho_ThresholdRegion, 10.5);
            HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
            HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(20).TupleConcat(20), new HTuple(80).TupleConcat(80));
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
            HOperatorSet.SmallestCircle(ho_SortedRegions, out hv_Rows, out hv_Cols, out hv_Area);

            if (hv_Rows.Length > 0)
            {
                hv_LastRow = hv_Rows.DArr[hv_Rows.Length - 1];
                hv_LastCol = hv_Cols.DArr[hv_Rows.Length - 1];
                HOperatorSet.SetLineWidth(displayBase2.DisplayHandle, 2);
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_LastRow, hv_LastCol, 15, 0);
                //hv_PinRows = hv_Rows;
                //hv_PinCols = hv_Cols;
                displayBase2.AddRegion(new LeoBase.HsbRegions()
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

            HOperatorSet.MultImage(ho_Image, ho_Image, out ho_TempImage, 0.06, 0);
            HOperatorSet.Threshold(ho_TempImage, out ho_ThresholdRegion, 200, 255);
            HOperatorSet.ClosingCircle(ho_ThresholdRegion, out ho_ThresholdRegion, 10.5);
            HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
            HOperatorSet.SelectShape(ho_ConnectionRegion, out ho_SelectedRegions, new HTuple("width").TupleConcat("height"), "and", new HTuple(20).TupleConcat(20), new HTuple(80).TupleConcat(80));
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character", "true", "row");
            HOperatorSet.SmallestCircle(ho_SortedRegions, out hv_Rows, out hv_Cols, out hv_Area);

            if (hv_Rows.Length > 0)
            {
                hv_FirstRow = hv_Rows.DArr[0];
                hv_FirstCol = hv_Cols.DArr[0];
                HOperatorSet.SetLineWidth(displayBase2.DisplayHandle, 2);
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_FirstRow, hv_FirstCol, 15, 0);
                //hv_PinRows = hv_Rows;
                //hv_PinCols = hv_Cols;
                displayBase2.AddRegion(new LeoBase.HsbRegions()
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

        public void AutoGetBase1()
        {
            if (nGetBase1 == 0)
            {
                Common.ProC.Card1.AbsoluteMove(0, Common.Data.Base_Height, 0.01, 0.5, 0.5);
                nGetBase1 = 10;
            }
            else if (nGetBase1 == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - Common.Data.Base_Height) < 0.001)
                {
                    nGetBase1 = 20;
                }
            }
            else if (nGetBase1 == 20)
            {
                Common.Data.Base_Grab1X = OmronPLC.GetInstance().GetAddressFloatValue("2246");
                Common.Data.Base_Grab1Y = OmronPLC.GetInstance().GetAddressFloatValue("2250");
                Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->拍照位1 X轴：[{Common.Data.Base_Grab1X.ToString("0.000")}],Y轴：[{ Common.Data.Base_Grab1Y.ToString("0.000")}]--");
                Common.Cam2.Snap();
                b_GetBase1 = true;
                b_GetBase1Finish = false;
                nGetBase1 = 30;
            }
            else if (nGetBase1 == 30)
            {
                if (b_GetBase1Finish)
                {
                    nGetBase1 = -1;
                }
            }

            if (b_GetBase1)
            {
                if (Common.QueCam2.Count > 0)
                {
                    HObject ho_Image = Common.QueCam2.Dequeue();
                    displayBase2.DisplaySourceImage(ho_Image);
                    HTuple hv_LastRow, hv_LastCol;
                    double Xpos, Ypos;
                    GetLastPin(ho_Image, out hv_LastRow, out hv_LastCol);
                    Common.HalObject.FirstPinCalibra.ImageToAxis(hv_LastRow, hv_LastCol, out Xpos, out Ypos);
                    Common.Data.Base_FirstPinX = Xpos;
                    Common.Data.Base_FirstPinY = Ypos;
                    Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准1 X轴：[{ Common.Data.Base_FirstPinX.ToString("0.000")}],Y轴：[{ Common.Data.Base_FirstPinY.ToString("0.000")}]--");

                    OmronPLC.GetInstance().Send("10281", 25);
                    b_GetBase1 = false;
                    b_GetBase1Finish = true;
                }
            }
        }
        public void AutoGetBase2()
        {
            if (nGetBase2 == 0)
            {
                Common.ProC.Card1.AbsoluteMove(0, Common.Data.Base_Height, 0.01, 0.5, 0.5);
                nGetBase2 = 10;
            }
            else if (nGetBase2 == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - Common.Data.Base_Height) < 0.001)
                {
                    nGetBase2 = 20;
                }
            }

            if (nGetBase2 == 20)
            {
                Common.Data.Base_Grab2X = OmronPLC.GetInstance().GetAddressFloatValue("2246");
                Common.Data.Base_Grab2Y = OmronPLC.GetInstance().GetAddressFloatValue("2250");
                Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->拍照位2 X轴：[{Common.Data.Base_Grab2X.ToString("0.000")}],Y轴：[{ Common.Data.Base_Grab2Y.ToString("0.000")}]--");

                Common.Cam2.Snap();
                b_GetBase2 = true;
                nGetBase2 = 30;
            }
            else if (nGetBase2 == 30)
            {
                if (b_GetBase2Finish)
                {
                    nGetBase2 = -1;
                }
            }
            if (b_GetBase2)
            {
                if (Common.QueCam2.Count > 0)
                {
                    HObject ho_Image = Common.QueCam2.Dequeue();
                    displayBase2.DisplaySourceImage(ho_Image);
                    HTuple hv_FirstRow, hv_FirstCol, hv_Angle, hv_Deg;
                    double Xpos, Ypos;
                    GetFirstPin(ho_Image, out hv_FirstRow, out hv_FirstCol);

                    Common.HalObject.SecondPinCalibra.ImageToAxis(hv_FirstRow, hv_FirstCol, out Xpos, out Ypos);

                    //计算第二点的位置
                    Common.Data.Base_LastPinX = Xpos;
                    Common.Data.Base_LastPinY = Ypos;
                    Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准2 X轴：[{ Common.Data.Base_LastPinX.ToString("0.000")}],Y轴：[{ Common.Data.Base_LastPinY.ToString("0.000")}]--");

                    //计算角度
                    HOperatorSet.AngleLl(Common.Data.Base_LastPinX, Common.Data.Base_LastPinY, Common.Data.Base_FirstPinX, Common.Data.Base_FirstPinY,
                       Common.Data.Base_Grab1X, Common.Data.Base_Grab1Y, Common.Data.Base_Grab2X, Common.Data.Base_Grab2Y, out hv_Angle);
                    HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);
                    Common.Data.Base_PinDeg = hv_Deg;
                    Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准针卡角度：[{  Common.Data.Base_PinDeg.ToString("0.000")}]--");
                    b_GetBase2 = false;
                    b_GetBase2Finish = true;
                    OmronPLC.GetInstance().Send("10281", 26);
                }
            }
        }

        public void AutoGetNow1()
        {
            if (nGetNow1 == 0)
            {
                Common.ProC.Card1.AbsoluteMove(0, Common.Data.Base_Height, 0.01, 0.5, 0.5);
                nGetNow1 = 10;
            }
            else if (nGetNow1 == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - Common.Data.Base_Height) < 0.001)
                {
                    nGetNow1 = 20;
                }
            }
            else if (nGetNow1 == 20)
            {
                double grabX1 = OmronPLC.GetInstance().GetAddressFloatValue("2246");
                double grabY1 = OmronPLC.GetInstance().GetAddressFloatValue("2250");
                Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->拍照位1 X轴：[{grabX1.ToString("0.000")}],Y轴：[{ grabY1.ToString("0.000")}]--");

                Common.Cam2.Snap();
                b_GetNow1 = true;
                b_GetNow1Finish = false;
                nGetNow1 = 30;
            }
            else if (nGetNow1 == 30)
            {
                if (b_GetNow1Finish)
                {
                    nGetNow1 = -1;
                }
            }
            if (b_GetNow1)
            {
                if (Common.QueCam2.Count > 0)
                {
                    HObject ho_Image = Common.QueCam2.Dequeue();
                    displayBase2.DisplaySourceImage(ho_Image);
                    HTuple hv_LastRow, hv_LastCol;
                    double Xpos, Ypos;
                    if (GetLastPin(ho_Image, out hv_LastRow, out hv_LastCol))
                    {
                        Common.HalObject.FirstPinCalibra.ImageToAxis(hv_LastRow, hv_LastCol, out Xpos, out Ypos);
                        Common.Data.Now_FirstPinX = Xpos;
                        Common.Data.Now_FirstPinY = Ypos;
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前1 X轴：[{ Common.Data.Now_FirstPinX.ToString("0.000")}],Y轴：[{ Common.Data.Now_FirstPinY.ToString("0.000")}]--");

                        OmronPLC.GetInstance().Send("10281", 25);
                    }
                    else
                    {
                        Common.ShowMsgEvent.Invoke($"Error:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->拍照失败！--");
                        OmronPLC.GetInstance().Send("10281", 27);
                    }
                    b_GetNow1 = false;
                    b_GetNow1Finish = true;
                }
            }
        }
        public void AutoGetNow2()
        {
            if (nGetNow2 == 0)
            {
                Common.ProC.Card1.AbsoluteMove(0, Common.Data.Base_Height, 0.01, 0.5, 0.5);
                nGetNow2 = 10;
            }
            else if (nGetNow2 == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - Common.Data.Base_Height) < 0.001)
                {
                    nGetNow2 = 20;
                }
            }
            if (nGetNow2 == 20)
            {
                double grabX2 = OmronPLC.GetInstance().GetAddressFloatValue("2246");
                double grabY2 = OmronPLC.GetInstance().GetAddressFloatValue("2250");
                Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->拍照位2 X轴：[{grabX2.ToString("0.000")}],Y轴：[{ grabY2.ToString("0.000")}]--");
                Common.Cam2.Snap();
                b_GetNow2 = true;
                nGetNow2 = 30;
            }
            else if (nGetNow2 == 30)
            {
                if (b_GetNow2Finish)
                {
                    nGetNow2 = -1;
                }
            }
            if (b_GetNow2)
            {
                if (Common.QueCam2.Count > 0)
                {
                    HObject ho_Image = Common.QueCam2.Dequeue();
                    displayBase2.DisplaySourceImage(ho_Image);
                    HTuple hv_FirstRow, hv_FirstCol, hv_Angle, hv_Deg;
                    double Xpos, Ypos;
                    if (GetFirstPin(ho_Image, out hv_FirstRow, out hv_FirstCol))
                    {
                        Common.HalObject.SecondPinCalibra.ImageToAxis(hv_FirstRow, hv_FirstCol, out Xpos, out Ypos);

                        //计算第二点的位置
                        Common.Data.Now_LastPinX = Xpos;
                        Common.Data.Now_LastPinY = Ypos;
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前2 X轴：[{ Common.Data.Now_LastPinX.ToString("0.000")}],Y轴：[{ Common.Data.Now_LastPinY.ToString("0.000")}]--");

                        //计算角度
                        HOperatorSet.AngleLl(Common.Data.Now_LastPinX, Common.Data.Now_LastPinY, Common.Data.Now_FirstPinX, Common.Data.Now_FirstPinY,
                           Common.Data.Base_Grab1X, Common.Data.Base_Grab1Y, Common.Data.Base_Grab2X, Common.Data.Base_Grab2Y, out hv_Angle);
                        HOperatorSet.TupleDeg(hv_Angle, out hv_Deg);
                        Common.Data.Now_PinDeg = hv_Deg;
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前针卡角度：[{  Common.Data.Now_PinDeg.ToString("0.000")}]--");

                        Common.Data.OffsetX = ((Common.Data.Now_FirstPinX + Common.Data.Now_LastPinX) / 2 - (Common.Data.Base_FirstPinX + Common.Data.Base_LastPinX) / 2);
                        Common.Data.OffsetY = ((Common.Data.Now_FirstPinY + Common.Data.Now_LastPinY) / 2 - (Common.Data.Base_FirstPinY + Common.Data.Base_LastPinY) / 2);
                        Common.Data.OffsetDeg = (Common.Data.Now_PinDeg - Common.Data.Base_PinDeg);
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前针卡相对标准针卡角度偏移：[{  Common.Data.OffsetDeg.ToString("0.000")}]--");
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前针卡相对标准针卡X偏移：[{  Common.Data.OffsetX.ToString("0.000")}]--");
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前针卡相对标准针卡Y偏移：[{  Common.Data.OffsetY.ToString("0.000")}]--");
                        OmronPLC.GetInstance().Send("10281", 26);
                    }
                    else
                    {
                        Common.ShowMsgEvent.Invoke($"Error:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->拍照失败--");
                        OmronPLC.GetInstance().Send("10281", 27);
                    }
                    b_GetNow2 = false;
                    b_GetNow2Finish = true;

                }
            }
        }
        public void AutoFocus()
        {
            if (nFocusStep == 0)
            {
                targetPos = Math.Round(Common.Data.startPos + nImageIndex * Common.Data.stepPos, 4);
                Common.ProC.Card1.AbsoluteMove(0, targetPos, 0.01, 0.5, 0.5);
                nFocusStep = 10;
            }
            else if (nFocusStep == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - targetPos) < 0.001)
                {
                    Common.ProC.Start_T(0, 200);
                    nFocusStep = 20;
                }
            }
            else if (nFocusStep == 20)
            {
                if (Common.ProC.TimerSignal[0])
                {
                    Common.ProC.Reset_T(0);
                    Common.Data.PosList.Add(Common.ProC.Card1.LogPos[0]);
                    Common.Cam2.grabFinishCall = Common.grabImage2;
                    Common.Cam2.Snap();
                    nImageIndex++;
                    //BackWorker.ReportProgress(nImageIndex);
                    Common.ProC.Start_T(1, 300);
                    nFocusStep = 30;
                }
            }
            else if (nFocusStep == 30)
            {
                if (Common.ProC.TimerSignal[1])
                {
                    Common.ProC.Reset_T(1);
                    if (targetPos == Common.Data.endPos)
                    {
                        nFocusStep = 40;

                        //Btn_Start.Enabled = true;
                        //Btn_Stop.Enabled = false;
                    }
                    else
                    {
                        nFocusStep = 0;
                    }
                }
            }
            else if (nFocusStep == 40)
            {
                Common.ProC.Card1.AbsoluteMove(0, 2, 0.01, 0.5, 0.5);
                nFocusStep = 50;
            }
            else if (nFocusStep == 50)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - 2) < 0.001)
                {
                    //回安全位置成功
                    OmronPLC.GetInstance().Send("10281", 23);
                    nFocusStep = -1;
                }
            }
        }
        public void AutoFirstPinCalibra(int PointIndex)
        {
            if (nPinFirstStep == 0)
            {
                Common.ProC.Card1.AbsoluteMove(0, Common.Data.Base_Height, 0.01, 0.5, 0.5);
                nPinFirstStep = 10;
            }
            else if (nPinFirstStep == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - Common.Data.Base_Height) < 0.001)
                {
                    nPinFirstStep = 20;
                }
            }
            else if (nPinFirstStep == 20)
            {
                double CurrentX = OmronPLC.GetInstance().GetAddressFloatValue("2246");
                double CurrentY = OmronPLC.GetInstance().GetAddressFloatValue("2250");
                Common.HalObject.FirstPinCalibra.XposList[8 - PointIndex] = CurrentX;
                Common.HalObject.FirstPinCalibra.YposList[8 - PointIndex] = CurrentY;
                Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前标定X轴：[{CurrentX.ToString("0.000")}],Y轴：[{CurrentY.ToString("0.000")}]--");
                Common.Cam2.Snap();
                b_GetFirstPinPoints = true;
                nPinFirstStep = 30;
            }
            else if (nPinFirstStep == 30)
            {
                if (b_GetFirstPinPointsFinish)
                {
                    Common.ProC.Start_T(31, 500);
                    nPinFirstStep = 40;
                }
            }
            else if (nPinFirstStep == 40)
            {
                if (Common.ProC.TimerSignal[31])
                {
                    Common.ProC.Reset_T(31);
                    OmronPLC.GetInstance().Send("10281", PointIndex + 33);
                    nPinFirstStep = -1;
                }
            }

            if (b_GetFirstPinPoints)
            {
                if (Common.QueCam2.Count > 0)
                {
                    HObject ho_Image = Common.QueCam2.Dequeue();
                    displayBase2.DisplaySourceImage(ho_Image);
                    HOperatorSet.WriteImage(ho_Image, "bmp", 0, $"{Common.Data.str_SavePath}\\{PointIndex}.bmp");
                    HTuple hv_LastRow = null, hv_LastCol = null;
                    if (GetLastPin(displayBase2.SourceImage, out hv_LastRow, out hv_LastCol))
                    {

                        Common.HalObject.FirstPinCalibra.AddImagePoint(PointIndex, hv_LastRow, hv_LastCol, Leo9Calibra.MoveType.AxisMove);
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->第{PointIndex}点图像坐标Row：[{hv_LastRow.D.ToString("0.000")}],Col：[{hv_LastCol.D.ToString("0.000")}]--");

                        if (PointIndex == 8) //最后一个点
                        {
                            Common.HalObject.FirstPinCalibra.GenHomMat2D();
                            Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->标定1完成！--");
                            OmronPLC.GetInstance().Send("10281", 42);
                        }
                    }
                    else
                    {
                        OmronPLC.GetInstance().Send("10281", 43);
                    }
                    b_GetFirstPinPoints = false;
                    b_GetFirstPinPointsFinish = true;
                }
            }
        }
        public void AutoSecondPinCalibra(int PointIndex)
        {
            if (nPinSecondStep == 0)
            {
                Common.ProC.Card1.AbsoluteMove(0, Common.Data.Base_Height, 0.01, 0.5, 0.5);
                nPinSecondStep = 10;
            }
            else if (nPinSecondStep == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(Common.ProC.Card1.LogPos[0] - Common.Data.Base_Height) < 0.001)
                {
                    nPinSecondStep = 20;
                }
            }
            else if (nPinSecondStep == 20)
            {
                double CurrentX = OmronPLC.GetInstance().GetAddressFloatValue("2246");
                double CurrentY = OmronPLC.GetInstance().GetAddressFloatValue("2250");
                Common.HalObject.SecondPinCalibra.XposList[8 - PointIndex] = CurrentX;
                Common.HalObject.SecondPinCalibra.YposList[8 - PointIndex] = CurrentY;
                Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前标定X轴：[{CurrentX.ToString("0.000")}],Y轴：[{CurrentY.ToString("0.000")}]--");
                Common.Cam2.Snap();
                b_GetSecondPinPoints = true;
                nPinSecondStep = 30;
            }
            else if (nPinSecondStep == 30)
            {
                if (b_GetSecondPinPointsFinish)
                {
                    Common.ProC.Start_T(31, 500);
                    nPinSecondStep = 40;
                }
            }
            else if (nPinSecondStep == 40)
            {
                if (Common.ProC.TimerSignal[31])
                {
                    Common.ProC.Reset_T(31);
                    OmronPLC.GetInstance().Send("10281", PointIndex + 63);
                    nPinSecondStep = -1;
                }
            }

            if (b_GetSecondPinPoints)
            {
                if (Common.QueCam2.Count > 0)
                {
                    HObject ho_Image = Common.QueCam2.Dequeue();
                    displayBase2.DisplaySourceImage(ho_Image);
                    HOperatorSet.WriteImage(ho_Image, "bmp", 0, $"{Common.Data.str_SavePath}\\{PointIndex}.bmp");
                    HTuple hv_LastRow = null, hv_LastCol = null;
                    if (GetFirstPin(displayBase2.SourceImage, out hv_LastRow, out hv_LastCol))
                    {
                        Common.HalObject.SecondPinCalibra.AddImagePoint(PointIndex, hv_LastRow, hv_LastCol, Leo9Calibra.MoveType.AxisMove);
                        Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->第{PointIndex}点图像坐标Row：[{hv_LastRow.D.ToString("0.000")}],Col：[{hv_LastCol.D.ToString("0.000")}]--");

                        if (PointIndex == 8) //最后一个点
                        {
                            Common.HalObject.SecondPinCalibra.GenHomMat2D();
                            Common.ShowMsgEvent.Invoke($"Tips:{ DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->标定2完成！--");
                            OmronPLC.GetInstance().Send("10281", 42);
                        }
                    }
                    else
                    {
                        OmronPLC.GetInstance().Send("10281", 43);
                    }
                    b_GetSecondPinPoints = false;
                    b_GetSecondPinPointsFinish = true;
                }
            }
        }
        public void AutoGoSafeZ()
        {
            if (nZsafe == 0)
            {
                Common.ProC.Card1.AbsoluteMove(0, 4, 0.1, 0.5, 0.5);
                nZsafe = 10;
            }
            else if (nZsafe == 10)
            {
                if (!Common.ProC.Card1.b_Move[0] && Math.Abs(4 - Common.ProC.Card1.LogPos[0]) < 0.001)
                {
                    nZsafe = -1;
                    b_Zsafe = false;
                    OmronPLC.GetInstance().Send("10281", 20);
                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms") + " -->Z已回到安全位置--");
                }
            }
        }
        public void AutoPlatCalibra()
        {
            if (nCalibraStep == 0)
            {
                Common.ShowMsgEvent($"Tips: 开始第{iCalibraIndex}点标定。。");
                Common.ProC.MovePlaX(hv_PhysX.DArr[iCalibraIndex], MoveType.Absolute);
                nCalibraStep = 10;
            }
            else if (nCalibraStep == 10)
            {
                if (!Common.ProC.Card1.b_Move[1] && !Common.ProC.Card1.b_Move[2] && Math.Abs(hv_PhysX.DArr[iCalibraIndex] - Common.ProC.Card1.LogPos[1]) < 0.001 && Math.Abs(hv_PhysX.DArr[iCalibraIndex] - Common.ProC.Card1.LogPos[2]) < 0.001)
                {
                    Common.ProC.Card1.AbsoluteMove(3, hv_PhysY.DArr[iCalibraIndex], 0.1, 0.5, 0.5);
                    nCalibraStep = 20;
                }
            }
            else if (nCalibraStep == 20)
            {
                if (!Common.ProC.Card1.b_Move[3] && Math.Abs(hv_PhysY.DArr[iCalibraIndex] - Common.ProC.Card1.LogPos[3]) < 0.001)
                {
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
                    HObject ho_Circle = new HObject(), ho_SortedRegions = new HObject(), ho_TempImage = new HObject();
                    HTuple hv_Radius = null, hv_Rows = null, hv_Cols = null;
                    HOperatorSet.GenEmptyObj(out ho_TempImage);
                    HOperatorSet.GenEmptyObj(out ho_ThresholdRegion);
                    HOperatorSet.GenEmptyObj(out ho_ConnectionRegion);
                    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                    HOperatorSet.GenEmptyObj(out ho_SortedRegions);

                    HOperatorSet.Threshold(displayBase1.SourceImage, out ho_ThresholdRegion, 180, 255);
                    HOperatorSet.Connection(ho_ThresholdRegion, out ho_ConnectionRegion);
                    HOperatorSet.SelectShapeStd(ho_ConnectionRegion, out ho_SelectedRegions, "max_area", new HTuple(70));
                    HOperatorSet.SmallestCircle(ho_SelectedRegions, out hv_Rows, out hv_Cols, out hv_Radius);
                    HOperatorSet.GenCircle(out ho_Circle, hv_Rows, hv_Cols, hv_Radius);
                    HOperatorSet.SetLineWidth(displayBase1.DisplayHandle, 2);
                    displayBase1.AddRegion(new LeoBase.HsbRegions() { new LeoBase.HsbRegion() { Color = LeoBase.HsbColor.blue, Draw = LeoBase.HsbDraw.margin, Value = ho_Circle } });
                    hv_ImageRows = hv_ImageRows.TupleConcat(hv_Rows);
                    hv_ImageCols = hv_ImageCols.TupleConcat(hv_Cols);
                    if (hv_ImageRows.Length == 9)
                    {
                        Common.HalObject.hom_Plat_ImageToAxis.VectorToHomMat2d(hv_ImageRows, hv_ImageCols, hv_PhysX, hv_PhysY);
                        Common.ShowMsgEvent("Tips: 九点标定完成");
                        iCalibraIndex = -1;
                    }
                }
            }
        }

        public void AutoMatch()
        {
            if (nMatch == 0)
            {
                Common.Cam1.Snap();
                Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms") + " -->芯片相机第一次拍照--");
                nMatch = 10;
            }
            else if (nMatch == 10)
            {
                if (Common.QueCam1.Count > 0)
                {
                    HObject ho_Image = Common.QueCam1.Dequeue();
                    displayBase1.DisplaySourceImage(ho_Image);
                    HTuple hv_Deg2, hv_CenterRow2, hv_CenterCol12;
                    HTuple hv_BaseX = null, hv_BaseY = null, hv_TempX = null, hv_TempY = null;
                    GetPositionAndDeg(displayBase1.SourceImage, out hv_Deg2, out hv_CenterRow2, out hv_CenterCol12);
                    //计算偏移
                    hv_BaseX = Common.HalObject.hom_Plat_ImageToAxis.AffineTransPoint2d(Common.Data.Base_PlatRow, Common.Data.Base_PlatCol, out hv_BaseY);
                    hv_TempX = Common.HalObject.hom_Plat_ImageToAxis.AffineTransPoint2d(hv_CenterRow2, hv_CenterCol12, out hv_TempY);
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准芯片X：[{hv_BaseX.D.ToString("0.000")}]，Y:[{hv_BaseY.D.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前芯片X：[{hv_TempX.D.ToString("0.000")}]，Y:[{hv_TempY.D.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->针卡补偿X：[{Common.Data.OffsetX.ToString("0.000")}]，Y:[{Common.Data.OffsetY.ToString("0.000")}]--");

                    offsetDeg = (Common.Data.Base_PlatDeg + Common.Data.OffsetDeg) - hv_Deg2.D;
                    offsetX = (hv_BaseX - Common.Data.OffsetX) - hv_TempX;
                    offsetY = (hv_BaseY + Common.Data.OffsetY) - hv_TempY;
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->产品角度偏移：[{offsetDeg.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->产品X偏移：[{offsetX.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->产品Y偏移：[{offsetY.ToString("0.000")}]--");

                    //Common.ProC.MovePlaU(offsetDeg, MoveType.Relative);
                    double MoveX1 = 56.568 * Math.Cos((offsetDeg + 45) * Math.PI / 180) - 56.568 * Math.Cos(45 * Math.PI / 180) + offsetX;
                    double MoveX2 = 56.568 * Math.Cos((offsetDeg + 315) * Math.PI / 180) - 56.568 * Math.Cos(315 * Math.PI / 180) + offsetX;
                    double MoveY = 56.568 * Math.Sin((offsetDeg + 135) * Math.PI / 180) - 56.568 * Math.Sin(135 * Math.PI / 180) + offsetY;

                    Common.ProC.Card1.AxisPmove(1, MoveX1);
                    Common.ProC.Card1.AxisPmove(2, MoveX2);
                    Common.ProC.Card1.AxisPmove(3, MoveY);
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
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->芯片相机第二次拍照--");
                    nMatch = 40;
                }
            }
            else if (nMatch == 40)
            {
                if (Common.QueCam1.Count > 0)
                {
                    HObject ho_Image = Common.QueCam1.Dequeue();
                    displayBase1.DisplaySourceImage(ho_Image);
                    HTuple hv_Deg2, hv_CenterRow2, hv_CenterCol12;
                    HTuple hv_BaseX = null, hv_BaseY = null, hv_TempX = null, hv_TempY = null;
                    GetPositionAndDeg(displayBase1.SourceImage, out hv_Deg2, out hv_CenterRow2, out hv_CenterCol12);
                    //计算偏移
                    hv_BaseX = Common.HalObject.hom_Plat_ImageToAxis.AffineTransPoint2d(Common.Data.Base_PlatRow, Common.Data.Base_PlatCol, out hv_BaseY);
                    hv_TempX = Common.HalObject.hom_Plat_ImageToAxis.AffineTransPoint2d(hv_CenterRow2, hv_CenterCol12, out hv_TempY);
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->基准芯片X：[{hv_BaseX.D.ToString("0.000")}]，Y:[{hv_BaseY.D.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前芯片X：[{hv_TempX.D.ToString("0.000")}]，Y:[{hv_TempY.D.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->针卡补偿X：[{Common.Data.OffsetX.ToString("0.000")}]，Y:[{Common.Data.OffsetY.ToString("0.000")}]--");

                    offsetDeg = (Common.Data.Base_PlatDeg + Common.Data.OffsetDeg) - hv_Deg2.D;
                    offsetX = (hv_BaseX - Common.Data.OffsetX) - hv_TempX;
                    offsetY = (hv_BaseY + Common.Data.OffsetY) - hv_TempY;
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->产品角度偏移：[{offsetDeg.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->产品X偏移：[{offsetX.ToString("0.000")}]--");
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->产品Y偏移：[{offsetY.ToString("0.000")}]--");

                    //Common.ProC.MovePlaU(offsetDeg, MoveType.Relative);
                    double MoveX1 = 56.568 * Math.Cos((offsetDeg + 45) * Math.PI / 180) - 56.568 * Math.Cos(45 * Math.PI / 180) + offsetX;
                    double MoveX2 = 56.568 * Math.Cos((offsetDeg + 315) * Math.PI / 180) - 56.568 * Math.Cos(315 * Math.PI / 180) + offsetX;
                    double MoveY = 56.568 * Math.Sin((offsetDeg + 135) * Math.PI / 180) - 56.568 * Math.Sin(135 * Math.PI / 180) + offsetY;

                    Common.ProC.Card1.AxisPmove(1, MoveX1);
                    Common.ProC.Card1.AxisPmove(2, MoveX2);
                    Common.ProC.Card1.AxisPmove(3, MoveY);
                    nMatch = 45;
                }
            }
            else if (nMatch == 45)
            {
                if (!Common.ProC.Card1.b_Move[1] && !Common.ProC.Card1.b_Move[2] && !Common.ProC.Card1.b_Move[3])
                {
                    //发完成信号
                    nMatch = -1;
                    b_Match = false; //对位完成
                    Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->对位完成--");
                    OmronPLC.GetInstance().Send("10280", 33);
                }
            }
        }
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

            Common.ShowMsgEvent.Invoke($"Tips: { DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms")} -->当前芯片 长：[{(hv_Length11.D * 2).ToString("0.000")}]，宽:[{(hv_Length21.D * 2).ToString("0.000")}]--");


        }

    }
}
