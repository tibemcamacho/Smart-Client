using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;

namespace Elipgo.SmartClient.UserControls.MainBar
{
    partial class VisualSearchBarControl
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
            this.LabelDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonCalendar
            // 
            this.ButtonCalendar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCalendar.Location = new System.Drawing.Point(1164, 19);
            this.ButtonCalendar.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonCalendar.Name = "ButtonCalendar";
            this.ButtonCalendar.Size = new System.Drawing.Size(36, 36);
            this.ButtonCalendar.TabIndex = 14;
            this.ButtonCalendar.TabStop = false;
            // 
            // LabelDate
            // 
            this.LabelDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelDate.ForeColor = System.Drawing.Color.White;
            this.LabelDate.Location = new System.Drawing.Point(1060, 32);
            this.LabelDate.Margin = new System.Windows.Forms.Padding(0);
            this.LabelDate.Name = "LabelDate";
            this.LabelDate.Size = new System.Drawing.Size(99, 23);
            this.LabelDate.TabIndex = 17;
            this.LabelDate.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
            this.LabelDate.Text = "label1";
            this.LabelDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VisualSearchBarControl
            // 
            this.Controls.Add(this.LabelDate);
            this.Controls.Add(this.ButtonCalendar);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "VisualSearchBarControl";
            this.Size = new System.Drawing.Size(1201, 72);

            this.ResumeLayout(false);



        }
        private Shared.ButtonCalendarControl ButtonCalendar;
        private System.Windows.Forms.Label LabelDate;
        #endregion
    }
}
