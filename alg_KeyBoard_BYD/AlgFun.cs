using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alg_KeyBoard_BYD
{
    public class AlgFun : AlgBaseFun
    {
        public AlgFun()
        {
            StructAlgResult structAlgResult = new StructAlgResult(1);
        }

        public void CreateModel(string pn, string filePath, out string modelImgSaveFilePath, HObject img, string drawType, double[] drawParam, int contrast, out HTuple ModelID, bool isSave = false)
        {
            ModelID = null;
            HObject rect, reduceImage, cropImage;
            HOperatorSet.GenEmptyObj(out rect);
            HOperatorSet.GenEmptyObj(out reduceImage);
            HOperatorSet.GenEmptyObj(out cropImage);
            modelImgSaveFilePath = "";
            try
            {
                rect.Dispose();

                GetRect(drawType, drawParam, ref rect);

                reduceImage.Dispose();
                HOperatorSet.ReduceDomain(img, rect, out reduceImage);
                HOperatorSet.CropDomain(reduceImage, out cropImage);
                if (isSave)
                {
                    string modelName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_model";
                    modelImgSaveFilePath = filePath + "/" + modelName;

                    string[] files = FileHelper.GetFileNames(filePath);
                    foreach (var item in files)
                    {
                        if (item.Contains("_model"))
                        {
                            FileHelper.DeleteFile(item);
                        }
                    }
                    //bool isExist = FileHelper.Contains(filePath, "_model");
                    HOperatorSet.WriteImage(reduceImage, "bmp", 0, modelImgSaveFilePath);

                }
                //HOperatorSet.CreateShapeModel(reduceImage, "auto", -0.1, 0.5, "auto", "auto", "use_polarity", "auto", contrast, out ModelID);
                HOperatorSet.CreateScaledShapeModel(reduceImage, "auto", 0, (new HTuple(2)).TupleRad(), "auto", 0.8, 1.2, "auto", "auto", "use_polarity", "auto", "auto", out ModelID);
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "DoWithImage-CreateModel");
            }
            finally
            {
                rect.Dispose();
                reduceImage.Dispose();
                cropImage.Dispose();
            }

        }

        public bool FindModel(HObject img, HTuple ModelID, string roiDrawType, double[] roiDrawParam,
            out HTuple Row, out HTuple Column, out HTuple Angle, out HTuple Score)
        {
            Row = null; Column = null; Angle = null; Score = null;
            HObject rect, reduceImage, cropImage;
            HOperatorSet.GenEmptyObj(out rect);
            HOperatorSet.GenEmptyObj(out reduceImage);
            HOperatorSet.GenEmptyObj(out cropImage);

            rect.Dispose();

            try
            {
                GetRect(roiDrawType, roiDrawParam, ref rect);

                reduceImage.Dispose();
                HOperatorSet.ReduceDomain(img, rect, out reduceImage);
                //HOperatorSet.CropDomain(reduceImage, out cropImage);
                //
                HOperatorSet.FindShapeModel(reduceImage, ModelID, -0.2, 1.5, 0.05, 1, 0.05, "least_squares", 0, 0.9, out Row, out Column, out Angle, out Score);

                return Score.TupleLength() > 0;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "DoWithImage-FindModel");
                return false;
            }
            finally
            {
                rect.Dispose();
                reduceImage.Dispose();
                cropImage.Dispose();
            }
        }

        public void AffineImage(HObject img, HTuple oRow, HTuple oCol, HTuple oAngle, HTuple nRow, HTuple nCol, HTuple nAngle, out HObject affineImage)
        {
            HOperatorSet.GenEmptyObj(out affineImage);
            HTuple homMat2d = null;
            //HOperatorSet.VectorAngleToRigid(nRow, nCol, nAngle, oRow, oCol, oAngle, out homMat2d);
            HOperatorSet.VectorAngleToRigid(nRow, nCol, nAngle, oRow, oCol, oAngle, out homMat2d);

            affineImage.Dispose();
            HOperatorSet.AffineTransImage(img, out affineImage, homMat2d, "constant", "false");
        }

        public void GetRect(string drawType, double[] drawParam, ref HObject rect)
        {

            if (drawType == "rectangle1")
            {
                HOperatorSet.GenRectangle1(out rect, drawParam[0], drawParam[1], drawParam[2], drawParam[3]);

            }
            else
            {
                HOperatorSet.GenRectangle2(out rect, drawParam[0], drawParam[1], drawParam[2], drawParam[3], drawParam[4]);
            }
        }

        public void Detector_MLB_jumper(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);

                HObject showRegion = new HObject();
                HObject outImg = new HObject();
                HTuple result = null;
                HOperatorSet.GenEmptyObj(out showRegion);
                // halcon原始函数
                Mesa_jumper_flex_cosmetic(inputImg, nodeInfo.roi1, out showRegion, out result);
                // HOperatorSet.ConcatObj(nodeInfo.roi1,  nodeInfo.searchRect1,   out showRegion);
                if (result == 0)
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = -1;
                structAlgResult.MeasureValue[1] = -1;

                structAlgResult.outImage = outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Antenna_shrapnel");
            }
        }
        public void Detector_Antenna_shrapnel(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject outImg = new HObject();
                HTuple distance = null;

                //图像预处理
                HTuple hv_Mean = new HTuple();
                HObject ho_Temp = new HObject(), ho_TempRegion = new HObject();
                HOperatorSet.GrayFeatures(nodeInfo.roi1, inputImg, new HTuple("mean"), out hv_Mean);
                HOperatorSet.MultImage(inputImg, inputImg, out ho_Temp, new HTuple(255 / (hv_Mean * hv_Mean)), 0);
                HOperatorSet.GrayOpeningRect(ho_Temp, out ho_Temp, 1, 7);


                // halcon原始函数
                Antenna_shrapnel(ho_Temp, nodeInfo.roi1, nodeInfo.roi2, out outImg, out showRegion, Convert.ToDouble(nodeInfo.lstNodeInfo[0].param[2]), out distance);


                if (distance.Length > 1)
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = distance[0].D;
                structAlgResult.MeasureValue[1] = distance[1].D;

                structAlgResult.outImage = outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Antenna_shrapnel");
            }
        }

        public void Detector_MLB_jumper_flex_B2B(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult, bool isDebug)
        {
            try
            {
                structAlgResult = new StructAlgResult(12);
                HObject showRegion = new HObject();
                HObject outImg = new HObject();
                HTuple outRow = null, outCol = null;

                ////Leo Update 20210920
                // halcon原始函数 (-0.8, -1.0, -5.9, -1.2, -4.5, -3.0) (-0.3, -5.0, -5.5, -3.0, -3.0, -1.5)
                HTuple hv_BaseOffsetX = new HTuple(-0.8, -1.0, -5.9, -1.2, -4.5, -3.0), hv_BaseOffsetY = new HTuple(5.4, 0.7, 0.2, 2.7, 2.7, 4.2);
                HTuple hv_Homat2d = new HTuple(-0.0115193, -6.19262e-05, 11.8986, 3.59688e-05, -0.011524, 14.0544), hv_Pix2mm = new HTuple(0.011524);
                //图像预处理
                HTuple hv_Mean = new HTuple();
                HObject ho_Temp = new HObject(), ho_TempRegion = new HObject();
                HOperatorSet.Union2(nodeInfo.roi1, nodeInfo.roi2, out ho_TempRegion);
                HOperatorSet.GrayFeatures(ho_TempRegion, inputImg, new HTuple("mean"), out hv_Mean);
                HOperatorSet.MultImage(inputImg, inputImg, out ho_Temp, new HTuple(255 / (hv_Mean * hv_Mean)), 0);
                HOperatorSet.GrayOpeningRect(ho_Temp, out ho_Temp, 11, 1);

                MLB_jumper_flex_B2B(ho_Temp, nodeInfo.roi1, nodeInfo.roi2, out showRegion, hv_BaseOffsetX, hv_BaseOffsetY, hv_Homat2d, hv_Pix2mm, out outRow, out outCol);
                if (isDebug && outRow.Length == 6)
                {
                    HOperatorSet.WriteTuple(outRow, $"{Application.StartupPath}//SysCfg//Rows.tup");
                    HOperatorSet.WriteTuple(outCol, $"{Application.StartupPath}//SysCfg//Cols.tup");
                }
                if (outRow.Length == 0)
                {
                    HOperatorSet.ReadTuple($"{Application.StartupPath}//SysCfg//Rows.tup", out outRow);
                    HOperatorSet.ReadTuple($"{Application.StartupPath}//SysCfg//Cols.tup", out outCol);
                }
                if (outRow.Length > 0)
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                for (int i = 0; i < outRow.Length; i++)
                {
                    structAlgResult.MeasureValue[i] = outRow[i].D;
                    structAlgResult.MeasureValue[i + 6] = outCol[i].D;
                }
                structAlgResult.outImage = outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_MLB_jumper_flex_B2B");
            }
        }

        public void Detector_Connector_buckle(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject ho_outImg = new HObject();
                HTuple hv_result = null;

                // halcon原始函数
                Connector_buckle(inputImg, nodeInfo.roi2, out showRegion, out hv_result);

                if (hv_result.D == 0)
                {

                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = -1;
                structAlgResult.MeasureValue[1] = -1;

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Connector_buckle");
            }
        }


        public void Detector_Connector_buckle_Lzj(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject ho_outImg = new HObject();
                HTuple hv_result = null;

                // halcon原始函数
                ConnectorBuckleLzj(inputImg, nodeInfo.roi2, out showRegion,out ho_outImg, out hv_result);

                if (hv_result.D == 0)
                {

                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = -1;
                structAlgResult.MeasureValue[1] = -1;

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Connector_buckle");
            }
        }

        public void Detector_MLB_Connector_buckle(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject ho_outImg = new HObject();
                HTuple hv_result = null;

                // halcon原始函数
                MLB_ConnectorBuckle(inputImg, nodeInfo.roi2, out showRegion, out ho_outImg, out hv_result);

                if (hv_result.D == 0)
                {

                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = -1;
                structAlgResult.MeasureValue[1] = -1;

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Connector_buckle");
            }
        }

        public void Detector_Mesa_Flex(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject ho_outImg = new HObject();
                HTuple hv_value = null;

                // halcon原始函数
                //
                HObject reduceImgh;
                HOperatorSet.ReduceDomain(inputImg, nodeInfo.roi1, out reduceImgh);
                //HOperatorSet.WriteImage(reduceImgh,"bmp",0, "reduceImgh");
                //Flex(inputImg, nodeInfo.roi1, out showRegion, out ho_outImg, _power_flex, out hv_value);
                Mesa_Flex(inputImg, nodeInfo.roi1, out showRegion, out hv_value);

                if (hv_value.Length > 0)
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = hv_value[0];
                structAlgResult.MeasureValue[1] = hv_value[1];

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Flex");
            }
        }

        public void Detector_Flex(HObject inputImg, NodeDeInfo nodeInfo, int _power_flex, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject ho_outImg = new HObject();
                HTuple hv_value = null;

                // halcon原始函数
                //
                HObject reduceImgh;
                HOperatorSet.ReduceDomain(inputImg, nodeInfo.roi1, out reduceImgh);
                //HOperatorSet.WriteImage(reduceImgh,"bmp",0, "reduceImgh");
                if (_power_flex == 0)//flex
                {
                    flex(inputImg, nodeInfo.roi1, out showRegion, out ho_outImg, _power_flex, out hv_value);
                }
                else if (_power_flex == 2) //MLB flex
                {
                    //Flex(inputImg, nodeInfo.roi1, out showRegion, out ho_outImg, _power_flex, out hv_value);
                    MLB_Flex(inputImg, nodeInfo.roi1, out showRegion, out hv_value);
                }
                else if (_power_flex == 1) //power flex
                {
                    HTuple hv_ModelID = new HTuple();
                    HOperatorSet.ReadShapeModel($"{Application.StartupPath}\\SysCfg\\model.shm", out hv_ModelID);
                    Power_Flex(inputImg, nodeInfo.roi1, out showRegion, out ho_outImg, hv_ModelID, out hv_value);
                    HOperatorSet.ClearShapeModel(hv_ModelID);
                }

                if (hv_value.Length > 0)
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = hv_value[0];
                structAlgResult.MeasureValue[1] = hv_value[1];

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Flex");
            }
        }

        public void Detector_Flash_MLB_Flex(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject ho_outImg = new HObject();
                HTuple hv_value = null;

                // halcon原始函数
                //
                HObject reduceImgh;
                HOperatorSet.ReduceDomain(inputImg, nodeInfo.roi1, out reduceImgh);
                //HOperatorSet.WriteImage(reduceImgh,"bmp",0, "reduceImgh");

                Flash_MLB_Flex(inputImg, nodeInfo.roi1, out showRegion, out hv_value);

                if (hv_value.Length > 0)
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = hv_value[0];
                structAlgResult.MeasureValue[1] = hv_value[1];

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Flex");
            }
        }

        public void Detector_Power_switch(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                HObject ho_outImg = new HObject();
                HTuple hv_value = null;

                // halcon原始函数
                Power_switch(inputImg, nodeInfo.roi1, out showRegion, out hv_value);

                if (hv_value.Length > 0)
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                structAlgResult.MeasureValue[0] = hv_value;
                structAlgResult.MeasureValue[1] = -1;

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Power_switch");
            }
        }


        public void Detector_Find_Special_screws(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                showRegion = nodeInfo.modelRect.Clone();
                HObject ho_outImg = new HObject();
                HTuple[] hv_value = new HTuple[4];

                // halcon原始函数
                //find_model_screws(inputImg, nodeInfo.searchRect1, out ho_outImg, out showRegion, nodeInfo.modelId, out hv_value[0], out hv_value[1], out hv_value[2], out hv_value[3]);

                if (find_special_screw_deeplearning(inputImg,nodeInfo.modelRect))
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                //structAlgResult.MeasureValue[0] = hv_value[0];
                //structAlgResult.MeasureValue[1] = hv_value[1];

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Find_model_screws");
            }
        }

        public void Detector_Find_model_screws(HObject inputImg, NodeDeInfo nodeInfo, ref StructAlgResult structAlgResult)
        {
            try
            {
                structAlgResult = new StructAlgResult(2);
                HObject showRegion = new HObject();
                showRegion = nodeInfo.modelRect.Clone();
                HObject ho_outImg = new HObject();
                HTuple[] hv_value = new HTuple[4];

                // halcon原始函数
                //find_model_screws(inputImg, nodeInfo.searchRect1, out ho_outImg, out showRegion, nodeInfo.modelId, out hv_value[0], out hv_value[1], out hv_value[2], out hv_value[3]);

                if (find_screw_deeplearning(inputImg, nodeInfo.modelRect))
                {
                    structAlgResult.Success = true;
                }
                else
                {
                    structAlgResult.Success = false;
                }
                //赋值
                //structAlgResult.MeasureValue[0] = hv_value[0];
                //structAlgResult.MeasureValue[1] = hv_value[1];

                structAlgResult.outImage = ho_outImg;
                structAlgResult.Region = showRegion;
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "Detector_Find_model_screws");
            }
        }

    }






    public struct StructAlgResult
    {
        public double[] MeasureValue;  //各检测项的结果        
        public HObject outImage;
        public HObject Region;
        public bool Success;
        public string errorMsg;

        public StructAlgResult(int measureCount)
        {

            outImage = new HObject();
            Region = new HObject();
            Success = true;
            errorMsg = "";
            MeasureValue = new double[measureCount];


        }
    }
}
