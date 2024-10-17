namespace NotificationCommonLibrary.Extensions
{
    public static class StringExtension
    {
        public static string Begining(this string str, int length)
        {
            if (str.Length <= length)
            {
                return str;
            }
            else
            {
                return str.Substring(0, length);
            }
        }

        public static int? ParseToInt(this string? str)
        {
            if (int.TryParse(str, out int res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public static double? ParseToDouble(this string? str)
        {
            if (double.TryParse(str.Replace('.', ','), out double res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public static decimal? ParseToDecimal(this string? str)
        {
            if (decimal.TryParse(str?.Replace('.', ','), out decimal res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? ParseToDate(this string? str)
        {
            if (DateTime.TryParse(str, out var res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public static T ParseToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }

        public static string ReflectionFormat(this string str, object values)
        {
            foreach (var property in values.GetType().GetProperties())
            {
                var key = property.Name;
                var value = property.GetValue(values);

                str = str.Replace("{" + key + "}", value?.ToString() ?? "");
            }

            return str;
        }

        public static string IfNullOrWhiteSpace(this string? str, string replacement)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return replacement;
            }
            else
            {
                return str;
            }
        }

        public static string? NullIfEmpty(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            else
            {
                return str;
            }
        }
    }
}
