using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class AlarmDiagnosticControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmDiagnosticControl));
            this._viewDropdown = new Bunifu.UI.WinForms.BunifuDropdown();
            this._buttonIstanPlayback = new Bunifu.Framework.UI.BunifuImageButton();
            this._panelContextBar = new System.Windows.Forms.Panel();
            this._dvfsRelated = new Bunifu.UI.WinForms.BunifuDropdown();
            ((System.ComponentModel.ISupportInitialize)(this._buttonIstanPlayback)).BeginInit();
            this.SuspendLayout();
            // 
            // ViewDropdown
            // 
            this._viewDropdown.BackColor = System.Drawing.Color.Transparent;
            this._viewDropdown.BorderRadius = 1;
            this._viewDropdown.Color = System.Drawing.Color.Transparent;
            this._viewDropdown.Cursor = System.Windows.Forms.Cursors.Hand;
            this._viewDropdown.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this._viewDropdown.DisabledColor = System.Drawing.Color.Gray;
            this._viewDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._viewDropdown.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this._viewDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._viewDropdown.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this._viewDropdown.FillDropDown = false;
            this._viewDropdown.FillIndicator = false;
            this._viewDropdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._viewDropdown.ForeColor = System.Drawing.Color.White;
            this._viewDropdown.FormattingEnabled = true;
            this._viewDropdown.Icon = null;
            this._viewDropdown.IndicatorColor = System.Drawing.Color.White;
            this._viewDropdown.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this._viewDropdown.ItemBackColor = System.Drawing.Color.DimGray;
            this._viewDropdown.ItemBorderColor = System.Drawing.Color.DimGray;
            this._viewDropdown.ItemForeColor = System.Drawing.Color.White;
            this._viewDropdown.ItemHeight = 19;
            this._viewDropdown.ItemHighLightColor = System.Drawing.Color.Gray;
            this._viewDropdown.Location = new System.Drawing.Point(1178, 54);
            this._viewDropdown.Name = "ViewDropdown";
            this._viewDropdown.Size = new System.Drawing.Size(217, 25);
            this._viewDropdown.TabIndex = 1;
            this._viewDropdown.Text = null;
            // 
            // ButtonIstanPlayback
            // 
            this._buttonIstanPlayback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonIstanPlayback.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonIstanPlayback.Image = ((System.Drawing.Image)(resources.GetObject("ButtonIstanPlayback.Image")));
            this._buttonIstanPlayback.ImageActive = null;
            this._buttonIstanPlayback.Location = new System.Drawing.Point(610, 54);
            this._buttonIstanPlayback.Margin = new System.Windows.Forms.Padding(0);
            this._buttonIstanPlayback.Name = "ButtonIstanPlayback";
            this._buttonIstanPlayback.Size = new System.Drawing.Size(1050, 24);
            this._buttonIstanPlayback.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonIstanPlayback.TabIndex = 10;
            this._buttonIstanPlayback.TabStop = false;
            this._buttonIstanPlayback.Visible = false;
            this._buttonIstanPlayback.Zoom = 10;
            this._buttonIstanPlayback.Click += new System.EventHandler(this.ButtonIstanPlayback_Click);
            // 
            // PanelContextBar
            // 
            this._panelContextBar.BackColor = System.Drawing.Color.Transparent;
            this._panelContextBar.Location = new System.Drawing.Point(5, 108);
            this._panelContextBar.Name = "PanelContextBar";
            this._panelContextBar.Size = new System.Drawing.Size(1393, 50);
            this._panelContextBar.TabIndex = 0;
            // 
            // DvfsRelated
            // 
            this._dvfsRelated.BackColor = System.Drawing.Color.Transparent;
            this._dvfsRelated.BorderRadius = 1;
            this._dvfsRelated.Color = System.Drawing.Color.Transparent;
            this._dvfsRelated.Cursor = System.Windows.Forms.Cursors.Hand;
            this._dvfsRelated.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this._dvfsRelated.DisabledColor = System.Drawing.Color.Gray;
            this._dvfsRelated.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._dvfsRelated.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this._dvfsRelated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._dvfsRelated.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this._dvfsRelated.FillDropDown = false;
            this._dvfsRelated.FillIndicator = false;
            this._dvfsRelated.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._dvfsRelated.ForeColor = System.Drawing.Color.White;
            this._dvfsRelated.FormattingEnabled = true;
            this._dvfsRelated.Icon = null;
            this._dvfsRelated.IndicatorColor = System.Drawing.Color.White;
            this._dvfsRelated.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this._dvfsRelated.ItemBackColor = System.Drawing.Color.DimGray;
            this._dvfsRelated.ItemBorderColor = System.Drawing.Color.DimGray;
            this._dvfsRelated.ItemForeColor = System.Drawing.Color.White;
            this._dvfsRelated.ItemHeight = 19;
            this._dvfsRelated.ItemHighLightColor = System.Drawing.Color.Gray;
            this._dvfsRelated.Location = new System.Drawing.Point(1178, 95);
            this._dvfsRelated.Name = "DvfsRelated";
            this._dvfsRelated.Size = new System.Drawing.Size(217, 25);
            this._dvfsRelated.TabIndex = 58;
            this._dvfsRelated.Text = null;
            this._dvfsRelated.Visible = false;
            // 
            // AlarmDiagnosticControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._dvfsRelated);
            this.Controls.Add(this._panelContextBar);
            this.Controls.Add(this._viewDropdown);
            this.Controls.Add(this._buttonIstanPlayback);
            this.Name = "AlarmDiagnosticControl";
            this.Load += new System.EventHandler(this.AlarmDiagnosticControl_Load);
            this.Controls.SetChildIndex(this._buttonIstanPlayback, 0);
            this.Controls.SetChildIndex(this._viewDropdown, 0);
            this.Controls.SetChildIndex(this._panelContextBar, 0);
            this.Controls.SetChildIndex(this._dvfsRelated, 0);
            ((System.ComponentModel.ISupportInitialize)(this._buttonIstanPlayback)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuDropdown _viewDropdown;
        private System.Windows.Forms.Panel _panelContextBar;
        private Bunifu.Framework.UI.BunifuImageButton _buttonIstanPlayback;
        private Bunifu.UI.WinForms.BunifuDropdown _dvfsRelated;
    }
}
