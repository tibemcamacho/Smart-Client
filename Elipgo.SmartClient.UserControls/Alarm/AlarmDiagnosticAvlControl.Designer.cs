namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class AlarmDiagnosticAvlControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmDiagnosticAvlControl));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties9 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties10 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties11 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties12 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges5 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges6 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.ToggleSwitch.ToggleState toggleState7 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState8 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState9 = new Bunifu.ToggleSwitch.ToggleState();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAlarmConfirm = new System.Windows.Forms.Label();
            this.bunifuSeparator1 = new Bunifu.Framework.UI.BunifuSeparator();
            this.txtNote = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.ButtonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.ButtonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.dbFlowLayoutPanel1 = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.ContentEmail = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.ScrollEmail = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.ContentBody = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.ContentStep = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.ScrollBar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.AlarmStateToggleSwitch = new Bunifu.ToggleSwitch.BunifuToggleSwitch();
            this.AlarmState = new System.Windows.Forms.Label();
            this.Note = new System.Windows.Forms.Label();
            this.AddNote = new System.Windows.Forms.Label();
            this.PanelCamera = new System.Windows.Forms.Panel();
            this.AlarmDateTime = new System.Windows.Forms.Label();
            this.DeviceName = new System.Windows.Forms.Label();
            this.AlarmLocation = new System.Windows.Forms.Label();
            this.AlarmType = new System.Windows.Forms.Label();
            this.AlarmIcon = new System.Windows.Forms.PictureBox();
            this.Procedure = new System.Windows.Forms.Label();
            this.dbFlowLayoutPanel1.SuspendLayout();
            this.ContentBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlarmIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1460, 637);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 21);
            this.label1.TabIndex = 55;
            this.label1.Text = "Add note";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAlarmConfirm
            // 
            this.lblAlarmConfirm.BackColor = System.Drawing.Color.Transparent;
            this.lblAlarmConfirm.ForeColor = System.Drawing.Color.White;
            this.lblAlarmConfirm.Location = new System.Drawing.Point(1461, 577);
            this.lblAlarmConfirm.Name = "lblAlarmConfirm";
            this.lblAlarmConfirm.Size = new System.Drawing.Size(200, 24);
            this.lblAlarmConfirm.TabIndex = 54;
            this.lblAlarmConfirm.Text = "Alarma confirmada";
            this.lblAlarmConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(1461, 467);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Size = new System.Drawing.Size(417, 35);
            this.bunifuSeparator1.TabIndex = 53;
            this.bunifuSeparator1.Transparency = 255;
            this.bunifuSeparator1.Vertical = false;
            // 
            // txtNote
            // 
            this.txtNote.AcceptsReturn = false;
            this.txtNote.AcceptsTab = false;
            this.txtNote.AnimationSpeed = 200;
            this.txtNote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNote.BackColor = System.Drawing.Color.Transparent;
            this.txtNote.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtNote.BackgroundImage")));
            this.txtNote.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.txtNote.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.txtNote.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.txtNote.BorderColorIdle = System.Drawing.Color.Silver;
            this.txtNote.BorderRadius = 20;
            this.txtNote.BorderThickness = 1;
            this.txtNote.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtNote.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNote.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.txtNote.DefaultText = "";
            this.txtNote.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtNote.ForeColor = System.Drawing.Color.Silver;
            this.txtNote.HideSelection = true;
            this.txtNote.IconLeft = null;
            this.txtNote.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNote.IconPadding = 10;
            this.txtNote.IconRight = null;
            this.txtNote.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNote.Lines = new string[0];
            this.txtNote.Location = new System.Drawing.Point(1462, 382);
            this.txtNote.MaxLength = 32767;
            this.txtNote.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtNote.Modified = false;
            this.txtNote.Multiline = false;
            this.txtNote.Name = "txtNote";
            stateProperties9.BorderColor = System.Drawing.Color.Silver;
            stateProperties9.FillColor = System.Drawing.Color.Empty;
            stateProperties9.ForeColor = System.Drawing.Color.Empty;
            stateProperties9.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtNote.OnActiveState = stateProperties9;
            stateProperties10.BorderColor = System.Drawing.Color.Empty;
            stateProperties10.FillColor = System.Drawing.Color.White;
            stateProperties10.ForeColor = System.Drawing.Color.Empty;
            stateProperties10.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtNote.OnDisabledState = stateProperties10;
            stateProperties11.BorderColor = System.Drawing.Color.Silver;
            stateProperties11.FillColor = System.Drawing.Color.Empty;
            stateProperties11.ForeColor = System.Drawing.Color.Empty;
            stateProperties11.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtNote.OnHoverState = stateProperties11;
            stateProperties12.BorderColor = System.Drawing.Color.Silver;
            stateProperties12.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            stateProperties12.ForeColor = System.Drawing.Color.Silver;
            stateProperties12.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtNote.OnIdleState = stateProperties12;
            this.txtNote.PasswordChar = '\0';
            this.txtNote.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtNote.PlaceholderText = Elipgo.SmartClient.Common.Properties.Resources.AddObservation;
            this.txtNote.ReadOnly = false;
            this.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNote.SelectedText = "";
            this.txtNote.SelectionLength = 0;
            this.txtNote.SelectionStart = 0;
            this.txtNote.ShortcutsEnabled = true;
            this.txtNote.Size = new System.Drawing.Size(417, 62);
            this.txtNote.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Bunifu;
            this.txtNote.TabIndex = 52;
            this.txtNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtNote.TextMarginBottom = 0;
            this.txtNote.TextMarginLeft = 5;
            this.txtNote.TextMarginTop = 0;
            this.txtNote.TextPlaceholder = Elipgo.SmartClient.Common.Properties.Resources.AddObservation;
            this.txtNote.UseSystemPasswordChar = false;
            this.txtNote.WordWrap = true;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.AllowToggling = false;
            this.ButtonCancel.AnimationSpeed = 200;
            this.ButtonCancel.AutoGenerateColors = false;
            this.ButtonCancel.BackColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.BackColor1 = System.Drawing.Color.Transparent;
            this.ButtonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonCancel.BackgroundImage")));
            this.ButtonCancel.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.ButtonText = "CANCEL";
            this.ButtonCancel.ButtonTextMarginLeft = 0;
            this.ButtonCancel.ColorContrastOnClick = 45;
            this.ButtonCancel.ColorContrastOnHover = 45;
            this.ButtonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges5.BottomLeft = true;
            borderEdges5.BottomRight = true;
            borderEdges5.TopLeft = true;
            borderEdges5.TopRight = true;
            this.ButtonCancel.CustomizableEdges = borderEdges5;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonCancel.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonCancel.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.ButtonCancel.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonCancel.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCancel.IconMarginLeft = 11;
            this.ButtonCancel.IconPadding = 10;
            this.ButtonCancel.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCancel.IdleBorderColor = System.Drawing.Color.DimGray;
            this.ButtonCancel.IdleBorderRadius = 30;
            this.ButtonCancel.IdleBorderThickness = 1;
            this.ButtonCancel.IdleFillColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.IdleIconLeftImage = null;
            this.ButtonCancel.IdleIconRightImage = null;
            this.ButtonCancel.IndicateFocus = false;
            this.ButtonCancel.Location = new System.Drawing.Point(1692, 847);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonCancel.onHoverState.BorderRadius = 30;
            this.ButtonCancel.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.onHoverState.BorderThickness = 1;
            this.ButtonCancel.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonCancel.onHoverState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.onHoverState.IconLeftImage = null;
            this.ButtonCancel.onHoverState.IconRightImage = null;
            this.ButtonCancel.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this.ButtonCancel.OnIdleState.BorderRadius = 30;
            this.ButtonCancel.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.OnIdleState.BorderThickness = 1;
            this.ButtonCancel.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.OnIdleState.IconLeftImage = null;
            this.ButtonCancel.OnIdleState.IconRightImage = null;
            this.ButtonCancel.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonCancel.OnPressedState.BorderRadius = 30;
            this.ButtonCancel.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.OnPressedState.BorderThickness = 1;
            this.ButtonCancel.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonCancel.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.OnPressedState.IconLeftImage = null;
            this.ButtonCancel.OnPressedState.IconRightImage = null;
            this.ButtonCancel.Size = new System.Drawing.Size(92, 37);
            this.ButtonCancel.TabIndex = 51;
            this.ButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonCancel.TextMarginLeft = 0;
            this.ButtonCancel.UseDefaultRadiusAndThickness = true;
            // 
            // ButtonOK
            // 
            this.ButtonOK.AllowToggling = false;
            this.ButtonOK.AnimationSpeed = 200;
            this.ButtonOK.AutoGenerateColors = false;
            this.ButtonOK.BackColor = System.Drawing.Color.Transparent;
            this.ButtonOK.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this.ButtonOK.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.ButtonText = "OK";
            this.ButtonOK.ButtonTextMarginLeft = 0;
            this.ButtonOK.ColorContrastOnClick = 45;
            this.ButtonOK.ColorContrastOnHover = 45;
            this.ButtonOK.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges6.BottomLeft = true;
            borderEdges6.BottomRight = true;
            borderEdges6.TopLeft = true;
            borderEdges6.TopRight = true;
            this.ButtonOK.CustomizableEdges = borderEdges6;
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ButtonOK.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonOK.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonOK.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.ButtonOK.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonOK.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonOK.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOK.IconMarginLeft = 11;
            this.ButtonOK.IconPadding = 10;
            this.ButtonOK.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOK.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.IdleBorderRadius = 30;
            this.ButtonOK.IdleBorderThickness = 1;
            this.ButtonOK.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.IdleIconLeftImage = null;
            this.ButtonOK.IdleIconRightImage = null;
            this.ButtonOK.IndicateFocus = false;
            this.ButtonOK.Location = new System.Drawing.Point(1790, 847);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonOK.onHoverState.BorderRadius = 30;
            this.ButtonOK.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.onHoverState.BorderThickness = 1;
            this.ButtonOK.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonOK.onHoverState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.onHoverState.IconLeftImage = null;
            this.ButtonOK.onHoverState.IconRightImage = null;
            this.ButtonOK.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.OnIdleState.BorderRadius = 30;
            this.ButtonOK.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.OnIdleState.BorderThickness = 1;
            this.ButtonOK.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.OnIdleState.IconLeftImage = null;
            this.ButtonOK.OnIdleState.IconRightImage = null;
            this.ButtonOK.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonOK.OnPressedState.BorderRadius = 30;
            this.ButtonOK.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.OnPressedState.BorderThickness = 1;
            this.ButtonOK.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonOK.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.OnPressedState.IconLeftImage = null;
            this.ButtonOK.OnPressedState.IconRightImage = null;
            this.ButtonOK.Size = new System.Drawing.Size(92, 37);
            this.ButtonOK.TabIndex = 50;
            this.ButtonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonOK.TextMarginLeft = 0;
            this.ButtonOK.UseDefaultRadiusAndThickness = true;
            // 
            // dbFlowLayoutPanel1
            // 
            this.dbFlowLayoutPanel1.Controls.Add(this.ContentEmail);
            this.dbFlowLayoutPanel1.Controls.Add(this.ScrollEmail);
            this.dbFlowLayoutPanel1.Location = new System.Drawing.Point(1456, 687);
            this.dbFlowLayoutPanel1.Name = "dbFlowLayoutPanel1";
            this.dbFlowLayoutPanel1.Size = new System.Drawing.Size(426, 135);
            this.dbFlowLayoutPanel1.TabIndex = 49;
            // 
            // ContentEmail
            // 
            this.ContentEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentEmail.ForeColor = System.Drawing.Color.White;
            this.ContentEmail.Location = new System.Drawing.Point(3, 3);
            this.ContentEmail.Name = "ContentEmail";
            this.ContentEmail.Size = new System.Drawing.Size(394, 124);
            this.ContentEmail.TabIndex = 12;
            // 
            // ScrollEmail
            // 
            this.ScrollEmail.AllowCursorChanges = true;
            this.ScrollEmail.AllowHomeEndKeysDetection = false;
            this.ScrollEmail.AllowIncrementalClickMoves = true;
            this.ScrollEmail.AllowMouseDownEffects = true;
            this.ScrollEmail.AllowMouseHoverEffects = true;
            this.ScrollEmail.AllowScrollingAnimations = true;
            this.ScrollEmail.AllowScrollKeysDetection = true;
            this.ScrollEmail.AllowScrollOptionsMenu = true;
            this.ScrollEmail.AllowShrinkingOnFocusLost = false;
            this.ScrollEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollEmail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollEmail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ScrollEmail.BackgroundImage")));
            this.ScrollEmail.BindingContainer = null;
            this.ScrollEmail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollEmail.BorderRadius = 10;
            this.ScrollEmail.BorderThickness = 1;
            this.ScrollEmail.DurationBeforeShrink = 2000;
            this.ScrollEmail.LargeChange = 10;
            this.ScrollEmail.Location = new System.Drawing.Point(404, 4);
            this.ScrollEmail.Margin = new System.Windows.Forms.Padding(4);
            this.ScrollEmail.Maximum = 100;
            this.ScrollEmail.Minimum = 0;
            this.ScrollEmail.MinimumThumbLength = 18;
            this.ScrollEmail.Name = "ScrollEmail";
            this.ScrollEmail.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.ScrollEmail.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.ScrollEmail.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.ScrollEmail.ScrollBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollEmail.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollEmail.ShrinkSizeLimit = 3;
            this.ScrollEmail.Size = new System.Drawing.Size(13, 122);
            this.ScrollEmail.SmallChange = 1;
            this.ScrollEmail.TabIndex = 8;
            this.ScrollEmail.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.ScrollEmail.ThumbLength = 18;
            this.ScrollEmail.ThumbMargin = 1;
            this.ScrollEmail.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this.ScrollEmail.Value = 0;
            // 
            // ContentBody
            // 
            this.ContentBody.Controls.Add(this.ContentStep);
            this.ContentBody.Controls.Add(this.ScrollBar);
            this.ContentBody.Location = new System.Drawing.Point(1461, 91);
            this.ContentBody.Name = "ContentBody";
            this.ContentBody.Size = new System.Drawing.Size(421, 123);
            this.ContentBody.TabIndex = 48;
            // 
            // ContentStep
            // 
            this.ContentStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentStep.ForeColor = System.Drawing.Color.White;
            this.ContentStep.Location = new System.Drawing.Point(3, 3);
            this.ContentStep.Name = "ContentStep";
            this.ContentStep.Size = new System.Drawing.Size(389, 115);
            this.ContentStep.TabIndex = 12;
            // 
            // ScrollBar
            // 
            this.ScrollBar.AllowCursorChanges = true;
            this.ScrollBar.AllowHomeEndKeysDetection = false;
            this.ScrollBar.AllowIncrementalClickMoves = true;
            this.ScrollBar.AllowMouseDownEffects = true;
            this.ScrollBar.AllowMouseHoverEffects = true;
            this.ScrollBar.AllowScrollingAnimations = true;
            this.ScrollBar.AllowScrollKeysDetection = true;
            this.ScrollBar.AllowScrollOptionsMenu = true;
            this.ScrollBar.AllowShrinkingOnFocusLost = false;
            this.ScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollBar.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ScrollBar.BackgroundImage")));
            this.ScrollBar.BindingContainer = null;
            this.ScrollBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.BorderRadius = 10;
            this.ScrollBar.BorderThickness = 1;
            this.ScrollBar.DurationBeforeShrink = 2000;
            this.ScrollBar.LargeChange = 10;
            this.ScrollBar.Location = new System.Drawing.Point(399, 4);
            this.ScrollBar.Margin = new System.Windows.Forms.Padding(4);
            this.ScrollBar.Maximum = 100;
            this.ScrollBar.Minimum = 0;
            this.ScrollBar.MinimumThumbLength = 18;
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.ScrollBar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.ScrollBar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.ScrollBar.ScrollBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.ShrinkSizeLimit = 3;
            this.ScrollBar.Size = new System.Drawing.Size(13, 113);
            this.ScrollBar.SmallChange = 1;
            this.ScrollBar.TabIndex = 8;
            this.ScrollBar.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.ScrollBar.ThumbLength = 18;
            this.ScrollBar.ThumbMargin = 1;
            this.ScrollBar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this.ScrollBar.Value = 0;
            this.ScrollBar.Visible = false;
            // 
            // AlarmStateToggleSwitch
            // 
            this.AlarmStateToggleSwitch.Animation = 5;
            this.AlarmStateToggleSwitch.BackColor = System.Drawing.Color.Transparent;
            this.AlarmStateToggleSwitch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AlarmStateToggleSwitch.BackgroundImage")));
            this.AlarmStateToggleSwitch.Checked = true;
            this.AlarmStateToggleSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AlarmStateToggleSwitch.InnerCirclePadding = 3;
            this.AlarmStateToggleSwitch.Location = new System.Drawing.Point(1838, 583);
            this.AlarmStateToggleSwitch.Name = "AlarmStateToggleSwitch";
            this.AlarmStateToggleSwitch.Size = new System.Drawing.Size(35, 18);
            this.AlarmStateToggleSwitch.TabIndex = 47;
            toggleState7.BackColor = System.Drawing.Color.Empty;
            toggleState7.BackColorInner = System.Drawing.Color.Empty;
            toggleState7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            toggleState7.BorderColorInner = System.Drawing.Color.Empty;
            toggleState7.BorderRadius = 1;
            toggleState7.BorderRadiusInner = 1;
            toggleState7.BorderThickness = 1;
            toggleState7.BorderThicknessInner = 1;
            this.AlarmStateToggleSwitch.ToggleStateDisabled = toggleState7;
            toggleState8.BackColor = System.Drawing.Color.Empty;
            toggleState8.BackColorInner = System.Drawing.Color.Empty;
            toggleState8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            toggleState8.BorderColorInner = System.Drawing.Color.Empty;
            toggleState8.BorderRadius = 1;
            toggleState8.BorderRadiusInner = 1;
            toggleState8.BorderThickness = 1;
            toggleState8.BorderThicknessInner = 1;
            this.AlarmStateToggleSwitch.ToggleStateOff = toggleState8;
            toggleState9.BackColor = System.Drawing.Color.DodgerBlue;
            toggleState9.BackColorInner = System.Drawing.Color.White;
            toggleState9.BorderColor = System.Drawing.Color.DodgerBlue;
            toggleState9.BorderColorInner = System.Drawing.Color.White;
            toggleState9.BorderRadius = 17;
            toggleState9.BorderRadiusInner = 11;
            toggleState9.BorderThickness = 1;
            toggleState9.BorderThicknessInner = 1;
            this.AlarmStateToggleSwitch.ToggleStateOn = toggleState9;
            this.AlarmStateToggleSwitch.Value = true;
            // 
            // AlarmState
            // 
            this.AlarmState.BackColor = System.Drawing.Color.Transparent;
            this.AlarmState.ForeColor = System.Drawing.Color.White;
            this.AlarmState.Location = new System.Drawing.Point(1460, 504);
            this.AlarmState.Name = "AlarmState";
            this.AlarmState.Size = new System.Drawing.Size(200, 24);
            this.AlarmState.TabIndex = 44;
            this.AlarmState.Text = "State alarm";
            this.AlarmState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Note
            // 
            this.Note.BackColor = System.Drawing.Color.Transparent;
            this.Note.ForeColor = System.Drawing.Color.White;
            this.Note.Location = new System.Drawing.Point(1462, 347);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(200, 21);
            this.Note.TabIndex = 45;
            this.Note.Text = "note";
            this.Note.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddNote
            // 
            this.AddNote.BackColor = System.Drawing.Color.Transparent;
            this.AddNote.ForeColor = System.Drawing.Color.White;
            this.AddNote.Location = new System.Drawing.Point(1460, 298);
            this.AddNote.Name = "AddNote";
            this.AddNote.Size = new System.Drawing.Size(200, 21);
            this.AddNote.TabIndex = 46;
            this.AddNote.Text = "Add note";
            this.AddNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelCamera
            // 
            this.PanelCamera.BackColor = System.Drawing.Color.Transparent;
            this.PanelCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelCamera.Location = new System.Drawing.Point(5, 153);
            this.PanelCamera.Name = "PanelCamera";
            this.PanelCamera.Size = new System.Drawing.Size(1393, 731);
            this.PanelCamera.TabIndex = 43;
            // 
            // AlarmDateTime
            // 
            this.AlarmDateTime.BackColor = System.Drawing.Color.Transparent;
            this.AlarmDateTime.ForeColor = System.Drawing.Color.White;
            this.AlarmDateTime.Location = new System.Drawing.Point(196, 53);
            this.AlarmDateTime.Name = "AlarmDateTime";
            this.AlarmDateTime.Size = new System.Drawing.Size(200, 21);
            this.AlarmDateTime.TabIndex = 42;
            this.AlarmDateTime.Text = "DateTime";
            this.AlarmDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeviceName
            // 
            this.DeviceName.BackColor = System.Drawing.Color.Transparent;
            this.DeviceName.ForeColor = System.Drawing.Color.White;
            this.DeviceName.Location = new System.Drawing.Point(2, 53);
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Size = new System.Drawing.Size(200, 21);
            this.DeviceName.TabIndex = 38;
            this.DeviceName.Text = "Device Name";
            this.DeviceName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AlarmLocation
            // 
            this.AlarmLocation.BackColor = System.Drawing.Color.Transparent;
            this.AlarmLocation.ForeColor = System.Drawing.Color.White;
            this.AlarmLocation.Location = new System.Drawing.Point(50, 15);
            this.AlarmLocation.Name = "AlarmLocation";
            this.AlarmLocation.Size = new System.Drawing.Size(300, 16);
            this.AlarmLocation.TabIndex = 39;
            this.AlarmLocation.Text = "Location";
            // 
            // AlarmType
            // 
            this.AlarmType.BackColor = System.Drawing.Color.Transparent;
            this.AlarmType.ForeColor = System.Drawing.Color.White;
            this.AlarmType.Location = new System.Drawing.Point(50, -1);
            this.AlarmType.Name = "AlarmType";
            this.AlarmType.Size = new System.Drawing.Size(300, 16);
            this.AlarmType.TabIndex = 40;
            this.AlarmType.Text = "AlarmType";
            // 
            // AlarmIcon
            // 
            this.AlarmIcon.BackColor = System.Drawing.Color.Transparent;
            this.AlarmIcon.Location = new System.Drawing.Point(1, 3);
            this.AlarmIcon.Margin = new System.Windows.Forms.Padding(0);
            this.AlarmIcon.Name = "AlarmIcon";
            this.AlarmIcon.Size = new System.Drawing.Size(32, 32);
            this.AlarmIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AlarmIcon.TabIndex = 41;
            this.AlarmIcon.TabStop = false;
            // 
            // Procedure
            // 
            this.Procedure.BackColor = System.Drawing.Color.Transparent;
            this.Procedure.ForeColor = System.Drawing.Color.White;
            this.Procedure.Location = new System.Drawing.Point(1459, 5);
            this.Procedure.Name = "Procedure";
            this.Procedure.Size = new System.Drawing.Size(200, 24);
            this.Procedure.TabIndex = 56;
            this.Procedure.Text = "Procedure";
            this.Procedure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AlarmDiagnosticAvlControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Procedure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAlarmConfirm);
            this.Controls.Add(this.bunifuSeparator1);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.dbFlowLayoutPanel1);
            this.Controls.Add(this.ContentBody);
            this.Controls.Add(this.AlarmStateToggleSwitch);
            this.Controls.Add(this.AlarmState);
            this.Controls.Add(this.Note);
            this.Controls.Add(this.AddNote);
            this.Controls.Add(this.PanelCamera);
            this.Controls.Add(this.AlarmDateTime);
            this.Controls.Add(this.DeviceName);
            this.Controls.Add(this.AlarmLocation);
            this.Controls.Add(this.AlarmType);
            this.Controls.Add(this.AlarmIcon);
            this.Name = "AlarmDiagnosticAvlControl";
            this.Size = new System.Drawing.Size(1920, 950);
            this.dbFlowLayoutPanel1.ResumeLayout(false);
            this.ContentBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AlarmIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAlarmConfirm;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator1;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtNote;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonCancel;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonOK;
        private Common.DBFlowLayoutPanel dbFlowLayoutPanel1;
        private Common.DBFlowLayoutPanel ContentEmail;
        private Bunifu.UI.WinForms.BunifuVScrollBar ScrollEmail;
        private Common.DBFlowLayoutPanel ContentBody;
        private Common.DBFlowLayoutPanel ContentStep;
        private Bunifu.UI.WinForms.BunifuVScrollBar ScrollBar;
        private Bunifu.ToggleSwitch.BunifuToggleSwitch AlarmStateToggleSwitch;
        private System.Windows.Forms.Label AlarmState;
        private System.Windows.Forms.Label Note;
        private System.Windows.Forms.Label AddNote;
        private System.Windows.Forms.Panel PanelCamera;
        private System.Windows.Forms.Label AlarmDateTime;
        private System.Windows.Forms.Label DeviceName;
        private System.Windows.Forms.Label AlarmLocation;
        private System.Windows.Forms.Label AlarmType;
        private System.Windows.Forms.PictureBox AlarmIcon;
        private System.Windows.Forms.Label Procedure;
    }
}
