using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers.Joystick;
using Splat;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers
{
    public delegate void PtzCommandEventHandler(List<ActionCommand> actionCommands);

    public partial class PtzUserControl : UserControl
    {
        public event EventHandler<PtzMovement> ButtonMouseDown;
        public event EventHandler<PtzMovement> ButtonMouseUp;
        public event PtzCommandEventHandler PtzJoystickStateEvent;
        public event ButtonPressedEventHandler PtzJoystickButtonEvent;
        //USBJoystick joystick;
        private readonly IDriverFactoryJoystick driverFactoryJoystick = Locator.Current.GetService<IDriverFactoryJoystick>();
        //IDriverJoystick joystick;
        public PtzUserControl()
        {
            InitializeComponent();
            //joystick = driverFactoryJoystick.GetDriverJoystinck();
            ButtonUp.MouseDown += ButtonUp_MouseDown;
            ButtonDown.MouseDown += ButtonDown_MouseDown;
            ButtonLeft.MouseDown += ButtonLeft_MouseDown;
            ButtonRight.MouseDown += ButtonRight_MouseDown;
            ButtonCenter.MouseDown += ButtonCenter_MouseDown;
            ButtonUpLeft.MouseDown += ButtonUpLeft_MouseDown;
            ButtonDownLeft.MouseDown += ButtonDownLeft_MouseDown;
            ButtonUpRight.MouseDown += ButtonUpRight_MouseDown;
            ButtonDownRight.MouseDown += ButtonDownRight_MouseDown;

            ButtonUp.MouseUp += ButtonUp_MouseUp;
            ButtonDown.MouseUp += ButtonDown_MouseUp;
            ButtonLeft.MouseUp += ButtonLeft_MouseUp;
            ButtonRight.MouseUp += ButtonRight_MouseUp;
            ButtonCenter.MouseUp += ButtonCenter_MouseUp;
            ButtonUpLeft.MouseUp += ButtonUpLeft_MouseUp;
            ButtonDownLeft.MouseUp += ButtonDownLeft_MouseUp;
            ButtonUpRight.MouseUp += ButtonUpRight_MouseUp;
            ButtonDownRight.MouseUp += ButtonDownRight_MouseUp;

            ButtonUp.Image = FileResources.ptz_arrow_up;
            ButtonDown.Image = FileResources.ptz_arrow_down;
            ButtonLeft.Image = FileResources.ptz_arrow_left;
            ButtonRight.Image = FileResources.ptz_arrow_right;
            ButtonCenter.Image = FileResources.ptz_center;
            ButtonUpLeft.Image = FileResources.ptz_arrow_up_left;
            ButtonDownLeft.Image = FileResources.ptz_arrow_down_left;
            ButtonUpRight.Image = FileResources.ptz_arrow_up_right;
            ButtonDownRight.Image = FileResources.ptz_arrow_down_right;

            //joystick
            driverFactoryJoystick.JoystickStatePtzEvent += JoystickEventState;
            driverFactoryJoystick.JoystickButtonEvent += JoystickButtonEvent;
            //joystick.JoystickButtonEvent += JoystickButtonEvent;
            //joystick.joystickStateEvent += JoystickEventState;

            this.Load += PtzUserControl_Load;
        }

        private void PtzUserControl_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }


        private void ButtonDownRight_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.DownRight);
        }

        private void ButtonUpRight_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.UpRight);
        }

        private void ButtonDownLeft_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.DownLeft);
        }

        private void ButtonUpLeft_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.UpLeft);
        }

        private void ButtonCenter_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.Center);
        }

        private void ButtonRight_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.Right);
        }

        private void ButtonLeft_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.Left);
        }

        private void ButtonDown_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.Down);
        }

        private void ButtonUp_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonMouseUp?.Invoke(this, PtzMovement.Up);
        }

        private void ButtonDownRight_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.DownRight);
        }

        private void ButtonUpRight_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.UpRight);
        }

        private void ButtonDownLeft_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.DownLeft);
        }

        private void ButtonUpLeft_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.UpLeft);
        }

        private void ButtonCenter_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.Center);
        }

        private void ButtonRight_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.Right);
        }

        private void ButtonLeft_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.Left);
        }

        private void ButtonDown_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.Down);
        }

        private void ButtonUp_MouseDown(object sender, EventArgs e)
        {
            ButtonMouseDown?.Invoke(this, PtzMovement.Up);
        }

        public void JoystickEventState(List<ActionCommand> actionCommands)
        {
            PtzJoystickStateEvent?.Invoke(actionCommands);
        }

        public void JoystickButtonEvent(List<ActionCommand> pressedButton)
        {
            PtzJoystickButtonEvent?.Invoke(pressedButton);
        }

        public void StartJoystick()
        {
            driverFactoryJoystick.StartJoystick();
        }

        public void StopJoystick()
        {
            //driverFactoryJoystick.Dispose();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                ButtonMouseDown?.Invoke(this, PtzMovement.Up);
                return true;
            }
            if (keyData == Keys.Left)
            {
                ButtonMouseDown?.Invoke(this, PtzMovement.Left);
                return true;
            }
            if (keyData == Keys.Right)
            {
                ButtonMouseDown?.Invoke(this, PtzMovement.Right);
                return true;
            }
            if (keyData == Keys.Down)
            {
                ButtonMouseDown?.Invoke(this, PtzMovement.Down);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    ButtonMouseUp?.Invoke(this, PtzMovement.Up);
                    break;
                case Keys.Left:
                    ButtonMouseUp?.Invoke(this, PtzMovement.Left);
                    break;
                case Keys.Right:
                    ButtonMouseUp?.Invoke(this, PtzMovement.Right);
                    break;
                case Keys.Down:
                    ButtonMouseUp?.Invoke(this, PtzMovement.Down);
                    break;
            }
            base.OnKeyUp(e);
        }

    }
}
