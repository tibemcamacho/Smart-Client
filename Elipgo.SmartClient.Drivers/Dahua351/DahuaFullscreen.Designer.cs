namespace Elipgo.SmartClient.Drivers.Dahua351
{
    partial class DahuaFullscreen
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
            this.Dispose();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DahuaFullscreen));
            this.PictureFullscreen = new System.Windows.Forms.PictureBox();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.bClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PictureFullscreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureFullscreen
            // 
            this.PictureFullscreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureFullscreen.Location = new System.Drawing.Point(0, 0);
            this.PictureFullscreen.Name = "PictureFullscreen";
            this.PictureFullscreen.Size = new System.Drawing.Size(800, 450);
            this.PictureFullscreen.TabIndex = 0;
            this.PictureFullscreen.TabStop = false;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.BackColor = System.Drawing.Color.Transparent;
            this.ButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this.ButtonClose.ImageActive = null;
            this.ButtonClose.Location = new System.Drawing.Point(753, 12);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(35, 38);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClose.TabIndex = 1;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Visible = false;
            this.ButtonClose.Zoom = 10;
            // 
            // bClose
            // 
            this.bClose.Location = new System.Drawing.Point(-10, -10);
            this.bClose.Margin = new System.Windows.Forms.Padding(0);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(0, 0);
            this.bClose.TabIndex = 2;
            this.bClose.Text = "button1";
            this.bClose.UseVisualStyleBackColor = true;
            // 
            // DahuaFullscreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.PictureFullscreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DahuaFullscreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DahuaFullscreen";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.PictureFullscreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureFullscreen;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
        private System.Windows.Forms.Button bClose;
    }
}