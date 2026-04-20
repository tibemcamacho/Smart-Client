using System.ComponentModel.DataAnnotations;

namespace Elipgo.SmartClient.SignalR.Connection
{
    public enum EventTypes
    {
        [Display(Name = "alarms")]
        Alarms,
        [Display(Name = "lpr")]
        LPR,
        [Display(Name = "faceRecognition")]
        FaceRecognition,
        [Display(Name = "avl")]
        AVL,
        [Display(Name = "security")]
        Security,
        [Display(Name = "cameras")]
        Cameras,
        [Display(Name = "general")]
        General,
        [Display(Name = "iot")]
        Iot,
        [Display(Name = "logoutUser")]
        LogoutUser,
        [Display(Name = "refreshAlarms")]
        RefreshAlarms,
        [Display(Name = "VmsCreated")]
        VmsCreated,
        [Display(Name = "camerasState")]
        CamerasState

    }
}
