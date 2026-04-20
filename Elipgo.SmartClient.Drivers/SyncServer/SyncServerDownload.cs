using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using System;
using System.Linq;
using System.Net;

namespace Elipgo.SmartClient.Drivers.SyncServer
{
    public class SynServerDownload : IDriverDownload, IDisposable
    {
        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private WebClient ClientDownload { get; set; } = new WebClient();

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        public SynServerDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName)
        {
            BookmarkGroupElement = bookmarkGroupElement;
            Camera = camera;
            StartTime = DateTime.Parse(bookmarkGroupElement.TimeStart);
            EndTime = DateTime.Parse(bookmarkGroupElement.TimeEnd);
            FileName = fileName + ".mp4";
        }

        public void Dispose()
        {
        }

        public void Start()
        {
            try
            {
                var syncserver = Camera.SyncServers.Where(x => x.Id == BookmarkGroupElement.SsrId).FirstOrDefault();
                string StartTimeString = StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTimeString = EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                string StartTimeFinal = StartTimeString.Replace(" ", "T");
                var EndTimeFinal = EndTimeString.Replace(" ", "T");

                Uri uri = new Uri("http://" + syncserver.Host + ":" + syncserver.Port + "/SyncServerWebApi/api/camera/download?EntityId=" + syncserver.EntityId + "&CameraId=" + Camera.Id + "&StartDate=" + StartTimeFinal + "&EndDate=" + EndTimeFinal + "&ProtocolEncoded=H264_1&TypeRecorder=" + syncserver.SyncServerRecordersEntity.FirstOrDefault().TypeRecorder);

                ClientDownload.DownloadProgressChanged += Client_DownloadProgressChanged;
                ClientDownload.DownloadFileCompleted += Client_DownloadFileCompleted;
                ClientDownload.DownloadFileAsync(uri, FileName);
            }
            catch (Exception ex)
            {
                OnDownloadError?.Invoke(this, ex.Message);
                Logger.Log("Vault SyncServerDownload Exception: " + ex.Message, LogPriority.Fatal);
            }

        }

        public void Stop()
        {
            try
            {
                ClientDownload.CancelAsync();
            }
            catch (Exception ex)
            {
                OnDownloadError?.Invoke(this, ex.Message);
                Logger.Log("Vault SyncServerDownload Exception: " + ex.Message, LogPriority.Fatal);
            }
        }


        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var client = (WebClient)sender;
            client.DownloadProgressChanged -= Client_DownloadProgressChanged;
            client.DownloadFileCompleted -= Client_DownloadFileCompleted;

            if (e.Error != null)
            {
                OnDownloadError?.Invoke(this, e.Error.Message);
                Logger.Log("Vault SyncServerDownload Exception: " + e, LogPriority.Information);
                return;
            }

            if (e.Cancelled)
            {
                try
                {
                    if (System.IO.File.Exists(FileName))
                        System.IO.File.Delete(FileName);
                }
                catch (Exception ex)
                {
                    OnDownloadError?.Invoke(this, e.Error.Message);
                    Logger.Log("Vault SyncServerDownload Exception: " + ex.Message, LogPriority.Fatal);
                }

                return;
            }

            BookmarkGroupElement.Progress = 100;
            OnDownloadProgress?.Invoke(this, 100);
            OnDownloadCompleted?.Invoke(this, FileName);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            BookmarkGroupElement.Progress = e.ProgressPercentage;
            OnDownloadProgress?.Invoke(this, e.ProgressPercentage);
        }
    }
}
