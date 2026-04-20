using Elipgo.SmartClient.Common.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elipgo.SmartClient.Common.Enum
{
    public enum FontName
    {
        [Display(Name = Constants.DISPLAY_NAME_FONT_ROBOTO_BOLD)]
        [Description(Constants.PATH_FONT_ROBOTO_BOLD)]
        ROBOTO_BOLD,

        [Display(Name = Constants.DISPLAY_NAME_FONT_ROBOTO_LIGHT)]
        [Description(Constants.PATH_FONT_ROBOTO_LIGHT)]
        ROBOTO_LIGHT,

        [Display(Name = Constants.DISPLAY_NAME_FONT_ROBOTO_MEDIUM)]
        [Description(Constants.PATH_FONT_ROBOTO_MEDIUM)]
        ROBOTO_MEDIUM,

        [Display(Name = Constants.DISPLAY_NAME_FONT_ROBOTO_REGULAR)]
        [Description(Constants.PATH_FONT_ROBOTO_REGULAR)]
        ROBOTO_REGULAR
    }
}
