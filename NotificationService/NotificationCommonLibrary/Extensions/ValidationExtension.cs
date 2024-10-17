namespace NotificationCommonLibrary.Extensions
{
    public static class ValidationExtension
    {
        public static string ValidateSize(this string? value, string name, int? minSize, int? maxSize)
        {
            if (value == null)
            {
                throw new Exception($"Поле \"{name}\" не может быть пустым.");
            }

            if (minSize != null && value.Length < minSize)
            {
                throw new Exception($"Поле \"{name}\" меньше требуемого размера. Длинна поля должна быть от {minSize ?? 0} до {maxSize ?? 1000000} символов.");
            }

            if (maxSize != null && value.Length > maxSize)
            {
                throw new Exception($"Поле \"{name}\" больше требуемого размера. Длинна поля должна быть от {minSize ?? 0} до {maxSize ?? 1000000} символов.");
            }

            return value;
        }

        public static T ValidateNotNull<T>(this T? value, string name) where T : class
        {
            if (value == null)
            {
                throw new Exception($"Поле \"{name}\" не может быть пустым.");
            }

            return value;
        }

        public static T ValidateNotNull<T>(this T? value, string name) where T : struct
        {
            if (value == null)
            {
                throw new Exception($"Поле \"{name}\" не может быть пустым.");
            }

            return value.Value;
        }
    }
}
