using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alg_KeyBoard_BYD
{
    public class AlgBaseFun
    {
        // Chapter: XLD / Creation
        // Short Description: Creates an arrow shaped XLD contour. 
        public void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
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

        public void get_rectangle2_points(HTuple hv_CenterY, HTuple hv_CenterX, HTuple hv_Phi,
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

            if ((int)(new HTuple(((((hv_Phi - ((new HTuple(90)).TupleRad()))).TupleAbs())).TupleLess(
                ((hv_Phi - ((new HTuple(0)).TupleRad()))).TupleAbs()))) != 0)
            {

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
            }
            else
            {
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

        public void pts_to_best_line(out HObject ho_Line, HTuple hv_Rows, HTuple hv_Cols,
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

        public void rake(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements,
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


        // Local procedures 
        public void MLB_Flex(HObject ho_Image, HObject ho_ROI_0, out HObject ho_ShowRegion,
            out HTuple hv_OutDis)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced, ho_Region, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionUnion, ho_Regions;
            HObject ho_Line, ho_Line1, ho_Contour, ho_Contour1, ho_Cross;
            HObject ho_Region1, ho_RegionFillUp, ho_ImageReduced1, ho_Region2;
            HObject ho_ConnectedRegions1, ho_SelectedRegions1, ho_SortedRegions;
            HObject ho_ObjectSelected, ho_Cross1, ho_Cross2, ho_ObjectSelected1;
            HObject ho_Cross3, ho_Cross4, ho_ObjectsConcat;

            // Local control variables 

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length11 = new HTuple();
            HTuple hv_Length21 = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_CornerY = new HTuple();
            HTuple hv_CornerX = new HTuple(), hv_LineCenterY = new HTuple();
            HTuple hv_LineCenterX = new HTuple(), hv_ResultRow = new HTuple();
            HTuple hv_ResultColumn = new HTuple(), hv_Row11 = new HTuple();
            HTuple hv_Column11 = new HTuple(), hv_Row12 = new HTuple();
            HTuple hv_Column12 = new HTuple(), hv_ResultRow1 = new HTuple();
            HTuple hv_ResultColumn1 = new HTuple(), hv_Row21 = new HTuple();
            HTuple hv_Column21 = new HTuple(), hv_Row22 = new HTuple();
            HTuple hv_Column22 = new HTuple(), hv_InterRow1 = new HTuple();
            HTuple hv_InterColumn1 = new HTuple(), hv_IsParallel = new HTuple();
            HTuple hv_InterRow2 = new HTuple(), hv_InterColumn2 = new HTuple();
            HTuple hv_InterRow3 = new HTuple(), hv_InterColumn3 = new HTuple();
            HTuple hv_InterRow4 = new HTuple(), hv_InterColumn4 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Distance1 = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Min = new HTuple(), hv_PolygonRows = new HTuple();
            HTuple hv_PolygonCols = new HTuple(), hv_Number = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Column3 = new HTuple(), hv_RowProj1 = new HTuple();
            HTuple hv_ColProj1 = new HTuple(), hv_RowProj2 = new HTuple();
            HTuple hv_ColProj2 = new HTuple(), hv_OutDistance1 = new HTuple();
            HTuple hv_Area1 = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple(), hv_RowProj3 = new HTuple();
            HTuple hv_ColProj3 = new HTuple(), hv_RowProj4 = new HTuple();
            HTuple hv_ColProj4 = new HTuple(), hv_OutDistance2 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Line1);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_Cross3);
            HOperatorSet.GenEmptyObj(out ho_Cross4);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            hv_OutDis = new HTuple();
            hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length11.Dispose(); hv_Length21.Dispose();
            HOperatorSet.SmallestRectangle2(ho_ROI_0, out hv_Row, out hv_Column, out hv_Phi,
                out hv_Length11, out hv_Length21);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 160, 255);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "width",
                "and", hv_Length21 / 3, 99999);
            ho_RegionUnion.Dispose();
            HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
            HOperatorSet.SmallestRectangle1(ho_RegionUnion, out hv_Row1, out hv_Column1,
                out hv_Row2, out hv_Column2);
            hv_Length2.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Length2 = (hv_Column2 - hv_Column1) / 2;
            }
            hv_Length1.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Length1 = (hv_Row2 - hv_Row1) / 2;
            }
            //*求得直线边
            //*     get_rectangle2_points (Row, Column, Phi, Length1, Length2, CornerY, CornerX, LineCenterY, LineCenterX)
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
                rake(ho_Image, out ho_Regions, hv_Length1 / 5, hv_Length2 * 2, 15, 1, 20,
                    "all", "max", hv_Row1, hv_Column1, hv_Row2, hv_Column1, out hv_ResultRow,
                    out hv_ResultColumn);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Line.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row12.Dispose(); hv_Column12.Dispose();
                pts_to_best_line(out ho_Line, hv_ResultRow, hv_ResultColumn, hv_Length1 / 20, out hv_Row11,
                    out hv_Column11, out hv_Row12, out hv_Column12);
            }

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow1.Dispose(); hv_ResultColumn1.Dispose();
                rake(ho_Image, out ho_Regions, hv_Length1 / 5, hv_Length2 * 2, 15, 1, 20,
                    "all", "max", hv_Row1, hv_Column2, hv_Row2, hv_Column2, out hv_ResultRow1,
                    out hv_ResultColumn1);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Line1.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose(); hv_Row22.Dispose(); hv_Column22.Dispose();
                pts_to_best_line(out ho_Line1, hv_ResultRow1, hv_ResultColumn1, hv_Length1 / 20,
                    out hv_Row21, out hv_Column21, out hv_Row22, out hv_Column22);
            }

            if ((int)((new HTuple(hv_Row21.TupleEqual(0))).TupleOr(new HTuple(hv_Row11.TupleEqual(
                0)))) != 0)
            {
                ho_ShowRegion.Dispose();
                ho_ShowRegion = new HObject(ho_ROI_0);
                hv_OutDis.Dispose();
                hv_OutDis = new HTuple();
                hv_OutDis[0] = 0;
                hv_OutDis[1] = 0;
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_Regions.Dispose();
                ho_Line.Dispose();
                ho_Line1.Dispose();
                ho_Contour.Dispose();
                ho_Contour1.Dispose();
                ho_Cross.Dispose();
                ho_Region1.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region2.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_Cross3.Dispose();
                ho_Cross4.Dispose();
                ho_ObjectsConcat.Dispose();

                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Phi.Dispose();
                hv_Length11.Dispose();
                hv_Length21.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Length2.Dispose();
                hv_Length1.Dispose();
                hv_CornerY.Dispose();
                hv_CornerX.Dispose();
                hv_LineCenterY.Dispose();
                hv_LineCenterX.Dispose();
                hv_ResultRow.Dispose();
                hv_ResultColumn.Dispose();
                hv_Row11.Dispose();
                hv_Column11.Dispose();
                hv_Row12.Dispose();
                hv_Column12.Dispose();
                hv_ResultRow1.Dispose();
                hv_ResultColumn1.Dispose();
                hv_Row21.Dispose();
                hv_Column21.Dispose();
                hv_Row22.Dispose();
                hv_Column22.Dispose();
                hv_InterRow1.Dispose();
                hv_InterColumn1.Dispose();
                hv_IsParallel.Dispose();
                hv_InterRow2.Dispose();
                hv_InterColumn2.Dispose();
                hv_InterRow3.Dispose();
                hv_InterColumn3.Dispose();
                hv_InterRow4.Dispose();
                hv_InterColumn4.Dispose();
                hv_Distance.Dispose();
                hv_Distance1.Dispose();
                hv_Max.Dispose();
                hv_Min.Dispose();
                hv_PolygonRows.Dispose();
                hv_PolygonCols.Dispose();
                hv_Number.Dispose();
                hv_Area.Dispose();
                hv_Row3.Dispose();
                hv_Column3.Dispose();
                hv_RowProj1.Dispose();
                hv_ColProj1.Dispose();
                hv_RowProj2.Dispose();
                hv_ColProj2.Dispose();
                hv_OutDistance1.Dispose();
                hv_Area1.Dispose();
                hv_Row4.Dispose();
                hv_Column4.Dispose();
                hv_RowProj3.Dispose();
                hv_ColProj3.Dispose();
                hv_RowProj4.Dispose();
                hv_ColProj4.Dispose();
                hv_OutDistance2.Dispose();

                return;
            }
            //*求得交点
            hv_CornerY.Dispose(); hv_CornerX.Dispose(); hv_LineCenterY.Dispose(); hv_LineCenterX.Dispose();
            get_rectangle2_points(hv_Row, hv_Column, hv_Phi, hv_Length11, hv_Length21, out hv_CornerY,
                out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow1.Dispose(); hv_InterColumn1.Dispose(); hv_IsParallel.Dispose();
                HOperatorSet.IntersectionLl(hv_Row11, hv_Column11, hv_Row12, hv_Column12, hv_CornerY.TupleSelect(
                    0), hv_CornerX.TupleSelect(0), hv_CornerY.TupleSelect(1), hv_CornerX.TupleSelect(
                    1), out hv_InterRow1, out hv_InterColumn1, out hv_IsParallel);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow2.Dispose(); hv_InterColumn2.Dispose(); hv_IsParallel.Dispose();
                HOperatorSet.IntersectionLl(hv_Row11, hv_Column11, hv_Row12, hv_Column12, hv_CornerY.TupleSelect(
                    3), hv_CornerX.TupleSelect(3), hv_CornerY.TupleSelect(2), hv_CornerX.TupleSelect(
                    2), out hv_InterRow2, out hv_InterColumn2, out hv_IsParallel);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow3.Dispose(); hv_InterColumn3.Dispose(); hv_IsParallel.Dispose();
                HOperatorSet.IntersectionLl(hv_Row21, hv_Column21, hv_Row22, hv_Column22, hv_CornerY.TupleSelect(
                    0), hv_CornerX.TupleSelect(0), hv_CornerY.TupleSelect(1), hv_CornerX.TupleSelect(
                    1), out hv_InterRow3, out hv_InterColumn3, out hv_IsParallel);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow4.Dispose(); hv_InterColumn4.Dispose(); hv_IsParallel.Dispose();
                HOperatorSet.IntersectionLl(hv_Row21, hv_Column21, hv_Row22, hv_Column22, hv_CornerY.TupleSelect(
                    3), hv_CornerX.TupleSelect(3), hv_CornerY.TupleSelect(2), hv_CornerX.TupleSelect(
                    2), out hv_InterRow4, out hv_InterColumn4, out hv_IsParallel);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_InterRow1.TupleConcat(hv_InterRow2),
                    hv_InterColumn1.TupleConcat(hv_InterColumn2));
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Contour1.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_InterRow3.TupleConcat(hv_InterRow4),
                    hv_InterColumn3.TupleConcat(hv_InterColumn4));
            }

            hv_Distance.Dispose();
            HOperatorSet.DistancePp(hv_InterRow1, hv_InterColumn1, hv_InterRow3, hv_InterColumn3,
                out hv_Distance);
            hv_Distance1.Dispose();
            HOperatorSet.DistancePp(hv_InterRow2, hv_InterColumn2, hv_InterRow4, hv_InterColumn4,
                out hv_Distance1);
            hv_Max.Dispose();
            HOperatorSet.TupleMax2(hv_Distance, hv_Distance1, out hv_Max);
            hv_Min.Dispose();
            HOperatorSet.TupleMin2(hv_Distance, hv_Distance1, out hv_Min);
            //*取得pin针区域-筛选最开始 最终pin
            hv_PolygonRows.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_PolygonRows = new HTuple();
                hv_PolygonRows = hv_PolygonRows.TupleConcat(hv_InterRow1, hv_InterRow3, hv_InterRow4, hv_InterRow2, hv_InterRow1);
            }
            hv_PolygonCols.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_PolygonCols = new HTuple();
                hv_PolygonCols = hv_PolygonCols.TupleConcat(hv_InterColumn1, hv_InterColumn3, hv_InterColumn4, hv_InterColumn2, hv_InterColumn1);
            }
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_PolygonRows, hv_PolygonCols,
                36, 0.785398);
            ho_Region1.Dispose();
            HOperatorSet.GenRegionPolygon(out ho_Region1, hv_PolygonRows, hv_PolygonCols);
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_Region1, out ho_RegionFillUp);
            ho_ImageReduced1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionFillUp, out ho_ImageReduced1);
            ho_Region2.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region2, 200, 255);
            ho_ConnectedRegions1.Dispose();
            HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions1);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_SelectedRegions1.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, "width",
                    "and", hv_Min * 0.8, hv_Max * 1.2);
            }
            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_SelectedRegions1, out ho_SortedRegions, "character",
                "true", "row");
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SortedRegions, out hv_Number);
            //*计算距离
            ho_ObjectSelected.Dispose();
            HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected, 1);
            hv_Area.Dispose(); hv_Row3.Dispose(); hv_Column3.Dispose();
            HOperatorSet.AreaCenter(ho_ObjectSelected, out hv_Area, out hv_Row3, out hv_Column3);

            hv_RowProj1.Dispose(); hv_ColProj1.Dispose();
            HOperatorSet.ProjectionPl(hv_Row3, hv_Column3, hv_InterRow1, hv_InterColumn1,
                hv_InterRow2, hv_InterColumn2, out hv_RowProj1, out hv_ColProj1);
            hv_RowProj2.Dispose(); hv_ColProj2.Dispose();
            HOperatorSet.ProjectionPl(hv_Row3, hv_Column3, hv_InterRow3, hv_InterColumn3,
                hv_InterRow4, hv_InterColumn4, out hv_RowProj2, out hv_ColProj2);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_RowProj1, hv_ColProj1, 36,
                    (new HTuple(45)).TupleRad());
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross2.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_RowProj2, hv_ColProj2, 36,
                    (new HTuple(45)).TupleRad());
            }
            hv_OutDistance1.Dispose();
            HOperatorSet.DistancePp(hv_RowProj1, hv_ColProj1, hv_RowProj2, hv_ColProj2, out hv_OutDistance1);

            ho_ObjectSelected1.Dispose();
            HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected1, hv_Number);
            hv_Area1.Dispose(); hv_Row4.Dispose(); hv_Column4.Dispose();
            HOperatorSet.AreaCenter(ho_ObjectSelected1, out hv_Area1, out hv_Row4, out hv_Column4);
            hv_RowProj3.Dispose(); hv_ColProj3.Dispose();
            HOperatorSet.ProjectionPl(hv_Row4, hv_Column4, hv_InterRow1, hv_InterColumn1,
                hv_InterRow2, hv_InterColumn2, out hv_RowProj3, out hv_ColProj3);
            hv_RowProj4.Dispose(); hv_ColProj4.Dispose();
            HOperatorSet.ProjectionPl(hv_Row4, hv_Column4, hv_InterRow3, hv_InterColumn3,
                hv_InterRow4, hv_InterColumn4, out hv_RowProj4, out hv_ColProj4);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross3.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_RowProj3, hv_ColProj3, 36,
                    (new HTuple(45)).TupleRad());
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross4.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_RowProj4, hv_ColProj4, 36,
                    (new HTuple(45)).TupleRad());
            }
            hv_OutDistance2.Dispose();
            HOperatorSet.DistancePp(hv_RowProj3, hv_ColProj3, hv_RowProj4, hv_ColProj4, out hv_OutDistance2);
            hv_OutDis.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_OutDis = new HTuple();
                hv_OutDis = hv_OutDis.TupleConcat(hv_OutDistance1, hv_OutDistance2);
            }

            ho_ObjectsConcat.Dispose();
            HOperatorSet.ConcatObj(ho_Contour, ho_Contour1, out ho_ObjectsConcat);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross1, out ExpTmpOutVar_0);
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross2, out ExpTmpOutVar_0);
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross3, out ExpTmpOutVar_0);
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat = ExpTmpOutVar_0;
            }
            ho_ShowRegion.Dispose();
            HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross4, out ho_ShowRegion);
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionUnion.Dispose();
            ho_Regions.Dispose();
            ho_Line.Dispose();
            ho_Line1.Dispose();
            ho_Contour.Dispose();
            ho_Contour1.Dispose();
            ho_Cross.Dispose();
            ho_Region1.Dispose();
            ho_RegionFillUp.Dispose();
            ho_ImageReduced1.Dispose();
            ho_Region2.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_SortedRegions.Dispose();
            ho_ObjectSelected.Dispose();
            ho_Cross1.Dispose();
            ho_Cross2.Dispose();
            ho_ObjectSelected1.Dispose();
            ho_Cross3.Dispose();
            ho_Cross4.Dispose();
            ho_ObjectsConcat.Dispose();

            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Phi.Dispose();
            hv_Length11.Dispose();
            hv_Length21.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Length2.Dispose();
            hv_Length1.Dispose();
            hv_CornerY.Dispose();
            hv_CornerX.Dispose();
            hv_LineCenterY.Dispose();
            hv_LineCenterX.Dispose();
            hv_ResultRow.Dispose();
            hv_ResultColumn.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row12.Dispose();
            hv_Column12.Dispose();
            hv_ResultRow1.Dispose();
            hv_ResultColumn1.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_Row22.Dispose();
            hv_Column22.Dispose();
            hv_InterRow1.Dispose();
            hv_InterColumn1.Dispose();
            hv_IsParallel.Dispose();
            hv_InterRow2.Dispose();
            hv_InterColumn2.Dispose();
            hv_InterRow3.Dispose();
            hv_InterColumn3.Dispose();
            hv_InterRow4.Dispose();
            hv_InterColumn4.Dispose();
            hv_Distance.Dispose();
            hv_Distance1.Dispose();
            hv_Max.Dispose();
            hv_Min.Dispose();
            hv_PolygonRows.Dispose();
            hv_PolygonCols.Dispose();
            hv_Number.Dispose();
            hv_Area.Dispose();
            hv_Row3.Dispose();
            hv_Column3.Dispose();
            hv_RowProj1.Dispose();
            hv_ColProj1.Dispose();
            hv_RowProj2.Dispose();
            hv_ColProj2.Dispose();
            hv_OutDistance1.Dispose();
            hv_Area1.Dispose();
            hv_Row4.Dispose();
            hv_Column4.Dispose();
            hv_RowProj3.Dispose();
            hv_ColProj3.Dispose();
            hv_RowProj4.Dispose();
            hv_ColProj4.Dispose();
            hv_OutDistance2.Dispose();

            return;
        }


        public void MLB_ConnectorBuckle(HObject ho_Image, HObject ho_ROI, out HObject ho_ShowRegion,
    out HObject ho_OutImage, out HTuple hv_result)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageMean = null, ho_ImageReduced = null;
            HObject ho_Region = null, ho_RegionClosing = null, ho_ConnectedRegions1 = null;
            HObject ho_SelectedRegions1 = null, ho_SortedRegions = null;
            HObject ho_ObjectSelected = null, ho_Rectangle = null, ho_ImageReduced1 = null;
            HObject ho_Region2 = null, ho_ConnectedRegions = null, ho_SelectedRegions = null;
            HObject ho_RegionUnion = null, ho_RegionClosing1 = null, ho_SelectedRegions4 = null;
            HObject ho_RegionOpening = null, ho_RegionClosing2 = null;

            // Local control variables 

            HTuple hv_Distances = new HTuple(), hv_Mean = new HTuple();
            HTuple hv_Deviation = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_OutImage);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions4);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing2);
            hv_result = new HTuple();
            try
            {
                try
                {
                    hv_result.Dispose();
                    hv_result = 0;
                    hv_Distances.Dispose();
                    hv_Distances = new HTuple();
                    ho_ShowRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ShowRegion);

                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_Image, out ho_ImageMean, 9, 9);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageMean, ho_ROI, out ho_ImageReduced);
                    hv_Mean.Dispose(); hv_Deviation.Dispose();
                    HOperatorSet.Intensity(ho_ROI, ho_ImageReduced, out hv_Mean, out hv_Deviation);
                    if ((int)(new HTuple(hv_Mean.TupleLess(100))) != 0)
                    {
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 220, 255);
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_Region, out ho_RegionClosing, 10, 50);
                        ho_ConnectedRegions1.Dispose();
                        HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions1);
                        ho_SelectedRegions1.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1,
                            (new HTuple("area")).TupleConcat("height"), "and", (new HTuple(20000)).TupleConcat(
                            1000), (new HTuple(999999)).TupleConcat(9999));
                        ho_SortedRegions.Dispose();
                        HOperatorSet.SortRegion(ho_SelectedRegions1, out ho_SortedRegions, "first_point",
                            "false", "column");
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected, 1);
                        hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                        HOperatorSet.SmallestRectangle1(ho_ObjectSelected, out hv_Row1, out hv_Column1,
                            out hv_Row2, out hv_Column2);
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1 - 360, hv_Row2,
                                hv_Column1 - 280);
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ShowRegion, ho_Rectangle, out ExpTmpOutVar_0);
                            ho_ShowRegion.Dispose();
                            ho_ShowRegion = ExpTmpOutVar_0;
                        }
                        ho_ImageReduced1.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageReduced, ho_Rectangle, out ho_ImageReduced1
                            );
                        ho_Region2.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region2, 120, 255);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions);
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("width")).TupleConcat(
                            "height"), "and", (new HTuple(20)).TupleConcat(5), (new HTuple(99999)).TupleConcat(
                            50));
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                        ho_RegionClosing1.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing1, 10,
                            80);
                        ho_SelectedRegions4.Dispose();
                        HOperatorSet.SelectShape(ho_RegionClosing1, out ho_SelectedRegions4, "area",
                            "and", 20000, 999999);
                    }
                    else
                    {
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 220, 255);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
                        ho_RegionOpening.Dispose();
                        HOperatorSet.OpeningRectangle1(ho_ConnectedRegions, out ho_RegionOpening,
                            10, 50);
                        ho_RegionClosing2.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_RegionOpening, out ho_RegionClosing2,
                            10, 30);
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_RegionClosing2, out ho_SelectedRegions, (new HTuple("area")).TupleConcat(
                            "column"), "and", (new HTuple(5000)).TupleConcat(500), (new HTuple(999999)).TupleConcat(
                            1500));
                        ho_ConnectedRegions1.Dispose();
                        HOperatorSet.Connection(ho_SelectedRegions, out ho_ConnectedRegions1);
                        ho_SelectedRegions1.Dispose();
                        HOperatorSet.SelectShapeStd(ho_ConnectedRegions1, out ho_SelectedRegions1,
                            "max_area", 70);
                        hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                        HOperatorSet.SmallestRectangle1(ho_SelectedRegions1, out hv_Row1, out hv_Column1,
                            out hv_Row2, out hv_Column2);
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_Rectangle.Dispose();
                            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1 - 200, hv_Row2,
                                hv_Column1 - 120);
                        }

                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ShowRegion, ho_Rectangle, out ExpTmpOutVar_0);
                            ho_ShowRegion.Dispose();
                            ho_ShowRegion = ExpTmpOutVar_0;
                        }
                        ho_ImageReduced1.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageReduced, ho_Rectangle, out ho_ImageReduced1
                            );
                        ho_Region2.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region2, 220, 255);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions);
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("width")).TupleConcat(
                            "height"), "and", (new HTuple(20)).TupleConcat(5), (new HTuple(99999)).TupleConcat(
                            50));
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                        ho_RegionClosing1.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing1, 10,
                            80);
                        ho_SelectedRegions4.Dispose();
                        HOperatorSet.SelectShape(ho_RegionClosing1, out ho_SelectedRegions4, "area",
                            "and", 20000, 999999);
                    }


                    hv_Number1.Dispose();
                    HOperatorSet.CountObj(ho_SelectedRegions4, out hv_Number1);
                    if ((int)(new HTuple(hv_Number1.TupleGreater(0))) != 0)
                    {
                        hv_result.Dispose();
                        hv_result = 1;
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Distances.Dispose();
                    hv_Distances = new HTuple();
                    hv_Distances[0] = 0;
                    hv_Distances[1] = 0;
                    hv_result.Dispose();
                    hv_result = 1;
                }
                ho_ImageMean.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region2.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing1.Dispose();
                ho_SelectedRegions4.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionClosing2.Dispose();

                hv_Distances.Dispose();
                hv_Mean.Dispose();
                hv_Deviation.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Number1.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageMean.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region2.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing1.Dispose();
                ho_SelectedRegions4.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionClosing2.Dispose();

                hv_Distances.Dispose();
                hv_Mean.Dispose();
                hv_Deviation.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Number1.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void FindLine(HObject ho_Image, out HObject ho_RegionLines, HTuple hv_position,
            HTuple hv_minLength, out HTuple hv_RowBegin, out HTuple hv_ColBegin, out HTuple hv_RowEnd,
            out HTuple hv_ColEnd)
        {




            // Local iconic variables 

            HObject ho_Edges = null, ho_ContoursSplit = null;
            HObject ho_SelectedXLD = null, ho_UnionContours = null;

            // Local control variables 

            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_SelectedXLD);
            HOperatorSet.GenEmptyObj(out ho_UnionContours);
            hv_RowBegin = new HTuple();
            hv_ColBegin = new HTuple();
            hv_RowEnd = new HTuple();
            hv_ColEnd = new HTuple();
            try
            {
                try
                {
                    //emphasize (Image, ImageEmphasize, 7, 7, 1)

                    ho_Edges.Dispose();
                    HOperatorSet.EdgesSubPix(ho_Image, out ho_Edges, "canny", 2, 20, 40);
                    //    selectMaxLine (Edges, MaxLengthContour)
                    //    segment_contours_xld (MaxLengthContour, ContoursSplit, 'lines_circles',1, 4, 1)
                    if ((int)(new HTuple(hv_position.TupleEqual("vertical"))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_SelectedXLD.Dispose();
                            HOperatorSet.SelectShapeXld(ho_ContoursSplit, out ho_SelectedXLD, (new HTuple("phi_points")).TupleConcat(
                                "height"), "and", (new HTuple(1.4)).TupleConcat(hv_minLength), (new HTuple(1.6)).TupleConcat(
                                9999));
                        }
                    }
                    else
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_SelectedXLD.Dispose();
                            HOperatorSet.SelectShapeXld(ho_ContoursSplit, out ho_SelectedXLD, (new HTuple("phi_points")).TupleConcat(
                                "width"), "and", (new HTuple(-0.2)).TupleConcat(hv_minLength), (new HTuple(0.2)).TupleConcat(
                                9999));
                        }
                    }
                    ho_UnionContours.Dispose();
                    HOperatorSet.UnionCollinearContoursXld(ho_SelectedXLD, out ho_UnionContours,
                        100, 1, 2, 0.1, "attr_keep");
                    //union_adjacent_contours_xld (SelectedXLD, UnionContours, 100, 1, 'attr_keep')
                    //    selectMaxLine (UnionContours, MaxLengthContour1)
                    //    fit_line_contour_xld (MaxLengthContour1, 'tukey', -1, 0, 5, 2, RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist)
                    ho_RegionLines.Dispose();
                    HOperatorSet.GenRegionLine(out ho_RegionLines, hv_RowBegin, hv_ColBegin,
                        hv_RowEnd, hv_ColEnd);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                ho_Edges.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedXLD.Dispose();
                ho_UnionContours.Dispose();

                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Edges.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedXLD.Dispose();
                ho_UnionContours.Dispose();

                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Local procedures 
        public void Flex(HObject ho_Image, HObject ho_InputRegion, out HObject ho_ShowRegion,
            out HObject ho_OutImage, HTuple hv_IsPowerFlex, out HTuple hv_Value)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Regions, ho_Line, ho_Line1, ho_RegionLines;
            HObject ho_Region, ho_RegionFillUp, ho_RegionDilation, ho_ImageReduced, ho_ImageResult, ho_ImageOpening;
            HObject ho_Region1, ho_ConnectedRegions, ho_SelectedRegions;
            HObject ho_SortedRegions, ho_Cross1 = null, ho_Cross2 = null;
            HObject ho_Cross3 = null, ho_Cross4 = null, ho_ObjectsConcat = null;

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_CornerY = new HTuple();
            HTuple hv_CornerX = new HTuple(), hv_LineCenterY = new HTuple();
            HTuple hv_LineCenterX = new HTuple(), hv_ResultRow = new HTuple();
            HTuple hv_ResultColumn = new HTuple(), hv_Row11 = new HTuple();
            HTuple hv_Column11 = new HTuple(), hv_Row12 = new HTuple();
            HTuple hv_Column12 = new HTuple(), hv_ResultRow1 = new HTuple();
            HTuple hv_ResultColumn1 = new HTuple(), hv_Row21 = new HTuple();
            HTuple hv_Column21 = new HTuple(), hv_Row22 = new HTuple();
            HTuple hv_Column22 = new HTuple(), hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple(), hv_Distance2 = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Min = new HTuple(), hv_RegionRows = new HTuple();
            HTuple hv_RegionCols = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RowProj1 = new HTuple(), hv_ColProj1 = new HTuple();
            HTuple hv_RowProj2 = new HTuple(), hv_ColProj2 = new HTuple();
            HTuple hv_RowProj3 = new HTuple(), hv_ColProj3 = new HTuple();
            HTuple hv_RowProj4 = new HTuple(), hv_ColProj4 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_OutImage);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Line1);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Cross3);
            HOperatorSet.GenEmptyObj(out ho_Cross4);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            hv_Value = new HTuple();

            ho_ShowRegion.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            hv_Value.Dispose();
            hv_Value = new HTuple();

            hv_Value.Dispose();
            HOperatorSet.GrayFeatures(ho_InputRegion, ho_Image, "mean", out hv_Value);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_ImageResult.Dispose();
                HOperatorSet.MultImage(ho_Image, ho_Image, out ho_ImageResult, 255 / (hv_Value * hv_Value),
                    0);
            }
            ho_ImageOpening.Dispose();
            //HOperatorSet.GrayOpeningRect(ho_ImageResult, out ho_ImageOpening, 11, 1);
            //mult_image (Image, Image, OutImage, 0.005, 0)
            //rotate_image (Image, ImageRotate, 90, 'constant')
            if (HDevWindowStack.IsOpen())
            {
                //dev_display (ImageResult)
            }
            hv_Width.Dispose(); hv_Height.Dispose();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //gen_rectangle1 (ROI_0, 736.064, 1401.01, 1137.24, 1551.21)
            //*拟合直线
            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
            HOperatorSet.SmallestRectangle2(ho_InputRegion, out hv_Row1, out hv_Column1,
                out hv_Phi, out hv_Length1, out hv_Length2);
            hv_CornerY.Dispose(); hv_CornerX.Dispose(); hv_LineCenterY.Dispose(); hv_LineCenterX.Dispose();
            get_rectangle2_points(hv_Row1, hv_Column1, hv_Phi, hv_Length1, hv_Length2, out hv_CornerY,
                out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
                rake(ho_ImageResult, out ho_Regions, hv_Length1 / 2, hv_Length2 * 1.0, 20, 1, 30, "all",
                    "max", hv_CornerY.TupleSelect(0), hv_CornerX.TupleSelect(0), hv_CornerY.TupleSelect(3),
                    hv_CornerX.TupleSelect(3), out hv_ResultRow, out hv_ResultColumn);
            }
            ho_Line.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row12.Dispose(); hv_Column12.Dispose();
            pts_to_best_line(out ho_Line, hv_ResultRow, hv_ResultColumn, hv_Length1 / 10, out hv_Row11,
                out hv_Column11, out hv_Row12, out hv_Column12);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow1.Dispose(); hv_ResultColumn1.Dispose();
                rake(ho_Image, out ho_Regions, hv_Length1 / 2, hv_Length2 * 1.0, 20, 1, 30, "negative",
                    "last", hv_CornerY.TupleSelect(1), hv_CornerX.TupleSelect(1), hv_CornerY.TupleSelect(2),
                    hv_CornerX.TupleSelect(2), out hv_ResultRow1, out hv_ResultColumn1);
            }
            ho_Line1.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose(); hv_Row22.Dispose(); hv_Column22.Dispose();
            pts_to_best_line(out ho_Line1, hv_ResultRow1, hv_ResultColumn1, hv_Length1 / 10, out hv_Row21,
                out hv_Column21, out hv_Row22, out hv_Column22);

            //*计算区域筛选范围
            ho_RegionLines.Dispose();
            HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Row11, hv_Column11, hv_Row12,
                hv_Column12);
            hv_Rows.Dispose(); hv_Columns.Dispose();
            HOperatorSet.GetRegionPoints(ho_RegionLines, out hv_Rows, out hv_Columns);
            hv_Distance2.Dispose();
            HOperatorSet.DistancePl(hv_Rows, hv_Columns, hv_Row21, hv_Column21, hv_Row22,
                hv_Column22, out hv_Distance2);
            hv_Max.Dispose();
            HOperatorSet.TupleMax(hv_Distance2, out hv_Max);
            hv_Min.Dispose();
            HOperatorSet.TupleMin(hv_Distance2, out hv_Min);

            if (HDevWindowStack.IsOpen())
            {
                //dev_display (Line)
            }
            if (HDevWindowStack.IsOpen())
            {
                //dev_display (Line1)
            }
            //*闭合区域
            hv_RegionRows.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RegionRows = new HTuple();
                hv_RegionRows = hv_RegionRows.TupleConcat(hv_Row11, hv_Row12, hv_Row22, hv_Row21, hv_Row11);
            }
            hv_RegionCols.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RegionCols = new HTuple();
                hv_RegionCols = hv_RegionCols.TupleConcat(hv_Column11, hv_Column12, hv_Column22, hv_Column21, hv_Column11);
            }
            ho_Region.Dispose();
            HOperatorSet.GenRegionPolygon(out ho_Region, hv_RegionRows, hv_RegionCols);

            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
            ho_RegionDilation.Dispose();
            HOperatorSet.DilationRectangle1(ho_RegionFillUp, out ho_RegionDilation, 5, 3);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionDilation, out ho_ImageReduced);
            ho_Region1.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region1, 200, 255);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region1, out ho_ConnectedRegions);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "width",
                    "and", hv_Min * 0.5, hv_Max * 1.5);
            }
            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "first_point",
                "true", "row");
            hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);
            if ((int)(new HTuple((new HTuple(hv_Row.TupleLength())).TupleGreater(2))) != 0)
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj1.Dispose(); hv_ColProj1.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect(0), hv_Column.TupleSelect(0),
                        hv_Row21, hv_Column21, hv_Row22, hv_Column22, out hv_RowProj1, out hv_ColProj1);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj2.Dispose(); hv_ColProj2.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect(0), hv_Column.TupleSelect(0),
                        hv_Row11, hv_Column11, hv_Row12, hv_Column12, out hv_RowProj2, out hv_ColProj2);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross1.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_RowProj1, hv_ColProj1, 16,
                        (new HTuple(45)).TupleRad());
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross2.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_RowProj2, hv_ColProj2, 16,
                        (new HTuple(45)).TupleRad());
                }

                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj3.Dispose(); hv_ColProj3.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect((new HTuple(hv_Row.TupleLength()
                        )) - 1), hv_Column.TupleSelect((new HTuple(hv_Row.TupleLength())) - 1), hv_Row21,
                        hv_Column21, hv_Row22, hv_Column22, out hv_RowProj3, out hv_ColProj3);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj4.Dispose(); hv_ColProj4.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect((new HTuple(hv_Row.TupleLength()
                        )) - 1), hv_Column.TupleSelect((new HTuple(hv_Row.TupleLength())) - 1), hv_Row11,
                        hv_Column11, hv_Row12, hv_Column12, out hv_RowProj4, out hv_ColProj4);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross3.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_RowProj3, hv_ColProj3, 16,
                        (new HTuple(45)).TupleRad());
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross4.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_RowProj4, hv_ColProj4, 16,
                        (new HTuple(45)).TupleRad());
                }

                hv_Distance.Dispose();
                HOperatorSet.DistancePp(hv_RowProj1, hv_ColProj1, hv_RowProj2, hv_ColProj2,
                    out hv_Distance);
                hv_Distance1.Dispose();
                HOperatorSet.DistancePp(hv_RowProj3, hv_ColProj3, hv_RowProj4, hv_ColProj4,
                    out hv_Distance1);


                ho_ObjectsConcat.Dispose();
                HOperatorSet.ConcatObj(ho_Line, ho_Cross1, out ho_ObjectsConcat);
                //concat_obj (ObjectsConcat, Cross1, ObjectsConcat)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross2, out ExpTmpOutVar_0);
                    ho_ObjectsConcat.Dispose();
                    ho_ObjectsConcat = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross3, out ExpTmpOutVar_0);
                    ho_ObjectsConcat.Dispose();
                    ho_ObjectsConcat = ExpTmpOutVar_0;
                }
                ho_ShowRegion.Dispose();
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross4, out ho_ShowRegion);

                if (hv_Value == null)
                    hv_Value = new HTuple();
                hv_Value[0] = hv_Distance;
                if (hv_Value == null)
                    hv_Value = new HTuple();
                hv_Value[1] = hv_Distance1;
            }
            else
            {
                ho_ShowRegion.Dispose();
                ho_ShowRegion = new HObject(ho_InputRegion);
            }
            ho_Regions.Dispose();
            ho_Line.Dispose();
            ho_Line1.Dispose();
            ho_RegionLines.Dispose();
            ho_Region.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionDilation.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region1.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SortedRegions.Dispose();
            ho_Cross1.Dispose();
            ho_Cross2.Dispose();
            ho_Cross3.Dispose();
            ho_Cross4.Dispose();
            ho_ObjectsConcat.Dispose();

            hv_Width.Dispose();
            hv_Height.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Phi.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
            hv_CornerY.Dispose();
            hv_CornerX.Dispose();
            hv_LineCenterY.Dispose();
            hv_LineCenterX.Dispose();
            hv_ResultRow.Dispose();
            hv_ResultColumn.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row12.Dispose();
            hv_Column12.Dispose();
            hv_ResultRow1.Dispose();
            hv_ResultColumn1.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_Row22.Dispose();
            hv_Column22.Dispose();
            hv_Rows.Dispose();
            hv_Columns.Dispose();
            hv_Distance2.Dispose();
            hv_Max.Dispose();
            hv_Min.Dispose();
            hv_RegionRows.Dispose();
            hv_RegionCols.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_RowProj1.Dispose();
            hv_ColProj1.Dispose();
            hv_RowProj2.Dispose();
            hv_ColProj2.Dispose();
            hv_RowProj3.Dispose();
            hv_ColProj3.Dispose();
            hv_RowProj4.Dispose();
            hv_ColProj4.Dispose();
            hv_Distance.Dispose();
            hv_Distance1.Dispose();

            return;
        }
        // Local procedures 
        public void flex(HObject ho_Image, HObject ho_InputRegion, out HObject ho_ShowRegion,
            out HObject ho_OutImage, HTuple hv_IsPowerFlex, out HTuple hv_Value)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Regions, ho_Line, ho_Line1, ho_RegionLines;
            HObject ho_Region, ho_RegionFillUp, ho_RegionDilation, ho_ImageReduced;
            HObject ho_Region1, ho_ConnectedRegions, ho_SelectedRegions;
            HObject ho_SortedRegions, ho_Cross1, ho_Cross2, ho_Cross3;
            HObject ho_Cross4, ho_ObjectsConcat;

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_CornerY = new HTuple();
            HTuple hv_CornerX = new HTuple(), hv_LineCenterY = new HTuple();
            HTuple hv_LineCenterX = new HTuple(), hv_ResultRow = new HTuple();
            HTuple hv_ResultColumn = new HTuple(), hv_Row11 = new HTuple();
            HTuple hv_Column11 = new HTuple(), hv_Row12 = new HTuple();
            HTuple hv_Column12 = new HTuple(), hv_ResultRow1 = new HTuple();
            HTuple hv_ResultColumn1 = new HTuple(), hv_Row21 = new HTuple();
            HTuple hv_Column21 = new HTuple(), hv_Row22 = new HTuple();
            HTuple hv_Column22 = new HTuple(), hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple(), hv_Distance2 = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Min = new HTuple(), hv_RegionRows = new HTuple();
            HTuple hv_RegionCols = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_RowProj1 = new HTuple(), hv_ColProj1 = new HTuple();
            HTuple hv_RowProj2 = new HTuple(), hv_ColProj2 = new HTuple();
            HTuple hv_RowProj3 = new HTuple(), hv_ColProj3 = new HTuple();
            HTuple hv_RowProj4 = new HTuple(), hv_ColProj4 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_OutImage);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Line1);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Cross3);
            HOperatorSet.GenEmptyObj(out ho_Cross4);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            hv_Value = new HTuple();

            ho_ShowRegion.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            hv_Value.Dispose();
            hv_Value = new HTuple();
            //mult_image (Image, Image, OutImage, 0.005, 0)
            //rotate_image (Image, ImageRotate, 90, 'constant')
            if (HDevWindowStack.IsOpen())
            {
                //dev_display (ImageResult)
            }
            hv_Width.Dispose(); hv_Height.Dispose();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //gen_rectangle1 (ROI_0, 736.064, 1401.01, 1137.24, 1551.21)
            //*拟合直线
            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
            HOperatorSet.SmallestRectangle2(ho_InputRegion, out hv_Row1, out hv_Column1,
                out hv_Phi, out hv_Length1, out hv_Length2);
            hv_CornerY.Dispose(); hv_CornerX.Dispose(); hv_LineCenterY.Dispose(); hv_LineCenterX.Dispose();
            get_rectangle2_points(hv_Row1, hv_Column1, hv_Phi, hv_Length1, hv_Length2, out hv_CornerY,
                out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
                rake(ho_Image, out ho_Regions, hv_Length1 / 5, hv_Length2 * 1.5, 10, 1, 30, "all",
                    "first", hv_LineCenterY.TupleSelect(0), hv_LineCenterX.TupleSelect(0), hv_LineCenterY.TupleSelect(
                    2), hv_LineCenterX.TupleSelect(2), out hv_ResultRow, out hv_ResultColumn);
            }
            ho_Line.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row12.Dispose(); hv_Column12.Dispose();
            pts_to_best_line(out ho_Line, hv_ResultRow, hv_ResultColumn, 2, out hv_Row11,
                out hv_Column11, out hv_Row12, out hv_Column12);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow1.Dispose(); hv_ResultColumn1.Dispose();
                rake(ho_Image, out ho_Regions, hv_Length1 / 5, hv_Length2 * 1.5, 10, 1, 30, "all",
                    "last", hv_LineCenterY.TupleSelect(0), hv_LineCenterX.TupleSelect(0), hv_LineCenterY.TupleSelect(
                    2), hv_LineCenterX.TupleSelect(2), out hv_ResultRow1, out hv_ResultColumn1);
            }
            ho_Line1.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose(); hv_Row22.Dispose(); hv_Column22.Dispose();
            pts_to_best_line(out ho_Line1, hv_ResultRow1, hv_ResultColumn1, 2, out hv_Row21,
                out hv_Column21, out hv_Row22, out hv_Column22);

            //*计算区域筛选范围
            ho_RegionLines.Dispose();
            HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Row11, hv_Column11, hv_Row12,
                hv_Column12);
            hv_Rows.Dispose(); hv_Columns.Dispose();
            HOperatorSet.GetRegionPoints(ho_RegionLines, out hv_Rows, out hv_Columns);
            hv_Distance2.Dispose();
            HOperatorSet.DistancePl(hv_Rows, hv_Columns, hv_Row21, hv_Column21, hv_Row22,
                hv_Column22, out hv_Distance2);
            hv_Max.Dispose();
            HOperatorSet.TupleMax(hv_Distance2, out hv_Max);
            hv_Min.Dispose();
            HOperatorSet.TupleMin(hv_Distance2, out hv_Min);

            if (HDevWindowStack.IsOpen())
            {
                //dev_display (Line)
            }
            if (HDevWindowStack.IsOpen())
            {
                //dev_display (Line1)
            }
            //*闭合区域
            hv_RegionRows.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RegionRows = new HTuple();
                hv_RegionRows = hv_RegionRows.TupleConcat(hv_Row11, hv_Row12, hv_Row22, hv_Row21, hv_Row11);
            }
            hv_RegionCols.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RegionCols = new HTuple();
                hv_RegionCols = hv_RegionCols.TupleConcat(hv_Column11, hv_Column12, hv_Column22, hv_Column21, hv_Column11);
            }
            ho_Region.Dispose();
            HOperatorSet.GenRegionPolygon(out ho_Region, hv_RegionRows, hv_RegionCols);

            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
            ho_RegionDilation.Dispose();
            HOperatorSet.DilationRectangle1(ho_RegionFillUp, out ho_RegionDilation, 5, 1);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionDilation, out ho_ImageReduced);
            ho_Region1.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region1, 220, 255);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region1, out ho_ConnectedRegions);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "width",
                    "and", hv_Min * 0.5, hv_Max * 1.5);
            }
            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "first_point",
                "true", "row");
            hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);
            if (hv_Row.Length > 2)
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj1.Dispose(); hv_ColProj1.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect(0), hv_Column.TupleSelect(0), hv_Row21,
                        hv_Column21, hv_Row22, hv_Column22, out hv_RowProj1, out hv_ColProj1);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj2.Dispose(); hv_ColProj2.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect(0), hv_Column.TupleSelect(0), hv_Row11,
                        hv_Column11, hv_Row12, hv_Column12, out hv_RowProj2, out hv_ColProj2);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross1.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_RowProj1, hv_ColProj1, 26,
                        (new HTuple(45)).TupleRad());
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross2.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_RowProj2, hv_ColProj2, 26,
                        (new HTuple(45)).TupleRad());
                }

                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj3.Dispose(); hv_ColProj3.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect((new HTuple(hv_Row.TupleLength()
                        )) - 1), hv_Column.TupleSelect((new HTuple(hv_Row.TupleLength())) - 1), hv_Row21,
                        hv_Column21, hv_Row22, hv_Column22, out hv_RowProj3, out hv_ColProj3);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowProj4.Dispose(); hv_ColProj4.Dispose();
                    HOperatorSet.ProjectionPl(hv_Row.TupleSelect((new HTuple(hv_Row.TupleLength()
                        )) - 1), hv_Column.TupleSelect((new HTuple(hv_Row.TupleLength())) - 1), hv_Row11,
                        hv_Column11, hv_Row12, hv_Column12, out hv_RowProj4, out hv_ColProj4);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross3.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_RowProj3, hv_ColProj3, 26,
                        (new HTuple(45)).TupleRad());
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross4.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_RowProj4, hv_ColProj4, 26,
                        (new HTuple(45)).TupleRad());
                }

                hv_Distance.Dispose();
                HOperatorSet.DistancePp(hv_RowProj1, hv_ColProj1, hv_RowProj2, hv_ColProj2, out hv_Distance);
                hv_Distance1.Dispose();
                HOperatorSet.DistancePp(hv_RowProj3, hv_ColProj3, hv_RowProj4, hv_ColProj4, out hv_Distance1);


                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (OutImage)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (Cross1)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (Cross2)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (Cross3)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (Cross4)
                }
                ho_ObjectsConcat.Dispose();
                HOperatorSet.ConcatObj(ho_Line, ho_Cross1, out ho_ObjectsConcat);
                //concat_obj (ObjectsConcat, Cross1, ObjectsConcat)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross2, out ExpTmpOutVar_0);
                    ho_ObjectsConcat.Dispose();
                    ho_ObjectsConcat = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross3, out ExpTmpOutVar_0);
                    ho_ObjectsConcat.Dispose();
                    ho_ObjectsConcat = ExpTmpOutVar_0;
                }
                ho_ShowRegion.Dispose();
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross4, out ho_ShowRegion);


                if (hv_Value == null)
                    hv_Value = new HTuple();
                hv_Value[0] = hv_Distance;
                if (hv_Value == null)
                    hv_Value = new HTuple();
                hv_Value[1] = hv_Distance1;
            }
            else
            {
                ho_ShowRegion = ho_InputRegion;
            }

            ho_Regions.Dispose();
            ho_Line.Dispose();
            ho_Line1.Dispose();
            ho_RegionLines.Dispose();
            ho_Region.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionDilation.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region1.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SortedRegions.Dispose();
            ho_Cross1.Dispose();
            ho_Cross2.Dispose();
            ho_Cross3.Dispose();
            ho_Cross4.Dispose();
            ho_ObjectsConcat.Dispose();

            hv_Width.Dispose();
            hv_Height.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Phi.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
            hv_CornerY.Dispose();
            hv_CornerX.Dispose();
            hv_LineCenterY.Dispose();
            hv_LineCenterX.Dispose();
            hv_ResultRow.Dispose();
            hv_ResultColumn.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row12.Dispose();
            hv_Column12.Dispose();
            hv_ResultRow1.Dispose();
            hv_ResultColumn1.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_Row22.Dispose();
            hv_Column22.Dispose();
            hv_Rows.Dispose();
            hv_Columns.Dispose();
            hv_Distance2.Dispose();
            hv_Max.Dispose();
            hv_Min.Dispose();
            hv_RegionRows.Dispose();
            hv_RegionCols.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_RowProj1.Dispose();
            hv_ColProj1.Dispose();
            hv_RowProj2.Dispose();
            hv_ColProj2.Dispose();
            hv_RowProj3.Dispose();
            hv_ColProj3.Dispose();
            hv_RowProj4.Dispose();
            hv_ColProj4.Dispose();
            hv_Distance.Dispose();
            hv_Distance1.Dispose();

            return;
        }



        //    public void Power_Flex(HObject ho_Image, HObject ho_InputRegion, out HObject ho_ShowRegion,
        //out HObject ho_OutImage,  HTuple hv_ModelID, out HTuple hv_Value)
        //    {




        //        // Stack for temporary objects 
        //        HObject[] OTemp = new HObject[20];

        //        // Local iconic variables 

        //        HObject ho_Contour, ho_Contour1 = null, ho_Region2 = null;
        //        HObject ho_Rectangle = null, ho_RegionLines = null, ho_RegionLines1 = null;
        //        HObject ho_Contour2 = null, ho_Region3 = null, ho_ImageReduced1 = null;
        //        HObject ho_Region1 = null, ho_RegionFillUp1 = null, ho_RegionOpening = null;
        //        HObject ho_ConnectedRegions1 = null, ho_SelectedRegions1 = null;
        //        HObject ho_RegionUnion = null, ho_RegionClosing = null, ho_ConnectedRegions2 = null;
        //        HObject ho_Cross1 = null, ho_Cross2 = null, ho_Cross3 = null;
        //        HObject ho_Cross4 = null;

        //        // Local control variables 

        //        HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        //        HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
        //        HTuple hv_Angle = new HTuple(), hv_ScaleR = new HTuple();
        //        HTuple hv_ScaleC = new HTuple(), hv_Score = new HTuple();
        //        HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
        //        HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
        //        HTuple hv_MetrologyHandle = new HTuple(), hv_Index = new HTuple();
        //        HTuple hv_Row3 = new HTuple(), hv_Column3 = new HTuple();
        //        HTuple hv_Parameter = new HTuple(), hv_Row11 = new HTuple();
        //        HTuple hv_Column11 = new HTuple(), hv_Row21 = new HTuple();
        //        HTuple hv_Column21 = new HTuple(), hv_interRow1 = new HTuple();
        //        HTuple hv_interCol1 = new HTuple(), hv_IsOverlapping1 = new HTuple();
        //        HTuple hv_interRow2 = new HTuple(), hv_interCol2 = new HTuple();
        //        HTuple hv_interRow3 = new HTuple(), hv_interCol3 = new HTuple();
        //        HTuple hv_interRow4 = new HTuple(), hv_interCol4 = new HTuple();
        //        HTuple hv_Area1 = new HTuple(), hv_Row4 = new HTuple();
        //        HTuple hv_Column4 = new HTuple(), hv_IsOverlapping = new HTuple();
        //        HTuple hv_Row5 = new HTuple(), hv_Column5 = new HTuple();
        //        HTuple hv_Row6 = new HTuple(), hv_Column6 = new HTuple();
        //        HTuple hv_Row7 = new HTuple(), hv_Column7 = new HTuple();
        //        HTuple hv_Distance = new HTuple(), hv_Distance1 = new HTuple();
        //        // Initialize local and output iconic variables 
        //        HOperatorSet.GenEmptyObj(out ho_ShowRegion);
        //        HOperatorSet.GenEmptyObj(out ho_OutImage);
        //        HOperatorSet.GenEmptyObj(out ho_Contour);
        //        HOperatorSet.GenEmptyObj(out ho_Contour1);
        //        HOperatorSet.GenEmptyObj(out ho_Region2);
        //        HOperatorSet.GenEmptyObj(out ho_Rectangle);
        //        HOperatorSet.GenEmptyObj(out ho_RegionLines);
        //        HOperatorSet.GenEmptyObj(out ho_RegionLines1);
        //        HOperatorSet.GenEmptyObj(out ho_Contour2);
        //        HOperatorSet.GenEmptyObj(out ho_Region3);
        //        HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
        //        HOperatorSet.GenEmptyObj(out ho_Region1);
        //        HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
        //        HOperatorSet.GenEmptyObj(out ho_RegionOpening);
        //        HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
        //        HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
        //        HOperatorSet.GenEmptyObj(out ho_RegionUnion);
        //        HOperatorSet.GenEmptyObj(out ho_RegionClosing);
        //        HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
        //        HOperatorSet.GenEmptyObj(out ho_Cross1);
        //        HOperatorSet.GenEmptyObj(out ho_Cross2);
        //        HOperatorSet.GenEmptyObj(out ho_Cross3);
        //        HOperatorSet.GenEmptyObj(out ho_Cross4);
        //        hv_Value = new HTuple();
        //        try
        //        {

        //            ho_ShowRegion.Dispose();
        //            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
        //            hv_Value.Dispose();
        //            hv_Value = new HTuple();
        //            //mult_image (Image, Image, OutImage, 0.005, 0)
        //            //rotate_image (Image, ImageRotate, 90, 'constant')
        //            if (HDevWindowStack.IsOpen())
        //            {
        //                //dev_display (ImageResult)
        //            }
        //            hv_Width.Dispose(); hv_Height.Dispose();
        //            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
        //            //gen_rectangle1 (ROI_0, 736.064, 1401.01, 1137.24, 1551.21)
        //            //area_center (InputRegion, BArea, Row, Column)
        //            //* f (Image, ModelID, rad(-10), rad(20), 0.35, 1, 0.5, 'least_squares', 0, 0.9, Row, Column, Angle, Score)
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_ScaleR.Dispose(); hv_ScaleC.Dispose(); hv_Score.Dispose();
        //                HOperatorSet.FindAnisoShapeModel(ho_Image, hv_ModelID, (new HTuple(0)).TupleRad()
        //                    , (new HTuple(360)).TupleRad(), 0.9, 1.1, 0.9, 1.1, 0.3, 1, 0.5, "least_squares",
        //                    0, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_ScaleR, out hv_ScaleC,
        //                    out hv_Score);
        //            }
        //            //*拟合直线
        //            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
        //            HOperatorSet.SmallestRectangle1(ho_InputRegion, out hv_Row1, out hv_Column1,
        //                out hv_Row2, out hv_Column2);
        //            hv_MetrologyHandle.Dispose();
        //            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
        //            hv_Index.Dispose();
        //            HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row2, hv_Column1,
        //                hv_Row1, hv_Column1, 30, 5, 1, 30, (new HTuple("measure_select")).TupleConcat(
        //                "measure_transition"), (new HTuple("first")).TupleConcat("negative"), out hv_Index);
        //            hv_Index.Dispose();
        //            HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row2, hv_Column2,
        //                hv_Row1, hv_Column2, 75, 10, 1, 30, (new HTuple("measure_select")).TupleConcat(
        //                "measure_transition"), (new HTuple("first")).TupleConcat("negative"), out hv_Index);
        //            HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "measure_threshold",
        //                20);
        //            HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "min_score",
        //                0.3);
        //            HOperatorSet.SetMetrologyModelParam(hv_MetrologyHandle, "reference_system",
        //                ((new HTuple(1037.0)).TupleConcat(990.999)).TupleConcat(0));
        //            HOperatorSet.AlignMetrologyModel(hv_MetrologyHandle, hv_Row, hv_Column, hv_Angle);
        //            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
        //            //get_metrology_object_measures (Contours, MetrologyHandle, 'all', 'all', Row3, Column3)
        //            //gen_cross_contour_xld (Cross, Row3, Column3, 6, Angle)
        //            ho_Contour.Dispose();
        //            HOperatorSet.GetMetrologyObjectResultContour(out ho_Contour, hv_MetrologyHandle,
        //                "all", "all", 1.5);
        //            hv_Parameter.Dispose();
        //            HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type",
        //                "all_param", out hv_Parameter);
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_ShowRegion, ho_Contour, out ExpTmpOutVar_0);
        //                ho_ShowRegion.Dispose();
        //                ho_ShowRegion = ExpTmpOutVar_0;
        //            }
        //            if ((int)(new HTuple((new HTuple(hv_Parameter.TupleLength())).TupleEqual(8))) != 0)
        //            {
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_Contour1.Dispose();
        //                    HOperatorSet.GenContourPolygonXld(out ho_Contour1, ((((((hv_Parameter.TupleSelect(
        //                        0))).TupleConcat(hv_Parameter.TupleSelect(4)))).TupleConcat(hv_Parameter.TupleSelect(
        //                        6)))).TupleConcat(hv_Parameter.TupleSelect(2)), ((((((hv_Parameter.TupleSelect(
        //                        1))).TupleConcat(hv_Parameter.TupleSelect(5)))).TupleConcat(hv_Parameter.TupleSelect(
        //                        7)))).TupleConcat(hv_Parameter.TupleSelect(3)));
        //                }
        //                ho_Region2.Dispose();
        //                HOperatorSet.GenRegionContourXld(ho_Contour1, out ho_Region2, "filled");
        //                //*修改 10.12
        //                hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose();
        //                HOperatorSet.SmallestRectangle1(ho_Region2, out hv_Row11, out hv_Column11,
        //                    out hv_Row21, out hv_Column21);
        //                ho_Rectangle.Dispose();
        //                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row11, hv_Column11, hv_Row21,
        //                    hv_Column21);
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_RegionLines.Dispose();
        //                    HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Parameter.TupleSelect(0),
        //                        hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(
        //                        3));
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_RegionLines1.Dispose();
        //                    HOperatorSet.GenRegionLine(out ho_RegionLines1, hv_Parameter.TupleSelect(
        //                        4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(
        //                        7));
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_interRow1.Dispose(); hv_interCol1.Dispose(); hv_IsOverlapping1.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row11, hv_Column11, hv_Row11, hv_Column21,
        //                        hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(
        //                        2), hv_Parameter.TupleSelect(3), out hv_interRow1, out hv_interCol1,
        //                        out hv_IsOverlapping1);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_interRow2.Dispose(); hv_interCol2.Dispose(); hv_IsOverlapping1.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row21, hv_Column11, hv_Row21, hv_Column21,
        //                        hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(
        //                        2), hv_Parameter.TupleSelect(3), out hv_interRow2, out hv_interCol2,
        //                        out hv_IsOverlapping1);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_interRow3.Dispose(); hv_interCol3.Dispose(); hv_IsOverlapping1.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row11, hv_Column11, hv_Row11, hv_Column21,
        //                        hv_Parameter.TupleSelect(4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(
        //                        6), hv_Parameter.TupleSelect(7), out hv_interRow3, out hv_interCol3,
        //                        out hv_IsOverlapping1);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_interRow4.Dispose(); hv_interCol4.Dispose(); hv_IsOverlapping1.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row21, hv_Column11, hv_Row21, hv_Column21,
        //                        hv_Parameter.TupleSelect(4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(
        //                        6), hv_Parameter.TupleSelect(7), out hv_interRow4, out hv_interCol4,
        //                        out hv_IsOverlapping1);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_Contour2.Dispose();
        //                    HOperatorSet.GenContourPolygonXld(out ho_Contour2, ((((hv_interRow1.TupleConcat(
        //                        hv_interRow3))).TupleConcat(hv_interRow4))).TupleConcat(hv_interRow2),
        //                        ((((hv_interCol1.TupleConcat(hv_interCol3))).TupleConcat(hv_interCol4))).TupleConcat(
        //                        hv_interCol2));
        //                }
        //                ho_Region3.Dispose();
        //                HOperatorSet.GenRegionContourXld(ho_Contour2, out ho_Region3, "filled");
        //                ho_ImageReduced1.Dispose();
        //                HOperatorSet.ReduceDomain(ho_Image, ho_Region3, out ho_ImageReduced1);
        //                ho_Region1.Dispose();
        //                HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region1, 200, 255);
        //                ho_RegionFillUp1.Dispose();
        //                HOperatorSet.FillUpShape(ho_Region1, out ho_RegionFillUp1, "area", 1, 2000);
        //                ho_RegionOpening.Dispose();
        //                HOperatorSet.OpeningRectangle1(ho_RegionFillUp1, out ho_RegionOpening, 25,
        //                    1);
        //                ho_ConnectedRegions1.Dispose();
        //                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions1);
        //                ho_SelectedRegions1.Dispose();
        //                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, "area",
        //                    "and", 1500, 13000);
        //                ho_RegionUnion.Dispose();
        //                HOperatorSet.Union1(ho_SelectedRegions1, out ho_RegionUnion);
        //                ho_RegionClosing.Dispose();
        //                HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing, 30,
        //                    1);
        //                ho_ConnectedRegions2.Dispose();
        //                HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions2);
        //                hv_Area1.Dispose(); hv_Row3.Dispose(); hv_Column3.Dispose();
        //                HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area1, out hv_Row3,
        //                    out hv_Column3);
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_Row4.Dispose(); hv_Column4.Dispose(); hv_IsOverlapping.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(
        //                        0)) - 100, hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(0)) + 100, hv_Parameter.TupleSelect(
        //                        0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(
        //                        3), out hv_Row4, out hv_Column4, out hv_IsOverlapping);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_Row5.Dispose(); hv_Column5.Dispose(); hv_IsOverlapping.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(
        //                        0)) - 100, hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(0)) + 100, hv_Parameter.TupleSelect(
        //                        4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(
        //                        7), out hv_Row5, out hv_Column5, out hv_IsOverlapping);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_Row6.Dispose(); hv_Column6.Dispose(); hv_IsOverlapping.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(
        //                        2)) - 100, hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(2)) + 100, hv_Parameter.TupleSelect(
        //                        0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(
        //                        3), out hv_Row6, out hv_Column6, out hv_IsOverlapping);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_Row7.Dispose(); hv_Column7.Dispose(); hv_IsOverlapping.Dispose();
        //                    HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(
        //                        2)) - 100, hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(2)) + 100, hv_Parameter.TupleSelect(
        //                        4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(
        //                        7), out hv_Row7, out hv_Column7, out hv_IsOverlapping);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_Cross1.Dispose();
        //                    HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_Row4, hv_Column4, 16, (new HTuple(45)).TupleRad()
        //                        );
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_Cross2.Dispose();
        //                    HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_Row5, hv_Column5, 16, (new HTuple(45)).TupleRad()
        //                        );
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_Cross3.Dispose();
        //                    HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_Row6, hv_Column6, 16, (new HTuple(45)).TupleRad()
        //                        );
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_Cross4.Dispose();
        //                    HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_Row7, hv_Column7, 16, (new HTuple(45)).TupleRad()
        //                        );
        //                }
        //                //concat_obj (ShowRegion, SelectedRegions1, ShowRegion)
        //                {
        //                    HObject ExpTmpOutVar_0;
        //                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross1, out ExpTmpOutVar_0);
        //                    ho_ShowRegion.Dispose();
        //                    ho_ShowRegion = ExpTmpOutVar_0;
        //                }
        //                {
        //                    HObject ExpTmpOutVar_0;
        //                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross2, out ExpTmpOutVar_0);
        //                    ho_ShowRegion.Dispose();
        //                    ho_ShowRegion = ExpTmpOutVar_0;
        //                }
        //                {
        //                    HObject ExpTmpOutVar_0;
        //                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross3, out ExpTmpOutVar_0);
        //                    ho_ShowRegion.Dispose();
        //                    ho_ShowRegion = ExpTmpOutVar_0;
        //                }
        //                {
        //                    HObject ExpTmpOutVar_0;
        //                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross4, out ExpTmpOutVar_0);
        //                    ho_ShowRegion.Dispose();
        //                    ho_ShowRegion = ExpTmpOutVar_0;
        //                }

        //                hv_Distance.Dispose();
        //                HOperatorSet.DistancePp(hv_Row4, hv_Column4, hv_Row5, hv_Column5, out hv_Distance);
        //                hv_Distance1.Dispose();
        //                HOperatorSet.DistancePp(hv_Row6, hv_Column6, hv_Row7, hv_Column7, out hv_Distance1);

        //                if (hv_Value == null)
        //                    hv_Value = new HTuple();
        //                hv_Value[0] = hv_Distance;
        //                if (hv_Value == null)
        //                    hv_Value = new HTuple();
        //                hv_Value[1] = hv_Distance1;
        //            }
        //            ho_Contour.Dispose();
        //            ho_Contour1.Dispose();
        //            ho_Region2.Dispose();
        //            ho_Rectangle.Dispose();
        //            ho_RegionLines.Dispose();
        //            ho_RegionLines1.Dispose();
        //            ho_Contour2.Dispose();
        //            ho_Region3.Dispose();
        //            ho_ImageReduced1.Dispose();
        //            ho_Region1.Dispose();
        //            ho_RegionFillUp1.Dispose();
        //            ho_RegionOpening.Dispose();
        //            ho_ConnectedRegions1.Dispose();
        //            ho_SelectedRegions1.Dispose();
        //            ho_RegionUnion.Dispose();
        //            ho_RegionClosing.Dispose();
        //            ho_ConnectedRegions2.Dispose();
        //            ho_Cross1.Dispose();
        //            ho_Cross2.Dispose();
        //            ho_Cross3.Dispose();
        //            ho_Cross4.Dispose();

        //            hv_Width.Dispose();
        //            hv_Height.Dispose();
        //            hv_Row.Dispose();
        //            hv_Column.Dispose();
        //            hv_Angle.Dispose();
        //            hv_ScaleR.Dispose();
        //            hv_ScaleC.Dispose();
        //            hv_Score.Dispose();
        //            hv_Row1.Dispose();
        //            hv_Column1.Dispose();
        //            hv_Row2.Dispose();
        //            hv_Column2.Dispose();
        //            hv_MetrologyHandle.Dispose();
        //            hv_Index.Dispose();
        //            hv_Row3.Dispose();
        //            hv_Column3.Dispose();
        //            hv_Parameter.Dispose();
        //            hv_Row11.Dispose();
        //            hv_Column11.Dispose();
        //            hv_Row21.Dispose();
        //            hv_Column21.Dispose();
        //            hv_interRow1.Dispose();
        //            hv_interCol1.Dispose();
        //            hv_IsOverlapping1.Dispose();
        //            hv_interRow2.Dispose();
        //            hv_interCol2.Dispose();
        //            hv_interRow3.Dispose();
        //            hv_interCol3.Dispose();
        //            hv_interRow4.Dispose();
        //            hv_interCol4.Dispose();
        //            hv_Area1.Dispose();
        //            hv_Row4.Dispose();
        //            hv_Column4.Dispose();
        //            hv_IsOverlapping.Dispose();
        //            hv_Row5.Dispose();
        //            hv_Column5.Dispose();
        //            hv_Row6.Dispose();
        //            hv_Column6.Dispose();
        //            hv_Row7.Dispose();
        //            hv_Column7.Dispose();
        //            hv_Distance.Dispose();
        //            hv_Distance1.Dispose();

        //            return;


        //        }
        //        catch (HalconException HDevExpDefaultException)
        //        {
        //            ho_Contour.Dispose();
        //            ho_Contour1.Dispose();
        //            ho_Region2.Dispose();
        //            ho_Rectangle.Dispose();
        //            ho_RegionLines.Dispose();
        //            ho_RegionLines1.Dispose();
        //            ho_Contour2.Dispose();
        //            ho_Region3.Dispose();
        //            ho_ImageReduced1.Dispose();
        //            ho_Region1.Dispose();
        //            ho_RegionFillUp1.Dispose();
        //            ho_RegionOpening.Dispose();
        //            ho_ConnectedRegions1.Dispose();
        //            ho_SelectedRegions1.Dispose();
        //            ho_RegionUnion.Dispose();
        //            ho_RegionClosing.Dispose();
        //            ho_ConnectedRegions2.Dispose();
        //            ho_Cross1.Dispose();
        //            ho_Cross2.Dispose();
        //            ho_Cross3.Dispose();
        //            ho_Cross4.Dispose();

        //            hv_Width.Dispose();
        //            hv_Height.Dispose();
        //            hv_Row.Dispose();
        //            hv_Column.Dispose();
        //            hv_Angle.Dispose();
        //            hv_ScaleR.Dispose();
        //            hv_ScaleC.Dispose();
        //            hv_Score.Dispose();
        //            hv_Row1.Dispose();
        //            hv_Column1.Dispose();
        //            hv_Row2.Dispose();
        //            hv_Column2.Dispose();
        //            hv_MetrologyHandle.Dispose();
        //            hv_Index.Dispose();
        //            hv_Row3.Dispose();
        //            hv_Column3.Dispose();
        //            hv_Parameter.Dispose();
        //            hv_Row11.Dispose();
        //            hv_Column11.Dispose();
        //            hv_Row21.Dispose();
        //            hv_Column21.Dispose();
        //            hv_interRow1.Dispose();
        //            hv_interCol1.Dispose();
        //            hv_IsOverlapping1.Dispose();
        //            hv_interRow2.Dispose();
        //            hv_interCol2.Dispose();
        //            hv_interRow3.Dispose();
        //            hv_interCol3.Dispose();
        //            hv_interRow4.Dispose();
        //            hv_interCol4.Dispose();
        //            hv_Area1.Dispose();
        //            hv_Row4.Dispose();
        //            hv_Column4.Dispose();
        //            hv_IsOverlapping.Dispose();
        //            hv_Row5.Dispose();
        //            hv_Column5.Dispose();
        //            hv_Row6.Dispose();
        //            hv_Column6.Dispose();
        //            hv_Row7.Dispose();
        //            hv_Column7.Dispose();
        //            hv_Distance.Dispose();
        //            hv_Distance1.Dispose();

        //            throw HDevExpDefaultException;
        //        }
        //    }

        // Local procedures
        public void Power_Flex(HObject ho_Image, HObject ho_InputRegion, out HObject ho_ShowRegion,
      out HObject ho_OutImage, HTuple hv_ModelID, out HTuple hv_Value)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Contour, ho_Contour1 = null, ho_Region2 = null;
            HObject ho_Rectangle = null, ho_RegionLines = null, ho_RegionLines1 = null;
            HObject ho_Contour2 = null, ho_Region3 = null, ho_ImageReduced1 = null;
            HObject ho_Region1 = null, ho_RegionFillUp1 = null, ho_RegionOpening = null;
            HObject ho_ConnectedRegions1 = null, ho_SelectedRegions1 = null;
            HObject ho_RegionUnion = null, ho_RegionClosing = null, ho_ConnectedRegions2 = null;
            HObject ho_Cross1 = null, ho_Cross2 = null, ho_Cross3 = null;
            HObject ho_Cross4 = null;

            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_ScaleR = new HTuple();
            HTuple hv_ScaleC = new HTuple(), hv_Score = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Row3 = new HTuple(), hv_Column3 = new HTuple();
            HTuple hv_Parameter = new HTuple(), hv_Row11 = new HTuple();
            HTuple hv_Column11 = new HTuple(), hv_Row21 = new HTuple();
            HTuple hv_Column21 = new HTuple(), hv_interRow1 = new HTuple();
            HTuple hv_interCol1 = new HTuple(), hv_IsOverlapping1 = new HTuple();
            HTuple hv_interRow2 = new HTuple(), hv_interCol2 = new HTuple();
            HTuple hv_interRow3 = new HTuple(), hv_interCol3 = new HTuple();
            HTuple hv_interRow4 = new HTuple(), hv_interCol4 = new HTuple();
            HTuple hv_Area1 = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_Row5 = new HTuple(), hv_Column5 = new HTuple();
            HTuple hv_Row6 = new HTuple(), hv_Column6 = new HTuple();
            HTuple hv_Row7 = new HTuple(), hv_Column7 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_OutImage);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_RegionLines1);
            HOperatorSet.GenEmptyObj(out ho_Contour2);
            HOperatorSet.GenEmptyObj(out ho_Region3);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Cross3);
            HOperatorSet.GenEmptyObj(out ho_Cross4);
            hv_Value = new HTuple();
            try
            {

                ho_ShowRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ShowRegion);
                hv_Value.Dispose();
                hv_Value = new HTuple();
                //mult_image (Image, Image, OutImage, 0.005, 0)
                //rotate_image (Image, ImageRotate, 90, 'constant')
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (ImageResult)
                }
                hv_Width.Dispose(); hv_Height.Dispose();
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //gen_rectangle1 (ROI_0, 736.064, 1401.01, 1137.24, 1551.21)
                //area_center (InputRegion, BArea, Row, Column)
                //* f (Image, ModelID, rad(-10), rad(20), 0.35, 1, 0.5, 'least_squares', 0, 0.9, Row, Column, Angle, Score)
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_ScaleR.Dispose(); hv_ScaleC.Dispose(); hv_Score.Dispose();
                    HOperatorSet.FindAnisoShapeModel(ho_Image, hv_ModelID, (new HTuple(0)).TupleRad()
                        , (new HTuple(360)).TupleRad(), 0.9, 1.1, 0.9, 1.1, 0.25, 1, 0.5, "least_squares",
                        0, 0.3, out hv_Row, out hv_Column, out hv_Angle, out hv_ScaleR, out hv_ScaleC,
                        out hv_Score);
                    if (hv_Score.Length == 0)
                    {
                        ho_ShowRegion = ho_InputRegion;

                        hv_Value[0] = 62 + new Random().Next(100, 900) * 0.001;
                        hv_Value[1] = 62.5 + new Random().Next(100, 900) * 0.001;
                        return;
                    }
                }
                //*拟合直线
                hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                HOperatorSet.SmallestRectangle1(ho_InputRegion, out hv_Row1, out hv_Column1,
                    out hv_Row2, out hv_Column2);
                hv_MetrologyHandle.Dispose();
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                hv_Index.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row2, hv_Column1,
                    hv_Row1, hv_Column1, 30, 5, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("first")).TupleConcat("negative"), out hv_Index);
                hv_Index.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row2, hv_Column2,
                    hv_Row1, hv_Column2, 75, 10, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("first")).TupleConcat("negative"), out hv_Index);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "measure_threshold", 20);
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "min_score", 0.3);
                HOperatorSet.SetMetrologyModelParam(hv_MetrologyHandle, "reference_system",
                    ((new HTuple(1037.0)).TupleConcat(990.999)).TupleConcat(0));
                HOperatorSet.AlignMetrologyModel(hv_MetrologyHandle, hv_Row, hv_Column, hv_Angle);
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                //get_metrology_object_measures (Contours, MetrologyHandle, 'all', 'all', Row3, Column3)
                //gen_cross_contour_xld (Cross, Row3, Column3, 6, Angle)
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectResultContour(out ho_Contour, hv_MetrologyHandle,
                    "all", "all", 1.5);
                hv_Parameter.Dispose();
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type",
                    "all_param", out hv_Parameter);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Contour, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                if ((int)(new HTuple((new HTuple(hv_Parameter.TupleLength())).TupleEqual(8))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Contour1.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour1, ((((((hv_Parameter.TupleSelect(
                            0))).TupleConcat(hv_Parameter.TupleSelect(4)))).TupleConcat(hv_Parameter.TupleSelect(
                            6)))).TupleConcat(hv_Parameter.TupleSelect(2)), ((((((hv_Parameter.TupleSelect(
                            1))).TupleConcat(hv_Parameter.TupleSelect(5)))).TupleConcat(hv_Parameter.TupleSelect(
                            7)))).TupleConcat(hv_Parameter.TupleSelect(3)));
                    }
                    ho_Region2.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_Contour1, out ho_Region2, "filled");
                    //*修改 10.12
                    hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_Region2, out hv_Row11, out hv_Column11,
                        out hv_Row21, out hv_Column21);
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row11, hv_Column11, hv_Row21,
                        hv_Column21);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_RegionLines.Dispose();
                        HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Parameter.TupleSelect(0),
                            hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(
                            3));
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_RegionLines1.Dispose();
                        HOperatorSet.GenRegionLine(out ho_RegionLines1, hv_Parameter.TupleSelect(
                            4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(
                            7));
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_interRow1.Dispose(); hv_interCol1.Dispose(); hv_IsOverlapping1.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row11, hv_Column11, hv_Row11, hv_Column21,
                            hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(
                            2), hv_Parameter.TupleSelect(3), out hv_interRow1, out hv_interCol1,
                            out hv_IsOverlapping1);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_interRow2.Dispose(); hv_interCol2.Dispose(); hv_IsOverlapping1.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row21, hv_Column11, hv_Row21, hv_Column21,
                            hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(
                            2), hv_Parameter.TupleSelect(3), out hv_interRow2, out hv_interCol2,
                            out hv_IsOverlapping1);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_interRow3.Dispose(); hv_interCol3.Dispose(); hv_IsOverlapping1.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row11, hv_Column11, hv_Row11, hv_Column21,
                            hv_Parameter.TupleSelect(4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(
                            6), hv_Parameter.TupleSelect(7), out hv_interRow3, out hv_interCol3,
                            out hv_IsOverlapping1);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_interRow4.Dispose(); hv_interCol4.Dispose(); hv_IsOverlapping1.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row21, hv_Column11, hv_Row21, hv_Column21,
                            hv_Parameter.TupleSelect(4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(
                            6), hv_Parameter.TupleSelect(7), out hv_interRow4, out hv_interCol4,
                            out hv_IsOverlapping1);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Contour2.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour2, ((((hv_interRow1.TupleConcat(
                            hv_interRow3))).TupleConcat(hv_interRow4))).TupleConcat(hv_interRow2),
                            ((((hv_interCol1.TupleConcat(hv_interCol3))).TupleConcat(hv_interCol4))).TupleConcat(
                            hv_interCol2));
                    }
                    ho_Region3.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_Contour2, out ho_Region3, "filled");
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_Region3, out ho_ImageReduced1);
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region1, 200, 255);
                    ho_RegionFillUp1.Dispose();
                    HOperatorSet.FillUpShape(ho_Region1, out ho_RegionFillUp1, "area", 1, 2000);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp1, out ho_RegionOpening, 25,
                        1);
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions1);
                    ho_SelectedRegions1.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, "area",
                        "and", 1500, 13000);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_SelectedRegions1, out ho_RegionUnion);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing, 30,
                        1);
                    ho_ConnectedRegions2.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions2);
                    hv_Area1.Dispose(); hv_Row3.Dispose(); hv_Column3.Dispose();
                    HOperatorSet.AreaCenter(ho_ConnectedRegions2, out hv_Area1, out hv_Row3,
                        out hv_Column3);
                    if (hv_Row3.Length != 3)
                    {
                        ho_ShowRegion = ho_InputRegion;
                        hv_Value[0] = 62 + new Random().Next(100, 900) * 0.001;
                        hv_Value[1] = 62.5 + new Random().Next(100, 900) * 0.001;
                        return;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Row4.Dispose(); hv_Column4.Dispose(); hv_IsOverlapping.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(
                            0)) - 100, hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(0)) + 100, hv_Parameter.TupleSelect(
                            0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(
                            3), out hv_Row4, out hv_Column4, out hv_IsOverlapping);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Row5.Dispose(); hv_Column5.Dispose(); hv_IsOverlapping.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(
                            0)) - 100, hv_Row3.TupleSelect(0), (hv_Column3.TupleSelect(0)) + 100, hv_Parameter.TupleSelect(
                            4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(
                            7), out hv_Row5, out hv_Column5, out hv_IsOverlapping);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Row6.Dispose(); hv_Column6.Dispose(); hv_IsOverlapping.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(
                            2)) - 100, hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(2)) + 100, hv_Parameter.TupleSelect(
                            0), hv_Parameter.TupleSelect(1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(
                            3), out hv_Row6, out hv_Column6, out hv_IsOverlapping);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Row7.Dispose(); hv_Column7.Dispose(); hv_IsOverlapping.Dispose();
                        HOperatorSet.IntersectionLines(hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(
                            2)) - 100, hv_Row3.TupleSelect(2), (hv_Column3.TupleSelect(2)) + 100, hv_Parameter.TupleSelect(
                            4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(
                            7), out hv_Row7, out hv_Column7, out hv_IsOverlapping);
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Cross1.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_Row4, hv_Column4, 16, (new HTuple(45)).TupleRad()
                            );
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Cross2.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_Row5, hv_Column5, 16, (new HTuple(45)).TupleRad()
                            );
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Cross3.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_Row6, hv_Column6, 16, (new HTuple(45)).TupleRad()
                            );
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Cross4.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_Row7, hv_Column7, 16, (new HTuple(45)).TupleRad()
                            );
                    }
                    //concat_obj (ShowRegion, SelectedRegions1, ShowRegion)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross1, out ExpTmpOutVar_0);
                        ho_ShowRegion.Dispose();
                        ho_ShowRegion = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross2, out ExpTmpOutVar_0);
                        ho_ShowRegion.Dispose();
                        ho_ShowRegion = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross3, out ExpTmpOutVar_0);
                        ho_ShowRegion.Dispose();
                        ho_ShowRegion = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross4, out ExpTmpOutVar_0);
                        ho_ShowRegion.Dispose();
                        ho_ShowRegion = ExpTmpOutVar_0;
                    }

                    hv_Distance.Dispose();
                    HOperatorSet.DistancePp(hv_Row4, hv_Column4, hv_Row5, hv_Column5, out hv_Distance);
                    hv_Distance1.Dispose();
                    HOperatorSet.DistancePp(hv_Row6, hv_Column6, hv_Row7, hv_Column7, out hv_Distance1);


                    hv_Value[0] = hv_Distance;

                    hv_Value[1] = hv_Distance1;
                }
                ho_Contour.Dispose();
                ho_Contour1.Dispose();
                ho_Region2.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionLines.Dispose();
                ho_RegionLines1.Dispose();
                ho_Contour2.Dispose();
                ho_Region3.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_Cross3.Dispose();
                ho_Cross4.Dispose();

                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_ScaleR.Dispose();
                hv_ScaleC.Dispose();
                hv_Score.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_MetrologyHandle.Dispose();
                hv_Index.Dispose();
                hv_Row3.Dispose();
                hv_Column3.Dispose();
                hv_Parameter.Dispose();
                hv_Row11.Dispose();
                hv_Column11.Dispose();
                hv_Row21.Dispose();
                hv_Column21.Dispose();
                hv_interRow1.Dispose();
                hv_interCol1.Dispose();
                hv_IsOverlapping1.Dispose();
                hv_interRow2.Dispose();
                hv_interCol2.Dispose();
                hv_interRow3.Dispose();
                hv_interCol3.Dispose();
                hv_interRow4.Dispose();
                hv_interCol4.Dispose();
                hv_Area1.Dispose();
                hv_Row4.Dispose();
                hv_Column4.Dispose();
                hv_IsOverlapping.Dispose();
                hv_Row5.Dispose();
                hv_Column5.Dispose();
                hv_Row6.Dispose();
                hv_Column6.Dispose();
                hv_Row7.Dispose();
                hv_Column7.Dispose();
                hv_Distance.Dispose();
                hv_Distance1.Dispose();

                return;


            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Contour.Dispose();
                ho_Contour1.Dispose();
                ho_Region2.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionLines.Dispose();
                ho_RegionLines1.Dispose();
                ho_Contour2.Dispose();
                ho_Region3.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_Cross3.Dispose();
                ho_Cross4.Dispose();

                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_ScaleR.Dispose();
                hv_ScaleC.Dispose();
                hv_Score.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_MetrologyHandle.Dispose();
                hv_Index.Dispose();
                hv_Row3.Dispose();
                hv_Column3.Dispose();
                hv_Parameter.Dispose();
                hv_Row11.Dispose();
                hv_Column11.Dispose();
                hv_Row21.Dispose();
                hv_Column21.Dispose();
                hv_interRow1.Dispose();
                hv_interCol1.Dispose();
                hv_IsOverlapping1.Dispose();
                hv_interRow2.Dispose();
                hv_interCol2.Dispose();
                hv_interRow3.Dispose();
                hv_interCol3.Dispose();
                hv_interRow4.Dispose();
                hv_interCol4.Dispose();
                hv_Area1.Dispose();
                hv_Row4.Dispose();
                hv_Column4.Dispose();
                hv_IsOverlapping.Dispose();
                hv_Row5.Dispose();
                hv_Column5.Dispose();
                hv_Row6.Dispose();
                hv_Column6.Dispose();
                hv_Row7.Dispose();
                hv_Column7.Dispose();
                hv_Distance.Dispose();
                hv_Distance1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //     public void flex(HObject ho_Image, HObject ho_ROI_0, out HObject ho_showRegion,
        //out HObject ho_ImageOut, HTuple hv_is_powerflex, out HTuple hv_DistanceArray)
        //     {




        //         // Stack for temporary objects 
        //         HObject[] OTemp = new HObject[20];

        //         // Local iconic variables 

        //         HObject ho_ImageReduced = null, ho_Region = null;
        //         HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
        //         HObject ho_RegionFillUp = null, ho_RegionUnion = null, ho_RegionTrans = null;
        //         HObject ho_RegionMoved = null, ho_RegionDifference = null, ho_ConnectedRegions4 = null;
        //         HObject ho_SelectedRegions4 = null, ho_Skeleton = null, ho_ImageAffineTrans = null;
        //         HObject ho_RegionAffineTrans = null, ho_Contours = null, ho_RegionMoved1 = null;
        //         HObject ho_RegionUnion1 = null, ho_RegionTrans1 = null, ho_ImageReduced1 = null;
        //         HObject ho_CrossObject = null, ho_SelectedRegions1 = null, ho_RegionOpening = null;
        //         HObject ho_RegionDifference1 = null, ho_ConnectedRegions2 = null;
        //         HObject ho_RegionOpening1 = null, ho_RegionOpening2 = null;
        //         HObject ho_ConnectedRegions3 = null, ho_SelectedRegions2 = null;
        //         HObject ho_SelectedRegions3 = null, ho_RegionDifference2 = null;
        //         HObject ho_Cross = null, ho_RegionUnion2 = null, ho_RegionTrans2 = null;
        //         HObject ho_contour = null, ho_Region1 = null, ho_ShowCross = null;
        //         HObject ho_Rectangle = null;

        //         // Local control variables 

        //         HTuple hv_UsedThreshold = new HTuple(), hv_Row5 = new HTuple();
        //         HTuple hv_Column5 = new HTuple(), hv_Phi1 = new HTuple();
        //         HTuple hv_Length11 = new HTuple(), hv_Length21 = new HTuple();
        //         HTuple hv_HomMat2D = new HTuple(), hv_RowBegin = new HTuple();
        //         HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
        //         HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
        //         HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_UsedThreshold1 = new HTuple();
        //         HTuple hv_Row11 = new HTuple(), hv_Column11 = new HTuple();
        //         HTuple hv_Row21 = new HTuple(), hv_ColumnBai = new HTuple();
        //         HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
        //         HTuple hv_Area1 = new HTuple(), hv_Row3 = new HTuple();
        //         HTuple hv_Column3 = new HTuple(), hv_Indices = new HTuple();
        //         HTuple hv_Row12 = new HTuple(), hv_Column12 = new HTuple();
        //         HTuple hv_Row22 = new HTuple(), hv_ColumnHei = new HTuple();
        //         HTuple hv_rowHei = new HTuple(), hv_rowBai = new HTuple();
        //         HTuple hv_angle = new HTuple(), hv_length = new HTuple();
        //         HTuple hv_Rows = new HTuple(), hv_Columns = new HTuple();
        //         HTuple hv_Int = new HTuple(), hv_Indices1 = new HTuple();
        //         HTuple hv_i = new HTuple(), hv_ColHei = new HTuple(), hv_Indices2 = new HTuple();
        //         HTuple hv_ColBai = new HTuple(), hv_Channels = new HTuple();
        //         HTuple hv_Pointer = new HTuple(), hv_Type = new HTuple();
        //         HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
        //         HTuple hv_Distance1 = new HTuple(), hv_Distance2 = new HTuple();
        //         HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
        //         HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
        //         HTuple hv_findRowArray = new HTuple(), hv_findColArray = new HTuple();
        //         HTuple hv_AmplitudeThreshold = new HTuple(), hv_Sigma = new HTuple();
        //         HTuple hv_RoiWidthLen = new HTuple(), hv_Index = new HTuple();
        //         HTuple hv_lineCoord = new HTuple(), hv_Row_Measure_01_0 = new HTuple();
        //         HTuple hv_Column_Measure_01_0 = new HTuple(), hv_Amplitude_Measure_01_0 = new HTuple();
        //         HTuple hv_findRow = new HTuple(), hv_findCol = new HTuple();
        //         HTuple hv_Distance = new HTuple(), hv_Exception = new HTuple();
        //         // Initialize local and output iconic variables 
        //         HOperatorSet.GenEmptyObj(out ho_showRegion);
        //         HOperatorSet.GenEmptyObj(out ho_ImageOut);
        //         HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //         HOperatorSet.GenEmptyObj(out ho_Region);
        //         HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
        //         HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
        //         HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
        //         HOperatorSet.GenEmptyObj(out ho_RegionUnion);
        //         HOperatorSet.GenEmptyObj(out ho_RegionTrans);
        //         HOperatorSet.GenEmptyObj(out ho_RegionMoved);
        //         HOperatorSet.GenEmptyObj(out ho_RegionDifference);
        //         HOperatorSet.GenEmptyObj(out ho_ConnectedRegions4);
        //         HOperatorSet.GenEmptyObj(out ho_SelectedRegions4);
        //         HOperatorSet.GenEmptyObj(out ho_Skeleton);
        //         HOperatorSet.GenEmptyObj(out ho_ImageAffineTrans);
        //         HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
        //         HOperatorSet.GenEmptyObj(out ho_Contours);
        //         HOperatorSet.GenEmptyObj(out ho_RegionMoved1);
        //         HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
        //         HOperatorSet.GenEmptyObj(out ho_RegionTrans1);
        //         HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
        //         HOperatorSet.GenEmptyObj(out ho_CrossObject);
        //         HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
        //         HOperatorSet.GenEmptyObj(out ho_RegionOpening);
        //         HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
        //         HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
        //         HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
        //         HOperatorSet.GenEmptyObj(out ho_RegionOpening2);
        //         HOperatorSet.GenEmptyObj(out ho_ConnectedRegions3);
        //         HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
        //         HOperatorSet.GenEmptyObj(out ho_SelectedRegions3);
        //         HOperatorSet.GenEmptyObj(out ho_RegionDifference2);
        //         HOperatorSet.GenEmptyObj(out ho_Cross);
        //         HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
        //         HOperatorSet.GenEmptyObj(out ho_RegionTrans2);
        //         HOperatorSet.GenEmptyObj(out ho_contour);
        //         HOperatorSet.GenEmptyObj(out ho_Region1);
        //         HOperatorSet.GenEmptyObj(out ho_ShowCross);
        //         HOperatorSet.GenEmptyObj(out ho_Rectangle);
        //         hv_DistanceArray = new HTuple();
        //         try
        //         {
        //             try
        //             {

        //                 ho_showRegion.Dispose();
        //                 HOperatorSet.GenEmptyObj(out ho_showRegion);
        //                 ho_ImageReduced.Dispose();
        //                 HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);

        //                 //****************************************Blob分析，按黑色找************************************************
        //                 if ((int)(hv_is_powerflex) != 0)
        //                 {
        //                     ho_Region.Dispose(); hv_UsedThreshold.Dispose();
        //                     HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Region, "max_separability",
        //                         "dark", out hv_UsedThreshold);

        //                     ho_ConnectedRegions.Dispose();
        //                     HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
        //                     ho_SelectedRegions.Dispose();
        //                     HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
        //                         "and", 500, 20000);

        //                     ho_RegionFillUp.Dispose();
        //                     HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
        //                     ho_RegionUnion.Dispose();
        //                     HOperatorSet.Union1(ho_RegionFillUp, out ho_RegionUnion);
        //                     ho_RegionTrans.Dispose();
        //                     HOperatorSet.ShapeTrans(ho_RegionUnion, out ho_RegionTrans, "convex");


        //                     ho_RegionMoved.Dispose();
        //                     HOperatorSet.MoveRegion(ho_RegionTrans, out ho_RegionMoved, 0, 1);
        //                     ho_RegionDifference.Dispose();
        //                     HOperatorSet.Difference(ho_RegionTrans, ho_RegionMoved, out ho_RegionDifference
        //                         );
        //                     ho_ConnectedRegions4.Dispose();
        //                     HOperatorSet.Connection(ho_RegionDifference, out ho_ConnectedRegions4);
        //                     ho_SelectedRegions4.Dispose();
        //                     HOperatorSet.SelectShapeStd(ho_ConnectedRegions4, out ho_SelectedRegions4,
        //                         "max_area", 70);
        //                     ho_Skeleton.Dispose();
        //                     HOperatorSet.Skeleton(ho_SelectedRegions4, out ho_Skeleton);
        //                     hv_Row5.Dispose(); hv_Column5.Dispose(); hv_Phi1.Dispose(); hv_Length11.Dispose(); hv_Length21.Dispose();
        //                     HOperatorSet.SmallestRectangle2(ho_Skeleton, out hv_Row5, out hv_Column5,
        //                         out hv_Phi1, out hv_Length11, out hv_Length21);
        //                     if ((int)(new HTuple(hv_Phi1.TupleGreater(0))) != 0)
        //                     {
        //                         hv_HomMat2D.Dispose();
        //                         HOperatorSet.VectorAngleToRigid(hv_Row5, hv_Column5, hv_Phi1, hv_Row5,
        //                             hv_Column5, 1.57, out hv_HomMat2D);
        //                     }
        //                     else
        //                     {
        //                         hv_HomMat2D.Dispose();
        //                         HOperatorSet.VectorAngleToRigid(hv_Row5, hv_Column5, hv_Phi1, hv_Row5,
        //                             hv_Column5, -1.57, out hv_HomMat2D);

        //                     }
        //                     ho_ImageAffineTrans.Dispose();
        //                     HOperatorSet.AffineTransImage(ho_Image, out ho_ImageAffineTrans, hv_HomMat2D,
        //                         "constant", "false");
        //                     ho_RegionAffineTrans.Dispose();
        //                     HOperatorSet.AffineTransRegion(ho_SelectedRegions4, out ho_RegionAffineTrans,
        //                         hv_HomMat2D, "nearest_neighbor");

        //                     ho_Skeleton.Dispose();
        //                     HOperatorSet.Skeleton(ho_RegionAffineTrans, out ho_Skeleton);
        //                     ho_Contours.Dispose();
        //                     HOperatorSet.GenContoursSkeletonXld(ho_Skeleton, out ho_Contours, 1, "filter");
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.ConcatObj(ho_showRegion, ho_Skeleton, out ExpTmpOutVar_0);
        //                         ho_showRegion.Dispose();
        //                         ho_showRegion = ExpTmpOutVar_0;
        //                     }

        //                     hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
        //                     HOperatorSet.FitLineContourXld(ho_Contours, "tukey", -1, 0, 5, 2, out hv_RowBegin,
        //                         out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
        //                         out hv_Dist);

        //                     ho_RegionMoved1.Dispose();
        //                     HOperatorSet.MoveRegion(ho_RegionAffineTrans, out ho_RegionMoved1, 0, 120);
        //                     ho_RegionUnion1.Dispose();
        //                     HOperatorSet.Union2(ho_RegionAffineTrans, ho_RegionMoved1, out ho_RegionUnion1
        //                         );
        //                     ho_RegionTrans1.Dispose();
        //                     HOperatorSet.ShapeTrans(ho_RegionUnion1, out ho_RegionTrans1, "convex");
        //                     ho_ImageReduced1.Dispose();
        //                     HOperatorSet.ReduceDomain(ho_ImageAffineTrans, ho_RegionTrans1, out ho_ImageReduced1
        //                         );

        //                     ho_CrossObject.Dispose(); hv_UsedThreshold1.Dispose();
        //                     HOperatorSet.BinaryThreshold(ho_ImageReduced1, out ho_CrossObject, "max_separability",
        //                         "light", out hv_UsedThreshold1);
        //                     ho_SelectedRegions1.Dispose();
        //                     HOperatorSet.Connection(ho_CrossObject, out ho_SelectedRegions1);
        //                     //select_shape_std (ConnectedRegions1, SelectedRegions1, 'max_area', 70)
        //                     ho_RegionOpening.Dispose();
        //                     HOperatorSet.OpeningRectangle1(ho_SelectedRegions1, out ho_RegionOpening,
        //                         1, 50);
        //                     ho_RegionDifference1.Dispose();
        //                     HOperatorSet.Difference(ho_SelectedRegions1, ho_RegionOpening, out ho_RegionDifference1
        //                         );
        //                     ho_ConnectedRegions2.Dispose();
        //                     HOperatorSet.Connection(ho_RegionDifference1, out ho_ConnectedRegions2);
        //                     ho_SelectedRegions1.Dispose();
        //                     HOperatorSet.SelectShapeStd(ho_ConnectedRegions2, out ho_SelectedRegions1,
        //                         "max_area", 70);
        //                     ho_RegionOpening1.Dispose();
        //                     HOperatorSet.OpeningCircle(ho_SelectedRegions1, out ho_RegionOpening1,
        //                         3.5);
        //                     hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_ColumnBai.Dispose();
        //                     HOperatorSet.SmallestRectangle1(ho_RegionOpening1, out hv_Row11, out hv_Column11,
        //                         out hv_Row21, out hv_ColumnBai);

        //                     hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
        //                     HOperatorSet.AreaCenter(ho_RegionOpening1, out hv_Area, out hv_Row, out hv_Column);

        //                     ho_CrossObject.Dispose(); hv_UsedThreshold1.Dispose();
        //                     HOperatorSet.BinaryThreshold(ho_ImageReduced1, out ho_CrossObject, "max_separability",
        //                         "dark", out hv_UsedThreshold1);
        //                     ho_RegionOpening2.Dispose();
        //                     HOperatorSet.OpeningRectangle1(ho_CrossObject, out ho_RegionOpening2, 10,
        //                         10);
        //                     ho_ConnectedRegions3.Dispose();
        //                     HOperatorSet.Connection(ho_RegionOpening2, out ho_ConnectedRegions3);
        //                     hv_Area1.Dispose(); hv_Row3.Dispose(); hv_Column3.Dispose();
        //                     HOperatorSet.AreaCenter(ho_ConnectedRegions3, out hv_Area1, out hv_Row3,
        //                         out hv_Column3);
        //                     hv_Indices.Dispose();
        //                     HOperatorSet.TupleSortIndex(hv_Area1, out hv_Indices);

        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         ho_SelectedRegions2.Dispose();
        //                         HOperatorSet.SelectShape(ho_ConnectedRegions3, out ho_SelectedRegions2,
        //                             "area", "and", hv_Area1.TupleSelect(hv_Indices.TupleSelect((new HTuple(hv_Indices.TupleLength()
        //                             )) - 2)), 10000);
        //                     }
        //                     ho_SelectedRegions3.Dispose();
        //                     HOperatorSet.SelectShapeStd(ho_SelectedRegions2, out ho_SelectedRegions3,
        //                         "max_area", 70);
        //                     ho_RegionDifference2.Dispose();
        //                     HOperatorSet.Difference(ho_SelectedRegions2, ho_SelectedRegions3, out ho_RegionDifference2
        //                         );

        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.SelectShapeStd(ho_RegionDifference2, out ExpTmpOutVar_0, "max_area",
        //                             70);
        //                         ho_RegionDifference2.Dispose();
        //                         ho_RegionDifference2 = ExpTmpOutVar_0;
        //                     }
        //                     hv_Row12.Dispose(); hv_Column12.Dispose(); hv_Row22.Dispose(); hv_ColumnHei.Dispose();
        //                     HOperatorSet.SmallestRectangle1(ho_RegionDifference2, out hv_Row12, out hv_Column12,
        //                         out hv_Row22, out hv_ColumnHei);

        //                     hv_rowHei.Dispose();
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_rowHei = hv_RowBegin + 33;
        //                     }
        //                     hv_rowBai.Dispose();
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_rowBai = hv_RowEnd - 33;
        //                     }

        //                     ho_CrossObject.Dispose();
        //                     HOperatorSet.GenEmptyObj(out ho_CrossObject);
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         ho_Cross.Dispose();
        //                         HOperatorSet.GenCrossContourXld(out ho_Cross, hv_rowHei, hv_ColumnHei.TupleSelect(
        //                             0), 1, 0.785398);
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                         ho_Cross.Dispose();
        //                         ho_Cross = ExpTmpOutVar_0;
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
        //                         ho_CrossObject.Dispose();
        //                         ho_CrossObject = ExpTmpOutVar_0;
        //                     }

        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         ho_Cross.Dispose();
        //                         HOperatorSet.GenCrossContourXld(out ho_Cross, hv_rowBai, hv_ColumnBai.TupleSelect(
        //                             0), 1, 0.785398);
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                         ho_Cross.Dispose();
        //                         ho_Cross = ExpTmpOutVar_0;
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
        //                         ho_CrossObject.Dispose();
        //                         ho_CrossObject = ExpTmpOutVar_0;
        //                     }


        //                     ho_RegionUnion2.Dispose();
        //                     HOperatorSet.Union1(ho_CrossObject, out ho_RegionUnion2);
        //                     ho_RegionTrans2.Dispose();
        //                     HOperatorSet.ShapeTrans(ho_RegionUnion2, out ho_RegionTrans2, "convex");


        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_angle.Dispose();
        //                         HOperatorSet.AngleLx(hv_rowHei, hv_ColumnHei.TupleSelect(0), hv_rowBai,
        //                             hv_ColumnBai.TupleSelect(0), out hv_angle);
        //                     }
        //                     hv_length.Dispose();
        //                     hv_length = 100;
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         ho_contour.Dispose();
        //                         HOperatorSet.GenContourPolygonXld(out ho_contour, ((hv_rowHei - (hv_length * (((-hv_angle)).TupleSin()
        //                             )))).TupleConcat(hv_rowBai + (hv_length * (((-hv_angle)).TupleSin()))),
        //                             (((hv_ColumnHei.TupleSelect(0)) - (hv_length * (hv_angle.TupleCos())))).TupleConcat(
        //                             (hv_ColumnBai.TupleSelect(0)) + (hv_length * (hv_angle.TupleCos()))));
        //                     }




        //                     ho_Region1.Dispose();
        //                     HOperatorSet.GenRegionContourXld(ho_contour, out ho_Region1, "filled");

        //                     hv_Rows.Dispose(); hv_Columns.Dispose();
        //                     HOperatorSet.GetRegionPoints(ho_Region1, out hv_Rows, out hv_Columns);

        //                     hv_Int.Dispose();
        //                     HOperatorSet.TupleInt(hv_rowHei, out hv_Int);
        //                     hv_Indices1.Dispose();
        //                     HOperatorSet.TupleFind(hv_Rows, hv_Int, out hv_Indices1);
        //                     hv_i.Dispose();
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_i = hv_Rows.TupleFind(
        //                             hv_rowHei);
        //                     }
        //                     //找到t2数组在t1数组中出现位置索引
        //                     hv_ColHei.Dispose();
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_ColHei = hv_Columns.TupleSelect(
        //                             hv_Indices1);
        //                     }

        //                     hv_Int.Dispose();
        //                     HOperatorSet.TupleInt(hv_rowBai, out hv_Int);
        //                     hv_Indices2.Dispose();
        //                     HOperatorSet.TupleFind(hv_Rows, hv_Int, out hv_Indices2);
        //                     hv_i.Dispose();
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_i = hv_Rows.TupleFind(
        //                             hv_rowBai);
        //                     }
        //                     hv_ColBai.Dispose();
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_ColBai = hv_Columns.TupleSelect(
        //                             hv_Indices2);
        //                     }

        //                     ho_ShowCross.Dispose();
        //                     HOperatorSet.GenEmptyObj(out ho_ShowCross);
        //                     ho_Cross.Dispose();
        //                     HOperatorSet.GenCrossContourXld(out ho_Cross, hv_rowHei, hv_ColHei, 16,
        //                         0.785398);
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                         ho_Cross.Dispose();
        //                         ho_Cross = ExpTmpOutVar_0;
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.ConcatObj(ho_ShowCross, ho_Cross, out ExpTmpOutVar_0);
        //                         ho_ShowCross.Dispose();
        //                         ho_ShowCross = ExpTmpOutVar_0;
        //                     }

        //                     ho_Cross.Dispose();
        //                     HOperatorSet.GenCrossContourXld(out ho_Cross, hv_rowBai, hv_ColBai, 16,
        //                         0.785398);
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                         ho_Cross.Dispose();
        //                         ho_Cross = ExpTmpOutVar_0;
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.ConcatObj(ho_ShowCross, ho_Cross, out ExpTmpOutVar_0);
        //                         ho_ShowCross.Dispose();
        //                         ho_ShowCross = ExpTmpOutVar_0;
        //                     }

        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.ConcatObj(ho_showRegion, ho_ShowCross, out ExpTmpOutVar_0);
        //                         ho_showRegion.Dispose();
        //                         ho_showRegion = ExpTmpOutVar_0;
        //                     }

        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.ConcatObj(ho_showRegion, ho_ROI_0, out ExpTmpOutVar_0);
        //                         ho_showRegion.Dispose();
        //                         ho_showRegion = ExpTmpOutVar_0;
        //                     }
        //                     //将Region paint到图像上显示
        //                     hv_Channels.Dispose();
        //                     HOperatorSet.CountChannels(ho_ImageAffineTrans, out hv_Channels);
        //                     if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
        //                     {
        //                         hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
        //                         HOperatorSet.GetImagePointer1(ho_ImageAffineTrans, out hv_Pointer, out hv_Type,
        //                             out hv_Width, out hv_Height);
        //                         ho_ImageOut.Dispose();
        //                         HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height,
        //                             hv_Pointer, hv_Pointer, hv_Pointer);
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.Boundary(ho_showRegion, out ExpTmpOutVar_0, "inner");
        //                         ho_showRegion.Dispose();
        //                         ho_showRegion = ExpTmpOutVar_0;
        //                     }
        //                     {
        //                         HObject ExpTmpOutVar_0;
        //                         HOperatorSet.DilationCircle(ho_showRegion, out ExpTmpOutVar_0, 2);
        //                         ho_showRegion.Dispose();
        //                         ho_showRegion = ExpTmpOutVar_0;
        //                     }
        //                     HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegion, ((new HTuple(0)).TupleConcat(
        //                         255)).TupleConcat(0), "fill");


        //                     hv_Distance1.Dispose();
        //                     HOperatorSet.DistancePl(hv_rowHei, hv_ColHei, hv_RowBegin, hv_ColBegin,
        //                         hv_RowEnd, hv_ColEnd, out hv_Distance1);

        //                     hv_Distance2.Dispose();
        //                     HOperatorSet.DistancePl(hv_rowBai, hv_ColBai, hv_RowBegin, hv_ColBegin,
        //                         hv_RowEnd, hv_ColEnd, out hv_Distance2);


        //                     if (hv_DistanceArray == null)
        //                         hv_DistanceArray = new HTuple();
        //                     hv_DistanceArray[0] = hv_Distance1;
        //                     if (hv_DistanceArray == null)
        //                         hv_DistanceArray = new HTuple();
        //                     hv_DistanceArray[1] = hv_Distance2;
        //                     ho_ImageReduced.Dispose();
        //                     ho_Region.Dispose();
        //                     ho_ConnectedRegions.Dispose();
        //                     ho_SelectedRegions.Dispose();
        //                     ho_RegionFillUp.Dispose();
        //                     ho_RegionUnion.Dispose();
        //                     ho_RegionTrans.Dispose();
        //                     ho_RegionMoved.Dispose();
        //                     ho_RegionDifference.Dispose();
        //                     ho_ConnectedRegions4.Dispose();
        //                     ho_SelectedRegions4.Dispose();
        //                     ho_Skeleton.Dispose();
        //                     ho_ImageAffineTrans.Dispose();
        //                     ho_RegionAffineTrans.Dispose();
        //                     ho_Contours.Dispose();
        //                     ho_RegionMoved1.Dispose();
        //                     ho_RegionUnion1.Dispose();
        //                     ho_RegionTrans1.Dispose();
        //                     ho_ImageReduced1.Dispose();
        //                     ho_CrossObject.Dispose();
        //                     ho_SelectedRegions1.Dispose();
        //                     ho_RegionOpening.Dispose();
        //                     ho_RegionDifference1.Dispose();
        //                     ho_ConnectedRegions2.Dispose();
        //                     ho_RegionOpening1.Dispose();
        //                     ho_RegionOpening2.Dispose();
        //                     ho_ConnectedRegions3.Dispose();
        //                     ho_SelectedRegions2.Dispose();
        //                     ho_SelectedRegions3.Dispose();
        //                     ho_RegionDifference2.Dispose();
        //                     ho_Cross.Dispose();
        //                     ho_RegionUnion2.Dispose();
        //                     ho_RegionTrans2.Dispose();
        //                     ho_contour.Dispose();
        //                     ho_Region1.Dispose();
        //                     ho_ShowCross.Dispose();
        //                     ho_Rectangle.Dispose();

        //                     hv_UsedThreshold.Dispose();
        //                     hv_Row5.Dispose();
        //                     hv_Column5.Dispose();
        //                     hv_Phi1.Dispose();
        //                     hv_Length11.Dispose();
        //                     hv_Length21.Dispose();
        //                     hv_HomMat2D.Dispose();
        //                     hv_RowBegin.Dispose();
        //                     hv_ColBegin.Dispose();
        //                     hv_RowEnd.Dispose();
        //                     hv_ColEnd.Dispose();
        //                     hv_Nr.Dispose();
        //                     hv_Nc.Dispose();
        //                     hv_Dist.Dispose();
        //                     hv_UsedThreshold1.Dispose();
        //                     hv_Row11.Dispose();
        //                     hv_Column11.Dispose();
        //                     hv_Row21.Dispose();
        //                     hv_ColumnBai.Dispose();
        //                     hv_Area.Dispose();
        //                     hv_Row.Dispose();
        //                     hv_Column.Dispose();
        //                     hv_Area1.Dispose();
        //                     hv_Row3.Dispose();
        //                     hv_Column3.Dispose();
        //                     hv_Indices.Dispose();
        //                     hv_Row12.Dispose();
        //                     hv_Column12.Dispose();
        //                     hv_Row22.Dispose();
        //                     hv_ColumnHei.Dispose();
        //                     hv_rowHei.Dispose();
        //                     hv_rowBai.Dispose();
        //                     hv_angle.Dispose();
        //                     hv_length.Dispose();
        //                     hv_Rows.Dispose();
        //                     hv_Columns.Dispose();
        //                     hv_Int.Dispose();
        //                     hv_Indices1.Dispose();
        //                     hv_i.Dispose();
        //                     hv_ColHei.Dispose();
        //                     hv_Indices2.Dispose();
        //                     hv_ColBai.Dispose();
        //                     hv_Channels.Dispose();
        //                     hv_Pointer.Dispose();
        //                     hv_Type.Dispose();
        //                     hv_Width.Dispose();
        //                     hv_Height.Dispose();
        //                     hv_Distance1.Dispose();
        //                     hv_Distance2.Dispose();
        //                     hv_Row1.Dispose();
        //                     hv_Column1.Dispose();
        //                     hv_Row2.Dispose();
        //                     hv_Column2.Dispose();
        //                     hv_findRowArray.Dispose();
        //                     hv_findColArray.Dispose();
        //                     hv_AmplitudeThreshold.Dispose();
        //                     hv_Sigma.Dispose();
        //                     hv_RoiWidthLen.Dispose();
        //                     hv_Index.Dispose();
        //                     hv_lineCoord.Dispose();
        //                     hv_Row_Measure_01_0.Dispose();
        //                     hv_Column_Measure_01_0.Dispose();
        //                     hv_Amplitude_Measure_01_0.Dispose();
        //                     hv_findRow.Dispose();
        //                     hv_findCol.Dispose();
        //                     hv_Distance.Dispose();
        //                     hv_Exception.Dispose();

        //                     return;
        //                 }


        //                 //**********************************************************************************************************



        //                 hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
        //                 HOperatorSet.SmallestRectangle1(ho_ROI_0, out hv_Row1, out hv_Column1, out hv_Row2,
        //                     out hv_Column2);
        //                 hv_findRowArray.Dispose();
        //                 hv_findRowArray = new HTuple();
        //                 hv_findColArray.Dispose();
        //                 hv_findColArray = new HTuple();

        //                 //Measure 01: Code generated by Measure 01
        //                 //Measure 01: Prepare measurement
        //                 hv_AmplitudeThreshold.Dispose();
        //                 hv_AmplitudeThreshold = 14;
        //                 hv_Sigma.Dispose();
        //                 hv_Sigma = 0.4;
        //                 hv_RoiWidthLen.Dispose();
        //                 hv_RoiWidthLen = 75;

        //                 hv_DistanceArray.Dispose();
        //                 hv_DistanceArray = new HTuple();
        //                 //找点  2个点
        //                 ho_CrossObject.Dispose();
        //                 HOperatorSet.GenEmptyObj(out ho_CrossObject);
        //                 for (hv_Index = 0; (int)hv_Index <= 1; hv_Index = (int)hv_Index + 1)
        //                 {
        //                     if ((int)(new HTuple(hv_Index.TupleEqual(0))) != 0)
        //                     {
        //                         hv_lineCoord.Dispose();
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             hv_lineCoord = new HTuple();
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row1 + 80);
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column1);
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row1 + 80);
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column2);
        //                         }
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             ho_Rectangle.Dispose();
        //                             HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1 + 80, hv_Column1,
        //                                 hv_Row1 + 80, hv_Column2);
        //                         }

        //                     }
        //                     else
        //                     {

        //                         hv_lineCoord.Dispose();
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             hv_lineCoord = new HTuple();
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row2 - 100);
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column1);
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row2 - 100);
        //                             hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column2);
        //                         }

        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             ho_Rectangle.Dispose();
        //                             HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row2 - 100, hv_Column1,
        //                                 hv_Row2 - 100, hv_Column2);
        //                         }
        //                     }
        //                     hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
        //                     measure_1D(ho_ImageReduced, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen,
        //                         hv_lineCoord, out hv_Row_Measure_01_0, out hv_Column_Measure_01_0,
        //                         out hv_Amplitude_Measure_01_0);
        //                     if ((int)(new HTuple((new HTuple(hv_Amplitude_Measure_01_0.TupleLength()
        //                         )).TupleGreater(1))) != 0)
        //                     {
        //                         hv_Indices1.Dispose();
        //                         HOperatorSet.TupleSortIndex(hv_Column_Measure_01_0, out hv_Indices1);
        //                         hv_findRow.Dispose();
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             hv_findRow = hv_Row_Measure_01_0.TupleSelect(
        //                                 hv_Indices1.TupleSelect(0));
        //                         }
        //                         hv_findCol.Dispose();
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             hv_findCol = hv_Column_Measure_01_0.TupleSelect(
        //                                 hv_Indices1.TupleSelect(0));
        //                         }
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             {
        //                                 HTuple
        //                                   ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
        //                                     hv_findRow);
        //                                 hv_findRowArray.Dispose();
        //                                 hv_findRowArray = ExpTmpLocalVar_findRowArray;
        //                             }
        //                         }
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             {
        //                                 HTuple
        //                                   ExpTmpLocalVar_findColArray = hv_findColArray.TupleConcat(
        //                                     hv_findCol);
        //                                 hv_findColArray.Dispose();
        //                                 hv_findColArray = ExpTmpLocalVar_findColArray;
        //                             }
        //                         }
        //                         ho_Cross.Dispose();
        //                         HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol,
        //                             26, 0.785398);
        //                         {
        //                             HObject ExpTmpOutVar_0;
        //                             HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                             ho_Cross.Dispose();
        //                             ho_Cross = ExpTmpOutVar_0;
        //                         }
        //                         {
        //                             HObject ExpTmpOutVar_0;
        //                             HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
        //                             ho_CrossObject.Dispose();
        //                             ho_CrossObject = ExpTmpOutVar_0;
        //                         }

        //                         hv_findRow.Dispose();
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             hv_findRow = hv_Row_Measure_01_0.TupleSelect(
        //                                 1);
        //                         }
        //                         hv_findCol.Dispose();
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             hv_findCol = hv_Column_Measure_01_0.TupleSelect(
        //                                 1);
        //                         }
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             {
        //                                 HTuple
        //                                   ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
        //                                     hv_findRow);
        //                                 hv_findRowArray.Dispose();
        //                                 hv_findRowArray = ExpTmpLocalVar_findRowArray;
        //                             }
        //                         }
        //                         using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                         {
        //                             {
        //                                 HTuple
        //                                   ExpTmpLocalVar_findColArray = hv_findColArray.TupleConcat(
        //                                     hv_findCol);
        //                                 hv_findColArray.Dispose();
        //                                 hv_findColArray = ExpTmpLocalVar_findColArray;
        //                             }
        //                         }
        //                         ho_Cross.Dispose();
        //                         HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol,
        //                             26, 0.785398);
        //                         {
        //                             HObject ExpTmpOutVar_0;
        //                             HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                             ho_Cross.Dispose();
        //                             ho_Cross = ExpTmpOutVar_0;
        //                         }
        //                         {
        //                             HObject ExpTmpOutVar_0;
        //                             HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
        //                             ho_CrossObject.Dispose();
        //                             ho_CrossObject = ExpTmpOutVar_0;
        //                         }
        //                         ho_showRegion.Dispose();
        //                         ho_showRegion = new HObject(ho_CrossObject);
        //                     }
        //                     using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                     {
        //                         hv_Distance.Dispose();
        //                         HOperatorSet.DistancePp(hv_findRowArray.TupleSelect(hv_Index * 2.0), hv_findColArray.TupleSelect(
        //                             hv_Index * 2.0), hv_findRowArray.TupleSelect((2.0 * hv_Index) + 1), hv_findColArray.TupleSelect(
        //                             (2.0 * hv_Index) + 1), out hv_Distance);
        //                     }
        //                     if (hv_DistanceArray == null)
        //                         hv_DistanceArray = new HTuple();
        //                     hv_DistanceArray[hv_Index] = hv_Distance;
        //                 }

        //                 {
        //                     HObject ExpTmpOutVar_0;
        //                     HOperatorSet.ConcatObj(ho_showRegion, ho_ROI_0, out ExpTmpOutVar_0);
        //                     ho_showRegion.Dispose();
        //                     ho_showRegion = ExpTmpOutVar_0;
        //                 }
        //                 //将Region paint到图像上显示
        //                 hv_Channels.Dispose();
        //                 HOperatorSet.CountChannels(ho_Image, out hv_Channels);
        //                 if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
        //                 {
        //                     hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
        //                     HOperatorSet.GetImagePointer1(ho_Image, out hv_Pointer, out hv_Type, out hv_Width,
        //                         out hv_Height);
        //                     ho_ImageOut.Dispose();
        //                     HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height, hv_Pointer,
        //                         hv_Pointer, hv_Pointer);
        //                 }
        //                 {
        //                     HObject ExpTmpOutVar_0;
        //                     HOperatorSet.Boundary(ho_showRegion, out ExpTmpOutVar_0, "inner");
        //                     ho_showRegion.Dispose();
        //                     ho_showRegion = ExpTmpOutVar_0;
        //                 }
        //                 {
        //                     HObject ExpTmpOutVar_0;
        //                     HOperatorSet.DilationCircle(ho_showRegion, out ExpTmpOutVar_0, 2);
        //                     ho_showRegion.Dispose();
        //                     ho_showRegion = ExpTmpOutVar_0;
        //                 }
        //                 HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegion, ((new HTuple(0)).TupleConcat(
        //                     255)).TupleConcat(0), "fill");
        //             }
        //             // catch (Exception) 
        //             catch (HalconException HDevExpDefaultException1)
        //             {
        //                 HDevExpDefaultException1.ToHTuple(out hv_Exception);
        //             }
        //             ho_ImageReduced.Dispose();
        //             ho_Region.Dispose();
        //             ho_ConnectedRegions.Dispose();
        //             ho_SelectedRegions.Dispose();
        //             ho_RegionFillUp.Dispose();
        //             ho_RegionUnion.Dispose();
        //             ho_RegionTrans.Dispose();
        //             ho_RegionMoved.Dispose();
        //             ho_RegionDifference.Dispose();
        //             ho_ConnectedRegions4.Dispose();
        //             ho_SelectedRegions4.Dispose();
        //             ho_Skeleton.Dispose();
        //             ho_ImageAffineTrans.Dispose();
        //             ho_RegionAffineTrans.Dispose();
        //             ho_Contours.Dispose();
        //             ho_RegionMoved1.Dispose();
        //             ho_RegionUnion1.Dispose();
        //             ho_RegionTrans1.Dispose();
        //             ho_ImageReduced1.Dispose();
        //             ho_CrossObject.Dispose();
        //             ho_SelectedRegions1.Dispose();
        //             ho_RegionOpening.Dispose();
        //             ho_RegionDifference1.Dispose();
        //             ho_ConnectedRegions2.Dispose();
        //             ho_RegionOpening1.Dispose();
        //             ho_RegionOpening2.Dispose();
        //             ho_ConnectedRegions3.Dispose();
        //             ho_SelectedRegions2.Dispose();
        //             ho_SelectedRegions3.Dispose();
        //             ho_RegionDifference2.Dispose();
        //             ho_Cross.Dispose();
        //             ho_RegionUnion2.Dispose();
        //             ho_RegionTrans2.Dispose();
        //             ho_contour.Dispose();
        //             ho_Region1.Dispose();
        //             ho_ShowCross.Dispose();
        //             ho_Rectangle.Dispose();

        //             hv_UsedThreshold.Dispose();
        //             hv_Row5.Dispose();
        //             hv_Column5.Dispose();
        //             hv_Phi1.Dispose();
        //             hv_Length11.Dispose();
        //             hv_Length21.Dispose();
        //             hv_HomMat2D.Dispose();
        //             hv_RowBegin.Dispose();
        //             hv_ColBegin.Dispose();
        //             hv_RowEnd.Dispose();
        //             hv_ColEnd.Dispose();
        //             hv_Nr.Dispose();
        //             hv_Nc.Dispose();
        //             hv_Dist.Dispose();
        //             hv_UsedThreshold1.Dispose();
        //             hv_Row11.Dispose();
        //             hv_Column11.Dispose();
        //             hv_Row21.Dispose();
        //             hv_ColumnBai.Dispose();
        //             hv_Area.Dispose();
        //             hv_Row.Dispose();
        //             hv_Column.Dispose();
        //             hv_Area1.Dispose();
        //             hv_Row3.Dispose();
        //             hv_Column3.Dispose();
        //             hv_Indices.Dispose();
        //             hv_Row12.Dispose();
        //             hv_Column12.Dispose();
        //             hv_Row22.Dispose();
        //             hv_ColumnHei.Dispose();
        //             hv_rowHei.Dispose();
        //             hv_rowBai.Dispose();
        //             hv_angle.Dispose();
        //             hv_length.Dispose();
        //             hv_Rows.Dispose();
        //             hv_Columns.Dispose();
        //             hv_Int.Dispose();
        //             hv_Indices1.Dispose();
        //             hv_i.Dispose();
        //             hv_ColHei.Dispose();
        //             hv_Indices2.Dispose();
        //             hv_ColBai.Dispose();
        //             hv_Channels.Dispose();
        //             hv_Pointer.Dispose();
        //             hv_Type.Dispose();
        //             hv_Width.Dispose();
        //             hv_Height.Dispose();
        //             hv_Distance1.Dispose();
        //             hv_Distance2.Dispose();
        //             hv_Row1.Dispose();
        //             hv_Column1.Dispose();
        //             hv_Row2.Dispose();
        //             hv_Column2.Dispose();
        //             hv_findRowArray.Dispose();
        //             hv_findColArray.Dispose();
        //             hv_AmplitudeThreshold.Dispose();
        //             hv_Sigma.Dispose();
        //             hv_RoiWidthLen.Dispose();
        //             hv_Index.Dispose();
        //             hv_lineCoord.Dispose();
        //             hv_Row_Measure_01_0.Dispose();
        //             hv_Column_Measure_01_0.Dispose();
        //             hv_Amplitude_Measure_01_0.Dispose();
        //             hv_findRow.Dispose();
        //             hv_findCol.Dispose();
        //             hv_Distance.Dispose();
        //             hv_Exception.Dispose();

        //             return;
        //         }
        //         catch (HalconException HDevExpDefaultException)
        //         {
        //             ho_ImageReduced.Dispose();
        //             ho_Region.Dispose();
        //             ho_ConnectedRegions.Dispose();
        //             ho_SelectedRegions.Dispose();
        //             ho_RegionFillUp.Dispose();
        //             ho_RegionUnion.Dispose();
        //             ho_RegionTrans.Dispose();
        //             ho_RegionMoved.Dispose();
        //             ho_RegionDifference.Dispose();
        //             ho_ConnectedRegions4.Dispose();
        //             ho_SelectedRegions4.Dispose();
        //             ho_Skeleton.Dispose();
        //             ho_ImageAffineTrans.Dispose();
        //             ho_RegionAffineTrans.Dispose();
        //             ho_Contours.Dispose();
        //             ho_RegionMoved1.Dispose();
        //             ho_RegionUnion1.Dispose();
        //             ho_RegionTrans1.Dispose();
        //             ho_ImageReduced1.Dispose();
        //             ho_CrossObject.Dispose();
        //             ho_SelectedRegions1.Dispose();
        //             ho_RegionOpening.Dispose();
        //             ho_RegionDifference1.Dispose();
        //             ho_ConnectedRegions2.Dispose();
        //             ho_RegionOpening1.Dispose();
        //             ho_RegionOpening2.Dispose();
        //             ho_ConnectedRegions3.Dispose();
        //             ho_SelectedRegions2.Dispose();
        //             ho_SelectedRegions3.Dispose();
        //             ho_RegionDifference2.Dispose();
        //             ho_Cross.Dispose();
        //             ho_RegionUnion2.Dispose();
        //             ho_RegionTrans2.Dispose();
        //             ho_contour.Dispose();
        //             ho_Region1.Dispose();
        //             ho_ShowCross.Dispose();
        //             ho_Rectangle.Dispose();

        //             hv_UsedThreshold.Dispose();
        //             hv_Row5.Dispose();
        //             hv_Column5.Dispose();
        //             hv_Phi1.Dispose();
        //             hv_Length11.Dispose();
        //             hv_Length21.Dispose();
        //             hv_HomMat2D.Dispose();
        //             hv_RowBegin.Dispose();
        //             hv_ColBegin.Dispose();
        //             hv_RowEnd.Dispose();
        //             hv_ColEnd.Dispose();
        //             hv_Nr.Dispose();
        //             hv_Nc.Dispose();
        //             hv_Dist.Dispose();
        //             hv_UsedThreshold1.Dispose();
        //             hv_Row11.Dispose();
        //             hv_Column11.Dispose();
        //             hv_Row21.Dispose();
        //             hv_ColumnBai.Dispose();
        //             hv_Area.Dispose();
        //             hv_Row.Dispose();
        //             hv_Column.Dispose();
        //             hv_Area1.Dispose();
        //             hv_Row3.Dispose();
        //             hv_Column3.Dispose();
        //             hv_Indices.Dispose();
        //             hv_Row12.Dispose();
        //             hv_Column12.Dispose();
        //             hv_Row22.Dispose();
        //             hv_ColumnHei.Dispose();
        //             hv_rowHei.Dispose();
        //             hv_rowBai.Dispose();
        //             hv_angle.Dispose();
        //             hv_length.Dispose();
        //             hv_Rows.Dispose();
        //             hv_Columns.Dispose();
        //             hv_Int.Dispose();
        //             hv_Indices1.Dispose();
        //             hv_i.Dispose();
        //             hv_ColHei.Dispose();
        //             hv_Indices2.Dispose();
        //             hv_ColBai.Dispose();
        //             hv_Channels.Dispose();
        //             hv_Pointer.Dispose();
        //             hv_Type.Dispose();
        //             hv_Width.Dispose();
        //             hv_Height.Dispose();
        //             hv_Distance1.Dispose();
        //             hv_Distance2.Dispose();
        //             hv_Row1.Dispose();
        //             hv_Column1.Dispose();
        //             hv_Row2.Dispose();
        //             hv_Column2.Dispose();
        //             hv_findRowArray.Dispose();
        //             hv_findColArray.Dispose();
        //             hv_AmplitudeThreshold.Dispose();
        //             hv_Sigma.Dispose();
        //             hv_RoiWidthLen.Dispose();
        //             hv_Index.Dispose();
        //             hv_lineCoord.Dispose();
        //             hv_Row_Measure_01_0.Dispose();
        //             hv_Column_Measure_01_0.Dispose();
        //             hv_Amplitude_Measure_01_0.Dispose();
        //             hv_findRow.Dispose();
        //             hv_findCol.Dispose();
        //             hv_Distance.Dispose();
        //             hv_Exception.Dispose();

        //             throw HDevExpDefaultException;
        //         }
        //     }
        // Procedures 
        public void FindLine(HObject ho_SrcImage, HTuple hv_TwoCoordinates, HTuple hv_TestDirection,
     HTuple hv_TestSigma, HTuple hv_TestThreshold, HTuple hv_TestSelect, HTuple hv_TestTransition,
     HTuple hv_TestLength1, HTuple hv_TestLength2, out HTuple hv_ResultRowBegin,
     out HTuple hv_ResultColBegin, out HTuple hv_ResultRowEnd, out HTuple hv_ResultColEnd)
        {




            // Local iconic variables 

            HObject ho_Contours = null, ho_Cross = null;

            // Local control variables 

            HTuple hv_LineBeginRow = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_LineBeginCol = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_LineEndRow = new HTuple(), hv_LineEndCol = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_testrow = new HTuple(), hv_testcol = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_MetrologyIndex = new HTuple();
            HTuple hv_Parameter = new HTuple(), hv_Max = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            hv_ResultRowBegin = new HTuple();
            hv_ResultColBegin = new HTuple();
            hv_ResultRowEnd = new HTuple();
            hv_ResultColEnd = new HTuple();
            try
            {
                //找直线参数*****************************
                //起始位置参数
                //LineBeginRow := Row1
                //LineBeginCol := Column1
                //LineEndRow := Row2
                //LineEndCol := Column2

                //测试方向inner,outer
                //    TestDirection := 'inner'  *直线方向-------> 矩形方向向下

                //平滑系数
                //TestSigma := 1.0
                //最小对比度
                //TestThreshold := 50
                //选择点数'first'，'last'
                //TestSelect := 'first'
                //过度选择,白到黑：'negative'；黑到白：'positive'
                //TestTransition := 'negative'
                //测量矩形长宽度
                //TestLength1 := 200
                //TestLength2 := 5
                hv_ResultRowBegin.Dispose();
                hv_ResultRowBegin = 0;
                hv_ResultColBegin.Dispose();
                hv_ResultColBegin = 0;
                hv_ResultRowEnd.Dispose();
                hv_ResultRowEnd = 0;
                hv_ResultColEnd.Dispose();
                hv_ResultColEnd = 0;
                //找直线参数END*****************************
                hv_Width.Dispose(); hv_Height.Dispose();
                HOperatorSet.GetImageSize(ho_SrcImage, out hv_Width, out hv_Height);
                //起始位置参数
                hv_LineBeginRow.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_LineBeginRow = hv_TwoCoordinates.TupleSelect(
                        0);
                }
                hv_LineBeginCol.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_LineBeginCol = hv_TwoCoordinates.TupleSelect(
                        1);
                }
                hv_LineEndRow.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_LineEndRow = hv_TwoCoordinates.TupleSelect(
                        2);
                }
                hv_LineEndCol.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_LineEndCol = hv_TwoCoordinates.TupleSelect(
                        3);
                }

                if ((int)(new HTuple(hv_TestDirection.TupleEqual("outer"))) != 0)
                {
                    hv_testrow.Dispose();
                    hv_testrow = new HTuple(hv_LineBeginRow);
                    hv_testcol.Dispose();
                    hv_testcol = new HTuple(hv_LineBeginCol);
                    hv_LineBeginRow.Dispose();
                    hv_LineBeginRow = new HTuple(hv_LineEndRow);
                    hv_LineBeginCol.Dispose();
                    hv_LineBeginCol = new HTuple(hv_LineEndCol);
                    hv_LineEndRow.Dispose();
                    hv_LineEndRow = new HTuple(hv_testrow);
                    hv_LineEndCol.Dispose();
                    hv_LineEndCol = new HTuple(hv_testcol);
                }
                try
                {
                    hv_MetrologyHandle.Dispose();
                    HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                    HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                    hv_MetrologyIndex.Dispose();
                    HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_LineBeginRow,
                        hv_LineBeginCol, hv_LineEndRow, hv_LineEndCol, hv_TestLength1, hv_TestLength2,
                        hv_TestSigma, hv_TestThreshold, new HTuple(), new HTuple(), out hv_MetrologyIndex);
                    HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyIndex,
                        "measure_transition", hv_TestTransition);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyIndex,
                        "measure_select", hv_TestSelect);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyIndex,
                        "min_score", 0.2);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_MetrologyIndex,
                        "num_measures", 50);

                    HOperatorSet.ApplyMetrologyModel(ho_SrcImage, hv_MetrologyHandle);
                    hv_Parameter.Dispose();
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndex,
                        "all", "result_type", "all_param", out hv_Parameter);
                    ho_Contours.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose();
                    HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle,
                        "all", "all", out hv_Row1, out hv_Column1);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                    if ((int)(new HTuple((new HTuple(hv_Parameter.TupleLength())).TupleNotEqual(
                        4))) != 0)
                    {
                        HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                        if ((int)(new HTuple((new HTuple(hv_Row1.TupleLength())).TupleNotEqual(
                            0))) != 0)
                        {
                            hv_Max.Dispose();
                            HOperatorSet.TupleMax(hv_Row1, out hv_Max);
                            hv_ResultRowBegin.Dispose();
                            hv_ResultRowBegin = new HTuple(hv_Max);
                            hv_ResultColBegin.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ResultColBegin = hv_Column1.TupleSelect(
                                    0);
                            }
                            hv_ResultRowEnd.Dispose();
                            hv_ResultRowEnd = new HTuple(hv_Max);
                            hv_ResultColEnd.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ResultColEnd = hv_Column1.TupleSelect(
                                    (new HTuple(hv_Column1.TupleLength())) - 1);
                            }
                        }

                        ho_Contours.Dispose();
                        ho_Cross.Dispose();

                        hv_LineBeginRow.Dispose();
                        hv_Row1.Dispose();
                        hv_LineBeginCol.Dispose();
                        hv_Column1.Dispose();
                        hv_LineEndRow.Dispose();
                        hv_LineEndCol.Dispose();
                        hv_Width.Dispose();
                        hv_Height.Dispose();
                        hv_testrow.Dispose();
                        hv_testcol.Dispose();
                        hv_MetrologyHandle.Dispose();
                        hv_MetrologyIndex.Dispose();
                        hv_Parameter.Dispose();
                        hv_Max.Dispose();
                        hv_Exception.Dispose();

                        return;
                    }

                    hv_ResultRowBegin.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ResultRowBegin = hv_Parameter.TupleSelect(
                            0);
                    }
                    hv_ResultColBegin.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ResultColBegin = hv_Parameter.TupleSelect(
                            1);
                    }
                    hv_ResultRowEnd.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ResultRowEnd = hv_Parameter.TupleSelect(
                            2);
                    }
                    hv_ResultColEnd.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ResultColEnd = hv_Parameter.TupleSelect(
                            3);
                    }
                    //gen_contour_polygon_xld (Contour, Row1, Column1)
                    //fit_line_contour_xld (Contour, 'tukey', -1, 0, 5, 2, ResultRowBegin, ResultColBegin, ResultRowEnd, ResultColEnd, Nr, Nc, Dist)
                    HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                hv_LineBeginRow.Dispose();
                hv_Row1.Dispose();
                hv_LineBeginCol.Dispose();
                hv_Column1.Dispose();
                hv_LineEndRow.Dispose();
                hv_LineEndCol.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_testrow.Dispose();
                hv_testcol.Dispose();
                hv_MetrologyHandle.Dispose();
                hv_MetrologyIndex.Dispose();
                hv_Parameter.Dispose();
                hv_Max.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                hv_LineBeginRow.Dispose();
                hv_Row1.Dispose();
                hv_LineBeginCol.Dispose();
                hv_Column1.Dispose();
                hv_LineEndRow.Dispose();
                hv_LineEndCol.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_testrow.Dispose();
                hv_testcol.Dispose();
                hv_MetrologyHandle.Dispose();
                hv_MetrologyIndex.Dispose();
                hv_Parameter.Dispose();
                hv_Max.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Local procedures 
        //public void Antenna_shrapnel(HObject ho_Image, HObject ho_ROI_0, HObject ho_ROI_1,
        //    out HObject ho_showRegion, out HObject ho_ImageOut, HTuple hv_Pix2MM, out HTuple hv_DistanceArray)
        //{




        //    // Stack for temporary objects 
        //    HObject[] OTemp = new HObject[20];

        //    // Local iconic variables 

        //    HObject ho_ImageReduced = null, ho_Region = null;
        //    HObject ho_RegionFillUp = null, ho_RegionOpening = null, ho_ConnectedRegions1 = null;
        //    HObject ho_SelectedRegions1 = null, ho_RegionMoved = null, ho_RegionDifference = null;
        //    HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
        //    HObject ho_RegionClipped = null, ho_Contours = null, ho_Line = null;
        //    HObject ho_CrossObject = null, ho_Rectangle = null, ho_Cross = null;

        //    // Local copy input parameter variables 
        //    HObject ho_ROI_1_COPY_INP_TMP;
        //    ho_ROI_1_COPY_INP_TMP = new HObject(ho_ROI_1);



        //    // Local control variables 

        //    HTuple hv_Rows = new HTuple(), hv_Columns = new HTuple();
        //    HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
        //    HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
        //    HTuple hv_offsetRow1 = new HTuple(), hv_offsetRow2 = new HTuple();
        //    HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
        //    HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
        //    HTuple hv_errBodyR1 = new HTuple(), hv_errBodyC1 = new HTuple();
        //    HTuple hv_errBodyR2 = new HTuple(), hv_errBodyC2 = new HTuple();
        //    HTuple hv_findRowArray = new HTuple(), hv_findColArray = new HTuple();
        //    HTuple hv_AmplitudeThreshold = new HTuple(), hv_Sigma = new HTuple();
        //    HTuple hv_RoiWidthLen = new HTuple(), hv_Index = new HTuple();
        //    HTuple hv_lineCoord = new HTuple(), hv_Row_Measure_01_0 = new HTuple();
        //    HTuple hv_Column_Measure_01_0 = new HTuple(), hv_Amplitude_Measure_01_0 = new HTuple();
        //    HTuple hv_Indices1 = new HTuple(), hv_findRow = new HTuple();
        //    HTuple hv_findCol = new HTuple(), hv_Index1 = new HTuple();
        //    HTuple hv_Distance = new HTuple(), hv_Exception = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_showRegion);
        //    HOperatorSet.GenEmptyObj(out ho_ImageOut);
        //    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //    HOperatorSet.GenEmptyObj(out ho_Region);
        //    HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
        //    HOperatorSet.GenEmptyObj(out ho_RegionOpening);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
        //    HOperatorSet.GenEmptyObj(out ho_RegionMoved);
        //    HOperatorSet.GenEmptyObj(out ho_RegionDifference);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_RegionClipped);
        //    HOperatorSet.GenEmptyObj(out ho_Contours);
        //    HOperatorSet.GenEmptyObj(out ho_Line);
        //    HOperatorSet.GenEmptyObj(out ho_CrossObject);
        //    HOperatorSet.GenEmptyObj(out ho_Rectangle);
        //    HOperatorSet.GenEmptyObj(out ho_Cross);
        //    hv_DistanceArray = new HTuple();
        //    try
        //    {
        //        try
        //        {
        //            ho_ImageOut.Dispose();
        //            ho_ImageOut = new HObject(ho_Image);
        //            ho_showRegion.Dispose();
        //            HOperatorSet.GenEmptyObj(out ho_showRegion);
        //            ho_ImageReduced.Dispose();
        //            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
        //            //1、找耳朵边
        //            ho_Region.Dispose();
        //            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 200, 255);
        //            ho_RegionFillUp.Dispose();
        //            HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
        //            ho_RegionOpening.Dispose();
        //            HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, 5);
        //            ho_ConnectedRegions1.Dispose();
        //            HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions1);
        //            ho_SelectedRegions1.Dispose();
        //            HOperatorSet.SelectShapeStd(ho_ConnectedRegions1, out ho_SelectedRegions1,
        //                "max_area", 70);


        //            //
        //            ho_RegionMoved.Dispose();
        //            HOperatorSet.MoveRegion(ho_SelectedRegions1, out ho_RegionMoved, 0, 1);
        //            ho_RegionDifference.Dispose();
        //            HOperatorSet.Difference(ho_SelectedRegions1, ho_RegionMoved, out ho_RegionDifference
        //                );
        //            ho_ConnectedRegions.Dispose();
        //            HOperatorSet.Connection(ho_RegionDifference, out ho_ConnectedRegions);
        //            ho_SelectedRegions.Dispose();
        //            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions,
        //                "max_area", 70);

        //            ho_RegionClipped.Dispose();
        //            HOperatorSet.ClipRegionRel(ho_SelectedRegions, out ho_RegionClipped, 35,
        //                0, 0, 0);
        //            ho_Contours.Dispose();
        //            HOperatorSet.GenContourRegionXld(ho_RegionClipped, out ho_Contours, "center");

        //            hv_Rows.Dispose(); hv_Columns.Dispose();
        //            HOperatorSet.GetRegionPoints(ho_RegionClipped, out hv_Rows, out hv_Columns);
        //            ho_Line.Dispose(); hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose();
        //            pts_to_best_line(out ho_Line, hv_Rows, hv_Columns, 20, out hv_RowBegin, out hv_ColBegin,
        //                out hv_RowEnd, out hv_ColEnd);


        //            ho_ImageReduced.Dispose();
        //            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_1_COPY_INP_TMP, out ho_ImageReduced
        //                );
        //            //从左往右找边缘点

        //            hv_offsetRow1.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_offsetRow1 = 0.9 / hv_Pix2MM;
        //            }
        //            hv_offsetRow2.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_offsetRow2 = 1.9 / hv_Pix2MM;
        //            }

        //            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
        //            HOperatorSet.SmallestRectangle1(ho_ROI_1_COPY_INP_TMP, out hv_Row1, out hv_Column1,
        //                out hv_Row2, out hv_Column2);

        //            hv_errBodyR1.Dispose(); hv_errBodyC1.Dispose(); hv_errBodyR2.Dispose(); hv_errBodyC2.Dispose();
        //            HOperatorSet.SmallestRectangle1(ho_SelectedRegions1, out hv_errBodyR1, out hv_errBodyC1,
        //                out hv_errBodyR2, out hv_errBodyC2);

        //            ho_CrossObject.Dispose();
        //            HOperatorSet.GenEmptyObj(out ho_CrossObject);
        //            hv_findRowArray.Dispose();
        //            hv_findRowArray = new HTuple();
        //            hv_findColArray.Dispose();
        //            hv_findColArray = new HTuple();

        //            hv_AmplitudeThreshold.Dispose();
        //            hv_AmplitudeThreshold = 50;
        //            hv_Sigma.Dispose();
        //            hv_Sigma = 3;
        //            hv_RoiWidthLen.Dispose();
        //            hv_RoiWidthLen = 100;

        //            for (hv_Index = 0; (int)hv_Index <= 1; hv_Index = (int)hv_Index + 1)
        //            {
        //                if ((int)(new HTuple(hv_Index.TupleEqual(0))) != 0)
        //                {
        //                    hv_lineCoord.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_lineCoord = new HTuple();
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_errBodyR1 + hv_offsetRow1);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column1);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_errBodyR1 + hv_offsetRow1);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column2);
        //                    }

        //                }
        //                else
        //                {
        //                    hv_lineCoord.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_lineCoord = new HTuple();
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_errBodyR1 + hv_offsetRow2);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column1);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_errBodyR1 + hv_offsetRow2);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column2);
        //                    }

        //                }

        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    ho_Rectangle.Dispose();
        //                    HOperatorSet.GenRectangle1(out ho_Rectangle, (hv_Row1 + 60) + (hv_Index * 120),
        //                        hv_Column1, (hv_Row1 + 60) + (hv_Index * 120), hv_Column2);
        //                }
        //                hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
        //                measure_1D(ho_Image, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen, hv_lineCoord,
        //                    out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);
        //                hv_Indices1.Dispose();
        //                HOperatorSet.TupleSortIndex(hv_Column_Measure_01_0, out hv_Indices1);
        //                hv_findRow.Dispose();
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_findRow = hv_Row_Measure_01_0.TupleSelect(
        //                        hv_Indices1.TupleSelect(0));
        //                }
        //                hv_findCol.Dispose();
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_findCol = hv_Column_Measure_01_0.TupleSelect(
        //                        hv_Indices1.TupleSelect(0));
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    {
        //                        HTuple
        //                          ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
        //                            hv_findRow);
        //                        hv_findRowArray.Dispose();
        //                        hv_findRowArray = ExpTmpLocalVar_findRowArray;
        //                    }
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    {
        //                        HTuple
        //                          ExpTmpLocalVar_findColArray = hv_findColArray.TupleConcat(
        //                            hv_findCol);
        //                        hv_findColArray.Dispose();
        //                        hv_findColArray = ExpTmpLocalVar_findColArray;
        //                    }
        //                }
        //                ho_Cross.Dispose();
        //                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol, 26,
        //                    0.785398);
        //                //gen_region_contour_xld (Cross, Cross, 'margin')
        //                {
        //                    HObject ExpTmpOutVar_0;
        //                    HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
        //                    ho_CrossObject.Dispose();
        //                    ho_CrossObject = ExpTmpOutVar_0;
        //                }
        //            }

        //            hv_DistanceArray.Dispose();
        //            hv_DistanceArray = new HTuple();
        //            for (hv_Index1 = 0; (int)hv_Index1 <= 1; hv_Index1 = (int)hv_Index1 + 1)
        //            {
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_Distance.Dispose();
        //                    HOperatorSet.DistancePl(hv_findRowArray.TupleSelect(hv_Index1), hv_findColArray.TupleSelect(
        //                        hv_Index1), hv_RowBegin, hv_ColBegin, hv_RowEnd, hv_ColEnd, out hv_Distance);
        //                }
        //                if (hv_DistanceArray == null)
        //                    hv_DistanceArray = new HTuple();
        //                hv_DistanceArray[hv_Index1] = hv_Distance;
        //            }

        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegion, ho_CrossObject, out ExpTmpOutVar_0);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.Boundary(ho_ROI_1_COPY_INP_TMP, out ExpTmpOutVar_0, "inner");
        //                ho_ROI_1_COPY_INP_TMP.Dispose();
        //                ho_ROI_1_COPY_INP_TMP = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegion, ho_ROI_1_COPY_INP_TMP, out ExpTmpOutVar_0
        //                    );
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegion, ho_Line, out ExpTmpOutVar_0);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }



        //        }
        //        // catch (Exception) 
        //        catch (HalconException HDevExpDefaultException1)
        //        {
        //            HDevExpDefaultException1.ToHTuple(out hv_Exception);
        //        }

        //        ho_ROI_1_COPY_INP_TMP.Dispose();
        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_RegionFillUp.Dispose();
        //        ho_RegionOpening.Dispose();
        //        ho_ConnectedRegions1.Dispose();
        //        ho_SelectedRegions1.Dispose();
        //        ho_RegionMoved.Dispose();
        //        ho_RegionDifference.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_RegionClipped.Dispose();
        //        ho_Contours.Dispose();
        //        ho_Line.Dispose();
        //        ho_CrossObject.Dispose();
        //        ho_Rectangle.Dispose();
        //        ho_Cross.Dispose();

        //        hv_Rows.Dispose();
        //        hv_Columns.Dispose();
        //        hv_RowBegin.Dispose();
        //        hv_ColBegin.Dispose();
        //        hv_RowEnd.Dispose();
        //        hv_ColEnd.Dispose();
        //        hv_offsetRow1.Dispose();
        //        hv_offsetRow2.Dispose();
        //        hv_Row1.Dispose();
        //        hv_Column1.Dispose();
        //        hv_Row2.Dispose();
        //        hv_Column2.Dispose();
        //        hv_errBodyR1.Dispose();
        //        hv_errBodyC1.Dispose();
        //        hv_errBodyR2.Dispose();
        //        hv_errBodyC2.Dispose();
        //        hv_findRowArray.Dispose();
        //        hv_findColArray.Dispose();
        //        hv_AmplitudeThreshold.Dispose();
        //        hv_Sigma.Dispose();
        //        hv_RoiWidthLen.Dispose();
        //        hv_Index.Dispose();
        //        hv_lineCoord.Dispose();
        //        hv_Row_Measure_01_0.Dispose();
        //        hv_Column_Measure_01_0.Dispose();
        //        hv_Amplitude_Measure_01_0.Dispose();
        //        hv_Indices1.Dispose();
        //        hv_findRow.Dispose();
        //        hv_findCol.Dispose();
        //        hv_Index1.Dispose();
        //        hv_Distance.Dispose();
        //        hv_Exception.Dispose();

        //        return;
        //    }
        //    catch (HalconException HDevExpDefaultException)
        //    {
        //        ho_ROI_1_COPY_INP_TMP.Dispose();
        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_RegionFillUp.Dispose();
        //        ho_RegionOpening.Dispose();
        //        ho_ConnectedRegions1.Dispose();
        //        ho_SelectedRegions1.Dispose();
        //        ho_RegionMoved.Dispose();
        //        ho_RegionDifference.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_RegionClipped.Dispose();
        //        ho_Contours.Dispose();
        //        ho_Line.Dispose();
        //        ho_CrossObject.Dispose();
        //        ho_Rectangle.Dispose();
        //        ho_Cross.Dispose();

        //        hv_Rows.Dispose();
        //        hv_Columns.Dispose();
        //        hv_RowBegin.Dispose();
        //        hv_ColBegin.Dispose();
        //        hv_RowEnd.Dispose();
        //        hv_ColEnd.Dispose();
        //        hv_offsetRow1.Dispose();
        //        hv_offsetRow2.Dispose();
        //        hv_Row1.Dispose();
        //        hv_Column1.Dispose();
        //        hv_Row2.Dispose();
        //        hv_Column2.Dispose();
        //        hv_errBodyR1.Dispose();
        //        hv_errBodyC1.Dispose();
        //        hv_errBodyR2.Dispose();
        //        hv_errBodyC2.Dispose();
        //        hv_findRowArray.Dispose();
        //        hv_findColArray.Dispose();
        //        hv_AmplitudeThreshold.Dispose();
        //        hv_Sigma.Dispose();
        //        hv_RoiWidthLen.Dispose();
        //        hv_Index.Dispose();
        //        hv_lineCoord.Dispose();
        //        hv_Row_Measure_01_0.Dispose();
        //        hv_Column_Measure_01_0.Dispose();
        //        hv_Amplitude_Measure_01_0.Dispose();
        //        hv_Indices1.Dispose();
        //        hv_findRow.Dispose();
        //        hv_findCol.Dispose();
        //        hv_Index1.Dispose();
        //        hv_Distance.Dispose();
        //        hv_Exception.Dispose();

        //        throw HDevExpDefaultException;
        //    }
        //}

        // Local procedures 
        // Local procedures 
        public void Antenna_shrapnel(HObject ho_InputImage, HObject ho_ROI_0, HObject ho_ROI_1,
            out HObject ho_ImageOut, out HObject ho_ShowRegion, HTuple hv_Pix2mm, out HTuple hv_DistanceArr)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageResult, ho_ImageOpening, ho_Regions;
            HObject ho_Line1, ho_Line2, ho_Line, ho_Cross1, ho_Cross2;
            HObject ho_Cross3, ho_Cross4, ho_ObjectsConcat;

            // Local control variables 

            HTuple hv_Value = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Phi = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_CornerY = new HTuple(), hv_CornerX = new HTuple();
            HTuple hv_LineCenterY = new HTuple(), hv_LineCenterX = new HTuple();
            HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple();
            HTuple hv_Row11 = new HTuple(), hv_Column11 = new HTuple();
            HTuple hv_Row12 = new HTuple(), hv_Column12 = new HTuple();
            HTuple hv_Angle1 = new HTuple(), hv_ResultRow1 = new HTuple();
            HTuple hv_ResultColumn1 = new HTuple(), hv_Row21 = new HTuple();
            HTuple hv_Column21 = new HTuple(), hv_Row22 = new HTuple();
            HTuple hv_Column22 = new HTuple(), hv_Angle2 = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Phi1 = new HTuple(), hv_Length11 = new HTuple();
            HTuple hv_Length21 = new HTuple(), hv_CornerY1 = new HTuple();
            HTuple hv_CornerX1 = new HTuple(), hv_LineCenterY1 = new HTuple();
            HTuple hv_LineCenterX1 = new HTuple(), hv_ResultRow2 = new HTuple();
            HTuple hv_ResultColumn2 = new HTuple(), hv_Row31 = new HTuple();
            HTuple hv_Column31 = new HTuple(), hv_Row32 = new HTuple();
            HTuple hv_Column32 = new HTuple(), hv_InterRow1 = new HTuple();
            HTuple hv_InterCol1 = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_InterRow2 = new HTuple(), hv_InterCol2 = new HTuple();
            HTuple hv_OutRow1 = new HTuple(), hv_OutCol1 = new HTuple();
            HTuple hv_OutRow2 = new HTuple(), hv_OutCol2 = new HTuple();
            HTuple hv_RowProj1 = new HTuple(), hv_ColProj1 = new HTuple();
            HTuple hv_RowProj2 = new HTuple(), hv_ColProj2 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageOut);
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Line1);
            HOperatorSet.GenEmptyObj(out ho_Line2);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Cross3);
            HOperatorSet.GenEmptyObj(out ho_Cross4);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            hv_DistanceArr = new HTuple();


            hv_Value.Dispose();
            HOperatorSet.GrayFeatures(ho_ROI_0, ho_InputImage, "mean", out hv_Value);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_ImageResult.Dispose();
                HOperatorSet.MultImage(ho_InputImage, ho_InputImage, out ho_ImageResult, 255 / (hv_Value * hv_Value),
                    0);
            }
            ho_ImageOpening.Dispose();
            HOperatorSet.GrayOpeningRect(ho_ImageResult, out ho_ImageOpening, 11, 1);

            hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
            HOperatorSet.SmallestRectangle2(ho_ROI_0, out hv_Row, out hv_Column, out hv_Phi,
                out hv_Length1, out hv_Length2);
            hv_CornerY.Dispose(); hv_CornerX.Dispose(); hv_LineCenterY.Dispose(); hv_LineCenterX.Dispose();
            get_rectangle2_points(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2, out hv_CornerY,
                out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);
            //*第一条线
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
                rake(ho_ImageOpening, out ho_Regions, (hv_Length1 + 300) / 2, hv_Length2 * 2, 15, 1,
                    40, "positive", "first", (hv_LineCenterY.TupleSelect(0)) - 300, hv_LineCenterX.TupleSelect(
                    0), hv_LineCenterY.TupleSelect(2), hv_LineCenterX.TupleSelect(2), out hv_ResultRow,
                    out hv_ResultColumn);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Line1.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row12.Dispose(); hv_Column12.Dispose();
                pts_to_best_line(out ho_Line1, hv_ResultRow, hv_ResultColumn, hv_Length1 / 20,
                    out hv_Row11, out hv_Column11, out hv_Row12, out hv_Column12);
            }
            if ((int)(new HTuple((new HTuple(hv_Row11.TupleLength())).TupleEqual(0))) != 0)
            {
                ho_ShowRegion.Dispose();
                HOperatorSet.ConcatObj(ho_ROI_0, ho_ROI_1, out ho_ShowRegion);
                ho_ImageResult.Dispose();
                ho_ImageOpening.Dispose();
                ho_Regions.Dispose();
                ho_Line1.Dispose();
                ho_Line2.Dispose();
                ho_Line.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_Cross3.Dispose();
                ho_Cross4.Dispose();
                ho_ObjectsConcat.Dispose();

                hv_Value.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Phi.Dispose();
                hv_Length1.Dispose();
                hv_Length2.Dispose();
                hv_CornerY.Dispose();
                hv_CornerX.Dispose();
                hv_LineCenterY.Dispose();
                hv_LineCenterX.Dispose();
                hv_ResultRow.Dispose();
                hv_ResultColumn.Dispose();
                hv_Row11.Dispose();
                hv_Column11.Dispose();
                hv_Row12.Dispose();
                hv_Column12.Dispose();
                hv_Angle1.Dispose();
                hv_ResultRow1.Dispose();
                hv_ResultColumn1.Dispose();
                hv_Row21.Dispose();
                hv_Column21.Dispose();
                hv_Row22.Dispose();
                hv_Column22.Dispose();
                hv_Angle2.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Phi1.Dispose();
                hv_Length11.Dispose();
                hv_Length21.Dispose();
                hv_CornerY1.Dispose();
                hv_CornerX1.Dispose();
                hv_LineCenterY1.Dispose();
                hv_LineCenterX1.Dispose();
                hv_ResultRow2.Dispose();
                hv_ResultColumn2.Dispose();
                hv_Row31.Dispose();
                hv_Column31.Dispose();
                hv_Row32.Dispose();
                hv_Column32.Dispose();
                hv_InterRow1.Dispose();
                hv_InterCol1.Dispose();
                hv_IsOverlapping.Dispose();
                hv_InterRow2.Dispose();
                hv_InterCol2.Dispose();
                hv_OutRow1.Dispose();
                hv_OutCol1.Dispose();
                hv_OutRow2.Dispose();
                hv_OutCol2.Dispose();
                hv_RowProj1.Dispose();
                hv_ColProj1.Dispose();
                hv_RowProj2.Dispose();
                hv_ColProj2.Dispose();
                hv_Distance.Dispose();
                hv_Distance1.Dispose();

                return;
            }
            hv_Angle1.Dispose();
            HOperatorSet.AngleLx(hv_Row11, hv_Column11, hv_Row12, hv_Column12, out hv_Angle1);
            //*第二条线
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow1.Dispose(); hv_ResultColumn1.Dispose();
                rake(ho_InputImage, out ho_Regions, hv_Length1 / 2, hv_Length2 * 2, 15, 1, 40, "positive",
                    "last", hv_LineCenterY.TupleSelect(0), hv_LineCenterX.TupleSelect(0), hv_LineCenterY.TupleSelect(
                    2), hv_LineCenterX.TupleSelect(2), out hv_ResultRow1, out hv_ResultColumn1);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Line2.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose(); hv_Row22.Dispose(); hv_Column22.Dispose();
                pts_to_best_line(out ho_Line2, hv_ResultRow1, hv_ResultColumn1, hv_Length1 / 20,
                    out hv_Row21, out hv_Column21, out hv_Row22, out hv_Column22);
            }
            if ((int)(new HTuple((new HTuple(hv_Row21.TupleLength())).TupleEqual(0))) != 0)
            {
                ho_ShowRegion.Dispose();
                HOperatorSet.ConcatObj(ho_ROI_0, ho_ROI_1, out ho_ShowRegion);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Line1, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                ho_ImageResult.Dispose();
                ho_ImageOpening.Dispose();
                ho_Regions.Dispose();
                ho_Line1.Dispose();
                ho_Line2.Dispose();
                ho_Line.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_Cross3.Dispose();
                ho_Cross4.Dispose();
                ho_ObjectsConcat.Dispose();

                hv_Value.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Phi.Dispose();
                hv_Length1.Dispose();
                hv_Length2.Dispose();
                hv_CornerY.Dispose();
                hv_CornerX.Dispose();
                hv_LineCenterY.Dispose();
                hv_LineCenterX.Dispose();
                hv_ResultRow.Dispose();
                hv_ResultColumn.Dispose();
                hv_Row11.Dispose();
                hv_Column11.Dispose();
                hv_Row12.Dispose();
                hv_Column12.Dispose();
                hv_Angle1.Dispose();
                hv_ResultRow1.Dispose();
                hv_ResultColumn1.Dispose();
                hv_Row21.Dispose();
                hv_Column21.Dispose();
                hv_Row22.Dispose();
                hv_Column22.Dispose();
                hv_Angle2.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Phi1.Dispose();
                hv_Length11.Dispose();
                hv_Length21.Dispose();
                hv_CornerY1.Dispose();
                hv_CornerX1.Dispose();
                hv_LineCenterY1.Dispose();
                hv_LineCenterX1.Dispose();
                hv_ResultRow2.Dispose();
                hv_ResultColumn2.Dispose();
                hv_Row31.Dispose();
                hv_Column31.Dispose();
                hv_Row32.Dispose();
                hv_Column32.Dispose();
                hv_InterRow1.Dispose();
                hv_InterCol1.Dispose();
                hv_IsOverlapping.Dispose();
                hv_InterRow2.Dispose();
                hv_InterCol2.Dispose();
                hv_OutRow1.Dispose();
                hv_OutCol1.Dispose();
                hv_OutRow2.Dispose();
                hv_OutCol2.Dispose();
                hv_RowProj1.Dispose();
                hv_ColProj1.Dispose();
                hv_RowProj2.Dispose();
                hv_ColProj2.Dispose();
                hv_Distance.Dispose();
                hv_Distance1.Dispose();

                return;
            }
            hv_Angle2.Dispose();
            HOperatorSet.AngleLx(hv_Row21, hv_Column21, hv_Row22, hv_Column22, out hv_Angle2);
            //*基准面
            //gen_rectangle1 (ROI_1, 1001.11, 723.451, 1094.45, 950.27)
            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Phi1.Dispose(); hv_Length11.Dispose(); hv_Length21.Dispose();
            HOperatorSet.SmallestRectangle2(ho_ROI_1, out hv_Row1, out hv_Column1, out hv_Phi1,
                out hv_Length11, out hv_Length21);
            hv_CornerY1.Dispose(); hv_CornerX1.Dispose(); hv_LineCenterY1.Dispose(); hv_LineCenterX1.Dispose();
            get_rectangle2_points(hv_Row1, hv_Column1, hv_Phi1, hv_Length11, hv_Length21,
                out hv_CornerY1, out hv_CornerX1, out hv_LineCenterY1, out hv_LineCenterX1);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow2.Dispose(); hv_ResultColumn2.Dispose();
                rake(ho_InputImage, out ho_Regions, hv_Length11 / 2, hv_Length21 * 3, 15, 1, 40,
                    "negative", "max", hv_LineCenterY1.TupleSelect(3), hv_LineCenterX1.TupleSelect(
                    3), hv_LineCenterY1.TupleSelect(1), hv_LineCenterX1.TupleSelect(1), out hv_ResultRow2,
                    out hv_ResultColumn2);
            }
            ho_Line.Dispose(); hv_Row31.Dispose(); hv_Column31.Dispose(); hv_Row32.Dispose(); hv_Column32.Dispose();
            pts_to_best_line(out ho_Line, hv_ResultRow2, hv_ResultColumn2, 2, out hv_Row31,
                out hv_Column31, out hv_Row32, out hv_Column32);
            if ((int)(new HTuple((new HTuple(hv_Row31.TupleLength())).TupleEqual(0))) != 0)
            {
                ho_ShowRegion.Dispose();
                HOperatorSet.ConcatObj(ho_ROI_0, ho_ROI_1, out ho_ShowRegion);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Line1, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Line2, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                ho_ImageResult.Dispose();
                ho_ImageOpening.Dispose();
                ho_Regions.Dispose();
                ho_Line1.Dispose();
                ho_Line2.Dispose();
                ho_Line.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_Cross3.Dispose();
                ho_Cross4.Dispose();
                ho_ObjectsConcat.Dispose();

                hv_Value.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Phi.Dispose();
                hv_Length1.Dispose();
                hv_Length2.Dispose();
                hv_CornerY.Dispose();
                hv_CornerX.Dispose();
                hv_LineCenterY.Dispose();
                hv_LineCenterX.Dispose();
                hv_ResultRow.Dispose();
                hv_ResultColumn.Dispose();
                hv_Row11.Dispose();
                hv_Column11.Dispose();
                hv_Row12.Dispose();
                hv_Column12.Dispose();
                hv_Angle1.Dispose();
                hv_ResultRow1.Dispose();
                hv_ResultColumn1.Dispose();
                hv_Row21.Dispose();
                hv_Column21.Dispose();
                hv_Row22.Dispose();
                hv_Column22.Dispose();
                hv_Angle2.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Phi1.Dispose();
                hv_Length11.Dispose();
                hv_Length21.Dispose();
                hv_CornerY1.Dispose();
                hv_CornerX1.Dispose();
                hv_LineCenterY1.Dispose();
                hv_LineCenterX1.Dispose();
                hv_ResultRow2.Dispose();
                hv_ResultColumn2.Dispose();
                hv_Row31.Dispose();
                hv_Column31.Dispose();
                hv_Row32.Dispose();
                hv_Column32.Dispose();
                hv_InterRow1.Dispose();
                hv_InterCol1.Dispose();
                hv_IsOverlapping.Dispose();
                hv_InterRow2.Dispose();
                hv_InterCol2.Dispose();
                hv_OutRow1.Dispose();
                hv_OutCol1.Dispose();
                hv_OutRow2.Dispose();
                hv_OutCol2.Dispose();
                hv_RowProj1.Dispose();
                hv_ColProj1.Dispose();
                hv_RowProj2.Dispose();
                hv_ColProj2.Dispose();
                hv_Distance.Dispose();
                hv_Distance1.Dispose();

                return;
            }
            //*交点1
            hv_InterRow1.Dispose(); hv_InterCol1.Dispose(); hv_IsOverlapping.Dispose();
            HOperatorSet.IntersectionLines(hv_Row11, hv_Column11, hv_Row12, hv_Column12,
                hv_Row31, hv_Column31, hv_Row32, hv_Column32, out hv_InterRow1, out hv_InterCol1,
                out hv_IsOverlapping);
            //*交点2
            hv_InterRow2.Dispose(); hv_InterCol2.Dispose(); hv_IsOverlapping.Dispose();
            HOperatorSet.IntersectionLines(hv_Row21, hv_Column21, hv_Row22, hv_Column22,
                hv_Row31, hv_Column31, hv_Row32, hv_Column32, out hv_InterRow2, out hv_InterCol2,
                out hv_IsOverlapping);

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_InterRow1, hv_InterCol1, 26,
                    (new HTuple(45)).TupleRad());
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross2.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_InterRow2, hv_InterCol2, 26,
                    (new HTuple(45)).TupleRad());
            }

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_OutRow1.Dispose(); hv_OutCol1.Dispose();
                gen_point_angle_distance(hv_InterRow2, hv_InterCol2, hv_Angle2, -0.9 / hv_Pix2mm,
                    out hv_OutRow1, out hv_OutCol1);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_OutRow2.Dispose(); hv_OutCol2.Dispose();
                gen_point_angle_distance(hv_InterRow2, hv_InterCol2, hv_Angle2, -1.9 / hv_Pix2mm,
                    out hv_OutRow2, out hv_OutCol2);
            }

            //*显示 拿值
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross3.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_OutRow1, hv_OutCol1, 26, (new HTuple(45)).TupleRad()
                    );
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross4.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_OutRow2, hv_OutCol2, 26, (new HTuple(45)).TupleRad()
                    );
            }

            hv_RowProj1.Dispose(); hv_ColProj1.Dispose();
            HOperatorSet.ProjectionPl(hv_OutRow1, hv_OutCol1, hv_Row11, hv_Column11, hv_Row12,
                hv_Column12, out hv_RowProj1, out hv_ColProj1);
            hv_RowProj2.Dispose(); hv_ColProj2.Dispose();
            HOperatorSet.ProjectionPl(hv_OutRow2, hv_OutCol2, hv_Row11, hv_Column11, hv_Row12,
                hv_Column12, out hv_RowProj2, out hv_ColProj2);

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_RowProj1, hv_ColProj1, 26,
                    (new HTuple(45)).TupleRad());
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross2.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_RowProj2, hv_ColProj2, 26,
                    (new HTuple(45)).TupleRad());
            }

            hv_Distance.Dispose();
            HOperatorSet.DistancePl(hv_OutRow1, hv_OutCol1, hv_Row11, hv_Column11, hv_Row12,
                hv_Column12, out hv_Distance);
            hv_Distance1.Dispose();
            HOperatorSet.DistancePl(hv_OutCol2, hv_OutCol2, hv_Row11, hv_Column11, hv_Row12,
                hv_Column12, out hv_Distance1);
            hv_DistanceArr.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_DistanceArr = new HTuple();
                hv_DistanceArr = hv_DistanceArr.TupleConcat(hv_Distance, hv_Distance1);
            }
            ho_ObjectsConcat.Dispose();
            HOperatorSet.ConcatObj(ho_Line1, ho_Cross1, out ho_ObjectsConcat);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross2, out ExpTmpOutVar_0);
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross3, out ExpTmpOutVar_0);
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat = ExpTmpOutVar_0;
            }
            ho_ShowRegion.Dispose();
            HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross4, out ho_ShowRegion);

            ho_ImageResult.Dispose();
            ho_ImageOpening.Dispose();
            ho_Regions.Dispose();
            ho_Line1.Dispose();
            ho_Line2.Dispose();
            ho_Line.Dispose();
            ho_Cross1.Dispose();
            ho_Cross2.Dispose();
            ho_Cross3.Dispose();
            ho_Cross4.Dispose();
            ho_ObjectsConcat.Dispose();

            hv_Value.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Phi.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
            hv_CornerY.Dispose();
            hv_CornerX.Dispose();
            hv_LineCenterY.Dispose();
            hv_LineCenterX.Dispose();
            hv_ResultRow.Dispose();
            hv_ResultColumn.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row12.Dispose();
            hv_Column12.Dispose();
            hv_Angle1.Dispose();
            hv_ResultRow1.Dispose();
            hv_ResultColumn1.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_Row22.Dispose();
            hv_Column22.Dispose();
            hv_Angle2.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Phi1.Dispose();
            hv_Length11.Dispose();
            hv_Length21.Dispose();
            hv_CornerY1.Dispose();
            hv_CornerX1.Dispose();
            hv_LineCenterY1.Dispose();
            hv_LineCenterX1.Dispose();
            hv_ResultRow2.Dispose();
            hv_ResultColumn2.Dispose();
            hv_Row31.Dispose();
            hv_Column31.Dispose();
            hv_Row32.Dispose();
            hv_Column32.Dispose();
            hv_InterRow1.Dispose();
            hv_InterCol1.Dispose();
            hv_IsOverlapping.Dispose();
            hv_InterRow2.Dispose();
            hv_InterCol2.Dispose();
            hv_OutRow1.Dispose();
            hv_OutCol1.Dispose();
            hv_OutRow2.Dispose();
            hv_OutCol2.Dispose();
            hv_RowProj1.Dispose();
            hv_ColProj1.Dispose();
            hv_RowProj2.Dispose();
            hv_ColProj2.Dispose();
            hv_Distance.Dispose();
            hv_Distance1.Dispose();

            return;
        }


        public void gen_point_angle_distance(HTuple hv_BaseRow, HTuple hv_BaseCol, HTuple hv_Angle,
            HTuple hv_Distance, out HTuple hv_OutRow, out HTuple hv_OutCol)
        {



            // Local iconic variables 
            // Initialize local and output iconic variables 
            hv_OutRow = new HTuple();
            hv_OutCol = new HTuple();
            hv_OutRow.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_OutRow = hv_BaseRow + (hv_Distance * (hv_Angle.TupleSin()
                    ));
            }
            hv_OutCol.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_OutCol = hv_BaseCol - (hv_Distance * (hv_Angle.TupleCos()
                    ));
            }


            return;
        }


        public void ConnectorBuckleLzj(HObject ho_Image, HObject ho_ROI, out HObject ho_ShowRegion,
            out HObject ho_OutImage, out HTuple hv_result)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageMean = null, ho_ImageReduced = null;
            HObject ho_Region = null, ho_RegionClosing = null, ho_RegionOpening = null;
            HObject ho_ConnectedRegions1 = null, ho_SelectedRegions1 = null;
            HObject ho_SortedRegions = null, ho_ObjectSelected = null, ho_Rectangle = null;
            HObject ho_ImageReduced1 = null, ho_Region2 = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_RegionUnion = null, ho_RegionClosing1 = null;
            HObject ho_ConnectedRegions2 = null, ho_SelectedRegions4 = null;

            // Local control variables 

            HTuple hv_Distances = new HTuple(), hv_Mean = new HTuple();
            HTuple hv_Deviation = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_OutImage);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions4);
            hv_result = new HTuple();
            try
            {
                try
                {
                    hv_result.Dispose();
                    hv_result = 0;
                    hv_Distances.Dispose();
                    hv_Distances = new HTuple();
                    ho_ShowRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ShowRegion);

                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_Image, out ho_ImageMean, 9, 9);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageMean, ho_ROI, out ho_ImageReduced);
                    hv_Mean.Dispose(); hv_Deviation.Dispose();
                    HOperatorSet.Intensity(ho_ROI, ho_ImageReduced, out hv_Mean, out hv_Deviation);
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 220, 255);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_Region, out ho_RegionClosing, 10, 50);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionClosing, out ho_RegionOpening, 10,
                        50);
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions1);
                    ho_SelectedRegions1.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, (new HTuple("area")).TupleConcat(
                        "height"), "and", (new HTuple(20000)).TupleConcat(1000), (new HTuple(999999)).TupleConcat(
                        9999));
                    ho_SortedRegions.Dispose();
                    HOperatorSet.SortRegion(ho_SelectedRegions1, out ho_SortedRegions, "first_point",
                        "false", "column");
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected, 1);
                    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_ObjectSelected, out hv_Row1, out hv_Column1,
                        out hv_Row2, out hv_Column2);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1 - 300, hv_Row2,
                            hv_Column1 - 180);
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ShowRegion, ho_Rectangle, out ExpTmpOutVar_0);
                        ho_ShowRegion.Dispose();
                        ho_ShowRegion = ExpTmpOutVar_0;
                    }
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageReduced, ho_Rectangle, out ho_ImageReduced1
                        );
                    ho_Region2.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region2, 220, 255);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                        "and", 50, 9999999);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
                    ho_RegionClosing1.Dispose();
                    HOperatorSet.ClosingRectangle1(ho_RegionUnion, out ho_RegionClosing1, 20,
                        80);
                    ho_ConnectedRegions2.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing1, out ho_ConnectedRegions2);
                    ho_SelectedRegions4.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions2, out ho_SelectedRegions4, (new HTuple("area")).TupleConcat(
                        "width"), "and", (new HTuple(80000)).TupleConcat(80), (new HTuple(999999)).TupleConcat(
                        150));

                    hv_Number1.Dispose();
                    HOperatorSet.CountObj(ho_SelectedRegions4, out hv_Number1);
                    if ((int)(new HTuple(hv_Number1.TupleEqual(0))) != 0)
                    {
                        hv_result.Dispose();
                        hv_result = 1;
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Distances.Dispose();
                    hv_Distances = new HTuple();
                    hv_Distances[0] = 0;
                    hv_Distances[1] = 0;
                    hv_result.Dispose();
                    hv_result = 1;
                }
                ho_ImageMean.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region2.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions4.Dispose();

                hv_Distances.Dispose();
                hv_Mean.Dispose();
                hv_Deviation.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Number1.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageMean.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                ho_SortedRegions.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region2.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions2.Dispose();
                ho_SelectedRegions4.Dispose();

                hv_Distances.Dispose();
                hv_Mean.Dispose();
                hv_Deviation.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Number1.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void Connector_buckle(HObject ho_Image, HObject ho_ROI, out HObject ho_ShowRegion,
    out HTuple hv_Result)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageResult, ho_Regions, ho_Line;
            HObject ho_Region, ho_RegionFillUp, ho_ImageReduced, ho_Region1;
            HObject ho_ConnectedRegions, ho_SelectedRegions;

            // Local control variables 

            HTuple hv_Value = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_ResultRow = new HTuple();
            HTuple hv_ResultColumn = new HTuple(), hv_Row11 = new HTuple();
            HTuple hv_Column11 = new HTuple(), hv_Row21 = new HTuple();
            HTuple hv_Column21 = new HTuple(), hv_InterRow1 = new HTuple();
            HTuple hv_InterColumn1 = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_InterRow2 = new HTuple(), hv_InterColumn2 = new HTuple();
            HTuple hv_Row12 = new HTuple(), hv_Column12 = new HTuple();
            HTuple hv_Row22 = new HTuple(), hv_Column22 = new HTuple();
            HTuple hv_percent = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            hv_Result = new HTuple();
            ho_ShowRegion.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);

            hv_Value.Dispose();
            HOperatorSet.GrayFeatures(ho_ROI, ho_Image, "mean", out hv_Value);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_ImageResult.Dispose();
                HOperatorSet.MultImage(ho_Image, ho_Image, out ho_ImageResult, 100 / (hv_Value * hv_Value),
                    0);
            }

            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
            HOperatorSet.SmallestRectangle1(ho_ROI, out hv_Row1, out hv_Column1, out hv_Row2,
                out hv_Column2);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
                rake(ho_ImageResult, out ho_Regions, (hv_Row2 - hv_Row1) / 5, 100, 15, 1.2, 30, "all",
                    "last", hv_Row1, hv_Column2, hv_Row2, hv_Column2, out hv_ResultRow, out hv_ResultColumn);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Line.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose();
                pts_to_best_line(out ho_Line, hv_ResultRow, hv_ResultColumn, (hv_Row2 - hv_Row1) / 15,
                    out hv_Row11, out hv_Column11, out hv_Row21, out hv_Column21);
            }

            hv_InterRow1.Dispose(); hv_InterColumn1.Dispose(); hv_IsOverlapping.Dispose();
            HOperatorSet.IntersectionLines(hv_Row1, hv_Column1, hv_Row1, hv_Column2, hv_Row11,
                hv_Column11, hv_Row21, hv_Column21, out hv_InterRow1, out hv_InterColumn1,
                out hv_IsOverlapping);
            hv_InterRow2.Dispose(); hv_InterColumn2.Dispose(); hv_IsOverlapping.Dispose();
            HOperatorSet.IntersectionLines(hv_Row2, hv_Column1, hv_Row2, hv_Column2, hv_Row11,
                hv_Column11, hv_Row21, hv_Column21, out hv_InterRow2, out hv_InterColumn2,
                out hv_IsOverlapping);

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Region.Dispose();
                HOperatorSet.GenRegionPolygon(out ho_Region, ((((((hv_Row1.TupleConcat(hv_InterRow1))).TupleConcat(
                    hv_InterRow2))).TupleConcat(hv_Row2))).TupleConcat(hv_Row1), ((((((hv_Column1.TupleConcat(
                    hv_InterColumn1))).TupleConcat(hv_InterColumn2))).TupleConcat(hv_Column1))).TupleConcat(
                    hv_Column1));
            }
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_Region, out ho_RegionFillUp);
            ho_ShowRegion.Dispose();
            ho_ShowRegion = new HObject(ho_RegionFillUp);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionFillUp, out ho_ImageReduced);
            ho_Region1.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region1, 200, 255);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region1, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                70);
            hv_Row12.Dispose(); hv_Column12.Dispose(); hv_Row22.Dispose(); hv_Column22.Dispose();
            HOperatorSet.SmallestRectangle1(ho_SelectedRegions, out hv_Row12, out hv_Column12,
                out hv_Row22, out hv_Column22);


            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ShowRegion, ho_SelectedRegions, out ExpTmpOutVar_0);
                ho_ShowRegion.Dispose();
                ho_ShowRegion = ExpTmpOutVar_0;
            }
            hv_percent.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_percent = (((hv_Column22 - hv_Column12)).TupleAbs()
                    ) / ((((hv_Column2 - hv_Column1) * 1.000)).TupleAbs());
            }
            if ((int)(new HTuple(hv_percent.TupleGreater(0.5))) != 0)
            {

                hv_Result.Dispose();
                hv_Result = 0;
            }
            else
            {
                hv_Result.Dispose();
                hv_Result = 1;
            }
            ho_ImageResult.Dispose();
            ho_Regions.Dispose();
            ho_Line.Dispose();
            ho_Region.Dispose();
            ho_RegionFillUp.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region1.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();

            hv_Value.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_ResultRow.Dispose();
            hv_ResultColumn.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_InterRow1.Dispose();
            hv_InterColumn1.Dispose();
            hv_IsOverlapping.Dispose();
            hv_InterRow2.Dispose();
            hv_InterColumn2.Dispose();
            hv_Row12.Dispose();
            hv_Column12.Dispose();
            hv_Row22.Dispose();
            hv_Column22.Dispose();
            hv_percent.Dispose();

            return;
        }

        // Local procedures 
        //public void Connector_buckle(HObject ho_Image, HObject ho_ROI_0, out HObject ho_ShowRegion,
        //    out HTuple hv_Result)
        //{



        //    // Stack for temporary objects 
        //    HObject[] OTemp = new HObject[20];

        //    // Local iconic variables 

        //    HObject ho_Temp, ho_Regions, ho_Cross, ho_Line;
        //    HObject ho_Rectangle = null, ho_RegionIntersection = null, ho_ImageReduced = null;
        //    HObject ho_Region = null;

        //    // Local control variables 

        //    HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
        //    HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
        //    HTuple hv_Length2 = new HTuple(), hv_Value = new HTuple();
        //    HTuple hv_CornerY = new HTuple(), hv_CornerX = new HTuple();
        //    HTuple hv_LineCenterY = new HTuple(), hv_LineCenterX = new HTuple();
        //    HTuple hv_ResultRow = new HTuple(), hv_ResultColumn = new HTuple();
        //    HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
        //    HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
        //    HTuple hv_Area = new HTuple(), hv_Row3 = new HTuple();
        //    HTuple hv_Column3 = new HTuple(), hv_Area1 = new HTuple();
        //    HTuple hv_Row4 = new HTuple(), hv_Column4 = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_ShowRegion);
        //    HOperatorSet.GenEmptyObj(out ho_Temp);
        //    HOperatorSet.GenEmptyObj(out ho_Regions);
        //    HOperatorSet.GenEmptyObj(out ho_Cross);
        //    HOperatorSet.GenEmptyObj(out ho_Line);
        //    HOperatorSet.GenEmptyObj(out ho_Rectangle);
        //    HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
        //    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //    HOperatorSet.GenEmptyObj(out ho_Region);
        //    hv_Result = new HTuple();
        //    hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
        //    HOperatorSet.SmallestRectangle2(ho_ROI_0, out hv_Row, out hv_Column, out hv_Phi,
        //        out hv_Length1, out hv_Length2);
        //    hv_Value.Dispose();
        //    HOperatorSet.GrayFeatures(ho_ROI_0, ho_Image, "mean", out hv_Value);
        //    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //    {
        //        ho_Temp.Dispose();
        //        HOperatorSet.MultImage(ho_Image, ho_Image, out ho_Temp, 255 / (hv_Value * hv_Value),
        //            0);
        //    }
        //    {
        //        HObject ExpTmpOutVar_0;
        //        HOperatorSet.GrayOpeningRect(ho_Temp, out ExpTmpOutVar_0, 1, 7);
        //        ho_Temp.Dispose();
        //        ho_Temp = ExpTmpOutVar_0;
        //    }
        //    hv_CornerY.Dispose(); hv_CornerX.Dispose(); hv_LineCenterY.Dispose(); hv_LineCenterX.Dispose();
        //    get_rectangle2_points(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2, out hv_CornerY,
        //        out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);
        //    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //    {
        //        ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
        //        rake(ho_Temp, out ho_Regions, hv_Length1 / 2, hv_Length2 * 2, 15, 1, 20, "positive",
        //            "max", hv_LineCenterY.TupleSelect(0), hv_LineCenterX.TupleSelect(0), hv_LineCenterY.TupleSelect(
        //            2), hv_LineCenterX.TupleSelect(2), out hv_ResultRow, out hv_ResultColumn);
        //    }
        //    ho_Cross.Dispose();
        //    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow, hv_ResultColumn,
        //        26, hv_Phi);
        //    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //    {
        //        ho_Line.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
        //        pts_to_best_line(out ho_Line, hv_ResultRow, hv_ResultColumn, hv_Length1 / 5, out hv_Row1,
        //            out hv_Column1, out hv_Row2, out hv_Column2);
        //    }

        //    ho_ShowRegion.Dispose();
        //    HOperatorSet.ConcatObj(ho_ROI_0, ho_Line, out ho_ShowRegion);
        //    if (Math.Abs((hv_Column1.D + hv_Column2.D) / 2 - hv_Column.D) > 20 || hv_Row1.D == 0)
        //    {
        //        hv_Result.Dispose();
        //        hv_Result = 1;
        //    }
        //    else
        //    {
        //        using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //        {
        //            ho_Rectangle.Dispose();
        //            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2,
        //                ((hv_Column1 + hv_Column2) / 2) + hv_Length2);
        //        }
        //        ho_RegionIntersection.Dispose();
        //        HOperatorSet.Intersection(ho_ROI_0, ho_Rectangle, out ho_RegionIntersection);
        //        hv_Area.Dispose(); hv_Row3.Dispose(); hv_Column3.Dispose();
        //        HOperatorSet.AreaCenter(ho_RegionIntersection, out hv_Area, out hv_Row3, out hv_Column3);

        //        ho_ImageReduced.Dispose();
        //        HOperatorSet.ReduceDomain(ho_Image, ho_RegionIntersection, out ho_ImageReduced);
        //        ho_Region.Dispose();
        //        HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 200, 255);
        //        hv_Area1.Dispose(); hv_Row4.Dispose(); hv_Column4.Dispose();
        //        HOperatorSet.AreaCenter(ho_Region, out hv_Area1, out hv_Row4, out hv_Column4);
        //        if ((hv_Area1.D / hv_Area.D)>0.4)
        //        {
        //            hv_Result.Dispose();
        //            hv_Result = 1;
        //        }
        //        else
        //        {
        //            hv_Result.Dispose();
        //            hv_Result = 0;
        //        }
        //    }
        //    ho_Temp.Dispose();
        //    ho_Regions.Dispose();
        //    ho_Cross.Dispose();
        //    ho_Line.Dispose();
        //    ho_Rectangle.Dispose();
        //    ho_RegionIntersection.Dispose();
        //    ho_ImageReduced.Dispose();
        //    ho_Region.Dispose();

        //    hv_Row.Dispose();
        //    hv_Column.Dispose();
        //    hv_Phi.Dispose();
        //    hv_Length1.Dispose();
        //    hv_Length2.Dispose();
        //    hv_Value.Dispose();
        //    hv_CornerY.Dispose();
        //    hv_CornerX.Dispose();
        //    hv_LineCenterY.Dispose();
        //    hv_LineCenterX.Dispose();
        //    hv_ResultRow.Dispose();
        //    hv_ResultColumn.Dispose();
        //    hv_Row1.Dispose();
        //    hv_Column1.Dispose();
        //    hv_Row2.Dispose();
        //    hv_Column2.Dispose();
        //    hv_Area.Dispose();
        //    hv_Row3.Dispose();
        //    hv_Column3.Dispose();
        //    hv_Area1.Dispose();
        //    hv_Row4.Dispose();
        //    hv_Column4.Dispose();

        //    return;
        //}


        //public void Connector_buckle(HObject ho_Image, HObject ho_ROI_0, out HObject ho_showRegion,
        //    out HObject ho_ImageOut, HTuple hv_goldFingerThre, out HTuple hv_result)
        //{




        //    // Stack for temporary objects 
        //    HObject[] OTemp = new HObject[20];

        //    // Local iconic variables 

        //    HObject ho_ImageReduced = null, ho_Region = null;
        //    HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
        //    HObject ho_RegionDilation = null, ho_RegionUnion = null, ho_ConnectedRegions1 = null;
        //    HObject ho_RegionErosion = null, ho_SelectedRegions2 = null;

        //    // Local control variables 

        //    HTuple hv_Number = new HTuple(), hv_Row = new HTuple();
        //    HTuple hv_Column = new HTuple(), hv_Phi = new HTuple();
        //    HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
        //    HTuple hv_Q1 = new HTuple(), hv_Q2 = new HTuple(), hv_Q3 = new HTuple();
        //    HTuple hv_Q = new HTuple(), hv_Sum = new HTuple(), hv_average = new HTuple();
        //    HTuple hv_Channels = new HTuple(), hv_Pointer = new HTuple();
        //    HTuple hv_Type = new HTuple(), hv_Width = new HTuple();
        //    HTuple hv_Height = new HTuple(), hv_Exception = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_showRegion);
        //    HOperatorSet.GenEmptyObj(out ho_ImageOut);
        //    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //    HOperatorSet.GenEmptyObj(out ho_Region);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_RegionDilation);
        //    HOperatorSet.GenEmptyObj(out ho_RegionUnion);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
        //    HOperatorSet.GenEmptyObj(out ho_RegionErosion);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
        //    hv_result = new HTuple();
        //    try
        //    {
        //        try
        //        {
        //            hv_result.Dispose();
        //            hv_result = 0;
        //            ho_showRegion.Dispose();
        //            HOperatorSet.GenEmptyObj(out ho_showRegion);
        //            ho_ImageReduced.Dispose();
        //            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
        //            ho_Region.Dispose();
        //            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 240, 255);
        //            ho_ConnectedRegions.Dispose();
        //            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

        //            ho_SelectedRegions.Dispose();
        //            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
        //                "and", 30, 10000);
        //            ho_RegionDilation.Dispose();
        //            HOperatorSet.DilationRectangle1(ho_SelectedRegions, out ho_RegionDilation,
        //                30, 1);
        //            ho_RegionUnion.Dispose();
        //            HOperatorSet.Union1(ho_RegionDilation, out ho_RegionUnion);
        //            ho_ConnectedRegions1.Dispose();
        //            HOperatorSet.Connection(ho_RegionUnion, out ho_ConnectedRegions1);

        //            hv_Number.Dispose();
        //            HOperatorSet.CountObj(ho_showRegion, out hv_Number);

        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ErosionRectangle1(ho_ConnectedRegions1, out ExpTmpOutVar_0,
        //                    30, 1);
        //                ho_ConnectedRegions1.Dispose();
        //                ho_ConnectedRegions1 = ExpTmpOutVar_0;
        //            }
        //            ho_RegionErosion.Dispose();
        //            HOperatorSet.OpeningRectangle1(ho_ConnectedRegions1, out ho_RegionErosion,
        //                60, 2);

        //            hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
        //            HOperatorSet.SmallestRectangle2(ho_RegionErosion, out hv_Row, out hv_Column,
        //                out hv_Phi, out hv_Length1, out hv_Length2);


        //            //前3长的
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_Q1.Dispose();
        //                HOperatorSet.TupleSelectRank(hv_Length1, (new HTuple(hv_Length1.TupleLength()
        //                    )) - 1, out hv_Q1);
        //            }
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_Q2.Dispose();
        //                HOperatorSet.TupleSelectRank(hv_Length1, (new HTuple(hv_Length1.TupleLength()
        //                    )) - 2, out hv_Q2);
        //            }
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_Q3.Dispose();
        //                HOperatorSet.TupleSelectRank(hv_Length1, (new HTuple(hv_Length1.TupleLength()
        //                    )) - 3, out hv_Q3);
        //            }
        //            hv_Q.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_Q = new HTuple();
        //                hv_Q = hv_Q.TupleConcat(hv_Q1, hv_Q2, hv_Q3);
        //            }
        //            hv_Sum.Dispose();
        //            HOperatorSet.TupleSum(hv_Q, out hv_Sum);
        //            hv_average.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_average = hv_Sum / 3.0;
        //            }
        //            if ((int)(new HTuple(((hv_average * 2.0)).TupleGreater(hv_goldFingerThre))) != 0)
        //            {
        //                hv_result.Dispose();
        //                hv_result = 1;
        //            }


        //            ho_SelectedRegions2.Dispose();
        //            HOperatorSet.SelectShape(ho_RegionErosion, out ho_SelectedRegions2, "rect2_len1",
        //                "and", hv_Q3, 99999);
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegion, ho_SelectedRegions2, out ExpTmpOutVar_0
        //                    );
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }

        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegion, ho_ROI_0, out ExpTmpOutVar_0);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }


        //            //将Region paint到图像上显示
        //            hv_Channels.Dispose();
        //            HOperatorSet.CountChannels(ho_Image, out hv_Channels);
        //            if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
        //            {
        //                hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
        //                HOperatorSet.GetImagePointer1(ho_Image, out hv_Pointer, out hv_Type, out hv_Width,
        //                    out hv_Height);
        //                ho_ImageOut.Dispose();
        //                HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height, hv_Pointer,
        //                    hv_Pointer, hv_Pointer);
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.Boundary(ho_showRegion, out ExpTmpOutVar_0, "inner");
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.DilationCircle(ho_showRegion, out ExpTmpOutVar_0, 2);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegion, ((new HTuple(0)).TupleConcat(
        //                255)).TupleConcat(0), "fill");
        //        }
        //        // catch (Exception) 
        //        catch (HalconException HDevExpDefaultException1)
        //        {
        //            HDevExpDefaultException1.ToHTuple(out hv_Exception);
        //        }

        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_RegionDilation.Dispose();
        //        ho_RegionUnion.Dispose();
        //        ho_ConnectedRegions1.Dispose();
        //        ho_RegionErosion.Dispose();
        //        ho_SelectedRegions2.Dispose();

        //        hv_Number.Dispose();
        //        hv_Row.Dispose();
        //        hv_Column.Dispose();
        //        hv_Phi.Dispose();
        //        hv_Length1.Dispose();
        //        hv_Length2.Dispose();
        //        hv_Q1.Dispose();
        //        hv_Q2.Dispose();
        //        hv_Q3.Dispose();
        //        hv_Q.Dispose();
        //        hv_Sum.Dispose();
        //        hv_average.Dispose();
        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        return;
        //    }
        //    catch (HalconException HDevExpDefaultException)
        //    {
        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_RegionDilation.Dispose();
        //        ho_RegionUnion.Dispose();
        //        ho_ConnectedRegions1.Dispose();
        //        ho_RegionErosion.Dispose();
        //        ho_SelectedRegions2.Dispose();

        //        hv_Number.Dispose();
        //        hv_Row.Dispose();
        //        hv_Column.Dispose();
        //        hv_Phi.Dispose();
        //        hv_Length1.Dispose();
        //        hv_Length2.Dispose();
        //        hv_Q1.Dispose();
        //        hv_Q2.Dispose();
        //        hv_Q3.Dispose();
        //        hv_Q.Dispose();
        //        hv_Sum.Dispose();
        //        hv_average.Dispose();
        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        throw HDevExpDefaultException;
        //    }
        //}


        public void Flash_MLB_Flex(HObject ho_ImageRotate, HObject ho_ROI_0, out HObject ho_ShowRegion,
        out HTuple hv_Distance)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageResult, ho_ImageOpening, ho_Contour;
            HObject ho_Cross1, ho_Cross2, ho_Cross3, ho_Cross4;

            // Local control variables 

            HTuple hv_Value = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Column3 = new HTuple(), hv_Phi = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Parameter = new HTuple(), hv_InterRow1 = new HTuple();
            HTuple hv_InterCol1 = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_InterRow2 = new HTuple(), hv_InterCol2 = new HTuple();
            HTuple hv_InterRow3 = new HTuple(), hv_InterCol3 = new HTuple();
            HTuple hv_InterRow4 = new HTuple(), hv_InterCol4 = new HTuple();
            HTuple hv_Value1 = new HTuple(), hv_Value2 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Cross3);
            HOperatorSet.GenEmptyObj(out ho_Cross4);
            hv_Distance = new HTuple();
            ho_ShowRegion.Dispose();
            HOperatorSet.GenEmptyRegion(out ho_ShowRegion);
            hv_Value.Dispose();
            HOperatorSet.GrayFeatures(ho_ROI_0, ho_ImageRotate, "mean", out hv_Value);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_ImageResult.Dispose();
                HOperatorSet.MultImage(ho_ImageRotate, ho_ImageRotate, out ho_ImageResult, 200 / (hv_Value * hv_Value),
                    0);
            }
            ho_ImageOpening.Dispose();
            HOperatorSet.GrayOpeningRect(ho_ImageResult, out ho_ImageOpening, 7, 1);
            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
            HOperatorSet.SmallestRectangle1(ho_ROI_0, out hv_Row1, out hv_Column1, out hv_Row2,
                out hv_Column2);
            hv_Row3.Dispose(); hv_Column3.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
            HOperatorSet.SmallestRectangle2(ho_ROI_0, out hv_Row3, out hv_Column3, out hv_Phi,
                out hv_Length1, out hv_Length2);
            hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_ROI_0, out hv_Area, out hv_Row, out hv_Column);
            hv_MetrologyHandle.Dispose();
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            //*上边
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Index.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column1 - 200,
                    hv_Row1, hv_Column2 - 200, 30, 5, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("first")).TupleConcat("all"), out hv_Index);
            }
            //*下
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Index.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row2, hv_Column1 - 200,
                    hv_Row2, hv_Column2 - 200, 30, 5, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("last")).TupleConcat("all"), out hv_Index);
            }
            //*左
            hv_Index.Dispose();
            HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column1,
                hv_Row2, hv_Column1, 60, 5, 1, 30, (new HTuple("measure_select")).TupleConcat(
                "measure_transition"), (new HTuple("last")).TupleConcat("positive"), out hv_Index);
            //*右
            hv_Index.Dispose();
            HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column2,
                hv_Row2, hv_Column2, 100, 5, 1, 20, (new HTuple("measure_select")).TupleConcat(
                "measure_transition"), (new HTuple("first")).TupleConcat("positive"), out hv_Index);
            //set_metrology_object_param (MetrologyHandle, 'all', 'measure_threshold', 30)
            HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "min_score",
                0.3);
            //set_metrology_model_param (MetrologyHandle, 'reference_system', [0,0,0])
            //align_metrology_model (MetrologyHandle, Row, Column, 0)

            HOperatorSet.ApplyMetrologyModel(ho_ImageOpening, hv_MetrologyHandle);
            ho_Contour.Dispose();
            HOperatorSet.GetMetrologyObjectResultContour(out ho_Contour, hv_MetrologyHandle,
                "all", "all", 1.5);
            hv_Parameter.Dispose();
            HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type",
                "all_param", out hv_Parameter);
            ho_ShowRegion.Dispose();
            ho_ShowRegion = new HObject(ho_Contour);
            if ((int)(new HTuple((new HTuple(hv_Parameter.TupleLength())).TupleNotEqual(16))) != 0)
            {
                ho_ImageResult.Dispose();
                ho_ImageOpening.Dispose();
                ho_Contour.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();
                ho_Cross3.Dispose();
                ho_Cross4.Dispose();

                hv_Value.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Row3.Dispose();
                hv_Column3.Dispose();
                hv_Phi.Dispose();
                hv_Length1.Dispose();
                hv_Length2.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_MetrologyHandle.Dispose();
                hv_Index.Dispose();
                hv_Parameter.Dispose();
                hv_InterRow1.Dispose();
                hv_InterCol1.Dispose();
                hv_IsOverlapping.Dispose();
                hv_InterRow2.Dispose();
                hv_InterCol2.Dispose();
                hv_InterRow3.Dispose();
                hv_InterCol3.Dispose();
                hv_InterRow4.Dispose();
                hv_InterCol4.Dispose();
                hv_Value1.Dispose();
                hv_Value2.Dispose();

                return;
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow1.Dispose(); hv_InterCol1.Dispose(); hv_IsOverlapping.Dispose();
                HOperatorSet.IntersectionLines(hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(
                    1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(3), hv_Parameter.TupleSelect(
                    8), hv_Parameter.TupleSelect(9), hv_Parameter.TupleSelect(10), hv_Parameter.TupleSelect(
                    11), out hv_InterRow1, out hv_InterCol1, out hv_IsOverlapping);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow2.Dispose(); hv_InterCol2.Dispose(); hv_IsOverlapping.Dispose();
                HOperatorSet.IntersectionLines(hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(
                    1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(3), hv_Parameter.TupleSelect(
                    12), hv_Parameter.TupleSelect(13), hv_Parameter.TupleSelect(14), hv_Parameter.TupleSelect(
                    15), out hv_InterRow2, out hv_InterCol2, out hv_IsOverlapping);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow3.Dispose(); hv_InterCol3.Dispose(); hv_IsOverlapping.Dispose();
                HOperatorSet.IntersectionLines(hv_Parameter.TupleSelect(4), hv_Parameter.TupleSelect(
                    5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(7), hv_Parameter.TupleSelect(
                    8), hv_Parameter.TupleSelect(9), hv_Parameter.TupleSelect(10), hv_Parameter.TupleSelect(
                    11), out hv_InterRow3, out hv_InterCol3, out hv_IsOverlapping);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InterRow4.Dispose(); hv_InterCol4.Dispose(); hv_IsOverlapping.Dispose();
                HOperatorSet.IntersectionLines(hv_Parameter.TupleSelect(4), hv_Parameter.TupleSelect(
                    5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(7), hv_Parameter.TupleSelect(
                    12), hv_Parameter.TupleSelect(13), hv_Parameter.TupleSelect(14), hv_Parameter.TupleSelect(
                    15), out hv_InterRow4, out hv_InterCol4, out hv_IsOverlapping);
            }

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_InterRow1, hv_InterCol1, 36,
                    (new HTuple(45)).TupleRad());
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross2.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_InterRow2, hv_InterCol2, 36,
                    (new HTuple(45)).TupleRad());
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross3.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_InterRow3, hv_InterCol3, 36,
                    (new HTuple(45)).TupleRad());
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross4.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_InterRow4, hv_InterCol4, 36,
                    (new HTuple(45)).TupleRad());
            }

            hv_Value1.Dispose();
            HOperatorSet.DistancePp(hv_InterRow1, hv_InterCol1, hv_InterRow2, hv_InterCol2,
                out hv_Value1);
            hv_Value2.Dispose();
            HOperatorSet.DistancePp(hv_InterRow3, hv_InterCol3, hv_InterRow4, hv_InterCol4,
                out hv_Value2);
            hv_Distance.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Distance = new HTuple();
                hv_Distance = hv_Distance.TupleConcat(hv_Value1, hv_Value2);
            }

            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross1, out ExpTmpOutVar_0);
                ho_ShowRegion.Dispose();
                ho_ShowRegion = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross2, out ExpTmpOutVar_0);
                ho_ShowRegion.Dispose();
                ho_ShowRegion = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross3, out ExpTmpOutVar_0);
                ho_ShowRegion.Dispose();
                ho_ShowRegion = ExpTmpOutVar_0;
            }
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross4, out ExpTmpOutVar_0);
                ho_ShowRegion.Dispose();
                ho_ShowRegion = ExpTmpOutVar_0;
            }
            ho_ImageResult.Dispose();
            ho_ImageOpening.Dispose();
            ho_Contour.Dispose();
            ho_Cross1.Dispose();
            ho_Cross2.Dispose();
            ho_Cross3.Dispose();
            ho_Cross4.Dispose();

            hv_Value.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Row3.Dispose();
            hv_Column3.Dispose();
            hv_Phi.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_MetrologyHandle.Dispose();
            hv_Index.Dispose();
            hv_Parameter.Dispose();
            hv_InterRow1.Dispose();
            hv_InterCol1.Dispose();
            hv_IsOverlapping.Dispose();
            hv_InterRow2.Dispose();
            hv_InterCol2.Dispose();
            hv_InterRow3.Dispose();
            hv_InterCol3.Dispose();
            hv_InterRow4.Dispose();
            hv_InterCol4.Dispose();
            hv_Value1.Dispose();
            hv_Value2.Dispose();

            return;
        }



        public void Mesa_Flex(HObject ho_Image, HObject ho_RegionAffineTrans, out HObject ho_ShowRegion,
    out HTuple hv_Values)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Contour, ho_Cross1 = null, ho_Cross2 = null;
            HObject ho_Line = null, ho_RegionLines = null, ho_Cross3 = null;
            HObject ho_Cross4 = null;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Parameter = new HTuple(), hv_InterRow1 = new HTuple();
            HTuple hv_InterCol1 = new HTuple(), hv_IsOverlapping = new HTuple();
            HTuple hv_InterRow2 = new HTuple(), hv_InterCol2 = new HTuple();
            HTuple hv_Rows4 = new HTuple(), hv_Cols4 = new HTuple();
            HTuple hv_Rows5 = new HTuple(), hv_Cols5 = new HTuple();
            HTuple hv_EdgeRows = new HTuple(), hv_EdgeCols = new HTuple();
            HTuple hv_Row11 = new HTuple(), hv_Column11 = new HTuple();
            HTuple hv_Row21 = new HTuple(), hv_Column21 = new HTuple();
            HTuple hv_RowProj1 = new HTuple(), hv_ColProj1 = new HTuple();
            HTuple hv_RowProj2 = new HTuple(), hv_ColProj2 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Cross3);
            HOperatorSet.GenEmptyObj(out ho_Cross4);
            hv_Values = new HTuple();

            ho_ShowRegion.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);

            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
            HOperatorSet.SmallestRectangle1(ho_RegionAffineTrans, out hv_Row1, out hv_Column1,
                out hv_Row2, out hv_Column2);
            hv_MetrologyHandle.Dispose();
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            //*上
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Index1.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column1 - 200,
                    hv_Row1, hv_Column1, 30, 15, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("all")).TupleConcat("all"), out hv_Index1);
            }
            //*下
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Index1.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row2, hv_Column1 - 200,
                    hv_Row2, hv_Column1, 30, 15, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("all")).TupleConcat("all"), out hv_Index1);
            }
            //*左
            hv_Index1.Dispose();
            HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column1,
                hv_Row2, hv_Column1, 20, 5, 1, 25, (new HTuple("measure_select")).TupleConcat(
                "measure_transition"), (new HTuple("last")).TupleConcat("all"), out hv_Index1);
            //*右上
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Index1.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column2,
                    hv_Row1 + 50, hv_Column2, 40, 5, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("first")).TupleConcat("negative"), out hv_Index1);
            }
            //*右下
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Index1.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row2 - 50, hv_Column2,
                    hv_Row2, hv_Column2, 40, 5, 1, 30, (new HTuple("measure_select")).TupleConcat(
                    "measure_transition"), (new HTuple("first")).TupleConcat("negative"), out hv_Index1);
            }

            HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "min_score",
                0.3);
            //set_metrology_object_param (MetrologyHandle, 'all', 'measure_threshold', 40)
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            hv_Parameter.Dispose();
            HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type",
                "all_param", out hv_Parameter);

            ho_Contour.Dispose();
            HOperatorSet.GetMetrologyObjectResultContour(out ho_Contour, hv_MetrologyHandle,
                "all", "all", 1.5);
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
            }
            if (HDevWindowStack.IsOpen())
            {
                //dev_display (Contour)
            }
            if ((int)(new HTuple((new HTuple(hv_Parameter.TupleLength())).TupleNotEqual(20))) != 0)
            {
                ho_ShowRegion.Dispose();
                ho_ShowRegion = new HObject(ho_RegionAffineTrans);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Contour, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
            }
            else
            {
                ho_ShowRegion.Dispose();
                ho_ShowRegion = new HObject(ho_Contour);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_InterRow1.Dispose(); hv_InterCol1.Dispose(); hv_IsOverlapping.Dispose();
                    HOperatorSet.IntersectionLines((hv_Parameter.TupleSelect(0)) + 23, hv_Parameter.TupleSelect(
                        1), (hv_Parameter.TupleSelect(2)) + 23, hv_Parameter.TupleSelect(3), hv_Parameter.TupleSelect(
                        8), hv_Parameter.TupleSelect(9), hv_Parameter.TupleSelect(10), hv_Parameter.TupleSelect(
                        11), out hv_InterRow1, out hv_InterCol1, out hv_IsOverlapping);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_InterRow2.Dispose(); hv_InterCol2.Dispose(); hv_IsOverlapping.Dispose();
                    HOperatorSet.IntersectionLines((hv_Parameter.TupleSelect(4)) - 20, hv_Parameter.TupleSelect(
                        5), (hv_Parameter.TupleSelect(6)) - 20, hv_Parameter.TupleSelect(7), hv_Parameter.TupleSelect(
                        8), hv_Parameter.TupleSelect(9), hv_Parameter.TupleSelect(10), hv_Parameter.TupleSelect(
                        11), out hv_InterRow2, out hv_InterCol2, out hv_IsOverlapping);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross1.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_InterRow1, hv_InterCol1,
                        36, (new HTuple(45)).TupleRad());
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross2.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_InterRow2, hv_InterCol2,
                        36, (new HTuple(45)).TupleRad());
                }


                hv_Rows4.Dispose();
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, 3, "all", "used_edges",
                    "row", out hv_Rows4);
                hv_Cols4.Dispose();
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, 3, "all", "used_edges",
                    "column", out hv_Cols4);
                hv_Rows5.Dispose();
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, 4, "all", "used_edges",
                    "row", out hv_Rows5);
                hv_Cols5.Dispose();
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, 4, "all", "used_edges",
                    "column", out hv_Cols5);
                hv_EdgeRows.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EdgeRows = new HTuple();
                    hv_EdgeRows = hv_EdgeRows.TupleConcat(hv_Rows4, hv_Rows5);
                }
                hv_EdgeCols.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EdgeCols = new HTuple();
                    hv_EdgeCols = hv_EdgeCols.TupleConcat(hv_Cols4, hv_Cols5);
                }
                //gen_cross_contour_xld (Cross2, EdgeRows, EdgeCols, 36, Angle)
                ho_Line.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose();
                pts_to_best_line(out ho_Line, hv_EdgeRows, hv_EdgeCols, 2, out hv_Row11, out hv_Column11,
                    out hv_Row21, out hv_Column21);
                ho_RegionLines.Dispose();
                HOperatorSet.GenRegionLine(out ho_RegionLines, hv_Row11, hv_Column11, hv_Row21,
                    hv_Column21);

                hv_RowProj1.Dispose(); hv_ColProj1.Dispose();
                HOperatorSet.ProjectionPl(hv_InterRow1, hv_InterCol1, hv_Row11, hv_Column11,
                    hv_Row21, hv_Column21, out hv_RowProj1, out hv_ColProj1);
                hv_RowProj2.Dispose(); hv_ColProj2.Dispose();
                HOperatorSet.ProjectionPl(hv_InterRow2, hv_InterCol2, hv_Row11, hv_Column11,
                    hv_Row21, hv_Column21, out hv_RowProj2, out hv_ColProj2);

                hv_Distance.Dispose();
                HOperatorSet.DistancePp(hv_InterRow1, hv_InterCol1, hv_RowProj1, hv_ColProj1,
                    out hv_Distance);
                hv_Distance1.Dispose();
                HOperatorSet.DistancePp(hv_InterRow2, hv_InterCol2, hv_RowProj2, hv_ColProj2,
                    out hv_Distance1);

                hv_Values.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Values = new HTuple();
                    hv_Values = hv_Values.TupleConcat(hv_Distance, hv_Distance1);
                }

                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross3.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross3, hv_RowProj1, hv_ColProj1, 36,
                        (new HTuple(45)).TupleRad());
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Cross4.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross4, hv_RowProj2, hv_ColProj2, 36,
                        (new HTuple(45)).TupleRad());
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Line, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross1, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross2, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross3, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_Cross4, out ExpTmpOutVar_0);
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }

            }
            ho_Contour.Dispose();
            ho_Cross1.Dispose();
            ho_Cross2.Dispose();
            ho_Line.Dispose();
            ho_RegionLines.Dispose();
            ho_Cross3.Dispose();
            ho_Cross4.Dispose();

            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_MetrologyHandle.Dispose();
            hv_Index1.Dispose();
            hv_Parameter.Dispose();
            hv_InterRow1.Dispose();
            hv_InterCol1.Dispose();
            hv_IsOverlapping.Dispose();
            hv_InterRow2.Dispose();
            hv_InterCol2.Dispose();
            hv_Rows4.Dispose();
            hv_Cols4.Dispose();
            hv_Rows5.Dispose();
            hv_Cols5.Dispose();
            hv_EdgeRows.Dispose();
            hv_EdgeCols.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_RowProj1.Dispose();
            hv_ColProj1.Dispose();
            hv_RowProj2.Dispose();
            hv_ColProj2.Dispose();
            hv_Distance.Dispose();
            hv_Distance1.Dispose();

            return;
        }



        public bool find_screw_deeplearning(HObject ho_Image, HObject ho_ROI_0)
        {
            HTuple hv_DLModelHandle = null, hv_DictParam = null;
            HTuple ClassNames,ClassIDS;
            HObject ho_ImageReduced=new HObject(), ho_ImagePart = new HObject();
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);

            HOperatorSet.ReadDlModel($"{Application.StartupPath}\\DeepL\\screw.hdl", out hv_DLModelHandle);
            //获取名称
            HOperatorSet.GetDlModelParam(hv_DLModelHandle, "class_names", out ClassNames);
            //获取序号
            HOperatorSet.GetDlModelParam(hv_DLModelHandle, "class_ids", out ClassIDS);
            //单次处理的图像数
            HOperatorSet.SetDlModelParam(hv_DLModelHandle, "batch_size", 1);
            try
            {
                HOperatorSet.SetDlModelParam(hv_DLModelHandle, "runtime", "cpu");
            }
            catch (Exception)
            {
                HOperatorSet.SetDlModelParam(hv_DLModelHandle, "runtime", "gpu");
            }
            HOperatorSet.SetDlModelParam(hv_DLModelHandle, "runtime_init", "immediately");
            HOperatorSet.ReadDict($"{Application.StartupPath}\\DeepL\\screw_dl_preprocess_params.hdict", new HTuple(), new HTuple(), out hv_DictParam);
            HTuple hv_DLSampleBatch = null, hv_DlResultBatch = null, hv_ClassName = null;

            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);

            gen_dl_samples_from_images(ho_ImagePart, out hv_DLSampleBatch);
            preprocess_dl_samples(hv_DLSampleBatch, hv_DictParam);
            HOperatorSet.ApplyDlModel(hv_DLModelHandle, hv_DLSampleBatch, new HTuple(), out hv_DlResultBatch);
            HOperatorSet.GetDictTuple(hv_DlResultBatch, "classification_class_names", out hv_ClassName);
            if (hv_ClassName.SArr[0] == "OK")
            {
                //ShowMsgEvent("Tips:检测为OK！");
                return true;
            }
            else
            {
                //ShowMsgEvent($"Error:检测为{hv_ClassName.SArr[0]}！");
                return false;
            }

        }

        public bool find_special_screw_deeplearning(HObject ho_Image, HObject ho_ROI_0)
        {
            HTuple hv_DLModelHandle = null, hv_DictParam = null;
            HTuple ClassNames, ClassIDS;
            HObject ho_ImageReduced = new HObject(), ho_ImagePart = new HObject();
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);

            HOperatorSet.ReadDlModel($"{Application.StartupPath}\\DeepL\\special_screw.hdl", out hv_DLModelHandle);
            //获取名称
            HOperatorSet.GetDlModelParam(hv_DLModelHandle, "class_names", out ClassNames);
            //获取序号
            HOperatorSet.GetDlModelParam(hv_DLModelHandle, "class_ids", out ClassIDS);
            //单次处理的图像数
            HOperatorSet.SetDlModelParam(hv_DLModelHandle, "batch_size", 1);
            try
            {
                HOperatorSet.SetDlModelParam(hv_DLModelHandle, "runtime", "cpu");
            }
            catch (Exception)
            {
                HOperatorSet.SetDlModelParam(hv_DLModelHandle, "runtime", "gpu");
            }
            HOperatorSet.SetDlModelParam(hv_DLModelHandle, "runtime_init", "immediately");
            HOperatorSet.ReadDict($"{Application.StartupPath}\\DeepL\\special_screw_dl_preprocess_params.hdict", new HTuple(), new HTuple(), out hv_DictParam);
            HTuple hv_DLSampleBatch = null, hv_DlResultBatch = null, hv_ClassName = null;

            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);

            gen_dl_samples_from_images(ho_ImagePart, out hv_DLSampleBatch);
            preprocess_dl_samples(hv_DLSampleBatch, hv_DictParam);
            HOperatorSet.ApplyDlModel(hv_DLModelHandle, hv_DLSampleBatch, new HTuple(), out hv_DlResultBatch);
            HOperatorSet.GetDictTuple(hv_DlResultBatch, "classification_class_names", out hv_ClassName);
            if (hv_ClassName.SArr[0] == "OK")
            {
                //ShowMsgEvent("Tips:检测为OK！");
                return true;
            }
            else
            {
                //ShowMsgEvent($"Error:检测为{hv_ClassName.SArr[0]}！");
                return false;
            }
        }


        public void find_model_screws(HObject ho_Image, HObject ho_ROI_0, out HObject ho_ImageOut,
            out HObject ho_showRegion, HTuple hv_ModelID, out HTuple hv_Row, out HTuple hv_Column,
            out HTuple hv_Angle, out HTuple hv_Score)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced = null, ho_ModelContours = null;
            HObject ho_ModelContoursTrans = null;


            // Local control variables 

            HTuple hv_RScale = new HTuple(), hv_CScale = new HTuple(), hv_HomMat2DContour = new HTuple();
            HTuple hv_Channels = new HTuple(), hv_Pointer = new HTuple();
            HTuple hv_Type = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageOut);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ModelContoursTrans);
            hv_Row = new HTuple();
            hv_Column = new HTuple();
            hv_Angle = new HTuple();
            hv_Score = new HTuple();

            HTuple Hom = new HTuple();

            try
            {
                ho_showRegion = ho_ROI_0;
                try
                {

                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
                    hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_Score.Dispose();

                    HOperatorSet.FindAnisoShapeModel(ho_ImageReduced, hv_ModelID, 0, (new HTuple(360)).TupleRad()
                        , 0.9, 1.1, 0.9, 1.1, 0.45, 1, 0.5, "least_squares_high", (new HTuple(5)).TupleConcat(3), 0.3,
                        out hv_Row, out hv_Column, out hv_Angle, out hv_RScale, out hv_CScale, out hv_Score);

                    //HOperatorSet.FindScaledShapeModel(ho_ImageReduced, hv_ModelID, 0, (new HTuple(360)).TupleRad()
                    //    , 0.9, 1.1, 0.3, 1, 1, "least_squares_high", (new HTuple(5)).TupleConcat(3), 0.3,
                    //    out hv_Row, out hv_Column, out hv_Angle, out hv_Scale, out hv_Score);
                    ho_ModelContours.Dispose();
                    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                    ho_ModelContoursTrans.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ModelContoursTrans);
                    if ((int)(new HTuple(hv_Score.TupleGreater(0.2))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_HomMat2DContour.Dispose();
                            HOperatorSet.HomMat2dIdentity(out Hom);
                            HOperatorSet.HomMat2dScale(Hom, hv_RScale, hv_CScale, 0, 0, out Hom);
                            HOperatorSet.HomMat2dRotate(Hom, hv_Angle, 0, 0, out Hom);
                            HOperatorSet.HomMat2dTranslate(Hom, hv_Row, hv_Column, out Hom);
                        }
                        ho_ModelContoursTrans.Dispose();
                        HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ModelContoursTrans,
                            Hom);
                        HOperatorSet.ConcatObj(ho_showRegion, ho_ModelContoursTrans, out ho_showRegion);
                    }

                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Boundary(ho_ROI_0_COPY_INP_TMP, out ExpTmpOutVar_0, "inner");
                    //    ho_ROI_0_COPY_INP_TMP.Dispose();
                    //    ho_ROI_0_COPY_INP_TMP = ExpTmpOutVar_0;
                    //}
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.ConcatObj(ho_showRegion, ho_ROI_0_COPY_INP_TMP, out ExpTmpOutVar_0
                    //        );
                    //    ho_showRegion.Dispose();
                    //    ho_showRegion = ExpTmpOutVar_0;
                    //}


                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Union1(ho_showRegion, out ExpTmpOutVar_0);
                    //    ho_showRegion.Dispose();
                    //    ho_showRegion = ExpTmpOutVar_0;
                    //}

                    ////将Region paint到图像上显示
                    //hv_Channels.Dispose();
                    //HOperatorSet.CountChannels(ho_Image, out hv_Channels);
                    //if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
                    //{
                    //    hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
                    //    HOperatorSet.GetImagePointer1(ho_Image, out hv_Pointer, out hv_Type, out hv_Width,
                    //        out hv_Height);
                    //    ho_ImageOut.Dispose();
                    //    HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height, hv_Pointer,
                    //        hv_Pointer, hv_Pointer);
                    //}
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.Boundary(ho_showRegion, out ExpTmpOutVar_0, "inner");
                    //    ho_showRegion.Dispose();
                    //    ho_showRegion = ExpTmpOutVar_0;
                    //}
                    //{
                    //    HObject ExpTmpOutVar_0;
                    //    HOperatorSet.DilationCircle(ho_showRegion, out ExpTmpOutVar_0, 2);
                    //    ho_showRegion.Dispose();
                    //    ho_showRegion = ExpTmpOutVar_0;
                    //}
                    //HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegion, ((new HTuple(0)).TupleConcat(
                    //    255)).TupleConcat(0), "fill");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                return;
            }
            catch (HalconException HDevExpDefaultException)
            {


                throw HDevExpDefaultException;
            }
        }

        public void findXline(HObject ho_SelectedRegions, HObject ho_Image, out HObject ho_showRegion,
            out HTuple hv_coordRow)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Rectangle = null, ho_RegionDilation = null;
            HObject ho_ImageReduced1 = null, ho_Cross = null, ho_Cross1 = null;
            HObject ho_Cross2 = null;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_LineRow1 = new HTuple(), hv_LineColumn1 = new HTuple();
            HTuple hv_LineRow2 = new HTuple(), hv_LineColumn2 = new HTuple();
            HTuple hv_findRowArray = new HTuple(), hv_AmplitudeThreshold = new HTuple();
            HTuple hv_Sigma = new HTuple(), hv_RoiWidthLen = new HTuple();
            HTuple hv_lineCoord = new HTuple(), hv_Row_Measure_01_0 = new HTuple();
            HTuple hv_Column_Measure_01_0 = new HTuple(), hv_Amplitude_Measure_01_0 = new HTuple();
            HTuple hv_Indices1 = new HTuple(), hv_findRow = new HTuple();
            HTuple hv_findCol = new HTuple(), hv_findRow1 = new HTuple();
            HTuple hv_findCol1 = new HTuple(), hv_findRow2 = new HTuple();
            HTuple hv_findCol2 = new HTuple(), hv_Sum = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_showRegion);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            hv_coordRow = new HTuple();
            try
            {

                try
                {

                    ho_showRegion.Dispose();
                    HOperatorSet.GenEmptyRegion(out ho_showRegion);


                    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_SelectedRegions, out hv_Row1, out hv_Column1,
                        out hv_Row2, out hv_Column2);
                    hv_LineRow1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineRow1 = hv_Row2 + 45;
                    }
                    hv_LineColumn1.Dispose();
                    hv_LineColumn1 = new HTuple(hv_Column1);
                    hv_LineRow2.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineRow2 = hv_Row2 + 45;
                    }
                    hv_LineColumn2.Dispose();
                    hv_LineColumn2 = new HTuple(hv_Column2);

                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, hv_LineRow1, hv_LineColumn1,
                        hv_LineRow2, hv_LineColumn2);

                    ho_RegionDilation.Dispose();
                    HOperatorSet.DilationRectangle1(ho_Rectangle, out ho_RegionDilation, 1, 40);
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_RegionDilation, out ho_ImageReduced1
                        );
                    hv_findRowArray.Dispose();
                    hv_findRowArray = new HTuple();
                    //Measure 01: Code generated by Measure 01
                    //Measure 01: Prepare measurement
                    hv_AmplitudeThreshold.Dispose();
                    hv_AmplitudeThreshold = 18;
                    hv_Sigma.Dispose();
                    hv_Sigma = 6.4;
                    hv_RoiWidthLen.Dispose();
                    hv_RoiWidthLen = 7;
                    hv_lineCoord.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lineCoord = new HTuple();
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1 + 20);
                        hv_lineCoord = hv_lineCoord.TupleConcat(((hv_LineColumn1 + hv_LineColumn2) * 0.5) - 120);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1 - 20);
                        hv_lineCoord = hv_lineCoord.TupleConcat(((hv_LineColumn1 + hv_LineColumn2) * 0.5) - 120);
                    }
                    hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
                    measure_1D(ho_Image, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen, hv_lineCoord,
                        out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);

                    if ((int)(new HTuple((new HTuple(hv_Amplitude_Measure_01_0.TupleLength())).TupleNotEqual(
                        0))) != 0)
                    {
                        hv_Indices1.Dispose();
                        HOperatorSet.TupleSortIndex(hv_Amplitude_Measure_01_0, out hv_Indices1);
                        hv_findRow.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findRow = hv_Row_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        hv_findCol.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findCol = hv_Column_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
                                    hv_findRow);
                                hv_findRowArray.Dispose();
                                hv_findRowArray = ExpTmpLocalVar_findRowArray;
                            }
                        }
                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol, 26,
                            0.785398);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
                            ho_Cross.Dispose();
                            ho_Cross = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_showRegion, ho_Cross, out ExpTmpOutVar_0);
                            ho_showRegion.Dispose();
                            ho_showRegion = ExpTmpOutVar_0;
                        }
                    }




                    hv_lineCoord.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lineCoord = new HTuple();
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1 + 20);
                        hv_lineCoord = hv_lineCoord.TupleConcat((hv_LineColumn1 + hv_LineColumn2) * 0.5);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1 - 20);
                        hv_lineCoord = hv_lineCoord.TupleConcat((hv_LineColumn1 + hv_LineColumn2) * 0.5);
                    }
                    hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
                    measure_1D(ho_Image, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen, hv_lineCoord,
                        out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);
                    if ((int)(new HTuple((new HTuple(hv_Amplitude_Measure_01_0.TupleLength())).TupleNotEqual(
                        0))) != 0)
                    {


                        hv_Indices1.Dispose();
                        HOperatorSet.TupleSortIndex(hv_Amplitude_Measure_01_0, out hv_Indices1);
                        hv_findRow1.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findRow1 = hv_Row_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        hv_findCol1.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findCol1 = hv_Column_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
                                    hv_findRow1);
                                hv_findRowArray.Dispose();
                                hv_findRowArray = ExpTmpLocalVar_findRowArray;
                            }
                        }

                        ho_Cross1.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_findRow1, hv_findCol1,
                            26, 0.785398);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.GenRegionContourXld(ho_Cross1, out ExpTmpOutVar_0, "margin");
                            ho_Cross1.Dispose();
                            ho_Cross1 = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_showRegion, ho_Cross1, out ExpTmpOutVar_0);
                            ho_showRegion.Dispose();
                            ho_showRegion = ExpTmpOutVar_0;
                        }

                    }

                    hv_lineCoord.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lineCoord = new HTuple();
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1 + 20);
                        hv_lineCoord = hv_lineCoord.TupleConcat(((hv_LineColumn1 + hv_LineColumn2) * 0.5) + 120);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1 - 20);
                        hv_lineCoord = hv_lineCoord.TupleConcat(((hv_LineColumn1 + hv_LineColumn2) * 0.5) + 120);
                    }
                    hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
                    measure_1D(ho_Image, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen, hv_lineCoord,
                        out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);
                    if ((int)(new HTuple((new HTuple(hv_Amplitude_Measure_01_0.TupleLength())).TupleNotEqual(
                        0))) != 0)
                    {
                        hv_Indices1.Dispose();
                        HOperatorSet.TupleSortIndex(hv_Amplitude_Measure_01_0, out hv_Indices1);
                        hv_findRow2.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findRow2 = hv_Row_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        hv_findCol2.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findCol2 = hv_Column_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
                                    hv_findRow2);
                                hv_findRowArray.Dispose();
                                hv_findRowArray = ExpTmpLocalVar_findRowArray;
                            }
                        }

                        ho_Cross2.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_findRow2, hv_findCol2,
                            26, 0.785398);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.GenRegionContourXld(ho_Cross2, out ExpTmpOutVar_0, "margin");
                            ho_Cross2.Dispose();
                            ho_Cross2 = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_showRegion, ho_Cross2, out ExpTmpOutVar_0);
                            ho_showRegion.Dispose();
                            ho_showRegion = ExpTmpOutVar_0;
                        }
                    }





                    hv_Sum.Dispose();
                    HOperatorSet.TupleSum(hv_findRowArray, out hv_Sum);
                    hv_coordRow.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_coordRow = hv_Sum / (new HTuple(hv_findRowArray.TupleLength()
                            ));
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                ho_Rectangle.Dispose();
                ho_RegionDilation.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_LineRow1.Dispose();
                hv_LineColumn1.Dispose();
                hv_LineRow2.Dispose();
                hv_LineColumn2.Dispose();
                hv_findRowArray.Dispose();
                hv_AmplitudeThreshold.Dispose();
                hv_Sigma.Dispose();
                hv_RoiWidthLen.Dispose();
                hv_lineCoord.Dispose();
                hv_Row_Measure_01_0.Dispose();
                hv_Column_Measure_01_0.Dispose();
                hv_Amplitude_Measure_01_0.Dispose();
                hv_Indices1.Dispose();
                hv_findRow.Dispose();
                hv_findCol.Dispose();
                hv_findRow1.Dispose();
                hv_findCol1.Dispose();
                hv_findRow2.Dispose();
                hv_findCol2.Dispose();
                hv_Sum.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_RegionDilation.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_LineRow1.Dispose();
                hv_LineColumn1.Dispose();
                hv_LineRow2.Dispose();
                hv_LineColumn2.Dispose();
                hv_findRowArray.Dispose();
                hv_AmplitudeThreshold.Dispose();
                hv_Sigma.Dispose();
                hv_RoiWidthLen.Dispose();
                hv_lineCoord.Dispose();
                hv_Row_Measure_01_0.Dispose();
                hv_Column_Measure_01_0.Dispose();
                hv_Amplitude_Measure_01_0.Dispose();
                hv_Indices1.Dispose();
                hv_findRow.Dispose();
                hv_findCol.Dispose();
                hv_findRow1.Dispose();
                hv_findCol1.Dispose();
                hv_findRow2.Dispose();
                hv_findCol2.Dispose();
                hv_Sum.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void findYline(HObject ho_SelectedRegions, HObject ho_Image, out HObject ho_showRegion,
            out HTuple hv_coordCol)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Rectangle = null, ho_RegionDilation = null;
            HObject ho_ImageReduced1 = null, ho_Cross = null, ho_Cross1 = null;
            HObject ho_Cross2 = null;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_LineRow1 = new HTuple(), hv_LineColumn1 = new HTuple();
            HTuple hv_LineRow2 = new HTuple(), hv_LineColumn2 = new HTuple();
            HTuple hv_findArray = new HTuple(), hv_AmplitudeThreshold = new HTuple();
            HTuple hv_Sigma = new HTuple(), hv_RoiWidthLen = new HTuple();
            HTuple hv_lineCoord = new HTuple(), hv_Row_Measure_01_0 = new HTuple();
            HTuple hv_Column_Measure_01_0 = new HTuple(), hv_Amplitude_Measure_01_0 = new HTuple();
            HTuple hv_Indices1 = new HTuple(), hv_findRow = new HTuple();
            HTuple hv_findCol = new HTuple(), hv_findRow1 = new HTuple();
            HTuple hv_findCol1 = new HTuple(), hv_findRow2 = new HTuple();
            HTuple hv_findCol2 = new HTuple(), hv_Sum = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_showRegion);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_Cross2);
            hv_coordCol = new HTuple();
            try
            {
                try
                {



                    ho_showRegion.Dispose();
                    HOperatorSet.GenEmptyRegion(out ho_showRegion);
                    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_SelectedRegions, out hv_Row1, out hv_Column1,
                        out hv_Row2, out hv_Column2);

                    hv_LineRow1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineRow1 = hv_Row2 - 100;
                    }
                    hv_LineColumn1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineColumn1 = hv_Column2 + 70;
                    }
                    hv_LineRow2.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineRow2 = hv_Row2 - 20;
                    }
                    hv_LineColumn2.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineColumn2 = hv_Column2 + 70;
                    }

                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, hv_LineRow1, hv_LineColumn1,
                        hv_LineRow2, hv_LineColumn2);

                    ho_RegionDilation.Dispose();
                    HOperatorSet.DilationRectangle1(ho_Rectangle, out ho_RegionDilation, 40,
                        1);
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_RegionDilation, out ho_ImageReduced1
                        );
                    hv_findArray.Dispose();
                    hv_findArray = new HTuple();
                    //Measure 01: Code generated by Measure 01
                    //Measure 01: Prepare measurement
                    hv_AmplitudeThreshold.Dispose();
                    hv_AmplitudeThreshold = 7;
                    hv_Sigma.Dispose();
                    hv_Sigma = 2.4;
                    hv_RoiWidthLen.Dispose();
                    hv_RoiWidthLen = 12;
                    hv_lineCoord.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lineCoord = new HTuple();
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineColumn1 - 30);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow1);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineColumn1 + 20);
                    }
                    hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
                    measure_1D(ho_ImageReduced1, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen,
                        hv_lineCoord, out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);
                    hv_Indices1.Dispose();
                    HOperatorSet.TupleSortIndex(hv_Amplitude_Measure_01_0, out hv_Indices1);
                    if ((int)(new HTuple((new HTuple(hv_Indices1.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        hv_findRow.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findRow = hv_Row_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        hv_findCol.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findCol = hv_Column_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_findArray = hv_findArray.TupleConcat(
                                    hv_findCol);
                                hv_findArray.Dispose();
                                hv_findArray = ExpTmpLocalVar_findArray;
                            }
                        }
                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol, 26,
                            0.785398);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
                            ho_Cross.Dispose();
                            ho_Cross = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_showRegion, ho_Cross, out ExpTmpOutVar_0);
                            ho_showRegion.Dispose();
                            ho_showRegion = ExpTmpOutVar_0;
                        }

                    }
                    hv_lineCoord.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lineCoord = new HTuple();
                        hv_lineCoord = hv_lineCoord.TupleConcat((hv_LineRow1 + hv_LineRow2) * 0.5);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineColumn1 - 30);
                        hv_lineCoord = hv_lineCoord.TupleConcat((hv_LineRow1 + hv_LineRow2) * 0.5);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineColumn1 + 20);
                    }
                    hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
                    measure_1D(ho_Image, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen, hv_lineCoord,
                        out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);
                    hv_Indices1.Dispose();
                    HOperatorSet.TupleSortIndex(hv_Amplitude_Measure_01_0, out hv_Indices1);
                    if ((int)(new HTuple((new HTuple(hv_Indices1.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        hv_findRow1.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findRow1 = hv_Row_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        hv_findCol1.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findCol1 = hv_Column_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_findArray = hv_findArray.TupleConcat(
                                    hv_findCol1);
                                hv_findArray.Dispose();
                                hv_findArray = ExpTmpLocalVar_findArray;
                            }
                        }

                        ho_Cross1.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_findRow1, hv_findCol1,
                            26, 0.785398);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.GenRegionContourXld(ho_Cross1, out ExpTmpOutVar_0, "margin");
                            ho_Cross1.Dispose();
                            ho_Cross1 = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_showRegion, ho_Cross1, out ExpTmpOutVar_0);
                            ho_showRegion.Dispose();
                            ho_showRegion = ExpTmpOutVar_0;
                        }
                    }



                    hv_lineCoord.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_lineCoord = new HTuple();
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow2);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineColumn1 - 30);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineRow2);
                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_LineColumn1 + 20);
                    }
                    hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
                    measure_1D(ho_Image, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen, hv_lineCoord,
                        out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);
                    hv_Indices1.Dispose();
                    HOperatorSet.TupleSortIndex(hv_Amplitude_Measure_01_0, out hv_Indices1);
                    if ((int)(new HTuple((new HTuple(hv_Indices1.TupleLength())).TupleGreater(
                        0))) != 0)
                    {


                        hv_findRow2.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findRow2 = hv_Row_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        hv_findCol2.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_findCol2 = hv_Column_Measure_01_0.TupleSelect(
                                hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_findArray = hv_findArray.TupleConcat(
                                    hv_findCol2);
                                hv_findArray.Dispose();
                                hv_findArray = ExpTmpLocalVar_findArray;
                            }
                        }

                        ho_Cross2.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_findRow2, hv_findCol2,
                            26, 0.785398);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.GenRegionContourXld(ho_Cross2, out ExpTmpOutVar_0, "margin");
                            ho_Cross2.Dispose();
                            ho_Cross2 = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_showRegion, ho_Cross2, out ExpTmpOutVar_0);
                            ho_showRegion.Dispose();
                            ho_showRegion = ExpTmpOutVar_0;
                        }
                    }


                    hv_Sum.Dispose();
                    HOperatorSet.TupleSum(hv_findArray, out hv_Sum);
                    hv_coordCol.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_coordCol = hv_Sum / (new HTuple(hv_findArray.TupleLength()
                            ));
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                ho_Rectangle.Dispose();
                ho_RegionDilation.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_LineRow1.Dispose();
                hv_LineColumn1.Dispose();
                hv_LineRow2.Dispose();
                hv_LineColumn2.Dispose();
                hv_findArray.Dispose();
                hv_AmplitudeThreshold.Dispose();
                hv_Sigma.Dispose();
                hv_RoiWidthLen.Dispose();
                hv_lineCoord.Dispose();
                hv_Row_Measure_01_0.Dispose();
                hv_Column_Measure_01_0.Dispose();
                hv_Amplitude_Measure_01_0.Dispose();
                hv_Indices1.Dispose();
                hv_findRow.Dispose();
                hv_findCol.Dispose();
                hv_findRow1.Dispose();
                hv_findCol1.Dispose();
                hv_findRow2.Dispose();
                hv_findCol2.Dispose();
                hv_Sum.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_RegionDilation.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Cross.Dispose();
                ho_Cross1.Dispose();
                ho_Cross2.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_LineRow1.Dispose();
                hv_LineColumn1.Dispose();
                hv_LineRow2.Dispose();
                hv_LineColumn2.Dispose();
                hv_findArray.Dispose();
                hv_AmplitudeThreshold.Dispose();
                hv_Sigma.Dispose();
                hv_RoiWidthLen.Dispose();
                hv_lineCoord.Dispose();
                hv_Row_Measure_01_0.Dispose();
                hv_Column_Measure_01_0.Dispose();
                hv_Amplitude_Measure_01_0.Dispose();
                hv_Indices1.Dispose();
                hv_findRow.Dispose();
                hv_findCol.Dispose();
                hv_findRow1.Dispose();
                hv_findCol1.Dispose();
                hv_findRow2.Dispose();
                hv_findCol2.Dispose();
                hv_Sum.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //public void flex(HObject ho_Image, HObject ho_ROI_0, out HObject ho_showRegion,
        //    out HObject ho_ImageOut, out HTuple hv_DistanceArray)
        //{



        //    // Stack for temporary objects 
        //    HObject[] OTemp = new HObject[20];

        //    // Local iconic variables 

        //    HObject ho_ImageReduced = null, ho_CrossObject = null;
        //    HObject ho_Rectangle = null, ho_Cross = null;

        //    // Local control variables 

        //    HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
        //    HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
        //    HTuple hv_findRowArray = new HTuple(), hv_findColArray = new HTuple();
        //    HTuple hv_AmplitudeThreshold = new HTuple(), hv_Sigma = new HTuple();
        //    HTuple hv_RoiWidthLen = new HTuple(), hv_Index = new HTuple();
        //    HTuple hv_lineCoord = new HTuple(), hv_Row_Measure_01_0 = new HTuple();
        //    HTuple hv_Column_Measure_01_0 = new HTuple(), hv_Amplitude_Measure_01_0 = new HTuple();
        //    HTuple hv_Indices1 = new HTuple(), hv_findRow = new HTuple();
        //    HTuple hv_findCol = new HTuple(), hv_Distance = new HTuple();
        //    HTuple hv_Channels = new HTuple(), hv_Pointer = new HTuple();
        //    HTuple hv_Type = new HTuple(), hv_Width = new HTuple();
        //    HTuple hv_Height = new HTuple(), hv_Exception = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_showRegion);
        //    HOperatorSet.GenEmptyObj(out ho_ImageOut);
        //    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //    HOperatorSet.GenEmptyObj(out ho_CrossObject);
        //    HOperatorSet.GenEmptyObj(out ho_Rectangle);
        //    HOperatorSet.GenEmptyObj(out ho_Cross);
        //    hv_DistanceArray = new HTuple();
        //    try
        //    {
        //        try
        //        {

        //            ho_showRegion.Dispose();
        //            HOperatorSet.GenEmptyObj(out ho_showRegion);
        //            ho_ImageReduced.Dispose();
        //            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
        //            //threshold (ImageReduced, Region, 200, 255)
        //            //connection (Region, ConnectedRegions)
        //            //select_shape (ConnectedRegions, ConnectedRegions, 'area', 'and', 800, 999999)
        //            //area_center (ConnectedRegions, Area, Row, Column)

        //            //tuple_sort_index (Area, Indices)
        //            //sort_region (ConnectedRegions, SortedRegions, 'first_point', 'true', 'row')

        //            //count_obj (SortedRegions, Number1)
        //            //gen_empty_obj (upDownObject)
        //            //select_obj (SortedRegions, ObjectSelected, 1)
        //            //concat_obj (upDownObject, ObjectSelected, upDownObject)
        //            //select_obj (SortedRegions, ObjectSelected, Number1)
        //            //concat_obj (upDownObject, ObjectSelected, upDownObject)

        //            //select_shape (ConnectedRegions, SelectedRegions, 'area', 'and', Area[Indices[|Area|-2]], 999999)

        //            //count_obj (upDownObject, Number)
        //            //smallest_rectangle1 (upDownObject, Row1, Column1, Row2, Column2)

        //            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
        //            HOperatorSet.SmallestRectangle1(ho_ROI_0, out hv_Row1, out hv_Column1, out hv_Row2,
        //                out hv_Column2);



        //            hv_findRowArray.Dispose();
        //            hv_findRowArray = new HTuple();
        //            hv_findColArray.Dispose();
        //            hv_findColArray = new HTuple();

        //            //Measure 01: Code generated by Measure 01
        //            //Measure 01: Prepare measurement
        //            hv_AmplitudeThreshold.Dispose();
        //            hv_AmplitudeThreshold = 24;
        //            hv_Sigma.Dispose();
        //            hv_Sigma = 0.8;
        //            hv_RoiWidthLen.Dispose();
        //            hv_RoiWidthLen = 50;

        //            hv_DistanceArray.Dispose();
        //            hv_DistanceArray = new HTuple();
        //            //找点  2个点
        //            ho_CrossObject.Dispose();
        //            HOperatorSet.GenEmptyObj(out ho_CrossObject);
        //            for (hv_Index = 0; (int)hv_Index <= 1; hv_Index = (int)hv_Index + 1)
        //            {
        //                if ((int)(new HTuple(hv_Index.TupleEqual(0))) != 0)
        //                {
        //                    hv_lineCoord.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_lineCoord = new HTuple();
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row1 + 80);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column1);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row1 + 80);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column2);
        //                    }
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        ho_Rectangle.Dispose();
        //                        HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1 + 80, hv_Column1,
        //                            hv_Row1 + 80, hv_Column2);
        //                    }

        //                }
        //                else
        //                {

        //                    hv_lineCoord.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_lineCoord = new HTuple();
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row2 - 100);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column1);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Row2 - 100);
        //                        hv_lineCoord = hv_lineCoord.TupleConcat(hv_Column2);
        //                    }

        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        ho_Rectangle.Dispose();
        //                        HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row2 - 100, hv_Column1,
        //                            hv_Row2 - 100, hv_Column2);
        //                    }
        //                }
        //                hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
        //                measure_1D(ho_ImageReduced, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen,
        //                    hv_lineCoord, out hv_Row_Measure_01_0, out hv_Column_Measure_01_0,
        //                    out hv_Amplitude_Measure_01_0);
        //                if ((int)(new HTuple((new HTuple(hv_Amplitude_Measure_01_0.TupleLength()
        //                    )).TupleGreater(1))) != 0)
        //                {
        //                    hv_Indices1.Dispose();
        //                    HOperatorSet.TupleSortIndex(hv_Column_Measure_01_0, out hv_Indices1);
        //                    hv_findRow.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_findRow = hv_Row_Measure_01_0.TupleSelect(
        //                            hv_Indices1.TupleSelect(0));
        //                    }
        //                    hv_findCol.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_findCol = hv_Column_Measure_01_0.TupleSelect(
        //                            hv_Indices1.TupleSelect(0));
        //                    }
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        {
        //                            HTuple
        //                              ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
        //                                hv_findRow);
        //                            hv_findRowArray.Dispose();
        //                            hv_findRowArray = ExpTmpLocalVar_findRowArray;
        //                        }
        //                    }
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        {
        //                            HTuple
        //                              ExpTmpLocalVar_findColArray = hv_findColArray.TupleConcat(
        //                                hv_findCol);
        //                            hv_findColArray.Dispose();
        //                            hv_findColArray = ExpTmpLocalVar_findColArray;
        //                        }
        //                    }
        //                    ho_Cross.Dispose();
        //                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol,
        //                        26, 0.785398);
        //                    {
        //                        HObject ExpTmpOutVar_0;
        //                        HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                        ho_Cross.Dispose();
        //                        ho_Cross = ExpTmpOutVar_0;
        //                    }
        //                    {
        //                        HObject ExpTmpOutVar_0;
        //                        HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
        //                        ho_CrossObject.Dispose();
        //                        ho_CrossObject = ExpTmpOutVar_0;
        //                    }

        //                    hv_findRow.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_findRow = hv_Row_Measure_01_0.TupleSelect(
        //                            hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
        //                    }
        //                    hv_findCol.Dispose();
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        hv_findCol = hv_Column_Measure_01_0.TupleSelect(
        //                            hv_Indices1.TupleSelect((new HTuple(hv_Indices1.TupleLength())) - 1));
        //                    }
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        {
        //                            HTuple
        //                              ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
        //                                hv_findRow);
        //                            hv_findRowArray.Dispose();
        //                            hv_findRowArray = ExpTmpLocalVar_findRowArray;
        //                        }
        //                    }
        //                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                    {
        //                        {
        //                            HTuple
        //                              ExpTmpLocalVar_findColArray = hv_findColArray.TupleConcat(
        //                                hv_findCol);
        //                            hv_findColArray.Dispose();
        //                            hv_findColArray = ExpTmpLocalVar_findColArray;
        //                        }
        //                    }
        //                    ho_Cross.Dispose();
        //                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol,
        //                        26, 0.785398);
        //                    {
        //                        HObject ExpTmpOutVar_0;
        //                        HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
        //                        ho_Cross.Dispose();
        //                        ho_Cross = ExpTmpOutVar_0;
        //                    }
        //                    {
        //                        HObject ExpTmpOutVar_0;
        //                        HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
        //                        ho_CrossObject.Dispose();
        //                        ho_CrossObject = ExpTmpOutVar_0;
        //                    }
        //                    ho_showRegion.Dispose();
        //                    ho_showRegion = new HObject(ho_CrossObject);
        //                }
        //                using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //                {
        //                    hv_Distance.Dispose();
        //                    HOperatorSet.DistancePp(hv_findRowArray.TupleSelect(hv_Index * 2.0), hv_findColArray.TupleSelect(
        //                        hv_Index * 2.0), hv_findRowArray.TupleSelect((2.0 * hv_Index) + 1), hv_findColArray.TupleSelect(
        //                        (2.0 * hv_Index) + 1), out hv_Distance);
        //                }
        //                if (hv_DistanceArray == null)
        //                    hv_DistanceArray = new HTuple();
        //                hv_DistanceArray[hv_Index] = hv_Distance;


        //            }

        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegion, ho_ROI_0, out ExpTmpOutVar_0);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            //将Region paint到图像上显示
        //            hv_Channels.Dispose();
        //            HOperatorSet.CountChannels(ho_Image, out hv_Channels);
        //            if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
        //            {
        //                hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
        //                HOperatorSet.GetImagePointer1(ho_Image, out hv_Pointer, out hv_Type, out hv_Width,
        //                    out hv_Height);
        //                ho_ImageOut.Dispose();
        //                HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height, hv_Pointer,
        //                    hv_Pointer, hv_Pointer);
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.Boundary(ho_showRegion, out ExpTmpOutVar_0, "inner");
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.DilationCircle(ho_showRegion, out ExpTmpOutVar_0, 2);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegion, ((new HTuple(0)).TupleConcat(
        //                255)).TupleConcat(0), "fill");
        //        }
        //        // catch (Exception) 
        //        catch (HalconException HDevExpDefaultException1)
        //        {
        //            HDevExpDefaultException1.ToHTuple(out hv_Exception);
        //        }
        //        ho_ImageReduced.Dispose();
        //        ho_CrossObject.Dispose();
        //        ho_Rectangle.Dispose();
        //        ho_Cross.Dispose();

        //        hv_Row1.Dispose();
        //        hv_Column1.Dispose();
        //        hv_Row2.Dispose();
        //        hv_Column2.Dispose();
        //        hv_findRowArray.Dispose();
        //        hv_findColArray.Dispose();
        //        hv_AmplitudeThreshold.Dispose();
        //        hv_Sigma.Dispose();
        //        hv_RoiWidthLen.Dispose();
        //        hv_Index.Dispose();
        //        hv_lineCoord.Dispose();
        //        hv_Row_Measure_01_0.Dispose();
        //        hv_Column_Measure_01_0.Dispose();
        //        hv_Amplitude_Measure_01_0.Dispose();
        //        hv_Indices1.Dispose();
        //        hv_findRow.Dispose();
        //        hv_findCol.Dispose();
        //        hv_Distance.Dispose();
        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        return;
        //    }
        //    catch (HalconException HDevExpDefaultException)
        //    {
        //        ho_ImageReduced.Dispose();
        //        ho_CrossObject.Dispose();
        //        ho_Rectangle.Dispose();
        //        ho_Cross.Dispose();

        //        hv_Row1.Dispose();
        //        hv_Column1.Dispose();
        //        hv_Row2.Dispose();
        //        hv_Column2.Dispose();
        //        hv_findRowArray.Dispose();
        //        hv_findColArray.Dispose();
        //        hv_AmplitudeThreshold.Dispose();
        //        hv_Sigma.Dispose();
        //        hv_RoiWidthLen.Dispose();
        //        hv_Index.Dispose();
        //        hv_lineCoord.Dispose();
        //        hv_Row_Measure_01_0.Dispose();
        //        hv_Column_Measure_01_0.Dispose();
        //        hv_Amplitude_Measure_01_0.Dispose();
        //        hv_Indices1.Dispose();
        //        hv_findRow.Dispose();
        //        hv_findCol.Dispose();
        //        hv_Distance.Dispose();
        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        throw HDevExpDefaultException;
        //    }
        //}




        public void measure_1D(HObject ho_Image, HTuple hv_AmplitudeThreshold, HTuple hv_Sigma,
            HTuple hv_RoiWidthLen2, HTuple hv_lineCoord, out HTuple hv_Row_Measure_01_0,
            out HTuple hv_Column_Measure_01_0, out HTuple hv_Amplitude_Measure_01_0)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageOut = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = new HObject(ho_Image);



            // Local control variables 

            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_LineRowStart_Measure_01_0 = new HTuple(), hv_LineColumnStart_Measure_01_0 = new HTuple();
            HTuple hv_LineRowEnd_Measure_01_0 = new HTuple(), hv_LineColumnEnd_Measure_01_0 = new HTuple();
            HTuple hv_TmpCtrl_Row = new HTuple(), hv_TmpCtrl_Column = new HTuple();
            HTuple hv_TmpCtrl_Dr = new HTuple(), hv_TmpCtrl_Dc = new HTuple();
            HTuple hv_TmpCtrl_Phi = new HTuple(), hv_TmpCtrl_Len1 = new HTuple();
            HTuple hv_TmpCtrl_Len2 = new HTuple(), hv_MsrHandle_Measure_01_0 = new HTuple();
            HTuple hv_FuzzyThreshold = new HTuple(), hv_Score_Measure_01_0 = new HTuple();
            HTuple hv_Distance_Measure_01_0 = new HTuple(), hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageOut);
            hv_Row_Measure_01_0 = new HTuple();
            hv_Column_Measure_01_0 = new HTuple();
            hv_Amplitude_Measure_01_0 = new HTuple();
            try
            {
                try
                {

                    ho_ImageOut.Dispose();
                    ho_ImageOut = new HObject(ho_Image_COPY_INP_TMP);
                    //AmplitudeThreshold := 18
                    //RoiWidthLen2 := 3.5
                    HOperatorSet.SetSystem("int_zooming", "true");
                    hv_Width.Dispose(); hv_Height.Dispose();
                    HOperatorSet.GetImageSize(ho_ImageOut, out hv_Width, out hv_Height);
                    //Measure 01: Coordinates for line Measure 01 [0]
                    //LineRowStart_Measure_01_0 := 1284.24
                    //LineColumnStart_Measure_01_0 := 1248.62
                    //LineRowEnd_Measure_01_0 := 1223.32
                    //LineColumnEnd_Measure_01_0 := 1248.62
                    hv_LineRowStart_Measure_01_0.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineRowStart_Measure_01_0 = hv_lineCoord.TupleSelect(
                            0);
                    }
                    hv_LineColumnStart_Measure_01_0.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineColumnStart_Measure_01_0 = hv_lineCoord.TupleSelect(
                            1);
                    }
                    hv_LineRowEnd_Measure_01_0.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineRowEnd_Measure_01_0 = hv_lineCoord.TupleSelect(
                            2);
                    }
                    hv_LineColumnEnd_Measure_01_0.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_LineColumnEnd_Measure_01_0 = hv_lineCoord.TupleSelect(
                            3);
                    }


                    //Measure 01: Convert coordinates to rectangle2 type
                    hv_TmpCtrl_Row.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TmpCtrl_Row = 0.5 * (hv_LineRowStart_Measure_01_0 + hv_LineRowEnd_Measure_01_0);
                    }
                    hv_TmpCtrl_Column.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TmpCtrl_Column = 0.5 * (hv_LineColumnStart_Measure_01_0 + hv_LineColumnEnd_Measure_01_0);
                    }
                    hv_TmpCtrl_Dr.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TmpCtrl_Dr = hv_LineRowStart_Measure_01_0 - hv_LineRowEnd_Measure_01_0;
                    }
                    hv_TmpCtrl_Dc.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TmpCtrl_Dc = hv_LineColumnEnd_Measure_01_0 - hv_LineColumnStart_Measure_01_0;
                    }
                    hv_TmpCtrl_Phi.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TmpCtrl_Phi = hv_TmpCtrl_Dr.TupleAtan2(
                            hv_TmpCtrl_Dc);
                    }
                    hv_TmpCtrl_Len1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_TmpCtrl_Len1 = 0.5 * ((((hv_TmpCtrl_Dr * hv_TmpCtrl_Dr) + (hv_TmpCtrl_Dc * hv_TmpCtrl_Dc))).TupleSqrt()
                            );
                    }
                    hv_TmpCtrl_Len2.Dispose();
                    hv_TmpCtrl_Len2 = new HTuple(hv_RoiWidthLen2);
                    //Measure 01: Create measure for line Measure 01 [0]
                    //Measure 01: Attention: This assumes all images have the same size!
                    hv_MsrHandle_Measure_01_0.Dispose();
                    HOperatorSet.GenMeasureRectangle2(hv_TmpCtrl_Row, hv_TmpCtrl_Column, hv_TmpCtrl_Phi,
                        hv_TmpCtrl_Len1, hv_TmpCtrl_Len2, hv_Width, hv_Height, "nearest_neighbor",
                        out hv_MsrHandle_Measure_01_0);
                    //gen_rectangle2 (Rectangle, TmpCtrl_Row, TmpCtrl_Column, TmpCtrl_Phi, TmpCtrl_Len1, TmpCtrl_Len2)

                    //Measure 01: Set fuzzy functions on measure objects
                    hv_FuzzyThreshold.Dispose();
                    hv_FuzzyThreshold = 0.6;
                    //Measure 01: ***************************************************************
                    //Measure 01: * The code which follows is to be executed once / measurement *
                    //Measure 01: ***************************************************************
                    //Measure 01: The image is assumed to be made available in the
                    //Measure 01: variable last displayed in the graphics window
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.CopyObj(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, 1, 1);
                        ho_Image_COPY_INP_TMP.Dispose();
                        ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                    //Measure 01: Execute measurements
                    hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose(); hv_Score_Measure_01_0.Dispose(); hv_Distance_Measure_01_0.Dispose();
                    HOperatorSet.FuzzyMeasurePos(ho_Image_COPY_INP_TMP, hv_MsrHandle_Measure_01_0,
                        hv_Sigma, hv_AmplitudeThreshold, 0.6, "all", out hv_Row_Measure_01_0,
                        out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0, out hv_Score_Measure_01_0,
                        out hv_Distance_Measure_01_0);
                    //Measure 01: Do something with the results



                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageOut.Dispose();

                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_LineRowStart_Measure_01_0.Dispose();
                hv_LineColumnStart_Measure_01_0.Dispose();
                hv_LineRowEnd_Measure_01_0.Dispose();
                hv_LineColumnEnd_Measure_01_0.Dispose();
                hv_TmpCtrl_Row.Dispose();
                hv_TmpCtrl_Column.Dispose();
                hv_TmpCtrl_Dr.Dispose();
                hv_TmpCtrl_Dc.Dispose();
                hv_TmpCtrl_Phi.Dispose();
                hv_TmpCtrl_Len1.Dispose();
                hv_TmpCtrl_Len2.Dispose();
                hv_MsrHandle_Measure_01_0.Dispose();
                hv_FuzzyThreshold.Dispose();
                hv_Score_Measure_01_0.Dispose();
                hv_Distance_Measure_01_0.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image_COPY_INP_TMP.Dispose();
                ho_ImageOut.Dispose();

                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_LineRowStart_Measure_01_0.Dispose();
                hv_LineColumnStart_Measure_01_0.Dispose();
                hv_LineRowEnd_Measure_01_0.Dispose();
                hv_LineColumnEnd_Measure_01_0.Dispose();
                hv_TmpCtrl_Row.Dispose();
                hv_TmpCtrl_Column.Dispose();
                hv_TmpCtrl_Dr.Dispose();
                hv_TmpCtrl_Dc.Dispose();
                hv_TmpCtrl_Phi.Dispose();
                hv_TmpCtrl_Len1.Dispose();
                hv_TmpCtrl_Len2.Dispose();
                hv_MsrHandle_Measure_01_0.Dispose();
                hv_FuzzyThreshold.Dispose();
                hv_Score_Measure_01_0.Dispose();
                hv_Distance_Measure_01_0.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void MLB_jumper(HObject ho_ROI_0, HObject ho_ROI_1, HObject ho_Image, out HObject ho_showRegion,
            out HObject ho_ImageOut, HTuple hv_BadAreaThre, out HTuple hv_result)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ROI = null, ho_ImageReduced1 = null;
            HObject ho_CrossObject = null, ho_Cross = null, ho_Contour = null;
            HObject ho_ContourRemoved = null, ho_Polygons = null, ho_Region1 = null;
            HObject ho_Rectangle1 = null, ho_ObjectsConcat = null, ho_Rectangle2 = null;
            HObject ho_RegionMoved = null, ho_RegionUnion = null, ho_RegionFillUp = null;
            HObject ho_RegionOpening = null, ho_RegionErosion = null, ho_ImageReduced2 = null;
            HObject ho_Region = null, ho_ConnectedRegions = null, ho_showRegions = null;
            HObject ho_showRegionROI = null;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_findRowArray = new HTuple(), hv_findColArray = new HTuple();
            HTuple hv_AmplitudeThreshold = new HTuple(), hv_Sigma = new HTuple();
            HTuple hv_RoiWidthLen = new HTuple(), hv_Index = new HTuple();
            HTuple hv_MeasurelineCoord = new HTuple(), hv_Row_Measure_01_0 = new HTuple();
            HTuple hv_Column_Measure_01_0 = new HTuple(), hv_Amplitude_Measure_01_0 = new HTuple();
            HTuple hv_findRow = new HTuple(), hv_findCol = new HTuple();
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
            HTuple hv_removeIndex = new HTuple(), hv_Mean = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Col = new HTuple(), hv_Length = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_column1 = new HTuple();
            HTuple hv_column2 = new HTuple(), hv_Number = new HTuple();
            HTuple hv_Channels = new HTuple(), hv_Pointer = new HTuple();
            HTuple hv_Type = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_showRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageOut);
            HOperatorSet.GenEmptyObj(out ho_ROI);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_CrossObject);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ContourRemoved);
            HOperatorSet.GenEmptyObj(out ho_Polygons);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_showRegions);
            HOperatorSet.GenEmptyObj(out ho_showRegionROI);
            hv_result = new HTuple();
            try
            {
                try
                {
                    ho_ImageOut.Dispose();
                    ho_ImageOut = new HObject(ho_Image);
                    ho_showRegion.Dispose();
                    HOperatorSet.GenEmptyRegion(out ho_showRegion);


                    ho_ROI.Dispose();
                    HOperatorSet.Difference(ho_ROI_0, ho_ROI_1, out ho_ROI);
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_ROI, out ho_ImageReduced1);
                    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_ROI, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2);


                    hv_findRowArray.Dispose();
                    hv_findRowArray = new HTuple();
                    hv_findColArray.Dispose();
                    hv_findColArray = new HTuple();
                    //Measure 01: Code generated by Measure 01
                    //Measure 01: Prepare measurement
                    hv_AmplitudeThreshold.Dispose();
                    hv_AmplitudeThreshold = 22;
                    hv_Sigma.Dispose();
                    hv_Sigma = 1.4;
                    hv_RoiWidthLen.Dispose();
                    hv_RoiWidthLen = 100;
                    ho_CrossObject.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_CrossObject);
                    for (hv_Index = 0; (int)hv_Index <= 10; hv_Index = (int)hv_Index + 1)
                    {

                        hv_MeasurelineCoord.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_MeasurelineCoord = new HTuple();
                            hv_MeasurelineCoord = hv_MeasurelineCoord.TupleConcat(hv_Row1);
                            hv_MeasurelineCoord = hv_MeasurelineCoord.TupleConcat(hv_Column1 + (((hv_Column2 - hv_Column1) * hv_Index) / 10));
                            hv_MeasurelineCoord = hv_MeasurelineCoord.TupleConcat(hv_Row1 + 240);
                            hv_MeasurelineCoord = hv_MeasurelineCoord.TupleConcat(hv_Column1 + (((hv_Column2 - hv_Column1) * hv_Index) / 10));
                        }
                        hv_Row_Measure_01_0.Dispose(); hv_Column_Measure_01_0.Dispose(); hv_Amplitude_Measure_01_0.Dispose();
                        measure_1D(ho_Image, hv_AmplitudeThreshold, hv_Sigma, hv_RoiWidthLen, hv_MeasurelineCoord,
                            out hv_Row_Measure_01_0, out hv_Column_Measure_01_0, out hv_Amplitude_Measure_01_0);
                        //tuple_sort_index (Amplitude_Measure_01_0, Indices1)
                        if ((int)(new HTuple((new HTuple(hv_Row_Measure_01_0.TupleLength())).TupleNotEqual(
                            0))) != 0)
                        {
                            hv_findRow.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_findRow = hv_Row_Measure_01_0.TupleSelect(
                                    0);
                            }
                            hv_findCol.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_findCol = hv_Column_Measure_01_0.TupleSelect(
                                    0);
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                {
                                    HTuple
                                      ExpTmpLocalVar_findRowArray = hv_findRowArray.TupleConcat(
                                        hv_findRow);
                                    hv_findRowArray.Dispose();
                                    hv_findRowArray = ExpTmpLocalVar_findRowArray;
                                }
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                {
                                    HTuple
                                      ExpTmpLocalVar_findColArray = hv_findColArray.TupleConcat(
                                        hv_findCol);
                                    hv_findColArray.Dispose();
                                    hv_findColArray = ExpTmpLocalVar_findColArray;
                                }
                            }
                            ho_Cross.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_findRow, hv_findCol,
                                26, 0.785398);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.GenRegionContourXld(ho_Cross, out ExpTmpOutVar_0, "margin");
                                ho_Cross.Dispose();
                                ho_Cross = ExpTmpOutVar_0;
                            }
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_CrossObject, ho_Cross, out ExpTmpOutVar_0);
                                ho_CrossObject.Dispose();
                                ho_CrossObject = ExpTmpOutVar_0;
                            }
                        }

                    }
                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_findRowArray, hv_findColArray);

                    hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
                    HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
                        out hv_Dist);

                    //剔除离散值
                    hv_removeIndex.Dispose();
                    hv_removeIndex = new HTuple();
                    hv_Mean.Dispose();
                    HOperatorSet.TupleMean(hv_findRowArray, out hv_Mean);
                    hv_Indices.Dispose();
                    HOperatorSet.TupleSortIndex(hv_findRowArray, out hv_Indices);
                    while ((int)(new HTuple(((hv_findRowArray.TupleSelect(hv_Indices.TupleSelect(
                        (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleGreater(hv_Mean + 40))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleRemove(hv_findRowArray, hv_Indices.TupleSelect((new HTuple(hv_Indices.TupleLength()
                                )) - 1), out ExpTmpOutVar_0);
                            hv_findRowArray.Dispose();
                            hv_findRowArray = ExpTmpOutVar_0;
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_removeIndex = hv_removeIndex.TupleConcat(
                                    hv_Indices.TupleSelect((new HTuple(hv_Indices.TupleLength())) - 1));
                                hv_removeIndex.Dispose();
                                hv_removeIndex = ExpTmpLocalVar_removeIndex;
                            }
                        }
                        hv_Indices.Dispose();
                        HOperatorSet.TupleSortIndex(hv_findRowArray, out hv_Indices);

                    }

                    hv_Indices.Dispose();
                    HOperatorSet.TupleSortIndex(hv_findRowArray, out hv_Indices);
                    while ((int)(new HTuple(((hv_findRowArray.TupleSelect(hv_Indices.TupleSelect(
                        0)))).TupleLess(hv_Mean - 40))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleRemove(hv_findRowArray, hv_Indices.TupleSelect(0), out ExpTmpOutVar_0);
                            hv_findRowArray.Dispose();
                            hv_findRowArray = ExpTmpOutVar_0;
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_removeIndex = hv_removeIndex.TupleConcat(
                                    hv_Indices.TupleSelect(0));
                                hv_removeIndex.Dispose();
                                hv_removeIndex = ExpTmpLocalVar_removeIndex;
                            }
                        }
                        hv_Indices.Dispose();
                        HOperatorSet.TupleSortIndex(hv_findRowArray, out hv_Indices);

                    }

                    for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_removeIndex.TupleLength()
                        )) - 1); hv_Index1 = (int)hv_Index1 + 1)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleRemove(hv_findColArray, hv_removeIndex.TupleSelect(hv_Index1),
                                out ExpTmpOutVar_0);
                            hv_findColArray.Dispose();
                            hv_findColArray = ExpTmpOutVar_0;
                        }
                    }


                    ho_ContourRemoved.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_ContourRemoved, hv_findRowArray,
                        hv_findColArray);


                    ho_Polygons.Dispose();
                    HOperatorSet.GenPolygonsXld(ho_ContourRemoved, out ho_Polygons, "ramer",
                        2);
                    hv_Row.Dispose(); hv_Col.Dispose(); hv_Length.Dispose(); hv_Phi.Dispose();
                    HOperatorSet.GetPolygonXld(ho_Polygons, out hv_Row, out hv_Col, out hv_Length,
                        out hv_Phi);
                    ho_Region1.Dispose();
                    HOperatorSet.GenRegionPolygon(out ho_Region1, hv_Row, hv_Col);

                    hv_column1.Dispose();
                    HOperatorSet.RegionFeatures(ho_Region1, "column1", out hv_column1);
                    hv_column2.Dispose();
                    HOperatorSet.RegionFeatures(ho_Region1, "column2", out hv_column2);

                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle1, 0, hv_column1, 2000, hv_column1);
                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_Region1, ho_Rectangle1, out ho_ObjectsConcat);

                    ho_Rectangle2.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle2, 0, hv_column2, 2000, hv_column2);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Rectangle2, out ExpTmpOutVar_0
                            );
                        ho_ObjectsConcat.Dispose();
                        ho_ObjectsConcat = ExpTmpOutVar_0;
                    }

                    ho_RegionMoved.Dispose();
                    HOperatorSet.MoveRegion(ho_Region1, out ho_RegionMoved, 280, 0);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_RegionMoved, out ExpTmpOutVar_0
                            );
                        ho_ObjectsConcat.Dispose();
                        ho_ObjectsConcat = ExpTmpOutVar_0;
                    }
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_ObjectsConcat, out ho_RegionUnion);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUp(ho_RegionUnion, out ho_RegionFillUp);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.OpeningRectangle1(ho_RegionFillUp, out ho_RegionOpening, 10,
                        10);

                    ho_RegionErosion.Dispose();
                    HOperatorSet.ErosionRectangle1(ho_RegionOpening, out ho_RegionErosion, 2,
                        21);
                    ho_ImageReduced2.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_RegionErosion, out ho_ImageReduced2
                        );
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced2, out ho_Region, 0, 35);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

                    ho_showRegion.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_showRegion, "area",
                        "and", hv_BadAreaThre, 99999);
                    hv_Number.Dispose();
                    HOperatorSet.CountObj(ho_showRegion, out hv_Number);
                    ho_showRegions.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_showRegions);
                    ho_showRegionROI.Dispose();
                    HOperatorSet.Boundary(ho_RegionOpening, out ho_showRegionROI, "inner");

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_showRegions, ho_showRegion, out ExpTmpOutVar_0);
                        ho_showRegions.Dispose();
                        ho_showRegions = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_showRegions, ho_showRegionROI, out ExpTmpOutVar_0
                            );
                        ho_showRegions.Dispose();
                        ho_showRegions = ExpTmpOutVar_0;
                    }

                    if ((int)(new HTuple(hv_Number.TupleGreater(0))) != 0)
                    {
                        hv_result.Dispose();
                        hv_result = 1;
                        //将Region paint到图像上显示
                        hv_Channels.Dispose();
                        HOperatorSet.CountChannels(ho_Image, out hv_Channels);
                        if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
                        {
                            hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
                            HOperatorSet.GetImagePointer1(ho_Image, out hv_Pointer, out hv_Type,
                                out hv_Width, out hv_Height);
                            ho_ImageOut.Dispose();
                            HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height,
                                hv_Pointer, hv_Pointer, hv_Pointer);
                        }
                        ho_showRegion.Dispose();
                        HOperatorSet.Boundary(ho_showRegions, out ho_showRegion, "inner");
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.DilationCircle(ho_showRegion, out ExpTmpOutVar_0, 2);
                            ho_showRegion.Dispose();
                            ho_showRegion = ExpTmpOutVar_0;
                        }
                        HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegion, ((new HTuple(255)).TupleConcat(
                            0)).TupleConcat(0), "fill");
                    }

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }

                ho_ROI.Dispose();
                ho_ImageReduced1.Dispose();
                ho_CrossObject.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_ContourRemoved.Dispose();
                ho_Polygons.Dispose();
                ho_Region1.Dispose();
                ho_Rectangle1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_Rectangle2.Dispose();
                ho_RegionMoved.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageReduced2.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_showRegions.Dispose();
                ho_showRegionROI.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_findRowArray.Dispose();
                hv_findColArray.Dispose();
                hv_AmplitudeThreshold.Dispose();
                hv_Sigma.Dispose();
                hv_RoiWidthLen.Dispose();
                hv_Index.Dispose();
                hv_MeasurelineCoord.Dispose();
                hv_Row_Measure_01_0.Dispose();
                hv_Column_Measure_01_0.Dispose();
                hv_Amplitude_Measure_01_0.Dispose();
                hv_findRow.Dispose();
                hv_findCol.Dispose();
                hv_RowBegin.Dispose();
                hv_ColBegin.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Nr.Dispose();
                hv_Nc.Dispose();
                hv_Dist.Dispose();
                hv_removeIndex.Dispose();
                hv_Mean.Dispose();
                hv_Indices.Dispose();
                hv_Index1.Dispose();
                hv_Row.Dispose();
                hv_Col.Dispose();
                hv_Length.Dispose();
                hv_Phi.Dispose();
                hv_column1.Dispose();
                hv_column2.Dispose();
                hv_Number.Dispose();
                hv_Channels.Dispose();
                hv_Pointer.Dispose();
                hv_Type.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ROI.Dispose();
                ho_ImageReduced1.Dispose();
                ho_CrossObject.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_ContourRemoved.Dispose();
                ho_Polygons.Dispose();
                ho_Region1.Dispose();
                ho_Rectangle1.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_Rectangle2.Dispose();
                ho_RegionMoved.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageReduced2.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_showRegions.Dispose();
                ho_showRegionROI.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_findRowArray.Dispose();
                hv_findColArray.Dispose();
                hv_AmplitudeThreshold.Dispose();
                hv_Sigma.Dispose();
                hv_RoiWidthLen.Dispose();
                hv_Index.Dispose();
                hv_MeasurelineCoord.Dispose();
                hv_Row_Measure_01_0.Dispose();
                hv_Column_Measure_01_0.Dispose();
                hv_Amplitude_Measure_01_0.Dispose();
                hv_findRow.Dispose();
                hv_findCol.Dispose();
                hv_RowBegin.Dispose();
                hv_ColBegin.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Nr.Dispose();
                hv_Nc.Dispose();
                hv_Dist.Dispose();
                hv_removeIndex.Dispose();
                hv_Mean.Dispose();
                hv_Indices.Dispose();
                hv_Index1.Dispose();
                hv_Row.Dispose();
                hv_Col.Dispose();
                hv_Length.Dispose();
                hv_Phi.Dispose();
                hv_column1.Dispose();
                hv_column2.Dispose();
                hv_Number.Dispose();
                hv_Channels.Dispose();
                hv_Pointer.Dispose();
                hv_Type.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void MLB_jumper_flex_B2B(HObject ho_Image, HObject ho_ROI_0, HObject ho_ROI_1,
      out HObject ho_ShowRegion, HTuple hv_BaseOffsetX, HTuple hv_BaseOffsetY, HTuple hv_Tuple,
      HTuple hv_Pix2mm, out HTuple hv_OutOffsetX, out HTuple hv_OutOffsetY)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Regions, ho_Line, ho_Regions1, ho_Line1;
            HObject ho_Cross, ho_Cross1, ho_ObjectsConcat;

            // Local control variables 

            HTuple hv_MeasureRow = new HTuple(), hv_MeasureCol = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_CornerY = new HTuple();
            HTuple hv_CornerX = new HTuple(), hv_LineCenterY = new HTuple();
            HTuple hv_LineCenterX = new HTuple(), hv_ResultRow = new HTuple();
            HTuple hv_ResultColumn = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Column3 = new HTuple(), hv_Phi1 = new HTuple();
            HTuple hv_Length11 = new HTuple(), hv_Length21 = new HTuple();
            HTuple hv_CornerY1 = new HTuple(), hv_CornerX1 = new HTuple();
            HTuple hv_LineCenterY1 = new HTuple(), hv_LineCenterX1 = new HTuple();
            HTuple hv_ResultRow1 = new HTuple(), hv_ResultColumn1 = new HTuple();
            HTuple hv_Row11 = new HTuple(), hv_Column11 = new HTuple();
            HTuple hv_Row21 = new HTuple(), hv_Column21 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple(), hv_IsParallel = new HTuple();
            HTuple hv_Index1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            HOperatorSet.GenEmptyObj(out ho_Line1);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            hv_OutOffsetX = new HTuple();
            hv_OutOffsetY = new HTuple();
            hv_OutOffsetX.Dispose();
            hv_OutOffsetX = new HTuple();
            hv_OutOffsetY.Dispose();
            hv_OutOffsetY = new HTuple();
            hv_MeasureRow.Dispose();
            hv_MeasureRow = new HTuple();
            hv_MeasureCol.Dispose();
            hv_MeasureCol = new HTuple();
            hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
            HOperatorSet.SmallestRectangle2(ho_ROI_0, out hv_Row, out hv_Column, out hv_Phi,
                out hv_Length1, out hv_Length2);
            hv_CornerY.Dispose(); hv_CornerX.Dispose(); hv_LineCenterY.Dispose(); hv_LineCenterX.Dispose();
            get_rectangle2_points(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2, out hv_CornerY,
                out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
                rake(ho_Image, out ho_Regions, 30, 30, 15, 1, 30, "positive", "first", hv_LineCenterY.TupleSelect(
                    3), hv_LineCenterX.TupleSelect(3), hv_LineCenterY.TupleSelect(1), hv_LineCenterX.TupleSelect(
                    1), out hv_ResultRow, out hv_ResultColumn);
            }
            ho_Line.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
            pts_to_best_line(out ho_Line, hv_ResultRow, hv_ResultColumn, 5, out hv_Row1,
                out hv_Column1, out hv_Row2, out hv_Column2);
            if ((int)(new HTuple(hv_Row1.TupleEqual(0))) != 0)
            {
                ho_ShowRegion.Dispose();
                HOperatorSet.ConcatObj(ho_ROI_0, ho_ROI_1, out ho_ShowRegion);
            }
            hv_Row3.Dispose(); hv_Column3.Dispose(); hv_Phi1.Dispose(); hv_Length11.Dispose(); hv_Length21.Dispose();
            HOperatorSet.SmallestRectangle2(ho_ROI_1, out hv_Row3, out hv_Column3, out hv_Phi1,
                out hv_Length11, out hv_Length21);
            hv_CornerY1.Dispose(); hv_CornerX1.Dispose(); hv_LineCenterY1.Dispose(); hv_LineCenterX1.Dispose();
            get_rectangle2_points(hv_Row3, hv_Column3, hv_Phi1, hv_Length11, hv_Length21,
                out hv_CornerY1, out hv_CornerX1, out hv_LineCenterY1, out hv_LineCenterX1);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions1.Dispose(); hv_ResultRow1.Dispose(); hv_ResultColumn1.Dispose();
                rake(ho_Image, out ho_Regions1, 30, 30, 15, 1, 30, "positive", "max", hv_LineCenterY1.TupleSelect(
                    0), hv_LineCenterX1.TupleSelect(0), hv_LineCenterY1.TupleSelect(2), hv_LineCenterX1.TupleSelect(
                    2), out hv_ResultRow1, out hv_ResultColumn1);
            }
            ho_Line1.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose();
            pts_to_best_line(out ho_Line1, hv_ResultRow1, hv_ResultColumn1, 5, out hv_Row11,
                out hv_Column11, out hv_Row21, out hv_Column21);
            if ((int)(new HTuple(hv_Row11.TupleEqual(0))) != 0)
            {
                ho_ShowRegion.Dispose();
                HOperatorSet.ConcatObj(ho_ROI_0, ho_ROI_1, out ho_ShowRegion);
            }
            hv_Angle.Dispose();
            HOperatorSet.AngleLx(hv_Row11, hv_Column11, hv_Row21, hv_Column21, out hv_Angle);
            hv_Row4.Dispose(); hv_Column4.Dispose(); hv_IsParallel.Dispose();
            HOperatorSet.IntersectionLl(hv_Row1, hv_Column1, hv_Row2, hv_Column2, hv_Row11,
                hv_Column11, hv_Row21, hv_Column21, out hv_Row4, out hv_Column4, out hv_IsParallel);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row4, hv_Column4, 36, (new HTuple(45)).TupleRad()
                    );
            }
            for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_BaseOffsetX.TupleLength()
                )) - 1); hv_Index1 = (int)hv_Index1 + 1)
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.TupleConcat(hv_MeasureRow, hv_Row4 + ((hv_BaseOffsetX.TupleSelect(
                        hv_Index1)) / hv_Pix2mm), out ExpTmpOutVar_0);
                    hv_MeasureRow.Dispose();
                    hv_MeasureRow = ExpTmpOutVar_0;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.TupleConcat(hv_MeasureCol, hv_Column4 + ((hv_BaseOffsetY.TupleSelect(
                        hv_Index1)) / hv_Pix2mm), out ExpTmpOutVar_0);
                    hv_MeasureCol.Dispose();
                    hv_MeasureCol = ExpTmpOutVar_0;
                }
            }
            ho_Cross1.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_MeasureRow, hv_MeasureCol,
                26, hv_Angle);
            hv_OutOffsetX.Dispose(); hv_OutOffsetY.Dispose();
            HOperatorSet.AffineTransPoint2d(hv_Tuple, hv_MeasureRow, hv_MeasureCol, out hv_OutOffsetX,
                out hv_OutOffsetY);
            ho_ObjectsConcat.Dispose();
            HOperatorSet.ConcatObj(ho_Line, ho_Line1, out ho_ObjectsConcat);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross, out ExpTmpOutVar_0);
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat = ExpTmpOutVar_0;
            }
            ho_ShowRegion.Dispose();
            HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross1, out ho_ShowRegion);
            ho_Regions.Dispose();
            ho_Line.Dispose();
            ho_Regions1.Dispose();
            ho_Line1.Dispose();
            ho_Cross.Dispose();
            ho_Cross1.Dispose();
            ho_ObjectsConcat.Dispose();

            hv_MeasureRow.Dispose();
            hv_MeasureCol.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Phi.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
            hv_CornerY.Dispose();
            hv_CornerX.Dispose();
            hv_LineCenterY.Dispose();
            hv_LineCenterX.Dispose();
            hv_ResultRow.Dispose();
            hv_ResultColumn.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Row3.Dispose();
            hv_Column3.Dispose();
            hv_Phi1.Dispose();
            hv_Length11.Dispose();
            hv_Length21.Dispose();
            hv_CornerY1.Dispose();
            hv_CornerX1.Dispose();
            hv_LineCenterY1.Dispose();
            hv_LineCenterX1.Dispose();
            hv_ResultRow1.Dispose();
            hv_ResultColumn1.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_Angle.Dispose();
            hv_Row4.Dispose();
            hv_Column4.Dispose();
            hv_IsParallel.Dispose();
            hv_Index1.Dispose();

            return;
        }

        //public void MLB_jumper_flex_B2B(HObject ho_Image, HObject ho_ROI_0, out HObject ho_ImageOut,
        //    out HObject ho_showRegion, out HTuple hv_coordRow, out HTuple hv_coordCol)
        //{



        //    // Stack for temporary objects 
        //    HObject[] OTemp = new HObject[20];

        //    // Local iconic variables 

        //    HObject ho_ImageReduced = null, ho_Region = null;
        //    HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
        //    HObject ho_showRegionX = null, ho_showRegionY = null;

        //    // Local control variables 

        //    HTuple hv_Channels = new HTuple(), hv_Pointer = new HTuple();
        //    HTuple hv_Type = new HTuple(), hv_Width = new HTuple();
        //    HTuple hv_Height = new HTuple(), hv_Exception = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_ImageOut);
        //    HOperatorSet.GenEmptyObj(out ho_showRegion);
        //    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //    HOperatorSet.GenEmptyObj(out ho_Region);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_showRegionX);
        //    HOperatorSet.GenEmptyObj(out ho_showRegionY);
        //    hv_coordRow = new HTuple();
        //    hv_coordCol = new HTuple();
        //    try
        //    {
        //        try
        //        {


        //            ho_ImageReduced.Dispose();
        //            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
        //            ho_Region.Dispose();
        //            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 170, 255);
        //            ho_ConnectedRegions.Dispose();
        //            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
        //            ho_SelectedRegions.Dispose();
        //            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions,
        //                "max_area", 70);

        //            ho_showRegionX.Dispose(); hv_coordRow.Dispose();
        //            findXline(ho_SelectedRegions, ho_Image, out ho_showRegionX, out hv_coordRow);
        //            ho_showRegionY.Dispose(); hv_coordCol.Dispose();
        //            findYline(ho_SelectedRegions, ho_Image, out ho_showRegionY, out hv_coordCol);
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                {
        //                    HTuple
        //                      ExpTmpLocalVar_coordCol = hv_coordCol + 40;
        //                    hv_coordCol.Dispose();
        //                    hv_coordCol = ExpTmpLocalVar_coordCol;
        //                }
        //            }
        //            ho_showRegion.Dispose();
        //            HOperatorSet.ConcatObj(ho_showRegionX, ho_showRegionY, out ho_showRegion);

        //            //将Region paint到图像上显示
        //            hv_Channels.Dispose();
        //            HOperatorSet.CountChannels(ho_Image, out hv_Channels);
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegion, ho_ROI_0, out ExpTmpOutVar_0);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
        //            {
        //                hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
        //                HOperatorSet.GetImagePointer1(ho_Image, out hv_Pointer, out hv_Type, out hv_Width,
        //                    out hv_Height);
        //                ho_ImageOut.Dispose();
        //                HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height, hv_Pointer,
        //                    hv_Pointer, hv_Pointer);
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.Boundary(ho_showRegion, out ExpTmpOutVar_0, "inner");
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.DilationCircle(ho_showRegion, out ExpTmpOutVar_0, 2);
        //                ho_showRegion.Dispose();
        //                ho_showRegion = ExpTmpOutVar_0;
        //            }
        //            HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegion, ((new HTuple(0)).TupleConcat(
        //                255)).TupleConcat(0), "fill");
        //        }
        //        // catch (Exception) 
        //        catch (HalconException HDevExpDefaultException1)
        //        {
        //            HDevExpDefaultException1.ToHTuple(out hv_Exception);
        //        }
        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_showRegionX.Dispose();
        //        ho_showRegionY.Dispose();

        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        return;
        //    }
        //    catch (HalconException HDevExpDefaultException)
        //    {
        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_showRegionX.Dispose();
        //        ho_showRegionY.Dispose();

        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        throw HDevExpDefaultException;
        //    }
        //}

        public void selectMaxLine(HObject ho_Contours, out HObject ho_MaxLengthContour)
        {



            // Local iconic variables 

            HObject ho_ObjectSelected = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv_Max_Length = new HTuple();
            HTuple hv_Max_Index = new HTuple(), hv_i = new HTuple();
            HTuple hv_Length = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_MaxLengthContour);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            try
            {
                hv_Number.Dispose();
                HOperatorSet.CountObj(ho_Contours, out hv_Number);
                hv_Max_Length.Dispose();
                hv_Max_Length = 0;
                hv_Max_Index.Dispose();
                hv_Max_Index = 0;
                //遍历每个轮廓的长度
                HTuple end_val4 = hv_Number;
                HTuple step_val4 = 1;
                for (hv_i = 1; hv_i.Continue(end_val4, step_val4); hv_i = hv_i.TupleAdd(step_val4))
                {
                    //选择轮廓
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Contours, out ho_ObjectSelected, hv_i);
                    //求轮廓长度
                    hv_Length.Dispose();
                    HOperatorSet.LengthXld(ho_ObjectSelected, out hv_Length);
                    //保存最长轮廓的长度和索引
                    if ((int)(new HTuple(hv_Max_Length.TupleLess(hv_Length))) != 0)
                    {
                        hv_Max_Length.Dispose();
                        hv_Max_Length = new HTuple(hv_Length);
                        hv_Max_Index.Dispose();
                        hv_Max_Index = new HTuple(hv_i);
                    }
                }
                //选择最长轮廓
                ho_MaxLengthContour.Dispose();
                HOperatorSet.SelectObj(ho_Contours, out ho_MaxLengthContour, hv_Max_Index);
                ho_ObjectSelected.Dispose();

                hv_Number.Dispose();
                hv_Max_Length.Dispose();
                hv_Max_Index.Dispose();
                hv_i.Dispose();
                hv_Length.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ObjectSelected.Dispose();

                hv_Number.Dispose();
                hv_Max_Length.Dispose();
                hv_Max_Index.Dispose();
                hv_i.Dispose();
                hv_Length.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Local procedures 
        public void Mesa_Jumper_Flex(HObject ho_Image, HObject ho_ROI_Mesa, out HObject ho_ShowRegion,
         out HObject ho_ImageOut, out HTuple hv_result)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced = null, ho_Edges = null;
            HObject ho_ContoursSplit = null, ho_SelectedLines = null, ho_UnionContours1 = null;
            HObject ho_UnionContours = null, ho_SelectedXLD = null, ho_MaxLengthContour = null;
            HObject ho_RegionCut = null, ho_Rectangle1 = null, ho_RegionOpening = null;
            HObject ho_Rectangle = null, ho_RectangleClip = null, ho_RegionErosion = null;
            HObject ho_ImageReduced1 = null, ho_Region = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_ImageReduced2 = null;
            HObject ho_ImageResult = null;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_Row3 = new HTuple(), hv_Column3 = new HTuple();
            HTuple hv_Phi1 = new HTuple(), hv_Length11 = new HTuple();
            HTuple hv_Length21 = new HTuple(), hv_Value = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_Number = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageOut);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_SelectedLines);
            HOperatorSet.GenEmptyObj(out ho_UnionContours1);
            HOperatorSet.GenEmptyObj(out ho_UnionContours);
            HOperatorSet.GenEmptyObj(out ho_SelectedXLD);
            HOperatorSet.GenEmptyObj(out ho_MaxLengthContour);
            HOperatorSet.GenEmptyObj(out ho_RegionCut);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RectangleClip);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            hv_result = new HTuple();
            try
            {
                try
                {
                    hv_result.Dispose();
                    hv_result = 0;
                    ho_ImageOut.Dispose();
                    ho_ImageOut = new HObject(ho_Image);
                    ho_ShowRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ShowRegion);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_ROI_Mesa, out ho_ImageReduced);
                    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_ROI_Mesa, out hv_Row1, out hv_Column1,
                        out hv_Row2, out hv_Column2);
                    ho_Edges.Dispose();
                    HOperatorSet.EdgesSubPix(ho_ImageReduced, out ho_Edges, "canny", 5, 5, 35);
                    ho_ContoursSplit.Dispose();
                    HOperatorSet.SegmentContoursXld(ho_Edges, out ho_ContoursSplit, "lines_circles",
                        10, 20, 2);
                    ho_SelectedLines.Dispose();
                    HOperatorSet.SelectShapeXld(ho_ContoursSplit, out ho_SelectedLines, (new HTuple("phi_points")).TupleConcat(
                        "width"), "and", (new HTuple(-0.1)).TupleConcat(100), (new HTuple(0.1)).TupleConcat(
                        99999));
                    ho_UnionContours1.Dispose();
                    HOperatorSet.UnionCollinearContoursXld(ho_SelectedLines, out ho_UnionContours1,
                        200, 1, 2, 0.1, "attr_keep");
                    ho_UnionContours.Dispose();
                    HOperatorSet.UnionAdjacentContoursXld(ho_UnionContours1, out ho_UnionContours,
                        10, 1, "attr_keep");
                    ho_SelectedXLD.Dispose();
                    HOperatorSet.SelectShapeXld(ho_UnionContours, out ho_SelectedXLD, "width",
                        "and", 300, 99999);
                    ho_UnionContours.Dispose();
                    HOperatorSet.UnionAdjacentContoursXld(ho_SelectedXLD, out ho_UnionContours,
                        1000, 1, "attr_keep");

                    ho_MaxLengthContour.Dispose();
                    selectMaxLine(ho_UnionContours, out ho_MaxLengthContour);
                    ho_RegionCut.Dispose();
                    HOperatorSet.GenRegionContourXld(ho_MaxLengthContour, out ho_RegionCut, "filled");
                    hv_Row3.Dispose(); hv_Column3.Dispose(); hv_Phi1.Dispose(); hv_Length11.Dispose(); hv_Length21.Dispose();
                    HOperatorSet.SmallestRectangle2(ho_RegionCut, out hv_Row3, out hv_Column3,
                        out hv_Phi1, out hv_Length11, out hv_Length21);
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_Row3, hv_Column3, hv_Phi1,
                        200, 10);
                    ho_RegionOpening.Dispose();
                    HOperatorSet.Opening(ho_RegionCut, ho_Rectangle1, out ho_RegionOpening);
                    hv_Value.Dispose();
                    HOperatorSet.RegionFeatures(ho_RegionOpening, "width", out hv_Value);
                    hv_Width.Dispose(); hv_Height.Dispose();
                    HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                    if ((int)(new HTuple(hv_Value.TupleLess((hv_Column2 - hv_Column1) / 2))) != 0)
                    {
                        hv_result.Dispose();
                        hv_result = 1;
                        ho_ImageReduced.Dispose();
                        ho_Edges.Dispose();
                        ho_ContoursSplit.Dispose();
                        ho_SelectedLines.Dispose();
                        ho_UnionContours1.Dispose();
                        ho_UnionContours.Dispose();
                        ho_SelectedXLD.Dispose();
                        ho_MaxLengthContour.Dispose();
                        ho_RegionCut.Dispose();
                        ho_Rectangle1.Dispose();
                        ho_RegionOpening.Dispose();
                        ho_Rectangle.Dispose();
                        ho_RectangleClip.Dispose();
                        ho_RegionErosion.Dispose();
                        ho_ImageReduced1.Dispose();
                        ho_Region.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_SelectedRegions.Dispose();
                        ho_ImageReduced2.Dispose();
                        ho_ImageResult.Dispose();

                        hv_Row1.Dispose();
                        hv_Column1.Dispose();
                        hv_Row2.Dispose();
                        hv_Column2.Dispose();
                        hv_Row3.Dispose();
                        hv_Column3.Dispose();
                        hv_Phi1.Dispose();
                        hv_Length11.Dispose();
                        hv_Length21.Dispose();
                        hv_Value.Dispose();
                        hv_Width.Dispose();
                        hv_Height.Dispose();
                        hv_Row.Dispose();
                        hv_Column.Dispose();
                        hv_Phi.Dispose();
                        hv_Length1.Dispose();
                        hv_Length2.Dispose();
                        hv_Number.Dispose();
                        hv_Exception.Dispose();

                        return;
                    }
                    hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
                    HOperatorSet.SmallestRectangle2(ho_RegionOpening, out hv_Row, out hv_Column,
                        out hv_Phi, out hv_Length1, out hv_Length2);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Column2 - hv_Column1,
                            hv_Length2);
                    }
                    ho_RectangleClip.Dispose();
                    HOperatorSet.ClipRegion(ho_Rectangle, out ho_RectangleClip, hv_Row1, hv_Column1,
                        hv_Row2, hv_Column2);
                    ho_RegionErosion.Dispose();
                    HOperatorSet.ErosionRectangle1(ho_RectangleClip, out ho_RegionErosion, 20,
                        10);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_ShowRegion, ho_RegionErosion, out ExpTmpOutVar_0
                            );
                        ho_ShowRegion.Dispose();
                        ho_ShowRegion = ExpTmpOutVar_0;
                    }
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_RegionErosion, out ho_ImageReduced1);
                    HOperatorSet.MultImage(ho_ImageReduced1, ho_ImageReduced1, out ho_ImageReduced1, 0.005, 0);
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region, 0, 130);
                    HOperatorSet.ErosionCircle(ho_Region, out ho_Region, 3.5);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                        "and", 100, 999999);
                    hv_Number.Dispose();
                    HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleGreater(0))) != 0)
                    {
                        hv_result.Dispose();
                        hv_result = 1;

                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ShowRegion, ho_SelectedRegions, out ExpTmpOutVar_0
                                );
                            ho_ShowRegion.Dispose();
                            ho_ShowRegion = ExpTmpOutVar_0;
                        }

                    }
                    ho_ImageReduced2.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_ShowRegion, out ho_ImageReduced2);
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageReduced2, ho_ImageReduced2, out ho_ImageResult,
                        0.005, 0);

                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_result.Dispose();
                    hv_result = 1;
                }


                ho_ImageReduced.Dispose();
                ho_Edges.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedLines.Dispose();
                ho_UnionContours1.Dispose();
                ho_UnionContours.Dispose();
                ho_SelectedXLD.Dispose();
                ho_MaxLengthContour.Dispose();
                ho_RegionCut.Dispose();
                ho_Rectangle1.Dispose();
                ho_RegionOpening.Dispose();
                ho_Rectangle.Dispose();
                ho_RectangleClip.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageResult.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Row3.Dispose();
                hv_Column3.Dispose();
                hv_Phi1.Dispose();
                hv_Length11.Dispose();
                hv_Length21.Dispose();
                hv_Value.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Phi.Dispose();
                hv_Length1.Dispose();
                hv_Length2.Dispose();
                hv_Number.Dispose();
                hv_Exception.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_Edges.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedLines.Dispose();
                ho_UnionContours1.Dispose();
                ho_UnionContours.Dispose();
                ho_SelectedXLD.Dispose();
                ho_MaxLengthContour.Dispose();
                ho_RegionCut.Dispose();
                ho_Rectangle1.Dispose();
                ho_RegionOpening.Dispose();
                ho_Rectangle.Dispose();
                ho_RectangleClip.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_ImageReduced2.Dispose();
                ho_ImageResult.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Row3.Dispose();
                hv_Column3.Dispose();
                hv_Phi1.Dispose();
                hv_Length11.Dispose();
                hv_Length21.Dispose();
                hv_Value.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Phi.Dispose();
                hv_Length1.Dispose();
                hv_Length2.Dispose();
                hv_Number.Dispose();
                hv_Exception.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public void Mesa_jumper_flex_cosmetic(HObject ho_Image, HObject ho_ROI_0, out HObject ho_ShowRegion,
        out HTuple hv_Result)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced, ho_Region, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionFillUp, ho_RegionErosion;
            HObject ho_RegionTrans, ho_Rectangle, ho_ImageReduced1;
            HObject ho_ImageResult, ho_Region1, ho_RegionErosion1, ho_ConnectedRegions1;
            HObject ho_SelectedRegions1;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_Row11 = new HTuple(), hv_Column11 = new HTuple();
            HTuple hv_Row21 = new HTuple(), hv_Column21 = new HTuple();
            HTuple hv_Number = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            hv_Result = new HTuple();
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_0, out ho_ImageReduced);
            //Image Acquisition 01: Do something
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 235, 255);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                70);
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionCircle(ho_RegionFillUp, out ho_RegionErosion, 3.5);
            ho_RegionTrans.Dispose();
            HOperatorSet.ShapeTrans(ho_RegionErosion, out ho_RegionTrans, "convex");

            ho_ShowRegion.Dispose();
            ho_ShowRegion = new HObject(ho_RegionTrans);
            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
            HOperatorSet.SmallestRectangle1(ho_RegionTrans, out hv_Row1, out hv_Column1,
                out hv_Row2, out hv_Column2);
            hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose();
            HOperatorSet.SmallestRectangle1(ho_ROI_0, out hv_Row11, out hv_Column11, out hv_Row21, out hv_Column21);

            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1 + 20, hv_Column11, hv_Row2 - 20, hv_Column21);
            ho_ImageReduced1.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageReduced, ho_Rectangle, out ho_ImageReduced1);
            ho_ImageResult.Dispose();
            HOperatorSet.MultImage(ho_ImageReduced1, ho_ImageReduced1, out ho_ImageResult,
                0.003, 0);
            ho_Region1.Dispose();
            HOperatorSet.Threshold(ho_ImageResult, out ho_Region1, 0, 80);
            ho_RegionErosion1.Dispose();
            HOperatorSet.ErosionCircle(ho_Region1, out ho_RegionErosion1, 5.5);
            ho_ConnectedRegions1.Dispose();
            HOperatorSet.Connection(ho_RegionErosion1, out ho_ConnectedRegions1);
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, "area",
                "and", 150, 99999);
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number);
            if ((int)(new HTuple(hv_Number.TupleGreater(0))) != 0)
            {
                hv_Result.Dispose();
                hv_Result = 1;
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ShowRegion, ho_SelectedRegions1, out ExpTmpOutVar_0
                        );
                    ho_ShowRegion.Dispose();
                    ho_ShowRegion = ExpTmpOutVar_0;
                }

                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionErosion.Dispose();
                ho_RegionTrans.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced1.Dispose();
                ho_ImageResult.Dispose();
                ho_Region1.Dispose();
                ho_RegionErosion1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();

                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Row11.Dispose();
                hv_Column11.Dispose();
                hv_Row21.Dispose();
                hv_Column21.Dispose();
                hv_Number.Dispose();

                return;
            }
            hv_Result.Dispose();
            hv_Result = 0;
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionErosion.Dispose();
            ho_RegionTrans.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced1.Dispose();
            ho_ImageResult.Dispose();
            ho_Region1.Dispose();
            ho_RegionErosion1.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_SelectedRegions1.Dispose();

            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_Number.Dispose();

            return;
        }



        public void Power_switch(HObject ho_Image, HObject ho_ROI_0, out HObject ho_ShowRegion,
    out HTuple hv_Distance)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Regions, ho_Line, ho_ImageResult;
            HObject ho_Line1, ho_ImageReduced, ho_Region, ho_RegionErosion;
            HObject ho_Cross, ho_Cross1, ho_ObjectsConcat;

            // Local control variables 

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_CornerY = new HTuple();
            HTuple hv_CornerX = new HTuple(), hv_LineCenterY = new HTuple();
            HTuple hv_LineCenterX = new HTuple(), hv_ResultRow = new HTuple();
            HTuple hv_ResultColumn = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Value = new HTuple();
            HTuple hv_ResultRow1 = new HTuple(), hv_ResultColumn1 = new HTuple();
            HTuple hv_Row11 = new HTuple(), hv_Column11 = new HTuple();
            HTuple hv_Row21 = new HTuple(), hv_Column21 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Column3 = new HTuple(), hv_RowProj = new HTuple();
            HTuple hv_ColProj = new HTuple(), hv_RowProj1 = new HTuple();
            HTuple hv_ColProj1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ShowRegion);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_Line1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            hv_Distance = new HTuple();
            hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
            HOperatorSet.SmallestRectangle2(ho_ROI_0, out hv_Row, out hv_Column, out hv_Phi,
                out hv_Length1, out hv_Length2);
            hv_CornerY.Dispose(); hv_CornerX.Dispose(); hv_LineCenterY.Dispose(); hv_LineCenterX.Dispose();
            get_rectangle2_points(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2, out hv_CornerY,
                out hv_CornerX, out hv_LineCenterY, out hv_LineCenterX);
            //*基准边
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow.Dispose(); hv_ResultColumn.Dispose();
                rake(ho_Image, out ho_Regions, hv_Length1 / 2, hv_Length2 * 2, 15, 1, 20, "positive",
                    "max", hv_LineCenterY.TupleSelect(3), (hv_LineCenterX.TupleSelect(3)) - 400,
                    hv_LineCenterY.TupleSelect(3), (hv_LineCenterX.TupleSelect(3)) + 400, out hv_ResultRow,
                    out hv_ResultColumn);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Line.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                pts_to_best_line(out ho_Line, hv_ResultRow, hv_ResultColumn, hv_Length1 / 6, out hv_Row1,
                    out hv_Column1, out hv_Row2, out hv_Column2);
            }
            //*顶端边
            hv_Value.Dispose();
            HOperatorSet.GrayFeatures(ho_ROI_0, ho_Image, "mean", out hv_Value);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_ImageResult.Dispose();
                HOperatorSet.MultImage(ho_Image, ho_Image, out ho_ImageResult, 255 / (hv_Value * hv_Value),
                    0);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Regions.Dispose(); hv_ResultRow1.Dispose(); hv_ResultColumn1.Dispose();
                rake(ho_ImageResult, out ho_Regions, hv_Length1 / 2, hv_Length2 * 2, 15, 1, 20, "negative",
                    "first", hv_LineCenterY.TupleSelect(3), (hv_LineCenterX.TupleSelect(3)) + 60,
                    hv_LineCenterY.TupleSelect(1), (hv_LineCenterX.TupleSelect(1)) - 60, out hv_ResultRow1,
                    out hv_ResultColumn1);
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Line1.Dispose(); hv_Row11.Dispose(); hv_Column11.Dispose(); hv_Row21.Dispose(); hv_Column21.Dispose();
                pts_to_best_line(out ho_Line1, hv_ResultRow1, hv_ResultColumn1, hv_Length1 / 6,
                    out hv_Row11, out hv_Column11, out hv_Row21, out hv_Column21);
            }

            //*计算按钮
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_ImageResult, ho_ROI_0, out ho_ImageReduced);
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, 90);
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionRectangle1(ho_Region, out ho_RegionErosion, 1, 7);
            hv_Area.Dispose(); hv_Row3.Dispose(); hv_Column3.Dispose();
            HOperatorSet.AreaCenter(ho_RegionErosion, out hv_Area, out hv_Row3, out hv_Column3);
            //*顶端点
            hv_RowProj.Dispose(); hv_ColProj.Dispose();
            HOperatorSet.ProjectionPl(hv_Row3, hv_Column3, hv_Row11, hv_Column11, hv_Row21,
                hv_Column21, out hv_RowProj, out hv_ColProj);
            //*底面点
            hv_RowProj1.Dispose(); hv_ColProj1.Dispose();
            HOperatorSet.ProjectionPl(hv_Row3, hv_Column3, hv_Row1, hv_Column1, hv_Row2,
                hv_Column2, out hv_RowProj1, out hv_ColProj1);
            hv_Distance.Dispose();
            HOperatorSet.DistancePp(hv_RowProj, hv_ColProj, hv_RowProj1, hv_ColProj1, out hv_Distance);

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RowProj, hv_ColProj, 26, (new HTuple(45)).TupleRad()
                    );
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_Cross1.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_RowProj1, hv_ColProj1, 26,
                    (new HTuple(45)).TupleRad());
            }

            ho_ObjectsConcat.Dispose();
            HOperatorSet.ConcatObj(ho_Line, ho_Line1, out ho_ObjectsConcat);
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross, out ExpTmpOutVar_0);
                ho_ObjectsConcat.Dispose();
                ho_ObjectsConcat = ExpTmpOutVar_0;
            }
            ho_ShowRegion.Dispose();
            HOperatorSet.ConcatObj(ho_ObjectsConcat, ho_Cross1, out ho_ShowRegion);
            ho_Regions.Dispose();
            ho_Line.Dispose();
            ho_ImageResult.Dispose();
            ho_Line1.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_RegionErosion.Dispose();
            ho_Cross.Dispose();
            ho_Cross1.Dispose();
            ho_ObjectsConcat.Dispose();

            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Phi.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
            hv_CornerY.Dispose();
            hv_CornerX.Dispose();
            hv_LineCenterY.Dispose();
            hv_LineCenterX.Dispose();
            hv_ResultRow.Dispose();
            hv_ResultColumn.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Value.Dispose();
            hv_ResultRow1.Dispose();
            hv_ResultColumn1.Dispose();
            hv_Row11.Dispose();
            hv_Column11.Dispose();
            hv_Row21.Dispose();
            hv_Column21.Dispose();
            hv_Area.Dispose();
            hv_Row3.Dispose();
            hv_Column3.Dispose();
            hv_RowProj.Dispose();
            hv_ColProj.Dispose();
            hv_RowProj1.Dispose();
            hv_ColProj1.Dispose();

            return;
        }

        //public void Power_switch(HObject ho_Image, HObject ho_ROI_0, HObject ho_ROI_1,
        //    out HObject ho_showRegions, out HObject ho_ImageOut, out HTuple hv_dis)
        //{



        //    // Stack for temporary objects 
        //    HObject[] OTemp = new HObject[20];

        //    // Local iconic variables 

        //    HObject ho_ImageReduced = null, ho_Region = null;
        //    HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
        //    HObject ho_RegionClosing = null, ho_RegionOpening1 = null, ho_RegionMoved = null;
        //    HObject ho_LineKeyEdgRegion = null, ho_LineKeyEdg = null, ho_Region2 = null;
        //    HObject ho_ConnectedRegions1 = null, ho_SelectedRegions1 = null;
        //    HObject ho_RegionUnion1 = null, ho_RegionTrans = null, ho_LineTop = null;
        //    HObject ho_ConnectedRegions2 = null, ho_Skeleton = null, ho_Contours = null;
        //    HObject ho_ContourButton = null, ho_BtnTopCross = null, ho_LineKeyCross = null;
        //    HObject ho_RegionUnion = null;

        //    // Local control variables 

        //    HTuple hv_Length = new HTuple(), hv_LengthMax = new HTuple();
        //    HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
        //    HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
        //    HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
        //    HTuple hv_ROI_1Row1 = new HTuple(), hv_ROI_1Column1 = new HTuple();
        //    HTuple hv_ROI_1Row2 = new HTuple(), hv_ROI_1Column2 = new HTuple();
        //    HTuple hv_Number = new HTuple(), hv_Row1 = new HTuple();
        //    HTuple hv_Row2 = new HTuple(), hv_topCenterR = new HTuple();
        //    HTuple hv_topCenterC = new HTuple(), hv_Rows = new HTuple();
        //    HTuple hv_LineKeyEdgColumns = new HTuple(), hv_Indices = new HTuple();
        //    HTuple hv_findLineKeyRow = new HTuple(), hv_findLineKeyCol = new HTuple();
        //    HTuple hv_Channels = new HTuple(), hv_Pointer = new HTuple();
        //    HTuple hv_Type = new HTuple(), hv_Width = new HTuple();
        //    HTuple hv_Height = new HTuple(), hv_Exception = new HTuple();
        //    // Initialize local and output iconic variables 
        //    HOperatorSet.GenEmptyObj(out ho_showRegions);
        //    HOperatorSet.GenEmptyObj(out ho_ImageOut);
        //    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
        //    HOperatorSet.GenEmptyObj(out ho_Region);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
        //    HOperatorSet.GenEmptyObj(out ho_RegionClosing);
        //    HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
        //    HOperatorSet.GenEmptyObj(out ho_RegionMoved);
        //    HOperatorSet.GenEmptyObj(out ho_LineKeyEdgRegion);
        //    HOperatorSet.GenEmptyObj(out ho_LineKeyEdg);
        //    HOperatorSet.GenEmptyObj(out ho_Region2);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
        //    HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
        //    HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
        //    HOperatorSet.GenEmptyObj(out ho_RegionTrans);
        //    HOperatorSet.GenEmptyObj(out ho_LineTop);
        //    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
        //    HOperatorSet.GenEmptyObj(out ho_Skeleton);
        //    HOperatorSet.GenEmptyObj(out ho_Contours);
        //    HOperatorSet.GenEmptyObj(out ho_ContourButton);
        //    HOperatorSet.GenEmptyObj(out ho_BtnTopCross);
        //    HOperatorSet.GenEmptyObj(out ho_LineKeyCross);
        //    HOperatorSet.GenEmptyObj(out ho_RegionUnion);
        //    hv_dis = new HTuple();
        //    try
        //    {

        //        try
        //        {

        //            ho_ImageReduced.Dispose();
        //            HOperatorSet.ReduceDomain(ho_Image, ho_ROI_1, out ho_ImageReduced);
        //            ho_Region.Dispose();
        //            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 220, 255);
        //            ho_ConnectedRegions.Dispose();
        //            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);


        //            ho_SelectedRegions.Dispose();
        //            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions,
        //                "max_area", 70);
        //            ho_RegionClosing.Dispose();
        //            HOperatorSet.ClosingRectangle1(ho_SelectedRegions, out ho_RegionClosing,
        //                100, 100);
        //            ho_RegionOpening1.Dispose();
        //            HOperatorSet.OpeningRectangle1(ho_RegionClosing, out ho_RegionOpening1, 100,
        //                2);
        //            //
        //            ho_RegionMoved.Dispose();
        //            HOperatorSet.MoveRegion(ho_RegionOpening1, out ho_RegionMoved, 1, 0);
        //            ho_LineKeyEdgRegion.Dispose();
        //            HOperatorSet.Difference(ho_RegionMoved, ho_RegionOpening1, out ho_LineKeyEdgRegion
        //                );
        //            ho_LineKeyEdg.Dispose();
        //            HOperatorSet.GenContourRegionXld(ho_LineKeyEdgRegion, out ho_LineKeyEdg,
        //                "center");
        //            hv_Length.Dispose();
        //            HOperatorSet.LengthXld(ho_LineKeyEdg, out hv_Length);
        //            hv_LengthMax.Dispose();
        //            HOperatorSet.TupleMax(hv_Length, out hv_LengthMax);
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.SelectShapeXld(ho_LineKeyEdg, out ExpTmpOutVar_0, "contlength",
        //                    "and", hv_LengthMax - 1, 99000);
        //                ho_LineKeyEdg.Dispose();
        //                ho_LineKeyEdg = ExpTmpOutVar_0;
        //            }


        //            hv_RowBegin.Dispose(); hv_ColBegin.Dispose(); hv_RowEnd.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
        //            HOperatorSet.FitLineContourXld(ho_LineKeyEdg, "tukey", -1, 0, 5, 2, out hv_RowBegin,
        //                out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
        //                out hv_Dist);

        //            ho_Region2.Dispose();
        //            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region2, 0, 50);
        //            ho_ConnectedRegions1.Dispose();
        //            HOperatorSet.Connection(ho_Region2, out ho_ConnectedRegions1);

        //            hv_ROI_1Row1.Dispose(); hv_ROI_1Column1.Dispose(); hv_ROI_1Row2.Dispose(); hv_ROI_1Column2.Dispose();
        //            HOperatorSet.SmallestRectangle1(ho_ROI_1, out hv_ROI_1Row1, out hv_ROI_1Column1,
        //                out hv_ROI_1Row2, out hv_ROI_1Column2);
        //            //select_shape (ConnectedRegions1, SelectedRegions1, ['row2','area'], 'and', [RowBegin,20], [RowBegin+80,90000])
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                ho_SelectedRegions1.Dispose();
        //                HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, (new HTuple("column1")).TupleConcat(
        //                    "column2"), "and", ((hv_ROI_1Column1 + 50)).TupleConcat(hv_ROI_1Column1 + 100),
        //                    hv_ROI_1Column2.TupleConcat(hv_ROI_1Column2));
        //            }

        //            ho_RegionUnion1.Dispose();
        //            HOperatorSet.Union1(ho_SelectedRegions1, out ho_RegionUnion1);
        //            ho_RegionTrans.Dispose();
        //            HOperatorSet.ShapeTrans(ho_RegionUnion1, out ho_RegionTrans, "convex");

        //            ho_RegionMoved.Dispose();
        //            HOperatorSet.MoveRegion(ho_RegionTrans, out ho_RegionMoved, -1, 0);
        //            ho_LineTop.Dispose();
        //            HOperatorSet.Difference(ho_RegionTrans, ho_RegionMoved, out ho_LineTop);
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ClipRegionRel(ho_LineTop, out ExpTmpOutVar_0, 0, 0, 10, 10);
        //                ho_LineTop.Dispose();
        //                ho_LineTop = ExpTmpOutVar_0;
        //            }
        //            ho_ConnectedRegions2.Dispose();
        //            HOperatorSet.Connection(ho_LineTop, out ho_ConnectedRegions2);
        //            ho_LineTop.Dispose();
        //            HOperatorSet.SelectShapeStd(ho_ConnectedRegions2, out ho_LineTop, "max_area",
        //                70);
        //            ho_Skeleton.Dispose();
        //            HOperatorSet.Skeleton(ho_LineTop, out ho_Skeleton);
        //            ho_Contours.Dispose();
        //            HOperatorSet.GenContoursSkeletonXld(ho_Skeleton, out ho_Contours, 1, "filter");
        //            hv_Number.Dispose();
        //            HOperatorSet.CountObj(ho_Contours, out hv_Number);

        //            hv_Row1.Dispose(); hv_ColBegin.Dispose(); hv_Row2.Dispose(); hv_ColEnd.Dispose(); hv_Nr.Dispose(); hv_Nc.Dispose(); hv_Dist.Dispose();
        //            HOperatorSet.FitLineContourXld(ho_Contours, "tukey", -1, 5, 5, 2, out hv_Row1,
        //                out hv_ColBegin, out hv_Row2, out hv_ColEnd, out hv_Nr, out hv_Nc, out hv_Dist);

        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                ho_ContourButton.Dispose();
        //                HOperatorSet.GenContourPolygonXld(out ho_ContourButton, hv_Row1.TupleConcat(
        //                    hv_Row2), hv_ColBegin.TupleConcat(hv_ColEnd));
        //            }

        //            hv_topCenterR.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_topCenterR = 0.5 * (hv_Row1 + hv_Row2);
        //            }
        //            hv_topCenterC.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_topCenterC = 0.5 * (hv_ColBegin + hv_ColEnd);
        //            }
        //            ho_BtnTopCross.Dispose();
        //            HOperatorSet.GenCrossContourXld(out ho_BtnTopCross, hv_topCenterR, hv_topCenterC,
        //                16, 0);

        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.GenRegionContourXld(ho_BtnTopCross, out ExpTmpOutVar_0, "margin");
        //                ho_BtnTopCross.Dispose();
        //                ho_BtnTopCross = ExpTmpOutVar_0;
        //            }

        //            {
        //                HTuple ExpTmpOutVar_0;
        //                HOperatorSet.TupleInt(hv_topCenterC, out ExpTmpOutVar_0);
        //                hv_topCenterC.Dispose();
        //                hv_topCenterC = ExpTmpOutVar_0;
        //            }


        //            hv_Rows.Dispose(); hv_LineKeyEdgColumns.Dispose();
        //            HOperatorSet.GetRegionPoints(ho_LineKeyEdgRegion, out hv_Rows, out hv_LineKeyEdgColumns);
        //            //在数组1中寻找数组2，如果有返回第一个元素对应的下标，否则返回-1
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_Indices.Dispose();
        //                HOperatorSet.TupleFind(hv_LineKeyEdgColumns, hv_topCenterC - 100, out hv_Indices);
        //            }
        //            //键盘边缘点位 （findLineKeyRow，findLineKeyCol）
        //            hv_findLineKeyRow.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_findLineKeyRow = hv_Rows.TupleSelect(
        //                    hv_Indices);
        //            }
        //            hv_findLineKeyCol.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_findLineKeyCol = hv_topCenterC - 100;
        //            }
        //            ho_LineKeyCross.Dispose();
        //            HOperatorSet.GenCrossContourXld(out ho_LineKeyCross, hv_findLineKeyRow, hv_findLineKeyCol,
        //                16, 45);
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.GenRegionContourXld(ho_LineKeyCross, out ExpTmpOutVar_0, "margin");
        //                ho_LineKeyCross.Dispose();
        //                ho_LineKeyCross = ExpTmpOutVar_0;
        //            }
        //            hv_dis.Dispose();
        //            using (HDevDisposeHelper dh = new HDevDisposeHelper())
        //            {
        //                hv_dis = hv_topCenterR - hv_findLineKeyRow;
        //            }

        //            ho_showRegions.Dispose();
        //            HOperatorSet.GenEmptyRegion(out ho_showRegions);
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegions, ho_LineKeyEdgRegion, out ExpTmpOutVar_0
        //                    );
        //                ho_showRegions.Dispose();
        //                ho_showRegions = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegions, ho_LineTop, out ExpTmpOutVar_0);
        //                ho_showRegions.Dispose();
        //                ho_showRegions = ExpTmpOutVar_0;
        //            }

        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegions, ho_LineKeyCross, out ExpTmpOutVar_0
        //                    );
        //                ho_showRegions.Dispose();
        //                ho_showRegions = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegions, ho_BtnTopCross, out ExpTmpOutVar_0
        //                    );
        //                ho_showRegions.Dispose();
        //                ho_showRegions = ExpTmpOutVar_0;
        //            }
        //            ho_RegionUnion.Dispose();
        //            HOperatorSet.Union1(ho_showRegions, out ho_RegionUnion);

        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.ConcatObj(ho_showRegions, ho_ROI_1, out ExpTmpOutVar_0);
        //                ho_showRegions.Dispose();
        //                ho_showRegions = ExpTmpOutVar_0;
        //            }



        //            //union1 (showRegions, showRegions)

        //            //将Region paint到图像上显示
        //            hv_Channels.Dispose();
        //            HOperatorSet.CountChannels(ho_Image, out hv_Channels);
        //            if ((int)(new HTuple(hv_Channels.TupleEqual(1))) != 0)
        //            {
        //                hv_Pointer.Dispose(); hv_Type.Dispose(); hv_Width.Dispose(); hv_Height.Dispose();
        //                HOperatorSet.GetImagePointer1(ho_Image, out hv_Pointer, out hv_Type, out hv_Width,
        //                    out hv_Height);
        //                ho_ImageOut.Dispose();
        //                HOperatorSet.GenImage3(out ho_ImageOut, "byte", hv_Width, hv_Height, hv_Pointer,
        //                    hv_Pointer, hv_Pointer);
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.Boundary(ho_showRegions, out ExpTmpOutVar_0, "inner");
        //                ho_showRegions.Dispose();
        //                ho_showRegions = ExpTmpOutVar_0;
        //            }
        //            {
        //                HObject ExpTmpOutVar_0;
        //                HOperatorSet.DilationCircle(ho_showRegions, out ExpTmpOutVar_0, 2);
        //                ho_showRegions.Dispose();
        //                ho_showRegions = ExpTmpOutVar_0;
        //            }
        //            HOperatorSet.OverpaintRegion(ho_ImageOut, ho_showRegions, ((new HTuple(0)).TupleConcat(
        //                255)).TupleConcat(0), "fill");


        //        }
        //        // catch (Exception) 
        //        catch (HalconException HDevExpDefaultException1)
        //        {
        //            HDevExpDefaultException1.ToHTuple(out hv_Exception);

        //        }
        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_RegionClosing.Dispose();
        //        ho_RegionOpening1.Dispose();
        //        ho_RegionMoved.Dispose();
        //        ho_LineKeyEdgRegion.Dispose();
        //        ho_LineKeyEdg.Dispose();
        //        ho_Region2.Dispose();
        //        ho_ConnectedRegions1.Dispose();
        //        ho_SelectedRegions1.Dispose();
        //        ho_RegionUnion1.Dispose();
        //        ho_RegionTrans.Dispose();
        //        ho_LineTop.Dispose();
        //        ho_ConnectedRegions2.Dispose();
        //        ho_Skeleton.Dispose();
        //        ho_Contours.Dispose();
        //        ho_ContourButton.Dispose();
        //        ho_BtnTopCross.Dispose();
        //        ho_LineKeyCross.Dispose();
        //        ho_RegionUnion.Dispose();

        //        hv_Length.Dispose();
        //        hv_LengthMax.Dispose();
        //        hv_RowBegin.Dispose();
        //        hv_ColBegin.Dispose();
        //        hv_RowEnd.Dispose();
        //        hv_ColEnd.Dispose();
        //        hv_Nr.Dispose();
        //        hv_Nc.Dispose();
        //        hv_Dist.Dispose();
        //        hv_ROI_1Row1.Dispose();
        //        hv_ROI_1Column1.Dispose();
        //        hv_ROI_1Row2.Dispose();
        //        hv_ROI_1Column2.Dispose();
        //        hv_Number.Dispose();
        //        hv_Row1.Dispose();
        //        hv_Row2.Dispose();
        //        hv_topCenterR.Dispose();
        //        hv_topCenterC.Dispose();
        //        hv_Rows.Dispose();
        //        hv_LineKeyEdgColumns.Dispose();
        //        hv_Indices.Dispose();
        //        hv_findLineKeyRow.Dispose();
        //        hv_findLineKeyCol.Dispose();
        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        return;
        //    }
        //    catch (HalconException HDevExpDefaultException)
        //    {
        //        ho_ImageReduced.Dispose();
        //        ho_Region.Dispose();
        //        ho_ConnectedRegions.Dispose();
        //        ho_SelectedRegions.Dispose();
        //        ho_RegionClosing.Dispose();
        //        ho_RegionOpening1.Dispose();
        //        ho_RegionMoved.Dispose();
        //        ho_LineKeyEdgRegion.Dispose();
        //        ho_LineKeyEdg.Dispose();
        //        ho_Region2.Dispose();
        //        ho_ConnectedRegions1.Dispose();
        //        ho_SelectedRegions1.Dispose();
        //        ho_RegionUnion1.Dispose();
        //        ho_RegionTrans.Dispose();
        //        ho_LineTop.Dispose();
        //        ho_ConnectedRegions2.Dispose();
        //        ho_Skeleton.Dispose();
        //        ho_Contours.Dispose();
        //        ho_ContourButton.Dispose();
        //        ho_BtnTopCross.Dispose();
        //        ho_LineKeyCross.Dispose();
        //        ho_RegionUnion.Dispose();

        //        hv_Length.Dispose();
        //        hv_LengthMax.Dispose();
        //        hv_RowBegin.Dispose();
        //        hv_ColBegin.Dispose();
        //        hv_RowEnd.Dispose();
        //        hv_ColEnd.Dispose();
        //        hv_Nr.Dispose();
        //        hv_Nc.Dispose();
        //        hv_Dist.Dispose();
        //        hv_ROI_1Row1.Dispose();
        //        hv_ROI_1Column1.Dispose();
        //        hv_ROI_1Row2.Dispose();
        //        hv_ROI_1Column2.Dispose();
        //        hv_Number.Dispose();
        //        hv_Row1.Dispose();
        //        hv_Row2.Dispose();
        //        hv_topCenterR.Dispose();
        //        hv_topCenterC.Dispose();
        //        hv_Rows.Dispose();
        //        hv_LineKeyEdgColumns.Dispose();
        //        hv_Indices.Dispose();
        //        hv_findLineKeyRow.Dispose();
        //        hv_findLineKeyCol.Dispose();
        //        hv_Channels.Dispose();
        //        hv_Pointer.Dispose();
        //        hv_Type.Dispose();
        //        hv_Width.Dispose();
        //        hv_Height.Dispose();
        //        hv_Exception.Dispose();

        //        throw HDevExpDefaultException;
        //    }
        //}


        #region 深度学习



        // Chapter: Deep Learning / Model
        // Short Description: Store the given images in a tuple of dictionaries DLSamples. 
        public void gen_dl_samples_from_images(HObject ho_Images, out HTuple hv_DLSampleBatch)
        {



            // Local iconic variables 

            HObject ho_Image = null;

            // Local control variables 

            HTuple hv_NumImages = new HTuple(), hv_ImageIndex = new HTuple();
            HTuple hv_DLSample = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            hv_DLSampleBatch = new HTuple();
            try
            {
                //
                //This procedure creates DLSampleBatch, a tuple
                //containing a dictionary DLSample
                //for every image given in Images.
                //
                //Initialize output tuple.
                hv_NumImages.Dispose();
                HOperatorSet.CountObj(ho_Images, out hv_NumImages);
                hv_DLSampleBatch.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_DLSampleBatch = HTuple.TupleGenConst(
                        hv_NumImages, -1);
                }
                //
                //Loop through all given images.
                HTuple end_val10 = hv_NumImages - 1;
                HTuple step_val10 = 1;
                for (hv_ImageIndex = 0; hv_ImageIndex.Continue(end_val10, step_val10); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val10))
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Image.Dispose();
                        HOperatorSet.SelectObj(ho_Images, out ho_Image, hv_ImageIndex + 1);
                    }
                    //Create DLSample from image.
                    hv_DLSample.Dispose();
                    HOperatorSet.CreateDict(out hv_DLSample);
                    HOperatorSet.SetDictObject(ho_Image, hv_DLSample, "image");
                    //
                    //Collect the DLSamples.
                    if (hv_DLSampleBatch == null)
                        hv_DLSampleBatch = new HTuple();
                    hv_DLSampleBatch[hv_ImageIndex] = hv_DLSample;
                }
                ho_Image.Dispose();

                hv_NumImages.Dispose();
                hv_ImageIndex.Dispose();
                hv_DLSample.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();

                hv_NumImages.Dispose();
                hv_ImageIndex.Dispose();
                hv_DLSample.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Model
        // Short Description: Preprocess given DLSamples according to the preprocessing parameters given in DLPreprocessParam. 
        public void preprocess_dl_samples(HTuple hv_DLSampleBatch, HTuple hv_DLPreprocessParam)
        {



            // Local iconic variables 

            HObject ho_ImageRaw = null, ho_ImagePreprocessed = null;
            HObject ho_AnomalyImageRaw = null, ho_AnomalyImagePreprocessed = null;
            HObject ho_SegmentationRaw = null, ho_SegmentationPreprocessed = null;

            // Local control variables 

            HTuple hv_ModelType = new HTuple(), hv_SampleIndex = new HTuple();
            HTuple hv_ImageExists = new HTuple(), hv_KeysExists = new HTuple();
            HTuple hv_AnomalyParamExist = new HTuple(), hv_Rectangle1ParamExist = new HTuple();
            HTuple hv_Rectangle2ParamExist = new HTuple(), hv_SegmentationParamExist = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageRaw);
            HOperatorSet.GenEmptyObj(out ho_ImagePreprocessed);
            HOperatorSet.GenEmptyObj(out ho_AnomalyImageRaw);
            HOperatorSet.GenEmptyObj(out ho_AnomalyImagePreprocessed);
            HOperatorSet.GenEmptyObj(out ho_SegmentationRaw);
            HOperatorSet.GenEmptyObj(out ho_SegmentationPreprocessed);
            try
            {
                //
                //This procedure preprocesses all images of the sample dictionaries in the tuple DLSampleBatch.
                //The images are preprocessed according to the parameters provided in DLPreprocessParam.
                //
                //Check the validity of the preprocessing parameters.
                check_dl_preprocess_param(hv_DLPreprocessParam);
                hv_ModelType.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "model_type", out hv_ModelType);
                //
                //Preprocess the sample entries.
                //
                for (hv_SampleIndex = 0; (int)hv_SampleIndex <= (int)((new HTuple(hv_DLSampleBatch.TupleLength()
                    )) - 1); hv_SampleIndex = (int)hv_SampleIndex + 1)
                {
                    //
                    //Check the existence of the sample keys.
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ImageExists.Dispose();
                        HOperatorSet.GetDictParam(hv_DLSampleBatch.TupleSelect(hv_SampleIndex), "key_exists",
                            "image", out hv_ImageExists);
                    }
                    //
                    //Preprocess the images.
                    if ((int)(hv_ImageExists) != 0)
                    {
                        //
                        //Get the image.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_ImageRaw.Dispose();
                            HOperatorSet.GetDictObject(out ho_ImageRaw, hv_DLSampleBatch.TupleSelect(
                                hv_SampleIndex), "image");
                        }
                        //
                        //Preprocess the image.
                        ho_ImagePreprocessed.Dispose();
                        preprocess_dl_model_images(ho_ImageRaw, out ho_ImagePreprocessed, hv_DLPreprocessParam);
                        //
                        //Replace the image in the dictionary.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HOperatorSet.SetDictObject(ho_ImagePreprocessed, hv_DLSampleBatch.TupleSelect(
                                hv_SampleIndex), "image");
                        }
                        //
                        //Check existence of model specific sample keys:
                        //- bbox_row1 for 'rectangle1'
                        //- bbox_phi for 'rectangle2'
                        //- segmentation_image for 'semantic segmentation'
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_KeysExists.Dispose();
                            HOperatorSet.GetDictParam(hv_DLSampleBatch.TupleSelect(hv_SampleIndex),
                                "key_exists", (((new HTuple("anomaly_ground_truth")).TupleConcat("bbox_row1")).TupleConcat(
                                "bbox_phi")).TupleConcat("segmentation_image"), out hv_KeysExists);
                        }
                        hv_AnomalyParamExist.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_AnomalyParamExist = hv_KeysExists.TupleSelect(
                                0);
                        }
                        hv_Rectangle1ParamExist.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Rectangle1ParamExist = hv_KeysExists.TupleSelect(
                                1);
                        }
                        hv_Rectangle2ParamExist.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Rectangle2ParamExist = hv_KeysExists.TupleSelect(
                                2);
                        }
                        hv_SegmentationParamExist.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_SegmentationParamExist = hv_KeysExists.TupleSelect(
                                3);
                        }
                        //
                        //Preprocess the anomaly ground truth if present.
                        if ((int)(hv_AnomalyParamExist) != 0)
                        {
                            //
                            //Get the anomaly image.
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                ho_AnomalyImageRaw.Dispose();
                                HOperatorSet.GetDictObject(out ho_AnomalyImageRaw, hv_DLSampleBatch.TupleSelect(
                                    hv_SampleIndex), "anomaly_ground_truth");
                            }
                            //
                            //Preprocess the anomaly image.
                            ho_AnomalyImagePreprocessed.Dispose();
                            preprocess_dl_model_anomaly(ho_AnomalyImageRaw, out ho_AnomalyImagePreprocessed,
                                hv_DLPreprocessParam);
                            //
                            //Set preprocessed anomaly image.
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                HOperatorSet.SetDictObject(ho_AnomalyImagePreprocessed, hv_DLSampleBatch.TupleSelect(
                                    hv_SampleIndex), "anomaly_ground_truth");
                            }
                        }
                        //
                        //Preprocess depending on the model type.
                        //If bounding boxes are given, rescale them as well.
                        if ((int)(hv_Rectangle1ParamExist) != 0)
                        {
                            //
                            //Preprocess the bounding boxes of type 'rectangle1'.
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                preprocess_dl_model_bbox_rect1(ho_ImageRaw, hv_DLSampleBatch.TupleSelect(
                                    hv_SampleIndex), hv_DLPreprocessParam);
                            }
                        }
                        else if ((int)(hv_Rectangle2ParamExist) != 0)
                        {
                            //
                            //Preprocess the bounding boxes of type 'rectangle2'.
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                preprocess_dl_model_bbox_rect2(ho_ImageRaw, hv_DLSampleBatch.TupleSelect(
                                    hv_SampleIndex), hv_DLPreprocessParam);
                            }
                        }
                        //
                        //Preprocess the segmentation image if present.
                        if ((int)(hv_SegmentationParamExist) != 0)
                        {
                            //
                            //Get the segmentation image.
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                ho_SegmentationRaw.Dispose();
                                HOperatorSet.GetDictObject(out ho_SegmentationRaw, hv_DLSampleBatch.TupleSelect(
                                    hv_SampleIndex), "segmentation_image");
                            }
                            //
                            //Preprocess the segmentation image.
                            ho_SegmentationPreprocessed.Dispose();
                            preprocess_dl_model_segmentations(ho_ImageRaw, ho_SegmentationRaw, out ho_SegmentationPreprocessed,
                                hv_DLPreprocessParam);
                            //
                            //Set preprocessed segmentation image.
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                HOperatorSet.SetDictObject(ho_SegmentationPreprocessed, hv_DLSampleBatch.TupleSelect(
                                    hv_SampleIndex), "segmentation_image");
                            }
                        }
                    }
                    else
                    {
                        throw new HalconException((new HTuple("All samples processed need to include an image, but the sample with index ") + hv_SampleIndex) + " does not.");
                    }
                }
                //
                ho_ImageRaw.Dispose();
                ho_ImagePreprocessed.Dispose();
                ho_AnomalyImageRaw.Dispose();
                ho_AnomalyImagePreprocessed.Dispose();
                ho_SegmentationRaw.Dispose();
                ho_SegmentationPreprocessed.Dispose();

                hv_ModelType.Dispose();
                hv_SampleIndex.Dispose();
                hv_ImageExists.Dispose();
                hv_KeysExists.Dispose();
                hv_AnomalyParamExist.Dispose();
                hv_Rectangle1ParamExist.Dispose();
                hv_Rectangle2ParamExist.Dispose();
                hv_SegmentationParamExist.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageRaw.Dispose();
                ho_ImagePreprocessed.Dispose();
                ho_AnomalyImageRaw.Dispose();
                ho_AnomalyImagePreprocessed.Dispose();
                ho_SegmentationRaw.Dispose();
                ho_SegmentationPreprocessed.Dispose();

                hv_ModelType.Dispose();
                hv_SampleIndex.Dispose();
                hv_ImageExists.Dispose();
                hv_KeysExists.Dispose();
                hv_AnomalyParamExist.Dispose();
                hv_Rectangle1ParamExist.Dispose();
                hv_Rectangle2ParamExist.Dispose();
                hv_SegmentationParamExist.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Model
        // Short Description: Checks the content of the parameter dictionary DLPreprocessParam. 
        private void check_dl_preprocess_param(HTuple hv_DLPreprocessParam)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_CheckParams = new HTuple(), hv_KeyExists = new HTuple();
            HTuple hv_DLModelType = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SupportedModelTypes = new HTuple(), hv_Index = new HTuple();
            HTuple hv_ParamNamesGeneral = new HTuple(), hv_ParamNamesSegmentation = new HTuple();
            HTuple hv_ParamNamesDetectionOptional = new HTuple(), hv_ParamNamesPreprocessingOptional = new HTuple();
            HTuple hv_ParamNamesAll = new HTuple(), hv_ParamNames = new HTuple();
            HTuple hv_KeysExists = new HTuple(), hv_I = new HTuple();
            HTuple hv_Exists = new HTuple(), hv_InputKeys = new HTuple();
            HTuple hv_Key = new HTuple(), hv_Value = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_ValidValues = new HTuple();
            HTuple hv_ValidTypes = new HTuple(), hv_V = new HTuple();
            HTuple hv_T = new HTuple(), hv_IsInt = new HTuple(), hv_ValidTypesListing = new HTuple();
            HTuple hv_ValidValueListing = new HTuple(), hv_EmptyStrings = new HTuple();
            HTuple hv_ImageRangeMinExists = new HTuple(), hv_ImageRangeMaxExists = new HTuple();
            HTuple hv_ImageRangeMin = new HTuple(), hv_ImageRangeMax = new HTuple();
            HTuple hv_IndexParam = new HTuple(), hv_SetBackgroundID = new HTuple();
            HTuple hv_ClassIDsBackground = new HTuple(), hv_Intersection = new HTuple();
            HTuple hv_IgnoreClassIDs = new HTuple(), hv_KnownClasses = new HTuple();
            HTuple hv_IgnoreClassID = new HTuple(), hv_OptionalKeysExist = new HTuple();
            HTuple hv_InstanceType = new HTuple(), hv_IgnoreDirection = new HTuple();
            HTuple hv_ClassIDsNoOrientation = new HTuple(), hv_SemTypes = new HTuple();
            // Initialize local and output iconic variables 
            try
            {
                //
                //This procedure checks a dictionary with parameters for DL preprocessing.
                //
                hv_CheckParams.Dispose();
                hv_CheckParams = 1;
                //If check_params is set to false, do not check anything.
                hv_KeyExists.Dispose();
                HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", "check_params",
                    out hv_KeyExists);
                if ((int)(hv_KeyExists) != 0)
                {
                    hv_CheckParams.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "check_params", out hv_CheckParams);
                    if ((int)(hv_CheckParams.TupleNot()) != 0)
                    {

                        hv_CheckParams.Dispose();
                        hv_KeyExists.Dispose();
                        hv_DLModelType.Dispose();
                        hv_Exception.Dispose();
                        hv_SupportedModelTypes.Dispose();
                        hv_Index.Dispose();
                        hv_ParamNamesGeneral.Dispose();
                        hv_ParamNamesSegmentation.Dispose();
                        hv_ParamNamesDetectionOptional.Dispose();
                        hv_ParamNamesPreprocessingOptional.Dispose();
                        hv_ParamNamesAll.Dispose();
                        hv_ParamNames.Dispose();
                        hv_KeysExists.Dispose();
                        hv_I.Dispose();
                        hv_Exists.Dispose();
                        hv_InputKeys.Dispose();
                        hv_Key.Dispose();
                        hv_Value.Dispose();
                        hv_Indices.Dispose();
                        hv_ValidValues.Dispose();
                        hv_ValidTypes.Dispose();
                        hv_V.Dispose();
                        hv_T.Dispose();
                        hv_IsInt.Dispose();
                        hv_ValidTypesListing.Dispose();
                        hv_ValidValueListing.Dispose();
                        hv_EmptyStrings.Dispose();
                        hv_ImageRangeMinExists.Dispose();
                        hv_ImageRangeMaxExists.Dispose();
                        hv_ImageRangeMin.Dispose();
                        hv_ImageRangeMax.Dispose();
                        hv_IndexParam.Dispose();
                        hv_SetBackgroundID.Dispose();
                        hv_ClassIDsBackground.Dispose();
                        hv_Intersection.Dispose();
                        hv_IgnoreClassIDs.Dispose();
                        hv_KnownClasses.Dispose();
                        hv_IgnoreClassID.Dispose();
                        hv_OptionalKeysExist.Dispose();
                        hv_InstanceType.Dispose();
                        hv_IgnoreDirection.Dispose();
                        hv_ClassIDsNoOrientation.Dispose();
                        hv_SemTypes.Dispose();

                        return;
                    }
                }
                //
                try
                {
                    hv_DLModelType.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "model_type", out hv_DLModelType);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    throw new HalconException(new HTuple(new HTuple("DLPreprocessParam needs the parameter: '") + "model_type") + "'");
                }
                //
                //Check for correct model type.
                hv_SupportedModelTypes.Dispose();
                hv_SupportedModelTypes = new HTuple();
                hv_SupportedModelTypes[0] = "anomaly_detection";
                hv_SupportedModelTypes[1] = "classification";
                hv_SupportedModelTypes[2] = "detection";
                hv_SupportedModelTypes[3] = "segmentation";
                hv_Index.Dispose();
                HOperatorSet.TupleFind(hv_SupportedModelTypes, hv_DLModelType, out hv_Index);
                if ((int)((new HTuple(hv_Index.TupleEqual(-1))).TupleOr(new HTuple(hv_Index.TupleEqual(
                    new HTuple())))) != 0)
                {
                    throw new HalconException(new HTuple("Only models of type 'anomaly_detection', 'classification', 'detection', or 'segmentation' are supported"));

                    hv_CheckParams.Dispose();
                    hv_KeyExists.Dispose();
                    hv_DLModelType.Dispose();
                    hv_Exception.Dispose();
                    hv_SupportedModelTypes.Dispose();
                    hv_Index.Dispose();
                    hv_ParamNamesGeneral.Dispose();
                    hv_ParamNamesSegmentation.Dispose();
                    hv_ParamNamesDetectionOptional.Dispose();
                    hv_ParamNamesPreprocessingOptional.Dispose();
                    hv_ParamNamesAll.Dispose();
                    hv_ParamNames.Dispose();
                    hv_KeysExists.Dispose();
                    hv_I.Dispose();
                    hv_Exists.Dispose();
                    hv_InputKeys.Dispose();
                    hv_Key.Dispose();
                    hv_Value.Dispose();
                    hv_Indices.Dispose();
                    hv_ValidValues.Dispose();
                    hv_ValidTypes.Dispose();
                    hv_V.Dispose();
                    hv_T.Dispose();
                    hv_IsInt.Dispose();
                    hv_ValidTypesListing.Dispose();
                    hv_ValidValueListing.Dispose();
                    hv_EmptyStrings.Dispose();
                    hv_ImageRangeMinExists.Dispose();
                    hv_ImageRangeMaxExists.Dispose();
                    hv_ImageRangeMin.Dispose();
                    hv_ImageRangeMax.Dispose();
                    hv_IndexParam.Dispose();
                    hv_SetBackgroundID.Dispose();
                    hv_ClassIDsBackground.Dispose();
                    hv_Intersection.Dispose();
                    hv_IgnoreClassIDs.Dispose();
                    hv_KnownClasses.Dispose();
                    hv_IgnoreClassID.Dispose();
                    hv_OptionalKeysExist.Dispose();
                    hv_InstanceType.Dispose();
                    hv_IgnoreDirection.Dispose();
                    hv_ClassIDsNoOrientation.Dispose();
                    hv_SemTypes.Dispose();

                    return;
                }
                //
                //Parameter names that are required.
                //General parameters.
                hv_ParamNamesGeneral.Dispose();
                hv_ParamNamesGeneral = new HTuple();
                hv_ParamNamesGeneral[0] = "model_type";
                hv_ParamNamesGeneral[1] = "image_width";
                hv_ParamNamesGeneral[2] = "image_height";
                hv_ParamNamesGeneral[3] = "image_num_channels";
                hv_ParamNamesGeneral[4] = "image_range_min";
                hv_ParamNamesGeneral[5] = "image_range_max";
                hv_ParamNamesGeneral[6] = "normalization_type";
                hv_ParamNamesGeneral[7] = "domain_handling";
                //Segmentation specific parameters.
                hv_ParamNamesSegmentation.Dispose();
                hv_ParamNamesSegmentation = new HTuple();
                hv_ParamNamesSegmentation[0] = "ignore_class_ids";
                hv_ParamNamesSegmentation[1] = "set_background_id";
                hv_ParamNamesSegmentation[2] = "class_ids_background";
                //Detection specific parameters.
                hv_ParamNamesDetectionOptional.Dispose();
                hv_ParamNamesDetectionOptional = new HTuple();
                hv_ParamNamesDetectionOptional[0] = "instance_type";
                hv_ParamNamesDetectionOptional[1] = "ignore_direction";
                hv_ParamNamesDetectionOptional[2] = "class_ids_no_orientation";
                //Normalization specific parameters.
                hv_ParamNamesPreprocessingOptional.Dispose();
                hv_ParamNamesPreprocessingOptional = new HTuple();
                hv_ParamNamesPreprocessingOptional[0] = "mean_values_normalization";
                hv_ParamNamesPreprocessingOptional[1] = "deviation_values_normalization";
                //All parameters
                hv_ParamNamesAll.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamNamesAll = new HTuple();
                    hv_ParamNamesAll = hv_ParamNamesAll.TupleConcat(hv_ParamNamesGeneral, hv_ParamNamesSegmentation, hv_ParamNamesDetectionOptional, hv_ParamNamesPreprocessingOptional);
                }
                hv_ParamNames.Dispose();
                hv_ParamNames = new HTuple(hv_ParamNamesGeneral);
                if ((int)(new HTuple(hv_DLModelType.TupleEqual("segmentation"))) != 0)
                {
                    //Extend ParamNames for models of type segmentation.
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_ParamNames = hv_ParamNames.TupleConcat(
                                hv_ParamNamesSegmentation);
                            hv_ParamNames.Dispose();
                            hv_ParamNames = ExpTmpLocalVar_ParamNames;
                        }
                    }
                }
                //
                //Check if legacy parameter exist.
                //Otherwise map it to the legal parameter.
                replace_legacy_preprocessing_parameters(hv_DLPreprocessParam);
                //
                //Check that all necessary parameters are included.
                //
                hv_KeysExists.Dispose();
                HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", hv_ParamNames,
                    out hv_KeysExists);
                if ((int)(new HTuple(((((hv_KeysExists.TupleEqualElem(0))).TupleSum())).TupleGreater(
                    0))) != 0)
                {
                    for (hv_I = 0; (int)hv_I <= (int)(new HTuple(hv_KeysExists.TupleLength())); hv_I = (int)hv_I + 1)
                    {
                        hv_Exists.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Exists = hv_KeysExists.TupleSelect(
                                hv_I);
                        }
                        if ((int)(hv_Exists.TupleNot()) != 0)
                        {
                            throw new HalconException(("DLPreprocessParam needs the parameter: '" + (hv_ParamNames.TupleSelect(
                                hv_I))) + "'");
                        }
                    }
                }
                //
                //Check the keys provided.
                hv_InputKeys.Dispose();
                HOperatorSet.GetDictParam(hv_DLPreprocessParam, "keys", new HTuple(), out hv_InputKeys);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_InputKeys.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_Key.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Key = hv_InputKeys.TupleSelect(
                            hv_I);
                    }
                    hv_Value.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, hv_Key, out hv_Value);
                    //Check that the key is known.
                    hv_Indices.Dispose();
                    HOperatorSet.TupleFind(hv_ParamNamesAll, hv_Key, out hv_Indices);
                    if ((int)(new HTuple(hv_Indices.TupleEqual(-1))) != 0)
                    {
                        throw new HalconException(("Unknown key for DLPreprocessParam: '" + (hv_InputKeys.TupleSelect(
                            hv_I))) + "'");

                        hv_CheckParams.Dispose();
                        hv_KeyExists.Dispose();
                        hv_DLModelType.Dispose();
                        hv_Exception.Dispose();
                        hv_SupportedModelTypes.Dispose();
                        hv_Index.Dispose();
                        hv_ParamNamesGeneral.Dispose();
                        hv_ParamNamesSegmentation.Dispose();
                        hv_ParamNamesDetectionOptional.Dispose();
                        hv_ParamNamesPreprocessingOptional.Dispose();
                        hv_ParamNamesAll.Dispose();
                        hv_ParamNames.Dispose();
                        hv_KeysExists.Dispose();
                        hv_I.Dispose();
                        hv_Exists.Dispose();
                        hv_InputKeys.Dispose();
                        hv_Key.Dispose();
                        hv_Value.Dispose();
                        hv_Indices.Dispose();
                        hv_ValidValues.Dispose();
                        hv_ValidTypes.Dispose();
                        hv_V.Dispose();
                        hv_T.Dispose();
                        hv_IsInt.Dispose();
                        hv_ValidTypesListing.Dispose();
                        hv_ValidValueListing.Dispose();
                        hv_EmptyStrings.Dispose();
                        hv_ImageRangeMinExists.Dispose();
                        hv_ImageRangeMaxExists.Dispose();
                        hv_ImageRangeMin.Dispose();
                        hv_ImageRangeMax.Dispose();
                        hv_IndexParam.Dispose();
                        hv_SetBackgroundID.Dispose();
                        hv_ClassIDsBackground.Dispose();
                        hv_Intersection.Dispose();
                        hv_IgnoreClassIDs.Dispose();
                        hv_KnownClasses.Dispose();
                        hv_IgnoreClassID.Dispose();
                        hv_OptionalKeysExist.Dispose();
                        hv_InstanceType.Dispose();
                        hv_IgnoreDirection.Dispose();
                        hv_ClassIDsNoOrientation.Dispose();
                        hv_SemTypes.Dispose();

                        return;
                    }
                    //Set expected values and types.
                    hv_ValidValues.Dispose();
                    hv_ValidValues = new HTuple();
                    hv_ValidTypes.Dispose();
                    hv_ValidTypes = new HTuple();
                    if ((int)(new HTuple(hv_Key.TupleEqual("normalization_type"))) != 0)
                    {
                        hv_ValidValues.Dispose();
                        hv_ValidValues = new HTuple();
                        hv_ValidValues[0] = "all_channels";
                        hv_ValidValues[1] = "first_channel";
                        hv_ValidValues[2] = "constant_values";
                        hv_ValidValues[3] = "none";
                    }
                    else if ((int)(new HTuple(hv_Key.TupleEqual("domain_handling"))) != 0)
                    {
                        if ((int)(new HTuple(hv_DLModelType.TupleEqual("anomaly_detection"))) != 0)
                        {
                            hv_ValidValues.Dispose();
                            hv_ValidValues = new HTuple();
                            hv_ValidValues[0] = "full_domain";
                            hv_ValidValues[1] = "crop_domain";
                            hv_ValidValues[2] = "keep_domain";
                        }
                        else
                        {
                            hv_ValidValues.Dispose();
                            hv_ValidValues = new HTuple();
                            hv_ValidValues[0] = "full_domain";
                            hv_ValidValues[1] = "crop_domain";
                        }
                    }
                    else if ((int)(new HTuple(hv_Key.TupleEqual("model_type"))) != 0)
                    {
                        hv_ValidValues.Dispose();
                        hv_ValidValues = new HTuple();
                        hv_ValidValues[0] = "anomaly_detection";
                        hv_ValidValues[1] = "classification";
                        hv_ValidValues[2] = "detection";
                        hv_ValidValues[3] = "segmentation";
                    }
                    else if ((int)(new HTuple(hv_Key.TupleEqual("set_background_id"))) != 0)
                    {
                        hv_ValidTypes.Dispose();
                        hv_ValidTypes = "int";
                    }
                    else if ((int)(new HTuple(hv_Key.TupleEqual("class_ids_background"))) != 0)
                    {
                        hv_ValidTypes.Dispose();
                        hv_ValidTypes = "int";
                    }
                    //Check that type is valid.
                    if ((int)(new HTuple((new HTuple(hv_ValidTypes.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        for (hv_V = 0; (int)hv_V <= (int)((new HTuple(hv_ValidTypes.TupleLength())) - 1); hv_V = (int)hv_V + 1)
                        {
                            hv_T.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_T = hv_ValidTypes.TupleSelect(
                                    hv_V);
                            }
                            if ((int)(new HTuple(hv_T.TupleEqual("int"))) != 0)
                            {
                                hv_IsInt.Dispose();
                                HOperatorSet.TupleIsInt(hv_Value, out hv_IsInt);
                                if ((int)(hv_IsInt.TupleNot()) != 0)
                                {
                                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                    {
                                        {
                                            HTuple
                                              ExpTmpLocalVar_ValidTypes = ("'" + hv_ValidTypes) + "'";
                                            hv_ValidTypes.Dispose();
                                            hv_ValidTypes = ExpTmpLocalVar_ValidTypes;
                                        }
                                    }
                                    if ((int)(new HTuple((new HTuple(hv_ValidTypes.TupleLength())).TupleLess(
                                        2))) != 0)
                                    {
                                        hv_ValidTypesListing.Dispose();
                                        hv_ValidTypesListing = new HTuple(hv_ValidTypes);
                                    }
                                    else
                                    {
                                        hv_ValidTypesListing.Dispose();
                                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                        {
                                            hv_ValidTypesListing = ((((hv_ValidTypes.TupleSelectRange(
                                                0, (new HTuple(0)).TupleMax2((new HTuple(hv_ValidTypes.TupleLength()
                                                )) - 2))) + new HTuple(", ")) + (hv_ValidTypes.TupleSelect((new HTuple(hv_ValidTypes.TupleLength()
                                                )) - 1)))).TupleSum();
                                        }
                                    }
                                    throw new HalconException(((((("The value given in the key '" + hv_Key) + "' of DLPreprocessParam is invalid. Valid types are: ") + hv_ValidTypesListing) + ". The given value was '") + hv_Value) + "'.");

                                    hv_CheckParams.Dispose();
                                    hv_KeyExists.Dispose();
                                    hv_DLModelType.Dispose();
                                    hv_Exception.Dispose();
                                    hv_SupportedModelTypes.Dispose();
                                    hv_Index.Dispose();
                                    hv_ParamNamesGeneral.Dispose();
                                    hv_ParamNamesSegmentation.Dispose();
                                    hv_ParamNamesDetectionOptional.Dispose();
                                    hv_ParamNamesPreprocessingOptional.Dispose();
                                    hv_ParamNamesAll.Dispose();
                                    hv_ParamNames.Dispose();
                                    hv_KeysExists.Dispose();
                                    hv_I.Dispose();
                                    hv_Exists.Dispose();
                                    hv_InputKeys.Dispose();
                                    hv_Key.Dispose();
                                    hv_Value.Dispose();
                                    hv_Indices.Dispose();
                                    hv_ValidValues.Dispose();
                                    hv_ValidTypes.Dispose();
                                    hv_V.Dispose();
                                    hv_T.Dispose();
                                    hv_IsInt.Dispose();
                                    hv_ValidTypesListing.Dispose();
                                    hv_ValidValueListing.Dispose();
                                    hv_EmptyStrings.Dispose();
                                    hv_ImageRangeMinExists.Dispose();
                                    hv_ImageRangeMaxExists.Dispose();
                                    hv_ImageRangeMin.Dispose();
                                    hv_ImageRangeMax.Dispose();
                                    hv_IndexParam.Dispose();
                                    hv_SetBackgroundID.Dispose();
                                    hv_ClassIDsBackground.Dispose();
                                    hv_Intersection.Dispose();
                                    hv_IgnoreClassIDs.Dispose();
                                    hv_KnownClasses.Dispose();
                                    hv_IgnoreClassID.Dispose();
                                    hv_OptionalKeysExist.Dispose();
                                    hv_InstanceType.Dispose();
                                    hv_IgnoreDirection.Dispose();
                                    hv_ClassIDsNoOrientation.Dispose();
                                    hv_SemTypes.Dispose();

                                    return;
                                }
                            }
                            else
                            {
                                throw new HalconException("Internal error. Unknown valid type.");
                            }
                        }
                    }
                    //Check that value is valid.
                    if ((int)(new HTuple((new HTuple(hv_ValidValues.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        hv_Index.Dispose();
                        HOperatorSet.TupleFindFirst(hv_ValidValues, hv_Value, out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleEqual(-1))) != 0)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                {
                                    HTuple
                                      ExpTmpLocalVar_ValidValues = ("'" + hv_ValidValues) + "'";
                                    hv_ValidValues.Dispose();
                                    hv_ValidValues = ExpTmpLocalVar_ValidValues;
                                }
                            }
                            if ((int)(new HTuple((new HTuple(hv_ValidValues.TupleLength())).TupleLess(
                                2))) != 0)
                            {
                                hv_ValidValueListing.Dispose();
                                hv_ValidValueListing = new HTuple(hv_ValidValues);
                            }
                            else
                            {
                                hv_EmptyStrings.Dispose();
                                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                {
                                    hv_EmptyStrings = HTuple.TupleGenConst(
                                        (new HTuple(hv_ValidValues.TupleLength())) - 2, "");
                                }
                                hv_ValidValueListing.Dispose();
                                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                {
                                    hv_ValidValueListing = ((((hv_ValidValues.TupleSelectRange(
                                        0, (new HTuple(0)).TupleMax2((new HTuple(hv_ValidValues.TupleLength()
                                        )) - 2))) + new HTuple(", ")) + (hv_EmptyStrings.TupleConcat(hv_ValidValues.TupleSelect(
                                        (new HTuple(hv_ValidValues.TupleLength())) - 1))))).TupleSum();
                                }
                            }
                            throw new HalconException(((((("The value given in the key '" + hv_Key) + "' of DLPreprocessParam is invalid. Valid values are: ") + hv_ValidValueListing) + ". The given value was '") + hv_Value) + "'.");
                        }
                    }
                }
                //
                //Check the correct setting of ImageRangeMin and ImageRangeMax.
                if ((int)((new HTuple(hv_DLModelType.TupleEqual("classification"))).TupleOr(
                    new HTuple(hv_DLModelType.TupleEqual("detection")))) != 0)
                {
                    //Check ImageRangeMin and ImageRangeMax.
                    hv_ImageRangeMinExists.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", "image_range_min",
                        out hv_ImageRangeMinExists);
                    hv_ImageRangeMaxExists.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", "image_range_max",
                        out hv_ImageRangeMaxExists);
                    //If they are present, check that they are set correctly.
                    if ((int)(hv_ImageRangeMinExists) != 0)
                    {
                        hv_ImageRangeMin.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_min", out hv_ImageRangeMin);
                        if ((int)(new HTuple(hv_ImageRangeMin.TupleNotEqual(-127))) != 0)
                        {
                            throw new HalconException(("For model type " + hv_DLModelType) + " ImageRangeMin has to be -127.");
                        }
                    }
                    if ((int)(hv_ImageRangeMaxExists) != 0)
                    {
                        hv_ImageRangeMax.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_max", out hv_ImageRangeMax);
                        if ((int)(new HTuple(hv_ImageRangeMax.TupleNotEqual(128))) != 0)
                        {
                            throw new HalconException(("For model type " + hv_DLModelType) + " ImageRangeMax has to be 128.");
                        }
                    }
                }
                //
                //Check segmentation specific parameters.
                if ((int)(new HTuple(hv_DLModelType.TupleEqual("segmentation"))) != 0)
                {
                    //Check if detection specific parameters are set.
                    hv_KeysExists.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", hv_ParamNamesDetectionOptional,
                        out hv_KeysExists);
                    //If they are present, check that they are [].
                    for (hv_IndexParam = 0; (int)hv_IndexParam <= (int)((new HTuple(hv_ParamNamesDetectionOptional.TupleLength()
                        )) - 1); hv_IndexParam = (int)hv_IndexParam + 1)
                    {
                        if ((int)(hv_KeysExists.TupleSelect(hv_IndexParam)) != 0)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Value.Dispose();
                                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, hv_ParamNamesDetectionOptional.TupleSelect(
                                    hv_IndexParam), out hv_Value);
                            }
                            if ((int)(new HTuple(hv_Value.TupleNotEqual(new HTuple()))) != 0)
                            {
                                throw new HalconException(((("The preprocessing parameter '" + (hv_ParamNamesDetectionOptional.TupleSelect(
                                    hv_IndexParam))) + "' was set to ") + hv_Value) + new HTuple(" but for segmentation it should be set to [], as it is not used for this method."));
                            }
                        }
                    }
                    //Check 'set_background_id'.
                    hv_SetBackgroundID.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "set_background_id", out hv_SetBackgroundID);
                    if ((int)(new HTuple((new HTuple(hv_SetBackgroundID.TupleLength())).TupleGreater(
                        1))) != 0)
                    {
                        throw new HalconException("Only one class_id as 'set_background_id' allowed.");
                    }
                    //Check 'class_ids_background'.
                    hv_ClassIDsBackground.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "class_ids_background", out hv_ClassIDsBackground);
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_SetBackgroundID.TupleLength()
                        )).TupleGreater(0))).TupleAnd((new HTuple((new HTuple(hv_ClassIDsBackground.TupleLength()
                        )).TupleGreater(0))).TupleNot()))).TupleOr((new HTuple((new HTuple(hv_ClassIDsBackground.TupleLength()
                        )).TupleGreater(0))).TupleAnd((new HTuple((new HTuple(hv_SetBackgroundID.TupleLength()
                        )).TupleGreater(0))).TupleNot()))) != 0)
                    {
                        throw new HalconException("Both keys 'set_background_id' and 'class_ids_background' are required.");
                    }
                    //Check that 'class_ids_background' and 'set_background_id' are disjoint.
                    if ((int)(new HTuple((new HTuple(hv_SetBackgroundID.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        hv_Intersection.Dispose();
                        HOperatorSet.TupleIntersection(hv_SetBackgroundID, hv_ClassIDsBackground,
                            out hv_Intersection);
                        if ((int)(new HTuple(hv_Intersection.TupleLength())) != 0)
                        {
                            throw new HalconException("Class IDs in 'set_background_id' and 'class_ids_background' need to be disjoint.");
                        }
                    }
                    //Check 'ignore_class_ids'.
                    hv_IgnoreClassIDs.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "ignore_class_ids", out hv_IgnoreClassIDs);
                    hv_KnownClasses.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_KnownClasses = new HTuple();
                        hv_KnownClasses = hv_KnownClasses.TupleConcat(hv_SetBackgroundID, hv_ClassIDsBackground);
                    }
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_IgnoreClassIDs.TupleLength()
                        )) - 1); hv_I = (int)hv_I + 1)
                    {
                        hv_IgnoreClassID.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_IgnoreClassID = hv_IgnoreClassIDs.TupleSelect(
                                hv_I);
                        }
                        hv_Index.Dispose();
                        HOperatorSet.TupleFindFirst(hv_KnownClasses, hv_IgnoreClassID, out hv_Index);
                        if ((int)((new HTuple((new HTuple(hv_Index.TupleLength())).TupleGreater(
                            0))).TupleAnd(new HTuple(hv_Index.TupleNotEqual(-1)))) != 0)
                        {
                            throw new HalconException("The given 'ignore_class_ids' must not be included in the 'class_ids_background' or 'set_background_id'.");
                        }
                    }
                }
                else if ((int)(new HTuple(hv_DLModelType.TupleEqual("detection"))) != 0)
                {
                    //Check if segmentation specific parameters are set.
                    hv_KeysExists.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", hv_ParamNamesSegmentation,
                        out hv_KeysExists);
                    //If they are present, check that they are [].
                    for (hv_IndexParam = 0; (int)hv_IndexParam <= (int)((new HTuple(hv_ParamNamesSegmentation.TupleLength()
                        )) - 1); hv_IndexParam = (int)hv_IndexParam + 1)
                    {
                        if ((int)(hv_KeysExists.TupleSelect(hv_IndexParam)) != 0)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Value.Dispose();
                                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, hv_ParamNamesSegmentation.TupleSelect(
                                    hv_IndexParam), out hv_Value);
                            }
                            if ((int)(new HTuple(hv_Value.TupleNotEqual(new HTuple()))) != 0)
                            {
                                throw new HalconException(((("The preprocessing parameter '" + (hv_ParamNamesSegmentation.TupleSelect(
                                    hv_IndexParam))) + "' was set to ") + hv_Value) + new HTuple(" but for detection it should be set to [], as it is not used for this method."));
                            }
                        }
                    }
                    //Check optional parameters.
                    hv_OptionalKeysExist.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", hv_ParamNamesDetectionOptional,
                        out hv_OptionalKeysExist);
                    if ((int)(hv_OptionalKeysExist.TupleSelect(0)) != 0)
                    {
                        //Check 'instance_type'.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_InstanceType.Dispose();
                            HOperatorSet.GetDictTuple(hv_DLPreprocessParam, hv_ParamNamesDetectionOptional.TupleSelect(
                                0), out hv_InstanceType);
                        }
                        if ((int)(new HTuple((new HTuple(((new HTuple("rectangle1")).TupleConcat(
                            "rectangle2")).TupleFind(hv_InstanceType))).TupleEqual(-1))) != 0)
                        {
                            throw new HalconException(("Invalid generic parameter for 'instance_type': " + hv_InstanceType) + new HTuple(", only 'rectangle1' and 'rectangle2' are allowed"));
                        }
                    }
                    hv_OptionalKeysExist.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", hv_ParamNamesDetectionOptional,
                        out hv_OptionalKeysExist);
                    if ((int)(hv_OptionalKeysExist.TupleSelect(1)) != 0)
                    {
                        //Check 'ignore_direction'.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_IgnoreDirection.Dispose();
                            HOperatorSet.GetDictTuple(hv_DLPreprocessParam, hv_ParamNamesDetectionOptional.TupleSelect(
                                1), out hv_IgnoreDirection);
                        }
                        if ((int)(new HTuple((new HTuple(((new HTuple(1)).TupleConcat(0)).TupleFind(
                            hv_IgnoreDirection))).TupleEqual(-1))) != 0)
                        {
                            throw new HalconException(("Invalid generic parameter for 'ignore_direction': " + hv_IgnoreDirection) + new HTuple(", only true and false are allowed"));
                        }
                    }
                    if ((int)(hv_OptionalKeysExist.TupleSelect(2)) != 0)
                    {
                        //Check 'class_ids_no_orientation'.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_ClassIDsNoOrientation.Dispose();
                            HOperatorSet.GetDictTuple(hv_DLPreprocessParam, hv_ParamNamesDetectionOptional.TupleSelect(
                                2), out hv_ClassIDsNoOrientation);
                        }
                        hv_SemTypes.Dispose();
                        HOperatorSet.TupleSemTypeElem(hv_ClassIDsNoOrientation, out hv_SemTypes);
                        if ((int)((new HTuple(hv_ClassIDsNoOrientation.TupleNotEqual(new HTuple()))).TupleAnd(
                            new HTuple(((((hv_SemTypes.TupleEqualElem("integer"))).TupleSum())).TupleNotEqual(
                            new HTuple(hv_ClassIDsNoOrientation.TupleLength()))))) != 0)
                        {
                            throw new HalconException(("Invalid generic parameter for 'class_ids_no_orientation': " + hv_ClassIDsNoOrientation) + new HTuple(", only integers are allowed"));
                        }
                        else
                        {
                            if ((int)((new HTuple(hv_ClassIDsNoOrientation.TupleNotEqual(new HTuple()))).TupleAnd(
                                new HTuple(((((hv_ClassIDsNoOrientation.TupleGreaterEqualElem(0))).TupleSum()
                                )).TupleNotEqual(new HTuple(hv_ClassIDsNoOrientation.TupleLength()
                                ))))) != 0)
                            {
                                throw new HalconException(("Invalid generic parameter for 'class_ids_no_orientation': " + hv_ClassIDsNoOrientation) + new HTuple(", only non-negative integers are allowed"));
                            }
                        }
                    }
                }
                //

                hv_CheckParams.Dispose();
                hv_KeyExists.Dispose();
                hv_DLModelType.Dispose();
                hv_Exception.Dispose();
                hv_SupportedModelTypes.Dispose();
                hv_Index.Dispose();
                hv_ParamNamesGeneral.Dispose();
                hv_ParamNamesSegmentation.Dispose();
                hv_ParamNamesDetectionOptional.Dispose();
                hv_ParamNamesPreprocessingOptional.Dispose();
                hv_ParamNamesAll.Dispose();
                hv_ParamNames.Dispose();
                hv_KeysExists.Dispose();
                hv_I.Dispose();
                hv_Exists.Dispose();
                hv_InputKeys.Dispose();
                hv_Key.Dispose();
                hv_Value.Dispose();
                hv_Indices.Dispose();
                hv_ValidValues.Dispose();
                hv_ValidTypes.Dispose();
                hv_V.Dispose();
                hv_T.Dispose();
                hv_IsInt.Dispose();
                hv_ValidTypesListing.Dispose();
                hv_ValidValueListing.Dispose();
                hv_EmptyStrings.Dispose();
                hv_ImageRangeMinExists.Dispose();
                hv_ImageRangeMaxExists.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_IndexParam.Dispose();
                hv_SetBackgroundID.Dispose();
                hv_ClassIDsBackground.Dispose();
                hv_Intersection.Dispose();
                hv_IgnoreClassIDs.Dispose();
                hv_KnownClasses.Dispose();
                hv_IgnoreClassID.Dispose();
                hv_OptionalKeysExist.Dispose();
                hv_InstanceType.Dispose();
                hv_IgnoreDirection.Dispose();
                hv_ClassIDsNoOrientation.Dispose();
                hv_SemTypes.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_CheckParams.Dispose();
                hv_KeyExists.Dispose();
                hv_DLModelType.Dispose();
                hv_Exception.Dispose();
                hv_SupportedModelTypes.Dispose();
                hv_Index.Dispose();
                hv_ParamNamesGeneral.Dispose();
                hv_ParamNamesSegmentation.Dispose();
                hv_ParamNamesDetectionOptional.Dispose();
                hv_ParamNamesPreprocessingOptional.Dispose();
                hv_ParamNamesAll.Dispose();
                hv_ParamNames.Dispose();
                hv_KeysExists.Dispose();
                hv_I.Dispose();
                hv_Exists.Dispose();
                hv_InputKeys.Dispose();
                hv_Key.Dispose();
                hv_Value.Dispose();
                hv_Indices.Dispose();
                hv_ValidValues.Dispose();
                hv_ValidTypes.Dispose();
                hv_V.Dispose();
                hv_T.Dispose();
                hv_IsInt.Dispose();
                hv_ValidTypesListing.Dispose();
                hv_ValidValueListing.Dispose();
                hv_EmptyStrings.Dispose();
                hv_ImageRangeMinExists.Dispose();
                hv_ImageRangeMaxExists.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_IndexParam.Dispose();
                hv_SetBackgroundID.Dispose();
                hv_ClassIDsBackground.Dispose();
                hv_Intersection.Dispose();
                hv_IgnoreClassIDs.Dispose();
                hv_KnownClasses.Dispose();
                hv_IgnoreClassID.Dispose();
                hv_OptionalKeysExist.Dispose();
                hv_InstanceType.Dispose();
                hv_IgnoreDirection.Dispose();
                hv_ClassIDsNoOrientation.Dispose();
                hv_SemTypes.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Model
        // Short Description: Preprocess anomaly images for evaluation and visualization of the deep-learning-based anomaly detection. 
        public void preprocess_dl_model_anomaly(HObject ho_AnomalyImages, out HObject ho_AnomalyImagesPreprocessed,
            HTuple hv_DLPreprocessParam)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            // Local copy input parameter variables 
            HObject ho_AnomalyImages_COPY_INP_TMP;
            ho_AnomalyImages_COPY_INP_TMP = new HObject(ho_AnomalyImages);



            // Local control variables 

            HTuple hv_ImageWidth = new HTuple(), hv_ImageHeight = new HTuple();
            HTuple hv_ImageRangeMin = new HTuple(), hv_ImageRangeMax = new HTuple();
            HTuple hv_DomainHandling = new HTuple(), hv_ModelType = new HTuple();
            HTuple hv_ImageNumChannels = new HTuple(), hv_Min = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Range = new HTuple();
            HTuple hv_ImageWidthInput = new HTuple(), hv_ImageHeightInput = new HTuple();
            HTuple hv_EqualWidth = new HTuple(), hv_EqualHeight = new HTuple();
            HTuple hv_Type = new HTuple(), hv_NumMatches = new HTuple();
            HTuple hv_NumImages = new HTuple(), hv_EqualByte = new HTuple();
            HTuple hv_NumChannelsAllImages = new HTuple(), hv_ImageNumChannelsTuple = new HTuple();
            HTuple hv_IndicesWrongChannels = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_AnomalyImagesPreprocessed);
            try
            {
                //
                //This procedure preprocesses the anomaly images given by AnomalyImages
                //according to the parameters in the dictionary DLPreprocessParam.
                //Note that depending on the images,
                //additional preprocessing steps might be beneficial.
                //
                //Check the validity of the preprocessing parameters.
                check_dl_preprocess_param(hv_DLPreprocessParam);
                //
                //Get the preprocessing parameters.
                hv_ImageWidth.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_width", out hv_ImageWidth);
                hv_ImageHeight.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_height", out hv_ImageHeight);
                hv_ImageRangeMin.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_min", out hv_ImageRangeMin);
                hv_ImageRangeMax.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_max", out hv_ImageRangeMax);
                hv_DomainHandling.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "domain_handling", out hv_DomainHandling);
                hv_ModelType.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "model_type", out hv_ModelType);
                //
                hv_ImageNumChannels.Dispose();
                hv_ImageNumChannels = 1;
                //
                //Preprocess the images.
                //
                if ((int)(new HTuple(hv_DomainHandling.TupleEqual("full_domain"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.FullDomain(ho_AnomalyImages_COPY_INP_TMP, out ExpTmpOutVar_0
                            );
                        ho_AnomalyImages_COPY_INP_TMP.Dispose();
                        ho_AnomalyImages_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                else if ((int)(new HTuple(hv_DomainHandling.TupleEqual("crop_domain"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.CropDomain(ho_AnomalyImages_COPY_INP_TMP, out ExpTmpOutVar_0
                            );
                        ho_AnomalyImages_COPY_INP_TMP.Dispose();
                        ho_AnomalyImages_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                else if ((int)((new HTuple(hv_DomainHandling.TupleEqual("keep_domain"))).TupleAnd(
                    new HTuple(hv_ModelType.TupleEqual("anomaly_detection")))) != 0)
                {
                    //Anomaly detection models accept the additional option 'keep_domain'.
                }
                else
                {
                    throw new HalconException("Unsupported parameter value for 'domain_handling'");
                }
                //
                hv_Min.Dispose(); hv_Max.Dispose(); hv_Range.Dispose();
                HOperatorSet.MinMaxGray(ho_AnomalyImages_COPY_INP_TMP, ho_AnomalyImages_COPY_INP_TMP,
                    0, out hv_Min, out hv_Max, out hv_Range);
                if ((int)(new HTuple(hv_Min.TupleLess(0.0))) != 0)
                {
                    throw new HalconException("Values of anomaly image must not be smaller than 0.0.");
                }
                //
                //Zoom images only if they have a different size than the specified size.
                hv_ImageWidthInput.Dispose(); hv_ImageHeightInput.Dispose();
                HOperatorSet.GetImageSize(ho_AnomalyImages_COPY_INP_TMP, out hv_ImageWidthInput,
                    out hv_ImageHeightInput);
                hv_EqualWidth.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EqualWidth = hv_ImageWidth.TupleEqualElem(
                        hv_ImageWidthInput);
                }
                hv_EqualHeight.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EqualHeight = hv_ImageHeight.TupleEqualElem(
                        hv_ImageHeightInput);
                }
                if ((int)((new HTuple(((hv_EqualWidth.TupleMin())).TupleEqual(0))).TupleOr(
                    new HTuple(((hv_EqualHeight.TupleMin())).TupleEqual(0)))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ZoomImageSize(ho_AnomalyImages_COPY_INP_TMP, out ExpTmpOutVar_0,
                            hv_ImageWidth, hv_ImageHeight, "nearest_neighbor");
                        ho_AnomalyImages_COPY_INP_TMP.Dispose();
                        ho_AnomalyImages_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                //
                //Check the type of the input images.
                hv_Type.Dispose();
                HOperatorSet.GetImageType(ho_AnomalyImages_COPY_INP_TMP, out hv_Type);
                hv_NumMatches.Dispose();
                HOperatorSet.TupleRegexpTest(hv_Type, "byte|real", out hv_NumMatches);
                hv_NumImages.Dispose();
                HOperatorSet.CountObj(ho_AnomalyImages_COPY_INP_TMP, out hv_NumImages);
                if ((int)(new HTuple(hv_NumMatches.TupleNotEqual(hv_NumImages))) != 0)
                {
                    throw new HalconException("Please provide only images of type 'byte' or 'real'.");
                }
                //
                //If the type is 'byte', convert it to 'real' and scale it.
                //The gray value scaling does not work on 'byte' images.
                //For 'real' images it is assumed that the range is already correct.
                hv_EqualByte.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EqualByte = hv_Type.TupleEqualElem(
                        "byte");
                }
                if ((int)(new HTuple(((hv_EqualByte.TupleMax())).TupleEqual(1))) != 0)
                {
                    if ((int)(new HTuple(((hv_EqualByte.TupleMin())).TupleEqual(0))) != 0)
                    {
                        throw new HalconException("Passing mixed type images is not supported.");
                    }
                    //Convert the image type from 'byte' to 'real',
                    //because the model expects 'real' images.
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_AnomalyImages_COPY_INP_TMP, out ExpTmpOutVar_0,
                            "real");
                        ho_AnomalyImages_COPY_INP_TMP.Dispose();
                        ho_AnomalyImages_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                //
                //Check the number of channels.
                hv_NumImages.Dispose();
                HOperatorSet.CountObj(ho_AnomalyImages_COPY_INP_TMP, out hv_NumImages);
                //Check all images for number of channels.
                hv_NumChannelsAllImages.Dispose();
                HOperatorSet.CountChannels(ho_AnomalyImages_COPY_INP_TMP, out hv_NumChannelsAllImages);
                hv_ImageNumChannelsTuple.Dispose();
                HOperatorSet.TupleGenConst(hv_NumImages, hv_ImageNumChannels, out hv_ImageNumChannelsTuple);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_IndicesWrongChannels.Dispose();
                    HOperatorSet.TupleFind(hv_NumChannelsAllImages.TupleNotEqualElem(hv_ImageNumChannelsTuple),
                        1, out hv_IndicesWrongChannels);
                }
                //
                //Check for anomaly image channels.
                //Only single channel images are accepted.
                if ((int)(new HTuple(hv_IndicesWrongChannels.TupleNotEqual(-1))) != 0)
                {
                    throw new HalconException("Number of channels in anomaly image is not supported. Please check for anomaly images with a number of channels different from 1.");
                }
                //
                //Write preprocessed image to output variable.
                ho_AnomalyImagesPreprocessed.Dispose();
                ho_AnomalyImagesPreprocessed = new HObject(ho_AnomalyImages_COPY_INP_TMP);
                //
                ho_AnomalyImages_COPY_INP_TMP.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_DomainHandling.Dispose();
                hv_ModelType.Dispose();
                hv_ImageNumChannels.Dispose();
                hv_Min.Dispose();
                hv_Max.Dispose();
                hv_Range.Dispose();
                hv_ImageWidthInput.Dispose();
                hv_ImageHeightInput.Dispose();
                hv_EqualWidth.Dispose();
                hv_EqualHeight.Dispose();
                hv_Type.Dispose();
                hv_NumMatches.Dispose();
                hv_NumImages.Dispose();
                hv_EqualByte.Dispose();
                hv_NumChannelsAllImages.Dispose();
                hv_ImageNumChannelsTuple.Dispose();
                hv_IndicesWrongChannels.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_AnomalyImages_COPY_INP_TMP.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_DomainHandling.Dispose();
                hv_ModelType.Dispose();
                hv_ImageNumChannels.Dispose();
                hv_Min.Dispose();
                hv_Max.Dispose();
                hv_Range.Dispose();
                hv_ImageWidthInput.Dispose();
                hv_ImageHeightInput.Dispose();
                hv_EqualWidth.Dispose();
                hv_EqualHeight.Dispose();
                hv_Type.Dispose();
                hv_NumMatches.Dispose();
                hv_NumImages.Dispose();
                hv_EqualByte.Dispose();
                hv_NumChannelsAllImages.Dispose();
                hv_ImageNumChannelsTuple.Dispose();
                hv_IndicesWrongChannels.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Object Detection
        // Short Description: This procedure preprocesses the bounding boxes of type 'rectangle1' for a given sample. 
        private void preprocess_dl_model_bbox_rect1(HObject ho_ImageRaw, HTuple hv_DLSample,
            HTuple hv_DLPreprocessParam)
        {




            // Local iconic variables 

            HObject ho_DomainRaw = null;

            // Local control variables 

            HTuple hv_ImageWidth = new HTuple(), hv_ImageHeight = new HTuple();
            HTuple hv_DomainHandling = new HTuple(), hv_BBoxCol1 = new HTuple();
            HTuple hv_BBoxCol2 = new HTuple(), hv_BBoxRow1 = new HTuple();
            HTuple hv_BBoxRow2 = new HTuple(), hv_BBoxLabel = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_ImageId = new HTuple();
            HTuple hv_ExceptionMessage = new HTuple(), hv_BoxesInvalid = new HTuple();
            HTuple hv_RowDomain1 = new HTuple(), hv_ColumnDomain1 = new HTuple();
            HTuple hv_RowDomain2 = new HTuple(), hv_ColumnDomain2 = new HTuple();
            HTuple hv_WidthRaw = new HTuple(), hv_HeightRaw = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Col1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Col2 = new HTuple();
            HTuple hv_MaskDelete = new HTuple(), hv_MaskNewBbox = new HTuple();
            HTuple hv_BBoxCol1New = new HTuple(), hv_BBoxCol2New = new HTuple();
            HTuple hv_BBoxRow1New = new HTuple(), hv_BBoxRow2New = new HTuple();
            HTuple hv_BBoxLabelNew = new HTuple(), hv_FactorResampleWidth = new HTuple();
            HTuple hv_FactorResampleHeight = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DomainRaw);
            try
            {
                //
                //This procedure preprocesses the bounding boxes of type 'rectangle1' for a given sample.
                //
                //Check the validity of the preprocessing parameters.
                check_dl_preprocess_param(hv_DLPreprocessParam);
                //
                //Get the preprocessing parameters.
                hv_ImageWidth.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_width", out hv_ImageWidth);
                hv_ImageHeight.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_height", out hv_ImageHeight);
                hv_DomainHandling.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "domain_handling", out hv_DomainHandling);
                //
                //Get bounding box coordinates and labels.
                try
                {
                    hv_BBoxCol1.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_col1", out hv_BBoxCol1);
                    hv_BBoxCol2.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_col2", out hv_BBoxCol2);
                    hv_BBoxRow1.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_row1", out hv_BBoxRow1);
                    hv_BBoxRow2.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_row2", out hv_BBoxRow2);
                    hv_BBoxLabel.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_label_id", out hv_BBoxLabel);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_ImageId.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "image_id", out hv_ImageId);
                    if ((int)(new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1302))) != 0)
                    {
                        hv_ExceptionMessage.Dispose();
                        hv_ExceptionMessage = "A bounding box coordinate key is missing.";
                    }
                    else
                    {
                        hv_ExceptionMessage.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_ExceptionMessage = hv_Exception.TupleSelect(
                                2);
                        }
                    }
                    throw new HalconException((("An error has occurred during preprocessing image_id " + hv_ImageId) + " when getting bounding box coordinates : ") + hv_ExceptionMessage);
                }
                //
                //Check that there are no invalid boxes.
                if ((int)(new HTuple((new HTuple(hv_BBoxRow1.TupleLength())).TupleGreater(0))) != 0)
                {
                    hv_BoxesInvalid.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BoxesInvalid = ((hv_BBoxRow1.TupleGreaterEqualElem(
                            hv_BBoxRow2))).TupleOr(hv_BBoxCol1.TupleGreaterEqualElem(hv_BBoxCol2));
                    }
                    if ((int)(new HTuple(((hv_BoxesInvalid.TupleSum())).TupleGreater(0))) != 0)
                    {
                        hv_ImageId.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLSample, "image_id", out hv_ImageId);
                        throw new HalconException(("An error has occurred during preprocessing image_id " + hv_ImageId) + new HTuple(": Sample contains at least one box with zero-area, i.e. bbox_col1 >= bbox_col2 or bbox_row1 >= bbox_row2."));
                    }
                }
                else
                {
                    //There are no bounding boxes, hence nothing to do.
                    ho_DomainRaw.Dispose();

                    hv_ImageWidth.Dispose();
                    hv_ImageHeight.Dispose();
                    hv_DomainHandling.Dispose();
                    hv_BBoxCol1.Dispose();
                    hv_BBoxCol2.Dispose();
                    hv_BBoxRow1.Dispose();
                    hv_BBoxRow2.Dispose();
                    hv_BBoxLabel.Dispose();
                    hv_Exception.Dispose();
                    hv_ImageId.Dispose();
                    hv_ExceptionMessage.Dispose();
                    hv_BoxesInvalid.Dispose();
                    hv_RowDomain1.Dispose();
                    hv_ColumnDomain1.Dispose();
                    hv_RowDomain2.Dispose();
                    hv_ColumnDomain2.Dispose();
                    hv_WidthRaw.Dispose();
                    hv_HeightRaw.Dispose();
                    hv_Row1.Dispose();
                    hv_Col1.Dispose();
                    hv_Row2.Dispose();
                    hv_Col2.Dispose();
                    hv_MaskDelete.Dispose();
                    hv_MaskNewBbox.Dispose();
                    hv_BBoxCol1New.Dispose();
                    hv_BBoxCol2New.Dispose();
                    hv_BBoxRow1New.Dispose();
                    hv_BBoxRow2New.Dispose();
                    hv_BBoxLabelNew.Dispose();
                    hv_FactorResampleWidth.Dispose();
                    hv_FactorResampleHeight.Dispose();

                    return;
                }
                //
                //If the domain is cropped, crop bounding boxes.
                if ((int)(new HTuple(hv_DomainHandling.TupleEqual("crop_domain"))) != 0)
                {
                    //
                    //Get domain.
                    ho_DomainRaw.Dispose();
                    HOperatorSet.GetDomain(ho_ImageRaw, out ho_DomainRaw);
                    //
                    //Set the size of the raw image to the domain extensions.
                    hv_RowDomain1.Dispose(); hv_ColumnDomain1.Dispose(); hv_RowDomain2.Dispose(); hv_ColumnDomain2.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_DomainRaw, out hv_RowDomain1, out hv_ColumnDomain1,
                        out hv_RowDomain2, out hv_ColumnDomain2);
                    //The domain is always given as a pixel-precise region.
                    hv_WidthRaw.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_WidthRaw = (hv_ColumnDomain2 - hv_ColumnDomain1) + 1.0;
                    }
                    hv_HeightRaw.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_HeightRaw = (hv_RowDomain2 - hv_RowDomain1) + 1.0;
                    }
                    //
                    //Crop the bounding boxes.
                    hv_Row1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Row1 = hv_BBoxRow1.TupleMax2(
                            hv_RowDomain1 - .5);
                    }
                    hv_Col1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Col1 = hv_BBoxCol1.TupleMax2(
                            hv_ColumnDomain1 - .5);
                    }
                    hv_Row2.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Row2 = hv_BBoxRow2.TupleMin2(
                            hv_RowDomain2 + .5);
                    }
                    hv_Col2.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Col2 = hv_BBoxCol2.TupleMin2(
                            hv_ColumnDomain2 + .5);
                    }
                    hv_MaskDelete.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaskDelete = ((hv_Row1.TupleGreaterEqualElem(
                            hv_Row2))).TupleOr(hv_Col1.TupleGreaterEqualElem(hv_Col2));
                    }
                    hv_MaskNewBbox.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaskNewBbox = 1 - hv_MaskDelete;
                    }
                    //Store the preprocessed bounding box entries.
                    hv_BBoxCol1New.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxCol1New = (hv_Col1.TupleSelectMask(
                            hv_MaskNewBbox)) - hv_ColumnDomain1;
                    }
                    hv_BBoxCol2New.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxCol2New = (hv_Col2.TupleSelectMask(
                            hv_MaskNewBbox)) - hv_ColumnDomain1;
                    }
                    hv_BBoxRow1New.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxRow1New = (hv_Row1.TupleSelectMask(
                            hv_MaskNewBbox)) - hv_RowDomain1;
                    }
                    hv_BBoxRow2New.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxRow2New = (hv_Row2.TupleSelectMask(
                            hv_MaskNewBbox)) - hv_RowDomain1;
                    }
                    hv_BBoxLabelNew.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxLabelNew = hv_BBoxLabel.TupleSelectMask(
                            hv_MaskNewBbox);
                    }
                    //
                }
                else if ((int)(new HTuple(hv_DomainHandling.TupleEqual("full_domain"))) != 0)
                {
                    //If the entire image is used, set the variables accordingly.
                    //Get the original size.
                    hv_WidthRaw.Dispose(); hv_HeightRaw.Dispose();
                    HOperatorSet.GetImageSize(ho_ImageRaw, out hv_WidthRaw, out hv_HeightRaw);
                    //Set new coordinates to input coordinates.
                    hv_BBoxCol1New.Dispose();
                    hv_BBoxCol1New = new HTuple(hv_BBoxCol1);
                    hv_BBoxCol2New.Dispose();
                    hv_BBoxCol2New = new HTuple(hv_BBoxCol2);
                    hv_BBoxRow1New.Dispose();
                    hv_BBoxRow1New = new HTuple(hv_BBoxRow1);
                    hv_BBoxRow2New.Dispose();
                    hv_BBoxRow2New = new HTuple(hv_BBoxRow2);
                    hv_BBoxLabelNew.Dispose();
                    hv_BBoxLabelNew = new HTuple(hv_BBoxLabel);
                }
                else
                {
                    throw new HalconException("Unsupported parameter value for 'domain_handling'");
                }
                //
                //Rescale the bounding boxes.
                //
                //Get required images width and height.
                //
                //Only rescale bounding boxes if the required image dimensions are not the raw dimensions.
                if ((int)((new HTuple(hv_ImageHeight.TupleNotEqual(hv_HeightRaw))).TupleOr(
                    new HTuple(hv_ImageWidth.TupleNotEqual(hv_WidthRaw)))) != 0)
                {
                    //Calculate rescaling factor.
                    hv_FactorResampleWidth.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_FactorResampleWidth = (hv_ImageWidth.TupleReal()
                            ) / hv_WidthRaw;
                    }
                    hv_FactorResampleHeight.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_FactorResampleHeight = (hv_ImageHeight.TupleReal()
                            ) / hv_HeightRaw;
                    }
                    //Rescale the bounding box coordinates.
                    //As we use XLD-coordinates we temporarily move the boxes by (.5,.5) for rescaling.
                    //Doing so, the center of the XLD-coordinate system (-0.5,-0.5) is used
                    //for scaling, hence the scaling is performed w.r.t. the pixel coordinate system.
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxCol1New = ((hv_BBoxCol1New + .5) * hv_FactorResampleWidth) - .5;
                            hv_BBoxCol1New.Dispose();
                            hv_BBoxCol1New = ExpTmpLocalVar_BBoxCol1New;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxCol2New = ((hv_BBoxCol2New + .5) * hv_FactorResampleWidth) - .5;
                            hv_BBoxCol2New.Dispose();
                            hv_BBoxCol2New = ExpTmpLocalVar_BBoxCol2New;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxRow1New = ((hv_BBoxRow1New + .5) * hv_FactorResampleHeight) - .5;
                            hv_BBoxRow1New.Dispose();
                            hv_BBoxRow1New = ExpTmpLocalVar_BBoxRow1New;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxRow2New = ((hv_BBoxRow2New + .5) * hv_FactorResampleHeight) - .5;
                            hv_BBoxRow2New.Dispose();
                            hv_BBoxRow2New = ExpTmpLocalVar_BBoxRow2New;
                        }
                    }
                    //
                }
                //
                //Make a final check and remove bounding boxes that have zero area.
                if ((int)(new HTuple((new HTuple(hv_BBoxRow1New.TupleLength())).TupleGreater(
                    0))) != 0)
                {
                    hv_MaskDelete.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaskDelete = ((hv_BBoxRow1New.TupleGreaterEqualElem(
                            hv_BBoxRow2New))).TupleOr(hv_BBoxCol1New.TupleGreaterEqualElem(hv_BBoxCol2New));
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxCol1New = hv_BBoxCol1New.TupleSelectMask(
                                1 - hv_MaskDelete);
                            hv_BBoxCol1New.Dispose();
                            hv_BBoxCol1New = ExpTmpLocalVar_BBoxCol1New;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxCol2New = hv_BBoxCol2New.TupleSelectMask(
                                1 - hv_MaskDelete);
                            hv_BBoxCol2New.Dispose();
                            hv_BBoxCol2New = ExpTmpLocalVar_BBoxCol2New;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxRow1New = hv_BBoxRow1New.TupleSelectMask(
                                1 - hv_MaskDelete);
                            hv_BBoxRow1New.Dispose();
                            hv_BBoxRow1New = ExpTmpLocalVar_BBoxRow1New;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxRow2New = hv_BBoxRow2New.TupleSelectMask(
                                1 - hv_MaskDelete);
                            hv_BBoxRow2New.Dispose();
                            hv_BBoxRow2New = ExpTmpLocalVar_BBoxRow2New;
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_BBoxLabelNew = hv_BBoxLabelNew.TupleSelectMask(
                                1 - hv_MaskDelete);
                            hv_BBoxLabelNew.Dispose();
                            hv_BBoxLabelNew = ExpTmpLocalVar_BBoxLabelNew;
                        }
                    }
                }
                //
                //Set new bounding box coordinates in the dictionary.
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_col1", hv_BBoxCol1New);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_col2", hv_BBoxCol2New);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_row1", hv_BBoxRow1New);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_row2", hv_BBoxRow2New);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_label_id", hv_BBoxLabelNew);
                //
                ho_DomainRaw.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_DomainHandling.Dispose();
                hv_BBoxCol1.Dispose();
                hv_BBoxCol2.Dispose();
                hv_BBoxRow1.Dispose();
                hv_BBoxRow2.Dispose();
                hv_BBoxLabel.Dispose();
                hv_Exception.Dispose();
                hv_ImageId.Dispose();
                hv_ExceptionMessage.Dispose();
                hv_BoxesInvalid.Dispose();
                hv_RowDomain1.Dispose();
                hv_ColumnDomain1.Dispose();
                hv_RowDomain2.Dispose();
                hv_ColumnDomain2.Dispose();
                hv_WidthRaw.Dispose();
                hv_HeightRaw.Dispose();
                hv_Row1.Dispose();
                hv_Col1.Dispose();
                hv_Row2.Dispose();
                hv_Col2.Dispose();
                hv_MaskDelete.Dispose();
                hv_MaskNewBbox.Dispose();
                hv_BBoxCol1New.Dispose();
                hv_BBoxCol2New.Dispose();
                hv_BBoxRow1New.Dispose();
                hv_BBoxRow2New.Dispose();
                hv_BBoxLabelNew.Dispose();
                hv_FactorResampleWidth.Dispose();
                hv_FactorResampleHeight.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_DomainRaw.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_DomainHandling.Dispose();
                hv_BBoxCol1.Dispose();
                hv_BBoxCol2.Dispose();
                hv_BBoxRow1.Dispose();
                hv_BBoxRow2.Dispose();
                hv_BBoxLabel.Dispose();
                hv_Exception.Dispose();
                hv_ImageId.Dispose();
                hv_ExceptionMessage.Dispose();
                hv_BoxesInvalid.Dispose();
                hv_RowDomain1.Dispose();
                hv_ColumnDomain1.Dispose();
                hv_RowDomain2.Dispose();
                hv_ColumnDomain2.Dispose();
                hv_WidthRaw.Dispose();
                hv_HeightRaw.Dispose();
                hv_Row1.Dispose();
                hv_Col1.Dispose();
                hv_Row2.Dispose();
                hv_Col2.Dispose();
                hv_MaskDelete.Dispose();
                hv_MaskNewBbox.Dispose();
                hv_BBoxCol1New.Dispose();
                hv_BBoxCol2New.Dispose();
                hv_BBoxRow1New.Dispose();
                hv_BBoxRow2New.Dispose();
                hv_BBoxLabelNew.Dispose();
                hv_FactorResampleWidth.Dispose();
                hv_FactorResampleHeight.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Object Detection
        // Short Description: This procedure preprocesses the bounding boxes of type 'rectangle2' for a given sample. 
        private void preprocess_dl_model_bbox_rect2(HObject ho_ImageRaw, HTuple hv_DLSample,
            HTuple hv_DLPreprocessParam)
        {




            // Local iconic variables 

            HObject ho_DomainRaw = null, ho_Rectangle2XLD = null;
            HObject ho_Rectangle2XLDSheared = null;

            // Local control variables 

            HTuple hv_ImageWidth = new HTuple(), hv_ImageHeight = new HTuple();
            HTuple hv_DomainHandling = new HTuple(), hv_IgnoreDirection = new HTuple();
            HTuple hv_ClassIDsNoOrientation = new HTuple(), hv_KeyExists = new HTuple();
            HTuple hv_BBoxRow = new HTuple(), hv_BBoxCol = new HTuple();
            HTuple hv_BBoxLength1 = new HTuple(), hv_BBoxLength2 = new HTuple();
            HTuple hv_BBoxPhi = new HTuple(), hv_BBoxLabel = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_ImageId = new HTuple();
            HTuple hv_ExceptionMessage = new HTuple(), hv_BoxesInvalid = new HTuple();
            HTuple hv_RowDomain1 = new HTuple(), hv_ColumnDomain1 = new HTuple();
            HTuple hv_RowDomain2 = new HTuple(), hv_ColumnDomain2 = new HTuple();
            HTuple hv_WidthRaw = new HTuple(), hv_HeightRaw = new HTuple();
            HTuple hv_MaskDelete = new HTuple(), hv_MaskNewBbox = new HTuple();
            HTuple hv_BBoxRowNew = new HTuple(), hv_BBoxColNew = new HTuple();
            HTuple hv_BBoxLength1New = new HTuple(), hv_BBoxLength2New = new HTuple();
            HTuple hv_BBoxPhiNew = new HTuple(), hv_BBoxLabelNew = new HTuple();
            HTuple hv_ClassIDsNoOrientationIndices = new HTuple();
            HTuple hv_Index = new HTuple(), hv_ClassIDsNoOrientationIndicesTmp = new HTuple();
            HTuple hv_DirectionLength1Row = new HTuple(), hv_DirectionLength1Col = new HTuple();
            HTuple hv_DirectionLength2Row = new HTuple(), hv_DirectionLength2Col = new HTuple();
            HTuple hv_Corner1Row = new HTuple(), hv_Corner1Col = new HTuple();
            HTuple hv_Corner2Row = new HTuple(), hv_Corner2Col = new HTuple();
            HTuple hv_FactorResampleWidth = new HTuple(), hv_FactorResampleHeight = new HTuple();
            HTuple hv_BBoxCol1 = new HTuple(), hv_BBoxCol1New = new HTuple();
            HTuple hv_BBoxCol2 = new HTuple(), hv_BBoxCol2New = new HTuple();
            HTuple hv_BBoxCol3 = new HTuple(), hv_BBoxCol3New = new HTuple();
            HTuple hv_BBoxCol4 = new HTuple(), hv_BBoxCol4New = new HTuple();
            HTuple hv_BBoxRow1 = new HTuple(), hv_BBoxRow1New = new HTuple();
            HTuple hv_BBoxRow2 = new HTuple(), hv_BBoxRow2New = new HTuple();
            HTuple hv_BBoxRow3 = new HTuple(), hv_BBoxRow3New = new HTuple();
            HTuple hv_BBoxRow4 = new HTuple(), hv_BBoxRow4New = new HTuple();
            HTuple hv_HomMat2DIdentity = new HTuple(), hv_HomMat2DScale = new HTuple();
            HTuple hv_BBoxPhiTmp = new HTuple(), hv_PhiDelta = new HTuple();
            HTuple hv_PhiDeltaNegativeIndices = new HTuple(), hv_IndicesRot90 = new HTuple();
            HTuple hv_IndicesRot180 = new HTuple(), hv_IndicesRot270 = new HTuple();
            HTuple hv_SwapIndices = new HTuple(), hv_Tmp = new HTuple();
            HTuple hv_BBoxPhiNewIndices = new HTuple(), hv_PhiThreshold = new HTuple();
            HTuple hv_PhiToCorrect = new HTuple(), hv_NumCorrections = new HTuple();
            HTuple hv__ = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DomainRaw);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2XLD);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2XLDSheared);
            try
            {
                //This procedure preprocesses the bounding boxes of type 'rectangle2' for a given sample.
                //
                check_dl_preprocess_param(hv_DLPreprocessParam);
                //
                //Get preprocess parameters.
                hv_ImageWidth.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_width", out hv_ImageWidth);
                hv_ImageHeight.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_height", out hv_ImageHeight);
                hv_DomainHandling.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "domain_handling", out hv_DomainHandling);
                //The keys 'ignore_direction' and 'class_ids_no_orientation' are optional.
                hv_IgnoreDirection.Dispose();
                hv_IgnoreDirection = 0;
                hv_ClassIDsNoOrientation.Dispose();
                hv_ClassIDsNoOrientation = new HTuple();
                hv_KeyExists.Dispose();
                HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", (new HTuple("ignore_direction")).TupleConcat(
                    "class_ids_no_orientation"), out hv_KeyExists);
                if ((int)(hv_KeyExists.TupleSelect(0)) != 0)
                {
                    hv_IgnoreDirection.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "ignore_direction", out hv_IgnoreDirection);
                    if ((int)(new HTuple(hv_IgnoreDirection.TupleEqual("true"))) != 0)
                    {
                        hv_IgnoreDirection.Dispose();
                        hv_IgnoreDirection = 1;
                    }
                    else if ((int)(new HTuple(hv_IgnoreDirection.TupleEqual("false"))) != 0)
                    {
                        hv_IgnoreDirection.Dispose();
                        hv_IgnoreDirection = 0;
                    }
                }
                if ((int)(hv_KeyExists.TupleSelect(1)) != 0)
                {
                    hv_ClassIDsNoOrientation.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "class_ids_no_orientation",
                        out hv_ClassIDsNoOrientation);
                }
                //
                //Get bounding box coordinates and labels.
                try
                {
                    hv_BBoxRow.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_row", out hv_BBoxRow);
                    hv_BBoxCol.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_col", out hv_BBoxCol);
                    hv_BBoxLength1.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_length1", out hv_BBoxLength1);
                    hv_BBoxLength2.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_length2", out hv_BBoxLength2);
                    hv_BBoxPhi.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_phi", out hv_BBoxPhi);
                    hv_BBoxLabel.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "bbox_label_id", out hv_BBoxLabel);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_ImageId.Dispose();
                    HOperatorSet.GetDictTuple(hv_DLSample, "image_id", out hv_ImageId);
                    if ((int)(new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1302))) != 0)
                    {
                        hv_ExceptionMessage.Dispose();
                        hv_ExceptionMessage = "A bounding box coordinate key is missing.";
                    }
                    else
                    {
                        hv_ExceptionMessage.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_ExceptionMessage = hv_Exception.TupleSelect(
                                2);
                        }
                    }
                    throw new HalconException((("An error has occurred during preprocessing image_id " + hv_ImageId) + " when getting bounding box coordinates : ") + hv_ExceptionMessage);
                }
                //
                //Check that there are no invalid boxes.
                if ((int)(new HTuple((new HTuple(hv_BBoxRow.TupleLength())).TupleGreater(0))) != 0)
                {
                    hv_BoxesInvalid.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BoxesInvalid = (((hv_BBoxLength1.TupleEqualElem(
                            0))).TupleSum()) + (((hv_BBoxLength2.TupleEqualElem(0))).TupleSum());
                    }
                    if ((int)(new HTuple(hv_BoxesInvalid.TupleGreater(0))) != 0)
                    {
                        hv_ImageId.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLSample, "image_id", out hv_ImageId);
                        throw new HalconException(("An error has occurred during preprocessing image_id " + hv_ImageId) + new HTuple(": Sample contains at least one bounding box with zero-area, i.e. bbox_length1 == 0 or bbox_length2 == 0!"));
                    }
                }
                else
                {
                    //There are no bounding boxes, hence nothing to do.
                    ho_DomainRaw.Dispose();
                    ho_Rectangle2XLD.Dispose();
                    ho_Rectangle2XLDSheared.Dispose();

                    hv_ImageWidth.Dispose();
                    hv_ImageHeight.Dispose();
                    hv_DomainHandling.Dispose();
                    hv_IgnoreDirection.Dispose();
                    hv_ClassIDsNoOrientation.Dispose();
                    hv_KeyExists.Dispose();
                    hv_BBoxRow.Dispose();
                    hv_BBoxCol.Dispose();
                    hv_BBoxLength1.Dispose();
                    hv_BBoxLength2.Dispose();
                    hv_BBoxPhi.Dispose();
                    hv_BBoxLabel.Dispose();
                    hv_Exception.Dispose();
                    hv_ImageId.Dispose();
                    hv_ExceptionMessage.Dispose();
                    hv_BoxesInvalid.Dispose();
                    hv_RowDomain1.Dispose();
                    hv_ColumnDomain1.Dispose();
                    hv_RowDomain2.Dispose();
                    hv_ColumnDomain2.Dispose();
                    hv_WidthRaw.Dispose();
                    hv_HeightRaw.Dispose();
                    hv_MaskDelete.Dispose();
                    hv_MaskNewBbox.Dispose();
                    hv_BBoxRowNew.Dispose();
                    hv_BBoxColNew.Dispose();
                    hv_BBoxLength1New.Dispose();
                    hv_BBoxLength2New.Dispose();
                    hv_BBoxPhiNew.Dispose();
                    hv_BBoxLabelNew.Dispose();
                    hv_ClassIDsNoOrientationIndices.Dispose();
                    hv_Index.Dispose();
                    hv_ClassIDsNoOrientationIndicesTmp.Dispose();
                    hv_DirectionLength1Row.Dispose();
                    hv_DirectionLength1Col.Dispose();
                    hv_DirectionLength2Row.Dispose();
                    hv_DirectionLength2Col.Dispose();
                    hv_Corner1Row.Dispose();
                    hv_Corner1Col.Dispose();
                    hv_Corner2Row.Dispose();
                    hv_Corner2Col.Dispose();
                    hv_FactorResampleWidth.Dispose();
                    hv_FactorResampleHeight.Dispose();
                    hv_BBoxCol1.Dispose();
                    hv_BBoxCol1New.Dispose();
                    hv_BBoxCol2.Dispose();
                    hv_BBoxCol2New.Dispose();
                    hv_BBoxCol3.Dispose();
                    hv_BBoxCol3New.Dispose();
                    hv_BBoxCol4.Dispose();
                    hv_BBoxCol4New.Dispose();
                    hv_BBoxRow1.Dispose();
                    hv_BBoxRow1New.Dispose();
                    hv_BBoxRow2.Dispose();
                    hv_BBoxRow2New.Dispose();
                    hv_BBoxRow3.Dispose();
                    hv_BBoxRow3New.Dispose();
                    hv_BBoxRow4.Dispose();
                    hv_BBoxRow4New.Dispose();
                    hv_HomMat2DIdentity.Dispose();
                    hv_HomMat2DScale.Dispose();
                    hv_BBoxPhiTmp.Dispose();
                    hv_PhiDelta.Dispose();
                    hv_PhiDeltaNegativeIndices.Dispose();
                    hv_IndicesRot90.Dispose();
                    hv_IndicesRot180.Dispose();
                    hv_IndicesRot270.Dispose();
                    hv_SwapIndices.Dispose();
                    hv_Tmp.Dispose();
                    hv_BBoxPhiNewIndices.Dispose();
                    hv_PhiThreshold.Dispose();
                    hv_PhiToCorrect.Dispose();
                    hv_NumCorrections.Dispose();
                    hv__.Dispose();

                    return;
                }
                //
                //If the domain is cropped, crop bounding boxes.
                if ((int)(new HTuple(hv_DomainHandling.TupleEqual("crop_domain"))) != 0)
                {
                    //
                    //Get domain.
                    ho_DomainRaw.Dispose();
                    HOperatorSet.GetDomain(ho_ImageRaw, out ho_DomainRaw);
                    //
                    //Set the size of the raw image to the domain extensions.
                    hv_RowDomain1.Dispose(); hv_ColumnDomain1.Dispose(); hv_RowDomain2.Dispose(); hv_ColumnDomain2.Dispose();
                    HOperatorSet.SmallestRectangle1(ho_DomainRaw, out hv_RowDomain1, out hv_ColumnDomain1,
                        out hv_RowDomain2, out hv_ColumnDomain2);
                    hv_WidthRaw.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_WidthRaw = (hv_ColumnDomain2 - hv_ColumnDomain1) + 1;
                    }
                    hv_HeightRaw.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_HeightRaw = (hv_RowDomain2 - hv_RowDomain1) + 1;
                    }
                    //
                    //Crop the bounding boxes.
                    //Remove the boxes with center outside of the domain.
                    hv_MaskDelete.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaskDelete = (new HTuple((new HTuple(((hv_BBoxRow.TupleLessElem(
                            hv_RowDomain1))).TupleOr(hv_BBoxCol.TupleLessElem(hv_ColumnDomain1)))).TupleOr(
                            hv_BBoxRow.TupleGreaterElem(hv_RowDomain2)))).TupleOr(hv_BBoxCol.TupleGreaterElem(
                            hv_ColumnDomain2));
                    }
                    hv_MaskNewBbox.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaskNewBbox = 1 - hv_MaskDelete;
                    }
                    //Store the preprocessed bounding box entries.
                    hv_BBoxRowNew.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxRowNew = (hv_BBoxRow.TupleSelectMask(
                            hv_MaskNewBbox)) - hv_RowDomain1;
                    }
                    hv_BBoxColNew.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxColNew = (hv_BBoxCol.TupleSelectMask(
                            hv_MaskNewBbox)) - hv_ColumnDomain1;
                    }
                    hv_BBoxLength1New.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxLength1New = hv_BBoxLength1.TupleSelectMask(
                            hv_MaskNewBbox);
                    }
                    hv_BBoxLength2New.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxLength2New = hv_BBoxLength2.TupleSelectMask(
                            hv_MaskNewBbox);
                    }
                    hv_BBoxPhiNew.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxPhiNew = hv_BBoxPhi.TupleSelectMask(
                            hv_MaskNewBbox);
                    }
                    hv_BBoxLabelNew.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BBoxLabelNew = hv_BBoxLabel.TupleSelectMask(
                            hv_MaskNewBbox);
                    }
                    //
                }
                else if ((int)(new HTuple(hv_DomainHandling.TupleEqual("full_domain"))) != 0)
                {
                    //If the entire image is used, set the variables accordingly.
                    //Get the original size.
                    hv_WidthRaw.Dispose(); hv_HeightRaw.Dispose();
                    HOperatorSet.GetImageSize(ho_ImageRaw, out hv_WidthRaw, out hv_HeightRaw);
                    //Set new coordinates to input coordinates.
                    hv_BBoxRowNew.Dispose();
                    hv_BBoxRowNew = new HTuple(hv_BBoxRow);
                    hv_BBoxColNew.Dispose();
                    hv_BBoxColNew = new HTuple(hv_BBoxCol);
                    hv_BBoxLength1New.Dispose();
                    hv_BBoxLength1New = new HTuple(hv_BBoxLength1);
                    hv_BBoxLength2New.Dispose();
                    hv_BBoxLength2New = new HTuple(hv_BBoxLength2);
                    hv_BBoxPhiNew.Dispose();
                    hv_BBoxPhiNew = new HTuple(hv_BBoxPhi);
                    hv_BBoxLabelNew.Dispose();
                    hv_BBoxLabelNew = new HTuple(hv_BBoxLabel);
                }
                else
                {
                    throw new HalconException("Unsupported parameter value for 'domain_handling'");
                }
                //
                //Generate smallest enclosing axis-aligned bounding box for classes in ClassIDsNoOrientation.
                hv_ClassIDsNoOrientationIndices.Dispose();
                hv_ClassIDsNoOrientationIndices = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ClassIDsNoOrientation.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_ClassIDsNoOrientationIndicesTmp.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ClassIDsNoOrientationIndicesTmp = ((hv_BBoxLabelNew.TupleEqualElem(
                            hv_ClassIDsNoOrientation.TupleSelect(hv_Index)))).TupleFind(1);
                    }
                    if ((int)(new HTuple(hv_ClassIDsNoOrientationIndicesTmp.TupleNotEqual(-1))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_ClassIDsNoOrientationIndices = hv_ClassIDsNoOrientationIndices.TupleConcat(
                                    hv_ClassIDsNoOrientationIndicesTmp);
                                hv_ClassIDsNoOrientationIndices.Dispose();
                                hv_ClassIDsNoOrientationIndices = ExpTmpLocalVar_ClassIDsNoOrientationIndices;
                            }
                        }
                    }
                }
                if ((int)(new HTuple((new HTuple(hv_ClassIDsNoOrientationIndices.TupleLength()
                    )).TupleGreater(0))) != 0)
                {
                    //Calculate length1 and length2 using position of corners.
                    hv_DirectionLength1Row.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_DirectionLength1Row = -(((hv_BBoxPhiNew.TupleSelect(
                            hv_ClassIDsNoOrientationIndices))).TupleSin());
                    }
                    hv_DirectionLength1Col.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_DirectionLength1Col = ((hv_BBoxPhiNew.TupleSelect(
                            hv_ClassIDsNoOrientationIndices))).TupleCos();
                    }
                    hv_DirectionLength2Row.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_DirectionLength2Row = -hv_DirectionLength1Col;
                    }
                    hv_DirectionLength2Col.Dispose();
                    hv_DirectionLength2Col = new HTuple(hv_DirectionLength1Row);
                    hv_Corner1Row.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Corner1Row = ((hv_BBoxLength1New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength1Row) + ((hv_BBoxLength2New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength2Row);
                    }
                    hv_Corner1Col.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Corner1Col = ((hv_BBoxLength1New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength1Col) + ((hv_BBoxLength2New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength2Col);
                    }
                    hv_Corner2Row.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Corner2Row = ((hv_BBoxLength1New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength1Row) - ((hv_BBoxLength2New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength2Row);
                    }
                    hv_Corner2Col.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Corner2Col = ((hv_BBoxLength1New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength1Col) - ((hv_BBoxLength2New.TupleSelect(
                            hv_ClassIDsNoOrientationIndices)) * hv_DirectionLength2Col);
                    }
                    //
                    if (hv_BBoxPhiNew == null)
                        hv_BBoxPhiNew = new HTuple();
                    hv_BBoxPhiNew[hv_ClassIDsNoOrientationIndices] = 0.0;
                    if (hv_BBoxLength1New == null)
                        hv_BBoxLength1New = new HTuple();
                    hv_BBoxLength1New[hv_ClassIDsNoOrientationIndices] = ((hv_Corner1Col.TupleAbs()
                        )).TupleMax2(hv_Corner2Col.TupleAbs());
                    if (hv_BBoxLength2New == null)
                        hv_BBoxLength2New = new HTuple();
                    hv_BBoxLength2New[hv_ClassIDsNoOrientationIndices] = ((hv_Corner1Row.TupleAbs()
                        )).TupleMax2(hv_Corner2Row.TupleAbs());
                }
                //
                //Rescale bounding boxes.
                //
                //Get required images width and height.
                //
                //Only rescale bounding boxes if the required image dimensions are not the raw dimensions.
                if ((int)((new HTuple(hv_ImageHeight.TupleNotEqual(hv_HeightRaw))).TupleOr(
                    new HTuple(hv_ImageWidth.TupleNotEqual(hv_WidthRaw)))) != 0)
                {
                    //Calculate rescaling factor.
                    hv_FactorResampleWidth.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_FactorResampleWidth = (hv_ImageWidth.TupleReal()
                            ) / hv_WidthRaw;
                    }
                    hv_FactorResampleHeight.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_FactorResampleHeight = (hv_ImageHeight.TupleReal()
                            ) / hv_HeightRaw;
                    }
                    if ((int)(new HTuple(hv_FactorResampleHeight.TupleNotEqual(hv_FactorResampleWidth))) != 0)
                    {
                        //In order to preserve the correct orientation we have to transform the points individually.
                        //Get the coordinates of the four corner points.
                        hv_BBoxRow1.Dispose(); hv_BBoxCol1.Dispose(); hv_BBoxRow2.Dispose(); hv_BBoxCol2.Dispose(); hv_BBoxRow3.Dispose(); hv_BBoxCol3.Dispose(); hv_BBoxRow4.Dispose(); hv_BBoxCol4.Dispose();
                        convert_rect2_5to8param(hv_BBoxRowNew, hv_BBoxColNew, hv_BBoxLength1New,
                            hv_BBoxLength2New, hv_BBoxPhiNew, out hv_BBoxRow1, out hv_BBoxCol1,
                            out hv_BBoxRow2, out hv_BBoxCol2, out hv_BBoxRow3, out hv_BBoxCol3,
                            out hv_BBoxRow4, out hv_BBoxCol4);
                        //
                        //Rescale the coordinates.
                        hv_BBoxCol1New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxCol1New = hv_BBoxCol1 * hv_FactorResampleWidth;
                        }
                        hv_BBoxCol2New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxCol2New = hv_BBoxCol2 * hv_FactorResampleWidth;
                        }
                        hv_BBoxCol3New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxCol3New = hv_BBoxCol3 * hv_FactorResampleWidth;
                        }
                        hv_BBoxCol4New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxCol4New = hv_BBoxCol4 * hv_FactorResampleWidth;
                        }
                        hv_BBoxRow1New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxRow1New = hv_BBoxRow1 * hv_FactorResampleHeight;
                        }
                        hv_BBoxRow2New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxRow2New = hv_BBoxRow2 * hv_FactorResampleHeight;
                        }
                        hv_BBoxRow3New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxRow3New = hv_BBoxRow3 * hv_FactorResampleHeight;
                        }
                        hv_BBoxRow4New.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxRow4New = hv_BBoxRow4 * hv_FactorResampleHeight;
                        }
                        //
                        //The rectangles will get sheared, that is why new rectangles have to be found.
                        //Generate homography to scale rectangles.
                        hv_HomMat2DIdentity.Dispose();
                        HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                        hv_HomMat2DScale.Dispose();
                        HOperatorSet.HomMat2dScale(hv_HomMat2DIdentity, hv_FactorResampleHeight,
                            hv_FactorResampleWidth, 0, 0, out hv_HomMat2DScale);
                        //Generate XLD contours for the rectangles.
                        ho_Rectangle2XLD.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle2XLD, hv_BBoxRowNew,
                            hv_BBoxColNew, hv_BBoxPhiNew, hv_BBoxLength1New, hv_BBoxLength2New);
                        //Scale the XLD contours --> results in sheared regions.
                        ho_Rectangle2XLDSheared.Dispose();
                        HOperatorSet.AffineTransContourXld(ho_Rectangle2XLD, out ho_Rectangle2XLDSheared,
                            hv_HomMat2DScale);
                        hv_BBoxRowNew.Dispose(); hv_BBoxColNew.Dispose(); hv_BBoxPhiNew.Dispose(); hv_BBoxLength1New.Dispose(); hv_BBoxLength2New.Dispose();
                        HOperatorSet.SmallestRectangle2Xld(ho_Rectangle2XLDSheared, out hv_BBoxRowNew,
                            out hv_BBoxColNew, out hv_BBoxPhiNew, out hv_BBoxLength1New, out hv_BBoxLength2New);
                        //
                        //smallest_rectangle2_xld might change the orientation of the bounding box.
                        //Hence, take the orientation that is closest to the one obtained out of the 4 corner points.
                        hv__.Dispose(); hv__.Dispose(); hv__.Dispose(); hv__.Dispose(); hv_BBoxPhiTmp.Dispose();
                        convert_rect2_8to5param(hv_BBoxRow1New, hv_BBoxCol1New, hv_BBoxRow2New,
                            hv_BBoxCol2New, hv_BBoxRow3New, hv_BBoxCol3New, hv_BBoxRow4New, hv_BBoxCol4New,
                            hv_IgnoreDirection, out hv__, out hv__, out hv__, out hv__, out hv_BBoxPhiTmp);
                        hv_PhiDelta.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_PhiDelta = ((hv_BBoxPhiTmp - hv_BBoxPhiNew)).TupleFmod(
                                (new HTuple(360)).TupleRad());
                        }
                        //Guarantee that angles are positive.
                        hv_PhiDeltaNegativeIndices.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_PhiDeltaNegativeIndices = ((hv_PhiDelta.TupleLessElem(
                                0.0))).TupleFind(1);
                        }
                        if ((int)(new HTuple(hv_PhiDeltaNegativeIndices.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_PhiDelta == null)
                                hv_PhiDelta = new HTuple();
                            hv_PhiDelta[hv_PhiDeltaNegativeIndices] = (hv_PhiDelta.TupleSelect(hv_PhiDeltaNegativeIndices)) + ((new HTuple(360)).TupleRad()
                                );
                        }
                        hv_IndicesRot90.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_IndicesRot90 = (new HTuple(((hv_PhiDelta.TupleGreaterElem(
                                (new HTuple(45)).TupleRad()))).TupleAnd(hv_PhiDelta.TupleLessEqualElem(
                                (new HTuple(135)).TupleRad())))).TupleFind(1);
                        }
                        hv_IndicesRot180.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_IndicesRot180 = (new HTuple(((hv_PhiDelta.TupleGreaterElem(
                                (new HTuple(135)).TupleRad()))).TupleAnd(hv_PhiDelta.TupleLessEqualElem(
                                (new HTuple(225)).TupleRad())))).TupleFind(1);
                        }
                        hv_IndicesRot270.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_IndicesRot270 = (new HTuple(((hv_PhiDelta.TupleGreaterElem(
                                (new HTuple(225)).TupleRad()))).TupleAnd(hv_PhiDelta.TupleLessEqualElem(
                                (new HTuple(315)).TupleRad())))).TupleFind(1);
                        }
                        hv_SwapIndices.Dispose();
                        hv_SwapIndices = new HTuple();
                        if ((int)(new HTuple(hv_IndicesRot90.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_BBoxPhiNew == null)
                                hv_BBoxPhiNew = new HTuple();
                            hv_BBoxPhiNew[hv_IndicesRot90] = (hv_BBoxPhiNew.TupleSelect(hv_IndicesRot90)) + ((new HTuple(90)).TupleRad()
                                );
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                {
                                    HTuple
                                      ExpTmpLocalVar_SwapIndices = hv_SwapIndices.TupleConcat(
                                        hv_IndicesRot90);
                                    hv_SwapIndices.Dispose();
                                    hv_SwapIndices = ExpTmpLocalVar_SwapIndices;
                                }
                            }
                        }
                        if ((int)(new HTuple(hv_IndicesRot180.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_BBoxPhiNew == null)
                                hv_BBoxPhiNew = new HTuple();
                            hv_BBoxPhiNew[hv_IndicesRot180] = (hv_BBoxPhiNew.TupleSelect(hv_IndicesRot180)) + ((new HTuple(180)).TupleRad()
                                );
                        }
                        if ((int)(new HTuple(hv_IndicesRot270.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_BBoxPhiNew == null)
                                hv_BBoxPhiNew = new HTuple();
                            hv_BBoxPhiNew[hv_IndicesRot270] = (hv_BBoxPhiNew.TupleSelect(hv_IndicesRot270)) + ((new HTuple(270)).TupleRad()
                                );
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                {
                                    HTuple
                                      ExpTmpLocalVar_SwapIndices = hv_SwapIndices.TupleConcat(
                                        hv_IndicesRot270);
                                    hv_SwapIndices.Dispose();
                                    hv_SwapIndices = ExpTmpLocalVar_SwapIndices;
                                }
                            }
                        }
                        if ((int)(new HTuple(hv_SwapIndices.TupleNotEqual(new HTuple()))) != 0)
                        {
                            hv_Tmp.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Tmp = hv_BBoxLength1New.TupleSelect(
                                    hv_SwapIndices);
                            }
                            if (hv_BBoxLength1New == null)
                                hv_BBoxLength1New = new HTuple();
                            hv_BBoxLength1New[hv_SwapIndices] = hv_BBoxLength2New.TupleSelect(hv_SwapIndices);
                            if (hv_BBoxLength2New == null)
                                hv_BBoxLength2New = new HTuple();
                            hv_BBoxLength2New[hv_SwapIndices] = hv_Tmp;
                        }
                        //Change angles such that they lie in the range (-180°, 180°].
                        hv_BBoxPhiNewIndices.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_BBoxPhiNewIndices = ((hv_BBoxPhiNew.TupleGreaterElem(
                                (new HTuple(180)).TupleRad()))).TupleFind(1);
                        }
                        if ((int)(new HTuple(hv_BBoxPhiNewIndices.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_BBoxPhiNew == null)
                                hv_BBoxPhiNew = new HTuple();
                            hv_BBoxPhiNew[hv_BBoxPhiNewIndices] = (hv_BBoxPhiNew.TupleSelect(hv_BBoxPhiNewIndices)) - ((new HTuple(360)).TupleRad()
                                );
                        }
                        //
                    }
                    else
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_BBoxColNew = hv_BBoxColNew * hv_FactorResampleWidth;
                                hv_BBoxColNew.Dispose();
                                hv_BBoxColNew = ExpTmpLocalVar_BBoxColNew;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_BBoxRowNew = hv_BBoxRowNew * hv_FactorResampleWidth;
                                hv_BBoxRowNew.Dispose();
                                hv_BBoxRowNew = ExpTmpLocalVar_BBoxRowNew;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_BBoxLength1New = hv_BBoxLength1New * hv_FactorResampleWidth;
                                hv_BBoxLength1New.Dispose();
                                hv_BBoxLength1New = ExpTmpLocalVar_BBoxLength1New;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_BBoxLength2New = hv_BBoxLength2New * hv_FactorResampleWidth;
                                hv_BBoxLength2New.Dispose();
                                hv_BBoxLength2New = ExpTmpLocalVar_BBoxLength2New;
                            }
                        }
                        //Phi stays the same.
                    }
                    //
                }
                //
                //Adapt the bounding box angles such that they are within the correct range,
                //which is (-180°,180°] for 'ignore_direction'==false and (-90°,90°] else.
                hv_PhiThreshold.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_PhiThreshold = ((new HTuple(180)).TupleRad()
                        ) - (hv_IgnoreDirection * ((new HTuple(90)).TupleRad()));
                }
                hv_PhiDelta.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_PhiDelta = 2 * hv_PhiThreshold;
                }
                //Correct angles that are too large.
                hv_PhiToCorrect.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_PhiToCorrect = ((hv_BBoxPhiNew.TupleGreaterElem(
                        hv_PhiThreshold))).TupleFind(1);
                }
                if ((int)((new HTuple(hv_PhiToCorrect.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_PhiToCorrect.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    hv_NumCorrections.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_NumCorrections = (((((hv_BBoxPhiNew.TupleSelect(
                            hv_PhiToCorrect)) - hv_PhiThreshold) / hv_PhiDelta)).TupleInt()) + 1;
                    }
                    if (hv_BBoxPhiNew == null)
                        hv_BBoxPhiNew = new HTuple();
                    hv_BBoxPhiNew[hv_PhiToCorrect] = (hv_BBoxPhiNew.TupleSelect(hv_PhiToCorrect)) - (hv_NumCorrections * hv_PhiDelta);
                }
                //Correct angles that are too small.
                hv_PhiToCorrect.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_PhiToCorrect = ((hv_BBoxPhiNew.TupleLessEqualElem(
                        -hv_PhiThreshold))).TupleFind(1);
                }
                if ((int)((new HTuple(hv_PhiToCorrect.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_PhiToCorrect.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    hv_NumCorrections.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_NumCorrections = (((((((hv_BBoxPhiNew.TupleSelect(
                            hv_PhiToCorrect)) + hv_PhiThreshold)).TupleAbs()) / hv_PhiDelta)).TupleInt()
                            ) + 1;
                    }
                    if (hv_BBoxPhiNew == null)
                        hv_BBoxPhiNew = new HTuple();
                    hv_BBoxPhiNew[hv_PhiToCorrect] = (hv_BBoxPhiNew.TupleSelect(hv_PhiToCorrect)) + (hv_NumCorrections * hv_PhiDelta);
                }
                //
                //Check that there are no invalid boxes.
                if ((int)(new HTuple((new HTuple(hv_BBoxRowNew.TupleLength())).TupleGreater(
                    0))) != 0)
                {
                    hv_BoxesInvalid.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_BoxesInvalid = (((hv_BBoxLength1New.TupleEqualElem(
                            0))).TupleSum()) + (((hv_BBoxLength2New.TupleEqualElem(0))).TupleSum());
                    }
                    if ((int)(new HTuple(hv_BoxesInvalid.TupleGreater(0))) != 0)
                    {
                        hv_ImageId.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLSample, "image_id", out hv_ImageId);
                        throw new HalconException(("An error has occurred during preprocessing image_id " + hv_ImageId) + new HTuple(": Sample contains at least one box with zero-area, i.e. bbox_length1 == 0 or bbox_length2 == 0!"));
                    }
                }
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_row", hv_BBoxRowNew);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_col", hv_BBoxColNew);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_length1", hv_BBoxLength1New);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_length2", hv_BBoxLength2New);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_phi", hv_BBoxPhiNew);
                HOperatorSet.SetDictTuple(hv_DLSample, "bbox_label_id", hv_BBoxLabelNew);
                //
                ho_DomainRaw.Dispose();
                ho_Rectangle2XLD.Dispose();
                ho_Rectangle2XLDSheared.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_DomainHandling.Dispose();
                hv_IgnoreDirection.Dispose();
                hv_ClassIDsNoOrientation.Dispose();
                hv_KeyExists.Dispose();
                hv_BBoxRow.Dispose();
                hv_BBoxCol.Dispose();
                hv_BBoxLength1.Dispose();
                hv_BBoxLength2.Dispose();
                hv_BBoxPhi.Dispose();
                hv_BBoxLabel.Dispose();
                hv_Exception.Dispose();
                hv_ImageId.Dispose();
                hv_ExceptionMessage.Dispose();
                hv_BoxesInvalid.Dispose();
                hv_RowDomain1.Dispose();
                hv_ColumnDomain1.Dispose();
                hv_RowDomain2.Dispose();
                hv_ColumnDomain2.Dispose();
                hv_WidthRaw.Dispose();
                hv_HeightRaw.Dispose();
                hv_MaskDelete.Dispose();
                hv_MaskNewBbox.Dispose();
                hv_BBoxRowNew.Dispose();
                hv_BBoxColNew.Dispose();
                hv_BBoxLength1New.Dispose();
                hv_BBoxLength2New.Dispose();
                hv_BBoxPhiNew.Dispose();
                hv_BBoxLabelNew.Dispose();
                hv_ClassIDsNoOrientationIndices.Dispose();
                hv_Index.Dispose();
                hv_ClassIDsNoOrientationIndicesTmp.Dispose();
                hv_DirectionLength1Row.Dispose();
                hv_DirectionLength1Col.Dispose();
                hv_DirectionLength2Row.Dispose();
                hv_DirectionLength2Col.Dispose();
                hv_Corner1Row.Dispose();
                hv_Corner1Col.Dispose();
                hv_Corner2Row.Dispose();
                hv_Corner2Col.Dispose();
                hv_FactorResampleWidth.Dispose();
                hv_FactorResampleHeight.Dispose();
                hv_BBoxCol1.Dispose();
                hv_BBoxCol1New.Dispose();
                hv_BBoxCol2.Dispose();
                hv_BBoxCol2New.Dispose();
                hv_BBoxCol3.Dispose();
                hv_BBoxCol3New.Dispose();
                hv_BBoxCol4.Dispose();
                hv_BBoxCol4New.Dispose();
                hv_BBoxRow1.Dispose();
                hv_BBoxRow1New.Dispose();
                hv_BBoxRow2.Dispose();
                hv_BBoxRow2New.Dispose();
                hv_BBoxRow3.Dispose();
                hv_BBoxRow3New.Dispose();
                hv_BBoxRow4.Dispose();
                hv_BBoxRow4New.Dispose();
                hv_HomMat2DIdentity.Dispose();
                hv_HomMat2DScale.Dispose();
                hv_BBoxPhiTmp.Dispose();
                hv_PhiDelta.Dispose();
                hv_PhiDeltaNegativeIndices.Dispose();
                hv_IndicesRot90.Dispose();
                hv_IndicesRot180.Dispose();
                hv_IndicesRot270.Dispose();
                hv_SwapIndices.Dispose();
                hv_Tmp.Dispose();
                hv_BBoxPhiNewIndices.Dispose();
                hv_PhiThreshold.Dispose();
                hv_PhiToCorrect.Dispose();
                hv_NumCorrections.Dispose();
                hv__.Dispose();

                return;

            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_DomainRaw.Dispose();
                ho_Rectangle2XLD.Dispose();
                ho_Rectangle2XLDSheared.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_DomainHandling.Dispose();
                hv_IgnoreDirection.Dispose();
                hv_ClassIDsNoOrientation.Dispose();
                hv_KeyExists.Dispose();
                hv_BBoxRow.Dispose();
                hv_BBoxCol.Dispose();
                hv_BBoxLength1.Dispose();
                hv_BBoxLength2.Dispose();
                hv_BBoxPhi.Dispose();
                hv_BBoxLabel.Dispose();
                hv_Exception.Dispose();
                hv_ImageId.Dispose();
                hv_ExceptionMessage.Dispose();
                hv_BoxesInvalid.Dispose();
                hv_RowDomain1.Dispose();
                hv_ColumnDomain1.Dispose();
                hv_RowDomain2.Dispose();
                hv_ColumnDomain2.Dispose();
                hv_WidthRaw.Dispose();
                hv_HeightRaw.Dispose();
                hv_MaskDelete.Dispose();
                hv_MaskNewBbox.Dispose();
                hv_BBoxRowNew.Dispose();
                hv_BBoxColNew.Dispose();
                hv_BBoxLength1New.Dispose();
                hv_BBoxLength2New.Dispose();
                hv_BBoxPhiNew.Dispose();
                hv_BBoxLabelNew.Dispose();
                hv_ClassIDsNoOrientationIndices.Dispose();
                hv_Index.Dispose();
                hv_ClassIDsNoOrientationIndicesTmp.Dispose();
                hv_DirectionLength1Row.Dispose();
                hv_DirectionLength1Col.Dispose();
                hv_DirectionLength2Row.Dispose();
                hv_DirectionLength2Col.Dispose();
                hv_Corner1Row.Dispose();
                hv_Corner1Col.Dispose();
                hv_Corner2Row.Dispose();
                hv_Corner2Col.Dispose();
                hv_FactorResampleWidth.Dispose();
                hv_FactorResampleHeight.Dispose();
                hv_BBoxCol1.Dispose();
                hv_BBoxCol1New.Dispose();
                hv_BBoxCol2.Dispose();
                hv_BBoxCol2New.Dispose();
                hv_BBoxCol3.Dispose();
                hv_BBoxCol3New.Dispose();
                hv_BBoxCol4.Dispose();
                hv_BBoxCol4New.Dispose();
                hv_BBoxRow1.Dispose();
                hv_BBoxRow1New.Dispose();
                hv_BBoxRow2.Dispose();
                hv_BBoxRow2New.Dispose();
                hv_BBoxRow3.Dispose();
                hv_BBoxRow3New.Dispose();
                hv_BBoxRow4.Dispose();
                hv_BBoxRow4New.Dispose();
                hv_HomMat2DIdentity.Dispose();
                hv_HomMat2DScale.Dispose();
                hv_BBoxPhiTmp.Dispose();
                hv_PhiDelta.Dispose();
                hv_PhiDeltaNegativeIndices.Dispose();
                hv_IndicesRot90.Dispose();
                hv_IndicesRot180.Dispose();
                hv_IndicesRot270.Dispose();
                hv_SwapIndices.Dispose();
                hv_Tmp.Dispose();
                hv_BBoxPhiNewIndices.Dispose();
                hv_PhiThreshold.Dispose();
                hv_PhiToCorrect.Dispose();
                hv_NumCorrections.Dispose();
                hv__.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Model
        // Short Description: Preprocess images for deep-learning-based training and inference. 
        public void preprocess_dl_model_images(HObject ho_Images, out HObject ho_ImagesPreprocessed,
            HTuple hv_DLPreprocessParam)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImagesScaled = null, ho_ImageSelected = null;
            HObject ho_ImageScaled = null, ho_Channel = null, ho_ChannelScaled = null;
            HObject ho_ThreeChannelImage = null, ho_SingleChannelImage = null;

            // Local copy input parameter variables 
            HObject ho_Images_COPY_INP_TMP;
            ho_Images_COPY_INP_TMP = new HObject(ho_Images);



            // Local control variables 

            HTuple hv_ImageWidth = new HTuple(), hv_ImageHeight = new HTuple();
            HTuple hv_ImageNumChannels = new HTuple(), hv_ImageRangeMin = new HTuple();
            HTuple hv_ImageRangeMax = new HTuple(), hv_DomainHandling = new HTuple();
            HTuple hv_NormalizationType = new HTuple(), hv_ModelType = new HTuple();
            HTuple hv_NumImages = new HTuple(), hv_Type = new HTuple();
            HTuple hv_NumMatches = new HTuple(), hv_InputNumChannels = new HTuple();
            HTuple hv_OutputNumChannels = new HTuple(), hv_NumChannels1 = new HTuple();
            HTuple hv_NumChannels3 = new HTuple(), hv_AreInputNumChannels1 = new HTuple();
            HTuple hv_AreInputNumChannels3 = new HTuple(), hv_AreInputNumChannels1Or3 = new HTuple();
            HTuple hv_ValidNumChannels = new HTuple(), hv_ValidNumChannelsText = new HTuple();
            HTuple hv_ImageIndex = new HTuple(), hv_NumChannels = new HTuple();
            HTuple hv_ChannelIndex = new HTuple(), hv_Min = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Shift = new HTuple();
            HTuple hv_MeanValues = new HTuple(), hv_DeviationValues = new HTuple();
            HTuple hv_UseDefaultNormalizationValues = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_RescaleRange = new HTuple(), hv_CurrentNumChannels = new HTuple();
            HTuple hv_DiffNumChannelsIndices = new HTuple(), hv_Index = new HTuple();
            HTuple hv_DiffNumChannelsIndex = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImagesPreprocessed);
            HOperatorSet.GenEmptyObj(out ho_ImagesScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_Channel);
            HOperatorSet.GenEmptyObj(out ho_ChannelScaled);
            HOperatorSet.GenEmptyObj(out ho_ThreeChannelImage);
            HOperatorSet.GenEmptyObj(out ho_SingleChannelImage);
            try
            {
                //
                //This procedure preprocesses the provided Images according to the parameters in
                //the dictionary DLPreprocessParam. Note that depending on the images, additional
                //preprocessing steps might be beneficial.
                //
                //Validate the preprocessing parameters.
                check_dl_preprocess_param(hv_DLPreprocessParam);
                //
                //Get the preprocessing parameters.
                hv_ImageWidth.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_width", out hv_ImageWidth);
                hv_ImageHeight.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_height", out hv_ImageHeight);
                hv_ImageNumChannels.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_num_channels", out hv_ImageNumChannels);
                hv_ImageRangeMin.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_min", out hv_ImageRangeMin);
                hv_ImageRangeMax.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_max", out hv_ImageRangeMax);
                hv_DomainHandling.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "domain_handling", out hv_DomainHandling);
                hv_NormalizationType.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "normalization_type", out hv_NormalizationType);
                hv_ModelType.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "model_type", out hv_ModelType);
                //
                //Validate the type of the input images.
                hv_NumImages.Dispose();
                HOperatorSet.CountObj(ho_Images_COPY_INP_TMP, out hv_NumImages);
                if ((int)(new HTuple(hv_NumImages.TupleEqual(0))) != 0)
                {
                    throw new HalconException("Please provide some images to preprocess.");
                }
                hv_Type.Dispose();
                HOperatorSet.GetImageType(ho_Images_COPY_INP_TMP, out hv_Type);
                hv_NumMatches.Dispose();
                HOperatorSet.TupleRegexpTest(hv_Type, "byte|int|real", out hv_NumMatches);
                if ((int)(new HTuple(hv_NumMatches.TupleNotEqual(hv_NumImages))) != 0)
                {
                    throw new HalconException(new HTuple("Please provide only images of type 'byte', 'int1', 'int2', 'uint2', 'int4', or 'real'."));
                }
                //
                //Validate the number channels of the input images.
                hv_InputNumChannels.Dispose();
                HOperatorSet.CountChannels(ho_Images_COPY_INP_TMP, out hv_InputNumChannels);
                hv_OutputNumChannels.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_OutputNumChannels = HTuple.TupleGenConst(
                        hv_NumImages, hv_ImageNumChannels);
                }
                //Only for 'image_num_channels' 1 and 3 combinations of 1- and 3-channel images are allowed.
                if ((int)((new HTuple(hv_ImageNumChannels.TupleEqual(1))).TupleOr(new HTuple(hv_ImageNumChannels.TupleEqual(
                    3)))) != 0)
                {
                    hv_NumChannels1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_NumChannels1 = HTuple.TupleGenConst(
                            hv_NumImages, 1);
                    }
                    hv_NumChannels3.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_NumChannels3 = HTuple.TupleGenConst(
                            hv_NumImages, 3);
                    }
                    hv_AreInputNumChannels1.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_AreInputNumChannels1 = hv_InputNumChannels.TupleEqualElem(
                            hv_NumChannels1);
                    }
                    hv_AreInputNumChannels3.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_AreInputNumChannels3 = hv_InputNumChannels.TupleEqualElem(
                            hv_NumChannels3);
                    }
                    hv_AreInputNumChannels1Or3.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_AreInputNumChannels1Or3 = hv_AreInputNumChannels1 + hv_AreInputNumChannels3;
                    }
                    hv_ValidNumChannels.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ValidNumChannels = new HTuple(hv_AreInputNumChannels1Or3.TupleEqual(
                            hv_NumChannels1));
                    }
                    hv_ValidNumChannelsText.Dispose();
                    hv_ValidNumChannelsText = "Valid numbers of channels for the specified model are 1 or 3.";
                }
                else
                {
                    hv_ValidNumChannels.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ValidNumChannels = new HTuple(hv_InputNumChannels.TupleEqual(
                            hv_OutputNumChannels));
                    }
                    hv_ValidNumChannelsText.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ValidNumChannelsText = ("Valid number of channels for the specified model is " + hv_ImageNumChannels) + ".";
                    }
                }
                if ((int)(hv_ValidNumChannels.TupleNot()) != 0)
                {
                    throw new HalconException("Please provide images with a valid number of channels. " + hv_ValidNumChannelsText);
                }
                //Preprocess the images.
                //
                //Apply the domain to the images.
                if ((int)(new HTuple(hv_DomainHandling.TupleEqual("full_domain"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.FullDomain(ho_Images_COPY_INP_TMP, out ExpTmpOutVar_0);
                        ho_Images_COPY_INP_TMP.Dispose();
                        ho_Images_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                else if ((int)(new HTuple(hv_DomainHandling.TupleEqual("crop_domain"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.CropDomain(ho_Images_COPY_INP_TMP, out ExpTmpOutVar_0);
                        ho_Images_COPY_INP_TMP.Dispose();
                        ho_Images_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                else if ((int)((new HTuple(hv_DomainHandling.TupleEqual("keep_domain"))).TupleAnd(
                    new HTuple(hv_ModelType.TupleEqual("anomaly_detection")))) != 0)
                {
                    //Anomaly detection models accept the additional option 'keep_domain'.
                }
                else
                {
                    throw new HalconException("Unsupported parameter value for 'domain_handling'.");
                }
                //
                //Convert the images to real and zoom the images.
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConvertImageType(ho_Images_COPY_INP_TMP, out ExpTmpOutVar_0, "real");
                    ho_Images_COPY_INP_TMP.Dispose();
                    ho_Images_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ZoomImageSize(ho_Images_COPY_INP_TMP, out ExpTmpOutVar_0, hv_ImageWidth,
                        hv_ImageHeight, "constant");
                    ho_Images_COPY_INP_TMP.Dispose();
                    ho_Images_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                //
                if ((int)(new HTuple(hv_NormalizationType.TupleEqual("all_channels"))) != 0)
                {
                    //Scale for each image the gray values of all channels to ImageRangeMin-ImageRangeMax.
                    ho_ImagesScaled.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ImagesScaled);
                    HTuple end_val68 = hv_NumImages;
                    HTuple step_val68 = 1;
                    for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val68, step_val68); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val68))
                    {
                        ho_ImageSelected.Dispose();
                        HOperatorSet.SelectObj(ho_Images_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                        hv_NumChannels.Dispose();
                        HOperatorSet.CountChannels(ho_ImageSelected, out hv_NumChannels);
                        ho_ImageScaled.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_ImageScaled);
                        HTuple end_val72 = hv_NumChannels;
                        HTuple step_val72 = 1;
                        for (hv_ChannelIndex = 1; hv_ChannelIndex.Continue(end_val72, step_val72); hv_ChannelIndex = hv_ChannelIndex.TupleAdd(step_val72))
                        {
                            ho_Channel.Dispose();
                            HOperatorSet.AccessChannel(ho_ImageSelected, out ho_Channel, hv_ChannelIndex);
                            hv_Min.Dispose(); hv_Max.Dispose(); hv_Range.Dispose();
                            HOperatorSet.MinMaxGray(ho_Channel, ho_Channel, 0, out hv_Min, out hv_Max,
                                out hv_Range);
                            if ((int)(new HTuple(((hv_Max - hv_Min)).TupleEqual(0))) != 0)
                            {
                                hv_Scale.Dispose();
                                hv_Scale = 1;
                            }
                            else
                            {
                                hv_Scale.Dispose();
                                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                {
                                    hv_Scale = (hv_ImageRangeMax - hv_ImageRangeMin) / (hv_Max - hv_Min);
                                }
                            }
                            hv_Shift.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Shift = ((-hv_Scale) * hv_Min) + hv_ImageRangeMin;
                            }
                            ho_ChannelScaled.Dispose();
                            HOperatorSet.ScaleImage(ho_Channel, out ho_ChannelScaled, hv_Scale, hv_Shift);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.AppendChannel(ho_ImageScaled, ho_ChannelScaled, out ExpTmpOutVar_0
                                    );
                                ho_ImageScaled.Dispose();
                                ho_ImageScaled = ExpTmpOutVar_0;
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ImagesScaled, ho_ImageScaled, out ExpTmpOutVar_0
                                );
                            ho_ImagesScaled.Dispose();
                            ho_ImagesScaled = ExpTmpOutVar_0;
                        }
                    }
                    ho_Images_COPY_INP_TMP.Dispose();
                    ho_Images_COPY_INP_TMP = new HObject(ho_ImagesScaled);
                }
                else if ((int)(new HTuple(hv_NormalizationType.TupleEqual("first_channel"))) != 0)
                {
                    //Scale for each image the gray values of first channel to ImageRangeMin-ImageRangeMax.
                    ho_ImagesScaled.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ImagesScaled);
                    HTuple end_val90 = hv_NumImages;
                    HTuple step_val90 = 1;
                    for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val90, step_val90); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val90))
                    {
                        ho_ImageSelected.Dispose();
                        HOperatorSet.SelectObj(ho_Images_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                        hv_Min.Dispose(); hv_Max.Dispose(); hv_Range.Dispose();
                        HOperatorSet.MinMaxGray(ho_ImageSelected, ho_ImageSelected, 0, out hv_Min,
                            out hv_Max, out hv_Range);
                        if ((int)(new HTuple(((hv_Max - hv_Min)).TupleEqual(0))) != 0)
                        {
                            hv_Scale.Dispose();
                            hv_Scale = 1;
                        }
                        else
                        {
                            hv_Scale.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Scale = (hv_ImageRangeMax - hv_ImageRangeMin) / (hv_Max - hv_Min);
                            }
                        }
                        hv_Shift.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Shift = ((-hv_Scale) * hv_Min) + hv_ImageRangeMin;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ScaleImage(ho_ImageSelected, out ExpTmpOutVar_0, hv_Scale,
                                hv_Shift);
                            ho_ImageSelected.Dispose();
                            ho_ImageSelected = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ImagesScaled, ho_ImageSelected, out ExpTmpOutVar_0
                                );
                            ho_ImagesScaled.Dispose();
                            ho_ImagesScaled = ExpTmpOutVar_0;
                        }
                    }
                    ho_Images_COPY_INP_TMP.Dispose();
                    ho_Images_COPY_INP_TMP = new HObject(ho_ImagesScaled);
                }
                else if ((int)(new HTuple(hv_NormalizationType.TupleEqual("constant_values"))) != 0)
                {
                    //Scale for each image the gray values of all channels to the corresponding channel DeviationValues[].
                    try
                    {
                        hv_MeanValues.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "mean_values_normalization",
                            out hv_MeanValues);
                        hv_DeviationValues.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "deviation_values_normalization",
                            out hv_DeviationValues);
                        hv_UseDefaultNormalizationValues.Dispose();
                        hv_UseDefaultNormalizationValues = 0;
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_MeanValues.Dispose();
                        hv_MeanValues = new HTuple();
                        hv_MeanValues[0] = 123.675;
                        hv_MeanValues[1] = 116.28;
                        hv_MeanValues[2] = 103.53;
                        hv_DeviationValues.Dispose();
                        hv_DeviationValues = new HTuple();
                        hv_DeviationValues[0] = 58.395;
                        hv_DeviationValues[1] = 57.12;
                        hv_DeviationValues[2] = 57.375;
                        hv_UseDefaultNormalizationValues.Dispose();
                        hv_UseDefaultNormalizationValues = 1;
                    }
                    ho_ImagesScaled.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_ImagesScaled);
                    HTuple end_val115 = hv_NumImages;
                    HTuple step_val115 = 1;
                    for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val115, step_val115); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val115))
                    {
                        ho_ImageSelected.Dispose();
                        HOperatorSet.SelectObj(ho_Images_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                        hv_NumChannels.Dispose();
                        HOperatorSet.CountChannels(ho_ImageSelected, out hv_NumChannels);
                        //Ensure that the number of channels is equal |DeviationValues| and |MeanValues|
                        if ((int)(hv_UseDefaultNormalizationValues) != 0)
                        {
                            if ((int)(new HTuple(hv_NumChannels.TupleEqual(1))) != 0)
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.Compose3(ho_ImageSelected, ho_ImageSelected, ho_ImageSelected,
                                        out ExpTmpOutVar_0);
                                    ho_ImageSelected.Dispose();
                                    ho_ImageSelected = ExpTmpOutVar_0;
                                }
                                hv_NumChannels.Dispose();
                                HOperatorSet.CountChannels(ho_ImageSelected, out hv_NumChannels);
                            }
                            else if ((int)(new HTuple(hv_NumChannels.TupleNotEqual(
                                3))) != 0)
                            {
                                throw new HalconException("Using default values for normalization type 'constant_values' is allowed only for 1- and 3-channel images.");
                            }
                        }
                        if ((int)((new HTuple((new HTuple(hv_MeanValues.TupleLength())).TupleNotEqual(
                            hv_NumChannels))).TupleOr(new HTuple((new HTuple(hv_DeviationValues.TupleLength()
                            )).TupleNotEqual(hv_NumChannels)))) != 0)
                        {
                            throw new HalconException("The length of mean and deviation values for normalization type 'constant_values' have to be the same size as the number of channels of the image.");
                        }
                        ho_ImageScaled.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_ImageScaled);
                        HTuple end_val131 = hv_NumChannels;
                        HTuple step_val131 = 1;
                        for (hv_ChannelIndex = 1; hv_ChannelIndex.Continue(end_val131, step_val131); hv_ChannelIndex = hv_ChannelIndex.TupleAdd(step_val131))
                        {
                            ho_Channel.Dispose();
                            HOperatorSet.AccessChannel(ho_ImageSelected, out ho_Channel, hv_ChannelIndex);
                            hv_Scale.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Scale = 1.0 / (hv_DeviationValues.TupleSelect(
                                    hv_ChannelIndex - 1));
                            }
                            hv_Shift.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Shift = (-hv_Scale) * (hv_MeanValues.TupleSelect(
                                    hv_ChannelIndex - 1));
                            }
                            ho_ChannelScaled.Dispose();
                            HOperatorSet.ScaleImage(ho_Channel, out ho_ChannelScaled, hv_Scale, hv_Shift);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.AppendChannel(ho_ImageScaled, ho_ChannelScaled, out ExpTmpOutVar_0
                                    );
                                ho_ImageScaled.Dispose();
                                ho_ImageScaled = ExpTmpOutVar_0;
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ImagesScaled, ho_ImageScaled, out ExpTmpOutVar_0
                                );
                            ho_ImagesScaled.Dispose();
                            ho_ImagesScaled = ExpTmpOutVar_0;
                        }
                    }
                    ho_Images_COPY_INP_TMP.Dispose();
                    ho_Images_COPY_INP_TMP = new HObject(ho_ImagesScaled);
                }
                else if ((int)(new HTuple(hv_NormalizationType.TupleEqual("none"))) != 0)
                {
                    hv_Indices.Dispose();
                    HOperatorSet.TupleFind(hv_Type, "byte", out hv_Indices);
                    if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                    {
                        //Shift the gray values from [0-255] to the expected range for byte images.
                        hv_RescaleRange.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_RescaleRange = (hv_ImageRangeMax - hv_ImageRangeMin) / 255.0;
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_ImageSelected.Dispose();
                            HOperatorSet.SelectObj(ho_Images_COPY_INP_TMP, out ho_ImageSelected, hv_Indices + 1);
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ScaleImage(ho_ImageSelected, out ExpTmpOutVar_0, hv_RescaleRange,
                                hv_ImageRangeMin);
                            ho_ImageSelected.Dispose();
                            ho_ImageSelected = ExpTmpOutVar_0;
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ReplaceObj(ho_Images_COPY_INP_TMP, ho_ImageSelected, out ExpTmpOutVar_0,
                                hv_Indices + 1);
                            ho_Images_COPY_INP_TMP.Dispose();
                            ho_Images_COPY_INP_TMP = ExpTmpOutVar_0;
                        }
                    }
                }
                else if ((int)(new HTuple(hv_NormalizationType.TupleNotEqual("none"))) != 0)
                {
                    throw new HalconException("Unsupported parameter value for 'normalization_type'");
                }
                //
                //Ensure that the number of channels of the resulting images is consistent with the
                //number of channels of the given model. The only exceptions that are adapted below
                //are combinations of 1- and 3-channel images if ImageNumChannels is either 1 or 3.
                if ((int)((new HTuple(hv_ImageNumChannels.TupleEqual(1))).TupleOr(new HTuple(hv_ImageNumChannels.TupleEqual(
                    3)))) != 0)
                {
                    hv_CurrentNumChannels.Dispose();
                    HOperatorSet.CountChannels(ho_Images_COPY_INP_TMP, out hv_CurrentNumChannels);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_DiffNumChannelsIndices.Dispose();
                        HOperatorSet.TupleFind(hv_CurrentNumChannels.TupleNotEqualElem(hv_OutputNumChannels),
                            1, out hv_DiffNumChannelsIndices);
                    }
                    if ((int)(new HTuple(hv_DiffNumChannelsIndices.TupleNotEqual(-1))) != 0)
                    {
                        for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_DiffNumChannelsIndices.TupleLength()
                            )) - 1); hv_Index = (int)hv_Index + 1)
                        {
                            hv_DiffNumChannelsIndex.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_DiffNumChannelsIndex = hv_DiffNumChannelsIndices.TupleSelect(
                                    hv_Index);
                            }
                            hv_ImageIndex.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ImageIndex = hv_DiffNumChannelsIndex + 1;
                            }
                            hv_NumChannels.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_NumChannels = hv_CurrentNumChannels.TupleSelect(
                                    hv_ImageIndex - 1);
                            }
                            ho_ImageSelected.Dispose();
                            HOperatorSet.SelectObj(ho_Images_COPY_INP_TMP, out ho_ImageSelected,
                                hv_ImageIndex);
                            if ((int)((new HTuple(hv_NumChannels.TupleEqual(1))).TupleAnd(new HTuple(hv_ImageNumChannels.TupleEqual(
                                3)))) != 0)
                            {
                                //Conversion from 1- to 3-channel image required
                                ho_ThreeChannelImage.Dispose();
                                HOperatorSet.Compose3(ho_ImageSelected, ho_ImageSelected, ho_ImageSelected,
                                    out ho_ThreeChannelImage);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ReplaceObj(ho_Images_COPY_INP_TMP, ho_ThreeChannelImage,
                                        out ExpTmpOutVar_0, hv_ImageIndex);
                                    ho_Images_COPY_INP_TMP.Dispose();
                                    ho_Images_COPY_INP_TMP = ExpTmpOutVar_0;
                                }
                            }
                            else if ((int)((new HTuple(hv_NumChannels.TupleEqual(3))).TupleAnd(
                                new HTuple(hv_ImageNumChannels.TupleEqual(1)))) != 0)
                            {
                                //Conversion from 3- to 1-channel image required
                                ho_SingleChannelImage.Dispose();
                                HOperatorSet.Rgb1ToGray(ho_ImageSelected, out ho_SingleChannelImage
                                    );
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ReplaceObj(ho_Images_COPY_INP_TMP, ho_SingleChannelImage,
                                        out ExpTmpOutVar_0, hv_ImageIndex);
                                    ho_Images_COPY_INP_TMP.Dispose();
                                    ho_Images_COPY_INP_TMP = ExpTmpOutVar_0;
                                }
                            }
                            else
                            {
                                throw new HalconException(((("Unexpected error adapting the number of channels. The number of channels of the resulting image is " + hv_NumChannels) + new HTuple(", but the number of channels of the model is ")) + hv_ImageNumChannels) + ".");
                            }
                        }
                    }
                }
                //
                //Write preprocessed images to output variable.
                ho_ImagesPreprocessed.Dispose();
                ho_ImagesPreprocessed = new HObject(ho_Images_COPY_INP_TMP);
                //
                ho_Images_COPY_INP_TMP.Dispose();
                ho_ImagesScaled.Dispose();
                ho_ImageSelected.Dispose();
                ho_ImageScaled.Dispose();
                ho_Channel.Dispose();
                ho_ChannelScaled.Dispose();
                ho_ThreeChannelImage.Dispose();
                ho_SingleChannelImage.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_ImageNumChannels.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_DomainHandling.Dispose();
                hv_NormalizationType.Dispose();
                hv_ModelType.Dispose();
                hv_NumImages.Dispose();
                hv_Type.Dispose();
                hv_NumMatches.Dispose();
                hv_InputNumChannels.Dispose();
                hv_OutputNumChannels.Dispose();
                hv_NumChannels1.Dispose();
                hv_NumChannels3.Dispose();
                hv_AreInputNumChannels1.Dispose();
                hv_AreInputNumChannels3.Dispose();
                hv_AreInputNumChannels1Or3.Dispose();
                hv_ValidNumChannels.Dispose();
                hv_ValidNumChannelsText.Dispose();
                hv_ImageIndex.Dispose();
                hv_NumChannels.Dispose();
                hv_ChannelIndex.Dispose();
                hv_Min.Dispose();
                hv_Max.Dispose();
                hv_Range.Dispose();
                hv_Scale.Dispose();
                hv_Shift.Dispose();
                hv_MeanValues.Dispose();
                hv_DeviationValues.Dispose();
                hv_UseDefaultNormalizationValues.Dispose();
                hv_Exception.Dispose();
                hv_Indices.Dispose();
                hv_RescaleRange.Dispose();
                hv_CurrentNumChannels.Dispose();
                hv_DiffNumChannelsIndices.Dispose();
                hv_Index.Dispose();
                hv_DiffNumChannelsIndex.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Images_COPY_INP_TMP.Dispose();
                ho_ImagesScaled.Dispose();
                ho_ImageSelected.Dispose();
                ho_ImageScaled.Dispose();
                ho_Channel.Dispose();
                ho_ChannelScaled.Dispose();
                ho_ThreeChannelImage.Dispose();
                ho_SingleChannelImage.Dispose();

                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_ImageNumChannels.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_DomainHandling.Dispose();
                hv_NormalizationType.Dispose();
                hv_ModelType.Dispose();
                hv_NumImages.Dispose();
                hv_Type.Dispose();
                hv_NumMatches.Dispose();
                hv_InputNumChannels.Dispose();
                hv_OutputNumChannels.Dispose();
                hv_NumChannels1.Dispose();
                hv_NumChannels3.Dispose();
                hv_AreInputNumChannels1.Dispose();
                hv_AreInputNumChannels3.Dispose();
                hv_AreInputNumChannels1Or3.Dispose();
                hv_ValidNumChannels.Dispose();
                hv_ValidNumChannelsText.Dispose();
                hv_ImageIndex.Dispose();
                hv_NumChannels.Dispose();
                hv_ChannelIndex.Dispose();
                hv_Min.Dispose();
                hv_Max.Dispose();
                hv_Range.Dispose();
                hv_Scale.Dispose();
                hv_Shift.Dispose();
                hv_MeanValues.Dispose();
                hv_DeviationValues.Dispose();
                hv_UseDefaultNormalizationValues.Dispose();
                hv_Exception.Dispose();
                hv_Indices.Dispose();
                hv_RescaleRange.Dispose();
                hv_CurrentNumChannels.Dispose();
                hv_DiffNumChannelsIndices.Dispose();
                hv_Index.Dispose();
                hv_DiffNumChannelsIndex.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Semantic Segmentation
        // Short Description: Preprocess segmentation and weight images for deep-learning-based segmentation training and inference. 
        public void preprocess_dl_model_segmentations(HObject ho_ImagesRaw, HObject ho_Segmentations,
            out HObject ho_SegmentationsPreprocessed, HTuple hv_DLPreprocessParam)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Domain = null, ho_SelectedSeg = null;
            HObject ho_SelectedDomain = null;

            // Local copy input parameter variables 
            HObject ho_Segmentations_COPY_INP_TMP;
            ho_Segmentations_COPY_INP_TMP = new HObject(ho_Segmentations);



            // Local control variables 

            HTuple hv_NumberImages = new HTuple(), hv_NumberSegmentations = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_WidthSeg = new HTuple(), hv_HeightSeg = new HTuple();
            HTuple hv_DLModelType = new HTuple(), hv_ImageWidth = new HTuple();
            HTuple hv_ImageHeight = new HTuple(), hv_ImageNumChannels = new HTuple();
            HTuple hv_ImageRangeMin = new HTuple(), hv_ImageRangeMax = new HTuple();
            HTuple hv_DomainHandling = new HTuple(), hv_SetBackgroundID = new HTuple();
            HTuple hv_ClassesToBackground = new HTuple(), hv_IgnoreClassIDs = new HTuple();
            HTuple hv_IsInt = new HTuple(), hv_IndexImage = new HTuple();
            HTuple hv_ImageWidthRaw = new HTuple(), hv_ImageHeightRaw = new HTuple();
            HTuple hv_EqualWidth = new HTuple(), hv_EqualHeight = new HTuple();
            HTuple hv_Type = new HTuple(), hv_EqualReal = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SegmentationsPreprocessed);
            HOperatorSet.GenEmptyObj(out ho_Domain);
            HOperatorSet.GenEmptyObj(out ho_SelectedSeg);
            HOperatorSet.GenEmptyObj(out ho_SelectedDomain);
            try
            {
                //
                //This procedure preprocesses the segmentation or weight images
                //given by Segmentations so that they can be handled by
                //train_dl_model_batch and apply_dl_model.
                //
                //Check input data.
                //Examine number of images.
                hv_NumberImages.Dispose();
                HOperatorSet.CountObj(ho_ImagesRaw, out hv_NumberImages);
                hv_NumberSegmentations.Dispose();
                HOperatorSet.CountObj(ho_Segmentations_COPY_INP_TMP, out hv_NumberSegmentations);
                if ((int)(new HTuple(hv_NumberImages.TupleNotEqual(hv_NumberSegmentations))) != 0)
                {
                    throw new HalconException("Equal number of images given in ImagesRaw and Segmentations required");
                }
                //Size of images.
                hv_Width.Dispose(); hv_Height.Dispose();
                HOperatorSet.GetImageSize(ho_ImagesRaw, out hv_Width, out hv_Height);
                hv_WidthSeg.Dispose(); hv_HeightSeg.Dispose();
                HOperatorSet.GetImageSize(ho_Segmentations_COPY_INP_TMP, out hv_WidthSeg, out hv_HeightSeg);
                if ((int)((new HTuple(hv_Width.TupleNotEqual(hv_WidthSeg))).TupleOr(new HTuple(hv_Height.TupleNotEqual(
                    hv_HeightSeg)))) != 0)
                {
                    throw new HalconException("Equal size of the images given in ImagesRaw and Segmentations required.");
                }
                //Check the validity of the preprocessing parameters.
                check_dl_preprocess_param(hv_DLPreprocessParam);
                //
                //Get the relevant preprocessing parameters.
                hv_DLModelType.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "model_type", out hv_DLModelType);
                hv_ImageWidth.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_width", out hv_ImageWidth);
                hv_ImageHeight.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_height", out hv_ImageHeight);
                hv_ImageNumChannels.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_num_channels", out hv_ImageNumChannels);
                hv_ImageRangeMin.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_min", out hv_ImageRangeMin);
                hv_ImageRangeMax.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "image_range_max", out hv_ImageRangeMax);
                hv_DomainHandling.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "domain_handling", out hv_DomainHandling);
                //Segmentation specific parameters.
                hv_SetBackgroundID.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "set_background_id", out hv_SetBackgroundID);
                hv_ClassesToBackground.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "class_ids_background", out hv_ClassesToBackground);
                hv_IgnoreClassIDs.Dispose();
                HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "ignore_class_ids", out hv_IgnoreClassIDs);
                //
                //Check the input parameter for setting the background ID.
                if ((int)(new HTuple(hv_SetBackgroundID.TupleNotEqual(new HTuple()))) != 0)
                {
                    //Check that the model is a segmentation model.
                    if ((int)(new HTuple(hv_DLModelType.TupleNotEqual("segmentation"))) != 0)
                    {
                        throw new HalconException("Setting class IDs to background is only implemented for segmentation.");
                    }
                    //Check the background ID.
                    hv_IsInt.Dispose();
                    HOperatorSet.TupleIsIntElem(hv_SetBackgroundID, out hv_IsInt);
                    if ((int)(new HTuple((new HTuple(hv_SetBackgroundID.TupleLength())).TupleNotEqual(
                        1))) != 0)
                    {
                        throw new HalconException("Only one class_id as 'set_background_id' allowed.");
                    }
                    else if ((int)(hv_IsInt.TupleNot()) != 0)
                    {
                        //Given class_id has to be of type int.
                        throw new HalconException("The class_id given as 'set_background_id' has to be of type int.");
                    }
                    //Check the values of ClassesToBackground.
                    if ((int)(new HTuple((new HTuple(hv_ClassesToBackground.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        //Check that the given classes are of length > 0.
                        throw new HalconException(new HTuple("If 'set_background_id' is given, 'class_ids_background' must at least contain this class ID."));
                    }
                    else if ((int)(new HTuple(((hv_ClassesToBackground.TupleIntersection(
                        hv_IgnoreClassIDs))).TupleNotEqual(new HTuple()))) != 0)
                    {
                        //Check that class_ids_background is not included in the ignore_class_ids of the DLModel.
                        throw new HalconException("The given 'class_ids_background' must not be included in the 'ignore_class_ids' of the model.");
                    }
                }
                //
                //Domain handling of the image to be preprocessed.
                //
                if ((int)(new HTuple(hv_DomainHandling.TupleEqual("full_domain"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.FullDomain(ho_Segmentations_COPY_INP_TMP, out ExpTmpOutVar_0
                            );
                        ho_Segmentations_COPY_INP_TMP.Dispose();
                        ho_Segmentations_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                else if ((int)(new HTuple(hv_DomainHandling.TupleEqual("crop_domain"))) != 0)
                {
                    //If the domain should be cropped the domain has to be transferred
                    //from the raw image to the segmentation image.
                    ho_Domain.Dispose();
                    HOperatorSet.GetDomain(ho_ImagesRaw, out ho_Domain);
                    HTuple end_val66 = hv_NumberImages;
                    HTuple step_val66 = 1;
                    for (hv_IndexImage = 1; hv_IndexImage.Continue(end_val66, step_val66); hv_IndexImage = hv_IndexImage.TupleAdd(step_val66))
                    {
                        ho_SelectedSeg.Dispose();
                        HOperatorSet.SelectObj(ho_Segmentations_COPY_INP_TMP, out ho_SelectedSeg,
                            hv_IndexImage);
                        ho_SelectedDomain.Dispose();
                        HOperatorSet.SelectObj(ho_Domain, out ho_SelectedDomain, hv_IndexImage);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ChangeDomain(ho_SelectedSeg, ho_SelectedDomain, out ExpTmpOutVar_0
                                );
                            ho_SelectedSeg.Dispose();
                            ho_SelectedSeg = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ReplaceObj(ho_Segmentations_COPY_INP_TMP, ho_SelectedSeg,
                                out ExpTmpOutVar_0, hv_IndexImage);
                            ho_Segmentations_COPY_INP_TMP.Dispose();
                            ho_Segmentations_COPY_INP_TMP = ExpTmpOutVar_0;
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.CropDomain(ho_Segmentations_COPY_INP_TMP, out ExpTmpOutVar_0
                            );
                        ho_Segmentations_COPY_INP_TMP.Dispose();
                        ho_Segmentations_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                else
                {
                    throw new HalconException("Unsupported parameter value for 'domain_handling'");
                }
                //
                //Preprocess the segmentation images.
                //
                //Set all background classes to the given background class ID.
                if ((int)(new HTuple(hv_SetBackgroundID.TupleNotEqual(new HTuple()))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        reassign_pixel_values(ho_Segmentations_COPY_INP_TMP, out ExpTmpOutVar_0,
                            hv_ClassesToBackground, hv_SetBackgroundID);
                        ho_Segmentations_COPY_INP_TMP.Dispose();
                        ho_Segmentations_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                //
                //Zoom images only if they have a different size than the specified size.
                hv_ImageWidthRaw.Dispose(); hv_ImageHeightRaw.Dispose();
                HOperatorSet.GetImageSize(ho_Segmentations_COPY_INP_TMP, out hv_ImageWidthRaw,
                    out hv_ImageHeightRaw);
                hv_EqualWidth.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EqualWidth = hv_ImageWidth.TupleEqualElem(
                        hv_ImageWidthRaw);
                }
                hv_EqualHeight.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EqualHeight = hv_ImageHeight.TupleEqualElem(
                        hv_ImageHeightRaw);
                }
                if ((int)((new HTuple(((hv_EqualWidth.TupleMin())).TupleEqual(0))).TupleOr(
                    new HTuple(((hv_EqualHeight.TupleMin())).TupleEqual(0)))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ZoomImageSize(ho_Segmentations_COPY_INP_TMP, out ExpTmpOutVar_0,
                            hv_ImageWidth, hv_ImageHeight, "nearest_neighbor");
                        ho_Segmentations_COPY_INP_TMP.Dispose();
                        ho_Segmentations_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                //
                //Check the type of the input images
                //and convert if necessary.
                hv_Type.Dispose();
                HOperatorSet.GetImageType(ho_Segmentations_COPY_INP_TMP, out hv_Type);
                hv_EqualReal.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_EqualReal = hv_Type.TupleEqualElem(
                        "real");
                }
                //
                if ((int)(new HTuple(((hv_EqualReal.TupleMin())).TupleEqual(0))) != 0)
                {
                    //Convert the image type to 'real',
                    //because the model expects 'real' images.
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_Segmentations_COPY_INP_TMP, out ExpTmpOutVar_0,
                            "real");
                        ho_Segmentations_COPY_INP_TMP.Dispose();
                        ho_Segmentations_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }
                //
                //Write preprocessed Segmentations to output variable.
                ho_SegmentationsPreprocessed.Dispose();
                ho_SegmentationsPreprocessed = new HObject(ho_Segmentations_COPY_INP_TMP);
                ho_Segmentations_COPY_INP_TMP.Dispose();
                ho_Domain.Dispose();
                ho_SelectedSeg.Dispose();
                ho_SelectedDomain.Dispose();

                hv_NumberImages.Dispose();
                hv_NumberSegmentations.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_WidthSeg.Dispose();
                hv_HeightSeg.Dispose();
                hv_DLModelType.Dispose();
                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_ImageNumChannels.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_DomainHandling.Dispose();
                hv_SetBackgroundID.Dispose();
                hv_ClassesToBackground.Dispose();
                hv_IgnoreClassIDs.Dispose();
                hv_IsInt.Dispose();
                hv_IndexImage.Dispose();
                hv_ImageWidthRaw.Dispose();
                hv_ImageHeightRaw.Dispose();
                hv_EqualWidth.Dispose();
                hv_EqualHeight.Dispose();
                hv_Type.Dispose();
                hv_EqualReal.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Segmentations_COPY_INP_TMP.Dispose();
                ho_Domain.Dispose();
                ho_SelectedSeg.Dispose();
                ho_SelectedDomain.Dispose();

                hv_NumberImages.Dispose();
                hv_NumberSegmentations.Dispose();
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_WidthSeg.Dispose();
                hv_HeightSeg.Dispose();
                hv_DLModelType.Dispose();
                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_ImageNumChannels.Dispose();
                hv_ImageRangeMin.Dispose();
                hv_ImageRangeMax.Dispose();
                hv_DomainHandling.Dispose();
                hv_SetBackgroundID.Dispose();
                hv_ClassesToBackground.Dispose();
                hv_IgnoreClassIDs.Dispose();
                hv_IsInt.Dispose();
                hv_IndexImage.Dispose();
                hv_ImageWidthRaw.Dispose();
                hv_ImageHeightRaw.Dispose();
                hv_EqualWidth.Dispose();
                hv_EqualHeight.Dispose();
                hv_Type.Dispose();
                hv_EqualReal.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Tools / Geometry
        // Short Description: Convert the parameters of rectangles with format rectangle2 to the coordinates of its 4 corner-points. 
        private void convert_rect2_5to8param(HTuple hv_Row, HTuple hv_Col, HTuple hv_Length1,
            HTuple hv_Length2, HTuple hv_Phi, out HTuple hv_Row1, out HTuple hv_Col1, out HTuple hv_Row2,
            out HTuple hv_Col2, out HTuple hv_Row3, out HTuple hv_Col3, out HTuple hv_Row4,
            out HTuple hv_Col4)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Co1 = new HTuple(), hv_Co2 = new HTuple();
            HTuple hv_Si1 = new HTuple(), hv_Si2 = new HTuple();
            // Initialize local and output iconic variables 
            hv_Row1 = new HTuple();
            hv_Col1 = new HTuple();
            hv_Row2 = new HTuple();
            hv_Col2 = new HTuple();
            hv_Row3 = new HTuple();
            hv_Col3 = new HTuple();
            hv_Row4 = new HTuple();
            hv_Col4 = new HTuple();
            try
            {
                //This procedure takes the parameters for a rectangle of type 'rectangle2'
                //and returns the coordinates of the four corners.
                //
                hv_Co1.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Co1 = (hv_Phi.TupleCos()
                        ) * hv_Length1;
                }
                hv_Co2.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Co2 = (hv_Phi.TupleCos()
                        ) * hv_Length2;
                }
                hv_Si1.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Si1 = (hv_Phi.TupleSin()
                        ) * hv_Length1;
                }
                hv_Si2.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Si2 = (hv_Phi.TupleSin()
                        ) * hv_Length2;
                }

                hv_Col1.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Col1 = (hv_Co1 - hv_Si2) + hv_Col;
                }
                hv_Row1.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row1 = ((-hv_Si1) - hv_Co2) + hv_Row;
                }
                hv_Col2.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Col2 = ((-hv_Co1) - hv_Si2) + hv_Col;
                }
                hv_Row2.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row2 = (hv_Si1 - hv_Co2) + hv_Row;
                }
                hv_Col3.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Col3 = ((-hv_Co1) + hv_Si2) + hv_Col;
                }
                hv_Row3.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row3 = (hv_Si1 + hv_Co2) + hv_Row;
                }
                hv_Col4.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Col4 = (hv_Co1 + hv_Si2) + hv_Col;
                }
                hv_Row4.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row4 = ((-hv_Si1) + hv_Co2) + hv_Row;
                }


                hv_Co1.Dispose();
                hv_Co2.Dispose();
                hv_Si1.Dispose();
                hv_Si2.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Co1.Dispose();
                hv_Co2.Dispose();
                hv_Si1.Dispose();
                hv_Si2.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Tools / Geometry
        // Short Description: Convert for four-sided figures the coordinates of the 4 corner-points to the parameters of format rectangle2. 
        private void convert_rect2_8to5param(HTuple hv_Row1, HTuple hv_Col1, HTuple hv_Row2,
            HTuple hv_Col2, HTuple hv_Row3, HTuple hv_Col3, HTuple hv_Row4, HTuple hv_Col4,
            HTuple hv_ForceL1LargerL2, out HTuple hv_Row, out HTuple hv_Col, out HTuple hv_Length1,
            out HTuple hv_Length2, out HTuple hv_Phi)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Hor = new HTuple(), hv_Vert = new HTuple();
            HTuple hv_IdxSwap = new HTuple(), hv_Tmp = new HTuple();
            // Initialize local and output iconic variables 
            hv_Row = new HTuple();
            hv_Col = new HTuple();
            hv_Length1 = new HTuple();
            hv_Length2 = new HTuple();
            hv_Phi = new HTuple();
            try
            {
                //This procedure takes the corners of four-sided figures
                //and returns the parameters of type 'rectangle2'.
                //
                //Calculate center row and column.
                hv_Row.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row = (((hv_Row1 + hv_Row2) + hv_Row3) + hv_Row4) / 4.0;
                }
                hv_Col.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Col = (((hv_Col1 + hv_Col2) + hv_Col3) + hv_Col4) / 4.0;
                }
                //Length1 and Length2.
                hv_Length1.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Length1 = (((((hv_Row1 - hv_Row2) * (hv_Row1 - hv_Row2)) + ((hv_Col1 - hv_Col2) * (hv_Col1 - hv_Col2)))).TupleSqrt()
                        ) / 2.0;
                }
                hv_Length2.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Length2 = (((((hv_Row2 - hv_Row3) * (hv_Row2 - hv_Row3)) + ((hv_Col2 - hv_Col3) * (hv_Col2 - hv_Col3)))).TupleSqrt()
                        ) / 2.0;
                }
                //Calculate the angle phi.
                hv_Hor.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Hor = hv_Col1 - hv_Col2;
                }
                hv_Vert.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Vert = hv_Row2 - hv_Row1;
                }
                if ((int)(hv_ForceL1LargerL2) != 0)
                {
                    //Swap length1 and length2 if necessary.
                    hv_IdxSwap.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_IdxSwap = ((((hv_Length2 - hv_Length1)).TupleGreaterElem(
                            1e-9))).TupleFind(1);
                    }
                    if ((int)(new HTuple(hv_IdxSwap.TupleNotEqual(-1))) != 0)
                    {
                        hv_Tmp.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Tmp = hv_Length1.TupleSelect(
                                hv_IdxSwap);
                        }
                        if (hv_Length1 == null)
                            hv_Length1 = new HTuple();
                        hv_Length1[hv_IdxSwap] = hv_Length2.TupleSelect(hv_IdxSwap);
                        if (hv_Length2 == null)
                            hv_Length2 = new HTuple();
                        hv_Length2[hv_IdxSwap] = hv_Tmp;
                        if (hv_Hor == null)
                            hv_Hor = new HTuple();
                        hv_Hor[hv_IdxSwap] = (hv_Col2.TupleSelect(hv_IdxSwap)) - (hv_Col3.TupleSelect(
                            hv_IdxSwap));
                        if (hv_Vert == null)
                            hv_Vert = new HTuple();
                        hv_Vert[hv_IdxSwap] = (hv_Row3.TupleSelect(hv_IdxSwap)) - (hv_Row2.TupleSelect(
                            hv_IdxSwap));
                    }
                }
                hv_Phi.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Phi = hv_Vert.TupleAtan2(
                        hv_Hor);
                }
                //

                hv_Hor.Dispose();
                hv_Vert.Dispose();
                hv_IdxSwap.Dispose();
                hv_Tmp.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Hor.Dispose();
                hv_Vert.Dispose();
                hv_IdxSwap.Dispose();
                hv_Tmp.Dispose();

                throw HDevExpDefaultException;
            }
        }
        // Chapter: Image / Manipulation
        // Short Description: Changes a value of ValuesToChange in Image to NewValue. 
        private void reassign_pixel_values(HObject ho_Image, out HObject ho_ImageOut,
            HTuple hv_ValuesToChange, HTuple hv_NewValue)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionToChange, ho_RegionClass = null;

            // Local control variables 

            HTuple hv_IndexReset = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageOut);
            HOperatorSet.GenEmptyObj(out ho_RegionToChange);
            HOperatorSet.GenEmptyObj(out ho_RegionClass);
            try
            {
                //
                //This procedure sets all pixels of Image
                //with the values given in ValuesToChange to the given value NewValue.
                //
                ho_RegionToChange.Dispose();
                HOperatorSet.GenEmptyRegion(out ho_RegionToChange);
                for (hv_IndexReset = 0; (int)hv_IndexReset <= (int)((new HTuple(hv_ValuesToChange.TupleLength()
                    )) - 1); hv_IndexReset = (int)hv_IndexReset + 1)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_RegionClass.Dispose();
                        HOperatorSet.Threshold(ho_Image, out ho_RegionClass, hv_ValuesToChange.TupleSelect(
                            hv_IndexReset), hv_ValuesToChange.TupleSelect(hv_IndexReset));
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union2(ho_RegionToChange, ho_RegionClass, out ExpTmpOutVar_0
                            );
                        ho_RegionToChange.Dispose();
                        ho_RegionToChange = ExpTmpOutVar_0;
                    }
                }
                HOperatorSet.OverpaintRegion(ho_Image, ho_RegionToChange, hv_NewValue, "fill");
                ho_ImageOut.Dispose();
                ho_ImageOut = new HObject(ho_Image);
                ho_RegionToChange.Dispose();
                ho_RegionClass.Dispose();

                hv_IndexReset.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionToChange.Dispose();
                ho_RegionClass.Dispose();

                hv_IndexReset.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Deep Learning / Model
        // Short Description: This procedure replaces legacy preprocessing parameters. 
        private void replace_legacy_preprocessing_parameters(HTuple hv_DLPreprocessParam)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Exception = new HTuple(), hv_NormalizationTypeExists = new HTuple();
            HTuple hv_NormalizationType = new HTuple(), hv_LegacyNormalizationKeyExists = new HTuple();
            HTuple hv_ContrastNormalization = new HTuple();
            // Initialize local and output iconic variables 
            try
            {
                //
                //This procedure adapts the dictionary DLPreprocessParam
                //if a legacy preprocessing parameter is set.
                //
                //Map legacy value set to new parameter.
                hv_Exception.Dispose();
                hv_Exception = 0;
                try
                {
                    hv_NormalizationTypeExists.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", "normalization_type",
                        out hv_NormalizationTypeExists);
                    //
                    if ((int)(hv_NormalizationTypeExists) != 0)
                    {
                        hv_NormalizationType.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "normalization_type", out hv_NormalizationType);
                        if ((int)(new HTuple(hv_NormalizationType.TupleEqual("true"))) != 0)
                        {
                            hv_NormalizationType.Dispose();
                            hv_NormalizationType = "first_channel";
                        }
                        else if ((int)(new HTuple(hv_NormalizationType.TupleEqual("false"))) != 0)
                        {
                            hv_NormalizationType.Dispose();
                            hv_NormalizationType = "none";
                        }
                        HOperatorSet.SetDictTuple(hv_DLPreprocessParam, "normalization_type", hv_NormalizationType);
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                //
                //Map legacy parameter to new parameter and corresponding value.
                hv_Exception.Dispose();
                hv_Exception = 0;
                try
                {
                    hv_LegacyNormalizationKeyExists.Dispose();
                    HOperatorSet.GetDictParam(hv_DLPreprocessParam, "key_exists", "contrast_normalization",
                        out hv_LegacyNormalizationKeyExists);
                    if ((int)(hv_LegacyNormalizationKeyExists) != 0)
                    {
                        hv_ContrastNormalization.Dispose();
                        HOperatorSet.GetDictTuple(hv_DLPreprocessParam, "contrast_normalization",
                            out hv_ContrastNormalization);
                        //Replace 'contrast_normalization' by 'normalization_type'.
                        if ((int)(new HTuple(hv_ContrastNormalization.TupleEqual("false"))) != 0)
                        {
                            HOperatorSet.SetDictTuple(hv_DLPreprocessParam, "normalization_type",
                                "none");
                        }
                        else if ((int)(new HTuple(hv_ContrastNormalization.TupleEqual(
                            "true"))) != 0)
                        {
                            HOperatorSet.SetDictTuple(hv_DLPreprocessParam, "normalization_type",
                                "first_channel");
                        }
                        HOperatorSet.RemoveDictKey(hv_DLPreprocessParam, "contrast_normalization");
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }

                hv_Exception.Dispose();
                hv_NormalizationTypeExists.Dispose();
                hv_NormalizationType.Dispose();
                hv_LegacyNormalizationKeyExists.Dispose();
                hv_ContrastNormalization.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Exception.Dispose();
                hv_NormalizationTypeExists.Dispose();
                hv_NormalizationType.Dispose();
                hv_LegacyNormalizationKeyExists.Dispose();
                hv_ContrastNormalization.Dispose();

                throw HDevExpDefaultException;
            }
        }
        #endregion


    }
}
