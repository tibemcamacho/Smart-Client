namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarCheckBoxControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SidebarCheckBoxControl));
            this.CheckBox = new Bunifu.UI.WinForms.BunifuCheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabelName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CheckBox
            // 
            this.CheckBox.AllowBindingControlAnimation = true;
            this.CheckBox.AllowBindingControlColorChanges = false;
            this.CheckBox.AllowBindingControlLocation = true;
            this.CheckBox.AllowCheckBoxAnimation = false;
            this.CheckBox.AllowCheckmarkAnimation = true;
            this.CheckBox.AllowOnHoverStates = true;
            this.CheckBox.AutoCheck = true;
            this.CheckBox.BackColor = System.Drawing.Color.Gray;
            this.CheckBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CheckBox.BackgroundImage")));
            this.CheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CheckBox.BindingControl = null;
            this.CheckBox.BindingControlPosition = Bunifu.UI.WinForms.BunifuCheckBox.BindingControlPositions.Right;
            this.CheckBox.Checked = true;
            this.CheckBox.CheckState = Bunifu.UI.WinForms.BunifuCheckBox.CheckStates.Checked;
            this.CheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CheckBox.CustomCheckmarkImage = null;
            this.CheckBox.Location = new System.Drawing.Point(219, 15);
            this.CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.CheckBox.MinimumSize = new System.Drawing.Size(23, 21);
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.OnCheck.BorderColor = System.Drawing.Color.DodgerBlue;
            this.CheckBox.OnCheck.BorderRadius = 2;
            this.CheckBox.OnCheck.BorderThickness = 2;
            this.CheckBox.OnCheck.CheckBoxColor = System.Drawing.Color.DodgerBlue;
            this.CheckBox.OnCheck.CheckmarkColor = System.Drawing.Color.White;
            this.CheckBox.OnCheck.CheckmarkThickness = 2;
            this.CheckBox.OnDisable.BorderColor = System.Drawing.Color.LightGray;
            this.CheckBox.OnDisable.BorderRadius = 2;
            this.CheckBox.OnDisable.BorderThickness = 2;
            this.CheckBox.OnDisable.CheckBoxColor = System.Drawing.Color.Transparent;
            this.CheckBox.OnDisable.CheckmarkColor = System.Drawing.Color.LightGray;
            this.CheckBox.OnDisable.CheckmarkThickness = 2;
            this.CheckBox.OnHoverChecked.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.CheckBox.OnHoverChecked.BorderRadius = 2;
            this.CheckBox.OnHoverChecked.BorderThickness = 2;
            this.CheckBox.OnHoverChecked.CheckBoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.CheckBox.OnHoverChecked.CheckmarkColor = System.Drawing.Color.White;
            this.CheckBox.OnHoverChecked.CheckmarkThickness = 2;
            this.CheckBox.OnHoverUnchecked.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.CheckBox.OnHoverUnchecked.BorderRadius = 2;
            this.CheckBox.OnHoverUnchecked.BorderThickness = 1;
            this.CheckBox.OnHoverUnchecked.CheckBoxColor = System.Drawing.Color.Transparent;
            this.CheckBox.OnUncheck.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.CheckBox.OnUncheck.BorderRadius = 2;
            this.CheckBox.OnUncheck.BorderThickness = 1;
            this.CheckBox.OnUncheck.CheckBoxColor = System.Drawing.Color.Transparent;
            this.CheckBox.Size = new System.Drawing.Size(28, 28);
            this.CheckBox.Style = Bunifu.UI.WinForms.BunifuCheckBox.CheckBoxStyles.Bunifu;
            this.CheckBox.TabIndex = 0;
            this.CheckBox.ThreeState = false;
            this.CheckBox.ToolTipText = null;
            this.CheckBox.CheckedChanged += new System.EventHandler<Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs>(this.BunifuCheckBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LabelName);
            this.panel1.Controls.Add(this.CheckBox);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 59);
            this.panel1.TabIndex = 2;
            this.panel1.Click += new System.EventHandler(this.LabelName_Click);
            // 
            // LabelName
            // 
            this.LabelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LabelName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelName.Location = new System.Drawing.Point(21, 7);
            this.LabelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(190, 44);
            this.LabelName.TabIndex = 1;
            this.LabelName.Text = "label1";
            this.LabelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelName.Click += new System.EventHandler(this.LabelName_Click);
            // 
            // SidebarCheckBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SidebarCheckBoxControl";
            this.Size = new System.Drawing.Size(267, 60);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuCheckBox CheckBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabelName;
    }
}
