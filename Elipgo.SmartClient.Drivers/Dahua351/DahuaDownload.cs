using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Drivers.Dahua351.NetSDKCS;
using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Drivers.Dahua351
{
    public class DahuaDownload : IDriverDownload, IDisposable, IConectionNotification
    {
        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        private IntPtr loginHandle = IntPtr.Zero;
        private IntPtr fileHandle = IntPtr.Zero;

        private fDisConnectCallBack disConnectHandle;
        private readonly fTimeDownLoadPosCallBack timeDownLoadPosHandle;
        private readonly int tryLimit;
        private int retryCount = 0;
        private readonly Random _random = new Random();
        private double TimeReConnectionCheck = 2;

        public DahuaDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName)
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

            BookmarkGroupElement = bookmarkGroupElement;
            Camera = camera;
            StartTime = DateTime.Parse(bookmarkGroupElement.TimeStart);
            EndTime = DateTime.Parse(bookmarkGroupElement.TimeEnd);
            FileName = fileName + ".dav";

            disConnectHandle = new fDisConnectCallBack(DisConnectCallBack);
            timeDownLoadPosHandle = new fTimeDownLoadPosCallBack(TimeDownLoadPosCallBack);

            bool res = NETClient.Init(disConnectHandle, IntPtr.Zero, null);
            if (res)
            {
                if (Login())
                {
                    Logger.Log(String.Format("New Dahua Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                }
            }
        }

        private bool Login()
        {

            NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
            try
            {
                ushort videoPort = 0;
                try
                {
                    videoPort = (UInt16)Camera.VideoPort;
                }
                catch (Exception ex)
                {
                    Logger.Log("ID_PORTERROR port: " + Camera.VideoPort + " Exception: " + ex.Message, LogPriority.Fatal);
                    return false;
                }
                loginHandle = NETClient.Login(Camera.Host, videoPort, Camera.User, Camera.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
                if (loginHandle != IntPtr.Zero)
                {
                    Logger.Log(String.Format(" Dahua Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                }
                else
                {
                    if (retryCount <= tryLimit)
                    {
                        Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => Login());
                        Logger.Log(String.Format("Dahua Login() Error to Camera login current {4} of {5}:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCount, tryLimit), LogPriority.Information);
                        retryCount++;
                    }
                    else
                    {
                        Logger.Log(String.Format("Dahua Login() reached max retry number, then it is  disconnected: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                        OnDownloadError?.Invoke(this, string.Format("Dahua login error"));

                    }
                }
                return loginHandle != IntPtr.Zero;

            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Login Excepotion: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                return false;
            }
        }

        public void Dispose()
        {
            if (loginHandle != IntPtr.Zero)
            {
                NETClient.Logout(loginHandle);
                Logger.Log(String.Format(" Dispose Logout Dahua {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
            }
            NETClient.Cleanup();
            this.OnDispose?.Invoke(Common.Enum.Drivers.NETSDK_351, Guid.NewGuid().ToString(), this);
        }

        public void Start()
        {
            if (loginHandle != IntPtr.Zero)
            {

                SetDeviceMode(loginHandle, GetStreamType(), EM_RECORD_TYPE.ALL);
                TryToReConnect();
            }
        }

        private void TryToReConnect()
        {
            try
            {
                if (retryCount >= tryLimit)
                {
                    OnDownloadError?.Invoke(this, string.Format("Dahua login error"));
                    Logger.Log(String.Format("Dahua TryToReConnect reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    return;
                }

                retryCount++;
                if (!Connect())
                {
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Logger.Log(String.Format(" TryToReConnect RealStar failed  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    Task.Delay(r).ContinueWith(t => TryToReConnect());
                }
                else
                {
                    Logger.Log(String.Format(" TryToReConnect RealStar Connected  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    retryCount = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format(" TryToReConnect Exception: {4}  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex.Message), LogPriority.Fatal);
            }
        }

        private bool Connect()
        {
            try
            {
                fileHandle = NETClient.DownloadByTime(loginHandle, Camera.Channel - 1, EM_QUERY_RECORD_TYPE.ALL, StartTime, EndTime, FileName, timeDownLoadPosHandle, IntPtr.Zero, null, IntPtr.Zero, IntPtr.Zero);
                if (fileHandle != IntPtr.Zero)
                {
                    return true;
                }
                else
                {
                    Logger.Log(String.Format(" {0} SetDeviceMode: {1}, {2}, {3}.", NETClient.GetLastError(), loginHandle.ToString(), string.IsNullOrWhiteSpace(Camera.RecordingMode) ? "null" : Camera.RecordingMode, EM_RECORD_TYPE.ALL.ToString()), LogPriority.Warning);
                    return false;
                }
            }
            catch (NETClientExcetion ex)
            {
                Logger.Log(String.Format("Login Excepotion: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                return false;
            }

        }

        public void Stop()
        {
            try
            {
                StopDownLoad();
                bool result = NETClient.Logout(loginHandle);
                if (result)
                {
                    loginHandle = IntPtr.Zero;
                }
            }
            catch (Exception)
            {
                //OnDownloadError?.Invoke(this, ex.Message);
            }
        }

        private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            Stop();
        }

        private void SetDeviceMode(IntPtr lLoginID, EM_STREAM_TYPE streamType, EM_RECORD_TYPE recordType)
        {
            IntPtr pStream = IntPtr.Zero;
            IntPtr pRecordType = IntPtr.Zero;
            try
            {
                //set streamType
                pStream = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                Marshal.StructureToPtr((int)streamType, pStream, true);
                NETClient.SetDeviceMode(lLoginID, EM_USEDEV_MODE.RECORD_STREAM_TYPE, pStream);

                //set recordType
                pRecordType = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                Marshal.StructureToPtr((int)recordType, pRecordType, true);
                NETClient.SetDeviceMode(lLoginID, EM_USEDEV_MODE.RECORD_TYPE, pRecordType);
            }
            finally
            {
                Marshal.FreeHGlobal(pStream);
                Marshal.FreeHGlobal(pRecordType);
            }
        }

        internal void StopDownLoad()
        {
            if (IntPtr.Zero != fileHandle)
            {
                NETClient.StopDownload(fileHandle);
                fileHandle = IntPtr.Zero;
            }
        }

        private void TimeDownLoadPosCallBack(IntPtr lPlayHandle, uint dwTotalSize, uint dwDownLoadSize, int index, NET_RECORDFILE_INFO recordfileinfo, IntPtr dwUser)
        {
            try
            {
                if (lPlayHandle != fileHandle)
                {
                    return;
                }
                int downLoadOver = -1;
                if ((uint)downLoadOver != dwDownLoadSize)
                {
                    float progress = (dwDownLoadSize * 100) / dwTotalSize;
                    BookmarkGroupElement.Progress = progress;
                    OnDownloadProgress?.Invoke(this, (int)progress);
                }
                else
                {
                    BookmarkGroupElement.Success = true;
                    BookmarkGroupElement.Progress = 100;
                    StopDownLoad();
                    AsyncCallback callbackProgress = new AsyncCallback(CallbackProgress);
                    OnDownloadProgress?.BeginInvoke(this, 100, callbackProgress, null);
                    AsyncCallback callbackCompleted = new AsyncCallback(CallbackCompleted);
                    OnDownloadCompleted?.BeginInvoke(this, FileName, callbackCompleted, null);
                }
            }
            catch (Exception ex )
            {
                Logger.Log(ex);
            }
            
        }

        private EM_STREAM_TYPE GetStreamType()
        {
            if (string.IsNullOrWhiteSpace(Camera.RecordingMode) || Camera.RecordingMode.ToLower() == "mainstream")
            {
                return EM_STREAM_TYPE.MAIN;
            }
            else
            {
                return EM_STREAM_TYPE.EXTRA_1;
            }
        }

        private void CallbackProgress(IAsyncResult result)
        {
        }

        private void CallbackCompleted(IAsyncResult result)
        {
            Dispose();
        }

        public void Disconect(IntPtr HandledBrocaster)
        {

        }

        public void Connect(IntPtr HandledBrocaster)
        {

        }

        private string GetFirmwareVersion()
        {
            return string.Empty;
        }
    }
}
