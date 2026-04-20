
using System;
using Vlc.DotNet.Forms;

namespace Elipgo.SmartClient.Drivers.VRec5
{
    partial class VRec5InstantPlaybackUserControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VRec5InstantPlaybackUserControl));
            this.PanelVideo = new System.Windows.Forms.Panel();

            this.PanelControls = new System.Windows.Forms.Panel();
            this.LabelSpeed = new System.Windows.Forms.Label();
            this.ButtonSlow = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonFast = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonSnapshot = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonFullScreen = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonBookmark = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonRewSecs = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonFwdSecs = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonPause = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonPlay = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonZoom = new Bunifu.Framework.UI.BunifuImageButton();
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.LabelCapacityNotAvailable = new System.Windows.Forms.Label();
            this.SliderTooltip = new System.Windows.Forms.Label();
            this.panelNoConnection = new System.Windows.Forms.Panel();
            this.panelFondoLogo = new System.Windows.Forms.PictureBox();
            this.slider = new Bunifu.Framework.UI.BunifuSlider();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.PanelControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonSlow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonSnapshot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFullScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonBookmark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonRewSecs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFwdSecs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelVideo
            // 
            this.PanelVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelVideo.Controls.Add(this.panelFondoLogo);
            this.PanelVideo.Controls.Add(this.panelNoConnection);
            this.PanelVideo.Controls.Add(this.vlcControl);
            this.PanelVideo.Location = new System.Drawing.Point(0, 0);
            this.PanelVideo.Margin = new System.Windows.Forms.Padding(0);
            this.PanelVideo.Name = "PanelVideo";
            this.PanelVideo.Size = new System.Drawing.Size(713, 373);
            this.PanelVideo.TabIndex = 1;
            this.bunifuToolTip1.SetToolTip(this.PanelVideo, "");
            this.bunifuToolTip1.SetToolTipIcon(this.PanelVideo, null);
            this.bunifuToolTip1.SetToolTipTitle(this.PanelVideo, "");
            // 
            // PanelControls
            // 
            this.PanelControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.PanelControls.Controls.Add(this.LabelSpeed);
            this.PanelControls.Controls.Add(this.ButtonSlow);
            this.PanelControls.Controls.Add(this.ButtonFast);
            this.PanelControls.Controls.Add(this.ButtonSnapshot);
            this.PanelControls.Controls.Add(this.ButtonFullScreen);
            this.PanelControls.Controls.Add(this.ButtonBookmark);
            this.PanelControls.Controls.Add(this.ButtonRewSecs);
            this.PanelControls.Controls.Add(this.ButtonFwdSecs);
            this.PanelControls.Controls.Add(this.ButtonPause);
            this.PanelControls.Controls.Add(this.ButtonPlay);
            this.PanelControls.Controls.Add(this.ButtonZoom);
            this.PanelControls.Location = new System.Drawing.Point(0, 381);
            this.PanelControls.Margin = new System.Windows.Forms.Padding(0);
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.Size = new System.Drawing.Size(713, 51);
            this.PanelControls.TabIndex = 68;
            this.bunifuToolTip1.SetToolTip(this.PanelControls, "");
            this.bunifuToolTip1.SetToolTipIcon(this.PanelControls, null);
            this.bunifuToolTip1.SetToolTipTitle(this.PanelControls, "");
            // 
            // LabelSpeed
            // 
            this.LabelSpeed.ForeColor = System.Drawing.Color.White;
            this.LabelSpeed.Location = new System.Drawing.Point(210, 20);
            this.LabelSpeed.Name = "LabelSpeed";
            this.LabelSpeed.Size = new System.Drawing.Size(31, 18);
            this.LabelSpeed.TabIndex = 13;
            this.LabelSpeed.Text = "1X";
            this.LabelSpeed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bunifuToolTip1.SetToolTip(this.LabelSpeed, "");
            this.bunifuToolTip1.SetToolTipIcon(this.LabelSpeed, null);
            this.bunifuToolTip1.SetToolTipTitle(this.LabelSpeed, "");
            // 
            // ButtonSlow
            // 
            this.ButtonSlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonSlow.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSlow.Image")));
            this.ButtonSlow.ImageActive = null;
            this.ButtonSlow.Location = new System.Drawing.Point(183, 15);
            this.ButtonSlow.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonSlow.Name = "ButtonSlow";
            this.ButtonSlow.Size = new System.Drawing.Size(24, 24);
            this.ButtonSlow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonSlow.TabIndex = 12;
            this.ButtonSlow.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonSlow, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonSlow, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonSlow, "");
            this.ButtonSlow.Zoom = 10;
            this.ButtonSlow.Click += new System.EventHandler(this.ButtonSlow_Click);
            // 
            // ButtonFast
            // 
            this.ButtonFast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFast.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFast.Image")));
            this.ButtonFast.ImageActive = null;
            this.ButtonFast.Location = new System.Drawing.Point(244, 15);
            this.ButtonFast.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonFast.Name = "ButtonFast";
            this.ButtonFast.Size = new System.Drawing.Size(24, 24);
            this.ButtonFast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonFast.TabIndex = 11;
            this.ButtonFast.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonFast, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonFast, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonFast, "");
            this.ButtonFast.Zoom = 10;
            this.ButtonFast.Click += new System.EventHandler(this.ButtonFast_Click);
            // 
            // ButtonSnapshot
            // 
            this.ButtonSnapshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSnapshot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonSnapshot.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSnapshot.Image")));
            this.ButtonSnapshot.ImageActive = null;
            this.ButtonSnapshot.Location = new System.Drawing.Point(573, 15);
            this.ButtonSnapshot.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonSnapshot.Name = "ButtonSnapshot";
            this.ButtonSnapshot.Size = new System.Drawing.Size(24, 24);
            this.ButtonSnapshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonSnapshot.TabIndex = 10;
            this.ButtonSnapshot.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonSnapshot, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonSnapshot, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonSnapshot, "");
            this.ButtonSnapshot.Zoom = 10;
            this.ButtonSnapshot.Click += new System.EventHandler(this.ButtonSnapshot_Click);
            // 
            // ButtonFullScreen
            // 
            this.ButtonFullScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonFullScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFullScreen.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFullScreen.Image")));
            this.ButtonFullScreen.ImageActive = null;
            this.ButtonFullScreen.Location = new System.Drawing.Point(499, 15);
            this.ButtonFullScreen.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonFullScreen.Name = "ButtonFullScreen";
            this.ButtonFullScreen.Size = new System.Drawing.Size(24, 24);
            this.ButtonFullScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonFullScreen.TabIndex = 5;
            this.ButtonFullScreen.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonFullScreen, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonFullScreen, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonFullScreen, "");
            this.ButtonFullScreen.Visible = false;
            this.ButtonFullScreen.Zoom = 10;
            // 
            // ButtonBookmark
            // 
            this.ButtonBookmark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonBookmark.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonBookmark.Image = ((System.Drawing.Image)(resources.GetObject("ButtonBookmark.Image")));
            this.ButtonBookmark.ImageActive = null;
            this.ButtonBookmark.Location = new System.Drawing.Point(536, 15);
            this.ButtonBookmark.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonBookmark.Name = "ButtonBookmark";
            this.ButtonBookmark.Size = new System.Drawing.Size(24, 24);
            this.ButtonBookmark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonBookmark.TabIndex = 4;
            this.ButtonBookmark.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonBookmark, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonBookmark, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonBookmark, "");
            this.ButtonBookmark.Zoom = 10;
            // 
            // ButtonRewSecs
            // 
            this.ButtonRewSecs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonRewSecs.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRewSecs.Image")));
            this.ButtonRewSecs.ImageActive = null;
            this.ButtonRewSecs.Location = new System.Drawing.Point(344, 15);
            this.ButtonRewSecs.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonRewSecs.Name = "ButtonRewSecs";
            this.ButtonRewSecs.Size = new System.Drawing.Size(24, 24);
            this.ButtonRewSecs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonRewSecs.TabIndex = 3;
            this.ButtonRewSecs.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonRewSecs, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonRewSecs, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonRewSecs, "");
            this.ButtonRewSecs.Zoom = 10;
            this.ButtonRewSecs.Click += new System.EventHandler(this.ButtonRewSecs_Click);
            // 
            // ButtonFwdSecs
            // 
            this.ButtonFwdSecs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFwdSecs.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFwdSecs.Image")));
            this.ButtonFwdSecs.ImageActive = null;
            this.ButtonFwdSecs.Location = new System.Drawing.Point(388, 15);
            this.ButtonFwdSecs.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonFwdSecs.Name = "ButtonFwdSecs";
            this.ButtonFwdSecs.Size = new System.Drawing.Size(24, 24);
            this.ButtonFwdSecs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonFwdSecs.TabIndex = 2;
            this.ButtonFwdSecs.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonFwdSecs, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonFwdSecs, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonFwdSecs, "");
            this.ButtonFwdSecs.Zoom = 10;
            this.ButtonFwdSecs.Click += new System.EventHandler(this.ButtonFwdSecs_Click);
            // 
            // ButtonPause
            // 
            this.ButtonPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonPause.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPause.Image")));
            this.ButtonPause.ImageActive = null;
            this.ButtonPause.Location = new System.Drawing.Point(61, 15);
            this.ButtonPause.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonPause.Name = "ButtonPause";
            this.ButtonPause.Size = new System.Drawing.Size(24, 24);
            this.ButtonPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonPause.TabIndex = 1;
            this.ButtonPause.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonPause, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonPause, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonPause, "");
            this.ButtonPause.Zoom = 10;
            this.ButtonPause.Click += new System.EventHandler(this.ButtonPause_Click);
            // 
            // ButtonPlay
            // 
            this.ButtonPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonPlay.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPlay.Image")));
            this.ButtonPlay.ImageActive = null;
            this.ButtonPlay.Location = new System.Drawing.Point(15, 15);
            this.ButtonPlay.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonPlay.Name = "ButtonPlay";
            this.ButtonPlay.Size = new System.Drawing.Size(24, 24);
            this.ButtonPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonPlay.TabIndex = 0;
            this.ButtonPlay.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonPlay, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonPlay, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonPlay, "");
            this.ButtonPlay.Zoom = 10;
            this.ButtonPlay.Click += new System.EventHandler(this.ButtonPlay_Click);
            // 
            // ButtonZoom
            // 
            this.ButtonZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZoom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonZoom.ImageActive = null;
            this.ButtonZoom.Location = new System.Drawing.Point(608, 15);
            this.ButtonZoom.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonZoom.Name = "ButtonZoom";
            this.ButtonZoom.Size = new System.Drawing.Size(24, 24);
            this.ButtonZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonZoom.TabIndex = 14;
            this.ButtonZoom.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.ButtonZoom, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonZoom, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonZoom, "");
            this.ButtonZoom.Zoom = 10;
            this.ButtonZoom.Click += new System.EventHandler(this.ButtonZoom_Click);
            // 
            // vlcControl
            // 
            this.vlcControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vlcControl.BackColor = System.Drawing.Color.Black;
            this.vlcControl.Location = new System.Drawing.Point(0, 0);
            this.vlcControl.Margin = new System.Windows.Forms.Padding(0);
            this.vlcControl.Name = "vlcControl";
            this.vlcControl.Size = new System.Drawing.Size(713, 373);
            this.vlcControl.Spu = -1;
            this.vlcControl.TabIndex = 7;
            this.vlcControl.Text = "vlcControl";
            this.bunifuToolTip1.SetToolTip(this.vlcControl, "");
            this.bunifuToolTip1.SetToolTipIcon(this.vlcControl, null);
            this.bunifuToolTip1.SetToolTipTitle(this.vlcControl, "");
            this.vlcControl.VlcLibDirectory = null;
            this.vlcControl.VlcMediaplayerOptions = null;
            this.vlcControl.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.OnVlcControlNeedLibDirectory);
            // 
            // LabelCapacityNotAvailable
            // 
            this.LabelCapacityNotAvailable.Location = new System.Drawing.Point(0, 0);
            this.LabelCapacityNotAvailable.Name = "LabelCapacityNotAvailable";
            this.LabelCapacityNotAvailable.Size = new System.Drawing.Size(100, 23);
            this.LabelCapacityNotAvailable.Visible = false;
            this.LabelCapacityNotAvailable.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.LabelCapacityNotAvailable, "");
            this.bunifuToolTip1.SetToolTipIcon(this.LabelCapacityNotAvailable, null);
            this.bunifuToolTip1.SetToolTipTitle(this.LabelCapacityNotAvailable, "");
            // 
            // SliderTooltip
            // 
            this.SliderTooltip.AutoSize = true;
            this.SliderTooltip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.SliderTooltip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SliderTooltip.ForeColor = System.Drawing.Color.White;
            this.SliderTooltip.Location = new System.Drawing.Point(0, 344);
            this.SliderTooltip.Margin = new System.Windows.Forms.Padding(0);
            this.SliderTooltip.Name = "SliderTooltip";
            this.SliderTooltip.Padding = new System.Windows.Forms.Padding(1);
            this.SliderTooltip.Size = new System.Drawing.Size(80, 15);
            this.SliderTooltip.TabIndex = 69;
            this.SliderTooltip.Text = "SliderTooltip";
            this.SliderTooltip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuToolTip1.SetToolTip(this.SliderTooltip, "");
            this.bunifuToolTip1.SetToolTipIcon(this.SliderTooltip, null);
            this.bunifuToolTip1.SetToolTipTitle(this.SliderTooltip, "");
            this.SliderTooltip.Visible = false;
            // 
            // panelNoConnection
            // 
            this.panelNoConnection.BackColor = System.Drawing.Color.Black;
            this.panelNoConnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this.panelNoConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelNoConnection.Location = new System.Drawing.Point(156, 272);
            this.panelNoConnection.Margin = new System.Windows.Forms.Padding(0);
            this.panelNoConnection.Name = "panelNoConnection";
            this.panelNoConnection.Size = new System.Drawing.Size(200, 38);
            this.panelNoConnection.TabIndex = 9;
            this.bunifuToolTip1.SetToolTip(this.panelNoConnection, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panelNoConnection, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panelNoConnection, "");
            // 
            // panelFondoLogo
            // 
            this.panelFondoLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.panelFondoLogo.Location = new System.Drawing.Point(46, 48);
            this.panelFondoLogo.Name = "panelFondoLogo";
            this.panelFondoLogo.Size = new System.Drawing.Size(274, 145);
            this.panelFondoLogo.TabIndex = 6;
            this.panelFondoLogo.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.panelFondoLogo, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panelFondoLogo, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panelFondoLogo, "");
            this.panelFondoLogo.Visible = false;
            // 
            // slider
            // 
            this.slider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slider.BackColor = System.Drawing.Color.Transparent;
            this.slider.BackgroudColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.slider.BorderRadius = 0;
            this.slider.Enabled = false;
            this.slider.IndicatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            this.slider.Location = new System.Drawing.Point(0, 361);
            this.slider.Margin = new System.Windows.Forms.Padding(0);
            this.slider.MaximumValue = 100;
            this.slider.Name = "slider";
            this.slider.Size = new System.Drawing.Size(712, 30);
            this.slider.TabIndex = 69;
            this.bunifuToolTip1.SetToolTip(this.slider, "");
            this.bunifuToolTip1.SetToolTipIcon(this.slider, null);
            this.bunifuToolTip1.SetToolTipTitle(this.slider, "");
            this.slider.Value = 0;
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
            // VRec5InstantPlaybackUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.LabelCapacityNotAvailable);
            this.Controls.Add(this.SliderTooltip);
            this.Controls.Add(this.slider);
            this.Controls.Add(this.PanelControls);
            this.Controls.Add(this.PanelVideo);
            this.Name = "VRec5InstantPlaybackUserControl";
            this.Size = new System.Drawing.Size(713, 432);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            this.PanelControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonSlow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonSnapshot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFullScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonBookmark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonRewSecs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFwdSecs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel PanelVideo;
        private System.Windows.Forms.Panel PanelControls;
        private Bunifu.Framework.UI.BunifuImageButton ButtonRewSecs;
        private Bunifu.Framework.UI.BunifuImageButton ButtonFwdSecs;
        private Bunifu.Framework.UI.BunifuImageButton ButtonPause;
        private Bunifu.Framework.UI.BunifuImageButton ButtonPlay;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoom;
        private Bunifu.Framework.UI.BunifuSlider slider;
        private Bunifu.Framework.UI.BunifuImageButton ButtonBookmark;
        private System.Windows.Forms.Label LabelCapacityNotAvailable;
        private Bunifu.Framework.UI.BunifuImageButton ButtonFullScreen;
        private Bunifu.Framework.UI.BunifuImageButton ButtonSnapshot;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private System.Windows.Forms.Panel panelNoConnection;
        private System.Windows.Forms.PictureBox panelFondoLogo;
        private System.Windows.Forms.Label SliderTooltip;
        private Vlc.DotNet.Forms.VlcControl vlcControl;
        private System.Windows.Forms.Label LabelSpeed;
        private Bunifu.Framework.UI.BunifuImageButton ButtonSlow;
        private Bunifu.Framework.UI.BunifuImageButton ButtonFast;
    }
}
