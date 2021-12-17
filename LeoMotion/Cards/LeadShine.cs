using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LeoMotion
{
    /// <summary>
    /// 雷赛
    /// </summary>
    [Serializable]
    public class LeadShine : Card
    {
        public LeadShine()
        {

        }

        public LeadShine(int AxisNum)
        {
            LogPos = new double[AxisNum];
            ActPos = new double[AxisNum];
            CurrentSpeed = new double[AxisNum];
            b_Move = new bool[AxisNum];
            SoftLmtEnable = new bool[AxisNum];
            PSoftLmt = new double[AxisNum];
            NSoftLmt = new double[AxisNum];
            PulsePerMM = new double[AxisNum];
            StartVList = new double[AxisNum];
            SpeedList = new double[AxisNum];
            Aspeed = new double[AxisNum];
            TaccList = new double[AxisNum];
            Singal = new bool[16];
        }

        //适用于多卡情况
        public LeadShine(int cardNo, int AxisNum)
        {
            m_CardNo = cardNo;
            LogPos = new double[AxisNum];
            ActPos = new double[AxisNum];
            CurrentSpeed = new double[AxisNum];
            b_Move = new bool[AxisNum];
            SoftLmtEnable = new bool[AxisNum];
            PSoftLmt = new double[AxisNum];
            NSoftLmt = new double[AxisNum];
            PulsePerMM = new double[AxisNum];
            StartVList = new double[AxisNum];
            SpeedList = new double[AxisNum];
            Aspeed = new double[AxisNum];
            TaccList = new double[AxisNum];
            Singal = new bool[16];
        }

        /// <summary>
        /// 设置高速脉冲输出
        /// </summary>
        /// <param name="CompareIO">高速脉冲输出口</param>
        /// <param name="Axis">指定轴</param>
        /// <param name="BegionPos">比较位置</param>
        /// <param name="InCmpInc">线性增量</param>
        /// <param name="CompareTimes">比较次数</param>
        /// <param name="ContinusTime">高电平持续时间(us)</param>
        public void SetLinerHcmp(int CompareIO, int Axis, int BegionPos, int InCmpInc, int CompareTimes, int ContinusTime)
        {
            LTDMC.dmc_hcmp_set_config((ushort)m_CardNo, (ushort)CompareIO, 3, 0, (ushort)1, CompareTimes);
            LTDMC.dmc_hcmp_set_mode((ushort)m_CardNo, (ushort)CompareIO, (ushort)5);
            LTDMC.dmc_hcmp_clear_points((ushort)m_CardNo, (ushort)CompareIO);
            LTDMC.dmc_hcmp_set_liner((ushort)m_CardNo, (ushort)CompareIO, InCmpInc, CompareTimes);
            LTDMC.dmc_hcmp_add_point((ushort)m_CardNo, (ushort)CompareIO, BegionPos);
        }

        public override int AbsoluteMove(int Axis, double Pulse, double StartV, double Speed,double ASpeed, double Tacc)
        {
            Int32 pos = Convert.ToInt32(Pulse * PulsePerMM[Axis]);
            Int32 startSpeed = Convert.ToInt32(StartV * PulsePerMM[Axis]);
            Int32 speed = Convert.ToInt32(Speed * PulsePerMM[Axis]);

            Result = LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, startSpeed, speed, Tacc, Tacc, startSpeed);
            Result = LTDMC.dmc_pmove((ushort)m_CardNo, (ushort)Axis, pos, (ushort)1);
            return Result;
        }

        public override bool AxisMove(int Axis, MoveType MoveType, double Taget, double StartV, double Speed, double ASpeed, double TAcc)
        {
            Int32 taget = Convert.ToInt32(Taget * PulsePerMM[Axis]);
            Int32 startSpeed = Convert.ToInt32(StartV * PulsePerMM[Axis]);
            Int32 speed = Convert.ToInt32(Speed * PulsePerMM[Axis]);

            short done = 0;
            switch (MoveType)
            {
                case MoveType.Relative:
                    LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, startSpeed, speed, TAcc, TAcc, startSpeed);
                    LTDMC.dmc_pmove((ushort)m_CardNo, (ushort)Axis, taget, (ushort)0);
                    break;
                case MoveType.Keep:
                    LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, startSpeed, speed, TAcc, TAcc, startSpeed);
                    if (Taget > 0)
                    {
                        LTDMC.dmc_vmove((ushort)m_CardNo, (ushort)Axis, (ushort)1); //正向一直运动
                    }
                    else
                    {
                        LTDMC.dmc_vmove((ushort)m_CardNo, (ushort)Axis, (ushort)0); //负向一直运动
                    }
                    return false;
                case MoveType.Absolute:
                    LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, startSpeed, speed, TAcc, TAcc, startSpeed);
                    LTDMC.dmc_pmove((ushort)m_CardNo, (ushort)Axis, taget, (ushort)1);
                    break;
                case MoveType.HardStop:
                    LTDMC.dmc_stop((ushort)m_CardNo, (ushort)Axis, (ushort)0);
                    break;
                case MoveType.SmoothStop:
                    LTDMC.dmc_stop((ushort)m_CardNo, (ushort)Axis, (ushort)1);
                    break;
                default:
                    break;
            }
            do
            {
                done = LTDMC.dmc_check_done((ushort)m_CardNo, (ushort)Axis); //0：指定轴正在运行，1：指定轴已停止
            } while (done != 1);
            return true;
        }

        public override int AxisPmove(int Axis, double Taget)
        {
            Int32 taget = Convert.ToInt32(Taget * PulsePerMM[Axis]);
            Result = LTDMC.dmc_pmove((ushort)m_CardNo, (ushort)Axis, taget, (ushort)0);
            return Result;
        }

        public override int GetCurrentInf()
        {
            for (int i = 0; i < LogPos.Length; i++)
            {
                LogPos[i] = LTDMC.dmc_get_position((ushort)m_CardNo, (ushort)i) / PulsePerMM[i];
                ActPos[i] = LTDMC.dmc_get_encoder((ushort)m_CardNo, (ushort)i) / PulsePerMM[i];
                CurrentSpeed[i] = LTDMC.dmc_read_current_speed((ushort)m_CardNo, (ushort)i) / PulsePerMM[i];
                if (LTDMC.dmc_check_done((ushort)m_CardNo, (ushort)i) == 0)//0：指定轴正在运行，1：指定轴已停止);
                {
                    b_Move[i] = true;
                }
                else
                {
                    b_Move[i] = false;
                }
            }
            for (int j = 0; j < Singal.Length; j++)
            {
                if (LTDMC.dmc_read_inbit((ushort)m_CardNo, (ushort)j) == 0)
                {
                    Singal[j] = true;
                }
                else
                {
                    Singal[j] = false;
                }
            }
            return 0;
        }

        public override int GetStatus(int axis, out int value, int mode)
        {
            if (mode == 0)
            {
                value = LTDMC.dmc_check_done((ushort)m_CardNo, (ushort)axis); //0：指定轴正在运行，1：指定轴已停止
            }
            else
            {
                value = LTDMC.dmc_check_done_multicoor((ushort)m_CardNo, (ushort)axis);//0：指定轴正在运行，1：指定轴已停止
            }
            return value;
        }

        public override bool GoHome(int Axis, int Dir, int VelMode, double Vel, int Mode)
        {
            Int32 vel = Convert.ToInt32(Vel * PulsePerMM[Axis]);

            ushort done = 0;

            LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, vel / 10, vel, 0.1, 0.1, 0);
            LTDMC.dmc_set_homemode((ushort)m_CardNo, (ushort)Axis, (ushort)Dir, (ushort)VelMode, (ushort)Mode, (ushort)0);
            LTDMC.dmc_set_home_el_return((ushort)m_CardNo, (ushort)Axis, (ushort)0);
            LTDMC.dmc_home_move((ushort)m_CardNo, (ushort)Axis);
            Thread.Sleep(10);
            do
            {
                LTDMC.dmc_get_home_result((ushort)m_CardNo, (ushort)Axis, ref done);
                //done = LTDMC.dmc_check_done((ushort)m_CardNo, (ushort)Axis); //0：指定轴正在运行，1：指定轴已停止
            } while (done != 1);
            Thread.Sleep(10);
            LTDMC.dmc_set_position((ushort)m_CardNo, (ushort)Axis, (short)0);
            return true;
        }

        public override bool GoNLmt(int Axis, double Vel)
        {
            Int32 vel = Convert.ToInt32(Vel * PulsePerMM[Axis]);

            ushort done = 0;
            LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, vel / 10, vel, 0.1, 0.1, vel / 10);
            //卡号 轴号 负向 高速回零  一次限位回零加反找  保留参数固定为0
            LTDMC.dmc_set_homemode((ushort)m_CardNo, (ushort)Axis, (ushort)0, (ushort)1, (ushort)11, (ushort)0);
            LTDMC.dmc_home_move((ushort)m_CardNo, (ushort)Axis);
            Thread.Sleep(10);
            do
            {
                LTDMC.dmc_get_home_result((ushort)m_CardNo, (ushort)Axis, ref done);
                //done = LTDMC.dmc_check_done((ushort)m_CardNo, (ushort)Axis); //0：指定轴正在运行，1：指定轴已停止
            } while (done != 1);
            Thread.Sleep(10);
            LTDMC.dmc_set_position((ushort)m_CardNo, (ushort)Axis, (short)0);
            return true;
        }

        public override int InitBoard()
        {
            Result = LTDMC.dmc_board_init();
            for (int i = 0; i < LogPos.Length; i++)
            {
                Result = LTDMC.dmc_write_sevon_pin((ushort)m_CardNo, (ushort)i, (ushort)0);
            }
            return Result;
        }

        public override int KeepMove(int Axis, int Direction)
        {
            Result = LTDMC.dmc_vmove((ushort)m_CardNo, (ushort)Axis, (ushort)Direction); //正向一直运动
            return Result;
        }

        public override void ReadInput()
        {
            int Value = 0;
            for (int i = 0; i < Singal.Length; i++)
            {
                Value = LTDMC.dmc_read_inbit((ushort)m_CardNo, (ushort)i);
                if (Value == 0)
                {
                    Singal[i] = false;
                }
                else
                {
                    Singal[i] = true;
                }
            }
        }

        public override int RelativeMove(int Axis, double Pulse, double StartV, double Speed, double ASpeed, double Tacc)
        {
            Int32 pos = Convert.ToInt32(Pulse * PulsePerMM[Axis]);
            Int32 startSpeed = Convert.ToInt32(StartV * PulsePerMM[Axis]);
            Int32 speed = Convert.ToInt32(Speed * PulsePerMM[Axis]);

            Result = LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, startSpeed, speed, Tacc, Tacc, startSpeed);
            Result = LTDMC.dmc_pmove((ushort)m_CardNo, (ushort)Axis, pos, (ushort)0);
            return Result;
        }

        public override int SetHardStop(int IOindex, int Value, int Logic)
        {
            ushort MapIOType = 6;
            uint Filter = 10;
            for (int i = 0; i < LogPos.Length; i++)
            {
                Result = LTDMC.dmc_set_AxisIoMap((ushort)m_CardNo, (ushort)i, 3, MapIOType, (ushort)IOindex, Filter);
                Result = LTDMC.dmc_set_emg_mode((ushort)m_CardNo, (ushort)i, (ushort)Value, (ushort)Logic);
            }
            return Result;
        }

        /// <summary>
        /// 该函数用于设定正/负方向限位输入nLMT信号的模式
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Value1">正限位： 0 - 有效 ，1 -无效</param>
        /// <param name="Value2">负限位： 0 - 有效 ，1 -无效</param>
        /// <param name="Logic"> 0－低电平有效  1－高电平有效</param>
        /// <returns></returns>
        public override int SetLimitMode(int Axis, int Value1, int Value2, int Logic)
        {
            if (Value1 == 0 && Value2 == 0)
            {
                Result = LTDMC.dmc_set_el_mode((ushort)m_CardNo, (ushort)Axis, (ushort)1, (ushort)Logic, (ushort)0);//正负限位都启用
            }
            else if (Value1 == 1 && Value2 == 0)
            {
                Result = LTDMC.dmc_set_el_mode((ushort)m_CardNo, (ushort)Axis, (ushort)2, (ushort)Logic, (ushort)0);//正限位禁止，负限位启用
            }
            else if (Value1 == 0 && Value2 == 1)
            {
                Result = LTDMC.dmc_set_el_mode((ushort)m_CardNo, (ushort)Axis, (ushort)3, (ushort)Logic, (ushort)0);//负限位禁止，正限位启用
            }
            else if (Value1 == 1 && Value2 == 1)
            {
                Result = LTDMC.dmc_set_el_mode((ushort)m_CardNo, (ushort)Axis, (ushort)0, (ushort)Logic, (ushort)0);//正负限位都不启用
            }
            return Result;
        }

        public override int SetPulseMode(int Axis, int Value)
        {
            Result = LTDMC.dmc_set_pulse_outmode((ushort)m_CardNo, (ushort)Axis, (ushort)Value);
            return Result;
        }

        public override int SetSpeed(int Axis, double StartV, double Speed, double ASpeed, double Add)
        {
            Int32 startSpeed = Convert.ToInt32(StartV * PulsePerMM[Axis]);
            Int32 speed = Convert.ToInt32(Speed * PulsePerMM[Axis]);
            Int32 add = Convert.ToInt32(Add * PulsePerMM[Axis]);

            Result = LTDMC.dmc_set_profile((ushort)m_CardNo, (ushort)Axis, startSpeed, speed, add, add, startSpeed / 4);
            return Result;
        }

        public override int SetupPos(int Axis, double Pos, int Mode)
        {
            Int32 pos = Convert.ToInt32(Pos * PulsePerMM[Axis]);
            Result = LTDMC.dmc_set_position((ushort)m_CardNo, (ushort)Axis, pos);
            return Result;
        }

        public override int StopRun(int Axis, int Mode)
        {
            Result = LTDMC.dmc_stop((ushort)m_CardNo, (ushort)Axis, (ushort)Mode);
            return Result;
        }

        public override int WriteOutput(int Number, int Value)
        {
            Result = LTDMC.dmc_write_outbit((ushort)m_CardNo, (ushort)Number, (ushort)Value);
            return Result;
        }

        public override int PtsTableMove(int Axis, uint count, double[] pTime, double[] pPos, double[] pPercent)
        {
            Int32[] npos = new Int32[pPos.Length];
            for (int i = 0; i < pPos.Length; i++)
            {
                npos[i] = Convert.ToInt32(pPos[i] * PulsePerMM[Axis]);
            }
            Result = LTDMC.dmc_PtsTable((ushort)m_CardNo, (ushort)Axis, count, pTime, npos, pPercent);
            ushort[] axisList = new ushort[] { (ushort)Axis };
            Result = LTDMC.dmc_PvtMove((ushort)m_CardNo, 1, axisList);
            return Result;
        }

        public override Int32 SetSoftLmt(int Axis, int Enable, int Source, int SlAction, double N_Limit, double P_Limit)
        {
            if (N_Limit < P_Limit)
            {
                Result = LTDMC.dmc_set_softlimit((ushort)m_CardNo, (ushort)Axis, (ushort)Enable, (ushort)Source, (ushort)SlAction, Convert.ToInt32(N_Limit * PulsePerMM[Axis]), Convert.ToInt32(P_Limit * PulsePerMM[Axis]));
                return Result;
            }
            else
            {
                return -1;
            }
        }

    }
}
