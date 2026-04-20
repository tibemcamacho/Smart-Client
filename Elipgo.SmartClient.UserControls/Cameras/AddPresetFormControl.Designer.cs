using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.Cameras
{
    partial class AddPresetFormControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPresetFormControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.ButtonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.LabelTitle = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.LabelName = new System.Windows.Forms.Label();
            this.TextBoxPresetName = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.errorManager = new System.Windows.Forms.ErrorProvider(this.components);
            this.DropdownPreset = new Bunifu.UI.WinForms.BunifuDropdown();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.BackColor = System.Drawing.Color.Black;
            this.PanelHeader.Controls.Add(this.ButtonClose);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(409, 26);
            this.PanelHeader.TabIndex = 9;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this.ButtonClose.ImageActive = null;
            this.ButtonClose.Location = new System.Drawing.Point(379, 3);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(22, 20);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Zoom = 10;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonCancel_Click);
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
            this.ButtonCancel.ButtonText = "_CANCEL_";
            this.ButtonCancel.ButtonTextMarginLeft = 0;
            this.ButtonCancel.ColorContrastOnClick = 45;
            this.ButtonCancel.ColorContrastOnHover = 45;
            this.ButtonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.ButtonCancel.CustomizableEdges = borderEdges1;
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
            this.ButtonCancel.Location = new System.Drawing.Point(194, 151);
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
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonCancel.TextMarginLeft = 0;
            this.ButtonCancel.UseDefaultRadiusAndThickness = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
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
            this.ButtonOK.ButtonText = "_OK_";
            this.ButtonOK.ButtonTextMarginLeft = 0;
            this.ButtonOK.ColorContrastOnClick = 45;
            this.ButtonOK.ColorContrastOnHover = 45;
            this.ButtonOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOK.CustomizableEdges = borderEdges1;
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
            this.ButtonOK.Location = new System.Drawing.Point(292, 151);
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
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonOK.TextMarginLeft = 0;
            this.ButtonOK.UseDefaultRadiusAndThickness = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // LabelTitle
            // 
            this.LabelTitle.AutoEllipsis = false;
            this.LabelTitle.Cursor = null;
            this.LabelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTitle.ForeColor = System.Drawing.Color.White;
            this.LabelTitle.Location = new System.Drawing.Point(23, 37);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelTitle.Size = new System.Drawing.Size(150, 27);
            this.LabelTitle.TabIndex = 16;
            this.LabelTitle.Text = "Preset";
            this.LabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelName
            // 
            this.LabelName.AutoSize = true;
            this.LabelName.ForeColor = System.Drawing.Color.White;
            this.LabelName.Location = new System.Drawing.Point(20, 79);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(44, 13);
            this.LabelName.TabIndex = 15;
            this.LabelName.Text = Resources.Name;//"Nombre";
            // 
            // TextBoxPresetName
            // 
            this.TextBoxPresetName.AcceptsReturn = false;
            this.TextBoxPresetName.AcceptsTab = false;
            this.TextBoxPresetName.AnimationSpeed = 200;
            this.TextBoxPresetName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.TextBoxPresetName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.TextBoxPresetName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.TextBoxPresetName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TextBoxPresetName.BackgroundImage")));
            this.TextBoxPresetName.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.TextBoxPresetName.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.TextBoxPresetName.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.TextBoxPresetName.BorderColorIdle = System.Drawing.Color.DimGray;
            this.TextBoxPresetName.BorderRadius = 1;
            this.TextBoxPresetName.BorderThickness = 1;
            this.TextBoxPresetName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.TextBoxPresetName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBoxPresetName.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.TextBoxPresetName.DefaultText = "New Preset";
            this.TextBoxPresetName.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.TextBoxPresetName.ForeColor = System.Drawing.Color.White;
            this.TextBoxPresetName.HideSelection = true;
            this.TextBoxPresetName.IconLeft = null;
            this.TextBoxPresetName.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBoxPresetName.IconPadding = 10;
            this.TextBoxPresetName.IconRight = null;
            this.TextBoxPresetName.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBoxPresetName.Lines = new string[] {
        "New Preset"};
            this.TextBoxPresetName.Location = new System.Drawing.Point(23, 95);
            this.TextBoxPresetName.MaxLength = 32767;
            this.TextBoxPresetName.MinimumSize = new System.Drawing.Size(1, 1);
            this.TextBoxPresetName.Modified = false;
            this.TextBoxPresetName.Multiline = false;
            this.TextBoxPresetName.Name = "TextBoxPresetName";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.TextBoxPresetName.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.TextBoxPresetName.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.TextBoxPresetName.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.DimGray;
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.TextBoxPresetName.OnIdleState = stateProperties4;
            this.TextBoxPresetName.PasswordChar = '\0';
            this.TextBoxPresetName.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.TextBoxPresetName.PlaceholderText = "";
            this.TextBoxPresetName.ReadOnly = false;
            this.TextBoxPresetName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TextBoxPresetName.SelectedText = "";
            this.TextBoxPresetName.SelectionLength = 0;
            this.TextBoxPresetName.SelectionStart = 10;
            this.TextBoxPresetName.ShortcutsEnabled = true;
            this.TextBoxPresetName.Size = new System.Drawing.Size(363, 35);
            this.TextBoxPresetName.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.TextBoxPresetName.TabIndex = 14;
            this.TextBoxPresetName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TextBoxPresetName.TextMarginBottom = 0;
            this.TextBoxPresetName.TextMarginLeft = 5;
            this.TextBoxPresetName.TextMarginTop = 0;
            this.TextBoxPresetName.TextPlaceholder = "";
            this.TextBoxPresetName.UseSystemPasswordChar = false;
            this.TextBoxPresetName.WordWrap = true;
            this.TextBoxPresetName.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxName_Validating);
            // 
            // errorManager
            // 
            this.errorManager.ContainerControl = this;
            // 
            // DropdownPreset
            // 
            this.DropdownPreset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.DropdownPreset.BorderRadius = 1;
            this.DropdownPreset.Color = System.Drawing.Color.Gray;
            this.DropdownPreset.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.DropdownPreset.DisabledColor = System.Drawing.Color.Gray;
            this.DropdownPreset.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropdownPreset.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thick;
            this.DropdownPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DropdownPreset.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.DropdownPreset.FillDropDown = false;
            this.DropdownPreset.FillIndicator = false;
            this.DropdownPreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DropdownPreset.ForeColor = System.Drawing.Color.White;
            this.DropdownPreset.FormattingEnabled = true;
            this.DropdownPreset.Icon = null;
            this.DropdownPreset.IndicatorColor = System.Drawing.Color.White;
            this.DropdownPreset.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.DropdownPreset.ItemBackColor = System.Drawing.Color.DimGray;
            this.DropdownPreset.ItemBorderColor = System.Drawing.Color.DimGray;
            this.DropdownPreset.ItemForeColor = System.Drawing.Color.White;
            this.DropdownPreset.ItemHeight = 26;
            this.DropdownPreset.ItemHighLightColor = System.Drawing.Color.Gray;
            this.DropdownPreset.Location = new System.Drawing.Point(23, 98);
            this.DropdownPreset.Name = "DropdownPreset";
            this.DropdownPreset.Size = new System.Drawing.Size(363, 32);
            this.DropdownPreset.TabIndex = 17;
            this.DropdownPreset.Text = null;
            // 
            // AddPresetFormControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.DropdownPreset);
            this.Controls.Add(this.LabelTitle);
            this.Controls.Add(this.LabelName);
            this.Controls.Add(this.TextBoxPresetName);
            this.Controls.Add(this.PanelHeader);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.ButtonCancel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "AddPresetFormControl";
            this.Size = new System.Drawing.Size(409, 203);
            this.Load += new System.EventHandler(this.AddPresetFormControl_Load);
            this.PanelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonOK, ButtonCancel;
        private System.Windows.Forms.Panel PanelHeader;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
        private Bunifu.Framework.UI.BunifuCustomLabel LabelTitle;
        private System.Windows.Forms.Label LabelName;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox TextBoxPresetName;
        private System.Windows.Forms.ErrorProvider errorManager;
        private Bunifu.UI.WinForms.BunifuDropdown DropdownPreset;
    }
}
