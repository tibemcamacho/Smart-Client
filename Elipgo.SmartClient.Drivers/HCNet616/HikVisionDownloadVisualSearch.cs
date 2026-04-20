using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Splat;
using System;
using System.Configuration;
using System.Timers;

namespace Elipgo.SmartClient.Drivers.HCNet616
{
    public class HikVisionDownloadVisualSearch : IDriverDownloadVisualSearch, IDisposable, IConectionNotification
    {
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private Int32 loginHandle = -1;
        private Int32 fileHandle = -1;
        private readonly int tryLimit;
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }

        public event OnDownloadVisualSearchCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadVisualSearchProgressEventHandler OnDownloadProgress;
        public event OnDownloadVisualSearchErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        private int StartDigitalChannel = 0;
        private System.Timers.Timer timerDownload = new System.Timers.Timer();
        private bool canCheckDownload;

        public HikVisionDownloadVisualSearch(CameraDTO camera)
        {
            Camera = camera;
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            canCheckDownload = true;
        }

        public void Dispose()
        {
            if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.HCNetSDK_616))
            {
                Logger.Log(String.Format(" Dispose Logout Hik {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                CHCNetSDK.NET_DVR_Logout(loginHandle);
            }
            CHCNetSDK.NET_DVR_Cleanup();
        }

        private bool TryLimit(ref Int16 _try, string errMsg)
        {
            bool blimit = false;
            _try++;
            System.Threading.Thread.Sleep(3000);
            if (_try >= tryLimit)
            {
                OnDownloadError?.Invoke(this, errMsg);
                blimit = true;
            }
            return blimit;
        }

        private void IntialiazeDriver()
        {
            bool res = CHCNetSDK.NET_DVR_Init();
            Int16 _try = 0;
            if (res)
            {
                IntPtr ptr;
                if (loginControl.GetDeviceLogin(Camera, out ptr, Common.Enum.Drivers.HCNetSDK_616, this))
                {
                    loginHandle = (Int32)ptr;
                    loginControl.AddChannel(Camera, this, Common.Enum.Drivers.HCNetSDK_616);
                    Logger.Log(String.Format("Reusing a Hikivion Login to Camara {0}", Camera.Name), LogPriority.Information);
                    return;
                }
                while (_try <= (tryLimit + 1))
                {
                    CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                    ushort videoPort = Camera.VideoPort == 0 ? (ushort)8000 : (ushort)Camera.VideoPort;
                    loginHandle = CHCNetSDK.NET_DVR_Login_V30(Camera.Host, videoPort, Camera.User, Camera.Password, ref deviceInfo);
                    if (loginHandle != -1)
                    {
                        this.StartDigitalChannel = deviceInfo.byStartDChan;
                        loginControl.AddDevice(Camera, (IntPtr)loginHandle, this, Common.Enum.Drivers.HCNetSDK_616);
                        Logger.Log(String.Format("New Hikivion Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                        break;
                    }
                    else
                    {
                        if (TryLimit(ref _try, "HikVision try to reconnect"))
                        {
                            Logger.Log(string.Format("IntialiazeDriver Reconnection Failed {0} , excced the maximun number of re try ", Camera.Name), LogPriority.Important);
                            break;
                        }
                    }
                }
            }
        }
        public void Start(string fileName, DateTime startTime, DateTime endTime)
        {
            Int16 _try = 0;
            IntialiazeDriver();
            if (loginHandle == -1)
            {
                OnDownloadError?.Invoke(this, String.Format("Login error {0}", Camera.Name));
                return;
            }

            FileName = fileName + ".tmp";
            StartTime = startTime;
            EndTime = endTime;

            CHCNetSDK.NET_DVR_PLAYCOND struDownPara = new CHCNetSDK.NET_DVR_PLAYCOND();
            struDownPara.dwChannel = (uint)HikvisionHelper.GetChannelNumber(Camera.Channel, this.StartDigitalChannel);
            struDownPara.struStartTime.dwYear = (uint)StartTime.Year;
            struDownPara.struStartTime.dwMonth = (uint)StartTime.Month;
            struDownPara.struStartTime.dwDay = (uint)StartTime.Day;
            struDownPara.struStartTime.dwHour = (uint)StartTime.Hour;
            struDownPara.struStartTime.dwMinute = (uint)StartTime.Minute;
            struDownPara.struStartTime.dwSecond = (uint)StartTime.Second;
            struDownPara.struStopTime.dwYear = (uint)EndTime.Year;
            struDownPara.struStopTime.dwMonth = (uint)EndTime.Month;
            struDownPara.struStopTime.dwDay = (uint)EndTime.Day;
            struDownPara.struStopTime.dwHour = (uint)EndTime.Hour;
            struDownPara.struStopTime.dwMinute = (uint)EndTime.Minute;
            struDownPara.struStopTime.dwSecond = (uint)EndTime.Second;

            // 16-Julio-2021 * vmon-4389 * ddvl * Se espera que intente 3 veces antes de informar el error.
            while (_try <= (tryLimit + 1)) // prevent indefinite cycle
            {
                try
                {
                    fileHandle = CHCNetSDK.NET_DVR_GetFileByTime_V40(loginHandle, FileName, ref struDownPara);
                    uint iOutValue = 0;
                    if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(fileHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
                    {
                        if (TryLimit(ref _try, "HikVision try to reconnect"))
                        {
                            Logger.Log(string.Format("Reconnection Failed {0} , excced the maximun number of re try ", Camera.Name), LogPriority.Important);
                            break;
                        }
                    }
                    else
                    {
                        timerDownload.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        timerDownload.Interval = 4000;
                        timerDownload.Start();
                        Logger.Log(string.Format("Device {0} has started download", Camera.Name), LogPriority.Information);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (TryLimit(ref _try, ex.Message))
                    {
                        Logger.Log(string.Format("Reconnection Exception Failed {0} , excced the maximun number of re try ", Camera.Name), LogPriority.Fatal);
                        break;
                    }
                }
            } // end while _try
        }
        private static readonly object _lock = new object();
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                if (canCheckDownload)
                {
                    canCheckDownload = false;
                }
                else
                {
                    return;
                }
            }
            int iPos = 0;
            //Get downloading process
            iPos = CHCNetSDK.NET_DVR_GetDownloadPos(fileHandle);
            if (iPos == 100)  //Finish downloading
            {
                canCheckDownload = false;
                OnDownloadProgress?.Invoke(this, 100);
                OnDownloadCompleted?.Invoke(this, FileName);
                StopDownLoad();
                timerDownload.Stop();
            }
            else if (iPos == 200) //Network abnormal,download failed
            {
                canCheckDownload = false;
                OnDownloadError?.Invoke(this, "Download failed");
                timerDownload.Stop();
            }
            else
            {
                canCheckDownload = true;
                Logger.Log(string.Format("Device {0} is downloading a file, progress {1}", Camera.Name, (int)iPos), LogPriority.Information);
                OnDownloadProgress?.Invoke(this, (int)iPos);
            }
        }

        internal void StopDownLoad()
        {
            if (fileHandle != -1)
            {
                CHCNetSDK.NET_DVR_StopGetFile(fileHandle);
                fileHandle = -1;
                timerDownload.Stop();
            }
        }

        public void Stop()
        {
            try
            {
                StopDownLoad();
                bool result = CHCNetSDK.NET_DVR_Logout(loginHandle);
                if (result)
                {
                    loginHandle = -1;
                }
            }
            catch (Exception)
            {
                //OnDownloadError?.Invoke(this, ex.Message);
            }
        }

        public void Disconect(IntPtr HandledBrocaster)
        {

        }

        public void Connect(IntPtr HandledBrocaster)
        {

        }
    }
}
