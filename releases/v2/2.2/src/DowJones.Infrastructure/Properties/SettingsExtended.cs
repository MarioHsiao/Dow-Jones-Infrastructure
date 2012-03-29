// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsExtended.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Configuration;
using System.Diagnostics;
using System.Web;
using DowJones.Configuration;
using DowJones.Caching;

namespace DowJones.Properties
{
    /// <summary>
    /// The settings.
    /// </summary>
    public sealed partial class Settings
    {
        /// <summary>
        /// Gets DefaultAccessPointCode.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("O")]
        public string DefaultAccessPointCode
        {
            get { return (string)this["DefaultAccessPointCode"]; }
        }

        /// <summary>
        /// Gets DefaultProductPrefix.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("GL")]
        public string DefaultProductPrefix
        {
            get { return (string)this["DefaultProductPrefix"]; }
        }

        /// <summary>
        /// Get default product name used in direct URL for authentication.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("global")]
        public string DefaultProductName
        {
            get { return (string)this["DefaultProductName"]; }
        }



        /// <summary>
        /// Gets DefaultClientCodeType.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("D")]
        public string DefaultClientCodeType
        {
            get { return (string)this["DefaultClientCodeType"]; }
        }

        /// <summary>
        /// Gets DefaultInterfaceLanguage.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("en")]
        public string DefaultInterfaceLanguage
        {
            get { return (string)this["DefaultInterfaceLanguage"]; }
        }

        /// <summary>
        /// Gets DefaultFactivaPrefix.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("FP")]
        public string DefaultFactivaPrefix
        {
            get { return (string)this["DefaultFactivaPrefix"]; }
        }

        /// <summary>
        /// Gets EntitlementsCookieDomain.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue(".factiva.com")]
        public string EntitlementsCookieDomain
        {
            get { return (string)this["EntitlementsCookieDomain"]; }
        }

        /// <summary>
        /// Gets a value indicating whether EnableScriptResourceCompression.
        /// </summary>
        [ApplicationScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [DefaultSettingValueAttribute("False")]
        public bool EnableScriptResourceCompression
        {
            get { return (bool)this["EnableScriptResourceCompression"]; }
        }

        /// <summary>
        /// Gets EntitlementsCookiePath.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("/")]
        public string EntitlementsCookiePath
        {
            get { return (string)this["EntitlementsCookiePath"]; }
        }

        /// <summary>
        /// Gets or sets PodcastLightweightUser.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        [DefaultSettingValue("<lightWeightUser><userId>pcastfeed1</userId><userPassword>Pa55wd</userPassword><productId>16</productId><accessPointCode>AA</accessPointCode></lightWeightUser>")]
        public LightWeightUser PodcastLightweightUser
        {
            get { return (LightWeightUser)this["PodcastLightweightUser"]; }
            set { this["PodcastLightweightUser"] = value; }
        }
        
        /*
         
         <lightWeightUser name="InternalReaderLightWeightUser">
          <userId>inreaderlw</userId>
          <userPassword>pa55word</userPassword>
          <productId>16</productId>
        </lightWeightUser>
         
         */

        /// <summary>
        /// Gets or sets InternalReaderLightweightUser.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        [DefaultSettingValue("<lightWeightUser><userId>inreaderlw</userId><userPassword>pa55word</userPassword><productId>16</productId></lightWeightUser>")]
        public LightWeightUser InternalReaderLightweightUser
        {
            get { return (LightWeightUser)this["InternalReaderLightweightUser"]; }
            set { this["InternalReaderLightweightUser"] = value; }
        }

        /// <summary>
        /// Gets or sets LanguageWeaverApiKey.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("6a6b240a182a3c5341f41c9f39f557ff0fc68fe8")]
        public string LanguageWeaverApiKey
        {
            get { return (string)this["LanguageWeaverApiKey"]; }
            set { this["LanguageWeaverApiKey"] = value; }
        }

        /// <summary>
        /// Gets or sets LanguageWeaverApiKey.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("api.int.dowjones.com")]
        public string RestApiBaseHost
        {
            get { return (string)this["RestApiBaseHost"]; }
            set { this["RestApiBaseHost"] = value; }
        }

        /// <summary>
        /// Gets or sets DjConsultantConfigurationXmlRelativePath.
        /// </summary>
        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("~/configuration/data/ConsultantContentqueries_v1.0.xml")]
        public string DjConsultantConfigurationXmlRelativePath
        {
            get { return (string)this["DjConsultantConfigurationXmlRelativePath"]; }
            set { this["DjConsultantConfigurationXmlRelativePath"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool IsDebugMode
        {
            get
            {
                var debugMode = (bool)this["IsDebugMode"];

                // Are we in a web process?
                if(!debugMode && HttpContext.Current != null)
                {
                    debugMode = HttpContext.Current.IsDebuggingEnabled;
                }

                return debugMode;
            }
            set { this["IsDebugMode"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://api.int.dowjones.com/api")]
        public string DashboardServiceBaseUrl
        {
            get { return (string)this["DashboardServiceBaseUrl"]; }
            set { this["DashboardServiceBaseUrl"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("Dashboard/SearchRequest/1.0/xml")]
        public string SearchRequestServicePath
        {
            get { return (string)this["SearchRequestServicePath"]; }
            set { this["SearchRequestServicePath"] = value; }
        }

        [ApplicationScopedSetting]
        [DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("Dashboard/Headlines/1.0/list/xml")]
        public string HeadlinesServicePath
        {
            get { return (string)this["HeadlinesServicePath"]; }
            set { this["HeadlinesServicePath"] = value; }
        }




        //Caching start
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
        [DefaultSettingValue("60")]
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
        //Caching end
    }
}