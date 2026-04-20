namespace Elipgo.SmartClient.Drivers.Kurento
{
    partial class KurentoInstantPlaybackUserControl
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
            this.browser = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.PanelControls = new System.Windows.Forms.Panel();
            this.panelNoConnection = new System.Windows.Forms.Panel();
            this.panelFondoLogo = new System.Windows.Forms.PictureBox();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.browser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.AllowExternalDrop = true;
            this.browser.CreationProperties = null;
            this.browser.DefaultBackgroundColor = System.Drawing.Color.White;
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Margin = new System.Windows.Forms.Padding(0);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(713, 432);
            this.browser.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.browser, "");
            this.bunifuToolTip1.SetToolTipIcon(this.browser, null);
            this.bunifuToolTip1.SetToolTipTitle(this.browser, "");
            this.browser.ZoomFactor = 1D;
            this.browser.WebMessageReceived += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs>(this.Browser_WebMessageReceived);
            // 
            // PanelControls
            // 
            this.PanelControls.Location = new System.Drawing.Point(0, 0);
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.Size = new System.Drawing.Size(200, 100);
            this.PanelControls.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.PanelControls, "");
            this.bunifuToolTip1.SetToolTipIcon(this.PanelControls, null);
            this.bunifuToolTip1.SetToolTipTitle(this.PanelControls, "");
            // 
            // panelNoConnection
            // 
            this.panelNoConnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this.panelNoConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelNoConnection.Location = new System.Drawing.Point(251, 153);
            this.panelNoConnection.Margin = new System.Windows.Forms.Padding(0);
            this.panelNoConnection.Name = "panelNoConnection";
            this.panelNoConnection.Size = new System.Drawing.Size(200, 80);
            this.panelNoConnection.TabIndex = 68;
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
            // bunifuToolTip1
            // 
            this.bunifuToolTip1.Active = true;
            this.bunifuToolTip1.AlignTextWithTitle = false;
            this.bunifuToolTip1.AllowAutoClose = false;
            this.bunifuToolTip1.AllowFading = true;
            this.bunifuToolTip1.AutoCloseDuration = 5000;
            this.bunifuToolTip1.BackColor = System.Drawing.SystemColors.Control;
            this.bunifuToolTip1.BorderColor = System.Drawing.Color.Gainsboro;
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
            this.bunifuToolTip1.TextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuToolTip1.TextMargin = 2;
            this.bunifuToolTip1.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bunifuToolTip1.TitleForeColor = System.Drawing.Color.Black;
            this.bunifuToolTip1.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.bunifuToolTip1.ToolTipTitle = null;
            // 
            // KurentoInstantPlaybackUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browser);
            this.Controls.Add(this.panelNoConnection);
            this.Controls.Add(this.panelFondoLogo);
            this.Name = "KurentoInstantPlaybackUserControl";
            this.Size = new System.Drawing.Size(713, 432);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this.browser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 browser;
        private System.Windows.Forms.Panel PanelControls;
        private System.Windows.Forms.Panel panelNoConnection;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private System.Windows.Forms.PictureBox panelFondoLogo;
    }
}
