using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers.Joystick
{

    public delegate void JoystickSettingHandler(bool invertXChecked, bool invertYChecked, bool invertZRotationChecked);
    public delegate void MappingActionsListViewHandler(ListViewItem[] mappingActionsListView);
    public delegate void ContainerSelectedHandler(string keyPress);
    public delegate void DriverChangeHandler();

    public interface IDriverJoystick
    {
        bool ConfigMode { get; set; }
        List<ActionCommand> ActionMappingsListModel { get; set; }
        event JoystickSettingHandler JoystickSetting;
        event MappingActionsListViewHandler MappingActionsListView;
        event ButtonPressedSettinghEventHandler JoystickButtonSettingEvent;
        event ContainerSelectedHandler ContainerSelectedEvent;
        event DriverChangeHandler DriverFactoryChange;
        event ButtonPressedEventHandler JoystickButtonEvent;
        event CommandPtzEventHandler CommandPtzEvent;

        bool StartJoystick(float samplingFrequence = 0);
        List<string> GetCustomConfig();
        string GetCurrentJoystickConf();
        void LoadConfiguration(string CurrentConfiguration);
        void SetJoystickSetting(GlobalJoystickSetting setting, string settingValue);
        void UpdateListView();
        void SetActionCommand(ButtonOrAxis buttonOrAxis, ActionCommand actionCommand);
        void Save(string name);
        void Dispose();
        void StopJoystick();
        void StatePollingTimerTick();
    }
}
