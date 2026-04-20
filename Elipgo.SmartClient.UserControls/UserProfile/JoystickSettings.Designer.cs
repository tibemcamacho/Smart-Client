namespace Elipgo.SmartClient.UserControls.UserProfile
{
    partial class JoystickSettings
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
            if (this.joystick != null)
                this.joystick.ConfigMode = false;
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JoystickSettings));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges7 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges8 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges9 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bunifuButton1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.pnlContainerUSB = new System.Windows.Forms.Panel();
            this.invertZRotationCheckBox = new System.Windows.Forms.CheckBox();
            this.invertYCheckBox = new System.Windows.Forms.CheckBox();
            this.invertXCheckBox = new System.Windows.Forms.CheckBox();
            this.actionsMappingListView = new System.Windows.Forms.ListView();
            this.actionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bunifuButtonGuardar = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.bunifuButtonCancelar = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.DropDownConfig = new Bunifu.UI.WinForms.BunifuDropdown();
            this.actionsLabel = new System.Windows.Forms.Label();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.bunifuImageButton2 = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.panel1.SuspendLayout();
            this.pnlContainerUSB.SuspendLayout();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pnlContainerUSB);
            this.panel1.Controls.Add(this.bunifuButtonGuardar);
            this.panel1.Controls.Add(this.bunifuButtonCancelar);
            this.panel1.Controls.Add(this.DropDownConfig);
            this.panel1.Controls.Add(this.actionsLabel);
            this.panel1.Controls.Add(this.PanelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(378, 537);
            this.panel1.TabIndex = 0;
            // 
            // bunifuButton1
            // 
            this.bunifuButton1.AllowToggling = false;
            this.bunifuButton1.AnimationSpeed = 200;
            this.bunifuButton1.AutoGenerateColors = false;
            this.bunifuButton1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuButton1.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.bunifuButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuButton1.BackgroundImage")));
            this.bunifuButton1.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.ButtonText = "Calibrar";
            this.bunifuButton1.ButtonTextMarginLeft = 0;
            this.bunifuButton1.ColorContrastOnClick = 45;
            this.bunifuButton1.ColorContrastOnHover = 45;
            this.bunifuButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges7.BottomLeft = true;
            borderEdges7.BottomRight = true;
            borderEdges7.TopLeft = true;
            borderEdges7.TopRight = true;
            this.bunifuButton1.CustomizableEdges = borderEdges7;
            this.bunifuButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bunifuButton1.DisabledBorderColor = System.Drawing.Color.Empty;
            this.bunifuButton1.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.bunifuButton1.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.bunifuButton1.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.bunifuButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.bunifuButton1.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButton1.IconMarginLeft = 11;
            this.bunifuButton1.IconPadding = 10;
            this.bunifuButton1.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuButton1.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButton1.IdleBorderRadius = 30;
            this.bunifuButton1.IdleBorderThickness = 1;
            this.bunifuButton1.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButton1.IdleIconLeftImage = null;
            this.bunifuButton1.IdleIconRightImage = null;
            this.bunifuButton1.IndicateFocus = false;
            this.bunifuButton1.Location = new System.Drawing.Point(254, 174);
            this.bunifuButton1.Name = "bunifuButton1";
            this.bunifuButton1.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.bunifuButton1.onHoverState.BorderRadius = 30;
            this.bunifuButton1.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.onHoverState.BorderThickness = 1;
            this.bunifuButton1.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.bunifuButton1.onHoverState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.onHoverState.IconLeftImage = null;
            this.bunifuButton1.onHoverState.IconRightImage = null;
            this.bunifuButton1.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButton1.OnIdleState.BorderRadius = 30;
            this.bunifuButton1.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnIdleState.BorderThickness = 1;
            this.bunifuButton1.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this.bunifuButton1.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.OnIdleState.IconLeftImage = null;
            this.bunifuButton1.OnIdleState.IconRightImage = null;
            this.bunifuButton1.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButton1.OnPressedState.BorderRadius = 30;
            this.bunifuButton1.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnPressedState.BorderThickness = 1;
            this.bunifuButton1.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButton1.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.OnPressedState.IconLeftImage = null;
            this.bunifuButton1.OnPressedState.IconRightImage = null;
            this.bunifuButton1.Size = new System.Drawing.Size(92, 37);
            this.bunifuButton1.TabIndex = 39;
            this.bunifuButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuButton1.TextMarginLeft = 0;
            this.bunifuButton1.UseDefaultRadiusAndThickness = true;
            this.bunifuButton1.Click += new System.EventHandler(this.bunifuButton1_Click);
            // 
            // pnlContainerUSB
            // 
            this.pnlContainerUSB.BackColor = System.Drawing.Color.Transparent;
            this.pnlContainerUSB.Controls.Add(this.bunifuButton1);
            this.pnlContainerUSB.Controls.Add(this.invertZRotationCheckBox);
            this.pnlContainerUSB.Controls.Add(this.invertYCheckBox);
            this.pnlContainerUSB.Controls.Add(this.invertXCheckBox);
            this.pnlContainerUSB.Controls.Add(this.actionsMappingListView);
            this.pnlContainerUSB.Location = new System.Drawing.Point(3, 157);
            this.pnlContainerUSB.Name = "pnlContainerUSB";
            this.pnlContainerUSB.Size = new System.Drawing.Size(372, 322);
            this.pnlContainerUSB.TabIndex = 40;
            // 
            // invertZRotationCheckBox
            // 
            this.invertZRotationCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.invertZRotationCheckBox.ForeColor = System.Drawing.Color.Silver;
            this.invertZRotationCheckBox.Location = new System.Drawing.Point(254, 136);
            this.invertZRotationCheckBox.Name = "invertZRotationCheckBox";
            this.invertZRotationCheckBox.Size = new System.Drawing.Size(104, 32);
            this.invertZRotationCheckBox.TabIndex = 35;
            this.invertZRotationCheckBox.Text = "Invertir Rotación Eje Z";
            this.invertZRotationCheckBox.CheckedChanged += new System.EventHandler(this.invertZRotationCheckBox_CheckedChanged);
            // 
            // invertYCheckBox
            // 
            this.invertYCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.invertYCheckBox.ForeColor = System.Drawing.Color.Silver;
            this.invertYCheckBox.Location = new System.Drawing.Point(254, 91);
            this.invertYCheckBox.Name = "invertYCheckBox";
            this.invertYCheckBox.Size = new System.Drawing.Size(104, 24);
            this.invertYCheckBox.TabIndex = 34;
            this.invertYCheckBox.Text = "Invertir Eje Y";
            this.invertYCheckBox.CheckedChanged += new System.EventHandler(this.invertYCheckBox_CheckedChanged);
            // 
            // invertXCheckBox
            // 
            this.invertXCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invertXCheckBox.ForeColor = System.Drawing.Color.Silver;
            this.invertXCheckBox.Location = new System.Drawing.Point(254, 48);
            this.invertXCheckBox.Name = "invertXCheckBox";
            this.invertXCheckBox.Size = new System.Drawing.Size(104, 24);
            this.invertXCheckBox.TabIndex = 33;
            this.invertXCheckBox.Text = "Invertir Eje X";
            this.invertXCheckBox.CheckedChanged += new System.EventHandler(this.invertXCheckBox_CheckedChanged);
            // 
            // actionsMappingListView
            // 
            this.actionsMappingListView.BackColor = System.Drawing.Color.Black;
            this.actionsMappingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.actionColumnHeader,
            this.buttonColumnHeader});
            this.actionsMappingListView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.actionsMappingListView.ForeColor = System.Drawing.Color.Silver;
            this.actionsMappingListView.HideSelection = false;
            this.actionsMappingListView.Location = new System.Drawing.Point(18, 10);
            this.actionsMappingListView.Name = "actionsMappingListView";
            this.actionsMappingListView.OwnerDraw = true;
            this.actionsMappingListView.Size = new System.Drawing.Size(222, 297);
            this.actionsMappingListView.TabIndex = 32;
            this.actionsMappingListView.UseCompatibleStateImageBehavior = false;
            this.actionsMappingListView.View = System.Windows.Forms.View.Details;
            this.actionsMappingListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.actionsMappingListView_DrawColumnHeader);
            this.actionsMappingListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.actionsMappingListView_DrawItem);
            // 
            // actionColumnHeader
            // 
            this.actionColumnHeader.Text = "Acción";
            this.actionColumnHeader.Width = 100;
            // 
            // buttonColumnHeader
            // 
            this.buttonColumnHeader.Text = "Botón";
            this.buttonColumnHeader.Width = 100;
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
            this.bunifuButtonGuardar.ButtonText = Elipgo.SmartClient.Common.Properties.Resources.ButtonOK;
            this.bunifuButtonGuardar.ButtonTextMarginLeft = 0;
            this.bunifuButtonGuardar.ColorContrastOnClick = 45;
            this.bunifuButtonGuardar.ColorContrastOnHover = 45;
            this.bunifuButtonGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges8.BottomLeft = true;
            borderEdges8.BottomRight = true;
            borderEdges8.TopLeft = true;
            borderEdges8.TopRight = true;
            this.bunifuButtonGuardar.CustomizableEdges = borderEdges8;
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
            this.bunifuButtonGuardar.Location = new System.Drawing.Point(269, 485);
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
            this.bunifuButtonCancelar.ButtonText = Elipgo.SmartClient.Common.Properties.Resources.ButtonCancel;
            this.bunifuButtonCancelar.ButtonTextMarginLeft = 0;
            this.bunifuButtonCancelar.ColorContrastOnClick = 45;
            this.bunifuButtonCancelar.ColorContrastOnHover = 45;
            this.bunifuButtonCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges9.BottomLeft = true;
            borderEdges9.BottomRight = true;
            borderEdges9.TopLeft = true;
            borderEdges9.TopRight = true;
            this.bunifuButtonCancelar.CustomizableEdges = borderEdges9;
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
            this.bunifuButtonCancelar.Location = new System.Drawing.Point(175, 485);
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
            // DropDownConfig
            // 
            this.DropDownConfig.BackColor = System.Drawing.Color.Transparent;
            this.DropDownConfig.BorderRadius = 1;
            this.DropDownConfig.Color = System.Drawing.Color.Gray;
            this.DropDownConfig.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.DropDownConfig.DisabledColor = System.Drawing.Color.Gray;
            this.DropDownConfig.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropDownConfig.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thick;
            this.DropDownConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DropDownConfig.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.DropDownConfig.FillDropDown = false;
            this.DropDownConfig.FillIndicator = false;
            this.DropDownConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DropDownConfig.ForeColor = System.Drawing.Color.White;
            this.DropDownConfig.FormattingEnabled = true;
            this.DropDownConfig.Icon = null;
            this.DropDownConfig.IndicatorColor = System.Drawing.Color.White;
            this.DropDownConfig.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.DropDownConfig.ItemBackColor = System.Drawing.Color.DimGray;
            this.DropDownConfig.ItemBorderColor = System.Drawing.Color.DimGray;
            this.DropDownConfig.ItemForeColor = System.Drawing.Color.White;
            this.DropDownConfig.ItemHeight = 26;
            this.DropDownConfig.ItemHighLightColor = System.Drawing.Color.Gray;
            this.DropDownConfig.Location = new System.Drawing.Point(17, 90);
            this.DropDownConfig.Name = "DropDownConfig";
            this.DropDownConfig.Size = new System.Drawing.Size(250, 32);
            this.DropDownConfig.TabIndex = 17;
            this.DropDownConfig.Text = null;
            // 
            // actionsLabel
            // 
            this.actionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.actionsLabel.ForeColor = System.Drawing.Color.Silver;
            this.actionsLabel.Location = new System.Drawing.Point(18, 41);
            this.actionsLabel.Name = "actionsLabel";
            this.actionsLabel.Size = new System.Drawing.Size(100, 16);
            this.actionsLabel.TabIndex = 29;
            this.actionsLabel.Text = "Acciones";
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
            this.lblTitle.Text = "title";
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
            // JoystickSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Name = "JoystickSettings";
            this.Size = new System.Drawing.Size(378, 537);
            this.panel1.ResumeLayout(false);
            this.pnlContainerUSB.ResumeLayout(false);
            this.PanelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PanelHeader;
        private Bunifu.UI.WinForms.BunifuDropdown DropDownConfig;
        private Bunifu.Framework.UI.BunifuImageButton bunifuImageButton1;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
        private Bunifu.Framework.UI.BunifuImageButton bunifuImageButton2;
        private System.Windows.Forms.CheckBox invertZRotationCheckBox;
        private System.Windows.Forms.CheckBox invertYCheckBox;
        private System.Windows.Forms.CheckBox invertXCheckBox;
        private System.Windows.Forms.ListView actionsMappingListView;
        private System.Windows.Forms.ColumnHeader actionColumnHeader;
        private System.Windows.Forms.ColumnHeader buttonColumnHeader;
        private System.Windows.Forms.Label actionsLabel;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton bunifuButton1;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton bunifuButtonGuardar;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton bunifuButtonCancelar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContainerUSB;
    }
}
