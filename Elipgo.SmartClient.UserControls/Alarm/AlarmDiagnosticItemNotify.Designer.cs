namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class AlarmDiagnosticItemNotify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmDiagnosticItemNotify));
            this.LabelName = new System.Windows.Forms.Label();
            this.CheckBox = new Bunifu.UI.WinForms.BunifuCheckBox();
            this.SuspendLayout();
            // 
            // LabelName
            // 
            this.LabelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LabelName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelName.Location = new System.Drawing.Point(-4, 0);
            this.LabelName.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(142, 36);
            this.LabelName.TabIndex = 3;
            this.LabelName.Text = "label1";
            this.LabelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.CheckBox.Location = new System.Drawing.Point(170, 7);
            this.CheckBox.MinimumSize = new System.Drawing.Size(17, 17);
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
            this.CheckBox.Size = new System.Drawing.Size(21, 21);
            this.CheckBox.Style = Bunifu.UI.WinForms.BunifuCheckBox.CheckBoxStyles.Bunifu;
            this.CheckBox.TabIndex = 2;
            this.CheckBox.ThreeState = false;
            this.CheckBox.ToolTipText = null;
            // 
            // AlarmDiagnosticItemNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.LabelName);
            this.Controls.Add(this.CheckBox);
            this.Name = "AlarmDiagnosticItemNotify";
            this.Size = new System.Drawing.Size(213, 39);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LabelName;
        private Bunifu.UI.WinForms.BunifuCheckBox CheckBox;
    }
}
