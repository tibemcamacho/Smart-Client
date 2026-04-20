using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace Elipgo.SmartClient.Drivers.Joystick
{
    public class DriverFactoryJoystinck : IDriverFactoryJoystick
    {
        IDriverJoystick driver;
        DispatcherTimer statePollingTimer = new DispatcherTimer();
        private static float DEFAULT_SAMPLING_FREQUENCE = 5;
        public DriverFactoryJoystinck()
        {
            float samplingFrequence = DEFAULT_SAMPLING_FREQUENCE;
            statePollingTimer.Interval = TimeSpan.FromMilliseconds((1000 / samplingFrequence));
            statePollingTimer.Tick += StatePollingTimer_Tick;
        }

        public event CommandPtzEventHandler JoystickStatePtzEvent;
        public event ButtonPressedEventHandler JoystickButtonEvent;

        public IDriverJoystick GetDriverJoystinck()
        {

            DirectInput dinput = new DirectInput();

            if (dinput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices).Count() > 0)
            {
                if (dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).Count() > 0)
                {
                    if (!(driver is USBJoystick))
                    {
                        driver = new USBJoystick();
                        driver.CommandPtzEvent += Driver_CommandPtzEvent;
                        driver.JoystickButtonEvent += Driver_JoystickButtonEvent;
                    }
                }
                else
                {
                    if (!(driver is COMJoystick))
                    {
                        if (driver != null)
                        {
                            driver.CommandPtzEvent -= Driver_CommandPtzEvent;
                            driver.JoystickButtonEvent -= Driver_JoystickButtonEvent;
                        }
                        driver = new COMJoystick();
                    }
                }
            }
            return driver;
        }

        private void Driver_JoystickButtonEvent(List<Common.DTOs.ActionCommand> pressedButton)
        {
            JoystickButtonEvent(pressedButton);
        }

        private void Driver_CommandPtzEvent(List<Common.DTOs.ActionCommand> actionCommands)
        {
            JoystickStatePtzEvent(actionCommands);
        }

        public void Dispose()
        {
            if (statePollingTimer.IsEnabled)
            {
                statePollingTimer.IsEnabled = false;
                statePollingTimer.Stop();
                if (driver != null)
                    driver.Dispose();
            }
            if (driver != null)
            {
                driver.CommandPtzEvent -= Driver_CommandPtzEvent;
                driver.JoystickButtonEvent -= Driver_JoystickButtonEvent;
                driver = null;
            }
        }

        public bool StartJoystick()
        {
            if (!statePollingTimer.IsEnabled)
            {
                GetDriverJoystinck();
                if (driver != null)
                {
                    if (driver is USBJoystick)
                    {
                        statePollingTimer.IsEnabled = true;
                        statePollingTimer.Start();
                    }
                    return driver.StartJoystick();
                }

            }
            return true;
        }

        private void StatePollingTimer_Tick(object sender, EventArgs e)
        {
            driver.StatePollingTimerTick();

        }
    }
}
