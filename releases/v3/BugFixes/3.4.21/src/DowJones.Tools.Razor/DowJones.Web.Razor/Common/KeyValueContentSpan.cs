using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;

namespace DowJones.Web.Razor.Common
{
    public class KeyValueContentSpan : CodeSpan
    {
        private static readonly Regex KeyValueExpression = new Regex("(?<Key>[^=]*)=(?<Value>[^,]*),?", RegexOptions.Compiled);
        private IEnumerable<KeyValuePair<string, string>> _clientResourceProperties;

        public KeyValueContentSpan(SourceLocation location, string content)
            : base(location, content)
        {
        }

        protected internal IEnumerable<KeyValuePair<string, string>> ContentKeyValuePairs
        {
            get
            {
                if(_clientResourceProperties == null)
                {
                    var matches = KeyValueExpression.Matches(Content ?? string.Empty);

                    var keyValuePairs =
                        from match in matches.Cast<Match>()
                        let nameGroup = match.Groups["Key"]
                        let valueGroup = match.Groups["Value"]
                        where nameGroup != null && valueGroup != null
                        select new KeyValuePair<string, string>(nameGroup.Value.Trim(), valueGroup.Value.Trim());
                    
                    _clientResourceProperties = keyValuePairs.ToArray();
                }
                
                return _clientResourceProperties;
            }
            set { _clientResourceProperties = value; }
        }

        public string SingletonValue
        {
            get
            {
                var match = Regex.Match(Content ?? string.Empty, @"^([^,=]*)$");
                return match.Success ? match.Value : null;
            }
        }

        protected internal string GetContentPropertyValue(string key)
        {
            return ContentKeyValuePairs
                .Where(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant())
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        protected internal bool GetContentPropertyValueBool(string key)
        {
            var value = GetContentPropertyValue(key);

            bool parsedValue;
            if (bool.TryParse(value, out parsedValue))
                return parsedValue;

            return false;
        }

        protected internal TEnum? GetContentPropertyEnum<TEnum>(string key)
            where TEnum : struct 
        {
            var value = GetContentPropertyValue(key);

            TEnum parsedEnum;

            if (Enum.TryParse(value, true, out parsedEnum))
                return parsedEnum;

            return null;
        }

    }
}