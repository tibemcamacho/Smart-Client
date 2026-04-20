namespace Elipgo.SmartClient.UserControls.Sidebar.Element
{
    partial class ItemCheckList
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
            this.fBtnCheck = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // fBtnCheck
            // 
            this.fBtnCheck.BackColor = System.Drawing.Color.Transparent;
            this.fBtnCheck.BackgroundImage = global::Elipgo.SmartClient.Common.Properties.FileResources.icon_filter;
            this.fBtnCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fBtnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fBtnCheck.Location = new System.Drawing.Point(1, 1);
            this.fBtnCheck.Name = "fBtnCheck";
            this.fBtnCheck.Size = new System.Drawing.Size(24, 24);
            this.fBtnCheck.TabIndex = 12;
            this.fBtnCheck.Click += new System.EventHandler(this.fBtnCheck_Click);
            this.fBtnCheck.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fBtnCheck_MouseClick);
            this.fBtnCheck.MouseHover += new System.EventHandler(this.fBtnCheck_MouseHover);
            this.fBtnCheck.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fBtnCheck_MouseMove);
            // 
            // ItemCheckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.fBtnCheck);
            this.Name = "ItemCheckList";
            this.Size = new System.Drawing.Size(25, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel fBtnCheck;
    }
}
