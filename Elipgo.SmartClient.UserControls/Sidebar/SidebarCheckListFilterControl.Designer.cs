using Elipgo.SmartClient.Common;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarCheckListControl
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
            this.panCheckListOption = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // panCheckListOption
            // 
            this.panCheckListOption.Location = new System.Drawing.Point(0, 3);
            this.panCheckListOption.Name = "panCheckListOption";
            this.panCheckListOption.Size = new System.Drawing.Size(203, 174);
            this.panCheckListOption.TabIndex = 0;
            // 
            // SidebarCheckListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.panCheckListOption);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "SidebarCheckListControl";
            this.Size = new System.Drawing.Size(203, 180);
            this.VisibleChanged += new System.EventHandler(this.SidebarCheckListControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private DBFlowLayoutPanel panCheckListOption;
    }
}
