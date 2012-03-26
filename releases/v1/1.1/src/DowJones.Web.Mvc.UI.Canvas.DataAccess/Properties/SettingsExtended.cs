// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsExtended.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Configuration;
using System.Diagnostics;
using DowJones.Utilities.Configuration;
using DowJones.Utilities.Managers;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties 
{
    /// <summary>
    /// The settings.
    /// </summary>
    internal sealed partial class Settings
    {
        /// <summary>
        /// Gets or sets InternalReaderLightweightUser.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        [DefaultSettingValue("<lightWeightUser><userId>snap_proxy</userId><userPassword>pa55w0rd</userPassword><productId>16</productId><clientCodeType>D</clientCodeType><accessPointCodeUsage>SC</accessPointCodeUsage></lightWeightUser>")]
        public LightWeightUser DataAccessProxyUser
        {
            get { return (LightWeightUser)this["DataAccessProxyUser"]; }
            set { this["DataAccessProxyUser"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("e73a8e8ffcaa4646b8e6d12ffac23199")]
        public string ThunderBallEntitlementToken
        {
            get { return (string)this["ThunderBallEntitlementToken"]; }
            set { this["ThunderBallEntitlementToken"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://wsapi.marketwatch.com/ThunderBall/ChartService.svc")]
        public string ThunderBallEndpointAddress
        {
            get { return (string)this["ThunderBallEndpointAddress"]; }
            set { this["ThunderBallEndpointAddress"] = value; }
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
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("prod")]
        public string Environment
        {
            get { return (string)this["Environment"]; }
            set { this["Environment"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("617")]
        public int MaxNumberOfThreads
        {
            get { return (int)this["MaxNumberOfThreads"]; }
            set { this["MaxNumberOfThreads"] = value; }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheSyndicationNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheSyndicationNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheCompanyOverviewNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheCompanyOverviewNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheTopNewsNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheTopNewsNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheAlertsNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheAlertsNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheRadarNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheRadarNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheNewsstandNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheNewsstandNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheSourcesNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheSourcesNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheTrendingNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheTrendingNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheSummaryNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheSummaryNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheCustomTopicsNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheCustomTopicsNewsPageModuleService"];
            }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool CacheRegionalMapNewsPageModuleService
        {
            get
            {
                return (bool)this["CacheRegionalMapNewsPageModuleService"];
            }
        }

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
        public CacheExiprationPolicy DefaultCacheExpirationPolicy
        {
            get { return (CacheExiprationPolicy)this["DefaultCacheExpirationPolicy"]; }
            set { this["DefaultCacheExpirationPolicy"] = value; }
        }

        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValueAttribute("True")]
        public bool UsePcm
        {
            get
            {
                return (bool)this["UsePcm"];
            }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("Reuters Investor")]
        public string MarketDataProvider
        {
            get { return (string)this["MarketDataProvider"]; }
            set { this["MarketDataProvider"] = value; }
        }


        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://about.reuters.com/productinfo/")]
        public string MarketDataProviderUrl
        {
            get { return (string)this["MarketDataProviderUrl"]; }
            set { this["MarketDataProviderUrl"] = value; }
        }


        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("10000")]
        public int TrendingNumberOfEntitiesToRequest
        {
            get { return (int)this["TrendingNumberOfEntitiesToRequest"]; }
            set { this["TrendingNumberOfEntitiesToRequest"] = value; }
        }
    }
}
