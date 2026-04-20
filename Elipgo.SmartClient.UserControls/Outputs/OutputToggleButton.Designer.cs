namespace Elipgo.SmartClient.UserControls.Outputs
{
    partial class OutputToggleButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputToggleButton));
            this.Button = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.Button)).BeginInit();
            this.SuspendLayout();
            // 
            // Button
            // 
            this.Button.ErrorImage = ((System.Drawing.Image)(resources.GetObject("Remove.ErrorImage")));
            this.Button.Image = ((System.Drawing.Image)(resources.GetObject("Remove.Image")));
            this.Button.ImageActive = null;
            this.Button.InitialImage = ((System.Drawing.Image)(resources.GetObject("Remove.InitialImage")));
            this.Button.Location = new System.Drawing.Point(0, 0);
            this.Button.Name = "Button";
            this.Button.Size = new System.Drawing.Size(24, 24);
            this.Button.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Button.TabIndex = 0;
            this.Button.TabStop = false;
            this.Button.Zoom = 10;
            this.Button.Click += new System.EventHandler(this.Button_Click);
            // 
            // OutputToggleButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Controls.Add(this.Button); 
            this.Name = "OutputToggleButton";
            this.Size = new System.Drawing.Size(24, 24);
            this.ResumeLayout(false);

        }

        #endregion

        public Bunifu.Framework.UI.BunifuImageButton Button;
    }
}
