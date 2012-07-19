// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageMapper.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Language Mapper class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation.Providers.LanguageWeaver
{
    /// <summary>
    /// Language Mapper class
    /// </summary>
    public static class LanguageMapper
    {
        /// <summary>
        /// Tries the convert to iso639 language.
        /// </summary>
        /// <param name="contentLanguage">The content language.</param>
        /// <param name="iso639Language">The iso639 language.</param>
        /// <returns><c>true</c> if the conversion is successful; otherwise, <c>false</c>.</returns>
        public static bool TryConvertToIso639Language(ContentLanguage contentLanguage, out string iso639Language)
        {
            var isFound = true;

            iso639Language = null;
            switch (contentLanguage)
            {
                case ContentLanguage.bg:
                    iso639Language = "bul";
                    break;
                case ContentLanguage.zhtw:
                    iso639Language = "cht";
                    break;
                case ContentLanguage.zhcn:
                    iso639Language = "chi";
                    break;
                case ContentLanguage.cs:
                    iso639Language = "cze";
                    break;
                case ContentLanguage.da:
                    iso639Language = "dan";
                    break;
                case ContentLanguage.nl:
                    iso639Language = "dut";
                    break;
                case ContentLanguage.en:
                    iso639Language = "eng";
                    break;
                case ContentLanguage.fi:
                    iso639Language = "fin";
                    break;
                case ContentLanguage.fr:
                    iso639Language = "fra";
                    break;
                case ContentLanguage.de:
                    iso639Language = "ger";
                    break;
                case ContentLanguage.hu:
                    iso639Language = "hun";
                    break;
                case ContentLanguage.it:
                    iso639Language = "ita";
                    break;
                case ContentLanguage.ja:
                    iso639Language = "jpn";
                    break;
                case ContentLanguage.no:
                    iso639Language = "nor";
                    break;
                case ContentLanguage.pl:
                    iso639Language = "pol";
                    break;
                case ContentLanguage.pt:
                    iso639Language = "por";
                    break;
                case ContentLanguage.ru:
                    iso639Language = "rus";
                    break;
                case ContentLanguage.es:
                    iso639Language = "spa";
                    break;
                case ContentLanguage.sv:
                    iso639Language = "swe";
                    break;
                case ContentLanguage.tr:
                    iso639Language = "tur";
                    break;
                case ContentLanguage.ko:
                    iso639Language = "kor";
                    break;
                //Infosys-23/Oct/2010-START:Bahasa changes
                case ContentLanguage.id:
                    iso639Language = "ind";
                    break;
                //Infosys-23/Oct/2010-END:Bahasa changes
                default:
                    isFound = false;
                    break;
            }

            return isFound;
        }

        /// <summary>
        /// Tries the convert to content language.
        /// </summary>
        /// <param name="iso639Language">The iso639 language.</param>
        /// <param name="contentLanguage">The content language.</param>
        /// <returns><c>true</c> if the conversion is successful; otherwise, <c>false</c>.</returns>
        public static bool TryConvertToContentLanguage(string iso639Language, out ContentLanguage? contentLanguage)
        {
            var isFound = true;

            contentLanguage = null;
            switch (iso639Language)
            {
                case "bul":
                    contentLanguage = ContentLanguage.bg;
                    break;
                case "cht":
                    contentLanguage = ContentLanguage.zhtw;
                    break;
                case "chi":
                    contentLanguage = ContentLanguage.zhcn;
                    break;
                case "cze":
                    contentLanguage = ContentLanguage.cs;
                    break;
                case "dan":
                    contentLanguage = ContentLanguage.da;
                    break;
                case "dut":
                    contentLanguage = ContentLanguage.nl;
                    break;
                case "eng":
                    contentLanguage = ContentLanguage.en;
                    break;
                case "fin":
                    contentLanguage = ContentLanguage.fi;
                    break;
                case "fra":
                    contentLanguage = ContentLanguage.fr;
                    break;
                case "ger":
                    contentLanguage = ContentLanguage.de;
                    break;
                case "hun":
                    contentLanguage = ContentLanguage.hu;
                    break;
                case "ita":
                    contentLanguage = ContentLanguage.it;
                    break;
                case "jpn":
                    contentLanguage = ContentLanguage.ja;
                    break;
                case "nor":
                    contentLanguage = ContentLanguage.no;
                    break;
                case "pol":
                    contentLanguage = ContentLanguage.pl;
                    break;
                case "por":
                    contentLanguage = ContentLanguage.pt;
                    break;
                case "rus":
                    contentLanguage = ContentLanguage.ru;
                    break;
                case "spa":
                    contentLanguage = ContentLanguage.es;
                    break;
                case "swe":
                    contentLanguage = ContentLanguage.sv;
                    break;
                case "tur":
                    contentLanguage = ContentLanguage.tr;
                    break;
                case "kor":
                    contentLanguage = ContentLanguage.ko;
                    break;
                default:
                    isFound = false;
                    break;
            }

            return isFound;
        }
    }
}
