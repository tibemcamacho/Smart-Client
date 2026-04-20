using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class EnumExtension
    {
        public static string GetDescription(this System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static TAttribute GetAttribute<TAttribute>(this System.Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static String convertToString(this System.Enum eff)
        {
            return System.Enum.GetName(eff.GetType(), eff);
        }

        public static T GetEnumFromDescription<T>(this string description) where T : System.Enum
        {
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                var fieldInfo = typeof(T).GetField(value.ToString());
                var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                if (attribute != null && attribute.Description == description)
                {
                    return value;
                }
            }

            return default;
        }

        public static string GetDisplayName(Enum.AlarmType value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DisplayAttribute>();
            return attr?.GetName() ?? value.ToString();
        }

        public static string GetTranslation(Enum.AlarmType value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<TranslationAttribute>();
            return attr?.GetValue() ?? value.ToString();
        }
    }
}
