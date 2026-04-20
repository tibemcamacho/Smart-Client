using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Drivers.Joystick
{
    public delegate void CommandPtzEventHandler(List<ActionCommand> actionCommands);
    public interface IDriverFactoryJoystick
    {
        event CommandPtzEventHandler JoystickStatePtzEvent;
        event ButtonPressedEventHandler JoystickButtonEvent;
        IDriverJoystick GetDriverJoystinck();
        void Dispose();
        bool StartJoystick();
    }
}
