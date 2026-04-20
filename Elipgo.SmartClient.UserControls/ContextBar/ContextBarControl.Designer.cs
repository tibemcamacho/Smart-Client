namespace Elipgo.SmartClient.UserControls.ContextBar
{
    partial class ContextBarControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextBarControl));
            this._removeButton = new Bunifu.Framework.UI.BunifuImageButton();
            this._iconElement = new System.Windows.Forms.PictureBox();
            this._elementName = new System.Windows.Forms.Label();
            this._groupName = new System.Windows.Forms.Label();
            this._bunifuToolTip = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this._elementNvrName = new System.Windows.Forms.Label();
            this._levelContext = new System.Windows.Forms.Label();
            this._rangeContext = new System.Windows.Forms.Label();
            
            ((System.ComponentModel.ISupportInitialize)(this._removeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._iconElement)).BeginInit();
            this.SuspendLayout();
            // 
            // Remove
            // 
            this._removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._removeButton.BackColor = System.Drawing.Color.Transparent;
            this._removeButton.ErrorImage = ((System.Drawing.Image)(resources.GetObject("Remove.ErrorImage")));
            this._removeButton.Image = ((System.Drawing.Image)(resources.GetObject("Remove.Image")));
            this._removeButton.ImageActive = null;
            this._removeButton.InitialImage = ((System.Drawing.Image)(resources.GetObject("Remove.InitialImage")));
            this._removeButton.Location = new System.Drawing.Point(1562, 16);
            this._removeButton.Margin = new System.Windows.Forms.Padding(0);
            this._removeButton.Name = "Remove";
            this._removeButton.Size = new System.Drawing.Size(24, 24);
            this._removeButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._removeButton.TabIndex = 0;
            this._removeButton.TabStop = false;
            this._bunifuToolTip.SetToolTip(this._removeButton, "");
            this._bunifuToolTip.SetToolTipIcon(this._removeButton, null);
            this._bunifuToolTip.SetToolTipTitle(this._removeButton, "");
            this._removeButton.Zoom = 10;
            // 
            // IconElement
            // 
            this._iconElement.Location = new System.Drawing.Point(79, 14);
            this._iconElement.Margin = new System.Windows.Forms.Padding(0);
            this._iconElement.Name = "IconElement";
            this._iconElement.Size = new System.Drawing.Size(32, 32);
            this._iconElement.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._iconElement.TabIndex = 1;
            this._iconElement.TabStop = false;
            this._bunifuToolTip.SetToolTip(this._iconElement, "");
            this._bunifuToolTip.SetToolTipIcon(this._iconElement, null);
            this._bunifuToolTip.SetToolTipTitle(this._iconElement, "");
            // 
            // ElementName
            // 
            this._elementName.ForeColor = System.Drawing.Color.White;
            this._elementName.Location = new System.Drawing.Point(117, 0);
            this._elementName.Name = "ElementName";
            this._elementName.Size = new System.Drawing.Size(300, 30);
            this._elementName.TabIndex = 2;
            this._elementName.Text = "Device Name";
            this._elementName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this._bunifuToolTip.SetToolTip(this._elementName, "");
            this._bunifuToolTip.SetToolTipIcon(this._elementName, null);
            this._bunifuToolTip.SetToolTipTitle(this._elementName, "");
            // 
            // GroupName
            // 
            this._groupName.ForeColor = System.Drawing.Color.White;
            this._groupName.Location = new System.Drawing.Point(117, 30);
            this._groupName.Name = "GroupName";
            this._groupName.Size = new System.Drawing.Size(300, 30);
            this._groupName.TabIndex = 3;
            this._groupName.Text = "Device Name";
            this._bunifuToolTip.SetToolTip(this._groupName, "");
            this._bunifuToolTip.SetToolTipIcon(this._groupName, null);
            this._bunifuToolTip.SetToolTipTitle(this._groupName, "");
            // 
            // bunifuToolTip1
            // 
            this._bunifuToolTip.Active = true;
            this._bunifuToolTip.AlignTextWithTitle = false;
            this._bunifuToolTip.AllowAutoClose = true;
            this._bunifuToolTip.AllowFading = true;
            this._bunifuToolTip.AutoCloseDuration = 1000;
            this._bunifuToolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this._bunifuToolTip.BorderColor = System.Drawing.Color.Black;
            this._bunifuToolTip.ClickToShowDisplayControl = false;
            this._bunifuToolTip.ConvertNewlinesToBreakTags = true;
            this._bunifuToolTip.DisplayControl = null;
            this._bunifuToolTip.EntryAnimationSpeed = 350;
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
            // ElementNvrName
            // 
            this._elementNvrName.AutoSize = true;
            this._elementNvrName.ForeColor = System.Drawing.Color.White;
            this._elementNvrName.Location = new System.Drawing.Point(77, 39);
            this._elementNvrName.MinimumSize = new System.Drawing.Size(300, 16);
            this._elementNvrName.Name = "ElementNvrName";
            this._elementNvrName.Size = new System.Drawing.Size(300, 16);
            this._elementNvrName.TabIndex = 4;
            this._elementNvrName.Text = "Device Name";
            this._bunifuToolTip.SetToolTip(this._elementNvrName, "");
            this._bunifuToolTip.SetToolTipIcon(this._elementNvrName, null);
            this._bunifuToolTip.SetToolTipTitle(this._elementNvrName, "");
            this._elementNvrName.Visible = false;
            // 
            // LevelContext
            // 
            this._levelContext.ForeColor = System.Drawing.Color.White;
            this._levelContext.Location = new System.Drawing.Point(423, 0);
            this._levelContext.Name = "LevelContext";
            this._levelContext.Size = new System.Drawing.Size(200, 30);
            this._levelContext.TabIndex = 4;
            this._levelContext.Text = "ContextLevel";
            this._levelContext.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this._bunifuToolTip.SetToolTip(this._levelContext, "");
            this._bunifuToolTip.SetToolTipIcon(this._levelContext, null);
            this._bunifuToolTip.SetToolTipTitle(this._levelContext, "");
            this._levelContext.Visible = false;
            // 
            // RangeContext
            // 
            this._rangeContext.ForeColor = System.Drawing.Color.White;
            this._rangeContext.Location = new System.Drawing.Point(423, 30);
            this._rangeContext.Name = "RangeContext";
            this._rangeContext.Size = new System.Drawing.Size(200, 30);
            this._rangeContext.TabIndex = 5;
            this._rangeContext.Text = "CotextRange";
            this._bunifuToolTip.SetToolTip(this._rangeContext, "");
            this._bunifuToolTip.SetToolTipIcon(this._rangeContext, null);
            this._bunifuToolTip.SetToolTipTitle(this._rangeContext, "");
            this._rangeContext.Visible = false;
            // 
            //
            // 
            // ContextBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            
            this.Controls.Add(this._elementNvrName);
            this.Controls.Add(this._groupName);
            this.Controls.Add(this._elementName);
            this.Controls.Add(this._iconElement);
            this.Controls.Add(this._removeButton);
            this.Controls.Add(this._levelContext);
            this.Controls.Add(this._rangeContext);
            this.Name = "ContextBarControl";
            this.Size = new System.Drawing.Size(1600, 60);
            this._bunifuToolTip.SetToolTip(this, "");
            this._bunifuToolTip.SetToolTipIcon(this, null);
            this._bunifuToolTip.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this._removeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._iconElement)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuImageButton _removeButton;
        private System.Windows.Forms.PictureBox _iconElement;
        private System.Windows.Forms.Label _elementName;
        private System.Windows.Forms.Label _groupName;
        private Bunifu.UI.WinForms.BunifuToolTip _bunifuToolTip;
        private System.Windows.Forms.Label _elementNvrName;
        private System.Windows.Forms.Label _levelContext;
        private System.Windows.Forms.Label _rangeContext;
      
    }
}
