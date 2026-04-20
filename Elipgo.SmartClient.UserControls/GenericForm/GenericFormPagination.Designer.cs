namespace Elipgo.SmartClient.UserControls.GenericForm
{
    partial class GenericFormPagination
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
            this.panelPage = new System.Windows.Forms.Panel();
            this.labelN = new System.Windows.Forms.Label();
            this.labelPagB = new System.Windows.Forms.Label();
            this.labelPag = new System.Windows.Forms.Label();
            this.labelNextPage = new System.Windows.Forms.Label();
            this.labelEndPage = new System.Windows.Forms.Label();
            this.labelStartPage = new System.Windows.Forms.Label();
            this.labelbackPage = new System.Windows.Forms.Label();
            this.labelActual = new System.Windows.Forms.Label();
            this.panelPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPage
            // 
            this.panelPage.Controls.Add(this.labelN);
            this.panelPage.Controls.Add(this.labelPagB);
            this.panelPage.Controls.Add(this.labelPag);
            this.panelPage.Controls.Add(this.labelNextPage);
            this.panelPage.Controls.Add(this.labelEndPage);
            this.panelPage.Controls.Add(this.labelStartPage);
            this.panelPage.Controls.Add(this.labelbackPage);
            this.panelPage.Controls.Add(this.labelActual);
            this.panelPage.Location = new System.Drawing.Point(3, 3);
            this.panelPage.Name = "panelPage";
            this.panelPage.Size = new System.Drawing.Size(180, 36);
            this.panelPage.TabIndex = 23;
            this.panelPage.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPage_Paint);
            // 
            // labelN
            // 
            this.labelN.AutoSize = true;
            this.labelN.BackColor = System.Drawing.Color.Transparent;
            this.labelN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelN.ForeColor = System.Drawing.Color.White;
            this.labelN.Location = new System.Drawing.Point(105, 5);
            this.labelN.Name = "labelN";
            this.labelN.Size = new System.Drawing.Size(13, 15);
            this.labelN.TabIndex = 34;
            this.labelN.Text = "..";
            // 
            // labelPagB
            // 
            this.labelPagB.AutoSize = true;
            this.labelPagB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelPagB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPagB.ForeColor = System.Drawing.Color.White;
            this.labelPagB.Location = new System.Drawing.Point(52, 5);
            this.labelPagB.Name = "labelPagB";
            this.labelPagB.Size = new System.Drawing.Size(14, 15);
            this.labelPagB.TabIndex = 33;
            this.labelPagB.Text = "0";
            this.labelPagB.Click += new System.EventHandler(this.labelPagB_Click);
            // 
            // labelPag
            // 
            this.labelPag.AutoSize = true;
            this.labelPag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelPag.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPag.ForeColor = System.Drawing.Color.White;
            this.labelPag.Location = new System.Drawing.Point(92, 5);
            this.labelPag.Name = "labelPag";
            this.labelPag.Size = new System.Drawing.Size(14, 15);
            this.labelPag.TabIndex = 32;
            this.labelPag.Text = "0";
            this.labelPag.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelNextPage
            // 
            this.labelNextPage.AutoSize = true;
            this.labelNextPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelNextPage.ForeColor = System.Drawing.Color.White;
            this.labelNextPage.Location = new System.Drawing.Point(116, 6);
            this.labelNextPage.Name = "labelNextPage";
            this.labelNextPage.Size = new System.Drawing.Size(13, 13);
            this.labelNextPage.TabIndex = 31;
            this.labelNextPage.Text = ">";
            this.labelNextPage.Click += new System.EventHandler(this.labelNextPage_Click);
            // 
            // labelEndPage
            // 
            this.labelEndPage.AutoSize = true;
            this.labelEndPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelEndPage.ForeColor = System.Drawing.Color.White;
            this.labelEndPage.Location = new System.Drawing.Point(136, 6);
            this.labelEndPage.Name = "labelEndPage";
            this.labelEndPage.Size = new System.Drawing.Size(19, 13);
            this.labelEndPage.TabIndex = 30;
            this.labelEndPage.Text = ">>";
            this.labelEndPage.Click += new System.EventHandler(this.labelEndPage_Click);
            // 
            // labelStartPage
            // 
            this.labelStartPage.AutoSize = true;
            this.labelStartPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelStartPage.ForeColor = System.Drawing.Color.White;
            this.labelStartPage.Location = new System.Drawing.Point(12, 6);
            this.labelStartPage.Name = "labelStartPage";
            this.labelStartPage.Size = new System.Drawing.Size(19, 13);
            this.labelStartPage.TabIndex = 29;
            this.labelStartPage.Text = "<<";
            this.labelStartPage.Click += new System.EventHandler(this.labelStartPage_Click);
            // 
            // labelbackPage
            // 
            this.labelbackPage.AutoSize = true;
            this.labelbackPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelbackPage.ForeColor = System.Drawing.Color.White;
            this.labelbackPage.Location = new System.Drawing.Point(35, 6);
            this.labelbackPage.Name = "labelbackPage";
            this.labelbackPage.Size = new System.Drawing.Size(13, 13);
            this.labelbackPage.TabIndex = 28;
            this.labelbackPage.Text = "<";
            this.labelbackPage.Click += new System.EventHandler(this.labelbackPage_Click);
            // 
            // labelActual
            // 
            this.labelActual.AutoSize = true;
            this.labelActual.BackColor = System.Drawing.Color.Transparent;
            this.labelActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActual.ForeColor = System.Drawing.Color.White;
            this.labelActual.Image = global::Elipgo.SmartClient.UserControls.Properties.Resources.elipse;
            this.labelActual.Location = new System.Drawing.Point(71, 5);
            this.labelActual.Name = "labelActual";
            this.labelActual.Size = new System.Drawing.Size(14, 15);
            this.labelActual.TabIndex = 27;
            this.labelActual.Text = "0";
            this.labelActual.Click += new System.EventHandler(this.labelTotal_Click);
            // 
            // GenericFormPagination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelPage);
            this.Name = "GenericFormPagination";
            this.Size = new System.Drawing.Size(183, 39);
            this.panelPage.ResumeLayout(false);
            this.panelPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPage;
        private System.Windows.Forms.Label labelNextPage;
        private System.Windows.Forms.Label labelEndPage;
        private System.Windows.Forms.Label labelStartPage;
        private System.Windows.Forms.Label labelbackPage;
        private System.Windows.Forms.Label labelActual;
        private System.Windows.Forms.Label labelPag;
        private System.Windows.Forms.Label labelPagB;
        private System.Windows.Forms.Label labelN;
    }
}
