using System;

namespace Peak.Speedoku.Scripts.Common.Extensions
{
    /// <summary>
    /// Extensions for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// (TEnum) Enum.Parse(typeof(TEnum), value)
        /// </summary>
        public static TEnum ParseToEnum<TEnum>(this string value)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
        /// <summary>
        /// (TEnum) Enum.Parse(typeof(TEnum), value)
        /// </summary>
        public static int ParseToInt(this string value)
        {
            return int.Parse(value);
        }
    }
}