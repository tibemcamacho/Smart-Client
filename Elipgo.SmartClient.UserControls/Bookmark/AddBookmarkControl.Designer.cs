using Elipgo.SmartClient.Common.Properties;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Bookmark
{
    partial class AddBookmarkControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddBookmarkControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges3 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges4 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this._labelName = new System.Windows.Forms.Label();
            this._textBookmarkName = new Bunifu.Framework.BunifuCustomTextbox();
            this._separatorBookMarkName = new Bunifu.Framework.UI.BunifuSeparator();
            this._buttonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._buttonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._labelBookmark = new System.Windows.Forms.Label();
            this._labelEnd = new System.Windows.Forms.Label();
            this._labelStart = new System.Windows.Forms.Label();
            this._buttonCalendarEndTime = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonCalendarStartTime = new Bunifu.Framework.UI.BunifuImageButton();
            this._labelStartDate = new System.Windows.Forms.Label();
            this._labelEndDate = new System.Windows.Forms.Label();
            this._pictureBoxStartTime = new System.Windows.Forms.PictureBox();
            this._pictureBoxEndTime = new System.Windows.Forms.PictureBox();
            this._labelEndTime = new System.Windows.Forms.Label();
            this._labelStartTime = new System.Windows.Forms.Label();
            this._labelError = new System.Windows.Forms.Label();
            this._panelSelectDate = new System.Windows.Forms.Panel();
            this._panelButtons = new System.Windows.Forms.Panel();
            this._monthCalendar = new System.Windows.Forms.MonthCalendar();
            this._btnApply = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this._timePicker = new System.Windows.Forms.DateTimePicker();
            this._panelStartDateControls = new System.Windows.Forms.Panel();
            this._panelEndDateControls = new System.Windows.Forms.Panel();
            this._panelBookMarkName = new System.Windows.Forms.Panel();
            this._panelButtonsBookMark = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._buttonCalendarEndTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonCalendarStartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxStartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxEndTime)).BeginInit();
            this._panelStartDateControls.SuspendLayout();
            this._panelEndDateControls.SuspendLayout();
            this._panelBookMarkName.SuspendLayout();
            this._panelButtonsBookMark.SuspendLayout();
            this.SuspendLayout();
            // 
            // _labelName
            // 
            this._labelName.AutoSize = true;
            this._labelName.ForeColor = System.Drawing.Color.White;
            this._labelName.Location = new System.Drawing.Point(3, 3);
            this._labelName.Margin = new System.Windows.Forms.Padding(0);
            this._labelName.Name = "_labelName";
            this._labelName.Size = new System.Drawing.Size(35, 13);
            this._labelName.TabIndex = 26;
            this._labelName.Text = "Name";
            this._labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // _textBookmarkName
            // 
            this._textBookmarkName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._textBookmarkName.BorderColor = System.Drawing.Color.SeaGreen;
            this._textBookmarkName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textBookmarkName.ForeColor = System.Drawing.Color.White;
            this._textBookmarkName.Location = new System.Drawing.Point(4, 25);
            this._textBookmarkName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._textBookmarkName.Name = "_textBookmarkName";
            this._textBookmarkName.Size = new System.Drawing.Size(502, 13);
            this._textBookmarkName.TabIndex = 5;
            this._textBookmarkName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBookmarkName_KeyPress);
            // 
            // _separatorBookMarkName
            // 
            this._separatorBookMarkName.BackColor = System.Drawing.Color.Transparent;
            this._separatorBookMarkName.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this._separatorBookMarkName.LineThickness = 1;
            this._separatorBookMarkName.Location = new System.Drawing.Point(4, 27);
            this._separatorBookMarkName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._separatorBookMarkName.Name = "_separatorBookMarkName";
            this._separatorBookMarkName.Size = new System.Drawing.Size(502, 35);
            this._separatorBookMarkName.TabIndex = 24;
            this._separatorBookMarkName.Transparency = 255;
            this._separatorBookMarkName.Vertical = false;
            // 
            // _buttonOK
            // 
            this._buttonOK.AllowToggling = false;
            this._buttonOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._buttonOK.AnimationSpeed = 200;
            this._buttonOK.AutoGenerateColors = false;
            this._buttonOK.BackColor = System.Drawing.Color.Transparent;
            this._buttonOK.BackColor1 = System.Drawing.Color.DodgerBlue;
            this._buttonOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("_buttonOK.BackgroundImage")));
            this._buttonOK.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.ButtonText = "_OK_";
            this._buttonOK.ButtonTextMarginLeft = 0;
            this._buttonOK.ColorContrastOnClick = 45;
            this._buttonOK.ColorContrastOnHover = 45;
            this._buttonOK.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges3.BottomLeft = true;
            borderEdges3.BottomRight = true;
            borderEdges3.TopLeft = true;
            borderEdges3.TopRight = true;
            this._buttonOK.CustomizableEdges = borderEdges3;
            this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this._buttonOK.DisabledBorderColor = System.Drawing.Color.Empty;
            this._buttonOK.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this._buttonOK.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this._buttonOK.Enabled = false;
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
            this._buttonOK.Location = new System.Drawing.Point(27, 12);
            this._buttonOK.Margin = new System.Windows.Forms.Padding(0);
            this._buttonOK.Name = "_buttonOK";
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
            this._buttonOK.Size = new System.Drawing.Size(92, 36);
            this._buttonOK.TabIndex = 6;
            this._buttonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonOK.TextMarginLeft = 0;
            this._buttonOK.UseDefaultRadiusAndThickness = true;
            this._buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.AllowToggling = false;
            this._buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._buttonCancel.AnimationSpeed = 200;
            this._buttonCancel.AutoGenerateColors = false;
            this._buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this._buttonCancel.BackColor1 = System.Drawing.Color.Transparent;
            this._buttonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("_buttonCancel.BackgroundImage")));
            this._buttonCancel.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.ButtonText = "_CANCEL_";
            this._buttonCancel.ButtonTextMarginLeft = 0;
            this._buttonCancel.ColorContrastOnClick = 45;
            this._buttonCancel.ColorContrastOnHover = 45;
            this._buttonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges4.BottomLeft = true;
            borderEdges4.BottomRight = true;
            borderEdges4.TopLeft = true;
            borderEdges4.TopRight = true;
            this._buttonCancel.CustomizableEdges = borderEdges4;
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
            this._buttonCancel.Location = new System.Drawing.Point(152, 12);
            this._buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._buttonCancel.Name = "_buttonCancel";
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
            this._buttonCancel.Size = new System.Drawing.Size(92, 36);
            this._buttonCancel.TabIndex = 7;
            this._buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonCancel.TextMarginLeft = 0;
            this._buttonCancel.UseDefaultRadiusAndThickness = true;
            this._buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // _labelBookmark
            // 
            this._labelBookmark.AutoSize = true;
            this._labelBookmark.ForeColor = System.Drawing.Color.White;
            this._labelBookmark.Location = new System.Drawing.Point(8, 20);
            this._labelBookmark.Margin = new System.Windows.Forms.Padding(0);
            this._labelBookmark.Name = "_labelBookmark";
            this._labelBookmark.Size = new System.Drawing.Size(95, 13);
            this._labelBookmark.TabIndex = 29;
            this._labelBookmark.Text = "Generar bookmark";
            this._labelBookmark.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // _labelEnd
            // 
            this._labelEnd.AutoSize = true;
            this._labelEnd.ForeColor = System.Drawing.Color.White;
            this._labelEnd.Location = new System.Drawing.Point(4, 3);
            this._labelEnd.Margin = new System.Windows.Forms.Padding(0);
            this._labelEnd.Name = "_labelEnd";
            this._labelEnd.Size = new System.Drawing.Size(21, 13);
            this._labelEnd.TabIndex = 46;
            this._labelEnd.Text = "Fin";
            this._labelEnd.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // _labelStart
            // 
            this._labelStart.AutoSize = true;
            this._labelStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            this._labelStart.Location = new System.Drawing.Point(3, 3);
            this._labelStart.Margin = new System.Windows.Forms.Padding(0);
            this._labelStart.Name = "_labelStart";
            this._labelStart.Size = new System.Drawing.Size(32, 13);
            this._labelStart.TabIndex = 9;
            this._labelStart.Text = "Inicio";
            this._labelStart.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // _buttonCalendarEndTime
            // 
            this._buttonCalendarEndTime.Image = ((System.Drawing.Image)(resources.GetObject("_buttonCalendarEndTime.Image")));
            this._buttonCalendarEndTime.ImageActive = null;
            this._buttonCalendarEndTime.Location = new System.Drawing.Point(9, 26);
            this._buttonCalendarEndTime.Margin = new System.Windows.Forms.Padding(0);
            this._buttonCalendarEndTime.Name = "_buttonCalendarEndTime";
            this._buttonCalendarEndTime.Size = new System.Drawing.Size(24, 24);
            this._buttonCalendarEndTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonCalendarEndTime.TabIndex = 0;
            this._buttonCalendarEndTime.TabStop = false;
            this._buttonCalendarEndTime.Zoom = 10;
            this._buttonCalendarEndTime.Click += new System.EventHandler(this.ButtonCalendarEndTime_Click);
            // 
            // _buttonCalendarStartTime
            // 
            this._buttonCalendarStartTime.Image = ((System.Drawing.Image)(resources.GetObject("_buttonCalendarStartTime.Image")));
            this._buttonCalendarStartTime.ImageActive = null;
            this._buttonCalendarStartTime.Location = new System.Drawing.Point(6, 26);
            this._buttonCalendarStartTime.Name = "_buttonCalendarStartTime";
            this._buttonCalendarStartTime.Size = new System.Drawing.Size(24, 24);
            this._buttonCalendarStartTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonCalendarStartTime.TabIndex = 0;
            this._buttonCalendarStartTime.TabStop = false;
            this._buttonCalendarStartTime.Zoom = 10;
            this._buttonCalendarStartTime.Click += new System.EventHandler(this.ButtonCalendarStartTime_Click);
            // 
            // _labelStartDate
            // 
            this._labelStartDate.AutoSize = true;
            this._labelStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelStartDate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this._labelStartDate.Location = new System.Drawing.Point(36, 31);
            this._labelStartDate.Name = "_labelStartDate";
            this._labelStartDate.Size = new System.Drawing.Size(81, 16);
            this._labelStartDate.TabIndex = 47;
            this._labelStartDate.Text = "30/01/2025";
            // 
            // _labelEndDate
            // 
            this._labelEndDate.AutoSize = true;
            this._labelEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelEndDate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this._labelEndDate.Location = new System.Drawing.Point(36, 31);
            this._labelEndDate.Name = "_labelEndDate";
            this._labelEndDate.Size = new System.Drawing.Size(81, 16);
            this._labelEndDate.TabIndex = 48;
            this._labelEndDate.Text = "01/02/2025";
            // 
            // _pictureBoxStartTime
            // 
            this._pictureBoxStartTime.Image = ((System.Drawing.Image)(resources.GetObject("_pictureBoxStartTime.Image")));
            this._pictureBoxStartTime.Location = new System.Drawing.Point(140, 26);
            this._pictureBoxStartTime.Name = "_pictureBoxStartTime";
            this._pictureBoxStartTime.Size = new System.Drawing.Size(24, 24);
            this._pictureBoxStartTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBoxStartTime.TabIndex = 50;
            this._pictureBoxStartTime.TabStop = false;
            // 
            // _pictureBoxEndTime
            // 
            this._pictureBoxEndTime.Image = ((System.Drawing.Image)(resources.GetObject("_pictureBoxEndTime.Image")));
            this._pictureBoxEndTime.Location = new System.Drawing.Point(136, 26);
            this._pictureBoxEndTime.Name = "_pictureBoxEndTime";
            this._pictureBoxEndTime.Size = new System.Drawing.Size(24, 24);
            this._pictureBoxEndTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBoxEndTime.TabIndex = 51;
            this._pictureBoxEndTime.TabStop = false;
            // 
            // _labelEndTime
            // 
            this._labelEndTime.AutoSize = true;
            this._labelEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelEndTime.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this._labelEndTime.Location = new System.Drawing.Point(171, 31);
            this._labelEndTime.Name = "_labelEndTime";
            this._labelEndTime.Size = new System.Drawing.Size(63, 16);
            this._labelEndTime.TabIndex = 52;
            this._labelEndTime.Text = "13:26:31";
            // 
            // _labelStartTime
            // 
            this._labelStartTime.AutoSize = true;
            this._labelStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelStartTime.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this._labelStartTime.Location = new System.Drawing.Point(175, 31);
            this._labelStartTime.Name = "_labelStartTime";
            this._labelStartTime.Size = new System.Drawing.Size(63, 16);
            this._labelStartTime.TabIndex = 53;
            this._labelStartTime.Text = "13:26:31";
            // 
            // _labelError
            // 
            this._labelError.AutoSize = true;
            this._labelError.ForeColor = System.Drawing.Color.White;
            this._labelError.Location = new System.Drawing.Point(5, 48);
            this._labelError.Margin = new System.Windows.Forms.Padding(0);
            this._labelError.Name = "";
            this._labelError.Size = new System.Drawing.Size(28, 13);
            this._labelError.TabIndex = 54;
            this._labelError.Text = "";
            this._labelError.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // _panelSelectDate
            // 
            this._panelSelectDate.Location = new System.Drawing.Point(0, 0);
            this._panelSelectDate.Name = "_panelSelectDate";
            this._panelSelectDate.Size = new System.Drawing.Size(200, 100);
            this._panelSelectDate.TabIndex = 0;
            // 
            // _panelButtons
            // 
            this._panelButtons.Location = new System.Drawing.Point(0, 0);
            this._panelButtons.Name = "_panelButtons";
            this._panelButtons.Size = new System.Drawing.Size(200, 100);
            this._panelButtons.TabIndex = 0;
            // 
            // _monthCalendar
            // 
            this._monthCalendar.Location = new System.Drawing.Point(0, 0);
            this._monthCalendar.Name = "_monthCalendar";
            this._monthCalendar.TabIndex = 0;
            // 
            // _btnApply
            // 
            this._btnApply.Location = new System.Drawing.Point(0, 0);
            this._btnApply.Text = Resources.ApplyButton;
            this._btnApply.Name = "_btnApply";
            this._btnApply.Size = new System.Drawing.Size(75, 23);
            this._btnApply.TabIndex = 0;
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(0, 0);
            this._btnClose.Text = Resources.ButtonClose;
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(75, 23);
            this._btnClose.TabIndex = 0;
            // 
            // _timePicker
            // 
            this._timePicker.Location = new System.Drawing.Point(0, 0);
            this._timePicker.Name = "_timePicker";
            this._timePicker.Size = new System.Drawing.Size(200, 20);
            this._timePicker.TabIndex = 0;
            // 
            // _panelStartDateControls
            // 
            this._panelStartDateControls.Controls.Add(this._pictureBoxStartTime);
            this._panelStartDateControls.Controls.Add(this._buttonCalendarStartTime);
            this._panelStartDateControls.Controls.Add(this._labelStartTime);
            this._panelStartDateControls.Controls.Add(this._labelStartDate);
            this._panelStartDateControls.Controls.Add(this._labelStart);
            this._panelStartDateControls.Location = new System.Drawing.Point(11, 45);
            this._panelStartDateControls.Name = "_panelStartDateControls";
            this._panelStartDateControls.Size = new System.Drawing.Size(243, 66);
            this._panelStartDateControls.TabIndex = 55;
            // 
            // _panelEndDateControls
            // 
            this._panelEndDateControls.Controls.Add(this._pictureBoxEndTime);
            this._panelEndDateControls.Controls.Add(this._buttonCalendarEndTime);
            this._panelEndDateControls.Controls.Add(this._labelEndDate);
            this._panelEndDateControls.Controls.Add(this._labelEnd);
            this._panelEndDateControls.Controls.Add(this._labelEndTime);
            this._panelEndDateControls.Location = new System.Drawing.Point(303, 45);
            this._panelEndDateControls.Name = "_panelEndDateControls";
            this._panelEndDateControls.Size = new System.Drawing.Size(243, 66);
            this._panelEndDateControls.TabIndex = 56;
            // 
            // _panelBookMarkName
            // 
            this._panelBookMarkName.Controls.Add(this._labelError);
            this._panelBookMarkName.Controls.Add(this._textBookmarkName);
            this._panelBookMarkName.Controls.Add(this._separatorBookMarkName);
            this._panelBookMarkName.Controls.Add(this._labelName);
            this._panelBookMarkName.Location = new System.Drawing.Point(572, 45);
            this._panelBookMarkName.Name = "_panelBookMarkName";
            this._panelBookMarkName.Size = new System.Drawing.Size(510, 80);
            this._panelBookMarkName.TabIndex = 57;
            // 
            // _panelButtonsBookMark
            // 
            this._panelButtonsBookMark.Controls.Add(this._buttonOK);
            this._panelButtonsBookMark.Controls.Add(this._buttonCancel);
            this._panelButtonsBookMark.Location = new System.Drawing.Point(1105, 45);
            this._panelButtonsBookMark.Name = "_panelButtonsBookMark";
            this._panelButtonsBookMark.Size = new System.Drawing.Size(274, 66);
            this._panelButtonsBookMark.TabIndex = 57;
            // 
            // AddBookmarkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.Controls.Add(this._panelButtonsBookMark);
            this.Controls.Add(this._panelBookMarkName);
            this.Controls.Add(this._panelEndDateControls);
            this.Controls.Add(this._panelStartDateControls);
            this.Controls.Add(this._labelBookmark);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AddBookmarkControl";
            this.Size = new System.Drawing.Size(1393, 150);
            ((System.ComponentModel.ISupportInitialize)(this._buttonCalendarEndTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonCalendarStartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxStartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxEndTime)).EndInit();
            this._panelStartDateControls.ResumeLayout(false);
            this._panelStartDateControls.PerformLayout();
            this._panelEndDateControls.ResumeLayout(false);
            this._panelEndDateControls.PerformLayout();
            this._panelBookMarkName.ResumeLayout(false);
            this._panelBookMarkName.PerformLayout();
            this._panelButtonsBookMark.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label _labelName;
        private Bunifu.Framework.BunifuCustomTextbox _textBookmarkName;
        private Bunifu.Framework.UI.BunifuSeparator _separatorBookMarkName;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonOK;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonCancel;
        private System.Windows.Forms.Label _labelBookmark;
        private System.Windows.Forms.Label _labelEnd;
        private System.Windows.Forms.Label _labelStart;
        private Bunifu.Framework.UI.BunifuImageButton _buttonCalendarEndTime;
        private Bunifu.Framework.UI.BunifuImageButton _buttonCalendarStartTime;
        private System.Windows.Forms.Label _labelStartDate;
        private System.Windows.Forms.Label _labelEndDate;
        private System.Windows.Forms.PictureBox _pictureBoxStartTime;
        private System.Windows.Forms.PictureBox _pictureBoxEndTime;
        private System.Windows.Forms.Label _labelEndTime;
        private System.Windows.Forms.Label _labelStartTime;
        private Label _labelError;
        private Panel _panelSelectDate;
        private Panel _panelButtons;
        private MonthCalendar _monthCalendar;
        private Button _btnApply;
        private Button _btnClose;
        private DateTimePicker _timePicker;
        private Panel _panelStartDateControls;
        private Panel _panelEndDateControls;
        private Panel _panelBookMarkName;
        private Panel _panelButtonsBookMark;
    }
}
