using HalconDotNet;
using PvDotNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeoCam
{
    public class ECamera : ICamera, IDisposable
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

        public delegate void ImageGrabFinishCall(HObject ho_Image);
        public ImageGrabFinishCall grabFinishCall;

        #region MyRegion

        //private PvStreamGEV mStream = new PvStreamGEV();
        private PvSystem _System = null;

        private const UInt16 _BufferCount = 5;
        private PvDevice _Device = null;
        private PvDeviceGEV _DeviceGEV = null;
        private PvStream _Stream = null;


        private PvPipeline _Pipeline = null;
        private PvAcquisitionStateManager _Manager = null;

        private bool _IsStopping = false;

        private System.Threading.Thread _GrabImageThread = null;


        private List<string> _Vname = new List<string>();
        private int m_MissImage = 0;

        #endregion


        public void Close()
        {
            try
            {
                this.Status = CamStatus.Close;
                //PvBuffer lBuffer = null;
                PvResult lOperationResult = new PvResult(PvResultCode.OK);
                PvResult lResult = new PvResult(PvResultCode.OK);

                // Stop acquisition.
                this._Device.Parameters.ExecuteCommand("AcquisitionStop");

                // Disable streaming after sending the AcquisitionStop command.
                this._Device.StreamDisable();

                this._Pipeline.Stop();

                this._IsStopping = true;
                this._GrabImageThread.Join();
                this._GrabImageThread = null;

                /*
                // Abort all buffers in the stream and dequeue
                this._Stream.AbortQueuedBuffers();
                for (int i = 0; i < this._Stream.QueuedBufferCount; i++)
                {
                    lResult = this._Stream.RetrieveBuffer(ref lBuffer, ref lOperationResult);
                    if (lResult.IsOK)
                    {
                        lBuffer = null;
                    }
                }
                */

                if (this._Stream != null)
                {
                    this._Stream.Close();
                    this._Stream = null;
                }


                if (this._Device != null)
                {
                    this._Device.Disconnect();
                    this._Device = null;
                }
            }
            catch (Exception ex)
            {
                //LogRecord.LogHelper.Exception(this.GetType(), ex);
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
                object mMin, mMax;
                mReturn = getParamValue("PixelFormat", ref mPixelFormat, out mMin, out mMax);

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

            if (this._System == null)
            {
                this._System = new PvSystem();
            }

            try
            {
                PvDeviceInfo lDeviceInfo = null;
                //查找序列号相同的相机
                this._System.Find();

                for (uint i = 0; i < this._System.DeviceCount; i += 1)
                {
                    string UserName = this._System.GetDeviceInfo(i).UserDefinedName;

                    if (this._System.GetDeviceInfo(i).UserDefinedName == serialNumber)
                    {
                        lDeviceInfo = this._System.GetDeviceInfo(i);
                        break;
                    }
                }

                if (lDeviceInfo == null)
                {
                    return mReturn;
                }
                if (this.Status != CamStatus.Open)
                {
                    this._Device = PvDevice.CreateAndConnect(lDeviceInfo);

                    this._Stream = PvStream.CreateAndOpen(lDeviceInfo);
                    if (this._Stream == null)
                    {
                        return mReturn;
                    }

                    this._Pipeline = new PvPipeline(this._Stream);

                    this._DeviceGEV = _Device as PvDeviceGEV;
                    if (this._DeviceGEV != null)
                    {
                        Int64 lPayloadSize = this._Device.PayloadSize;

                        UInt32 lBufferCount = (this._Stream.QueuedBufferMaximum < _BufferCount) ? this._Stream.QueuedBufferMaximum : _BufferCount;
                        PvBuffer[] lBuffers = new PvBuffer[lBufferCount];
                        for (UInt32 i = 0; i < lBufferCount; i++)
                        {
                            lBuffers[i] = new PvBuffer();
                            lBuffers[i].Alloc((UInt32)lPayloadSize);
                        }

                        for (UInt32 i = 0; i < lBufferCount; i++)
                        {
                            this._Stream.QueueBuffer(lBuffers[i]);
                        }

                        PvStreamGEV lSGEV = this._Stream as PvStreamGEV;
                        this._DeviceGEV.SetStreamDestination(lSGEV.LocalIPAddress, lSGEV.LocalPort);
                    }

                    this._Pipeline.BufferSize = this._Device.PayloadSize;
                    this._Pipeline.BufferCount = _BufferCount;
                    this._Pipeline.Start();
                    this._Manager = new PvAcquisitionStateManager(this._DeviceGEV, this._Stream);

                    this.SerialNumber = serialNumber;
                    this.Status = CamStatus.Open;
                    this.AcquisitionMode = CamAcquisitionMode.Continuous;
                    this.TriggerMode = CamTriggerMode.Off;
                    this.TriggerSouce = CamTriggerSouce.Software;
                    //this.SPixelFormat = CamPixelFormat.Mono8;


                    SetAcquisitionMode(this.AcquisitionMode);
                    SetTriggerMode(this.TriggerMode);
                    SetTriggerSouce(this.TriggerSouce);
                    //SetPixelFormat(this.SPixelFormat);
                    CamPixelFormat mP;
                    bool mR = GetPixelFormat(out mP);
                    this.SPixelFormat = mP;

                    this._IsStopping = false;

                    this._GrabImageThread = new System.Threading.Thread(new ParameterizedThreadStart(ThreadProc));
                    object[] lParameters = new object[] { this._Stream, this._DeviceGEV, this._Pipeline };
                    this._GrabImageThread.Name = serialNumber;
                    this._GrabImageThread.IsBackground = true;
                    this._GrabImageThread.Start(lParameters);

                    this._Device.StreamEnable();

                    //Start acquisition on the device
                    //this._Device.Parameters.ExecuteCommand("AcquisitionStart");
                    this.IsStart = false;

                    object ImgHeight = 0, ImageWidth = 0;
                    object mMin, mMax;
                    getParamValue("Height", ref ImgHeight, out mMin, out mMax);
                    getParamValue("Width", ref ImageWidth, out mMin, out mMax);

                    this.Width = Convert.ToInt32(ImageWidth);
                    this.Height = Convert.ToInt32(ImgHeight);

                    mReturn = true;
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
                return mReturn;
            }

            return mReturn;
        }

        public bool SetAcquisitionMode(CamAcquisitionMode acquisitionMode = CamAcquisitionMode.Continuous)
        {
            bool mReturn = false;
            if (this.Status == CamStatus.Open)
            {

                this.Stop();

                this.AcquisitionMode = acquisitionMode;
                string mCommand = string.Format("{0}", this.AcquisitionMode);
                mReturn = setParamValue("AcquisitionMode", mCommand);
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
                    mReturn = setParamValue("ExposureTime", value);
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
                    mReturn = setParamValue("Gain", value);
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
                    mReturn = setParamValue("Gamma", value);
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
                    object mValue = 0;
                    object mMin, mMax;
                    mReturn = getParamValue("ExposureTime", ref mValue, out mMin, out mMax);
                    value = Convert.ToInt32(mValue);
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
                    object mValue = 0;
                    object mMin, mMax;
                    mReturn = getParamValue("Gain", ref mValue, out mMin, out mMax);
                    value = Convert.ToDouble(mValue);
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
                    object mValue = 0;
                    object mMin, mMax;
                    mReturn = getParamValue("Gamma", ref mValue, out mMin, out mMax);
                    value = Convert.ToDouble(mValue);
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
                mReturn = setParamValue("TriggerSouce", mCommand);
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
                mReturn = setParamValue("TriggerMode", mCommand);
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
                mReturn = setParamValue("PixelFormat", mCommand);
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
                    if (this._Pipeline != null)
                    {
                        this.IsStart = true;
                        if (this.AcquisitionMode != CamAcquisitionMode.Continuous)
                        {
                            SetAcquisitionMode(CamAcquisitionMode.Continuous);
                        }
                        this._Manager.Start();
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
                MessageBox.Show(ex.Message);
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
                    if (this._Pipeline != null)
                    {
                        if (this.AcquisitionMode != CamAcquisitionMode.SingleFrame)
                        {
                            SetAcquisitionMode(CamAcquisitionMode.SingleFrame);
                        }
                        this._Manager.Start();
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
            }
            return mReturn;
        }
        public bool Stop()
        {
            bool mReturn = false;
            try
            {
                if (this.Status == CamStatus.Open)
                {
                    if (this._Pipeline != null)
                    {
                        if (this._Pipeline.IsStarted && this._Device.IsConnected)// && this.IsStart)
                        {
                            if (this.IsStart)
                            {
                                this.IsStart = false;
                                OnStartStopEvent(new CameraStartEventArgs(this.SerialNumber, this.IsStart));
                            }
                            this._Manager.Stop();
                            //this._Device.Parameters.ExecuteCommand("AcquisitionStop");
                            mReturn = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }

            return mReturn;
        }

        #region MyRegion


        private bool setParamValue(string ye_ParamName, object ye_ParamValue)
        {
            bool mReturn = false;
            try
            {
                if (this._DeviceGEV == null)
                {
                    return mReturn;
                }
                PvGenParameter y_Param = this._DeviceGEV.Parameters.Get(ye_ParamName);
                if (y_Param == null)
                {
                    return mReturn;
                }
                if (!y_Param.IsWritable)//海康相机不支持写入triggermode参数
                {
                    return mReturn;
                }
                switch (y_Param.Type)
                {
                    case PvGenType.Boolean:
                        this._DeviceGEV.Parameters.SetBooleanValue(ye_ParamName, Convert.ToBoolean(ye_ParamValue));
                        mReturn = true;
                        break;
                    case PvGenType.Command:
                        //这里是记录执行是否成功
                        this._DeviceGEV.Parameters.ExecuteCommand(ye_ParamName);
                        mReturn = true;
                        break;
                    case PvGenType.Enum:
                        this._DeviceGEV.Parameters.SetEnumValue(ye_ParamName, Convert.ToString(ye_ParamValue));
                        mReturn = true;
                        break;
                    case PvGenType.Float:
                        this._DeviceGEV.Parameters.SetFloatValue(ye_ParamName, Convert.ToDouble(ye_ParamValue));
                        mReturn = true;
                        break;
                    case PvGenType.Integer:
                        this._DeviceGEV.Parameters.SetIntegerValue(ye_ParamName, Convert.ToInt32(ye_ParamValue));
                        mReturn = true;
                        break;
                    case PvGenType.String:
                        this._DeviceGEV.Parameters.SetStringValue(ye_ParamName, Convert.ToString(ye_ParamValue));
                        mReturn = true;
                        break;
                    default:
                        mReturn = false;
                        break;
                }

            }
            catch (Exception ex)
            {
                mReturn = false;
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }

            return mReturn;
        }

        private bool getParamValue(string ye_ParamName, ref object ye_ParamValue, out object ye_ParamValueMin, out object ye_ParamValueMax)
        {
            bool mReturn = false;
            ye_ParamValueMin = null;
            ye_ParamValueMax = null;

            if (this._DeviceGEV == null)
            {
                return mReturn;
            }
            PvGenParameter y_Param = this._DeviceGEV.Parameters.Get(ye_ParamName);
            if (y_Param == null)
            {
                return mReturn;
            }
            //if (!y_Param.IsReadable)
            //{
            //    return false;
            //}
            try
            {

                switch (y_Param.Type)
                {
                    case PvGenType.Boolean:
                        ye_ParamValue = this._DeviceGEV.Parameters.GetBooleanValue(ye_ParamName);
                        mReturn = true;
                        break;
                    case PvGenType.Command:
                        //这里是记录执行是否成功
                        this._DeviceGEV.Parameters.ExecuteCommand(ye_ParamName);
                        mReturn = true;
                        break;
                    case PvGenType.Enum:

                        //如何获取枚举列表
                        //IEnumerator ColorMode = y_Param.GetEnumerator();
                        //IEnumerator ColorMode2 = ye_DeviceGEV.Parameters.GetEnumerator2();

                        ye_ParamValue = this._DeviceGEV.Parameters.GetEnumValueAsString(ye_ParamName);
                        //ye_ParamValue = ye_DeviceGEV.Parameters.GetEnumerator()
                        mReturn = true;
                        break;
                    case PvGenType.Float:
                        ye_ParamValue = this._DeviceGEV.Parameters.GetFloatValue(ye_ParamName);
                        double temp_valmin = 0;
                        double temp_valmax = 0;
                        this._DeviceGEV.Parameters.GetFloatRange(ye_ParamName, ref temp_valmin, ref temp_valmax);

                        ye_ParamValueMax = (object)temp_valmax;
                        ye_ParamValueMin = (object)temp_valmin;
                        mReturn = true;
                        break;
                    case PvGenType.Integer:
                        ye_ParamValue = this._DeviceGEV.Parameters.GetIntegerValue(ye_ParamName);
                        long temp_min = 0;
                        long temp_max = 0;
                        this._DeviceGEV.Parameters.GetIntegerRange(ye_ParamName, ref temp_min, ref temp_max);

                        ye_ParamValueMax = (object)temp_max;
                        ye_ParamValueMin = (object)temp_min;
                        mReturn = true;
                        break;
                    case PvGenType.String:
                        ye_ParamValue = this._DeviceGEV.Parameters.GetStringValue(ye_ParamName);
                        mReturn = true;
                        break;
                    default:
                        mReturn = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                mReturn = false;
                LogRecord.LogHelper.Exception(this.GetType(), ex);
            }

            return mReturn;
        }

        HObject ho_Image;
        private void ThreadProc(object param)
        {
            object[] lParameters = (object[])param;

            PvStream mStream = lParameters[0] as PvStream;
            PvDeviceGEV mDeviceGEV = lParameters[1] as PvDeviceGEV;
            PvPipeline mPvPipeline = lParameters[2] as PvPipeline;

            object ImgHeight = 0, ImageWidth = 0;
            object mMin, mMax;
            getParamValue("Height", ref ImgHeight, out mMin, out mMax);
            getParamValue("Width", ref ImageWidth, out mMin, out mMax);

            int m_Height = Convert.ToInt32(ImgHeight);
            int m_Width = Convert.ToInt32(ImageWidth);
            //HImage m_Image = new HImage();

            if (ho_Image != null ? ho_Image.IsInitialized() : false)
            {
                ho_Image.Dispose();
            }
            HOperatorSet.GenEmptyObj(out ho_Image);
            while (!this._IsStopping)
            {
                PvBuffer lBuffer = null;

                //PvResult lOperationResult = new PvResult(PvResultCode.OK);
                ////Retrieve next buffer from acquisition pipeline
                //PvResult lResult = mStream.RetrieveBuffer(ref lBuffer, ref lOperationResult, 100);

                PvResult lResult = mPvPipeline.RetrieveNextBuffer(ref lBuffer, 1000);

                if (lResult.IsOK && lResult.IsSuccess)
                {

                    if (lBuffer.Image.Width == m_Width && lBuffer.Image.Height == m_Height)
                    {
                        ho_Image.Dispose();

                        if (this.SPixelFormat == CamPixelFormat.Mono8)
                        {
                            IntPtr mImagePoint = new IntPtr();
                            unsafe
                            {
                                mImagePoint = (IntPtr)lBuffer.Image.DataPointer;
                            }

                            HOperatorSet.GenImage1(out ho_Image, "byte", m_Width, m_Height, mImagePoint);
                        }
                        else
                        {
                            Bitmap mBitmap = new Bitmap(m_Width, m_Height, PixelFormat.Format32bppRgb);//Format32bppRgb or Format24bppRgb
                            lBuffer.Image.CopyToBitmap(mBitmap);
                            Rectangle rect = new Rectangle(0, 0, m_Width, m_Height);
                            BitmapData bmpData = mBitmap.LockBits(rect, ImageLockMode.ReadOnly, mBitmap.PixelFormat);
                            //m_Image.GenImageInterleaved(bmpData.Scan0, "bgr", m_Width, m_Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                            HOperatorSet.GenImageInterleaved(out ho_Image, bmpData.Scan0, "bgrx", m_Width, m_Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                            //HOperatorSet.GenImage1(out ho_Image, "byte", m_Width, m_Height, bmpData.Scan0);
                            mBitmap.UnlockBits(bmpData);
                        }

                        this.ImageCount++;
                        grabFinishCall(ho_Image);
                        ho_Image.Dispose();
                        //OnNewImageEvent(new CameraEventArgs(this.SerialNumber, this.CamType, this.ImageCount, IntPtr.Zero, ho_Image.Clone(), m_Width, m_Height));

                        //PublicDatabase.Unit.CameraData<HImage> mCameraData = new PublicDatabase.Unit.CameraData<HImage>(this.ImageCount, m_Image.Clone());

                        //PublicDatabase.CameraVariable.Instance().Push<PublicDatabase.Unit.CameraData<HImage>>(m_VirturalCamName, mCameraData, this.ImageMaxPool);//this.ImageMaxPool

                        //m_Image.Dispose();

                        //Console.WriteLine("image：{0} ----------------- {1}----{2}", m_VirturalCamName, this.ImageCount,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

                        Console.WriteLine("GrabImage:{0}", this.ImageCount);
                    }
                    else
                    {
                        m_MissImage++;
                        //Console.WriteLine("丢帧：{0},{1},{2}", m_MissImage, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), this.ImageCount);
                    }
                    //mStream.QueueBuffer(lBuffer);
                    mPvPipeline.ReleaseBuffer(lBuffer);
                    GC.Collect();
                }
                else
                {
                    Console.WriteLine("Fail GrabImage");
                }

            };

        }

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

        //int m_Count;
        /// <summary>
        /// 异步回调函数
        /// </summary>
        /// <param name="ar"></param>
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


        #endregion

        public void Dispose()
        {
            Close();
            //throw new NotImplementedException();
        }


        public ECamera()
        {
            this.CamType = CameraType.CCD;
        }

        ~ECamera()
        {
            Stop();
            Close();
        }
    }
}
