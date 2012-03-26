using System.Configuration;
using System.Diagnostics;
using DowJones.Configuration;

namespace DowJones.Pages
{
    public partial class Settings
    {
        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CachePageItems
        {
            get
            {
                return (bool)this["CachePageItems"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CachePageListItems
        {
            get
            {
                return (bool)this["CachePageListItems"];
            }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("30")]
        public int DefaultCacheExpirationTime
        {
            get { return (int)this["DefaultCacheExpirationTime"]; }
            set { this["DefaultCacheExpirationTime"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("15")]
        public int DefaultCacheRefreshInterval
        {
            get { return (int)this["DefaultCacheRefreshInterval"]; }
            set { this["DefaultCacheRefreshInterval"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("Sliding")]
        public DowJones.Caching.CacheExiprationPolicy DefaultCacheExpirationPolicy
        {
            get { return (DowJones.Caching.CacheExiprationPolicy)this["DefaultCacheExpirationPolicy"]; }
            set { this["DefaultCacheExpirationPolicy"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("prod")]
        public string Environment
        {
            get { return (string)this["Environment"]; }
            set { this["Environment"] = value; }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool IncludeCacheKeyGeneration
        {
            get
            {
                return (bool)this["IncludeCacheKeyGeneration"];
            }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("1_0")]
        public string Version
        {
            get { return (string)this["Version"]; }
            set { this["Version"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        [DefaultSettingValue("<lightWeightUser><userId>snap_proxy</userId><userPassword>pa55w0rd</userPassword><productId>16</productId><clientCodeType>D</clientCodeType><accessPointCodeUsage>SC</accessPointCodeUsage></lightWeightUser>")]
        public LightWeightUser PageMetadataProxyUser
        {
            get { return (LightWeightUser)this["PageMetadataProxyUser"]; }
            set { this["PageMetadataProxyUser"] = value; }
        }
    }
}
