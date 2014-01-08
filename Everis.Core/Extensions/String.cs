
using System;
namespace Everis.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the format item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="value">The string instance to format.</param>
        /// <param name="arguments">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of this string instance in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="arguments" />.</returns>
        public static string FormatWith(this string value, params object[] parameters)
        {
            return string.Format(value, parameters);
        }

    }
}
