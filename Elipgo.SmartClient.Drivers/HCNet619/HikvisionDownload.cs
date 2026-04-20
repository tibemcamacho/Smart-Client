using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Splat;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Drivers.HCNet619
{
    public class HikvisionDownload : IDriverDownload, IDisposable, IConectionNotification
    {
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        private Int32 loginHandle = -1;
        private Int32 fileHandle = -1;

        private System.Timers.Timer timerDownload = new System.Timers.Timer();
        private readonly int tryLimit;
        private int StartDigitalChannel = 0;

        private Int16 _try = 0;
        private Int16 _retry = 0;
        private readonly Random _random = new Random();
        private double TimeReConnectionCheck = 2;

        public HikvisionDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName)
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

            BookmarkGroupElement = bookmarkGroupElement;
            Camera = camera;
            StartTime = DateTime.Parse(bookmarkGroupElement.TimeStart);
            EndTime = DateTime.Parse(bookmarkGroupElement.TimeEnd);
            FileName = fileName + ".mp4";

            bool res = CHCNetSDK.NET_DVR_Init();
            if (!res)
            {
                OnDownloadError?.Invoke(this, "Login inicializar");
            }
        }
        private bool Login()
        {
            bool result = false;

            if (_try >= tryLimit)
            {
                Logger.Log(String.Format("HikVision Login() reached max retry number, then it is  disconnected: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                OnDownloadError?.Invoke(this, "Login error");
                return result;
            }

            IntPtr ptr;
            if (loginControl.GetDeviceLogin(Camera, out ptr, Common.Enum.Drivers.HCNetSDK_619, this))
            {
                //_try = 0;
                loginHandle = (Int32)ptr;
                loginControl.AddChannel(Camera, this, Common.Enum.Drivers.HCNetSDK_619);
                Logger.Log(String.Format("Reusing a Hikivion Login to Camara {0}", Camera.Name), LogPriority.Information);
                return true;
            }

            //bool res = CHCNetSDK.NET_DVR_Init();
            //if (!res)
            //{
            //    throw new Exception("Error to init driver Hikvision");
            //}


            CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            ushort videoPort = Camera.VideoPort == 0 ? (ushort)8000 : (ushort)Camera.VideoPort;
            loginHandle = CHCNetSDK.NET_DVR_Login_V30(Camera.Host, videoPort, Camera.User, Camera.Password, ref deviceInfo);
            if (loginHandle != -1)
            {
                _try = 0;
                this.StartDigitalChannel = deviceInfo.byStartDChan;
                loginControl.AddDevice(Camera, (IntPtr)loginHandle, this, Common.Enum.Drivers.HCNetSDK_616);
                Logger.Log(String.Format("New HikVision Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                result = true;
            }
            else
            {
                _try++;
                Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * _try) }, () => Login());
                Logger.Log(String.Format("HikVision Login() Error to Camera login current {4} of {5}:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, _try, tryLimit), LogPriority.Information);
            }

            return result;
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

        public void Start()
        {
            try
            {
                if (Login())
                {
                    Logger.Log(String.Format("Reusing a Hikivion Login to Camara {0}", Camera.Name), LogPriority.Information);

                    try
                    {
                        if (OpenCamera())
                        {
                            timerDownload.Elapsed += TimerDownload_Tick;
                            timerDownload.Interval = 2000;//TimeSpan.FromMilliseconds(1000);
                            timerDownload.Start();
                        }
                    }
                    catch (Exception)
                    {
                        _retry++;
                        int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                        Logger.Log("HikVision try to reconnect");
                        Task.Delay(r).ContinueWith(t => OpenCamera());
                    }
                }
            }
            catch (Exception)
            {
                _try++;
                int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                Logger.Log("HikVision try to reconnect");
                Task.Delay(r).ContinueWith(t => Start());
            }


        }

        private bool OpenCamera()
        {
            bool result = false;
            //bool res = CHCNetSDK.NET_DVR_Init();
            //if (!res)
            //{
            //    throw new Exception("Error to init driver Hikvision");
            //}
            //CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            //ushort videoPort = Camera.VideoPort == 0 ? (ushort)8000 : (ushort)Camera.VideoPort;
            //loginHandle = CHCNetSDK.NET_DVR_Login_V30(Camera.Host, videoPort, Camera.User, Camera.Password, ref deviceInfo);

            if (loginHandle != -1)
            {
                if (_retry >= tryLimit)
                {
                    Logger.Log(String.Format("HikVision Start reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    OnDownloadError?.Invoke(this, string.Format("HikVision login error"));
                    return result;
                }

                CHCNetSDK.NET_DVR_PLAYCOND struDownPara = new CHCNetSDK.NET_DVR_PLAYCOND();

                struDownPara.dwChannel = (uint)Camera.Channel;//(uint)HikvisionHelper.GetChannelNumber(Camera.Channel, this.StartDigitalChannel);
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

                fileHandle = CHCNetSDK.NET_DVR_GetFileByTime_V40(loginHandle, FileName, ref struDownPara);
                uint iOutValue = 0;
                if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(fileHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
                {

                    _retry++;
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Logger.Log("HikVision try to reconnect");
                    Task.Delay(r).ContinueWith(t => OpenCamera());
                }
                else
                {
                    result = true;
                    _retry = 0;
                }
            }
            else
            {
                throw new Exception("Error to camera login");
            }

            return result;
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
        internal void StopDownLoad()
        {
            if (fileHandle != -1 && BookmarkGroupElement.Progress == 100)
            {
                CHCNetSDK.NET_DVR_StopGetFile(fileHandle);
                fileHandle = -1;
                timerDownload.Stop();
            }
        }

        private void TimerDownload_Tick(object sender, EventArgs e)
        {
            int iPos = 0;

            //Get downloading process
            iPos = CHCNetSDK.NET_DVR_GetDownloadPos(fileHandle);
            if (iPos == 100)  //Finish downloading
            {
                BookmarkGroupElement.Progress = 100;
                BookmarkGroupElement.Success = true;
                OnDownloadProgress?.Invoke(this, 100);
                OnDownloadCompleted?.Invoke(this, FileName);
                StopDownLoad();
                timerDownload.Stop();
            }
            else if (iPos == 200) //Network abnormal,download failed
            {
                FileInfo fi = new FileInfo(FileName);
                if (fi.Length > 0)
                {
                    BookmarkGroupElement.Progress = 100;
                    BookmarkGroupElement.Success = true;
                    OnDownloadProgress?.Invoke(this, 100);
                    OnDownloadCompleted?.Invoke(this, FileName);
                    StopDownLoad();
                }
                else
                {
                    OnDownloadError?.Invoke(this, "Download failed");
                }

                timerDownload.Stop();
            }
            else
            {
                BookmarkGroupElement.Progress = iPos;
                OnDownloadProgress?.Invoke(this, (int)iPos);
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
