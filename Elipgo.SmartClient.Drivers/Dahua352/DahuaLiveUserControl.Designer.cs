namespace Elipgo.SmartClient.Drivers.Dahua352
{
    partial class DahuaLiveUserControl
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
            if (PtzStatus == true)
            {//si al momneto de cerrar el contenedor sigue activo el comando ptz desactivo el joystick
                if (this._ptzUserControl != null)
                    this._ptzUserControl.StopJoystick();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DahuaLiveUserControl));
            this.picture = new System.Windows.Forms.PictureBox();
            this.ButtonZoomOut = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonZoomIn = new Bunifu.Framework.UI.BunifuImageButton();
            this.panelFondoLogo = new System.Windows.Forms.PictureBox();
            this.panelconnection = new System.Windows.Forms.Panel();
            this.ButtonTalk = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonTalk)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picture.Location = new System.Drawing.Point(0, 0);
            this.picture.Margin = new System.Windows.Forms.Padding(0);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(450, 297);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
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
            this.ButtonZoomOut.TabIndex = 1;
            this.ButtonZoomOut.TabStop = false;
            this.ButtonZoomOut.Zoom = 10;
            this.ButtonZoomOut.Click += new System.EventHandler(this.ButtonZoomOut_Click);
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
            this.ButtonZoomIn.TabIndex = 2;
            this.ButtonZoomIn.TabStop = false;
            this.ButtonZoomIn.Zoom = 10;
            this.ButtonZoomIn.Click += new System.EventHandler(this.ButtonZoomIn_Click);
            // 
            // panelFondoLogo
            // 
            //this.panelFondoLogo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelFondoLogo.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.panelFondoLogo.Location = new System.Drawing.Point(46, 48);
            this.panelFondoLogo.Name = "panelFondoLogo";
            this.panelFondoLogo.Size = new System.Drawing.Size(274, 145);
            this.panelFondoLogo.TabIndex = 6;
            this.panelFondoLogo.TabStop = false;
            // 
            // ButtonTalk
            // 
            this.ButtonTalk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonTalk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonTalk.Image = ((System.Drawing.Image)(resources.GetObject("ButtonTalk.Image")));
            this.ButtonTalk.ImageActive = null;
            this.ButtonTalk.Location = new System.Drawing.Point(9, 261);
            this.ButtonTalk.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonTalk.Name = "ButtonTalk";
            this.ButtonTalk.Size = new System.Drawing.Size(24, 24);
            this.ButtonTalk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonTalk.TabIndex = 7;
            this.ButtonTalk.TabStop = false;
            this.ButtonTalk.Zoom = 10;
            // 
            // panelconnection
            // 
            this.panelconnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this.panelconnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom; // 17/Jun/2021 * vmon-4308 * ddvl  
            this.panelconnection.Location = new System.Drawing.Point(0, 0);
            this.panelconnection.Name = "panelconnection";
            this.panelconnection.Size = new System.Drawing.Size(200, 80); // 17/Jun/2021 * vmon-4308 * ddvl  
            this.panelconnection.TabIndex = 3;
            // 
            // DahuaLiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ButtonTalk);
            this.Controls.Add(this.ButtonZoomIn);
            this.Controls.Add(this.ButtonZoomOut);
            this.Controls.Add(this.picture);
            this.Controls.Add(this.panelFondoLogo);
            this.Controls.Add(this.panelconnection);
            this.Name = "DahuaLiveUserControl";
            this.Size = new System.Drawing.Size(450, 297);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonTalk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picture;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomOut;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomIn;
        private System.Windows.Forms.PictureBox panelFondoLogo;
        private System.Windows.Forms.Panel panelconnection;
        private Bunifu.Framework.UI.BunifuImageButton ButtonTalk;
    }
}
