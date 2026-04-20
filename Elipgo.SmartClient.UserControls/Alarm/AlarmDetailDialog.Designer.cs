using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class AlarmDetailDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmDetailDialog));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges5 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges6 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.ButtonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.ButtonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.LabelTitle = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblAttendDate = new System.Windows.Forms.Label();
            this.lblAttendByUser = new System.Windows.Forms.Label();
            this.AlertMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonOK
            // 
            this.ButtonOK.AllowToggling = false;
            this.ButtonOK.AnimationSpeed = 200;
            this.ButtonOK.AutoGenerateColors = false;
            this.ButtonOK.BackColor = System.Drawing.Color.Transparent;
            this.ButtonOK.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this.ButtonOK.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.ButtonText = "_OK_";
            this.ButtonOK.ButtonTextMarginLeft = 0;
            this.ButtonOK.ColorContrastOnClick = 45;
            this.ButtonOK.ColorContrastOnHover = 45;
            this.ButtonOK.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges5.BottomLeft = true;
            borderEdges5.BottomRight = true;
            borderEdges5.TopLeft = true;
            borderEdges5.TopRight = true;
            this.ButtonOK.CustomizableEdges = borderEdges5;
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ButtonOK.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonOK.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonOK.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.ButtonOK.Enabled = false;
            this.ButtonOK.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonOK.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonOK.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOK.IconMarginLeft = 11;
            this.ButtonOK.IconPadding = 10;
            this.ButtonOK.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOK.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.IdleBorderRadius = 30;
            this.ButtonOK.IdleBorderThickness = 1;
            this.ButtonOK.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.IdleIconLeftImage = null;
            this.ButtonOK.IdleIconRightImage = null;
            this.ButtonOK.IndicateFocus = false;
            this.ButtonOK.Location = new System.Drawing.Point(292, 229);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonOK.onHoverState.BorderRadius = 30;
            this.ButtonOK.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.onHoverState.BorderThickness = 1;
            this.ButtonOK.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonOK.onHoverState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.onHoverState.IconLeftImage = null;
            this.ButtonOK.onHoverState.IconRightImage = null;
            this.ButtonOK.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.OnIdleState.BorderRadius = 30;
            this.ButtonOK.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.OnIdleState.BorderThickness = 1;
            this.ButtonOK.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.OnIdleState.IconLeftImage = null;
            this.ButtonOK.OnIdleState.IconRightImage = null;
            this.ButtonOK.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonOK.OnPressedState.BorderRadius = 30;
            this.ButtonOK.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.OnPressedState.BorderThickness = 1;
            this.ButtonOK.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonOK.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.OnPressedState.IconLeftImage = null;
            this.ButtonOK.OnPressedState.IconRightImage = null;
            this.ButtonOK.Size = new System.Drawing.Size(92, 37);
            this.ButtonOK.TabIndex = 3;
            this.ButtonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonOK.TextMarginLeft = 0;
            this.ButtonOK.UseDefaultRadiusAndThickness = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.AllowToggling = false;
            this.ButtonCancel.AnimationSpeed = 200;
            this.ButtonCancel.AutoGenerateColors = false;
            this.ButtonCancel.BackColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.BackColor1 = System.Drawing.Color.Transparent;
            this.ButtonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonCancel.BackgroundImage")));
            this.ButtonCancel.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.ButtonText = "_CANCEL_";
            this.ButtonCancel.ButtonTextMarginLeft = 0;
            this.ButtonCancel.ColorContrastOnClick = 45;
            this.ButtonCancel.ColorContrastOnHover = 45;
            this.ButtonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges6.BottomLeft = true;
            borderEdges6.BottomRight = true;
            borderEdges6.TopLeft = true;
            borderEdges6.TopRight = true;
            this.ButtonCancel.CustomizableEdges = borderEdges6;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonCancel.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonCancel.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.ButtonCancel.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonCancel.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCancel.IconMarginLeft = 11;
            this.ButtonCancel.IconPadding = 10;
            this.ButtonCancel.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCancel.IdleBorderColor = System.Drawing.Color.DimGray;
            this.ButtonCancel.IdleBorderRadius = 30;
            this.ButtonCancel.IdleBorderThickness = 1;
            this.ButtonCancel.IdleFillColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.IdleIconLeftImage = null;
            this.ButtonCancel.IdleIconRightImage = null;
            this.ButtonCancel.IndicateFocus = false;
            this.ButtonCancel.Location = new System.Drawing.Point(194, 229);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonCancel.onHoverState.BorderRadius = 30;
            this.ButtonCancel.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.onHoverState.BorderThickness = 1;
            this.ButtonCancel.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonCancel.onHoverState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.onHoverState.IconLeftImage = null;
            this.ButtonCancel.onHoverState.IconRightImage = null;
            this.ButtonCancel.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this.ButtonCancel.OnIdleState.BorderRadius = 30;
            this.ButtonCancel.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.OnIdleState.BorderThickness = 1;
            this.ButtonCancel.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.OnIdleState.IconLeftImage = null;
            this.ButtonCancel.OnIdleState.IconRightImage = null;
            this.ButtonCancel.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonCancel.OnPressedState.BorderRadius = 30;
            this.ButtonCancel.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.OnPressedState.BorderThickness = 1;
            this.ButtonCancel.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonCancel.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.OnPressedState.IconLeftImage = null;
            this.ButtonCancel.OnPressedState.IconRightImage = null;
            this.ButtonCancel.Size = new System.Drawing.Size(92, 37);
            this.ButtonCancel.TabIndex = 4;
            this.ButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonCancel.TextMarginLeft = 0;
            this.ButtonCancel.UseDefaultRadiusAndThickness = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // LabelTitle
            // 
            this.LabelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTitle.ForeColor = System.Drawing.Color.White;
            this.LabelTitle.Location = new System.Drawing.Point(22, 8);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelTitle.Size = new System.Drawing.Size(362, 50);
            this.LabelTitle.TabIndex = 17;
            this.LabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelTitle.AutoSize = false;
            this.LabelTitle.TextChanged += Title_TextChanged;
            // 
            // txtComments
            // 
            this.txtComments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.txtComments.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComments.ForeColor = System.Drawing.Color.White;
            this.txtComments.Location = new System.Drawing.Point(22, 109);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(362, 82);
            this.txtComments.TabIndex = 18;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lblAttendDate
            // 
            this.lblAttendDate.BackColor = System.Drawing.Color.Transparent;
            this.lblAttendDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.lblAttendDate.Location = new System.Drawing.Point(24, 79);
            this.lblAttendDate.Name = "lblAttendDate";
            this.lblAttendDate.Size = new System.Drawing.Size(281, 16);
            this.lblAttendDate.TabIndex = 20;
            this.lblAttendDate.Text = "Attend Date";
            // 
            // lblAttendByUser
            // 
            this.lblAttendByUser.ForeColor = System.Drawing.Color.White;
            this.lblAttendByUser.Location = new System.Drawing.Point(24, 61);
            this.lblAttendByUser.Name = "lblAttendByUser";
            this.lblAttendByUser.Size = new System.Drawing.Size(279, 16);
            this.lblAttendByUser.TabIndex = 19;
            this.lblAttendByUser.Text = "Attend By User";
            // 
            // AlertMessage
            // 
            this.AlertMessage.BackColor = System.Drawing.Color.Transparent;
            this.AlertMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.AlertMessage.Location = new System.Drawing.Point(24, 200);
            this.AlertMessage.Name = "AlertMessage";
            this.AlertMessage.Size = new System.Drawing.Size(281, 16);
            this.AlertMessage.TabIndex = 21;
            this.AlertMessage.Text = "Alert Message";
            this.AlertMessage.Visible = false;
            // 
            // AlarmDetailDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.AlertMessage);
            this.Controls.Add(this.lblAttendDate);
            this.Controls.Add(this.lblAttendByUser);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.LabelTitle);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Name = "AlarmDetailDialog";
            this.Size = new System.Drawing.Size(409, 280);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonOK;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonCancel;
        private Bunifu.Framework.UI.BunifuCustomLabel LabelTitle;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lblAttendDate;
        private System.Windows.Forms.Label lblAttendByUser;
        private System.Windows.Forms.Label AlertMessage;
    }
}
