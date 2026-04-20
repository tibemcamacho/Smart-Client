using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Dialog
{
    partial class DialogReadOnlyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogReadOnlyControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.LabelTitle = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.ButtonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.ScrollBar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelTitle
            // 
            this.LabelTitle.AutoEllipsis = false;
            this.LabelTitle.AutoSize = false;
            this.LabelTitle.Cursor = null;
            this.LabelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTitle.Location = new System.Drawing.Point(22, 37);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelTitle.Size = new System.Drawing.Size(188, 37);
            this.LabelTitle.TabIndex = 0;
            this.LabelTitle.Text = "_GRID_";
            this.LabelTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //this.LabelTitle.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.ColumnCount = 5;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.TableLayoutPanel.Location = new System.Drawing.Point(22, 91);
            this.TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.Size = new System.Drawing.Size(900, 708);
            this.TableLayoutPanel.TabIndex = 0;
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
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.ButtonOK.CustomizableEdges = borderEdges1;
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ButtonOK.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonOK.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonOK.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
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
            this.ButtonOK.Location = new System.Drawing.Point(847, 825);
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
            this.ButtonOK.TabIndex = 2;
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
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.ButtonCancel.CustomizableEdges = borderEdges2;
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
            this.ButtonCancel.Location = new System.Drawing.Point(753, 826);
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
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonCancel.TextMarginLeft = 0;
            this.ButtonCancel.UseDefaultRadiusAndThickness = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ScrollBar
            // 
            this.ScrollBar.AllowCursorChanges = true;
            this.ScrollBar.AllowHomeEndKeysDetection = false;
            this.ScrollBar.AllowIncrementalClickMoves = true;
            this.ScrollBar.AllowMouseDownEffects = true;
            this.ScrollBar.AllowMouseHoverEffects = true;
            this.ScrollBar.AllowScrollingAnimations = true;
            this.ScrollBar.AllowScrollKeysDetection = true;
            this.ScrollBar.AllowScrollOptionsMenu = true;
            this.ScrollBar.AllowShrinkingOnFocusLost = false;
            this.ScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollBar.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ScrollBar.BackgroundImage")));
            this.ScrollBar.BindingContainer = null;
            this.ScrollBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.BorderRadius = 0;
            this.ScrollBar.BorderThickness = 1;
            this.ScrollBar.DurationBeforeShrink = 2000;
            this.ScrollBar.LargeChange = 10;
            this.ScrollBar.Location = new System.Drawing.Point(925, 91);
            this.ScrollBar.Maximum = 100;
            this.ScrollBar.Minimum = 0;
            this.ScrollBar.MinimumThumbLength = 18;
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.ScrollBar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.ScrollBar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.ScrollBar.ScrollBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.ShrinkSizeLimit = 3;
            this.ScrollBar.Size = new System.Drawing.Size(16, 708);
            this.ScrollBar.SmallChange = 1;
            this.ScrollBar.TabIndex = 8;
            this.ScrollBar.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.ScrollBar.ThumbLength = 69;
            this.ScrollBar.ThumbMargin = 1;
            this.ScrollBar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this.ScrollBar.Value = 0;
            // 
            // PanelHeader
            // 
            //this.PanelHeader.AllowDrop = true;
            this.PanelHeader.BackColor = System.Drawing.Color.Black;
            this.PanelHeader.Controls.Add(this.ButtonClose);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(963, 32);
            this.PanelHeader.TabIndex = 9;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this.ButtonClose.ImageActive = null;
            this.ButtonClose.Location = new System.Drawing.Point(925, 3);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(24, 24);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Zoom = 10;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // DialogReadOnlyControl
            // 
            var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelHeader);
            this.Controls.Add(this.ScrollBar);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.TableLayoutPanel);
            this.Controls.Add(this.LabelTitle);
            this.Name = "DialogReadOnlyControl";

            //2049 x 1280  125%
            if (workingArea.Width == 2048 && workingArea.Height == 1280)
            {
                this.Size = new System.Drawing.Size(1100, 1050);
            } else
            {
               this.Size = new System.Drawing.Size(963, 880);
            }
            this.PanelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCustomLabel LabelTitle;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonOK;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonCancel;
        private Bunifu.UI.WinForms.BunifuVScrollBar ScrollBar;
        private System.Windows.Forms.Panel PanelHeader;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
    }
}
