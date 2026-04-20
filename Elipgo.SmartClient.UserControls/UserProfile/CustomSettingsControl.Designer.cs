using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    partial class CustomSettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomSettingsControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txbDownloadFolder = new System.Windows.Forms.TextBox();
            this.lblDownloadFolder = new System.Windows.Forms.Label();
            this.grbSidebar = new System.Windows.Forms.GroupBox();
            this.lblSidebarElements = new System.Windows.Forms.Label();
            this.switchDevStatus = new Bunifu.Framework.UI.BunifuiOSSwitch();
            this.lblStatusDev = new System.Windows.Forms.Label();
            this.ddlSidebarElements = new Bunifu.UI.WinForms.BunifuDropdown();
            this.grbQuickViewAlarms = new System.Windows.Forms.GroupBox();
            this.lblQViewAlarms = new System.Windows.Forms.Label();
            this.lblAutoQViewAlarms = new System.Windows.Forms.Label();
            this.switchQViewAlarms = new Bunifu.Framework.UI.BunifuiOSSwitch();
            this.ddlQViewAlarms = new Bunifu.UI.WinForms.BunifuDropdown();
            this.lblLenguage = new System.Windows.Forms.Label();
            this.picBoxLanguaje = new System.Windows.Forms.PictureBox();
            this.bunifuButtonGuardar = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.bunifuButtonCancelar = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.bunifuImageButton2 = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.ddlLanguage = new Elipgo.SmartClient.UserControls.Shared.Dropdown();
            this.panel1.SuspendLayout();
            this.grbSidebar.SuspendLayout();
            this.grbQuickViewAlarms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLanguaje)).BeginInit();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.txbDownloadFolder);
            this.panel1.Controls.Add(this.lblDownloadFolder);
            this.panel1.Controls.Add(this.grbSidebar);
            this.panel1.Controls.Add(this.grbQuickViewAlarms);
            this.panel1.Controls.Add(this.lblLenguage);
            this.panel1.Controls.Add(this.picBoxLanguaje);
            this.panel1.Controls.Add(this.bunifuButtonGuardar);
            this.panel1.Controls.Add(this.bunifuButtonCancelar);
            this.panel1.Controls.Add(this.ddlLanguage);
            this.panel1.Controls.Add(this.PanelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(378, 493);
            this.panel1.TabIndex = 0;
            // 
            // txbDownloadFolder
            // 
            this.txbDownloadFolder.BackColor = System.Drawing.SystemColors.Info;
            this.txbDownloadFolder.ForeColor = System.Drawing.Color.Black;
            this.txbDownloadFolder.Location = new System.Drawing.Point(21, 113);
            this.txbDownloadFolder.Name = "txbDownloadFolder";
            this.txbDownloadFolder.Size = new System.Drawing.Size(326, 20);
            this.txbDownloadFolder.TabIndex = 50;
            // 
            // lblDownloadFolder
            // 
            this.lblDownloadFolder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloadFolder.ForeColor = System.Drawing.Color.Silver;
            this.lblDownloadFolder.Location = new System.Drawing.Point(18, 85);
            this.lblDownloadFolder.Name = "lblDownloadFolder";
            this.lblDownloadFolder.Size = new System.Drawing.Size(181, 22);
            this.lblDownloadFolder.TabIndex = 49;
            this.lblDownloadFolder.Text = Resources.FolderBrowser;
            // 
            // grbSidebar
            // 
            this.grbSidebar.Controls.Add(this.lblSidebarElements);
            this.grbSidebar.Controls.Add(this.switchDevStatus);
            this.grbSidebar.Controls.Add(this.lblStatusDev);
            this.grbSidebar.Controls.Add(this.ddlSidebarElements);
            this.grbSidebar.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.grbSidebar.ForeColor = System.Drawing.Color.Silver;
            this.grbSidebar.Location = new System.Drawing.Point(21, 148);
            this.grbSidebar.Name = "grbSidebar";
            this.grbSidebar.Size = new System.Drawing.Size(326, 130);
            this.grbSidebar.TabIndex = 0;
            this.grbSidebar.TabStop = false;
            this.grbSidebar.Text = "Sidebar";
            // 
            // lblSidebarElements
            // 
            this.lblSidebarElements.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSidebarElements.ForeColor = System.Drawing.Color.Silver;
            this.lblSidebarElements.Location = new System.Drawing.Point(6, 32);
            this.lblSidebarElements.Name = "lblSidebarElements";
            this.lblSidebarElements.Size = new System.Drawing.Size(123, 20);
            this.lblSidebarElements.TabIndex = 42;
            this.lblSidebarElements.Text = "No. " + Resources.Elements;
            // 
            // switchDevStatus
            // 
            this.switchDevStatus.BackColor = System.Drawing.Color.Transparent;
            this.switchDevStatus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("switchDevStatus.BackgroundImage")));
            this.switchDevStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchDevStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchDevStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.switchDevStatus.Location = new System.Drawing.Point(143, 80);
            this.switchDevStatus.Margin = new System.Windows.Forms.Padding(0);
            this.switchDevStatus.Name = "switchDevStatus";
            this.switchDevStatus.OffColor = System.Drawing.Color.Gray;
            this.switchDevStatus.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.switchDevStatus.Size = new System.Drawing.Size(35, 20);
            this.switchDevStatus.TabIndex = 2;
            this.switchDevStatus.Value = false;
            // 
            // lblStatusDev
            // 
            this.lblStatusDev.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusDev.ForeColor = System.Drawing.Color.Silver;
            this.lblStatusDev.Location = new System.Drawing.Point(6, 75);
            this.lblStatusDev.Name = "lblStatusDev";
            this.lblStatusDev.Size = new System.Drawing.Size(123, 39);
            this.lblStatusDev.TabIndex = 43;
            this.lblStatusDev.Text = Resources.DeviceStatus;
            // 
            // ddlSidebarElements
            // 
            this.ddlSidebarElements.BackColor = System.Drawing.Color.Transparent;
            this.ddlSidebarElements.BorderRadius = 1;
            this.ddlSidebarElements.Color = System.Drawing.Color.Gray;
            this.ddlSidebarElements.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.ddlSidebarElements.DisabledColor = System.Drawing.Color.Gray;
            this.ddlSidebarElements.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlSidebarElements.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thick;
            this.ddlSidebarElements.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSidebarElements.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.ddlSidebarElements.FillDropDown = false;
            this.ddlSidebarElements.FillIndicator = false;
            this.ddlSidebarElements.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ddlSidebarElements.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ddlSidebarElements.ForeColor = System.Drawing.Color.White;
            this.ddlSidebarElements.FormattingEnabled = true;
            this.ddlSidebarElements.Icon = null;
            this.ddlSidebarElements.IndicatorColor = System.Drawing.Color.White;
            this.ddlSidebarElements.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.ddlSidebarElements.ItemBackColor = System.Drawing.Color.DimGray;
            this.ddlSidebarElements.ItemBorderColor = System.Drawing.Color.DimGray;
            this.ddlSidebarElements.ItemForeColor = System.Drawing.Color.White;
            this.ddlSidebarElements.ItemHeight = 26;
            this.ddlSidebarElements.ItemHighLightColor = System.Drawing.Color.Gray;
            this.ddlSidebarElements.Location = new System.Drawing.Point(143, 24);
            this.ddlSidebarElements.Name = "ddlSidebarElements";
            this.ddlSidebarElements.Size = new System.Drawing.Size(166, 32);
            this.ddlSidebarElements.TabIndex = 41;
            this.ddlSidebarElements.Text = null;
            // 
            // grbQuickViewAlarms
            // 
            this.grbQuickViewAlarms.Controls.Add(this.lblQViewAlarms);
            this.grbQuickViewAlarms.Controls.Add(this.lblAutoQViewAlarms);
            this.grbQuickViewAlarms.Controls.Add(this.switchQViewAlarms);
            this.grbQuickViewAlarms.Controls.Add(this.ddlQViewAlarms);
            this.grbQuickViewAlarms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grbQuickViewAlarms.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.grbQuickViewAlarms.ForeColor = System.Drawing.Color.Silver;
            this.grbQuickViewAlarms.Location = new System.Drawing.Point(21, 293);
            this.grbQuickViewAlarms.Name = "grbQuickViewAlarms";
            this.grbQuickViewAlarms.Size = new System.Drawing.Size(326, 129);
            this.grbQuickViewAlarms.TabIndex = 48;
            this.grbQuickViewAlarms.TabStop = false;
            this.grbQuickViewAlarms.Text = Resources.AlarmsQuickView;
            // 
            // lblQViewAlarms
            // 
            this.lblQViewAlarms.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQViewAlarms.ForeColor = System.Drawing.Color.Silver;
            this.lblQViewAlarms.Location = new System.Drawing.Point(6, 34);
            this.lblQViewAlarms.Name = "lblQViewAlarms";
            this.lblQViewAlarms.Size = new System.Drawing.Size(123, 20);
            this.lblQViewAlarms.TabIndex = 46;
            this.lblQViewAlarms.Text = "No. " +  Resources.Elements;
            // 
            // lblAutoQViewAlarms
            // 
            this.lblAutoQViewAlarms.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoQViewAlarms.ForeColor = System.Drawing.Color.Silver;
            this.lblAutoQViewAlarms.Location = new System.Drawing.Point(6, 73);
            this.lblAutoQViewAlarms.Name = "lblAutoQViewAlarms";
            this.lblAutoQViewAlarms.Size = new System.Drawing.Size(123, 39);
            this.lblAutoQViewAlarms.TabIndex = 47;
            this.lblAutoQViewAlarms.Text = Resources.DisplayAutomatically;
            // 
            // switchQViewAlarms
            // 
            this.switchQViewAlarms.BackColor = System.Drawing.Color.Transparent;
            this.switchQViewAlarms.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("switchQViewAlarms.BackgroundImage")));
            this.switchQViewAlarms.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchQViewAlarms.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchQViewAlarms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.switchQViewAlarms.Location = new System.Drawing.Point(143, 83);
            this.switchQViewAlarms.Margin = new System.Windows.Forms.Padding(0);
            this.switchQViewAlarms.Name = "switchQViewAlarms";
            this.switchQViewAlarms.OffColor = System.Drawing.Color.Gray;
            this.switchQViewAlarms.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.switchQViewAlarms.Size = new System.Drawing.Size(35, 20);
            this.switchQViewAlarms.TabIndex = 44;
            this.switchQViewAlarms.Value = false;
            // 
            // ddlQViewAlarms
            // 
            this.ddlQViewAlarms.BackColor = System.Drawing.Color.Transparent;
            this.ddlQViewAlarms.BorderRadius = 1;
            this.ddlQViewAlarms.Color = System.Drawing.Color.Gray;
            this.ddlQViewAlarms.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.ddlQViewAlarms.DisabledColor = System.Drawing.Color.Gray;
            this.ddlQViewAlarms.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlQViewAlarms.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thick;
            this.ddlQViewAlarms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlQViewAlarms.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.ddlQViewAlarms.FillDropDown = false;
            this.ddlQViewAlarms.FillIndicator = false;
            this.ddlQViewAlarms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ddlQViewAlarms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ddlQViewAlarms.ForeColor = System.Drawing.Color.White;
            this.ddlQViewAlarms.FormattingEnabled = true;
            this.ddlQViewAlarms.Icon = null;
            this.ddlQViewAlarms.IndicatorColor = System.Drawing.Color.White;
            this.ddlQViewAlarms.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.ddlQViewAlarms.ItemBackColor = System.Drawing.Color.DimGray;
            this.ddlQViewAlarms.ItemBorderColor = System.Drawing.Color.DimGray;
            this.ddlQViewAlarms.ItemForeColor = System.Drawing.Color.White;
            this.ddlQViewAlarms.ItemHeight = 26;
            this.ddlQViewAlarms.ItemHighLightColor = System.Drawing.Color.Gray;
            this.ddlQViewAlarms.Location = new System.Drawing.Point(143, 27);
            this.ddlQViewAlarms.Name = "ddlQViewAlarms";
            this.ddlQViewAlarms.Size = new System.Drawing.Size(166, 32);
            this.ddlQViewAlarms.TabIndex = 45;
            this.ddlQViewAlarms.Text = null;
            // 
            // lblLenguage
            // 
            this.lblLenguage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLenguage.ForeColor = System.Drawing.Color.Silver;
            this.lblLenguage.Location = new System.Drawing.Point(18, 52);
            this.lblLenguage.Name = "lblLenguage";
            this.lblLenguage.Size = new System.Drawing.Size(123, 22);
            this.lblLenguage.TabIndex = 40;
            this.lblLenguage.Text = Resources.Language;
            // 
            // picBoxLanguaje
            // 
            this.picBoxLanguaje.Image = global::Elipgo.SmartClient.UserControls.Properties.Resources.language_es;
            this.picBoxLanguaje.Location = new System.Drawing.Point(156, 47);
            this.picBoxLanguaje.Name = "picBoxLanguaje";
            this.picBoxLanguaje.Size = new System.Drawing.Size(32, 32);
            this.picBoxLanguaje.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxLanguaje.TabIndex = 39;
            this.picBoxLanguaje.TabStop = false;
            // 
            // bunifuButtonGuardar
            // 
            this.bunifuButtonGuardar.AllowToggling = false;
            this.bunifuButtonGuardar.AnimationSpeed = 200;
            this.bunifuButtonGuardar.AutoGenerateColors = false;
            this.bunifuButtonGuardar.BackColor = System.Drawing.Color.Transparent;
            this.bunifuButtonGuardar.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.bunifuButtonGuardar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuButtonGuardar.BackgroundImage")));
            this.bunifuButtonGuardar.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonGuardar.ButtonText = Resources.ButtonOK;
            this.bunifuButtonGuardar.ButtonTextMarginLeft = 0;
            this.bunifuButtonGuardar.ColorContrastOnClick = 45;
            this.bunifuButtonGuardar.ColorContrastOnHover = 45;
            this.bunifuButtonGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.bunifuButtonGuardar.CustomizableEdges = borderEdges1;
            this.bunifuButtonGuardar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bunifuButtonGuardar.DisabledBorderColor = System.Drawing.Color.Empty;
            this.bunifuButtonGuardar.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.bunifuButtonGuardar.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.bunifuButtonGuardar.Enabled = false;
            this.bunifuButtonGuardar.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.bunifuButtonGuardar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.bunifuButtonGuardar.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonGuardar.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButtonGuardar.IconMarginLeft = 11;
            this.bunifuButtonGuardar.IconPadding = 10;
            this.bunifuButtonGuardar.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButtonGuardar.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButtonGuardar.IdleBorderRadius = 30;
            this.bunifuButtonGuardar.IdleBorderThickness = 1;
            this.bunifuButtonGuardar.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButtonGuardar.IdleIconLeftImage = null;
            this.bunifuButtonGuardar.IdleIconRightImage = null;
            this.bunifuButtonGuardar.IndicateFocus = false;
            this.bunifuButtonGuardar.Location = new System.Drawing.Point(269, 438);
            this.bunifuButtonGuardar.Name = "bunifuButtonGuardar";
            this.bunifuButtonGuardar.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.bunifuButtonGuardar.onHoverState.BorderRadius = 30;
            this.bunifuButtonGuardar.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonGuardar.onHoverState.BorderThickness = 1;
            this.bunifuButtonGuardar.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.bunifuButtonGuardar.onHoverState.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonGuardar.onHoverState.IconLeftImage = null;
            this.bunifuButtonGuardar.onHoverState.IconRightImage = null;
            this.bunifuButtonGuardar.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButtonGuardar.OnIdleState.BorderRadius = 30;
            this.bunifuButtonGuardar.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonGuardar.OnIdleState.BorderThickness = 1;
            this.bunifuButtonGuardar.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButtonGuardar.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonGuardar.OnIdleState.IconLeftImage = null;
            this.bunifuButtonGuardar.OnIdleState.IconRightImage = null;
            this.bunifuButtonGuardar.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButtonGuardar.OnPressedState.BorderRadius = 30;
            this.bunifuButtonGuardar.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonGuardar.OnPressedState.BorderThickness = 1;
            this.bunifuButtonGuardar.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButtonGuardar.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonGuardar.OnPressedState.IconLeftImage = null;
            this.bunifuButtonGuardar.OnPressedState.IconRightImage = null;
            this.bunifuButtonGuardar.Size = new System.Drawing.Size(92, 37);
            this.bunifuButtonGuardar.TabIndex = 38;
            this.bunifuButtonGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuButtonGuardar.TextMarginLeft = 0;
            this.bunifuButtonGuardar.UseDefaultRadiusAndThickness = true;
            this.bunifuButtonGuardar.Click += new System.EventHandler(this.bunifuButtonGuardar_Click);
            // 
            // bunifuButtonCancelar
            // 
            this.bunifuButtonCancelar.AllowToggling = false;
            this.bunifuButtonCancelar.AnimationSpeed = 200;
            this.bunifuButtonCancelar.AutoGenerateColors = false;
            this.bunifuButtonCancelar.BackColor = System.Drawing.Color.Transparent;
            this.bunifuButtonCancelar.BackColor1 = System.Drawing.Color.Transparent;
            this.bunifuButtonCancelar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuButtonCancelar.BackgroundImage")));
            this.bunifuButtonCancelar.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonCancelar.ButtonText = Resources.cancel;
            this.bunifuButtonCancelar.ButtonTextMarginLeft = 0;
            this.bunifuButtonCancelar.ColorContrastOnClick = 45;
            this.bunifuButtonCancelar.ColorContrastOnHover = 45;
            this.bunifuButtonCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.bunifuButtonCancelar.CustomizableEdges = borderEdges2;
            this.bunifuButtonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bunifuButtonCancelar.DisabledBorderColor = System.Drawing.Color.Empty;
            this.bunifuButtonCancelar.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.bunifuButtonCancelar.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.bunifuButtonCancelar.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.bunifuButtonCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.bunifuButtonCancelar.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonCancelar.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButtonCancelar.IconMarginLeft = 11;
            this.bunifuButtonCancelar.IconPadding = 10;
            this.bunifuButtonCancelar.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButtonCancelar.IdleBorderColor = System.Drawing.Color.DimGray;
            this.bunifuButtonCancelar.IdleBorderRadius = 30;
            this.bunifuButtonCancelar.IdleBorderThickness = 1;
            this.bunifuButtonCancelar.IdleFillColor = System.Drawing.Color.Transparent;
            this.bunifuButtonCancelar.IdleIconLeftImage = null;
            this.bunifuButtonCancelar.IdleIconRightImage = null;
            this.bunifuButtonCancelar.IndicateFocus = false;
            this.bunifuButtonCancelar.Location = new System.Drawing.Point(175, 438);
            this.bunifuButtonCancelar.Name = "bunifuButtonCancelar";
            this.bunifuButtonCancelar.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.bunifuButtonCancelar.onHoverState.BorderRadius = 30;
            this.bunifuButtonCancelar.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonCancelar.onHoverState.BorderThickness = 1;
            this.bunifuButtonCancelar.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.bunifuButtonCancelar.onHoverState.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonCancelar.onHoverState.IconLeftImage = null;
            this.bunifuButtonCancelar.onHoverState.IconRightImage = null;
            this.bunifuButtonCancelar.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this.bunifuButtonCancelar.OnIdleState.BorderRadius = 30;
            this.bunifuButtonCancelar.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonCancelar.OnIdleState.BorderThickness = 1;
            this.bunifuButtonCancelar.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this.bunifuButtonCancelar.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonCancelar.OnIdleState.IconLeftImage = null;
            this.bunifuButtonCancelar.OnIdleState.IconRightImage = null;
            this.bunifuButtonCancelar.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButtonCancelar.OnPressedState.BorderRadius = 30;
            this.bunifuButtonCancelar.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButtonCancelar.OnPressedState.BorderThickness = 1;
            this.bunifuButtonCancelar.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButtonCancelar.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.bunifuButtonCancelar.OnPressedState.IconLeftImage = null;
            this.bunifuButtonCancelar.OnPressedState.IconRightImage = null;
            this.bunifuButtonCancelar.Size = new System.Drawing.Size(92, 37);
            this.bunifuButtonCancelar.TabIndex = 37;
            this.bunifuButtonCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuButtonCancelar.TextMarginLeft = 0;
            this.bunifuButtonCancelar.UseDefaultRadiusAndThickness = true;
            this.bunifuButtonCancelar.Click += new System.EventHandler(this.bunifuButtonCancelar_Click);
            // 
            // PanelHeader
            // 
            this.PanelHeader.BackColor = System.Drawing.Color.Black;
            this.PanelHeader.Controls.Add(this.lblTitle);
            this.PanelHeader.Controls.Add(this.bunifuImageButton2);
            this.PanelHeader.Controls.Add(this.bunifuImageButton1);
            this.PanelHeader.Controls.Add(this.ButtonClose);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(374, 32);
            this.PanelHeader.TabIndex = 28;
            this.PanelHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelHeader_MouseDown);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblTitle.ForeColor = System.Drawing.Color.Silver;
            this.lblTitle.Location = new System.Drawing.Point(23, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(270, 24);
            this.lblTitle.TabIndex = 30;
            this.lblTitle.Text = Resources.CustomSettings;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            // 
            // bunifuImageButton2
            // 
            this.bunifuImageButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuImageButton2.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton2.Image")));
            this.bunifuImageButton2.ImageActive = null;
            this.bunifuImageButton2.Location = new System.Drawing.Point(351, 5);
            this.bunifuImageButton2.Name = "bunifuImageButton2";
            this.bunifuImageButton2.Size = new System.Drawing.Size(24, 24);
            this.bunifuImageButton2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bunifuImageButton2.TabIndex = 2;
            this.bunifuImageButton2.TabStop = false;
            this.bunifuImageButton2.Zoom = 10;
            this.bunifuImageButton2.Click += new System.EventHandler(this.bunifuImageButton2_Click);
            // 
            // bunifuImageButton1
            // 
            this.bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
            this.bunifuImageButton1.ImageActive = null;
            this.bunifuImageButton1.Location = new System.Drawing.Point(444, 3);
            this.bunifuImageButton1.Name = "bunifuImageButton1";
            this.bunifuImageButton1.Size = new System.Drawing.Size(24, 24);
            this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bunifuImageButton1.TabIndex = 1;
            this.bunifuImageButton1.TabStop = false;
            this.bunifuImageButton1.Zoom = 10;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this.ButtonClose.ImageActive = null;
            this.ButtonClose.Location = new System.Drawing.Point(925, 3);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(24, 24);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Zoom = 10;
            // 
            // ddlLanguage
            // 
            this.ddlLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlLanguage.BackColor = System.Drawing.Color.Black;
            this.ddlLanguage.ForeColor = System.Drawing.Color.White;
            this.ddlLanguage.Location = new System.Drawing.Point(197, 47);
            this.ddlLanguage.Margin = new System.Windows.Forms.Padding(0);
            this.ddlLanguage.MaximumSize = new System.Drawing.Size(150, 38);
            this.ddlLanguage.Name = "ddlLanguage";
            this.ddlLanguage.SelectedIndex = 0;
            this.ddlLanguage.Size = new System.Drawing.Size(150, 32);
            this.ddlLanguage.TabIndex = 17;
            // 
            // CustomSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Name = "CustomSettingsControl";
            this.Size = new System.Drawing.Size(378, 493);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grbSidebar.ResumeLayout(false);
            this.grbQuickViewAlarms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLanguaje)).EndInit();
            this.PanelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PanelHeader;
        private Shared.Dropdown ddlLanguage;
        private Bunifu.Framework.UI.BunifuImageButton bunifuImageButton1;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
        private Bunifu.Framework.UI.BunifuImageButton bunifuImageButton2;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton bunifuButtonGuardar;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton bunifuButtonCancelar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox picBoxLanguaje;
        private System.Windows.Forms.Label lblLenguage;
        private System.Windows.Forms.Label lblSidebarElements;
        private Bunifu.UI.WinForms.BunifuDropdown ddlSidebarElements;
        private Bunifu.Framework.UI.BunifuiOSSwitch switchDevStatus;
        private System.Windows.Forms.Label lblStatusDev;
        private System.Windows.Forms.Label lblAutoQViewAlarms;
        private System.Windows.Forms.Label lblQViewAlarms;
        private Bunifu.UI.WinForms.BunifuDropdown ddlQViewAlarms;
        private Bunifu.Framework.UI.BunifuiOSSwitch switchQViewAlarms;
        private System.Windows.Forms.GroupBox grbQuickViewAlarms;
        private System.Windows.Forms.GroupBox grbSidebar;
        private System.Windows.Forms.Label lblDownloadFolder;
        private System.Windows.Forms.TextBox txbDownloadFolder;
    }
}
