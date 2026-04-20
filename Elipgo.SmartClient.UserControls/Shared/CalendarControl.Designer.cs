using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.Shared
{
    partial class CalendarControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarControl));
            this._monthCalendar = new System.Windows.Forms.MonthCalendar();
            this._timePicker = new System.Windows.Forms.DateTimePicker();
            this._panelClear = new System.Windows.Forms.Panel();
            this._buttonClose = new System.Windows.Forms.Button();
            this._buttonOK = new System.Windows.Forms.Button();
            this._listBoxRecords = new System.Windows.Forms.ListBox();
            this._labelInicio = new System.Windows.Forms.Label();
            this._labelListado = new System.Windows.Forms.Label();
            this._progressBar = new Bunifu.UI.Winforms.BunifuProgressBar();
            this._panelClear.SuspendLayout();
            this.SuspendLayout();
            // 
            // monthCalendar
            // 
            this._monthCalendar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this._monthCalendar.Location = new System.Drawing.Point(9, 71);
            this._monthCalendar.Name = "monthCalendar";
            this._monthCalendar.TabIndex = 3;
            // 
            // TimePicker
            // 
            this._timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._timePicker.Location = new System.Drawing.Point(136, 243);
            this._timePicker.Name = "TimePicker";
            this._timePicker.ShowUpDown = true;
            this._timePicker.Size = new System.Drawing.Size(100, 20);
            this._timePicker.TabIndex = 4;
            // 
            // panelClear
            // 
            this._panelClear.BackColor = System.Drawing.SystemColors.Window;
            this._panelClear.Controls.Add(this._buttonClose);
            this._panelClear.Controls.Add(this._buttonOK);
            this._panelClear.Location = new System.Drawing.Point(41, 212);
            this._panelClear.Name = "panelClear";
            this._panelClear.Size = new System.Drawing.Size(183, 21);
            this._panelClear.TabIndex = 0;
            // 
            // ButtonClose
            // 
            this._buttonClose.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this._buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonClose.Location = new System.Drawing.Point(24, -1);
            this._buttonClose.Name = "ButtonClose";
            this._buttonClose.Size = new System.Drawing.Size(49, 19);
            this._buttonClose.TabIndex = 3;
            this._buttonClose.Text = Resources.ButtonClose;
            this._buttonClose.UseVisualStyleBackColor = true;
            this._buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonOK
            // 
            this._buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this._buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._buttonOK.Location = new System.Drawing.Point(80, -1);
            this._buttonOK.Name = "ButtonOK";
            this._buttonOK.Size = new System.Drawing.Size(49, 19);
            this._buttonOK.TabIndex = 2;
            this._buttonOK.Text = "OK";
            this._buttonOK.UseVisualStyleBackColor = true;
            // 
            // records
            // 
            this._listBoxRecords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._listBoxRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._listBoxRecords.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this._listBoxRecords.FormattingEnabled = true;
            this._listBoxRecords.ItemHeight = 9;
            this._listBoxRecords.Items.AddRange(new object[] {
            "Select a day and a camera"});
            this._listBoxRecords.Location = new System.Drawing.Point(9, 29);
            this._listBoxRecords.Name = "records";
            this._listBoxRecords.Size = new System.Drawing.Size(227, 22);
            this._listBoxRecords.TabIndex = 5;
            this._listBoxRecords.DoubleClick += new System.EventHandler(this.ListBoxRecords_SelectedIndexChanged);
            // 
            // labelInicio
            // 
            this._labelInicio.AutoSize = true;
            this._labelInicio.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this._labelInicio.Location = new System.Drawing.Point(10, 246);
            this._labelInicio.Name = "labelInicio";
            this._labelInicio.Size = new System.Drawing.Size(101, 13);
            this._labelInicio.TabIndex = 6;
            this._labelInicio.Text = "Recording start time";
            // 
            // labelListado
            // 
            this._labelListado.AutoSize = true;
            this._labelListado.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this._labelListado.Location = new System.Drawing.Point(14, 7);
            this._labelListado.Name = "labelListado";
            this._labelListado.Size = new System.Drawing.Size(71, 13);
            this._labelListado.TabIndex = 7;
            this._labelListado.Text = "Recording list";
            // 
            // progress
            // 
            this._progressBar.Animation = 0;
            this._progressBar.AnimationStep = 10;
            this._progressBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("progress.BackgroundImage")));
            this._progressBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this._progressBar.BorderRadius = 1;
            this._progressBar.BorderThickness = 1;
            this._progressBar.Location = new System.Drawing.Point(9, 35);
            this._progressBar.MaximumValue = 100;
            this._progressBar.MinimumValue = 0;
            this._progressBar.Name = "progress";
            this._progressBar.ProgressBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this._progressBar.ProgressColorLeft = System.Drawing.Color.DodgerBlue;
            this._progressBar.ProgressColorRight = System.Drawing.Color.DodgerBlue;
            this._progressBar.Size = new System.Drawing.Size(227, 19);
            this._progressBar.TabIndex = 8;
            this._progressBar.Value = 50;
            this._progressBar.Visible = false;
            // 
            // CalendarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panelClear);
            this.Controls.Add(this._timePicker);
            this.Controls.Add(this._monthCalendar);
            this.Controls.Add(this._listBoxRecords);
            this.Controls.Add(this._labelListado);
            this.Controls.Add(this._labelInicio);
            this.Controls.Add(this._progressBar);
            this.Name = "CalendarControl";
            this.Size = new System.Drawing.Size(249, 273);
            this._panelClear.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar _monthCalendar;
        private System.Windows.Forms.DateTimePicker _timePicker;
        private System.Windows.Forms.Panel _panelClear;
        private System.Windows.Forms.Button _buttonOK;
        private System.Windows.Forms.ListBox _listBoxRecords;
        private System.Windows.Forms.Label _labelInicio;
        private System.Windows.Forms.Label _labelListado;
        private Bunifu.UI.Winforms.BunifuProgressBar _progressBar;
        private System.Windows.Forms.Button _buttonClose;
    }
}
