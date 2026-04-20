namespace Elipgo.SmartClient.Drivers.GenericDriver
{
    partial class AxisLiveUserControl
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
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AxisLiveUserControl));
            this.ButtonZoomIn = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonZoomOut = new Bunifu.Framework.UI.BunifuImageButton();
            this.panelFondoLogo = new System.Windows.Forms.PictureBox();
            this.panelNoConnection = new System.Windows.Forms.Panel();
            this.ButtonTalk = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonTalk)).BeginInit();
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
            // AxisLiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ButtonTalk);
            this.Controls.Add(this.panelNoConnection);
            this.Controls.Add(this.panelFondoLogo);
            this.Controls.Add(this.ButtonZoomIn);
            this.Controls.Add(this.ButtonZoomOut);
            this.Name = "AxisLiveUserControl";
            this.Size = new System.Drawing.Size(450, 297);
            this.Load += new System.EventHandler(this.AxisLiveUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonZoomOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonTalk)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomIn;
        private Bunifu.Framework.UI.BunifuImageButton ButtonZoomOut;
        private System.Windows.Forms.PictureBox panelFondoLogo;
        private System.Windows.Forms.Panel panelNoConnection;
        private Bunifu.Framework.UI.BunifuImageButton ButtonTalk;
    }
}