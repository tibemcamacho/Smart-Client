using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.AccessControlBar
{
    partial class AccessControlBarControl
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
            components = new System.ComponentModel.Container();
            this.ddlServersAC = new Bunifu.UI.WinForms.BunifuDropdown();
            this.ddlLabel = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.SuspendLayout();
            // 
            // ddlServersAC
            // 
            this.ddlServersAC.BackColor = System.Drawing.Color.Transparent;
            this.ddlServersAC.BorderRadius = 1;
            this.ddlServersAC.Color = System.Drawing.Color.Transparent;
            this.ddlServersAC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ddlServersAC.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.ddlServersAC.DisabledColor = System.Drawing.Color.Gray;
            this.ddlServersAC.DisplayMember = "Name";
            this.ddlServersAC.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlServersAC.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.ddlServersAC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlServersAC.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.ddlServersAC.FillDropDown = false;
            this.ddlServersAC.FillIndicator = false;
            this.ddlServersAC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ddlServersAC.ForeColor = System.Drawing.Color.White;
            this.ddlServersAC.FormattingEnabled = true;
            this.ddlServersAC.Icon = null;
            this.ddlServersAC.IndicatorColor = System.Drawing.Color.White;
            this.ddlServersAC.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.ddlServersAC.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.ddlServersAC.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.ddlServersAC.ItemForeColor = System.Drawing.Color.White;
            this.ddlServersAC.ItemHeight = 26;
            this.ddlServersAC.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.ddlServersAC.Location = new System.Drawing.Point(24, 4);
            this.ddlServersAC.Margin = new System.Windows.Forms.Padding(0);
            this.ddlServersAC.Name = "ddlServersAC";
            this.ddlServersAC.Size = new System.Drawing.Size(187, 37);
            this.ddlServersAC.TabIndex = 14;
            this.ddlServersAC.Text = null;
            this.ddlServersAC.ValueMember = "Key";
            // 
            // LabelVaultBarTitle
            // 
            this.ddlLabel.AutoEllipsis = false;
            this.ddlLabel.AutoSize = false;
            this.ddlLabel.CausesValidation = false;
            this.ddlLabel.Cursor = null;
            this.ddlLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ddlLabel.ForeColor = System.Drawing.Color.White;
            this.ddlLabel.Location = new System.Drawing.Point(3, 15);
            this.ddlLabel.Name = "LabelGridTitle";
            this.ddlLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddlLabel.Size = new System.Drawing.Size(152, 45);
            this.ddlLabel.TabIndex = 3;
            this.ddlLabel.Text = "Server:";
            this.ddlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AccessControlBarControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ddlServersAC);
            this.Controls.Add(this.ddlLabel);
            this.Name = "AccessControlBarControl";
            this.Size = new System.Drawing.Size(940, 72);
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.UI.WinForms.BunifuDropdown ddlServersAC;
        private Bunifu.Framework.UI.BunifuCustomLabel ddlLabel;

    }
}
