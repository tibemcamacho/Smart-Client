namespace Elipgo.SmartClient.UserControls.UserProfile
{
    partial class ItemsProfile
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
            CustomDispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dbFlowLayoutPanel1 = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // dbFlowLayoutPanel1
            // 
            this.dbFlowLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.dbFlowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.dbFlowLayoutPanel1.Name = "dbFlowLayoutPanel1";
            this.dbFlowLayoutPanel1.Size = new System.Drawing.Size(204, 33);
            this.dbFlowLayoutPanel1.TabIndex = 1;
            // 
            // ItemsProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dbFlowLayoutPanel1);
            this.Name = "ItemsProfile";
            this.Size = new System.Drawing.Size(213, 44);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.DBFlowLayoutPanel dbFlowLayoutPanel1;
    }
}
