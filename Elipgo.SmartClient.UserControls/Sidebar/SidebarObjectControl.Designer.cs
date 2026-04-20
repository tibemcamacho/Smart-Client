namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarObjectControl
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
        	this.LabelName = new System.Windows.Forms.Label();
        	this.PanelIcon = new System.Windows.Forms.Panel();
        	this.LabelSubName = new System.Windows.Forms.Label();
            this.PanelStatus = new System.Windows.Forms.Panel();
			this.SuspendLayout();
            // 
     		// LabelName
            // 
            this.LabelName.AutoEllipsis = true;
            this.LabelName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelName.Location = new System.Drawing.Point(63, 14);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(179, 22);
            this.LabelName.TabIndex = 0;
            this.LabelName.Text = "label1";
            this.LabelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelIcon
            // 
            this.PanelIcon.BackColor = System.Drawing.Color.Transparent;
            this.PanelIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PanelIcon.Location = new System.Drawing.Point(20, 15);
            this.PanelIcon.Margin = new System.Windows.Forms.Padding(0);
            this.PanelIcon.Name = "PanelIcon";
            this.PanelIcon.Size = new System.Drawing.Size(40, 42);
            this.PanelIcon.TabIndex = 2;
			//
            // LabelSubName
            // 
            this.LabelSubName.Location = new System.Drawing.Point(63, 36);
            this.LabelSubName.Name = "LabelSubName";
            this.LabelSubName.Size = new System.Drawing.Size(179, 13);
            this.LabelSubName.TabIndex = 3;
            this.LabelSubName.Text = "label1";
            this.LabelSubName.Visible = false;
            // 
            //
            // PanelStatus
            // 
            this.PanelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelStatus.BackColor = System.Drawing.Color.Transparent;
            this.PanelStatus.Location = new System.Drawing.Point(250, 31);
            this.PanelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.PanelStatus.Name = "PanelStatus";
            this.PanelStatus.Size = new System.Drawing.Size(10, 10);
            this.PanelStatus.TabIndex = 1;
            //
            // 
            // SidebarObjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.LabelSubName);
            this.Controls.Add(this.PanelIcon);
            this.Controls.Add(this.LabelName);
            this.Controls.Add(this.PanelStatus);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SidebarObjectControl";
            this.Size = new System.Drawing.Size(270, 72);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelIcon;
        private System.Windows.Forms.Panel PanelStatus;
        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.Label LabelSubName;
    }
}
