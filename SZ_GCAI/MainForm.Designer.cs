namespace SZ_BydKeyboard
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.metroShell1 = new DevComponents.DotNetBar.Metro.MetroShell();
            this.metroTabPanel1 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.label4 = new System.Windows.Forms.Label();
            this.Btn_MotionSetting = new DevComponents.DotNetBar.ButtonX();
            this.label3 = new System.Windows.Forms.Label();
            this.Btn_PositionSetting = new DevComponents.DotNetBar.ButtonX();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_SaveLayout = new DevComponents.DotNetBar.ButtonX();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_GoHome = new DevComponents.DotNetBar.ButtonX();
            this.BTN_QuitSystem = new DevComponents.DotNetBar.ButtonX();
            this.label14 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.BTN_AutoRun = new DevComponents.DotNetBar.ButtonX();
            this.BTN_Pause = new DevComponents.DotNetBar.ButtonX();
            this.label13 = new System.Windows.Forms.Label();
            this.metroTabPanel2 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.metroAppButton1 = new DevComponents.DotNetBar.Metro.MetroAppButton();
            this.metroTabItem1 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem2 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.qatCustomizeItem1 = new DevComponents.DotNetBar.QatCustomizeItem();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem4 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer3 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem5 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer4 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer5 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem6 = new DevComponents.DotNetBar.LabelItem();
            this.BackWorker = new System.ComponentModel.BackgroundWorker();
            this.itemContainer2 = new DevComponents.DotNetBar.ItemContainer();
            this.MainPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.metroShell1.SuspendLayout();
            this.metroTabPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroShell1
            // 
            this.metroShell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.metroShell1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell1.CaptionVisible = true;
            this.metroShell1.Controls.Add(this.metroTabPanel1);
            this.metroShell1.Controls.Add(this.metroTabPanel2);
            this.metroShell1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroShell1.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroShell1.ForeColor = System.Drawing.Color.Black;
            this.metroShell1.HelpButtonText = null;
            this.metroShell1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.metroAppButton1,
            this.metroTabItem1,
            this.metroTabItem2});
            this.metroShell1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShell1.Location = new System.Drawing.Point(1, 1);
            this.metroShell1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.metroShell1.Name = "metroShell1";
            this.metroShell1.QuickToolbarItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.qatCustomizeItem1});
            this.metroShell1.Size = new System.Drawing.Size(1582, 121);
            this.metroShell1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell1.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell1.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell1.SystemText.QatDialogOkButton = "OK";
            this.metroShell1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell1.TabIndex = 0;
            this.metroShell1.TabStripFont = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // metroTabPanel1
            // 
            this.metroTabPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel1.Controls.Add(this.label8);
            this.metroTabPanel1.Controls.Add(this.buttonX3);
            this.metroTabPanel1.Controls.Add(this.label4);
            this.metroTabPanel1.Controls.Add(this.Btn_MotionSetting);
            this.metroTabPanel1.Controls.Add(this.label3);
            this.metroTabPanel1.Controls.Add(this.Btn_PositionSetting);
            this.metroTabPanel1.Controls.Add(this.label1);
            this.metroTabPanel1.Controls.Add(this.Btn_SaveLayout);
            this.metroTabPanel1.Controls.Add(this.label5);
            this.metroTabPanel1.Controls.Add(this.buttonX1);
            this.metroTabPanel1.Controls.Add(this.label2);
            this.metroTabPanel1.Controls.Add(this.Btn_GoHome);
            this.metroTabPanel1.Controls.Add(this.BTN_QuitSystem);
            this.metroTabPanel1.Controls.Add(this.label14);
            this.metroTabPanel1.Controls.Add(this.label7);
            this.metroTabPanel1.Controls.Add(this.BTN_AutoRun);
            this.metroTabPanel1.Controls.Add(this.BTN_Pause);
            this.metroTabPanel1.Controls.Add(this.label13);
            this.metroTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel1.Location = new System.Drawing.Point(0, 52);
            this.metroTabPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.metroTabPanel1.Name = "metroTabPanel1";
            this.metroTabPanel1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.metroTabPanel1.Size = new System.Drawing.Size(1582, 69);
            // 
            // 
            // 
            this.metroTabPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel1.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label8.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(61, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 41;
            this.label8.Text = "控制设置";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.buttonX3.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonX3.Location = new System.Drawing.Point(63, 7);
            this.buttonX3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.buttonX3.Size = new System.Drawing.Size(48, 40);
            this.buttonX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX3.SubItemsExpandWidth = 0;
            this.buttonX3.Symbol = "";
            this.buttonX3.SymbolColor = System.Drawing.Color.DodgerBlue;
            this.buttonX3.SymbolSize = 25F;
            this.buttonX3.TabIndex = 40;
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label4.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(117, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "运动控制";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_MotionSetting
            // 
            this.Btn_MotionSetting.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Btn_MotionSetting.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.Btn_MotionSetting.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_MotionSetting.Location = new System.Drawing.Point(119, 7);
            this.Btn_MotionSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Btn_MotionSetting.Name = "Btn_MotionSetting";
            this.Btn_MotionSetting.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.Btn_MotionSetting.Size = new System.Drawing.Size(48, 40);
            this.Btn_MotionSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Btn_MotionSetting.SubItemsExpandWidth = 0;
            this.Btn_MotionSetting.Symbol = "";
            this.Btn_MotionSetting.SymbolColor = System.Drawing.Color.DodgerBlue;
            this.Btn_MotionSetting.SymbolSize = 25F;
            this.Btn_MotionSetting.TabIndex = 38;
            this.Btn_MotionSetting.Click += new System.EventHandler(this.Btn_MotionSetting_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label3.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(171, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "定位设置";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_PositionSetting
            // 
            this.Btn_PositionSetting.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Btn_PositionSetting.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.Btn_PositionSetting.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_PositionSetting.Location = new System.Drawing.Point(173, 7);
            this.Btn_PositionSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Btn_PositionSetting.Name = "Btn_PositionSetting";
            this.Btn_PositionSetting.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.Btn_PositionSetting.Size = new System.Drawing.Size(48, 40);
            this.Btn_PositionSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Btn_PositionSetting.SubItemsExpandWidth = 0;
            this.Btn_PositionSetting.Symbol = "";
            this.Btn_PositionSetting.SymbolColor = System.Drawing.Color.DodgerBlue;
            this.Btn_PositionSetting.SymbolSize = 25F;
            this.Btn_PositionSetting.TabIndex = 36;
            this.Btn_PositionSetting.Click += new System.EventHandler(this.Btn_PositionSetting_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(225, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 35;
            this.label1.Text = "保存布局";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_SaveLayout
            // 
            this.Btn_SaveLayout.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Btn_SaveLayout.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.Btn_SaveLayout.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_SaveLayout.Location = new System.Drawing.Point(227, 7);
            this.Btn_SaveLayout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Btn_SaveLayout.Name = "Btn_SaveLayout";
            this.Btn_SaveLayout.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.Btn_SaveLayout.Size = new System.Drawing.Size(48, 40);
            this.Btn_SaveLayout.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Btn_SaveLayout.SubItemsExpandWidth = 0;
            this.Btn_SaveLayout.Symbol = "";
            this.Btn_SaveLayout.SymbolColor = System.Drawing.Color.DodgerBlue;
            this.Btn_SaveLayout.SymbolSize = 25F;
            this.Btn_SaveLayout.TabIndex = 34;
            this.Btn_SaveLayout.Click += new System.EventHandler(this.Btn_SaveLayout_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label5.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(19, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 31;
            this.label5.Text = "登录";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.buttonX1.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonX1.Location = new System.Drawing.Point(9, 7);
            this.buttonX1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.buttonX1.Size = new System.Drawing.Size(48, 40);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.SubItemsExpandWidth = 0;
            this.buttonX1.Symbol = "";
            this.buttonX1.SymbolColor = System.Drawing.SystemColors.MenuHighlight;
            this.buttonX1.SymbolSize = 25F;
            this.buttonX1.TabIndex = 30;
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label2.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(1368, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "回原";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_GoHome
            // 
            this.Btn_GoHome.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Btn_GoHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_GoHome.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.Btn_GoHome.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_GoHome.Location = new System.Drawing.Point(1358, 7);
            this.Btn_GoHome.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Btn_GoHome.Name = "Btn_GoHome";
            this.Btn_GoHome.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.Btn_GoHome.Size = new System.Drawing.Size(48, 40);
            this.Btn_GoHome.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Btn_GoHome.SubItemsExpandWidth = 0;
            this.Btn_GoHome.Symbol = "";
            this.Btn_GoHome.SymbolColor = System.Drawing.SystemColors.MenuHighlight;
            this.Btn_GoHome.SymbolSize = 25F;
            this.Btn_GoHome.TabIndex = 28;
            this.Btn_GoHome.Click += new System.EventHandler(this.Btn_GoHome_Click);
            // 
            // BTN_QuitSystem
            // 
            this.BTN_QuitSystem.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BTN_QuitSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_QuitSystem.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.BTN_QuitSystem.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_QuitSystem.Location = new System.Drawing.Point(1517, 7);
            this.BTN_QuitSystem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTN_QuitSystem.Name = "BTN_QuitSystem";
            this.BTN_QuitSystem.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.BTN_QuitSystem.Size = new System.Drawing.Size(48, 40);
            this.BTN_QuitSystem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BTN_QuitSystem.SubItemsExpandWidth = 0;
            this.BTN_QuitSystem.Symbol = "";
            this.BTN_QuitSystem.SymbolColor = System.Drawing.Color.Red;
            this.BTN_QuitSystem.SymbolSize = 25F;
            this.BTN_QuitSystem.TabIndex = 20;
            this.BTN_QuitSystem.Click += new System.EventHandler(this.BTN_QuitSystem_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label14.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(1421, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 25;
            this.label14.Text = "启动";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label7.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(1527, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "退出";
            // 
            // BTN_AutoRun
            // 
            this.BTN_AutoRun.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BTN_AutoRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_AutoRun.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.BTN_AutoRun.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_AutoRun.Location = new System.Drawing.Point(1411, 7);
            this.BTN_AutoRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTN_AutoRun.Name = "BTN_AutoRun";
            this.BTN_AutoRun.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.BTN_AutoRun.Size = new System.Drawing.Size(48, 40);
            this.BTN_AutoRun.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BTN_AutoRun.SubItemsExpandWidth = 0;
            this.BTN_AutoRun.Symbol = "";
            this.BTN_AutoRun.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BTN_AutoRun.SymbolSize = 25F;
            this.BTN_AutoRun.TabIndex = 24;
            this.BTN_AutoRun.Click += new System.EventHandler(this.BTN_AutoRun_Click);
            // 
            // BTN_Pause
            // 
            this.BTN_Pause.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BTN_Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_Pause.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.BTN_Pause.Enabled = false;
            this.BTN_Pause.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Pause.Location = new System.Drawing.Point(1464, 7);
            this.BTN_Pause.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTN_Pause.Name = "BTN_Pause";
            this.BTN_Pause.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15, 2, 2, 15);
            this.BTN_Pause.Size = new System.Drawing.Size(48, 40);
            this.BTN_Pause.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BTN_Pause.SubItemsExpandWidth = 0;
            this.BTN_Pause.Symbol = "";
            this.BTN_Pause.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BTN_Pause.SymbolSize = 25F;
            this.BTN_Pause.TabIndex = 22;
            this.BTN_Pause.Click += new System.EventHandler(this.BTN_Pause_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label13.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(1474, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 23;
            this.label13.Text = "暂停";
            // 
            // metroTabPanel2
            // 
            this.metroTabPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel2.Location = new System.Drawing.Point(0, 52);
            this.metroTabPanel2.Name = "metroTabPanel2";
            this.metroTabPanel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.metroTabPanel2.Size = new System.Drawing.Size(1582, 69);
            // 
            // 
            // 
            this.metroTabPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel2.TabIndex = 2;
            this.metroTabPanel2.Visible = false;
            // 
            // metroAppButton1
            // 
            this.metroAppButton1.AutoExpandOnClick = true;
            this.metroAppButton1.CanCustomize = false;
            this.metroAppButton1.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.metroAppButton1.ImagePaddingHorizontal = 0;
            this.metroAppButton1.ImagePaddingVertical = 0;
            this.metroAppButton1.Name = "metroAppButton1";
            this.metroAppButton1.ShowSubItems = false;
            this.metroAppButton1.Text = "&File";
            // 
            // metroTabItem1
            // 
            this.metroTabItem1.Checked = true;
            this.metroTabItem1.Name = "metroTabItem1";
            this.metroTabItem1.Panel = this.metroTabPanel1;
            this.metroTabItem1.Text = "&主页";
            // 
            // metroTabItem2
            // 
            this.metroTabItem2.Name = "metroTabItem2";
            this.metroTabItem2.Panel = this.metroTabPanel2;
            this.metroTabItem2.Text = "metroTabItem2";
            this.metroTabItem2.Visible = false;
            // 
            // qatCustomizeItem1
            // 
            this.qatCustomizeItem1.BeginGroup = true;
            this.qatCustomizeItem1.Name = "qatCustomizeItem1";
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199))))));
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.DragDropSupport = true;
            this.metroStatusBar1.Font = new System.Drawing.Font("楷体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroStatusBar1.ForeColor = System.Drawing.Color.Black;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem3,
            this.itemContainer1,
            this.labelItem4,
            this.itemContainer3,
            this.labelItem5,
            this.itemContainer4,
            this.labelItem1,
            this.itemContainer5,
            this.labelItem6});
            this.metroStatusBar1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroStatusBar1.Location = new System.Drawing.Point(1, 809);
            this.metroStatusBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.Size = new System.Drawing.Size(1582, 21);
            this.metroStatusBar1.TabIndex = 1;
            this.metroStatusBar1.Text = "metroStatusBar1";
            // 
            // labelItem3
            // 
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.Symbol = "";
            this.labelItem3.SymbolSize = 10F;
            this.labelItem3.Text = "相机连接成功";
            // 
            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.Name = "itemContainer1";
            // 
            // 
            // 
            this.itemContainer1.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer1.TitleStyle.BorderRightWidth = 200;
            this.itemContainer1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // labelItem4
            // 
            this.labelItem4.Name = "labelItem4";
            this.labelItem4.Symbol = "";
            this.labelItem4.SymbolSize = 10F;
            this.labelItem4.Text = "电机使能中";
            // 
            // itemContainer3
            // 
            // 
            // 
            // 
            this.itemContainer3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer3.Name = "itemContainer3";
            // 
            // 
            // 
            this.itemContainer3.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer3.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // labelItem5
            // 
            this.labelItem5.Name = "labelItem5";
            this.labelItem5.Symbol = "";
            this.labelItem5.SymbolSize = 10F;
            this.labelItem5.Text = "MES连接成功";
            // 
            // itemContainer4
            // 
            // 
            // 
            // 
            this.itemContainer4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer4.Name = "itemContainer4";
            // 
            // 
            // 
            this.itemContainer4.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer4.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Symbol = "";
            this.labelItem1.SymbolSize = 10F;
            this.labelItem1.Text = "光源控制器连接成功";
            // 
            // itemContainer5
            // 
            // 
            // 
            // 
            this.itemContainer5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer5.Name = "itemContainer5";
            // 
            // 
            // 
            this.itemContainer5.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer5.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // labelItem6
            // 
            this.labelItem6.ForeColor = System.Drawing.Color.Orange;
            this.labelItem6.Name = "labelItem6";
            this.labelItem6.Symbol = "";
            this.labelItem6.SymbolSize = 10F;
            this.labelItem6.Text = "IC卡登录成功";
            // 
            // BackWorker
            // 
            this.BackWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackWorker_DoWork);
            // 
            // itemContainer2
            // 
            // 
            // 
            // 
            this.itemContainer2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer2.Name = "itemContainer2";
            // 
            // 
            // 
            this.itemContainer2.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer2.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.ForeColor = System.Drawing.Color.Black;
            this.MainPanel.Location = new System.Drawing.Point(1, 122);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1582, 687);
            this.MainPanel.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 831);
            this.CloseBoxVisible = false;
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.metroShell1);
            this.Controls.Add(this.metroStatusBar1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "深圳晶凯芯片定位组装系统 V1.4.0.2 （2021.08.07 Update.）";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.metroShell1.ResumeLayout(false);
            this.metroShell1.PerformLayout();
            this.metroTabPanel1.ResumeLayout(false);
            this.metroTabPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Metro.MetroShell metroShell1;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel1;
        private DevComponents.DotNetBar.Metro.MetroAppButton metroAppButton1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem1;
        private DevComponents.DotNetBar.QatCustomizeItem qatCustomizeItem1;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.Metro.MetroStatusBar metroStatusBar1;
        private DevComponents.DotNetBar.ButtonX BTN_QuitSystem;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label7;
        private DevComponents.DotNetBar.ButtonX BTN_AutoRun;
        private DevComponents.DotNetBar.ButtonX BTN_Pause;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.ButtonX Btn_GoHome;
        private System.ComponentModel.BackgroundWorker BackWorker;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.Label label8;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.ButtonX Btn_MotionSetting;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.ButtonX Btn_PositionSetting;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.ButtonX Btn_SaveLayout;
        private DevComponents.DotNetBar.LabelItem labelItem3;
        private DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.LabelItem labelItem4;
        private DevComponents.DotNetBar.ItemContainer itemContainer3;
        private DevComponents.DotNetBar.LabelItem labelItem5;
        private DevComponents.DotNetBar.ItemContainer itemContainer2;
        private WeifenLuo.WinFormsUI.Docking.DockPanel MainPanel;
        private DevComponents.DotNetBar.ItemContainer itemContainer4;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.ItemContainer itemContainer5;
        private DevComponents.DotNetBar.LabelItem labelItem6;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel2;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem2;
    }
}

