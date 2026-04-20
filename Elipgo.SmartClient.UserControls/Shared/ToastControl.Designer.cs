using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.UserProfile;
using System.Drawing;

namespace Elipgo.SmartClient.UserControls.Shared
{
    partial class ToastControl
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToastControl));
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            this.PanelHeader.SuspendLayout();
            // 
            // ToastControl
            // 
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Size = new System.Drawing.Size(350, 100);
            this.ShowInTaskbar = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Controls.Add(this.PanelHeader);
            // 
            // PanelHeader
            // 
            this.PanelHeader.BackColor = System.Drawing.Color.Black;
            this.PanelHeader.Controls.Add(this.ButtonClose);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top; // Ocupa todo el tope
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(this.Width, 32);
            this.PanelHeader.TabIndex = 27;

            // 
            // ButtonClose
            // 
            this.ButtonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonClose.BackColor = Color.Transparent;
            this.ButtonClose.Image = FileResources.icon_close;
            this.ButtonClose.Size = new System.Drawing.Size(24, 24);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClose.Location = new Point(this.Width - this.ButtonClose.Width - 5, 5);
            this.ButtonClose.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Zoom = 10;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);

            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
        private System.Windows.Forms.Panel PanelHeader;
    }
}
