﻿using System;
using System.Diagnostics;
using System.Globalization;
using DowJones.Utilities.Core;
using log4net;

namespace DowJones.Core
{
    public static class CultureManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CultureManager));

        public static CultureInfo GetCultureInfoFromInterfaceLanguage(string interfaceLanguage)
        {
            CultureInfo culture = null;

            interfaceLanguage = interfaceLanguage ?? string.Empty;

            InterfaceLanguage language;
            bool isValidLanguage = Enum.TryParse(interfaceLanguage, true, out language);

            if (isValidLanguage)
            {
                switch(language)
                {
                    case (InterfaceLanguage.template):
                        culture = CultureInfo.InvariantCulture;
                        break;

                    case (InterfaceLanguage.zhcn):
                        culture = new CultureInfo("zh-CN");
                        break;

                    case (InterfaceLanguage.zhtw):
                        culture = new CultureInfo("zh-TW");
                        break;

                    default:
                        try
                        {
                            culture = new CultureInfo(language.ToString());
                        }
                        catch(CultureNotFoundException notFoundException)
                        {
                            Debug.WriteLine("Could not find valid culture for interface language {0}", language);
                            Logger.Error(notFoundException);
                        }
                        break;
                }
            }

            return culture ?? CultureInfo.CurrentUICulture;
        }

    }
}
