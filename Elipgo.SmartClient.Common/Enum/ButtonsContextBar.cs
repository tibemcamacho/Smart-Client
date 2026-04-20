
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elipgo.SmartClient.Common.Enum
{
    public class DescriptionEN : Attribute
    {
        public string Descripcion { get; set; }

    }
    public class Permission : Attribute
    {
        public string PermissionKey { get; set; }
    }
    public class PermissionLive : Attribute
    {
        public string PermissionKey { get; set; }
    }
    public class PermissionPlayback : Attribute
    {
        public string PermissionKey { get; set; }


    }
    public class PermissionFullScreen : Attribute
    {
        public string PermissionKey { get; set; }


    }
    public class PermissionAlarm : Attribute
    {
        public string PermissionKey { get; set; }


    }
    public class PermissionVisualSearch : Attribute
    {
        public string PermissionKey { get; set; }


    }
    public enum ButtonsContextBar
    {
        [Display(Name = Constants.NAME_NONE)]
        [Description(Constants.DESCRIPTION_NONE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_NONE_EN)]
        None,
        [Display(Name = Constants.NAME_EDIT)]
        [Description(Constants.DESCRIPTION_EDIT)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_EDIT_EN)]
        Edit,
        [Display(Name = Constants.NAME_DELETE)]
        [Description(Constants.DESCRIPTION_DELETE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DELETE_EN)]
        Delete,
        [Display(Name = Constants.NAME_REMOVE)]
        [Description(Constants.DESCRIPTION_REMOVE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_REMOVE_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.removeCameraGrid")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.removeCameraGrid")]
        Remove,
        [Display(Name = Constants.NAME_SNAPSHOT)]
        [Description(Constants.DESCRIPTION_SNAPSHOT)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_SNAPSHOT_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.snapshot")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.snapshot")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.snapshot")]
        Snapshot,
        [Display(Name = Constants.NAME_VIDEOCLIP)]
        [Description(Constants.DESCRIPTION_VIDEOCLIP)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_VIDEOCLIP_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.videoclip")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.videoclip")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.videoclip")]
        Videoclip,
        [Display(Name = Constants.NAME_TALK)]
        [Description(Constants.DESCRIPTION_TALK)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_TALK_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.talk")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.talk")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.talk")]
        Talk,
        [Display(Name = Constants.NAME_LISTEN)]
        [Description(Constants.DESCRIPTION_LISTEN)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LISTEN_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.listen")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.listen")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.listen")]
        Listen,
        [Display(Name = Constants.NAME_PRESETS)]
        [Description(Constants.DESCRIPTION_PRESETS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_PRESETS_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.presets")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.presets")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.Presets")]
        Presets,
        [Display(Name = Constants.NAME_GUARDS)]
        [Description(Constants.DESCRIPTION_GUARDS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_GUARDS_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.guards")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.guards")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.guards")]
        Guards,
        [Display(Name = Constants.NAME_PTZ)]
        [Description(Constants.DESCRIPTION_PTZ)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_PTZ_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.usePTZ")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.usePTZ")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.usePTZ")]
        Ptz,
        [Display(Name = Constants.NAME_SEQUENCING)]
        [Description(Constants.DESCRIPTION_SEQUENCING)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_SEQUENCING_EN)]
        //[PermissionLive(PermissionKey = "auth.app.apps.live.usePTZ")]
        //[PermissionPlayback(PermissionKey = "auth.app.apps.playback.usePTZ")]
        //[PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.usePTZ")]
        Sequencing,
        [Display(Name = Constants.NAME_BOOKMARK)]
        [Description(Constants.DESCRIPTION_BOOKMARK)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_BOOKMARK_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.createBookmark")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.createBookmark")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.createBookmark")]
        Bookmark,
        [Display(Name = Constants.NAME_FULLSCREEN)]
        [Description(Constants.DESCRIPTION_FULLSCREEN)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_FULLSCREEN_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.fullscreen")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.fullscreen")]
        Fullscreen,
        [Display(Name = Constants.NAME_ACTIVECAROUSEL)]
        [Description(Constants.DESCRIPTION_ACTIVECAROUSEL)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_ACTIVECAROUSEL_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.listCarousels")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.listCarousels")]
        ActiveCarousel,
        [Display(Name = Constants.NAME_OUTPUTTOGGLESWITCH)]
        [Description(Constants.DESCRIPTION_OUTPUTTOGGLESWITCH)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_OUTPUTTOGGLESWITCH_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.outputtoggleswitch")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.outputtoggleswitch")]
        OutputToggleSwitch,
        [Display(Name = Constants.NAME_INPUTSTATE)]
        [Description(Constants.DESCRIPTION_INPUTSTATE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_INPUTSTATE_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.inputstate")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.inputstate")]
        InputState,
        [Display(Name = Constants.NAME_INSTANTPLAYBACK)]
        [Description(Constants.DESCRIPTION_INSTANTPLAYBACK)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_INSTANTPLAYBACK_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.instantPlayback")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.instantPlayback")]
        [PermissionVisualSearch(PermissionKey = "auth.app.apps.visualsearch.instantPlayback")]
        InstantPlayback,
        [Display(Name = Constants.NAME_DIGITALZOOM)]
        [Description(Constants.DESCRIPTION_DIGITALZOOM)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DIGITALZOOM_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.digitalzoom")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.digitalzoom")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.digitalzoom")]
        DigitalZoom,
        [Display(Name = Constants.NAME_SELECTSTREAM)]
        [Description(Constants.DESCRIPTION_SELECTSTREAM)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_SELECTSTREAM_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.changeStreamFlow")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.changeStreamFlow")]
        SelectStream,
        [Display(Name = Constants.NAME_ADDPRESET)]
        [Description(Constants.DESCRIPTION_ADDPRESET)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_ADDPRESET_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.createPresets")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.createPresets")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.Presets")]
        CreatePreset,
        [Display(Name = Constants.NAME_GRIDCLEAR)]
        [Description(Constants.DESCRIPTION_GRIDCLEAR)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_GRIDCLEAR_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.cleanGrid")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.cleanGrid")]
        GridCleaner,
        [Display(Name = Constants.NAME_GROUP)]
        [Description(Constants.DESCRIPTION_GROUP)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_GROUP_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.listGroups")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.listGroups")]
        Group,
        [Display(Name = Constants.NAME_GRID)]
        [Description(Constants.DESCRIPTION_GRID)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_GRID_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.changeGrid")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.changeGrid")]
        Grid,
        [Display(Name = Constants.NAME_SCENE)]
        [Description(Constants.DESCRIPTION_SCENE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_SCENE_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.listScenes")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.listScenes")]
        Scene,
        [Display(Name = Constants.NAME_IODESCRIPTION)]
        [Description(Constants.DESCRIPTION_IODESCRIPTION)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_IODESCRIPTION_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.IoDescription")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.IoDescription")]
        IoDescription,
        [Display(Name = Constants.NAME_PLAY)]
        [Description(Constants.DESCRIPTION_PLAY)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_PLAY_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.Play")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.Play")]
        Play,
        [Display(Name = Constants.NAME_FWDSECS)]
        [Description(Constants.DESCRIPTION_FWDSECS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_FWDSECS_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.FwdSecs")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.FwdSecs")]
        FwdSecs,
        [Display(Name = Constants.NAME_REWSECS)]
        [Description(Constants.DESCRIPTION_REWSECS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_REWSECS_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.RewSecs")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.RewSecs")]
        RewSecs,
        [Display(Name = Constants.NAME_PAUSE)]
        [Description(Constants.DESCRIPTION_PAUSE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_PAUSE_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.Pause")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.Pause")]
        Pause,
        [Display(Name = Constants.NAME_SLOW)]
        [Description(Constants.DESCRIPTION_SLOW)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_SLOW_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.Slow")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.Slow")]
        Slow,
        [Display(Name = Constants.NAME_FAST)]
        [Description(Constants.DESCRIPTION_FAST)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_FAST_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.Fast")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.Fast")]
        Fast,
        [Display(Name = Constants.NAME_REW)]
        [Description(Constants.DESCRIPTION_REW)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_REW_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.Rew")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.Rew")]
        Rew,
        [Display(Name = Constants.NAME_CALENDAR)]
        [Description(Constants.DESCRIPTION_CALENDAR)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_CALENDAR_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.Calendar")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.Calendar")]
        Calendar,
        [Display(Name = Constants.NAME_DEFAULTSIDETEXT)]
        [Description(Constants.DESCRIPTION_DEFAULTSIDETEXT)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DEFAULTSIDETEXT_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.defaultSiteText")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.defaultSiteText")]
        defaultSiteText,
        [Description(Constants.DESCRIPTION_DEVICES)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DEVICES_EN)]
        Devices,
        [Display(Name = Constants.NAME_CHANGEPASSWORD)]
        [Description(Constants.DESCRIPTION_CHANGEPASSWORD)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_CHANGEPASSWORD_EN)]
        [Permission(PermissionKey = "auth.app.apps.changePassword")]
        ChangePassword,
        [PermissionLive(PermissionKey = "auth.app.apps.live.createGroups")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.createGroups")]
        CreateGroup,
        [PermissionLive(PermissionKey = "auth.app.apps.live.deleteGroups")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.deleteGroups")]
        DeleteGroup,
        [PermissionLive(PermissionKey = "auth.app.apps.live.modifyGroups")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.modifyGroups")]
        EditGroup,
        [PermissionLive(PermissionKey = "auth.app.apps.live.createScenes")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.createScenes")]
        CreateScene,
        [PermissionLive(PermissionKey = "auth.app.apps.live.deleteScenes")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.deleteScenes")]
        DeleteScene,
        [PermissionLive(PermissionKey = "auth.app.apps.live.modifyScenes")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.modifyScenes")]
        EditScene,
        [PermissionLive(PermissionKey = "auth.app.apps.live.createCarousels")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.createCarousels")]
        CreateCarousel,
        [PermissionLive(PermissionKey = "auth.app.apps.live.deleteCarousels")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.deleteCarousels")]
        DeleteCarousel,
        [PermissionLive(PermissionKey = "auth.app.apps.live.modifyCarousels")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.modifyCarousels")]
        EditCarousel,
        [PermissionLive(PermissionKey = "auth.app.apps.live.deletePresets")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.deletePreset")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.deletePresets")]
        DeletePresets,
        [PermissionLive(PermissionKey = "auth.app.apps.live.modifyPresets")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.modifyPresets")]
        [PermissionFullScreen(PermissionKey = "auth.app.apps.fullscreen.modifyPresets")]
        ModifyPresets,
        [PermissionLive(PermissionKey = "auth.app.apps.live.deleteGuard")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.deleteGuard")]
        DeleteGuard,
        [PermissionLive(PermissionKey = "auth.app.apps.live.modifyGuard")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.modifyGuard")]
        EditGuard,
        [PermissionLive(PermissionKey = "auth.app.apps.live.createGuard")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.createGuard")]
        CreateGuard,
        [Display(Name = Constants.NAME_PLAYBACKSYNCAMERA)]
        [Description(Constants.DESCRIPTION_PLAYBACKSYNCAMERA)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_PLAYBACKSYNCAMERA_EN)]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.PlaybackSync")]
        PlaybackSyncCamera,
        [Display(Name = Constants.NAME_DIAGNOSEALARMS)]
        [Description(Constants.DESCRIPTION_DIAGNOSEALARMS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DIAGNOSEALARMS_EN)]
        [PermissionAlarm(PermissionKey = "auth.app.apps.alarm.diagnoseAlarms")]
        DiagnoseAlarm,
        [Display(Name = Constants.NAME_DISCARDALARMS)]
        [Description(Constants.DESCRIPTION_DISCARDALARMS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DISCARDALARMS_EN)]
        [PermissionAlarm(PermissionKey = "auth.app.apps.alarm.discardAlarms")]
        DiscardAlarm,
        [Display(Name = Constants.NAME_DISCARDALLALARMS)]
        [Description(Constants.DESCRIPTION_DISCARDALLALARMS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DISCARDALLALARMS_EN)]
        [PermissionAlarm(PermissionKey = "auth.app.apps.alarm.discardAllAlarms")]
        DiscardAllAlarms,
        [Display(Name = Constants.NAME_REFRESH)]
        [Description(Constants.DESCRIPTION_REFRESH)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_REFRESH_EN)]
        Refresh,
        [Display(Name = Constants.NAME_SEARCH)]
        [Description(Constants.DESCRIPTION_SEARCH)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_SEARCH_EN)]
        Search,
        [Display(Name = Constants.NAME_DOWNLOAD)]
        [Description(Constants.DESCRIPTION_DOWNLOAD)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DOWNLOAD_EN)]
        Download,
        [Display(Name = Constants.NAME_VERBOOKMARK)]
        [Description(Constants.DESCRIPTION_VIEW_BOOKMARK)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_VIEW_BOOKMARK_EN)]
        VerBookMark,
        [Display(Name = Constants.NAME_RECORDER)]
        [Description(Constants.DESCRIPTION_VIEW_RECORDER)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_VIEW_RECORDER_EN)]
        Recorder,
        [Display(Name = Constants.NAME_VIEWMINUTES)]
        [Description(Constants.DESCRIPTION_VIEW_MINUTES)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_VIEW_MINUTES_EN)]
        ViewMinutes,
        [Display(Name = Constants.NAME_VIEWHOURS)]
        [Description(Constants.DESCRIPTION_VIEW_HOURS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_VIEW_HOURS_EN)]
        ViewHours,
        [Display(Name = Constants.NAME_VIEWSECONDS)]
        [Description(Constants.DESCRIPTION_VIEW_SECONDS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_VIEW_SECONDS_EN)]
        ViewSeconds,
        [Display(Name = Constants.NAME_VISUALSEARCH)]
        [Description(Constants.DESCRIPTION_VISUALSEARCH)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_VISUALSEARCH_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.visualsearch")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.visualsearch")]
        VisualSearch,
        [Display(Name = Constants.NAME_SPEED)]
        [Description(Constants.DESCRIPTION_NAME_SPEED)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_SPEED_EN)]
        Speed,
        [Display(Name = Constants.NAME_TALK)]
        [Description(Constants.DESCRIPTION_TALK)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_TALK_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.TalkAll")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.TalkAll")]
        TalkAll,
        [Display(Name = Constants.NAME_FOLDERBROWSER)]
        [Description(Constants.DESCRIPTION_FOLDERBROWSER)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_FOLDERBROWSER_EN)]
        FolderBrowser,
        [Display(Name = Constants.NAME_DEVICESTATUS)]
        [Description(Constants.DESCRIPTION_DEVICESTATUS)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DEVICESTATUS_EN)]
        DeviceStatus,
        [Display(Name = Constants.NAME_DIGITALZOOM)]
        [Description(Constants.DESCRIPTION_DIGITALZOOM)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_DIGITALZOOM_EN)]
        [PermissionLive(PermissionKey = "auth.app.apps.live.digitalzoomAll")]
        [PermissionPlayback(PermissionKey = "auth.app.apps.playback.digitalzoomAll")]
        DigitalZoomAll
    }
}
