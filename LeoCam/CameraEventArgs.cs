using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace LeoCam
{
    public class CameraEventArgs : EventArgs
    {
        public string SerialNumber { get; private set; }

        public CameraType CamType { get; private set; }

        public uint ImageIndex { get; private set; }
        
        public IntPtr Pixels { get; private set; }

        public HObject HOimg { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public Bitmap BitMap { get; private set; }
        public byte[] ImgData { get; private set; }


        public CameraEventArgs(string serialNumber, CameraType camType, uint imageIndex, IntPtr pixels,HObject hoImg, int width, int height)
        {
            this.SerialNumber = serialNumber;
            this.CamType = camType;
            this.ImageIndex = imageIndex;
            
            this.Pixels = pixels;
            this.HOimg = hoImg;
            this.Width = width;
            this.Height = height;
        }
    }

    public class CameraStartEventArgs : EventArgs
    {
        public string SerialNumber { get; private set; }

        public bool Start { get; private set; }

        public CameraStartEventArgs(string serialNumber,bool start)
        {
            this.SerialNumber = serialNumber;
            this.Start = start;
        }
    }

    public class CameraExceptionEventArgs : EventArgs
    {
        public string SerialNumber { get; private set; }

        public string Message { get; private set; }

        public CameraExceptionEventArgs(string serialNumber, string msg)
        {
            this.SerialNumber = serialNumber;
            this.Message = msg;
        }
    }



}
