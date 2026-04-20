namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    partial class ElementContainerControl
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
            this.Dispose();
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelElement = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // PanelElement
            // 
            this.PanelElement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelElement.Location = new System.Drawing.Point(0, 0);
            this.PanelElement.Margin = new System.Windows.Forms.Padding(0);
            this.PanelElement.Name = "PanelElement";
            this.PanelElement.Size = new System.Drawing.Size(499, 321);
            this.PanelElement.TabIndex = 0;
            // 
            // ElementContainerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelElement);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ElementContainerControl";
            this.Size = new System.Drawing.Size(499, 321);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelElement;
    }
}
