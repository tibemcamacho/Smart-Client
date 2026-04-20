namespace Elipgo.SmartClient.UserControls.Outputs
{
    partial class OutputItems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputItems));
            this.Label = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Switch = new Bunifu.Framework.UI.BunifuiOSSwitch();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label.Location = new System.Drawing.Point(3, 0);
            this.Label.Margin = new System.Windows.Forms.Padding(0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(164, 20);
            this.Label.TabIndex = 1;
            this.Label.Text = "label1";
            this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Label.Click += new System.EventHandler(this.Label_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Label);
            this.panel1.Controls.Add(this.Switch);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 24);
            this.panel1.TabIndex = 3;
            // 
            // Switch
            // 
            this.Switch.BackColor = System.Drawing.Color.Transparent;
            this.Switch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Switch.BackgroundImage")));
            this.Switch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Switch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Switch.Location = new System.Drawing.Point(173, 0);
            this.Switch.Margin = new System.Windows.Forms.Padding(0);
            this.Switch.Name = "Switch";
            this.Switch.OffColor = System.Drawing.Color.Gray;
            this.Switch.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.Switch.Size = new System.Drawing.Size(35, 20);
            this.Switch.TabIndex = 2;
            this.Switch.Value = true;
            this.Switch.Click += new System.EventHandler(this.Switch_Click);
            // 
            // OutputItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "OutputItems";
            this.Size = new System.Drawing.Size(214, 30);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.Panel panel1;
        private Bunifu.Framework.UI.BunifuiOSSwitch Switch;
    }
}
