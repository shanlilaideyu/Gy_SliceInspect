using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alg_KeyBoard_BYD
{
    public class DoCode
    {
        public AlgFun ImageCore;

        public bool _isLoad = false;

        public Dictionary<int, NodeDeInfo> lstNodeParamInfo;

        public bool isDebug = false;

        public void initVisionParams()
        {
            LoadParam();
            //Task task = new Task(() =>
            //{
            //    LoadParam();
            //});
            //task.Start();
        }

        public DoCode()
        {
            ImageCore = new AlgFun();

            initVisionParams();
        }


        public bool LoadParam()
        {
            try
            {
                lstNodeParamInfo = new Dictionary<int, NodeDeInfo>();
                OpNodeInfo opNodeInfo = new OpNodeInfo();

                for (int i = 0; i < directoryClass.faiCount; i++)
                {
                    NodeDeInfo nodeDeInfo = new NodeDeInfo();
                    string FileName = "./Config/" + directoryClass.liaohao + "/" + (i + 1);
                    nodeDeInfo = opNodeInfo.GetNodeInfo(FileName);

                    //////读取图像旋转角度 
                    ////string Fai_paramsName = "./Config/" + directoryClass.liaohao + "/FAI_params.txt";
                    ////StreamReader sr = new StreamReader(Fai_paramsName, Encoding.UTF8);
                    ////String line;
                    ////int j = 0;
                    ////line = sr.ReadLine().Trim() ;
                    ////string[] temp = line.Split('#');
                    ////if (temp.Length>1)
                    ////{
                    ////    nodeDeInfo.RotateAngleImg = Convert.ToInt16(temp[1]);
                    ////}
                    //////////////////////////读取图像旋转角度 end

                    lstNodeParamInfo.Add(i + 1, nodeDeInfo);
                    _isLoad = true;
                }
            }
            catch (Exception ex)
            {
                _isLoad = false;
                return false;
            }
            return _isLoad;
        }


        public static bool IsScrew(int fai)
        {
            if (directoryClass.liaohao == "Star")
            {
                if (new List<int> { 2, 13, 14, 15 }.Contains(fai))
                {
                    return true;
                }
            }
            else if (directoryClass.liaohao == "Starfire")
            {
                if (new List<int> { 2, 6, 7, 8 }.Contains(fai))
                {
                    return true;
                }
            }
            else if (directoryClass.liaohao == "Flash")
            {
                if (new List<int> { 2, 3, 6, 8, 10, 11, 12 }.Contains(fai))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检测算法
        /// </summary>
        /// <param name="srcImg">输入图像</param>
        /// <param name="fai">拍照位 （一个拍照位可能包含不止1个检查项）</param>
        /// <param name="structAlgResult">返回结果结构</param>
        public void DealImage(HObject srcImg, int fai, out List<alg_KeyBoard_BYD.StructAlgResult> structAlgResultList, bool isDebug = false)
        {
            structAlgResultList = new List<StructAlgResult>();
            StructAlgResult structAlgResult = new StructAlgResult();
            structAlgResult.outImage = srcImg;
            structAlgResult.MeasureValue = new double[1] { 0 };
            structAlgResult.Region = null;
            structAlgResult.Success = false;
            structAlgResult.errorMsg = "";

            //****
            /* 
             *  1 模板定位ROI
             *  2 搜索ROI
             *  3 
             *  5
             */

            //提取配置信息
            if (_isLoad == false)
            {
                initVisionParams();
            }

            NodeDeInfo nodeInfo = lstNodeParamInfo[fai];
            HObject ho_modelRoi, ho_Roi1, ho_Roi2, ReduceDomain;

            HOperatorSet.GenEmptyObj(out ho_modelRoi);
            HOperatorSet.GenEmptyObj(out ho_Roi1);
            HOperatorSet.GenEmptyObj(out ho_Roi2);
            HOperatorSet.GenEmptyObj(out ReduceDomain);


            try
            {
                structAlgResult = new StructAlgResult(2);

                if (nodeInfo.lstNodeInfo[0].param[0] == "0")
                {
                    //ho_Roi1 = nodeInfo.roi1; ho_Roi2 = nodeInfo.roi2;
                }
                else
                {
                    #region
                    if (!IsScrew(fai)) //如果是螺丝就不找模板了
                    {
                        HTuple row, col, angle, scale, score;
                        if (ReduceDomain != null) ReduceDomain.Dispose();
                        HTuple r11, c11, r22, c22;
                        HOperatorSet.SmallestRectangle1(nodeInfo.searchRect1, out r11, out c11, out r22, out c22);
                        HOperatorSet.ReduceDomain(srcImg, nodeInfo.searchRect1, out ReduceDomain);
                        if (isDebug)
                        {
                            HOperatorSet.WriteImage(srcImg, "bmp", 0, "srcImg");
                            HOperatorSet.WriteImage(ReduceDomain, "bmp", 0, "ReduceDomain");
                        }
                        if (nodeInfo.modelId != null)
                        {
                            // HOperatorSet.FindShapeModel(ReduceDomain, nodeInfo.modelId, -0.2, 0.9, 0.5, 1, 0.5, "least_squares", 0, 0.9, out row, out col, out angle, out score);
                            HOperatorSet.FindScaledShapeModel(ReduceDomain, nodeInfo.modelId, new HTuple(-10).TupleRad(), (new HTuple(20)).TupleRad(), 0.98, 1.02, 0.3, 1, 1, "least_squares_high", (new HTuple(5)).TupleConcat(
               3), 0.5, out row, out col, out angle, out scale, out score);
                        }
                        else
                        {
                            structAlgResult.errorMsg = "模板缺失";
                            structAlgResult.Success = false;
                            structAlgResultList.Add(structAlgResult);
                            return;
                        }
                        if (score < 0.1 || score == null)
                        {
                            structAlgResult.errorMsg = "查找模板失败";
                            structAlgResult.Success = false;
                            structAlgResultList.Add(structAlgResult);
                            return;
                        }
                        HTuple area1, r1, c1;
                        HOperatorSet.AreaCenter(nodeInfo.modelRect, out area1, out r1, out c1);

                        HTuple hv_HomMat2D = null;
                        HOperatorSet.VectorAngleToRigid(r1, c1, 0, row, col, 0, out hv_HomMat2D);
                        //校正ROI
                        HOperatorSet.AffineTransRegion(nodeInfo.modelRect, out nodeInfo.modelRect, hv_HomMat2D, "nearest_neighbor");
                        if ((fai == 3 && (directoryClass.liaohao == "Star" || directoryClass.liaohao == "Starfire") || (fai == 4 && directoryClass.liaohao == "Flash")))
                        {

                        }
                        else
                        {
                            HOperatorSet.AffineTransRegion(nodeInfo.roi1, out nodeInfo.roi1, hv_HomMat2D, "nearest_neighbor");
                        }
                        HOperatorSet.AffineTransRegion(nodeInfo.roi2, out nodeInfo.roi2, hv_HomMat2D, "nearest_neighbor");
                        if (isDebug)
                        {
                            HObject ReduceModelRect = new HObject();
                            HObject ReduceRoi1Rect = new HObject();
                            //   HObject ReduceRoi2Rect = new HObject();
                            HOperatorSet.ReduceDomain(srcImg, nodeInfo.modelRect, out ReduceModelRect);
                            HOperatorSet.ReduceDomain(srcImg, nodeInfo.roi1, out ReduceRoi1Rect);
                            //   HOperatorSet.ReduceDomain(srcImg, nodeInfo.roi2, out ReduceRoi2Rect);

                            HOperatorSet.WriteImage(ReduceModelRect, "bmp", 0, "ReduceModelRect");
                            HOperatorSet.WriteImage(ReduceRoi1Rect, "bmp", 0, "ReduceRoi1Rect");
                            //  HOperatorSet.WriteImage(ReduceRoi2Rect, "bmp",0, "ReduceRoi2Rect");
                        }
                    }
                    #endregion
                }
                if (directoryClass.liaohao == "Star")
                {
                    #region star 检测
                    switch (fai)
                    {
                        case 1:
                            //Power_switch
                            ImageCore.Detector_Power_switch(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 2:
                            // Switch_screw_1	
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 3:
                            //Power_flex 
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 1, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 4:
                            //Mesa flex&Connector buckle
                            ImageCore.Detector_Mesa_Flex(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 5:
                            //MLB jumper flex cosmetic 1",
                            ImageCore.Detector_MLB_jumper(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 6:
                            //MLB jumper flex cosmetic 2",
                            ImageCore.Detector_MLB_jumper(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 7:
                            //MLB jumper flex cosmetic 3",
                            ImageCore.Detector_MLB_jumper(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 8:
                            //MLB jumper flex cosmetic 4",
                            ImageCore.Detector_MLB_jumper(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                     
                        case 9:
                            // MLB jumper flex B2B",
                            ImageCore.Detector_MLB_jumper_flex_B2B(srcImg, nodeInfo, ref structAlgResult,isDebug);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 10:
                            //"E75 flex&Connector buckle",
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 0, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            break;
                        case 11:
                            // "MLB flex&Connector buckle",
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 2, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 12:
                            //Main board screw 3
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 13:
                            //Main board screw 2,
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 14:
                            //Main board screw 1,
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 15:
                            // Antenna shrapnel      
                            ImageCore.Detector_Antenna_shrapnel(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                else if (directoryClass.liaohao == "Starfire")
                {
                    #region StarFire
                    switch (fai)
                    {
                        case 1:
                            //Power_switch
                            ImageCore.Detector_Power_switch(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 2:
                            // Switch_screw_1	
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 3:
                            //Power_flex 
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 1, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 4:
                            //"E75 flex&Connector buckle",
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 0, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            break;
                        case 5:
                            // "MLB flex&Connector buckle",
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 2, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            ImageCore.Detector_Connector_buckle_Lzj(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 6:
                            //Main board screw 3
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 7:
                            //Main board screw 2,
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 8:
                            //Main board screw 1,
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 9:
                            // Antenna shrapnel      
                            ImageCore.Detector_Antenna_shrapnel(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        default:
                            break;
                    }
                    #endregion
                }
                else if (directoryClass.liaohao == "Flash")
                {
                    #region Flash 检测
                    switch (fai)
                    {
                        case 1:
                            //Power_switch
                            ImageCore.Detector_Power_switch(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 2:
                            // Switch_screw_1	
                            ImageCore.Detector_Find_Special_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 3:
                            // Switch_screw_2	
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 4:
                            //Power_flex 
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 1, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 5:
                            //Mesa flex&Connector buckle
                            ImageCore.Detector_Mesa_Flex(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);

                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;

                        case 6:
                            //Main board screw 5
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 7:
                            //"E75 flex&Connector buckle",
                            ImageCore.Detector_Flex(srcImg, nodeInfo, 0, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                   
                        case 8:
                            // "MLB flex&Connector buckle",
                            ImageCore.Detector_Flash_MLB_Flex(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            ImageCore.Detector_Connector_buckle(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 9:
                            //Main board screw 3
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 10:
                            //Main board screw 2,
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 11:
                            //Main board screw 1,
                            ImageCore.Detector_Find_model_screws(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        case 12:
                            // Antenna shrapnel      
                            ImageCore.Detector_Antenna_shrapnel(srcImg, nodeInfo, ref structAlgResult);
                            structAlgResultList.Add(structAlgResult);
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.RecordRunLog(string.Format("error:{0},StackTrace:{1}", ex.Message, ex.StackTrace), "DoCore DealImage");

            }
            finally
            {
                ho_modelRoi.Dispose();
                ho_Roi1.Dispose();
                ho_Roi2.Dispose();
                ReduceDomain.Dispose();
            }
        }

        public bool openImg(out HObject img)
        {
            img = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return false;

            if (openFileDialog.FileNames.Length != 0)
            {
                HOperatorSet.ReadImage(out img, (HTuple)openFileDialog.FileNames);
                // inputImgFileName = openFileDialog.FileNames[0];
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
