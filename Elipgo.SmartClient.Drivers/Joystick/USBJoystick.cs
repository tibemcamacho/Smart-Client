using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Drivers.Joystick;
using SlimDX.DirectInput;
using Splat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers
{
    public delegate void ButtonPressedSettinghEventHandler(List<ActionCommand> pressedButton);

    public class USBJoystick : IDriverJoystick, IDisposable
    {
        public event CommandPtzEventHandler CommandPtzEvent;
        public event ButtonPressedEventHandler JoystickButtonEvent;
        public event ButtonPressedSettinghEventHandler JoystickButtonSettingEvent;
        public event JoystickSettingHandler JoystickSetting;
        public event MappingActionsListViewHandler MappingActionsListView;
        public event ContainerSelectedHandler ContainerSelectedEvent;
        public event DriverChangeHandler DriverFactoryChange;

        public bool ConfigMode { get; set; }
        public List<ActionCommand> ActionMappingsListModel { get; set; } = new List<ActionCommand>();

        private SlimDX.DirectInput.Joystick joystickDevice;
        private WaitHandle joystickEventWaitHandle = null;
        private JoystickConfiguration _configuration;

        private Hashtable actionCommandsBuffer;
        private List<ActionCommand> asyncComands;
        private List<ActionCommand> joystickActionCommands;
        private readonly object _lockObjPtzUserControl = new object();
        private ActionCommand lastXYAxisActionCommand = null;
        private ActionCommand lastZAxisActionCommand = null;
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();

        private static float DEFAULT_SAMPLING_FREQUENCE = 5;
        private const int MAX_BUTTONS = 20;
        private const double THRESHOLD = 0.1;

        private const string ZOOM_IN = "Zoom In";
        private const string ZOOM_OUT = "Zoom Out";
        private const string CALL_PRESET = "Preset ";
        private const string CALL_GUARD = "Guard ";
        DateTime? dateWindowsKey = null;

        public USBJoystick()
        {
            ConfigMode = false;
            ActionMappingsListModel = new List<ActionCommand>();
            joystickEventWaitHandle = new AutoResetEvent(false);
            InitializeJoystick();
        }

        private void InitializeJoystick()
        {
            DirectInput dinput = new DirectInput();
            foreach (DeviceInstance device in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    joystickDevice = new SlimDX.DirectInput.Joystick(dinput, device.InstanceGuid);
                    joystickDevice.SetNotification(joystickEventWaitHandle);
                    if (Configuration == null)
                    {
                        Configuration = new JoystickConfiguration();
                        Configuration.Load();
                        actionCommandsBuffer = new Hashtable();
                        asyncComands = new List<ActionCommand>();
                        joystickActionCommands = new List<ActionCommand>();
                    }

                    return;
                }
                catch (DirectInputException)
                {
                }
            }
        }

        public JoystickConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        public bool StartJoystick(float samplingFrequence = 0)
        {
            /// Si el joystick no se pudo inicializar antes,
            /// lo intento inicializar ahora.
            if (joystickDevice == null)
            {
                InitializeJoystick();
                if (joystickDevice == null)
                {
                    notification.Show(Common.Properties.Resources.NoJoystickConnected, null);
                    return false;
                }
            }
            joystickDevice.Acquire();
            return true;
        }

        public void StatePollingTimerTick()
        {
            try
            {
                lock (_lockObjPtzUserControl)
                {
                    if (ConfigMode == true)
                    {// si esta en modo configuracion entonces llamo a otro evento para no con
                        ReadConfigButtons();
                    }
                    else
                    {
                        ReadAxis();
                        ReadButtons();
                    }
                }
            }
            catch
            {
                //LoggerClient.Logger.write(this.GetType().FullName, LoggerEntryType.Error, LoggerCategory.Normal, "Error reading usb joystick state: " + e.Message);
                // this.stop();
            }
        }

        public List<string> GetCustomConfig()
        {
            var list = Configuration.GetCustomConfig();
            list.Insert(0, "Nuevo");

            return list;
        }

        public string GetCurrentJoystickConf()
        {
            return Configuration.GetCurrentJoystickConf();

        }

        public void LoadConfiguration(string CurrentConfiguration)
        {
            Configuration.Load(CurrentConfiguration);
            ActionMappingsListModel.Clear();
            ReadConfiguration();
        }

        private void ReadConfiguration()
        {
            bool invertXAxis = bool.Parse(Configuration.GetJoystickSetting(GlobalJoystickSetting.InvertXAxis));
            bool invertYAxis = bool.Parse(Configuration.GetJoystickSetting(GlobalJoystickSetting.InvertYAxis));
            bool invertZAxisRotation = bool.Parse(Configuration.GetJoystickSetting(GlobalJoystickSetting.InvertZAxisRotation));

            JoystickSetting(invertXAxis, invertYAxis, invertZAxisRotation);

            ActionCommand ac;

            #region Zoom Configuration
            //Zoom Out
            ac = Configuration.GetActionCommand(PtzCommand.ZOOM_ADD_CONTROL, true);
            if (ac == null)
                //No esta configurado => creo uno vacio y lo agrego
                ActionMappingsListModel.Add(new ActionCommand(PtzCommand.ZOOM_ADD_CONTROL, 0.5));
            else
                //Esta configurado => lo agrego
                ActionMappingsListModel.Add(ac);

            //Zoom In
            ac = Configuration.GetActionCommand(PtzCommand.ZOOM_DEC_CONTROL, true);
            if (ac == null)
                ActionMappingsListModel.Add(new ActionCommand(PtzCommand.ZOOM_DEC_CONTROL, -0.5));
            else
                ActionMappingsListModel.Add(ac);
            #endregion
            
            #region Presets Configuration
            for (int i = 1; i <= JoystickConfiguration.MAX_PRESETS; i++)
            {
                ac = Configuration.GetActionCommand(PtzCommand.CallPreset, i, true);
                if (ac == null)
                    //No esta configurado => creo uno vacio y lo agrego
                    ActionMappingsListModel.Add(new ActionCommand(PtzCommand.CallPreset, i));
                else
                    //Esta configurado => lo agrego
                    ActionMappingsListModel.Add(ac);
            }
            #endregion

            #region Guard Configuration
            for (int i = 1; i <= JoystickConfiguration.MAX_GUARDS; i++)
            {
                ac = Configuration.GetActionCommand(PtzCommand.CallGuard, i, true);
                if (ac == null)
                    //No esta configurado => creo uno vacio y lo agrego
                    ActionMappingsListModel.Add(new ActionCommand(PtzCommand.CallGuard, i));
                else
                    //Esta configurado => lo agrego
                    ActionMappingsListModel.Add(ac);
            }
            #endregion

            UpdateListView();
        }

        public void UpdateListView()
        {
            List<ListViewItem> mappingActionsListView = new List<ListViewItem>();
            foreach (object o in ActionMappingsListModel)
            {
                ActionCommand ac = (ActionCommand)o;
                ListViewItem item = new ListViewItem(GetFriendlyName(ac));
                if (ac.buttonOrAxis != ButtonOrAxis.None)
                    item.SubItems.Add(ac.buttonOrAxis.ToString());

                mappingActionsListView.Add(item);
            }
            MappingActionsListView(mappingActionsListView.ToArray());
        }

        private string GetFriendlyName(ActionCommand actionCommand)
        {
            switch (actionCommand.command)
            {
                case PtzCommand.ZOOM_ADD_CONTROL:
                    return ZOOM_IN;
                case PtzCommand.ZOOM_DEC_CONTROL:
                    return ZOOM_OUT;
                case PtzCommand.CallPreset:
                    return CALL_PRESET + actionCommand.Parameter;
                case PtzCommand.CallGuard:
                    return CALL_GUARD + actionCommand.Parameter;
                default:
                    return string.Empty;
            }
        }

        public void SetJoystickSetting(GlobalJoystickSetting setting, string settingValue)
        {
            Configuration.SetJoystickSetting(setting, settingValue);
        }

        //private void pollJoystickState(object o)
        //{
        //    try
        //    {
        //        lock (_lockObjPtzUserControl)
        //        {
        //            if (configMode == true)
        //            {// si esta en modo configuracion entonces llamo a otro evento para no con
        //                readConfigButtons();
        //            }
        //            else
        //            {
        //                readAxes();
        //                readButtons();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        //LoggerClient.Logger.write(this.GetType().FullName, LoggerEntryType.Error, LoggerCategory.Normal, "Error reading usb joystick state: " + e.Message);
        //        // this.stop();
        //    }
        //    finally
        //    {
        //        System.Threading.Monitor.Exit(_lockObjPtzUserControl);
        //    }
        //}

        private void ReadAxis()
        {
            joystickActionCommands.Clear();
            JoystickState jss = joystickDevice.GetCurrentState();
            double yAxis = Stabilize(((double)jss.Y - 32768) / 32768, THRESHOLD);
            double xAxis = Stabilize(((double)jss.X - 32768) / 32768, THRESHOLD);
            if (xAxis != 0d || yAxis != 0d)
            {
                if (bool.Parse(Configuration.GetJoystickSetting(GlobalJoystickSetting.InvertXAxis)))
                    xAxis *= -1;
                if (bool.Parse(Configuration.GetJoystickSetting(GlobalJoystickSetting.InvertYAxis)))
                    yAxis *= -1;
                ActionCommand ac = new ActionCommand(xAxis, yAxis);
                this.lastXYAxisActionCommand = ac;
                joystickActionCommands.Add(ac);
            }
            else if (this.lastXYAxisActionCommand != null)
            {// to joystick stop we must send lastest command with invoke in false 
                lastXYAxisActionCommand.isInvoked = false;
                joystickActionCommands.Add(lastXYAxisActionCommand.clone());
                lastXYAxisActionCommand = null;
            }

            #region ZAxis
            double zAxis = Stabilize(((double)jss.Z - 32768) / 32768, THRESHOLD);
            if (zAxis != 0d)
            {
                if (bool.Parse(Configuration.GetJoystickSetting(GlobalJoystickSetting.InvertZAxisRotation)))
                    zAxis *= -1;
                ActionCommand commandAxisZ = new ActionCommand(zAxis);
                lastZAxisActionCommand = commandAxisZ;
                joystickActionCommands.Add(commandAxisZ);
            }
            else if (lastZAxisActionCommand != null)
            {
                lastZAxisActionCommand.isInvoked = false;
                joystickActionCommands.Add(lastZAxisActionCommand.clone());
                lastZAxisActionCommand = null;
            }

            #endregion

            #region ZAxisRotation
            if (jss.RotationZ != 0)
            {
                ActionCommand command = Configuration.GetActionCommand(ButtonOrAxis.ZAxisRotation);
                if (command != null)
                {
                    double zAxisRotation = Stabilize(((double)jss.RotationZ - 32768) / 32768, 4 * THRESHOLD);
                    if (zAxisRotation != 0d)
                    {
                        if (bool.Parse(Configuration.GetJoystickSetting(GlobalJoystickSetting.InvertZAxisRotation)))
                            zAxisRotation *= -1;
                        command.Parameter = zAxisRotation;
                        command.isInvoked = zAxisRotation != 0d;

                        lock (actionCommandsBuffer.SyncRoot)
                            actionCommandsBuffer[command.command] = command;
                    }

                }
            }
            #endregion
            
            if (joystickActionCommands != null && joystickActionCommands.Count > 0)
            {
                CommandPtzEvent(joystickActionCommands);
            }
        }

        public void StopJoystick()
        {
            if (joystickDevice != null)
            {
                joystickDevice.Unacquire();
                joystickDevice = null;
            }
        }

        private double Stabilize(double d, double threshold)
        {
            return Math.Abs(d) > threshold ? d : 0d;
        }

        private void ReadButtons()
        {
            asyncComands.Clear();
            //TODO: checkear si se desconecto el joystick => stop
            JoystickState jss = joystickDevice.GetCurrentState();
            /// Lectura de los botones
            var buttons = jss.GetButtons().Select((value, index) => new { Value = value, Index = index }).Where(x => x.Value == true).Select(x => x.Index).ToList();
            if (buttons.Count > 0)
            {
                foreach (var x in buttons)
                {
                    if (x == 15)
                    {
                        if (dateWindowsKey == null)
                            dateWindowsKey = DateTime.Now;

                        TimeSpan dp = DateTime.Now.Subtract(Convert.ToDateTime(dateWindowsKey));
                        if (dp.TotalSeconds > 4.1)
                        {
                            this.Dispose();
                            dateWindowsKey = null;
                            return;
                        }
                    }

                    ActionCommand actionCommand = Configuration.GetActionCommand("Button" + x);
                    if (actionCommand != null)
                    {
                        ActionCommand stopCommand = actionCommand.clone();
                        actionCommand.isInvoked = true;
                        stopCommand.isInvoked = false;
                        asyncComands.Add(actionCommand);
                        asyncComands.Add(stopCommand);
                    }
                }

                if (JoystickButtonEvent != null && asyncComands.Count() > 0)
                {
                    JoystickButtonEvent(asyncComands);
                }
            }

        }

        private void ReadConfigButtons()
        {
            asyncComands.Clear();
            //TODO: checkear si se desconecto el joystick => stop
            JoystickState jss = joystickDevice.GetCurrentState();
            /// Lectura de los botones
            var buttons = jss.GetButtons().Select((value, index) => new { Value = value, Index = index }).Where(x => x.Value == true).Select(x => x.Index).ToList();
            if (buttons.Count > 0)
            {
                foreach (var x in buttons)
                {
                    ActionCommand actionCommand = new ActionCommand();
                    actionCommand.buttonOrAxis = (ButtonOrAxis)Enum.Parse(typeof(ButtonOrAxis), "Button" + x, true);
                    actionCommand.Parameter = x;
                    asyncComands.Add(actionCommand);
                    Console.Write(asyncComands);
                }

                if (JoystickButtonSettingEvent != null && asyncComands.Count() > 0)
                {
                    JoystickButtonSettingEvent(asyncComands);
                }
            }
        }

        public void SetActionCommand(ButtonOrAxis buttonOrAxis, ActionCommand actionCommand)
        {
            Configuration.SetActionCommand(buttonOrAxis, actionCommand);
        }

        public void Save(string name)
        {
            Configuration.Save(name);
        }

        public void Dispose()
        {
            StopJoystick();
        }
    }
}
