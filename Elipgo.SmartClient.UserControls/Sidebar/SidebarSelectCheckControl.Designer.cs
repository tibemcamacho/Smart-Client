namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarSelectCheckControl
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
            this.fBtnViewList = new System.Windows.Forms.Panel();
            this.itemCheckList1 = new Elipgo.SmartClient.UserControls.Sidebar.Element.ItemCheckList();
            //this.bddParent = new Bunifu.UI.WinForms.BunifuDropdown();
            this.ucbddParent = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();
            this.SuspendLayout();
            // 
            // fBtnViewList
            // 
            this.fBtnViewList.BackColor = System.Drawing.Color.Transparent;
            this.fBtnViewList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fBtnViewList.Location = new System.Drawing.Point(267, 8);
            this.fBtnViewList.Name = "fBtnViewList";
            this.fBtnViewList.Size = new System.Drawing.Size(24, 24);
            this.fBtnViewList.TabIndex = 12;
            // 
            // itemCheckList1
            // 
            this.itemCheckList1.BackColor = System.Drawing.Color.Transparent;
            this.itemCheckList1.Location = new System.Drawing.Point(228, 8);
            this.itemCheckList1.Name = "itemCheckList1";
            this.itemCheckList1.Size = new System.Drawing.Size(25, 25);
            this.itemCheckList1.TabIndex = 13;
            // 
            // bddParent
            // 
            this.ucbddParent.BackColor = System.Drawing.Color.Transparent;
            this.ucbddParent.ForeColor = System.Drawing.Color.White;
            this.ucbddParent.Location = new System.Drawing.Point(24, 4);
            this.ucbddParent.Name = "bddParent";
            this.ucbddParent.Size = new System.Drawing.Size(187, 32);
            this.ucbddParent.TabIndex = 14;
            this.ucbddParent.SelectedIndexChanged += new System.EventHandler(this.BddParent_SelectedIndexChanged);

            //this.bddParent.BackColor = System.Drawing.Color.Transparent;
            //this.bddParent.BorderRadius = 1;
            //this.bddParent.Color = System.Drawing.Color.Transparent;
            //this.bddParent.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.bddParent.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            //this.bddParent.DisabledColor = System.Drawing.Color.Gray;
            //this.bddParent.DisplayMember = "Name";
            //this.bddParent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //this.bddParent.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            //this.bddParent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.bddParent.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            //this.bddParent.FillDropDown = false;
            //this.bddParent.FillIndicator = false;
            //this.bddParent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.bddParent.ForeColor = System.Drawing.Color.White;
            //this.bddParent.FormattingEnabled = true;
            //this.bddParent.Icon = null;
            //this.bddParent.IndicatorColor = System.Drawing.Color.White;
            //this.bddParent.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            //this.bddParent.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.bddParent.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.bddParent.ItemForeColor = System.Drawing.Color.White;
            //this.bddParent.ItemHeight = 26;
            //this.bddParent.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            //this.bddParent.Location = new System.Drawing.Point(24, 4);
            //this.bddParent.Margin = new System.Windows.Forms.Padding(0);
            //this.bddParent.Name = "bddParent";
            //this.bddParent.Size = new System.Drawing.Size(187, 32);
            //this.bddParent.TabIndex = 14;
            //this.bddParent.Text = null;
            //this.bddParent.ValueMember = "Key";
            // 
            // SidebarSelectCheckControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ucbddParent);
            this.Controls.Add(this.itemCheckList1);
            this.Controls.Add(this.fBtnViewList);
            this.Name = "SidebarSelectCheckControl";
            this.Size = new System.Drawing.Size(333, 40);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel fBtnViewList;
        private Element.ItemCheckList itemCheckList1;
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown ucbddParent;
    }
}
