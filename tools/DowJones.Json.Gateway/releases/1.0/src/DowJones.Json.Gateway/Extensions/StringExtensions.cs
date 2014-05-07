using System;
using System.Globalization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Extensions
{
    public static class StringExtentions
    {

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
        };

        /// <summary>
        /// Determines whether the specified string is null or empty.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns>
        /// <c>true</c> if the specified value is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Determines whether the specified expression is numeric.(http://support.microsoft.com/default.aspx?scid=kb;en-us;329488)
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// <c>true</c> if the specified expression is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string expression)
        {
            // Variable to collect the Return value of the TryParse method.

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            var isNum = Double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        /// <summary>
        /// Determines whether the specified string is not null or empty.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns>
        /// <c>true</c> if [is not empty] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotEmpty(this string value)
        {
            return value.IsNullOrEmpty() == false;
        }

        /// <summary>
        /// Checks whether the string is empty and returns a default value in case.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Either the string or the default value.</returns>
        public static string IfEmpty(this string value, string defaultValue)
        {
            return value.IsNotEmpty() ? value : defaultValue;
        }

        /// <summary>
        /// Formats the value with the parameters using string.Format.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatWith(this string value, params object[] parameters)
        {
            return string.Format(value, parameters);
        }

        /// <summary>
        /// Serializes object to json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="indented">whether or not to indent the data</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool indented = false)
        {
            var r = JsonConvert.SerializeObject(obj, 
                indented ? Formatting.Indented : Formatting.None, 
                JsonSerializerSettings);

            if (r == "[]" || r == "{}")
            {
                return String.Empty;
            }
            return r;
        }
    }
}
