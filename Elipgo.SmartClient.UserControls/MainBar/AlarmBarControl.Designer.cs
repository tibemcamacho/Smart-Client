using System;
using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.MainBar
{
    partial class AlarmBarControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmBarControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.ButtonToggleAlarm = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.buttonClearAlarm = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.buttonClearAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGridClear
            // 
            this.buttonClearAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //this.buttonClearAlarm.Image = ((System.Drawing.Image)(resources.GetObject("buttonGridClear.Image")));
            this.buttonClearAlarm.ImageActive = null;
            this.buttonClearAlarm.InitialImage = null;
            this.buttonClearAlarm.Location = new System.Drawing.Point(1154, 24);
            this.buttonClearAlarm.Name = "buttonGridClear";
            this.buttonClearAlarm.Size = new System.Drawing.Size(24, 24);
            this.buttonClearAlarm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonClearAlarm.TabIndex = 0;
            this.buttonClearAlarm.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.buttonClearAlarm, "");
            this.bunifuToolTip1.SetToolTipIcon(this.buttonClearAlarm, null);
            this.bunifuToolTip1.SetToolTipTitle(this.buttonClearAlarm, "");
            this.buttonClearAlarm.Zoom = 10;
            this.buttonClearAlarm.Click += new System.EventHandler(this.ButtonRemoveAlarms_Click);

            // 
            // bunifuToolTip1
            // 
            this.bunifuToolTip1.Active = true;
            this.bunifuToolTip1.AlignTextWithTitle = false;
            this.bunifuToolTip1.AllowAutoClose = true;
            this.bunifuToolTip1.AllowFading = true;
            this.bunifuToolTip1.AutoCloseDuration = 2000;
            this.bunifuToolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.bunifuToolTip1.BorderColor = System.Drawing.Color.Black;
            this.bunifuToolTip1.ClickToShowDisplayControl = false;
            this.bunifuToolTip1.ConvertNewlinesToBreakTags = true;
            this.bunifuToolTip1.DisplayControl = null;
            this.bunifuToolTip1.EntryAnimationSpeed = 350;
            this.bunifuToolTip1.ExitAnimationSpeed = 200;
            this.bunifuToolTip1.GenerateAutoCloseDuration = false;
            this.bunifuToolTip1.IconMargin = 6;
            this.bunifuToolTip1.InitialDelay = 0;
            this.bunifuToolTip1.Name = "bunifuToolTip1";
            this.bunifuToolTip1.Opacity = 1D;
            this.bunifuToolTip1.OverrideToolTipTitles = false;
            this.bunifuToolTip1.Padding = new System.Windows.Forms.Padding(10);
            this.bunifuToolTip1.ReshowDelay = 100;
            this.bunifuToolTip1.ShowAlways = true;
            this.bunifuToolTip1.ShowBorders = false;
            this.bunifuToolTip1.ShowIcons = true;
            this.bunifuToolTip1.ShowShadows = true;
            this.bunifuToolTip1.Tag = null;
            this.bunifuToolTip1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.bunifuToolTip1.TextForeColor = System.Drawing.Color.White;
            this.bunifuToolTip1.TextMargin = 2;
            this.bunifuToolTip1.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bunifuToolTip1.TitleForeColor = System.Drawing.Color.Black;
            this.bunifuToolTip1.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.bunifuToolTip1.ToolTipTitle = null;
            // 
            // ButtonToogleBookmars
            // 

            this.ButtonToggleAlarm.AllowToggling = false;
            this.ButtonToggleAlarm.AnimationSpeed = 200;
            this.ButtonToggleAlarm.AutoGenerateColors = false;
            //this.ButtonToggleAlarm.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this.ButtonToggleAlarm.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonToggleAlarm.ButtonText = Resources.AlarmHistory;
            this.ButtonToggleAlarm.ButtonTextMarginLeft = 1;
            this.ButtonToggleAlarm.ColorContrastOnClick = 45;
            this.ButtonToggleAlarm.ColorContrastOnHover = 45;
            this.ButtonToggleAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonToggleAlarm.CustomizableEdges = borderEdges1;
            this.ButtonToggleAlarm.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ButtonToggleAlarm.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonToggleAlarm.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            this.ButtonToggleAlarm.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            this.ButtonToggleAlarm.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonToggleAlarm.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonToggleAlarm.ForeColor = System.Drawing.Color.White;
            this.ButtonToggleAlarm.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonToggleAlarm.IconMarginLeft = 11;
            this.ButtonToggleAlarm.IconPadding = 10;
            this.ButtonToggleAlarm.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonToggleAlarm.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.ButtonToggleAlarm.IdleBorderRadius = 30;
            this.ButtonToggleAlarm.IdleBorderThickness = 1;
            //this.ButtonToggleAlarm.IdleIconLeftImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this.ButtonToggleAlarm.IdleIconRightImage = null;
            this.ButtonToggleAlarm.IndicateFocus = false;
            this.ButtonToggleAlarm.Location = new System.Drawing.Point(700, 17); // Point(1155, 17) // ddvl/31-Mzo-2021/vmon-3980/ relacionado a defecto de UX
            this.ButtonToggleAlarm.Name = "ButtonToggleBookmarkGrid";
            this.ButtonToggleAlarm.Size = new System.Drawing.Size(240, 37);
            this.ButtonToggleAlarm.TabIndex = 21;
            this.ButtonToggleAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonToggleAlarm.TextMarginLeft = 1;
            this.ButtonToggleAlarm.UseDefaultRadiusAndThickness = true;
            this.ButtonToggleAlarm.Visible = true;
            // 
            // AlarmBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClearAlarm);
            this.Controls.Add(this.ButtonToggleAlarm);
            this.Name = "AlarmBarControl";
            this.Size = new System.Drawing.Size(1201, 72);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this.buttonClearAlarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.Framework.UI.BunifuImageButton buttonClearAlarm;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonToggleAlarm;

    }
}
