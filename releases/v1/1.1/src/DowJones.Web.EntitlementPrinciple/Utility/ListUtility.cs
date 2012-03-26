using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DowJones.Web.EntitlementPrinciple.Utility
{
    internal class ListUtility
    {
        public static bool FindInList(IEnumerable<string> list, string find)
        {
            return list.Any(item => item.Trim().ToLower().IndexOf(find.ToLower()) != -1);
        }

        public static string FindAnyValue(IEnumerable<string> list)
        {
            return list.Where(item => !string.IsNullOrEmpty(item)).FirstOrDefault();
        }

        public static bool HasAccess(IEnumerable<string> tokensCol, string token, string tokenValue)
        {
            var tokenFound = false;
            foreach (var ac in tokensCol)
            {
                var nv = ac.Split('=');
                if (nv.Length < 2) continue;
                if (nv[0] != token) continue;
                tokenFound = true;
                if (nv[1] == tokenValue)
                    return true;
            }
            return tokenFound == false;
        }

        public static int MatchPatternForNewsPage(IEnumerable<string> list, string pattern, int returnGroupIndex)
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
