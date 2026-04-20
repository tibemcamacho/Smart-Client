namespace Elipgo.SmartClient.Drivers.HCNet616
{
    partial class HikvisionLiveUserControl
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
                this._ptzUserControl.StopJoystick();
            }
            base.Dispose(disposing);
            this.Dispose();
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HikvisionLiveUserControl));
            this._picture = new System.Windows.Forms.PictureBox();
            this._buttonZoomOut = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonZoomIn = new Bunifu.Framework.UI.BunifuImageButton();
            this._panelFondoLogo = new System.Windows.Forms.PictureBox();
            this._panelConnection = new System.Windows.Forms.Panel();
            this._buttonTalk = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this._picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelFondoLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonTalk)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this._picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._picture.Location = new System.Drawing.Point(0, 0);
            this._picture.Margin = new System.Windows.Forms.Padding(0);
            this._picture.Name = "picture";
            this._picture.Size = new System.Drawing.Size(450, 297);
            this._picture.TabIndex = 0;
            this._picture.TabStop = false;
            // 
            // ButtonZoomOut
            // 
            this._buttonZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonZoomOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("ButtonZoomOut.Image")));
            this._buttonZoomOut.ImageActive = null;
            this._buttonZoomOut.Location = new System.Drawing.Point(415, 261);
            this._buttonZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this._buttonZoomOut.Name = "ButtonZoomOut";
            this._buttonZoomOut.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonZoomOut.TabIndex = 1;
            this._buttonZoomOut.TabStop = false;
            this._buttonZoomOut.Zoom = 10;
            //this.ButtonZoomOut.Click += new System.EventHandler(this.ButtonZoomOut_Click);
            // 
            // ButtonZoomIn
            // 
            this._buttonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonZoomIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("ButtonZoomIn.Image")));
            this._buttonZoomIn.ImageActive = null;
            this._buttonZoomIn.Location = new System.Drawing.Point(415, 228);
            this._buttonZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this._buttonZoomIn.Name = "ButtonZoomIn";
            this._buttonZoomIn.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonZoomIn.TabIndex = 2;
            this._buttonZoomIn.TabStop = false;
            this._buttonZoomIn.Zoom = 10;
            //this.ButtonZoomIn.Click += new System.EventHandler(this.ButtonZoomIn_Click);
            // 
            // panelFondoLogo
            // 
            //this.panelFondoLogo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this._panelFondoLogo.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this._panelFondoLogo.Location = new System.Drawing.Point(46, 48);
            this._panelFondoLogo.Name = "panelFondoLogo";
            this._panelFondoLogo.Size = new System.Drawing.Size(274, 145);
            this._panelFondoLogo.TabIndex = 6;
            this._panelFondoLogo.TabStop = false;
            // 
            // ButtonTalk
            // 
            this._buttonTalk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonTalk.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonTalk.Image = ((System.Drawing.Image)(resources.GetObject("ButtonTalk.Image")));
            this._buttonTalk.ImageActive = null;
            this._buttonTalk.Location = new System.Drawing.Point(9, 261);
            this._buttonTalk.Margin = new System.Windows.Forms.Padding(0);
            this._buttonTalk.Name = "ButtonTalk";
            this._buttonTalk.Size = new System.Drawing.Size(24, 24);
            this._buttonTalk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonTalk.TabIndex = 7;
            this._buttonTalk.TabStop = false;
            this._buttonTalk.Zoom = 10;
            // 
            // panelconnection
            // 
            this._panelConnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this._panelConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom; // 17/Jun/2021 * vmon-4308 * ddvl  
            this._panelConnection.Location = new System.Drawing.Point(0, 0);
            this._panelConnection.Name = "panelconnection";
            this._panelConnection.Size = new System.Drawing.Size(200, 80); // 17/Jun/2021 * vmon-4308 * ddvl  
            this._panelConnection.TabIndex = 3;
            this._panelConnection.Visible = false;
            // 
            // HikvisionLiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._buttonTalk);
            this.Controls.Add(this._panelConnection);
            this.Controls.Add(this._buttonZoomIn);
            this.Controls.Add(this._buttonZoomOut);
            this.Controls.Add(this._picture);
            this.Controls.Add(this._panelFondoLogo);
            this.Name = "HikvisionLiveUserControl";
            this.Size = new System.Drawing.Size(450, 297);
            ((System.ComponentModel.ISupportInitialize)(this._picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelFondoLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonTalk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _picture;
        private Bunifu.Framework.UI.BunifuImageButton _buttonZoomOut;
        private Bunifu.Framework.UI.BunifuImageButton _buttonZoomIn;
        private System.Windows.Forms.PictureBox _panelFondoLogo;
        private System.Windows.Forms.Panel _panelConnection;
        private Bunifu.Framework.UI.BunifuImageButton _buttonTalk;
    }
}
