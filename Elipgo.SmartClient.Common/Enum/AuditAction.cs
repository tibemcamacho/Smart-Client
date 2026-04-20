using System.ComponentModel.DataAnnotations;

namespace Elipgo.SmartClient.Common.Enum
{
    public enum AuditAction
    {
        [Display(Name = "Create Bookmark")]
        CREATE_BOOKMARK,
        [Display(Name = "Create SnapShot")]
        CREATE_SNAPSHOT,
        [Display(Name = "Create VideoClip")]
        CREATE_VIDEOCLIP,
        [Display(Name = "Delete Camera in Grilla")]
        DELETE_CAMERA_IN_GRILLA,
        [Display(Name = "Start Guard")]
        START_GUARD,
        [Display(Name = "Stop Guard")]
        STOP_GUARD,
        [Display(Name = "Save Preset")]
        SAVE_PRESET,
        [Display(Name = "View Alarms")]
        VIEW_ALARMS,
        [Display(Name = "Diagnostic Alarms")]
        DIAGNOSTIC_ALARMS,
        [Display(Name = "Discard alarms")]
        DISCARD_ALARMS,
        [Display(Name = "View Carousels")]
        VIEW_CAROUSELS,
        [Display(Name = "Export BookMark")]
        EXPORT_BOOKMARK,
        [Display(Name = "Out Of LineBackend")]
        OUTOFLINE_BACKEND,
        [Display(Name = "Change Password")]
        CHANGE_PASSWORD,
        [Display(Name = "Live view")]
        LIVE_VIEW,
        [Display(Name = "PlayBack")]
        PLAYBACK,
        [Display(Name = "Exec Scene")]
        EXEC_SCENE,
        [Display(Name = "LogOut")]
        LOGOUT,
        [Display(Name = "View camera live")]
        LIVE_VIEW_CAMERA_IN_GRILLA,
        [Display(Name = "View camara playback")]
        PLAYBACK_VIEW_CAMERA_IN_GRILLA

    }
}
