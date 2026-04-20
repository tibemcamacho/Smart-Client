namespace Elipgo.SmartClient.UserControls.FaceObject
{
    partial class FaceElement
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
            this.PanelData = new System.Windows.Forms.Panel();
            this.LabelList = new System.Windows.Forms.Label();
            this.LabelDate = new System.Windows.Forms.Label();
            this.LabelName = new System.Windows.Forms.Label();
            this.PanelRecognition = new System.Windows.Forms.Panel();
            this.PanelIdentity = new System.Windows.Forms.Panel();
            this.PanelPercent = new System.Windows.Forms.Panel();
            this.LabelPercent = new System.Windows.Forms.Label();
            this.PanelData.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelData
            // 
            this.PanelData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelData.Controls.Add(this.LabelList);
            this.PanelData.Controls.Add(this.LabelDate);
            this.PanelData.Controls.Add(this.LabelName);
            this.PanelData.Location = new System.Drawing.Point(0, 103);
            this.PanelData.Margin = new System.Windows.Forms.Padding(0);
            this.PanelData.Name = "PanelData";
            this.PanelData.Size = new System.Drawing.Size(234, 39);
            this.PanelData.TabIndex = 0;
            // 
            // LabelList
            // 
            this.LabelList.AutoSize = true;
            this.LabelList.ForeColor = System.Drawing.Color.White;
            this.LabelList.Location = new System.Drawing.Point(154, 12);
            this.LabelList.Name = "LabelList";
            this.LabelList.Size = new System.Drawing.Size(35, 13);
            this.LabelList.TabIndex = 2;
            this.LabelList.Text = "label1";
            // 
            // LabelDate
            // 
            this.LabelDate.ForeColor = System.Drawing.Color.White;
            this.LabelDate.Location = new System.Drawing.Point(8, 25);
            this.LabelDate.Name = "LabelDate";
            this.LabelDate.Size = new System.Drawing.Size(109, 11);
            this.LabelDate.TabIndex = 1;
            this.LabelDate.Text = "label1";
            // 
            // LabelName
            // 
            this.LabelName.AutoSize = true;
            this.LabelName.ForeColor = System.Drawing.Color.White;
            this.LabelName.Location = new System.Drawing.Point(8, 5);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(35, 13);
            this.LabelName.TabIndex = 0;
            this.LabelName.Text = "label1";
            // 
            // PanelRecognition
            // 
            this.PanelRecognition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelRecognition.Location = new System.Drawing.Point(0, 0);
            this.PanelRecognition.Margin = new System.Windows.Forms.Padding(0);
            this.PanelRecognition.Name = "PanelRecognition";
            this.PanelRecognition.Size = new System.Drawing.Size(117, 103);
            this.PanelRecognition.TabIndex = 1;
            // 
            // PanelIdentity
            // 
            this.PanelIdentity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelIdentity.Location = new System.Drawing.Point(117, 0);
            this.PanelIdentity.Margin = new System.Windows.Forms.Padding(0);
            this.PanelIdentity.Name = "PanelIdentity";
            this.PanelIdentity.Size = new System.Drawing.Size(117, 103);
            this.PanelIdentity.TabIndex = 2;
            // 
            // PanelPercent
            // 
            this.PanelPercent.BackColor = System.Drawing.Color.Transparent;
            this.PanelPercent.ForeColor = System.Drawing.Color.White;
            this.PanelPercent.Location = new System.Drawing.Point(99, 32);
            this.PanelPercent.Margin = new System.Windows.Forms.Padding(0);
            this.PanelPercent.Name = "PanelPercent";
            this.PanelPercent.Size = new System.Drawing.Size(36, 36);
            this.PanelPercent.TabIndex = 3;
            // 
            // LabelPercent
            // 
            this.LabelPercent.ForeColor = System.Drawing.Color.White;
            this.LabelPercent.Location = new System.Drawing.Point(3, 10);
            this.LabelPercent.Name = "LabelPercent";
            this.LabelPercent.Size = new System.Drawing.Size(30, 17);
            this.LabelPercent.TabIndex = 0;
            this.LabelPercent.Text = "30%";
            this.LabelPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FaceElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelPercent);
            this.Controls.Add(this.PanelIdentity);
            this.Controls.Add(this.PanelRecognition);
            this.Controls.Add(this.PanelData);
            this.Controls.Add(this.LabelPercent);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FaceElement";
            this.Size = new System.Drawing.Size(234, 142);
            this.PanelData.ResumeLayout(false);
            this.PanelData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelData;
        private System.Windows.Forms.Panel PanelRecognition;
        private System.Windows.Forms.Panel PanelIdentity;
        private System.Windows.Forms.Panel PanelPercent;
        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.Label LabelDate;
        private System.Windows.Forms.Label LabelPercent;
        private System.Windows.Forms.Label LabelList;
    }
}
