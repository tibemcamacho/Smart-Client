using Elipgo.SmartClient.Common;

namespace Elipgo.SmartClient.UserControls.Vault
{
    partial class VaultItemCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VaultItemCard));
            this.IconPanel = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.Label1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.Label2 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.ProgressBar = new Bunifu.UI.Winforms.BunifuProgressBar();
            this.Label3 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.SuspendLayout();
            // 
            // IconPanel
            // 
            this.IconPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.IconPanel.CausesValidation = false;
            this.IconPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IconPanel.Location = new System.Drawing.Point(58, 39);
            this.IconPanel.Name = "IconPanel";
            this.IconPanel.Size = new System.Drawing.Size(70, 70);
            this.IconPanel.TabIndex = 0;
            this.IconPanel.Click += new System.EventHandler(this.VaultItemCard_Click);
            this.IconPanel.DoubleClick += new System.EventHandler(this.VaultItemCard_DoubleClick);
            // 
            // Label1
            // 
            this.Label1.CausesValidation = false;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(3, 116);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(172, 45);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "_label1_";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label1.Click += new System.EventHandler(this.VaultItemCard_Click);
            this.Label1.DoubleClick += new System.EventHandler(this.VaultItemCard_DoubleClick);
            // 
            // Label2
            // 
            this.Label2.CausesValidation = false;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(3, 158);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(172, 41);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "_label2_";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label2.Click += new System.EventHandler(this.VaultItemCard_Click);
            this.Label2.DoubleClick += new System.EventHandler(this.VaultItemCard_DoubleClick);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Animation = 0;
            this.ProgressBar.AnimationStep = 10;
            this.ProgressBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ProgressBar.BackgroundImage")));
            this.ProgressBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.ProgressBar.BorderRadius = 1;
            this.ProgressBar.BorderThickness = 1;
            this.ProgressBar.Location = new System.Drawing.Point(3, 223);
            this.ProgressBar.MaximumValue = 100;
            this.ProgressBar.MinimumValue = 0;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.ProgressBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.ProgressBar.ProgressColorLeft = System.Drawing.Color.DodgerBlue;
            this.ProgressBar.ProgressColorRight = System.Drawing.Color.DodgerBlue;
            this.ProgressBar.Size = new System.Drawing.Size(172, 10);
            this.ProgressBar.TabIndex = 7;
            this.ProgressBar.Value = 50;
            this.ProgressBar.Visible = false;
            this.ProgressBar.Click += new System.EventHandler(this.VaultItemCard_Click);
            this.ProgressBar.DoubleClick += new System.EventHandler(this.VaultItemCard_DoubleClick);
            // 
            // Label3
            // 
            this.Label3.CausesValidation = false;
            this.Label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Label3.ForeColor = System.Drawing.Color.White;
            this.Label3.Location = new System.Drawing.Point(4, 202);
            this.Label3.Name = "Label3";
            this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label3.Size = new System.Drawing.Size(172, 15);
            this.Label3.TabIndex = 8;
            this.Label3.Text = "_label3_";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label3.Visible = false;
            this.Label3.Click += new System.EventHandler(this.VaultItemCard_Click);
            this.Label3.DoubleClick += new System.EventHandler(this.VaultItemCard_DoubleClick);
            // 
            // VaultItemCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.CausesValidation = false;
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.IconPanel);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            //this.MinimumSize = new System.Drawing.Size(180, 232);
            this.Name = "VaultItemCard";
            this.Size = new System.Drawing.Size(180, 232);
            this.Click += new System.EventHandler(this.VaultItemCard_Click);
            this.DoubleClick += new System.EventHandler(this.VaultItemCard_DoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private DBFlowLayoutPanel IconPanel;
        private Bunifu.Framework.UI.BunifuCustomLabel Label1;
        private Bunifu.Framework.UI.BunifuCustomLabel Label2;
        //private Bunifu.Framework.UI.BunifuImageButton ButtonContexMenu;
        private Bunifu.UI.Winforms.BunifuProgressBar ProgressBar;
        private Bunifu.Framework.UI.BunifuCustomLabel Label3;
    }
}
