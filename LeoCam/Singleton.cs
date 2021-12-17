using HalconDotNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoCam
{
    public class Singleton
    {
        protected static Singleton _instance = null;
        protected static object _lock = new object();

        private Singleton()
        {
            this.CameraNames = new Dictionary<string, Dictionary<CameraType, string>>();
            this.Cameras = new ConcurrentDictionary<string, ICamera>();

            //this.HKCamera = new CamHK.CameraSingle();
            //this.HKCamera.NewImageEvent += new EventHandler<CamHK.CameraEventArgs>(Cammera_NewImageEvent);
            //this.HKCamera.DeviceListAcq();

            this.Images = new ConcurrentDictionary<string, ConcurrentQueue<HObject>>();
        }

        public static Singleton GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Singleton();
                    }
                }
            }

            return _instance;
        }


        /// <summary>
        /// 实体相机与其对应的相机类型、描述
        /// </summary>
        public Dictionary<string, Dictionary<CameraType, string>> CameraNames { get; private set; }

        public bool IsInitCamera { get;private set; }
        private ConcurrentDictionary<string, ConcurrentQueue<HObject>> Images;

        private ConcurrentDictionary<string, ICamera> Cameras { get; set; }
        
        public event Action<string, HObject> NewHOimage = null;
        public event Action<string, bool> CameraStart = null;
        public event Action<string, string> CameraException = null;

        protected virtual void OnNewHOimage(string serialNumber, HObject image)
        {
            if (this.NewHOimage!=null)
            {
                this.NewHOimage(serialNumber, image);
                //AsyncCallback nn = new AsyncCallback(exceptionEventCallback);
                ////handler.BeginInvoke(handler, e, nn, e);
                //this.NewHOimage.BeginInvoke(serialNumber, image, nn, this.NewHOimage);
            }
        }

        void exceptionEventCallback(IAsyncResult ar)
        {
            GC.Collect();
        }

        protected virtual void OnCameraStart(string serialNumber, bool status)
        {
            if (this.CameraStart != null)
            {
                this.CameraStart(serialNumber, status);
            }
        }
        protected virtual void OnCameraException(string serialNumber, string message)
        {
            if (this.CameraException != null)
            {
                this.CameraException(serialNumber, message);
            }
        }


        /// <summary>
        /// 添加相机列表
        /// </summary>
        /// <param name="cameraSerialNumber"></param>
        /// <param name="camType"></param>
        /// <param name="camDescription"></param>
        public void SetEntitiesCamera(string cameraSerialNumber, CameraType camType, string camDescription)
        {
            if ((!cameraSerialNumber.Trim().Equals("")) && (!this.CameraNames.ContainsKey(cameraSerialNumber)))
            {
                Dictionary<CameraType, string> m = new Dictionary<CameraType, string>();
                m.Add(camType, camDescription);
                this.CameraNames.Add(cameraSerialNumber, m);
            }
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="entitiesCameraNames"></param>
        public void InitCameraAll()
        {
            #region 一个实体相机对应一个虚拟相机（CamX）
            
            this.Images = new ConcurrentDictionary<string, ConcurrentQueue<HObject>>();

            foreach (var cam in this.Cameras)
            {
                if (cam.Value.Status == CamStatus.Open)
                {
                    cam.Value.Close();
                }
            }

            CameraFactory m_CameraFactory = new CameraFactory();
            this.Cameras = new ConcurrentDictionary<string, ICamera>();

            foreach (var job in this.CameraNames)
            {
                foreach (var item in job.Value)
                {
                    ICamera m_ICamera = m_CameraFactory.CreateCameraFactory(item.Key);
                   
                    if (!job.Key.Equals(""))
                    {
                        bool mr = m_ICamera.Open(job.Key);
                        m_ICamera.NewImageEvent += M_ICamera_NewImageEvent;
                        m_ICamera.CameraStartEvent += M_ICamera_CameraStartEvent;
                        m_ICamera.CameraExceptionEvent += M_ICamera_CameraExceptionEvent;
                        this.Cameras.TryAdd(job.Key, m_ICamera);
                        this.Images.TryAdd(job.Key, new ConcurrentQueue<HObject>());
                        this.IsInitCamera = true;
                    }
                }

            }
            
            #endregion

            //this.HKCamera.open(this.HKCamera.Devices[0]);
        }

        private void M_ICamera_CameraExceptionEvent(object sender, CameraExceptionEventArgs e)
        {
            OnCameraException(e.SerialNumber, e.Message);
        }

        private void M_ICamera_CameraStartEvent(object sender, CameraStartEventArgs e)
        {
            OnCameraStart(e.SerialNumber, e.Start);
        }
        
        public void CloseCameraAll()
        {
            foreach (var cam in this.Cameras)
            {
                if (cam.Value.Status == CamStatus.Open)
                {
                    cam.Value.Close();
                    cam.Value.NewImageEvent -= M_ICamera_NewImageEvent;
                    cam.Value.CameraStartEvent -= M_ICamera_CameraStartEvent;
                }
            }
        }

        private void M_ICamera_NewImageEvent(object sender, CameraEventArgs e)
        {
            if (this.Images.ContainsKey(e.SerialNumber))
            {
                if (e.HOimg != null)
                {
                    if (e.HOimg.CountObj() > 0)
                    {
                        OnNewHOimage(e.SerialNumber, e.HOimg);
                        //this.Images[e.SerialNumber].Enqueue(e.HOimg);
                    }
                }
            }

        }

        public HObject GetHoImage(string serilaNumber)
        {
            HObject mImage = new HObject();
            mImage.Dispose();
            if (this.Images.ContainsKey(serilaNumber))
            {
                if (this.Images[serilaNumber].Count() > 0)
                {
                    if(this.Images[serilaNumber].TryDequeue(out mImage))
                    {
                        return mImage;
                    }
                    else
                    {
                        return mImage = null;
                    }
                }
                else
                {
                    return mImage = null;
                }
            }
            else
            {
                return mImage = null;
            }
        }


        public bool CheckCamera(string serialNumber)
        {
            bool mResult = false;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                mResult = true;
            }

            return mResult;
        }

        public bool Snap(string serialNumber)
        {
            bool mResult = false;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                mResult = this.Cameras[serialNumber].Snap();
            }

            return mResult;
        }

        public bool SetExposure(string serialNumber, int exposureTime)
        {
            bool mResult = false;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                mResult = this.Cameras[serialNumber].SetExposure(exposureTime);
            }
            
            return mResult;
        }

        public bool Start(string serialNumber)
        {
            bool mResult = false;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                mResult = this.Cameras[serialNumber].Start();
            }

            return mResult;
        }

        public bool Stop(string serialNumber)
        {
            bool mResult = false;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                mResult = this.Cameras[serialNumber].Stop();
            }

            return mResult;
        }

        public bool GetExposure(string serialNumber, out int exposureTime)
        {
            bool mResult = false;
            exposureTime = 0;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                this.Cameras[serialNumber].GetExposure(out exposureTime);
                mResult = true;
            }
            
            return mResult;
        }

        public bool GetGain(string serialNumber, out double gain)
        {
            bool mResult = false;
            gain = 0;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                mResult = this.Cameras[serialNumber].GetGain(out gain);
            }
            
            return mResult;
        }

        public bool GetGamma(string serialNumber, out double gamma)
        {
            bool mResult = false;
            gamma = 0;

            if (this.Cameras.ContainsKey(serialNumber))
            {
                mResult = this.Cameras[serialNumber].GetGamma(out gamma);
            }

            return mResult;
        }


    }
}
