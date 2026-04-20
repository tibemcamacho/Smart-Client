using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers
{

    public partial class SequencingUserControl : UserControl
    {
        public event EventHandler<int> ButtonMouseUp;

        private SequencingDTO _sequencing;

        public SequencingUserControl(SequencingDTO sequencing)
        {
            InitializeComponent();
            this._sequencing = sequencing;
            ButtonRight.Visible = ButtonUp.Visible = ButtonDown.Visible = ButtonLeft.Visible = false;
            if (sequencing.dvfIdUp != 0)
            {
                ButtonUp.MouseUp += ButtonUp_MouseUp;
                ButtonUp.Image = FileResources.ptz_arrow_up;
                ButtonUp.Visible = true;
            }

            if (sequencing.dvfIdDown != 0)
            {
                ButtonDown.MouseUp += ButtonDown_MouseUp;
                ButtonDown.Image = FileResources.ptz_arrow_down;
                ButtonDown.Visible = true;
            }

            if (sequencing.dvfIdLeft != 0)
            {
                ButtonLeft.MouseUp += ButtonLeft_MouseUp;
                ButtonLeft.Image = FileResources.ptz_arrow_left;
                ButtonLeft.Visible = true;
            }

            if (sequencing.dvfIdRight != 0)
            {
                ButtonRight.MouseUp += ButtonRight_MouseUp;
                ButtonRight.Image = FileResources.ptz_arrow_right;
                ButtonRight.Visible = true;
            }
        }

        private void ButtonRight_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, _sequencing.dvfIdRight);
        }

        private void ButtonLeft_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, _sequencing.dvfIdLeft);
        }

        private void ButtonDown_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, _sequencing.dvfIdDown);
        }

        private void ButtonUp_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, _sequencing.dvfIdUp);
        }

    }
}
