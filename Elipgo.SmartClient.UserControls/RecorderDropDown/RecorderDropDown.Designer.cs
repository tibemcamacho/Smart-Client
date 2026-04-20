namespace Elipgo.SmartClient.UserControls.RecorderDropDown
{
    partial class RecorderDropDown
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecorderDropDown));
            this.ButtonOptionSelected = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonOptionSelected
            // 
            this.ButtonOptionSelected.BackColor = System.Drawing.Color.Transparent;
            this.ButtonOptionSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOptionSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonOptionSelected.FlatAppearance.BorderSize = 0;
            this.ButtonOptionSelected.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ButtonOptionSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonOptionSelected.ForeColor = System.Drawing.Color.White;
            this.ButtonOptionSelected.Image = ((System.Drawing.Image)(resources.GetObject("ButtonOptionSelected.Image")));
            this.ButtonOptionSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonOptionSelected.Location = new System.Drawing.Point(0, 0);
            this.ButtonOptionSelected.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonOptionSelected.Name = "ButtonOptionSelected";
            this.ButtonOptionSelected.Size = new System.Drawing.Size(250, 38);
            this.ButtonOptionSelected.TabIndex = 1;
            this.ButtonOptionSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ButtonOptionSelected.UseVisualStyleBackColor = false;
            this.ButtonOptionSelected.Click += new System.EventHandler(this.ButtonOptionSelected_Click);
            // 
            // RecorderDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ButtonOptionSelected);
            this.ForeColor = System.Drawing.Color.Snow;
            this.Name = "RecorderDropDown";
            this.Size = new System.Drawing.Size(250, 38);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonOptionSelected;
    }
}
