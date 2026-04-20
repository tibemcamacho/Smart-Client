using Elipgo.SmartClient.Common;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    partial class GenericFormItemCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericFormItemCard));
            Bunifu.ToggleSwitch.ToggleState toggleState1 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState2 = new Bunifu.ToggleSwitch.ToggleState();
            Bunifu.ToggleSwitch.ToggleState toggleState3 = new Bunifu.ToggleSwitch.ToggleState();
            this.EntityIconPanel = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.Label1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.label2 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.PrivateIconPanel = new Elipgo.SmartClient.Common.DBFlowLayoutPanel();
            this.bunifuToggleSwitch = new Bunifu.ToggleSwitch.BunifuToggleSwitch();
            this.buttonContexMenu = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.buttonContexMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // EntityIconPanel
            // 
            this.EntityIconPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.EntityIconPanel.CausesValidation = false;
            this.EntityIconPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EntityIconPanel.Location = new System.Drawing.Point(58, 39);
            this.EntityIconPanel.Name = "EntityIconPanel";
            this.EntityIconPanel.Size = new System.Drawing.Size(65, 65);
            this.EntityIconPanel.TabIndex = 0;
            this.EntityIconPanel.Click += new System.EventHandler(this.GenericFormItemCard_Click);
            this.EntityIconPanel.DoubleClick += new System.EventHandler(this.GenericFormItemCard_DoubleClick);
            // 
            // Label1
            // 
            this.Label1.CausesValidation = false;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(3, 140);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(172, 40);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "_label1_";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label1.Click += new System.EventHandler(this.GenericFormItemCard_Click);
            this.Label1.DoubleClick += new System.EventHandler(this.GenericFormItemCard_DoubleClick);
            // 
            // label2
            // 
            this.label2.CausesValidation = false;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 176);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(172, 35);
            this.label2.TabIndex = 4;
            this.label2.Text = "_label2_";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.GenericFormItemCard_Click);
            this.label2.DoubleClick += new System.EventHandler(this.GenericFormItemCard_DoubleClick);
            // 
            // PrivateIconPanel
            // 
            this.PrivateIconPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PrivateIconPanel.BackgroundImage")));
            this.PrivateIconPanel.CausesValidation = false;
            this.PrivateIconPanel.ForeColor = System.Drawing.Color.Transparent;
            this.PrivateIconPanel.Location = new System.Drawing.Point(110, 25);
            this.PrivateIconPanel.Name = "PrivateIconPanel";
            this.PrivateIconPanel.Size = new System.Drawing.Size(24, 23);
            this.PrivateIconPanel.TabIndex = 5;
            // 
            // bunifuToggleSwitch
            // 
            this.bunifuToggleSwitch.Animation = 5;
            this.bunifuToggleSwitch.BackColor = System.Drawing.Color.Transparent;
            this.bunifuToggleSwitch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuToggleSwitch.BackgroundImage")));
            this.bunifuToggleSwitch.CausesValidation = false;
            this.bunifuToggleSwitch.Checked = false;
            this.bunifuToggleSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuToggleSwitch.InnerCirclePadding = 3;
            this.bunifuToggleSwitch.Location = new System.Drawing.Point(67, 120);
            this.bunifuToggleSwitch.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuToggleSwitch.Name = "bunifuToggleSwitch";
            this.bunifuToggleSwitch.Size = new System.Drawing.Size(42, 15);
            this.bunifuToggleSwitch.TabIndex = 2;
            toggleState1.BackColor = System.Drawing.Color.Empty;
            toggleState1.BackColorInner = System.Drawing.Color.Empty;
            toggleState1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            toggleState1.BorderColorInner = System.Drawing.Color.Empty;
            toggleState1.BorderRadius = 1;
            toggleState1.BorderRadiusInner = 1;
            toggleState1.BorderThickness = 1;
            toggleState1.BorderThicknessInner = 1;
            this.bunifuToggleSwitch.ToggleStateDisabled = toggleState1;
            toggleState2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            toggleState2.BackColorInner = System.Drawing.Color.White;
            toggleState2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            toggleState2.BorderColorInner = System.Drawing.Color.White;
            toggleState2.BorderRadius = 17;
            toggleState2.BorderRadiusInner = 11;
            toggleState2.BorderThickness = 1;
            toggleState2.BorderThicknessInner = 1;
            this.bunifuToggleSwitch.ToggleStateOff = toggleState2;
            toggleState3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            toggleState3.BackColorInner = System.Drawing.Color.White;
            toggleState3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            toggleState3.BorderColorInner = System.Drawing.Color.White;
            toggleState3.BorderRadius = 17;
            toggleState3.BorderRadiusInner = 11;
            toggleState3.BorderThickness = 1;
            toggleState3.BorderThicknessInner = 1;
            this.bunifuToggleSwitch.ToggleStateOn = toggleState3;
            this.bunifuToggleSwitch.Value = false;
            this.bunifuToggleSwitch.OnValuechange += new System.EventHandler(this.bunifuToggleSwitch_OnValuechange);
            // 
            // buttonContexMenu
            // 
            this.buttonContexMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonContexMenu.Image = ((System.Drawing.Image)(resources.GetObject("buttonContexMenu.Image")));
            this.buttonContexMenu.ImageActive = null;
            this.buttonContexMenu.Location = new System.Drawing.Point(143, 9);
            this.buttonContexMenu.Name = "buttonContexMenu";
            this.buttonContexMenu.Size = new System.Drawing.Size(24, 24);
            this.buttonContexMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonContexMenu.TabIndex = 6;
            this.buttonContexMenu.TabStop = false;
            this.buttonContexMenu.Zoom = 10;
            this.buttonContexMenu.Click += new System.EventHandler(this.ButtonContexMenu_Click);
            // 
            // GenericFormItemCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.CausesValidation = false;
            this.Controls.Add(this.buttonContexMenu);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.PrivateIconPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bunifuToggleSwitch);
            this.Controls.Add(this.EntityIconPanel);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(170, 212);
            this.Name = "GenericFormItemCard";
            this.Size = new System.Drawing.Size(180, 232);
            this.Click += new System.EventHandler(this.GenericFormItemCard_Click);
            this.DoubleClick += new System.EventHandler(this.GenericFormItemCard_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.buttonContexMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DBFlowLayoutPanel EntityIconPanel;
        private Bunifu.ToggleSwitch.BunifuToggleSwitch bunifuToggleSwitch;
        private Bunifu.Framework.UI.BunifuCustomLabel Label1;
        private Bunifu.Framework.UI.BunifuCustomLabel label2;
        private DBFlowLayoutPanel  PrivateIconPanel;
        private Bunifu.Framework.UI.BunifuImageButton buttonContexMenu;
    }
}
