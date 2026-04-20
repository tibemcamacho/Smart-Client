using System.ComponentModel.DataAnnotations;

namespace Elipgo.SmartClient.Common.Enum
{
    public enum ActionScenes
    {
        [Display(Name = "turnOff")]
        TurnOff,

        [Display(Name = "turnOn")]
        TurnOn,

        [Display(Name = "pulseOn05Seconds")]
        PulseOn05Seconds,

        [Display(Name = "pulseOn3Seconds")]
        PulseOn3Seconds,

        [Display(Name = "pulseOn10Seconds")]
        PulseOn10Seconds,

        [Display(Name = "pulseOn30Seconds")]
        PulseOn30Seconds
    }
}
