using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Drivers.Dahua352.NetSDKCS;
using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Elipgo.SmartClient.Drivers.Dahua352
{
    public class DahuaDownload : IDriverDownload, IDisposable
    {
        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private readonly int tryLimit;

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        private IntPtr loginHandle = IntPtr.Zero;
        private IntPtr fileHandle = IntPtr.Zero;

        private fDisConnectCallBack disConnectHandle;
        private readonly fTimeDownLoadPosCallBack timeDownLoadPosHandle;

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
                NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
                ushort videoPort = Camera.VideoPort == 0 ? (ushort)37777 : (ushort)Camera.VideoPort;
                loginHandle = NETClient.Login(Camera.Host, videoPort, Camera.User, Camera.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
            }
        }

        public void Dispose()
        {
            if (loginHandle != IntPtr.Zero)
            {
                NETClient.Logout(loginHandle);
            }

            NETClient.Cleanup();
        }

        public void Start()
        {
            Int16 _try = 0;

            // 16-Julio-2021 * vmon-4389 * ddvl * Se espera que intente 3 veces antes de informar el error.
            while (_try <= (tryLimit + 1)) // prevent indefinite cycle
            {
                try
                {
                    if (loginHandle != IntPtr.Zero)
                    {
                        SetDeviceMode(loginHandle, EM_STREAM_TYPE.MAIN, EM_RECORD_TYPE.ALL);
                        fileHandle = NETClient.DownloadByTime(loginHandle, Camera.Channel - 1, EM_QUERY_RECORD_TYPE.ALL, StartTime, EndTime, FileName, timeDownLoadPosHandle, IntPtr.Zero, null, IntPtr.Zero, IntPtr.Zero);
                        //fileHandle = NETClient.DownloadByTime(loginHandle, Camera.Channel -1, EM_QUERY_RECORD_TYPE.ALL, StartTime, EndTime, FileName, timeDownLoadPosHandle, IntPtr.Zero);
                        //NET_IN_ADAPTIVE_DOWNLOAD_BY_TIME param = new NET_IN_ADAPTIVE_DOWNLOAD_BY_TIME();
                        //param.nChannelID = Camera.Channel;
                        //param.szSavedFileName = FileName;
                        //param.stStartTime = NET_TIME.FromDateTime(StartTime);
                        //param.stStopTime = NET_TIME.FromDateTime(EndTime);
                        //param.cbDownLoadPos = timeDownLoadPosHandle;
                        //param.dwPosUser = IntPtr.Zero;
                        //param.fDownLoadDataCallBack = null;
                        //param.emDataType = EM_REAL_DATA_TYPE.EM_REAL_DATA_TYPE_MP4;
                        //param.dwDataUser = IntPtr.Zero;
                        //param.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_ADAPTIVE_DOWNLOAD_BY_TIME));

                        //NET_OUT_ADAPTIVE_DOWNLOAD_BY_TIME output = new NET_OUT_ADAPTIVE_DOWNLOAD_BY_TIME();
                        //output.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_ADAPTIVE_DOWNLOAD_BY_TIME));

                        //var t = (uint)Marshal.SizeOf(typeof(string)) * FileName.Length;
                        //fileHandle = NETClient.AdaptativeDownloadByTime(loginHandle, Camera.Channel, ref param, ref output, 5000);
                        if (fileHandle != IntPtr.Zero)
                        {
                            break;
                        }
                        if (TryLimit(ref _try
                            , "Excced the maximun number of re try"
                            , String.Format("DahuaDownload 352 start faild channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle)
                            , LogPriority.Information))
                        {
                            Logger.Log("Reconnection Failed, excced the maximun number of re try ");
                            break;
                        }
                    }
                    else
                    {
                        if (TryLimit(ref _try
                       , "Excced the maximun number of Loggin re try"
                       , String.Format(" DahuaDownload 352 start faild channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle)
                       , LogPriority.Information))
                        {
                            Logger.Log("Reconnection 352 Login Failed, excced the maximun number of re try ");
                            break;
                        }
                        //si no se loggeo intento volver a loggear
                        OnDownloadError?.Invoke(this, string.Format("Dahua Start Login error try {0}", _try));
                        Logger.Log("Dahua 352 Start Login Handler is null", LogPriority.Information);
                        NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
                        ushort videoPort = Camera.VideoPort == 0 ? (ushort)37777 : (ushort)Camera.VideoPort;
                        loginHandle = NETClient.Login(Camera.Host, videoPort, Camera.User, Camera.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
                    }
                }
                catch (NETClientExcetion ex)
                {
                    if (TryLimit(ref _try
                        , ex.Message
                        , String.Format("DahuaDownload 352 start channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle) + " Exception:" + ex.Message
                        , LogPriority.Fatal))
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (TryLimit(ref _try
                        , ex.Message
                        , String.Format("DahuaDownload 352 start channel{0} start: {1} end:{2} filaname:{3} loginHandle:{4} timeDownLoadPosHandle:{5}", Camera.Channel - 1, StartTime, EndTime, FileName, loginHandle, timeDownLoadPosHandle) + " Exception:" + ex.Message
                        , LogPriority.Fatal))
                    {
                        break;
                    }
                }
            } // end while _try
        }

        private bool TryLimit(ref Int16 _try, string errMsg, string logDescription, LogPriority logPriority)
        {
            bool blimit = false;
            _try++;
            Logger.Log("try " + _try + logDescription, logPriority);
            System.Threading.Thread.Sleep(3000);
            if (_try >= tryLimit)
            {
                OnDownloadError?.Invoke(this, errMsg);
                blimit = true;
            }
            return blimit;
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
                BookmarkGroupElement.Progress = 100;
                OnDownloadProgress?.Invoke(this, 100);
                OnDownloadCompleted?.Invoke(this, FileName);
                StopDownLoad();
            }
        }


    }
}
