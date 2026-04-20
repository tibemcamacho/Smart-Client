using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Drivers
{
    public delegate void OnTimeChangedEventHandler(DateTime dateTime, object parent);
    public delegate void OnStateChangedEventHandler(PlaybackState state, object parent);

    public interface IDriverInstantPlayback : IDisposable
    {
        // Properties
        CameraDTO Camera { get; set; }
        Profile Profile { get; set; }
        List<ButtonsContextBar> Commands { get; }
        List<ButtonsContextBar> CommandsAudioPtz { get; }
        bool ClipStatus { get; set; }
        bool ZoomStatus { get; set; }
        bool BookmarkState { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        DateTime ActualTime { get; set; }

        // Events
        event OnVideoEventHandler OnVideo;
        event EventHandler<bool> OpenBookmark;
        event CameraSelectedEventHandler CameraSelected;
        event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        event OnDriverDispose OnDispose;
        event OnTimeChangedEventHandler OnTimeChanged;
        event OnStateChangedEventHandler OnStateChanged;

        // Methods
        bool Snapshot(string path);
        void ToggleFullScreen();
        bool VideoClipStart(string path);
        bool VideoClipStop();
        bool ToggleListen();
        bool Volume(int value);
        bool Play();
        bool Stop();
        bool Pause();
        bool Jump(int sec, bool asc);
        bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false);
        bool Slow();
        bool Fast();
        bool SyncSpeed(PlaySpeed masterSpeed, bool updateLabelSpeed);
        bool Rewind();
        bool CapacityNotAvailable(bool show);
        bool SetStartUpSpeed(int speed);
        bool ToogleDigitalZoom();
        PlaySpeed GetCurrentSpeed();
        List<ButtonsPlayBackBar> ButtonsNotAllowed();
        int Hash();
        bool PlayVideo();
        bool PlayNoAsync();
        void SelectSpeed(PlaySpeed speed);
        void UpdateSlider(double sliderMaxMinutes, PlayScaleTimeLine playScaleTimeLine, bool isVault, DateTime start, DateTime end, int block, int totalBlocks);
        int GetCurrentSliderValue();
        int GetMaxSliderValue();
        DateTime GetDateSelected();
    }
}

