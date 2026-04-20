using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Drivers.Dahua352.NetSDKCS;
using Splat;
using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Elipgo.SmartClient.Drivers.Dahua352
{
    public class DahuaDownloadVisualSearch : IDriverDownloadVisualSearch, IDisposable, IConectionNotification
    {
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }

        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        public event OnDownloadVisualSearchCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadVisualSearchProgressEventHandler OnDownloadProgress;
        public event OnDownloadVisualSearchErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        private IntPtr loginHandle = IntPtr.Zero;
        private IntPtr fileHandle = IntPtr.Zero;

        private fDisConnectCallBack disConnectHandle;
        private readonly fTimeDownLoadPosCallBack timeDownLoadPosHandle;

        private readonly int tryLimit;

        public DahuaDownloadVisualSearch(CameraDTO camera)
        {
            Camera = camera;
            disConnectHandle = new fDisConnectCallBack(DisConnectCallBack);
            timeDownLoadPosHandle = new fTimeDownLoadPosCallBack(TimeDownLoadPosCallBack);

            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
        }

        public void InitializeDriver()
        {
            Int16 _try = 0;
            if (NETClient.Init(disConnectHandle, IntPtr.Zero, null))
            {
                if (loginControl.GetDeviceLogin(Camera, out loginHandle, Common.Enum.Drivers.NETSDK_352, this))
                {
                    loginControl.AddChannel(Camera, this, Common.Enum.Drivers.NETSDK_352);
                    Logger.Log(String.Format("Reusing a Dahua Login to Camara {0}", Camera.Name), LogPriority.Information);
                    return;
                }
                NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
                ushort videoPort = Camera.VideoPort == 0 ? (ushort)37777 : (ushort)Camera.VideoPort;
                while (_try <= (tryLimit + 1))
                {
                    loginHandle = NETClient.Login(Camera.Host, videoPort, Camera.User, Camera.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
                    if (loginHandle == IntPtr.Zero)
                    {
                        Logger.Log(string.Format("Device Login {0}  is Zero, Error: {1} ", Camera.Name, NETClient.GetLastError()), LogPriority.Fatal);
                        if (TryLimit(ref _try, ""))
                        {
                            Logger.Log(string.Format("logind Failed, {0} excced the maximun number of re try", Camera.Name), LogPriority.Fatal);
                            break;
                        }
                        else
                        {
                            loginControl.AddDevice(Camera, (IntPtr)loginHandle, this, Common.Enum.Drivers.NETSDK_352);
                            Logger.Log(String.Format("New Dahua Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                            break;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                Logger.Log(string.Format(" NETClient_Init Failed Camera:{0}", Camera.Name), LogPriority.Information);
            }
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

        public void Dispose()
        {
            if (loginHandle != IntPtr.Zero)
                NETClient.Logout(loginHandle);
            NETClient.Cleanup();
        }

        public void Start(string fileName, DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            FileName = fileName + ".dav";
            Logger.Log(String.Format("Dahua 352 start entered camera {0} Channel{1} start: {2} end:{3} filaname:{4} ", Camera.Name, Camera.Channel - 1, startTime, endTime, fileName), LogPriority.Information);
            InitializeDriver();

            Int16 _try = 0;

            if (loginHandle == IntPtr.Zero)
            {
                OnDownloadError?.Invoke(this, String.Format("Login error {0}", Camera.Name));
                return;
            }

            Logger.Log(String.Format(" DahuaDownload start entered channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle), LogPriority.Information);

            SetDeviceMode(loginHandle, EM_STREAM_TYPE.MAIN, EM_RECORD_TYPE.ALL);
            while (_try <= (tryLimit + 1)) // prevent indefinite cycle
            {
                try
                {
                    fileHandle = NETClient.DownloadByTime(loginHandle, Camera.Channel - 1, EM_QUERY_RECORD_TYPE.ALL, StartTime, EndTime, FileName, timeDownLoadPosHandle, IntPtr.Zero, null, IntPtr.Zero, IntPtr.Zero);
                    if (fileHandle == IntPtr.Zero)
                    {
                        Logger.Log(String.Format(" DahuaDownload start faild channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle), LogPriority.Information);
                        if (TryLimit(ref _try, string.Format("Device Start  {0} filehandle is Zero, Error: {1} ", Camera.Name, NETClient.GetLastError())))
                        {
                            Logger.Log(string.Format("Reconnection Failed, {0} excced the maximun number of re try", Camera.Name), LogPriority.Fatal);
                            break;
                        }
                        //OnDownloadError?.Invoke(this, "No recordings are available");
                    }
                    else
                    {
                        Logger.Log(string.Format("Device {0} has started download", Camera.Name), LogPriority.Information);
                        break;
                    }
                }
                catch (NETClientExcetion ex)
                {
                    Logger.Log(String.Format(" DahuaDownload start channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle) + " Exception:" + ex.Message, LogPriority.Fatal);
                    if (TryLimit(ref _try, ex.Message)) break;
                    //OnDownloadError?.Invoke(this, ex.Message);
                }
                catch (Exception ex)
                {
                    Logger.Log(String.Format(" DahuaDownload start channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle) + " Exception:" + ex.Message, LogPriority.Fatal);
                    if (TryLimit(ref _try, ex.Message)) break;
                    //OnDownloadError?.Invoke(this, ex.Message);
                }
            }


        }

        public void Stop()
        {
            try
            {
                Logger.Log(String.Format(" DahuaDownload Stop entered channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle), LogPriority.Information);
                StopDownLoad();
                bool result = NETClient.Logout(loginHandle);
                if (result)
                {
                    Logger.Log(String.Format(" DahuaDownload Stop Logout entered channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle), LogPriority.Information);
                    loginHandle = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format(" DahuaDownload Stop Exception channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle) + " Exception:" + ex.Message, LogPriority.Fatal);
                OnDownloadError?.Invoke(this, ex.Message);
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
                    OnDownloadProgress?.Invoke(this, (int)progress);
                }
                else
                {
                    OnDownloadProgress?.Invoke(this, 100);
                    OnDownloadCompleted?.Invoke(this, FileName);
                    StopDownLoad();
                }
            }
            catch (Exception)
            {
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
