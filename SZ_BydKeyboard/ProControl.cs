using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeoMotion;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using LeoLink;
using Newtonsoft.Json;

namespace SZ_BydKeyboard
{
    [Serializable]
    public class ProControl
    {

        public bool bFix1 = false, bFix2 = false;

        //点位保存
        public double[] PositionList = new double[30];
        //延时保存
        public Int32[] DelayList = new Int32[30];
        //手轮输入
        public bool[] MpgInput = new bool[] { };

        //[NonSerialized]
        public GoogolTech Card1;  // 4轴卡
        [NonSerialized]
        public ExGoogolTech ExCard;

        [NonSerialized]
        public static ProControl _ProC;

        public static ProControl GetInstance()
        {
            if (_ProC == null)
            {
                _ProC = new ProControl();
            }
            return _ProC;
        }

        private ProControl()
        {

        }

        #region 卡1轴号及输入输出
        public enum Card1Axis
        {
            针卡相机Z轴 = 0,
            芯片平台X1轴 = 1,
            芯片平台X2轴 = 2,
            芯片平台Y轴 = 3,
        }

        public enum Card1Input
        {
            治具1气缸1负限 = 0,
            治具1气缸1正限 = 1,
            治具1气缸2负限 = 2,
            治具1气缸2正限 = 3,
            治具2气缸1负限 = 4,
            治具2气缸1正限 = 5,
            治具2气缸2负限 = 6,
            治具2气缸2正限 = 7,
            治具1感应 = 8,
            治具2感应 = 9,
            治具1安全光栅 = 10,
            治具2安全光栅 = 11,
            治具1放料完成 = 12,
            治具2放料完成 = 13,
            输入14 = 14,
            输入15 = 15
        }

        public enum Card1Output
        {
            治具1气缸1前进 = 0,
            治具1气缸1后退 = 1,
            治具1气缸2前进 = 2,
            治具1气缸2后退 = 3,
            治具2气缸1前进 = 4,
            治具2气缸1后退 = 5,
            治具2气缸2前进 = 6,
            治具2气缸2后退 = 7,
            绿灯 = 8,
            红灯 = 9,
            黄灯 = 10,
            蜂鸣器 = 11,
            治具1接料许可 = 12,
            治具2接料许可 = 13,
            报警输出 = 14,
            Z轴刹车 = 15
        }
        public enum ExInput
        {
            正面左门 = 0,
            正面右门,
            背面左门,
            背面右门,
            治具1对射感应,
            治具2对射感应,
            工位1放料完成,
            工位2放料完成,
            启动,
            复位,
            停止
        }
        public enum ExOutput
        {
            工位1允许放料 = 0,
            输出2,
            工位1检测OK,
            工位1检测NG,
            工位1复测,
            工位2允许放料,
            输出7,
            工位2检测OK,
            工位2检测NG,
            工位2复测,
            机台报警
        }


        #endregion

        #region

        public enum Timer
        {
            来料感应 = 0,
            左前剥料感应 = 1,
            左后剥料感应 = 2,
            右前剥料感应 = 3,
            右后剥料感应 = 4,
            左前真空吸气 = 5,
            左后真空吸气 = 6,
            左前真空放气 = 7,
            左后真空放气 = 8,
            右前真空吸气 = 9,
            右后真空吸气 = 10,
            右前真空放气 = 11,
            右后真空放气 = 12,
            左前电机1延时停止 = 13,
            左后电机1延时停止 = 14,
            右前电机1延时停止 = 15,
            右后电机1延时停止 = 16
        }

        #endregion

        //public int[] Time = new int[17];


        public bool[] TimerSignal = new bool[40];
        public bool[] TimerRun = new bool[40];
        public int[] SetTimer = new int[40];
        public int[] AddTimer = new int[40];
        private int ScanTime = 20;
        public void Start_T(int i_timerNum, int SetTime)//定时器开启
        {
            SetTimer[i_timerNum] = SetTime / ScanTime;
            TimerSignal[i_timerNum] = false;
            TimerRun[i_timerNum] = true;
            AddTimer[i_timerNum] = 0;
        }
        public void Reset_T(int i_timerNum)//复位定时器，并关闭信号
        {
            TimerSignal[i_timerNum] = false;
            TimerRun[i_timerNum] = false;
            AddTimer[i_timerNum] = 0;
        }
        public void Control_T()//内部定时器计时
        {
            for (int i = 0; i < 40; i++)
            {
                if (TimerRun[i])
                {
                    AddTimer[i]++;
                    if (AddTimer[i] >= SetTimer[i])
                    {
                        TimerSignal[i] = true;
                        TimerRun[i] = false;
                        AddTimer[i] = 0;
                    }
                }
            }
        }

        public bool InitParam()
        {
            try
            {
                if (Card1 == null)
                {
                    Card1 = new GoogolTech(0, 4);
                    Card1.AxisName = new string[] { "X轴", "Y1轴", "Y2轴", "Z轴" };
                    Card1.InputNames = new string[] { "治具1气缸1负限", "治具1气缸1正限", "治具1气缸2负限", "治具1气缸2正限", "治具2气缸1负限", "治具2气缸1正限", "治具2气缸2负限", "治具2气缸2正限",
                        "治具1感应", "治具2感应","治具1安全光栅", "治具2安全光栅", "治具1放料完成", "治具2放料完成",  "输入14", "输入15" };
                    Card1.OutputNames = new string[] { "治具1气缸1前进", "治具1气缸1后退", "治具1气缸2前进", "治具1气缸2后退", "治具2气缸1前进", "治具2气缸1后退", "治具2气缸2前进", "治具2气缸2后退",
                        "绿灯", "红灯", "黄灯","蜂鸣器", "输出13", "输出14", "报警输出", "Z轴刹车" };
                }
                Card1.cfgPath = $"{Application.StartupPath}\\SysCfg\\Card.cfg";
                if (Card1.InitBoard() == 0)
                {
                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + " -->运动控制卡初始化成功--");
                    //Card1.SetHardStop(2, 0, 0);
                    for (int i = 0; i < Card1.AxisName.Length; i++)
                    {
                        //Card1.SetPulseMode(i, 3); //0-2 为伺服
                        //Card1.SetLimitMode(i, 0, 0, 1); // 000 正负限位都启用 高电平有效
                        Card1.SetSpeed(i, Card1.StartVList[i], Card1.SpeedList[i], Card1.Aspeed[i], Card1.TaccList[i]);
                    }
                }
                else
                {
                    Common.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + " -->运动控制卡初始化失败--");
                }

                if (ExCard == null)
                {
                    ExCard = new ExGoogolTech();
                    ExCard.InputList = new bool[16];
                    ExCard.InputNames = new string[] { "正面左门", "正面右门", "背面左门", "背面右门", "治具1对射感应", "治具2对射感应", "工位1放料完成", "工位2放料完成", "启动", "复位", "停止", "输入12", "输入13", "输入14", "输入15", "输入16" };
                    ExCard.OutputNames = new string[] { "工位1允许放料", "输出2", "工位1检测OK", "工位1检测NG", "工位1复测", "工位2允许放料", "输出7", "工位2检测OK", "工位2检测NG", "工位2复测", "机台报警", "输出12", "输出13", "输出14", "输出15", "输出16" };
                }
                if (ExCard.InitCard(0, 1))
                {
                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + " -->拓展IO卡初始化成功--");
                }
                else
                {
                    Common.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + " -->拓展IO卡初始化失败--");
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff -->") + string.Format("运动控制卡初始化异常，原因为{0}--", ex.Message));
                return false;
            }
        }

        public void GetStatus()
        {
            Card1.GetCurrentInf();
            ExCard.GetAllInput();
            MpgInput = Card1.GetState(GoogolTech.StateType.MPG);
        }

        public void SetCardSoftLmt()
        {
            for (int i = 0; i < Common.ProC.Card1.SoftLmtEnable.Length; i++)
            {
                if (Common.ProC.Card1.SoftLmtEnable[i])
                {
                    Common.ProC.Card1.SetSoftLmt(i, 1, 0, 0, Common.ProC.Card1.NSoftLmt[i], Common.ProC.Card1.PSoftLmt[i]);
                }
                else
                {
                    Common.ProC.Card1.SetSoftLmt(i, 0, 0, 0, Common.ProC.Card1.NSoftLmt[i], Common.ProC.Card1.PSoftLmt[i]);
                }
            }
        }

        public void CancelCardSoftLmt()
        {
            for (int i = 0; i < Common.ProC.Card1.SoftLmtEnable.Length; i++)
            {
                Common.ProC.Card1.SetSoftLmt(i, 0, 0, 0, Common.ProC.Card1.NSoftLmt[i], Common.ProC.Card1.PSoftLmt[i]);
            }
        }

        public bool bHome = false;
        public async void AllAxisGohome()
        {
            Common.ShowMsgEvent("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff -->") + string.Format("开始回原点，请等待。。。--"));
            Card1.WriteOutput(Card1Output.Z轴刹车.GetHashCode(), 0);
            CancelCardSoftLmt();
            List<Task<bool>> taskList = new List<Task<bool>>();
            taskList.Add(Task<bool>.Run(() =>
            {
                return Card1.GoNLmt(0, 30); //负方向回
            }));
            Thread.Sleep(20);
            for (int i = 1; i < 4; i++)
            {
                int axis = i;
                taskList.Add(Task<bool>.Run(() =>
                {
                    return Card1.GoNLmt(axis, 50); //负方向回
                }));
                Thread.Sleep(20);
            }
            var b = await Task.WhenAll(taskList);
            bHome = true;
            Common.ShowMsgEvent("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff -->") + string.Format("所有轴回原完成--"));
            SetCardSoftLmt();
            for (int i = 0; i < 4; i++)
            {
                Card1.SetSpeed(i, Card1.StartVList[i], Card1.SpeedList[i], Card1.Aspeed[i], Card1.TaccList[i]);
            }
        }

        public void LoadParam()
        {
            string fp = $"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\MotionSetting.json";
            if (File.Exists(fp))
            {
                _ProC = JsonConvert.DeserializeObject<ProControl>(File.ReadAllText(fp));
            }
            else
            {
                _ProC = new ProControl();
            }
        }

        public void SaveParam()
        {
            string result = JsonConvert.SerializeObject(_ProC);
            if (!Directory.Exists($"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\"))
            {
                Directory.CreateDirectory($"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\");
            }
            string fp = $"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\MotionSetting.json";
            if (!File.Exists(fp))  // 判断是否已有相同文件 
            {
                FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                fs1.Close();
            }
            File.WriteAllText(fp, result.Replace(",\"", ",\r\n\""));
        }

        [NonSerialized]
        public int nCheckStep1 = -1, nCheckStep2 = -1;  //指示目前检测步骤
        [NonSerialized]
        public int nCheckIdx = -1;    //指示当前是第几个检测项
        [NonSerialized]
        public int nStation1 = -1, nStation2 = -1;
        [NonSerialized]
        public int nHeightMeasure = -1, nHeightIdx = 0;
        [NonSerialized]
        public bool StationResult1, StationResult2;


        public void AutoTestCheck1()
        {
            if (nCheckStep1 == 0)
            {
                CheckPos pos = Common.Data.DicCheck[Common.FaiNames[nCheckIdx]];
                Card1.AbsoluteMove(0, pos.Xpos, Card1.StartVList[0], Card1.SpeedList[0], Card1.Aspeed[0], Card1.TaccList[0]);
                Card1.AbsoluteMove(1, pos.Ypos, Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                Card1.AbsoluteMove(3, pos.Zpos, Card1.StartVList[3], Card1.SpeedList[3], Card1.Aspeed[3], Card1.TaccList[3]);
                if (Common.LightControlPort.IsOpen)
                {
                    Common.LightControlPort.Write($"SA{pos.Channel1.ToString().PadLeft(4, '0')}#\r\n");
                    Common.LightControlPort.Write($"SB{pos.Channel2.ToString().PadLeft(4, '0')}#\r\n");
                }
                nCheckStep1 = 10;
            }
            else if (nCheckStep1 == 10)
            {
                if (!Card1.b_Move[0] && !Card1.b_Move[1] && !Card1.b_Move[3])
                {
                    Common.Cam1.Snap();
                    Start_T(0, 160);
                    nCheckStep1 = 20;
                }
            }
            else if (nCheckStep1 == 20)
            {
                if (TimerSignal[0])
                {
                    Common.ShowMsgEvent($"Tips:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}---第{nCheckIdx + 1}次拍照完成！");
                    Reset_T(0);
                    if (nCheckIdx == Common.Data.DicCheck.Count - 1)
                    {
                        nCheckIdx = 0;
                        nCheckStep1 = -1;
                        nCheckStep2 = 0; //拍完了到工站2拍
                    }
                    else
                    {
                        nCheckIdx++;
                        nCheckStep1 = 0;
                    }
                }
            }

            if (nCheckStep2 == 0)
            {
                CheckPos pos = Common.Data.DicCheck[Common.FaiNames[nCheckIdx]];
                Card1.AbsoluteMove(0, pos.Xpos + Common.Data.OffsetX2toX1, Card1.StartVList[0], Card1.SpeedList[0], Card1.Aspeed[0], Card1.TaccList[0]);
                Card1.AbsoluteMove(2, pos.Ypos + Common.Data.OffsetY2toY1, Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                Card1.AbsoluteMove(3, pos.Zpos + Common.Data.OffsetZ2toZ1, Card1.StartVList[3], Card1.SpeedList[3], Card1.Aspeed[3], Card1.TaccList[3]);
                if (Common.LightControlPort.IsOpen)
                {
                    Common.LightControlPort.Write($"SA{pos.Channel1.ToString().PadLeft(4, '0')}#\r\n");
                    Common.LightControlPort.Write($"SB{pos.Channel2.ToString().PadLeft(4, '0')}#\r\n");
                }
                nCheckStep2 = 10;
            }
            else if (nCheckStep2 == 10)
            {
                if (!Card1.b_Move[0] && !Card1.b_Move[2] && !Card1.b_Move[3])
                {
                    Common.Cam1.Snap();
                    Start_T(1, 100);
                    nCheckStep2 = 20;
                }
            }
            else if (nCheckStep2 == 20)
            {
                if (TimerSignal[1])
                {
                    Reset_T(1);
                    if (nCheckIdx == Common.Data.DicCheck.Count - 1)
                    {
                        nCheckIdx = 0;
                        nCheckStep2 = -1;
                        nCheckStep1 = 0; //拍完了回工站1拍
                    }
                    else
                    {
                        nCheckIdx++;
                        nCheckStep2 = 0;
                    }
                }
            }
        }

        public void AutoMeasureHeight()
        {
            if (nHeightMeasure == 0)
            {
                if (Common.LightControlPort.IsOpen)
                {
                    Common.LightControlPort.Write($"SA0000#\r\n");
                    Common.LightControlPort.Write($"SB0000#\r\n");
                }
                nHeightMeasure = 5;
            }
            else if (nHeightMeasure == 5)
            {
                if (Common.HeightMeasureX.Count == 6)
                {
                    if (nCheckStep1 == 20)
                    {
                        Card1.AbsoluteMove(0, Common.HeightMeasureX[nHeightIdx], Card1.StartVList[0], Card1.SpeedList[0], Card1.Aspeed[0], Card1.TaccList[0]);
                        Card1.AbsoluteMove(1, Common.HeightMeasureY[nHeightIdx], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                    }
                    else if (nCheckStep2 == 20)
                    {
                        Card1.AbsoluteMove(0, Common.HeightMeasureX[nHeightIdx], Card1.StartVList[0], Card1.SpeedList[0], Card1.Aspeed[0], Card1.TaccList[0]);
                        Card1.AbsoluteMove(2, Common.HeightMeasureY[nHeightIdx], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                    }
                    nHeightMeasure = 10;
                }
            }
            else if (nHeightMeasure == 10)
            {
                if ((!Card1.b_Move[1] && !Card1.b_Move[0] && nCheckStep1 == 20) || (!Card1.b_Move[2] && !Card1.b_Move[0] && nCheckStep2 == 20))
                {
                    Start_T(25, 20);
                    nHeightMeasure = 15;
                }
            }
            else if (nHeightMeasure == 15)
            {
                if (TimerSignal[25])
                {
                    Reset_T(25);
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
                        nHeightMeasure = 20;
                    }
                    else
                    {
                        MessageBox.Show("请检查测高仪连接情况！", "警告", MessageBoxButtons.OK);
                        nHeightMeasure = -1;
                    }
                }
            }
            else if (nHeightMeasure == 30)
            {
                if (nHeightIdx == Common.HeightMeasureX.Count - 1)
                {
                    float[] P1 = new float[] { (float)Common.HeightMeasureX[0], (float)Common.HeightMeasureY[0], (float)Common.HeightMeasureZ[0] };
                    float[] P2 = new float[] { (float)Common.HeightMeasureX[1], (float)Common.HeightMeasureY[1], (float)Common.HeightMeasureZ[1] };
                    float[] P3 = new float[] { (float)Common.HeightMeasureX[2], (float)Common.HeightMeasureY[2], (float)Common.HeightMeasureZ[2] };
                    float[] P4 = new float[] { (float)Common.HeightMeasureX[3], (float)Common.HeightMeasureY[3], (float)Common.HeightMeasureZ[3] };
                    float[] P5 = new float[] { (float)Common.HeightMeasureX[4], (float)Common.HeightMeasureY[4], (float)Common.HeightMeasureZ[4] };
                    float[] P6 = new float[] { (float)Common.HeightMeasureX[5], (float)Common.HeightMeasureY[5], (float)Common.HeightMeasureZ[5] };
                    double Height1 = HeightMeasure.GetHeight(P1, P2, P3, P4) * -1;
                    double Height2 = HeightMeasure.GetHeight(P1, P2, P3, P5) * -1;
                    double Height3 = HeightMeasure.GetHeight(P1, P2, P3, P6) * -1;
                    double AverageHeight = (Height1 + Height2 + Height3) / 3;
                    if (bFix1)
                    {
                        DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_P1")] = Height1.ToString("0.0000");
                        DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_P2")] = Height2.ToString("0.0000");
                        DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_P3")] = Height3.ToString("0.0000");
                        DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_Average")] = AverageHeight.ToString("0.0000");
                    }
                    else
                    {
                        DataHelper.Values2[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_P1")] = Height1.ToString("0.0000");
                        DataHelper.Values2[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_P2")] = Height2.ToString("0.0000");
                        DataHelper.Values2[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_P3")] = Height3.ToString("0.0000");
                        DataHelper.Values2[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_Average")] = AverageHeight.ToString("0.0000");
                    }
                    //Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + $" -->B2B 测得高度为[{height.ToString("0.0000")}]--");
                    //DataHelper.Values[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B_Height")] = height.ToString("0.0000");
                    if (AverageHeight < Common.Data.LSL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB jumper flex B2B_Average")] ||
                        AverageHeight > Common.Data.USL[DataHelper.MeasureNames.ToList<string>().IndexOf("MLB jumper flex B2B_Average")])
                    {
                        Common.Data.NgCount[9] = Common.Data.NgCount[9];
                        if (bFix1)
                        {
                            DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B")] = "NG";
                        }
                        else
                        {
                            DataHelper.Values2[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B")] = "NG";
                        }
                        Common.SetB2B(false);
                        Common.nResultList.Add(1);
                    }
                    else
                    {
                        if (bFix1)
                        {
                            DataHelper.Values1[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B")] = "OK";
                        }
                        else
                        {
                            DataHelper.Values2[DataHelper.Names.ToList<string>().IndexOf("MLB jumper flex B2B")] = "OK";
                        }
                        Common.SetB2B(true);
                        Common.nResultList.Add(0);
                    }

                    Common.HeightMeasureZ = new List<double>();
                    Common.HeightMeasureX = new List<double>();
                    Common.HeightMeasureY = new List<double>();
                    if (nCheckStep1 == 20)
                    {
                        nCheckStep1 = 30;
                    }
                    else if (nCheckStep2 == 20)
                    {
                        nCheckStep2 = 30;
                    }
                    nHeightMeasure = -1;
                    nHeightIdx = 0;
                }
                else
                {
                    nHeightIdx++;
                    nHeightMeasure = 5;
                }
            }
        }

        public void AutoCheck1()
        {
            if (nCheckStep1 == 0)
            {
                CheckPos pos = Common.Data.DicCheck[Common.FaiNames[nCheckIdx]];
                Card1.AbsoluteMove(0, pos.Xpos, Card1.StartVList[0], Card1.SpeedList[0], Card1.Aspeed[0], Card1.TaccList[0]);
                Card1.AbsoluteMove(1, pos.Ypos, Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                Card1.AbsoluteMove(3, pos.Zpos, Card1.StartVList[3], Card1.SpeedList[3], Card1.Aspeed[3], Card1.TaccList[3]);
                if (Common.LightControlPort.IsOpen)
                {
                    Common.LightControlPort.Write($"SA{pos.Channel1.ToString().PadLeft(4, '0')}#SB{pos.Channel2.ToString().PadLeft(4, '0')}#\r\n");
                }
                Start_T(30, 40);
                nCheckStep1 = 10;
            }
            else if (nCheckStep1 == 10)
            {
                if (!Card1.b_Move[0] && !Card1.b_Move[1] && !Card1.b_Move[3] && TimerSignal[30])
                {
                    Reset_T(30);
                    Common.Cam1.Snap();
                    Start_T(0, 80);
                    nCheckStep1 = 20;
                }
            }
            else if (nCheckStep1 == 20)
            {
                if (TimerSignal[0])
                {
                    if (Common.FaiNames[nCheckIdx] == "MLB jumper flex B2B")
                    {
                        if (Common.HeightMeasureX.Count == 6)
                        {
                            Reset_T(0);
                            //开始测高
                            nHeightMeasure = 0;
                        }
                    }
                    else
                    {
                        Reset_T(0);
                        nCheckStep1 = 30;
                    }
                }
            }
            else if (nCheckStep1 == 30)
            {
                if (nCheckIdx == Common.Data.DicCheck.Count - 1)
                {
                    nCheckIdx = 0;
                    nCheckStep1 = -1;
                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + $" -->测试完成--");
                }
                else
                {
                    nCheckIdx++;
                    nCheckStep1 = 0;
                }
            }
        }

        public void AutoCheck2()
        {
            if (nCheckStep2 == 0)
            {

                CheckPos pos = Common.Data.DicCheck[Common.FaiNames[nCheckIdx]];
                Card1.AbsoluteMove(0, pos.Xpos + Common.Data.OffsetX2toX1, Card1.StartVList[0], Card1.SpeedList[0], Card1.Aspeed[0], Card1.TaccList[0]);
                Card1.AbsoluteMove(2, pos.Ypos + Common.Data.OffsetY2toY1, Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                Card1.AbsoluteMove(3, pos.Zpos + Common.Data.OffsetZ2toZ1, Card1.StartVList[3], Card1.SpeedList[3], Card1.Aspeed[3], Card1.TaccList[3]);
                if (Common.LightControlPort.IsOpen)
                {
                    Common.LightControlPort.Write($"SA{pos.Channel1.ToString().PadLeft(4, '0')}#SB{pos.Channel2.ToString().PadLeft(4, '0')}#\r\n");
                }
                Start_T(30, 40);
                nCheckStep2 = 10;
            }
            else if (nCheckStep2 == 10)
            {
                if (!Card1.b_Move[0] && !Card1.b_Move[2] && !Card1.b_Move[3] && TimerSignal[30])
                {
                    Reset_T(30);
                    Common.Cam1.Snap();
                    Start_T(1, 80);
                    nCheckStep2 = 20;
                }
            }
            else if (nCheckStep2 == 20)
            {
                if (TimerSignal[1])
                {
                    if (Common.FaiNames[nCheckIdx] == "MLB jumper flex B2B")
                    {
                        if (Common.HeightMeasureX.Count == 6)
                        {
                            Reset_T(1);
                            //开始测高
                            nHeightMeasure = 0;
                        }
                    }
                    else
                    {
                        Reset_T(1);
                        nCheckStep2 = 30;
                    }
                }
            }
            else if (nCheckStep2 == 30)
            {
                if (nCheckIdx == Common.Data.DicCheck.Count - 1)
                {
                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + $" -->测试完成--");
                    nCheckIdx = 0;
                    nCheckStep2 = -1;
                }
                else
                {
                    nCheckIdx++;
                    nCheckStep2 = 0;
                }
            }
        }

        public void OpenGreenLight()
        {
            Card1.WriteOutput(Card1Output.红灯.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.黄灯.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.绿灯.GetHashCode(), 0);
        }

        public void OpenRedLight()
        {
            Card1.WriteOutput(Card1Output.红灯.GetHashCode(), 0);
            Card1.WriteOutput(Card1Output.黄灯.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.绿灯.GetHashCode(), 1);
        }

        public void OpenYellowLight()
        {
            Card1.WriteOutput(Card1Output.红灯.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.黄灯.GetHashCode(), 0);
            Card1.WriteOutput(Card1Output.绿灯.GetHashCode(), 1);
        }

        public void OpenCylinder()
        {
            Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 0);
            Thread.Sleep(50);
            Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 0);
            Thread.Sleep(50);
            Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 0);
            Thread.Sleep(50);
            Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 1);
            Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 0);
        }


        public int iCloseCylinder1 = -1, iCloseCylinder2 = -1;

        public void AutoCloseCylinder1()
        {
            if (iCloseCylinder1 == 0) //长边夹
            {
                Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 0);
                Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 1);
                Start_T(10, 100);
                iCloseCylinder1 = 10;
            }
            else if (iCloseCylinder1 == 10)//长边松
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 0);
                    Start_T(10, 100);
                    iCloseCylinder1 = 20;
                }
            }
            else if (iCloseCylinder1 == 20)//短边夹
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 1);
                    Start_T(10, 100);
                    iCloseCylinder1 = 30;
                }
            }
            else if (iCloseCylinder1 == 30)//短边松
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 0);
                    Start_T(10, 100);
                    iCloseCylinder1 = 40;
                }
            }
            else if (iCloseCylinder1 == 40)//长边夹
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 1);
                    Start_T(10, 100);
                    iCloseCylinder1 = 50;
                }
            }
            else if (iCloseCylinder1 == 50)//短边夹
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 1);
                    iCloseCylinder1 = -1;
                }
            }
        }

        public void AutoCloseCylinder2()
        {
            if (iCloseCylinder2 == 0) //长边夹
            {
                Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 0);
                Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 1);
                Start_T(10, 100);
                iCloseCylinder2 = 10;
            }
            else if (iCloseCylinder2 == 10)//长边松
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 0);
                    Start_T(10, 100);
                    iCloseCylinder2 = 20;
                }
            }
            else if (iCloseCylinder2 == 20)//短边夹
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 1);
                    Start_T(10, 100);
                    iCloseCylinder2 = 30;
                }
            }
            else if (iCloseCylinder2 == 30)//短边松
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 0);
                    Start_T(10, 100);
                    iCloseCylinder2 = 40;
                }
            }
            else if (iCloseCylinder2 == 40)//长边夹
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 1);
                    Start_T(10, 100);
                    iCloseCylinder2 = 50;
                }
            }
            else if (iCloseCylinder2 == 50)//短边夹
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 1);
                    iCloseCylinder2 = -1;
                }
            }
        }



        int readDataCount1 = 0, readDataCount2 = 0;

        public void Station1WorksOnline()
        {
            if (nStation1 == 0)
            {
                Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 0);
                Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 0);
                Start_T(10, 100);
                nStation1 = 5;
            }
            else if (nStation1 == 5)
            {
                if (TimerSignal[10])
                {
                    Reset_T(10);
                    Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 0);
                    Card1.AbsoluteMove(1, Common.Data.DictPos["取料位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                    nStation1 = 10;
                }
            }
            else if (nStation1 == 10)
            {
                if (Card1.Singal[Card1Input.治具1气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具1气缸2正限.GetHashCode()] && !Card1.b_Move[1])
                {
                    ExCard.WriteOutput((ushort)ExOutput.工位1允许放料.GetHashCode(), 0);
                    nStation1 = 20;
                }
            }
            else if (nStation1 == 20)
            {
                if (Card1.Singal[Card1Input.治具1感应.GetHashCode()] && ExCard.InputList[ExInput.工位1放料完成.GetHashCode()] && !ExCard.InputList[ExInput.治具1对射感应.GetHashCode()])
                {
                    ExCard.WriteOutput((ushort)ExOutput.工位1允许放料.GetHashCode(), 1);
                    iCloseCylinder1 = 0; //开始夹料
                    Card1.AbsoluteMove(1, Common.Data.DictPos["扫码位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                    nStation1 = 40;
                }
            }
            else if (nStation1 == 40)
            {
                if (Card1.Singal[Card1Input.治具1气缸1负限.GetHashCode()] && Card1.Singal[Card1Input.治具1气缸2负限.GetHashCode()] && !Card1.b_Move[1] && iCloseCylinder1 == -1)
                {
                    //读码
                    if (Common.DataReadPort1.IsOpen)
                    {
                        Common.str_DataCode1 = "";
                        Common.DataReadPort1.Write("T\r\n");
                        readDataCount1++;
                        Start_T(12, 100);
                    }
                    nStation1 = 50;
                }
            }
            else if (nStation1 == 50)
            {
                if (TimerSignal[12])
                {
                    Reset_T(12);
                    if (Common.str_DataCode1 != "")
                    {
                        readDataCount1 = 0;
                        //读到码。。
                        Card1.AbsoluteMove(1, Common.Data.DictPos["等待位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                        nStation1 = 60;
                    }
                    else
                    {
                        if (readDataCount1 < 5)
                        {
                            nStation1 = 40; //扫不到码 再读一次
                        }
                        else
                        {
                            readDataCount1 = 0;
                            nStation1 = 70;
                            StationResult1 = false;
                        }
                    }
                }
            }
            else if (nStation1 == 60)
            {
                if (!Card1.b_Move[1])
                {
                    if (DataHelper.QueueUp(Common.str_DataCode1))
                    {
                        nStation1 = 65;
                    }
                    else
                    {
                        Common.NgSnList.Add(Common.str_DataCode1);
                        StationResult1 = false;
                        nStation1 = 70;
                    }
                }
            }
            else if (nStation1 == 65)
            {
                if (nCheckStep2 == -1)
                {
                    bFix1 = true;
                    Common.str_CurrentDataCode = Common.str_DataCode1;
                    //"SN","station_id","Time","FixtrueName", "Config",
                    DataHelper.Values1 = new string[DataHelper.Names.Length];
                    DataHelper.Values1[0] = Common.str_DataCode1;
                    DataHelper.Values1[1] = Common.mes.station_id;
                    DataHelper.Values1[2] = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}";
                    DataHelper.Values1[3] = $"Fix_1";

                    Common.ShowDataCode(Common.str_CurrentDataCode);
                    nCheckStep1 = 0; //开始检测
                    //Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + $" -->开始测试--");
                    Common.stop.Restart();
                    nStation1 = 70;
                }
            }
            else if (nStation1 == 70)
            {
                if (nCheckStep1 == -1) //检测完成
                {
                    TimeSpan ts = Common.stop.Elapsed;
                    // Format and display the TimeSpan value.
                    string elapsedTime = $"{ts.Seconds}秒{ts.Milliseconds}毫秒";
                    Common.ShowInspectTime(elapsedTime);
                    Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 0);
                    Start_T(13, 100);
                    nStation1 = 75;
                }
            }
            else if (nStation1 == 75)
            {
                if (TimerSignal[13])
                {
                    Reset_T(13);
                    Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 0);
                    Card1.AbsoluteMove(1, Common.Data.DictPos["放料位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                    nStation1 = 80;
                }
            }
            else if (nStation1 == 80)
            {
                if (Card1.Singal[Card1Input.治具1气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具1气缸2正限.GetHashCode()] && !Card1.b_Move[1])
                {
                    //指示产品OK或NG
                    if (StationResult1)
                    {
                        ExCard.WriteOutput((ushort)ExOutput.工位1检测OK.GetHashCode(), 0);
                        ExCard.WriteOutput((ushort)ExOutput.工位1检测NG.GetHashCode(), 1);
                        ExCard.WriteOutput((ushort)ExOutput.工位1复测.GetHashCode(), 1);
                        ExCard.WriteOutput((ushort)ExOutput.工位1允许放料.GetHashCode(), 0);
                        nStation1 = 90;
                    }
                    else
                    {
                        if (Common.str_DataCode1 == "")
                        {
                            ExCard.WriteOutput((ushort)ExOutput.工位1复测.GetHashCode(), 1);
                            ExCard.WriteOutput((ushort)ExOutput.工位1检测OK.GetHashCode(), 1);
                            ExCard.WriteOutput((ushort)ExOutput.工位1检测NG.GetHashCode(), 0);
                        }
                        else
                        {
                            if (Common.NgSnList.Contains(Common.str_DataCode1)) //已经是复测了
                            {
                                ExCard.WriteOutput((ushort)ExOutput.工位1复测.GetHashCode(), 1);
                                ExCard.WriteOutput((ushort)ExOutput.工位1检测OK.GetHashCode(), 1);
                                ExCard.WriteOutput((ushort)ExOutput.工位1检测NG.GetHashCode(), 0);
                            }
                            else
                            {
                                Common.NgSnList.Add(Common.str_DataCode1);    //第一次NG加入列表
                                ExCard.WriteOutput((ushort)ExOutput.工位1复测.GetHashCode(), 0);
                                ExCard.WriteOutput((ushort)ExOutput.工位1检测OK.GetHashCode(), 1);
                                ExCard.WriteOutput((ushort)ExOutput.工位1检测NG.GetHashCode(), 1);
                            }
                        }
                        ExCard.WriteOutput((ushort)ExOutput.工位1允许放料.GetHashCode(), 0);
                        nStation1 = 90;
                    }

                }
            }
            else if (nStation1 == 90)
            {
                if (!Card1.Singal[Card1Input.治具1感应.GetHashCode()] && Card1.Singal[Card1Input.治具1安全光栅.GetHashCode()] && !ExCard.InputList[ExInput.治具1对射感应.GetHashCode()])
                {
                    Start_T(13, 500);
                    nStation1 = 100;
                }
            }
            else if (nStation1 == 100)
            {
                if (TimerSignal[13])
                {
                    Reset_T(13);
                    nStation1 = 0;
                }
            }
        }

        public void Station2WorksOnline()
        {
            if (nStation2 == 0)
            {
                Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 0);
                Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 0);
                nStation2 = 5;
            }
            else if (nStation2 == 5)
            {
                Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 0);
                Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 0);

                Card1.AbsoluteMove(2, Common.Data.DictPos["取料位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                nStation2 = 10;
            }
            else if (nStation2 == 10)
            {
                if (Card1.Singal[Card1Input.治具2气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具2气缸2正限.GetHashCode()] && !Card1.b_Move[2])
                {
                    //Card1.WriteOutput(Card1Output.治具2接料许可.GetHashCode(), 1);
                    ExCard.WriteOutput((ushort)ExOutput.工位2允许放料.GetHashCode(), 0);
                    nStation2 = 20;
                }
            }
            else if (nStation2 == 20)
            {
                if (Card1.Singal[Card1Input.治具2感应.GetHashCode()] && ExCard.InputList[ExInput.工位2放料完成.GetHashCode()] && !ExCard.InputList[ExInput.治具2对射感应.GetHashCode()])
                {
                    ExCard.WriteOutput((ushort)ExOutput.工位2允许放料.GetHashCode(), 1);
                    iCloseCylinder2 = 0;
                    Card1.AbsoluteMove(2, Common.Data.DictPos["扫码位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                    nStation2 = 40;
                }
            }
            else if (nStation2 == 40)
            {
                if (Card1.Singal[Card1Input.治具2气缸1负限.GetHashCode()] && Card1.Singal[Card1Input.治具2气缸2负限.GetHashCode()] && !Card1.b_Move[2] && iCloseCylinder2 == -1)
                {
                    //读码
                    if (Common.DataReadPort2.IsOpen)
                    {
                        readDataCount2++;
                        Common.str_DataCode2 = "";
                        Common.DataReadPort2.Write("T\r\n");
                        Start_T(22, 100);
                    }
                    nStation2 = 50;
                }
            }
            else if (nStation2 == 50)
            {
                if (TimerSignal[22])
                {
                    Reset_T(22);
                    //读到码。。 
                    if (Common.str_DataCode2 != "")
                    {
                        Card1.AbsoluteMove(2, Common.Data.DictPos["等待位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                        nStation2 = 60;
                    }
                    else
                    {
                        if (readDataCount2 < 5)
                        {
                            nStation2 = 40; //扫不到码 再读一次
                        }
                        else
                        {
                            readDataCount2 = 0;
                            nStation2 = 70;
                            bFix2 = false;
                        }
                    }
                }
            }
            else if (nStation2 == 60)
            {
                if (!Card1.b_Move[2])
                {

                    if (DataHelper.QueueUp(Common.str_DataCode2)) //此处判定是否排队，排队失败则直接回取料位
                    {
                        nStation2 = 65;
                    }
                    else
                    {
                        Common.NgSnList.Add(Common.str_DataCode2);
                        StationResult2 = false;
                        nStation2 = 70;
                    }
                }
            }
            else if (nStation2 == 65)
            {
                if (nCheckStep1 == -1)
                {
                    bFix2 = false;
                    Common.str_CurrentDataCode = Common.str_DataCode2;
                    DataHelper.Values2 = new string[DataHelper.Names.Length];
                    DataHelper.Values2[0] = Common.str_DataCode2;
                    DataHelper.Values2[1] = Common.mes.station_id;
                    DataHelper.Values2[2] = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}";
                    DataHelper.Values2[3] = $"Fix_2";

                    Common.ShowDataCode(Common.str_CurrentDataCode);
                    nCheckStep2 = 0; //开始检测
                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + $" -->开始测试--");
                    Common.stop.Restart();
                    nStation2 = 70;
                }
            }
            else if (nStation2 == 70)
            {
                if (nCheckStep2 == -1) //检测完成
                {
                    Common.stop.Stop();
                    TimeSpan ts = Common.stop.Elapsed;
                    // Format and display the TimeSpan value.
                    string elapsedTime = $"{ts.Seconds}秒{ts.Milliseconds / 10}毫秒";
                    Common.ShowInspectTime(elapsedTime);

                    Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 0);
                    Start_T(23, 100);
                    nStation2 = 75;
                }
            }
            else if (nStation2 == 75)
            {
                if (TimerSignal[23])
                {
                    Reset_T(23);
                    Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 0);
                    Card1.AbsoluteMove(2, Common.Data.DictPos["放料位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                    nStation2 = 80;
                }
            }
            else if (nStation2 == 80)
            {
                if (Card1.Singal[Card1Input.治具2气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具2气缸2正限.GetHashCode()] && !Card1.b_Move[2])
                {
                    //指示产品OK或NG
                    if (StationResult2)
                    {
                        ExCard.WriteOutput((ushort)ExOutput.工位2检测OK.GetHashCode(), 0);
                        ExCard.WriteOutput((ushort)ExOutput.工位2检测NG.GetHashCode(), 1);
                        ExCard.WriteOutput((ushort)ExOutput.工位2复测.GetHashCode(), 1);
                        ExCard.WriteOutput((ushort)ExOutput.工位2允许放料.GetHashCode(), 0);
                        nStation2 = 90;
                    }
                    else
                    {
                        if (Common.str_DataCode2 == "")
                        {
                            ExCard.WriteOutput((ushort)ExOutput.工位2复测.GetHashCode(), 1);
                            ExCard.WriteOutput((ushort)ExOutput.工位2检测OK.GetHashCode(), 1);
                            ExCard.WriteOutput((ushort)ExOutput.工位2检测NG.GetHashCode(), 0);
                        }
                        else
                        {
                            if (Common.NgSnList.Contains(Common.str_DataCode2)) //已经是复测了
                            {
                                ExCard.WriteOutput((ushort)ExOutput.工位2复测.GetHashCode(), 1);
                                ExCard.WriteOutput((ushort)ExOutput.工位2检测OK.GetHashCode(), 1);
                                ExCard.WriteOutput((ushort)ExOutput.工位2检测NG.GetHashCode(), 0);
                            }
                            else
                            {
                                Common.NgSnList.Add(Common.str_DataCode2);    //第一次NG加入列表
                                ExCard.WriteOutput((ushort)ExOutput.工位2复测.GetHashCode(), 0);
                                ExCard.WriteOutput((ushort)ExOutput.工位2检测OK.GetHashCode(), 1);
                                ExCard.WriteOutput((ushort)ExOutput.工位2检测NG.GetHashCode(), 1);
                            }
                        }
                        ExCard.WriteOutput((ushort)ExOutput.工位2允许放料.GetHashCode(), 0);
                        nStation2 = 90;
                    }
                }
            }
            else if (nStation2 == 90)
            {
                if (!Card1.Singal[Card1Input.治具2感应.GetHashCode()] && Card1.Singal[Card1Input.治具2安全光栅.GetHashCode()] && !ExCard.InputList[ExInput.治具2对射感应.GetHashCode()])
                {
                    Start_T(22, 500);
                    nStation2 = 100;
                }
            }
            else if (nStation2 == 100)
            {
                if (TimerSignal[22])
                {
                    Reset_T(22);
                    nStation2 = 0;
                }
            }
        }

        public void Station1Works()
        {
            if (nStation1 == 0)
            {
                Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 0);
                nStation1 = 5;

            }
            else if (nStation1 == 5)
            {
                Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 0);
                Card1.AbsoluteMove(1, Common.Data.DictPos["取料位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                nStation1 = 10;
            }
            else if (nStation1 == 10)
            {
                if (Card1.Singal[Card1Input.治具1气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具1气缸2正限.GetHashCode()] && !Card1.b_Move[1])
                {
                    Card1.WriteOutput(Card1Output.治具1接料许可.GetHashCode(), 1);
                    nStation1 = 20;
                }
            }
            else if (nStation1 == 20)
            {
                if (Card1.Singal[Card1Input.治具1感应.GetHashCode()] /*&& Card1.Singal[Card1Input.治具1放料完成.GetHashCode()]*/)
                {
                    Start_T(11, 1000);
                    nStation1 = 30;
                }
            }
            else if (nStation1 == 30)
            {
                if (TimerSignal[11] && Card1.Singal[Card1Input.治具1感应.GetHashCode()])
                {
                    Reset_T(11);
                    Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 1);
                    nStation1 = 35;

                }
                else if (TimerSignal[11] && !Card1.Singal[Card1Input.治具1感应.GetHashCode()])
                {
                    nStation1 = 20;
                }
            }
            else if (nStation1 == 35)
            {
                Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 0);
                Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 1);
                Card1.AbsoluteMove(1, Common.Data.DictPos["扫码位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                nStation1 = 40;
            }
            else if (nStation1 == 40)
            {
                if (Card1.Singal[Card1Input.治具1气缸1负限.GetHashCode()] && Card1.Singal[Card1Input.治具1气缸2负限.GetHashCode()] && !Card1.b_Move[1])
                {
                    //读码
                    if (Common.DataReadPort1.IsOpen)
                    {
                        Common.DataReadPort1.Write("T\r\n");
                        Start_T(12, 100);
                    }
                    nStation1 = 50;
                }
            }
            else if (nStation1 == 50)
            {
                if (TimerSignal[12])
                {
                    Reset_T(12);
                    if (Common.str_DataCode1 != "")
                    {
                        //读到码。。
                        Card1.AbsoluteMove(1, Common.Data.DictPos["等待位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                        nStation1 = 60;
                    }
                    else
                    {
                        nStation1 = 40; //扫不到码 再读一次
                    }
                }
            }
            else if (nStation1 == 60)
            {
                if (!Card1.b_Move[1])
                {
                    if (Common.bOfflineMesUse)
                    {
                        if (DataHelper.QueueUp(Common.str_DataCode1))
                        {
                            nStation1 = 65; 
                        }
                        else
                        {
                            nStation1 = 70;
                        }
                    }
                    else
                    {
                        nStation1 = 65;
                    }
                }
            }
            else if (nStation1 == 65)
            {
                if (nCheckStep2 == -1)
                {
                    bFix1 = true;
                    Common.str_CurrentDataCode = Common.str_DataCode1;
                    //"SN","station_id","Time","FixtrueName", "Config",
                    DataHelper.Values1 = new string[DataHelper.Names.Length];
                    DataHelper.Values1[0] = Common.str_DataCode1;
                    DataHelper.Values1[1] = Common.mes.station_id;
                    DataHelper.Values1[2] = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}";
                    DataHelper.Values1[3] = $"Fix_1";

                    Common.str_DataCode1 = "";
                    Common.ShowDataCode(Common.str_CurrentDataCode);
                    nCheckStep1 = 0; //开始检测
                    //Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + $" -->开始测试--");
                    Common.stop.Restart();
                    nStation1 = 70;
                }
            }
            else if (nStation1 == 70)
            {
                if (nCheckStep1 == -1) //检测完成
                {
                    TimeSpan ts = Common.stop.Elapsed;
                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
                    Common.ShowInspectTime(elapsedTime);
                    Card1.WriteOutput(Card1Output.治具1气缸1前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具1气缸1后退.GetHashCode(), 0);
                    nStation1 = 75;
                }
            }
            else if (nStation1 == 75)
            {
                Card1.WriteOutput(Card1Output.治具1气缸2前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具1气缸2后退.GetHashCode(), 0);
                Card1.AbsoluteMove(1, Common.Data.DictPos["放料位1"], Card1.StartVList[1], Card1.SpeedList[1], Card1.Aspeed[1], Card1.TaccList[1]);
                nStation1 = 80;
            }
            else if (nStation1 == 80)
            {
                if (Card1.Singal[Card1Input.治具1气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具1气缸2正限.GetHashCode()] && !Card1.b_Move[1])
                {
                    //指示产品OK或NG
                    if (Common.RunMode == "Keep")
                    {
                        nStation1 = 0;
                    }
                    else
                    {
                        nStation1 = 90;
                    }
                }
            }
            else if (nStation1 == 90)
            {
                if (!Card1.Singal[Card1Input.治具1感应.GetHashCode()] && Card1.Singal[Card1Input.治具1安全光栅.GetHashCode()])
                {
                    Start_T(13, 500);
                    nStation1 = 100;
                }
            }
            else if (nStation1 == 100)
            {
                if (TimerSignal[13])
                {
                    Reset_T(13);
                    nStation1 = 0;
                }
            }
        }

        public void Station2Works()
        {
            if (nStation2 == 0)
            {
                Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 0);
                nStation2 = 5;
            }
            else if (nStation2 == 5)
            {
                Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 0);
                Card1.AbsoluteMove(2, Common.Data.DictPos["取料位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                nStation2 = 10;
            }
            else if (nStation2 == 10)
            {
                if (Card1.Singal[Card1Input.治具2气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具2气缸2正限.GetHashCode()] && !Card1.b_Move[2])
                {
                    Card1.WriteOutput(Card1Output.治具2接料许可.GetHashCode(), 1);
                    nStation2 = 20;
                }
            }
            else if (nStation2 == 20)
            {
                if (Card1.Singal[Card1Input.治具2感应.GetHashCode()]/* && Card1.Singal[Card1Input.治具2放料完成.GetHashCode()]*/)
                {
                    Start_T(21, 1000);
                    nStation2 = 30;
                }
            }
            else if (nStation2 == 30)
            {
                if (TimerSignal[21] && Card1.Singal[Card1Input.治具2感应.GetHashCode()])
                {
                    Reset_T(21);
                    Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 0);
                    Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 1);
                    nStation2 = 35;

                }
                else if (TimerSignal[21] && !Card1.Singal[Card1Input.治具2感应.GetHashCode()])
                {
                    nStation2 = 20;
                }
            }
            else if (nStation2 == 35)
            {
                Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 0);
                Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 1);
                Card1.AbsoluteMove(2, Common.Data.DictPos["扫码位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                nStation2 = 40;
            }
            else if (nStation2 == 40)
            {
                if (Card1.Singal[Card1Input.治具2气缸1负限.GetHashCode()] && Card1.Singal[Card1Input.治具2气缸2负限.GetHashCode()] && !Card1.b_Move[2])
                {
                    //读码
                    if (Common.DataReadPort2.IsOpen)
                    {
                        Common.str_DataCode2 = "";
                        Common.DataReadPort2.Write("T\r\n");
                        Start_T(22, 100);
                    }
                    nStation2 = 50;
                }
            }
            else if (nStation2 == 50)
            {
                if (TimerSignal[22])
                {
                    Reset_T(22);
                    //读到码。。
                    if (Common.str_DataCode2 != "")
                    {
                        Card1.AbsoluteMove(2, Common.Data.DictPos["等待位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                        nStation2 = 60;
                    }
                    else
                    {
                        nStation2 = 40;
                    }
                }
            }
            else if (nStation2 == 60)
            {
                if (!Card1.b_Move[2])
                {

                    if (Common.bOfflineMesUse)
                    {
                        if (DataHelper.QueueUp(Common.str_DataCode2))
                        {
                            nStation2 = 65;
                        }
                        else
                        {
                            nStation2 = 70;
                        }
                    }
                    else
                    {
                        nStation2 = 65;
                    }
                }
            }
            else if (nStation2 == 65)
            {
                if (nCheckStep1 == -1)
                {
                    bFix2 = false;
                    Common.str_CurrentDataCode = Common.str_DataCode2;
                    DataHelper.Values2 = new string[DataHelper.Names.Length];
                    DataHelper.Values2[0] = Common.str_DataCode2;
                    DataHelper.Values2[1] = Common.mes.station_id;
                    DataHelper.Values2[2] = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}";
                    DataHelper.Values2[3] = $"Fix_2";

                    Common.ShowDataCode(Common.str_CurrentDataCode);
                    nCheckStep2 = 0; //开始检测
                    Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff") + $" -->开始测试--");
                    Common.stop.Restart();
                    nStation2 = 70;
                }
            }
            else if (nStation2 == 70)
            {
                if (nCheckStep2 == -1) //检测完成
                {
                    Common.stop.Stop();
                    TimeSpan ts = Common.stop.Elapsed;
                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
                    Common.ShowInspectTime(elapsedTime);

                    Card1.WriteOutput(Card1Output.治具2气缸1前进.GetHashCode(), 1);
                    Card1.WriteOutput(Card1Output.治具2气缸1后退.GetHashCode(), 0);
                    nStation2 = 75;
                }
            }
            else if (nStation2 == 75)
            {
                Card1.WriteOutput(Card1Output.治具2气缸2前进.GetHashCode(), 1);
                Card1.WriteOutput(Card1Output.治具2气缸2后退.GetHashCode(), 0);
                Card1.AbsoluteMove(2, Common.Data.DictPos["放料位2"], Card1.StartVList[2], Card1.SpeedList[2], Card1.Aspeed[2], Card1.TaccList[2]);
                nStation2 = 80;
            }
            else if (nStation2 == 80)
            {
                if (Card1.Singal[Card1Input.治具2气缸1正限.GetHashCode()] && Card1.Singal[Card1Input.治具2气缸2正限.GetHashCode()] && !Card1.b_Move[2])
                {
                    //指示产品OK或NG
                    if (Common.RunMode == "Keep")
                    {
                        nStation2 = 0;
                    }
                    else if (Common.RunMode == "Once")
                    {
                        nStation2 = 90;
                    }
                }
            }
            else if (nStation2 == 90)
            {
                if (!Card1.Singal[Card1Input.治具2感应.GetHashCode()] && Card1.Singal[Card1Input.治具2安全光栅.GetHashCode()])
                {
                    Start_T(22, 500);
                    nStation2 = 100;
                }
            }
            else if (nStation2 == 100)
            {
                if (TimerSignal[22])
                {
                    Reset_T(22);
                    nStation2 = 0;
                }
            }
        }
    }
}
