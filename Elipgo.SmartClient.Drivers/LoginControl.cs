using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using System;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Drivers
{

    public interface IConectionNotification
    {
        void Disconect(IntPtr HandledBrocaster);
        void Connect(IntPtr HandledBrocaster);
    }

    public class LoginObject
    {
        private List<int> _channels;
        public LoginObject(IntPtr DeviceLogin, int channel, Common.Enum.Drivers Driver, IConectionNotification camara)
        {
            this.camara = camara;
            this.Driver = Driver;
            this.DeviceLogin = DeviceLogin;
            _channels = new List<int>();
            _channels.Add(channel);
        }


        public List<int> Channels { get => _channels; set => _channels = value; }
        public IntPtr DeviceLogin { get; set; }
        public Common.Enum.Drivers Driver { get; set; }
        public IConectionNotification camara;
        public bool IsDeviceLoginValid()
        {
            return this.DeviceLogin != IntPtr.Zero;
        }
    }
    public interface ILoginControl
    {
        bool RemoveChannelAndCanLogout(CameraDTO cameraDto, IConectionNotification camara, Common.Enum.Drivers driver);
        //bool canLogOut(CameraDTO cameraDto);
        void AddDevice(CameraDTO cameraDto, IntPtr DeviceLogin, IConectionNotification camara, Common.Enum.Drivers Driver);
        void AddChannel(CameraDTO cameraDto, IConectionNotification camara, Common.Enum.Drivers driver);
        bool GetDeviceLogin(CameraDTO cameraDto, out IntPtr login, Common.Enum.Drivers driver, IConectionNotification camara);
        void Disconect(IntPtr DeviceLogin, IntPtr HandledBrocaster);
        void Connect(IntPtr DeviceLogin, IntPtr HandledBrocaster);
    }

    public class LoginControl : ILoginControl
    {

        private Object sync = new object();
        public Dictionary<string, LoginObject> LoginPool = new Dictionary<string, LoginObject>();
        public bool RemoveChannelAndCanLogout(CameraDTO cameraDto, IConectionNotification camara, Common.Enum.Drivers driver)
        {
            if (cameraDto == null)
            {
                Logger.Log("Error RemoveChannel is null");
                return false;
            }
            string ipPort = cameraDto.Host + ":" + cameraDto.VideoPort + ":" + driver + camara.GetHashCode();
            try
            {
                lock (this.sync)
                {
                    LoginObject lo;
                    if (LoginPool.TryGetValue(ipPort, out lo) && lo.Driver == driver && lo.camara == camara)
                    {
                        if (this.listNotification.ContainsKey(lo.DeviceLogin) && lo.Driver == driver)
                        {
                            listNotification[lo.DeviceLogin].Remove(camara);
                            if (listNotification[lo.DeviceLogin].Count == 0 || listNotification.Count == 0)
                            {
                                listNotification.Remove(lo.DeviceLogin);
                            }
                        }
                        lo.Channels.Remove(cameraDto.Channel);
                        if (lo.Channels.Count == 0)
                        {
                            this.LoginPool.Remove(ipPort);
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("RemoveChannel {0} Exception {1}", cameraDto, ex), LogPriority.Fatal);
                return false;
            }
        }

        public void AddDevice(CameraDTO cameraDto, IntPtr DeviceLogin, IConectionNotification camara, Common.Enum.Drivers driver)
        {
            if (cameraDto == null)
            {
                Logger.Log("Error AddDevice is null");
                return;
            }
            string ipPort = cameraDto.Host + ":" + cameraDto.VideoPort + ":" + driver + camara.GetHashCode();
            try
            {
                lock (this.sync)
                {

                    LoginObject lo = null;
                    if (LoginPool.TryGetValue(ipPort, out lo) && lo.Driver == driver && lo.camara == camara)
                    {
                        lo.Channels.Add(cameraDto.Channel);
                    }
                    else
                    { //if not exist then I create a new one
                        LoginPool.Add(ipPort, new LoginObject(DeviceLogin, cameraDto.Channel, driver, camara));
                    }

                    if (this.listNotification.ContainsKey(DeviceLogin))
                    {
                        this.listNotification[DeviceLogin].Add(camara);
                    }
                    else
                    {
                        this.listNotification[DeviceLogin] = new List<IConectionNotification>();
                        this.listNotification[DeviceLogin].Add(camara);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("AddDevice {0} Exception: {1} ", cameraDto, ex), LogPriority.Fatal);
            }
        }
        public void AddChannel(CameraDTO cameraDto, IConectionNotification camara, Common.Enum.Drivers driver)
        {
            if (cameraDto == null)
            {
                Logger.Log("Error AddChannel is null");
                return;
            }
            lock (this.sync)
            {
                string ipPort = cameraDto.Host + ":" + cameraDto.VideoPort + ":" + driver + camara.GetHashCode();
                LoginObject lo;
                if (LoginPool.TryGetValue(ipPort, out lo) && lo.Driver == driver && lo.camara == camara)
                {
                    if (!this.LoginPool[ipPort].Channels.Contains(cameraDto.Channel))
                        this.LoginPool[ipPort].Channels.Add(cameraDto.Channel);

                    if (this.listNotification[lo.DeviceLogin].Contains(camara) == false)
                    {
                        this.listNotification[lo.DeviceLogin].Add(camara);
                    }
                }
            }
        }

        public bool GetDeviceLogin(CameraDTO cameraDto, out IntPtr login, Common.Enum.Drivers Driver, IConectionNotification camara)
        {
            try
            {
                string ipPort = cameraDto.Host + ":" + cameraDto.VideoPort + ":" + Driver + camara.GetHashCode();
                Logger.Log(String.Format("LoginControl try GetDeviceLogin to Host {0}", ipPort), LogPriority.Information);

                if (LoginPool.TryGetValue(ipPort, out LoginObject lo) && lo.Driver == Driver && lo.camara == camara)
                {
                    login = lo.DeviceLogin;
                    return true;
                }
                else
                {
                    login = IntPtr.Zero;
                    return false;
                }
            }
            catch (Exception)
            {
                login = IntPtr.Zero;
                return false;
            }
        }

        private Dictionary<IntPtr, List<IConectionNotification>> listNotification = new Dictionary<IntPtr, List<IConectionNotification>>();

        public void Disconect(IntPtr DeviceLogin, IntPtr HandledBrocaster)
        {
            try
            {
                if (listNotification.ContainsKey(DeviceLogin))
                {
                    var list = listNotification[DeviceLogin];
                    foreach (var it in list)
                    {
                        it.Disconect(HandledBrocaster);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("LoginControl Disconect Exception" + ex.Message, LogPriority.Fatal);
            }
        }

        public void Connect(IntPtr DeviceLogin, IntPtr HandledBrocaster)
        {
            try
            {
                if (listNotification.ContainsKey(DeviceLogin))
                {
                    var list = listNotification[DeviceLogin];
                    foreach (var it in list)
                    {
                        it.Connect(HandledBrocaster);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("LoginControl Connect Exception" + ex.Message, LogPriority.Fatal);

            }
        }
    }
}
