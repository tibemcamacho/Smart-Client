using Elipgo.SmartClient.Common.Properties;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Spinner
{
    public partial class SpinnerThreePoint : UserControl
    {
        private PictureBox _spinner { get; set; }
        public SpinnerThreePoint(Control container)
        {
            InitializeComponent();

            InitializeSpinner(container);
        }

        private void InitializeSpinner(Control control)
        {
            //this
            this.Size = control.Size;
            this.Location = control.Location;

            // spinner
            // 
            this._spinner = new PictureBox();
            this._spinner.Image = FileResources.spinner;
            this._spinner.InitialImage = null;
            //this._spinner.Location = control.Location;
            //this._spinner.Margin = new System.Windows.Forms.Padding(2);
            this._spinner.Name = "spinner";
            this._spinner.Size = control.Size;
            this._spinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._spinner.TabIndex = 0;
            this._spinner.TabStop = false;

            this.Controls.Add(this._spinner);
        }

        public new void Show()
        {
            this.Visible = true;
            this._spinner.Show();
        }

        public new void Hide()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Hide();
                });
                return;
            }
            this.Visible = false;
            this._spinner.Hide();
        }
    }
}
