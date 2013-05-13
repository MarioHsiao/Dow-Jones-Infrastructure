using System.Linq;
using DowJones.Exceptions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure;
using DowJones.Preferences;
using DowJones.Session;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Assemblers.Preferences
{
    public class PreferencesFactory : Factory<IPreferences>
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IUserSession _session;

        public PreferencesFactory(IPreferenceService preferenceService, IUserSession session)
        {
            _preferenceService = preferenceService;
            _session = session;
        }

        public override IPreferences Create()
        {
            IPreferences preferences = new DowJones.Preferences.Preferences(_session.InterfaceLanguage.ToString());

            if(!_session.IsValid())
                return preferences;

            var preferencesResponse = _preferenceService.GetItemsByClassId(new[] {
                    PreferenceClassID.TimeZone, 
                    PreferenceClassID.TimeFormat, 
                    PreferenceClassID.SearchLanguage
                });

            var responseCode = (preferencesResponse == null) ? -1 : preferencesResponse.rc;
            if (responseCode != 0)
            {
                // If we think our session is valid, but the preferences server says it's not, 
                // invalidate our local instance to avoid more invalid Gateway calls down the line
                if (responseCode == DowJonesUtilitiesException.ErrorInvalidSessionLong)
                    _session.Invalidate();

                return preferences;
            }

            // Clock type
            if (preferencesResponse != null)
            {
                if (preferencesResponse.TimeFormat != null && preferencesResponse.TimeFormat.TimeFormat == PreferenceTimeFormat.HOURS12)
                    preferences.ClockType = ClockType.TwelveHours;

                // Time Zone
                if (preferencesResponse.TimeZone != null)
                    preferences.TimeZone = preferencesResponse.TimeZone.ToString();

                // Content Languages
                if (preferencesResponse.SearchLanguage != null && !string.IsNullOrWhiteSpace(preferencesResponse.SearchLanguage.SearchLanguage))
                {
                    foreach (var lang in preferencesResponse.SearchLanguage.SearchLanguage.Split(',').Select(language => language.Trim().ToLower()))
                    {
                        if (lang == "all")
                        {
                            preferences.ContentLanguages.Clear();
                            break;
                        }
                        preferences.ContentLanguages.Add(lang);
                    }
                }
            }

            return preferences;
        }
    }
}