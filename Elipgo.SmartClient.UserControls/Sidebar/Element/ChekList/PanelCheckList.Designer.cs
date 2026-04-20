namespace Elipgo.SmartClient.UserControls.Sidebar.Element.ChekList
{
    partial class PanelCheckList
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
            this.dbFlowLayoutPanel1 = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // dbFlowLayoutPanel1
            // 
            this.dbFlowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.dbFlowLayoutPanel1.Name = "dbFlowLayoutPanel1";
            this.dbFlowLayoutPanel1.Size = new System.Drawing.Size(240, 27);
            this.dbFlowLayoutPanel1.TabIndex = 0;
            // 
            // PanelCheckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dbFlowLayoutPanel1);
            this.Name = "PanelCheckList";
            this.Size = new System.Drawing.Size(240, 50);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.DBFlowLayoutPanel dbFlowLayoutPanel1;
    }
}
