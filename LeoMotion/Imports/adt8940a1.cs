using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace LeoMotion
{
   class adt8940a1
    {
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_initial();
/************************************************
功能：初始化运动控制卡
(1)返回值>0时，表示8940a1卡的数量。如果为3，则下
   面的可用卡号分别为0、1、2；
(2)返回值=0时，说明没有安装8940a1卡；
(3)返回值<0时，-1 表示没有安装端口驱动程序，
               -2 表示PCI桥存在故障。
************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_lib_version(Int32 cardno);
/************************************************
功能：获取当前库版本号
返回值为库版本的版本号
************************************************/
       [DllImport("8940A1m.dll")]
       public static extern float adt8940a1_get_hardware_ver(Int32 cardno);
/**************功能：获取当前硬件版本**************
返回值为硬件版本号
************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_pulse_mode(Int32 cardno, Int32 axis, Int32 value, Int32 logic, Int32 dir_logic);
/*************************************************
功能：设置输出脉冲的工作方式
参数：
	cardno	    卡号
	axis		轴号(1-4)
	value       0：脉冲+脉冲方式		1：脉冲+方向方式
	logic		0：	正逻辑脉冲			1：	负逻辑脉冲
	dir_logic	0：方向输出信号正逻辑	1：方向输出信号负逻辑
返回值			0：正确					1：错误
默认模式：脉冲+方向，正逻辑脉冲，方向输出信号正逻辑
**************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_limit_mode(Int32 cardno, Int32 axis, Int32 v1, Int32 v2, Int32 logic);
/****************************************************
功能：设定正负方向限位输入nLMT信号的模式
参数：
	cardno	    卡号
	axis		轴号(1-4)
	v1          0：正限位有效			1：正限位无效
	v2          0：负限位有效			1：负限位无效
	logic       0：低电平有效			1：高电平有效
返回值			0：正确					1：错误 
默认模式为：正限位有效，负限位有效，低电平有效
*****************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_stop0_mode(Int32 cardno, Int32 axis, Int32 v, Int32 logic);
/****************************************************
功能：设置stop0信号的有效/无效和逻辑电平
参数：
	cardno	    卡号
	axis		轴号(1-4)
	v           0：无效					1：有效
	logic       0：低电平有效			1：高电平有效
返回值			0：正确					1：错误 
默认模式为：无效
*****************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_stop1_mode(Int32 cardno, Int32 axis, Int32 v, Int32 logic);
/****************************************************
功能：设置stop1信号的有效/无效和逻辑电平
参数：
	cardno	    卡号
	axis		轴号(1-4)
	v           0：无效					1：有效
	logic       0：低电平有效			1：高电平有效
返回值			0：正确					1：错误 
默认模式为：无效
******************************************************/

//----------------------------------------------------//
//               驱动状态检查函数                     //
//----------------------------------------------------//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_status(Int32 cardno, Int32 axis, out Int32 v);
/*****************************************************
功能：获取各轴的驱动状态
参数：
	cardno	    卡号
	axis		轴号(1-4)
	v           驱动状态指针
				0：驱动结束				非0：正在驱动
返回值			0：正确					 1：错误 
******************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_inp_status(Int32 cardno, out Int32 v);
/*****************************************************
功能：获取插补的驱动状态
参数：
	cardno      卡号
	v           插补状态指针
				0：插补结束				1：正在插补
返回值			0：正确					1：错误 
*****************************************************/

    [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_stopdata(Int32 cardno, Int32 axis, out Int32 v);
/*************************获取各轴的错误停止信息*******************
功能：获取各轴的错误停止信息
参数：
   cardno	   卡号
   axis		轴号（1-4）
   value	   停止信息的指针
              0：无错误              
              非0：有限位或是STOP停止信号触发.
              通过低四位表示触发脉冲停止的信号类型,正限位:D0,负限位:D1,STOP0:D2,STOP1:D3,
              例如:value值为1时,正限位
                   value值为2时,负限位
                   value值为4时,STOP0
                   value值为8时,STOP1
                  也可能组合出现,value为3（1+2）时,正负限位都触发等等

返回值         0：正确          1：错误
******************************************************************/

//----------------------------------------------------//
//               运动参数设定函数                     //
//----------------------------------------------------//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_acc(Int32 cardno, Int32 axis, Int32 add);
/****************************************************
功能：加速度设定
参数：
	cardno	    卡号
	axis		轴号(1-4)
	add         范围(1-64000)
实际值          add*125
返回值			0：正确					1：错误 

*****************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_startv(Int32 cardno, Int32 axis, Int32 speed);
/****************************************************
功能：初始速度设定
参数：
	cardno	    卡号
	axis		轴号(1-4)
	speed       范围(1-2M)
返回值			0：正确					1：错误 
******************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_speed(Int32 cardno, Int32 axis, Int32 speed);
/****************************************************
功能：驱动速度设定
参数：
	cardno	    卡号
	axis		轴号(1-4)
	speed       范围(1-2M)
返回值			0：正确					1：错误 
******************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_command_pos(Int32 cardno, Int32 axis, Int32 pos);
/****************************************************
功能：逻辑位置设定
参数：
	cardno	    卡号
	axis		轴号(1-4)
	pos         范围(-2147483648～+2147483647)
返回值			0：正确					1：错误 
*****************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_actual_pos(Int32 cardno, Int32 axis, Int32 pos);
/****************************************************
功能：实际位置设定
参数：
	cardno	    卡号
	axis		轴号(1-4)
	pos         范围(-2147483648～+2147483647)
返回值			0：正确					1：错误 
******************************************************/

//----------------------------------------------------//
//               运动参数检查函数                     //
//----------------------------------------------------//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_command_pos(Int32 cardno, Int32 axis, out Int32 pos);
/*****************************************************
功能：获取各轴的逻辑位置
参数：
	cardno	    卡号
	axis		轴号(1-4)
	pos         逻辑位置的指针
返回值			0：正确					1：错误 
*******************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_actual_pos(Int32 cardno, Int32 axis, out Int32 pos);
/*****************************************************
功能：获取各轴的实际位置
参数：
	cardno	    卡号
	axis		轴号(1-4)
	pos         实际位置的指针
返回值			0：正确					1：错误 
*****************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_speed(Int32 cardno, Int32 axis, out Int32 speed);
/*****************************************************
功能：获取各轴的当前驱动速度
参数：
	cardno	    卡号
	axis		轴号(1-4)
	speed       当前驱动速度的指针
返回值			0：正确					1：错误 
*****************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_out(Int32 cardno, Int32 number);
/*****************************************************
功能：获取输出点
参数：
	cardno	    卡号
	number		输出点
返回值			返回值:指定端口的当前状态,-1表示参数错误  
*****************************************************/

//----------------------------------------------------//
//                   驱动函数                         //
//----------------------------------------------------//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_pmove(Int32 cardno, Int32 axis, Int32 pos);
/****************************************************
功能：定量驱动
参数：
	cardno	    卡号
	axis		轴号(1-4)
	pos         输出的脉冲数(-268435455～+268435455)
				>0：正方向驱动			<0：负方向驱动
返回值			0：正确					1：错误
*****************************************************/
[DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_continue_move(Int32 cardno, Int32 axis, Int32 dir);
/*************************功能:单轴连续运动**************************
功能：     单轴连续运动
参数：
   cardno				卡号   
   axis				轴号(1-4)
   dir					0:正向 ;1：负向

返回值：   -1:由限位或STOP信号停止;	1:错误;	    0:正确.
注意:写入驱动命令前,一定要正确地设定速度参数.
       ********************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_dec_stop(Int32 cardno, Int32 axis);
/*****************************************************
功能：驱动减速停止
参数：
	cardno	    卡号
	axis		轴号(1-4)
返回值			0：正确					1：错误
*******************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_sudden_stop(Int32 cardno, Int32 axis);
/*******************************************************
功能：驱动立即停止
参数：
	cardno	    卡号
	axis		轴号(1-4)
返回值			0：正确					1：错误
*********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_inp_move2(Int32 cardno, Int32 axis1, Int32 axis2, Int32 pulse1, Int32 pulse2);
/*********************************************************
功能：两轴直线插补
参数：
	cardno	    卡号
	axis1,axis2  参与插补的轴号
	pulse1,pulse2 移动的相对距离(-8388608～+8388607)
返回值			0：正确					1：错误
*********************************************************/

       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_inp_move3(Int32 cardno, Int32 axis1, Int32 axis2, Int32 axis3, Int32 pulse1, Int32 pulse2, Int32 pulse3);
/*********************************************************
功能：三轴直线插补
参数：
	cardno	    卡号
	axis1,axis2,axis3  参与插补的轴号
	pulse1,pulse2,pulse3 移动的相对距离(-8388608～+8388607)
返回值			0：正确					1：错误
*********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_inp_move4(Int32 cardno, Int32 pulse1, Int32 pulse2, Int32 pulse3, Int32 pulse4);
/*********************************************************
功能：四轴直线插补
参数：
	cardno	    卡号
	pulse1,pulse2,pulse3,pulse4 
	XYZW四轴移动的相对距离(-8388608～+8388607)
返回值			0：正确					1：错误
*********************************************************/

//-------------------------------------------------//
//               开关量输入输出类                  //
//-------------------------------------------------//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_read_bit(Int32 cardno, Int32 number);
/*********************************************************
功能：读取输入点
参数：
	cardno	    卡号
	number      输入点(0-39)
返回值			0：低电平			1：高电平      -1：错误
*********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_write_bit(Int32 cardno, Int32 number, Int32 value);
/********************************************************
功能：输出
参数：
	cardno	    卡号
	number      输出点(0-15)
	value       0: 低电平           1: 高电平
返回值			0：正确				1：错误
*********************************************************/
       [DllImport("8940A1m.dll")]

       public static extern Int32 adt8940a1_set_suddenstop_mode(Int32 cardno, Int32 v, Int32 logic);
/********************************************************
功能：硬件停止模式设置
参数：
	cardno       卡号
	v            0：无效             1：有效
	logic        0：低电平有效       1：高电平有效
返回值			 0：正确			 1：错误
硬件停止信号固定使用P2端子板25引脚(IN31)
*********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_delay_status(Int32 cardno);
/********************************************************
功能：获取延时状态
参数：
	cardno       卡号
返回值			0：延时结束          1：延时进行中
*********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_delay_time(Int32 cardno, Int32 time);
/*********************************************************
功能：设定延时时间
参数：
	cardno       卡号
	time          延时时间
返回值			0：正确				1：错误
时间单位为1/8us
*********************************************************/

//*********************************************//
//               复合驱动类                    //
//*********************************************//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_symmetry_speed(Int32 cardno, Int32 axis, Int32 lspd, Int32 hspd, double tacc);
/*******************************************************
功能:	设定对称加减速的值
参数:
	cardno       卡号
	axis         轴号
	lspd         起步速度
    hspd         驱动速度
	tacc         加速时间
返回值           0:正确          1:错误  
*******************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_symmetry_relative_move(Int32 cardno, Int32 axis, Int32 pulse, Int32 lspd, Int32 hspd, double tacc);
/********************************************************
*功能:参照当前位置,以对称加减速进行定量移动
*参数:
      cardno-卡号
	  axis---轴号
	  pulse--脉冲
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)
返回值         0：正确          1：错误
*********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_symmetry_absolute_move(Int32 cardno, Int32 axis, Int32 pulse, Int32 lspd, Int32 hspd, double tacc);
/*********************************************************
*功能:参照零点位置,以对称加减速进行定量移动
*参数:
      cardno-卡号 
	  axis---轴号
	  pulse--脉冲
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)	
返回值         0：正确          1：错误
**********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_symmetry_relative_line2(Int32 cardno, Int32 axis1, Int32 axis2, Int32 pulse1, Int32 pulse2, Int32 lspd, Int32 hspd, double tacc);
/**********************************************************
*功能:参照当前位置,以对称加减速进行直线插补
*参数:
      cardno-卡号
	  axis1---轴号1
	  axis2---轴号2	
	  pulse1--脉冲1
	  pulse2--脉冲2
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)	 
返回值         0：正确          1：错误
***********************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_symmetry_absolute_line2(Int32 cardno, Int32 axis1, Int32 axis2, Int32 pulse1, Int32 pulse2, Int32 lspd, Int32 hspd, double tacc);
/***********************************************************
*功能:参照零点位置,以对称加减速进行直线插补
*参数:
      cardno-卡号
	  axis1---轴号1
	  axis2---轴号2	
	  pulse1--脉冲1
	  pulse2--脉冲2
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)
返回值         0：正确          1：错误
************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_symmetry_relative_line3(Int32 cardno, Int32 axis1, Int32 axis2, Int32 axis3, Int32 pulse1, Int32 pulse2, Int32 pulse3, Int32 lspd, Int32 hspd, double tacc);
/************************************************************
*功能:参照当前位置,以对称加减速进行直线插补
*参数:
      cardno-卡号
	  axis1---轴号1
	  axis2---轴号2	
	  axis3---轴号3	
	  pulse1--脉冲1
	  pulse2--脉冲2
	  pulse3--脉冲3
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)
返回值         0：正确          1：错误
***************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_symmetry_absolute_line3(Int32 cardno, Int32 axis1, Int32 axis2, Int32 axis3, Int32 pulse1, Int32 pulse2, Int32 pulse3, Int32 lspd, Int32 hspd, double tacc);
/**************************************************************
功能:参照零点位置,以对称加减速进行直线插补
参数:
      cardno-卡号
	  axis1---轴号1
	  axis2---轴号2	
	  axis3---轴号3
	  pulse1--脉冲1
	  pulse2--脉冲2
	  pulse3--脉冲3
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)	
返回值         0：正确          1：错误
****************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_symmetry_relative_line4(Int32 cardno, Int32 pulse1, Int32 pulse2, Int32 pulse3, Int32 pulse4, Int32 lspd, Int32 hspd, double tacc);
/*****************四轴直线插补相对运动****************
*功能:参照当前位置,以加减速进行直线插补
*参数:
      cardno-卡号	  
	  pulse1--脉冲1
	  pulse2--脉冲2
	  pulse3--脉冲3
	  pulse4--脉冲4
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)
******************************************************/
       [DllImport("8940A1m.dll")]

       public static extern Int32 adt8940a1_symmetry_absolute_line4(Int32 cardno, Int32 pulse1, Int32 pulse2, Int32 pulse3, Int32 pulse4, Int32 lspd, Int32 hspd, double tacc);
/*****************四轴对称直线插补绝对运动****************
*功能:参照零点位置,以对称加减速进行直线插补
*参数:
      cardno-卡号	 
	  pulse1--脉冲1
	  pulse2--脉冲2
	  pulse3--脉冲3
	  pulse4--脉冲4
	  lspd---低速
	  hspd---高速
      tacc---加速时间(单位:秒)
******************************************************/

//*********************************************//
//               外部驱动                    //
//*********************************************//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_manual_pmove(Int32 cardno, Int32 axis, Int32 pos);
/************************外部信号定量驱动函数**********************
功能:外部信号定量驱动函数
参数:
	cardno      卡号
	axis        轴号(1-4)
	pos         脉冲
返回值         0：正确          1：错误
	说明:(1)发出定量脉冲，但驱动没有立即进行，需要等到外部信号电平发生变化
		 (2)可以使用普通按钮,也可以接手轮
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_manual_continue(Int32 cardno, Int32 axis);
/************************外部信号连续驱动函数**********************
功能:外部信号连续驱动函数
参数:
	cardno     卡号
	axis       轴号(1-4)
返回值         0：正确          1：错误
	说明:(1)发出定量脉冲，但驱动没有立即进行，需要等到外部信号电平发生变化
		 (2)可以使用普通按钮,也可以接手轮
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_manual_disable(Int32 cardno, Int32 axis);
/***********************关闭外部信号驱动使能***********************
功能:关闭外部信号驱动使能
参数：
	cardno      卡号
	axis        轴号(1-4)
返回值         0：正确          1：错误
******************************************************************/

//*********************************************//
//               位置锁存                    //
//*********************************************//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_set_lock_position(Int32 cardno, Int32 axis, Int32 mode, Int32 regi, Int32 logical);
/****************************位置锁存设置函数**********************
功能:设置到位信号功能,锁定所有轴的逻辑位置和实际位置
参数:
	axis—参照轴
	mode—位置锁存工作模式|0:无效
			              |1:有效
	regi—计数器模式  |0:逻辑位置
					  |1:实际位置 
	logical—电平信号 |0:由高到低 
				      |1:由低到高
返回值         0：正确          1：错误
	说明:使用指定轴axis的IN信号作为触发信号						  
*******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_lock_status(Int32 cardno, Int32 axis, out Int32 v);
/*************************获取锁存状态***********************
功能:获取锁存状态
参数:
	cardno      卡号
	axis         轴号(1-4)
	V            0|未执行同步操作
			     1|执行过同步操作
返回值          0：正确					    1：错误
	说明:利用该函数可以捕捉位置锁存是否执行		
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_lock_position(Int32 cardno, Int32 axis, out Int32 pos);
/**************************获取锁定的位置**************************
功能:获取锁定的位置
参数:
	cardno      卡号
	axis         轴号(1-4)
	pos         锁存的位置
返回值         0：正确          1：错误
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_clr_lock_status(Int32 cardno, Int32 axis);
/**************************清除锁存状态**************************
功能:清除锁存状态
参数:
	cardno      卡号
	axis         轴号(1-4)
返回值         0：正确          1：错误
******************************************************************/


//*********************************************//
//               硬件缓存                    //
//*********************************************//
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_fifo_inp_move1(Int32 cardno, Int32 axis1, Int32 pulse1, Int32 speed);
/**************************单轴缓存**************************
功能:单轴缓存
参数:
	cardno      卡号
	axis1        轴号(1-4)
	pulse1       缓存的脉冲
	speed        缓存的速度
返回值         0：正确          1：错误
说明:共有2048个缓存空间，每条单轴缓存指令占用3个空间，可缓存682条指令
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_fifo_inp_move2(Int32 cardno, Int32 axis1, Int32 axis2, Int32 pulse1, Int32 pulse2, Int32 speed);
/**************************两轴缓存**************************
功能:两轴缓存
参数:
	cardno      卡号
	axis1        轴号(1-4)
	axis2        轴号(1-4)
	pulse1       缓存的脉冲数
	pulse2       缓存的脉冲数
	speed        缓存的速度
返回值         0：正确          1：错误
说明:共有2048个缓存空间，每条两轴缓存指令占用4个空间，可缓存512条指令
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_fifo_inp_move3(Int32 cardno, Int32 axis1, Int32 axis2, Int32 axis3, Int32 pulse1, Int32 pulse2, Int32 pulse3, Int32 speed);
/**************************三轴缓存**************************
功能:三轴缓存
参数:
	cardno      卡号
	axis1        轴号(1-4)
	axis2        轴号(1-4)
	axis3        轴号(1-4)
	pulse1       缓存的脉冲数
	pulse2       缓存的脉冲数
	pulse3       缓存的脉冲数
	speed        缓存的速度
返回值         0：正确          1：错误
说明:共有2048个缓存空间，每条三轴缓存指令占用5个空间，可缓存409条指令
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_fifo_inp_move4(Int32 cardno, Int32 pulse1, Int32 pulse2, Int32 pulse3, Int32 pulse4, Int32 speed);
/**************************四轴缓存**************************
功能:四轴缓存
参数:
	cardno      卡号
	axis1        轴号(1-4)
	axis2        轴号(1-4)
	axis3        轴号(1-4)
	axis4        轴号(1-4)
	pulse1       缓存的脉冲数
	pulse2       缓存的脉冲数
	pulse3       缓存的脉冲数
	pulse4       缓存的脉冲数
	speed        缓存的速度
返回值         0：正确          1：错误
说明:共有2048个缓存空间，每条四轴缓存指令占用6个空间，可缓存341条指令
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_reset_fifo(Int32 cardno);
/**************************重设缓存**************************
功能:清除缓存
参数:
	cardno      卡号
返回值         0：正确          1：错误
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_read_fifo_count(Int32 cardno, out Int32 value);
/**************************读取缓存数**********************
功能:读取缓存数，存放进去的指令还剩多少条未执行
参数:
	cardno      卡号
	value       未执行的指令所占的字节数
返回值         0：正确          1：错误
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_read_fifo_empty(Int32 cardno);
/**************************读取缓存状态**********************
功能:读取缓存是否为空
参数:
	cardno      卡号
返回值          0：非空          1：空
******************************************************************/
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_read_fifo_full(Int32 cardno);
/**************************读取缓存状态**********************
功能:读取缓存是否满了，满了之后将不能再存数据
参数:
	cardno      卡号
返回值           0：未满          1：满
******************************************************************/

        [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_fifo_arc(Int32 cardno, Int32 axis1, Int32 axis2, Int32 startposx, Int32 startposy, Int32 endposx, Int32 endposy, Int32 centerx, Int32 centery, Int32 dir, Int32 startv, Int32 speedv, Int32 acc);
///**************************************************************
//功能：硬件缓存2轴圆弧插补
//参数：
//cardno	         卡号
//axis1,axis2	     插补轴号(1-4)
//startpos,endpos,center 对应圆弧起点，终点，和圆心坐标
//dir              圆弧方向:0顺时针,1逆时针
//startv           起步速度       
//speedv           运行速度		
//acc              加速度		
//返回值		0：正确		-1：参数错误    -2 限位停止    1 外部停止		
//***************************************************************/



//*********************************************//
//               回原点                        //
//*********************************************//
       [DllImport("8940A1m.dll")]
public static extern Int32 adt8940a1_SetHomeMode_Ex(Int32 m_nCardNum, Int32 m_nAxisNum, Int32 m_nHomeDir, Int32 m_nStop0Active, Int32 m_nLimitActive, Int32 m_nStop1Active,
                            Int32 m_nBackRange, Int32 m_nEncoderZRange, Int32 m_nOffset);
///////////////////////////////////////////////////
//功能：设置回零信号，步骤参数
//参数：
// 	Int32    m_nCardNum		//卡号
// 	Int32    m_nAxisNum		//轴号
// 	Int32    m_nHomeDir		//回零方向 0:负方向 1:正方向
// 	Int32    m_nStop0Active	//stop0 有效电平设置；0：低电平停止	1：高电平停止
// 	Int32    m_nLimitActive	//limit信号 有效电平设置；0：低电平停止	1：高电平停止
// 	Int32    m_nStop1Active	//stop1 有效电平设置；0：低电平停止	1：高电平停止
// 	Int32   m_nBackRange		//反向距离 >1
// 	Int32   m_nEncoderZRange	//编码器Z相范围 >1
// 	Int32   m_nOffset		//原点偏移量；==0不偏移，>0正方向偏移，<0负方向偏移	
//返回值		0：正确					-1至-8：错误类型
//错误信息提示
//  -1   //参数1错误
//  -2   //参数2错误
//  -3   //参数3错误
//  -4   //参数4错误
//  -5   //参数5错误
//  -6   //参数6错误
//  -7   //参数7错误
//  -8   //参数8错误
////////////////////////////////////////////////////
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_SetHomeSpeed_Ex(Int32 m_nCardNum, Int32 m_nAxisNum, Int32 m_nStartSpeed, Int32 m_nSearchSpeed, Int32 m_nHomeSpeed, Int32 m_nAcc, Int32 m_nZPhaseSpeed);
///////////////////////////////////////////////////
//功能：回零速度参数
//参数：
// 	Int32    m_nCardNum		//卡号
// 	Int32    m_nAxisNum		//轴号
// 	Int32   m_nStartSpeed	//原点(STOP0)搜寻起始速度
// 	Int32   m_nSearchSpeed	//原点搜寻速度
// 	Int32   m_nHomeSpeed		//低速接近原点速度
// 	Int32   m_nAcc			//回原点过程中的加速度
// 	Int32   m_nZPhaseSpeed	//编码器Z相(STOP1)搜寻速度
//返回值		0：正确					-1至-7：错误类型
//错误信息提示
//  -1   //参数1错误
//  -2   //参数2错误
//  -3   //参数3错误
//  -4   //参数4错误
//  -5   //参数5错误
//  -6   //参数6错误
//  -7   //参数7错误
///////////////////////////////////////////////////
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_HomeProcess_Ex(Int32 m_nCardNum, Int32 m_nAxisNum);
///////////////////////////////////////////////////
//功能：启动回零
//参数：
// 	Int32    m_nCardNum		//卡号
// 	Int32    m_nAxisNum		//轴号
//返回值		0：正确					1：错误
//说明	调用该函数时启动回零动作
////////////////////////////////////////////////////
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_GetHomeStatus_Ex(Int32 m_nCardNum, Int32 m_nAxisNum);
///////////////////////////////////////////////////
//功能：获取回零状态
//参数：
// 	int    m_nCardNum		//卡号
// 	int    m_nAxisNum		//轴号
//返回值 0:回零成功;-1:参数1错误;-2:参数2错误;-3:回零未启动;
//(1-10)执行的步骤1 :快速接近原点，搜索STOP0
//   			  2 :检查STOP0是否找到
//			      3 :反向退出原点
//			      4 :检查反向退出原点是否完成
//			      5 :低速接近原点，搜索STOP0
//			      6 :检查STOP0搜索是否完成
//			  	  7 :低速接近Z相，搜索STOP1.如果STOP1设置为-1，则跳过7,8两步。
//			  	  8 :检查STOP1搜索是否完成
//			  	  9 :原点偏移
//			 	 10 :检查原点偏移
//		   	   -100x:回零第x步出现异常，例如-1001表示回零第1步出现异常
//			   -1020:回零被终止
////////////////////////////////////////////////////
       [DllImport("8940A1m.dll")]
       public static extern Int32 adt8940a1_get_card_index(Int32[] cardIndex);

   }
}
