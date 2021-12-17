using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace SZ_BydKeyboard
{
    [Serializable]
    public class Leo9Calibra
    {
        //X轴坐标
        public double[] XposList = new double[9];
        //Y轴坐标
        public double[] YposList = new double[9];
        //图像坐标Rows
        public double[] ImageRows = new double[9];
        //图像坐标Cols
        public double[] ImageCols = new double[9];
        //转换矩阵
        public HHomMat2D HomMat2D = new HHomMat2D();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="CenterX">中心点X</param>
        /// <param name="CenterY">中心点Y</param>
        /// <param name="Offset">偏移</param>
        /// <param name="mode">标定模式 相对还是绝对</param>
        public void Init(double CenterX, double CenterY, double Offset, CalibraMode mode)
        {
            XposList = new double[9] { -1 * Offset, -1 * Offset, -1 * Offset, 0, 0, 0, Offset, Offset, Offset };
            YposList = new double[9] { -1 * Offset, 0, Offset, Offset, 0, -1 * Offset, -1 * Offset, 0, Offset };
            ImageRows = new double[9];
            ImageCols = new double[9];

            if (mode == CalibraMode.Absolute)
            {
                for (int i = 0; i < XposList.Length; i++)
                {
                    XposList[i] += CenterX;
                    YposList[i] += CenterY;
                }
            }
        }

        public void GenHomMat2D()
        {
            HomMat2D.VectorToHomMat2d(ImageRows, ImageCols, XposList, YposList);
        }

        /// <summary>
        /// 加入图像定位点
        /// </summary>
        /// <param name="Index">第几点 序号从1开始</param>
        /// <param name="ImageRow"></param>
        /// <param name="ImageCol"></param>
        /// <param name="MoveType"></param>
        public void AddImagePoint(int Index, double ImageRow, double ImageCol, MoveType MoveType)
        {
            if (MoveType == MoveType.AxisMove)
            {
                ImageRows[Index] = ImageRow;
                ImageCols[Index] = ImageCol;
            }
            else if (MoveType == MoveType.CamMove)
            {
                ImageRows[8 - Index] = ImageRow;
                ImageCols[8 - Index] = ImageCol;
            }
        }

        public bool ImageToAxis(double Row, double Col, out double Xpos, out double Ypos)
        {
            if (!HomMat2D.Equals(new HHomMat2D()))
            {
                Xpos = HomMat2D.AffineTransPoint2d(Row, Col, out Ypos);
                return true;
            }
            else
            {
                Xpos = 0;
                Ypos = 0;
                return false;
            }
        }

        public bool AxisToImage(double Xpos, double Ypos, out double Row, out double Col)
        {
            if (!HomMat2D.Equals(new HHomMat2D()))
            {
                HHomMat2D HomMatInvert = HomMat2D.HomMat2dInvert();
                HomMatInvert.AffineTransPixel(Xpos, Ypos, out Row, out Col);
                return true;
            }
            else
            {
                Row = 0;
                Col = 0;
                return false;
            }
        }

        public enum CalibraMode
        {
            Absolute,
            Relative
        }

        public enum MoveType
        {
            CamMove,
            AxisMove,
        }
    }
}
