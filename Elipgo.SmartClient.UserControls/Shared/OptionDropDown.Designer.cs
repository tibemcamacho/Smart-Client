namespace Elipgo.SmartClient.UserControls.Shared
{
    partial class OptionDropDown
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
            this.PanelContainer = new System.Windows.Forms.Panel();
            this.OptionBtn = new System.Windows.Forms.Button();
            this.PanelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelContainer
            // 
            this.PanelContainer.BackColor = System.Drawing.Color.Transparent;
            this.PanelContainer.Controls.Add(this.OptionBtn);
            this.PanelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelContainer.Location = new System.Drawing.Point(0, 0);
            this.PanelContainer.Name = "PanelContainer";
            this.PanelContainer.Size = new System.Drawing.Size(150, 38);
            this.PanelContainer.TabIndex = 1;
            // 
            // OptionBtn
            // 
            this.OptionBtn.BackColor = System.Drawing.Color.Transparent;
            this.OptionBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OptionBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionBtn.FlatAppearance.BorderSize = 0;
            this.OptionBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.OptionBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.OptionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OptionBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.OptionBtn.Image = global::Elipgo.SmartClient.UserControls.Properties.Resources.icon_play;
            this.OptionBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OptionBtn.Location = new System.Drawing.Point(0, 0);
            this.OptionBtn.Name = "OptionBtn";
            this.OptionBtn.Size = new System.Drawing.Size(150, 38);
            this.OptionBtn.TabIndex = 0;
            this.OptionBtn.Text = "          Opcion";
            this.OptionBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OptionBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OptionBtn.UseVisualStyleBackColor = false;
            // 
            // OptionDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.PanelContainer);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "OptionDropDown";
            this.Size = new System.Drawing.Size(150, 38);
            this.PanelContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OptionBtn;
        private System.Windows.Forms.Panel PanelContainer;
    }
}
