namespace Elipgo.SmartClient.Drivers.Axis
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
            this._buttonZoomIn = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonZoomOut = new Bunifu.Framework.UI.BunifuImageButton();
            this._panelFondoLogo = new System.Windows.Forms.PictureBox();
            this._panelNoConnection = new System.Windows.Forms.Panel();
            this._buttonTalk = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelFondoLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonTalk)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonZoomIn
            // 
            this._buttonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonZoomIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("ButtonZoomIn.Image")));
            this._buttonZoomIn.ImageActive = null;
            this._buttonZoomIn.Location = new System.Drawing.Point(658, 317);
            this._buttonZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this._buttonZoomIn.Name = "ButtonZoomIn";
            this._buttonZoomIn.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonZoomIn.TabIndex = 4;
            this._buttonZoomIn.TabStop = false;
            this._buttonZoomIn.Zoom = 10;
            this._buttonZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomIn_MouseDown);
            this._buttonZoomIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomIn_MouseUp);
            // 
            // ButtonZoomOut
            // 
            this._buttonZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonZoomOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("ButtonZoomOut.Image")));
            this._buttonZoomOut.ImageActive = null;
            this._buttonZoomOut.Location = new System.Drawing.Point(658, 350);
            this._buttonZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this._buttonZoomOut.Name = "ButtonZoomOut";
            this._buttonZoomOut.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonZoomOut.TabIndex = 3;
            this._buttonZoomOut.TabStop = false;
            this._buttonZoomOut.Zoom = 10;
            this._buttonZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomOut_MouseDown);
            this._buttonZoomOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonZoomOut_MouseUp);
            // 
            // panelFondoLogo
            // 
            this._panelFondoLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this._panelFondoLogo.Location = new System.Drawing.Point(46, 48);
            this._panelFondoLogo.Name = "panelFondoLogo";
            this._panelFondoLogo.Size = new System.Drawing.Size(274, 145);
            this._panelFondoLogo.TabIndex = 6;
            this._panelFondoLogo.TabStop = false;
            this._panelFondoLogo.Visible = false;
            // 
            // panelNoConnection
            // 
            this._panelNoConnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this._panelNoConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._panelNoConnection.Location = new System.Drawing.Point(96, 79);
            this._panelNoConnection.Margin = new System.Windows.Forms.Padding(0);
            this._panelNoConnection.Name = "panelNoConnection";
            this._panelNoConnection.Size = new System.Drawing.Size(200, 38);
            this._panelNoConnection.TabIndex = 7;
            // 
            // ButtonTalk
            // 
            this._buttonTalk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._buttonTalk.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.ButtonTalk.Image = ((System.Drawing.Image)(resources.GetObject("ButtonTalk.Image")));
            this._buttonTalk.ImageActive = null;
            this._buttonTalk.Location = new System.Drawing.Point(15, 196);
            this._buttonTalk.Margin = new System.Windows.Forms.Padding(0);
            this._buttonTalk.Name = "ButtonTalk";
            this._buttonTalk.Size = new System.Drawing.Size(24, 24);
            this._buttonTalk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonTalk.TabIndex = 7;
            this._buttonTalk.TabStop = false;
            this._buttonTalk.Zoom = 10;
            this._buttonTalk.Image = Common.Properties.FileResources.icon_micr_on;
            this._buttonTalk.Visible = false;
            // 
            // panelNoConnection
            // 
            this._panelNoConnection.BackgroundImage = global::Elipgo.SmartClient.Drivers.Properties.Resources.reconnecting_es;
            this._panelNoConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._panelNoConnection.Location = new System.Drawing.Point(96, 79);
            this._panelNoConnection.Margin = new System.Windows.Forms.Padding(0);
            this._panelNoConnection.Name = "panelNoConnection";
            this._panelNoConnection.Size = new System.Drawing.Size(200, 38);
            this._panelNoConnection.TabIndex = 7;
            // 
            // AxisLiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._buttonTalk);
            this.Controls.Add(this._panelNoConnection);
            this.Controls.Add(this._buttonZoomIn);
            this.Controls.Add(this._buttonZoomOut);
            this.Controls.Add(this._panelFondoLogo);
            this.Name = "AxisLiveUserControl";
            this.Size = new System.Drawing.Size(393, 239);
            this.Load += new System.EventHandler(this.AxisLiveUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonZoomOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._panelFondoLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._buttonTalk)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private Bunifu.Framework.UI.BunifuImageButton _buttonZoomIn;
        private Bunifu.Framework.UI.BunifuImageButton _buttonZoomOut;
        private Bunifu.Framework.UI.BunifuImageButton _buttonTalk;
        private System.Windows.Forms.PictureBox _panelFondoLogo;
        private System.Windows.Forms.Panel _panelNoConnection;
    }
}
