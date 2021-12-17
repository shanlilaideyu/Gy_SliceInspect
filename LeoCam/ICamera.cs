using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoCam
{
    public enum CamStatus
    {
        None = 0,
        Close = 1,
        Open = 2,
        //Grab = 3,
        //Stop = 4,
    }


    public enum CameraType
    {
        File = 0,
        CCD = 1,
        LineScan = 4,
        HK = 5,
    }

    public enum CamTriggerSouce
    {
        Software = 0,
        Line0 = 1,
        Line2 = 2,
    }

    public enum CamTriggerMode
    {
        On = 0,
        Off = 1,
    }

    public enum CamAcquisitionMode
    {
        Continuous = 0,
        SingleFrame = 1,
    }

    public enum CamPixelFormat
    {
        Mono8 = 0,
        YUV422_YUV_Packed = 1,
        YUV422Packed = 2,
        BayerGR8 = 3,
        BayerRG8 = 4,
        BayerGB8 = 5,
        BayerBG8 = 6,
    }


    public interface ICamera
    {
        event EventHandler<CameraEventArgs> NewImageEvent;
        event EventHandler<CameraStartEventArgs> CameraStartEvent;
        event EventHandler<CameraExceptionEventArgs> CameraExceptionEvent;

        int Width { get; set; }
        int Height { get; set; }

        uint ImageCount { get; set; }

        string SerialNumber { get; set; }

        CameraType CamType { get; set; }

        CamStatus Status { get; set; }

        //List<VirtualCameraDescription> VirtualCameras { get; set; }

        uint ImageMaxPool { get; set; }

        CamAcquisitionMode AcquisitionMode { get; set; }
        CamTriggerSouce TriggerSouce { get; set; }
        CamTriggerMode TriggerMode { get; set; }
        CamPixelFormat SPixelFormat { get; set; }

        //CamSnapMode SnapMode { get; set; }

        bool IsStart { get; set; }

        bool Open(string serialNumber);

        void Close();

        bool Start();

        bool Stop();

        bool Snap();

        bool SetAcquisitionMode(CamAcquisitionMode acquisitionMode = CamAcquisitionMode.Continuous);

        bool SetTriggerSouce(CamTriggerSouce triggerSouce = CamTriggerSouce.Software);

        bool SetTriggerMode(CamTriggerMode triggerMode = CamTriggerMode.Off);

        bool SetPixelFormat(CamPixelFormat pixelFormat = CamPixelFormat.Mono8);

        bool GetPixelFormat(out CamPixelFormat pixelFormat);


        //void AddVirtualCamera(VirtualCameraDescription virtualCamName);

        bool SetExposure(int value);

        bool SetGain(double value);

        bool SetGamma(double value);

        bool GetExposure(out int value);

        bool GetGain(out double value);

        bool GetGamma(out double value);


        //bool Snap(string virtualCamName);

        //bool SnapContinue(string virtualCamName);

        //void ClearSnap();


    }

}
