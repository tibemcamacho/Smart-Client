namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class AlarmButtonControl
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
            this.components = new System.ComponentModel.Container();
            this._labelNumber = new Elipgo.SmartClient.UserControls.Alarm.LabelRoundCorners();
            this._buttonAlarm = new System.Windows.Forms.PictureBox();
            this._bunifuToolTip = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._buttonAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelNumber
            // 
            this._labelNumber._BackColor = System.Drawing.Color.DodgerBlue;
            this._labelNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelNumber.ForeColor = System.Drawing.Color.White;
            this._labelNumber.Location = new System.Drawing.Point(18, 0);
            this._labelNumber.Margin = new System.Windows.Forms.Padding(0);
            this._labelNumber.Name = "LabelNumber";
            this._labelNumber.Size = new System.Drawing.Size(16, 16);
            this._labelNumber.TabIndex = 2;
            this._labelNumber.Text = "+9";
            this._labelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._bunifuToolTip.SetToolTip(this._labelNumber, "");
            this._bunifuToolTip.SetToolTipIcon(this._labelNumber, null);
            this._bunifuToolTip.SetToolTipTitle(this._labelNumber, "");
            // 
            // ButtonAlarm
            // 
            this._buttonAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this._buttonAlarm.Location = new System.Drawing.Point(0, 0);
            this._buttonAlarm.Margin = new System.Windows.Forms.Padding(0);
            this._buttonAlarm.Name = "ButtonAlarm";
            this._buttonAlarm.Size = new System.Drawing.Size(34, 34);
            this._buttonAlarm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._buttonAlarm.TabIndex = 4;
            this._buttonAlarm.TabStop = false;
            this._bunifuToolTip.SetToolTip(this._buttonAlarm, "");
            this._bunifuToolTip.SetToolTipIcon(this._buttonAlarm, null);
            this._bunifuToolTip.SetToolTipTitle(this._buttonAlarm, "");
            // 
            // bunifuToolTip1
            // 
            this._bunifuToolTip.Active = true;
            this._bunifuToolTip.AlignTextWithTitle = false;
            this._bunifuToolTip.AllowAutoClose = true;
            this._bunifuToolTip.AllowFading = true;
            this._bunifuToolTip.AutoCloseDuration = 2000;
            this._bunifuToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this._bunifuToolTip.BorderColor = System.Drawing.Color.Black;
            this._bunifuToolTip.ClickToShowDisplayControl = false;
            this._bunifuToolTip.ConvertNewlinesToBreakTags = true;
            this._bunifuToolTip.DisplayControl = null;
            this._bunifuToolTip.EntryAnimationSpeed = 150;
            this._bunifuToolTip.ExitAnimationSpeed = 200;
            this._bunifuToolTip.GenerateAutoCloseDuration = false;
            this._bunifuToolTip.IconMargin = 6;
            this._bunifuToolTip.InitialDelay = 0;
            this._bunifuToolTip.Name = "bunifuToolTip1";
            this._bunifuToolTip.Opacity = 1D;
            this._bunifuToolTip.OverrideToolTipTitles = false;
            this._bunifuToolTip.Padding = new System.Windows.Forms.Padding(10);
            this._bunifuToolTip.ReshowDelay = 100;
            this._bunifuToolTip.ShowAlways = true;
            this._bunifuToolTip.ShowBorders = false;
            this._bunifuToolTip.ShowIcons = true;
            this._bunifuToolTip.ShowShadows = true;
            this._bunifuToolTip.Tag = null;
            this._bunifuToolTip.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this._bunifuToolTip.TextForeColor = System.Drawing.Color.White;
            this._bunifuToolTip.TextMargin = 2;
            this._bunifuToolTip.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._bunifuToolTip.TitleForeColor = System.Drawing.Color.Black;
            this._bunifuToolTip.ToolTipPosition = new System.Drawing.Point(0, 0);
            this._bunifuToolTip.ToolTipTitle = null;
            // 
            // AlarmButtonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._labelNumber);
            this.Controls.Add(this._buttonAlarm);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AlarmButtonControl";
            this.Size = new System.Drawing.Size(34, 34);
            this._bunifuToolTip.SetToolTip(this, "");
            this._bunifuToolTip.SetToolTipIcon(this, null);
            this._bunifuToolTip.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this._buttonAlarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private LabelRoundCorners _labelNumber;
        private System.Windows.Forms.PictureBox _buttonAlarm;
        private Bunifu.UI.WinForms.BunifuToolTip _bunifuToolTip;
    }
}
