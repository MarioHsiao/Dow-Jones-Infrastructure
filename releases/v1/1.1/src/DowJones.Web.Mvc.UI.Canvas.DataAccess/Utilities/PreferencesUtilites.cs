// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreferencesUtilites.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Session;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Services.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
    public class PreferencesUtilites
    {
        public static BasePreferences GetBasePreferences(ControlData controlData, string interfaceLanguage = "en")
        {
            var list = new List<PreferenceClassID>
                           {
                               PreferenceClassID.TimeZone,
                               PreferenceClassID.TimeFormat,
                               PreferenceClassID.SearchLanguage,
                           };
            return GetPreferences(controlData, interfaceLanguage, list);
        }

        private static BasePreferences GetPreferences(ControlData controlData, string interfaceLanguage, List<PreferenceClassID> list)
        {
            var request = new GetItemsByClassIDRequest
            {
                ClassID = list.ToArray(),
                ReturnBlob = true,
            };

            var preferenceResponse = PreferenceService.GetItemsByClassID(ControlDataManager.Clone(controlData), request);

            var response = new BasePreferences(interfaceLanguage)
            {
                ClockType = MapClockType(preferenceResponse.TimeFormat.TimeFormat),
                DebugInfo = 0,
                InterfaceLanguage = interfaceLanguage,
                TimeZone = preferenceResponse.TimeZone.ToString(),
                ContentLanguages = GetContentLanguages(preferenceResponse.SearchLanguage.SearchLanguage),
            };
            return response;
        }

        private static ContentLanguageCollection GetContentLanguages(string prefStr)
        {
            var temp = prefStr.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (prefStr.IsNullOrEmpty() || 
                prefStr.Equals("all", StringComparison.InvariantCultureIgnoreCase) ||
                temp.Length == 0)
            {
                return new ContentLanguageCollection();                
            }
            
            return new ContentLanguageCollection(temp);
        }

        private static ClockType MapClockType(PreferenceTimeFormat timeFormat)
        {
            switch (timeFormat)
            {
                case PreferenceTimeFormat.HOURS12:
                    return ClockType.TwelveHours;
                default:
                    return ClockType.TwentyFourHours;
            }
        }
    }
}
