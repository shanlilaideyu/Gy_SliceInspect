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
using System.IO;

namespace akControl
{
    public partial class akHalWindow : UserControl
    {
        private HWindow mWindow = (HWindow)null;
        private HTuple HalSystemColor;
        private HImage background_image = (HImage)null;
        private List<DrawingShowObject> drawing_objects = new List<DrawingShowObject>();
        private HDrawingObject selected_drawing_object = (HDrawingObject)null;
        private Stack<RegionShowObject> graphic_stack = new Stack<RegionShowObject>();
        private object stack_lock = new object();
        private HTuple imgWidth = (HTuple)null;
        private HTuple imgHeight = (HTuple)null;
        private double sc1 = Math.Sqrt(2.0);
        private double sc2 = 1.0 / Math.Sqrt(2.0);
        private bool isScMax = false;
        private bool isScMin = false;
        private ContextMenuStrip contextMenuStrip;
        private Point cmsLocation = Point.Empty;
        private int cmsHalLocationRow = 0;
        private int cmsHalLocationCol = 0;
        public akHalWindow()
        {
            InitializeComponent();
            this.InitContextMenu();
            this.pbox.MouseWheel += new MouseEventHandler(this.pbox_MouseWheel);

        }
        public void init()
        {
            this.mWindow = new HWindow();
            this.mWindow.OpenWindow(0, 0, this.pbox.Width, this.pbox.Height, this.pbox.Handle, "", "");
            this.HalSystemColor = this.mWindow.QueryColor();
            HObject img;
            HOperatorSet.GenImageConst(out img, "byte", 2500, 2500);
        }
        public void SetImage(HTuple fileName)
        {
            this.ClearBackImage();
            if (fileName == null || fileName.Length <= 0 || !File.Exists(fileName.S))
                return;
            this.background_image = new HImage(fileName);
            this.AttachBackImage();
        }
        public void SetImage(string fileName)
        {
            this.ClearBackImage();
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return;
            this.background_image = new HImage(fileName);
            this.AttachBackImage();
        }
        public void SetImage(HObject img)
        {
            if (img == null || !img.IsInitialized())
            {
                this.mWindow.ClearWindow();
                this.ClearBackImage();
            }
            else
            {
                this.ClearBackImage();
                this.background_image = new HImage(img);
                this.AttachBackImage();
            }
        }
        private void AttachBackImage()
        {
            try
            {
                if (this.mWindow == null)
                    return;
                this.mWindow.SetWindowExtents(0, 0, this.pbox.Width, this.pbox.Height);
                this.background_image.GetImageSize(out this.imgWidth, out this.imgHeight);
                this.mWindow.SetPart(0, 0, this.imgHeight.I - 1, this.imgWidth.I - 1);
                this.mWindow.AttachBackgroundToWindow(this.background_image);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void ClearBackImage()
        {
            if (this.background_image == null)
                return;
            this.mWindow.DetachBackgroundFromWindow();
            this.background_image.Dispose();
            this.background_image = (HImage)null;
        }
        public void ClearWindow()
        {
            if (this.mWindow == null)
                return;
            this.mWindow.ClearWindow();
        }

        public HTuple DrawRectangle1(ref HTuple drawID, int row1 = 50, int col1 = 50, int row2 = 100, int col2 = 100, string color = "green")
        {
          //  HTuple drawID = null;
            try
            {              
                if (!this.isRightColor(color))
                    color = "green";
                if (drawID!=null)
                {
                    HOperatorSet.DetachDrawingObjectFromWindow(this.mWindow, drawID);
                }
              
                HOperatorSet.CreateDrawingObjectRectangle1((HTuple)row1, (HTuple)col1, (HTuple)row2, (HTuple)col2,out drawID);
                HOperatorSet.SetDrawingObjectParams(drawID,(HTuple)nameof(color), (HTuple)color);
                HOperatorSet.AttachDrawingObjectToWindow(this.mWindow, drawID);

                //
                // HDrawingObject drawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, (HTuple)row1, (HTuple)col1, (HTuple)row2, (HTuple)col2);
                // drawingObject.SetDrawingObjectParams((HTuple)nameof(color), (HTuple)color);
                return drawID;
            }
            catch (Exception ex)
            {
                return drawID;
            }
        }

      
        public HTuple DrawRectangle2(
    int row = 50,
    int col = 50,
    double phi = 0.0,
    int width = 100,
    int height = 100,
    string color = "green")
        {
            HTuple drawID = null;
            if (!this.isRightColor(color))
                color = "green";
            HOperatorSet.CreateDrawingObjectRectangle2((HTuple)row, (HTuple)col, (HTuple)phi, (HTuple)width, (HTuple)height, out drawID);         
            HOperatorSet.SetDrawingObjectParams(drawID, (HTuple)nameof(color), (HTuple)color);
            HOperatorSet.AttachDrawingObjectToWindow(this.mWindow, drawID);
            return drawID;
        }



        public HTuple DrawCircle(int row = 50, int col = 50, int radius = 50, string color = "green")
        {
            HTuple drawID = null;
            if (!this.isRightColor(color))
                color = "green";
            HOperatorSet.CreateDrawingObjectCircle((HTuple)row, (HTuple)col, (HTuple)radius, out drawID);
            HOperatorSet.SetDrawingObjectParams(drawID, (HTuple)nameof(color), (HTuple)color);
            HOperatorSet.AttachDrawingObjectToWindow(this.mWindow, drawID);
            return drawID;

          
        }
        public long DrawEllipse(
     int row = 50,
     int col = 50,
     double phi = 0.0,
     int radius1 = 50,
     int radius2 = 50,
     string color = "green")
        {
            HTuple drawID = null;
            if (!this.isRightColor(color))
                color = "green";
            HOperatorSet.CreateDrawingObjectEllipse((HTuple)row, (HTuple)col, (HTuple)phi, (HTuple)radius1, (HTuple)radius2, out drawID);
            HOperatorSet.SetDrawingObjectParams(drawID, (HTuple)nameof(color), (HTuple)color);
            HOperatorSet.AttachDrawingObjectToWindow(this.mWindow, drawID);
            return drawID;
        }
        public HTuple DrawLine(int row1 = 50, int col1 = 50, int row2 = 50, int col2 = 100, string color = "green")
        {
            HTuple drawID = null;
            if (!this.isRightColor(color))
                color = "green";
            HOperatorSet.CreateDrawingObjectLine((HTuple)row1, (HTuple)col1, (HTuple)row2, (HTuple)col2, out drawID);
            HOperatorSet.SetDrawingObjectParams(drawID, (HTuple)nameof(color), (HTuple)color);
            HOperatorSet.AttachDrawingObjectToWindow(this.mWindow, drawID);
            return drawID;
        }
      
       
        public long DrawXld(double[] row, double[] col, string color = "green")
        {
            if (!this.isRightColor(color))
                color = "green";
            if (row == null || row.Length <= 0 || (col == null || col.Length <= 0 || col.Length != row.Length))
                return 0;
            HDrawingObject drawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.XLD_CONTOUR, (HTuple)row, (HTuple)col);
            drawingObject.SetDrawingObjectParams((HTuple)nameof(color), (HTuple)color);
            this.AttachDrawObj(drawingObject, "xld");
            return drawingObject.ID;
        }

        private void AttachDrawObj(HDrawingObject obj, string type)
        {
            this.drawing_objects.Add(new DrawingShowObject()
            {
                drawObject = obj,
                objType = type
            });
            obj.OnSelect(new HDrawingObject.HDrawingObjectCallbackClass(this.OnSelectDrawingObject));
            this.mWindow.AttachDrawingObjectToWindow(obj);
        }

        private void OnSelectDrawingObject(HDrawingObject dobj, HWindow hwin, string type) => this.selected_drawing_object = dobj;

        private void DeleteDrawingObject(HDrawingObject obj)
        {
            if (obj == null || !obj.IsInitialized())
                return;
            this.mWindow.DetachDrawingObjectFromWindow(obj);
            this.drawing_objects.Remove(this.drawing_objects.Find((Predicate<DrawingShowObject>)(t => t.drawObject.ID == obj.ID)));
            obj.Dispose();
        }
        public void DeleteAllDrawingObject()
        {
            if (this.drawing_objects == null)
                return;
            if (this.mWindow != null)
            {
                foreach (DrawingShowObject drawingObject in this.drawing_objects)
                    this.mWindow.DetachDrawingObjectFromWindow(drawingObject.drawObject);
            }
            this.drawing_objects.Clear();
        }

        //public List<Tuple<string, double[]>> GetAllGraphicParam()
        //{
        //    List<Tuple<string, double[]>> tupleList = new List<Tuple<string, double[]>>();
        //    foreach (DrawingShowObject drawingObject in this.drawing_objects)
        //        tupleList.Add(Tuple.Create<string, double[]>(drawingObject.objType, this.GetGraphicParam(drawingObject.objType, (HTuple)drawingObject.drawObject.ID)));
        //    return tupleList;
        //}

        //public Tuple<string, double[]> GetSelectGraphicParam()
        //{
        //    string objType = this.drawing_objects.Find((Predicate<DrawingShowObject>)(t => t.drawObject.ID == this.selected_drawing_object.ID)).objType;
        //    return Tuple.Create<string, double[]>(objType, this.GetGraphicParam(objType, (HTuple)this.selected_drawing_object.ID));
        //}

        public List<HObject> GetAllGraphicObject()
        {
            List<HObject> hobjectList = new List<HObject>();
            HObject objectVal = new HObject();
            foreach (DrawingShowObject drawingObject in this.drawing_objects)
                HOperatorSet.GetDrawingObjectIconic(out objectVal, (HTuple)drawingObject.drawObject.ID);
            return hobjectList;
        }
        public HObject GetSelectGraphicObject()
        {
            HObject objectVal = new HObject();
            HOperatorSet.GetDrawingObjectIconic(out objectVal, (HTuple)this.selected_drawing_object.ID);
            return objectVal;
        }
        public void DispObj(HObject obj, string drawType = "fill", string color = "red")
        {
            if (obj == null || !obj.IsInitialized())
                return;
            if (!this.isRightColor(color))
                color = (string)null;
            if (drawType != "fill" && drawType != "margin")
                drawType = (string)null;
            this.AddToStack(new RegionShowObject()
            {
                region = obj,
                drawColor = color,
                drawType = drawType
            });
            this.DisplayGraphicStack();
        }
        private void AddToStack(RegionShowObject obj)
        {
            lock (this.stack_lock)
              this.graphic_stack.Push(obj);
        }

        public void SetFont(HTuple hv_size) => clsCommon.set_display_font((HTuple)this.mWindow, hv_size, (HTuple)"mono", (HTuple)"true", (HTuple)"false");

        public void DispMessage(
          string text,
          int row,
          int col,
          string color = "red",
          bool isShowWithBox = false,
          string coordSystem = "image")
        {
            if (!this.isRightColor(color))
                color = "red";
            clsCommon.disp_message((HTuple)this.mWindow, (HTuple)text, (HTuple)coordSystem, (HTuple)row, (HTuple)col, (HTuple)color, (HTuple)(isShowWithBox ? "true" : "false"));
        }

        public void DispMessage1(string text, int row, int col, string color = "red")
        {
            if (!this.isRightColor(color))
                color = "red";
            HOperatorSet.SetColor((HTuple)this.mWindow, (HTuple)color);
            HOperatorSet.SetTposition((HTuple)this.mWindow, (HTuple)row, (HTuple)col);
            HOperatorSet.WriteString((HTuple)this.mWindow, (HTuple)text);
        }
        public double[] GetGraphicParam(string type, HTuple objId)
        {
            double[] numArray = (double[])null;
            HTuple paramValues = null;
            //objId = objId.;
            //HHandle handle = objId.H;
            switch (type)
            {
                case "rectangle1": 
                    HOperatorSet.GetDrawingObjectParams(objId, (HTuple)new string[4]
                    {
            "row1",
            "column1",
            "row2",
            "column2"
                    }, out paramValues);

                    
                    numArray = paramValues.ToDArr();
                    break;
                case "rectangle2":
                    HOperatorSet.GetDrawingObjectParams((HTuple)objId, (HTuple)new string[5]
                    {
            "row",
            "column",
            "phi",
            "length1",
            "length2"
                    }, out paramValues);
                    numArray = paramValues.ToDArr();
                    break;
                case "circle":
                    HOperatorSet.GetDrawingObjectParams(objId, (HTuple)new string[3]
                    {
            "row",
            "column",
            "radius"
                    }, out paramValues);
                    numArray = paramValues.ToDArr();
                    break;
                case "circle_sector":
                    HOperatorSet.GetDrawingObjectParams(objId, (HTuple)new string[5]
                    {
            "row",
            "column",
            "radius",
            "start_angle",
            "end_angle"
                    }, out paramValues);
                    numArray = paramValues.ToDArr();
                    break;
                case "ellipse":
                    HOperatorSet.GetDrawingObjectParams(objId, (HTuple)new string[5]
                    {
            "row",
            "column",
            "phi",
            "radius1",
            "radius2"
                    }, out paramValues);
                    numArray = paramValues.ToDArr();
                    break;
                case "ellipse_sector":
                    HOperatorSet.GetDrawingObjectParams(objId, (HTuple)new string[7]
                    {
            "row",
            "column",
            "phi",
            "radius1",
            "radius2",
            "start_angle",
            "end_angle"
                    }, out paramValues);
                    numArray = paramValues.ToDArr();
                    break;
                case "line":
                    HOperatorSet.GetDrawingObjectParams(objId, (HTuple)new string[4]
                    {
            "row1",
            "column1",
            "row2",
            "column2"
                    }, out paramValues);
                    numArray = paramValues.ToDArr();
                    break;
                case "xld":
                    HOperatorSet.GetDrawingObjectParams(objId, (HTuple)new string[2]
                    {
            "row",
            "column"
                    }, out paramValues);
                    numArray = paramValues.ToDArr();
                    break;
            }
            return numArray;
        }
        private void pbox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.mWindow == null)
                return;
            double direction = e.Delta > 0 ? this.sc1 : this.sc2;
            try
            {
                int row;
                int column;
                int _;
                this.mWindow.GetMposition(out row, out column, out _);
                this.ScaleShow(direction, e.Location, row, column);
            }
            catch (Exception ex)
            {
            }
        }
        private void pbox_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                int _;
                this.mWindow.GetMposition(out this.cmsHalLocationRow, out this.cmsHalLocationCol, out _);
                if (this.contextMenuStrip != null && e.Button == MouseButtons.Right && this.contextMenuStrip.Items.Count > 0)
                {
                    this.contextMenuStrip.Show((Control)this, new Point(e.X, e.Y));
                    this.cmsLocation = e.Location;
                    this.tsmDelGraphic.Enabled = true;
                    this.tsmDelAllGraphic.Enabled = true;
                    this.tsmDelObj.Enabled = true;
                    if (this.background_image == null)
                    {
                        this.SetMenuItemEnabled("AdpatSize", false);
                        this.SetMenuItemEnabled("saveImg", false);
                    }
                    else
                    {
                        this.SetMenuItemEnabled("AdpatSize", true);
                        this.SetMenuItemEnabled("saveImg", true);
                    }
                    if (this.background_image == null || this.isScMax)
                        this.SetMenuItemEnabled("zoomUp", false);
                    else
                        this.SetMenuItemEnabled("zoomUp", true);
                    if (this.background_image == null || this.isScMin)
                        this.SetMenuItemEnabled("zoomDown", false);
                    else
                        this.SetMenuItemEnabled("zoomDown", true);
                    if (this.drawing_objects.Count <= 0 || this.selected_drawing_object == null)
                        this.tsmDelGraphic.Enabled = false;
                    if (this.drawing_objects.Count <= 0)
                        this.tsmDelAllGraphic.Enabled = false;
                    if (this.drawing_objects.Count > 0 || this.graphic_stack.Count > 0)
                        return;
                    this.tsmDelObj.Enabled = false;
                }
                else
                    this.cmsLocation = Point.Empty;
            }
            catch (Exception ex)
            {
            }
        }

        private void tsmAdpatSize_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.mWindow == null)
                    return;
                this.mWindow.SetWindowExtents(0, 0, this.pbox.Width, this.pbox.Height);
                this.mWindow.SetPart(0, 0, this.imgHeight.I - 1, this.imgWidth.I - 1);
            }
            catch (Exception ex)
            {
            }
        }

        private void tsmDelGraphic_Click(object sender, EventArgs e)
        {
            this.DeleteDrawingObject(this.selected_drawing_object);
            this.selected_drawing_object = (HDrawingObject)null;
        }

        private void tsmDelAllGraphic_Click(object sender, EventArgs e)
        {
            lock (this.stack_lock)
            {
                foreach (DrawingShowObject drawingObject in this.drawing_objects)
                {
                    if (drawingObject.drawObject != null)
                        drawingObject.drawObject.Dispose();
                }
                this.selected_drawing_object = (HDrawingObject)null;
                this.drawing_objects.Clear();
            }
            this.DisplayGraphicStack();
        }

        private void tsmDelObj_Click(object sender, EventArgs e)
        {
            lock (this.stack_lock)
            {
                foreach (DrawingShowObject drawingObject in this.drawing_objects)
                    drawingObject.drawObject.Dispose();
                this.drawing_objects.Clear();
                this.selected_drawing_object = (HDrawingObject)null;
                this.graphic_stack.Clear();
            }
            this.DisplayGraphicStack();
        }
        private void DisplayGraphicStack()
        {
            lock (this.stack_lock)
            {
                HOperatorSet.SetSystem((HTuple)"flush_graphic", (HTuple)"false");
                this.mWindow.ClearWindow();
                string color = string.Empty;
                string mode = string.Empty;
                string draw = this.mWindow.GetDraw();
                while (this.graphic_stack.Count > 0)
                {
                    RegionShowObject regionShowObject = this.graphic_stack.Pop();
                    if (regionShowObject.drawColor != null && color != regionShowObject.drawColor)
                    {
                        color = regionShowObject.drawColor;
                        this.mWindow.SetColor(color);
                    }
                    if (regionShowObject.drawType != null && mode != regionShowObject.drawType)
                    {
                        mode = regionShowObject.drawType;
                        this.mWindow.SetDraw(mode);
                    }
                    this.mWindow.DispObj(regionShowObject.region);
                }
                if (draw != mode)
                    this.mWindow.SetDraw(draw);
                HOperatorSet.SetSystem((HTuple)"flush_graphic", (HTuple)"true");
            }
            this.mWindow.DispCross(-10.0, -10.0, 3.0, 0.0);
        }
        private void InitContextMenu() => this.contextMenuStrip = new ContextMenuStrip();

        public void RegisteredDefaultContextMenu()
        {
            if (this.contextMenuStrip == null)
                this.InitContextMenu();
            if (this.contextMenuStrip == null)
                return;
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem1.Size = new Size(168, 24);
            toolStripMenuItem1.Text = "放大";
            toolStripMenuItem1.Name = "zoomUp";
            if (File.Exists("ico/zoomup.png"))
                toolStripMenuItem1.Image = Image.FromFile("ico/zoomup.png");
            toolStripMenuItem1.Click += (EventHandler)((o, e) =>
            {
                if (this.background_image == null || this.cmsLocation == Point.Empty)
                    return;
                this.ScaleShow(this.sc2, this.cmsLocation, this.cmsHalLocationRow, this.cmsHalLocationCol);
            });
            this.contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem1);
            ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem2.Size = new Size(168, 24);
            toolStripMenuItem2.Text = "缩小";
            toolStripMenuItem2.Name = "zoomDown";
            if (File.Exists("ico/zoomdown.png"))
                toolStripMenuItem2.Image = Image.FromFile("ico/zoomdown.png");
            toolStripMenuItem2.Click += (EventHandler)((o, e) =>
            {
                if (this.background_image == null)
                    return;
                this.ScaleShow(this.sc1, this.cmsLocation, this.cmsHalLocationRow, this.cmsHalLocationCol);
            });
            this.contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem3.Size = new Size(168, 24);
            toolStripMenuItem3.Text = "原比例";
            toolStripMenuItem3.Name = "AdpatSize";
            if (File.Exists("ico/zoomresume.png"))
                toolStripMenuItem3.Image = Image.FromFile("ico/zoomresume.png");
            toolStripMenuItem3.Click += (EventHandler)((o, e) =>
            {
                if (this.background_image == null || (this.imgHeight == null || this.imgWidth == null))
                    return;
                this.mWindow.SetPart(0, 0, this.imgHeight.I - 1, this.imgWidth.I - 1);
            });
            this.contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem3);
            ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem4.Size = new Size(168, 24);
            toolStripMenuItem4.Text = "保存图片";
            toolStripMenuItem4.Name = "saveImg";
            if (File.Exists("ico/saveimage.png"))
                toolStripMenuItem4.Image = Image.FromFile("ico/saveimage.png");
            toolStripMenuItem4.Click += (EventHandler)((o, e) =>
            {
                if (this.background_image == null)
                    return;
                try
                {
                    this.background_image.WriteImage("jpeg", 0, DateTime.Now.ToString("yyyymmdd"));
                }
                catch (Exception ex)
                {
                }
            });
            this.contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem4);
        }

        public void RegisteredContextMenu(string menuText, EventHandler e)
        {
            if (this.contextMenuStrip == null || this.contextMenuStrip.Items.ContainsKey(menuText))
                return;
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Size = new Size(168, 24);
            toolStripMenuItem.Text = menuText;
            toolStripMenuItem.Name = menuText;
            toolStripMenuItem.Click += new EventHandler(e.Invoke);
            this.contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem);
        }

        public void SetContextMenuEnabled(string itemName, bool isEnabled)
        {
            if (!this.contextMenuStrip.Items.ContainsKey(itemName))
                return;
            this.contextMenuStrip.Items[itemName].Enabled = isEnabled;
        }

        public void ChangeContextMenuItem(
          string itemName,
          string text,
          EventHandler oldEvent,
          EventHandler newEvent)
        {
            if (!this.contextMenuStrip.Items.ContainsKey(itemName))
                return;
            this.contextMenuStrip.Items[itemName].Text = text;
            this.contextMenuStrip.Items[itemName].Click -= new EventHandler(oldEvent.Invoke);
            this.contextMenuStrip.Items[itemName].Click += new EventHandler(newEvent.Invoke);
        }
        private void SetMenuItemEnabled(string itemName, bool isEnabled)
        {
            if (this.contextMenuStrip == null || !this.contextMenuStrip.Items.ContainsKey(itemName))
                return;
            this.contextMenuStrip.Items[itemName].Enabled = isEnabled;
        }
        public void ChangeContextMenuItemText(string itemName, string text)
        {
            if (!this.contextMenuStrip.Items.ContainsKey(itemName))
                return;
            this.contextMenuStrip.Items[itemName].Text = text;
        }


        private bool isRightColor(string color)
        {
            HTuple htuple = this.HalSystemColor.TupleFind((HTuple)color);
            return htuple != null && htuple.Length != 0;
        }

        private void ScaleShow(double direction, Point e, int row, int col)
        {
            if (this.imgWidth == null || this.imgHeight == null)
                return;
            int row1 = 0;
            int column1 = 0;
            int row2 = 0;
            int column2 = 0;
            this.mWindow.GetPart(out row1, out column1, out row2, out column2);
            double num1 = (double)row - (double)(row2 - row1) * direction * (double)e.Y / (double)this.pbox.Height;
            double num2 = (double)row + (double)(row2 - row1) * direction * (1.0 - (double)e.Y / (double)this.pbox.Height);
            double num3 = (double)col - (double)(column2 - column1) * direction * (double)e.X / (double)this.pbox.Width;
            double num4 = (double)col + (double)(column2 - column1) * direction * (1.0 - (double)e.X / (double)this.pbox.Width);
            if (Math.Abs(num4 - num3) < 5.0 || Math.Abs(num2 - num1) < 5.0)
                return;
            double val1 = (double)((HTuple)Math.Abs(num4 - num3) / this.imgWidth);
            double val2 = (double)((HTuple)Math.Abs(num2 - num1) / this.imgHeight);
            if (Math.Max(val1, val2) > 8.0)
            {
                this.isScMin = true;
            }
            else
            {
                this.isScMin = false;
                if (Math.Max(val1, val2) < 0.125)
                {
                    this.isScMax = true;
                }
                else
                {
                    this.isScMax = false;
                    this.mWindow.SetPart(Convert.ToInt32(num1), Convert.ToInt32(num3), Convert.ToInt32(num2), Convert.ToInt32(num4));
                }
            }
        }
    }
}
