using Elipgo.SmartClient.UserControls.ElementContainer;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class AlarmDiagnosticBase
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
            if (this._panelCamera.Controls.Count > 0)
            {
                foreach (var it in this._panelCamera.Controls)
                {
                    if (it is ElementSnapshotControl)
                    {
                        (it as ElementSnapshotControl).Dispose();
                    }
                    else if (it is ElementCameraControl)
                    {
                        (it as ElementCameraControl).Dispose();
                    }
                    //else if ( it is CefSharp.WinForms.ChromiumWebBrowser)
                    //{
                    //    (it as CefSharp.WinForms.ChromiumWebBrowser).Dispose();
                    //}
                }
            }
            //if(this.browser != null)
            //{
            //    this.browser.Dispose();
            //}
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmDiagnosticBase));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.ToggleSwitch.ToggleState toggleState1 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState2 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState3 = new Bunifu.ToggleSwitch.ToggleState();
            this._alarmConfirm = new System.Windows.Forms.Label();
            this._bunifuSeparator1 = new Bunifu.Framework.UI.BunifuSeparator();
            this._txtNote = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this._buttonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._buttonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._alarmStateToggleSwitch = new Bunifu.ToggleSwitch.BunifuToggleSwitch();
            this._alarmState = new System.Windows.Forms.Label();
            this._addNote = new System.Windows.Forms.Label();
            this._panelCamera = new System.Windows.Forms.Panel();
            this._alarmDateTime = new System.Windows.Forms.Label();
            this._deviceName = new System.Windows.Forms.Label();
            this._alarmLocation = new System.Windows.Forms.Label();
            this._alarmType = new System.Windows.Forms.Label();
            this._alarmIcon = new System.Windows.Forms.PictureBox();
            this._procedure = new System.Windows.Forms.Label();
            this._alarmMessage = new System.Windows.Forms.Label();
            this._contentStep = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this._contentBody = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this._scrollBar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this._alarmIcon)).BeginInit();
            this._contentBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // AlarmConfirm
            // 
            this._alarmConfirm.BackColor = System.Drawing.Color.Transparent;
            this._alarmConfirm.ForeColor = System.Drawing.Color.White;
            this._alarmConfirm.Location = new System.Drawing.Point(1461, 770);
            this._alarmConfirm.Name = "AlarmConfirm";
            this._alarmConfirm.Size = new System.Drawing.Size(200, 24);
            this._alarmConfirm.TabIndex = 54;
            this._alarmConfirm.Text = "Alarma confirmada";
            this._alarmConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bunifuSeparator1
            // 
            this._bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this._bunifuSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this._bunifuSeparator1.LineThickness = 1;
            this._bunifuSeparator1.Location = new System.Drawing.Point(1461, 697);
            this._bunifuSeparator1.Name = "bunifuSeparator1";
            this._bunifuSeparator1.Size = new System.Drawing.Size(417, 35);
            this._bunifuSeparator1.TabIndex = 53;
            this._bunifuSeparator1.Transparency = 255;
            this._bunifuSeparator1.Vertical = false;
            // 
            // txtNote
            // 
            this._txtNote.AcceptsReturn = false;
            this._txtNote.AcceptsTab = false;
            this._txtNote.AnimationSpeed = 200;
            this._txtNote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this._txtNote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this._txtNote.BackColor = System.Drawing.Color.Transparent;
            this._txtNote.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtNote.BackgroundImage")));
            this._txtNote.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this._txtNote.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this._txtNote.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._txtNote.BorderColorIdle = System.Drawing.Color.Silver;
            this._txtNote.BorderRadius = 20;
            this._txtNote.BorderThickness = 1;
            this._txtNote.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this._txtNote.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtNote.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._txtNote.DefaultText = "";
            this._txtNote.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this._txtNote.ForeColor = System.Drawing.Color.Silver;
            this._txtNote.HideSelection = true;
            this._txtNote.IconLeft = null;
            this._txtNote.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this._txtNote.IconPadding = 10;
            this._txtNote.IconRight = null;
            this._txtNote.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this._txtNote.Lines = new string[0];
            this._txtNote.Location = new System.Drawing.Point(1461, 282);
            this._txtNote.MaxLength = 32767;
            this._txtNote.MinimumSize = new System.Drawing.Size(1, 1);
            this._txtNote.Modified = false;
            this._txtNote.Multiline = true;
            this._txtNote.Name = "txtNote";
            stateProperties1.BorderColor = System.Drawing.Color.Silver;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this._txtNote.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this._txtNote.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.Silver;
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this._txtNote.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.Silver;
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            stateProperties4.ForeColor = System.Drawing.Color.Silver;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this._txtNote.OnIdleState = stateProperties4;
            this._txtNote.PasswordChar = '\0';
            this._txtNote.PlaceholderForeColor = System.Drawing.Color.Silver;
            this._txtNote.PlaceholderText = Elipgo.SmartClient.Common.Properties.Resources.AddObservation;
            this._txtNote.ReadOnly = false;
            this._txtNote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._txtNote.SelectedText = "";
            this._txtNote.SelectionLength = 0;
            this._txtNote.SelectionStart = 0;
            this._txtNote.ShortcutsEnabled = true;
            this._txtNote.Size = new System.Drawing.Size(417, 392);
            this._txtNote.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Bunifu;
            this._txtNote.TabIndex = 52;
            this._txtNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this._txtNote.TextMarginBottom = 0;
            this._txtNote.TextMarginLeft = 5;
            this._txtNote.TextMarginTop = 0;
            this._txtNote.TextPlaceholder = Elipgo.SmartClient.Common.Properties.Resources.AddObservation;
            this._txtNote.UseSystemPasswordChar = false;
            this._txtNote.WordWrap = true;
            // 
            // ButtonCancel
            // 
            this._buttonCancel.AllowToggling = false;
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.AnimationSpeed = 200;
            this._buttonCancel.AutoGenerateColors = false;
            this._buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this._buttonCancel.BackColor1 = System.Drawing.Color.Transparent;
            this._buttonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonCancel.BackgroundImage")));
            this._buttonCancel.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.ButtonText = Elipgo.SmartClient.Common.Properties.Resources.ButtonCancel;
            this._buttonCancel.ButtonTextMarginLeft = 0;
            this._buttonCancel.ColorContrastOnClick = 45;
            this._buttonCancel.ColorContrastOnHover = 45;
            this._buttonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this._buttonCancel.CustomizableEdges = borderEdges1;
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.DisabledBorderColor = System.Drawing.Color.Empty;
            this._buttonCancel.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this._buttonCancel.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this._buttonCancel.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this._buttonCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._buttonCancel.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonCancel.IconMarginLeft = 11;
            this._buttonCancel.IconPadding = 10;
            this._buttonCancel.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonCancel.IdleBorderColor = System.Drawing.Color.DimGray;
            this._buttonCancel.IdleBorderRadius = 30;
            this._buttonCancel.IdleBorderThickness = 1;
            this._buttonCancel.IdleFillColor = System.Drawing.Color.Transparent;
            this._buttonCancel.IdleIconLeftImage = null;
            this._buttonCancel.IdleIconRightImage = null;
            this._buttonCancel.IndicateFocus = false;
            this._buttonCancel.Location = new System.Drawing.Point(1682, 905);
            this._buttonCancel.Name = "ButtonCancel";
            this._buttonCancel.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonCancel.onHoverState.BorderRadius = 30;
            this._buttonCancel.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.onHoverState.BorderThickness = 1;
            this._buttonCancel.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonCancel.onHoverState.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.onHoverState.IconLeftImage = null;
            this._buttonCancel.onHoverState.IconRightImage = null;
            this._buttonCancel.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this._buttonCancel.OnIdleState.BorderRadius = 30;
            this._buttonCancel.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.OnIdleState.BorderThickness = 1;
            this._buttonCancel.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this._buttonCancel.OnIdleState.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.OnIdleState.IconLeftImage = null;
            this._buttonCancel.OnIdleState.IconRightImage = null;
            this._buttonCancel.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonCancel.OnPressedState.BorderRadius = 30;
            this._buttonCancel.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.OnPressedState.BorderThickness = 1;
            this._buttonCancel.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonCancel.OnPressedState.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.OnPressedState.IconLeftImage = null;
            this._buttonCancel.OnPressedState.IconRightImage = null;
            this._buttonCancel.Size = new System.Drawing.Size(92, 37);
            this._buttonCancel.TabIndex = 51;
            this._buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonCancel.TextMarginLeft = 0;
            this._buttonCancel.UseDefaultRadiusAndThickness = true;
            // 
            // ButtonOK
            // 
            this._buttonOK.AllowToggling = false;
            this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonOK.AnimationSpeed = 200;
            this._buttonOK.AutoGenerateColors = false;
            this._buttonOK.BackColor = System.Drawing.Color.Transparent;
            this._buttonOK.BackColor1 = System.Drawing.Color.DodgerBlue;
            this._buttonOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this._buttonOK.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.ButtonText = Elipgo.SmartClient.Common.Properties.Resources.ButtonOK;
            this._buttonOK.ButtonTextMarginLeft = 0;
            this._buttonOK.ColorContrastOnClick = 45;
            this._buttonOK.ColorContrastOnHover = 45;
            this._buttonOK.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this._buttonOK.CustomizableEdges = borderEdges2;
            this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this._buttonOK.DisabledBorderColor = System.Drawing.Color.Empty;
            this._buttonOK.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this._buttonOK.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this._buttonOK.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this._buttonOK.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._buttonOK.ForeColor = System.Drawing.Color.White;
            this._buttonOK.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonOK.IconMarginLeft = 11;
            this._buttonOK.IconPadding = 10;
            this._buttonOK.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonOK.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.IdleBorderRadius = 30;
            this._buttonOK.IdleBorderThickness = 1;
            this._buttonOK.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.IdleIconLeftImage = null;
            this._buttonOK.IdleIconRightImage = null;
            this._buttonOK.IndicateFocus = false;
            this._buttonOK.Location = new System.Drawing.Point(1790, 905);
            this._buttonOK.Name = "ButtonOK";
            this._buttonOK.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonOK.onHoverState.BorderRadius = 30;
            this._buttonOK.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.onHoverState.BorderThickness = 1;
            this._buttonOK.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonOK.onHoverState.ForeColor = System.Drawing.Color.White;
            this._buttonOK.onHoverState.IconLeftImage = null;
            this._buttonOK.onHoverState.IconRightImage = null;
            this._buttonOK.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.OnIdleState.BorderRadius = 30;
            this._buttonOK.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.OnIdleState.BorderThickness = 1;
            this._buttonOK.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.OnIdleState.ForeColor = System.Drawing.Color.White;
            this._buttonOK.OnIdleState.IconLeftImage = null;
            this._buttonOK.OnIdleState.IconRightImage = null;
            this._buttonOK.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonOK.OnPressedState.BorderRadius = 30;
            this._buttonOK.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.OnPressedState.BorderThickness = 1;
            this._buttonOK.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonOK.OnPressedState.ForeColor = System.Drawing.Color.White;
            this._buttonOK.OnPressedState.IconLeftImage = null;
            this._buttonOK.OnPressedState.IconRightImage = null;
            this._buttonOK.Size = new System.Drawing.Size(92, 37);
            this._buttonOK.TabIndex = 50;
            this._buttonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonOK.TextMarginLeft = 0;
            this._buttonOK.UseDefaultRadiusAndThickness = true;
            // 
            // AlarmStateToggleSwitch
            // 
            this._alarmStateToggleSwitch.Animation = 5;
            this._alarmStateToggleSwitch.BackColor = System.Drawing.Color.Transparent;
            this._alarmStateToggleSwitch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AlarmStateToggleSwitch.BackgroundImage")));
            this._alarmStateToggleSwitch.Checked = true;
            this._alarmStateToggleSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this._alarmStateToggleSwitch.InnerCirclePadding = 3;
            this._alarmStateToggleSwitch.Location = new System.Drawing.Point(1838, 776);
            this._alarmStateToggleSwitch.Name = "AlarmStateToggleSwitch";
            this._alarmStateToggleSwitch.Size = new System.Drawing.Size(35, 18);
            this._alarmStateToggleSwitch.TabIndex = 47;
            toggleState1.BackColor = System.Drawing.Color.Empty;
            toggleState1.BackColorInner = System.Drawing.Color.Empty;
            toggleState1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            toggleState1.BorderColorInner = System.Drawing.Color.Empty;
            toggleState1.BorderRadius = 1;
            toggleState1.BorderRadiusInner = 1;
            toggleState1.BorderThickness = 1;
            toggleState1.BorderThicknessInner = 1;
            this._alarmStateToggleSwitch.ToggleStateDisabled = toggleState1;
            toggleState2.BackColor = System.Drawing.Color.Empty;
            toggleState2.BackColorInner = System.Drawing.Color.Empty;
            toggleState2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            toggleState2.BorderColorInner = System.Drawing.Color.Empty;
            toggleState2.BorderRadius = 1;
            toggleState2.BorderRadiusInner = 1;
            toggleState2.BorderThickness = 1;
            toggleState2.BorderThicknessInner = 1;
            this._alarmStateToggleSwitch.ToggleStateOff = toggleState2;
            toggleState3.BackColor = System.Drawing.Color.DodgerBlue;
            toggleState3.BackColorInner = System.Drawing.Color.White;
            toggleState3.BorderColor = System.Drawing.Color.DodgerBlue;
            toggleState3.BorderColorInner = System.Drawing.Color.White;
            toggleState3.BorderRadius = 17;
            toggleState3.BorderRadiusInner = 11;
            toggleState3.BorderThickness = 1;
            toggleState3.BorderThicknessInner = 1;
            this._alarmStateToggleSwitch.ToggleStateOn = toggleState3;
            this._alarmStateToggleSwitch.Value = true;
            // 
            // AlarmState
            // 
            this._alarmState.BackColor = System.Drawing.Color.Transparent;
            this._alarmState.ForeColor = System.Drawing.Color.White;
            this._alarmState.Location = new System.Drawing.Point(1460, 730);
            this._alarmState.Name = "AlarmState";
            this._alarmState.Size = new System.Drawing.Size(200, 24);
            this._alarmState.TabIndex = 44;
            this._alarmState.Text = "State alarm";
            this._alarmState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddNote
            // 
            this._addNote.BackColor = System.Drawing.Color.Transparent;
            this._addNote.ForeColor = System.Drawing.Color.White;
            this._addNote.Location = new System.Drawing.Point(1461, 247);
            this._addNote.Name = "AddNote";
            this._addNote.Size = new System.Drawing.Size(200, 21);
            this._addNote.TabIndex = 46;
            this._addNote.Text = "Add note";
            this._addNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelCamera
            // 
            this._panelCamera.BackColor = System.Drawing.Color.Transparent;
            this._panelCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panelCamera.Location = new System.Drawing.Point(5, 158);
            this._panelCamera.Name = "PanelCamera";
            this._panelCamera.Size = new System.Drawing.Size(1392, 791);
            this._panelCamera.TabIndex = 43;
            // 
            // AlarmDateTime
            // 
            this._alarmDateTime.BackColor = System.Drawing.Color.Transparent;
            this._alarmDateTime.ForeColor = System.Drawing.Color.White;
            this._alarmDateTime.Location = new System.Drawing.Point(196, 53);
            this._alarmDateTime.Name = "AlarmDateTime";
            this._alarmDateTime.Size = new System.Drawing.Size(200, 21);
            this._alarmDateTime.TabIndex = 42;
            this._alarmDateTime.Text = "DateTime";
            this._alarmDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeviceName
            // 
            this._deviceName.BackColor = System.Drawing.Color.Transparent;
            this._deviceName.ForeColor = System.Drawing.Color.White;
            this._deviceName.Location = new System.Drawing.Point(5, 53);
            this._deviceName.Name = "DeviceName";
            this._deviceName.Size = new System.Drawing.Size(200, 21);
            this._deviceName.TabIndex = 38;
            this._deviceName.Text = "Device Name";
            this._deviceName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AlarmLocation
            // 
            this._alarmLocation.BackColor = System.Drawing.Color.Transparent;
            this._alarmLocation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._alarmLocation.ForeColor = System.Drawing.Color.White;
            this._alarmLocation.Location = new System.Drawing.Point(54, 17);
            this._alarmLocation.Name = "AlarmLocation";
            this._alarmLocation.Size = new System.Drawing.Size(300, 16);
            this._alarmLocation.TabIndex = 39;
            this._alarmLocation.Text = "Location";
            // 
            // AlarmType
            // 
            this._alarmType.BackColor = System.Drawing.Color.Transparent;
            this._alarmType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._alarmType.ForeColor = System.Drawing.Color.White;
            this._alarmType.Location = new System.Drawing.Point(54, -1);
            this._alarmType.Name = "AlarmType";
            this._alarmType.Size = new System.Drawing.Size(300, 16);
            this._alarmType.TabIndex = 40;
            this._alarmType.Text = "AlarmType";
            // 
            // AlarmIcon
            // 
            this._alarmIcon.BackColor = System.Drawing.Color.Transparent;
            this._alarmIcon.Location = new System.Drawing.Point(5, 0);
            this._alarmIcon.Margin = new System.Windows.Forms.Padding(0);
            this._alarmIcon.Name = "AlarmIcon";
            this._alarmIcon.Size = new System.Drawing.Size(32, 32);
            this._alarmIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._alarmIcon.TabIndex = 41;
            this._alarmIcon.TabStop = false;
            // 
            // Procedure
            // 
            this._procedure.BackColor = System.Drawing.Color.Transparent;
            this._procedure.ForeColor = System.Drawing.Color.White;
            this._procedure.Location = new System.Drawing.Point(1461, 50);
            this._procedure.Name = "Procedure";
            this._procedure.Size = new System.Drawing.Size(200, 24);
            this._procedure.TabIndex = 56;
            this._procedure.Text = "Procedure";
            this._procedure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AlarmMessage
            // 
            this._alarmMessage.AutoSize = true;
            this._alarmMessage.BackColor = System.Drawing.Color.Transparent;
            this._alarmMessage.ForeColor = System.Drawing.Color.White;
            this._alarmMessage.Location = new System.Drawing.Point(5, 83);
            //this._alarmMessage.MaximumSize = new System.Drawing.Size(800, 42);
            //this._alarmMessage.MinimumSize = new System.Drawing.Size(800, 21);
            this._alarmMessage.Name = "AlarmMessage";
            //this._alarmMessage.Size = new System.Drawing.Size(980, 21);
            this._alarmMessage.TabIndex = 57;
            this._alarmMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ContentStep
            // 
            this._contentStep.ForeColor = System.Drawing.Color.White;
            this._contentStep.Location = new System.Drawing.Point(1461, 91);
            this._contentStep.Name = "ContentStep";
            this._contentStep.Size = new System.Drawing.Size(389, 140);
            this._contentStep.TabIndex = 12;
            // 
            // ContentBody
            // 
            this._contentBody.Controls.Add(this._scrollBar);
            this._contentBody.Location = new System.Drawing.Point(1461, 91);
            this._contentBody.Name = "ContentBody";
            this._contentBody.Size = new System.Drawing.Size(421, 140);
            this._contentBody.TabIndex = 48;
            // 
            // ScrollBar
            // 
            this._scrollBar.AllowCursorChanges = true;
            this._scrollBar.AllowHomeEndKeysDetection = false;
            this._scrollBar.AllowIncrementalClickMoves = true;
            this._scrollBar.AllowMouseDownEffects = true;
            this._scrollBar.AllowMouseHoverEffects = true;
            this._scrollBar.AllowScrollingAnimations = true;
            this._scrollBar.AllowScrollKeysDetection = true;
            this._scrollBar.AllowScrollOptionsMenu = true;
            this._scrollBar.AllowShrinkingOnFocusLost = false;
            this._scrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._scrollBar.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._scrollBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ScrollBar.BackgroundImage")));
            this._scrollBar.BindingContainer = null;
            this._scrollBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._scrollBar.BorderRadius = 1;
            this._scrollBar.BorderThickness = 1;
            this._scrollBar.DurationBeforeShrink = 2000;
            this._scrollBar.LargeChange = 10;
            this._scrollBar.Location = new System.Drawing.Point(4, 4);
            this._scrollBar.Margin = new System.Windows.Forms.Padding(4);
            this._scrollBar.Maximum = 100;
            this._scrollBar.Minimum = 0;
            this._scrollBar.MinimumThumbLength = 18;
            this._scrollBar.Name = "ScrollBar";
            this._scrollBar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this._scrollBar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this._scrollBar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this._scrollBar.ScrollBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._scrollBar.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._scrollBar.ShrinkSizeLimit = 3;
            this._scrollBar.Size = new System.Drawing.Size(13, 0);
            this._scrollBar.SmallChange = 1;
            this._scrollBar.TabIndex = 8;
            this._scrollBar.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this._scrollBar.ThumbLength = 18;
            this._scrollBar.ThumbMargin = 1;
            this._scrollBar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this._scrollBar.Value = 0;
            this._scrollBar.Visible = false;
            // 
            // AlarmDiagnosticBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._alarmStateToggleSwitch);
            this.Controls.Add(this._contentStep);
            this.Controls.Add(this._alarmMessage);
            this.Controls.Add(this._procedure);
            this.Controls.Add(this._alarmConfirm);
            this.Controls.Add(this._bunifuSeparator1);
            this.Controls.Add(this._txtNote);
            this.Controls.Add(this._buttonCancel);
            this.Controls.Add(this._buttonOK);
            this.Controls.Add(this._contentBody);
            this.Controls.Add(this._alarmState);
            this.Controls.Add(this._addNote);
            this.Controls.Add(this._panelCamera);
            this.Controls.Add(this._alarmDateTime);
            this.Controls.Add(this._deviceName);
            this.Controls.Add(this._alarmLocation);
            this.Controls.Add(this._alarmType);
            this.Controls.Add(this._alarmIcon);
            this.Name = "AlarmDiagnosticBase";
            this.Size = new System.Drawing.Size(1920, 950);
            this.Resize += new System.EventHandler(this.AlarmDiagnosticBase_Resize);
            ((System.ComponentModel.ISupportInitialize)(this._alarmIcon)).EndInit();
            this._contentBody.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _alarmConfirm;
        private Bunifu.Framework.UI.BunifuSeparator _bunifuSeparator1;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox _txtNote;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonCancel;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonOK;
        private Common.DBFlowLayoutPanel _contentBody;
        private Common.DBFlowLayoutPanel _contentStep;
        private Bunifu.UI.WinForms.BunifuVScrollBar _scrollBar;
        private Bunifu.ToggleSwitch.BunifuToggleSwitch _alarmStateToggleSwitch;
        private System.Windows.Forms.Label _alarmState;
        private System.Windows.Forms.Label _addNote;
        private System.Windows.Forms.Panel _panelCamera;
        private System.Windows.Forms.Label _alarmDateTime;
        private System.Windows.Forms.Label _deviceName;
        private System.Windows.Forms.Label _alarmLocation;
        private System.Windows.Forms.Label _alarmType;
        private System.Windows.Forms.PictureBox _alarmIcon;
        private System.Windows.Forms.Label _procedure;
        private System.Windows.Forms.Label _alarmMessage;
    }
}
