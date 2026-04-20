using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System.Drawing;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    partial class ItemCard
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
            this.IconElement.Image.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemCard));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.btnDescartar = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnDiagnosticar = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnRetrieve = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.btnDetail = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.lblModelo = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lblFechaHora = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lblPais = new System.Windows.Forms.Label();
            this.lblAcion = new System.Windows.Forms.Label();
            this.IconElement = new System.Windows.Forms.PictureBox();
            this.imgCamara = new System.Windows.Forms.PictureBox();
            this.lblMessage = new Bunifu.Framework.UI.BunifuCustomLabel();
            ((System.ComponentModel.ISupportInitialize)(this.IconElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCamara)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDescartar
            // 
            this.btnDescartar.AllowToggling = false;
            this.btnDescartar.AnimationSpeed = 200;
            this.btnDescartar.AutoGenerateColors = false;
            this.btnDescartar.BackColor = System.Drawing.Color.Transparent;
            this.btnDescartar.BackColor1 = System.Drawing.Color.Transparent;
            this.btnDescartar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDescartar.BackgroundImage")));
            this.btnDescartar.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDescartar.ButtonText = "Descartar";
            this.btnDescartar.ButtonTextMarginLeft = 0;
            this.btnDescartar.ColorContrastOnClick = 45;
            this.btnDescartar.ColorContrastOnHover = 45;
            this.btnDescartar.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnDescartar.CustomizableEdges = borderEdges1;
            this.btnDescartar.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnDescartar.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDescartar.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnDescartar.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnDescartar.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.btnDescartar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnDescartar.ForeColor = System.Drawing.Color.White;
            this.btnDescartar.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescartar.IconMarginLeft = 11;
            this.btnDescartar.IconPadding = 10;
            this.btnDescartar.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.btnDescartar.IdleBorderColor = System.Drawing.Color.White;
            this.btnDescartar.IdleBorderRadius = 32;
            this.btnDescartar.IdleBorderThickness = 1;
            this.btnDescartar.IdleFillColor = System.Drawing.Color.Transparent;
            this.btnDescartar.IdleIconLeftImage = null;
            this.btnDescartar.IdleIconRightImage = null;
            this.btnDescartar.IndicateFocus = false;
            this.btnDescartar.Location = new System.Drawing.Point(109, 354);
            this.btnDescartar.Name = "btnDescartar";
            this.btnDescartar.onHoverState.BorderColor = System.Drawing.Color.Transparent;
            this.btnDescartar.onHoverState.BorderRadius = 32;
            this.btnDescartar.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDescartar.onHoverState.BorderThickness = 1;
            this.btnDescartar.onHoverState.FillColor = System.Drawing.Color.Transparent;
            this.btnDescartar.onHoverState.ForeColor = System.Drawing.Color.Transparent;
            this.btnDescartar.onHoverState.IconLeftImage = null;
            this.btnDescartar.onHoverState.IconRightImage = null;
            this.btnDescartar.OnIdleState.BorderColor = System.Drawing.Color.White;
            this.btnDescartar.OnIdleState.BorderRadius = 32;
            this.btnDescartar.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDescartar.OnIdleState.BorderThickness = 1;
            this.btnDescartar.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this.btnDescartar.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnDescartar.OnIdleState.IconLeftImage = null;
            this.btnDescartar.OnIdleState.IconRightImage = null;
            this.btnDescartar.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnDescartar.OnPressedState.BorderRadius = 32;
            this.btnDescartar.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDescartar.OnPressedState.BorderThickness = 1;
            this.btnDescartar.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnDescartar.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnDescartar.OnPressedState.IconLeftImage = null;
            this.btnDescartar.OnPressedState.IconRightImage = null;
            this.btnDescartar.Size = new System.Drawing.Size(92, 36);
            this.btnDescartar.TabIndex = 0;
            this.btnDescartar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDescartar.TextMarginLeft = 0;
            this.btnDescartar.UseDefaultRadiusAndThickness = true;
            this.btnDescartar.Visible = false;
            // 
            // btnDiagnosticar
            // 
            this.btnDiagnosticar.AllowToggling = false;
            this.btnDiagnosticar.AnimationSpeed = 200;
            this.btnDiagnosticar.AutoGenerateColors = false;
            this.btnDiagnosticar.BackColor = System.Drawing.Color.Transparent;
            this.btnDiagnosticar.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.btnDiagnosticar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDiagnosticar.BackgroundImage")));
            this.btnDiagnosticar.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDiagnosticar.ButtonText = "Diagnosticar";
            this.btnDiagnosticar.ButtonTextMarginLeft = 0;
            this.btnDiagnosticar.ColorContrastOnClick = 45;
            this.btnDiagnosticar.ColorContrastOnHover = 45;
            this.btnDiagnosticar.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnDiagnosticar.CustomizableEdges = borderEdges2;
            this.btnDiagnosticar.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnDiagnosticar.DisabledBorderColor = System.Drawing.Color.Empty;
            this.btnDiagnosticar.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnDiagnosticar.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnDiagnosticar.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.btnDiagnosticar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnDiagnosticar.ForeColor = System.Drawing.Color.White;
            this.btnDiagnosticar.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiagnosticar.IconMarginLeft = 11;
            this.btnDiagnosticar.IconPadding = 10;
            this.btnDiagnosticar.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiagnosticar.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.btnDiagnosticar.IdleBorderRadius = 32;
            this.btnDiagnosticar.IdleBorderThickness = 1;
            this.btnDiagnosticar.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.btnDiagnosticar.IdleIconLeftImage = null;
            this.btnDiagnosticar.IdleIconRightImage = null;
            this.btnDiagnosticar.IndicateFocus = false;
            this.btnDiagnosticar.Location = new System.Drawing.Point(215, 354);
            this.btnDiagnosticar.Name = "btnDiagnosticar";
            this.btnDiagnosticar.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.btnDiagnosticar.onHoverState.BorderRadius = 32;
            this.btnDiagnosticar.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDiagnosticar.onHoverState.BorderThickness = 1;
            this.btnDiagnosticar.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.btnDiagnosticar.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnDiagnosticar.onHoverState.IconLeftImage = null;
            this.btnDiagnosticar.onHoverState.IconRightImage = null;
            this.btnDiagnosticar.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnDiagnosticar.OnIdleState.BorderRadius = 32;
            this.btnDiagnosticar.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDiagnosticar.OnIdleState.BorderThickness = 1;
            this.btnDiagnosticar.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this.btnDiagnosticar.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnDiagnosticar.OnIdleState.IconLeftImage = null;
            this.btnDiagnosticar.OnIdleState.IconRightImage = null;
            this.btnDiagnosticar.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnDiagnosticar.OnPressedState.BorderRadius = 32;
            this.btnDiagnosticar.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDiagnosticar.OnPressedState.BorderThickness = 1;
            this.btnDiagnosticar.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnDiagnosticar.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnDiagnosticar.OnPressedState.IconLeftImage = null;
            this.btnDiagnosticar.OnPressedState.IconRightImage = null;
            this.btnDiagnosticar.Size = new System.Drawing.Size(118, 36);
            this.btnDiagnosticar.TabIndex = 1;
            this.btnDiagnosticar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDiagnosticar.TextMarginLeft = 0;
            this.btnDiagnosticar.UseDefaultRadiusAndThickness = true;
            this.btnDiagnosticar.Visible = false;
            this.btnDiagnosticar.Click += new System.EventHandler(this.BtnDiagnosticar_Click);
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.AllowToggling = false;
            this.btnRetrieve.AnimationSpeed = 200;
            this.btnRetrieve.AutoGenerateColors = false;
            this.btnRetrieve.BackColor = System.Drawing.Color.Transparent;
            this.btnRetrieve.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.btnRetrieve.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDiagnosticar.BackgroundImage")));
            this.btnRetrieve.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRetrieve.ButtonText = "Recuperar";
            this.btnRetrieve.ButtonTextMarginLeft = 0;
            this.btnRetrieve.ColorContrastOnClick = 45;
            this.btnRetrieve.ColorContrastOnHover = 45;
            this.btnRetrieve.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.btnRetrieve.CustomizableEdges = borderEdges2;
            this.btnRetrieve.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnRetrieve.DisabledBorderColor = System.Drawing.Color.Empty;
            this.btnRetrieve.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnRetrieve.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnRetrieve.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.btnRetrieve.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnRetrieve.ForeColor = System.Drawing.Color.White;
            this.btnRetrieve.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.btnRetrieve.IconMarginLeft = 11;
            this.btnRetrieve.IconPadding = 10;
            this.btnRetrieve.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.btnRetrieve.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.btnRetrieve.IdleBorderRadius = 32;
            this.btnRetrieve.IdleBorderThickness = 1;
            this.btnRetrieve.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.btnRetrieve.IdleIconLeftImage = null;
            this.btnRetrieve.IdleIconRightImage = null;
            this.btnRetrieve.IndicateFocus = false;
            this.btnRetrieve.Location = new System.Drawing.Point(215, 354);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.btnRetrieve.onHoverState.BorderRadius = 32;
            this.btnRetrieve.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRetrieve.onHoverState.BorderThickness = 1;
            this.btnRetrieve.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.btnRetrieve.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnRetrieve.onHoverState.IconLeftImage = null;
            this.btnRetrieve.onHoverState.IconRightImage = null;
            this.btnRetrieve.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnRetrieve.OnIdleState.BorderRadius = 32;
            this.btnRetrieve.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRetrieve.OnIdleState.BorderThickness = 1;
            this.btnRetrieve.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this.btnRetrieve.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnRetrieve.OnIdleState.IconLeftImage = null;
            this.btnRetrieve.OnIdleState.IconRightImage = null;
            this.btnRetrieve.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnRetrieve.OnPressedState.BorderRadius = 32;
            this.btnRetrieve.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnRetrieve.OnPressedState.BorderThickness = 1;
            this.btnRetrieve.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnRetrieve.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnRetrieve.OnPressedState.IconLeftImage = null;
            this.btnRetrieve.OnPressedState.IconRightImage = null;
            this.btnRetrieve.Size = new System.Drawing.Size(118, 36);
            this.btnRetrieve.TabIndex = 1;
            this.btnRetrieve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRetrieve.TextMarginLeft = 0;
            this.btnRetrieve.UseDefaultRadiusAndThickness = true;
            this.btnRetrieve.Visible = false;
            // 
            // btnDetail
            // 
            this.btnDetail.AllowToggling = false;
            this.btnDetail.AnimationSpeed = 200;
            this.btnDetail.AutoGenerateColors = false;
            this.btnDetail.BackColor = System.Drawing.Color.Transparent;
            this.btnDetail.BackColor1 = System.Drawing.Color.Transparent;
            this.btnDetail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDescartar.BackgroundImage")));
            this.btnDetail.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDetail.ButtonText = "Detalle";
            this.btnDetail.ButtonTextMarginLeft = 0;
            this.btnDetail.ColorContrastOnClick = 45;
            this.btnDetail.ColorContrastOnHover = 45;
            this.btnDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnDetail.CustomizableEdges = borderEdges1;
            this.btnDetail.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnDetail.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDetail.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnDetail.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnDetail.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.btnDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.btnDetail.ForeColor = System.Drawing.Color.White;
            this.btnDetail.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.btnDetail.IconMarginLeft = 11;
            this.btnDetail.IconPadding = 10;
            this.btnDetail.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.btnDetail.IdleBorderColor = System.Drawing.Color.White;
            this.btnDetail.IdleBorderRadius = 32;
            this.btnDetail.IdleBorderThickness = 1;
            this.btnDetail.IdleFillColor = System.Drawing.Color.Transparent;
            this.btnDetail.IdleIconLeftImage = null;
            this.btnDetail.IdleIconRightImage = null;
            this.btnDetail.IndicateFocus = false;
            this.btnDetail.Location = new System.Drawing.Point(215, 354);
            this.btnDetail.Name = "btnDescartar";
            this.btnDetail.onHoverState.BorderColor = System.Drawing.Color.Transparent;
            this.btnDetail.onHoverState.BorderRadius = 32;
            this.btnDetail.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDetail.onHoverState.BorderThickness = 1;
            this.btnDetail.onHoverState.FillColor = System.Drawing.Color.Transparent;
            this.btnDetail.onHoverState.ForeColor = System.Drawing.Color.Transparent;
            this.btnDetail.onHoverState.IconLeftImage = null;
            this.btnDetail.onHoverState.IconRightImage = null;
            this.btnDetail.OnIdleState.BorderColor = System.Drawing.Color.White;
            this.btnDetail.OnIdleState.BorderRadius = 32;
            this.btnDetail.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDetail.OnIdleState.BorderThickness = 1;
            this.btnDetail.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this.btnDetail.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnDetail.OnIdleState.IconLeftImage = null;
            this.btnDetail.OnIdleState.IconRightImage = null;
            this.btnDetail.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnDetail.OnPressedState.BorderRadius = 32;
            this.btnDetail.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnDetail.OnPressedState.BorderThickness = 1;
            this.btnDetail.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnDetail.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnDetail.OnPressedState.IconLeftImage = null;
            this.btnDetail.OnPressedState.IconRightImage = null;
            this.btnDetail.Size = new System.Drawing.Size(92, 36);
            this.btnDetail.TabIndex = 0;
            this.btnDetail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDetail.TextMarginLeft = 0;
            this.btnDetail.UseDefaultRadiusAndThickness = true;
            this.btnDetail.Visible = false;
            // 
            // lblModelo
            // 
            this.lblModelo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelo.ForeColor = System.Drawing.Color.White;
            this.lblModelo.Location = new System.Drawing.Point(16, 246);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblModelo.Size = new System.Drawing.Size(327, 27);
            this.lblModelo.TabIndex = 2;
            this.lblModelo.Text = "lblModelo";
            // 
            // lblFechaHora
            // 
            this.lblFechaHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaHora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.lblFechaHora.Location = new System.Drawing.Point(16, 270);
            this.lblFechaHora.Name = "lblFechaHora";
            this.lblFechaHora.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblFechaHora.Size = new System.Drawing.Size(325, 22);
            this.lblFechaHora.TabIndex = 3;
            this.lblFechaHora.Text = "lblFechaHora";
            // 
            // lblPais
            // 
            this.lblPais.BackColor = System.Drawing.Color.Transparent;
            this.lblPais.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.lblPais.Location = new System.Drawing.Point(57, 36);
            this.lblPais.Name = "lblPais";
            this.lblPais.Size = new System.Drawing.Size(281, 16);
            this.lblPais.TabIndex = 9;
            this.lblPais.Text = "Device Name";
            // 
            // lblAcion
            // 
            this.lblAcion.ForeColor = System.Drawing.Color.White;
            this.lblAcion.Location = new System.Drawing.Point(57, 18);
            this.lblAcion.Name = "lblAcion";
            this.lblAcion.Size = new System.Drawing.Size(279, 16);
            this.lblAcion.TabIndex = 8;
            this.lblAcion.Text = "Device Name";
            // 
            // IconElement
            // 
            this.IconElement.Location = new System.Drawing.Point(16, 19);
            this.IconElement.Margin = new System.Windows.Forms.Padding(0);
            this.IconElement.Name = "IconElement";
            this.IconElement.Size = new System.Drawing.Size(32, 32);
            this.IconElement.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.IconElement.TabIndex = 7;
            this.IconElement.TabStop = false;
            // 
            // imgCamara
            // 
            this.imgCamara.Location = new System.Drawing.Point(16, 70);
            this.imgCamara.Name = "imgCamara";
            this.imgCamara.Size = new System.Drawing.Size(319, 170);
            this.imgCamara.TabIndex = 10;
            this.imgCamara.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(16, 286);
            this.lblMessage.MaximumSize = new System.Drawing.Size(340, 80);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMessage.Size = new System.Drawing.Size(325, 62);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "lblMessage";
            // 
            // ItemCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.imgCamara);
            this.Controls.Add(this.lblPais);
            this.Controls.Add(this.lblAcion);
            this.Controls.Add(this.IconElement);
            this.Controls.Add(this.lblFechaHora);
            this.Controls.Add(this.lblModelo);
            this.Controls.Add(this.btnDiagnosticar);
            this.Controls.Add(this.btnDescartar);
            this.Controls.Add(this.btnRetrieve);
            this.Controls.Add(this.btnDetail);
            this.Name = "ItemCard";
            this.Size = new System.Drawing.Size(354, 402);
            ((System.ComponentModel.ISupportInitialize)(this.IconElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgCamara)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnDescartar;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnDiagnosticar;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnRetrieve;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnDetail;
        private Bunifu.Framework.UI.BunifuCustomLabel lblModelo;
        private Bunifu.Framework.UI.BunifuCustomLabel lblFechaHora;
        private System.Windows.Forms.Label lblPais;
        private System.Windows.Forms.Label lblAcion;
        private System.Windows.Forms.PictureBox IconElement;
        private System.Windows.Forms.PictureBox imgCamara;
        private Bunifu.Framework.UI.BunifuCustomLabel lblMessage;
    }
}
