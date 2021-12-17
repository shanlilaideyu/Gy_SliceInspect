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
        //点位保存
        public double[] PositionList = new double[30];
        //延时保存
        public Int32[] DelayList = new Int32[30];


        //[NonSerialized]
        public LeadShine Card1;  // 8轴卡

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

        }

        public enum Card1Output
        {

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

        public int[] Time = new int[17];


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
                    Card1 = new LeadShine(0, 4);
                    Card1.AxisName = new string[] { "针卡相机Z", "平台X1", "平台X2", "平台Y" };
                    Card1.InputNames = new string[] { "输入0", "输入1", "输入2", "输入3", "输入4", "输入5", "输入6", "输入7", "输入8", "输入9", "输入10", "输入11", "输入12", "输入13", "输入14", "输入15" };
                    Card1.OutputNames = new string[] { "输出0", "输出1", "输出2", "输出3", "输出4", "输出5", "输出6", "输出7", "输出8", "输出9", "输出10", "输出11", "输出12", "输出13", "输出14", "输出15" };
                }
                if (Card1.InitBoard() == 0)
                {
                    Card1.SetHardStop(2, 0, 0);
                    for (int i = 0; i < Card1.AxisName.Length; i++)
                    {
                        Card1.SetPulseMode(i, 3); //0-2 为伺服
                        Card1.SetLimitMode(i, 0, 0, 1); // 000 正负限位都启用 高电平有效
                        Card1.SetSpeed(i, Card1.StartVList[i], Card1.SpeedList[i], Card1.TaccList[i]);
                    }
                }
                Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms") + " -->运动控制卡初始化成功--");
                return true;
            }
            catch (Exception ex)
            {
                Common.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("运动控制卡初始化异常，原因为{0}--", ex.Message));
                return false;
            }
        }

        public void GetStatus()
        {
            Card1.GetCurrentInf();
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

        public void MovePlaU(double UValuve, MoveType moveType)
        {
            //算推送量
            double _pos1 = 56.568 * Math.Cos((UValuve + 45) * Math.PI / 180) - 56.568 * Math.Cos(45 * Math.PI / 180);
            double _pos2 = 56.568 * Math.Cos((UValuve + 315) * Math.PI / 180) - 56.568 * Math.Cos(315 * Math.PI / 180);
            double _pos3 = 56.568 * Math.Sin((UValuve + 135) * Math.PI / 180) - 56.568 * Math.Sin(135 * Math.PI / 180);

            Common.ProC.Go_U(1, 2, 3, _pos1, _pos2, _pos3, moveType);
        }

        public void MovePlaX(double value, MoveType moveType)
        {
            Common.ProC.LineMulticoor(1, 2, value, value, moveType);
        }

        public async void AllAxisGohome()
        {
            CancelCardSoftLmt();
            List<Task<bool>> taskList = new List<Task<bool>>();
            //卡1
            //taskList.Add(Task<bool>.Run(() =>
            //{
            //    return Card1.GoHome(0,1, 0, 1, 1); //负方向回
            //}));
            for (int i = 1; i < Card1.AxisName.Length; i++)
            {
                int axis = i;

                taskList.Add(Task<bool>.Run(() =>
                {
                    return Card1.GoHome(axis,1,0,0.5,1); //负方向回
                }));
                Thread.Sleep(20);
            }
            var b = await Task.WhenAll(taskList);
            Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("所有轴回原完成--"));
            SetCardSoftLmt();
        }

        public void LoadParam()
        {
            string fp = $"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\MotionSetting.json";
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
            if (!Directory.Exists($"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\"))
            {
                Directory.CreateDirectory($"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\");
            }
            string fp = $"{System.Windows.Forms.Application.StartupPath}\\Products\\{Common.str_ProductName}\\SysCfg\\MotionSetting.json";
            if (!File.Exists(fp))  // 判断是否已有相同文件 
            {
                FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                fs1.Close();
            }
            File.WriteAllText(fp, result.Replace(",\"", ",\r\n\""));
        }

        public bool LineMulticoor(int Axis1, int Axis2, double Taget1, double Taget2, MoveType MoveType)//直线插补
        {
            short NN = 0;
            short done = 0;
            try
            {
                ushort[] axisList = new ushort[2] { (ushort)Axis1, (ushort)Axis2 };
                int[] DistList = new int[2] { (int)(Taget1 * Card1.PulsePerMM[Axis1]), (int)(Taget2 * Card1.PulsePerMM[Axis2]) };

                LTDMC.dmc_set_vector_profile_multicoor((ushort)Card1.m_CardNo, 0, 0, 1000, 0.2, 0, 0);

                if (MoveType == MoveType.Absolute)
                {
                    NN = LTDMC.dmc_line_multicoor((ushort)Card1.m_CardNo, 0, 2, axisList, DistList, 1);
                }
                else if (MoveType == MoveType.Relative)
                {
                    NN = LTDMC.dmc_line_multicoor((ushort)Card1.m_CardNo, 0, 2, axisList, DistList, 0);
                }

                do
                {
                    done = LTDMC.dmc_check_done_multicoor((ushort)Card1.m_CardNo, 0); //0：指定轴正在运行，1：指定轴已停止
                } while (done != 1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Go_U(int Axis1, int Axis2, int Axis3, double Taget1, double Taget2, double Taget3, MoveType MoveType)//U旋转
        {
            short done = 0;
            try
            {
                ushort[] axisList = new ushort[3] { (ushort)Axis1, (ushort)Axis2, (ushort)Axis3 };
                int[] DistList = new int[3] { (int)(Taget1 * Card1.PulsePerMM[Axis1]), (int)(Taget2 * Card1.PulsePerMM[Axis2]), (int)(Taget3 * Card1.PulsePerMM[Axis3]) };

                LTDMC.dmc_set_vector_profile_multicoor((ushort)Card1.m_CardNo, 0, 0, 1000, 0.2, 0, 0);

                if (MoveType == MoveType.Absolute)
                {
                    LTDMC.dmc_line_multicoor((ushort)Card1.m_CardNo, 0, 3, axisList, DistList, 1);
                }
                else if (MoveType == MoveType.Relative)
                {
                    LTDMC.dmc_line_multicoor((ushort)Card1.m_CardNo, 0, 3, axisList, DistList, 0);
                }
                do
                {
                    done = LTDMC.dmc_check_done_multicoor((ushort)Card1.m_CardNo, 0); //0：指定轴正在运行，1：指定轴已停止
                } while (done != 1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
