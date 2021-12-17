//
// Copyright (c) Advantech Corporation.  All rights reserved.
//
//
// Use of this source code is subject to the terms of the Advantech end-user
// license agreement (EULA) under which you licensed this SOFTWARE PRODUCT.
// If you did not accept the terms of the EULA, you are not authorized to use
// this source code. For a copy of the EULA, please see the LICENSE.RTF on your
// install media.
//

#ifdef __cplusplus
extern "C"
{
#endif

#include "AdvMotDev.h"
#include "AdvMotDrv.h"
#include "AdvMotPropID.h"
#include "AdvMotErr.h"


//Add for other Corp.           //  [6/8/2011 dan.yang]
#ifndef tagPT_DEVLIST
#define tagPT_DEVLIST
typedef struct tagPT_DEVLIST
{
    DWORD   dwDeviceNum;
    char	szDeviceName[50];    	
    SHORT   nNumOfSubdevices;	
} DEVLIST, *LPDEVLIST;
#endif

//  [11/26/2014 dan]
#ifndef _DEV_IO_MAP_INFO
#define _DEV_IO_MAP_INFO
typedef struct _DEV_IO_MAP_INFO
{
	char Name[50];  
	ULONG Index;
	ULONG Offset;
	ULONG ByteLength;
	ULONG SlotID;

	ULONG PortChanID;
	ULONG ModuleID;
	char ModuleName[16];
	char Description[100];
} DEV_IO_MAP_INFO, *PDEV_IO_MAP_INFO;
#endif

#ifndef _ADVAPI_SLAVE_INFO
#define _ADVAPI_SLAVE_INFO
typedef struct {
	ULONG desc; /**< Port descriptors. */
	ULONG link_up; /**< Link detected. */
	ULONG loop_closed; /**< Loop closed. */
	ULONG next_slave; /**< Connected Position of slaves. */
	ULONG delay_to_next_dc;
} port_info;

typedef struct _ADVAPI_SLAVE_INFO_
{
	ULONG SlaveID;
	ULONG Position;
	ULONG VendorID;
	ULONG ProductID;
	ULONG RevisionNo;
	ULONG SerialNo;
	ULONG driverCnts;
	port_info ports[4];
	ULONG transmission_delay;
	char DeviceName[64];
}ADVAPI_SLAVE_INFO, *PADVAPI_SLAVE_INFO;
#endif

#ifndef _ADVAPI_IO_PORT_INFO
#define _ADVAPI_IO_PORT_INFO
typedef struct _ADVAPI_IO_PORT_INFO
{
	U16 PortNum;
	U16 BitLength;
}ADVAPI_IO_PORT_INFO, *PADVAPI_IO_PORT_INFO;
#endif

#ifndef _DEV_ARES_INFO
#define _DEV_ARES_INFO
typedef struct _DEV_ARES_INFO
{
//	char Name[10][32];
	char Name[12][32];
} DEV_ARES_INFO, *PDEV_ARES_INFO;
#endif

#ifndef _DEV_PRE_SCAN_DATA
#define _DEV_PRE_SCAN_DATA
typedef struct _DEV_PRE_SCAN_DATA
{
	F64 XScanData;
	F64 YScanData;
	F64 ZScanData;
} DEV_PRE_SCAN_DATA, *PDEV_PRE_SCAN_DATA;
#endif

#ifndef  ADVCMNAPI
#if (_MSC_VER >= 800) || defined(_STDCALL_SUPPORTED)
#define ADVCMNAPI      __stdcall
#endif
#endif

/*
+-----------------------------------------------------------------------------+
| ************ Advantech Motion Master Device Type ID************************ |
+-----------------------------------------------------------------------------+
*/
//U32	ADVCMNAPI Acm_GetAvailableDevNum(U32 *DeviceNum); //Add for other corp. [dan.yang 2011.06.08]
U32 ADVCMNAPI Acm_GetAvailableDevs(DEVLIST *DeviceList, U32 MaxEntries, PU32 OutEntries); //Add for other corp. [dan.yang 2011.06.08]
void ADVCMNAPI GetAvailalbe_AMONet(DWORD		dwMasDevNum, //Add for other corp. [dan.yang 2011.07.13]
						 DEVLIST    *DeviceList,
						 ULONG      *devIndex,
						 ULONG		*OutEntries);
BOOL ADVCMNAPI Acm_GetErrorMessage(U32 ErrorCode, PI8 lpszError,  U32 nMaxError);//  [9/5/2011 dan.yang]

//Device operation
 U32 ADVCMNAPI Acm_DevOpen(U32 DeviceNumber, PHAND DeviceHandle);
 U32 ADVCMNAPI Acm_DevReOpen(HAND DeviceHandle); //  [11/25/2014 dan]
 U32 ADVCMNAPI Acm_DevClose(PHAND DeviceHandle);
U32 ADVCMNAPI Acm_GetLastError(HAND Handle);
 U32 ADVCMNAPI Acm_DevReadEEPROM(HAND DeviceHandle, U16 EEPROMAddr, PU16 readValue);
 U32 ADVCMNAPI Acm_DevWriteEEPROM(HAND DeviceHandle, U16 EEPROMAddr, U16 writeValue);

 U32 ADVCMNAPI Acm_DevReadEEPROM_Ex(HAND DeviceHandle, U16 PrivateID, PU32 PassWordArray, U32 PassArrayCnt, PU32 ReadArray, U32 BufferLength);//  [6/15/2012 dan.yang]
 U32 ADVCMNAPI Acm_DevWriteEEPROM_Ex(HAND DeviceHandle, U16 PrivateID, PU32 PassWordArray, U32 PassArrayCnt, PU32 WriteArray, U32 BufferLength); //  [6/15/2012 dan.yang]

U32 ADVCMNAPI Acm_GetProperty(HAND Handle, U32 PropertyID, PVOID Buffer, PU32 BufferLength);
U32 ADVCMNAPI Acm_SetProperty(HAND Handle, U32 PropertyID, PVOID Buffer, U32 BufferLength);
 //  [2/28/2014 dan.yang]
U32 ADVCMNAPI Acm_GetU32Property(HAND Handle, U32 PropertyID, PU32 Value);
U32 ADVCMNAPI Acm_GetI32Property(HAND Handle, U32 PropertyID, PI32 Value);
U32 ADVCMNAPI Acm_GetF64Property(HAND Handle, U32 PropertyID, PF64 Value);
U32 ADVCMNAPI Acm_GetStringProperty (HAND Handle, U32 PropertyID, PU8 Value);
U32 ADVCMNAPI Acm_SetU32Property (HAND Handle, U32 PropertyID, U32 Value);
U32 ADVCMNAPI Acm_SetI32Property (HAND Handle, U32 PropertyID, I32 Value);
U32 ADVCMNAPI Acm_SetF64Property (HAND Handle, U32 PropertyID, F64 Value);
U32 ADVCMNAPI Acm_SetStringProperty (HAND Handle, U32 PropertyID, PU8 Value);

U32 ADVCMNAPI Acm_SetMultiProperty (HAND Handle, PU32 PropertyIDArray, PF64 ValueArray, U32 PropertyCnt);
U32 ADVCMNAPI Acm_SetMultiU32Property (HAND Handle, PU32 PropertyIDArray, PU32 ValueArray, U32 PropertyCnt);
U32 ADVCMNAPI Acm_SetMultiI32Property (HAND Handle, PU32 PropertyIDArray, PI32 ValueArray, U32 PropertyCnt);
U32 ADVCMNAPI Acm_SetMultiF64Property (HAND Handle, PU32 PropertyIDArray, PF64 ValueArray, U32 PropertyCnt);

 //  [11/20/2014 dan]
U32 ADVCMNAPI Acm_GetChannelProperty(HAND Handle, U32 ChannelID, U32 PropertyID,  PF64 Value);
U32 ADVCMNAPI Acm_SetChannelProperty(HAND Handle, U32 ChannelID, U32 PropertyID,  F64 Value);
U32 ADVCMNAPI Acm_GetMultiChannelProperty(HAND Handle, U32 PropertyID, U32 StartChID, U32 ChCount, PF64 ValueArray);
U32 ADVCMNAPI Acm_SetMultiChannelProperty(HAND Handle, U32 PropertyID, U32 StartChID, U32 ChCount, PF64 ValueArray);

 U32 ADVCMNAPI Acm_DevEnableEvent(HAND DeviceHandle, U32 DevEnableEvt); //  [11/20/2014 dan]
 U32 ADVCMNAPI Acm_DevCheckEvent(HAND DeviceHandle, PU32 DevCheckEvt, U32 Millisecond); //  [11/20/2014 dan]

 U32 ADVCMNAPI Acm_EnableMotionEvent(HAND DeviceHandle,
						  PU32 AxEnableEvtArray,
						  PU32 GpEnableEvtArray,
						  U32 AxArrayElements,
						  U32 GpArrayElements);
 U32 ADVCMNAPI Acm_CheckMotionEvent(HAND DeviceHandle,
						 PU32 AxEvtStatusArray,
						 PU32 GpEvtStatusArray,
						 U32 AxArrayElements,
						 U32 GpArrayElements,
						 U32 Millisecond);
U32 ADVCMNAPI Acm_CancelCheckEvent(HAND ObjectHandle);
 U32 ADVCMNAPI Acm_DevEnableEvent_All(HAND  DeviceHandle,
	 PU32 DevEnableEvtArray,
	 PU32 AxEnableEvtArray,
	 PU32 GpEnableEvtArray, 
	 U32 AxArrayElements,
	 U32 GpArrayElements) ;
 U32 ADVCMNAPI Acm_DevCheckEvent_All(HAND  DeviceHandle, 
	 PU32  DevEvtStatusArray,
	 PU32  AxEvtStatusArray, 
	 PU32 GpEvtStatusArray,
	 U32  AxArrayElements, 
	 U32  GpArrayElements, 
	 U32  Millisecond);
 U32 ADVCMNAPI Acm_DevLoadConfig(HAND DeviceHandle, PI8 ConfigPath);
 U32 ADVCMNAPI Acm_DevFwDownload(HAND DeviceHandle, U32 Data, U32 DataID); //internal function don't release.
 U32 ADVCMNAPI Acm_DevSlaveFwDownload(HAND DeviceHandle, U16 RingNo, U16 Position, PI8 FileName, PI8 FilePath, U32 Password);  //  [6/4/2015 andy.wang]
 U32 ADVCMNAPI Acm_DevDownloadCAMTable (HAND DeviceHandle, 
							U32 CamTableID, 
							PF64 pMasterArray, 
							PF64 pSlaveArray, 
							PF64 pPointRangeArray, 
							PF64 pPointSlopeArray,    
							U32 ArrayElements);
 U32 ADVCMNAPI Acm_DevLoadCAMTableFile(HAND DeviceHandle, PI8 FilePath, U32 CamTableID, PU32 Range, PU32 PointsCount); // [dan.yang 2011.08.22]
 U32 ADVCMNAPI Acm_DevConfigCAMTable(HAND DeviceHandle,  //Add for pci1265 and pci1245 [dan.yang 2011.06.23]
						  U32 CamTableID,
						  U32 Periodic,
						  U32 MasterAbsolute,
						  U32 SlaveAbsolute);
 U32 ADVCMNAPI Acm_DevReadMailBox(HAND Handle, U16 par_id, U32 data_index, U32 data_count, PU32 DataBuffer); //[dan.yang 2012.12.28]
 U32 ADVCMNAPI Acm_DevReadMultiMailBox(HAND Handle, U8 object_id, PU16 par_id, PU32 DataBuffer, PU32 ErrorBuffer, U32 ArrayElements);
 U32 ADVCMNAPI Acm_DevWriteMailBox(HAND Handle, U16 par_id, U32 data_index, U32 data_count, PU32 DataBuffer); //[dan.yang 2012.12.28]
 U32 ADVCMNAPI Acm_DevWriteMultiMailBox(HAND Handle, U8 object_id, PU16 par_id, PU32 DataBuffer, PU32 ErrorBuffer, U32 ArrayElements);
 U32 ADVCMNAPI Acm_WriteRingBuffer(HAND Handle, U32 cmd_id, U32 data_index, U32 data_cnt, PU32 dataBuffer);
 U32 ADVCMNAPI Acm_ReadRingBuffer(HAND Handle, PU32 cmd_id, PU32 data_index, U32 data_cnt, PU32 dataBuffer);
 U32 ADVCMNAPI Acm_LoadENI(HAND DeviceHandle, PI8 FilePath); //  [11/12/2014 dan]
 U32 ADVCMNAPI Acm_DevGetComStatus(HAND DeviceHandle, U16 RingNo, PU16 pStatus); //  [11/20/2014 dan]
 U32 ADVCMNAPI Acm_DevGetErrorTable(HAND DeviceHandle, U16 RingNo, PU32 ErrorTableArray, PU32 ArrayElements);
 U32 ADVCMNAPI Acm_DevGetMasInfo(HAND DeviceHandle, PVOID pMasInfo, PU16 SlaveIPArray, PU32 SlvCnt);
 U32 ADVCMNAPI Acm_DevGetSlaveInfo(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, PVOID pInfo);
 U32 ADVCMNAPI Acm_DevGetModuleInfo(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, PU32 ModIDArray, PU32 ModCnt);
 U32 ADVCMNAPI Acm_DevGetIOInfo(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 Slot, U8 DataType, PVOID pInfo);
 U32 ADVCMNAPI Acm_DevGetSlaveDataCnt(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U8 DataType, PU32 DataCnt);
U32 ADVCMNAPI Acm_DevGetSlaveFwVersion(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, OUT PI8 VersionInfo); //  [6/4/2015 andy.wang]
 U32 ADVCMNAPI Acm_DevSetSlaveID(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 SlaveNewIP); //  [6/23/2015 andy.wang]
 U32 ADVCMNAPI Acm_CheckVersion(HAND DeviceHandle, U32 VersionID, PU32 Result); //  [11/3/2015 dandan.yang]:just for utility

 //YDD 2015.3.26
  U32 ADVCMNAPI Acm_DevMultiTrigSetPWMTableOnTime (HAND DeviceHandle, PU32 TimeTableArray, U32 ArrayCount); 
U32 ADVCMNAPI Acm_DevMultiTrigSetCmpDO(HAND DeviceHandle,U32 OFForON);
U32 ADVCMNAPI Acm_DevMultiTrigForceCmpOut(HAND DeviceHandle, U32 OFForON);
  U32 ADVCMNAPI Acm_DevMutiTrigSetCmpDO(HAND DeviceHandle,U32 OFForON);
  U32 ADVCMNAPI  Acm_DevMutiTrigForceCmpOut(HAND DeviceHandle, U32 OFForON);
//YDD Add End
 //Master device operation
 U32 ADVCMNAPI Acm_MasStartRing(HAND DeviceHandle, U16 RingNo);
 U32 ADVCMNAPI Acm_MasStopRing(HAND DeviceHandle, U16 RingNo);
 U32 ADVCMNAPI Acm_MasGetComStatus(HAND DeviceHandle, U16 RingNo, PU16 pStatus);
 U32 ADVCMNAPI Acm_MasGetComCyclicTime(HAND DeviceHandle, U16 RingNo, PF64 pTime);
 U32 ADVCMNAPI Acm_MasGetDataCyclicTime(HAND DeviceHandle, U16 RingNo, PF64 DataCyclicTime);
 U32 ADVCMNAPI Acm_MasGetActiveTable(HAND DeviceHandle, U16 RingNo, PU32 ActiveTableArray, PU32 ArrayElements);
 U32 ADVCMNAPI Acm_MasGetErrorTable(HAND DeviceHandle, U16 RingNo, PU32 ErrorTableArray, PU32 ArrayElements);
U32 ADVCMNAPI Acm_MasGetSlaveInfo(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, PU32 pInfo); //  [11/16/2014 dan]
 U32 ADVCMNAPI Acm_MasLogComStatus(HAND DeviceHandle, U16 RingNo);
 U32 ADVCMNAPI Acm_MasTrigOut(HAND DeviceHandle, U16 RingNo);//Engineer function, don't release
 U32 ADVCMNAPI Acm_MasGetRingStatus(HAND DeviceHandle, U16 RingNo, PU16 pStatus);//internal function don't release.

 //  [2/17/2016 dandan.yang]:Add for SRP
U32 ADVCMNAPI Acm_DevDownloadScanData(HAND DeviceHandle, PDEV_PRE_SCAN_DATA pScanDataArray, U32 ArrayLength);
//Axis operation
U32 ADVCMNAPI Acm_AxOpen(HAND DeviceHandle, U16 PhyAxis, PHAND AxisHandle);
U32 ADVCMNAPI Acm_AxOpenbyID(HAND DeviceHandle, U16 SlaveID, U8 SubID, PHAND AxisHandle);//Add for pci1203
U32 ADVCMNAPI Acm_AxClose(PHAND AxisHandle);
U32 ADVCMNAPI Acm_AxSetSvOn(HAND AxisHandle, U32 OnOff);
U32 ADVCMNAPI Acm_AxSetErcOn(HAND AxisHandle, U32 OnOff);
U32 ADVCMNAPI Acm_AxResetAlm(HAND AxisHandle, U32 OnOff);
U32 ADVCMNAPI Acm_AxMoveRel(HAND AxisHandle, F64 Distance);
U32 ADVCMNAPI Acm_AxMoveRel_T(HAND AxisHandle, F64 Distance, F64 Time, F64 Factor); //ydd 16.4.21.For luyuan.
U32 ADVCMNAPI Acm_AxMoveRel_SD(HAND AxisHandle, F64 Distance, F64 SDDistance); //Andy 17.01.17.For Allring.
U32 ADVCMNAPI Acm_AxMoveRel_EC(HAND AxisHandle, F64 Position);//zhaocui add 20131203 for hongjin
U32 ADVCMNAPI Acm_AxMoveAbs(HAND AxisHandle, F64 Position);
U32 ADVCMNAPI Acm_AxMoveAbs_T(HAND AxisHandle, F64 Position, F64 Time, F64 Factor); //ydd 16.4.21.For luyuan.
U32 ADVCMNAPI Acm_AxMoveAbs_SD(HAND AxisHandle, F64 Position, F64 SDPosition); //Andy 17.01.17.For Allring.
U32 ADVCMNAPI Acm_AxMoveAbs_EC(HAND AxisHandle, F64 Position);//  [8/18/2014 deng]
U32 ADVCMNAPI Acm_AxMoveVel(HAND AxisHandle, U16 Direction);
U32 ADVCMNAPI Acm_AxStopDec(HAND AxisHandle);
U32 ADVCMNAPI Acm_AxStopDecEx(HAND AxisHandle, F64 NewDec); //  [12/14/2012 dan.yang]
U32 ADVCMNAPI Acm_AxStopEmg(HAND AxisHandle);
U32 ADVCMNAPI Acm_AxMoveImpose(HAND AxisHandle, F64 Position, F64 NewVel); //Add for pci1265 and pci1245 [dan.yang 2011.04.20]
U32 ADVCMNAPI Acm_AxHomeEx(HAND AxisHandle, U32 DirMode);
U32 ADVCMNAPI Acm_AxHome(HAND AxisHandle, U32 HomeMode, U32 DirMode);
U32 ADVCMNAPI Acm_AxMoveHome(HAND AxisHandle, U32 HomeMode, U32 Dir);
U32 ADVCMNAPI Acm_AxMoveGantryHome(HAND AxisHandle, U32 HomeMode, U32 Dir);
// U32 Acm_AxPauseMotion(U32 AxisHandle);
// U32 Acm_AxResumeMotion(U32 AxisHandle);
U32 ADVCMNAPI Acm_AxChangeVel(HAND AxisHandle, F64 NewVel);
U32 ADVCMNAPI Acm_AxChangePos(HAND AxisHandle, F64 NewPos);
U32 ADVCMNAPI Acm_AxChangeVelByRate(HAND AxisHandle, U32 Rate); //  [11/21/2012 dan.yang]
//  [12/14/2012 dan.yang]
U32 ADVCMNAPI Acm_AxChangeVelEx(HAND AxisHandle,
					  F64  NewVel,
					  F64  NewAcc,
					  F64  NewDec);
U32 ADVCMNAPI Acm_AxChangeVelExByRate(HAND AxisHandle,
							U32  Rate,               //The percentage of velocity to be changed.
							F64  NewAcc,             // New acceleration.
							F64  NewDec);            // New deceleration.
U32 ADVCMNAPI Acm_AxResetError(HAND AxisHandle);
U32 ADVCMNAPI Acm_AxGetState(HAND AxisHandle, PU16 State);
U32 ADVCMNAPI Acm_AxGetMotionIO(HAND AxisHandle, PU32 Status);
U32 ADVCMNAPI Acm_AxGetMotionStatus(HAND AxisHandle, PU32 Status);
U32 ADVCMNAPI Acm_AxGetCmdPosition(HAND AxisHandle, PF64 Position);
U32 ADVCMNAPI Acm_AxGetMachPosition(HAND AxisHandle, PF64 Position);
U32 ADVCMNAPI Acm_AxSetCmdPosition(HAND AxisHandle, F64 Position);
U32 ADVCMNAPI Acm_AxGetActualPosition(HAND AxisHandle, PF64 Position);
U32 ADVCMNAPI Acm_AxSetActualPosition(HAND AxisHandle, F64 Position);
U32 ADVCMNAPI Acm_AxGetCmdVelocity(HAND AxisHandle, PF64 Velocity);
U32 ADVCMNAPI Acm_AxGetActVelocity(HAND AxisHandle, PF64 Velocity);//  [9/28/2015 dandan.yang]
U32 ADVCMNAPI Acm_AxGetLagCounter(HAND AxisHandle, PF64 Position);
U32 ADVCMNAPI Acm_AxSetExtDrive(HAND AxisHandle, U16 ExtDrvMode);

U32 ADVCMNAPI Acm_AxDoSetBit(HAND AxisHandle, U16	DoChannel, U8 BitData);
U32 ADVCMNAPI Acm_AxDiSetBit(HAND AxisHandle, U16	DiChannel, U8 BitData);
U32 ADVCMNAPI Acm_AxDoGetBit(HAND AxisHandle, U16	DoChannel, PU8 BitData);
U32 ADVCMNAPI Acm_AxDiGetBit(HAND AxisHandle, U16	DiChannel, PU8 BitData);
//ydd 2016.2.22
U32 ADVCMNAPI Acm_AxDoSetByte(HAND AxisHandle, U16 DoPort, U8 ByteData);
U32 ADVCMNAPI Acm_AxDoGetByte(HAND AxisHandle, U16 DoPort, PU8 ByteData);
U32 ADVCMNAPI Acm_AxDiGetByte(HAND AxisHandle, U16 DiPort, PU8 ByteData);

U32 ADVCMNAPI Acm_AxSimStartSuspendVel(HAND AxisHandle, U16 DriDir);
U32 ADVCMNAPI Acm_AxSimStartSuspendRel(HAND AxisHandle,F64 Distance);
U32 ADVCMNAPI Acm_AxSimStartSuspendAbs(HAND AxisHandle,F64 EndPoint);
U32 ADVCMNAPI Acm_AxSimStart(HAND AxisHandle);
U32 ADVCMNAPI Acm_AxSimStop(HAND AxisHandle);

U32 ADVCMNAPI Acm_AxGetLatchData(HAND AxisHandle, U32 PositionNo, PF64 Position); //  [5/30/2011 dan.yang]
U32 ADVCMNAPI Acm_AxStartSoftLatch(U32 AxisHandle);
U32 ADVCMNAPI Acm_AxResetLatch(HAND AxisHandle);//Add for pci1245 and pci1265 [dan.yang 2011.05.05]
U32 ADVCMNAPI Acm_AxGetLatchFlag(HAND AxisHandle, PU8 LatchFlag);//Add for pci1245 and pci1265 [dan.yang 2011.05.05]
U32 ADVCMNAPI Acm_AxTriggerLatch(HAND AxisHandle); //Add for pci1245 and pci1265 [dan.yang 2011.05.10]
U32	ADVCMNAPI Acm_AxReadLatchBuffer(HAND AxisHandle, PF64 LatchDataArray, PU32 DataCnt);
U32	ADVCMNAPI Acm_AxResetLatchBuffer(HAND AxisHandle);
U32	ADVCMNAPI Acm_AxGetLatchBufferStatus(HAND AxisHandle, PU32 RemainCnt, PU32 SpaceCnt);

U32 ADVCMNAPI Acm_AxCamInAx (HAND AxisHandle,   //Add for pci1245 and pci1265 [dan.yang 2011.05.10]
				   HAND MasAxisHandle, 
				   F64 MasterOffset,
				   F64 SlaveOffset, 
				   F64 MasterScaling, 
				   F64 SlaveScaling, 
				   U32 CamTableID, 
				   U32 RefSrc);
U32 ADVCMNAPI Acm_AxGearInAx(HAND AxisHandle,  //Add for pci1245 and pci1265 [dan.yang 2011.06.23]
				   HAND MasAxisHandle,
				   I32 Numerator, 
				   I32 Denominator, 
				   U32 RefSrc, 
				   U32 Absolute);
U32	ADVCMNAPI Acm_AxTangentInGp (HAND 	AxisHandle, //Add for pci1245 and pci1265 [dan.yang 2011.06.24]
					   HAND 	MasGroupHandle,
					   PI16		StartVectorArray,  //must three dimension
					   U8		Working_plane,		// 0:  XY plane (default),  1: YZ plane,  2: XZ plane
					   I16		Direction); //0: same with master group. 1:opposite
U32	ADVCMNAPI Acm_AxGantryInAx (HAND 	AxisHandle, //Add for pci1245 and pci1265 [dan.yang 2011.06.24]
					  HAND 	MasAxisHandle,
					  I16		RefMasterSrc,
					  I16		direction); //0: same with master axis. 1:opposite
U32 ADVCMNAPI Acm_AxPhaseAx(HAND AxisHandle, F64 Acc, F64 Dec, F64 PhaseSpeed, F64 PhaseDist);

//  [10/30/2013 dan.yang]: Add compare channel
U32 ADVCMNAPI Acm_AxSetChannelCmpSetting(HAND AxisHandle, U16 ChannelID, U32 CmpSrc, U32 CmpMethod, U32 CmpPulseMode, U32 CmpPulseWidth);
U32 ADVCMNAPI Acm_AxGetChannelCmpSetting(HAND AxisHandle, U16 ChannelID, PU32 CmpSrc, PU32 CmpMethod, PU32 CmpPulseMode, PU32 CmpPulseWidth);
U32 ADVCMNAPI Acm_AxResetChannelCmp (HAND AxisHandle, U16 ChannelID);
U32 ADVCMNAPI Acm_AxAddChannelCmpDatas(HAND AxisHandle, U16 ChannelID, PF64 TableArray, U32 ArrayCount);
U32 ADVCMNAPI Acm_AxGetChannelCmpData(HAND AxisHandle, U16 ChannelID, PF64 CmpData);
U32 ADVCMNAPI Acm_AxLoadChannelNextData(HAND AxisHandle, U16 ChannelID);
U32 ADVCMNAPI Acm_AxGetCmpbufferRemainCount(HAND AxisHandle, U16 ChannelID,PU32 DataCount);

//mining: Compare functions
U32 ADVCMNAPI Acm_AxSetCmpAuto(HAND AxisHandle, F64 Start, F64 End, F64 Interval);
U32 ADVCMNAPI Acm_AxGetCmpData(HAND AxisHandle, PF64 CmpPosition);
U32 ADVCMNAPI Acm_AxSetCmpData(HAND AxisHandle, F64	CmpPosition);
U32 ADVCMNAPI Acm_AxSetCmpTable(HAND AxisHandle, PF64 TableArray, I32	ArrayCount);
U32 ADVCMNAPI Acm_AxChangeCmpIndex(HAND AxisHandle,    //  [9/22/2014 yang.kai]
								   U32 CmpIndex);
//YDD Add for salehi copr. 2015.4.11
U32 ADVCMNAPI Acm_AxResetMPGOffset(HAND AxisHandle);
//YDD End

//2015.8.12
U32 ADVCMNAPI Acm_AxMovePTPBufferRel(
						   HAND AxisHandle, 
						   U16 MotionMode, 
						   PF64 PositionArray,
						   PF64 FLArray,
						   PF64 FHArray, 
						   PU16 TSArray,
						   U32 ArrayLength);
//2015.8.12
U32 ADVCMNAPI Acm_AxMovePTPBufferAbs(
						 HAND AxisHandle, 
						 U16 MotionMode, 
						 PF64 PositionArray,
						 PF64 FLArray,
						 PF64 FHArray, 
						 PU16 TSArray,
						 U32 ArrayLength);

//  [2/17/2016 dandan.yang]:Add fro SRP.
U32 ADVCMNAPI Acm_AxEnableCompensation(HAND AxisHandle,F64 ZStartPos);
U32 ADVCMNAPI Acm_AxGetCompensationValue(HAND AxisHandle, F64 XData, F64 YData, PF64 PCompensationValue);
U32 ADVCMNAPI Acm_AxSetCompenPara(HAND AxisHandle, HAND GroupHandle, U32 XScanDataCnt, U32 YScanDataCnt, U32 CompMode) ;


U32 ADVCMNAPI Acm_AxDIStartMoveAbs(HAND AxisHandle, U16 DIChannel, F64 Position);
U32 ADVCMNAPI Acm_AxDIStartMoveRel(HAND AxisHandle, U16 DIChannel, F64 Distance);
U32 ADVCMNAPI Acm_AxDIStartMoveVel(HAND AxisHandle, U16 DIChannel, U16 Direction);
U32 ADVCMNAPI Acm_AxDisableDIStart(HAND AxisHandle);


U32 ADVCMNAPI Acm_AxSetPWMTableOnTime(HAND AxisHandle, PU32 TimeTableArray, I32 ArrayCount);//zhaocui add


U32 ADVCMNAPI Acm_AxGetINxStopStatus(HAND AxisHandle,PU32 Stop_Flag);//  [2/28/2014 deng]
U32 ADVCMNAPI Acm_AxResetINxStopStatus(HAND AxisHandle);//  [2/28/2014 deng]


U32 ADVCMNAPI Acm_AxJog(HAND AxisHandle,U16 Direction); //[5/22/2014 kai.yang]

U32 ADVCMNAPI Acm_AxSetCmpDO(HAND AxisHandle,U32 OFForON);//  [12/3/2014 deng]
U32 ADVCMNAPI Acm_AxDownloadTorqueTable (HAND AxisHandle, PF64 PositionArray, PF64 TorqueArray, U32 ArrayElements); //[8/6/2015 andy.wang]
U32 ADVCMNAPI Acm_AxLoadTorqueTableFile(HAND AxisHandle, PI8 FilePath, PU32 PointsCount); //[8/6/2015 andy.wang]

//tehsin 2015/11/17
U32 ADVCMNAPI Acm_AxResetPVTTable(HAND AxisHandle);
U32 ADVCMNAPI Acm_AxLoadPVTTable(HAND AxisHandle, PF64 Position, PF64 Velocity, PF64 Time, U32 ArrayElements);
U32 ADVCMNAPI Acm_AxStartPVT(HAND AxisHandle, U8 Repeat);

U32 ADVCMNAPI Acm_AxCheckPTBuffer(HAND AxisHandle, PU16 Freespace);
U32 ADVCMNAPI Acm_AxAddPTData(HAND AxisHandle, F64 Position, F64 Time);
U32 ADVCMNAPI Acm_AxStartPT(HAND AxisHandle, U8 Repeat);
U32 ADVCMNAPI Acm_AxStartAllPT(PHAND AxisHandle, U8 Repeat, U32 ArrayElements);
U32 ADVCMNAPI Acm_AxResetPTData(HAND AxisHandle);
//tehsin 2015/11/17

U32 ADVCMNAPI Acm_AxAddPVAData(HAND AxisHandle, F64 Position, PF64 VelArray, PF64 AccArray, PF64 DecArray, U32 ArrayElements);

//Group operation
U32 ADVCMNAPI Acm_GpOpen(HAND DevHandle,PHAND GpHandle,USHORT GpID);//dan.yang 2015.8.6
U32 ADVCMNAPI Acm_GpAddAxis(PHAND GpHandle,HAND AxHandle);
U32 ADVCMNAPI Acm_GpRemAxis(HAND GroupHandle, HAND AxisHandle);
U32 ADVCMNAPI Acm_GpClose(PHAND GroupHandle);
U32 ADVCMNAPI Acm_GpGetState(HAND GroupHandle, PU16 State);
U32 ADVCMNAPI Acm_GpResetError(HAND GroupHandle);

U32 ADVCMNAPI Acm_GpMoveLinearRel(HAND GroupHandle, PF64 DistanceArray, PU32 ArrayElements);
U32 ADVCMNAPI Acm_GpMoveLinearAbs(HAND GroupHandle, PF64 PositionArray, PU32 ArrayElements);
U32 ADVCMNAPI Acm_GpMoveDirectRel(HAND GroupHandle, PF64 DistanceArray, PU32 ArrayElements);
U32 ADVCMNAPI Acm_GpMoveDirectAbs(HAND GroupHandle, PF64 PositionArray, PU32 ArrayElements);
U32 ADVCMNAPI Acm_GpMoveCircularRel(HAND  GroupHandle, PF64 CenterArray, PF64 EndArray,
                          PU32 ArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_GpMoveCircularAbs(HAND  GroupHandle, PF64 CenterArray, PF64 EndArray,
                          PU32 ArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_GpMoveCircularRel_3P(HAND GroupHandle, 
								 PF64	RefArray,
                                 PF64    EndArray,
                                 PU32	pArrayElements,
                                 I16     Direction );
U32 ADVCMNAPI Acm_GpMoveCircularAbs_3P(HAND GroupHandle, 
								 PF64	RefArray,
                                 PF64    EndArray,
                                 PU32	pArrayElements,
                                 I16     Direction );   
U32 ADVCMNAPI Acm_GpMoveCircularRel_Angle(HAND  GroupHandle,  //  [8/15/2012 dan.yang]
								PF64 CenterArray, 
								U16 Degree,
								PU32 ArrayElements,
								I16  Direction);

U32 ADVCMNAPI Acm_GpMoveCircularAbs_Angle(HAND  GroupHandle,  //  [8/15/2012 dan.yang]
								PF64 CenterArray, 
								U16 Degree,
                                PU32 ArrayElements,
								I16  Direction);
U32 ADVCMNAPI Acm_GpMoveArcRel_Angle(HAND  GroupHandle,  //  [9/22/2014 yang.kai]
										  PF64 CenterArray, 
										  F64 Degree,
										  PU32 ArrayElements,
										  I16  Direction);
U32 ADVCMNAPI Acm_GpMoveArcAbs_Angle(HAND  GroupHandle,  //  [9/22/2014 yang.kai]
										  PF64 CenterArray, 
										  F64 Degree,
										  PU32 ArrayElements,
										  I16  Direction);
U32 ADVCMNAPI Acm_GpMove3DArcAbs( HAND GroupHandle,
					    PF64 CenterArray,
						PF64 EndArray,
						PU32 pArrayElements,
						I16  Direction);//zhaocui add 201308

U32 ADVCMNAPI Acm_GpMove3DArcRel( HAND GroupHandle,
					       PF64 CenterArray,
						   PF64 EndArray,
						   PU32 pArrayElements,
						   I16  Direction);//zhaocui add 201308

U32 ADVCMNAPI Acm_GpMove3DArcAbs_V( HAND GroupHandle,
					    PF64 CenterArray,
						PF64 NVectorArray,
						F64  Degree,
						PU32 pArrayElements,
						I16  Direction);//zhaocui add 201308

U32 ADVCMNAPI Acm_GpMove3DArcRel_V( HAND GroupHandle,
					    PF64 CenterArray,
						PF64 NVectorArray,
						F64  Degree,
						PU32 pArrayElements,
						I16  Direction);//zhaocui add 201308

U32 ADVCMNAPI Acm_GpMove3DArcAbs_3P( HAND GroupHandle,
								 PF64 RefArray,
								 PF64 EndArray,
								 PU32 pArrayElements,
								 I16  Direction,
								 U16  cycCount);//zhaocui add 201501

U32 ADVCMNAPI Acm_GpMove3DArcRel_3P( HAND GroupHandle,
								 PF64 RefArray,
								 PF64 EndArray,
								 PU32 pArrayElements,
								 I16  Direction,
								 U16  cycCount);//zhaocui add 201501
U32 ADVCMNAPI Acm_GpMove3DArcAbs_3PAngle( HAND GroupHandle,//  [2/6/2015 dan]
						PF64 RefPoint_1,
						PF64 RefPoint_2,
						PU32 pArrayElements,
						I16  Direction,
						F64  Degree);
U32 ADVCMNAPI Acm_GpMove3DArcRel_3PAngle( HAND GroupHandle, //  [2/6/2015 dan]
						PF64 RefPoint_1,
						PF64 RefPoint_2,
						PU32 pArrayElements,
						I16  Direction,
						F64  Degree);
U32 ADVCMNAPI Acm_GpMoveHelixAbs(HAND GroupHandle, 
						PF64	CenterArray,
                        PF64    EndArray,
                        PU32	pArrayElements,
                        I16     Direction);   //  [6/21/2011 dan.yang]
U32 ADVCMNAPI Acm_GpMoveHelixRel(HAND GroupHandle, 
						PF64	CenterArray,
                        PF64    EndArray,
                        PU32	pArrayElements,
                        I16     Direction);   //  [6/21/2011 dan.yang]
U32 ADVCMNAPI Acm_GpMoveHelixAbs_3P(HAND GroupHandle, 
								 PF64	RefArray,
                                 PF64    EndArray,
                                 PU32	pArrayElements,
                                 I16     Direction );  //  [6/21/2011 dan.yang]
U32 ADVCMNAPI Acm_GpMoveHelixRel_3P(HAND GroupHandle, 
								 PF64	RefArray,
                                 PF64    EndArray,
                                 PU32	pArrayElements,
                                 I16     Direction );  //  [6/21/2011 dan.yang]
U32 ADVCMNAPI Acm_GpMoveHelixRel_Angle(HAND GroupHandle, 
						PF64	CenterArray,
                        PF64   EndArray,
                        PU32	pArrayElements,
                        I16    Direction);//added by zhaocui 201305
U32 ADVCMNAPI Acm_GpMoveHelixAbs_Angle(HAND GroupHandle, 
						PF64	CenterArray,
                        PF64   EndArray,
                        PU32	pArrayElements,
                        I16    Direction);//added by zhaocui 201305
U32 ADVCMNAPI Acm_GpMoveEllipticalRel(HAND GroupHandle, PF64 CenterArray, PF64 EndArray, PU32 pArrayElements, I16 Direction, F64 RatioSemiAxes);
U32 ADVCMNAPI Acm_GpMoveEllipticalAbs(HAND GroupHandle, PF64 CenterArray, PF64 EndArray, PU32 pArrayElements, I16 Direction, F64 RatioSemiAxes);

U32 ADVCMNAPI Acm_GpLoadPath(HAND GroupHandle, PI8 FilePath, PHAND PathHandle, PU32 pTotalCount);
U32 ADVCMNAPI Acm_GpUnloadPath(HAND GroupHandle, PHAND PathHandle);
U32 ADVCMNAPI Acm_GpMovePath(HAND GroupHandle, HAND PathHandle);
U32 ADVCMNAPI Acm_GpMoveAllPath(PHAND GroupHandle, U32 ArrayElements);
U32 ADVCMNAPI Acm_GpAddPath (HAND GroupHandle,U16 MoveCmd,U16 MoveMode,F64 FH,F64 FL,
				   PF64 EndPoint_DataArray,PF64 CenPoint_DataArray,PU32 ArrayElements);
U32 ADVCMNAPI Acm_GpResetPath (PHAND GroupHandle);
U32 ADVCMNAPI Acm_GpGetPathStatus (HAND GroupHandle, // IN, Group Handle
						 PU32 pCurIndex, // IN, OUT, Current index of path data in path buffer
						 PU32 pCurCmdFunc, // IN, OUT, Return current command function in executing
						 PU32 pRemainCount, // IN, OUT, Number of unexecuted path data in path
						 PU32 pFreeSpaceCount );
U32 ADVCMNAPI Acm_GpMoveSelPath(HAND	GroupHandle, //  [6/22/2011 dan.yang]
					  HAND  PathHandle,
					  U32	StartIndex,				// range: 0~9999
                      U32	EndIndex,				// range: 0~9999
                      U8	Repeat);
U32 ADVCMNAPI Acm_GpGetPathIndexStatus(HAND       	GroupHandle,  //  [6/22/2011 dan.yang]
								U32 		Index, 		// index of path
								PU16		CmdFunc, 
								PU16		MoveMode,
								PF64 	    FH,	
								PF64 	    FL, 
								PF64 	    EndPoint_DataArray, 
								PF64 	    CenPoint_DataArray,
								PU32 	    ArrayElements);
U32 ADVCMNAPI Acm_GpAddBSplinePath(
	HAND GroupHandle, 	//  Handle
	F64 FH, 			// Feed rate
	F64 FL,				// Initial feed rate
	F64 *CtrlP0List,	// The control points of axis 0 (X-Axis)
	F64 *CtrlP1List,	// The control points of axis 1 (Y-Axis)
	U32 CtrlPCount,		// The number of control points
	F64 *NodeList,		// *B-Spine knot list
	U32 NodeCount,		// *The number of knot
	U32 Degree,			// B-Spline degree
	U32 CutPointCount	// The number of line segments used in interpolation
	);

U32 Acm_GpAddCSplinePath(
	HAND GroupHandle, 		// Group Handle
	F64 FH, 				// Feed rate
	F64 FL,					// Initial feed rate
	F64 *CtrlP0List,		// The control points of axis 0 (X-Axis)
	F64 *CtrlP1List,		// The control points of axis 1 (Y-Axis)
	F64 *Tightness,			// 
	U32 CtrlPCount,			// The number of control points
	U32 CutPointCount		// The number of line segments used in interpolation
	);
U32 ADVCMNAPI Acm_GpResumeMotion(HAND GroupHandle); //  [10/12/2012 dan.yang]
U32 ADVCMNAPI Acm_GpPauseMotion(HAND GroupHandle); //  [10/12/2012 dan.yang]

U32 ADVCMNAPI Acm_GpStopDec(HAND GroupHandle);
U32 ADVCMNAPI Acm_GpStopDecEx (HAND GroupHandle, F64 NewDec);
U32 ADVCMNAPI Acm_GpStopEmg(HAND GroupHandle);
U32 ADVCMNAPI Acm_GpChangeVel(HAND GroupHandle, F64 NewVelocity);
U32 ADVCMNAPI Acm_GpChangeVelByRate(HAND GroupHandle, U32 Rate);//  [11/21/2012 dan.yang]
U32 ADVCMNAPI Acm_GpGetCmdVel(HAND GroupHandle, PF64 CmdVel); //Add for PCI1245 and PCI1265.[Dan.Yang 20110607]

U32 ADVCMNAPI Acm_GpGetINxStopStatus(HAND GroupHandle,PU32 Stop_Flag);//  [2/28/2014 deng]
U32 ADVCMNAPI Acm_GpResetINxStopStatus(HAND GroupHandle);//  [2/28/2014 deng]
// DIO
U32 ADVCMNAPI Acm_DaqDiGetByte(HAND DeviceHandle, U16 DiPort,PU8 ByteData);
U32 ADVCMNAPI Acm_DaqDiGetBit(HAND DeviceHandle, U16 DiChannel, PU8	BitData);
U32 ADVCMNAPI Acm_DaqDoSetByte(HAND DeviceHandle, U16 DoPort, U8	ByteData);
U32 ADVCMNAPI Acm_DaqDoSetBit(HAND	DeviceHandle, U16	DoChannel, U8 BitData);
U32 ADVCMNAPI Acm_DaqDiSetBit(HAND	DeviceHandle, U16	DiChannel, U8 BitData);
U32 ADVCMNAPI Acm_DaqDoGetByte(HAND DeviceHandle, U16 DoPort, PU8 ByteData);
U32 ADVCMNAPI Acm_DaqDoGetBit(HAND	DeviceHandle, U16	DoChannel, PU8	BitData);
U32 ADVCMNAPI Acm_DaqDiGetBytes(HAND DeviceHandle, U16 StartPort, U16 NumPort, PU8 ByteDataArray);
U32 ADVCMNAPI Acm_DaqDoSetBytes(HAND DeviceHandle, U16 StartPort, U16 NumPort, PU8 ByteDataArray);
U32 ADVCMNAPI Acm_DaqDoGetBytes(HAND DeviceHandle, U16 StartPort, U16 NumPort, PU8 ByteDataArray);

U32 ADVCMNAPI Acm_DaqDiGetByteEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 DiPort, PU8 ByteData);
U32 ADVCMNAPI Acm_DaqDiGetBitEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 DiChannel, PU8	BitData);
U32 ADVCMNAPI Acm_DaqDoSetByteEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 DoPort, U8 ByteData);
U32 ADVCMNAPI Acm_DaqDoSetBitEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16	DoChannel, U8 BitData);
U32 ADVCMNAPI Acm_DaqDoGetByteEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 DoPort, PU8 ByteData);
U32 ADVCMNAPI Acm_DaqDoGetBitEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 DoChannel, PU8 BitData);

//AIO [Dan.Yang 2011.07.19]
U32 ADVCMNAPI Acm_DaqAiGetRawData(HAND DeviceHandle, U16 AiChannel, PU16 AiData);
U32 ADVCMNAPI Acm_DaqAiGetVoltData(HAND DeviceHandle, U16	AiChannel, PF32	AiData);
U32 ADVCMNAPI Acm_DaqAiGetCurrData(HAND DeviceHandle, U16	AiChannel, PF32	AiData);
U32 ADVCMNAPI Acm_DaqAiZeroCalibration(HAND DeviceHandle, U16	AiChannel);
U32 ADVCMNAPI Acm_DaqAiSpanCalibration(HAND DeviceHandle, U16	AiChannel);
U32 ADVCMNAPI Acm_DaqAiGetChannelStatus(HAND DeviceHandle, U16	AiChannel, PU32 ChanStatus);
U32 ADVCMNAPI Acm_DaqAoSetRawData(HAND DeviceHandle, U16	AoChannel, U16 AoData);
U32 ADVCMNAPI Acm_DaqAoSetVoltData(HAND DeviceHandle, U16	AoChannel, F32 AoData);
U32 ADVCMNAPI Acm_DaqAoSetCurrData(HAND DeviceHandle, U16	AoChannel, F32 AoData);
U32 ADVCMNAPI Acm_DaqAoGetRawData(HAND DeviceHandle, U16	AoChannel, PU16 AoData);
U32 ADVCMNAPI Acm_DaqAoGetVoltData(HAND DeviceHandle, U16	AoChannel, PF32 AoData);
U32 ADVCMNAPI Acm_DaqAoGetCurrData(HAND DeviceHandle, U16	AoChannel, PF32 AoData);
U32 ADVCMNAPI Acm_DaqAoSetCaliType(HAND DeviceHandle, U16	AoChannel, U16 TrimType);
U32 ADVCMNAPI Acm_DaqAoSetCaliValue(HAND DeviceHandle, U16	AoChannel, U16 CaliData);
U32 ADVCMNAPI Acm_DaqAoCaliDone(HAND DeviceHandle, U16	AoChannel, bool done);
U32 ADVCMNAPI Acm_DaqAoCaliDefault(HAND DeviceHandle, U16	AoChannel);

U32 ADVCMNAPI Acm_DaqAiGetRawDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AiChannel, PU16 AiData);
U32 ADVCMNAPI Acm_DaqAiGetVoltDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AiChannel, PF32 AiData);
U32 ADVCMNAPI Acm_DaqAiGetCurrDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AiChannel, PF32 AiData);
U32 ADVCMNAPI Acm_DaqAiGetChannelStatusEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AiChannel, PU32 ChanStatus);
U32 ADVCMNAPI Acm_DaqAoSetRawDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16	AoChannel, U16 AoData);
U32 ADVCMNAPI Acm_DaqAoSetVoltDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AoChannel, F32 AoData);
U32 ADVCMNAPI Acm_DaqAoSetCurrDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AoChannel, F32 AoData);
U32 ADVCMNAPI Acm_DaqAoGetRawDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16	AoChannel, PU16 AoData);
U32 ADVCMNAPI Acm_DaqAoGetVoltDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AoChannel, PF32 AoData);
U32 ADVCMNAPI Acm_DaqAoGetCurrDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 AoChannel, PF32 AoData);

U32 ADVCMNAPI Acm_DaqGetIOLinkStatus(HAND DeviceHandle, PU32 pStatus); //ydd 16.2.4

//Counter [Andy.Wang 2015.04.21]
U32 ADVCMNAPI Acm_DaqCntTriggerCmp(HAND DeviceHandle, U16 CntChannel);
U32 ADVCMNAPI Acm_DaqCntTriggerLatch(HAND DeviceHandle, U16	CntChannel);
U32 ADVCMNAPI Acm_DaqCntResetLatch(HAND DeviceHandle, U16 CntChannel);
U32 ADVCMNAPI Acm_DaqCntResetCmp(HAND DeviceHandle, U16	CntChannel);
U32 ADVCMNAPI Acm_DaqCntResetCnt(HAND DeviceHandle, U16	CntChannel);
U32 ADVCMNAPI Acm_DaqCntGetCounterData(HAND DeviceHandle, U16 CntChannel, PF64 CounterData);
U32 ADVCMNAPI Acm_DaqCntSetCounterData(HAND DeviceHandle, U16 CntChannel, F64 CounterData);
U32 ADVCMNAPI Acm_DaqCntGetExtDriveData(HAND DeviceHandle, U16 CntChannel, PF64 CounterData);
U32 ADVCMNAPI Acm_DaqCntSetExtDriveData(HAND DeviceHandle, U16 CntChannel, F64 CounterData);
U32 ADVCMNAPI Acm_DaqCntGetLatchData(HAND DeviceHandle, U16	CntChannel, PF64 LatchData);
U32 ADVCMNAPI Acm_DaqCntGetCmpData(HAND DeviceHandle, U16 CntChannel, PF64 CmpData);
U32 ADVCMNAPI Acm_DaqCntSetCmpData(HAND DeviceHandle, U16 CntChannel, F64 CmpData);
U32 ADVCMNAPI Acm_DaqCntSetCmpTable(HAND DeviceHandle, U16 CntChannel, PF64 TableArray, I32 ArrayCount);
U32 ADVCMNAPI Acm_DaqCntSetCmpAuto(HAND DeviceHandle, U16 CntChannel, F64 Start, F64 End, F64 Interval);
U32	ADVCMNAPI Acm_DaqCntGetLatchBufferStatus(HAND DeviceHandle, U16 CntChannel, PU32 RemainCnt, PU32 SpaceCnt);
U32	ADVCMNAPI Acm_DaqCntReadLatchBuffer(HAND DeviceHandle, U16 CntChannel, PF64 LatchDataArray, PU32 DataCnt);

U32 ADVCMNAPI Acm_DaqCntTriggerCmpEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel);
U32 ADVCMNAPI Acm_DaqCntTriggerLatchEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel);
U32 ADVCMNAPI Acm_DaqCntResetLatchEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel);
U32 ADVCMNAPI Acm_DaqCntResetCmpEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel);
U32 ADVCMNAPI Acm_DaqCntResetCntEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel);
U32 ADVCMNAPI Acm_DaqCntGetCounterDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, PF64 CounterData);
U32 ADVCMNAPI Acm_DaqCntSetCounterDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, F64 CounterData);
U32 ADVCMNAPI Acm_DaqCntGetExtDriveDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, PF64 CounterData);
U32 ADVCMNAPI Acm_DaqCntSetExtDriveDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, F64 CounterData);
U32 ADVCMNAPI Acm_DaqCntGetLatchDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, PF64 LatchData);
U32 ADVCMNAPI Acm_DaqCntGetCmpDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, PF64 CmpData);
U32 ADVCMNAPI Acm_DaqCntSetCmpDataEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, F64 CmpData);
U32 ADVCMNAPI Acm_DaqCntSetCmpTableEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, PF64 TableArray, I32 ArrayCount);
U32 ADVCMNAPI Acm_DaqCntSetCmpAutoEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, F64 Start, F64 End, F64 Interval);
U32	ADVCMNAPI Acm_DaqCntGetLatchBufferStatusEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, PU32 RemainCnt, PU32 SpaceCnt);
U32	ADVCMNAPI Acm_DaqCntReadLatchBufferEx(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 CntChannel, PF64 LatchDataArray, PU32 DataCnt);

//Dan.Yang 2011.07.12 :Add for download Dsp Firmware program.
U32 ADVCMNAPI Acm_GetDSPFrmWareDwnLoadRate(HAND DeviceHandle, PF64 Percentage);
U32 ADVCMNAPI Acm_DevDownLoadDSPFrmWare_STP1(HAND DeviceHandle);
U32 ADVCMNAPI Acm_DevDownLoadDSPFrmWare_STP2(HAND DeviceHandle, PI8 FilePath);
U32 ADVCMNAPI Acm_DevDownLoadDSPFrmWare_STP3(HAND DeviceHandle);

U32 ADVCMNAPI Acm_DownLoadMCU_ST2(HAND DeviceHandle, U16 CPLDNo, PI8 FilePath);
U32 ADVCMNAPI Acm_DownLoadMCU_ST1(HAND DeviceHandle, U16 CPLDNo);
U32 ADVCMNAPI Acm_GetMCUDownLoadRate(HAND DeviceHandle, PF64 Percentage);

U32 ADVCMNAPI Acm_AxPWMOut(HAND AxisHandle,U32 OFForON,U32 PulseCount);
U32 ADVCMNAPI Acm_AxGetPWMOutState(HAND AxisHandle,PU32 OFForON);

U32 ADVCMNAPI Acm_DownLoadCPLD_ST2(HAND DeviceHandle, U16 CPLDNo, PI8 FilePath);
U32 ADVCMNAPI Acm_DownLoadCPLD_ST1(HAND DeviceHandle, U16 CPLDNo);
U32 ADVCMNAPI Acm_GetCPLDDownLoadRate(HAND DeviceHandle, PF64 Percentage);
U32 ADVCMNAPI Acm_DevMDaqConfig(HAND DeviceHandle, U16 ChannelID, U32 Period, U32 AxisNo, U32 Method, U32 ChanType, U32 Count);
U32 ADVCMNAPI Acm_DevMDaqStart(HAND DeviceHandle);
U32 ADVCMNAPI Acm_DevMDaqStop(HAND DeviceHandle);
U32 ADVCMNAPI Acm_DevMDaqReset(HAND DeviceHandle, U16 ChannelID);
U32 ADVCMNAPI Acm_DevMDaqGetStatus(HAND DeviceHandle, U16 ChannelID, PU16 CurrentCnt, PU8 Status);
U32 ADVCMNAPI Acm_DevMDaqGetData(HAND DeviceHandle, U16 ChannelID, U16 StartIndex, U16 MaxCount, PI32 DataBuffer);
U32 ADVCMNAPI Acm_DevMDaqGetConfig(HAND DeviceHandle, U16 ChannelID, PU32 Period, PU32 AxisNo, PU32 Method, PU32 ChanType, PU32 Count);

//YDD 2015.4.22
U32 ADVCMNAPI Acm_RegCallBackFunc(HAND Handle, ADV_USER_CALLBACK_FUNC CallBackFun, PVOID UserParamter);
U32 ADVCMNAPI Acm_EnableEventCallBack(HAND DeviceHandle);
U32 ADVCMNAPI Acm_RegCallBackFuncForOneEvent(HAND Handle, U32 EvtChannel, ADV_USER_CALLBACK_FUNC CallBackFun, PVOID UserParamter);
U32 ADVCMNAPI Acm_DevEnableMotionEvent(HAND  DeviceHandle,						
										PU32 AxEnableEvtArray,
										PU32 GpEnableEvtArray, 
										U32 AxArrayElements,
										U32 GpArrayElements) ;
//******************SCARA Robot*************************************************/
U32 Acm_GpRbSetMode(HAND GroupHandle, U16 Mode, PI32 CurActPosiArray, U32 ArrayElement);
U32 Acm_GpRbGetCmdPosition(HAND GroupHandle, PF64 PositionArray, U32 ArrayElement);
U32 Acm_GpRbGetActualPosition(HAND GroupHandle, PF64 PositionArray, U32 ArrayElement);
U32 Acm_GpRbGetArmCmdPosition(HAND GroupHandle, U16 ArmID, PF64 CurPosi);
U32 Acm_GpRbGetArmActualPosition(HAND GroupHandle, U16 ArmID, PF64 CurPosi);
//******************************************************************************/

//add by dujunling on 2012.11.22,for GM code function ,
U32 ADVCMNAPI Acm_GmOpen( IN HAND DeviceHandle, OUT PHAND GMSHandle );
U32 ADVCMNAPI Acm_GmClose(IN HAND GMSHandle);

U32 ADVCMNAPI Acm_GmLoadJob(IN HAND GMSHandle, IN PI8 JobFilePath, OUT PU32 ErrorRow);
U32 ADVCMNAPI Acm_GmUploadJob( IN HAND GMSHandle, IN OUT PI8 GCodeBuffer, IN OUT PU32 GCodeLength);
U32 ADVCMNAPI Acm_GmResetJob( IN HAND GMSHandle);

U32 ADVCMNAPI Acm_GmCommand(IN HAND GMSHandle, IN PI8 InputCmd);

U32 ADVCMNAPI Acm_GmRemoveAxisFromSystem(IN HAND GMSHandle, IN U32 uAxisId);

U32 ADVCMNAPI Acm_GmSetCompensationRadius(IN HAND GMSHandle, IN U32 DValue,IN F64 Radius);
U32 ADVCMNAPI Acm_GmGetCompensationRadius(IN HAND GMSHandle,  OUT PU32 pCRArray,IN OUT PU32 pCRArrayLength);
U32 ADVCMNAPI Acm_GmSetCurrentCompensationRadius(IN HAND GMSHandle, IN F64 Radius);
U32 ADVCMNAPI Acm_GmGetCurrentCompensationRadius(IN HAND GMSHandle,  IN OUT PF64 pCRadius);
U32 ADVCMNAPI Acm_GmSetToolLengthOffset(IN HAND GMSHandle, IN U32 HValue,IN F64 ToolLengthOffset);
U32 ADVCMNAPI Acm_GmGetToolLengthOffset(IN HAND GMSHandle, OUT PU32 pTLOArray,IN OUT PU32 pTLOArrayLength);
U32 ADVCMNAPI Acm_GmSetCurrentToolLengthOffset(IN HAND GMSHandle,IN F64 ToolLengthOffset);
U32 ADVCMNAPI Acm_GmGetCurrentToolLengthOffset(IN HAND GMSHandle, IN OUT PF64 pToolLengthOffset);

U32 ADVCMNAPI Acm_GmSetMacro(IN HAND GMSHandle, IN U32 uMacroCmd,IN const PI8 MacroCmdContent);
U32 ADVCMNAPI Acm_GmGetMacro(IN HAND GMSHandle, IN U32 uMacroCmd,OUT PI8 pMacroCmdContent,IN OUT PU32 pMacroCmdContentLength);
U32 ADVCMNAPI Acm_GmGetMacroArray(IN HAND GMSHandle,OUT PU32 pMacroArray,IN OUT PU32 pMacroArrayLength);

U32 ADVCMNAPI Acm_GmSetPreCustomFunction(IN HAND GMSHandle, IN U32 (*ptrPreCustomFunction)(const U32 CustomMCommand,U32 TWordValue,bool &bDone));
U32 ADVCMNAPI Acm_GmSetFinalCustomFunction(IN HAND GMSHandle, IN U32 (*ptrFinalCustomFunction)(const U32 CustomMCommand,U32 TWordValue,bool &bDone));

U32 ADVCMNAPI Acm_GmSetExtDoFunction(IN HAND GMSHandle, IN U32 (*ptrExtDoFunction)(U32 uDoCh,U32 uDoLevel));
U32 ADVCMNAPI Acm_GmSetExtDiFunction(IN HAND GMSHandle, IN U32 (*ptrExtDiFunction)(U32 uDiCh));

U32 ADVCMNAPI Acm_GmSetBreakPoint(IN HAND GMSHandle, IN U32 uBreakPoint);
U32 ADVCMNAPI Acm_GmAddBreakPoint(IN HAND GMSHandle, IN U32 uBreakPoint);
U32 ADVCMNAPI Acm_GmRemoveBreakPoint(IN HAND GMSHandle, IN U32 uBreakPoint);
U32 ADVCMNAPI Acm_GmClearBreakPoint(IN HAND GMSHandle);

U32 ADVCMNAPI Acm_GmSetSpindleRatio(IN HAND GMSHandle, IN F64 fSpindleRatio);
U32 ADVCMNAPI Acm_GmSetJogRatio(IN HAND GMSHandle, IN F64 fJogRatio);
U32 ADVCMNAPI Acm_GmSetFastFeedRateRatio(IN HAND GMSHandle, IN U32 uAxis,IN F64 fFastFeedRateRatio);
U32 ADVCMNAPI Acm_GmGetFastFeedRateRatio(IN HAND GMSHandle, IN U32 uAxis,OUT PF64 pFastFeedRateRatio);
U32 ADVCMNAPI Acm_GmSetFeedRateRatio(IN HAND GMSHandle, IN F64 fFeedRateRatio);
U32 ADVCMNAPI Acm_GmGetFeedRateRatio(IN HAND GMSHandle, OUT PF64 pFeedRateRatio);
U32 ADVCMNAPI Acm_GmSetFeedRate(IN HAND GMSHandle, IN F64 fFeedRate);
U32 ADVCMNAPI Acm_GmGetFeedRate(IN HAND GMSHandle, IN PF64 pFeedRate);
U32 ADVCMNAPI Acm_GmSetFastFeedRate(IN HAND GMSHandle, IN F64 fFastFeedRate);
U32 ADVCMNAPI Acm_GmGetFastFeedRate(IN HAND GMSHandle, OUT PF64 pFastFeedRate);
U32 ADVCMNAPI Acm_GmSetOptionPause(IN HAND GMSHandle, IN U32 uOptionPause);
U32 ADVCMNAPI Acm_GmGetOptionPause(IN HAND GMSHandle, OUT PU32 pOptionPause);
U32 ADVCMNAPI Acm_GmSetOptionSkip(IN HAND GMSHandle, IN U32 uOptionSkip);
U32 ADVCMNAPI Acm_GmGetOptionSkip(IN HAND GMSHandle, OUT PU32 pOptionSkip);
U32 ADVCMNAPI Acm_GmSetModuleRange(IN HAND GMSHandle, IN U32 ModuleRange);
U32 ADVCMNAPI Acm_GmGetModuleRange(IN HAND GMSHandle, IN PU32 pModuleRange);

U32 ADVCMNAPI Acm_GmGetCurrentFeedRate(IN HAND GMSHandle, OUT PF64 pCurrentFeedRate);
U32 ADVCMNAPI Acm_GmGetCurrentRow(IN HAND GMSHandle, OUT PU32 CurrentRow);
U32 ADVCMNAPI Acm_GmGetErrorRow(IN HAND GMSHandle, OUT PU32 ErrorRow);
U32 ADVCMNAPI Acm_GmGetCurrentCommand(IN HAND GMSHandle, OUT PI8 CurrentCmd, IN OUT PU32 CurrentCmdLength);
U32 ADVCMNAPI Acm_GmGetState(IN HAND GMSHandle, OUT PU32 GmsState);
U32 ADVCMNAPI Acm_GmGetPattern(IN HAND GMSHandle, IN U32 GmsPatternId, PVOID Buffer, PU32 BufferLength);
U32 ADVCMNAPI Acm_GmGetCurrentCoordinateSystem(IN HAND GMSHandle, OUT PU32 pCurrentCoordinateIndex,OUT PF64 pCoordinateArray,IN OUT PU32 pArrayLength);
U32 ADVCMNAPI Acm_GmGetError(IN HAND GMSHandle, OUT PU32 ErrorCode, OUT PU32 LastError);
U32 ADVCMNAPI Acm_GmDxfToGCodeTextEx(IN PI8 DxfFilePath, IN F64 DxfProportion, IN U32 Ellipse_Cut_Min_Angel , IN U32 Spline_Cut_Step ,OUT PI8 GCodeBuffer, IN OUT PU32 Length);
U32 ADVCMNAPI Acm_GmDxfToGCodeText(IN PI8 DxfFilePath, IN F64 DxfProportion, OUT PI8 GCodeBuffer, IN OUT PU32  Length);
U32 ADVCMNAPI Acm_GmDxfToGCodeFileEx(IN PI8 DxfFilePath, IN F64 DxfProportion,IN U32 Ellipse_Cut_Min_Angel , IN U32 Spline_Cut_Step ,IN PI8 GcodeFilePath,OUT PU32 Length);
U32 ADVCMNAPI Acm_GmDxfToGCodeFile(IN PI8 DxfFilePath, IN F64 DxfProportion,IN PI8 GcodeFilePath,PU32 Length);

//Added by W.Y.Z on 2014.08.28 for P point function
U32 ADVCMNAPI Acm_GmSetPoint(IN HAND GMSHandle, IN U32 id, IN PF64 fPoint);
U32 ADVCMNAPI Acm_GmGetPoint(IN HAND GMSHandle, IN U32 id,OUT PF64 fPoint);
U32 ADVCMNAPI Acm_GmLoadPoint(IN HAND GMSHandle, IN PI8 FilePath);
U32 ADVCMNAPI Acm_GmSavePoint(IN HAND GMSHandle, IN PI8 FilePath);
U32 ADVCMNAPI Acm_GmResetPoint(IN HAND GMSHandle);

U32 ADVCMNAPI Acm_GmSetPointF(IN HAND GMSHandle, IN U32 id, IN PF64 fPoint,IN U32 filterFlag);
U32 ADVCMNAPI Acm_GmGetPointF(IN HAND GMSHandle, IN U32 id,OUT PF64 fPoint,OUT PU32 filterflag);
U32 ADVCMNAPI Acm_GmLoadPointF(IN HAND GMSHandle, IN PI8 FilePath);
U32 ADVCMNAPI Acm_GmSavePointF(IN HAND GMSHandle, IN PI8 FilePath);
U32 ADVCMNAPI Acm_GmResetPointF(IN HAND GMSHandle);


//zhao cui add for robot 201307
U32 ADVCMNAPI Acm_RbGetActualPosition(HAND RbHandle, PF64 PositionArray, U32 ArrayElement);
U32 ADVCMNAPI Acm_RbGetCmdPosition(HAND RbHandle, PF64 PositionArray, U32 ArrayElement);
U32 ADVCMNAPI Acm_RbGetArmActualPosition(HAND RbHandle, U16 ArmID, PF64 CurPosi);
U32 ADVCMNAPI Acm_RbGetArmCmdPosition(HAND RbHandle, U16 ArmID, PF64 CurPosi);
U32 ADVCMNAPI Acm_RbOpen (HAND DeviceHandle,PHAND RbHandle);
U32 ADVCMNAPI Acm_RbInitial(HAND RbHandle, PF64 A, PF64 D, PF64 alpha, PF64 theta, PI32 joint_type, U32 ArrayElements);
U32 ADVCMNAPI Acm_RbClose (PHAND pRbHandle);
U32 ADVCMNAPI Acm_RbResetError (HAND RbHandle);
U32 ADVCMNAPI Acm_RbGetState (HAND RbHandle, PU16 PStates);
U32 ADVCMNAPI Acm_RbSetActPosition (HAND RbHandle, PF64 PositionArray, U32 ArrayElement);
U32 ADVCMNAPI Acm_RbMoveRel(HAND RbHandle, PF64 PosArray, PU32 ArrayElement);
U32 ADVCMNAPI Acm_RbMoveAbs(HAND RbHandle, PF64 PosArray, PU32 ArrayElement);
U32 ADVCMNAPI Acm_RbMoveDirectRel (HAND RbHandle, PF64 PositionArray, PU32 ArrayElements);
U32 ADVCMNAPI Acm_RbMoveDirectAbs (HAND RbHandle, PF64 PositionArray, PU32 ArrayElements);
U32 ADVCMNAPI Acm_RbMoveLinearRel( HAND RbHandle,PF64 PositionArray,PU32 pArrayElements);
U32 ADVCMNAPI Acm_RbMoveLinearAbs( HAND RbHandle,PF64 PositionArray,PU32 pArrayElements);
U32 ADVCMNAPI Acm_RbMoveArcRel( HAND RbHandle, PF64 CenterArray, PF64 EndArray, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMoveArcAbs( HAND RbHandle, PF64 CenterArray, PF64 EndArray, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMoveArcRel_3P ( HAND RbHandle, PF64 RefArray, PF64 EndArray, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMoveArcAbs_3P ( HAND RbHandle, PF64 RefArray, PF64 EndArray, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMoveArcRel_Angle(HAND RbHandle,	PF64 CenterArray, F64 Degree, PU32 ArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMoveArcAbs_Angle(HAND RbHandle,	PF64 CenterArray, F64 Degree, PU32 ArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMove3DArcAbs( HAND RbHandle, PF64 CenterArray, PF64 EndArray, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMove3DArcRel( HAND RbHandle, PF64 CenterArray, PF64 EndArray, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMove3DArcAbs_V( HAND RbHandle, PF64 CenterArray, PF64 NVectorArray, PF64 EndArray, F64 Degree, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMove3DArcRel_V( HAND RbHandle, PF64 CenterArray, PF64 NVectorArray, PF64 EndArray, F64 Degree, PU32 pArrayElements, I16 Direction);
U32 ADVCMNAPI Acm_RbMove3DArcAbs_3P( HAND RbHandle, PF64 RefArray, PF64 EndArray, PU32 pArrayElements, I16 Direction, U16 cycCount);
U32 ADVCMNAPI Acm_RbMove3DArcRel_3P( HAND RbHandle, PF64 RefArray, PF64 EndArray, PU32 pArrayElements, I16 Direction, U16 cycCount);
U32 ADVCMNAPI Acm_RbMove3DArcAbs_3PAngle( HAND RbHandle, PF64 RefPoint_1, PF64 RefPoint_2, PU32 pArrayElements, I16 Direction, F64 Degree);
U32 ADVCMNAPI Acm_RbMove3DArcRel_3PAngle( HAND RbHandle, PF64 RefPoint_1, PF64 RefPoint_2, PU32 pArrayElements, I16 Direction, F64 Degree);
U32 ADVCMNAPI Acm_RbAddPath (HAND RbHandle,U16 MoveCmd,U16 MoveMode,F64 FH,F64 FL,
				   PF64 EndPoint_DataArray,PF64 CenPoint_DataArray,PU32 ArrayElements);
U32 ADVCMNAPI Acm_RbResetPath (PHAND RbHandle);
U32 ADVCMNAPI Acm_RbGetPathStatus (HAND RbHandle, // IN, Group Handle
						 PU32 pCurIndex, // IN, OUT, Current index of path data in path buffer
						 PU32 pCurCmdFunc, // IN, OUT, Return current command function in executing
						 PU32 pRemainCount, // IN, OUT, Number of unexecuted path data in path
						 PU32 pFreeSpaceCount );
U32 ADVCMNAPI Acm_RbMovePath(HAND RbHandle, HAND PathHandle);
U32 ADVCMNAPI Acm_RbChangeVel(HAND RbHandle, F64 NewVelocity);
U32 ADVCMNAPI Acm_RbChangeVelByRate(HAND RbHandle, U32 Rate);
U32 ADVCMNAPI Acm_RbGetCmdVel(HAND RbHandle, PF64 CmdVel);
U32 ADVCMNAPI Acm_RbStopDec ( HAND RbHandle);
U32 ADVCMNAPI Acm_RbStopEmg(HAND RbHandle);
U32 ADVCMNAPI Acm_RbPauseMotion(HAND RbHandle);
U32 ADVCMNAPI Acm_RbResumeMotion(HAND RbHandle);
bool IsGMSHandleValid(HAND GMSHandle);
U32 ADVCMNAPI Acm_RbLoadPath(HAND RbHandle, PI8 FilePath, PHAND PathHandle, PU32 pTotalCount);//  [6/26/2014 deng]
U32 ADVCMNAPI Acm_RbUnloadPath(HAND RbHandle, PHAND PathHandle);//  [6/26/2014 deng]
U32 ADVCMNAPI Acm_RbMoveSelPath(HAND	RbHandle,
					  HAND  PathHandle,
					  U32	StartIndex,				// range: 0~9999
					  U32	EndIndex,				// range: 0~9999
					  U8	Repeat);//  [6/26/2014 deng]
U32 ADVCMNAPI Acm_RbGetPathIndexStatus(HAND       	RbHandle, 
							 U32 		Index, 				// index of path
							 PU16		CmdFunc, 
							 PU16		MoveMode,
							 PF64 	    FH,	
							 PF64 	    FL, 
							 PF64 	    EndPoint_DataArray, 
							 PF64 	    CenPoint_DataArray,
							 PU32 	    ArrayElements);//  [6/26/2014 deng]
U32 ADVCMNAPI Acm_RbSetExtDrive(HAND RbHandle, U16 ExtDrvMode);//  [7/3/2014 deng]
U32 ADVCMNAPI Acm_RbJog(HAND RbHandle,U16 Direction);//  [7/3/2014 deng]

U32 ADVCMNAPI Acm_ServoSetCom(U32 ComPortID, U32 Baudrate, U32 Timeout);//  [7/17/2014 yang.kai]
U32 ADVCMNAPI Acm_ServoGetAbsPosition(
							U32 ComPortID,  
							U32 ServoType,
							U32 ServoID, 
							U32 ServoAbsResolution,  
							U32 ServoCmdResolution,
							U32 EncoderDir,
							PF64 AbsPosition
							);                                           //  [7/17/2014 yang.kai]
U32 ADVCMNAPI Acm_AxSetCmdPosi_Pulse(HAND AxisHandle,F64 Position);      //  [7/17/2014 yang.kai]
U32 ADVCMNAPI Acm_AxSpecialDiSetBit(HAND AxisHandle,U16 DiType,U8 BitData);

U32 ADVCMNAPI Acm_DevEnableLTC(HAND DeviceHandle, U16 LtcID, U16 EnableMode);// [8/27/2014 deng]
U32 ADVCMNAPI Acm_DevLTCSaftyDist(HAND DeviceHandle, U16 LtcID, F64 SaftyDist);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevEnableCmp(HAND DeviceHandle, U16 CmpID, U16 EnableMode);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevLtcLinkCmp(HAND DeviceHandle, HAND AxisHandle, U16 EnableLink, U16 LtcID, U16 CmpID, F64 Offset);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevSetCmp(HAND DeviceHandle, U16 CmpID, U32 CmpLogic, U32 CmpSrc, U32 CmpMethod, U32 DOMode, U32 DOWidth);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevSetCmpDO(HAND DeviceHandle, U16 CmpID, U16 OnOff);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevSetCmpData(HAND DeviceHandle, U16 CmpID, F64 Data);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevSetCmpAuto(HAND DeviceHandle, U16 CmpID, F64 Start, F64 End, F64 Interval);
U32 ADVCMNAPI Acm_DevGetCmpData(HAND DeviceHandle, U16 CmpID, PF64 Data);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevEnableCmpFIFO(HAND DeviceHandle, U16 CmpID, U16 Enable);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevGetCmpFIFOCount(HAND DeviceHandle, U16 CmpID, PU16 DataCount);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevGetCmpCounter(HAND DeviceHandle, U16 CmpID, PU32 DataCount);
U32 ADVCMNAPI Acm_DevResetCmpFIFO(HAND DeviceHandle, U16 CmpID);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevSetLTCInPol(HAND DeviceHandle, U16 LtcID, U16 Logic);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevSetLTCInEdge(HAND DeviceHandle, U16 LtcID, U16 Edge);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevGetLTCData(HAND DeviceHandle, U16 LtcID, PF64 CommandPosition, PF64 Actualposition);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevGetLTCFlag(HAND DeviceHandle, U16 LtcID, PU8 LtcFlag);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevResetLTC(HAND DeviceHandle, U16 LtcID);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevGetCmpFlag(HAND DeviceHandle, U16 CmpID, PU8 CmpFlag);// [8/28/2014 deng]
U32 ADVCMNAPI Acm_DevResetCmpFlag(HAND DeviceHandle, U16 CmpID);// [9/5/2014 deng]
U32 ADVCMNAPI Acm_DevGetLtcLinkCmpStatus(HAND DeviceHandle, U16 LtcID, PU16 LinkStatus);// [9/17/2014 deng]
U32 ADVCMNAPI Acm_DevResetCmpData(HAND DeviceHandle, U16 CmpID);// [9/17/2014 deng]
U32 ADVCMNAPI Acm_DevGetLTCInEdge(HAND DeviceHandle, U16 LtcID, PU16 Edge);
U32 ADVCMNAPI Acm_DevGetLTCInPol(HAND DeviceHandle, U16 LtcID, PU16 Logic);
U32 ADVCMNAPI Acm_DevGetLTCSaftyDist(HAND DeviceHandle, U16 LtcID, PF64 SaftyDist);
U32 ADVCMNAPI Acm_DevGetLTCInSource(HAND DeviceHandle, U16 LtcID, PU16 Source);
U32 ADVCMNAPI Acm_DevSetLTCInSource(HAND DeviceHandle, U16 LtcID, U16 Source);
U32 ADVCMNAPI Acm_DevGetCmp(HAND DeviceHandle, U16 CmpID, PU32 CmpLogic, PU32 CmpSrc, PU32 CmpMethod, PU32 DOMode, PU32 DOWidth);

U32	ADVCMNAPI Acm_DevReadLatchBuffer(HAND DeviceHandle, U16 LtcID, PF64 CommandPositionArray, PF64 ActualPositionArray, PU32 DataCnt);
U32	ADVCMNAPI Acm_DevGetLatchBufferStatus(HAND DeviceHandle, U16 LtcID, PU32 RemainCnt, PU32 SpaceCnt, PU32 LtcCounter);
U32	ADVCMNAPI Acm_DevResetLatchBuffer(HAND DeviceHandle, U16 LtcID);

U32 ADVCMNAPI Acm_DevSetLTCInAxisID(HAND DeviceHandle, U16 LtcID, U32 AxisID);
U32 ADVCMNAPI Acm_DevGetLTCInAxisID(HAND DeviceHandle, U16 LtcID, PU32 AxisID);
U32 ADVCMNAPI Acm_DevSetCmpAxisID(HAND DeviceHandle, U16 CmpID, U32 AxisID);
U32 ADVCMNAPI Acm_DevGetCmpAxisID(HAND DeviceHandle, U16 CmpID, PU32 AxisID);

U32 ADVCMNAPI Acm_RbGetWorldPosFromJoint(HAND RbHandle,PF64 JointPosArray, PF64 WorldPosArray);  //  [8/12/2014 yang.kai]
U32 ADVCMNAPI Acm_GetDevNum(U32 DevType,U32 BoardID,PU32 DeviceNumber);            //  [9/2/2014 yang.kai]

//mapping 
U32 ADVCMNAPI Acm_DevSaveMapFile(HAND DeviceHandle, PI8 FilePath);
U32 ADVCMNAPI Acm_DevLoadMapFile(HAND DeviceHandle, PI8 FilePath);
U32 ADVCMNAPI Acm_DevUpLoadMapInfo(HAND DeviceHandle, U16 MapType, PDEV_IO_MAP_INFO MapInfoArray, PU32 ArrayLength);
U32 ADVCMNAPI Acm_DevDownLoadMapInfo(HAND DeviceHandle, U16 MapType, PDEV_IO_MAP_INFO MapInfoArray, U32 ArrayLength);

//just test
U32 ADVCMNAPI Acm_DevWriteMem_Ulong(HAND DeviceHandle, ULONG Address, ULONG dwData);
U32 ADVCMNAPI Acm_DevReadMem_Ushort(HAND DeviceHandle, ULONG Address, PUSHORT usData);
U32 ADVCMNAPI Acm_DevWriteMem_Ushort(HAND DeviceHandle, ULONG Address, USHORT dwData);
U32 ADVCMNAPI Acm_DevReadMem_Ulong(HAND DeviceHandle, ULONG Address, PULONG dwData);

//EtherCAT
U32 ADVCMNAPI Acm_DevSetSlaveStates(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, U16 SlvState);
U32 ADVCMNAPI Acm_DevGetSlaveStates(HAND DeviceHandle, U16 RingNo, U16 SlaveIP, PU16 SlvState);
U32 ADVCMNAPI Acm_DevWriteSDOData(HAND DeviceHandle, IN U16 RingNo, IN U16 SlaveIP, IN U16 Index, IN U16 SubIndex, IN U16 Type, IN U16 DataSize, IN PVOID pValue);
U32 ADVCMNAPI Acm_DevReadSDOData(HAND DeviceHandle, IN U16 RingNo, IN U16 SlaveIP, IN U16 Index, IN U16 SubIndex, IN U16 Type, IN U16 DataSize, OUT PVOID pValue);
U32 ADVCMNAPI Acm_DevWriteRegData(HAND DeviceHandle, IN U16 RingNo, IN U16 SlaveIP, IN U16 Address, IN U16 Type, IN U16 DataSize, IN PVOID pValue);
U32 ADVCMNAPI Acm_DevReadRegData(HAND DeviceHandle, IN U16 RingNo, IN U16 SlaveIP, IN U16 Address, IN U16 Type, IN U16 DataSize, OUT PVOID pValue);
U32 ADVCMNAPI Acm_DevReadEmgMessage(HAND DeviceHandle, IN U16 RingNo, IN U16 SlaveIP, IN U16 DataSize, OUT PU8 EmgMessage);
U32 ADVCMNAPI Acm_DevReadSlvCommErrCnt(HAND DeviceHandle, IN U16 RingNo, IN PU32 ErrCntArray, IN PU32 ArrayElements);
//ARES
U32 ADVCMNAPI Acm_DevDownloadAres(HAND DeviceHandle, PI8 FilePath);
U32 ADVCMNAPI Acm_DevGetAresVariable(HAND DeviceHandle, U32 Address, PF64 Value);
U32 ADVCMNAPI Acm_DevSetAresVariable(HAND DeviceHandle, U32 Address, F64 Value);
U32 ADVCMNAPI Acm_DevGetDownloadedAresName(HAND DeviceHandle, PDEV_ARES_INFO FileName, PU32 Count);
U32 ADVCMNAPI Acm_DevDeleteAresFile(HAND DeviceHandle, PI8 FileName);
U32 ADVCMNAPI Acm_DevClearAresFile(HAND DeviceHandle);
U32 ADVCMNAPI Acm_DevGetAresAllVariable(HAND DeviceHandle, PF64 Array, U32 length);//tehsin add

U32 ADVCMNAPI Acm_DevGetRunningAresName(HAND DeviceHandle, U32 proc_id, PI8 FileName);
U32 ADVCMNAPI Acm_DevGetAresState(HAND DeviceHandle, U32 proc_id, PU16 pState);
U32 ADVCMNAPI Acm_DevGetAresCrc(HAND DeviceHandle, U32 proc_id, PU32 Crc);
U32 ADVCMNAPI Acm_DevGetAresBreakPoints(HAND DeviceHandle, U32 proc_id, PU32 Breakpoints, PU32 Count);
U32 ADVCMNAPI Acm_DevGetAresNextOffset(HAND DeviceHandle, U32 proc_id, PU32 Offset);

U32 ADVCMNAPI Acm_DevLoadAresProgram(HAND DeviceHandle, U32 proc_id, PI8 FileName);
U32 ADVCMNAPI Acm_DevRunAres(HAND DeviceHandle,U32 proc_id);
U32 ADVCMNAPI Acm_DevHoldAres(HAND DeviceHandle,U32 proc_id);
U32 ADVCMNAPI Acm_DevStopAres(HAND DeviceHandle,U32 proc_id);
U32 ADVCMNAPI Acm_DevStepAres(HAND DeviceHandle, U32 proc_id,U32 Offset);
U32 ADVCMNAPI Acm_DevSetAresBreakPoint(HAND DeviceHandle, U32 proc_id,U32 Offset);
U32 ADVCMNAPI Acm_DevRemoveAresBreakPoint(HAND DeviceHandle, U32 proc_id,U32 Offset);
U32 ADVCMNAPI Acm_DevClearAresBreakPoints(HAND DeviceHandle,U32 proc_id);
U32 ADVCMNAPI Acm_DevGetAresErrorMsg(HAND DeviceHandle, U32 proc_id, PU32 errCode, PU32 errLine, PI8 fileName);

U32 ADVCMNAPI Acm_GpAresInfoCopy(HAND GroupHandle,void* info);//tehsin test

//SRP:Cylinder control. 2016.4.15
U32 ADVCMNAPI Acm_IOUnitAddDo(HAND DeviceHandle, U32 BufIndex, PU32 DoArray, U32 DoCnt);
U32 ADVCMNAPI Acm_IOUnitInpCfg(HAND DeviceHandle, U32 BufIndex, U32 ActionMode, U32 IOInpMode, U32 IOInpDelayT, U32 IOInpDI, U32 DOStatus, U32 WarnTime, U32 InpReset);
U32 ADVCMNAPI Acm_IOUnitInpEncCfg(HAND DeviceHandle, U32 BufIndex, U32 ActionMode, U32 AxisID, U32 EncCmpMode, F64 INPEncValue, F64 DOEncValue);
U32 ADVCMNAPI Acm_IOUnitStart(HAND DeviceHandle, U32 BufIndex, U32 ActionMode);
U32 ADVCMNAPI Acm_IOUnitReset(HAND DeviceHandle, U32 BufIndex, U32 ResetMode);
U32 ADVCMNAPI Acm_IOUnitStatus(HAND DeviceHandle, U32 BufIndex, PU32 MultiIOStatus);
U32 ADVCMNAPI Acm_IOUnitEnableEvent(HAND DeviceHandle, U32 BufIndex, U32 EvtEn);
U32 ADVCMNAPI Acm_IOUnitCheckEvent(HAND DeviceHandle, PU32 EvtStatusArray, U32 Arraycnt, U32 Millisecond);
//end.

//Cylinder revision. ydd 2016.11.9
U32 ADVCMNAPI Acm_CYLCreate(HAND DeviceHandle, U32 BufIndex, PU32 DoArray, U32 DoCnt, PHAND CYLHandle);
U32 ADVCMNAPI Acm_CYLFwdInpCfg (HAND CYLHandle, U32 IOInpMode, U32 IOInpDelayT, U32 IOInpDI, U32 WarnTime, U32 AxID, F64 EncValue);
U32 ADVCMNAPI Acm_CYLBwdInpCfg(HAND CYLHandle, U32 IOInpMode, U32 IOInpDelayT, U32 IOInpDI, U32 WarnTime, U32 AxID, F64 EncValue);
U32 ADVCMNAPI Acm_CYLMove(HAND CYLHandle, U32 ActionMode);
U32 ADVCMNAPI Acm_CYLStop(HAND CYLHandle, U32 ResetMode);
U32 ADVCMNAPI Acm_CYLStatus(HAND CYLHandle, PU32 MultiIOStatus);
U32 ADVCMNAPI Acm_CYLEnableEvent(HAND CYLHandle, U32 EvtEn);
U32 ADVCMNAPI Acm_CYLCheckEvent(HAND DeviceHandle, /*U32 BufIndex, */PU32 EvtStatusArray, U32 Arraycnt, U32 Millisecond);

U32 ADVCMNAPI Acm_AxMoveTorque(HAND AxisHandle, F64 Distance, F64 Torque, F64 Velocity, F64 PressTime, U8 Mode);
U32 ADVCMNAPI Acm_AxGetActTorque(HAND AxisHandle, PI32 Torque);
U32	ADVCMNAPI Acm_Ax2DCompensateInAx (HAND AxisHandle, HAND RelAxisHandle, PF32 Coefficient, PF32 RelCoefficient, U32 ArrayElements);
U32	ADVCMNAPI Acm_Ax1DCompensateTable (HAND AxisHandle, F32 OriginPos, F32 Pitch, PF32 OffsetData, U32 OffsetElements);
U32 ADVCMNAPI Acm_Dev2DCompensateTable (HAND DeviceHandle, HAND AxisHandle, HAND RelAxisHandle, F32 OriginPosX, F32 OriginPosY, F32 PitchX, F32 PitchY, PF32 OffsetDataX, PF32 OffsetDataY, U32 OffsetElementsX, U32 OffsetElementsY);
U32 ADVCMNAPI Acm_AxGetCompensatePosition(HAND AxisHandle, PF64 Position);

U32 ADVCMNAPI Acm_DevMultiTrigInitial(HAND DeviceHandle, IN U16 RingNo, IN U16 SlaveIP, IN U16 Enable, IN U8 PWM, IN U8 LTC, IN U8 MPG);
//Axis Follow Group
U32 ADVCMNAPI Acm_GpEnableAxisFollow(HAND GroupHandle, PU32 AxisArray, PF64 OffsetValue, PF64 RatioArray);
U32 ADVCMNAPI Acm_GpStopAxisFollow(HAND GroupHandle);
U32 ADVCMNAPI Acm_GpSetFollowWpOffset(HAND GroupHandle, PF64 WpCenterPos, PF64 WpOffset);

U32 ADVCMNAPI Acm_EnableOneDevEventCallBack(HANDLE DeviceHandle, ULONG EventID); //ydd test

U32 ADVCMNAPI Acm_AxGetRawData(HAND AxisHandle, U8 index, PF64 RawData);
#ifdef __cplusplus
}
#endif
