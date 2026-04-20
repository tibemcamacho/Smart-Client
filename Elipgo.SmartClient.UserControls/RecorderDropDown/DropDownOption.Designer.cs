namespace Elipgo.SmartClient.UserControls.RecorderDropDown
{
    partial class DropDownOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropDownOption));
            this.ButtonItem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonItem
            // 
            this.ButtonItem.BackColor = System.Drawing.Color.Transparent;
            this.ButtonItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonItem.FlatAppearance.BorderSize = 0;
            this.ButtonItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.ButtonItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.ButtonItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonItem.ForeColor = System.Drawing.SystemColors.Control;
            this.ButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("ButtonItem.Image")));
            this.ButtonItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ButtonItem.Location = new System.Drawing.Point(0, 0);
            this.ButtonItem.Name = "ButtonItem";
            this.ButtonItem.Size = new System.Drawing.Size(150, 38);
            this.ButtonItem.TabIndex = 0;
            this.ButtonItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ButtonItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ButtonItem.UseVisualStyleBackColor = false;
            this.ButtonItem.Click += new System.EventHandler(this.ButtonItem_Click);
            // 
            // OptionDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ButtonItem);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "OptionDropDown";
            this.Size = new System.Drawing.Size(150, 38);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonItem;
    }
}
