using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.PlaybackControls
{
    partial class PlaybackControlsUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaybackControlsUserControl));
            // this.ButtonRewind = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonBookmark = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonForward = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonPause = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonPlay = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonRew30Secs = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonCalendar = new Elipgo.SmartClient.UserControls.Shared.ButtonCalendarControl();
            this._labelTime = new System.Windows.Forms.Label();
            this._labelDate = new System.Windows.Forms.Label();
            this._labelSpeed = new System.Windows.Forms.Label();
            this._buttonSlow = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonFast = new Bunifu.Framework.UI.BunifuImageButton();
            this._bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonFw30Secs = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this._labelScaletime = new System.Windows.Forms.Label();
            this._labelScale = new System.Windows.Forms.Label();
            this._zoomTimeLinePlaybackControl = new PlaybackControls.ZoomTimeLinePlaybackControl();

            ((System.ComponentModel.ISupportInitialize)(this._buttonBookmark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonForward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonRew30Secs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonSlow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonFast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bunifuImageButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonFw30Secs)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonBookmark
            // 
            this._buttonBookmark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonBookmark.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonBookmark.Image = ((System.Drawing.Image)(resources.GetObject("ButtonBookmark.Image")));
            this._buttonBookmark.ImageActive = null;
            this._buttonBookmark.Location = new System.Drawing.Point(1546, 13);
            this._buttonBookmark.Name = "ButtonBookmark";
            this._buttonBookmark.Size = new System.Drawing.Size(24, 24);
            this._buttonBookmark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonBookmark.TabIndex = 12;
            this._buttonBookmark.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonBookmark, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonBookmark, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonBookmark, "");
            this._buttonBookmark.Zoom = 10;
            this._buttonBookmark.Click += new System.EventHandler(this.ButtonBookmark_Click);
            // 
            // ButtonForward
            // 
            this._buttonForward.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonForward.Image = ((System.Drawing.Image)(resources.GetObject("ButtonForward.Image")));
            this._buttonForward.ImageActive = null;
            this._buttonForward.Location = new System.Drawing.Point(118, 13);
            this._buttonForward.Name = "ButtonForward";
            this._buttonForward.Size = new System.Drawing.Size(24, 24);
            this._buttonForward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonForward.TabIndex = 11;
            this._buttonForward.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonForward, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonForward, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonForward, "");
            this._buttonForward.Zoom = 10;
            this._buttonForward.Click += new System.EventHandler(this.ButtonForward_Click);
            // 
            // ButtonPause
            // 
            this._buttonPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonPause.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPause.Image")));
            this._buttonPause.ImageActive = null;
            this._buttonPause.Location = new System.Drawing.Point(70, 13);
            this._buttonPause.Name = "ButtonPause";
            this._buttonPause.Size = new System.Drawing.Size(24, 24);
            this._buttonPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonPause.TabIndex = 10;
            this._buttonPause.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonPause, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonPause, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonPause, "");
            this._buttonPause.Zoom = 10;
            this._buttonPause.Click += new System.EventHandler(this.ButtonPause_Click);
            // 
            // ButtonPlay
            // 
            this._buttonPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonPlay.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPlay.Image")));
            this._buttonPlay.ImageActive = null;
            this._buttonPlay.Location = new System.Drawing.Point(70, 13);
            this._buttonPlay.Name = "ButtonPlay";
            this._buttonPlay.Size = new System.Drawing.Size(24, 24);
            this._buttonPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonPlay.TabIndex = 9;
            this._buttonPlay.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonPlay, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonPlay, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonPlay, "");
            this._buttonPlay.Zoom = 10;
            // 
            // ButtonRew30Secs
            // 
            this._buttonRew30Secs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this._buttonRew30Secs.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonRew30Secs.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRew30Secs.Image")));
            this._buttonRew30Secs.ImageActive = null;
            this._buttonRew30Secs.InitialImage = null;
            this._buttonRew30Secs.Location = new System.Drawing.Point(390, 13);
            this._buttonRew30Secs.Name = "ButtonRew30Secs";
            this._buttonRew30Secs.Size = new System.Drawing.Size(24, 24);
            this._buttonRew30Secs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonRew30Secs.TabIndex = 8;
            this._buttonRew30Secs.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonRew30Secs, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonRew30Secs, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonRew30Secs, "");
            this._buttonRew30Secs.Zoom = 10;
            this._buttonRew30Secs.Click += new System.EventHandler(this.ButtonRew30Secs_Click);
            // 
            // ButtonCalendar
            // 
            this._buttonCalendar.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.ButtonCalendar.Location = new System.Drawing.Point(793, 11);
            this._buttonCalendar.Margin = new System.Windows.Forms.Padding(0);
            this._buttonCalendar.Name = "ButtonCalendar";
            this._buttonCalendar.Size = new System.Drawing.Size(36, 36);
            this._buttonCalendar.TabIndex = 14;
            this._buttonCalendar.TabStop = false;
            // 
            // LabelTime
            // 
            this._labelTime.ForeColor = System.Drawing.Color.White;
            this._labelTime.Location = new System.Drawing.Point(839, 15);
            this._labelTime.Margin = new System.Windows.Forms.Padding(0);
            this._labelTime.Name = "LabelTime";
            this._labelTime.Size = new System.Drawing.Size(64, 21);
            this._labelTime.TabIndex = 15;
            this._labelTime.Text = "18:06:06";
            this._labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuToolTip1.SetToolTip(this._labelTime, "");
            this.bunifuToolTip1.SetToolTipIcon(this._labelTime, null);
            this.bunifuToolTip1.SetToolTipTitle(this._labelTime, "");
            // 
            // LabelDate
            // 
            this._labelDate.ForeColor = System.Drawing.Color.White;
            //this.LabelDate.Location = new System.Drawing.Point(703, 15);
            this._labelDate.Margin = new System.Windows.Forms.Padding(0);
            this._labelDate.Name = "LabelDate";
            this._labelDate.Size = new System.Drawing.Size(86, 21);
            this._labelDate.TabIndex = 16;
            this._labelDate.Text = "20/03/2020";
            this._labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuToolTip1.SetToolTip(this._labelDate, "");
            this.bunifuToolTip1.SetToolTipIcon(this._labelDate, null);
            this.bunifuToolTip1.SetToolTipTitle(this._labelDate, "");
            // 
            // LabelSpeed
            // 
            this._labelSpeed.ForeColor = System.Drawing.Color.White;
            this._labelSpeed.Location = new System.Drawing.Point(286, 15);
            this._labelSpeed.Name = "LabelSpeed";
            this._labelSpeed.Size = new System.Drawing.Size(31, 15);
            this._labelSpeed.TabIndex = 19;
            this._labelSpeed.Text = "1X";
            this._labelSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuToolTip1.SetToolTip(this._labelSpeed, "");
            this.bunifuToolTip1.SetToolTipIcon(this._labelSpeed, null);
            this.bunifuToolTip1.SetToolTipTitle(this._labelSpeed, "");
            // 
            // ButtonSlow
            // 
            this._buttonSlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonSlow.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSlow.Image")));
            this._buttonSlow.ImageActive = null;
            this._buttonSlow.Location = new System.Drawing.Point(259, 13);
            this._buttonSlow.Margin = new System.Windows.Forms.Padding(0);
            this._buttonSlow.Name = "ButtonSlow";
            this._buttonSlow.Size = new System.Drawing.Size(24, 24);
            this._buttonSlow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonSlow.TabIndex = 18;
            this._buttonSlow.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonSlow, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonSlow, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonSlow, "");
            this._buttonSlow.Zoom = 10;
            this._buttonSlow.Click += new System.EventHandler(this.ButtonSlow_Click);
            // 
            // ButtonFast
            // 
            this._buttonFast.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonFast.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFast.Image")));
            this._buttonFast.ImageActive = null;
            this._buttonFast.Location = new System.Drawing.Point(320, 13);
            this._buttonFast.Margin = new System.Windows.Forms.Padding(0);
            this._buttonFast.Name = "ButtonFast";
            this._buttonFast.Size = new System.Drawing.Size(24, 24);
            this._buttonFast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonFast.TabIndex = 17;
            this._buttonFast.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonFast, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonFast, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonFast, "");
            this._buttonFast.Zoom = 10;
            this._buttonFast.Click += new System.EventHandler(this.ButtonFast_Click);
            // 
            // bunifuImageButton1
            // 
            this._bunifuImageButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this._bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
            this._bunifuImageButton1.ImageActive = null;
            this._bunifuImageButton1.InitialImage = null;
            this._bunifuImageButton1.Location = new System.Drawing.Point(450, 13);
            this._bunifuImageButton1.Name = "bunifuImageButton1";
            this._bunifuImageButton1.Size = new System.Drawing.Size(24, 24);
            this._bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._bunifuImageButton1.TabIndex = 20;
            this._bunifuImageButton1.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._bunifuImageButton1, "");
            this.bunifuToolTip1.SetToolTipIcon(this._bunifuImageButton1, null);
            this.bunifuToolTip1.SetToolTipTitle(this._bunifuImageButton1, "");
            this._bunifuImageButton1.Zoom = 10;
            this._bunifuImageButton1.Visible = false;
            // 
            // ButtonFw30Secs
            // 
            this._buttonFw30Secs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this._buttonFw30Secs.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonFw30Secs.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFw30Secs.Image")));
            this._buttonFw30Secs.ImageActive = null;
            this._buttonFw30Secs.InitialImage = null;
            this._buttonFw30Secs.Location = new System.Drawing.Point(450, 13);
            this._buttonFw30Secs.Name = "ButtonFw30Secs";
            this._buttonFw30Secs.Size = new System.Drawing.Size(24, 24);
            this._buttonFw30Secs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonFw30Secs.TabIndex = 21;
            this._buttonFw30Secs.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this._buttonFw30Secs, "");
            this.bunifuToolTip1.SetToolTipIcon(this._buttonFw30Secs, null);
            this.bunifuToolTip1.SetToolTipTitle(this._buttonFw30Secs, "");
            this._buttonFw30Secs.Zoom = 10;
            this._buttonFw30Secs.Click += new System.EventHandler(this.ButtonFw30Secs_Click);
            // 
            // labelScale
            // 
            this._labelScale.ForeColor = System.Drawing.Color.White;
            this._labelScale.Margin = new System.Windows.Forms.Padding(0);
            this._labelScale.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFw30Secs.Image")));
            this._labelScale.Name = "labelScale";
            this._labelScale.Size = new System.Drawing.Size(24, 24);
            this._labelScale.TabIndex = 22;
            this._labelScale.Text = "";
            this._labelScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuToolTip1.SetToolTip(this._labelScale, "");
            this.bunifuToolTip1.SetToolTipIcon(this._labelScale, null);
            this.bunifuToolTip1.SetToolTipTitle(this._labelScale, "");
            //
            // labelScaletime
            // 
            this._labelScaletime.ForeColor = System.Drawing.Color.White;
            //this.labelScaletime.Location = new System.Drawing.Point(1080, 15);
            this._labelScaletime.Margin = new System.Windows.Forms.Padding(0);
            this._labelScaletime.Name = "labelScaletime";
            this._labelScaletime.Size = new System.Drawing.Size(86, 21);
            this._labelScaletime.TabIndex = 22;
            this._labelScaletime.Text = Resources.TimelineScale;
            this._labelScaletime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuToolTip1.SetToolTip(this._labelScaletime, "");
            this.bunifuToolTip1.SetToolTipIcon(this._labelScaletime, null);
            this.bunifuToolTip1.SetToolTipTitle(this._labelScaletime, "");
            //
            // zoomTimeLinePlaybackControl
            //
            this._zoomTimeLinePlaybackControl.Name = "zoomTimeLinePlaybackControl";
            this._zoomTimeLinePlaybackControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            // PlaybackControlsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._labelScaletime);
            this.Controls.Add(this._buttonFw30Secs);
            this.Controls.Add(this._bunifuImageButton1);
            this.Controls.Add(this._labelSpeed);
            this.Controls.Add(this._buttonSlow);
            this.Controls.Add(this._buttonFast);
            this.Controls.Add(this._labelDate);
            this.Controls.Add(this._labelTime);
            this.Controls.Add(this._buttonCalendar);
            //this.Controls.Add(this.ButtonRewind);
            this.Controls.Add(this._buttonBookmark);
            this.Controls.Add(this._buttonForward);
            this.Controls.Add(this._buttonPause);
            this.Controls.Add(this._buttonPlay);
            this.Controls.Add(this._buttonRew30Secs);
            this.Controls.Add(this._zoomTimeLinePlaybackControl);
            this.Controls.Add(this._labelScale);
            this.Name = "PlaybackControlsUserControl";
            this.Size = new System.Drawing.Size(1629, 51);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            // ((System.ComponentModel.ISupportInitialize)(this.ButtonRewind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonBookmark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonForward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonRew30Secs)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.ButtonCalendar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonSlow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonFast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bunifuImageButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonFw30Secs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private Bunifu.Framework.UI.BunifuImageButton ButtonRewind;
        private Bunifu.Framework.UI.BunifuImageButton _buttonBookmark;
        private Bunifu.Framework.UI.BunifuImageButton _buttonForward;
        private Bunifu.Framework.UI.BunifuImageButton _buttonPause;
        private Bunifu.Framework.UI.BunifuImageButton _buttonPlay;
        private Bunifu.Framework.UI.BunifuImageButton _buttonRew30Secs;
        private Shared.ButtonCalendarControl _buttonCalendar;
        private System.Windows.Forms.Label _labelTime;
        private System.Windows.Forms.Label _labelDate;
        private System.Windows.Forms.Label _labelSpeed;
        private Bunifu.Framework.UI.BunifuImageButton _buttonSlow;
        private Bunifu.Framework.UI.BunifuImageButton _buttonFast;
        private Bunifu.Framework.UI.BunifuImageButton _bunifuImageButton1;
        private Bunifu.Framework.UI.BunifuImageButton _buttonFw30Secs;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private System.Windows.Forms.Label _labelScale;
        private System.Windows.Forms.Label _labelScaletime;
        private PlaybackControls.ZoomTimeLinePlaybackControl _zoomTimeLinePlaybackControl;
        //private Shared.DropDownScaleTime dropDownScaleTime1;
    }
}