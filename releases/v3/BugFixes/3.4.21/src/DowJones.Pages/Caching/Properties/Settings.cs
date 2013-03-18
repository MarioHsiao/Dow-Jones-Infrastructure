using System.Configuration;

namespace DowJones.Pages.Caching
{
    public class Settings : ApplicationSettingsBase
    {
        private static readonly Settings DefaultInstance = ((Settings)(Synchronized(new Settings())));

        public static Settings Default
        {
            get { return DefaultInstance; }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CachePageItems
        {
            get
            {
                return (bool)this["CachePageItems"];
            }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CachePageListItems
        {
            get
            {
                return (bool)this["CachePageListItems"];
            }
        }
    }
}
