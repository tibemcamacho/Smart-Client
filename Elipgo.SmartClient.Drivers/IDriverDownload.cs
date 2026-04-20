using Elipgo.SmartClient.Common.DTOs;
using System;

namespace Elipgo.SmartClient.Drivers
{
    public delegate void OnDownloadCompletedEventHandler(object sender, string fileName);
    public delegate void OnDownloadProgressEventHandler(object sender, int progress);
    public delegate void OnDownloadErrorEventHandler(object sender, string message);

    public interface IDriverDownload : IDisposable
    {
        // Properties
        BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        CameraDTO Camera { get; set; }
        string FileName { get; set; }

        //Events
        event OnDownloadCompletedEventHandler OnDownloadCompleted;
        event OnDownloadProgressEventHandler OnDownloadProgress;
        event OnDownloadErrorEventHandler OnDownloadError;
        event OnDriverDispose OnDispose;

        // Method
        void Start();
        void Stop();
    }
}
