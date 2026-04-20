using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System.Drawing;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class ItemAlarmHeader
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
            if (this.imgCamara.Image != null)
            {
                this.imgCamara.Image.Dispose();
            }
            //this.IconElement.Image.Dispose();
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
            this.lblFechaHora = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lblAcion = new System.Windows.Forms.Label();
            this.lblMessage = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lblDiscard = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lblViewAlarm = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lblDeviceName = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.bttpItemAlarm = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.picAlarmGeolocation = new System.Windows.Forms.PictureBox();
            this.imgCamara = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAlarmGeolocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCamara)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFechaHora
            // 
            this.lblFechaHora.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFechaHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaHora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.lblFechaHora.Location = new System.Drawing.Point(160, 52);
            this.lblFechaHora.Name = "lblFechaHora";
            this.lblFechaHora.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFechaHora.Size = new System.Drawing.Size(282, 18);
            this.lblFechaHora.TabIndex = 3;
            this.lblFechaHora.Text = "lblFechaHora";
            this.bttpItemAlarm.SetToolTip(this.lblFechaHora, "");
            this.bttpItemAlarm.SetToolTipIcon(this.lblFechaHora, null);
            this.bttpItemAlarm.SetToolTipTitle(this.lblFechaHora, "");
            // 
            // lblAcion
            // 
            this.lblAcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcion.ForeColor = System.Drawing.Color.White;
            this.lblAcion.Location = new System.Drawing.Point(160, 13);
            this.lblAcion.Name = "lblAcion";
            this.lblAcion.Size = new System.Drawing.Size(282, 17);
            this.lblAcion.TabIndex = 8;
            this.lblAcion.Text = "Device Name";
            this.bttpItemAlarm.SetToolTip(this.lblAcion, "");
            this.bttpItemAlarm.SetToolTipIcon(this.lblAcion, null);
            this.bttpItemAlarm.SetToolTipTitle(this.lblAcion, "");
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(160, 68);
            this.lblMessage.MaximumSize = new System.Drawing.Size(340, 85);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(282, 15);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "lblMessage";
            this.bttpItemAlarm.SetToolTip(this.lblMessage, "");
            this.bttpItemAlarm.SetToolTipIcon(this.lblMessage, null);
            this.bttpItemAlarm.SetToolTipTitle(this.lblMessage, "");
            // 
            // lblDiscard
            // 
            this.lblDiscard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDiscard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscard.ForeColor = System.Drawing.Color.White;
            this.lblDiscard.Location = new System.Drawing.Point(161, 91);
            this.lblDiscard.MaximumSize = new System.Drawing.Size(340, 80);
            this.lblDiscard.Name = "lblDiscard";
            this.lblDiscard.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDiscard.Size = new System.Drawing.Size(88, 25);
            this.lblDiscard.TabIndex = 12;
            this.lblDiscard.Text = "Discard";
            this.bttpItemAlarm.SetToolTip(this.lblDiscard, "");
            this.bttpItemAlarm.SetToolTipIcon(this.lblDiscard, null);
            this.bttpItemAlarm.SetToolTipTitle(this.lblDiscard, "");
            // 
            // lblViewAlarm
            // 
            this.lblViewAlarm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblViewAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViewAlarm.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblViewAlarm.Location = new System.Drawing.Point(255, 91);
            this.lblViewAlarm.MaximumSize = new System.Drawing.Size(340, 80);
            this.lblViewAlarm.Name = "lblViewAlarm";
            this.lblViewAlarm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblViewAlarm.Size = new System.Drawing.Size(88, 25);
            this.lblViewAlarm.TabIndex = 13;
            this.lblViewAlarm.Text = "View Alarm";
            this.bttpItemAlarm.SetToolTip(this.lblViewAlarm, "");
            this.bttpItemAlarm.SetToolTipIcon(this.lblViewAlarm, null);
            this.bttpItemAlarm.SetToolTipTitle(this.lblViewAlarm, "");
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeviceName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDeviceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.lblDeviceName.Location = new System.Drawing.Point(161, 33);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDeviceName.Size = new System.Drawing.Size(282, 18);
            this.lblDeviceName.TabIndex = 14;
            this.lblDeviceName.Text = "lblDeviceName";
            this.lblDeviceName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bttpItemAlarm.SetToolTip(this.lblDeviceName, "");
            this.bttpItemAlarm.SetToolTipIcon(this.lblDeviceName, null);
            this.bttpItemAlarm.SetToolTipTitle(this.lblDeviceName, "");
            // 
            // bttpItemAlarm
            // 
            this.bttpItemAlarm.Active = true;
            this.bttpItemAlarm.AlignTextWithTitle = false;
            this.bttpItemAlarm.AllowAutoClose = true;
            this.bttpItemAlarm.AllowFading = true;
            this.bttpItemAlarm.AutoCloseDuration = 2000;
            this.bttpItemAlarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.bttpItemAlarm.BorderColor = System.Drawing.Color.Black;
            this.bttpItemAlarm.ClickToShowDisplayControl = false;
            this.bttpItemAlarm.ConvertNewlinesToBreakTags = true;
            this.bttpItemAlarm.DisplayControl = null;
            this.bttpItemAlarm.EntryAnimationSpeed = 150;
            this.bttpItemAlarm.ExitAnimationSpeed = 200;
            this.bttpItemAlarm.GenerateAutoCloseDuration = false;
            this.bttpItemAlarm.IconMargin = 6;
            this.bttpItemAlarm.InitialDelay = 0;
            this.bttpItemAlarm.Name = "bttpItemAlarm";
            this.bttpItemAlarm.Opacity = 1D;
            this.bttpItemAlarm.OverrideToolTipTitles = false;
            this.bttpItemAlarm.Padding = new System.Windows.Forms.Padding(10);
            this.bttpItemAlarm.ReshowDelay = 100;
            this.bttpItemAlarm.ShowAlways = true;
            this.bttpItemAlarm.ShowBorders = false;
            this.bttpItemAlarm.ShowIcons = true;
            this.bttpItemAlarm.ShowShadows = true;
            this.bttpItemAlarm.Tag = null;
            this.bttpItemAlarm.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.bttpItemAlarm.TextForeColor = System.Drawing.Color.White;
            this.bttpItemAlarm.TextMargin = 2;
            this.bttpItemAlarm.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bttpItemAlarm.TitleForeColor = System.Drawing.Color.Black;
            this.bttpItemAlarm.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.bttpItemAlarm.ToolTipTitle = null;
            // 
            // picAlarmGeolocation
            // 
            this.picAlarmGeolocation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAlarmGeolocation.Image = global::Elipgo.SmartClient.UserControls.Properties.Resources.icon_location_sidebar;
            this.picAlarmGeolocation.Location = new System.Drawing.Point(349, 91);
            this.picAlarmGeolocation.Name = "picAlarmGeolocation";
            this.picAlarmGeolocation.Size = new System.Drawing.Size(18, 18);
            this.picAlarmGeolocation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAlarmGeolocation.TabIndex = 15;
            this.picAlarmGeolocation.TabStop = false;
            this.bttpItemAlarm.SetToolTip(this.picAlarmGeolocation, "");
            this.bttpItemAlarm.SetToolTipIcon(this.picAlarmGeolocation, null);
            this.bttpItemAlarm.SetToolTipTitle(this.picAlarmGeolocation, "");
            // 
            // imgCamara
            // 
            this.imgCamara.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.imgCamara.Location = new System.Drawing.Point(6, 11);
            this.imgCamara.Name = "imgCamara";
            this.imgCamara.Size = new System.Drawing.Size(147, 102);
            this.imgCamara.TabIndex = 10;
            this.imgCamara.TabStop = false;
            this.bttpItemAlarm.SetToolTip(this.imgCamara, "");
            this.bttpItemAlarm.SetToolTipIcon(this.imgCamara, null);
            this.bttpItemAlarm.SetToolTipTitle(this.imgCamara, "");
            // 
            // ItemAlarmHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.picAlarmGeolocation);
            this.Controls.Add(this.lblDeviceName);
            this.Controls.Add(this.lblViewAlarm);
            this.Controls.Add(this.lblDiscard);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.imgCamara);
            this.Controls.Add(this.lblAcion);
            this.Controls.Add(this.lblFechaHora);
            this.Name = "ItemAlarmHeader";
            this.Size = new System.Drawing.Size(451, 123);
            this.bttpItemAlarm.SetToolTip(this, "");
            this.bttpItemAlarm.SetToolTipIcon(this, null);
            this.bttpItemAlarm.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this.picAlarmGeolocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCamara)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.Framework.UI.BunifuCustomLabel lblFechaHora;
        private System.Windows.Forms.Label lblAcion;
        private System.Windows.Forms.PictureBox imgCamara;
        private Bunifu.Framework.UI.BunifuCustomLabel lblMessage;
        private Bunifu.Framework.UI.BunifuCustomLabel lblDiscard;
        private Bunifu.Framework.UI.BunifuCustomLabel lblViewAlarm;
        private Bunifu.Framework.UI.BunifuCustomLabel lblDeviceName;
        private Bunifu.UI.WinForms.BunifuToolTip bttpItemAlarm;
        private System.Windows.Forms.PictureBox picAlarmGeolocation;
    }
}
