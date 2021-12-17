using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alg_KeyBoard_BYD
{
    public enum LocaterWay
    {
        ModelLocater = 0,
        Line1Arc2 = 1,
    }
    public class CFindLine
    {

        const double AngleToRad = 0.01745329222;
        const double RadToAngle = 57.29578049044;
        public const int _LineRoiCount = 1;
        #region  参数

        public LocaterWay _LocaterWay = LocaterWay.ModelLocater;
        public bool _IsThreshold = false;
        public int _MinThreshold = 0;
        public int _MaxThreshold = 255;
        /// <summary>//前景灰度
        /// 
        /// </summary>
        public int _ForeGary = 255;
        /// <summary>背景灰度
        /// 
        /// </summary>
        public int _BackGray = 0;
        public bool _IsShowBinRegion = false;
        private string _SavePath = "";

        public HTuple _LineRoiX = new HTuple();

        public HTuple _LineRoiY = new HTuple();


        public bool[] _ListRoiOk = new bool[_LineRoiCount];
        private HObject _ListMeasureRect = new HObject();
        private HObject _DitectionMea = new HObject();
        private double _LineFitScore = 0.5;
        private double _DeleteDist = 10;
        private int _ModelIndex = -1;

        public bool _isUseRoiOffset = false;
        public int _RoiOffsetRow = 0;
        public int _RoiOffsetCol = 0;
        public int ModelIndex
        {
            get
            {
                return _ModelIndex;

            }
            set
            {
                _ModelIndex = value;
            }
        }
        #region 显示参数

        private bool _ShowSearchLine = false;
        public bool ShowSearchLine
        {
            set
            {
                _ShowSearchLine = value;
            }
            get
            {
                return _ShowSearchLine;
            }

        }

        private int _ShowFindLinePt = 0;
        public bool ShowFindLinePt
        {
            set
            {
                if (value == true) _ShowFindLinePt = 1;
                else _ShowFindLinePt = 0;
            }
            get
            {
                if (_ShowFindLinePt == 0) return false;
                else return true;

            }
        }
        private int _ShowDeleteLinePt = 0;

        private int _ShowFindLineRect = 0;
        public bool ShowFindLineRect
        {
            set
            {
                if (value == true) _ShowFindLineRect = 1;
                else _ShowFindLineRect = 0;
            }
            get
            {
                if (_ShowFindLineRect == 0) return false;
                else return true;

            }

        }
        private int _ShowFitPt = 1;
        public bool ShowFitPt
        {
            set
            {
                if (value == true) _ShowFitPt = 1;
                else _ShowFitPt = 0;
            }
            get
            {
                if (_ShowFitPt == 0) return false;
                else return true;

            }
        }
        public bool ShowDeleteLinePt
        {
            set
            {
                if (true == value) _ShowDeleteLinePt = 1;
                else _ShowDeleteLinePt = 0;
            }
            get
            {
                if (_ShowDeleteLinePt == 0) return false;
                else return true;
            }
        }

        private int _ShowLineResult = 1;
        public bool ShowLineResult
        {
            set
            {
                if (true == value) _ShowLineResult = 1;
                else _ShowLineResult = 0;
            }
            get
            {
                if (_ShowLineResult == 0) return false;
                else return true;
            }
        }

        private int _ShowDirection = 1; //显示搜索方向
        public bool ShowDirection
        {
            set
            {
                if (true == value) _ShowDirection = 1;
                else _ShowDirection = 0;
            }
            get
            {
                if (_ShowDirection == 0) return false;
                else return true;
            }
        }
        #endregion
        #region 结果参数

        public List<bool> _ResultLineFindSuccessColl = new List<bool>();
        #endregion

        private int _IsLocater = 0;
        public bool IsLocater
        {
            set
            {
                if (true == value) _IsLocater = 1;
                else _IsLocater = 0;
            }
            get
            {
                if (_IsLocater == 0) return false;
                else return true;
            }
        }
        private bool _ListPtOk = new bool();

        #endregion

        #region 属性
        public double LineFitScore
        {
            set
            {
                _LineFitScore = value;
            }
            get
            {
                return _LineFitScore;
            }
        }
        public HTuple LineRoiY
        {
            get
            {
                return _LineRoiY;
            }
        }
        public HTuple LineRoiX
        {
            get
            {
                return _LineRoiX;
            }
        }
        public string SavePath
        {
            get
            {
                return _SavePath;
            }
            set
            {
                _SavePath = value;
            }
        }
        #endregion
        //直线变量 
        public HTuple m_Line_ROI_X;			//直线ROI x坐标数组,第一个值为起点，第二个字为终点
        public HTuple m_Line_ROI_Y;			//直线ROI y坐标数组,第一个值为起点，第二个字为终点

        public HTuple m_Line_Elements;		//rake工具卡尺数
        public int m_Line_Min_Points_Num;  //拟合直线最少点数
        public double m_Line_Sigma;			//直线边缘点滤波系数
        public string m_Line_Transition;		//直线边缘点极性
        public string m_Line_Point_Select;    //直线边缘点选择
        public int m_Line_Threshold;		//直线边缘点阈值

        public HTuple m_Line_Edges_X;			//直线卡尺工具找到点的x坐标数组
        public HTuple m_Line_Edges_Y;			//直线卡尺工具找到点的y坐标数组	
        public HTuple _LineDeleteX = new HTuple();
        public HTuple _LineDeleteY = new HTuple();
        public HTuple _LineFitX = new HTuple();
        public HTuple _LineFitY = new HTuple();
        public List<int> _PtOkNum = new List<int>();

        public HTuple m_Line_X_Begin;				//拟合直线的x坐标数组,第一个值为起点
        public HTuple m_Line_X_End;				//拟合直线的x坐标数组,第一个值为起点
        public HTuple m_Line_Y_Begin;				//拟合直线的y坐标数组,第一个值为起点，第二个字为终点
        public HTuple m_Line_Y_End;
        public HTuple _Nc;
        public HTuple _Nr;
        public HTuple _ResultAngle;
        public int m_Line_Caliper_Width;	//直线卡尺工具宽度
        public int m_Line_Caliper_Height;	//直线卡尺工具高度


        public HObject m_Line_Regions;			//直线卡尺区域
        public HObject m_Line_Trans_Regions;    //直线卡尺转换区域 
        public HObject m_Line_xld;				//拟合的直线xld
        private HObject _LineXldCollect = new HObject();
        public HObject LineXldCollect
        {
            get
            {
                return _LineXldCollect;
            }
        }
        public HObject m_Line_Cross;             //拟合直线的边缘点
        public HObject m_InputImage;             //图像


        //private PictureControl _PictureControl;
        public CFindLine(string SavePath1, HObject inputimg)
        {
            m_InputImage = inputimg;
            SavePath = SavePath1;
            //直线变量 
            m_Line_ROI_X = 0;			                            //直线ROI x坐标数组,第一个值为起点，第二个字为终点
            m_Line_ROI_Y = 0;			                            //直线ROI y坐标数组,第一个值为起点，第二个字为终点
            m_Line_Elements = 20;		                            //rake工具卡尺数
            m_Line_Min_Points_Num = 2;	                            //拟合直线最少点数
            m_Line_Sigma = 1;			                            //直线边缘点滤波系数
            m_Line_Transition = "all";	                            //直线边缘点极性
            m_Line_Point_Select = "all";                            //直线边缘点选择
            m_Line_Threshold = 15;		                            //直线边缘点阈值
            HOperatorSet.GenEmptyObj(out m_Line_Regions);          //直线卡尺区域

            HOperatorSet.GenEmptyObj(out m_Line_xld);				//拟合的直线xld

            m_Line_Caliper_Width = 15;		                        //直线卡尺工具宽度
            m_Line_Caliper_Height = 60;		                        //直线卡尺工具高度
        }
        public CFindLine(string SavePath1)
        {
            SavePath = SavePath1;
            //直线变量 
            m_Line_ROI_X = 0;			                            //直线ROI x坐标数组,第一个值为起点，第二个字为终点
            m_Line_ROI_Y = 0;			                            //直线ROI y坐标数组,第一个值为起点，第二个字为终点
            m_Line_Elements = 20;		                            //rake工具卡尺数
            m_Line_Min_Points_Num = 2;	                            //拟合直线最少点数
            m_Line_Sigma = 1;			                            //直线边缘点滤波系数
            m_Line_Transition = "all";	                            //直线边缘点极性
            m_Line_Point_Select = "all";                            //直线边缘点选择
            m_Line_Threshold = 15;		                            //直线边缘点阈值
            _ResultAngle = new HTuple();
            HOperatorSet.GenEmptyObj(out m_Line_Regions);          //直线卡尺区域

            HOperatorSet.GenEmptyObj(out m_Line_xld);				//拟合的直线xld

            m_Line_Caliper_Width = 15;		                        //直线卡尺工具宽度
            m_Line_Caliper_Height = 60;                             //直线卡尺工具高度

            // _PictureControl = qPictureControl;


            _ListMeasureRect = new HObject();
            _ListPtOk = true;
            _DitectionMea = new HObject();

            _LineRoiX = new HTuple();
            _LineRoiY = new HTuple();
            _LineRoiX.Append(50);
            _LineRoiX.Append(100);
            _LineRoiY.Append(50);
            _LineRoiY.Append(100);
        }
        //_ListRoiOk = true;
          

        ~CFindLine()
        {
            m_Line_Regions.Dispose();
            m_Line_xld.Dispose();
        }

        public void SaveData()
        {

            String q_strSavePath = directoryClass.liaohaoPath + "\\";
            FileHelper.CreateDirectory(q_strSavePath);

            HTuple LineParam = new HTuple();

            //14个
            LineParam.Append(m_Line_Elements);
            LineParam.Append(m_Line_Min_Points_Num);
            LineParam.Append(m_Line_Sigma);
            LineParam.Append(m_Line_Transition);
            LineParam.Append(m_Line_Point_Select);
            LineParam.Append(m_Line_Caliper_Width);
            LineParam.Append(m_Line_Caliper_Height);
            LineParam.Append(_DeleteDist);
            LineParam.Append(_ShowDeleteLinePt);
            LineParam.Append(_ShowFindLinePt);
            LineParam.Append(_ShowFitPt);
            LineParam.Append(_IsLocater);
            LineParam.Append(_ShowFindLineRect);
            LineParam.Append(ModelIndex);
            LineParam.Append(Convert.ToInt32(_ShowSearchLine));
            LineParam.Append(_LineFitScore);
            LineParam.Append(m_Line_Threshold);

            //阈值
            LineParam.Append(Convert.ToInt32(_IsThreshold));
            LineParam.Append(_MinThreshold);
            LineParam.Append(_MaxThreshold);
            LineParam.Append(_ForeGary);
            LineParam.Append(_BackGray);
            LineParam.Append(Convert.ToInt32(_IsShowBinRegion));

            LineParam.Append(Convert.ToInt32(_isUseRoiOffset));
            LineParam.Append(_RoiOffsetRow);
            LineParam.Append(_RoiOffsetCol);
            LineParam.Append((int)_LocaterWay);

            for (int i = 0; i < _LineRoiCount; i++)
            {
                LineParam.Append(Convert.ToInt32(_ListRoiOk[i]));
            }
            //    LineParam.Append(ModelIndex);

            HOperatorSet.WriteTuple(LineParam, q_strSavePath + "LineParam.tup");

            for (int i = 0; i < _LineRoiCount; i++)
            {
                if (_LineRoiY[i] == null)
                {
                    if (File.Exists(q_strSavePath + "LineRoiLineRow" + i + ".tup"))
                    {
                        File.Delete(q_strSavePath + "LineRoiLineRow" + i + ".tup");
                    }
                    if (File.Exists(q_strSavePath + "LineRoiLineCol" + i + ".tup"))
                    {
                        File.Delete(q_strSavePath + "LineRoiLineCol" + i + ".tup");
                    }
                    continue;
                }
                HOperatorSet.WriteTuple(_LineRoiY[i], q_strSavePath + "LineRoiLineRow" + i + ".tup");

                HOperatorSet.WriteTuple(_LineRoiX[i], q_strSavePath + "LineRoiLineCol" + i + ".tup");
            }
            if (m_Line_Regions.IsInitialized() == true)
            {
                if (m_Line_Regions.CountObj() > 0)
                    HOperatorSet.WriteObject(m_Line_Regions, q_strSavePath + "LinesRegions");
            }


        }
        public void ReadData()
        {

            String q_strSavePath = directoryClass.liaohaoPath + "\\";

            //String q_strSavePath = ReadPath + "\\"; //GlobalParam.JudgeStrPathLastSymbol(ReadPath, "/");

            if (File.Exists(q_strSavePath + "LineParam.tup"))
            {
                HTuple LineParam;
                HOperatorSet.ReadTuple(q_strSavePath + "LineParam.tup", out LineParam);

                (m_Line_Elements) = LineParam[0];
                (m_Line_Min_Points_Num) = LineParam[1];
                (m_Line_Sigma) = LineParam[2];
                (m_Line_Transition) = LineParam[3];
                (m_Line_Point_Select) = LineParam[4];
                (m_Line_Caliper_Width) = LineParam[5];
                (m_Line_Caliper_Height) = LineParam[6];
                _DeleteDist = LineParam[7];
                _ShowDeleteLinePt = LineParam[8];
                _ShowFindLinePt = LineParam[9];
                _ShowFitPt = LineParam[10];
                _IsLocater = LineParam[11];
                _ShowFindLineRect = LineParam[12];
                _ModelIndex = LineParam[13].I;
                _ShowSearchLine = Convert.ToBoolean(LineParam[14].I);
                _LineFitScore = LineParam[15].D;
                m_Line_Threshold = LineParam[16].I;

                _IsThreshold = Convert.ToBoolean(LineParam[17].I);
                _MinThreshold = LineParam[18].I;
                _MaxThreshold = LineParam[19].I;
                _ForeGary = LineParam[20].I;
                _BackGray = LineParam[21].I;
                _IsShowBinRegion = Convert.ToBoolean(LineParam[22].I);

                _isUseRoiOffset = Convert.ToBoolean(LineParam[23].I);
                _RoiOffsetRow = LineParam[24].I;
                _RoiOffsetCol = LineParam[25].I;
                _LocaterWay = (LocaterWay)LineParam[26].I;
                for (int i = 0; i < _LineRoiCount; i++)
                {
                    //    LineParam.Append(Convert.ToInt32(_ListRoiOk[i]));
                    _ListRoiOk[i] = Convert.ToBoolean(LineParam[27 + i].I);
                }
                //   _ModelIndex = LineParam[27 + _LineRoiCount].I;
            }

            for (int i = 0; i < _LineRoiCount; i++)
            {
                HTuple Line_ROI_X, Line_ROI_Y;
                if (File.Exists(q_strSavePath + "LineRoiLineRow" + i + ".tup"))
                {
                    HOperatorSet.ReadTuple(q_strSavePath + "LineRoiLineRow" + i + ".tup", out Line_ROI_Y);
                    _LineRoiY[i] = Line_ROI_Y;
                }

                if (File.Exists(q_strSavePath + "LineRoiLineCol" + i + ".tup"))
                {
                    HOperatorSet.ReadTuple(q_strSavePath + "LineRoiLineCol" + i + ".tup", out Line_ROI_X);
                    _LineRoiX[i] = Line_ROI_X;
                }
            }

            if (File.Exists(q_strSavePath + "LinesRegions.hobj"))
            {
                HOperatorSet.ReadObject(out m_Line_Regions, q_strSavePath + "LinesRegions.hobj");
            }

        }

        public void GenMesaureRectRegion(HTuple LineRoiY, HTuple LineRoiX)
        {
            if (LineRoiY == null) return;
            if (LineRoiY.Length != 2) return;

            HTuple hv_Row1, hv_Column1, hv_Row2, hv_Column2, Legnth, Phi;

            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables 

            HObject ho_RegionLines, ho_Rectangle = null, ho_RegionLinesPloy = null;
            HObject ho_Arrow1 = null;

            HOperatorSet.GenContourPolygonXld(out ho_RegionLines, LineRoiY, LineRoiX);

            HOperatorSet.GenPolygonsXld(ho_RegionLines, out ho_RegionLinesPloy, "ramer", 2);
            HOperatorSet.GetLinesXld(ho_RegionLinesPloy, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2, out Legnth, out Phi);
            // Local control variables 

            HTuple hv_ATan, hv_i, hv_RowC = new HTuple();
            HTuple hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out m_Line_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            HOperatorSet.GenEmptyObj(out ho_RegionLinesPloy);
            try
            {
                m_Line_Regions.Dispose();
                HOperatorSet.GenEmptyObj(out m_Line_Regions);
                //画矢量检测直线


                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());

                //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
                for (hv_i = 1; hv_i.Continue(m_Line_Elements, 1); hv_i = hv_i.TupleAdd(1))
                {
                    //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                    if ((int)(new HTuple(m_Line_Elements.TupleEqual(1))) != 0)
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, m_Line_Caliper_Height / 2, hv_Distance / 2);
                    }
                    else
                    {
                        //如果有多个测量矩形，产生该测量矩形xld
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (m_Line_Elements - 1));
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (m_Line_Elements - 1));
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, m_Line_Caliper_Height / 2, m_Line_Caliper_Width / 2);
                    }
                    //把测量矩形xld存储到显示对象
                    OTemp[SP_O] = m_Line_Regions.CopyObj(1, -1);
                    SP_O++;
                    m_Line_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle, out m_Line_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                    {
                        //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                        hv_RowL2 = hv_RowC + ((m_Line_Caliper_Height / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowC - ((m_Line_Caliper_Height / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColC + ((m_Line_Caliper_Height / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColC - ((m_Line_Caliper_Height / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2, 25, 25);
                        HObject Arrow;
                        gen_arrow_contour_xld(out Arrow, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2, 25, 25);
                        _DitectionMea = Arrow;
                        //把xld存储到显示对象
                        OTemp[SP_O] = m_Line_Regions.CopyObj(1, -1);
                        SP_O++;
                        m_Line_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out m_Line_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                }
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
                ho_RegionLinesPloy.Dispose();
                throw HDevExpDefaultException;

            }


        }

        private int HomMat2DNum(List<HTuple> HomMat2DCollection)
        {
            return HomMat2DCollection.Count;
        }
        private HTuple GetHomMat2D(List<HTuple> HomMat2DCollection, int HomMat2DIndex)
        {


            HTuple Hom2d = new HTuple();
            return HomMat2DCollection[HomMat2DIndex];

        }

        HObject ThresholdRegion;



        private void AffineRoiPt(HTuple Hom2D, out HTuple[] ROiAffineX, out HTuple[] ROiAffineY)
        {
            HOperatorSet.GenEmptyObj(out ThresholdRegion);
            HTuple[] LineRoiXCollect = new HTuple[_LineRoiCount];
            HTuple[] LineRoiYCollect = new HTuple[_LineRoiCount];
            for (int i = 0; i < _LineRoiCount; i++)
            {
                if (_LineRoiX == null ) continue;
                HTuple AffineRoiY, AffineRoiX;
                HOperatorSet.AffineTransPixel(Hom2D, _LineRoiY[i], _LineRoiX[i], out AffineRoiY, out AffineRoiX);
                LineRoiXCollect[i] = AffineRoiX;
                LineRoiYCollect[i] = AffineRoiY;
                double phi = (AffineRoiY[0] - AffineRoiY[1]) / (AffineRoiX[0] - AffineRoiX[1]);
                HTuple Phi = Math.Atan(phi);
                if (_IsThreshold)
                {
                    HObject ThresholdRoiTemp;
                    HOperatorSet.GenEmptyObj(out ThresholdRoiTemp);
                    double Length1 = Math.Sqrt(Math.Pow(AffineRoiY[0] - AffineRoiY[1], 2) + Math.Pow(AffineRoiX[0] - AffineRoiX[1], 2));
                    if ((AffineRoiY[0] + AffineRoiY[1]) / 2 < 0 || (AffineRoiX[0] + AffineRoiX[1]) / 2 < 0)
                    {

                    }
                    else
                        HOperatorSet.GenRectangle2(out ThresholdRoiTemp, (AffineRoiY[0] + AffineRoiY[1]) / 2,
                            (AffineRoiX[0] + AffineRoiX[1]) / 2, -Phi, Length1 / 2, m_Line_Caliper_Height / 2);

                    HOperatorSet.ConcatObj(ThresholdRegion, ThresholdRoiTemp, out ThresholdRegion);
                    ThresholdRoiTemp.Dispose();
                }
            }
            // PictureControl1.AddRoi("red", false, ThresholdRegion, "ThresholdRoi");
            ROiAffineX = LineRoiXCollect;
            ROiAffineY = LineRoiYCollect;
        }
        public bool Execute(List<HTuple> HomMat2D)
        {
            // if (HomMat2D==null) 
            ClearResult();
            HOperatorSet.GenEmptyObj(out m_Line_Trans_Regions);
            HOperatorSet.GenEmptyObj(out m_Line_xld);
            m_Line_Edges_X = new HTuple();
            m_Line_Edges_Y = new HTuple();
            HOperatorSet.GenEmptyObj(out _LineXldCollect);

            bool q_bFitLineSuccess = false;
            try
            {
                //判断图像是否为空
                if (!ObjectValided(m_InputImage))
                {
                    //Error.iErrorType = 1;
                    //Error.strErrorInfo = "图像为空!";
                    return false;
                }


                //判断ROI仿射变换矩阵是否有效，有效的时候，有6个数据 
                if (_IsLocater == 1)
                {
                    int Hom2DNum = HomMat2DNum(HomMat2D);
                    HTuple[] LineRoiXCollect;
                    HTuple[] LineRoiYCollect;
                    for (int i = 0; i < Hom2DNum; i++)
                    {
                        HTuple Hom2DIndex = GetHomMat2D(HomMat2D, i);
                        // HObject ThresholdRegion;
                        if (Hom2DIndex.Length == 6)
                        {
                            AffineRoiPt(Hom2DIndex, out LineRoiXCollect, out LineRoiYCollect);
                            if (ShowSearchLine)
                            {
                                HTuple AffineX = new HTuple(), AffineY = new HTuple();
                                HObject SearchRoi;
                                for (int j = 0; j < _LineRoiCount; j++)
                                {
                                    //  if (LineRoiYCollect[j] != null && LineRoiYCollect[j].TupleLength() > 0)
                                    if (_ListRoiOk[j] == true)
                                    {
                                        AffineX.Append(LineRoiXCollect[j]);
                                        AffineY.Append(LineRoiYCollect[j]);
                                    }
                                }
                                HOperatorSet.GenContourPolygonXld(out SearchRoi, AffineY, AffineX);
                                //qPictureControl.AddRoi("blue", false, SearchRoi, "搜索直线");
                            }


                            HTuple Row0, Row1, Col0, Col1, Nr, Nc;
                            q_bFitLineSuccess = FitLine(ThresholdRegion,
                            m_Line_Elements, m_Line_Caliper_Height, m_Line_Caliper_Width, m_Line_Sigma,
                            m_Line_Threshold, m_Line_Transition, m_Line_Point_Select, LineRoiYCollect, LineRoiXCollect,
                            out Col0, out Row0, out Col1, out Row1, out Nc, out Nr);
                            if (q_bFitLineSuccess == true)
                            {
                                m_Line_X_Begin.Append(Col0);
                                m_Line_X_End.Append(Col1);
                                m_Line_Y_Begin.Append(Row0);
                                m_Line_Y_End.Append(Row1);
                                _Nr.Append(Nr);
                                _Nc.Append(Nc);
                            }
                            else
                            {

                                m_Line_X_Begin.Append(-1);
                                m_Line_X_End.Append(-1);
                                m_Line_Y_Begin.Append(-1);
                                m_Line_Y_End.Append(-1);
                                _Nr.Append(-1);
                                _Nc.Append(-1);
                            }
                        }
                        else
                        {
                            q_bFitLineSuccess = false;
                            m_Line_X_Begin.Append(-1);
                            m_Line_X_End.Append(-1);
                            m_Line_Y_Begin.Append(-1);
                            m_Line_Y_End.Append(-1);
                            _PtOkNum.Add(0);
                        }
                        _ResultLineFindSuccessColl.Add(q_bFitLineSuccess);

                        //if (q_bFitLineSuccess == false)
                        //{
                        //    m_Line_X_Begin.Append(-1);
                        //    m_Line_X_End.Append(-1);
                        //    m_Line_Y_Begin.Append(-1);
                        //    m_Line_Y_End.Append(-1);
                        //    _Nr.Append(-1);
                        //    _Nc.Append(-1);

                        //}

                    }

                }
                else
                {
                    HTuple[] LineRoiYCollect = new HTuple[_LineRoiCount];
                    HTuple[] LineRoiXCollect = new HTuple[_LineRoiCount];
                    if (_isUseRoiOffset == true)
                    {
                        for (int i = 0; i < _LineRoiCount; i++)
                        {
                            if (_LineRoiY == null) continue;
                            LineRoiYCollect[i] = new HTuple();
                            LineRoiXCollect[i] = new HTuple();
                            LineRoiYCollect[i] = _LineRoiY + _RoiOffsetRow;
                            LineRoiXCollect[i] = _LineRoiX + _RoiOffsetCol;
                        }
                    }
                    else
                    {
                        LineRoiYCollect[0] = _LineRoiY;
                        LineRoiXCollect[0] = _LineRoiX;
                    }
                    HTuple Row0, Row1, Col0, Col1, Nr, Nc;
                    q_bFitLineSuccess = FitLine(null, m_Line_Elements, m_Line_Caliper_Height, m_Line_Caliper_Width, m_Line_Sigma,
                        m_Line_Threshold, m_Line_Transition, m_Line_Point_Select, LineRoiYCollect, LineRoiXCollect,
                         out Col0, out Row0, out Col1, out Row1, out Nc, out Nr);
                    _ResultLineFindSuccessColl.Add(q_bFitLineSuccess);
                    m_Line_X_Begin.Append(Col0);
                    m_Line_X_End.Append(Col1);
                    m_Line_Y_Begin.Append(Row0);
                    m_Line_Y_End.Append(Row1);
                    _Nr.Append(Nr);
                    _Nc.Append(Nc);
                }
            }
            catch (HalconException HDevExpDefaultException)
            {

            }

            if (q_bFitLineSuccess == false)
            {
                //qPictureControl.AddRoi("red", false, m_Line_Regions, "直线查找失败区域");
                //  HOperatorSet.DispObj(m_Line_Regions, Window);
                return false;
            }

            return true;
        }


        /// <summary> 执行查找直线，仿射矩阵使用的是字典,必然使用的是定位
        /// 
        /// </summary>
        /// <param name="qPictureControl"></param>
        /// <param name="HomMat2D"></param>
        /// <returns></returns>
        public bool Execute(Dictionary<string, HTuple> HomMat2D)
        {
            // if (HomMat2D==null) 
            ClearResult();
            HOperatorSet.GenEmptyObj(out m_Line_Trans_Regions);
            HOperatorSet.GenEmptyObj(out m_Line_xld);
            m_Line_Edges_X = new HTuple();
            m_Line_Edges_Y = new HTuple();
            HOperatorSet.GenEmptyObj(out _LineXldCollect);

            bool q_bFitLineSuccess = false;
            try
            {
                //判断图像是否为空
                if (!ObjectValided(m_InputImage))
                {

                    for (int i = 0; i < HomMat2D.Count; i++)
                    {
                        _ResultLineFindSuccessColl.Add(false);
                    }
                    return false;
                }
                int Hom2DNum = HomMat2D.Count;
                HTuple[] LineRoiXCollect;
                HTuple[] LineRoiYCollect;
                for (int i = 0; i < Hom2DNum; i++)
                {
                    if (HomMat2D[i.ToString()].Length == 6)
                    {
                        HTuple Hom2DIndex = HomMat2D[i.ToString()];
                        //     HObject ThresholdRegion;
                        AffineRoiPt(Hom2DIndex, out LineRoiXCollect, out LineRoiYCollect);

                        HTuple Row0, Row1, Col0, Col1, Nr, Nc;
                        q_bFitLineSuccess = FitLine(ThresholdRegion,
                        m_Line_Elements, m_Line_Caliper_Height, m_Line_Caliper_Width, m_Line_Sigma,
                        m_Line_Threshold, m_Line_Transition, m_Line_Point_Select, LineRoiYCollect, LineRoiXCollect,
                        out Col0, out Row0, out Col1, out Row1, out Nc, out Nr);
                        _ResultLineFindSuccessColl.Add(q_bFitLineSuccess);

                        if (ShowSearchLine)
                        {
                            HTuple AffineX = new HTuple(), AffineY = new HTuple();
                            HObject SearchRoi;
                            for (int j = 0; j < _LineRoiCount; j++)
                            {
                                if (LineRoiYCollect[j] != null && LineRoiYCollect[j].TupleLength() > 0)
                                {
                                    AffineX.Append(LineRoiXCollect[j]);
                                    AffineY.Append(LineRoiYCollect[j]);
                                }
                            }
                            HOperatorSet.GenContourPolygonXld(out SearchRoi, AffineY, AffineX);
                            //qPictureControl.AddRoi("blue", false, SearchRoi, "搜索直线");
                        }

                        if (q_bFitLineSuccess == false)
                        {
                            m_Line_X_Begin.Append(-1);
                            m_Line_X_End.Append(-1);
                            m_Line_Y_Begin.Append(-1);
                            m_Line_Y_End.Append(-1);
                            _Nr.Append(-1);
                            _Nc.Append(-1);

                        }
                        else
                        {
                            m_Line_X_Begin.Append(Col0);
                            m_Line_X_End.Append(Col1);
                            m_Line_Y_Begin.Append(Row0);
                            m_Line_Y_End.Append(Row1);
                            _Nr.Append(Nr);
                            _Nc.Append(Nc);
                        }
                    }
                    else
                    {
                        m_Line_X_Begin.Append(-1);
                        m_Line_X_End.Append(-1);
                        m_Line_Y_Begin.Append(-1);
                        m_Line_Y_End.Append(-1);
                        _Nr.Append(-1);
                        _Nc.Append(-1);
                        _PtOkNum.Add(0);
                        _ResultLineFindSuccessColl.Add(false);
                    }
                }


            }
            catch (HalconException HDevExpDefaultException)
            {

            }

            if (q_bFitLineSuccess == false)
            {
                // qPictureControl.AddRoi("red", false, m_Line_Regions, "直线查找失败区域");
                return false;
            }

            return true;
        }
        /// <summary> 添加所有的Object
        /// 
        /// </summary>
        /// <param name="qPictureControl"></param>
        private void ShowObject()
        {
            if (_ShowFindLinePt == 1)
            {
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, m_Line_Edges_Y, m_Line_Edges_X, 20, 0);
                //qPictureControl.AddRoi("yellow", false, cross, "找到的点");
            }
            if (_ShowFitPt == 1)
            {
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, _LineFitY, _LineFitX, 20, 0);
                //qPictureControl.AddRoi("green", false, cross, "拟合的点");
            }
            if (_ShowDeleteLinePt == 1)
            {
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, _LineDeleteY, _LineDeleteX, 20, 0);
                // qPictureControl.AddRoi("red", false, cross, "剔除的点");
            }

            if (_ShowFindLineRect == 1)
            {
                for (int i = 0; i < _LineRoiCount; i++)
                {
                    //if (_ListRoiOk[i] == true)
                        
                          //  string color = "green";
                           // if (_ListPtOk[i][j] == false) color = "red";
                            //qPictureControl.AddRoi(color, false, _ListMeasureRect[i][j], "MeasureRect");
                        
                }
            }

            if (ShowLineResult == true)
            {
                if (m_Line_xld.IsInitialized() == true)
                {
                    //qPictureControl.AddRoi("green", false, m_Line_xld, "LineResult");
                    //  HOperatorSet.ConcatObj(m_Line_xld, _LineXldCollect, out _LineXldCollect);
                }
            }

            ShowDirectonArrow();
        }

        public void ShowDirectonArrow()
        {

            if (ShowDirection == true)
            {
                //  if(_dir)

                if (_DitectionMea == null) return;
                if (_DitectionMea.IsInitialized() == true)
                {
                    if (_LineRoiX != null)
                        //if (_LineRoiX[i].TupleNumber() > 0)
                            ;
                    //qPictureControl.AddRoi("yellow", false, _DitectionMea[i], "Arrow");
                }
            }
        }



        public void Concat_Obj(ref HObject Obj1, ref HObject Obj2, ref HObject Obj3)
        {
            if (!Obj1.IsInitialized())
            {
                HOperatorSet.GenEmptyObj(out Obj1);
            }
            if (!Obj2.IsInitialized())
            {
                HOperatorSet.GenEmptyObj(out Obj2);
            }
            //             if (!(Obj1.IsInitialized()) || (Obj1.CountObj() < 1))
            //             {
            //                 HOperatorSet.CopyObj(Obj1,out Obj3,1,-1);
            //             }
            //             else
            {
                HOperatorSet.ConcatObj(Obj1, Obj2, out Obj3);
                HTuple Count = Obj3.CountObj();

            }
        }
        public void FindLinePt(HObject ho_Image, out HObject ho_Regions, int RoiIndex, HTuple hv_Elements,
    HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
    HTuple hv_Transition, HTuple hv_Select, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
    HTuple hv_Column2, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables 

            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null;


            // Local control variables 

            HTuple hv_Width, hv_Height, hv_ATan, hv_i;
            HTuple hv_RowC = new HTuple(), hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple(), hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple(), hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple(), hv_tRow = new HTuple();
            HTuple hv_tCol = new HTuple(), hv_t = new HTuple(), hv_Number = new HTuple();
            HTuple hv_j = new HTuple();

            HTuple hv_DetectWidth_COPY_INP_TMP = hv_DetectWidth.Clone();
            HTuple hv_Select_COPY_INP_TMP = hv_Select.Clone();
            HTuple hv_Transition_COPY_INP_TMP = hv_Transition.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);

            try
            {
                //获取图像尺寸
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //产生一个空显示对象，用于显示
                if (ho_Regions != null)
                    ho_Regions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Regions);
                //初始化边缘坐标数组
                hv_ResultRow = new HTuple();
                hv_ResultColumn = new HTuple();
                //产生直线xld
                ho_RegionLines.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                    hv_Column1.TupleConcat(hv_Column2));
                //存储到显示对象
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_RegionLines, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;
                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());

                //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
                for (hv_i = 1; hv_i.Continue(hv_Elements, 1); hv_i = hv_i.TupleAdd(1))
                {

                    //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                    if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                        //判断是否超出图像,超出不检测边缘
                        if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                            new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                            hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                        {
                            continue;
                        }
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        hv_DetectWidth_COPY_INP_TMP = hv_Distance.Clone();
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_Distance / 2);
                    }
                    else
                    {
                        //如果有多个测量矩形，产生该测量矩形xld
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1));
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1));
                        //判断是否超出图像,超出不检测边缘
                        if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                            new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                            hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                        {
                            continue;
                        }
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_DetectWidth_COPY_INP_TMP / 2);
                    }

                    //把测量矩形xld存储到显示对象
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HObject rect;

                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle, out ho_Regions);
                    HOperatorSet.GenRectangle2(out rect, hv_RowC, hv_ColC, hv_ATan, hv_DetectHeight / 2, hv_DetectWidth_COPY_INP_TMP / 2);
                    _ListMeasureRect=rect;

                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                    {
                        //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                        hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2, 25, 25);
                        HObject Arrow;
                        gen_arrow_contour_xld(out Arrow, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2, 25, 25);
                        _DitectionMea = Arrow;

                        //把xld存储到显示对象
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                    //产生测量对象句柄
                    HOperatorSet.GenMeasureRectangle2(hv_RowC, hv_ColC, hv_ATan, hv_DetectHeight / 2,
                        hv_DetectWidth_COPY_INP_TMP / 2, hv_Width, hv_Height, "nearest_neighbor",
                        out hv_MsrHandle_Measure);

                    //设置极性
                    if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative"))) != 0)
                    {
                        hv_Transition_COPY_INP_TMP = "negative";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive"))) != 0)
                        {

                            hv_Transition_COPY_INP_TMP = "positive";
                        }
                        else
                        {
                            hv_Transition_COPY_INP_TMP = "all";
                        }
                    }
                    //设置边缘位置。最强点是从所有边缘中选择幅度绝对值最大点，需要设置为'all'
                    if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first"))) != 0)
                    {
                        hv_Select_COPY_INP_TMP = "first";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last"))) != 0)
                        {

                            hv_Select_COPY_INP_TMP = "last";
                        }
                        else
                        {
                            hv_Select_COPY_INP_TMP = "all";
                        }
                    }
                    //检测边缘
                    HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold,
                        hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge,
                        out hv_Amplitude, out hv_Distance);
                    //if (hv_RowEdge != null)
                    //    if (hv_RowEdge.Length > 0)
                    //        _ListPtOk[RoiIndex].Add(true);
                    //    else _ListPtOk[RoiIndex].Add(false);
                    //else _ListPtOk[RoiIndex].Add(false);


                    ////清除测量对象句柄
                    HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);

                    //临时变量初始化
                    //tRow，tCol保存找到指定边缘的坐标
                    hv_tRow = 0;
                    hv_tCol = 0;
                    //t保存边缘的幅度绝对值
                    hv_t = 0;
                    //找到的边缘必须至少为1个
                    HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                    {
                        continue;
                    }
                    //有多个边缘时，选择幅度绝对值最大的边缘
                    for (hv_j = 0; hv_j.Continue(hv_Number - 1, 1); hv_j = hv_j.TupleAdd(1))
                    {
                        if ((int)(new HTuple(((((hv_Amplitude.TupleSelect(hv_j))).TupleAbs())).TupleGreater(
                            hv_t))) != 0)
                        {

                            hv_tRow = hv_RowEdge.TupleSelect(hv_j);
                            hv_tCol = hv_ColEdge.TupleSelect(hv_j);
                            hv_t = ((hv_Amplitude.TupleSelect(hv_j))).TupleAbs();
                        }
                    }
                    //把找到的边缘保存在输出数组
                    if ((int)(new HTuple(hv_t.TupleGreater(0))) != 0)
                    {
                        hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                        hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                    }
                }

                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        /// <summary> 查找多个直线区域内的点
        /// 
        /// </summary>
        /// <param name="ho_Image"></param> 输入图像，找到该图像上区域上的点
        /// <param name="hv_Elements"></param>找的点数
        /// <param name="hv_DetectHeight"></param> 检测的高度
        /// <param name="hv_DetectWidth"></param> 检测的宽度
        /// <param name="hv_Sigma"></param> 平滑
        /// <param name="hv_Threshold"></param> 对比度
        /// <param name="hv_Transition"></param> 查找方向 从黑到白 或从白到黑
        /// <param name="hv_Select"></param> 选择找到的点
        /// <returns></returns>
        public bool FitLine(HObject ThresholdRegion, HTuple hv_Elements,
    HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
    HTuple hv_Transition, HTuple hv_Select, HTuple[] LineRoiY, HTuple[] LineRoiX,
            out HTuple BeginX, out HTuple BeginY, out HTuple EndX, out HTuple EndY, out HTuple Nc, out HTuple Nr)
        {
            BeginY = new HTuple();
            BeginX = new HTuple();
            EndX = new HTuple();
            EndY = new HTuple();
            Nc = new HTuple();
            Nr = new HTuple();
            m_Line_Edges_X = new HTuple();
            m_Line_Edges_Y = new HTuple();
            _LineDeleteX = new HTuple();
            _LineDeleteY = new HTuple();
            _LineFitX = new HTuple();
            _LineFitY = new HTuple();
            for (int i = 0; i < _LineRoiCount; i++)
            {
                if (_ListMeasureRect != null)
                    _ListMeasureRect = null;
                //if (_ListPtOk[i] != null)
                //    _ListPtOk[i].Clear();
            }

            //  HTuple Row0, Row1, Col0, Col1, Nr, Nc;
            bool LineFineJudge = false;
            List<HTuple> ReaminX = new List<HTuple>();
            List<HTuple> ReaminY = new List<HTuple>();
            List<HTuple> DeleteX = new List<HTuple>();
            List<HTuple> DeleteY = new List<HTuple>();
            for (int i = 0; i < _LineRoiCount; i++)
            {
                //if (LineRoiY[i] == null)
                //{

                //    _ListRoiOk[i] = false;
                //    continue;
                //}
                //if (LineRoiY[i].TupleNumber() <= 0)
                //{
                //    _ListRoiOk[i] = false;
                //    continue;
                //}

                if (_ListRoiOk[i] == false) continue;

                //  _ListRoiOk[i] = true;

                HTuple Line_Edges_Y, Line_Edges_X, Line_Remain_Y, Line_Remain_X, Line_Delete_Y, Line_Delete_X;
                HObject CropImage;
                HOperatorSet.GenEmptyObj(out CropImage);
                //    CropImage = qPictureControl._InputImage.Clone();
                if (_IsThreshold == true)
                {
                    if (ThresholdRegion == null || ThresholdRegion.IsInitialized() == false || ThresholdRegion.CountObj() <= 0)
                    {
                        // CropImage = qPictureControl._InputImage;
                    }
                    else
                    {
                        HObject Domain, Threshold;
                        HObject DomainConcate;
                        HOperatorSet.GenEmptyObj(out Threshold);
                        HOperatorSet.GenEmptyObj(out DomainConcate);
                        HOperatorSet.GenEmptyObj(out Domain);
                        for (int j = 1; j <= ThresholdRegion.CountObj(); j++)
                        {
                            HObject ThresholdRegionSelect;
                            HOperatorSet.SelectObj(ThresholdRegion, out ThresholdRegionSelect, j);
                            HOperatorSet.ReduceDomain(m_InputImage, ThresholdRegionSelect, out Domain);
                            HOperatorSet.ConcatObj(DomainConcate, Domain, out DomainConcate);

                        }
                        HOperatorSet.Threshold(DomainConcate, out Threshold, _MinThreshold, _MaxThreshold);

                        HTuple Width, Height;
                        HOperatorSet.GetImageSize(m_InputImage, out Width, out Height);
                        HOperatorSet.RegionToBin(Threshold, out CropImage, _ForeGary, _BackGray, Width, Height);
                        if (_IsShowBinRegion == true)
                        {
                            // qPictureControl.AddRoi("white", true, Threshold, "ThresholdRegion");
                        }
                        Domain.Dispose();
                        Threshold.Dispose();
                        DomainConcate.Dispose();
                    }
                }
                else
                {
                    CropImage = m_InputImage;
                }

                FindLinePt(CropImage, out m_Line_Trans_Regions, i, m_Line_Elements, m_Line_Caliper_Height, m_Line_Caliper_Width,
                m_Line_Sigma, m_Line_Threshold, m_Line_Transition, m_Line_Point_Select,
                LineRoiY[i][0], LineRoiX[i][0], LineRoiY[i][1], LineRoiX[i][1],
                out Line_Edges_Y, out Line_Edges_X);
                //if (_IsThreshold == true)
                //    if (CropImage != null)
                //        CropImage.Dispose();
                DeleteOutliers(i, LineRoiY[i][0], LineRoiX[i][0], LineRoiY[i][1], LineRoiX[i][1], Line_Edges_Y, Line_Edges_X,
                    out Line_Remain_Y, out Line_Remain_X, out Line_Delete_Y, out Line_Delete_X);
                // if (Line_Remain_Y.Length < 2) return false;

                ReaminX.Add(Line_Remain_X);
                ReaminY.Add(Line_Remain_Y);
                DeleteX.Add(Line_Delete_X);
                DeleteY.Add(Line_Delete_Y);



                m_Line_Edges_Y.Append(ReaminY[i]);
                m_Line_Edges_X.Append(ReaminX[i]);
                _LineDeleteX.Append(DeleteX[i]);
                _LineDeleteY.Append(DeleteY[i]);
                _LineFitY.Append(ReaminY[i]);
                _LineFitX.Append(ReaminX[i]);

                //  _PtOkNum.Append(ReaminY[i].Length);
                _PtOkNum.Add(ReaminY[i].Length);
                if (ReaminX[i].Length < m_Line_Elements * _LineFitScore)
                {
                    LineFineJudge = false;
                    break;
                }
                LineFineJudge = true;

            }

            if (LineFineJudge == true)
            {
                pts_to_best_line(out m_Line_xld, _LineFitY, _LineFitX, m_Line_Min_Points_Num,
                    out BeginY, out BeginX, out EndY, out EndX, out Nr, out Nc);
            }

            ShowObject();
            if (BeginY == null && BeginY.Length <= 0) return false;
            return LineFineJudge;

        }

        /// <summary>在找到的点钟剔除距离过大的点
        /// 
        /// </summary>
        /// <param name="RoiX"></param>
        /// <param name="RoiY"></param>
        /// <returns></returns>
        private bool DeleteOutliers(int RoiIndex, HTuple RoiY1, HTuple RoiX1, HTuple RoiY2, HTuple RoiX2, HTuple PtY, HTuple PtX,
            out HTuple RemainPtY, out HTuple RemainPtX, out HTuple DeletePtY, out HTuple DeletePtX)
        {
            HTuple Distance;
            HTuple DeleteIndex = new HTuple();
            HOperatorSet.DistancePl(PtY, PtX, RoiY1, RoiX1, RoiY2, RoiX2, out Distance);

            for (int i = 0; i < Distance.Length; i++)
            {
                if (Distance[i] > _DeleteDist)
                {
                    //i为超出最大距离的索引
                    DeleteIndex.Append(i);
                    //if (_ListPtOk[RoiIndex].Count > i)
                    //    _ListPtOk[RoiIndex][i] = false;
                }

            }
            RemainPtX = PtX.TupleRemove(DeleteIndex);
            RemainPtY = PtY.TupleRemove(DeleteIndex);
            DeletePtX = PtX.TupleSelect(DeleteIndex);
            DeletePtY = PtY.TupleSelect(DeleteIndex);


            return false;
        }


        public bool ObjectValided(HObject Obj)
        {
            if (null == Obj) return false;
            if (!Obj.IsInitialized())
            {
                return false;
            }
            if (Obj.CountObj() < 1)
            {
                return false;
            }
            return true;
        }
        public void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
            HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {


            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables 

            HObject ho_TempArrow = null;


            // Local control variables 

            HTuple hv_Length, hv_ZeroLengthIndices, hv_DR;
            HTuple hv_DC, hv_HalfHeadWidth, hv_RowP1, hv_ColP1, hv_RowP2;
            HTuple hv_ColP2, hv_Index;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            try
            {
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
                HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
                //
                //Mark arrows with identical start and end point
                //(set Length to -1 to avoid division-by-zero exception)
                hv_ZeroLengthIndices = hv_Length.TupleFind(0);
                if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
                {
                    hv_Length[hv_ZeroLengthIndices] = -1;
                }
                //
                //Calculate auxiliary variables.
                hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
                hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
                hv_HalfHeadWidth = hv_HeadWidth / 2.0;
                //
                //Calculate end points of the arrow head.
                hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
                hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
                hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
                hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
                //
                //Finally create output XLD contour for each input point pair
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                    {
                        //Create_ single points for arrows with identical start and end point
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(
                            hv_Index), hv_Column1.TupleSelect(hv_Index));
                    }
                    else
                    {
                        //Create arrow contour
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
                    OTemp[SP_O] = ho_Arrow.CopyObj(1, -1);
                    SP_O++;
                    ho_Arrow.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_TempArrow, out ho_Arrow);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                }
                ho_TempArrow.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_TempArrow.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {


            // Local control variables 

            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();

            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 

            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically
            //   for each new textline.
            //Box: If set to 'true', the text is written within a white box.
            //
            //prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //display text box depending on text size
            if ((int)(new HTuple(hv_Box.TupleEqual("true"))) != 0)
            {
                //calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                HOperatorSet.SetColor(hv_WindowHandle, "white");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            else if ((int)(new HTuple(hv_Box.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Box";
                throw new HalconException(hv_Exception);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }
        public void pts_to_best_line(out HObject ho_Line, HTuple hv_Rows, HTuple hv_Cols,
    HTuple hv_ActiveNum, out HTuple hv_Row1, out HTuple hv_Column1, out HTuple hv_Row2,
    out HTuple hv_Column2, out HTuple hv_Nr, out HTuple hv_Nc)
        {


            // Local iconic variables 

            HObject ho_Contour = null;


            // Local control variables 

            HTuple hv_Length;
            HTuple hv_Dist = new HTuple(), hv_Length1 = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Contour);

            try
            {
                //初始化
                hv_Row1 = 0;
                hv_Column1 = 0;
                hv_Row2 = 0;
                hv_Column2 = 0;
                hv_Nr = 0;
                hv_Nc = 0;
                //产生一个空的直线对象，用于保存拟合后的直线
                ho_Line.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Line);
                //计算边缘数量
                HOperatorSet.TupleLength(hv_Cols, out hv_Length);
                //当边缘数量不小于有效点数时进行拟合
                if ((int)((new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum))).TupleAnd(
                    new HTuple(hv_ActiveNum.TupleGreater(1)))) != 0)
                {
                    //halcon的拟合是基于xld的，需要把边缘连接成xld
                    HTuple RowsSort = new HTuple(), ColsSort, SortIndex;
                    HOperatorSet.TupleSortIndex(hv_Cols, out SortIndex);
                    HOperatorSet.TupleSort(hv_Cols, out ColsSort);
                    for (int i = 0; i < SortIndex.Length; i++)
                    {
                        RowsSort.Append(hv_Rows[SortIndex[i].I]);
                    }

                    HTuple RowsSort1, ColsSort1 = new HTuple(), SortIndex1;
                    HOperatorSet.TupleSortIndex(hv_Rows, out SortIndex1);
                    HOperatorSet.TupleSort(hv_Rows, out RowsSort1);
                    for (int i = 0; i < SortIndex1.Length; i++)
                    {
                        ColsSort1.Append(hv_Cols[SortIndex1[i].I]);
                    }
                    double Dist1, Dist2;
                    Dist1 = Math.Abs(ColsSort[0].D - ColsSort[ColsSort.Length - 1].D);
                    Dist2 = Math.Abs(RowsSort1[0].D - RowsSort1[RowsSort1.Length - 1].D);
                    if (Dist1 < Dist2)
                    {
                        RowsSort = RowsSort1;
                        ColsSort = ColsSort1;
                    }

                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, RowsSort, ColsSort);
                    //拟合直线。使用的算法是'tukey'，其他算法请参考fit_line_contour_xld的描述部分。
                    HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out hv_Row1,
                        out hv_Column1, out hv_Row2, out hv_Column2, out hv_Nr, out hv_Nc, out hv_Dist);
                    //判断拟合结果是否有效：如果拟合成功，数组中元素的数量大于0
                    HOperatorSet.TupleLength(hv_Dist, out hv_Length1);
                    if ((int)(new HTuple(hv_Length1.TupleLess(1))) != 0)
                    {
                        ho_Contour.Dispose();

                        return;
                    }
                    //根据拟合结果，产生直线xld
                    ho_Line.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2),
                        hv_Column1.TupleConcat(hv_Column2));
                    //ho_Line = ho_Contour;
                }

                //   ho_Contour.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                // ho_Contour.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public HObject GetROIRegionTrans()
        {
            return m_Line_Trans_Regions;
        }
        //参数设置
        public void SetRoiWidth(int Width)
        {
            m_Line_Caliper_Width = Width;
        }

        public void SetRoiHeight(int Height)
        {
            m_Line_Caliper_Height = Height;
        }

        public void SetTransition(int iTransition)
        {
            if (0 == iTransition)
            {
                m_Line_Transition = "negative";
            }
            else if (1 == iTransition)
            {
                m_Line_Transition = "positive";

            }
            else if (2 == iTransition)
            {
                m_Line_Transition = "all";
            }
        }
        public void SetSelectPointWay(int iSelectPointWay)
        {

            if (0 == iSelectPointWay)
            {
                m_Line_Point_Select = "first";
            }
            else if (1 == iSelectPointWay)
            {
                m_Line_Point_Select = "last";
            }
            else if (2 == iSelectPointWay)
            {
                m_Line_Point_Select = "all";
            }


        }

        public void SetThreshold(int Threshold)
        {
            m_Line_Threshold = Threshold;
        }
        public void SetSigma(double Sigma)
        {
            m_Line_Sigma = Sigma;
        }
        public void SetElements(int NumPoint)
        {
            m_Line_Elements = NumPoint;
        }

        public void SetDeleteDist(double Distance)
        {
            _DeleteDist = Distance;
        }
        public double GetDeleteDist()
        {
            return _DeleteDist;
        }
        public int GetRoiWidth()
        {
            return m_Line_Caliper_Width;
        }
        public int GetRoiHeight()
        {
            return m_Line_Caliper_Height;
        }

        public int GetThreshold()
        {
            return m_Line_Threshold;
        }
        public double GetSigma()
        {
            return m_Line_Sigma;
        }
        public int GetElements()
        {
            return m_Line_Elements;
        }
        public int GetSelectPointWay()
        {
            if ("first" == m_Line_Point_Select)
            {
                return 0;
            }
            else if ("last" == m_Line_Point_Select)
            {
                return 1;
            }
            else if ("all" == m_Line_Point_Select)
            {
                return 2;
            }
            return 0;

        }
        public int GetTransition()
        {
            if ("negative" == m_Line_Transition)
            {
                return 0;
            }
            else if ("positive" == m_Line_Transition)
            {
                return 1;

            }
            else if ("all" == m_Line_Transition)
            {
                return 2;
            }
            return 0;
        }

        //获取结果图形
        public HObject GetResultLineObj()
        {
            return m_Line_xld;
        }


        //获取结果
        public HTuple GetResultBeginCol()
        {
            return m_Line_X_Begin;
        }
        public HTuple GetResultEndCol()
        {
            return m_Line_X_End;
        }
        public HTuple GetResultBeginRow()
        {
            return m_Line_Y_Begin;
        }
        public HTuple GetResultEndRow()
        {
            return m_Line_Y_End;
        }

        public HTuple GetResultEdgeRow()
        {
            return m_Line_Edges_Y;
        }
        public HTuple GetResultEdgeCol()
        {
            return m_Line_Edges_X;
        }

        public HTuple GetResultAnlge()
        {
            HTuple Angle = new HTuple();
            for (int i = 0; i < m_Line_X_Begin.Length; i++)
            {
                // if (_ResultLineFindSuccessColl[i] == true)
                {
                    if (m_Line_X_Begin[i].D == -1)
                    {
                        Angle.Append(-1);
                        continue;
                    }
                    double CalAngle = GetResultLineAngle(m_Line_X_Begin[i], m_Line_Y_Begin[i], m_Line_X_End[i], m_Line_Y_End[i], _Nc[i], _Nr[i]);
                    Angle.Append(CalAngle);
                }
            }
            return Angle;
        }

        /// <summary>得到拟合出来的直线的弧度，可获得多条拟合直线
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetResultLinePhi()
        {

            HTuple Phi = new HTuple();
            for (int i = 0; i < m_Line_X_Begin.Length; i++)
            {
                double phi = GetResultLinePhi(m_Line_X_Begin[i], m_Line_Y_Begin[i], m_Line_X_End[i], m_Line_Y_End[i], _Nc[i], _Nr[i]);
                //if (CalAngle < -180) CalAngle += 360;
                //else if (CalAngle > 360) CalAngle -= 360;
                Phi.Append(-phi);

            }

            return Phi;
        }

        public HTuple GetResultScore()
        {
            HTuple Score = new HTuple();
            for (int i = 0; i < m_Line_X_Begin.Length; i++)
            {

                if (m_Line_X_Begin[i].D == -1)
                {
                    Score.Append(0);
                    continue;
                }
                double PtCount = 0;
                double OkPtCount = 0;
                //   for (int j = 0; j < _PtOkNum.Length; j++)
                //       for (int j = 0; j < _LineRoiCount; j++)
                {
                    //  if (_ListRoiOk[j] == true)
                    {
                        PtCount += GetElements();
                        OkPtCount += _PtOkNum[i];
                    }
                }
                //  OkPtCount += _PtOkNum[i];
                Score.Append(OkPtCount / PtCount);
            }
            return Score;
        }
        public void ClearLineRoi()
        {
            _LineRoiX = new HTuple[_LineRoiCount];
            _LineRoiY = new HTuple[_LineRoiCount];
        }
        public void ClearLineRoiIndex(int RoiIndex)
        {
            _LineRoiX[RoiIndex] = new HTuple();
            _LineRoiY[RoiIndex] = new HTuple();
        }

        private void ClearResult()
        {

            _ResultLineFindSuccessColl = new List<bool>();
            m_Line_X_Begin = new HTuple();
            m_Line_X_End = new HTuple();
            m_Line_Y_Begin = new HTuple();
            m_Line_Y_End = new HTuple();
            _Nr = new HTuple();
            _Nc = new HTuple();
            //  _PtOkNum = new HTuple();
            _PtOkNum.Clear();
            for (int i = 0; i < _LineRoiCount; i++)
            {
                //if (_ListMeasureRect[i] != null)
                //    _ListMeasureRect[i].Clear();
                //if (_ListPtOk[i] != null)
                //    _ListPtOk[i].Clear();
            }
        }


        public double GetResultLinePhi(HTuple RowS, HTuple ColS, HTuple RowE, HTuple ColE, HTuple Nc, HTuple Nr)
        {
            if (RowS.TupleLength() < 1)
            {
                return 366;
            }

            PointF p1 = new PointF(0, 0), p2 = new PointF(0, 0);
            p1.X = (float)ColS[0].D;
            p1.Y = (float)RowS[0].D;

            p2.X = (float)ColE[0].D;
            p2.Y = (float)RowE[0].D;

            double nc = Nc[0].D;
            double nr = Nr[0].D;
            double k = -nc / nr;
            //    double k = -nr / nc;
            //图像坐标角度
            // double imageQ = RadToAngle * Math.Tan(k);
            if (nr == 0) return 366;

            double phi = Math.Atan(k);
            return phi;

        }

        /// <summary> 计算直线的角度，逆时针为正方向
        /// 
        /// </summary>
        /// <param name="RowS"></param>
        /// <param name="ColS"></param>
        /// <param name="RowE"></param>
        /// <param name="ColE"></param>
        /// <param name="Nc"></param>
        /// <param name="Nr"></param>
        /// <returns></returns>
        public double GetResultLineAngle(HTuple RowS, HTuple ColS, HTuple RowE, HTuple ColE, HTuple Nc, HTuple Nr)
        {

            if (RowS.TupleLength() < 1)
            {
                return 366;
            }

            PointF p1 = new PointF(0, 0), p2 = new PointF(0, 0);
            p1.X = (float)ColS[0].D;
            p1.Y = (float)RowS[0].D;

            p2.X = (float)ColE[0].D;
            p2.Y = (float)RowE[0].D;

            double nc = Nc[0].D;
            double nr = Nr[0].D;
            double k = -nc / nr;
            //    double k = -nr / nc;
            //图像坐标角度
            // double imageQ = RadToAngle * Math.Tan(k);
            double imageQ = RadToAngle * Math.Atan(k);
            double ret = 0;
            if ((p1.X == p2.X) && (p1.Y == p2.Y))
            {
                ret = 0;
            }
            else
            {
                if (p1.X == p2.X)
                {
                    ret = 90;
                }
                else
                {
                    if (Math.Abs(p1.X - p2.X) > Math.Abs(p1.Y - p2.Y))   //水平角度
                    {
                        ret = -imageQ;
                    }
                    else
                    {
                        ret = -imageQ;  //竖直角度
                        if (ret < 0)
                        {
                            ret = ret + 180;
                        }
                    }
                }
            }

            return ret;
        }

        public List<bool> GetResultFindLineSuccessColl()
        {
            return _ResultLineFindSuccessColl;
        }

    }
}


