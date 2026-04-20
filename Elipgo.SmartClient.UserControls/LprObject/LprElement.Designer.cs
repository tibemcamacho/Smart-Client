namespace Elipgo.SmartClient.UserControls.LprObject
{
    partial class LprElement
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
            this._labelList = new System.Windows.Forms.Label();
            this._labelPlate = new System.Windows.Forms.Label();
            this._labelDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabelList
            // 
            this._labelList.BackColor = System.Drawing.Color.Transparent;
            this._labelList.ForeColor = System.Drawing.Color.White;
            this._labelList.Location = new System.Drawing.Point(5, 13);
            this._labelList.Margin = new System.Windows.Forms.Padding(0);
            this._labelList.Name = "LabelList";
            this._labelList.Size = new System.Drawing.Size(224, 15);
            this._labelList.TabIndex = 0;
            this._labelList.Text = "label1";
            this._labelList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelPlate
            // 
            this._labelPlate.BackColor = System.Drawing.Color.Transparent;
            this._labelPlate.ForeColor = System.Drawing.Color.White;
            this._labelPlate.Location = new System.Drawing.Point(5, 32);
            this._labelPlate.Margin = new System.Windows.Forms.Padding(0);
            this._labelPlate.Name = "LabelPlate";
            this._labelPlate.Size = new System.Drawing.Size(224, 52);
            this._labelPlate.TabIndex = 1;
            this._labelPlate.Text = "label2";
            this._labelPlate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelDate
            // 
            this._labelDate.BackColor = System.Drawing.Color.Transparent;
            this._labelDate.ForeColor = System.Drawing.Color.White;
            this._labelDate.Location = new System.Drawing.Point(5, 84);
            this._labelDate.Margin = new System.Windows.Forms.Padding(0);
            this._labelDate.Name = "LabelDate";
            this._labelDate.Size = new System.Drawing.Size(224, 15);
            this._labelDate.TabIndex = 2;
            this._labelDate.Text = "label3";
            this._labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LprElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._labelDate);
            this.Controls.Add(this._labelPlate);
            this.Controls.Add(this._labelList);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LprElement";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(234, 107);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _labelList;
        private System.Windows.Forms.Label _labelPlate;
        private System.Windows.Forms.Label _labelDate;
    }
}
