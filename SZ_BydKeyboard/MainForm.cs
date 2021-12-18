using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
using LogRecord;
using LeoCam;
using HalconDotNet;
using System.Runtime.InteropServices;
using LeoMotion;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Ports;
using log4net;

namespace SZ_BydKeyboard
{
    public partial class MainForm : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //读取控件布局
        private IDockContent GetContentFromPersistString(string persistString)
        {
            try
            {
                if (persistString == typeof(FrmLogger).ToString())
                {
                    return Common.frmLog;
                }
                if (persistString == typeof(FrmShow).ToString())
                {
                    return Common.frmShow;
                }
                if (persistString == typeof(FrmDataSum).ToString())
                {
                    return Common.frmDataSum;
                }
                //if (persistString == typeof(FrmStatistics).ToString())
                //{
                //    return Common.frmStatistics;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Exception(this.GetType(), ex);

            }
            return null;
        }

        //初始化布局
        private void InitMainformLayout()
        {
            if (File.Exists(Path.Combine(Application.StartupPath, "SysCfg\\CustomUI.xml")))
            {
                DeserializeDockContent dd = new DeserializeDockContent(GetContentFromPersistString);
                MainPanel.LoadFromXml(Path.Combine(Application.StartupPath, "SysCfg\\CustomUI.xml"), dd);
            }
            else
            {
                Common.frmLog.Show(this.MainPanel, DockState.DockRight);
                Common.frmShow.Show(this.MainPanel, DockState.Document);
                Common.frmDataSum.Show(this.MainPanel, DockState.DockRight);
            }
        }

        private void InitPort()
        {
            if (Common.OpenSerialPort(Common.DataReadPort1, Common.Data.DataReadPortParam1))
            {
                Common.DataReadPort1.DataReceived += DataReadPort1_DataReceived;
                Common.ShowMsgEvent($"Tips:打开[读码器1]串口成功!");
                labelItem2.SymbolColor = Color.White;
                labelItem2.ForeColor = Color.White;
            }
            else
            {
                labelItem2.SymbolColor = Color.Orange;
                labelItem2.ForeColor = Color.Orange;
                Common.ShowMsgEvent($"Error:打开串口[读码器1]失败!");
            }

            if (Common.OpenSerialPort(Common.DataReadPort2, Common.Data.DataReadPortParam2))
            {
                Common.DataReadPort2.DataReceived += DataReadPort2_DataReceived;
                Common.ShowMsgEvent($"Tips:打开[读码器2]串口成功!");
                labelItem6.SymbolColor = Color.White;
                labelItem6.ForeColor = Color.White;
            }
            else
            {
                labelItem6.SymbolColor = Color.Orange;
                labelItem6.ForeColor = Color.Orange;
                Common.ShowMsgEvent($"Error:打开串口[读码器2]失败!");
            }
            if (Common.OpenSerialPort(Common.HeightSensorPort, Common.Data.HeightSensorPortParam))
            {
                Common.HeightSensorPort.DataReceived += HeightSensorPort_DataReceived;
                Common.ShowMsgEvent($"Tips:打开[高度传感器]串口成功!");
                labelItem7.SymbolColor = Color.White;
                labelItem7.ForeColor = Color.White;
            }
            else
            {
                labelItem7.SymbolColor = Color.Orange;
                labelItem7.ForeColor = Color.Orange;
                Common.ShowMsgEvent($"Error:打开串口[高度传感器]失败!");
            }

            if (Common.OpenSerialPort(Common.LightControlPort, Common.Data.LightControlPortParam))
            {
                Common.ShowMsgEvent($"Tips:打开[光源控制器]串口成功!");
                labelItem8.SymbolColor = Color.White;
                labelItem8.ForeColor = Color.White;
            }
            else
            {
                labelItem8.SymbolColor = Color.Orange;
                labelItem8.ForeColor = Color.Orange;
                Common.ShowMsgEvent($"Error:打开串口[光源控制器]失败!");
            }

            if (Common.OpenSerialPort(Common.IoPort, Common.Data.IoPortParam))
            {
                Common.ShowMsgEvent($"Tips:打开[IO模块]串口成功!");
                labelItem9.SymbolColor = Color.White;
                labelItem9.ForeColor = Color.White;
            }
            else
            {
                labelItem9.SymbolColor = Color.Orange;
                labelItem9.ForeColor = Color.Orange;
                Common.ShowMsgEvent($"Error:打开串口[IO模块]失败!");
            }
        }

        private void HeightSensorPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Common.HeightSensorPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[Common.HeightSensorPort.BytesToRead];
                Common.HeightSensorPort.Read(buffer, 0, Common.HeightSensorPort.BytesToRead);
                Common.HeightSensorPort.DiscardInBuffer();

                string HeightString = Encoding.ASCII.GetString(buffer);
                if (HeightString.Length > 3)
                {
                    double HeightValue = double.Parse(HeightString.Substring(1, HeightString.Length - 2));
                    Common.HeightMeasureZ.Add(HeightValue);
                    if (Common.ProC.nHeightMeasure == 20)
                    {
                        Common.ProC.nHeightMeasure = 30;
                    }
                    Common.ShowMsgEvent($"Tips:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}--当前测得高度值[{HeightValue}]--");
                }
            }
        }

        private void DataReadPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Common.DataReadPort2.BytesToRead > 0)
            {
                string str = Common.DataReadPort2.ReadTo("\r\n").Substring(1);
                if (str.Length > 1)
                {
                    Common.str_DataCode2 = str;
                    LogManager.GetLogger(typeof(MainForm)).Debug($"=======DataReadPort2_DataReceived {str}========");
                    Common.modelMgr.OnReceiveBarcode(str);
                    Common.ShowMsgEvent($"Tips:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}--工位2二维码为:[{ Common.str_DataCode2}]--");
                }
            }
        }

        private void DataReadPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Common.DataReadPort1.BytesToRead > 0)
            {
                string str = Common.DataReadPort1.ReadTo("\r\n").Substring(1);
                if (str.Length > 1)
                {
                    Common.str_DataCode1 = str;
                    LogManager.GetLogger(typeof(MainForm)).Debug($"=======DataReadPort1_DataReceived {str}========");
                    Common.modelMgr.OnReceiveBarcode(str);
                    Common.ShowMsgEvent($"Tips:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}--工位1二维码为:[{ Common.str_DataCode1}]--");
                }
            }
        }

        private void InitCamera()
        {
            try
            {
                Common.Cam1 = new ECamera();
                if (Common.Cam1.Open("CAM1"))
                {
                    Common.ShowMsgEvent($"Tips:打开相机[CAM1]成功!");
                    Common.Cam1.grabFinishCall = Common.grabImage1;
                }
                else
                {
                    Common.ShowMsgEvent($"Error:打开相机[CAM1]失败!");
                }
            }
            catch (Exception EX)
            {
                Common.ShowMsgEvent($"Error:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}{EX.Message}");
                LogHelper.Exception(this.GetType(), EX);
            }
        }

        public void LoadParam()
        {
            string fp = $"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\RunningData.json";
            if (File.Exists(fp))
            {
                Common.Data = JsonConvert.DeserializeObject<RunningData>(File.ReadAllText(fp));
                if (Common.Data.NgCount.Length == 0)
                {
                    Common.Data.NgCount = new int[Common.FaiNames.Length];
                }
            }
            else
            {
                Common.Data = new RunningData();
                Common.Data.DataReadPortParam1 = new PortParam("COM11", 19200, StopBits.One, 8, Parity.None);
                Common.Data.DataReadPortParam2 = new PortParam("COM12", 19200, StopBits.One, 8, Parity.None);
                Common.Data.HeightSensorPortParam = new PortParam("COM2", 9600, StopBits.One, 8, Parity.None);
                Common.Data.LightControlPortParam = new PortParam("COM3", 19200, StopBits.One, 8, Parity.None);
                Common.Data.IoPortParam = new PortParam("COM5", 19200, StopBits.One, 8, Parity.None);
            }

            fp = $"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\Mes.json";
            if (File.Exists(fp))
            {
                Common.mes = JsonConvert.DeserializeObject<MesData>(File.ReadAllText(fp));
            }
            else
            {
                Common.mes = new MesData();
                Common.mes.mac_address = MacAddressHelper.GetMacByIpConfig() ?? MacAddressHelper.GetMacByNetworkInterface()[0] ?? "unknown";
            }
        }

        public void LoadMultAndAdd()
        {
            HTuple hv_FileHandle, hv_String, hv_IsEOF;
            HOperatorSet.OpenFile($"{Application.StartupPath}\\Config\\{Common.str_ProductName}\\MeasureRevise.csv", "input", out hv_FileHandle);
            HOperatorSet.FreadLine(hv_FileHandle, out hv_String, out hv_IsEOF);
            HOperatorSet.FreadLine(hv_FileHandle, out hv_String, out hv_IsEOF);
            HOperatorSet.FreadLine(hv_FileHandle, out hv_String, out hv_IsEOF);
            HOperatorSet.FreadLine(hv_FileHandle, out hv_String, out hv_IsEOF);
            string[] MultList = hv_String.S.Split(',');
            for (int i = 1; i < MultList.Length; i++)
            {
                Common.Data.Mult1[i - 1] = double.Parse(MultList[i]);
            }

            HOperatorSet.FreadLine(hv_FileHandle, out hv_String, out hv_IsEOF);
            string[] AddList = hv_String.S.Split(',');
            for (int i = 1; i < AddList.Length; i++)
            {
                Common.Data.Add1[i - 1] = double.Parse(AddList[i]);
            }


            HOperatorSet.FreadLine(hv_FileHandle, out hv_String, out hv_IsEOF);
            string[] MultList2 = hv_String.S.Split(',');
            for (int i = 1; i < MultList2.Length; i++)
            {
                Common.Data.Mult2[i - 1] = double.Parse(MultList2[i]);
            }


            HOperatorSet.FreadLine(hv_FileHandle, out hv_String, out hv_IsEOF);
            string[] AddList2 = hv_String.S.Split(',');
            for (int i = 1; i < AddList2.Length; i++)
            {
                Common.Data.Add2[i - 1] = double.Parse(AddList2[i]);
            }
            HOperatorSet.CloseFile(hv_FileHandle);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Common.str_ProductName = IniHelper.Read("SystemCfg", "ProductName", "Star", $"{Application.StartupPath}\\SysCfg\\System.ini");
            Common.ProductSum = int.Parse(IniHelper.Read("RuningData", "Sum", "0", $"{Application.StartupPath}\\SysCfg\\System.ini"));
            Common.ProductNg = int.Parse(IniHelper.Read("RuningData", "Ng", "0", $"{Application.StartupPath}\\SysCfg\\System.ini"));
            Common.bSaveRawImage = bool.Parse(IniHelper.Read("ImageSave", "Save", "False", $"{Application.StartupPath}\\SysCfg\\System.ini"));
            Common.bSafeLightUse = bool.Parse(IniHelper.Read("SafeLight", "Use", "False", $"{Application.StartupPath}\\SysCfg\\System.ini"));
            Common.bOfflineMesUse= bool.Parse(IniHelper.Read("OfflineMES", "Use", "False", $"{Application.StartupPath}\\SysCfg\\System.ini"));

            LogManager.GetLogger(typeof(MainForm)).Debug("=======MainForm_Load========");
            if (Common.str_ProductName == "Star")
            {
                Common.FaiNames = Common.StarFais;
            }
            else if (Common.str_ProductName == "Starfire")
            {
                Common.FaiNames = Common.StarFireFais;
            }
            else if (Common.str_ProductName == "Flash")
            {
                Common.FaiNames = Common.FlashFais;
            }
            LogManager.GetLogger(typeof(MainForm)).Debug("=======HOperatorSet.SetSystem========");
            HOperatorSet.SetSystem("clip_region", "false");
            BackWorker.WorkerSupportsCancellation = true;
            BackWorker.WorkerReportsProgress = true;

            InitMainformLayout();
            Common.frmMain = this;

            LogManager.GetLogger(typeof(MainForm)).Debug("=======LoadConfig========");
            Common.modelMgr.LoadConfig();
            LogManager.GetLogger(typeof(MainForm)).Debug("=======ShowProductType========");
            Common.ShowProductType(Common.str_ProductName);
            LogManager.GetLogger(typeof(MainForm)).Debug("=======Common.modelMgr.SetLastModel========");
            Common.modelMgr.SetLastModel(Common.str_ProductName);
            Thread.Sleep(500);
            LogManager.GetLogger(typeof(MainForm)).Debug("=======InitCamera========");
            InitCamera();
            LoadParam();
            InitPort();
            Common.ProC.LoadParam();
            Common.ProC = ProControl.GetInstance();
            Common.ProC.InitParam();
            LogManager.GetLogger(typeof(MainForm)).Debug("=======LoadMotionData========");
            Common.frmMotionSetting.LoadMotionData();
            LogManager.GetLogger(typeof(MainForm)).Debug("=======LoadInspectCode========");
            Common.frmInspect.LoadInspectCode();
            //加载校准值
            LoadMultAndAdd();
            metroShell1.SelectedTab = metroTabItem1;
            Common.frmShow.ShowInitImage();
            Common.frmDataSum.UpdateSum(Common.ProductSum, Common.ProductNg);
            Common.modelMgr.BarcodeChanged += ModelMgr_BarcodeChanged;
        }

        private void ModelMgr_BarcodeChanged(object sender, string model)
        {
            Common.ShowProductType(model);
        }

        private void Btn_SaveLayout_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Application.StartupPath, "SysCfg\\CustomUI.xml")))
            {
                File.Delete(Path.Combine(Application.StartupPath, "SysCfg\\CustomUI.xml"));
            }
            MainPanel.SaveAsXml(Path.Combine(Application.StartupPath, "SysCfg\\CustomUI.xml"));
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            Common.frmSetting.Show();
            Common.frmSetting.Timer.Enabled = true;
            Common.frmSetting.BringToFront();
        }


        delegate void DelegateHome();

        private void Btn_GoHome_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("请确认设备处于安全状态，且回原路径无干扰碰撞可能再回原",
                 "提示信息 ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                Common.ProC.ExCard.WriteOutput((ushort)ProControl.ExOutput.工位1允许放料.GetHashCode(), 1);
                Common.ProC.ExCard.WriteOutput((ushort)ProControl.ExOutput.工位2允许放料.GetHashCode(), 1);
                Btn_GoHome.Enabled = false;
                if (Common.ProC != null)
                {
                    Common.ProC.OpenCylinder();
                    Common.ProC.AllAxisGohome();
                    Btn_GoHome.Enabled = false;
                    BTN_AutoRun.Enabled = true;
                    BTN_Pause.Enabled = false;
                }
            }
        }

        private void BTN_QuitSystem_Click(object sender, EventArgs e)
        {
            IniHelper.Write("SystemCfg", "ProductName", Common.str_ProductName, $"{Application.StartupPath}\\SysCfg\\System.ini");
            IniHelper.Write("RuningData", "Sum", Common.ProductSum.ToString(), $"{Application.StartupPath}\\SysCfg\\System.ini");
            IniHelper.Write("RuningData", "Ng", Common.ProductNg.ToString(), $"{Application.StartupPath}\\SysCfg\\System.ini");
            Common.ProC.Card1.WriteOutput(ProControl.Card1Output.Z轴刹车.GetHashCode(), 1);
            Common.ProC.SaveParam();
            if (Common.Cam1 != null ? Common.Cam1.Status == CamStatus.Open : false)
            {
                if (Common.Cam1.IsStart)
                {
                    Common.Cam1.Stop();
                }
                Common.Cam1.Close();
            }
            Common.frmMotionSetting.Dispose();
            Common.frmSetting.Dispose();
            Common.frmInspect.Dispose();
            Common.frmValueLimit.Dispose();
            Application.Exit();
        }

        private void BTN_AutoRun_Click(object sender, EventArgs e)
        {
            //更改回调。。。
            Common.Cam1.grabFinishCall = Common.grabImage1;
            Btn_GoHome.Enabled = false;
            BTN_AutoRun.Enabled = false;
            BTN_Pause.Enabled = true;

            BackWorker.RunWorkerAsync();
        }

        private void BTN_Pause_Click(object sender, EventArgs e)
        {
            Common.ProC.ExCard.WriteOutput((ushort)ProControl.ExOutput.工位1允许放料.GetHashCode(), 1);
            Common.ProC.ExCard.WriteOutput((ushort)ProControl.ExOutput.工位2允许放料.GetHashCode(), 1);

            BTN_AutoRun.Enabled = true;
            BTN_Pause.Enabled = false;
            Btn_GoHome.Enabled = true;
            Common.ProC.OpenYellowLight();
            Common.QueCam1.Clear();
            Common.ImageIdx = 0;
            BackWorker.CancelAsync();
        }

        public bool b_Start = false, b_Reset = false, b_HardStop = false, b_SafeSingal = false, b_Check = false, b_SafeDoor = false;

        private void BackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (BackWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(20);
                Common.ProC.GetStatus();
                Common.ProC.Control_T();
                #region 信号处理
                if(Common.ProC.ExCard.InputList[ProControl.ExInput.治具1对射感应.GetHashCode()] || 
                    Common.ProC.ExCard.InputList[ProControl.ExInput.治具2对射感应.GetHashCode()])
                {
                    LogManager.GetLogger(typeof(MainForm)).Debug($"=======治具对射感应,机台报警========");
                    Common.ProC.ExCard.WriteOutput((ushort)ProControl.ExOutput.机台报警.GetHashCode(), 0);
                    BTN_Pause_Click(this,null);
                }
                else if (Common.ProC.ExCard.InputList[ProControl.ExInput.启动.GetHashCode()] && !b_Start) //收到启动上升沿信号 
                {
                    Common.ProC.OpenGreenLight();
                    Common.ProC.nStation1 = 0;
                    Common.ProC.nStation2 = 0;
                    Common.ProC.nCheckStep1 = -1; Common.ProC.nCheckStep2 = -1;  //指示目前检测步骤
                    Common.ProC.nCheckIdx = 0;    //指示当前是第几个检测项
                    Common.ImageIdx = 0;
                    Common.nResultList = new List<int>();
                    b_Check = true;
                    Common.ProC.bHome = false;
                    Common.ProC.ExCard.WriteOutput((ushort)ProControl.ExOutput.机台报警.GetHashCode(), 0);
                }
                else if (Common.ProC.ExCard.InputList[ProControl.ExInput.复位.GetHashCode()] && !b_Reset)
                {
                    b_Check = false;
                    Common.ProC.AllAxisGohome();
                }
                else if (!Common.ProC.ExCard.InputList[ProControl.ExInput.停止.GetHashCode()] && b_HardStop)
                {
                    b_Check = false;
                    Common.ProC.Card1.StopRun(0, 0);
                    Common.ProC.Card1.StopRun(1, 0);
                    Common.ProC.Card1.StopRun(2, 0);
                    Common.ProC.Card1.StopRun(3, 0);
                    Common.ProC.OpenRedLight();
                    Common.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff -->") + string.Format("程序手动停止，请复位后再启动--"));
                }
                else if (b_Check && !b_SafeSingal && (!Common.ProC.Card1.Singal[ProControl.Card1Input.治具1安全光栅.GetHashCode()] || !Common.ProC.Card1.Singal[ProControl.Card1Input.治具2安全光栅.GetHashCode()]) && Common.bSafeLightUse)
                {
                    b_Check = false;
                    Common.ProC.Card1.StopRun(0, 0);
                    Common.ProC.Card1.StopRun(1, 0);
                    Common.ProC.Card1.StopRun(2, 0);
                    Common.ProC.Card1.StopRun(3, 0);
                    Common.ProC.OpenRedLight();
                    Common.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff -->") + string.Format("安全光栅触发报警，设备已停止，请复位后再启动--"));
                }
                else if (b_Check && b_SafeDoor && ((Common.ProC.ExCard.InputList[ProControl.ExInput.正面左门.GetHashCode()] || Common.ProC.ExCard.InputList[ProControl.ExInput.正面右门.GetHashCode()] ||
                    Common.ProC.ExCard.InputList[ProControl.ExInput.背面左门.GetHashCode()] || Common.ProC.ExCard.InputList[ProControl.ExInput.背面右门.GetHashCode()])))
                {
                    b_Check = false;
                    Common.ProC.Card1.StopRun(0, 0);
                    Common.ProC.Card1.StopRun(1, 0);
                    Common.ProC.Card1.StopRun(2, 0);
                    Common.ProC.Card1.StopRun(3, 0);
                    Common.ProC.OpenRedLight();
                    Common.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff -->") + string.Format("机舱门被打开，设备已停止，请复位后再启动--"));
                }

                if (Common.ProC.bHome)
                {
                    Common.ProC.OpenYellowLight();
                }

                #endregion

                if (b_Check)
                {
                    if (Common.RunOnline)
                    {
                        Common.ProC.AutoCloseCylinder1();
                        Common.ProC.AutoCloseCylinder2();
                        Common.ProC.Station1WorksOnline();
                        Common.ProC.Station2WorksOnline();
                    }
                    else
                    {
                        Common.ProC.Station1Works();
                        Common.ProC.Station2Works();
                    }
                    Common.ProC.AutoCheck1();
                    Common.ProC.AutoCheck2();
                    Common.ProC.AutoMeasureHeight();
                    Common.frmShow.InspectImage();
                }
                b_Start = Common.ProC.ExCard.InputList[ProControl.ExInput.启动.GetHashCode()];
                b_Reset = Common.ProC.ExCard.InputList[ProControl.ExInput.复位.GetHashCode()];
                b_HardStop = Common.ProC.ExCard.InputList[ProControl.ExInput.停止.GetHashCode()];
                b_SafeSingal = Common.ProC.Card1.Singal[ProControl.Card1Input.治具1安全光栅.GetHashCode()] && Common.ProC.Card1.Singal[ProControl.Card1Input.治具2安全光栅.GetHashCode()];
                b_SafeDoor = !(Common.ProC.ExCard.InputList[ProControl.ExInput.正面左门.GetHashCode()] || Common.ProC.ExCard.InputList[ProControl.ExInput.正面右门.GetHashCode()] ||
                    Common.ProC.ExCard.InputList[ProControl.ExInput.背面左门.GetHashCode()] || Common.ProC.ExCard.InputList[ProControl.ExInput.背面右门.GetHashCode()]);
            }
        }


        private async void buttonX2_Click(object sender, EventArgs e)
        {
            Common.ProC.CancelCardSoftLmt();
            List<Task<bool>> taskList = new List<Task<bool>>();
            //卡1
            for (int i = 0; i < Common.ProC.Card1.AxisName.Length; i++)
            {
                int axis = i;
                taskList.Add(Task<bool>.Run(() =>
                {
                    return Common.ProC.Card1.GoHome(axis, 0, 1, 2, 0); //负方向回
                }));
                Thread.Sleep(20);
            }
            var b = await Task.WhenAll(taskList);
            Common.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff -->") + string.Format("所有轴回原完成--"));
            Common.ProC.SetCardSoftLmt();
        }

        private void Btn_PositionSetting_Click(object sender, EventArgs e)
        {
            Common.frmInspect.Show();
            Common.frmInspect.BringToFront();
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            SaveMesAndRuningData();
        }

        private static void SaveMesAndRuningData()
        {
            try
            {
                string result = JsonConvert.SerializeObject(Common.Data);
                if (!Directory.Exists($"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\"))
                {
                    Directory.CreateDirectory($"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\");
                }
                string fp = $"{System.Windows.Forms.Application.StartupPath}\\Config\\{Common.str_ProductName}\\RunningData.json";
                if (!File.Exists(fp))  // 判断是否已有相同文件 
                {
                    FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                    fs1.Close();
                }
                File.WriteAllText(fp, result.Replace(",\"", ",\r\n\""));
                MessageBox.Show("运行参数保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败！原因为:{ex.Message}");
            }

            try
            {
                string result = JsonConvert.SerializeObject(Common.mes);
                if (!Directory.Exists($"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\"))
                {
                    Directory.CreateDirectory($"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\");
                }
                string fp = $"{System.Windows.Forms.Application.StartupPath}\\SysCfg\\Mes.json";
                if (!File.Exists(fp))  // 判断是否已有相同文件 
                {
                    FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                    fs1.Close();
                }
                File.WriteAllText(fp, result.Replace(",\"", ",\r\n\""));
                MessageBox.Show("Mes参数保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败！原因为:{ex.Message}");
            }
        }

        private void buttonX2_Click_1(object sender, EventArgs e)
        {
            Common.frmShow.LocalImageTest();
        }

        private void btnSN2Model_Click(object sender, EventArgs e)
        {
            string sn = this.tbSN.Text;
            Common.modelMgr.OnReceiveBarcode(sn);
            //Console.WriteLine(Common.modelMgr.SN2Model(sn));
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            Common.frmValueLimit.ShowDialog();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (buttonX6.Symbol == "\uf021")
            {
                Common.RunMode = "Once";
                buttonX6.Symbol = "\uf01e";
                buttonX6.SymbolColor = Color.Green;
                label11.Text = "单次检测";
            }
            else
            {
                Common.RunMode = "Keep";
                buttonX6.Symbol = "\uf021";
                buttonX6.SymbolColor = Color.DodgerBlue;
                label11.Text = "循环测试";
            }
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            if (buttonX7.Symbol == "\uf1eb")
            {
                Common.RunOnline = false;
                buttonX7.Symbol = "\uf127";
                buttonX7.SymbolColor = Color.Gray;
                label12.Text = "离线模式";
            }
            else
            {
                Common.RunOnline = true;
                buttonX7.Symbol = "\uf1eb";
                buttonX7.SymbolColor = Color.DodgerBlue;
                label12.Text = "在线模式";
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

        }

        private void Btn_MotionSetting_Click(object sender, EventArgs e)
        {
            Common.frmMotionSetting.cardControl1.Timer.Enabled = true;
            Common.frmMotionSetting.exIOControl1.Timer.Enabled = true;
            Common.frmMotionSetting.Show();
            Common.frmMotionSetting.BringToFront();
        }
    }
}