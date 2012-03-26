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
            var preferencesResponse = _preferenceService.GetItemsByClassId(new[] {
                    PreferenceClassID.TimeZone, 
                    PreferenceClassID.TimeFormat, 
                    PreferenceClassID.SearchLanguage
                });

            var preferences = new DowJones.Preferences.Preferences(_session.InterfaceLanguage.ToString());

            // Clock type
            if (preferencesResponse.TimeFormat != null && preferencesResponse.TimeFormat.TimeFormat == PreferenceTimeFormat.HOURS12)
                preferences.ClockType = ClockType.TwelveHours;

            // Time Zone
            if (preferencesResponse.TimeZone != null)
                preferences.TimeZone = preferencesResponse.TimeZone.ToString();

            // Content Languages
            if (preferencesResponse.SearchLanguage != null && !string.IsNullOrWhiteSpace(preferencesResponse.SearchLanguage.SearchLanguage))
            {
                foreach (var language in preferencesResponse.SearchLanguage.SearchLanguage.Split(','))
                {
                    var lang = language.Trim().ToLower();
                    if (lang == "all")
                    {
                        preferences.ContentLanguages.Clear();
                        break;
                    }
                    preferences.ContentLanguages.Add(lang);
                }
            }

            return preferences;
        }
    }
}