// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasePreferences.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Core;

namespace DowJones.Session
{
    [CollectionDataContract(Name = "contentLanguages", ItemName = "contentLanguage", Namespace = "")]
    public class ContentLanguageCollection : SortedSet<string>
    {
        public ContentLanguageCollection()
        {
        }

        public ContentLanguageCollection(IEnumerable<string> range)
            : base(range)
        {
        }
    }

    [DataContract(Name = "preferences", Namespace = "")]
    public class BasePreferences : IPreferences
    {
        private ContentLanguageCollection contentLanguages;
        private string interfaceLanguage;

        public BasePreferences()
        {
            InterfaceLanguage = "en";
            contentLanguages = new ContentLanguageCollection();
        }

        public BasePreferences(string interfaceLanguage)
        {
            InterfaceLanguage = LanguageUtilityManager.ValidateLanguageCode(interfaceLanguage);
            contentLanguages = new ContentLanguageCollection();
        }

        [DataMember(Name = "interfaceLanguage")]
        public string InterfaceLanguage
        {
            get
            {
                if (interfaceLanguage.IsNullOrEmpty())
                {
                    interfaceLanguage = "en";
                }

                return interfaceLanguage;
            }

            set
            {
                interfaceLanguage = LanguageUtilityManager.ValidateLanguageCode(value);
            }
        }

        [DataMember(Name = "timeZone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "clockType")]
        public ClockType ClockType { get; set; }

        // [DataMember(Name = "debugInfo")]
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
                value.RemoveWhere(contentLanguage => contentLanguage.Equals("all", StringComparison.InvariantCultureIgnoreCase) || contentLanguage.Equals(string.Empty, StringComparison.InvariantCultureIgnoreCase));
                contentLanguages = value;
            }
        }
    }
}
