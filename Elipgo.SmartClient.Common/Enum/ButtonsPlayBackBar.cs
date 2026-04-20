using Elipgo.SmartClient.Common.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elipgo.SmartClient.Common.Enum
{

    public enum ButtonsPlayBackBar
    {
        [Display(Name = Constants.NAME_BOOKMARK)]
        [Description(Constants.DESCRIPTION_BOOKMARK)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_BOOKMARK_EN)]
        Bookmark,
        Rewind,
        Play,
        Pause,
        Stop,
        Slow,
        Fast,
        RewSecs,
        FwdSecs
    }

    public enum PlayScaleTimeLine
    {
        Normal = 1,
        m15 = 2,
        m10 = 3,
        m5 = 6,

        x1,  
        x1_2,  
        x1_3, 
        x1_6 
    }
}
