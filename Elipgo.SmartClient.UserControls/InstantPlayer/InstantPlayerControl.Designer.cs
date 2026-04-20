using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.InstantPlayer
{
    partial class InstantPlayerControl
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
            this._labelGroupName = new System.Windows.Forms.Label();
            this._labelElementName = new System.Windows.Forms.Label();
            this._pictureBoxIconElement = new System.Windows.Forms.PictureBox();
            this._dropDownSelectRecorder = new Bunifu.UI.WinForms.BunifuDropdown();
            this._bunifuSeparator = new Bunifu.Framework.UI.BunifuSeparator();
            this._panelVideo = new System.Windows.Forms.Panel();
            this._addBookmarkControl = new Elipgo.SmartClient.UserControls.Bookmark.AddBookmarkControl(DateTime.Now);
            this._labelDate = new System.Windows.Forms.Label();
            this._buttonCalendar = new Elipgo.SmartClient.UserControls.Shared.ButtonCalendarControl();
            this._timeLineControl = new Elipgo.SmartClient.UserControls.PlaybackControls.TimeLineControl();
            this._zoomTimeLinePlaybackControl = new PlaybackControls.ZoomTimeLinePlaybackControl();
            this._labelTime = new System.Windows.Forms.Label();
            this._labelScaletime = new System.Windows.Forms.Label();
            this._labelScale = new System.Windows.Forms.Label();
            this._panelTimeLine = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxIconElement)).BeginInit();
            this._panelVideo.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupName
            // 
            this._labelGroupName.ForeColor = System.Drawing.Color.White;
            this._labelGroupName.Location = new System.Drawing.Point(73, 45);
            this._labelGroupName.Name = "GroupName";
            this._labelGroupName.Size = new System.Drawing.Size(300, 16);
            this._labelGroupName.TabIndex = 6;
            this._labelGroupName.Text = "Device Name";
            // 
            // ElementName
            // 
            this._labelElementName.ForeColor = System.Drawing.Color.White;
            this._labelElementName.Location = new System.Drawing.Point(73, 29);
            this._labelElementName.Name = "ElementName";
            this._labelElementName.Size = new System.Drawing.Size(300, 16);
            this._labelElementName.TabIndex = 5;
            this._labelElementName.Text = "Device Name";
            // 
            // IconElement
            // 
            this._pictureBoxIconElement.Location = new System.Drawing.Point(24, 29);
            this._pictureBoxIconElement.Margin = new System.Windows.Forms.Padding(0);
            this._pictureBoxIconElement.Name = "IconElement";
            this._pictureBoxIconElement.Size = new System.Drawing.Size(32, 32);
            this._pictureBoxIconElement.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBoxIconElement.TabIndex = 4;
            this._pictureBoxIconElement.TabStop = false;
            // 
            // buttonCalendar
            // 
            this._buttonCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCalendar.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonCalendar.Location = new System.Drawing.Point(700, 26);
            this._buttonCalendar.Margin = new System.Windows.Forms.Padding(0);
            this._buttonCalendar.Name = "buttonCalendar";
            this._buttonCalendar.Size = new System.Drawing.Size(36, 36);
            this._buttonCalendar.TabIndex = 18;
            // 
            // LabelDate
            // 
            this._labelDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._labelDate.ForeColor = System.Drawing.Color.White;
            this._labelDate.Location = new System.Drawing.Point(595, 36);
            this._labelDate.Margin = new System.Windows.Forms.Padding(0);
            this._labelDate.Name = "LabelDate";
            this._labelDate.Size = new System.Drawing.Size(99, 23);
            this._labelDate.TabIndex = 17;
            this._labelDate.Text = "label1";
            this._labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SelectRecorder
            // 
            this._dropDownSelectRecorder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._dropDownSelectRecorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this._dropDownSelectRecorder.BorderRadius = 1;
            this._dropDownSelectRecorder.Color = System.Drawing.Color.Transparent;
            this._dropDownSelectRecorder.Cursor = System.Windows.Forms.Cursors.Hand;
            this._dropDownSelectRecorder.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this._dropDownSelectRecorder.DisabledColor = System.Drawing.Color.Gray;
            this._dropDownSelectRecorder.DisplayMember = "Name";
            this._dropDownSelectRecorder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._dropDownSelectRecorder.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this._dropDownSelectRecorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._dropDownSelectRecorder.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this._dropDownSelectRecorder.FillDropDown = false;
            this._dropDownSelectRecorder.FillIndicator = false;
            this._dropDownSelectRecorder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._dropDownSelectRecorder.ForeColor = System.Drawing.Color.White;
            this._dropDownSelectRecorder.FormattingEnabled = true;
            this._dropDownSelectRecorder.Icon = null;
            this._dropDownSelectRecorder.IndicatorColor = System.Drawing.Color.White;
            this._dropDownSelectRecorder.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this._dropDownSelectRecorder.ItemBackColor = System.Drawing.Color.DimGray;
            this._dropDownSelectRecorder.ItemBorderColor = System.Drawing.Color.DimGray;
            this._dropDownSelectRecorder.ItemForeColor = System.Drawing.Color.White;
            this._dropDownSelectRecorder.ItemHeight = 19;
            this._dropDownSelectRecorder.ItemHighLightColor = System.Drawing.Color.Gray;
            this._dropDownSelectRecorder.Location = new System.Drawing.Point(376, 34);
            this._dropDownSelectRecorder.Margin = new System.Windows.Forms.Padding(0);
            this._dropDownSelectRecorder.Name = "SelectRecorder";
            this._dropDownSelectRecorder.Size = new System.Drawing.Size(207, 25);
            this._dropDownSelectRecorder.TabIndex = 15;
            this._dropDownSelectRecorder.Text = null;
            this._dropDownSelectRecorder.ValueMember = "Key";
            // 
            // bunifuSeparator1
            // 
            this._bunifuSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._bunifuSeparator.BackColor = System.Drawing.Color.Transparent;
            this._bunifuSeparator.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this._bunifuSeparator.LineThickness = 1;
            this._bunifuSeparator.Location = new System.Drawing.Point(582, 30);
            this._bunifuSeparator.Margin = new System.Windows.Forms.Padding(0);
            this._bunifuSeparator.Name = "bunifuSeparator1";
            this._bunifuSeparator.Size = new System.Drawing.Size(13, 30);
            this._bunifuSeparator.TabIndex = 16;
            this._bunifuSeparator.Transparency = 255;
            this._bunifuSeparator.Vertical = true;
            // 
            // labelScaletime
            // 
            this._labelScaletime.ForeColor = System.Drawing.Color.White;
            this._labelScaletime.Location = new System.Drawing.Point(1080, 15);
            this._labelScaletime.Margin = new System.Windows.Forms.Padding(0);
            this._labelScaletime.Name = "labelScaletime";
            this._labelScaletime.Size = new System.Drawing.Size(86, 21);
            this._labelScaletime.TabIndex = 22;
            this._labelScaletime.Text = "Escala timeline";
            this._labelScaletime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelScale
            // 
            this._labelScale.ForeColor = System.Drawing.Color.White;
            this._labelScale.Margin = new System.Windows.Forms.Padding(0);
            this._labelScale.Name = "labelScale";
            this._labelScale.Size = new System.Drawing.Size(10, 24);
            this._labelScale.TabIndex = 22;
            this._labelScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._labelScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._labelScale.Location = new System.Drawing.Point(600, 980);
            this._labelScale.Size = new System.Drawing.Size(194, 29);
            this._addBookmarkControl.TabIndex = 108;
            // 
            // TimeLineControl
            // 
            this._timeLineControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._timeLineControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._timeLineControl.Location = new System.Drawing.Point(0, 0);
            this._timeLineControl.Name = "TimeLineControl";
            this._timeLineControl.Size = new System.Drawing.Size(150, 45);
            this._timeLineControl.TabIndex = 1;
            this._timeLineControl.TabStop = false;
            // 
            // panelTimeLine
            // 
            this._panelTimeLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelTimeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._panelTimeLine.Cursor = System.Windows.Forms.Cursors.Hand;
            this._panelTimeLine.Location = new System.Drawing.Point(0, 0);
            this._panelTimeLine.Name = "_panelTimeLine";
            this._panelTimeLine.Size = new System.Drawing.Size(150, 45);
            this._panelTimeLine.TabIndex = 100;
            // 
            // zoomTimeLinePlaybackControl
            // 
            this._zoomTimeLinePlaybackControl.BackColor = System.Drawing.Color.Transparent;
            this._zoomTimeLinePlaybackControl.Location = new System.Drawing.Point(600, 980);
            this._zoomTimeLinePlaybackControl.Name = "zoomTimeLinePlaybackControl";
            this._zoomTimeLinePlaybackControl.Size = new System.Drawing.Size(194, 29);
            this._addBookmarkControl.TabIndex = 101;
            // 
            // PanelVideo
            // .
            this._panelVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panelVideo.Controls.Add(this._addBookmarkControl);
            this._panelVideo.Location = new System.Drawing.Point(24, 89);
            this._panelVideo.Name = "PanelVideo";
            this._panelVideo.Size = new System.Drawing.Size(713, 431);
            this._panelVideo.TabIndex = 0;
            // 
            // addBookmarkControl
            // 
            this._addBookmarkControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._addBookmarkControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._addBookmarkControl.Location = new System.Drawing.Point(0, 217);
            this._addBookmarkControl.Margin = new System.Windows.Forms.Padding(0);
            this._addBookmarkControl.Name = "addBookmarkControl";
            this._addBookmarkControl.Size = new System.Drawing.Size(711, 168);
            this._addBookmarkControl.TabIndex = 0;
            // 
            // LabelDate
            // 
            this._labelDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._labelDate.ForeColor = System.Drawing.Color.White;
            this._labelDate.Location = new System.Drawing.Point(595, 36);
            this._labelDate.Margin = new System.Windows.Forms.Padding(0);
            this._labelDate.Name = "LabelDate";
            this._labelDate.Size = new System.Drawing.Size(99, 23);
            this._labelDate.TabIndex = 17;
            this._labelDate.Text = "label1";
            this._labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelTime
            // 
            this._labelTime.ForeColor = System.Drawing.Color.White;
            this._labelTime.Location = new System.Drawing.Point(839, 15);
            this._labelTime.Margin = new System.Windows.Forms.Padding(0);
            this._labelTime.Name = "LabelTime";
            this._labelTime.Size = new System.Drawing.Size(64, 21);
            this._labelTime.TabIndex = 15;
            this._labelTime.Text = "00:00:00";
            this._labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InstantPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this._bunifuSeparator);
            this.Controls.Add(this._dropDownSelectRecorder);
            this.Controls.Add(this._labelGroupName);
            this.Controls.Add(this._labelElementName);
            this.Controls.Add(this._pictureBoxIconElement);
            this.Controls.Add(this._panelVideo);
            this.Controls.Add(this._buttonCalendar);
            this.Controls.Add(this._labelDate);
            this.Controls.Add(_zoomTimeLinePlaybackControl);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "InstantPlayerControl";
            this.Size = new System.Drawing.Size(761, 544);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxIconElement)).EndInit();
            this._panelTimeLine.ResumeLayout(false);
            this._panelVideo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label _labelGroupName;
        private System.Windows.Forms.Label _labelElementName;
        private System.Windows.Forms.PictureBox _pictureBoxIconElement;
        private Bunifu.UI.WinForms.BunifuDropdown _dropDownSelectRecorder;
        private Bunifu.Framework.UI.BunifuSeparator _bunifuSeparator;
        private System.Windows.Forms.Panel _panelVideo;
        private Shared.ButtonCalendarControl _buttonCalendar;
        private System.Windows.Forms.Label _labelDate;
        private Bookmark.AddBookmarkControl _addBookmarkControl;
        private System.Windows.Forms.Label _labelScale;
        private System.Windows.Forms.Label _labelTime;
        private System.Windows.Forms.Label _labelScaletime;
        private PlaybackControls.ZoomTimeLinePlaybackControl _zoomTimeLinePlaybackControl;
        private UserControls.PlaybackControls.TimeLineControl _timeLineControl;
        private System.Windows.Forms.Panel _panelTimeLine;
    }
}
