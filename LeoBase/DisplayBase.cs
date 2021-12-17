using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace LeoBase
{
    public partial class DisplayBase : UserControl, IDisplay<HObject>
    {
        public DisplayBase()
        {
            InitializeComponent();
            image = new HsbObj();
            InitContextMenuStrip();
            InitDoubleClick();

        }


        #region 快捷键

        public bool ContextMenuStripEnable
        {
            set
            {
                hWindowControl1.ContextMenuStrip = value ? cms : null;
            }
        }
        private ContextMenuStrip cms = new ContextMenuStrip();
        private void InitContextMenuStrip()
        {
            cms = new ContextMenuStrip();
            hWindowControl1.ContextMenuStrip = cms;
            //hWindowControl1.ContextMenuStrip.Items.Add("适应窗体");
            //hWindowControl1.ContextMenuStrip.Items.Add("打开图像");
            hWindowControl1.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
        }
        protected ContextMenuStrip WindowContextMenuStrip
        {
            get
            {
                if (hWindowControl1.ContextMenuStrip == null)
                {
                    hWindowControl1.ContextMenuStrip = new ContextMenuStrip();
                }
                return hWindowControl1.ContextMenuStrip;
            }
        }
        protected virtual void WindowContextMenuStripItemClicked(string ItemText)
        {

        }

        void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            hWindowControl1.ContextMenuStrip.Visible = false;
            switch (e.ClickedItem.Text)
            {
                case "适应窗体":
                    Zoom(ZoomMode.Reset); break;
                case "打开图像":
                    OpenImage(); break;
                default: break;
            }
            WindowContextMenuStripItemClicked(e.ClickedItem.Text);
            hWindowControl1.ContextMenuStrip.Visible = true;
        }
        #endregion
        [Browsable(false)]
        public HWindow DisplayHandle
        {
            get
            {
                return hWindowControl1.HalconWindow;
            }
        }
        private HsbObj image = new HsbObj();
        [Browsable(false)]
        public HObject SourceImage
        {
            get
            {
                if (image == null)
                    image = new HsbObj();
                return image.Value;
            }
            private set
            {
                if (!ImageExist)
                {
                    image.Value = value;
                    Zoom(ZoomMode.Reset);
                }
                image.Value = value;
                lock (image.ObjLock)
                {
                    if (ImageExist)
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(DisplayHandle);
                        HOperatorSet.DispObj(image.Value, hWindowControl1.HalconWindow);
                        HOperatorSet.SetSystem("flush_graphic", "true");
                    }
                    else
                    {
                        HOperatorSet.ClearWindow(DisplayHandle);
                    }
                }
            }
        }
        [Browsable(false)]
        public bool ImageExist
        {
            get
            {
                if (image == null)
                    return false;
                else
                    return image.Exist;
            }
        }

        #region Region

        private HsbRegions regionList;
        public HsbRegions RegionList
        {
            get
            {
                if (regionList == null)
                {
                    regionList = new HsbRegions();
                }
                return regionList;
            }
            set
            {

                if (regionList != null)
                    regionList.Dispose();
                regionList = value;

            }
        }

        public void AddRegion(HsbRegions region)
        {
            RegionList = region;
            DisplayRegion();
        }
        public void AddRegion(HsbRegion region)
        {
            RegionList.Dispose();
            RegionList.Add(region);
            DisplayRegion();
        }
        private void DisplayRegion()
        {
            foreach (HsbRegion item in RegionList)
            {
                lock (item.ObjLock)
                {
                    HOperatorSet.SetColor(DisplayHandle, item.Color.ToString());
                    HOperatorSet.SetDraw(DisplayHandle, item.Draw.ToString());
                    HOperatorSet.DispObj(item.Value, DisplayHandle);
                }
            }
        }
        #endregion

        #region Halcon算子相关
        private void GetImageSize(out double Width, out double Height)
        {
            lock (image.ObjLock)
            {
                try
                {
                    HTuple width, height;
                    HOperatorSet.GetImageSize(SourceImage, out width, out height);
                    Width = width[0].D;
                    Height = height[0].D;
                }
                catch
                {
                    Width = 0;
                    Height = 0;
                    Console.WriteLine("GetImageException!!");
                }
            }
        }

        private void GetMposition(out double Row, out double Column)
        {
            try
            {
                HTuple row, column, hv_Button;
                HOperatorSet.GetMposition(this.hWindowControl1.HalconWindow, out row, out column, out hv_Button);
                Row = row[0].I;
                Column = column[0].I;
            }
            catch
            {
                Row = 0.0;
                Column = 0.0;
                Console.WriteLine("GetMpositionException!!");

            }
        }

        private void GetViewPart(out double Row1, out double Col1, out double Row2, out double Col2)
        {
            try
            {
                HTuple m_ImgRow1, m_ImgCol1, m_ImgRow2, m_ImgCol2;
                HOperatorSet.GetPart(this.hWindowControl1.HalconWindow, out m_ImgRow1, out m_ImgCol1, out m_ImgRow2, out m_ImgCol2);
                Row1 = m_ImgRow1[0].I;
                Col1 = m_ImgCol1[0].I;
                Row2 = m_ImgRow2[0].I;
                Col2 = m_ImgCol2[0].I;
            }
            catch
            {
                Row1 = 0;
                Col1 = 0;
                GetImageSize(out Col2, out Row2);
                Console.WriteLine("GetViewPartException!!");

            }
        }
        private void SetViewPart(double m_ImgRow1, double m_ImgCol1, double m_ImgRow2, double m_ImgCol2)
        {
            try
            {
                HOperatorSet.SetPart(this.hWindowControl1.HalconWindow, m_ImgRow1, m_ImgCol1, m_ImgRow2, m_ImgCol2);
            }
            catch
            {
                Console.WriteLine("SetViewPartException!!");
            }
        }
        private void GetGrayval(double row, double col, out HTuple grayval)
        {
            lock (image.ObjLock)
            {
                try
                {
                    HTuple gray;
                    double imageHeight, imageWidth;
                    GetImageSize(out imageWidth, out imageHeight);
                    if (ImageExist && row > 0 && row < imageHeight && col > 0 && col < imageWidth)
                    {
                        HOperatorSet.GetGrayval(SourceImage, row, col, out gray);
                        grayval = gray;
                    }
                    else
                    {
                        grayval = 0;
                    }
                }
                catch
                {
                    Console.WriteLine("GetGrayvalException!!");

                    grayval = 0;
                }
            }
        }
        #endregion


        private void OpenImage()
        {
            OpenFileDialog openImageDialog = new OpenFileDialog();
            openImageDialog.Title = "打开图像";
            openImageDialog.Filter = "图像文件|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                HObject img;
                HOperatorSet.GenEmptyObj(out img);
                img.Dispose();
                HOperatorSet.ReadImage(out img, openImageDialog.FileName);
                DisplaySourceImage(img);
            }
        }
        public void ClearRegion()
        {
            foreach (HsbRegion item in RegionList)
            {
                lock (item.ObjLock)
                {
                    item.Dispose();
                }
            }
            if (ImageExist)
            {
                lock (image.ObjLock)
                {
                    HOperatorSet.DispObj(SourceImage, hWindowControl1.HalconWindow);
                }
            }
        }

        private void UpdateWindow()
        {
            if (ImageExist)
            {
                lock (image.ObjLock)
                {
                    HOperatorSet.SetSystem("flush_graphic", "false");
                    HOperatorSet.ClearWindow(DisplayHandle);

                    HOperatorSet.DispObj(SourceImage, hWindowControl1.HalconWindow);
                    HOperatorSet.SetSystem("flush_graphic", "true");
                    DisplayRegion();
                }
            }
        }

        private double ResetZoom = 0;

        /// <summary>
        /// 图像缩放
        /// </summary>
        /// <param name="type">缩放类型</param>
        private void Zoom(ZoomMode type)
        {
            if (!ImageExist)
            {
                return;
            }
            double row, column;
            double m_ImgRow1, m_ImgCol1, m_ImgRow2, m_ImgCol2;

            double scale = 1.0;

            GetMposition(out row, out column);
            switch (type)
            {
                case ZoomMode.ZoomIn:
                    scale = 1 / 0.9;
                    break;
                case ZoomMode.ZoomOut:
                    scale = 0.9;
                    break;
                case ZoomMode.Reset:
                    scale = 1.0;
                    break;
                default:
                    break;
            }

            double windowWidth = hWindowControl1.Width;
            double windowHeight = hWindowControl1.Height;
            double imageHeight, imageWidth;

            GetImageSize(out imageWidth, out imageHeight);
            double zoom = 1;
            if (type == ZoomMode.Reset)
            {
                ResetZoom = zoom = Math.Min(windowWidth / imageWidth, windowHeight / imageHeight);
                m_ImgRow1 = imageHeight / 2 - windowHeight / zoom / 2;
                m_ImgCol1 = imageWidth / 2 - windowWidth / zoom / 2;
                m_ImgRow2 = imageHeight / 2 + windowHeight / zoom / 2;
                m_ImgCol2 = imageWidth / 2 + windowWidth / zoom / 2;
            }
            else
            {
                GetViewPart(out m_ImgRow1, out m_ImgCol1, out m_ImgRow2, out m_ImgCol2);
                zoom = Math.Min(windowWidth / (m_ImgCol2 - m_ImgCol1), windowHeight / (m_ImgRow2 - m_ImgRow1));
            }
            //查看当前鼠标点在Old SetPartRectangle中的相对位置(相对于SetPartRectangle左上角点)
            double d_DistanceHeight = row - m_ImgRow1;
            double d_DistanceWidth = column - m_ImgCol1;
            double d_PercentSetPartHeight = d_DistanceHeight / (windowHeight / zoom);
            double d_PercentSetPartWidth = d_DistanceWidth / (windowWidth / zoom);

            zoom = scale * zoom;
            if (zoom > ResetZoom / 3)
            {
                //获取New SetPartRectangle的大小
                double SetPartRetHeight = windowHeight / zoom;//New SetPartRectangle的Height和Width
                double SetPartRetWidth = windowWidth / zoom;
                if (SetPartRetHeight > 20 && SetPartRetWidth > 20)
                {
                    //使鼠标点在New SetPartRectangle中的相对位置和在Old SetPartRectangle中的相对位置相同(相对于SetPartRectangle左上角点)
                    m_ImgRow1 = row - SetPartRetHeight * d_PercentSetPartHeight;
                    m_ImgCol1 = column - SetPartRetWidth * d_PercentSetPartWidth;
                    m_ImgRow2 = row + SetPartRetHeight * (1 - d_PercentSetPartHeight);
                    m_ImgCol2 = column + SetPartRetWidth * (1 - d_PercentSetPartWidth);
                    SetViewPart(m_ImgRow1, m_ImgCol1, m_ImgRow2, m_ImgCol2);
                }
                UpdateWindow();
                ClearWindow(imageWidth, imageHeight,
                   m_ImgRow1, m_ImgCol1, m_ImgRow2, m_ImgCol2);
            }
        }

        private void ClearWindow(double imageWidth, double imageHeight, double leftTopRow, double leftTopCol, double rightBottomRow, double rightBottomCol)
        {
            double heigh1 = rightBottomRow - leftTopRow;
            double width1 = rightBottomCol - leftTopCol;
            hWindowControl1.HalconWindow.ClearRectangle(
                0,
                0,
                hWindowControl1.Height,
                ((-leftTopCol) / width1 * hWindowControl1.Width));

            hWindowControl1.HalconWindow.ClearRectangle(
                0,
                0,
                ((-leftTopRow) / heigh1 * hWindowControl1.Height),
                hWindowControl1.Width);


            hWindowControl1.HalconWindow.ClearRectangle(
                ((imageHeight - leftTopRow) / heigh1 * hWindowControl1.Height),
                0,
                hWindowControl1.Height,
                hWindowControl1.Width);
            hWindowControl1.HalconWindow.ClearRectangle(
                0,
                ((imageWidth - leftTopCol) / width1 * hWindowControl1.Width),
                hWindowControl1.Height,
                hWindowControl1.Width);
        }

        private enum ZoomMode
        {
            ZoomIn,
            ZoomOut,
            Reset,
        }
        #region 交互事件
        public bool InteractiveEnable
        {
            set
            {
                ContextMenuStripEnable = value;
                MouseMoveEnable = value;
                MouseWheelEnable = value;
            }
            get { return MouseMoveEnable; }
        }
        public bool MouseWheelEnable = true;
        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (!MouseWheelEnable) return;
            if (!hWindowControl1.Focused) return;
            if (!ImageExist) return;
            if (e.Delta < 0)
            {
                Zoom(ZoomMode.ZoomIn);
            }
            else
            {
                Zoom(ZoomMode.ZoomOut);
            }
        }
        public bool MouseMoveEnable = true;
        private bool mouseDown = false;
        private HTuple mouseDownRow = 0;
        private HTuple mouseDownCol = 0;
        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (!MouseMoveEnable) return;
            if (!hWindowControl1.Focused) return;
            if (!ImageExist) return;
            HTuple gray;

            hWindowControl1.Focus();
            double row, col;

            GetMposition(out row, out col);

            GetGrayval(row, col, out gray);
            tsslCoord.Text = string.Format("{0:0} , {1:0}", row, col);
            HTuple hv_w, hv_h;
            HOperatorSet.GetImageSize(this.SourceImage, out hv_w, out hv_h);
            tsslSize.Text = string.Format("{0:0} * {1:0}", hv_w.I, hv_h.I);

            HTuple a;
            HOperatorSet.GetLut(DisplayHandle, out a);
            if (gray.Length == 1)
            {
                tsslGray.Text = string.Format("{0:0.000}", gray[0].D);
            }
            else if (gray.Length == 3)
            {
                tsslGray.Text = string.Format("{0:000},{1:000},{2:000}", gray[0].D, gray[1].D, gray[2].D);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

                HTuple hv_MouseCurrentRow, hv_MouseCurrentCol;
                if (!mouseDown)
                {
                    mouseDown = true;
                    //HOperatorSet.GetMposition(hWindowControl1.HalconWindow, out mouseDownRow, out mouseDownCol, out hv_Button);
                    mouseDownRow = row;
                    mouseDownCol = col;
                }
                hv_MouseCurrentRow = row;
                hv_MouseCurrentCol = col;

                //HOperatorSet.GetMposition(hWindowControl1.HalconWindow, out hv_MouseCurrentRow, out hv_MouseCurrentCol, out hv_Button);
                HTuple row1, col1, row2, col2;
                HOperatorSet.GetPart(hWindowControl1.HalconWindow, out row1, out col1, out row2, out col2);
                double offsetRow = hv_MouseCurrentRow[0].D - mouseDownRow[0].D;
                double offsetCol = hv_MouseCurrentCol[0].D - mouseDownCol[0].D;

                double leftTopRow = row1[0].D - offsetRow;
                double leftTopCol = col1[0].D - offsetCol;
                double rightBottomRow = row2[0].D - offsetRow;
                double rightBottomCol = col2[0].D - offsetCol;
                hWindowControl1.HalconWindow.SetPart((int)leftTopRow,
                    (int)leftTopCol,
                    (int)rightBottomRow,
                    (int)rightBottomCol);
                double imageWidth, imageHeight;
                GetImageSize(out imageWidth, out imageHeight);
                ClearWindow(imageWidth, imageHeight, leftTopRow, leftTopCol, rightBottomRow, rightBottomCol);

                UpdateWindow();
            }
            else
            {
                mouseDown = false;
            }
        }


        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (isFirst)
                {
                    timer.Enabled = true;
                    timer.Start();
                    dc = true;
                    isFirst = false;
                }
                else
                {
                    if (dc)
                    {
                        Zoom(ZoomMode.Reset);
                    }
                    dc = false;
                    isFirst = true;
                }
            }
        }

        #endregion
        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="img"></param>
        public void DisplaySourceImage(HObject img)
        {
            SourceImage = img.Clone();
            ClearRegion();
        }
        private int MessageFontSize = 12;
        /// <summary>
        /// 在窗体左上角开始显示文本
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="row"></param>
        /// <param name="color"></param>
        /// <param name="backColor"></param>
        public void DisplayMessage(string msg, int row, HsbColor color, bool backColor = false)
        {
            disp_message(DisplayHandle,
                msg,
                "image",
                MessageFontSize + row * (MessageFontSize + 10),
                MessageFontSize,
                color.ToString(),
                backColor ? "true" : "false");
        }
        /// <summary>
        /// 在图像的对应区域显示文本
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="color"></param>
        /// <param name="backColor"></param>
        public void DisplayMessage(string msg, HTuple row, HTuple col, HsbColor color, bool backColor = false)
        {
            disp_message(DisplayHandle,
                msg,
                "image",
                row,
                col,
                color.ToString(),
                backColor ? "true" : "false");
        }
        private void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
       HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_MaxAscent = null;
            HTuple hv_MaxDescent = null, hv_MaxWidth = null, hv_MaxHeight = null;
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRow = new HTuple();
            HTuple hv_FactorColumn = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple();
            HTuple hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
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



        private bool isFirst = true;
        private bool dc = false;
        private Timer timer = new Timer();
        private void InitDoubleClick()
        {
            timer.Interval = 300;
            timer.Tick += new EventHandler((o, c) =>
            {
                dc = false;
                isFirst = true;
                timer.Stop();
                timer.Enabled = false;
            });
        }


        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {

        }
    }
}
