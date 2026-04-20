namespace Elipgo.SmartClient.UserControls.Dialog
{
    partial class DialogReadOnlyItemControl
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
            this.Icon = new System.Windows.Forms.Panel();
            this.LabelName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Icon
            // 
            this.Icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Icon.Location = new System.Drawing.Point(56, 54);
            this.Icon.Margin = new System.Windows.Forms.Padding(0);
            this.Icon.Name = "Icon";
            this.Icon.Size = new System.Drawing.Size(72, 72);
            this.Icon.TabIndex = 0;
            this.Icon.Click += new System.EventHandler(this.DialogReadOnlyItemControl_Click);
            // 
            // LabelName
            // 
            this.LabelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelName.ForeColor = System.Drawing.Color.White;
            this.LabelName.Location = new System.Drawing.Point(0, 158);
            this.LabelName.Margin = new System.Windows.Forms.Padding(0);
            this.LabelName.Name = "LabelName";
            this.LabelName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelName.Size = new System.Drawing.Size(180, 59);
            this.LabelName.TabIndex = 1;
            this.LabelName.Text = "LabelName";
            this.LabelName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LabelName.Click += new System.EventHandler(this.DialogReadOnlyItemControl_Click);
            // 
            // DialogReadOnlyItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelName);
            this.Controls.Add(this.Icon);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DialogReadOnlyItemControl";
            this.Size = new System.Drawing.Size(180, 220);
            this.Click += new System.EventHandler(this.DialogReadOnlyItemControl_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Icon;
        private System.Windows.Forms.Label LabelName;
    }
}
