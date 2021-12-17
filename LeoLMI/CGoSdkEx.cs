/*********************************************
 * LMI Gocator SDK 二次开发
 * Power By Criss
 * Date:2019/01/21 14:28
 * Ver:1.0.0.0
 * *******************************************/
using System;
using Lmi3d.GoSdk;
using Lmi3d.GoSdk.Messages;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;
using Lmi3d.GoSdk.Outputs;
using System.Collections;

/*
 * GoSdkNet.dll
 * kApiNet.dll
 */ 

namespace LeoLMI
{
    public class CGoSdkEx
    {
        private GoSystem m_goSystem = null;
        private GoSensor m_goSensor = null;
        private GoAccelerator m_goAccelerator = null;
        private GoSetup m_goSetup = null;
        private GoLayout m_goLayout = null;
        private GoOutput m_goOutput = null;
        private GoEthernet m_goEthernet = null;

        private string m_strGoIPAddr = "192.168.1.10"; //默认IP
        private uint m_nGoID = 0; //默认产品ID
        private string m_strModel = "";

        public bool m_bInited = false; //初始化成功与否标志位
        private int m_nScanMode = -1;
        private long m_nImageZeroRowPos = 0;

        public delegate void GoSurfaceCall(IntPtr pData,long nWidth, long nHeight, ImageResInfo imgInfo);
        private GoSurfaceCall m_goSurfaceCall = null;
        private GoSurfaceCall m_goIntensityCall = null;

        public enum GoRunMode{Unknown = -1,Video = 0,Range = 1,Profile = 2,Surface = 3};

        public uint ID { get { return m_nGoID; } }

        public string Model { get { return m_strModel; } }


        public CGoSdkEx()
        {
            //KAPI初始化
            KApiLib.Construct();
            //GOSDK初始化
            GoSdkLib.Construct();
        }

        ~CGoSdkEx()
        {
            
        }

        /*
         * Lmi3d_InitDevice 初始化Lmi3D传感器
         * 返回值定义：
         * 0 初始化成功
         * -1 通过IP查找传感器失败
         * -2 其他初始化异常
         */
        public int Lmi3d_InitDevice(string strGoIP,bool bEnAccelerator = false)
        {
            try
            {
                m_goSystem = new GoSystem();
                if (bEnAccelerator)
                {
                    m_goAccelerator = new GoAccelerator();
                    m_goAccelerator.Start();
                }

                KIpAddress ipAddr = KIpAddress.Parse(strGoIP);
                m_goSensor = m_goSystem.FindSensorByIpAddress(ipAddr);
                if (bEnAccelerator)
                {
                    m_goAccelerator.Attach(m_goSensor);
                }

                m_goSystem.Connect();
                if (m_goSensor.IsConnected())
                {
                    m_strGoIPAddr = strGoIP;//产品IP
                    m_nGoID = m_goSensor.Id; //获取ID
                    m_strModel = m_goSensor.Model;
                    m_nScanMode = m_goSensor.ScanMode.Value;

                    m_goSystem.EnableData(true);
                    m_goSystem.SetDataHandler(Lmi3d_RecvDataAsync); //异步接收

                    m_goSetup = m_goSensor.Setup;
                    m_goOutput = m_goSensor.Output;
                    m_goEthernet = m_goOutput.GetEthernetAt(0);
                    m_goLayout = m_goSetup.GetLayout();

                    //开启输出
                    //GoOutput output = m_goSensor.Output;
                    //GoEthernet ethernetOutput = output.GetEthernetAt(0);
                    //ethernetOutput.ClearAllSources();
                    //ethernetOutput.AddSource(GoOutputSource.Measurement, 0);
                    //ethernetOutput.AddSource(GoOutputSource.Feature, 1);
                }
                else
                {
                    return -2;
                }
            }
            catch (KException ex)
            {
                m_bInited = false;
                string strErr = ex.Message;
                return -1;
            }

            m_bInited = true;
            return 0;
        }

        public int Lmi3d_InitDevice(uint nGoID, bool bEnAccelerator = false)
        {
            try
            {
                m_goSystem = new GoSystem();
                if(bEnAccelerator)
                {
                    m_goAccelerator = new GoAccelerator();
                    m_goAccelerator.Start();
                }
               
                m_goSensor = m_goSystem.FindSensorById(nGoID);
                if (bEnAccelerator)
                {
                    m_goAccelerator.Attach(m_goSensor);
                }
                   
                m_goSensor.Connect();

                if (m_goSensor.IsConnected())
                {
                    m_nGoID = nGoID; //产品ID
                    m_strModel = m_goSensor.Model;
                    m_nScanMode = m_goSensor.ScanMode.Value;

                    m_goSystem.EnableData(true);
                    m_goSystem.SetDataHandler(Lmi3d_RecvDataAsync); //异步接收

                    m_goSetup = m_goSensor.Setup;
                    m_goOutput = m_goSensor.Output;
                    m_goEthernet = m_goOutput.GetEthernetAt(0);
                    m_goLayout = m_goSetup.GetLayout();
                }
                else
                {
                    return -2;
                }
            }
            catch (Exception ex)
            {
                m_bInited = false;
                string strErr = ex.Message;
                return -1;
            }
            m_bInited = true;
            return 0;
        }

        public uint Lmi3d_GetID()
        {
            if (m_bInited)
            {
                return m_nGoID;
            }
            return 0;
        }

        public bool Lmi3d_GetInitStatus()
        {
            return m_bInited;
        }

        //关闭Lmi3D传感器
        public bool Lmi3d_CloseDevice()
        {
            try
            {
                if (m_bInited)
                {
                    if (m_goAccelerator != null)
                    {
                        if(m_goAccelerator.IsAttached(m_goSensor))
                        {
                            m_goAccelerator.Detach(m_goSensor);
                        }
                        m_goAccelerator.Stop();
                    }
                    m_goSystem.EnableData(false);
                    m_goSensor.Stop();
                    //m_goSystem.Disconnect();
                    //m_goSystem.Destroy();
                    m_goSystem = null;
                    m_bInited = false;
                    return true;
                }
            }
            catch(Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool Lmi3d_GetScanMode(out GoRunMode runMode)
        {
            try
            {
                if (m_bInited)
                {
                    m_nScanMode = m_goSetup.ScanMode.Value;
                    runMode = (GoRunMode)m_nScanMode;
                    return true;
                }
            }
            catch(KException ex)
            {
               Console.WriteLine(ex.Message);
            }
            runMode = GoRunMode.Unknown;
            return false;
        }

        public bool Lmi3d_SetScanMode(GoRunMode eMode)
        {
            try
            {
                if (m_bInited)
                {
                    int nMode = (int)eMode;
                    m_goSetup.ScanMode = (GoMode)nMode;
                    Lmi3d_Refresh(true);
                    return true;
                }
            }
            catch(KException ex)
            {
               Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool Lmi3d_Start_Sensor()
        {
            try
            {
                if (m_bInited)
                {
                    m_goSystem.LockState();
                    if(GoState.Ready== m_goSensor.State)
                    {
                        m_goSystem.Start();
                        Lmi3d_Refresh();
                    }
                    m_goSystem.UnlockState();
                return true;
                }
            }
            catch(KException ex)
            {
               Console.WriteLine(ex.Status + "," + ex.Message);
            }
            return false;
        }

        public bool Lmi3d_Stop_Sensor()
        {
            try
            {
                if (m_bInited)
                {
                    m_goSystem.LockState();
                    if (GoState.Running == m_goSensor.State)
                    {
                        m_goSystem.Stop();
                        Lmi3d_Refresh();
                    }
                    m_goSystem.UnlockState();
                    return true;
                 }
            }
            catch (KException ex)
            {
               Console.WriteLine(ex.Status + ","+ex.Message);
            }
            return false;
        }

        //异步获取数据
        private void Lmi3d_RecvDataAsync(KObject data)
        {
            try
            {
                GoDataSet ds = (GoDataSet)data;
                for (UInt32 i = 0; i < ds.Count; i++)
                {
                    GoDataMsg dataObj = (GoDataMsg)ds.Get(i);
                    Lmi3d_DataProcess(dataObj);
                }
            }
            catch (Exception ex)
            {
                string strErr = ex.Message;
            }
        }

        private void Lmi3d_DataProcess(GoDataMsg dataObj)
        {
           Console.WriteLine("接收到数据类型：" + dataObj.MessageType.ToString());
            switch (dataObj.MessageType)
            {
                case GoDataMessageType.Generic:
                    {
                        GoGenericMsg genericMsg = (GoGenericMsg)dataObj;
                       Console.WriteLine("genericMsg id：{0}", genericMsg.StreamStepId);
                    }
                    break;
                case GoDataMessageType.Stamp://帧数据
                    {
                        GoStampMsg stampMsg = (GoStampMsg)dataObj;
                        for (int i = 0; i < stampMsg.Count; i++)
                        {
                            GoStamp stamp = stampMsg.Get(i);
                           Console.WriteLine("帧索引 = {0}", stamp.FrameIndex);
                           Console.WriteLine("时间戳 = {0}", stamp.Timestamp);
                           Console.WriteLine("编码器值 = {0}", stamp.Encoder);
                            m_nImageZeroRowPos = stamp.Encoder; //获取图像零行所在编码器数值
                        }
                    }
                    break;
                case GoDataMessageType.UniformSurface: //均匀点云
                    {
                        GoUniformSurfaceMsg surfaceMsg = (GoUniformSurfaceMsg)dataObj;

                        //创建一个指针指向点云图
                        ImageResInfo imgInfo =new ImageResInfo();
                        imgInfo.m_nOffset_X = surfaceMsg.XOffset;
                        imgInfo.m_nOffset_Y = surfaceMsg.YOffset;
                        imgInfo.m_nOffset_Z = surfaceMsg.ZOffset;
                        imgInfo.m_nRes_X = surfaceMsg.XResolution;
                        imgInfo.m_nRes_Y = surfaceMsg.YResolution;
                        imgInfo.m_nRes_Z = surfaceMsg.ZResolution;
                        //m_goSurfaceCall?.Invoke(surfaceMsg.Data, surfaceMsg.Width, surfaceMsg.Length, imgInfo);
                        if(null != m_goSurfaceCall)
                        {
                            m_goSurfaceCall(surfaceMsg.Data, surfaceMsg.Width, surfaceMsg.Length, imgInfo);
                        }

                    }
                    break;
                case GoDataMessageType.UniformProfile: //均匀轮廓
                    {
                        GoUniformProfileMsg profileMsg = (GoUniformProfileMsg)dataObj;
                        if (profileMsg.StreamStep == GoDataStep.ToolDataOutput)
                        {
                           Console.WriteLine("profile dataid", profileMsg.StreamStepId);
                           Console.WriteLine("profile data", profileMsg.StreamStep);
                        }
                    }
                    break;
                case GoDataMessageType.SurfaceIntensity:  //亮度图
                    {
                        GoSurfaceIntensityMsg intensityMsg = (GoSurfaceIntensityMsg)dataObj;
                        long width = intensityMsg.Width;
                        long height = intensityMsg.Length;
                        long bufferSize = width * height;
                        IntPtr bufferPointer = intensityMsg.Data;

                        //创建一个指针指向点云图
                        ImageResInfo imgInfo = new ImageResInfo();
                        imgInfo.m_nOffset_X = intensityMsg.XOffset;
                        imgInfo.m_nOffset_Y = intensityMsg.YOffset;
                        imgInfo.m_nRes_X = intensityMsg.XResolution;
                        imgInfo.m_nRes_Y = intensityMsg.YResolution;
                        if(null != m_goIntensityCall)
                        {
                            m_goIntensityCall(intensityMsg.Data, intensityMsg.Width, intensityMsg.Length, imgInfo);
                         }
                        //m_goIntensityCall?.Invoke(intensityMsg.Data, intensityMsg.Width, intensityMsg.Length, imgInfo);
                    }
                    break;
                case GoDataMessageType.Measurement://测量
                    {
                        GoMeasurementMsg measurementMsg = (GoMeasurementMsg)dataObj;
                        for (int i = 0; i < measurementMsg.Count; i++)
                        {
                            GoMeasurementData measurementData = measurementMsg.Get(i);
                           Console.WriteLine("测量ID: {0}", measurementMsg.Id);
                           Console.WriteLine("测量数值: {0}", measurementData.Value);
                           Console.WriteLine("决策: {0}", measurementData.Decision);
                        }
                    }
                    break;
            }
        }

        //注册点云回调
        public void Lmi3d_RegSurfaceBack(GoSurfaceCall callBack)
        {
            if(callBack!=null)
            {
                m_goSurfaceCall = new GoSurfaceCall(callBack);
            }
        }

        //注册亮度回调
        public void Lmi3d_RegIntensityBack(GoSurfaceCall callBack)
        {
            if (callBack != null)
            {
                m_goIntensityCall = new GoSurfaceCall(callBack);
            }
        }

        //同步获取数据
        private void Lmi3d_RecvDataSync()
        {
            try
            {
                GoDataSet ds = m_goSystem.ReceiveData(30000000);
                for (UInt32 i = 0; i < ds.Count; i++)
                {

                    GoDataMsg dataObj = (GoDataMsg)ds.Get(i);
                    switch (dataObj.MessageType)
                    {
                        case GoDataMessageType.Stamp:
                            {
                                GoStampMsg stampMsg = (GoStampMsg)dataObj;
                                for (UInt32 j = 0; j < stampMsg.Count; j++)
                                {
                                    GoStamp stamp = stampMsg.Get(j);
                                   Console.WriteLine("Frame Index = {0}", stamp.FrameIndex);
                                   Console.WriteLine("Time Stamp = {0}", stamp.Timestamp);
                                   Console.WriteLine("Encoder Value = {0}", stamp.Encoder);
                                }
                            }
                            break;
                        case GoDataMessageType.Measurement:
                            {
                                GoMeasurementMsg measurementMsg = (GoMeasurementMsg)dataObj;
                                for (UInt32 k = 0; k < measurementMsg.Count; ++k)
                                {
                                    GoMeasurementData measurementData = measurementMsg.Get(k);
                                   Console.WriteLine("ID: {0}", measurementMsg.Id);
                                   Console.WriteLine("Value: {0}", measurementData.Value);
                                   Console.WriteLine("Decision: {0}", measurementData.Decision);
                                }
                            }
                            break;
                    }
                }
            }
            catch (KException ex)
            {
                string strErr = ex.Message;
            }
        }

        public void Lmi3d_ExportCsv(string strPath)
        {
            if(m_bInited)
            {
                m_goSensor.ExportCsv(strPath);
            }
        }

        public void Lmi3d_ResetEncoder()
        {
            try
            {
                if(m_bInited)
                {
                    m_goSystem.LockState();
                    m_goSensor.ResetEncoder();
                    Lmi3d_Refresh();
                    m_goSystem.UnlockState();
                }
            }
            catch(KException ex)
            {
               Console.WriteLine(ex.Message);
            }
        }

        /*
         * Lmi3d_GetEncoderNum 获取实时编码器数据(需要接入编码器)
         */ 
        public long Lmi3d_GetEncoderNum()
        {
            long nEncoder = -1;
            try
            {
                if(m_bInited)
                {
                    m_goSystem.LockState();
                    nEncoder = m_goSensor.Encoder();
                    m_goSystem.UnlockState();
                }
            }
            catch(KException ex)
            {
               Console.WriteLine(ex.Status + "," + ex.Message);
                nEncoder = -1;
                try
                {
                    m_goSensor.Connect();
                    nEncoder = m_goSensor.Encoder();
                }
                catch(KException ex1)
                {
                   Console.WriteLine(ex1.Status + "," + ex1.Message);
                    nEncoder = -2;
                }
            }
            return nEncoder;
        }

        public long Lmi3d_GetZeroRowPos()
        {
            long nEncoder = -1;
            try
            {
                if (m_bInited)
                {
                    nEncoder = m_nImageZeroRowPos;
                }
            }
            catch (Exception ex)
            {
                string strErr = ex.Message;
                nEncoder = -1;
            }
            return nEncoder;
        }

        public double Lmi3d_GetFrameRate()
        {
            double dRate = 0.0;
            try
            {
                if (m_bInited)
                {
                    GoSetup setup = m_goSensor.Setup;
                    dRate = setup.FrameRate;
                }
            }
            catch (Exception ex)
            {
                string strErr = ex.Message;
            }
            return dRate;
        }

        public int Lmi3d_GetExposureMode()
        {
            GoExposureMode nMode = -1;
            if (m_bInited)
            {
                nMode = m_goSetup.GetExposureMode(GoRole.Main);
            }
            return nMode;
        }

        public double Lmi3d_GetExposure()
        {
            double dValue = 0.0;
            if (m_bInited)
            {
                dValue = m_goSetup.GetExposure(GoRole.Main);
            }
            return dValue;
        }

        public long Lmi3d_GetExposureMultiCount()
        {
            long nCount = 0;
            if (m_bInited)
            {
                nCount = m_goSetup.GetExposureStepCount(GoRole.Main);
            }
            return nCount;
        }

        public double Lmi3d_GetExposureMultiStep(int nStep)
        {
            double dValue = 0.0;
            if (m_bInited)
            {
                dValue = m_goSetup.GetExposureStep(GoRole.Main, nStep);
            }
            return dValue;
        }

        public void Lmi3d_GetExposureMinMax(out double dMinExp,out double dMaxExp)
        {
            dMinExp = -1;
            dMaxExp = -1;
            try
            {
                if (m_bInited)
                {
                    dMinExp = m_goSetup.GetExposureLimitMin(GoRole.Main);
                    dMaxExp = m_goSetup.GetExposureLimitMax(GoRole.Main);
                    return;
                }
                dMinExp = 0.0;
                dMaxExp = 0.0;
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }            
        }

        public void Lmi3d_SetExposureMode(int nMode)
        {
           
            try
            {
                if (m_bInited)
                {
                    m_goSetup.SetExposureMode(GoRole.Main, nMode);
                    Lmi3d_Refresh();
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
        }

        public void Lmi3d_SetExposure(double fExpVal)
        {
            
            try
            {
				if (m_bInited)
	        	{
                    m_goSystem.LockState();
	                m_goSetup.SetExposure(GoRole.Main, fExpVal);
                    Lmi3d_Refresh();
                    m_goSystem.UnlockState();
                }
			}
            catch(Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
           
        }

        public void Lmi3d_SetExposureMulti(int nStep,double fMulExpoVal)
        {
            if (m_bInited)
            {
                //m_goLayout.MultiplexSinglePeriod = 0;
            }
        }

        public void Lmi3d_SetIpAddress(string strAddr)
        {
            if (m_bInited)
            {
                GoAddressInfo ipAdrr = new GoAddressInfo();
                ipAdrr.UseDhcp = false;
                ipAdrr.Address = KIpAddress.Parse(strAddr);
                ipAdrr.Mask = KIpAddress.Parse("255.255.255.0");
                m_goSensor.SetAddress(ipAdrr, true);
            }
        }

        public double Lmi3d_GetSurfaceFixedLength()
        {
            double fValue = 0.000;
            if (m_bInited)
            {
                GoSurfaceGeneration gsg = m_goSetup.GetSurfaceGeneration();
                fValue = gsg.FixedLengthLength;
            }
            return fValue;
        }

        public void Lmi3d_SetSurfaceFixedLength(double dLen)
        {
            if (m_bInited)
            {
                Lmi3d_Stop_Sensor();
                GoSurfaceGeneration gsg = m_goSetup.GetSurfaceGeneration();
                gsg.FixedLengthLength = dLen;
                Lmi3d_Refresh();
            }
        }

        public ArrayList Lmi3d_GetJobs()
        {
            Lmi3d_Refresh();
            ArrayList sensorJobName = new ArrayList();
            if (m_bInited)
            {
                string strJob = null;
                for (int i = 0; i < m_goSensor.FileCount; i++)
                {
                    strJob = m_goSensor.GetFileName(i);
                    if (!strJob.StartsWith("_live") &&strJob.EndsWith(".job"))
                    {
                        sensorJobName.Add(strJob);
                    }
                }
            }
            return sensorJobName;
        }

        public void Lmi3d_SetDefaultJob(string strJob)
        {
            if(m_bInited)
            {
                m_goSensor.DefaultJob = strJob;
                Lmi3d_Refresh();
            }
        }

        public string Lmi3d_GetDefaultJob()
        {
            string str = "";
            if(m_bInited)
            {
                str = m_goSensor.DefaultJob;
            }
            return str;
        }

        public string Lmi3d_GetLoadedJob()
        {
            string strFile = "";
            if (m_bInited)
            {
                bool bChanged = false;
                m_goSensor.LoadedJob(ref strFile, ref bChanged);
            }
            return strFile;
        }

        public void Lmi3d_LoadJob(string strJob)
        {
            if (m_bInited)
            {
               try
                {
                    m_goSensor.CopyFile(strJob, "_live.job");
                    Lmi3d_Refresh();
                }
                catch(KException ex)
                {
                   Console.WriteLine(ex.Message);
                }
            }
        }

        public void Lmi3d_DeleteJob(string strJob)
        {
            if(m_bInited)
            {
                m_goSensor.DeleteFile(strJob);
            }
        }

        public void Lmi3d_RenameJob(string strJobOld, string strJobNew)
        {
            if(m_bInited)
            {
                m_goSensor.CopyFile(strJobOld, strJobNew);
                Lmi3d_Refresh();
            }
        }

        public void Lmi3d_DownloadJob(string strJob,string strPath)
        {
            if (m_bInited)
            {
               try
                {
                    m_goSensor.DownloadFile(strJob, strPath);
                }
                catch(KException ex)
                {
                   Console.WriteLine(ex.Message);
                }
            }
        }

        public void Lmi3d_UploadJob(string strJob, string strPath)
        {
            if (m_bInited)
            {
                try
                {
                    m_goSensor.UploadFile(strPath, strJob);
                }
                catch (KException ex)
                {
                   Console.WriteLine(ex.Message);
                }
            }
        }

        public void Lmi3d_EnableIntensity(bool bEnable)
        {
            if(m_bInited)
            {
                try
                {
                    Lmi3d_Stop_Sensor();
                    //启动亮度图功能
                    m_goSetup.IntensityEnabled = bEnable;
                    //设置亮度图输出
                    if (bEnable)
                    {
                        m_goEthernet.AddSource(GoOutputSource.SurfaceIntensity, 0);
                    }
                    else
                    {
                        m_goEthernet.ClearSources(GoOutputSource.SurfaceIntensity);
                    }
                    Lmi3d_Refresh();
                }
                catch(KException ex)
                {
                   Console.WriteLine(ex.Message);
                }
            }
        }

        public void Lmi3d_Refresh(bool bForceRefresh = false)
        {
            if(m_bInited)
            {
                try
                {
                    if(m_goSystem.HasChanges()|| bForceRefresh)
                    {
                        m_goSystem.Refresh();
                    }
                    m_goSensor.Refresh();
                }
                catch(KException ex)
                {
                   Console.WriteLine(ex.Message);
                }
            }
        }

        public bool Lmi3d_GetGapFilingEnable(int nType)
        {
            bool bEnabled = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        bEnabled = m_goSetup.XGapFillingEnabled;
                        break;
                    case 1:
                        bEnabled = m_goSetup.YGapFillingEnabled;
                        break;
                }       
            }
            return bEnabled;
        }

        public bool Lmi3d_SetGapFilingEnable(int nType,bool bEnable)
        {
            bool bSet = false;
            if (m_bInited)
            {
               try
                {
                    m_goSystem.LockState();
                    switch (nType)
                    {
                        case 0:
                            m_goSetup.XGapFillingEnabled = bEnable;
                            break;
                        case 1:
                            m_goSetup.YGapFillingEnabled = bEnable;
                            break;
                    }
                    Lmi3d_Refresh();
                    m_goSystem.UnlockState();
                    bSet = true;
                }
                catch(KException ex)
                {
                   Console.WriteLine(ex.Message);
                    bSet = false;
                }
            }
            return bSet;
        }

        public bool Lmi3d_GetGapFilingMinMax(int nType,out double dMin,out double dMax)
        {
            bool bRtnOK = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        dMin = m_goSetup.XGapFillingWindowLimitMin;
                        dMax = m_goSetup.XGapFillingWindowLimitMax;
                        bRtnOK = true;
                        break;
                    case 1:
                        dMin = m_goSetup.YGapFillingWindowLimitMin;
                        dMax = m_goSetup.YGapFillingWindowLimitMax;
                        bRtnOK = true;
                        break;
                    default:
                        dMin = -1;
                        dMax = -1;
                        bRtnOK = false;
                        break;
                }
                return bRtnOK;
            }
            dMin = -1;
            dMax = -1;
            bRtnOK = false;
            return bRtnOK;
        }

        public bool Lmi3d_GetGapFilingValue(int nType,out double dVal)
        {
            bool bRtnOK = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        dVal = m_goSetup.XGapFillingWindow;
                        bRtnOK = true;
                        break;
                    case 1:
                        dVal = m_goSetup.YGapFillingWindow;
                        bRtnOK = true;
                        break;
                    default:
                        dVal = -1;
                        bRtnOK = false;
                        break;
                }
                return bRtnOK;
            }
            dVal = -1;
            bRtnOK = false;
            return bRtnOK;
        }

        public bool Lmi3d_SetGapFilingValue(int nType,double dVal)
        {
            bool bRtnOK = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        m_goSetup.XGapFillingWindow = dVal;
                        bRtnOK = true;
                        break;
                    case 1:
                        m_goSetup.YGapFillingWindow = dVal;
                        bRtnOK = true;
                        break;
                }
                return bRtnOK;
            }
            bRtnOK = false;
            return bRtnOK;
        }

        public bool Lmi3d_GetMedianEnable(int nType)
        {
            bool bEnabled = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        bEnabled = m_goSetup.XMedianEnabled;
                        break;
                    case 1:
                        bEnabled = m_goSetup.YMedianEnabled;
                        break;
                }
            }
            return bEnabled;
        }

        public bool Lmi3d_SetMedianEnable(int nType, bool bEnable)
        {
            bool bSet = false;
            if (m_bInited)
            {
                try
                {
                    switch (nType)
                    {
                        case 0:
                            m_goSetup.XMedianEnabled = bEnable;
                            break;
                        case 1:
                            m_goSetup.YMedianEnabled = bEnable;
                            break;
                    }
                    Lmi3d_Refresh();
                    bSet = true;
                }
                catch (KException ex)
                {
                   Console.WriteLine(ex.Message);
                    bSet = false;
                }
            }
            return bSet;
        }

        public bool Lmi3d_GetMedianMinMax(int nType, out double dMin, out double dMax)
        {
            bool bRtnOK = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        dMin = m_goSetup.XMedianWindowLimitMin;
                        dMax = m_goSetup.XMedianWindowLimitMax;
                        bRtnOK = true;
                        break;
                    case 1:
                        dMin = m_goSetup.YMedianWindowLimitMin;
                        dMax = m_goSetup.YMedianWindowLimitMax;
                        bRtnOK = true;
                        break;
                    default:
                        dMin = -1;
                        dMax = -1;
                        bRtnOK = false;
                        break;
                }
                return bRtnOK;
            }
            dMin = -1;
            dMax = -1;
            bRtnOK = false;
            return bRtnOK;
        }

        public bool Lmi3d_GetMedianValue(int nType, out double dVal)
        {
            bool bRtnOK = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        dVal = m_goSetup.XMedianWindow;
                        bRtnOK = true;
                        break;
                    case 1:
                        dVal = m_goSetup.YMedianWindow;
                        bRtnOK = true;
                        break;
                    default:
                        dVal = -1;
                        bRtnOK = false;
                        break;
                }
                return bRtnOK;
            }
            dVal = -1;
            bRtnOK = false;
            return bRtnOK;
        }

        public bool Lmi3d_GetSmoothingEnable(int nType)
        {
            bool bEnabled = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        bEnabled = m_goSetup.XSmoothingEnabled;
                        break;
                    case 1:
                        bEnabled = m_goSetup.YSmoothingEnabled;
                        break;
                }
            }
            return bEnabled;
        }

        public bool Lmi3d_SetSmoothingEnable(int nType, bool bEnable)
        {
            bool bSet = false;
            if (m_bInited)
            {
                try
                {
                    switch (nType)
                    {
                        case 0:
                            m_goSetup.XSmoothingEnabled = bEnable;
                            break;
                        case 1:
                            m_goSetup.YSmoothingEnabled = bEnable;
                            break;
                    }
                    Lmi3d_Refresh();
                    bSet = true;
                }
                catch (KException ex)
                {
                   Console.WriteLine(ex.Message);
                    bSet = false;
                }
            }
            return bSet;
        }

        public bool Lmi3d_GetSmoothingMinMax(int nType, out double dMin, out double dMax)
        {
            bool bRtnOK = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        dMin = m_goSetup.XSmoothingWindowLimitMin;
                        dMax = m_goSetup.XSmoothingWindowLimitMax;
                        bRtnOK = true;
                        break;
                    case 1:
                        dMin = m_goSetup.YSmoothingWindowLimitMin;
                        dMax = m_goSetup.YSmoothingWindowLimitMax;
                        bRtnOK = true;
                        break;
                    default:
                        dMin = -1;
                        dMax = -1;
                        bRtnOK = false;
                        break;
                }
                return bRtnOK;
            }
            dMin = -1;
            dMax = -1;
            bRtnOK = false;
            return bRtnOK;
        }

        public bool Lmi3d_GetSmoothingValue(int nType, out double dVal)
        {
            bool bRtnOK = false;
            if (m_bInited)
            {
                switch (nType)
                {
                    case 0:
                        dVal = m_goSetup.XSmoothingWindow;
                        bRtnOK = true;
                        break;
                    case 1:
                        dVal = m_goSetup.YSmoothingWindow;
                        bRtnOK = true;
                        break;
                    default:
                        dVal = -1;
                        bRtnOK = false;
                        break;
                }
                return bRtnOK;
            }
            dVal = -1;
            bRtnOK = false;
            return bRtnOK;
        }

        public void Lmi3d_Update()
        {
            if(m_bInited)
            {
                m_goSystem.UpdateMultiplexDelay();
            }
        }
    }

    public class  ImageResInfo
    {
        public double m_nOffset_X;
        public double m_nOffset_Y;
        public double m_nOffset_Z;
        public double m_nRes_X;
        public double m_nRes_Y;
        public double m_nRes_Z;
    }
}
