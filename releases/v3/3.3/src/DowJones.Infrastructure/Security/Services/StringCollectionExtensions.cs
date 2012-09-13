using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DowJones.Security.Services
{
    internal static class StringCollectionExtensions
    {
        public static bool HasAccess(this IEnumerable<string> tokensCol, string token, string tokenValue)
        {
            string tokenValuePair = string.Format("{0}={1}", token, tokenValue);
            return tokensCol.Any(tokenCol => tokenCol == tokenValuePair);
        }

        public static int MatchPattern(this IEnumerable<string> list, string pattern, int returnGroupIndex)
        {
            var regex = new Regex(pattern);
            int minValue = 0;
            int counter = 0;
            foreach (var item in list)
            {
                var match = regex.Match(item.Trim().ToLower());
                while (match.Success)
                {
                    int value;
                    Int32.TryParse(match.Groups[returnGroupIndex].Value, out value);
                    if(counter == 0)
                    {
                        minValue = value;
                    }
                    counter++;
                    minValue = value < minValue ? value : minValue;
                    match = match.NextMatch();
                }
            }
            return minValue;
        }
    }
        
}
