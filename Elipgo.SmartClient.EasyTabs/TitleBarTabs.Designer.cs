using Bunifu.Framework.UI;
using System.ComponentModel;

namespace Elipgo.SmartClient.EasyTabs
{
  partial class TitleBarTabs
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TitleBarTabs));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.alertBox = new System.Windows.Forms.PictureBox();
            this.buttonHome = new Bunifu.Framework.UI.BunifuImageButton();
            this.buttonToolbarHidden = new Bunifu.Framework.UI.BunifuImageButton();
            this.buttonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.buttonRestore = new Bunifu.Framework.UI.BunifuImageButton();
            this.buttonMinimize = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuToolTip = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alertBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonToolbarHidden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonRestore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonMinimize)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.panelHeader.Controls.Add(this.alertBox);
            this.panelHeader.Controls.Add(this.buttonHome);
            this.panelHeader.Controls.Add(this.buttonToolbarHidden);
            this.panelHeader.Controls.Add(this.buttonClose);
            this.panelHeader.Controls.Add(this.buttonRestore);
            this.panelHeader.Controls.Add(this.buttonMinimize);
            this.panelHeader.Location = new System.Drawing.Point(0, -2);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1920, 38);
            this.panelHeader.TabIndex = 3;
            this.bunifuToolTip.SetToolTip(this.panelHeader, "");
            this.bunifuToolTip.SetToolTipIcon(this.panelHeader, null);
            this.bunifuToolTip.SetToolTipTitle(this.panelHeader, "");
            // 
            // alertBox
            // 
            this.alertBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.alertBox.Image = ((System.Drawing.Image)(resources.GetObject("alertBox.Image")));
            this.alertBox.Location = new System.Drawing.Point(1788, 3);
            this.alertBox.Name = "alertBox";
            this.alertBox.Size = new System.Drawing.Size(35, 35);
            this.alertBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.alertBox.TabIndex = 5;
            this.alertBox.TabStop = false;
            this.bunifuToolTip.SetToolTip(this.alertBox, "Parece que perdimos la conexión con el servidor central, \r\nalgunas funciones podr" +
        "ían estar limitadas debido a este inconveniente.");
            this.bunifuToolTip.SetToolTipIcon(this.alertBox, null);
            this.bunifuToolTip.SetToolTipTitle(this.alertBox, "");
            this.alertBox.Visible = false;
            // 
            // buttonHome
            // 
            this.buttonHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonHome.ErrorImage = global::Elipgo.SmartClient.EasyTabs.Resources.home;
            this.buttonHome.Image = global::Elipgo.SmartClient.EasyTabs.Resources.home;
            this.buttonHome.ImageActive = null;
            this.buttonHome.InitialImage = global::Elipgo.SmartClient.EasyTabs.Resources.home;
            this.buttonHome.Location = new System.Drawing.Point(3, 10);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(35, 29);
            this.buttonHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonHome.TabIndex = 1;
            this.buttonHome.TabStop = false;
            this.bunifuToolTip.SetToolTip(this.buttonHome, "Home");
            this.bunifuToolTip.SetToolTipIcon(this.buttonHome, null);
            this.bunifuToolTip.SetToolTipTitle(this.buttonHome, "");
            this.buttonHome.Zoom = 10;
            this.buttonHome.Click += new System.EventHandler(this.ButtonHome_Click);
            // 
            // buttonToolbarHidden
            // 
            this.buttonToolbarHidden.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonToolbarHidden.ErrorImage = global::Elipgo.SmartClient.EasyTabs.Resources.icon_topbar_open_24px;
            this.buttonToolbarHidden.Image = global::Elipgo.SmartClient.EasyTabs.Resources.icon_topbar_open_24px;
            this.buttonToolbarHidden.ImageActive = null;
            this.buttonToolbarHidden.InitialImage = global::Elipgo.SmartClient.EasyTabs.Resources.icon_topbar_open_24px;
            this.buttonToolbarHidden.Location = new System.Drawing.Point(40, 14);
            this.buttonToolbarHidden.Name = "buttonToolbarHidden";
            this.buttonToolbarHidden.Size = new System.Drawing.Size(20, 20);
            this.buttonToolbarHidden.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonToolbarHidden.TabIndex = 1;
            this.buttonToolbarHidden.TabStop = false;
            this.bunifuToolTip.SetToolTip(this.buttonToolbarHidden, "CloseTopbar");
            this.bunifuToolTip.SetToolTipIcon(this.buttonToolbarHidden, null);
            this.bunifuToolTip.SetToolTipTitle(this.buttonToolbarHidden, "");
            this.buttonToolbarHidden.Zoom = 10;
            this.buttonToolbarHidden.Click += new System.EventHandler(this.ButtonToolbarHidden_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Image = global::Elipgo.SmartClient.EasyTabs.Resources.close;
            this.buttonClose.ImageActive = null;
            this.buttonClose.Location = new System.Drawing.Point(1888, 16);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(18, 18);
            this.buttonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonClose.TabIndex = 4;
            this.buttonClose.TabStop = false;
            this.bunifuToolTip.SetToolTip(this.buttonClose, "");
            this.bunifuToolTip.SetToolTipIcon(this.buttonClose, null);
            this.bunifuToolTip.SetToolTipTitle(this.buttonClose, "");
            this.buttonClose.Zoom = 0;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            this.buttonClose.MouseLeave += new System.EventHandler(this.ButtonClose_MouseLeave);
            this.buttonClose.MouseHover += new System.EventHandler(this.ButtonClose_MouseHover);
            // 
            // buttonRestore
            // 
            this.buttonRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRestore.Image = global::Elipgo.SmartClient.EasyTabs.Resources.restore;
            this.buttonRestore.ImageActive = null;
            this.buttonRestore.Location = new System.Drawing.Point(1842, 16);
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.Size = new System.Drawing.Size(18, 18);
            this.buttonRestore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonRestore.TabIndex = 3;
            this.buttonRestore.TabStop = false;
            this.bunifuToolTip.SetToolTip(this.buttonRestore, "");
            this.bunifuToolTip.SetToolTipIcon(this.buttonRestore, null);
            this.bunifuToolTip.SetToolTipTitle(this.buttonRestore, "");
            this.buttonRestore.Visible = false;
            this.buttonRestore.Zoom = 10;
            this.buttonRestore.Click += new System.EventHandler(this.ButtonRestore_Click);
            // 
            // buttonMinimize
            // 
            this.buttonMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMinimize.Image = global::Elipgo.SmartClient.EasyTabs.Resources.minimize;
            this.buttonMinimize.ImageActive = null;
            this.buttonMinimize.Location = new System.Drawing.Point(1842, 16);
            this.buttonMinimize.Name = "buttonMinimize";
            this.buttonMinimize.Size = new System.Drawing.Size(18, 18);
            this.buttonMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonMinimize.TabIndex = 2;
            this.buttonMinimize.TabStop = false;
            this.bunifuToolTip.SetToolTip(this.buttonMinimize, "");
            this.bunifuToolTip.SetToolTipIcon(this.buttonMinimize, null);
            this.bunifuToolTip.SetToolTipTitle(this.buttonMinimize, "");
            this.buttonMinimize.Zoom = 10;
            this.buttonMinimize.Click += new System.EventHandler(this.ButtonMinimize_Click);
            // 
            // bunifuToolTip
            // 
            this.bunifuToolTip.Active = true;
            this.bunifuToolTip.AlignTextWithTitle = false;
            this.bunifuToolTip.AllowAutoClose = true;
            this.bunifuToolTip.AllowFading = true;
            this.bunifuToolTip.AutoCloseDuration = 2000;
            this.bunifuToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.bunifuToolTip.BorderColor = System.Drawing.Color.Black;
            this.bunifuToolTip.ClickToShowDisplayControl = false;
            this.bunifuToolTip.ConvertNewlinesToBreakTags = true;
            this.bunifuToolTip.DisplayControl = null;
            this.bunifuToolTip.EntryAnimationSpeed = 350;
            this.bunifuToolTip.ExitAnimationSpeed = 200;
            this.bunifuToolTip.GenerateAutoCloseDuration = false;
            this.bunifuToolTip.IconMargin = 6;
            this.bunifuToolTip.InitialDelay = 0;
            this.bunifuToolTip.Name = "bunifuToolTip";
            this.bunifuToolTip.Opacity = 1D;
            this.bunifuToolTip.OverrideToolTipTitles = false;
            this.bunifuToolTip.Padding = new System.Windows.Forms.Padding(10);
            this.bunifuToolTip.ReshowDelay = 100;
            this.bunifuToolTip.ShowAlways = true;
            this.bunifuToolTip.ShowBorders = false;
            this.bunifuToolTip.ShowIcons = true;
            this.bunifuToolTip.ShowShadows = true;
            this.bunifuToolTip.Tag = null;
            this.bunifuToolTip.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.bunifuToolTip.TextForeColor = System.Drawing.Color.White;
            this.bunifuToolTip.TextMargin = 2;
            this.bunifuToolTip.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bunifuToolTip.TitleForeColor = System.Drawing.Color.Black;
            this.bunifuToolTip.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.bunifuToolTip.ToolTipTitle = null;
            // 
            // TitleBarTabs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.ClientSize = new System.Drawing.Size(1920, 894);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TitleBarTabs";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TitleBarTabs_Load);
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.alertBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonHome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonToolbarHidden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonRestore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonMinimize)).EndInit();
            this.ResumeLayout(false);

    }

        #endregion
        private System.Windows.Forms.Panel panelHeader;
        private Bunifu.Framework.UI.BunifuImageButton buttonClose;
        private Bunifu.Framework.UI.BunifuImageButton buttonRestore;
        private Bunifu.Framework.UI.BunifuImageButton buttonMinimize;
        private Bunifu.Framework.UI.BunifuImageButton buttonHome;
        private Bunifu.Framework.UI.BunifuImageButton buttonToolbarHidden;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip;
        private System.Windows.Forms.PictureBox alertBox;
    }
}