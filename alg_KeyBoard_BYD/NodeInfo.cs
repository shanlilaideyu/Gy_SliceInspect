using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alg_KeyBoard_BYD
{

    /// <summary>
    ///  Star/Starfire/Flash 3种产品
    ///      
    /// </summary>
    public enum eStarFais
    {
        Power_switch = 1,
        Switch_screw_1,
        Power_flex_Connector_buckle,
        Mesa_flex_Connector_buckle,
        Mesa_jumper_flex_cosmetic_1,
        Mesa_jumper_flex_cosmetic_2,
        Mesa_jumper_flex_cosmetic_3,
        Mesa_jumper_flex_cosmetic_4,
        MLB_jumper_flex_B2B,
        E75_flex_Connector_buckle,
        MLB_flex_Connector_buckle,
        Main_board_screw_3,
        Main_board_screw_2,
        Main_board_screw_1,
        Antenna_shrapnel,

    }
    public enum eStarFireFais
    {
        Power_switch = 1,
        Switch_screw_1,
        Power_flex_Connector_buckle,
        E75_flex_Connector_buckle,
        MLB_flex_Connector_buckle,
        Main_board_screw_3,
        Main_board_screw_2,
        Main_board_screw_1,
        Antenna_shrapnel,

    }
    public enum eFlashFais
    {
        Power_switch = 1,
        Switch_screw_1,
        Switch_screw_2,
        Power_flex_Connector_buckle,
        Mesa_flex_Connector_buckle,
        Main_board_screw_4,
        E75_flex_Connector_buckle,
        MLB_flex_Connector_buckle,
        Main_board_screw_3,
        Main_board_screw_2,
        Main_board_screw_1,
        Antenna_shrapnel
    }



    public class NodeInfo
    {
        public HTuple drawId;
        public string type;
        public double[] coor;
        public bool isSet = false;
        public string[] param;

    }

    public class NodeDeInfo
    {
        public string imageFilePath;
        public HTuple modelId;
        public HTuple modelId2;

        public HObject modelRect;
        public HObject searchRect1;
        public HTuple modelRow;
        public HTuple modelCol;
        public HTuple modelAngle;



        public HObject roi1;
        public HObject roi2;

        public List<NodeInfo> lstNodeInfo;

        public int RotateAngleImg;

    }

    ////测量项列表
    //public class FAIInfo
    //{
    //    public List<NodeDeInfo> lstNodeDeInfo { get; set; }
    //}


    public class OpNodeInfo
    {
        /// <summary>
        /// 获取工位配置参数 
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns></returns>
        public NodeDeInfo GetNodeInfo(string filePath)
        {

            string paramsFileName = filePath + "/params.txt";
            NodeDeInfo nodeDeInfo = new NodeDeInfo();

            string imagePath = "";
            List<NodeInfo> lstNode = new List<NodeInfo>();
            for (int i = 0; i < 6; i++)
            {
                lstNode.Add(new NodeInfo());
            }
            //txtpath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "test.txt";
            if (File.Exists(paramsFileName))
            {
                StreamReader sr = new StreamReader(paramsFileName, Encoding.UTF8);
                String line;
                int i = 0;
                line = sr.ReadLine();
                imagePath = line;
                while ((line = sr.ReadLine()) != null)
                {
                    lstNode[i].drawId = line.ToString();
                    lstNode[i].type = sr.ReadLine();
                    string coor = sr.ReadLine();
                    if (coor != "null")
                    {
                        lstNode[i].coor = Array.ConvertAll<string, double>(coor.Split(new char[] { ';' }), s => double.Parse(s));
                    }

                    lstNode[i].isSet = sr.ReadLine() == "True";

                    string param = sr.ReadLine();

                    if (param != "null")
                    {
                        lstNode[i].param = param.Split(new char[] { ';' });
                    }
                    i++;
                }
                sr.Close();
                nodeDeInfo.imageFilePath = imagePath;
                nodeDeInfo.lstNodeInfo = lstNode;
            }

            HTuple modelID;
            HOperatorSet.ReadShapeModel(filePath + "/model.sbm", out modelID);
            nodeDeInfo.modelId = modelID;
           
            //if (File.Exists(filePath + "/model1.sbm"))
            //{
            //    HTuple modelID2;
            //    HOperatorSet.ReadShapeModel(filePath + "/model2.sbm", out modelID2);
            //    nodeDeInfo.modelId2 = modelID2;
            //}
            //HTuple mId = null, len1 = null, len2 = null, score = null;
            //HObject img;
            //HOperatorSet.ReadImage(out img, imagePath);
            //HOperatorSet.FindShapeModel(img, nodeDeInfo.modelId, -0.2, 1.5, 0.05, 1, 0.05, "least_squares", 3, 0.8, out nodeDeInfo.modelRow, out nodeDeInfo.modelCol, out nodeDeInfo.modelAngle, out score);


            HObject rect1, rect2;
            HOperatorSet.GenEmptyObj(out rect1);
            HOperatorSet.GenEmptyObj(out rect2);

            AlgFun imgCore = new AlgFun();
            //定位ROI
            if (lstNode[0].coor != null && lstNode[0].coor.Length >= 4)
            {
                imgCore.GetRect(lstNode[0].type, lstNode[0].coor, ref rect1);
                nodeDeInfo.modelRect = rect1;
            }

            //搜索区域ROI
            if (lstNode[1].coor != null && lstNode[1].coor.Length >= 4)
            {
                imgCore.GetRect(lstNode[1].type, lstNode[1].coor, ref rect1);
                nodeDeInfo.searchRect1 = rect1;
            }

            //检测ROI1
            if (lstNode[2].coor != null && lstNode[2].coor.Length == 4)
            {
                imgCore.GetRect(lstNode[2].type, lstNode[2].coor, ref rect1);
                nodeDeInfo.roi1 = rect1;
            }
            //检测ROI2
            if (lstNode[3].coor != null && lstNode[3].coor.Length == 4)
            {
                imgCore.GetRect(lstNode[3].type, lstNode[3].coor, ref rect2);
                nodeDeInfo.roi2 = rect2;
            }
            return nodeDeInfo;
        }
    }


}
