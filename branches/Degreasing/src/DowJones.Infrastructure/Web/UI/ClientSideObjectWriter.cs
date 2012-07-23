// (c) Some parts Copyright 2002-2010 Telerik 
// This source is subject to the GNU General Public License, version 2
// See http://www.gnu.org/licenses/gpl-2.0.html. 
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.UI
{
    /// <summary>
    /// Class used to build initialization script of jQuery plug-in.
    /// </summary>
    public class ClientSideObjectWriter : IClientSideObjectWriter
    {
        // TODO: Move to resources
        private const string YouMustHaveToCallStartPriorCallingThisMethod = "YouMustHaveToCallStartPriorCallingThisMethod";
        private const string YouCannotCallStartMoreThanOnce = "YouCannotCallStartMoreThanOnce";

        private readonly string _id;
        private readonly string _type;
        private readonly TextWriter _writer;
        private readonly JsonSerializer _jsonWriter;

        private bool _hasStarted;
        private bool _appended;


        [Inject("Optional")]
        protected CultureInfo Culture
        {
            get { return _culture ?? CultureInfo.InvariantCulture; }
            set { _culture = value; }
        }
        private CultureInfo _culture;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSideObjectWriter"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        /// <param name="textWriter">The text writer.</param>
        public ClientSideObjectWriter(string id, string type, TextWriter textWriter)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            Guard.IsNotNullOrEmpty(type, "type");
            Guard.IsNotNull(textWriter, "textWriter");

            _id = id;
            _type = type;
            _writer = textWriter;

            _jsonWriter = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                TypeNameHandling = TypeNameHandling.None,
            };

            _jsonWriter.Converters.Add(new IsoDateTimeConverter());
        }

        /// <summary>
        /// Starts writing this instance.
        /// </summary>
        /// <returns></returns>
        public IClientSideObjectWriter Start()
        {
            if (_hasStarted)
            {
                throw new InvalidOperationException(YouCannotCallStartMoreThanOnce);
            }

            //Escape meta characters: http://api.jquery.com/category/selectors/
            var selector = @";&,.+*~':""!^$[]()|/".ToCharArray().Aggregate(_id, (current, chr) => current.Replace(chr.ToString(), @"\\" + chr));

            _writer.Write("$('#{0}').{1}(".FormatWith(selector, _type));
            _hasStarted = true;

            return this;
        }

        /// <summary>
        /// Appends the specified key value pair to the end of this instance.
        /// </summary>
        /// <param name="keyValuePair">The key value pair.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string keyValuePair)
        {
            EnsureStart();

            if (!string.IsNullOrEmpty(keyValuePair))
            {
                _writer.Write(_appended ? ", " : "{");
                _writer.Write(keyValuePair);

                if (!_appended)
                {
                    _appended = true;
                }
            }

            return this;
        }

        public IClientSideObjectWriter AppendKeyValuePairs<TValue>(string name, IEnumerable<KeyValuePair<string, TValue>> keyValuePairs)
        {
            var pairs = keyValuePairs ?? Enumerable.Empty<KeyValuePair<string, TValue>>();

            var serializedPairsValueBuilder = new StringBuilder();

            serializedPairsValueBuilder.AppendFormat("{0}: {{ ", name);

            foreach (var keyValuePair in pairs)
            {
                if (null != keyValuePair.Value)
                    serializedPairsValueBuilder
                        .AppendFormat("{0}: {1}, ",
                                      keyValuePair.Key,
                                      SerializeWithJsonNet(keyValuePair.Value));
            }

            if (pairs.Any())
                serializedPairsValueBuilder.Remove(serializedPairsValueBuilder.Length - 2, 2);

            serializedPairsValueBuilder.Append(" }");

            Append(serializedPairsValueBuilder.ToString());

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, string value)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                string formattedValue = QuoteString(value);

                Append("{0}:'{1}'".FormatWith(name, formattedValue));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and nullable value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter AppendNullableString(string name, string value)
        {

            if (!string.IsNullOrEmpty(name) && value != null)
            {
                string formattedValue = QuoteString(value);

                Append("{0}:'{1}'".FormatWith(name, formattedValue));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, int value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Append("{0}:{1}".FormatWith(name, value));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, int value, int defaultValue)
        {
            if (value != defaultValue)
            {
                Append(name, value);
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, int? value)
        {
            if (value.HasValue)
            {
                Append(name, value.Value);
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, double value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Append("{0}:'{1}'".FormatWith(name, value));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, double? value)
        {
            if (value.HasValue)
            {
                Append(name, value.Value);
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, decimal value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Append("{0}:'{1}'".FormatWith(name, value));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, decimal? value)
        {
            if (value.HasValue)
            {
                Append(name, value.Value);
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, bool value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Append("{0}:{1}".FormatWith(name, value.ToString(Culture).ToLower(Culture)));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, bool value, bool defaultValue)
        {
            if (value != defaultValue)
            {
                Append(name, value);
            }

            return this;
        }


        /// <summary>
        /// Appends the specified name and only the date of the passed DateTime collection
        /// </summary>
        public IClientSideObjectWriter AppendDatesOnly(string name, IEnumerable<DateTime> collection)
        {
            if (collection.Count() > 0)
            {
                List<DateTime> dates = collection.ToList();
                dates.Sort();

                StringBuilder builder = new StringBuilder();

                int year = -1;
                int month = -1;
                bool yearAppended = false;
                bool monthAppended = false;

                foreach (DateTime date in dates)
                {
                    if (year != date.Year)
                    {
                        if (yearAppended)
                        {
                            if (monthAppended)
                            {
                                builder.Append("]");
                            }
                            builder.Append("}");
                            builder.Append(",");
                            yearAppended = false;
                        }
                        builder.Append("'");
                        builder.Append(date.Year);
                        builder.Append("':{");

                        monthAppended = false;
                    }
                    if (month != date.Month)
                    {
                        if (monthAppended)
                        {
                            builder.Append("]");
                            builder.Append(",");
                            monthAppended = false;
                        }
                        builder.Append("'");
                        builder.Append(date.Month - 1);
                        builder.Append("':[");
                    }

                    if (year == date.Year && month == date.Month)
                    {
                        builder.Append(",");
                    }
                    builder.Append(date.Day);

                    if (month != date.Month)
                    {
                        month = date.Month;
                        monthAppended = true;
                    }

                    if (year != date.Year)
                    {
                        year = date.Year;
                        yearAppended = true;
                    }
                }
                builder.Append("]}");
                Append("{0}:{{{1}}}".FormatWith(name, builder.ToString()));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, Action action)
        {
            if (!string.IsNullOrEmpty(name) && (action != null))
            {
                Append("{0}:".FormatWith(name));
                action();
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, IList<string> values)
        {
            if (!string.IsNullOrEmpty(name) && !values.IsNullOrEmpty())
            {
                List<string> stringValues = new List<string>(values.Count);

                foreach (string value in values)
                {
                    stringValues.Add("'{0}'".FormatWith(QuoteString(value)));
                }

                Append("{0}:[{1}]".FormatWith(name, string.Join(",", stringValues.ToArray())));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append(string name, IList<int> values)
        {
            if (!string.IsNullOrEmpty(name) && !values.IsNullOrEmpty())
            {
                List<string> stringValues = new List<string>();

                foreach (int value in values)
                {
                    stringValues.Add(value.ToString(Culture));
                }

                Append("{0}:[{1}]".FormatWith(name, string.Join(",", stringValues.ToArray())));
            }

            return this;
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append<TEnum>(string name, TEnum value) where TEnum : IComparable, IFormattable
        {
            if (!string.IsNullOrEmpty(name))
            {
                ClientSideEnumValueAttribute valueAttribute = value.GetType().GetField(value.ToString())
                                                                             .GetCustomAttributes(true)
                                                                             .OfType<ClientSideEnumValueAttribute>()
                                                                             .FirstOrDefault();

                if (valueAttribute != null)
                {
                    Append("{0}:{1}".FormatWith(name, valueAttribute.Value));
                }
            }

            return this;
        }

        public IClientSideObjectWriter AppendClientEvents(IDictionary<string, string> clientEvents)
        {
            if (clientEvents == null || !clientEvents.Any())
                return this;

            return Append("{0}:{1}".FormatWith("eventHandlers", SerializeWithJsonNet(clientEvents)));
        }

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public IClientSideObjectWriter Append<TEnum>(string name, TEnum value, TEnum defaultValue) where TEnum : IComparable, IFormattable
        {
            if (!value.Equals(defaultValue))
            {
                Append(name, value);
            }

            return this;
        }

        public IClientSideObjectWriter AppendCollection(string name, IEnumerable value)
        {
            return Append("{0}:{1}".FormatWith(name, SerializeWithJsonNet(value)));
        }

        public IClientSideObjectWriter AppendObject(string name, object value)
        {
            return Append("{0}:{1}".FormatWith(name, SerializeWithJsonNet(value)));
        }

        /// <summary>
        /// Appends the specified name and Action or String specified in the ClientEvent object.
        /// </summary>
        public IClientSideObjectWriter AppendClientEvent(string name, string clientEvent)
        {
            if (name.IsNotEmpty() && clientEvent.IsNotEmpty())
            {
                Append("{0}:{1}".FormatWith(name, clientEvent));
            }

            return this;
        }

        /// <summary>
        /// Completes this instance.
        /// </summary>
        public void Complete()
        {
            EnsureStart();

            if (_appended)
            {
                _writer.Write("}");
            }

            _writer.Write(");");

            _hasStarted = false;
            _appended = false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Needs refactoring")]
        private string QuoteString(string value)
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(value))
            {
                int startIndex = 0;
                int count = 0;

                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];

                    if (c == '\r' || c == '\t' || c == '\"' || c == '\'' || c == '<' || c == '>' ||
                        c == '\\' || c == '\n' || c == '\b' || c == '\f' || c < ' ')
                    {
                        if (count > 0)
                        {
                            result.Append(value, startIndex, count);
                        }

                        startIndex = i + 1;
                        count = 0;
                    }

                    switch (c)
                    {
                        case '\r':
                            result.Append("\\r");
                            break;
                        case '\t':
                            result.Append("\\t");
                            break;
                        case '\"':
                            result.Append("\\\"");
                            break;
                        case '\\':
                            result.Append("\\\\");
                            break;
                        case '\n':
                            result.Append("\\n");
                            break;
                        case '\b':
                            result.Append("\\b");
                            break;
                        case '\f':
                            result.Append("\\f");
                            break;
                        case '\'':
                        case '>':
                        case '<':
                            AppendAsUnicode(result, c);
                            break;
                        default:
                            if (c < ' ')
                            {
                                AppendAsUnicode(result, c);
                            }
                            else
                            {
                                count++;
                            }

                            break;
                    }
                }

                if (result.Length == 0)
                {
                    result.Append(value);
                }
                else if (count > 0)
                {
                    result.Append(value, startIndex, count);
                }
            }

            return result.ToString();
        }

        private void AppendAsUnicode(StringBuilder builder, char c)
        {
            builder.Append("\\u");
            builder.AppendFormat(Culture, "{0:x4}", (int)c);
        }

        private void EnsureStart()
        {
            if (!_hasStarted)
            {
                throw new InvalidOperationException(YouMustHaveToCallStartPriorCallingThisMethod);
            }
        }

        private string SerializeWithJsonNet(object data)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                _jsonWriter.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }
    }
}