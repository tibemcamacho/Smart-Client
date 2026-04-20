namespace Elipgo.SmartClient.Common.Helpers
{
    public static class StringHelper
    {
        public static string Truncate(string text, int maxLength, string suffix = "...")
        {
            string str = text;
            if (maxLength > 0)
            {
                int length = maxLength - suffix.Length;
                if (length <= 0)
                {
                    return str;
                }
                if ((text != null) && (text.Length > maxLength))
                {
                    return text.Substring(0, length).TrimEnd(new char[0]) + suffix;
                }
            }
            return str;

        }
    }
}
