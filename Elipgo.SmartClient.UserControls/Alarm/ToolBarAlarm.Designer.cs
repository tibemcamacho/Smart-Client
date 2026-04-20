namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class ToolBarAlarm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolBarAlarm));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.FindButton = new System.Windows.Forms.Panel();
            this.FilterTextbox = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.SuspendLayout();
            // 
            // FindButton
            // 
            this.FindButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FindButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FindButton.Location = new System.Drawing.Point(252, 22);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(24, 24);
            this.FindButton.TabIndex = 10;
            this.FindButton.Visible = false;
            // 
            // FilterTextbox
            // 
            this.FilterTextbox.AcceptsReturn = false;
            this.FilterTextbox.AcceptsTab = false;
            this.FilterTextbox.AnimationSpeed = 200;
            this.FilterTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.FilterTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.FilterTextbox.BackColor = System.Drawing.Color.Transparent;
            this.FilterTextbox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("FilterTextbox.BackgroundImage")));
            this.FilterTextbox.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.FilterTextbox.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.BorderRadius = 20;
            this.FilterTextbox.BorderThickness = 1;
            this.FilterTextbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.FilterTextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FilterTextbox.DefaultFont = new System.Drawing.Font("Segoe UI", 12F);
            this.FilterTextbox.DefaultText = "";
            this.FilterTextbox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.ForeColor = System.Drawing.Color.White;
            this.FilterTextbox.HideSelection = true;
            this.FilterTextbox.IconLeft = null;
            this.FilterTextbox.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.FilterTextbox.IconPadding = 10;
            this.FilterTextbox.IconRight = null;
            this.FilterTextbox.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.FilterTextbox.Lines = new string[0];
            this.FilterTextbox.Location = new System.Drawing.Point(24, 20);
            this.FilterTextbox.MaxLength = 32767;
            this.FilterTextbox.MinimumSize = new System.Drawing.Size(1, 1);
            this.FilterTextbox.Modified = false;
            this.FilterTextbox.Multiline = false;
            this.FilterTextbox.Name = "FilterTextbox";
            stateProperties1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.FilterTextbox.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.FilterTextbox.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.FilterTextbox.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.FilterTextbox.OnIdleState = stateProperties4;
            this.FilterTextbox.PasswordChar = '\0';
            this.FilterTextbox.PlaceholderForeColor = System.Drawing.Color.White;
            this.FilterTextbox.PlaceholderText = "";
            this.FilterTextbox.ReadOnly = false;
            this.FilterTextbox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FilterTextbox.SelectedText = "";
            this.FilterTextbox.SelectionLength = 0;
            this.FilterTextbox.SelectionStart = 0;
            this.FilterTextbox.ShortcutsEnabled = true;
            this.FilterTextbox.Size = new System.Drawing.Size(259, 33);
            this.FilterTextbox.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Bunifu;
            this.FilterTextbox.TabIndex = 9;
            this.FilterTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FilterTextbox.TextMarginBottom = 0;
            this.FilterTextbox.TextMarginLeft = 5;
            this.FilterTextbox.TextMarginTop = 0;
            this.FilterTextbox.TextPlaceholder = "";
            this.FilterTextbox.UseSystemPasswordChar = false;
            this.FilterTextbox.Visible = false;
            this.FilterTextbox.WordWrap = true;
            // 
            // ToolBarAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.FilterTextbox);
            this.Name = "ToolBarAlarm";
            this.Size = new System.Drawing.Size(1537, 72);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FindButton;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox FilterTextbox;
    }
}
