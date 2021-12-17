using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advantech.Motion;

namespace LeoMotion
{
    /// <summary>
    /// 研华
    /// </summary>
    [Serializable]
    
    public class AdvanTech : Card
    {
        uint Ret = 0;

        public AdvanTech()
        {

        }

        public AdvanTech(int AxisNum)
        {
            LogPos = new double[AxisNum];
            ActPos = new double[AxisNum];
            CurrentSpeed = new double[AxisNum];
            b_Move = new bool[AxisNum];
            SoftLmtEnable = new bool[AxisNum];
            PSoftLmt = new double[AxisNum];
            NSoftLmt = new double[AxisNum];
            PulsePerMM = new double[AxisNum];
            Singal = new bool[16];
        }

        //适用于多卡情况
        public AdvanTech(int cardNo, int AxisNum)
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
            Singal = new bool[16];
        }

        public override int AbsoluteMove(int Axis, double Pulse, double StartV, double Speed,double ASpeed, double Tacc)
        {
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelLow, StartV * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelHigh, Speed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxAcc, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxDec, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxJerk, 0);
            Ret = Motion.mAcm_AxMoveAbs(m_hand[Axis], Pulse * PulsePerMM[Axis]);
            return (int)Ret;
        }

        public override bool AxisMove(int Axis, MoveType MoveType, double Taget, double StartV, double Speed, double ASpeed, double TAcc)
        {
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelLow, StartV * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelHigh, Speed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxAcc, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxDec, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxJerk, 0);
            switch (MoveType)
            {
                case MoveType.Relative:
                    Ret = Motion.mAcm_AxMoveRel(m_hand[Axis], Taget * PulsePerMM[Axis]);
                    break;
                case MoveType.Keep:
                    if (Taget > 0)
                    {
                        Ret = Motion.mAcm_AxMoveVel(m_hand[Axis], 0);
                    }
                    else if (Taget < 0)
                    {
                        Ret = Motion.mAcm_AxMoveVel(m_hand[Axis], 1);
                    }
                    return false;
                case MoveType.Absolute:
                    Ret = Motion.mAcm_AxMoveAbs(m_hand[Axis], Taget * PulsePerMM[Axis]);
                    break;
                case MoveType.HardStop:
                    Ret = Motion.mAcm_AxStopEmg(m_hand[Axis]);
                    break;
                case MoveType.SmoothStop:
                    Ret = Motion.mAcm_AxStopDec(m_hand[Axis]);
                    break;
                default:
                    break;
            }
            ushort status = 0;
            do
            {
                Ret = Motion.mAcm_AxGetState(m_hand[Axis], ref status);
            } while (status != 1);
            return true;
        }

        public override int AxisPmove(int Axis, double Value)
        {
            Ret = Motion.mAcm_AxMoveRel(m_hand[Axis], Value * PulsePerMM[Axis]);
            return (int)Ret;
        }

        public override int GetCurrentInf()
        {
            for (int i = 0; i < LogPos.Length; i++)
            {
                double cmdPos = 0, actPos = 0, speed = 0;
                Ret = Motion.mAcm_AxGetActualPosition(m_hand[i], ref actPos);
                ActPos[i] = actPos / PulsePerMM[i];
                Ret = Motion.mAcm_AxGetCmdPosition(m_hand[i], ref cmdPos);
                LogPos[i] = cmdPos / PulsePerMM[i];
                Ret = Motion.mAcm_AxGetCmdVelocity(m_hand[i], ref speed);
                CurrentSpeed[i] = speed / PulsePerMM[i];

                ushort status = 0;
                Ret = Motion.mAcm_AxGetState(m_hand[i], ref status);
                if (status != 1)//
                {
                    if (status == 3)
                    {
                        Ret = Motion.mAcm_AxResetError(m_hand[i]);
                    }
                    b_Move[i] = true;
                }
                else
                {
                    b_Move[i] = false;
                }
            }
            for (int j = 0; j < Singal.Length; j++)
            {
                byte bitdata = 0;
                Ret = Motion.mAcm_AxDiGetBit(m_hand[(j / 4)], (ushort)(j % 4), ref bitdata);
                if (bitdata == 1)
                {
                    Singal[j] = true;
                }
                else
                {
                    Singal[j] = false;
                }
            }
            return (int)Ret;
        }

        public override int GetStatus(int Axis, out int Value, int Mode)
        {
            uint value = 0;
            uint result = 0;
            result = Motion.mAcm_AxGetMotionStatus(m_hand[Axis], ref value);  //0：指定轴正在运行，1：指定轴已停止
            Value = (int)value;

            return (int)result;
        }

        public override bool GoHome(int Axis, int Dir, int VelMode, double Vel, int Mode)
        {
            Ret = Motion.mAcm_AxHome(m_hand[Axis], (uint)11, (uint)Dir); //原点加反找
            ushort status = 0;
            do
            {
                Ret = Motion.mAcm_AxGetState(m_hand[Axis], ref status);
            } while (status != 1);
            return true;
        }

        public override bool GoNLmt(int Axis, double Vel)
        {
            Ret = Motion.mAcm_AxHome(m_hand[Axis], (uint)12, (uint)1); //负限位加反找
            ushort status = 0;
            do
            {
                Ret = Motion.mAcm_AxGetState(m_hand[Axis], ref status);
            } while (status != 1);
            return true;
        }


        [NonSerialized]
        IntPtr[] m_hand = new IntPtr[16];

        public override int InitBoard()
        {
            uint deviceCount = 0;
            int result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
            if (result == (int)ErrorCode.SUCCESS)
            {
                IntPtr m_DeviceHandle = IntPtr.Zero;
                uint axisCount = 0;
                //IntPtr m_hand = IntPtr.Zero;
                Ret = Motion.mAcm_DevOpen((uint)CurAvailableDevs[0].DeviceNum, ref m_DeviceHandle);

                Motion.mAcm_GetU32Property(m_DeviceHandle, (uint)PropertyID.FT_DevAxesCount, ref axisCount);
                for (int i = 0; i < 4; i++)
                {
                    Ret = Motion.mAcm_AxOpen(m_DeviceHandle, (ushort)i, ref m_hand[i]);
                    if (i == 0)
                    {
                        Ret = Motion.mAcm_AxSetSvOn(m_hand[i], 1);
                    }
                }
                Ret = Motion.mAcm_DevLoadConfig(m_DeviceHandle, cfgPath);
            }
            return (int)Ret;
        }

        public override int KeepMove(int Axis, int Direction)
        {
            uint result = Motion.mAcm_AxMoveVel(m_hand[Axis], (ushort)Direction);
            return (int)result;
        }

        public override int PtsTableMove(int Axis, uint Count, double[] Time, double[] Pos, double[] Percent)
        {
            throw new NotImplementedException();
        }

        public override void ReadInput()
        {
            for (int j = 0; j < Singal.Length; j++)
            {
                byte bitdata = 0;
                Ret = Motion.mAcm_AxDiGetBit((IntPtr)(j / 4), (ushort)(j % 4), ref bitdata);
                if (bitdata == 0)
                {
                    Singal[j] = true;
                }
                else
                {
                    Singal[j] = false;
                }
            }
        }

        public override int RelativeMove(int Axis, double Pulse, double StartV, double Speed, double ASpeed, double Tacc)
        {
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelLow, StartV * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelHigh, Speed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxAcc, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxDec, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxJerk, 0);
            Ret = Motion.mAcm_AxMoveRel(m_hand[Axis], Pulse * PulsePerMM[Axis]);
            return (int)Ret;
        }

        public override int SetHardStop(int IOindex, int Value, int Logic)
        {
            throw new NotImplementedException();
        }

        public override int SetLimitMode(int Axis, int Value1, int Value2, int Logic)
        {
            //启用
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxElEnable, (uint)1);
            //模式
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxElReact, (uint)0);
            //电平设置
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxElLogic, (uint)0);
            return (int)Ret;
        }

        [NonSerialized]
        DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];
        public override int SetPulseMode(int Axis, int Value)
        {
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.CFG_AxPulseOutMode, (uint)0);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.CFG_AxPulseOutReverse, (uint)0);
            return 0;
        }

        public override int SetSoftLmt(int Axis, int Enable, int Source, int SlAction, double N_Limit, double P_Limit)
        {
            //启用
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxSwMelEnable, (uint)Enable); //设置负方向
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxSwPelEnable, (uint)Enable); //设置正方向
            //反应模式
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxSwMelReact, (uint)Enable);  //负
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxSwPelReact, (uint)Enable);  //正
            //值
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxSwMelValue, (uint)(N_Limit * PulsePerMM[Axis])); //负
            Ret = Motion.mAcm_SetU32Property(m_hand[Axis], (uint)PropertyID.CFG_AxSwPelValue, (uint)(P_Limit * PulsePerMM[Axis])); //正
            return (int)Ret;
        }

        public override int SetSpeed(int Axis, double StartV, double Speed, double ASpeed, double Add)
        {
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelLow, StartV * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxVelHigh, Speed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxAcc, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxDec, ASpeed * PulsePerMM[Axis]);
            Ret = Motion.mAcm_SetF64Property(m_hand[Axis], (uint)PropertyID.PAR_AxJerk, 0);
            return (int)Ret;
        }

        public override int SetupPos(int Axis, double Pos, int Mode)
        {
            switch (Mode)
            {
                case 0:
                    Ret = Motion.mAcm_AxSetCmdPosition(m_hand[Axis], Pos * PulsePerMM[Axis]);
                    break;
                case 1:
                    Ret = Motion.mAcm_AxSetActualPosition(m_hand[Axis], Pos * PulsePerMM[Axis]);
                    break;
                default:
                    break;
            }
            return (int)Ret;
        }

        public override int StopRun(int Axis, int Mode)
        {
            switch (Mode)
            {
                case 0:
                    Ret = Motion.mAcm_AxStopEmg(m_hand[Axis]);
                    break;
                case 1:
                    Ret = Motion.mAcm_AxStopDec(m_hand[Axis]);
                    break;
                default:
                    break;
            }
            return (int)Ret;
        }

        public override int WriteOutput(int Number, int Value)
        {
            Ret = Motion.mAcm_AxDoSetBit(m_hand[(int)(Number / 4)], (ushort)(Number % 4 + 4), (byte)Value);
            return (int)Ret;
        }
    }
}
