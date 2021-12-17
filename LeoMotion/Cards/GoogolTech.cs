using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeoMotion
{
    /// <summary>
    /// 固高
    /// </summary>
    [Serializable]
    public class GoogolTech : Card
    {
        short Ret = 0;
        public GoogolTech()
        {

        }

        public GoogolTech(int AxisNum)
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
        public GoogolTech(int cardNo, int AxisNum)
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
        /// //获取数字量输入状态,0-正极限状态；1-负极限状态；2-驱动报警；3-原点状态；4通用输入；5-电机到位；6-手轮输入；10轴使能；11报警解除；12输出状态
        /// </summary>
        public enum StateType
        {
            PositiveState = 0,
            NegativeState = 1,
            AlarmState = 2,
            OriState = 3,
            InputState = 4,
            AxisArrive = 5,
            MPG = 6,
            AxisOn = 10,
            AlarmClear = 11,
            OutputState = 12,
        }

        /// <summary>
        /// 根据信号类型获取输入信号
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        public bool[] GetState(StateType stateType)
        {
            int diState = 0;
            string diStr = "";
            //获取数字量输入状态,0-正极限状态；1-负极限状态；2-驱动报警；3-原点状态；4通用输入；5-电机到位；
            if ((int)stateType >= 10)
            {
                Ret = mc.GT_GetDo(Convert.ToInt16(stateType), out diState);
            }
            else
            {
                Ret = mc.GT_GetDi(Convert.ToInt16(stateType), out diState);
            }

            //转换为二进制字符串
            if (stateType == StateType.InputState || stateType == StateType.OutputState)
            {
                diStr = Convert.ToString(diState, 2).PadLeft(16, '0');
            }
            else if (stateType == StateType.AxisArrive || stateType == StateType.AxisOn)
            {
                diStr = Convert.ToString(diState, 2).PadLeft(4, '0');
            }
            else
            {
                diStr = Convert.ToString(diState, 2).PadLeft(8, '0');
            }

            #region 判断是否为真，并返回
            bool[] state = new bool[diStr.Length];
            //正限位
            for (int i = 0; i < diStr.Length; i++)
            {
                string strtemp = diStr.Substring(diStr.Length - (i + 1), 1);
                if (stateType == StateType.AxisArrive)
                {
                    state[i] = !strtemp.Equals("0");
                }
                else
                {
                    state[i] = strtemp.Equals("0");
                }
            }
            return state;
            #endregion
        }


        public override int AbsoluteMove(int Axis, double Pulse, double StartV, double Speed, double ASpeed, double TAcc)
        {
            Axis++;
            mc.TTrapPrm traParam;
            //清除当前轴报警
            Ret = mc.GT_ClrSts((short)Axis, 1);
            Ret = mc.GT_PrfTrap((short)Axis);
            Ret = mc.GT_GetTrapPrm((short)Axis, out traParam);
            //设置目标参数
            traParam.acc = ASpeed * PulsePerMM[Axis - 1] / 1000000;
            traParam.dec = ASpeed * PulsePerMM[Axis - 1] / 1000000;
            traParam.velStart = StartV * PulsePerMM[Axis - 1] / 1000;
            traParam.smoothTime = (short)(TAcc * 1000);
            Ret = mc.GT_SetTrapPrm((short)Axis, ref traParam);
            //设置目标位置及目标速度
            Ret = mc.GT_SetVel((short)Axis, Speed * PulsePerMM[Axis - 1] / 1000);
            Ret = mc.GT_SetPos((short)Axis, (int)(Pulse * PulsePerMM[Axis - 1]));
            return mc.GT_Update(1 << Axis - 1);
        }

        public override bool AxisMove(int Axis, MoveType MoveType, double Taget, double StartV, double Speed, double ASpeed, double TAcc)
        {
            Axis++;
            double nPos;
            uint pClock;
            mc.TTrapPrm traParam;
            mc.TJogPrm jogParam;
            //清除当前轴报警
            Ret = mc.GT_ClrSts((short)Axis, 1);
            switch (MoveType)
            {
                case MoveType.Relative:
                    //设置点位模式启动
                    Ret = mc.GT_GetPrfPos((short)Axis, out nPos, 1, out pClock);
                    Ret = mc.GT_PrfTrap((short)Axis);
                    Ret = mc.GT_GetTrapPrm((short)Axis, out traParam);
                    //设置目标参数
                    traParam.acc = ASpeed * PulsePerMM[Axis - 1] / 1000000;
                    traParam.dec = ASpeed * PulsePerMM[Axis - 1] / 1000000;
                    traParam.velStart = StartV * PulsePerMM[Axis - 1] / 1000;
                    traParam.smoothTime = (short)(TAcc * 1000);
                    Ret = mc.GT_SetTrapPrm((short)Axis, ref traParam);
                    //设置目标位置及目标速度
                    Ret = mc.GT_SetVel((short)Axis, Speed * PulsePerMM[Axis - 1] / 1000);
                    Ret = mc.GT_SetPos((short)Axis, Convert.ToInt32(nPos) + (int)(Taget * PulsePerMM[Axis - 1]));
                    Ret = mc.GT_Update(1 << Axis - 1);
                    break;
                case MoveType.Absolute:
                    //设置点位模式启动
                    Ret = mc.GT_PrfTrap((short)Axis);
                    Ret = mc.GT_GetTrapPrm((short)Axis, out traParam);
                    //设置目标参数
                    traParam.acc = ASpeed * PulsePerMM[Axis - 1] / 1000000;
                    traParam.dec = ASpeed * PulsePerMM[Axis - 1] / 1000000;
                    traParam.velStart = StartV * PulsePerMM[Axis - 1] / 1000;
                    traParam.smoothTime = (short)(TAcc * 1000);
                    Ret = mc.GT_SetTrapPrm((short)Axis, ref traParam);
                    //设置目标位置及目标速度
                    Ret = mc.GT_SetVel((short)Axis, Speed * PulsePerMM[Axis - 1] / 1000);
                    Ret = mc.GT_SetPos((short)Axis, (int)(Taget * PulsePerMM[Axis - 1]));
                    Ret = mc.GT_Update(1 << Axis - 1);
                    break;
                case MoveType.Keep:
                    //选中选择的轴 位置清零
                    Ret = mc.GT_ZeroPos((short)Axis, 1);
                    Ret = mc.GT_PrfJog((short)Axis);
                    Ret = mc.GT_GetJogPrm((short)Axis, out jogParam);
                    jogParam.acc = ASpeed * PulsePerMM[Axis - 1] / 1000;
                    jogParam.dec = ASpeed * PulsePerMM[Axis - 1] / 1000;
                    Ret = mc.GT_SetJogPrm((short)Axis, ref jogParam);
                    Ret = mc.GT_SetVel((short)Axis, Speed * PulsePerMM[Axis - 1] / 1000);
                    Ret = mc.GT_Update(1 << (Axis - 1));
                    return false;//持续运动一直不停 所以为false
                case MoveType.HardStop:
                    Ret = mc.GT_Stop(1 << Axis - 1, 1 << Axis - 1);
                    return true;
                case MoveType.SmoothStop:
                    Ret = mc.GT_Stop(1 << Axis - 1, 0 << Axis - 1);
                    return true;
                default:
                    return false;
            }
            if (MoveType == MoveType.Relative || MoveType == MoveType.Absolute)
            {
                int pSts;
                do
                {
                    Ret = mc.GT_GetSts((short)Axis, out pSts, 1, out pClock);
                } while ((pSts & (1 << 10)) != 0);
            }
            return true;
        }

        public override int AxisPmove(int Axis, double Value)
        {
            Axis++;
            double nPos;
            uint pClock;
            //清除当前轴报警
            Ret = mc.GT_ClrSts((short)Axis, 1);
            Ret = mc.GT_GetPrfPos((short)Axis, out nPos, 1, out pClock);
            Ret = mc.GT_PrfTrap((short)Axis);

            Ret = mc.GT_SetPos((short)Axis, (int)(nPos + Value * PulsePerMM[Axis - 1]));
            return mc.GT_Update(1 << Axis - 1);
        }

        public override int GetCurrentInf()
        {
            b_Move = GetState(StateType.AxisArrive);
            Singal = GetState(StateType.InputState);
            for (int i = 0; i < SpeedList.Length; i++)
            {
                uint pClock;
                double pos; double speed;
                Ret = mc.GT_GetEncPos((short)(i + 1), out pos, 1, out pClock);
                Ret = mc.GT_GetEncVel((short)(i + 1), out speed, 1, out pClock);
                LogPos[i] = (double)pos / PulsePerMM[i];
                CurrentSpeed[i] = (double)speed * 1000 / PulsePerMM[i];
            }
            return 0;
        }

        public override int GetStatus(int Axis, out int Value, int Mode)
        {
            throw new NotImplementedException();
        }

        public override bool GoHome(int Axis, int Dir, int VelMode, double Vel, int Mode)
        {
            Axis++;
            mc.THomePrm homeParam;
            mc.THomeStatus homeStatus;
            Ret = mc.GT_GetHomePrm((short)Axis, out homeParam);
            Ret = mc.GT_AlarmOff((short)Axis);
            Ret = mc.GT_ClrSts((short)Axis, 1);
            Ret = mc.GT_ZeroPos((short)Axis, 1);
            Ret = mc.GT_AxisOn((short)Axis);

            Ret = mc.GT_GetHomePrm((short)Axis, out homeParam);
            homeParam.mode = 11; //限位加反找
            homeParam.moveDir = -1; //负方向回
            homeParam.indexDir = -1;
            homeParam.edge = 0;
            homeParam.velHigh = Vel * PulsePerMM[Axis - 1] / 1000;
            homeParam.velLow = Vel * PulsePerMM[Axis - 1] / 2000;
            homeParam.acc = 0.1;
            homeParam.dec = 0.1;
            homeParam.searchHomeDistance = 0;
            homeParam.searchIndexDistance = 30000;
            homeParam.escapeStep = 1000;
            Ret = mc.GT_GoHome((short)Axis, ref homeParam);
            do
            {
                Ret = mc.GT_GetHomeStatus((short)Axis, out homeStatus);
            } while (homeStatus.run != 0);
            Ret = mc.GT_SetEncPos((short)Axis, 0);
            return true;
        }

        public override bool GoNLmt(int Axis, double Vel)
        {
            Axis++;
            mc.THomePrm homeParam;
            mc.THomeStatus homeStatus;
            Ret = mc.GT_AlarmOff((short)Axis);
            Ret = mc.GT_ClrSts((short)Axis, 1);
            Ret = mc.GT_ZeroPos((short)Axis, 1);
            Ret = mc.GT_AxisOn((short)Axis);

            Ret = mc.GT_GetHomePrm((short)Axis, out homeParam);
            homeParam.mode = 10; //限位回零
            homeParam.moveDir = -1; //负方向回
            homeParam.indexDir = -1;
            homeParam.edge = 0;
            homeParam.velHigh = Vel * PulsePerMM[Axis - 1] / 1000;
            homeParam.velLow = Vel * PulsePerMM[Axis - 1] / 2000;
            homeParam.acc = 0.1;
            homeParam.dec = 0.1;
            homeParam.searchHomeDistance = 0;
            homeParam.searchIndexDistance = 30000;
            homeParam.escapeStep = 1000;
            Ret = mc.GT_GoHome((short)Axis, ref homeParam);
            do
            {
                Ret = mc.GT_GetHomeStatus((short)Axis, out homeStatus);
            } while (homeStatus.run != 0);
            Thread.Sleep(300);
            Ret = mc.GT_ZeroPos((short)Axis, 1);
            return true;
        }

        public override int InitBoard()
        {
            try
            {
                Ret = Ret = mc.GT_Open(0, 1);
                Ret = Ret = mc.GT_Reset();
                Ret = Ret = mc.GT_LoadConfig(cfgPath);
                Ret = Ret = mc.GT_ClrSts(1, (short)SpeedList.Length);
                for (int i = 0; i < SpeedList.Length; i++)
                {
                    Ret = Ret = mc.GT_AxisOn((short)(i + 1));
                }
                return Ret;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override int KeepMove(int Axis, int Direction)
        {
            Axis++;
            double Vel;
            Ret = mc.GT_PrfJog((short)Axis);
            Ret = mc.GT_GetVel((short)Axis, out Vel);
            if (Direction == 0)
            {
                Ret = mc.GT_SetVel((short)Axis, Vel / 1000);
            }
            else
            {
                Ret = mc.GT_SetVel((short)Axis, Vel * -1 / 1000);
            }
            return mc.GT_Update(1 << (Axis - 1));
        }

        public override int PtsTableMove(int Axis, uint Count, double[] Time, double[] Pos, double[] Percent)
        {
            short tableID = 1;
            Ret = mc.GT_ClrSts((short)0, (short)(SpeedList.Length));
            Ret = mc.GT_AxisOn((short)Axis);
            Ret = mc.GT_PrfPvt((short)Axis);
            //mc.GT_PvtTable(tableID, Time.Length, ref Time[0], ref Pos[0],  0);
            Ret = mc.GT_PvtTablePercent(tableID, Time.Length, ref Time[0], ref Pos[0], ref Percent[0], 0);
            return mc.GT_PvtStart(1 << Axis - 1);
        }

        public override void ReadInput()
        {
            Singal = GetState(StateType.InputState);
        }

        public override int RelativeMove(int Axis, double Pulse, double StartV, double Speed, double ASpeed, double Tacc)
        {
            Axis++;
            double nPos;
            uint pClock;
            mc.TTrapPrm traParam;
            //设置点位模式启动
            Ret = mc.GT_GetPrfPos((short)Axis, out nPos, 1, out pClock);
            Ret = mc.GT_PrfTrap((short)Axis);
            Ret = mc.GT_GetTrapPrm((short)Axis, out traParam);
            //设置目标参数
            traParam.acc = ASpeed * PulsePerMM[Axis - 1] / 1000000;
            traParam.dec = ASpeed * PulsePerMM[Axis - 1] / 1000000;
            traParam.velStart = StartV * PulsePerMM[Axis - 1] / 1000;
            traParam.smoothTime = (short)(Tacc * 1000);
            Ret = mc.GT_SetTrapPrm((short)Axis, ref traParam);
            //设置目标位置及目标速度
            Ret = mc.GT_SetVel((short)Axis, Speed * PulsePerMM[Axis - 1] / 1000);
            Ret = mc.GT_SetPos((short)Axis, Convert.ToInt32(nPos) + (int)(Pulse * PulsePerMM[Axis - 1]));
            return mc.GT_Update(1 << Axis - 1);
        }

        public override int SetHardStop(int IOindex, int Value, int Logic)
        {
            if (Value == 1)
            {
                for (int i = 0; i < SpeedList.Length; i++)
                {
                    Ret = mc.GT_SetStopIo((short)i, 0, 4, (short)(IOindex + 1));
                }
            }
            return 0;
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
            if (Value1 == 0 && Value2 == 0) //都有效
            {
                return mc.GT_LmtsOn((short)Axis, -1);
            }
            else if (Value1 == 1 && Value2 == 0)
            {
                return mc.GT_LmtsOn((short)Axis, 0);
            }
            else if (Value1 == 0 && Value2 == 1)
            {
                return mc.GT_LmtsOn((short)Axis, 1);
            }
            else if (Value1 == 1 && Value2 == 1)
            {
                return mc.GT_LmtsOff((short)Axis, -1);
            }
            else
            {
                return -1;
            }
        }

        public override int SetPulseMode(int Axis, int Value)
        {
            if (Value == 0)
            {
                return mc.GT_StepPulse((short)Axis);
            }
            else
            {
                return mc.GT_StepDir((short)Axis);
            }
        }

        public override int SetSoftLmt(int Axis, int Enable, int Source, int SlAction, double N_Limit, double P_Limit)
        {
            if (Enable == 0) //0禁止
            {
                return mc.GT_SetSoftLimit((short)Axis, 0x7fffffff, -2147483648);
            }
            else
            {
                return mc.GT_SetSoftLimit((short)Axis, (int)(P_Limit * PulsePerMM[Axis]), (int)(N_Limit * PulsePerMM[Axis]));
            }
        }

        public override int SetSpeed(int Axis, double StartV, double Speed, double ASpeed, double Add)
        {
            Axis++;
            mc.TTrapPrm traParam;
            //清除当前轴报警
            Ret = mc.GT_ClrSts((short)Axis, 1);
            Ret = mc.GT_PrfTrap((short)Axis);
            Ret = mc.GT_GetTrapPrm((short)Axis, out traParam);
            //设置目标参数
            traParam.acc = ASpeed * PulsePerMM[Axis - 1] / 1000000;
            traParam.dec = ASpeed * PulsePerMM[Axis - 1] / 1000000;
            traParam.velStart = StartV * PulsePerMM[Axis - 1] / 1000;
            traParam.smoothTime = (short)Add;
            Ret = mc.GT_SetTrapPrm((short)Axis, ref traParam);
            return mc.GT_SetVel((short)Axis, Speed * PulsePerMM[Axis - 1] / 1000);
        }

        public override int SetupPos(int Axis, double Pos, int Mode)
        {
            Axis++;
            return mc.GT_SetEncPos((short)(Axis), (int)(Pos * PulsePerMM[Axis - 1]));
        }

        public override int StopRun(int Axis, int Mode)
        {
            Axis++;
            if (Mode == 0) //急停
            {
                return mc.GT_Stop(1 << Axis - 1, 1 << Axis - 1);
            }
            else
            {
                return mc.GT_Stop(1 << Axis - 1, 0 << Axis - 1);
            }
        }

        public override int WriteOutput(int Number, int Value)
        {
            return mc.GT_SetDoBit(mc.MC_GPO, (short)++Number, (short)Value);
        }
    }
}
