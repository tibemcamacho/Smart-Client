namespace Elipgo.SmartClient.UserControls.UserProfile
{
    partial class UserProfileComponent
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
            this.CustomDispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserProfileComponent));
            this._bunifuImageButtonProfile = new Bunifu.Framework.UI.BunifuImageButton();
            this._bunToolTipCurrentUser = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._bunifuImageButtonProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProfile
            // 
            this._bunifuImageButtonProfile.BackColor = System.Drawing.Color.Transparent;
            this._bunifuImageButtonProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._bunifuImageButtonProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this._bunifuImageButtonProfile.ErrorImage = null;
            this._bunifuImageButtonProfile.Image = ((System.Drawing.Image)(resources.GetObject("btnProfile.Image")));
            this._bunifuImageButtonProfile.ImageActive = null;
            this._bunifuImageButtonProfile.InitialImage = null;
            this._bunifuImageButtonProfile.Location = new System.Drawing.Point(0, 0);
            this._bunifuImageButtonProfile.Margin = new System.Windows.Forms.Padding(0);
            this._bunifuImageButtonProfile.Name = "btnProfile";
            this._bunifuImageButtonProfile.Size = new System.Drawing.Size(42, 44);
            this._bunifuImageButtonProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._bunifuImageButtonProfile.TabIndex = 0;
            this._bunifuImageButtonProfile.TabStop = false;
            this._bunToolTipCurrentUser.SetToolTip(this._bunifuImageButtonProfile, "");
            this._bunToolTipCurrentUser.SetToolTipIcon(this._bunifuImageButtonProfile, null);
            this._bunToolTipCurrentUser.SetToolTipTitle(this._bunifuImageButtonProfile, "");
            this._bunifuImageButtonProfile.Zoom = 10;
            this._bunifuImageButtonProfile.Click += new System.EventHandler(this.ButtonProfile_Click);
            // 
            // bunToolTipCurrentUser
            // 
            this._bunToolTipCurrentUser.Active = true;
            this._bunToolTipCurrentUser.AlignTextWithTitle = false;
            this._bunToolTipCurrentUser.AllowAutoClose = true;
            this._bunToolTipCurrentUser.AllowFading = true;
            this._bunToolTipCurrentUser.AutoCloseDuration = 2000;
            this._bunToolTipCurrentUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this._bunToolTipCurrentUser.BorderColor = System.Drawing.Color.Black;
            this._bunToolTipCurrentUser.ClickToShowDisplayControl = false;
            this._bunToolTipCurrentUser.ConvertNewlinesToBreakTags = true;
            this._bunToolTipCurrentUser.DisplayControl = null;
            this._bunToolTipCurrentUser.EntryAnimationSpeed = 350;
            this._bunToolTipCurrentUser.ExitAnimationSpeed = 200;
            this._bunToolTipCurrentUser.GenerateAutoCloseDuration = false;
            this._bunToolTipCurrentUser.IconMargin = 6;
            this._bunToolTipCurrentUser.InitialDelay = 0;
            this._bunToolTipCurrentUser.Name = "bunToolTipCurrentUser";
            this._bunToolTipCurrentUser.Opacity = 1D;
            this._bunToolTipCurrentUser.OverrideToolTipTitles = false;
            this._bunToolTipCurrentUser.Padding = new System.Windows.Forms.Padding(10);
            this._bunToolTipCurrentUser.ReshowDelay = 100;
            this._bunToolTipCurrentUser.ShowAlways = true;
            this._bunToolTipCurrentUser.ShowBorders = false;
            this._bunToolTipCurrentUser.ShowIcons = true;
            this._bunToolTipCurrentUser.ShowShadows = true;
            this._bunToolTipCurrentUser.Tag = null;
            this._bunToolTipCurrentUser.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this._bunToolTipCurrentUser.TextForeColor = System.Drawing.Color.White;
            this._bunToolTipCurrentUser.TextMargin = 2;
            this._bunToolTipCurrentUser.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._bunToolTipCurrentUser.TitleForeColor = System.Drawing.Color.Black;
            this._bunToolTipCurrentUser.ToolTipPosition = new System.Drawing.Point(0, 0);
            this._bunToolTipCurrentUser.ToolTipTitle = null;
            // 
            // UserProfileComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._bunifuImageButtonProfile);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserProfileComponent";
            this.Size = new System.Drawing.Size(42, 44);
            this._bunToolTipCurrentUser.SetToolTip(this, "");
            this._bunToolTipCurrentUser.SetToolTipIcon(this, null);
            this._bunToolTipCurrentUser.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this._bunifuImageButtonProfile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuImageButton _bunifuImageButtonProfile;
        private Bunifu.UI.WinForms.BunifuToolTip _bunToolTipCurrentUser;
    }
}
