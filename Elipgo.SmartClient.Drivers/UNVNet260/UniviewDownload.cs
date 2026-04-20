using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Drivers.UNVNet260
{
    public class UniviewDownload : IDriverDownload, IDisposable, IConectionNotification
    {
        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        private IntPtr loginHandle = IntPtr.Zero;
        private IntPtr fileHandle = IntPtr.Zero;
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        //private DispatcherTimer c = new DispatcherTimer();
        public System.Timers.Timer m_timer = new System.Timers.Timer();
        public event OnDriverDispose OnDispose;

        private readonly int tryLimit;
        private Int16 _retry = 0;
        private List<NETUNV.UNV_UPDATE_TIME_INFO> m_downloadInfoList = new List<NETUNV.UNV_UPDATE_TIME_INFO>();
        private int m_iLastCount = 0;
        private List<string> listView = new List<string>();
        private readonly Random _random = new Random();
        private double TimeReConnectionCheck = 2;

        public UniviewDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName)
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

            BookmarkGroupElement = bookmarkGroupElement;
            Camera = camera;
            StartTime = DateTime.Parse(bookmarkGroupElement.TimeStart);
            EndTime = DateTime.Parse(bookmarkGroupElement.TimeEnd);
            FileName = fileName + ".mp4";
            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_Init())
                OnDownloadError?.Invoke(this, "Error to init");
        }

        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;

        public void Dispose()
        {
            if (loginHandle != IntPtr.Zero)
            {
                if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.UNVNetSDK_260))
                {

                    int result = NETDEVSDK.NETDEV_Logout(loginHandle);
                    if (result == NETDEVSDK.TRUE)
                    {
                        Logger.Log(String.Format(" Dispose Logout uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                        loginHandle = IntPtr.Zero;
                        NETDEVSDK.NETDEV_Cleanup();
                    }
                    else
                        Logger.Log(String.Format(" Error to Logout uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                }
            }
        }

        private bool Login()
        {
            if (_retry >= tryLimit)
            {
                Logger.Log(String.Format("Uniview Login() reached max retry number, then it is  disconnected: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                OnDownloadError?.Invoke(this, "Login error");
                return false;
            }

            if (loginControl.GetDeviceLogin(Camera, out loginHandle, Common.Enum.Drivers.UNVNetSDK_260, this))
            {
                loginControl.AddChannel(Camera, this, Common.Enum.Drivers.UNVNetSDK_260);
                Logger.Log(String.Format("Reusing a Uniview Login to Camara {0}", Camera.Name), LogPriority.Information);
                return true;
            }

            int videoPort = Camera.HttpPort == 0 ? (int)8000 : (int)Camera.HttpPort;
            NETDEV_DEVICE_LOGIN_INFO_S pstDevLoginInfo = new NETDEV_DEVICE_LOGIN_INFO_S();
            NETDEV_SELOG_INFO_S pstSELogInfo = new NETDEV_SELOG_INFO_S();
            pstDevLoginInfo.szIPAddr = Camera.Host;
            pstDevLoginInfo.dwPort = videoPort;
            pstDevLoginInfo.szUserName = Camera.User;
            pstDevLoginInfo.szPassword = Camera.Password;
            pstDevLoginInfo.dwLoginProto = (int)NETDEV_LOGIN_PROTO_E.NETDEV_LOGIN_PROTO_ONVIF;

            loginHandle = NETDEVSDK.NETDEV_Login_V30(ref pstDevLoginInfo, ref pstSELogInfo);

            if (loginHandle != IntPtr.Zero)
            {
                _retry = 0;
                loginControl.AddDevice(Camera, (IntPtr)loginHandle, this, Common.Enum.Drivers.UNVNetSDK_260);
                Logger.Log(String.Format("New Uniview Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                return true;
            }
            else
            {
                _retry++;
                Logger.Log(string.Format("Device Login {0}  is Zero, Error: {1} ", Camera.Name, NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * _retry) }, () => Login());
            }
            return false;
        }

        public void Start()
        {
            if (Login())
            {
                try
                {
                    if (OpenCamera())
                    {
                        NETDEV_PLAYBACKCOND_S stPlayBackInfo = new NETDEV_PLAYBACKCOND_S();
                        String beginDateTimeStr = getInputStartDataTime();
                        String endDateTimeStr = getInputEndDataTime();

                        stPlayBackInfo.tBeginTime = this.getLongTime(beginDateTimeStr);
                        stPlayBackInfo.tEndTime = this.getLongTime(endDateTimeStr);

                        stPlayBackInfo.hPlayWnd = IntPtr.Zero;
                        stPlayBackInfo.dwDownloadSpeed = (int)NETDEV_E_DOWNLOAD_SPEED_E.NETDEV_DOWNLOAD_SPEED_ONE;
                        stPlayBackInfo.dwChannelID = Camera.Channel;

                        NETUNV.UNV_UPDATE_TIME_INFO stUpdateInfo = new NETUNV.UNV_UPDATE_TIME_INFO();
                        stUpdateInfo.lpHandle = fileHandle;
                        stUpdateInfo.tBeginTime = stPlayBackInfo.tBeginTime;
                        stUpdateInfo.tEndTime = stPlayBackInfo.tEndTime;
                        stUpdateInfo.strFileName = FileName;
                        stUpdateInfo.strFilePath = Path.GetDirectoryName(FileName);
                        stUpdateInfo.dwCount = 0;
                        stUpdateInfo.tCurTime = 0;
                        stUpdateInfo.downLoad_status = true;
                        m_downloadInfoList.Add(stUpdateInfo);
                        setListView(stUpdateInfo);

                        m_timer.Interval = 1000;
                        m_timer.Elapsed += TimerDownload_Tick;
                        m_timer.Start();
                    }
                }
                catch (Exception)
                {
                    _retry++;
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Logger.Log("Uniview try to reconnect");
                    Task.Delay(r).ContinueWith(t => OpenCamera());
                }
            }
        }

        private bool OpenCamera()
        {
            if (_retry >= tryLimit)
            {
                Logger.Log(String.Format("Uniview Start reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                OnDownloadError?.Invoke(this, string.Format("uniview download  error"));
                return false;
            }

            NETDEV_PLAYBACKCOND_S stPlayBackInfo = new NETDEV_PLAYBACKCOND_S();
            String beginDateTimeStr = getInputStartDataTime();
            String endDateTimeStr = getInputEndDataTime();

            stPlayBackInfo.tBeginTime = this.getLongTime(beginDateTimeStr);
            stPlayBackInfo.tEndTime = this.getLongTime(endDateTimeStr);

            stPlayBackInfo.hPlayWnd = loginHandle;
            stPlayBackInfo.dwDownloadSpeed = (int)NETDEV_E_DOWNLOAD_SPEED_E.NETDEV_DOWNLOAD_SPEED_EIGHT;
            stPlayBackInfo.dwLinkMode = (int)NETDEV_PROTOCAL_E.NETDEV_TRANSPROTOCAL_RTPTCP;
            stPlayBackInfo.dwPlaySpeed = (int)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_FORWARD;
            stPlayBackInfo.dwChannelID = Camera.Channel;

            byte[] localRecordPath;
            GetUTF8Buffer(FileName, NETDEVSDK.NETDEV_LEN_260, out localRecordPath);
            fileHandle = NETDEVSDK.NETDEV_GetFileByTime(loginHandle, ref stPlayBackInfo, localRecordPath, (int)NETDEV_MEDIA_FILE_FORMAT_E.NETDEV_MEDIA_FILE_MP4);
            if (IntPtr.Zero == fileHandle)
            {

                _retry++;
                int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                Logger.Log("Uniview try to reconnect");
                Task.Delay(r).ContinueWith(t => OpenCamera());
                return false;
            }
            else
            {
                _retry = 0;
                return true;
            }
        }

        public void Stop()
        {
            try
            {
                StopDownLoad();
                int result = NETDEVSDK.NETDEV_Logout(loginHandle);
                if (result == NETDEVSDK.TRUE)
                {
                    loginHandle = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Stop UniviewDownload {0}", ex), LogPriority.Fatal);
                OnDownloadError?.Invoke(this, ex.Message);
            }
        }

        internal void StopDownLoad()
        {
            if (fileHandle != IntPtr.Zero)
            {
                NETDEVSDK.NETDEV_StopGetFile(fileHandle);
                fileHandle = IntPtr.Zero;
                m_timer.Stop();
            }
        }

        private string getInputStartDataTime()
        {
            String beginDateTimeStr = this.StartTime.Year.ToString();
            beginDateTimeStr += ("-" + this.StartTime.Month.ToString());
            beginDateTimeStr += ("-" + this.StartTime.Day.ToString());

            beginDateTimeStr += (" " + this.StartTime.Hour.ToString());
            beginDateTimeStr += (":" + this.StartTime.Minute.ToString());
            beginDateTimeStr += (":" + this.StartTime.Second.ToString());

            return beginDateTimeStr;
        }

        private string getInputEndDataTime()
        {
            String endDateTimeStr = this.EndTime.Year.ToString();
            endDateTimeStr += ("-" + this.EndTime.Month.ToString());
            endDateTimeStr += ("-" + this.EndTime.Day.ToString());

            endDateTimeStr += (" " + this.EndTime.Hour.ToString());
            endDateTimeStr += (":" + this.EndTime.Minute.ToString());
            endDateTimeStr += (":" + this.EndTime.Second.ToString());
            return endDateTimeStr;
        }

        private long getLongTime(String strTime)
        {
            DateTime dateTime = Convert.ToDateTime(strTime).AddHours(-1);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 
            return (long)(dateTime - startTime).TotalSeconds;
        }

        private void GetUTF8Buffer(string inputString, int bufferLen, out byte[] utf8Buffer)
        {
            utf8Buffer = new byte[bufferLen];
            byte[] tempBuffer = System.Text.Encoding.UTF8.GetBytes(inputString);
            for (int i = 0; i < tempBuffer.Length; ++i)
            {
                utf8Buffer[i] = tempBuffer[i];
            }
        }

        private void TimerDownload_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < m_downloadInfoList.Count; i++)
            {
                if (m_downloadInfoList[i].downLoad_status == true)
                {
                    long iPlayTime = 0;
                    int iRet = NETDEVSDK.NETDEV_PlayBackControl(m_downloadInfoList[i].lpHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYTIME, ref iPlayTime);
                    if (NETDEVSDK.TRUE != iRet)
                    {
                        //NETDEMO_LOG_ERROR(NULL, "play back control get play time");
                    }

                    if (NETUNV.NETUNV_DOWNLOAD_TIME_COUNT < m_downloadInfoList[i].dwCount || iPlayTime >= m_downloadInfoList[i].tEndTime)
                    {
                        NETDEVSDK.NETDEV_StopGetFile(m_downloadInfoList[i].lpHandle);
                        m_downloadInfoList[i].downLoad_status = false;

                        if (m_downloadInfoList.Count < getListViewItemCount())
                        {
                            updateProgress(i + getListViewItemLastCount(), 100);
                        }
                        else
                        {
                            updateProgress(i, 100);
                        }
                    }
                    else
                    {
                        if (m_downloadInfoList[i].tCurTime == iPlayTime)
                        {
                            m_downloadInfoList[i].dwCount++;
                        }
                        else
                        {
                            m_downloadInfoList[i].dwCount = 0;
                            m_downloadInfoList[i].tCurTime = iPlayTime;
                            //update porcess
                            if (m_downloadInfoList.Count < getListViewItemCount())
                            {
                                updateProgress(i + getListViewItemLastCount(), (int)(((float)(iPlayTime - m_downloadInfoList[i].tBeginTime) / (m_downloadInfoList[i].tEndTime - m_downloadInfoList[i].tBeginTime)) * 100));
                            }
                            else
                            {
                                updateProgress(i, (int)(((float)(iPlayTime - m_downloadInfoList[i].tBeginTime) / (m_downloadInfoList[i].tEndTime - m_downloadInfoList[i].tBeginTime)) * 100));
                            }
                        }
                    }
                }
            }
        }

        public int getListViewItemCount()
        {
            return this.listView.Count;
        }

        public int getListViewItemLastCount()
        {
            return m_iLastCount;
        }

        public void updateProgress(int index, int progressValue)
        {
            if (progressValue == 100)
            {
                BookmarkGroupElement.Progress = 100;
                BookmarkGroupElement.Success = true;
                OnDownloadProgress?.Invoke(this, 100);
                OnDownloadCompleted?.Invoke(this, FileName);
                StopDownLoad();
                m_timer.Stop();
            }
            else
            {
                BookmarkGroupElement.Progress = progressValue;
                OnDownloadProgress?.Invoke(this, progressValue);
            }
        }

        public void setListView(NETUNV.UNV_UPDATE_TIME_INFO stUpdateInfo)
        {
            this.listView.AddRange(new List<string>() { getStrTime(stUpdateInfo.tBeginTime), getStrTime(stUpdateInfo.tEndTime), "0", stUpdateInfo.strFilePath });
        }

        private string getStrTime(long time)
        {
            DateTime startDateTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return startDateTime.AddSeconds(time).ToString("yyyy/MM/dd HH:mm:ss");
        }

        public void Disconect(IntPtr HandledBrocaster)
        {

        }

        public void Connect(IntPtr HandledBrocaster)
        {

        }
    }
}
