// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Preferences.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;

namespace DowJones.Preferences
{
    [DataContract(Name = "preferences", Namespace = "")]
    public class Preferences : IPreferences
    {
        private static readonly Predicate<string> IsInvalidContentLanguage =
            contentLanguage => string.IsNullOrWhiteSpace(contentLanguage)
                            || contentLanguage.Equals("All", StringComparison.InvariantCultureIgnoreCase) ;

        private ContentLanguageCollection contentLanguages;
        private string interfaceLanguage;

        public Preferences() : this(null)
        {
            TimeZone = "+00:00|1";
        }

        public Preferences(string interfaceLanguage)
        {
            contentLanguages = new ContentLanguageCollection();
            ClockType = ClockType.TwelveHours;
            InterfaceLanguage = interfaceLanguage;
        }

        [DataMember(Name = "interfaceLanguage")]
        public string InterfaceLanguage
        {
            get { return interfaceLanguage; }
            set { interfaceLanguage = LanguageUtilityManager.ValidateLanguageCode(value); }
        }

        [DataMember(Name = "timeZone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "clockType")]
        public ClockType ClockType { get; set; }

        public int DebugInfo { get; set; }

        [DataMember(Name = "contentLanguages")]
        public ContentLanguageCollection ContentLanguages
        {
            get
            {
                return contentLanguages ?? (contentLanguages = new ContentLanguageCollection());
            }

            set
            {
                var languages = value ?? new ContentLanguageCollection();
                languages.RemoveWhere(IsInvalidContentLanguage);
                contentLanguages = languages;
            }
        }

        public void AddContentLanguage(string contentLanguage)
        {
            if (!IsInvalidContentLanguage(contentLanguage))
                ContentLanguages.Add(contentLanguage);
        }
    }
}
