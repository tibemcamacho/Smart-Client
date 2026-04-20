namespace Elipgo.SmartClient.UserControls.UserProfile
{
    partial class ItemControl
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
            // Proteger la disposición en caso de que el control haya sido creado en otro hilo
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke((System.Windows.Forms.MethodInvoker)(() => Dispose(disposing)));
                    return;
                }
                catch
                {
                    // Si la invocación falla (por control destruido), continuar con dispose base protegido
                }
            }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemControl));
            Bunifu.ToggleSwitch.ToggleState toggleState1 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState2 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState3 = new Bunifu.ToggleSwitch.ToggleState();
            this._labelName = new System.Windows.Forms.Label();
            this._panelIcon = new System.Windows.Forms.Panel();
            this._bunifuToggleSwitch = new Bunifu.ToggleSwitch.BunifuToggleSwitch();
            this._panelIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelName
            // 
            this._labelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._labelName.BackColor = System.Drawing.Color.Transparent;
            this._labelName.Cursor = System.Windows.Forms.Cursors.Hand;
            this._labelName.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelName.ForeColor = System.Drawing.Color.Transparent;
            this._labelName.Location = new System.Drawing.Point(47, 0);
            this._labelName.Name = "LabelName";
            this._labelName.Size = new System.Drawing.Size(190, 32);
            this._labelName.TabIndex = 2;
            this._labelName.Text = "label1";
            this._labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._labelName.Click += new System.EventHandler(this.LabelName_Click);
            // 
            // icon
            // 
            this._panelIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._panelIcon.BackColor = System.Drawing.Color.Transparent;
            this._panelIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("icon.BackgroundImage")));
            this._panelIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._panelIcon.Controls.Add(this._bunifuToggleSwitch);
            this._panelIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this._panelIcon.Location = new System.Drawing.Point(3, 8);
            this._panelIcon.Name = "icon";
            this._panelIcon.Size = new System.Drawing.Size(24, 24);
            this._panelIcon.TabIndex = 13;
            this._panelIcon.Click += new System.EventHandler(this.LabelName_Click);
            // 
            // bunifuToggleSwitch
            // 
            this._bunifuToggleSwitch.Animation = 5;
            this._bunifuToggleSwitch.BackColor = System.Drawing.Color.Transparent;
            this._bunifuToggleSwitch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuToggleSwitch.BackgroundImage")));
            this._bunifuToggleSwitch.CausesValidation = false;
            this._bunifuToggleSwitch.Checked = false;
            this._bunifuToggleSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this._bunifuToggleSwitch.InnerCirclePadding = 4;
            this._bunifuToggleSwitch.Location = new System.Drawing.Point(0, 1);
            this._bunifuToggleSwitch.Margin = new System.Windows.Forms.Padding(4);
            this._bunifuToggleSwitch.Name = "bunifuToggleSwitch";
            this._bunifuToggleSwitch.Size = new System.Drawing.Size(40, 20);
            this._bunifuToggleSwitch.TabIndex = 14;
            toggleState1.BackColor = System.Drawing.Color.Empty;
            toggleState1.BackColorInner = System.Drawing.Color.Empty;
            toggleState1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            toggleState1.BorderColorInner = System.Drawing.Color.Empty;
            toggleState1.BorderRadius = 1;
            toggleState1.BorderRadiusInner = 1;
            toggleState1.BorderThickness = 1;
            toggleState1.BorderThicknessInner = 1;
            this._bunifuToggleSwitch.ToggleStateDisabled = toggleState1;
            toggleState2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            toggleState2.BackColorInner = System.Drawing.Color.White;
            toggleState2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            toggleState2.BorderColorInner = System.Drawing.Color.White;
            toggleState2.BorderRadius = 17;
            toggleState2.BorderRadiusInner = 11;
            toggleState2.BorderThickness = 1;
            toggleState2.BorderThicknessInner = 1;
            this._bunifuToggleSwitch.ToggleStateOff = toggleState2;
            toggleState3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            toggleState3.BackColorInner = System.Drawing.Color.White;
            toggleState3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            toggleState3.BorderColorInner = System.Drawing.Color.White;
            toggleState3.BorderRadius = 17;
            toggleState3.BorderRadiusInner = 11;
            toggleState3.BorderThickness = 1;
            toggleState3.BorderThicknessInner = 1;
            this._bunifuToggleSwitch.ToggleStateOn = toggleState3;
            this._bunifuToggleSwitch.Value = false;
            this._bunifuToggleSwitch.Visible = false;
            this._bunifuToggleSwitch.Click += new System.EventHandler(this.BunifuToggleSwitch_Click);
            // 
            // ItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.Controls.Add(this._panelIcon);
            this.Controls.Add(this._labelName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ItemControl";
            this.Size = new System.Drawing.Size(240, 36);
            this._panelIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _labelName;
        private System.Windows.Forms.Panel _panelIcon;
        private Bunifu.ToggleSwitch.BunifuToggleSwitch _bunifuToggleSwitch;
    }
}
