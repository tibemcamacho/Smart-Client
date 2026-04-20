
namespace Elipgo.SmartClient.Drivers.ONVIF
{
    partial class OnvifLiveUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnvifLiveUserControl));
            this.picture = new System.Windows.Forms.PictureBox();
            this.panelFondoLogo = new System.Windows.Forms.PictureBox();
            this.panelNoConnection = new System.Windows.Forms.Panel();
            this.ButtonZoomIn = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonZoomOut = new Bunifu.Framework.UI.BunifuImageButton();
            this.PanelVideo = new System.Windows.Forms.Panel();
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).BeginInit();
            this.PanelVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picture.Location = new System.Drawing.Point(0, 0);
            this.picture.Margin = new System.Windows.Forms.Padding(0);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(0, 0);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            // 
            // panelFondoLogo
            // 
            this.panelFondoLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelFondoLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.panelFondoLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelFondoLogo.Location = new System.Drawing.Point(0, 0);
            this.panelFondoLogo.Margin = new System.Windows.Forms.Padding(0);
            this.panelFondoLogo.Name = "panelFondoLogo";
            this.panelFondoLogo.Size = new System.Drawing.Size(0, 0);
            this.panelFondoLogo.TabIndex = 8;
            this.panelFondoLogo.TabStop = false;
            this.panelFondoLogo.Visible = false;
            // 
            // panelNoConnection
            // 
            this.panelNoConnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this.panelNoConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelNoConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNoConnection.Location = new System.Drawing.Point(0, 0);
            this.panelNoConnection.Margin = new System.Windows.Forms.Padding(0);
            this.panelNoConnection.Name = "panelNoConnection";
            this.panelNoConnection.Size = new System.Drawing.Size(545, 385);
            this.panelNoConnection.TabIndex = 9;
            // 
            // ButtonZoomIn
            // 
            this.ButtonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZoomIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("ButtonZoomIn.Image")));
            this.ButtonZoomIn.ImageActive = null;
            this.ButtonZoomIn.Location = new System.Drawing.Point(658, 317);
            this.ButtonZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonZoomIn.Name = "ButtonZoomIn";
            this.ButtonZoomIn.Size = new System.Drawing.Size(24, 24);
            this.ButtonZoomIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonZoomIn.TabIndex = 4;
            this.ButtonZoomIn.TabStop = false;
            this.ButtonZoomIn.Zoom = 10;
            this.ButtonZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomIn_MouseDown);
            this.ButtonZoomIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomIn_MouseUp);
            // 
            // ButtonZoomOut
            // 
            this.ButtonZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZoomOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("ButtonZoomOut.Image")));
            this.ButtonZoomOut.ImageActive = null;
            this.ButtonZoomOut.Location = new System.Drawing.Point(658, 350);
            this.ButtonZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonZoomOut.Name = "ButtonZoomOut";
            this.ButtonZoomOut.Size = new System.Drawing.Size(24, 24);
            this.ButtonZoomOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonZoomOut.TabIndex = 3;
            this.ButtonZoomOut.TabStop = false;
            this.ButtonZoomOut.Zoom = 10;
            this.ButtonZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomOut_MouseDown);
            this.ButtonZoomOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomOut_MouseUp);
            // 
            // PanelVideo
            // 
            this.PanelVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.PanelVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelVideo.Controls.Add(this.panelNoConnection);
            this.PanelVideo.Controls.Add(this.vlcControl);
            this.PanelVideo.Location = new System.Drawing.Point(0, 0);
            this.PanelVideo.Margin = new System.Windows.Forms.Padding(0);
            this.PanelVideo.Name = "PanelVideo";
            this.PanelVideo.Size = new System.Drawing.Size(547, 387);
            this.PanelVideo.TabIndex = 10;
            // 
            // vlcControl
            // 
            this.vlcControl.BackColor = System.Drawing.Color.Black;
            this.vlcControl.Location = new System.Drawing.Point(0, 0);
            this.vlcControl.Name = "vlcControl";
            this.vlcControl.Size = new System.Drawing.Size(1224, 644);
            this.vlcControl.Spu = -1;
            this.vlcControl.TabIndex = 10;
            this.vlcControl.Text = "vlcControl";
            this.vlcControl.VlcLibDirectory = null;
            this.vlcControl.VlcMediaplayerOptions = null;
            this.vlcControl.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.OnVlcControlNeedLibDirectory);
            // 
            // OnvifLiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelVideo);
            this.Controls.Add(this.ButtonZoomIn);
            this.Controls.Add(this.ButtonZoomOut);
            this.Controls.Add(this.panelFondoLogo);
            this.Controls.Add(this.picture);
            this.Name = "OnvifLiveUserControl";
            this.Size = new System.Drawing.Size(547, 387);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).EndInit();
            this.PanelVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picture;
        private System.Windows.Forms.PictureBox panelFondoLogo;
        private System.Windows.Forms.Panel panelNoConnection;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomIn;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomOut;
        private System.Windows.Forms.Panel PanelVideo;
        private Vlc.DotNet.Forms.VlcControl vlcControl;
    }
}
