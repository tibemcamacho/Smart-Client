namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    partial class ElementFaceControl
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
            //base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelPicture = new System.Windows.Forms.Panel();
            this.PanelList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // PanelPicture
            // 
            this.PanelPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelPicture.Location = new System.Drawing.Point(0, 0);
            this.PanelPicture.Margin = new System.Windows.Forms.Padding(0);
            this.PanelPicture.Name = "PanelPicture";
            this.PanelPicture.Size = new System.Drawing.Size(290, 213);
            this.PanelPicture.TabIndex = 2;
            // 
            // PanelList
            // 
            this.PanelList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelList.Location = new System.Drawing.Point(290, 0);
            this.PanelList.Margin = new System.Windows.Forms.Padding(0);
            this.PanelList.Name = "PanelList";
            this.PanelList.Size = new System.Drawing.Size(117, 213);
            this.PanelList.TabIndex = 3;
            // 
            // ElementFaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelList);
            this.Controls.Add(this.PanelPicture);
            this.Name = "ElementFaceControl";
            this.Size = new System.Drawing.Size(407, 213);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PanelPicture;
        private System.Windows.Forms.FlowLayoutPanel PanelList;
    }
}
