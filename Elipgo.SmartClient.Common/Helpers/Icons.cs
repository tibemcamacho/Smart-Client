using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using System.Drawing;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class Icons
    {
        public static Image GetDefaultIconForButton(ButtonsContextBar button)
        {
            Image response = null;
            switch (button)
            {
                case ButtonsContextBar.Fullscreen:
                    response = FileResources.icon_full_screen;
                    break;
                case ButtonsContextBar.Snapshot:
                    response = FileResources.icon_snapshot;
                    break;
                case ButtonsContextBar.Videoclip:
                    response = FileResources.icon_recorder;
                    break;
                case ButtonsContextBar.Talk:
                    response = FileResources.icon_micr_off;
                    break;
                case ButtonsContextBar.Listen:
                    response = FileResources.icon_sound_off;
                    break;
                case ButtonsContextBar.CreatePreset:
                    response = FileResources.icon_presets_new;
                    break;
                case ButtonsContextBar.Presets:
                    response = FileResources.icon_presets;
                    break;
                case ButtonsContextBar.Guards:
                    response = FileResources.icon_guards;
                    break;
                case ButtonsContextBar.Ptz:
                    response = FileResources.icon_ptz;
                    break;
                case ButtonsContextBar.Bookmark:
                    response = FileResources.icon_bookmarks;
                    break;
            }

            return response;
        }
    }
}
