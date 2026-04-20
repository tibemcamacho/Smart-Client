namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    partial class ElementLprControl
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
            this._panelPicture = new System.Windows.Forms.Panel();
            this._panelList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // PanelPicture
            // 
            this._panelPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panelPicture.Location = new System.Drawing.Point(0, 0);
            this._panelPicture.Margin = new System.Windows.Forms.Padding(0);
            this._panelPicture.Name = "PanelPicture";
            this._panelPicture.Size = new System.Drawing.Size(290, 213);
            this._panelPicture.TabIndex = 2;
            // 
            // PanelList
            // 
            this._panelList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panelList.Location = new System.Drawing.Point(290, 0);
            this._panelList.Margin = new System.Windows.Forms.Padding(0);
            this._panelList.Name = "PanelList";
            this._panelList.Size = new System.Drawing.Size(117, 213);
            this._panelList.TabIndex = 3;
            // 
            // ElementLprControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panelList);
            this.Controls.Add(this._panelPicture);
            this.Name = "ElementLprControl";
            this.Size = new System.Drawing.Size(407, 213);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel _panelPicture;
        private System.Windows.Forms.FlowLayoutPanel _panelList;
    }
}
