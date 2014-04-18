using System;
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
