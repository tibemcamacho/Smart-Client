using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Elipgo.SmartClient.Drivers.Joystick
{
    public class COMJoystick : IDriverJoystick, IDisposable
    {
        public bool ConfigMode { get; set; }
        private readonly object _dataReceived = new object();
        public List<ActionCommand> ActionMappingsListModel { get; set; }
        public event CommandPtzEventHandler CommandPtzEvent;
        public event ButtonPressedEventHandler JoystickButtonEvent;
        public event JoystickSettingHandler JoystickSetting;
        public event MappingActionsListViewHandler MappingActionsListView;
        public event ButtonPressedSettinghEventHandler JoystickButtonSettingEvent;
        public event ContainerSelectedHandler ContainerSelectedEvent;
        public event DriverChangeHandler DriverFactoryChange;

        public SerialPort serialPort;
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private string currentConfiguration;
        private DispatcherTimer timerCheck = null;
        private DateTime? dtDateEndBuffer = null;
        private string keyPress = string.Empty;
        public COMJoystick()
        {
            timerCheck = new DispatcherTimer();
            timerCheck.Tick += TimerCheck_Tick;
            timerCheck.Interval = TimeSpan.FromMilliseconds(5000);
            timerCheck.Start();
        }

        private void TimerCheck_Tick(object sender, EventArgs e)
        {
            lock (_dataReceived)
            {
                try
                {
                    if (dtDateEndBuffer != null)
                    {
                        TimeSpan dp = DateTime.Now.Subtract(Convert.ToDateTime(dtDateEndBuffer));
                        if (dp.TotalSeconds > 5)
                        {
                            dtDateEndBuffer = null;
                            this.Dispose();
                            DriverFactoryChange();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log("ex.Message --> " + ex.Message + " ex.StackTrace -->" + ex.StackTrace, LogPriority.Warning);
                }
            }
        }

        public List<string> GetCustomConfig()
        {
            return SerialPort.GetPortNames().OrderBy(x => x).ToList();
        }

        public void LoadConfiguration(string CurrentConfiguration)
        {
            currentConfiguration = CurrentConfiguration;
        }

        public string GetCurrentJoystickConf()
        {
            return currentConfiguration;
        }

        public void Save(string name)
        {
            try
            {
                serialPort = new SerialPort(name, 9600);
                if (!serialPort.IsOpen)
                {
                    serialPort.DtrEnable = true;
                    serialPort.BaudRate = 9600;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.DataBits = 8;
                    serialPort.Handshake = Handshake.None;
                    serialPort.ReceivedBytesThreshold = 1;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                    serialPort.ErrorReceived += serialPort_ErrorReceived;
                    serialPort.Open();
                    notification.Show(Common.Properties.Resources.JoystickConnected, null);
                }

            }
            catch (IOException)
            {
                notification.Show(Common.Properties.Resources.NoJoystickConnected, null);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogPriority.Warning);
                notification.Show(ex.Message, null);
            }
        }

        public void SetActionCommand(ButtonOrAxis buttonOrAxis, ActionCommand actionCommand)
        {
            throw new NotImplementedException();
        }

        public void SetJoystickSetting(GlobalJoystickSetting setting, string settingValue)
        {
            throw new NotImplementedException();
        }

        public bool StartJoystick(float samplingFrequence = 0)
        {
            return true;
        }

        public void UpdateListView()
        {
            throw new NotImplementedException();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            if (serialPort.IsOpen)
            {
                lock (_dataReceived)
                {
                    dtDateEndBuffer = DateTime.Now;
                    string cadenaTemporal = string.Empty;
                    try
                    {
                        SerialPort sp = (SerialPort)sender;
                        string indata = sp.ReadExisting();

                        byte[] bytes = Encoding.Default.GetBytes(indata);
                        string hexString = BitConverter.ToString(bytes);
                        hexString = hexString.Replace("-", "");
                        string numberHex = string.Empty;
                        if (hexString != Common.Enum.Joystick.GetEnumMemberAttrValue(JoystickHikvision.Signal) && hexString.Length >= 50)
                        {
                            switch (hexString.Substring(0, 8))
                            {
                                case "3F3F0049":
                                    numberHex = hexString.Substring(64, 8);
                                    break;
                                case "3F3F0019":
                                    if (hexString.Length > 114)
                                        numberHex = hexString.Substring(114, 8);
                                    break;
                                default:
                                    break;
                            }

                            if (Convert.ToInt32(numberHex, 16).ToString() == Common.Enum.Joystick.GetEnumMemberAttrValue(ButtonJoystickHikvision.ButtonF))
                            {
                                switch (hexString.Substring(0, 8))
                                {
                                    case "3F3F0049":
                                        keyPress = hexString.Substring(71, 3);
                                        break;
                                    case "3F3F0019":
                                        keyPress = hexString.Substring(121, 3);
                                        break;
                                    default:
                                        break;
                                }
                                keyPress = Common.Enum.Joystick.GetEnumMemberAttrValue((ButtonJoystickHikvision)Enum.Parse(typeof(ButtonJoystickHikvision), keyPress, true));
                            }
                            else
                            {
                                keyPress = (string.IsNullOrEmpty(keyPress) ? string.Empty : keyPress + "+") + Convert.ToInt32(numberHex, 16).ToString();
                                ContainerSelectedEvent(keyPress);
                                keyPress = string.Empty;
                            }
                        }
                        Console.WriteLine("<------------------------>");
                    }
                    catch (Exception ex)
                    {
                        keyPress = String.Empty;
                        //Console.WriteLine("ex.StackTrace ------------------------>" + cadenaTemporal + "  ----> " + ex.StackTrace);
                        Logger.Log("Event Type: " + ex.StackTrace, LogPriority.Warning);
                    }
                }
            }
        }

        private void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Logger.Log("Serial Port error Rcvd event fired.", LogPriority.Warning);
            string type = "";
            switch (e.EventType)
            {
                case SerialError.Frame:
                    type = "Frame";
                    break;
                case SerialError.Overrun:
                    type = "Overrun";
                    break;
                case SerialError.RXOver:
                    type = "RXOver";
                    break;
                case SerialError.RXParity:
                    type = "RXParity";
                    break;
                case SerialError.TXFull:
                    type = "TXFull";
                    break;
            }
            Logger.Log("Event Type: " + type, LogPriority.Warning);
        }

        public void Dispose()
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                    serialPort.Close();
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);
                serialPort.ErrorReceived -= serialPort_ErrorReceived;
                serialPort.Dispose();
            }

            timerCheck.Stop();
            timerCheck.Tick -= TimerCheck_Tick;
        }

        public void StopJoystick()
        {

        }

        public void StatePollingTimerTick()
        {
        }
    }
}
