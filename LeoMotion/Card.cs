using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeoMotion
{
    [Serializable]
    public enum MoveType
    {
        Relative,
        Keep,
        Absolute,
        HardStop,
        SmoothStop
    }

    [Serializable]
    public abstract class Card
    {

        public Card()
        {

        }
        public string cfgPath = "";

        public Int32 m_CardNo = 0;

        internal Int32 Result = -1;

        public string[] AxisName;

        //输入名称
        public string[] InputNames;
        //输出名称
        public string[] OutputNames;

        //当前输入IO
        public bool[] Singal;

        //逻辑位置
        public double[] ActPos;

        //当前位置
        public double[] LogPos;

        //当前速度
        public double[] CurrentSpeed;

        //轴状态
        public bool[] b_Move;

        //比例转换
        public double[] PulsePerMM;

        //正软限位
        public double[] PSoftLmt;

        //负软限位
        public double[] NSoftLmt;

        //正软限位是否启用？
        public bool[] SoftLmtEnable;
        //设置运行速度
        public double[] SpeedList;
        //设置初始速度
        public double[] StartVList;
        //加速度
        public double[] Aspeed;
        //设置加减速时间
        public double[] TaccList;


        /// <summary>
        /// 该函数用于单轴回负限位
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Pos">第一次往正方向走的脉冲</param>
        /// <param name="Vel">速度  第一次往正方向走的速度，第二次往负限位走的速度为Vel的一半，第三次碰到负限位再往正方向走的速度为 Vel的四分之一</param>
        /// <param name="Offset">原点偏移</param>
        /// <returns></returns>
        public abstract bool GoNLmt(Int32 Axis, double Vel);

        /// <summary>
        /// PTS规划运动
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="count">记数</param>
        /// <param name="pTime">时间</param>
        /// <param name="pPos">位置</param>
        /// <param name="pPercent">百分比</param>
        /// <returns></returns>
        public abstract int PtsTableMove(Int32 Axis, UInt32 Count, double[] Time, double[] Pos, double[] Percent);

        /// <summary>
        /// 该函数用于单轴回原点
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Dir">回原方向 回零方向，0：负向，1：正向</param>
        /// <param name="VelMode">回零速度模式：
        /// 0：低速回零，即以本指令前面的 dmc_set_profile 函数设置的起始速度运行
        /// 1：高速回零，即以本指令前面的 dmc_set_profile 函数设置的最大速度运行</param>
        /// <param name="Vel">速度</param>
        /// <param name="Mode">原点模式</param>
        /// <returns></returns>
        public abstract bool GoHome(Int32 Axis, Int32 Dir, Int32 VelMode, double Vel, int Mode);


        public abstract bool AxisMove(int Axis, MoveType MoveType, double Taget, double StartV, double Speed, double Aspeed, double TAcc);


        /// <summary>
        /// 该函数中包含了控制卡初始化常用的库函数，这是调用其他函数的基础，所以必须在示例程序中最先调用
        /// 返回值<=0表示初始化失败，返回值>0表示初始化成功
        /// </summary>
        /// <returns></returns>
        public abstract Int32 InitBoard();

        /// <summary>
        /// 依据参数的值，判断是匀速还是加减速，设置轴的初始速度、驱动速度和加速度
        /// 返回值=0正确，返回值=1错误
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="StartV">驱动速度</param>
        /// <param name="Speed">加速度</param>
        /// <param name="Add">加速时间</param>
        /// <returns></returns>
        public abstract Int32 SetSpeed(Int32 Axis, double StartV, double Speed, double Aspeed, double TAcc);

        /// <summary>
        /// 该函数用于驱动单个运动轴运动
        /// 返回值=0正确，返回值=1错误
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Value">输出脉冲数</param>
        /// <returns></returns>
        public abstract Int32 AxisPmove(Int32 Axis, double Value);

        /// <summary>
        /// 该函数用于驱动单个轴持续运动
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Direction">方向 0-正向 1-负向 </param>
        /// <returns></returns>
        public abstract Int32 KeepMove(Int32 Axis, Int32 Direction);

        /// <summary>
        /// 参照当前位置,以加减速进行定量移动
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Pulse">脉冲</param>
        /// <param name="StartV">低速</param>
        /// <param name="Speed">高速</param>
        /// <param name="Tacc">加速时间(单位:秒)</param>
        /// <returns></returns>
        public abstract Int32 RelativeMove(Int32 Axis, double Pulse, double StartV, double Speed, double Aspeed, double TAcc);

        /// <summary>
        /// 参照当前位置,以加减速运动模式移动到指定位置
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Pulse">位置</param>
        /// <param name="StartV">低速</param>
        /// <param name="Speed">高速</param>
        /// <param name="Tacc">加速时间(单位:秒)</param>
        /// <returns></returns>
        public abstract Int32 AbsoluteMove(Int32 Axis, double Pulse, double StartV, double Speed, double Aspeed, double TAcc);

        /// <summary>
        /// 该函数用于立即或减速停止轴的驱动
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Mode">减速方式(0－立即停止, 1－减速停止)</param>
        /// <returns></returns>
        public abstract Int32 StopRun(Int32 Axis, Int32 Mode);

        /// <summary>
        /// 该函数用于获取单轴的驱动状态或插补驱动状态
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Value">状态指针(0-驱动结束，非0－正在驱动)</param>
        /// <param name="Mode">(0-获取单轴驱动状态，1－获取插补驱动状态)</param>
        /// <returns></returns>
        public abstract Int32 GetStatus(Int32 Axis, out Int32 Value, Int32 Mode);

        /// <summary>
        ///  该函数用于反馈轴当前的逻辑位置，实际位置和运行速度
        ///  返回值=0正确，返回值=1错误
        /// </summary>
        /// <returns></returns>
        public abstract Int32 GetCurrentInf();


        /// <summary>
        ///  该函数用于输出单点信号
        ///  返回值=0正确，返回值=1错误
        /// </summary>
        /// <param name="Number">输出点(0 ~ 15)</param>
        /// <param name="Value">0-低电平、1－高电平</param>
        /// <returns></returns>
        public abstract Int32 WriteOutput(Int32 Number, Int32 Value);


        /// <summary>
        /// 该函数用于所有输入点
        /// 返回值：0 － 低电平，1 － 高电平，-1 － 错误
        /// </summary>
        /// <returns></returns>
        public abstract void ReadInput();


        /// <summary>
        /// 该函数用于设置逻辑位置和实际位置
        ///  返回值=0正确，返回值=1错误
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Pos">设置的位置值</param>
        /// <param name="Mode">0－设置逻辑位置,非0－设置实际位置</param>
        /// <returns></returns>
        public abstract Int32 SetupPos(Int32 Axis, double Pos, Int32 Mode);

        /// <summary>
        ///  该函数用于设置脉冲的工作方式
        ///  返回值=0正确，返回值=1错误
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Value">
        /// 0 --> 脉冲+脉冲方式
        /// 1 --> 脉冲+方向方式
        /// </param>
        /// <returns></returns>
        public abstract Int32 SetPulseMode(Int32 Axis, Int32 Value);


        /// <summary>
        /// 该函数用于设定正/负方向限位输入nLMT信号的模式
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Value1">正限位： 0 - 有效 ，1 -无效</param>
        /// <param name="Value2">负限位： 0 - 有效 ，1 -无效</param>
        /// <param name="Logic"> 0－低电平有效  1－高电平有效</param>
        /// <returns></returns>
        public abstract Int32 SetLimitMode(Int32 Axis, Int32 Value1, Int32 Value2, Int32 Logic);

        /// <summary>
        /// 该函数用于设定硬件停止信号的模式（急停）
        /// </summary>
        /// <param name="IOindex">急停IO口</param>
        /// <param name="Value">0－无效		 1－有效</param>
        /// <param name="Logic">0－低电平有效  1－高电平有效</param>
        /// <returns></returns>
        public abstract Int32 SetHardStop(Int32 IOindex, Int32 Value, Int32 Logic);

        /// <summary>
        /// 设置软限位
        /// </summary>
        /// <param name="Axis">轴号</param>
        /// <param name="Enable">是否启用 0禁止 1允许</param>
        /// <param name="Source">0：指令位置计数器，1：编码器计数器</param>
        /// <param name="SlAction">0：减速停止，1：立即停止</param>
        /// <param name="N_Limit">负限位</param>
        /// <param name="P_Limit">正限位</param>
        /// <returns></returns>
        public abstract Int32 SetSoftLmt(Int32 Axis, Int32 Enable, Int32 Source, Int32 SlAction, double N_Limit, double P_Limit);

    }
}
