// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStringDictionary.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the QueryStringDictionary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using DowJones.Encoders;

namespace DowJones.Url
{
    public class QueryStringDictionary : Dictionary<string, string>
    {
        private readonly IEncoder _encoder;

        public QueryStringDictionary()
            : base(StringComparer.CurrentCultureIgnoreCase)
        {
            _encoder = new DefaultEncoder();
        }

        public QueryStringDictionary(string query, IEncoder encoder)
            : base(StringComparer.CurrentCultureIgnoreCase)
        {
            _encoder = encoder;
            Initialise(query);
        }

        private void Initialise(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return;
            }

            Clear(); //// clear the collection
            query = query.Substring(1); //// remove the leading '?'

            query = HttpUtility.UrlDecode(query); //// not actually necessary if using a  "~" prefix

            if (!string.IsNullOrEmpty(_encoder.Prefix) && query.StartsWith(_encoder.Prefix))
            {
                query = query.Substring(_encoder.Prefix.Length);
                query = _encoder.Decode(query);
            }

            var pairs = query.Split(new[] { '&' });
            foreach (var s in pairs)
            {
                var pair = s.Split(new[] { '=' });
                this[pair[0]] = (pair.Length > 1) ? pair[1] : string.Empty;
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="queryStringDictionary">The query string dictionary.</param>
        public void AddRange(QueryStringDictionary queryStringDictionary)
        {
            foreach (var pair in queryStringDictionary)
            {
                this[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        new public string ToString()
        {
            return _ToString("e", false);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return _ToString(format, false);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="basicUtf8UrlEncoding">if set to <c>true</c> [basic UTF8 URL encoding].</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        internal string ToString(string format, bool basicUtf8UrlEncoding)
        {
            return _ToString(format, basicUtf8UrlEncoding);
        }

        private string _ToString(string format, bool basicUtf8UrlEncoding)
        {
            if (Keys.Count == 0)
            {
                return string.Empty;
            }


            var pairs = new List<string>();
            foreach (var s in Keys)
            {
                string val;
                if (!TryGetValue(s, out val))
                {
                    continue;
                }

                pairs.Add(basicUtf8UrlEncoding ? string.Concat(s, "=", HttpUtility.UrlEncode(val)) : string.Concat(s, "=", HttpUtility.UrlEncode(val).Replace("+", "%20")));
            }

            var qs = string.Join("&", pairs.ToArray());

            switch (format)
            {
                case "e": //// encoded
                    return string.Concat(_encoder.Prefix, _encoder.Encode(qs));
                case "p": //// plaintext
                    return qs;
                default:
                    throw new FormatException();
            }
        }

        #region Legacy Append Methods

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Append(string value)
        {
            this[value] = string.Empty;
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value.Trim()))
            {
                this[name] = value;
            }
            else
            {
                this[name] = string.Empty;
            }
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, int value)
        {
            Append(name, value.ToString());
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, double value)
        {
            Append(name, value.ToString());
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Append(string name, float value)
        {
            Append(name, value.ToString());
        }


        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void Append(string name, bool value)
        {
            if (string.IsNullOrEmpty(name)) return;
            this[name] = (value) ? 1.ToString() : 0.ToString();
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        internal static void Append(string name, int[] values)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (values == null || values.Length <= 0)
            {
                return;
            }

            var tValues = Array.ConvertAll(values, i => i.ToString());
            if (tValues.Length > 0)
            {
                Append(name, values);
            }
        }

        /// <summary>
        /// Appends the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public void Append(string name, string[] values)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (!IsValid(values))
            {
                return;
            }

            //// string[] tValues = Array.ConvertAll<string, string>(values, delegate(string s) { return (_basicUtf8UrlEncoding) ? HttpUtility.UrlEncode(s) : HttpUtility.UrlEncode(s).Replace("+", "%20"); });
            if (values.Length > 0)
            {
                this[name] = string.Join(",", values);
            }
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="objs">The array of objects.</param>
        /// <returns>The is valid.</returns>
        internal static bool IsValid(object[] objs)
        {
            return objs != null && objs.Length != 0;
        }

        #endregion
    }
}