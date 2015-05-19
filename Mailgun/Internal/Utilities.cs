using System.ComponentModel;

namespace Mailgun.Internal
{
    static class Utilities
    {
        /// <summary>
        /// Gets the string value for an enum.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumStringValue<TEnum>(TEnum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
