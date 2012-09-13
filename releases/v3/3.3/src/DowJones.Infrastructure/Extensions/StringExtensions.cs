// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for the string data type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using DowJones.Attributes;
using DowJones.DependencyInjection;
using DowJones.Globalization;
using DowJones.Search;

namespace DowJones.Extensions
{
     public enum HtmlEscapeType
     {
         DoubleQuote,
         SingleQuote,
     }



    /// <summary>
    /// Extension methods for the string data type
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex KeyValueExpression = new Regex("(?<Key>[^=]*)=(?<Value>[^;]*);?", RegexOptions.Compiled);
        private static readonly Regex NameExpression = new Regex("([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z]+)", RegexOptions.Compiled);

        #region Common string extensions

        /// <summary>
        /// Ases the title.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The formatted string.</returns>
        [DebuggerStepThrough]
        public static string AsTitle(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var lastIndex = value.LastIndexOf(".", StringComparison.Ordinal);

            if (lastIndex > -1)
            {
                value = value.Substring(lastIndex + 1);
            }

            return value.SplitPascalCase();
        }

        public static string EscapeForHtml(this string value, HtmlEscapeType type = HtmlEscapeType.DoubleQuote)
        {
            switch (type)
            {
                case HtmlEscapeType.DoubleQuote:
                    return value.Replace("\"", "&#34;");
                default:
                    return value.Replace("'", "&#39;");
            }
        }

        /// <summary>
        /// Splits the Pascal case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The formatted string.</returns>
        [DebuggerStepThrough]
        public static string SplitPascalCase(this string value)
        {
            return NameExpression.Replace(value, " $1").Trim();
        }

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

        public static string TrimToLastIndexOf(this string value, char searchChar)
        {
            if(string.IsNullOrWhiteSpace(value))
                return value;

            var lastIndex = value.LastIndexOf(searchChar);

            if (lastIndex <= 0)
                return value;

            return value.Substring(0, lastIndex);
        }

        /// <summary>
        /// Trims the text to a provided maximum length.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <returns>The trimmed string.</returns>
        /// <remarks>Proposed by Rene Schulte</remarks>
        public static string TrimToMaxLength(this string value, int maxLength)
        {
            return value == null || value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Trims the text to a provided maximum length and adds a suffix if required.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The trimmed string.</returns>
        /// <remarks>Proposed by Rene Schulte</remarks>
        public static string TrimToMaxLength(this string value, int maxLength, string suffix)
        {
            return value == null || value.Length <= maxLength ? value : string.Concat(value.Substring(0, maxLength), suffix);
        }

        /// <summary>
        /// Determines whether the comparison value string is contained within the input value string
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <param name="comparisonValue">The comparison value.</param>
        /// <param name="comparisonType">Type of the comparison to allow case sensitive or insensitive comparison.</param>
        /// <returns>
        /// <c>true</c> if input value contains the specified value, otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string inputValue, string comparisonValue, StringComparison comparisonType)
        {
            return inputValue.IndexOf(comparisonValue, comparisonType) != -1;
        }

        /// <summary>
        /// Converts a string of semicolon-delimited key/value pairs 
        /// into an array of KeyValuePair%lt;string, string/%gt;
        /// </summary>
        /// <param name="value">The semicolon-delimited string of key/value pairs (in the form of [Key]=[Value])</param>
        /// <returns>An array of KeyValuePair%lt;string, string/%gt;</returns>
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this string value)
        {
            var matches = KeyValueExpression.Matches(value ?? string.Empty);

            var keyValuePairs =
                from match in matches.Cast<Match>()
                let nameGroup = match.Groups["Key"]
                let valueGroup = match.Groups["Value"]
                where nameGroup != null && valueGroup != null
                select new KeyValuePair<string, string>(nameGroup.Value.Trim(), valueGroup.Value.Trim());

            return keyValuePairs.ToArray();
        }

        /// <summary>
        /// Loads the string into a LINQ to XML XDocument
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The XML document object model (XDocument)</returns>
        public static XDocument ToXDocument(this string xml)
        {
            return XDocument.Parse(xml);
        }

        /// <summary>
        /// Loads the string into a XML DOM object (XmlDocument)
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The XML document object model (XmlDocument)</returns>
        public static XmlDocument ToXmlDom(this string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return document;
        }

        /// <summary>
        /// Loads the string into a XML XPath DOM (XPathDocument)
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The XML XPath document object model (XPathNavigator)</returns>
        public static XPathNavigator ToXPath(this string xml)
        {
            var document = new XPathDocument(new StringReader(xml));
            return document.CreateNavigator();
        }

        /// <summary>
        /// Reverses / mirrors a string.
        /// </summary>
        /// <param name="value">The string to be reversed.</param>
        /// <returns>The reversed string</returns>
        public static string Reverse(this string value)
        {
            if (value.IsNullOrEmpty() || (value.Length == 1))
            {
                return value;
            }

            var chars = value.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// Ensures that a string starts with a given prefix.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <param name="prefix">The prefix value to check for.</param>
        /// <returns>The string value including the prefix</returns>
        /// <example>
        /// <code>
        /// var extension = "txt";
        /// var fileName = string.Concat(file.Name, extension.EnsureStartsWith("."));
        /// </code>
        /// </example>
        public static string EnsureStartsWith(this string value, string prefix)
        {
            return value.StartsWith(prefix) ? value : string.Concat(prefix, value);
        }

        /// <summary>
        /// Ensures that a string ends with a given suffix.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <param name="suffix">The suffix value to check for.</param>
        /// <returns>The string value including the suffix</returns>
        /// <example>
        /// <code>
        /// var url = "http://www.factiva.com";
        /// url = url.EnsureEndsWith("/"));
        /// </code>
        /// </example>
        public static string EnsureEndsWith(this string value, string suffix)
        {
            return value.EndsWith(suffix) ? value : string.Concat(value, suffix);
        }

        #endregion

        #region Regex based extension methods

        /// <summary>
        /// Uses regular expressions to determine if the string matches to a given regex pattern.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <returns>
        /// <c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// var isMatching = s.IsMatchingTo(@"^\d+$");
        /// </code>
        /// </example>
        public static bool IsMatchingTo(this string value, string regexPattern)
        {
            return IsMatchingTo(value, regexPattern, RegexOptions.None);
        }

        /// <summary>
        /// Uses regular expressions to determine if the string matches to a given regex pattern.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>
        /// <c>true</c> if the value is matching to the specified pattern; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// var isMatching = s.IsMatchingTo(@"^\d+$");
        /// </code>
        /// </example>
        public static bool IsMatchingTo(this string value, string regexPattern, RegexOptions options)
        {
            return Regex.IsMatch(value, regexPattern, options);
        }

        /// <summary>
        /// Uses regular expressions to replace parts of a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="replaceValue">The replacement value.</param>
        /// <returns>The newly created string</returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
        /// </code>
        /// </example>
        public static string ReplaceWith(this string value, string regexPattern, string replaceValue)
        {
            return ReplaceWith(value, regexPattern, replaceValue, RegexOptions.None);
        }

        /// <summary>
        /// Uses regular expressions to replace parts of a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="replaceValue">The replacement value.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>The newly created string</returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
        /// </code>
        /// </example>
        public static string ReplaceWith(this string value, string regexPattern, string replaceValue, RegexOptions options)
        {
            return Regex.Replace(value, regexPattern, replaceValue, options);
        }

        /// <summary>
        /// Uses regular expressions to replace parts of a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="evaluator">The replacement method / lambda expression.</param>
        /// <returns>The newly created string</returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
        /// </code>
        /// </example>
        public static string ReplaceWith(this string value, string regexPattern, MatchEvaluator evaluator)
        {
            return ReplaceWith(value, regexPattern, RegexOptions.None, evaluator);
        }

        /// <summary>
        /// Uses regular expressions to replace parts of a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <param name="evaluator">The replacement method / lambda expression.</param>
        /// <returns>The newly created string</returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// var replaced = s.ReplaceWith(@"\d", m => string.Concat(" -", m.Value, "- "));
        /// </code>
        /// </example>
        public static string ReplaceWith(this string value, string regexPattern, RegexOptions options, MatchEvaluator evaluator)
        {
            return Regex.Replace(value, regexPattern, evaluator, options);
        }

        /// <summary>
        /// Uses regular expressions to determine all matches of a given regex pattern.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <returns>A collection of all matches</returns>
        public static MatchCollection GetMatches(this string value, string regexPattern)
        {
            return GetMatches(value, regexPattern, RegexOptions.None);
        }

        /// <summary>
        /// Uses regular expressions to determine all matches of a given regex pattern.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>A collection of all matches</returns>
        public static MatchCollection GetMatches(this string value, string regexPattern, RegexOptions options)
        {
            return Regex.Matches(value, regexPattern, options);
        }

        /// <summary>
        /// Uses regular expressions to determine all matches of a given regex pattern and returns them as string enumeration.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <returns>An enumeration of matching strings</returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// foreach(var number in s.GetMatchingValues(@"\d")) {
        ///  Console.WriteLine(number);
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern)
        {
            return GetMatchingValues(value, regexPattern, RegexOptions.None);
        }

        /// <summary>
        /// Uses regular expressions to determine all matches of a given regex pattern and returns them as string enumeration.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>An enumeration of matching strings</returns>
        /// <example>
        /// <code>
        /// var s = "12345";
        /// foreach(var number in s.GetMatchingValues(@"\d")) {
        ///  Console.WriteLine(number);
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern, RegexOptions options)
        {
            return from Match match in GetMatches(value, regexPattern, options) where match.Success select match.Value;
        }

        /// <summary>
        /// Uses regular expressions to split a string into parts.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <returns>The splitted string array</returns>
        public static string[] Split(this string value, string regexPattern)
        {
            return value.Split(regexPattern, RegexOptions.None);
        }

        /// <summary>
        /// Uses regular expressions to split a string into parts.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="regexPattern">The regular expression pattern.</param>
        /// <param name="options">The regular expression options.</param>
        /// <returns>The splitted string array</returns>
        public static string[] Split(this string value, string regexPattern, RegexOptions options)
        {
            return Regex.Split(value, regexPattern, options);
        }

        /// <summary>
        /// Splits the given string into words and returns a string array.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The splitted string array</returns>
        public static string[] GetWords(this string value)
        {
            return value.Split(@"\W");
        }

        /// <summary>
        /// Checks whether the string is empty and returns a default value in case.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Either the string or the default value.</returns>
        public static string GetValueOrDefault(this string value, string defaultValue = null)
        {
            return value.HasValue() ? value : defaultValue;
        }

        /// <summary>
        /// Gets the nth "word" of a given string, where "words" are substrings separated by a given separator
        /// </summary>
        /// <param name="value">The string from which the word should be retrieved.</param>
        /// <param name="index">Index of the word (0-based).</param>
        /// <returns>
        /// The word at position n of the string.
        /// Trying to retrieve a word at a position lower than 0 or at a position where no word exists results in an exception.
        /// </returns>
        /// <remarks>Originally contributed by MMathews</remarks>
        public static string GetWordByIndex(this string value, int index)
        {
            var words = value.GetWords();

            if ((index < 0) || (index > words.Length - 1))
            {
                throw new IndexOutOfRangeException("The word number is out of range.");
            }

            return words[index];
        }

        public static bool IsUrl(this string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                   && Regex.IsMatch(value, @"^([^:].*//|~/)");
        }

        #endregion

        #region Bytes & Base64

        /// <summary>
        /// Converts the string to a byte-array using the default encoding
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The created byte array</returns>
        public static byte[] ToBytes(this string value)
        {
            return value.ToBytes(null);
        }

        /// <summary>
        /// Converts the string to a byte-array using the supplied encoding
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <param name="encoding">The encoding to be used.</param>
        /// <returns>The created byte array</returns>
        /// <example><code>
        /// var value = "Hello World";
        /// var ansiBytes = value.ToBytes(Encoding.GetEncoding(1252)); // 1252 = ANSI
        /// var utf8Bytes = value.ToBytes(Encoding.UTF8);
        /// </code></example>
        public static byte[] ToBytes(this string value, Encoding encoding)
        {
            encoding = encoding ?? Encoding.Default;
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Encodes the input value to a Base64 string using the default encoding.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The Base 64 encoded string</returns>
        public static string EncodeBase64(this string value)
        {
            return value.EncodeBase64(null);
        }

        /// <summary>
        /// Encodes the input value to a Base64 string using the supplied encoding.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The Base 64 encoded string</returns>
        public static string EncodeBase64(this string value, Encoding encoding)
        {
            encoding = encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decodes a Base 64 encoded value to a string using the default encoding.
        /// </summary>
        /// <param name="encodedValue">The Base 64 encoded value.</param>
        /// <returns>The decoded string</returns>
        public static string DecodeBase64(this string encodedValue)
        {
            return encodedValue.DecodeBase64(null);
        }

        /// <summary>
        /// Decodes a Base 64 encoded value to a string using the supplied encoding.
        /// </summary>
        /// <param name="encodedValue">The Base 64 encoded value.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The decoded string</returns>
        public static string DecodeBase64(this string encodedValue, Encoding encoding)
        {
            encoding = encoding ?? Encoding.UTF8;
            var bytes = Convert.FromBase64String(encodedValue);
            return encoding.GetString(bytes);
        }

        #endregion

        public static bool ContainsAtAnyIndex(this IEnumerable<string> list, string find, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return list.Any(item => item.IndexOf(find, comparison) != -1);
        }

        public static bool StartsWithAtAnyIndex(this IEnumerable<string> list, string find, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return list.Any(item => item.StartsWith(find, comparison));
        }

        [DebuggerStepThrough]
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Filters out all null items from a list
        /// </summary>
        /// <returns>The non-null items in a list</returns>
        public static IEnumerable<string> WhereNotNullOrEmpty(this IEnumerable<string> source)
        {
            return source.Where(x => x.HasValue());
        }

        public static TEnum ToEnum<TEnum>(this string source, TEnum defaultValue = default(TEnum)) 
            where TEnum : struct
        {
            var enumValue = defaultValue;

            if(source.HasValue())
                Enum.TryParse(source, true, out enumValue);

            return enumValue;
        }

        public static string ToTokenTranslation(this string s)
        {
            return ServiceLocator.Resolve<IResourceTextManager>().GetString(s);
        }

        public static string GetAssignedToken(this Enum e)
        {
            string strToken = null;
            var field = e.GetType().GetField(e.ToString());
            var displayAs = (AssignedToken)Attribute.GetCustomAttribute(field, typeof(AssignedToken));
            if (displayAs != null)
            {
                strToken = displayAs.Token;
            }

            return strToken;
        }
    }
}