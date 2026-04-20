namespace Elipgo.SmartClient.Drivers.GenericDriver
{
    partial class VlcLiveUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VlcLiveUserControl));
            this.panelFondoLogo = new System.Windows.Forms.PictureBox();
            this.panelNoConnection = new System.Windows.Forms.Panel();
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.ButtonZoomIn = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonZoomOut = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonZoomIn
            // 
            this.ButtonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonZoomIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("ButtonZoomIn.Image")));
            this.ButtonZoomIn.ImageActive = null;
            this.ButtonZoomIn.Location = new System.Drawing.Point(415, 228);
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
            this.ButtonZoomOut.Location = new System.Drawing.Point(415, 261);
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
            // panelFondoLogo
            // 
            this.panelFondoLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.panelFondoLogo.Location = new System.Drawing.Point(88, 76);
            this.panelFondoLogo.Name = "panelFondoLogo";
            this.panelFondoLogo.Size = new System.Drawing.Size(274, 145);
            this.panelFondoLogo.TabIndex = 7;
            this.panelFondoLogo.TabStop = false;
            this.panelFondoLogo.Visible = false;
            // 
            // panelNoConnection
            // 
            this.panelNoConnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this.panelNoConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelNoConnection.Location = new System.Drawing.Point(125, 129);
            this.panelNoConnection.Margin = new System.Windows.Forms.Padding(0);
            this.panelNoConnection.Name = "panelNoConnection";
            this.panelNoConnection.Size = new System.Drawing.Size(200, 38);
            this.panelNoConnection.TabIndex = 8;
            // 
            // vlcControl
            // 
            this.vlcControl.BackColor = System.Drawing.Color.Black;
            this.vlcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcControl.Location = new System.Drawing.Point(0, 0);
            this.vlcControl.Name = "vlcControl";
            this.vlcControl.Size = new System.Drawing.Size(547, 387);
            this.vlcControl.Spu = -1;
            this.vlcControl.TabIndex = 7;
            this.vlcControl.Text = "vlcControl";
            //this.vlcControl.VlcLibDirectory = ((System.IO.DirectoryInfo)(resources.GetObject("VLCPlayer.VlcLibDirectory")));
            this.vlcControl.VlcLibDirectory = null;
            this.vlcControl.VlcMediaplayerOptions = null;
        //        new string[] {
        //"--network-caching=200",
        //"--rtsp-frame-buffer-size=500000"};
            this.vlcControl.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.OnVlcControlNeedLibDirectory);
            // 
            // VlcLiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelNoConnection);
            this.Controls.Add(this.panelFondoLogo);
            this.Controls.Add(this.ButtonZoomIn);
            this.Controls.Add(this.ButtonZoomOut);
            this.Controls.Add(this.vlcControl);
            this.Name = "VlcLiveUserControl";
            this.Size = new System.Drawing.Size(547, 387);

            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox panelFondoLogo;
        private System.Windows.Forms.Panel panelNoConnection;
        private Vlc.DotNet.Forms.VlcControl vlcControl;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomIn;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomOut;
    }
}
