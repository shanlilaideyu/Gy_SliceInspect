using HalconDotNet;
using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeoCam
{
    public class HKCamera : ICamera, IDisposable
    {
        public CamAcquisitionMode AcquisitionMode { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public CameraType CamType { get; set; }

        public uint ImageCount { get; set; }

        public uint ImageMaxPool { get; set; }

        public bool IsStart { get; set; }

        public string SerialNumber { get; set; }

        public CamPixelFormat SPixelFormat { get; set; }

        public CamStatus Status { get; set; }

        public CamTriggerMode TriggerMode { get; set; }

        public CamTriggerSouce TriggerSouce { get; set; }

        public event EventHandler<CameraEventArgs> NewImageEvent;
        public event EventHandler<CameraStartEventArgs> CameraStartEvent;
        public event EventHandler<CameraExceptionEventArgs> CameraExceptionEvent;



        public void Close()
        {
            try
            {
                this.Status = CamStatus.Close;
                m_pOperator.Close();

            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }

        }


        public bool GetPixelFormat(out CamPixelFormat pixelFormat)
        {
            bool mReturn = false;
            pixelFormat = CamPixelFormat.Mono8;

            if (this.Status == CamStatus.Open)
            {
                //this.SPixelFormat = pixelFormat;
                object mPixelFormat = new object();
                //object mMin, mMax;
                //mReturn = getParamValue("PixelFormat", ref mPixelFormat, out mMin, out mMax);

                switch (mPixelFormat.ToString())
                {
                    case "Mono8":
                        pixelFormat = CamPixelFormat.Mono8;
                        break;
                    case "YUV422_YUV_Packed":
                        pixelFormat = CamPixelFormat.YUV422_YUV_Packed;
                        break;
                    case "YUV422Packed":
                        pixelFormat = CamPixelFormat.YUV422Packed;
                        break;
                    case "BayerGR8":
                        pixelFormat = CamPixelFormat.BayerGR8;
                        break;
                    case "BayerRG8":
                        pixelFormat = CamPixelFormat.BayerRG8;
                        break;
                    case "BayerGB8":
                        pixelFormat = CamPixelFormat.BayerGB8;
                        break;
                    case "BayerBG8":
                        pixelFormat = CamPixelFormat.BayerBG8;
                        break;
                    default:
                        break;
                }


            }

            return mReturn;
        }

        public bool Open(string serialNumber)
        {
            bool mReturn = false;

            try
            {
                if (this.Status != CamStatus.Open)
                {
                    DeviceListAcq();
                    if (!this._DeviceList.ContainsKey(serialNumber))
                    {
                        OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, "No camera found!"));
                        return mReturn;
                    }

                    //this.SPixelFormat = CamPixelFormat.Mono8;
                   
                    MyCamera.MV_CC_DEVICE_INFO device =
               (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[0],
                                                             typeof(MyCamera.MV_CC_DEVICE_INFO));
                    m_pOperator = new CameraOperator();
                    //打开设备
                   int nRet = m_pOperator.Open(ref device);
                    if (MyCamera.MV_OK != nRet)
                    {
                        OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, "The camera device failed to open!"));
                        //MessageBox.Show("设备打开失败!");
                        return mReturn;
                    }

                    this.SerialNumber = serialNumber;
                    this.Status = CamStatus.Open;
                    this.AcquisitionMode = CamAcquisitionMode.SingleFrame;
                    this.TriggerMode = CamTriggerMode.Off;
                    this.TriggerSouce = CamTriggerSouce.Software;

                    //m_pOperator.SetEnumValue("AcquisitionMode", 2);
                    //m_pOperator.SetEnumValue("TriggerMode", 0);

                    SetAcquisitionMode(this.AcquisitionMode);
                    SetTriggerMode(this.TriggerMode);
                    SetTriggerSouce(this.TriggerSouce);
                    //SetPixelFormat(this.SPixelFormat);
                    CamPixelFormat mP;
                    bool mR = GetPixelFormat(out mP);
                    this.SPixelFormat = mP;

                    //设置采集连续模式
                    //m_pOperator.SetEnumValue("AcquisitionMode", 2);
                    //m_pOperator.SetEnumValue("TriggerMode", 0);

                    //注册回调函数
                    ImageCallback = new MyCamera.cbOutputdelegate(SaveImage);
                    nRet = m_pOperator.RegisterImageCallBack(ImageCallback, IntPtr.Zero);
                    if (CameraOperator.CO_OK != nRet)
                    {
                        OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, "Registration callback failed!"));
                        //MessageBox.Show("注册回调失败!");
                        return mReturn;
                    }

                    ExceptionCallBac = new MyCamera.cbExceptiondelegate(ExceptionCall);
                    nRet = m_pOperator.RegisterExceptionCallBack(ExceptionCallBac, IntPtr.Zero);

                    if (CameraOperator.CO_OK != nRet)
                    {
                        OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, "Registration callback failed!"));
                        //MessageBox.Show("注册回调失败!");
                        return mReturn;
                    }

                    this.IsStart = false;

                    //object ImgHeight = 0, ImageWidth = 0;
                    //object mMin, mMax;
                    //getParamValue("Height", ref ImgHeight, out mMin, out mMax);
                    //getParamValue("Width", ref ImageWidth, out mMin, out mMax);


                    uint ImgHeight = 0;
                    m_pOperator.GetIntValue("Height", ref ImgHeight);
                    uint ImageWidth = 0;
                    m_pOperator.GetIntValue("Width", ref ImageWidth);

                    this.Width = Convert.ToInt32(ImageWidth);
                    this.Height = Convert.ToInt32(ImgHeight);

                    ////开始采集
                    //nRet = m_pOperator.StartGrabbing();
                    //if (MyCamera.MV_OK == nRet)
                    //{
                    //    mReturn = true;
                    //}

                    //mReturn = true;
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
                OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, ex.ToString()));
                return mReturn;
            }

            return mReturn;
        }

        public bool SetAcquisitionMode(CamAcquisitionMode acquisitionMode = CamAcquisitionMode.Continuous)
        {
            bool mReturn = false;

            if (this.Status == CamStatus.Open)
            {
                this.AcquisitionMode = acquisitionMode;
                string mCommand = string.Format("{0}", this.AcquisitionMode);
                //mReturn = setParamValue("AcquisitionMode", mCommand);
                m_pOperator.SetEnumValue("AcquisitionMode", 2);
            }

            return mReturn;
        }

        public bool SetExposure(int value)
        {
            bool mReturn = false;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    //mReturn = setParamValue("ExposureTime", value);
                   int mR = m_pOperator.SetFloatValue("ExposureTime",(float)(value));
                    if (MyCamera.MV_OK == mR)
                    {
                        mReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }

            return mReturn;
        }

        public bool SetGain(double value)
        {
            bool mReturn = false;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    //mReturn = setParamValue("Gain", value);
                    int mR = m_pOperator.SetFloatValue("Gain", (float)(value));
                    if (MyCamera.MV_OK == mR)
                    {
                        mReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }

            return mReturn;
        }

        public bool SetGamma(double value)
        {
            bool mReturn = false;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    int mR = m_pOperator.SetFloatValue("Gamma", (float)(value));
                    if (MyCamera.MV_OK == mR)
                    {
                        mReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }

            return mReturn;
        }

        public bool GetExposure(out int value)
        {
            bool mReturn = false;
            value = 0;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    float fExposure = 0;
                    int mR = m_pOperator.GetFloatValue("ExposureTime", ref fExposure);
                    value = Convert.ToInt32(fExposure);

                    if (MyCamera.MV_OK == mR)
                    {
                        mReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }
            return mReturn;
        }

        public bool GetGain(out double value)
        {
            bool mReturn = false;
            value = 0;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    //MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                    //m_pMyCamera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
                    //tbGain.Text = stParam.fCurValue.ToString("F1");

                    //object mValue = 0;
                    //object mMin, mMax;
                    //mReturn = getParamValue("Gain", ref mValue, out mMin, out mMax);
                    float fGain = 0;
                    int mR = m_pOperator.GetFloatValue("Gain", ref fGain);
                    value = Convert.ToDouble(fGain);
                    if (MyCamera.MV_OK == mR)
                    {
                        mReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }
            return mReturn;
        }

        public bool GetGamma(out double value)
        {
            bool mReturn = false;
            value = 0;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    //object mValue = 0;
                    //object mMin, mMax;
                    //mReturn = getParamValue("Gamma", ref mValue, out mMin, out mMax);
                    float fGamma = 0;
                   int mR = m_pOperator.GetFloatValue("Gamma", ref fGamma);
                    value = Convert.ToDouble(fGamma);
                    if (MyCamera.MV_OK == mR)
                    {
                        mReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }
            return mReturn;
        }

        public bool SetTriggerSouce(CamTriggerSouce triggerSouce = CamTriggerSouce.Software)
        {
            bool mReturn = false;

            if (this.Status == CamStatus.Open)
            {
                this.TriggerSouce = triggerSouce;
                string mCommand = string.Format("{0}", this.TriggerSouce);
                //mReturn = setParamValue("TriggerSouce", mCommand);
                int mR = -1;
                switch (triggerSouce)
                {
                    case CamTriggerSouce.Software:
                        mR = m_pOperator.SetEnumValue("TriggerSource", 7);
                        break;
                    case CamTriggerSouce.Line0:
                        mR = m_pOperator.SetEnumValue("TriggerSouce", 0);
                        break;
                    case CamTriggerSouce.Line2:
                        mR = m_pOperator.SetEnumValue("TriggerSouce", 1);
                        break;
                    default:
                        break;
                }
            }

            return mReturn;
        }

        public bool SetTriggerMode(CamTriggerMode triggerMode = CamTriggerMode.Off)
        {
            bool mReturn = false;

            if (this.Status == CamStatus.Open)
            {
                this.TriggerMode = triggerMode;
                string mCommand = string.Format("{0}", this.TriggerMode);
                //mReturn = setParamValue("TriggerMode", mCommand);
                int mR = -1;
                switch (triggerMode)
                {
                    case CamTriggerMode.On:
                        mR = m_pOperator.SetEnumValue("TriggerMode", 0);
                        break;
                    case CamTriggerMode.Off:
                    default:
                        mR = m_pOperator.SetEnumValue("TriggerMode", 1);
                        break;
                }
                
                if (MyCamera.MV_OK == mR)
                {
                    mReturn = true;
                }
            }

            return mReturn;
        }

        public bool SetPixelFormat(CamPixelFormat pixelFormat = CamPixelFormat.Mono8)
        {
            bool mReturn = false;

            if (this.Status == CamStatus.Open)
            {
                this.SPixelFormat = pixelFormat;
                string mCommand = string.Format("{0}", this.TriggerMode);
                //mReturn = setParamValue("PixelFormat", mCommand);
                int mR = -1;
                switch (pixelFormat)
                {
                    case CamPixelFormat.Mono8:
                        mR = m_pOperator.SetEnumValue("PixelFormat", 0x01080001);
                        break;
                    case CamPixelFormat.YUV422_YUV_Packed:
                        break;
                    case CamPixelFormat.YUV422Packed:
                        break;
                    case CamPixelFormat.BayerGR8:
                        break;
                    case CamPixelFormat.BayerRG8:
                        break;
                    case CamPixelFormat.BayerGB8:
                        break;
                    case CamPixelFormat.BayerBG8:
                        break;
                    default:
                        break;
                }
                if (MyCamera.MV_OK == mR)
                {
                    mReturn = true;
                }
            }

            return mReturn;
        }

        public bool Start()
        {
            bool mReturn = false;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    int nRet;
                    if (!this.IsStart)
                    {
                        //开始采集
                        nRet = m_pOperator.StartGrabbing();
                        if (MyCamera.MV_OK == nRet)
                        {
                            this.IsStart = true;
                            mReturn = true;
                        }
                    }
                    else
                    {
                        mReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.IsStart = false;
                mReturn = false;
                OnStartStopEvent(new CameraStartEventArgs(this.SerialNumber, this.IsStart));
                LogRecord.LogHelper.Exception(this.GetType(), ex);
                OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, ex.ToString()));
            }


            return mReturn;
        }

        public bool Snap()
        {
            bool mReturn = false;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    int nRet;

                    if (!this.IsStart)
                    {
                        mReturn = Start();
                    }

                    //触发命令
                    nRet = m_pOperator.CommandExecute("TriggerSoftware");

                    if (CameraOperator.CO_OK == nRet)
                    {
                        mReturn = true;
                    }
                    else
                    {
                        mReturn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.IsStart = false;
                mReturn = false;
                OnStartStopEvent(new CameraStartEventArgs(this.SerialNumber, this.IsStart));
                LogRecord.LogHelper.Exception(this.GetType(), ex);
                OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, ex.ToString()));
            }


            return mReturn;
        }

        public bool Stop()
        {
            bool mReturn = false;
            try
            {
                if (this.Status == CamStatus.Open&&this.IsStart)
                {
                    m_pOperator.StopGrabbing();
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
                OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, ex.ToString()));
            }
            this.IsStart = false;
            return mReturn;
        }


        public void Dispose()
        {
            Close();
            //throw new NotImplementedException();
        }

        #region MyRegion

        //private List<string> cbDeviceList = new List<string>();

        private Dictionary<string, int> _DeviceList;
        /// <summary>
        /// 设备列表
        /// </summary>
        public Dictionary<string, int> DeviceList { get { return this._DeviceList; } }

        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        CameraOperator m_pOperator;
        MyCamera.cbOutputdelegate ImageCallback;
        MyCamera.cbExceptiondelegate ExceptionCallBac;


        private void DeviceListAcq()
        {
            int nRet;
            /*创建设备列表*/
            System.GC.Collect();
            this._DeviceList = new Dictionary<string, int>();
            this._DeviceList.Clear();
            nRet = CameraOperator.EnumDevices(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                //MessageBox.Show("枚举设备失败!");
                return;
            }

            //在窗体列表中显示设备名
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    //if (gigeInfo.chUserDefinedName != "")
                    //{
                    //    this._DeviceList.Add("GigE: " + gigeInfo.chUserDefinedName);
                    //}
                    //else
                    //{
                    //    this._DeviceList.Add("GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                    //}
                    this._DeviceList.Add(gigeInfo.chSerialNumber, i);
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    //if (usbInfo.chUserDefinedName != "")
                    //{
                    //    cbDeviceList.Add("USB: " + usbInfo.chUserDefinedName);
                    //}
                    //else
                    //{
                    //    cbDeviceList.Add("USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                    //}
                    this._DeviceList.Add(usbInfo.chSerialNumber, i);
                }

            }
                        
        }

        private void SaveImage(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser)
        {
            try
            {
                HObject m_Image;
                HOperatorSet.GenEmptyObj(out m_Image);
                m_Image.Dispose();
                HOperatorSet.GenImage1(out m_Image, "byte", this.Width, this.Height, pData);

                this.ImageCount++;

                OnNewImageEvent(new CameraEventArgs(this.SerialNumber, this.CamType, this.ImageCount, IntPtr.Zero, m_Image.Clone(), this.Width, this.Height));
                m_Image.Dispose();
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }
            GC.Collect();
        }

        private void ExceptionCall(uint msgType, IntPtr pData)
        {
            if (msgType == MyCamera.MV_EXCEPTION_DEV_DISCONNECT)
            {
                /*m_bDisConnect = false;

                m_bGrabbing = false;
                int nRet = -1;

                DeInitCamera();

                if (m_pDeviceList.nDeviceNum == 0 || cbDeviceList.SelectedIndex == -1)
                {
                    ShowErrorMsg("No device, please Select", 0);
                    return;
                }

                // ch:获取选择的设备信息 | en:Get Used Device Info
                MyCamera.MV_CC_DEVICE_INFO device =
                    (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[cbDeviceList.SelectedIndex],
                                                                  typeof(MyCamera.MV_CC_DEVICE_INFO));

                // ch:打开设备 | en:Open Device
                while (!m_bDisConnect)
                {
                    nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
                    if (MyCamera.MV_OK != nRet)
                    {
                        ShowErrorMsg("Create Camera failed", nRet);
                        m_pMyCamera.MV_CC_DestroyDevice_NET();
                        continue;
                    }

                    nRet = m_pMyCamera.MV_CC_OpenDevice_NET();
                    if (MyCamera.MV_OK != nRet)
                    {
                        m_pMyCamera.MV_CC_DestroyDevice_NET();
                        continue;
                    }

                    else
                    {
                        nRet = InitCamera();
                        if (MyCamera.MV_OK != nRet)
                        {
                            m_pMyCamera.MV_CC_DestroyDevice_NET();
                            continue;
                        }
                        m_bDisConnect = true;
                    }
                }*/

                OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, "CamreaDisconnect"));
            }
            else
            {
                OnExceptionEvent(new CameraExceptionEventArgs(this.SerialNumber, msgType.ToString()));
            }

            
        }

        #endregion
        protected virtual void OnNewImageEvent(CameraEventArgs e)
        {
            EventHandler<CameraEventArgs> handler = this.NewImageEvent;

            if (handler != null)
            {
                AsyncCallback nn = new AsyncCallback(eventCallback);
                handler.BeginInvoke(handler, e, nn, e);
                //handler(this, e);
            }
        }
        
        void eventCallback(IAsyncResult ar)
        {
            //this._FileIndex += 1;
            //if (this._FileIndex >= this._count)
            //{
            //    this._FileIndex = 0;
            //}
            //else
            //{
            //    if (this.Status == CamStatus.Grab)
            //    {
            //        Thread.Sleep(50);
            //        this._ent.Set();
            //    }
            //}
            GC.Collect();
        }

        protected virtual void OnStartStopEvent(CameraStartEventArgs e)
        {
            EventHandler<CameraStartEventArgs> handler = this.CameraStartEvent;

            if (handler != null)
            {
                //AsyncCallback nn = new AsyncCallback(eventCallback);
                //handler.BeginInvoke(handler, e, nn, e);
                handler(this, e);
            }
        }

        protected virtual void OnExceptionEvent(CameraExceptionEventArgs e)
        {
            EventHandler<CameraExceptionEventArgs> handler = this.CameraExceptionEvent;

            if (handler != null)
            {
                AsyncCallback nn = new AsyncCallback(exceptionEventCallback);
                handler.BeginInvoke(handler, e, nn, e);
                //handler(this, e);
            }
        }
        
        void exceptionEventCallback(IAsyncResult ar)
        {
            GC.Collect();
        }

        public HKCamera()
        {
            this.CamType = CameraType.CCD;
        }

        ~HKCamera()
        {
            Stop();
            Close();
        }
    }
}
