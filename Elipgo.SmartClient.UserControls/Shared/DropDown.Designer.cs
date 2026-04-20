namespace Elipgo.SmartClient.UserControls.Shared
{
    partial class Dropdown
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dropdown));
            this.ddlTimer = new System.Windows.Forms.Timer(this.components);
            this.Panel_SelectBtn = new System.Windows.Forms.Panel();
            this.SelectButton = new System.Windows.Forms.Button();
            this.DropdownContainer = new System.Windows.Forms.Panel();
            this.Panel_SelectBtn.SuspendLayout();
            this.DropdownContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlTimer
            // 
            this.ddlTimer.Interval = 1;
            this.ddlTimer.Tick += new System.EventHandler(this.ddlTimer_Tick);
            // 
            // Panel_SelectBtn
            // 
            this.Panel_SelectBtn.BackColor = System.Drawing.Color.Transparent;
            this.Panel_SelectBtn.Controls.Add(this.SelectButton);
            this.Panel_SelectBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_SelectBtn.Location = new System.Drawing.Point(0, 0);
            this.Panel_SelectBtn.Margin = new System.Windows.Forms.Padding(0);
            this.Panel_SelectBtn.Name = "Panel_SelectBtn";
            this.Panel_SelectBtn.Size = new System.Drawing.Size(150, 38);
            this.Panel_SelectBtn.TabIndex = 0;
            // 
            // SelectButton
            // 
            this.SelectButton.BackColor = System.Drawing.Color.Transparent;
            this.SelectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SelectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectButton.FlatAppearance.BorderSize = 0;
            this.SelectButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SelectButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectButton.ForeColor = System.Drawing.Color.White;
            this.SelectButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectButton.Image")));
            this.SelectButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SelectButton.Location = new System.Drawing.Point(0, 0);
            this.SelectButton.Margin = new System.Windows.Forms.Padding(0);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(150, 38);
            this.SelectButton.TabIndex = 1;
            this.SelectButton.Text = "SelectButton";
            this.SelectButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectButton.UseVisualStyleBackColor = false;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // DropdownContainer
            // 
            this.DropdownContainer.BackColor = System.Drawing.Color.Transparent;
            this.DropdownContainer.Controls.Add(this.Panel_SelectBtn);
            this.DropdownContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DropdownContainer.Location = new System.Drawing.Point(0, 0);
            this.DropdownContainer.Margin = new System.Windows.Forms.Padding(0);
            this.DropdownContainer.Name = "DropdownContainer";
            this.DropdownContainer.Size = new System.Drawing.Size(150, 189);
            this.DropdownContainer.TabIndex = 1;
            // 
            // Dropdown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.DropdownContainer);
            this.ForeColor = System.Drawing.Color.Snow;
            this.Name = "Dropdown";
            this.Size = new System.Drawing.Size(150, 189);
            this.Panel_SelectBtn.ResumeLayout(false);
            this.DropdownContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer ddlTimer;
        private System.Windows.Forms.Panel Panel_SelectBtn;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Panel DropdownContainer;
    }
}
