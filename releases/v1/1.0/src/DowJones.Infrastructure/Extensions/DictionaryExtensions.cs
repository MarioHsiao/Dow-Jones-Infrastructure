// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Contains extension methods of IDictionary&lt;string, objectT&gt;.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace DowJones.Extensions
{
    /// <summary>
    /// Contains extension methods of IDictionary&lt;string, objectT&gt;.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key string.</param>
        /// <param name="value">The base value.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        public static void Merge(this IDictionary<string, object> instance, string key, object value, bool replaceExisting)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNullOrEmpty(key, "key");
            Guard.IsNotNull(value, "value");

            if (replaceExisting || !instance.ContainsKey(key))
            {
                instance[key] = value;
            }
        }

        public static bool IsNullOrEmpty(this IDictionary<string, object> instance)
        {
            return instance == null || instance.Count <= 0;
        }

        /// <summary>
        /// Appends the in value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key string.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="value">The base value.</param>
        public static void AppendInValue(this IDictionary<string, object> instance, string key, string separator, object value)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNullOrEmpty(key, "key");
            Guard.IsNotNullOrEmpty(separator, "separator");
            Guard.IsNotNull(value, "value");

            instance[key] = instance.ContainsKey(key) ? instance[key] + separator + value : value.ToString();
        }

        public static void AddStyleAttribute(this IDictionary<string, object> instance, string key, string value)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNullOrEmpty(key, "key");
            Guard.IsNotNull(value, "value");

            string style = string.Empty;

            if (instance.ContainsKey("style"))
            {
                style = (string)instance["style"];
            }

            var builder = new StringBuilder(style);
            builder.Append(key);
            builder.Append(":");
            builder.Append(value);
            builder.Append(";");

            instance["style"] = builder.ToString();
        }

        /// <summary>
        /// Appends the specified value at the beginning of the existing value
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key string.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="value">The base value.</param>
        public static void PrependInValue(this IDictionary<string, object> instance, string key, string separator, object value)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNullOrEmpty(key, "key");
            Guard.IsNotNullOrEmpty(separator, "separator");
            Guard.IsNotNull(value, "value");

            instance[key] = instance.ContainsKey(key) ? value + separator + instance[key] : value.ToString();
        }

        /// <summary>
        /// Toes the attribute string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>A attribute string </returns>
        public static string ToAttributeString(this IDictionary<string, object> instance)
        {
            Guard.IsNotNull(instance, "instance");

            var attributes = new StringBuilder();

            foreach (var attribute in instance)
            {
                attributes.Append(" {0}=\"{1}\"".FormatWith(HttpUtility.HtmlAttributeEncode(attribute.Key), HttpUtility.HtmlAttributeEncode(attribute.Value.ToString())));
            }

            return attributes.ToString();
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="from">the From object.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        public static void Merge<TValue>(this IDictionary<string, TValue> instance, IEnumerable<KeyValuePair<string, TValue>> from, bool replaceExisting = true)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNull(from, "from");

            foreach (var pair in from.Where(pair => replaceExisting || !instance.ContainsKey(pair.Key)))
            {
                instance[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        [DebuggerStepThrough]
        public static void Merge(this IDictionary<string, object> instance, object values, bool replaceExisting = true)
        {
            Merge(instance, new RouteValueDictionary(values), replaceExisting);
        }
    }
}
