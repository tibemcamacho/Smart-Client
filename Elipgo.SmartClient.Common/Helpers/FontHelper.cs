using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Text;

namespace Elipgo.SmartClient.Common.Helpers
{
    public enum FontSizes
    {
        Small_0 = 6,
        Small_1 = 7,
        Small_2 = 8,
        Small_3 = 9,
        Small_4 = 10,
        Small_5 = 11,
        Small_6 = 12,
        Small_7 = 13,
        Small_8 = 14,
        Small_9 = 16,
        Small_10 = 30,

        Medium_0 = 8,
        Medium_1 = 12,
        Medium_2 = 14,
        Medium_3 = 16,
        Medium_4 = 18,
        Medium_5 = 32,

        Large_0 = 20,
        Large_1 = 24,
        Large_2 = 26,
        Large_3 = 28,
        Large_4 = 30,
        Large_5 = 44,
    }

    public static class FontHelper
    {
        public static PrivateFontCollection PFC { get; } = new PrivateFontCollection();

        public static Font Get(FontSizes size, FontName fontName, FontStyle fontStyle = FontStyle.Regular, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {

            foreach (var item in PFC.Families)
            {
                if (item.Name.ToLower() == fontName.GetAttribute<DisplayAttribute>().Name)
                {
                    return new Font(item, (int)size, fontStyle, graphicsUnit);
                }
            }

            return PFC.Families.Length > 0 ? new Font(PFC.Families[0], (int)size, fontStyle, graphicsUnit) : LoadS(size, fontStyle, graphicsUnit);

        }

        public static void Load()
        {
            foreach (FontName font in System.Enum.GetValues(typeof(FontName)))
            {
                PFC.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + font.GetDescription());
            }
        }

        public static Font LoadS(FontSizes size, FontStyle fontStyle = FontStyle.Regular, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            Load();
            return new Font(PFC.Families[0], (int)size, fontStyle, graphicsUnit);
        }

        public static Font GetRobotoRegular(FontSizes size, FontStyle fontStyle, GraphicsUnit graphicsUnit)
        {
            if (PFC.Families.Length > 0)
            {
                return new Font(PFC.Families[0], (int)size, fontStyle, graphicsUnit);
            }
            else
            {
                AddFonts();
                return new Font(PFC.Families[0], (int)size, fontStyle, graphicsUnit);
            }
        }

        private static void AddFonts()
        {
            PFC.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + VariableResources.PATH_FONT_ROBOTO_REGULAR);
        }
    }
}
