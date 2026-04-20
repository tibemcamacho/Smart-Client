using System.Linq;
using System.Runtime.Serialization;

namespace Elipgo.SmartClient.Common.Enum
{
    public static class Joystick
    {
        public static string GetEnumMemberAttrValue<T>(T enumVal)
        {
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(enumVal.ToString());
            var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            if (attr != null)
            {
                return attr.Value;
            }

            return null;
        }
    }

    public enum JoystickHikvision
    {
        [EnumMember(Value = "3F3F001900000000000000000000000000002003100000003F")]
        Signal,
        [EnumMember(Value = "00000000")]
        CeroValue
    }
    public enum ButtonJoystickHikvision
    {
        [EnumMember(Value = "1061109567")]
        ButtonF,
        [EnumMember(Value = "F1")]
        F01,
        F02 = F01,
        [EnumMember(Value = "F2")]
        F03,
        F04 = F03,
        [EnumMember(Value = "F3")]
        F05,
        F06 = F05
    }

    public enum Buttons
    {
        F1,
        F2,
        F3
    }
}
