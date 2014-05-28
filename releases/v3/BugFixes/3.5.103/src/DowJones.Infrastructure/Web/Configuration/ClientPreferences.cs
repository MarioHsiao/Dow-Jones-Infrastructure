using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;
using DowJones.Preferences;
using Newtonsoft.Json;

namespace DowJones.Web.Configuration
{
    /// <summary>
    /// ClientPreferences class
    /// </summary>
    public class ClientPreferences
    {
        /// <summary>The type of the clock.</summary>
        [JsonProperty("adjustToDST",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool AdjustToDaylightSavingsTime { get; set; }

        /// <summary>The type of the clock.</summary>
        [JsonProperty("clockType",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ClockType ClockType { get; set; }

        /// <summary>The type of the clock.</summary>
        [JsonProperty("convertToLocalTime",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ConvertToLocalTime { get; set; }

        /// <summary>The content languages.</summary>
        [JsonProperty("contentLanguages",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ContentLanguageCollection ContentLanguages { get; set; }

        /// <summary>The interface language.</summary>
        [JsonProperty("interfaceLanguage",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string InterfaceLanguage { get; set; }

        /// <summary>The local time zone.</summary>
        [JsonProperty("timeZone",
                      NullValueHandling = NullValueHandling.Ignore,
                      DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TimeZone { get; set; }


        public ClientPreferences()
        {
        }

        public ClientPreferences(IPreferences preferences)
        {
            ClockType = preferences.ClockType;
            ContentLanguages = preferences.ContentLanguages;
            InterfaceLanguage = preferences.InterfaceLanguage;
            TimeZone = preferences.TimeZone;
        }
    }
}