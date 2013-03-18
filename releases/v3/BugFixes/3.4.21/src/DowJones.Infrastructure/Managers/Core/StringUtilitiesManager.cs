// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringUtilitiesManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CJKLanguages type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DowJones.Managers.Core
    {
        /// <summary>
        /// </summary>
    public enum CJKLanguages
    {
        /// <summary>
        /// No Language
        /// </summary>
        None,

        /// <summary>
        /// Chinese, Japanese, Korean Language
        /// </summary>
        CJK,

        /// <summary>
        /// Japanese Language
        /// </summary>
        JA
    }

    /// <summary>
    /// A set of static utility methods for manipulating strings.
    /// author ratcliffen, created on 18-Jun-2004 at 07:47:22
    /// converted dacostad, 5.22.2008
    /// </summary>
    public sealed class StringUtilitiesManager
    {
        private const string Amp = "&amp;";
        private const string Apos = "&apos;";
        private const string Greater = "&gt;";
        private const string Less = "&lt;";
        private const string Quote = "&quot;";

        private static readonly Regex ArabicContentRegex = new Regex("[\u0600-\u06FF]|[\u0750-\u077F]|[\uFB50-\uFDFF]|[\uFE70-\uFEFF]");
        private static readonly Regex HebrewContentRegex = new Regex("[\u0590-\u05FF]|[\uFB1D-\uFB40]");
        private static readonly Regex ArabicSlashHebrewContentRegex = new Regex("[\u0600-\u06FF]|[\u0750-\u077F]|[\uFB50-\uFDFF]|[\uFE70-\uFEFF]|[\u0590-\u05FF]|[\uFB1D-\uFB40]");

        public static bool HasArabicCharacters(string text)
        {
            return ArabicContentRegex.IsMatch(text);
        }

        public static bool HasHebrewCharacters(string text)
        {
            return HebrewContentRegex.IsMatch(text);
        }

        public static bool HasArabicSlashHebreCharacters(string text)
        {
            return ArabicSlashHebrewContentRegex.IsMatch(text);
        }

        /// <summary>
        /// Determines whether the specified s is empty.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>
        /// <c>true</c> if the specified s is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(string s)
        {
            return string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim());
        }

        /// <summary>
        /// Gets the name of the XML enum.
        /// </summary>
        /// <typeparam name="T">Any valid type.</typeparam>
        /// <param name="value">The string.</param>
        /// <returns>The string name associated with the enum</returns>
        public static string GetXmlEnumName<T>(string value)
        {
            var fieldInfo = typeof(T).GetField(value);
            var enumAttribute = (XmlEnumAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(XmlEnumAttribute));
            return enumAttribute != null ? enumAttribute.Name : value;
        }

        /// <summary>
        /// Determines whether the specified c is minus.
        /// </summary>
        /// <param name="c">the character to test</param>
        /// <returns>
        /// <c>true</c> if the specified c is minus or a hyphen; otherwise, <c>false</c>.
        /// </returns>
        /// Returns true if the character is a '-', hyphen, or a wide-character minus.
        public static bool IsMinus(char c)
        {
            return c == '-' || c == 0x2010 || c == 0x2012 || c == 0x2013 || c == 0x2210 || c == 0xFF0D;
        }

        /// <summary>
        /// Determines whether the specified expression is numeric.(http://support.microsoft.com/default.aspx?scid=kb;en-us;329488)
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// <c>true</c> if the specified expression is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(object expression)
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
        /// Determines whether the specified s is valid.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>
        /// <c>true</c> if the specified s is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValid(string s)
        {
            return !string.IsNullOrEmpty(s) && !string.IsNullOrEmpty(s.Trim());
        }

        /// <summary>
        /// Requireses the parenthesis.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>Returns true if the string is not already enclosed in parenthesis.</returns>
        public static bool RequiresParenthesis(string s)
        {
            if (IsEmpty(s))
            {
                return false;
            }

            if (s.StartsWith("(") && s.EndsWith(")"))
            {
                var count = 1;
                for (var i = 1; i < s.Length - 1; i++)
                {
                    var c = s[i];
                    switch (c)
                    {
                        case '(':
                            count++;
                            break;
                        case ')':
                            count--;
                            break;
                    }

                    if (count == 0)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// adds the string s to the buffer, enclosing in parenthesis only if necessary. - i.e. if the
        /// string is not already enclosed in parenthesis.
        /// </summary>
        /// <param name="buf">The buffer.</param>
        /// <param name="s">The string.</param>
        public static void AddWithParenthesis(StringBuilder buf, string s)
        {
            if (IsEmpty(s) || buf == null)
            {
                return;
            }

            if (RequiresParenthesis(s))
            {
                buf.Append('(').Append(s).Append(')');
            }
            else
            {
                buf.Append(s);
            }
        }

        /// <summary>
        /// If the character is a full width latin character, returns the standard half width latin
        /// character.
        /// </summary>
        /// <param name="c">The c the character to convert.</param>
        /// <returns>The half width character</returns>
        public static char ToHalfWidthLatin(char c)
        {
            if (c > 0xFF00 && c < 0xFF5F)
            {
                return (char)(c - 0xFEE0);
            }

            return c;
        }

        /// <summary>
        /// Checks to see if c matches the target character, ignoring the difference between half and full width latin characters.
        /// </summary>
        /// <param name="c">The character to test.</param>
        /// <param name="target">The character to test against.</param>
        /// <returns>Returns true if the character c matches the target character, ignoring the difference between
        /// half and full width latin characters.</returns>
        /// @param c      the character to test
        /// @param target the character to test against
        /// @return true if the character matches.
        public static bool EqualsIgnoreLatinWidth(char c, char target)
        {
            return target == ToHalfWidthLatin(c);
        }

        /// <summary>
        /// Converts any full width latin characters to their standard half width form.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns> the half width character</returns>
        public static string ToHalfWidthLatin(string s)
        {
            if (s == null)
            {
                return null;
            }

            var buf = new StringBuilder();

            foreach (var t in s)
            {
                buf.Append(ToHalfWidthLatin(t));
            }

            return buf.ToString();
        }

        /// <summary>
        /// Equalses the width of the ignore latin.
        /// </summary>
        /// <param name="s">The string to to test.</param>
        /// <param name="target">The string to to test against.</param>
        /// <returns>Returns true if the supplied string s equals the target string, ignoring the difference between
        /// half and full width latin characters.</returns>
        /// @param s      the string to to test
        /// @param target the string to test against
        /// @return true if the strings match
        public static bool EqualsIgnoreLatinWidth(string s, string target)
        {
            if (target == null)
            {
                return s == null;
            }

            return target.Equals(ToHalfWidthLatin(s));
        }

        /// <summary>
        /// Detects if the string contains Japanese specific kana or CJK kanji/han characters and returns
        /// the language detected - either 'ja', 'cjk', or null, if no CJK language detected. Note: given
        /// the set of languages currently supported by Factiva, a CJK character is assumed for any
        /// character with a codepoint over 0x3000. However, this is a coarse generalisation and may need
        /// to be refined as additional langauges are supported.
        /// </summary>
        /// <param name="s">The s. string to test</param>
        /// <returns>
        /// the query language if cjk language detected, otherwise null
        /// </returns>
        public static CJKLanguages DetectCJK(string s)
        {
            if (IsEmpty(s))
            {
                return CJKLanguages.None;
            }

            var lang = CJKLanguages.None;

            foreach (var codePoint in s)
            {
                if (codePoint <= 0x3000)
                {
                    continue;
                }

                if (lang == CJKLanguages.None)
                {
                    lang = CJKLanguages.CJK;
                }

                if (codePoint > 0x30FC && (codePoint <= 0xFF66 || codePoint >= 0xFF9D))
                {
                    continue;
                }

                lang = CJKLanguages.JA;
                break;
            }

            return lang;
        }

        /// <summary>
        /// Detects if the string contains Japanese specific kana or CJK kanji/han characters and returns
        /// the language detected - either 'ja', 'cjk', or null, if no CJK language detected. Note: given
        /// the set of languages currently supported by Factiva, a CJK character is assumed for any
        /// character with a codepoint over 0x3000. However, this is a coarse generalisation and may need
        /// to be refined as additional langauges are supported.
        /// </summary>
        /// <param name="s">The s. string to test</param>
        /// <returns>
        /// <c>true</c> if the specified s has CJK characters; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasCJK(string s)
        {
             if (IsEmpty(s))
             {
                 return false;
             }

            foreach (int codePoint in s)
            {
                if (codePoint > 0x3000)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Encodes all characters with codepoints above the ascii code page with their unicode codepoint
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The character encoded string.</returns>
        public static string CharacterEncode(string s)
        {
            return CharacterEncode(s, 0x007F);
        }

        /// <summary>
        /// Encodes all characters with codepoints greater than the supplied limit with their unicode codepoint character encoding.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <param name="limit">The limit the codepoint above which characters should be converted to their encoded form.</param>
        /// <returns>The character encoded string.</returns>
        /// Encodes all characters with codepoints greater than the supplied limit with their unicode
        /// codepoint character encoding.
        public static string CharacterEncode(string s, int limit)
        {
            if (IsEmpty(s))
            {
                return s;
            }

            var buf = new StringBuilder();
            foreach (char t in s)
            {
                // note: change
                int codePoint = t;
                if (codePoint > limit)
                {
                    buf.Append("&#x").Append(codePoint.ToString("x4")).Append(';');
                }
                else
                {
                    buf.Append(t);
                }
            }

            return buf.ToString();
        }

        /// <summary>
        /// Reverses any xml encodings in the string. (e.g. reverts '&amp;amp;' to '&amp;')
        /// </summary>
        /// <param name="s">The string containing xml encodings.</param>
        /// <returns>return the xml unencoded string</returns>
        public static string XmlUnencode(string s)
        {
            if (IsEmpty(s))
            {
                return s;
            }

            var sb = new StringBuilder(s);
            sb = sb.Replace(Amp, "&").Replace(Less, "<").Replace(Greater, ">").Replace(Quote, "\"").Replace(Apos, "'");
            return sb.ToString(); 
        }

        /**
   * Xml encodes the string. Takes care not to double encode &'s which are the start of xml
   * encodings.
   *
   * @param s the string to encode
   * @return the xml encoded string
   */
        public static string XmlEncode(string s)
        {
            if (IsEmpty(s))
            {
                return s;
            }

            StringBuilder buf = null;

            for (var i = 0; i < s.Length; i++)
            {
                var character = s[i];
                switch (character)
                {
                    case '<':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Less);
                        break;

                    case '>':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Greater);
                        break;

                    case '"':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Quote);
                        break;

                    case '\'':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Apos);
                        break;

                    case '&':
                        if (!IsStartOfXmlEncoding(s, i))
                        {
                            if (buf == null)
                            {
                                buf = GetInitialBuffer(s, i);
                            }
                        
                            buf.Append(Amp);
                            break;
                        }

                        goto default;
                    default:
                        if (buf != null)
                        {
                            buf.Append(character);
                        }

                        break;
                }
            }

            return buf == null ? s : buf.ToString();
        }

        /**
   * Xml encodes the string, except for the specified tag, which is preserved and replaced with the
   * replace tag if not null. Therefore, the tag, or the replace tag if specified, should already be
   * in an xml encoded form. Takes care not to double encode &'s which are the start of xml
   * encodings.
   *
   * @param s          the string to encode
   * @param tag        the tag to exclude from the encoding
   * @param replaceTag the replacement tag, if not null, to replace the tag with
   * @return the xml encoded string, with the specified tags preserved and replaced
   */
        public static string XmlEncode(string s, string tag, string replaceTag)
        {
            if (IsEmpty(s))
            {
                return s;
            }

            if (IsEmpty(tag))
            {
                return XmlEncode(s);
            }

            StringBuilder buf = null;

            for (var i = 0; i < s.Length; i++)
            {
                var character = s[i];
                switch (character)
                {
                    case '<':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        var isStartTag = false;
                        var isEndTag = false;
                        if (s[(i + 1)] == '/')
                        {
                            isEndTag = IsTag(s, i + 2, tag);
                        }
                        else
                            isStartTag = IsTag(s, i + 1, tag);

                        if (isStartTag || isEndTag)
                        {
                            if (isStartTag)
                            {
                                buf.Append('<');
                                i += tag.Length + 1;
                            }
                            else
                            {
                                buf.Append("</");
                                i += tag.Length + 2;
                            }

                            buf.Append(replaceTag ?? tag);

                            if (s[i] == '/')
                            {
                                buf.Append('/');
                                i++;
                            }

                            buf.Append('>');
                        }
                        else
                            buf.Append(Less);

                        break;

                    case '>':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Greater);
                        break;

                    case '"':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Quote);
                        break;

                    case '\'':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Apos);
                        break;

                    case '&':
                        if (!IsStartOfXmlEncoding(s, i))
                        {
                            if (buf == null)
                            {
                                buf = GetInitialBuffer(s, i);
                            }

                            buf.Append(Amp);
                            break;
                        }

                        // else false hit so fall thru to default
                        goto default;
                    default:
                        if (buf != null)
                        {
                            buf.Append(character);
                        }

                        break;
                }
            }

            return buf == null ? s : buf.ToString();
        }

        /// <summary>
        /// Xml encodes the string to be carried as attribute content - i.e escapes &amp;, &lt;, &gt; and " but
        /// leaves ', or the basis that the element content will be enclosed in "" and not ''. Takes care
        /// not to double encode &amp;'s which are the start of xml encodings.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>the encoded string, suitable to be carried as attribute content</returns>
        public static string XmlAttributeEncode(string s)
        {
            if (IsEmpty(s))
            {
                return s;
            }

            StringBuilder buf = null;

            for (var i = 0; i < s.Length; i++)
            {
                var character = s[i];
                switch (character)
                {
                    case '<':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }
                        
                        buf.Append(Less);
                        break;

                    case '>':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Greater);
                        break;

                    case '"':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Quote);
                        break;

                    case '&':
                        if (!IsStartOfXmlEncoding(s, i))
                        {
                            if (buf == null)
                            {
                                buf = GetInitialBuffer(s, i);
                            }

                            buf.Append(Amp);
                            break;
                        }

                        // else false hit so fall thru to default
                        goto default;
                        
                    default:
                        if (buf != null)
                        {
                            buf.Append(character);
                        }

                        break;
                }
            }

            return buf == null ? s : buf.ToString();
        }

        /// <summary>
        /// Xml encodes the string to be carried as element content - i.e escapes &amp;, &lt; and &gt; but leaves "
        /// and ' as these are permissible element content. Takes care not to double encode &amp;'s which are
        /// the start of xml encodings.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The encoded string, suitable to be carried as element content.</returns>
        public static string XmlElementEncode(string s)
        {
            if (IsEmpty(s))
            {
                return s;
            }

            StringBuilder buf = null;

            for (var i = 0; i < s.Length; i++)
            {
                var character = s[i];
                switch (character)
                {
                    case '<':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Less);
                        break;

                    case '>':
                        if (buf == null)
                        {
                            buf = GetInitialBuffer(s, i);
                        }

                        buf.Append(Greater);
                        break;

                    case '&':
                        if (!IsStartOfXmlEncoding(s, i))
                        {
                            if (buf == null)
                            {
                                buf = GetInitialBuffer(s, i);
                            }

                            buf.Append(Amp);
                            break;
                        }

                        // else false hit so fall thru to default
                        goto default;
                    default:
                        if (buf != null)
                        {
                            buf.Append(character);
                        }

                        break;
                }
            }

            return buf == null ? s : buf.ToString();
        }

        /// <summary>
        /// Determines whether [is start of XML encoding] [the specified s].
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="pos">The position in the string to test.</param>
        /// <returns>
        /// <c>true</c> if [is start of XML encoding] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsStartOfXmlEncoding(string s, int pos)
        {
            // check next 5 characters to find a ';' - or end of string - and extract string to test
            for (var i = pos + 1; i < s.Length - 1 && i < pos + 6; i++)
            {
                if (s[i] != ';')
                {
                    continue;
                }

                s = s.Substring(pos, i + 1);
                return s.Equals(Amp) || s.Equals(Less) || s.Equals(Greater) || s.Equals(Quote) || s.Equals(Apos);
            }

            return false;
        }

        /// <summary>
        /// Returns a buffer containing the string up to the position specified.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="pos">The position in the string to copy up to into the buffer.</param>
        /// <returns>The initial buffer.</returns>
        private static StringBuilder GetInitialBuffer(string s, int pos)
        {
            return new StringBuilder(s.Substring(0, pos));
        }

        /// <summary>
        /// Determines whether the specified s is tag.
        /// </summary>
        /// <param name="s">The the string containing the tag to check.</param>
        /// <param name="i">The position of the tag in the string.</param>
        /// <param name="tag">The value of the tag to check.</param>
        /// <returns>
        /// <c>true</c> if the specified s is tag; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTag(string s, int i, string tag)
        {
            var pos = s.IndexOf('>', i);
            if (pos == -1)
            {
                return false;
            }

            if (s[(pos - 1)] == '/')
            {
                pos--;
            }

            var find = s.Substring(i, pos);
            return find.Equals(tag);
        }
    }
}