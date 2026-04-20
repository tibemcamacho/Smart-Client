using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Drivers
{
    public delegate void CameraSelectedEventHandler(object sender, CameraDTO element);
    public delegate void CameraSelectedDoubleClickEventHandler(object sender);
    public delegate void OnVideoEventHandler(bool video, object parent);
    public delegate void ButtonPressedEventHandler(List<ActionCommand> pressedButton);
    public delegate void OnDriverDispose(Elipgo.SmartClient.Common.Enum.Drivers enumDriver, string nameTab, dynamic driver);
    public delegate void OnSequecingClick(int dvfID);
    public delegate void OnInitializeAudioEventHandler(object sender, bool audio);
    public delegate void OnAddExtraProfilesEventHandler(object sender, Profile profile);

    public interface IDriverLive : IDisposable
    {
        // Properties
        CameraDTO Camera { get; set; }
        Profile Profile { get; set; }
        List<Profile> Profiles { get; }
        List<ButtonsContextBar> Commands { get; }
        List<ButtonsContextBar> CommandsAudioPtz { get; }
        bool ListenStatus { get; set; }
        bool ClipStatus { get; set; }
        bool TalkStatus { get; set; }
        bool PtzStatus { get; set; }
        bool SequencingStatus { get; set; }
        bool DigitalZoomStatus { get; set; }
        bool InstantPlaybackStatus { get; set; }
        bool IsPlaying { get; set; }
        bool IsSequencingEnabled { get; }
        bool IsInitAudio { get; set; }

        // Events
        event CameraSelectedEventHandler CameraSelected;
        event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        event OnVideoEventHandler OnVideo;
        event ButtonPressedEventHandler PressedButtons;
        event OnDriverDispose OnDispose;
        event OnSequecingClick OnSequencing;
        event OnInitializeAudioEventHandler OnInitializeAudio;
        event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        // Methods
        Task<bool> Snapshot(string path, string token, int id);
        void ToggleFullScreen();
        bool VideoClipStart(string path);
        bool VideoClipStop();
        bool ToggleTalk();
        bool ToggleListen(bool Listen);
        bool Volume(int value);
        PresetDTO[] ListPresets();
        bool CallPreset(PresetDTO preset);
        bool SavePreset(PresetDTO preset);
        bool RemovePreset(PresetDTO preset);
        GuardDTO[] ListGuards();
        GuardForCreationDTO GetGuard(int guardId);
        bool CallGuard(ActivateGuardDTO guard);
        bool StopGuard(ActivateGuardDTO guard);
        bool SaveGuard(GuardForCreationDTO guard);
        bool RemoveGuard(GuardDTO guard);
        bool StateGuard(GuardDTO guard);
        bool Play();
        bool Stop();
        bool ChangeProfile(Profile profile, bool autoSwitching = false);
        void DisposeDragged();
        bool ToogleDigitalZoom();
        bool TooglePtz();
        bool ToogleSequencing(bool value);
        void UnsubcribePTZEvent();
        void SubcribePTZEvent();
        bool ToggleInstantPlayback();
        bool ToggleTalk(bool talkStatus);

        IOPortState InputPortState();
        IOPortState OuputPortState();
        void OuputPortChangeState(IOPortState state);
    }
}

