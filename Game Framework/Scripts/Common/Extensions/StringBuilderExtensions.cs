using System.Text;

namespace Peak.Speedoku.Scripts.Common.Extensions
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Clears the content of the StringBuilder. Does not change capacity.
        /// </summary>
        public static StringBuilder Clear(this StringBuilder stringBuilder)
        {
            return stringBuilder.Remove(0, stringBuilder.Length);
        }
    }
}