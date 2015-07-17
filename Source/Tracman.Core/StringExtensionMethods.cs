using System;
using System.Globalization;

namespace Tracman.Core
{
    public static class StringExtensionMethods
    {
        public static string FormatWith(this string value, params object[] args)
        {
            if (value == null) throw new ArgumentNullException("value");

            return string.Format(CultureInfo.InvariantCulture, value, args);
        }
    }
}