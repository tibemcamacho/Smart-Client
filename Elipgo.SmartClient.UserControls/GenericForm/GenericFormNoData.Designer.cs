using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    partial class GenericFormNoData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericFormNoData));
            this.Label1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.Label2 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoEllipsis = false;
            this.Label1.CausesValidation = false;
            this.Label1.Cursor = null;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(385, 151);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(177, 31);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "_label1_";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //this.Label1.TextFormat = Bunifu.Framework.UI.BunifuCustomLabel.opt.TextFormattingOptions.Default;
            // 
            // Label2
            // 
            this.Label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Label2.AutoEllipsis = false;
            this.Label2.CausesValidation = false;
            this.Label2.Cursor = null;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Label2.ForeColor = System.Drawing.Color.White;
            this.Label2.Location = new System.Drawing.Point(233, 229);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(456, 23);
            this.Label2.TabIndex = 5;
            this.Label2.Text = "_label1_";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //this.Label2.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            this.Label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // GenericFormNoData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Name = "GenericFormNoData";
            this.Size = new System.Drawing.Size(1575, 560);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCustomLabel Label1;
        private Bunifu.Framework.UI.BunifuCustomLabel Label2;
    }
}
