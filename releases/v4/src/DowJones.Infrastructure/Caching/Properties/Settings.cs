using System.Configuration;

namespace DowJones.Caching
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
        public bool IncludeCacheKeyGeneration
        {
            get { return (bool)this["IncludeCacheKeyGeneration"]; }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("prod")]
        public string Environment
        {
            get { return (string)this["Environment"]; }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("1_0")]
        public string Version
        {
            get { return (string)this["Version"]; }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("30")]
        public int DefaultCacheExpirationTime
        {
            get { return (int)this["DefaultCacheExpirationTime"]; }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("15")]
        public int DefaultCacheRefreshInterval
        {
            get { return (int)this["DefaultCacheRefreshInterval"]; }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("Sliding")]
        public CacheExiprationPolicy DefaultCacheExpirationPolicy
        {
            get { return (CacheExiprationPolicy)this["DefaultCacheExpirationPolicy"]; }
        }

        [ApplicationScopedSetting, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheSubscribableTopics
        {
            get { return (bool)this["CacheSubscribableTopics"]; }
        }
    }
}
