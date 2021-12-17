using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeoMotion
{
    /// <summary>
    /// 众为兴
    /// </summary>
    [Serializable]
    public class ADT4x : Card
    {
        const Int32 MAXAXIS = 4;
        public Int32 m_cardno = 0;

        public bool GoHome1(Int32 Axis)
        {
            if (AxisMove(Axis, MoveType.Relative, 10000, 6000, 6000,12000, 0))
            {
                if (AxisMove(Axis, MoveType.Relative, -100000000, 4000, 8000,8000, 0.5))
                {
                    if (AxisMove(Axis, MoveType.Relative, 1000, 4000, 8000,8000, 0.5))
                    {
                        SetupPos(Axis, 0, 0);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override bool AxisMove(int Axis, MoveType MoveType, double Taget, double StartV, double Speed, double Aspeed, double TAcc)
        {
            switch (MoveType)
            {
                case MoveType.Relative:
                    RelativeMove(Axis, Taget, StartV, Speed, Aspeed, TAcc);
                    break;
                case MoveType.Keep:
                    SetSpeed(Axis, StartV, Speed, Aspeed, Convert.ToInt32((Speed - StartV) / TAcc));
                    if (Taget > 0)
                        KeepMove(Axis, 0);
                    else if (Taget < 0)
                        KeepMove(Axis, 1);
                    return false;
                case MoveType.Absolute:
                    AbsoluteMove(Axis, Taget, StartV, Speed, Aspeed,TAcc);
                    break;
                case MoveType.HardStop:
                    StopRun(Axis, 0);
                    return true;
                case MoveType.SmoothStop:
                    StopRun(Axis, 1);
                    return true;
                default:
                    return false;
            }
            int Result, Value;
            do
            {
                Result = GetStatus(Axis, out Value, 0);
            } while (Value != 0 || Result != 0);
            return true;
        }

        public override int AbsoluteMove(int Axis, double Pulse, double StartV, double Speed, double Aspeed, double TAcc)
        {
            int PluseValue = Convert.ToInt32(Pulse * PulsePerMM[Axis]);
            int StartVValue = Convert.ToInt32(StartV * PulsePerMM[Axis]);
            int SpeedValue = Convert.ToInt32(Speed * PulsePerMM[Axis]);
            Result = adt8940a1.adt8940a1_symmetry_absolute_move(m_cardno, Axis, PluseValue, StartVValue, SpeedValue, TAcc);
            return Result;
        }

        public override int AxisPmove(int Axis, double Value)
        {
            int MoveValue = Convert.ToInt32(Value * PulsePerMM[Axis]);
            Result = adt8940a1.adt8940a1_pmove(m_cardno, Axis, MoveValue);

            return Result;
        }


        public override int GetStatus(int Axis, out int Value, int Mode)
        {
            if (Mode == 0)          //获取单轴驱动状态

                Result = adt8940a1.adt8940a1_get_status(m_cardno, Axis, out Value);

            else                  //获取插补驱动状态

                Result = adt8940a1.adt8940a1_get_inp_status(m_cardno, out Value);

            return Result;
        }

        public override bool GoNLmt(int Axis, double Vel)
        {
            if (AxisMove(Axis, MoveType.Relative, 10000, Vel, Vel, Vel * 2, 0))
            {
                if (AxisMove(Axis, MoveType.Relative, -100000000, Vel / 2, Vel / 2, Vel, 0))
                {
                    if (AxisMove(Axis, MoveType.Relative, 0, Vel / 4, Vel / 4, Vel / 2, 0))
                    {
                        SetupPos(Axis, 0, 0);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override int InitBoard()
        {
            Result = adt8940a1.adt8940a1_initial();         //卡初始化函数    

            if (Result <= 0) return Result;


            return Result;
        }

        public override int KeepMove(int Axis, int Direction)
        {
            Result = adt8940a1.adt8940a1_continue_move(0, Axis, Direction);
            return Result;
        }

        public override void ReadInput()
        {
            int value;
            for (int i = 0; i < Singal.Length; i++)
            {
                value = adt8940a1.adt8940a1_read_bit(m_cardno, i);
                if (value == 1)
                {
                    Singal[i] = true;
                }
                else
                {
                    Singal[i] = false;
                }
            }
        }

        public override int RelativeMove(int Axis, double Pulse, double StartV, double Speed, double Aspeed, double TAcc)
        {
            Int32 pos = Convert.ToInt32(Pulse * PulsePerMM[Axis]);
            Int32 startSpeed = Convert.ToInt32(StartV * PulsePerMM[Axis]);
            Int32 speed = Convert.ToInt32(Speed * PulsePerMM[Axis]);

            Result = adt8940a1.adt8940a1_symmetry_relative_move(m_cardno, Axis, pos, startSpeed, speed, TAcc);

            return Result;
        }

        public override int SetHardStop(int IOindex, int Value, int Logic)
        {
            Result = adt8940a1.adt8940a1_set_suddenstop_mode(m_cardno, Value, Logic);

            return Result;
        }

        public override int SetLimitMode(int Axis, int Value1, int Value2, int Logic)
        {

            Result = adt8940a1.adt8940a1_set_limit_mode(m_cardno, Axis, Value1, Value2, Logic);

            return Result;
        }

        public override int SetPulseMode(int Axis, int Value)
        {
            Result = adt8940a1.adt8940a1_set_pulse_mode(m_cardno, Axis, Value, 0, 0);

            return Result;
        }

        public override int SetSpeed(int Axis, double StartV, double Speed, double Aspeed, double TAcc)
        {

            Int32 startSpeed = Convert.ToInt32(StartV * PulsePerMM[Axis]);
            Int32 speed = Convert.ToInt32(Speed * PulsePerMM[Axis]);

            if (StartV - Speed >= 0) //匀速运动
            {
                Result = adt8940a1.adt8940a1_set_startv(m_cardno, Axis, startSpeed);

                adt8940a1.adt8940a1_set_speed(m_cardno, Axis, startSpeed);

            }
            else                    //加减速运动
            {
                Result = adt8940a1.adt8940a1_set_startv(m_cardno, Axis, startSpeed);

                adt8940a1.adt8940a1_set_speed(m_cardno, Axis, speed);

                adt8940a1.adt8940a1_set_acc(m_cardno, Axis, Convert.ToInt32((speed - startSpeed) / TAcc));
            }

            return Result;
        }

        public override int SetupPos(int Axis, double Pos, int Mode)
        {
            Int32 pos = Convert.ToInt32(Pos * PulsePerMM[Axis]);
            if (Mode == 0)
            {
                Result = adt8940a1.adt8940a1_set_command_pos(m_cardno, Axis, pos);
            }
            else
            {
                Result = adt8940a1.adt8940a1_set_actual_pos(m_cardno, Axis, pos);
            }

            return Result;
        }

        public override int StopRun(int Axis, int Mode)
        {
            if (Mode == 0)        //立即停止

                Result = adt8940a1.adt8940a1_sudden_stop(m_cardno, Axis);

            else                 //减速停止

                Result = adt8940a1.adt8940a1_dec_stop(m_cardno, Axis);

            return Result;

        }

        public override int WriteOutput(int Number, int Value)
        {
            Result = adt8940a1.adt8940a1_write_bit(m_cardno, Number, Value);

            return Result;
        }

        public override int GetCurrentInf()
        {
            int value, currentSpeed;
            for (int i = 0; i < LogPos.Length; i++)
            {
                Int32 logpos, actpos;
                Result = adt8940a1.adt8940a1_get_command_pos(m_cardno, i, out logpos);
                LogPos[i] = logpos / PulsePerMM[i];
                Result = adt8940a1.adt8940a1_get_actual_pos(m_cardno, i, out actpos);
                ActPos[i] = actpos / PulsePerMM[i];
                Result = adt8940a1.adt8940a1_get_speed(m_cardno, i, out currentSpeed);
                CurrentSpeed[i] = currentSpeed / PulsePerMM[i];
                Result = adt8940a1.adt8940a1_get_status(m_cardno, i, out value);
                if (value == 0)
                {
                    b_Move[i] = false;
                }
                else
                {
                    b_Move[i] = false;
                }
            }
            return Result;
        }

        public override bool GoHome(int Axis, int Dir, int VelMode, double Vel, int Mode)
        {
            throw new NotImplementedException();
        }

        public override int PtsTableMove(int Axis, uint count, double[] pTime, double[] pPos, double[] pPercent)
        {
            throw new NotImplementedException();
        }

        public override int SetSoftLmt(int Axis, int Enable, int Source, int SlAction, double N_Limit, double P_Limit)
        {
            throw new NotImplementedException();
        }
    }
}
