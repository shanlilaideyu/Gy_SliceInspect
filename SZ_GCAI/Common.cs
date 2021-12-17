using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeoCam;
using LeoLink;
using LeoMotion;
using HalconDotNet;
using System.IO;

namespace SZ_BydKeyboard
{
    [Serializable]
    public class RunningData
    {
        //相机Z轴 平台Z轴相对偏移
        public double Offset_CameraZtoPlatZ = 0.1;
        //图像角度与平台角度相对偏移
        public double Offset_PlatAngleToImageAngle = 0.0;
        //芯片相机基准
        public double Base_PlatRow, Base_PlatCol, Base_PlatDeg;
        //针卡相机基准
        public double Base_Grab1X, Base_Grab1Y, Base_Grab2X, Base_Grab2Y;
        public double Base_Height, Base_FirstPinX, Base_FirstPinY, Base_LastPinX, Base_LastPinY, Base_PinDeg;
        //当前针卡
        public double Now_Height, Now_FirstPinX, Now_FirstPinY, Now_LastPinX, Now_LastPinY, Now_PinDeg;
        //针卡补偿
        public double OffsetX = 0.0, OffsetY = 0.0, OffsetDeg = 0.0;

        //焊点抛弃序号
        public int[] nThrowIndex = new int[] { 5, 6, 8 };
        //自动对焦
        public string str_SavePath = "";
        public double startPos = 12.05, endPos = 12.10, stepPos = 0.002;
        public bool b_SaveImage = true;
        //点集
        public List<double> PosList = new List<double> { };
        //梯度集
        public List<double> Gradient = new List<double> { };
    }

    [Serializable]
    public class HalconObject
    {
        public HTuple hv_PinRows = null, hv_PinCols = null;
        public HTuple hv_PointRows = null, hv_PointCols = null;
        //平台九点转换
        public HHomMat2D hom_Plat_ImageToAxis = new HHomMat2D();
        //针卡相机->芯片相机
        public HHomMat2D hom_ImageBToImageA = new HHomMat2D();
        //对焦区域
        public HRegion ho_FocusRegion = new HRegion();
        //针卡相机标定1
        public Leo9Calibra FirstPinCalibra = new Leo9Calibra();

        //针卡相机标定2
        public Leo9Calibra SecondPinCalibra = new Leo9Calibra();
    }

    public class Common
    {
        public static string str_ProductName = "NewOne";
        //其他数据
        public static RunningData Data = new RunningData();
        //
        public static HalconObject HalObject = new HalconObject();

        //相机1
        public static ECamera Cam1;

        //相机2
        public static ECamera Cam2;

        //控制类
        public static ProControl ProC = ProControl.GetInstance();

        //2D图像队列
        public static Queue<HObject> QueCam1 = new Queue<HObject>();
        //3D图像队列
        public static Queue<HObject> QueCam2 = new Queue<HObject>();

        public delegate void ShowMsg(string str);
        public static ShowMsg ShowMsgEvent;

        //设置权限
        public delegate void DelegateSetUserLevel(string Level);
        public static DelegateSetUserLevel setUserLevelEvent;

        //显示窗口
        public static FrmShow frmShow = new FrmShow();
        //log窗口
        public static FrmLogger frmLog = new FrmLogger();
        //运动设置
        public static FrmMotionSetting frmMotionSetting = new FrmMotionSetting();
        //软件设置
        public static FrmSetting frmSetting = new FrmSetting();

        public static void grabImage1(HObject ho_Image)
        {
            Common.QueCam1.Enqueue(ho_Image.Clone());
        }

        public static void grabImage2(HObject ho_Image)
        {
            Common.QueCam2.Enqueue(ho_Image.Clone());
        }

        public static void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
    HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_TempArrow = null;

            // Local control variables 

            HTuple hv_Length = new HTuple(), hv_ZeroLengthIndices = new HTuple();
            HTuple hv_DR = new HTuple(), hv_DC = new HTuple(), hv_HalfHeadWidth = new HTuple();
            HTuple hv_RowP1 = new HTuple(), hv_ColP1 = new HTuple();
            HTuple hv_RowP2 = new HTuple(), hv_ColP2 = new HTuple();
            HTuple hv_Index = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            //This procedure generates arrow shaped XLD contours,
            //pointing from (Row1, Column1) to (Row2, Column2).
            //If starting and end point are identical, a contour consisting
            //of a single point is returned.
            //
            //input parameteres:
            //Row1, Column1: Coordinates of the arrows' starting points
            //Row2, Column2: Coordinates of the arrows' end points
            //HeadLength, HeadWidth: Size of the arrow heads in pixels
            //
            //output parameter:
            //Arrow: The resulting XLD contour
            //
            //The input tuples Row1, Column1, Row2, and Column2 have to be of
            //the same length.
            //HeadLength and HeadWidth either have to be of the same length as
            //Row1, Column1, Row2, and Column2 or have to be a single element.
            //If one of the above restrictions is violated, an error will occur.
            //
            //
            //Init
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            //
            //Calculate the arrow length
            hv_Length.Dispose();
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ZeroLengthIndices = hv_Length.TupleFind(
                    0);
            }
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            }
            hv_DC.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            }
            hv_HalfHeadWidth.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            }
            //
            //Calculate end points of the arrow head.
            hv_RowP1.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            }
            hv_ColP1.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            }
            hv_RowP2.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            }
            hv_ColP2.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            }
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                            hv_Column1.TupleSelect(hv_Index));
                    }
                }
                else
                {
                    //Create arrow contour
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                            hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                            ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                            hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();

            hv_Length.Dispose();
            hv_ZeroLengthIndices.Dispose();
            hv_DR.Dispose();
            hv_DC.Dispose();
            hv_HalfHeadWidth.Dispose();
            hv_RowP1.Dispose();
            hv_ColP1.Dispose();
            hv_RowP2.Dispose();
            hv_ColP2.Dispose();
            hv_Index.Dispose();

            return;
        }

        public static void get_rectangle2_points(HTuple hv_CenterY, HTuple hv_CenterX, HTuple hv_Phi,
            HTuple hv_Len1, HTuple hv_Len2, out HTuple hv_CornerY, out HTuple hv_CornerX,
            out HTuple hv_LineCenterY, out HTuple hv_LineCenterX)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_RowT = new HTuple(), hv_ColT = new HTuple();
            HTuple hv_Cos = new HTuple(), hv_Sin = new HTuple();
            // Initialize local and output iconic variables 
            hv_CornerY = new HTuple();
            hv_CornerX = new HTuple();
            hv_LineCenterY = new HTuple();
            hv_LineCenterX = new HTuple();
            hv_CornerX.Dispose();
            hv_CornerX = new HTuple();
            hv_CornerY.Dispose();
            hv_CornerY = new HTuple();
            hv_LineCenterX.Dispose();
            hv_LineCenterX = new HTuple();
            hv_LineCenterY.Dispose();
            hv_LineCenterY = new HTuple();

            hv_RowT.Dispose();
            hv_RowT = 0;
            hv_ColT.Dispose();
            hv_ColT = 0;

            if ((int)((new HTuple(hv_Len1.TupleLessEqual(0))).TupleOr(new HTuple(hv_Len2.TupleLessEqual(
                0)))) != 0)
            {

                hv_RowT.Dispose();
                hv_ColT.Dispose();
                hv_Cos.Dispose();
                hv_Sin.Dispose();

                return;
            }

            hv_Cos.Dispose();
            HOperatorSet.TupleCos(hv_Phi, out hv_Cos);
            hv_Sin.Dispose();
            HOperatorSet.TupleSin(hv_Phi, out hv_Sin);

            hv_ColT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColT = (hv_CenterX - (hv_Len1 * hv_Cos)) - (hv_Len2 * hv_Sin);
            }
            hv_RowT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowT = hv_CenterY - (((-hv_Len1) * hv_Sin) + (hv_Len2 * hv_Cos));
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerY = hv_CornerY.TupleConcat(
                        hv_RowT);
                    hv_CornerY.Dispose();
                    hv_CornerY = ExpTmpLocalVar_CornerY;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerX = hv_CornerX.TupleConcat(
                        hv_ColT);
                    hv_CornerX.Dispose();
                    hv_CornerX = ExpTmpLocalVar_CornerX;
                }
            }


            hv_ColT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColT = (hv_CenterX + (hv_Len1 * hv_Cos)) - (hv_Len2 * hv_Sin);
            }
            hv_RowT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowT = hv_CenterY - ((hv_Len1 * hv_Sin) + (hv_Len2 * hv_Cos));
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerY = hv_CornerY.TupleConcat(
                        hv_RowT);
                    hv_CornerY.Dispose();
                    hv_CornerY = ExpTmpLocalVar_CornerY;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerX = hv_CornerX.TupleConcat(
                        hv_ColT);
                    hv_CornerX.Dispose();
                    hv_CornerX = ExpTmpLocalVar_CornerX;
                }
            }


            hv_ColT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColT = (hv_CenterX + (hv_Len1 * hv_Cos)) + (hv_Len2 * hv_Sin);
            }
            hv_RowT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowT = hv_CenterY - ((hv_Len1 * hv_Sin) - (hv_Len2 * hv_Cos));
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerY = hv_CornerY.TupleConcat(
                        hv_RowT);
                    hv_CornerY.Dispose();
                    hv_CornerY = ExpTmpLocalVar_CornerY;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerX = hv_CornerX.TupleConcat(
                        hv_ColT);
                    hv_CornerX.Dispose();
                    hv_CornerX = ExpTmpLocalVar_CornerX;
                }
            }


            hv_ColT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColT = (hv_CenterX - (hv_Len1 * hv_Cos)) + (hv_Len2 * hv_Sin);
            }
            hv_RowT.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowT = hv_CenterY - (((-hv_Len1) * hv_Sin) - (hv_Len2 * hv_Cos));
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerY = hv_CornerY.TupleConcat(
                        hv_RowT);
                    hv_CornerY.Dispose();
                    hv_CornerY = ExpTmpLocalVar_CornerY;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_CornerX = hv_CornerX.TupleConcat(
                        hv_ColT);
                    hv_CornerX.Dispose();
                    hv_CornerX = ExpTmpLocalVar_CornerX;
                }
            }



            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterX = hv_LineCenterX.TupleConcat(
                        ((1.0 * (hv_CornerX.TupleSelect(0))) / 2) + ((1.0 * (hv_CornerX.TupleSelect(1))) / 2));
                    hv_LineCenterX.Dispose();
                    hv_LineCenterX = ExpTmpLocalVar_LineCenterX;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterX = hv_LineCenterX.TupleConcat(
                        ((1.0 * (hv_CornerX.TupleSelect(1))) / 2) + ((1.0 * (hv_CornerX.TupleSelect(2))) / 2));
                    hv_LineCenterX.Dispose();
                    hv_LineCenterX = ExpTmpLocalVar_LineCenterX;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterX = hv_LineCenterX.TupleConcat(
                        ((1.0 * (hv_CornerX.TupleSelect(2))) / 2) + ((1.0 * (hv_CornerX.TupleSelect(3))) / 2));
                    hv_LineCenterX.Dispose();
                    hv_LineCenterX = ExpTmpLocalVar_LineCenterX;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterX = hv_LineCenterX.TupleConcat(
                        ((1.0 * (hv_CornerX.TupleSelect(3))) / 2) + ((1.0 * (hv_CornerX.TupleSelect(0))) / 2));
                    hv_LineCenterX.Dispose();
                    hv_LineCenterX = ExpTmpLocalVar_LineCenterX;
                }
            }

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterY = hv_LineCenterY.TupleConcat(
                        ((1.0 * (hv_CornerY.TupleSelect(0))) / 2) + ((1.0 * (hv_CornerY.TupleSelect(1))) / 2));
                    hv_LineCenterY.Dispose();
                    hv_LineCenterY = ExpTmpLocalVar_LineCenterY;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterY = hv_LineCenterY.TupleConcat(
                        ((1.0 * (hv_CornerY.TupleSelect(1))) / 2) + ((1.0 * (hv_CornerY.TupleSelect(2))) / 2));
                    hv_LineCenterY.Dispose();
                    hv_LineCenterY = ExpTmpLocalVar_LineCenterY;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterY = hv_LineCenterY.TupleConcat(
                        ((1.0 * (hv_CornerY.TupleSelect(2))) / 2) + ((1.0 * (hv_CornerY.TupleSelect(3))) / 2));
                    hv_LineCenterY.Dispose();
                    hv_LineCenterY = ExpTmpLocalVar_LineCenterY;
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_LineCenterY = hv_LineCenterY.TupleConcat(
                        ((1.0 * (hv_CornerY.TupleSelect(3))) / 2) + ((1.0 * (hv_CornerY.TupleSelect(0))) / 2));
                    hv_LineCenterY.Dispose();
                    hv_LineCenterY = ExpTmpLocalVar_LineCenterY;
                }
            }






            hv_RowT.Dispose();
            hv_ColT.Dispose();
            hv_Cos.Dispose();
            hv_Sin.Dispose();

            return;
        }

        public static void pts_to_best_line(out HObject ho_Line, HTuple hv_Rows, HTuple hv_Cols,
            HTuple hv_ActiveNum, out HTuple hv_Row1, out HTuple hv_Column1, out HTuple hv_Row2,
            out HTuple hv_Column2)
        {



            // Local iconic variables 

            HObject ho_Contour = null;

            // Local control variables 

            HTuple hv_Length = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Length1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            hv_Row1 = new HTuple();
            hv_Column1 = new HTuple();
            hv_Row2 = new HTuple();
            hv_Column2 = new HTuple();
            //初始化
            hv_Row1.Dispose();
            hv_Row1 = 0;
            hv_Column1.Dispose();
            hv_Column1 = 0;
            hv_Row2.Dispose();
            hv_Row2 = 0;
            hv_Column2.Dispose();
            hv_Column2 = 0;
            //产生一个空的直线对象，用于保存拟合后的直线
            ho_Line.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Line);
            //计算边缘数量
            hv_Length.Dispose();
            HOperatorSet.TupleLength(hv_Cols, out hv_Length);
            //当边缘数量不小于有效点数时进行拟合
            if ((int)((new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum))).TupleAnd(new HTuple(hv_ActiveNum.TupleGreater(
                1)))) != 0)
            {
                //halcon的拟合是基于xld的，需要把边缘连接成xld
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                //拟合直线。使用的算法是'tukey'，其他算法请参考fit_line_contour_xld的描述部分。
                hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
                HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out hv_Row1,
                    out hv_Column1, out hv_Row2, out hv_Column2, out hv_Nr, out hv_Nc, out hv_Dist);
                //判断拟合结果是否有效：如果拟合成功，数组中元素的数量大于0
                hv_Length1.Dispose();
                HOperatorSet.TupleLength(hv_Dist, out hv_Length1);
                if ((int)(new HTuple(hv_Length1.TupleLess(1))) != 0)
                {
                    ho_Contour.Dispose();

                    hv_Length.Dispose();
                    hv_Nr.Dispose();
                    hv_Nc.Dispose();
                    hv_Dist.Dispose();
                    hv_Length1.Dispose();

                    return;
                }
                //根据拟合结果，产生直线xld
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Line.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2),
                        hv_Column1.TupleConcat(hv_Column2));
                }
            }

            ho_Contour.Dispose();

            hv_Length.Dispose();
            hv_Nr.Dispose();
            hv_Nc.Dispose();
            hv_Dist.Dispose();
            hv_Length1.Dispose();

            return;
        }

        public static void rake(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
            HTuple hv_Transition, HTuple hv_Select, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
            HTuple hv_Column2, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null;

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_ATan = new HTuple(), hv_i = new HTuple(), hv_RowC = new HTuple();
            HTuple hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple();
            HTuple hv_ColL2 = new HTuple(), hv_ColL1 = new HTuple();
            HTuple hv_MsrHandle_Measure = new HTuple(), hv_RowEdge = new HTuple();
            HTuple hv_ColEdge = new HTuple(), hv_Amplitude = new HTuple();
            HTuple hv_tRow = new HTuple(), hv_tCol = new HTuple();
            HTuple hv_t = new HTuple(), hv_Number = new HTuple(), hv_j = new HTuple();
            HTuple hv_DetectWidth_COPY_INP_TMP = new HTuple(hv_DetectWidth);
            HTuple hv_Select_COPY_INP_TMP = new HTuple(hv_Select);
            HTuple hv_Transition_COPY_INP_TMP = new HTuple(hv_Transition);

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            //获取图像尺寸
            hv_Width.Dispose(); hv_Height.Dispose();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //产生一个空显示对象，用于显示
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            //初始化边缘坐标数组
            hv_ResultRow.Dispose();
            hv_ResultRow = new HTuple();
            hv_ResultColumn.Dispose();
            hv_ResultColumn = new HTuple();
            //产生直线xld
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_RegionLines.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                    hv_Column1.TupleConcat(hv_Column2));
            }
            //存储到显示对象
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_Regions, ho_RegionLines, out ExpTmpOutVar_0);
                ho_Regions.Dispose();
                ho_Regions = ExpTmpOutVar_0;
            }
            //计算直线与x轴的夹角，逆时针方向为正向。
            hv_ATan.Dispose();
            HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

            //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                {
                    HTuple
                      ExpTmpLocalVar_ATan = hv_ATan + ((new HTuple(90)).TupleRad()
                        );
                    hv_ATan.Dispose();
                    hv_ATan = ExpTmpLocalVar_ATan;
                }
            }

            //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
            HTuple end_val18 = hv_Elements;
            HTuple step_val18 = 1;
            for (hv_i = 1; hv_i.Continue(end_val18, step_val18); hv_i = hv_i.TupleAdd(step_val18))
            {
                //RowC := Row1+(((Row2-Row1)*i)/(Elements+1))
                //ColC := Column1+(Column2-Column1)*i/(Elements+1)
                //if (RowC>Height-1 or RowC<0 or ColC>Width-1 or ColC<0)
                //continue
                //endif
                //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                {
                    hv_RowC.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                    }
                    hv_ColC.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                    }
                    //判断是否超出图像,超出不检测边缘
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                        new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                        hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                    {
                        continue;
                    }
                    hv_Distance.Dispose();
                    HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                    hv_DetectWidth_COPY_INP_TMP.Dispose();
                    hv_DetectWidth_COPY_INP_TMP = new HTuple(hv_Distance);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_Distance / 2);
                    }
                }
                else
                {
                    //如果有多个测量矩形，产生该测量矩形xld
                    hv_RowC.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1));
                    }
                    hv_ColC.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1));
                    }
                    //判断是否超出图像,超出不检测边缘
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                        new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                        hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                    {
                        continue;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_DetectWidth_COPY_INP_TMP / 2);
                    }
                }

                //把测量矩形xld存储到显示对象
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Regions, ho_Rectangle, out ExpTmpOutVar_0);
                    ho_Regions.Dispose();
                    ho_Regions = ExpTmpOutVar_0;
                }
                if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                {
                    //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                    hv_RowL2.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()
                            ));
                    }
                    hv_RowL1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()
                            ));
                    }
                    hv_ColL2.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()
                            ));
                    }
                    hv_ColL1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()
                            ));
                    }
                    ho_Arrow1.Dispose();
                    gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                        25, 25);
                    //把xld存储到显示对象
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Regions, ho_Arrow1, out ExpTmpOutVar_0);
                        ho_Regions.Dispose();
                        ho_Regions = ExpTmpOutVar_0;
                    }
                }
                //产生测量对象句柄
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_MsrHandle_Measure.Dispose();
                    HOperatorSet.GenMeasureRectangle2(hv_RowC, hv_ColC, hv_ATan, hv_DetectHeight / 2,
                        hv_DetectWidth_COPY_INP_TMP / 2, hv_Width, hv_Height, "nearest_neighbor",
                        out hv_MsrHandle_Measure);
                }

                //设置极性
                if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative"))) != 0)
                {
                    hv_Transition_COPY_INP_TMP.Dispose();
                    hv_Transition_COPY_INP_TMP = "negative";
                }
                else
                {
                    if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive"))) != 0)
                    {

                        hv_Transition_COPY_INP_TMP.Dispose();
                        hv_Transition_COPY_INP_TMP = "positive";
                    }
                    else
                    {
                        hv_Transition_COPY_INP_TMP.Dispose();
                        hv_Transition_COPY_INP_TMP = "all";
                    }
                }
                //设置边缘位置。最强点是从所有边缘中选择幅度绝对值最大点，需要设置为'all'
                if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first"))) != 0)
                {
                    hv_Select_COPY_INP_TMP.Dispose();
                    hv_Select_COPY_INP_TMP = "first";
                }
                else
                {
                    if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last"))) != 0)
                    {

                        hv_Select_COPY_INP_TMP.Dispose();
                        hv_Select_COPY_INP_TMP = "last";
                    }
                    else
                    {
                        hv_Select_COPY_INP_TMP.Dispose();
                        hv_Select_COPY_INP_TMP = "all";
                    }
                }
                //检测边缘
                hv_RowEdge.Dispose(); hv_ColEdge.Dispose(); hv_Amplitude.Dispose(); hv_Distance.Dispose();
                HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold,
                    hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge,
                    out hv_Amplitude, out hv_Distance);
                //清除测量对象句柄
                HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);

                //临时变量初始化
                //tRow，tCol保存找到指定边缘的坐标
                hv_tRow.Dispose();
                hv_tRow = 0;
                hv_tCol.Dispose();
                hv_tCol = 0;
                //t保存边缘的幅度绝对值
                hv_t.Dispose();
                hv_t = 0;
                //找到的边缘必须至少为1个
                hv_Number.Dispose();
                HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                {
                    continue;
                }
                //有多个边缘时，选择幅度绝对值最大的边缘
                HTuple end_val100 = hv_Number - 1;
                HTuple step_val100 = 1;
                for (hv_j = 0; hv_j.Continue(end_val100, step_val100); hv_j = hv_j.TupleAdd(step_val100))
                {
                    if ((int)(new HTuple(((((hv_Amplitude.TupleSelect(hv_j))).TupleAbs())).TupleGreater(
                        hv_t))) != 0)
                    {

                        hv_tRow.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_tRow = hv_RowEdge.TupleSelect(
                                hv_j);
                        }
                        hv_tCol.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_tCol = hv_ColEdge.TupleSelect(
                                hv_j);
                        }
                        hv_t.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_t = ((hv_Amplitude.TupleSelect(
                                hv_j))).TupleAbs();
                        }
                    }
                }
                //把找到的边缘保存在输出数组
                if ((int)(new HTuple(hv_t.TupleGreater(0))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_ResultRow = hv_ResultRow.TupleConcat(
                                hv_tRow);
                            hv_ResultRow.Dispose();
                            hv_ResultRow = ExpTmpLocalVar_ResultRow;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_ResultColumn = hv_ResultColumn.TupleConcat(
                                hv_tCol);
                            hv_ResultColumn.Dispose();
                            hv_ResultColumn = ExpTmpLocalVar_ResultColumn;
                        }
                    }
                }
            }

            ho_RegionLines.Dispose();
            ho_Rectangle.Dispose();
            ho_Arrow1.Dispose();

            hv_DetectWidth_COPY_INP_TMP.Dispose();
            hv_Select_COPY_INP_TMP.Dispose();
            hv_Transition_COPY_INP_TMP.Dispose();
            hv_Width.Dispose();
            hv_Height.Dispose();
            hv_ATan.Dispose();
            hv_i.Dispose();
            hv_RowC.Dispose();
            hv_ColC.Dispose();
            hv_Distance.Dispose();
            hv_RowL2.Dispose();
            hv_RowL1.Dispose();
            hv_ColL2.Dispose();
            hv_ColL1.Dispose();
            hv_MsrHandle_Measure.Dispose();
            hv_RowEdge.Dispose();
            hv_ColEdge.Dispose();
            hv_Amplitude.Dispose();
            hv_tRow.Dispose();
            hv_tCol.Dispose();
            hv_t.Dispose();
            hv_Number.Dispose();
            hv_j.Dispose();

            return;
        }

    }

    public class PortParam
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public StopBits StopBits { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }

        public PortParam(string portName, int baudRate, StopBits stopBits, int dataBits, Parity parity)
        {
            PortName = portName;
            BaudRate = baudRate;
            StopBits = stopBits;
            DataBits = dataBits;
            Parity = parity;
        }
    }
}
