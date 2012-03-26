// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageUtilityManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Attributes;

namespace DowJones.Globalization
{
    public class LanguageUtilityManager
    {
        private static readonly Regex RegexDialectRemoval = new Regex(
            @"-.*$",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled);

        /// <summary>
        /// Called in cases when the user comes into S2, and the the checking for access is not required.
        /// Free access to pages, as coming in with impersonation requirements.
        /// </summary>
        /// <param name="interfaceLanguageCode">Specified language code</param>
        /// <param name="fullLanguageSetAllowed">Allow full language set instead of the Search 20 specific set</param>
        /// <returns>A string object.</returns>
        public static string GetCultureInfoLanguageCode(string interfaceLanguageCode, bool fullLanguageSetAllowed)
        {
            if (string.IsNullOrEmpty(interfaceLanguageCode))
            {
                return "en";
            }

            switch (interfaceLanguageCode.ToLower())
            {
                case "en":
                case "template":
                    return "en";
                case "fr":
                    return "fr";
                case "de":
                    return "de";
                case "es":
                    return "es";
                case "it":
                    return "it";
                case "ja":
                    return "ja";
                case "ru":
                    return "ru";
                case "zhcn":
                    return "zhcn";
                case "zhtw":
                    return "zhtw";
                default:
                    throw new NotSupportedException(interfaceLanguageCode);
            }
        }

        public static string ValidateLanguageCode(string language)
        {
            if (language.IsNullOrEmpty())
            {
                return "en";
            }

            switch (language.ToLower())
            {
                default:
                    language = "en";
                    break;
                case "fr":
                    language = "fr";
                    break;
                case "de":
                    language = "de";
                    break;
                case "es":
                    language = "es";
                    break;
                case "it":
                    language = "it";
                    break;
                case "ja":
                    language = "ja";
                    break;
                case "ru":
                    language = "ru";
                    break;
                case "zh":
                case "zh-cn":
                case "zhcn":
                    language = "zhcn";
                    break;
                case "zh-tw":
                case "zhtw":
                    language = "zhtw";
                    break;
            }

            return language;
        }

        public static string ValidateLanguageCodeWithTemplate(string language)
        {
            if (language.IsNullOrEmpty())
            {
                return "en";
            }

            switch (language.ToLower())
            {
                default:
                    language = "en";
                    break;
                case "fr":
                    language = "fr";
                    break;
                case "de":
                    language = "de";
                    break;
                case "es":
                    language = "es";
                    break;
                case "it":
                    language = "it";
                    break;
                case "ja":
                    language = "ja";
                    break;
                case "ru":
                    language = "ru";
                    break;
                case "template":
                    language = "template";
                    break;
                case "zh":
                case "zh-cn":
                case "zhcn":
                    language = "zhcn";
                    break;
                case "zh-tw":
                case "zhtw":
                    language = "zhtw";
                    break;
            }

            return language;
        }

        /// <summary>
        /// Sets the thread culture.
        /// </summary>
        /// <param name="userLanguage">The interface language.</param>
        public static void SetThreadCulture(string userLanguage)
        {
            if (userLanguage == null)
            {
                throw new ArgumentNullException(@"userLanguage");
            }

            // Set interface language
            var interfaceLanguage = ValidateLanguageCodeWithTemplate(userLanguage);

            var cultureInfoCode = GetCultureInfoLanguageCode(interfaceLanguage, true);

            try
            {
                switch (interfaceLanguage)
                {
                    default:
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfoCode);
                        break;

                    case "zh":
                    case "zh-cn":
                    case "zhcn":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("zh-cn");
                        break;
                    case "zh-tw":
                    case "zhtw":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("zh-tw");
                        break;

                    case "template":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                        break;
                }
            }
            catch (Exception ex)
            {
                new DowJonesUtilitiesException(ex, -1);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            }

            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        public static string GetInterfaceLanguageFromBrowser()
        {
            var language = "en";
            if (HttpContext.Current != null)
            {
                var httpRequest = HttpContext.Current.Request;
                var httpAcceptLanguage = httpRequest.ServerVariables["HTTP_ACCEPT_LANGUAGE"];

                if (!string.IsNullOrEmpty(httpAcceptLanguage) && !string.IsNullOrEmpty(httpAcceptLanguage.Trim()))
                {
                    var acceptlang = httpAcceptLanguage.Split(new[] { ',' });
                    var brow = new BrowserAcceptedLangs[acceptlang.Length];
                    for (var i = 0; i < acceptlang.Length; i++)
                    {
                        brow[i] = new BrowserAcceptedLangs(acceptlang[i]);
                    }

                    Array.Sort(brow);
                    language = brow[0].Code.ToLower();
                    var index = language.IndexOf('-');
                    if (index > 0)
                    {
                        language = brow[0].Code.Substring(0, index);
                    }

                    return ValidateLanguageCodeWithTemplate(language);
                }
            }

            return language;
        }

        public static string[] GetSearchLanguages(string interfaceLanguage)
        {
            var httpRequest = HttpContext.Current.Request;

            // Always add English
            var ar = new List<string> { "en" };

            // Check to is if interface language is valid and add if not already there
            if (!string.IsNullOrEmpty(interfaceLanguage) &&
                Enum.IsDefined(typeof(Factiva.Gateway.Messages.Common.ContentLanguage), interfaceLanguage.ToLower()) &&
                !ar.Contains(interfaceLanguage.ToLower()))
            {
                ar.Add(interfaceLanguage.ToLower());
            }

            if (!string.IsNullOrEmpty(httpRequest.ServerVariables["HTTP_ACCEPT_LANGUAGE"]))
            {
                var acceptlang = httpRequest.ServerVariables["HTTP_ACCEPT_LANGUAGE"].Split(new[] { ',' });
                var brow = new BrowserAcceptedLangs[acceptlang.Length];
                for (var i = 0; i < acceptlang.Length; i++)
                {
                    brow[i] = new BrowserAcceptedLangs(acceptlang[i]);
                }

                Array.Sort(brow);
                foreach (BrowserAcceptedLangs t in brow)
                {
                    switch (t.Code)
                    {
                        default:
                            var temp = RegexDialectRemoval.Replace(t.Code, string.Empty);
                            if (Enum.IsDefined(typeof(Factiva.Gateway.Messages.Common.ContentLanguage), temp.ToLower()) &&
                                !ar.Contains(temp.ToLower()))
                            {
                                ar.Add(temp.ToLower());
                            }
                            else if (temp.ToLower() == "zh")
                            {
                                // add both Chinese if not added already
                                if (!ar.Contains(Factiva.Gateway.Messages.Common.ContentLanguage.zhcn.ToString()))
                                {
                                    ar.Add(Factiva.Gateway.Messages.Common.ContentLanguage.zhcn.ToString());
                                    ar.Add(Factiva.Gateway.Messages.Common.ContentLanguage.zhtw.ToString());
                                }
                            }

                            break;
                    }
                }
            }

            return ar.ToArray();
        }

        /// <summary>
        /// Returns a key value pair collection of language display name - language code for all available languages. Sorted by display name.
        /// </summary>
        public static List<KeyValuePair<string, string>> GetSortedLanguageList()
        {
            var translatedLanguageList = new Dictionary<string, string>();
            string langToken;
            foreach (ContentLanguage lang in Enum.GetValues(typeof(ContentLanguage)))
            {
                if (lang != ContentLanguage.fa)//Farsi language not allowed
                {
                    langToken = ((AssignedToken)Attribute.GetCustomAttribute(typeof(ContentLanguage).GetField(lang.ToString()), typeof(AssignedToken))).Token;
                    translatedLanguageList.Add(lang.ToString(), ResourceTextManager.Instance.GetString(langToken));
                }
            }

            var sortedList = new List<KeyValuePair<string, string>>(translatedLanguageList);
            sortedList.Sort(
                (firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value)
            );

            return sortedList;
        }
    }

    public class BrowserAcceptedLangs : IComparable
    {
        private readonly double qualityValue = 1.0;

        public BrowserAcceptedLangs(string langString)
        {
            if (langString == null || langString.Trim().Length == 0)
            {
                throw new NotSupportedException(langString);
            }

            var strs = langString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Code = strs[0];
            if (strs.Length == 2 && strs[1].IndexOf("q=") != -1)
            {
                double.TryParse(strs[1].Replace("q=", string.Empty), out qualityValue);
            }
        }

        public double QualityValue
        {
            get { return qualityValue; }
        }

        public string Code { get; set; }

        public int CompareTo(object obj)
        {
            var compareWith = obj as BrowserAcceptedLangs;
            if (compareWith != null)
            {
                if (compareWith.QualityValue > QualityValue)
                {
                    return 1;
                }

                return -1;
            }

            return -1;
        }
    }
}
