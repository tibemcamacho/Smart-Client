using Elipgo.SmartClient.Common.DTOs;
using System;

namespace Elipgo.SmartClient.Drivers
{
    public delegate void OnDownloadVisualSearchCompletedEventHandler(object sender, string fileName);
    public delegate void OnDownloadVisualSearchProgressEventHandler(object sender, int progress);
    public delegate void OnDownloadVisualSearchErrorEventHandler(object sender, string message);
    public interface IDriverDownloadVisualSearch : IDisposable
    {
        // Properties
        CameraDTO Camera { get; set; }
        string FileName { get; set; }

        //Events
        event OnDownloadVisualSearchCompletedEventHandler OnDownloadCompleted;
        event OnDownloadVisualSearchProgressEventHandler OnDownloadProgress;
        event OnDownloadVisualSearchErrorEventHandler OnDownloadError;
        event OnDriverDispose OnDispose;

        // Method
        void Start(string fileName, DateTime startTime, DateTime endTime);
        void Stop();
    }
}
