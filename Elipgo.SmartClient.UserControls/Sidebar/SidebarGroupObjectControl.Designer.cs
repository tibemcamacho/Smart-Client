namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarGroupObjectControl
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
            this.PanelAction = new System.Windows.Forms.Panel();
            this.LabelName = new System.Windows.Forms.Label();
            this.LabelOpenedCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PanelAction
            // 
            this.PanelAction.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.PanelAction.BackColor = System.Drawing.Color.Transparent;
            this.PanelAction.Location = new System.Drawing.Point(254, 30);
            this.PanelAction.Margin = new System.Windows.Forms.Padding(0);
            this.PanelAction.Name = "PanelAction";
            this.PanelAction.Size = new System.Drawing.Size(14, 12);
            this.PanelAction.TabIndex = 2;
            // 
            // LabelName
            // 
            this.LabelName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelName.AutoEllipsis = true;
            this.LabelName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelName.Location = new System.Drawing.Point(15, 0);
            this.LabelName.Margin = new System.Windows.Forms.Padding(0);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(215, 72);
            this.LabelName.TabIndex = 0;
            this.LabelName.Text = "label1";
            this.LabelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelOpenedCount
            // 
            this.LabelOpenedCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelOpenedCount.AutoEllipsis = true;
            this.LabelOpenedCount.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelOpenedCount.Location = new System.Drawing.Point(207, 51);
            this.LabelOpenedCount.Margin = new System.Windows.Forms.Padding(0);
            this.LabelOpenedCount.Name = "LabelOpenedCount";
            this.LabelOpenedCount.Size = new System.Drawing.Size(61, 21);
            this.LabelOpenedCount.TabIndex = 3;
            this.LabelOpenedCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LabelOpenedCount.Visible = false;
            // 
            // SidebarGroupObjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.LabelOpenedCount);
            this.Controls.Add(this.PanelAction);
            this.Controls.Add(this.LabelName);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "SidebarGroupObjectControl";
            this.Size = new System.Drawing.Size(277, 72);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelAction;
        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.Label LabelOpenedCount;
    }
}
