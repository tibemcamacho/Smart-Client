namespace Elipgo.SmartClient.UserControls.Shared
{
    partial class ShowTabBarControl
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
            this.pictureBoxTopbarOpen = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopbarOpen)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTopbarOpen
            // 
            this.pictureBoxTopbarOpen.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxTopbarOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTopbarOpen.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTopbarOpen.Name = "pbTopbarOpen";
            this.pictureBoxTopbarOpen.Size = new System.Drawing.Size(35, 35);
            this.pictureBoxTopbarOpen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTopbarOpen.TabIndex = 0;
            this.pictureBoxTopbarOpen.TabStop = false;
            this.pictureBoxTopbarOpen.Click += new System.EventHandler(this.PictureBoxTopbarOpen_Click);
            // 
            // showTabBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBoxTopbarOpen);
            this.Name = "showTabBarControl";
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Size = new System.Drawing.Size(35, 35);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopbarOpen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxTopbarOpen;
    }
}
