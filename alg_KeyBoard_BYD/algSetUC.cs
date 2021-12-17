using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;
using System.Collections;

namespace alg_KeyBoard_BYD
{
    public partial class algSetUC : UserControl
    {
        #region 字段
        int curLstIdx = 0;

        HTuple drawId = null;
        string drawType;
        public DoCode objDocode;
        public HObject img;

        List<string> lstBoxColor;
        List<string> lstBoxString;
        List<NodeInfo> lstBoxDrawInfo;

        private string _produce = null;

        string _inputImgFileName = "";

        /// <summary>
        /// 图像旋转 表    （依据，FAI_param配置文件）
        /// </summary>
        public List<int> RotateJ = new List<int>();

        #endregion


        #region 属性

        /// <summary>
        /// 料号
        /// </summary>
        /// 
        public string LiaoHao
        {
            get { return _produce; }
            set
            {
                _produce = value;
                this.cmblLiaohao.Text = value;
                directoryClass.liaohaoPath = value;
                directoryClass.liaohao = value;
                if (value != "" & value != null)
                {
                    cmblLiaohao.Items.Clear();
                    ShowCmbLiaoHao();
                    ShowCmbFAIs(value);
                }

            }
        }

        #endregion
        public algSetUC()
        {
            InitializeComponent();
            InitDo(1);
        }
        private bool ShowCmbFAIs(string liaohao)
        {
            try
            {
                cmbFAI.Items.Clear();
                string currentpath = Environment.CurrentDirectory;
                string filePath = Path.Combine(currentpath, "Config");
                filePath = Path.Combine(filePath, liaohao + "\\" + "FAI_params.txt");

                if (FileHelper.IsExistFile(filePath))
                {
                    StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
                    String line;
                    int i = 0;

                    RotateJ.Clear();
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        string[] temp = line.Split(new char[] { '#' });

                        cmbFAI.Items.Add(temp[0]);
                        if (temp.Length > 1) RotateJ.Add(Convert.ToInt16(temp[1]));
                        else RotateJ.Add(0);
                        i++;
                    }
                    directoryClass.faiCount = i;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ShowCmbLiaoHao()
        {
            cmblLiaohao.Items.AddRange(new string[] { "Star", "Starfire", "Flash" });
        }

        private void cmbFAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.cmbFAI.Text))
            {

                return;
            }
            string cmbFaiName = cmbFAI.Text;
            directoryClass.liaohao = cmblLiaohao.Text;
            switch (directoryClass.liaohao)
            {
                case "Star":
                    directoryClass.fai = (int)(eStarFais)Enum.Parse(typeof(eStarFais), cmbFaiName);
                    break;
                case "Starfire":
                    directoryClass.fai = (int)(eStarFireFais)Enum.Parse(typeof(eStarFireFais), cmbFaiName);
                    break;
                case "Flash":
                    directoryClass.fai = (int)(eFlashFais)Enum.Parse(typeof(eFlashFais), cmbFaiName);
                    break;
                default:
                    break;
            }
            GetFromFile(directoryClass.liaohao, directoryClass.fai);

            directoryClass.paramPath = GetParamsFileName() + "/params.txt";

            this.lstXm.SelectedIndex = 0;
            lstXm_SelectedIndexChanged(null, null);
            this.lstXm.Refresh();
        }

        private string GetParamsFileName()
        {
            string filePath = "";
            string _paramsFileName = "";

            filePath = "./Config/" + directoryClass.liaohao + "/" + Convert.ToInt32(directoryClass.fai);
            return _paramsFileName = filePath;
        }

        public void InitDo(int FAI)
        {
            lstBoxString = new List<string>();

            lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });

            //switch (FAI)
            //{
            //    case 1:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 2:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 3:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 4:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 5:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 6:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 7:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 8:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    case 9:
            //        lstBoxString.AddRange(new string[] { "定位模板", "定位模板搜索区域", "区域1", "区域2" });
            //        break;
            //    default:
            //        break;
            //}
            lstBoxColor = new List<string>();
            for (int i = 0; i < lstBoxString.Count; i++)
            {
                lstBoxColor.Add("red");
            }

            //lstBoxColor.AddRange(new string[] { "red", "red", "red", "red"});
            lstBoxDrawInfo = new List<NodeInfo>();
            for (int i = 0; i < lstBoxString.Count; i++)
            {
                lstBoxDrawInfo.Add(new NodeInfo());
            }
            lstXm.DataSource = lstBoxString.ToArray();
        }


        private void GetFromFile(string produce, int fai)
        {
            string _paramsFileName = GetParamsFileName() + "/params.txt";
            if (File.Exists(_paramsFileName))
            {
                StreamReader sr = new StreamReader(_paramsFileName, Encoding.UTF8);
                String line;
                int i = 0;
                line = sr.ReadLine();
                _inputImgFileName = line;

                if (_inputImgFileName == "")
                {
                    MessageBox.Show("图像丢失!");
                    return;
                }
                if (img == null)
                {
                    img = new HObject();
                }
                else
                {
                    img.Dispose();
                }

                HOperatorSet.ReadImage(out img, _inputImgFileName);
                // needRotate(ref img);


                if (line == null)
                {
                    InitDo(Convert.ToInt32(fai));
                }
                else
                {
                    this.picBox.SetImage(img);
                }

                while ((line = sr.ReadLine()) != null)
                {
                    // if (Convert.ToInt16( line ) > lstBoxDrawInfo.Count) break;

                    lstBoxDrawInfo[i].drawId = line.ToString();
                    lstBoxDrawInfo[i].type = sr.ReadLine();
                    string coor = sr.ReadLine();
                    if (coor != "null")
                    {
                        lstBoxDrawInfo[i].coor = Array.ConvertAll<string, double>(coor.Split(new char[] { ';' }), s => double.Parse(s));
                    }
                    lstBoxDrawInfo[i].isSet = sr.ReadLine() == "True";

                    if (lstBoxDrawInfo[i].isSet)
                    {
                        lstBoxColor[i] = "black";
                    }
                    else
                    {
                        lstBoxColor[i] = "red";
                    }
                    string param = sr.ReadLine();
                    if (param != "null")
                    {
                        lstBoxDrawInfo[i].param = param.Split(new char[] { ';' });
                    }
                    i++;
                }
                sr.Close();
                Console.WriteLine("2222");
            }
            else
            {
                InitDo(Convert.ToInt32(fai));
            }
        }

        /// <summary>
        /// 是否需要旋转
        /// </summary>
        /// <param name="img"></param>


        private void lstXm_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                Brush mybsh;
                if (lstBoxColor[e.Index].ToString() == "red")
                {
                    mybsh = Brushes.Red;
                }
                else
                {
                    mybsh = Brushes.Black;
                }

                // 焦点框
                e.DrawFocusRectangle();
                //文本 
                e.Graphics.DrawString(lstBoxString[e.Index].ToString(), e.Font, mybsh, e.Bounds, StringFormat.GenericDefault);
            }

        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (openFileDialog.FileNames.Length != 0)
            {
                HOperatorSet.ReadImage(out img, (HTuple)openFileDialog.FileNames);
                needRotate(ref img, directoryClass.fai);
                ///HOperatorSet.RotateImage(img, out img, objDocode.lstNodeParamInfo[directoryClass.fai].RotateAngleImg, "constant");
                this.picBox.SetImage(img);
                _inputImgFileName = openFileDialog.FileNames[0];
            }
        }


        /// <summary>输入图像，工位
        /// 
        /// </summary>
        /// <param name="img">输入图像 </param>
        /// <param name="fai">工位</param>
        public void needRotate(ref HObject img, int fai)
        {

            if (RotateJ[fai - 1].ToString() != "0")
            {
                HOperatorSet.RotateImage(img, out img, (int)RotateJ[directoryClass.fai - 1], "constant");
            }
        }

        private void BtnDrawRect1_Click(object sender, EventArgs e)
        {

            drawId = this.picBox.DrawRectangle1(ref drawId, (int)(this.picBox.Width * 0.3), (int)(this.picBox.Width * 0.3), (int)(this.picBox.Width * 0.8), (int)(this.picBox.Width * 0.8));
            drawType = "rectangle1";
            //lstBoxDrawInfo.Add(new DrawInfo() {drawId=id,

            //    });
            SetDrawBtnEnabled(false);
        }

        private void BtnDrawRect2_Click(object sender, EventArgs e)
        {
            drawId = this.picBox.DrawRectangle2((int)(this.picBox.Width * 0.5), (int)(this.picBox.Width * 0.5), 0.39, (int)(this.picBox.Width * 0.2), (int)(this.picBox.Width * 0.2));
            drawType = "rectangle2";
            //lstBoxDrawInfo.Add(new DrawInfo() {drawId=id,

            //    });
            SetDrawBtnEnabled(false);

        }

        private void BtnDrawLine_Click(object sender, EventArgs e)
        {
            drawId = this.picBox.DrawLine((int)(this.picBox.Width * 0.8), (int)(this.picBox.Width * 0.8), (int)(this.picBox.Width * 0.8), (int)(this.picBox.Width * 0.3));
            drawType = "line";

            //lstBoxDrawInfo.Add(new DrawInfo() {drawId=id,

            //    });
            SetDrawBtnEnabled(false);
        }
        private void SetDrawBtnEnabled(bool isE)
        {
            this.BtnDrawRect1.Enabled = isE;
            this.BtnDrawRect2.Enabled = isE;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveNode();
        }
        private void SaveNode()
        {
            if (drawId == null)
            {
                MessageBox.Show("未完成感兴趣区域的绘制，无法保存!");
                return;
            }

            var va = this.picBox.GetGraphicParam(drawType, drawId);

            string[] rs = null;
            if (!GetSaveParam(out rs))
            {
                return;
            }
            lstBoxDrawInfo[curLstIdx].param = rs;


            lstBoxDrawInfo[curLstIdx].drawId = drawId;
            lstBoxDrawInfo[curLstIdx].type = drawType;
            lstBoxDrawInfo[curLstIdx].coor = va;
            lstBoxDrawInfo[curLstIdx].isSet = true;

            if (drawType == "line")
            {
                //找线
                CFindLine objCFindLine = new CFindLine("", img);
                //产生直线xld
                HObject LineRegion;
                HOperatorSet.GenEmptyObj(out LineRegion);

                HObject ho_RegionLines;

                HTuple rows = new HTuple();
                HTuple cols = new HTuple();
                rows = rows.TupleConcat(lstBoxDrawInfo[curLstIdx].coor[0]);
                rows = rows.TupleConcat(lstBoxDrawInfo[curLstIdx].coor[2]);
                cols = cols.TupleConcat(lstBoxDrawInfo[curLstIdx].coor[1]);
                cols = cols.TupleConcat(lstBoxDrawInfo[curLstIdx].coor[3]);

                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, rows, cols);
                LineRegion = LineRegion.ConcatObj(ho_RegionLines);
                objCFindLine.GenMesaureRectRegion(rows, cols);

                objCFindLine.ShowDirectonArrow();
                this.picBox.DispObj(LineRegion);

                //////this.picBox.SetImage(LineRegion);
                //////qPictureControl.AddInputRoi("blue", false, LineRegion, "查找直线区域");
                HTuple lineRows1, lineRows2, lineCols1, lineCols2;
                GenPerpendLine(rows[0], cols[1], rows[1], cols[0], out lineRows1, out lineCols1, out lineRows2, out lineCols2);


                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, lineRows1.TupleConcat(lineRows2), lineCols1.TupleConcat(lineCols2));
                this.picBox.DispObj(ho_RegionLines, "fill", "green");

            }

            lstBoxColor[curLstIdx] = "black";

            //MessageBox.Show("保存成功!");
            this.lstXm.Refresh();
            this.picBox.DeleteAllDrawingObject();
            SetDrawBtnEnabled(true);
            this.cmbVal1.Text = "";
            this.cmbVal2.Text = "";
            this.cmbVal3.Text = "";

        }
        public void GenPerpendLine(HTuple BeginRow, HTuple BeginCol, HTuple EndRow, HTuple EndCol
            , out HTuple RowBegin, out HTuple ColBegin, out HTuple RowEnd, out HTuple ColEnd)
        {
            HObject Line;
            BeginRow.Append(EndRow);
            BeginCol.Append(EndCol);
            HOperatorSet.GenContourPolygonXld(out Line, BeginRow, BeginCol);
            HTuple angle;
            //   HOperatorSet.ang
            HTuple Hom2D;
            HOperatorSet.HomMat2dIdentity(out Hom2D);
            //HOperatorSet.HomMat2dRotate(Hom2D, Math.PI / 2, BeginRow[0], BeginCol[0], out Hom2D);
            HOperatorSet.HomMat2dRotate(Hom2D, 0, BeginRow[0], BeginCol[0], out Hom2D);

            HOperatorSet.AffineTransContourXld(Line, out Line, Hom2D);
            HTuple Nr, Nc, Dist;
            HOperatorSet.FitLineContourXld(Line, "tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
        }
        private bool GetSaveParam(out string[] rs)
        {
            rs = null;
            switch (lstBoxString[curLstIdx])
            {
                case "定位模板":
                    if (string.IsNullOrWhiteSpace(this.cmbVal1.Text))
                    {
                        MessageBox.Show("对比度值不能为空!");
                        return false;
                    }

                    int val1 = 0;
                    if (!int.TryParse(this.cmbVal1.Text, out val1))
                    {
                        MessageBox.Show("对比度值有误!");
                        return false;
                    }
                    //
                    if (string.IsNullOrWhiteSpace(this.cmbVal2.Text))
                    {
                        MessageBox.Show("匹配分值不能为空!");
                        return false;
                    }
                    int val2 = 0;
                    if (!int.TryParse(this.cmbVal2.Text, out val2))
                    {
                        MessageBox.Show("匹配分值有误!");
                        return false;
                    }
                    //
                    if (string.IsNullOrWhiteSpace(this.cmbVal3.Text))
                    {
                        MessageBox.Show("像素当量不能为空!");
                        return false;
                    }
                    double val3 = 0;
                    if (!double.TryParse(this.cmbVal3.Text, out val3))
                    {
                        MessageBox.Show("像素当量值有误!");
                        return false;
                    }

                    rs = new string[] { val1.ToString(), val2.ToString(), val3.ToString() };
                    break;
                case "定位模板搜索区域":
                    return true;
                default:
                    if (string.IsNullOrWhiteSpace(this.cmbVal1.Text))
                    {
                        MessageBox.Show("选择阈值不能为空!");
                        return false;
                    }

                    int val = 0;
                    if (!int.TryParse(this.cmbVal1.Text, out val))
                    {
                        MessageBox.Show("选择阈值有误!");
                        return false;
                    }
                    rs = new string[] { val.ToString() };
                    break;
            }

            return true;
        }

        private void lstXm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstXm.SelectedIndex < 0)
            {
                return;
            }
            curLstIdx = lstXm.SelectedIndex;
            ShowNodeInfo();
            cmbVal1.Focus();
        }
        private void ShowNodeInfo()
        {
            DrawG(lstBoxDrawInfo[curLstIdx]);
            this.lblprompt.Text = "请在图片框中绘制出" + lstBoxString[curLstIdx];
            var ps = lstBoxDrawInfo[curLstIdx].param;
            SetParamBoxShow(ps);
        }
        private void DrawG(NodeInfo nodeInfo)
        {
            if (nodeInfo != null)
            {
                this.picBox.DeleteAllDrawingObject();
                if (!nodeInfo.isSet)
                {
                    SetDrawBtnEnabled(true);
                    return;
                }
                switch (nodeInfo.type)
                {
                    case "rectangle1":
                        drawId = this.picBox.DrawRectangle1(ref this.drawId, (int)nodeInfo.coor[0], (int)nodeInfo.coor[1], (int)nodeInfo.coor[2], (int)nodeInfo.coor[3]);
                        drawType = "rectangle1";
                        SetDrawBtnEnabled(false);
                        break;
                    case "rectangle2":
                        drawId = this.picBox.DrawRectangle2((int)nodeInfo.coor[0], (int)nodeInfo.coor[1], nodeInfo.coor[2], (int)nodeInfo.coor[3], (int)nodeInfo.coor[4]);
                        drawType = "rectangle2";
                        SetDrawBtnEnabled(false);
                        break;
                    case "line":
                        drawId = this.picBox.DrawLine((int)nodeInfo.coor[0], (int)nodeInfo.coor[1], (int)nodeInfo.coor[2], (int)nodeInfo.coor[3]);
                        drawType = "line";
                        SetDrawBtnEnabled(false);
                        break;



                    default:
                        break;
                }
            }

        }

        private void SetParamBoxShow(string[] param)
        {
            switch (lstBoxString[curLstIdx])
            {
                case "定位模板":
                    this.label2.Text = "对比度值(设0不启用):";
                    if (param != null)
                    {
                        this.cmbVal1.Location = new System.Drawing.Point(125, 10);
                        this.cmbVal1.Text = param[0];
                        this.cmbVal2.Text = param[1];
                        this.cmbVal3.Text = param[2];

                    }
                    this.panel5.Visible = true;
                    this.panel5.Width = 680;
                    break;
                case "定位模板搜索区域":

                    this.panel5.Visible = false;
                    this.panel5.Width = 145;
                    break;
                default:
                    this.label2.Text = "选择阈值:";
                    if (param != null)
                    {
                        this.cmbVal1.Location = new System.Drawing.Point(80, 10);
                        this.cmbVal1.Text = param[0];
                    }
                    this.panel5.Visible = true;
                    this.panel5.Width = 165;
                    break;

            }
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;

            SaveToFile(LiaoHao);

            btnTest.Enabled = true;

        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="produce">料号</param>
        private void SaveToFile(string produce)
        {

            string filePath = "";
            string _paramsFileName = "";
            int fai = Convert.ToInt32(directoryClass.fai);

            filePath = "./Config/" + produce + "/" + fai;
            string inputImgSavePath = filePath + "/" + fai;
            FileHelper.CreateDirectory(filePath);
            _paramsFileName = filePath + "/params.txt";

            foreach (var item in lstBoxColor)
            {
                if (item == "red")
                {
                    MessageBox.Show("有区域未完成设置,无法保存!");
                    return;
                }
            }

            //if (!DoCode.IsScrew(fai))
            //{
            AlgFun imageCore = new AlgFun();
            HTuple modelId = null, row = null, col = null, angle = null, score = null;
            string modelImgSaveFilePath;
            imageCore.CreateModel(LiaoHao, filePath, out modelImgSaveFilePath, img, lstBoxDrawInfo[0].type,
                                 lstBoxDrawInfo[0].coor, Convert.ToInt32(lstBoxDrawInfo[0].param[0]), out modelId, true);

            imageCore.FindModel(img, modelId, lstBoxDrawInfo[1].type, lstBoxDrawInfo[1].coor, out row, out col, out angle, out score);

            if (score == null || score.TupleLength() <= 0)
            {
                MessageBox.Show("模板设置错误!");
                return;
            }
            else
            {
                string[] param = new string[] { lstBoxDrawInfo[0].param[0], lstBoxDrawInfo[0].param[1], lstBoxDrawInfo[0].param[2], row.D.ToString(), col.D.ToString(), 0.ToString() };
                lstBoxDrawInfo[0].param = param;
                HOperatorSet.WriteShapeModel(modelId, filePath + "/model.sbm");
            }
            //}
            //else
            //{

            //}


            HOperatorSet.WriteImage(img, "bmp", 0, inputImgSavePath);


            FileStream stream2 = File.Open(_paramsFileName, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            StreamWriter sw = new StreamWriter(stream2);


            sw.WriteLine(inputImgSavePath);

            for (int i = 0; i < lstBoxDrawInfo.Count; i++)
            {
                sw.WriteLine(lstBoxDrawInfo[i].drawId.ToString());
                sw.WriteLine(lstBoxDrawInfo[i].type);
                if (lstBoxDrawInfo[i].coor != null)
                {
                    sw.WriteLine(string.Join(";", lstBoxDrawInfo[i].coor));
                }
                else
                {
                    sw.WriteLine("null");
                }
                sw.WriteLine(lstBoxDrawInfo[i].isSet.ToString());
                if (lstBoxDrawInfo[i].param != null)
                {
                    sw.WriteLine(string.Join(";", lstBoxDrawInfo[i].param));
                }
                else
                {
                    sw.WriteLine("null");
                }
            }
            sw.Flush();
            sw.Close();
            stream2.Close();

            MessageBox.Show("保存成功!");
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            this.picBox.DeleteAllDrawingObject();
            SetDrawBtnEnabled(true);
        }

        private void picBox_Load(object sender, EventArgs e)
        {

            //picBox.init();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            foreach (var item in lstBoxColor)
            {
                if (item == "red")
                {
                    MessageBox.Show("有区域未完成设置,无法测试!");
                    return;
                }
            }

            DoCode objDocode = new DoCode();
            objDocode.initVisionParams();

            if (objDocode != null /*&& objDocode.openImg(out img)*/)
            {
                List<StructAlgResult> structAlgResultList = new List<StructAlgResult>();

                if (img.IsInitialized() && img != null)
                {
                    int m_countSatrt = Environment.TickCount;
                    if (directoryClass.fai == 9 && directoryClass.liaohao == "Star")
                    {
                        objDocode.DealImage(img, directoryClass.fai, out structAlgResultList, true);
                    }
                    else
                    {
                        objDocode.DealImage(img, directoryClass.fai, out structAlgResultList);
                    }

                    int m_countTime = Environment.TickCount - m_countSatrt;

                    string strResult = "";
                    if (structAlgResultList.Count == 2)
                    {
                        picBox.SetImage(img);
                        HObject ShowRegions = new HObject();
                        if (structAlgResultList[0].Region.IsInitialized() && structAlgResultList[0].Region != null && structAlgResultList[1].Region.IsInitialized() && structAlgResultList[1].Region != null)
                        {
                            HOperatorSet.ConcatObj(structAlgResultList[0].Region, structAlgResultList[1].Region, out ShowRegions);
                            picBox.DispObj(ShowRegions, "margin", "blue");
                        }
                        strResult = "耗时：" + m_countTime + "\r\n  测量值1：" + structAlgResultList[0].Success + " " + structAlgResultList[0].MeasureValue[0].ToString() + " & " + structAlgResultList[0].MeasureValue[1].ToString();
                        strResult = strResult + "\r\n  测量值2：" + structAlgResultList[1].Success + " " + structAlgResultList[1].MeasureValue[0].ToString() + " & " + structAlgResultList[1].MeasureValue[1].ToString();

                    }
                    if (structAlgResultList.Count == 1)
                    {
                        picBox.SetImage(img);
                        picBox.DispObj(structAlgResultList[0].Region, "margin", "blue");
                        strResult = "耗时：" + m_countTime + "\r\n  测量值：" + structAlgResultList[0].Success + " " + structAlgResultList[0].MeasureValue[0].ToString();
                        for (int i = 1; i < structAlgResultList[0].MeasureValue.Length; i++)
                        {
                            strResult += " & " + structAlgResultList[0].MeasureValue[i].ToString();
                        }

                    }
                    if (structAlgResultList.Count == 0)
                    {
                        return;  //检测失败
                    }
                    textBox1.Text = strResult;

                }
                else
                {
                    return;
                }

            }



        }

        private void cmblLiaohao_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCmbFAIs(cmblLiaohao.Text);
        }

        public void SetLiao(string liaohao)
        {
            //cmblLiaohao.SelectedItem = liaohao;
        }

        private void algSetUC_Load(object sender, EventArgs e)
        {

        }

    }
}
