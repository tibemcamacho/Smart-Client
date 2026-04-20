using Elipgo.SmartClient.Common.Properties;
namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class AlarmOptionsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmOptionsControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges5 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges6 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this._panelHeader = new System.Windows.Forms.Panel();
            this._panelHeaderTitle = new System.Windows.Forms.Panel();
            this._label = new System.Windows.Forms.Label();
            this._panelSilence = new System.Windows.Forms.Panel();
            this._labelSilenceAlarm = new System.Windows.Forms.Label();
            this._switch = new Bunifu.Framework.UI.BunifuiOSSwitch();
            this._panelFooter = new System.Windows.Forms.Panel();
            this._buttonCancelAll = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._buttonShowAll = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._panelHeader.SuspendLayout();
            this._panelHeaderTitle.SuspendLayout();
            this._panelSilence.SuspendLayout();
            this._panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this._panelHeader.Controls.Add(this._panelHeaderTitle);
            this._panelHeader.Controls.Add(this._panelSilence);
            this._panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelHeader.Location = new System.Drawing.Point(0, 0);
            this._panelHeader.Name = "pnlHeader";
            this._panelHeader.Size = new System.Drawing.Size(377, 47);
            this._panelHeader.TabIndex = 1;
            // 
            // pnlHeaderTitle
            // 
            this._panelHeaderTitle.Controls.Add(this._label);
            this._panelHeaderTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this._panelHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this._panelHeaderTitle.Name = "pnlHeaderTitle";
            this._panelHeaderTitle.Size = new System.Drawing.Size(117, 47);
            this._panelHeaderTitle.TabIndex = 7;
            // 
            // Label
            // 
            this._label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._label.Cursor = System.Windows.Forms.Cursors.Default;
            this._label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label.ForeColor = System.Drawing.Color.White;
            this._label.Location = new System.Drawing.Point(14, 11);
            this._label.Margin = new System.Windows.Forms.Padding(0);
            this._label.Name = "Label";
            this._label.Size = new System.Drawing.Size(88, 20);
            this._label.TabIndex = 2;
            this._label.Text = "label1";
            this._label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSilence
            // 
            this._panelSilence.Controls.Add(this._labelSilenceAlarm);
            this._panelSilence.Controls.Add(this._switch);
            this._panelSilence.Cursor = System.Windows.Forms.Cursors.Default;
            this._panelSilence.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelSilence.Location = new System.Drawing.Point(0, 0);
            this._panelSilence.Margin = new System.Windows.Forms.Padding(0);
            this._panelSilence.Name = "pnlSilence";
            this._panelSilence.Size = new System.Drawing.Size(377, 47);
            this._panelSilence.TabIndex = 6;
            // 
            // lblTextToggle
            // 
            this._labelSilenceAlarm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._labelSilenceAlarm.AutoSize = true;
            this._labelSilenceAlarm.ForeColor = System.Drawing.Color.White;
            this._labelSilenceAlarm.Location = new System.Drawing.Point(208, 17);
            this._labelSilenceAlarm.Name = "lblTextToggle";
            this._labelSilenceAlarm.Size = new System.Drawing.Size(112, 13);
            this._labelSilenceAlarm.TabIndex = 3;
            this._labelSilenceAlarm.Text = "Silenciar alarmas";
            // 
            // Switch
            // 
            this._switch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._switch.BackColor = System.Drawing.Color.Transparent;
            this._switch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Switch.BackgroundImage")));
            this._switch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._switch.Cursor = System.Windows.Forms.Cursors.Hand;
            this._switch.Location = new System.Drawing.Point(332, 11);
            this._switch.Margin = new System.Windows.Forms.Padding(0);
            this._switch.Name = "Switch";
            this._switch.OffColor = System.Drawing.Color.Gray;
            this._switch.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this._switch.Size = new System.Drawing.Size(35, 20);
            this._switch.TabIndex = 2;
            this._switch.Value = true;
            // 
            // pnlFooter
            // 
            this._panelFooter.Controls.Add(this._buttonCancelAll);
            this._panelFooter.Controls.Add(this._buttonShowAll);
            this._panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._panelFooter.Location = new System.Drawing.Point(0, 191);
            this._panelFooter.Name = "pnlFooter";
            this._panelFooter.Size = new System.Drawing.Size(377, 64);
            this._panelFooter.TabIndex = 2;
            // 
            // btnCancelAll
            // 
            this._buttonCancelAll.AllowToggling = false;
            this._buttonCancelAll.AnimationSpeed = 200;
            this._buttonCancelAll.AutoGenerateColors = false;
            this._buttonCancelAll.BackColor = System.Drawing.Color.Transparent;
            this._buttonCancelAll.BackColor1 = System.Drawing.Color.Transparent;
            this._buttonCancelAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancelAll.BackgroundImage")));
            this._buttonCancelAll.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancelAll.ButtonText = Resources.DiscardAll; //"Descartar todas";
            this._buttonCancelAll.ButtonTextMarginLeft = 0;
            this._buttonCancelAll.ColorContrastOnClick = 45;
            this._buttonCancelAll.ColorContrastOnHover = 45;
            this._buttonCancelAll.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges5.BottomLeft = true;
            borderEdges5.BottomRight = true;
            borderEdges5.TopLeft = true;
            borderEdges5.TopRight = true;
            this._buttonCancelAll.CustomizableEdges = borderEdges5;
            this._buttonCancelAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancelAll.DisabledBorderColor = System.Drawing.Color.Empty;
            this._buttonCancelAll.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this._buttonCancelAll.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this._buttonCancelAll.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this._buttonCancelAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._buttonCancelAll.ForeColor = System.Drawing.Color.White;
            this._buttonCancelAll.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonCancelAll.IconMarginLeft = 11;
            this._buttonCancelAll.IconPadding = 10;
            this._buttonCancelAll.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonCancelAll.IdleBorderColor = System.Drawing.Color.DimGray;
            this._buttonCancelAll.IdleBorderRadius = 30;
            this._buttonCancelAll.IdleBorderThickness = 1;
            this._buttonCancelAll.IdleFillColor = System.Drawing.Color.Transparent;
            this._buttonCancelAll.IdleIconLeftImage = null;
            this._buttonCancelAll.IdleIconRightImage = null;
            this._buttonCancelAll.IndicateFocus = false;
            this._buttonCancelAll.Location = new System.Drawing.Point(106, 12);
            this._buttonCancelAll.Name = "btnCancelAll";
            this._buttonCancelAll.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonCancelAll.onHoverState.BorderRadius = 30;
            this._buttonCancelAll.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancelAll.onHoverState.BorderThickness = 1;
            this._buttonCancelAll.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonCancelAll.onHoverState.ForeColor = System.Drawing.Color.White;
            this._buttonCancelAll.onHoverState.IconLeftImage = null;
            this._buttonCancelAll.onHoverState.IconRightImage = null;
            this._buttonCancelAll.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this._buttonCancelAll.OnIdleState.BorderRadius = 30;
            this._buttonCancelAll.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancelAll.OnIdleState.BorderThickness = 1;
            this._buttonCancelAll.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this._buttonCancelAll.OnIdleState.ForeColor = System.Drawing.Color.White;
            this._buttonCancelAll.OnIdleState.IconLeftImage = null;
            this._buttonCancelAll.OnIdleState.IconRightImage = null;
            this._buttonCancelAll.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonCancelAll.OnPressedState.BorderRadius = 30;
            this._buttonCancelAll.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancelAll.OnPressedState.BorderThickness = 1;
            this._buttonCancelAll.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonCancelAll.OnPressedState.ForeColor = System.Drawing.Color.White;
            this._buttonCancelAll.OnPressedState.IconLeftImage = null;
            this._buttonCancelAll.OnPressedState.IconRightImage = null;
            this._buttonCancelAll.Size = new System.Drawing.Size(141, 38);
            this._buttonCancelAll.TabIndex = 54;
            this._buttonCancelAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonCancelAll.TextMarginLeft = 0;
            this._buttonCancelAll.UseDefaultRadiusAndThickness = true;
            // 
            // btnShowAll
            // 
            this._buttonShowAll.AllowToggling = false;
            this._buttonShowAll.AnimationSpeed = 200;
            this._buttonShowAll.AutoGenerateColors = false;
            this._buttonShowAll.BackColor = System.Drawing.Color.Transparent;
            this._buttonShowAll.BackColor1 = System.Drawing.Color.DodgerBlue;
            this._buttonShowAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnShowAll.BackgroundImage")));
            this._buttonShowAll.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonShowAll.ButtonText = Resources.ViewAll; //"Ver todas";
            this._buttonShowAll.ButtonTextMarginLeft = 0;
            this._buttonShowAll.ColorContrastOnClick = 45;
            this._buttonShowAll.ColorContrastOnHover = 45;
            this._buttonShowAll.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges6.BottomLeft = true;
            borderEdges6.BottomRight = true;
            borderEdges6.TopLeft = true;
            borderEdges6.TopRight = true;
            this._buttonShowAll.CustomizableEdges = borderEdges6;
            this._buttonShowAll.DialogResult = System.Windows.Forms.DialogResult.None;
            this._buttonShowAll.DisabledBorderColor = System.Drawing.Color.Empty;
            this._buttonShowAll.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this._buttonShowAll.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this._buttonShowAll.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this._buttonShowAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._buttonShowAll.ForeColor = System.Drawing.Color.White;
            this._buttonShowAll.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonShowAll.IconMarginLeft = 11;
            this._buttonShowAll.IconPadding = 10;
            this._buttonShowAll.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonShowAll.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this._buttonShowAll.IdleBorderRadius = 30;
            this._buttonShowAll.IdleBorderThickness = 1;
            this._buttonShowAll.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this._buttonShowAll.IdleIconLeftImage = null;
            this._buttonShowAll.IdleIconRightImage = null;
            this._buttonShowAll.IndicateFocus = false;
            this._buttonShowAll.Location = new System.Drawing.Point(257, 12);
            this._buttonShowAll.Name = "btnShowAll";
            this._buttonShowAll.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonShowAll.onHoverState.BorderRadius = 30;
            this._buttonShowAll.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonShowAll.onHoverState.BorderThickness = 1;
            this._buttonShowAll.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonShowAll.onHoverState.ForeColor = System.Drawing.Color.White;
            this._buttonShowAll.onHoverState.IconLeftImage = null;
            this._buttonShowAll.onHoverState.IconRightImage = null;
            this._buttonShowAll.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this._buttonShowAll.OnIdleState.BorderRadius = 30;
            this._buttonShowAll.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonShowAll.OnIdleState.BorderThickness = 1;
            this._buttonShowAll.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this._buttonShowAll.OnIdleState.ForeColor = System.Drawing.Color.White;
            this._buttonShowAll.OnIdleState.IconLeftImage = null;
            this._buttonShowAll.OnIdleState.IconRightImage = null;
            this._buttonShowAll.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonShowAll.OnPressedState.BorderRadius = 30;
            this._buttonShowAll.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonShowAll.OnPressedState.BorderThickness = 1;
            this._buttonShowAll.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonShowAll.OnPressedState.ForeColor = System.Drawing.Color.White;
            this._buttonShowAll.OnPressedState.IconLeftImage = null;
            this._buttonShowAll.OnPressedState.IconRightImage = null;
            this._buttonShowAll.Size = new System.Drawing.Size(110, 38);
            this._buttonShowAll.TabIndex = 53;
            this._buttonShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonShowAll.TextMarginLeft = 0;
            this._buttonShowAll.UseDefaultRadiusAndThickness = true;
            // 
            // AlarmOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panelFooter);
            this.Controls.Add(this._panelHeader);
            this.Name = "AlarmOptionsControl";
            this.Size = new System.Drawing.Size(377, 255);
            this._panelHeader.ResumeLayout(false);
            this._panelHeaderTitle.ResumeLayout(false);
            this._panelSilence.ResumeLayout(false);
            this._panelSilence.PerformLayout();
            this._panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel _panelHeader;
        private System.Windows.Forms.Panel _panelHeaderTitle;
        private System.Windows.Forms.Label _label;
        internal System.Windows.Forms.Panel _panelSilence;
        private System.Windows.Forms.Label _labelSilenceAlarm;
        private Bunifu.Framework.UI.BunifuiOSSwitch _switch;
        private System.Windows.Forms.Panel _panelFooter;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonCancelAll;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonShowAll;
    }
}
