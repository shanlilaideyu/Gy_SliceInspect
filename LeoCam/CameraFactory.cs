using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoCam
{
    public class CameraFactory
    {
        public ICamera CreateCameraFactory(CameraType type)
        {
            switch (type)
            {
                case CameraType.File:
                    //return new CamFile.CameraFile();
                    break;
                case CameraType.CCD:
                    return new ECamera();
                    break;
                case CameraType.HK:
                    return new HKCamera();
                    break;
                case CameraType.LineScan:
                    //return new CamLineScan.CameraLineScan();
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
