// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsExtended.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Configuration;
using System.Diagnostics;
using System.Web;
using DowJones.Configuration;

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
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("O")]
        public string DefaultAccessPointCode
        {
            get { return (string)this["DefaultAccessPointCode"]; }
        }

        /// <summary>
        /// Gets DefaultProductPrefix.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("GL")]
        public string DefaultProductPrefix
        {
            get { return (string)this["DefaultProductPrefix"]; }
        }

        /// <summary>
        /// Get default product name used in direct URL for authentication.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("global")]
        public string DefaultProductName
        {
            get { return (string)this["DefaultProductName"]; }
        }



        /// <summary>
        /// Gets DefaultClientCodeType.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("D")]
        public string DefaultClientCodeType
        {
            get { return (string)this["DefaultClientCodeType"]; }
        }

        /// <summary>
        /// Gets DefaultInterfaceLanguage.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("en")]
        public string DefaultInterfaceLanguage
        {
            get { return (string)this["DefaultInterfaceLanguage"]; }
        }

        /// <summary>
        /// Gets DefaultFactivaPrefix.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("FP")]
        public string DefaultFactivaPrefix
        {
            get { return (string)this["DefaultFactivaPrefix"]; }
        }

        /// <summary>
        /// Gets EntitlementsCookieDomain.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue(".factiva.com")]
        public string EntitlementsCookieDomain
        {
            get { return (string)this["EntitlementsCookieDomain"]; }
        }

        /// <summary>
        /// Gets a value indicating whether EnableScriptResourceCompression.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValueAttribute("False")]
        public bool EnableScriptResourceCompression
        {
            get { return (bool)this["EnableScriptResourceCompression"]; }
        }

        /// <summary>
        /// Gets EntitlementsCookiePath.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("/")]
        public string EntitlementsCookiePath
        {
            get { return (string)this["EntitlementsCookiePath"]; }
        }

        /// <summary>
        /// Gets or sets PodcastLightweightUser.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
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
        [ApplicationScopedSetting, DebuggerNonUserCode]
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
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("6a6b240a182a3c5341f41c9f39f557ff0fc68fe8")]
        public string LanguageWeaverApiKey
        {
            get { return (string)this["LanguageWeaverApiKey"]; }
            set { this["LanguageWeaverApiKey"] = value; }
        }

        /// <summary>
        /// Gets or sets LanguageWeaverApiKey.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("api.int.dowjones.com")]
        public string RestApiBaseHost
        {
            get { return (string)this["RestApiBaseHost"]; }
            set { this["RestApiBaseHost"] = value; }
        }

        /// <summary>
        /// Gets or sets DjConsultantConfigurationXmlRelativePath.
        /// </summary>
        [ApplicationScopedSetting, DebuggerNonUserCode]
        [DefaultSettingValue("~/configuration/data/ConsultantContentqueries_v1.0.xml")]
        public string DjConsultantConfigurationXmlRelativePath
        {
            get { return (string)this["DjConsultantConfigurationXmlRelativePath"]; }
            set { this["DjConsultantConfigurationXmlRelativePath"] = value; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode]
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

        [ApplicationScopedSetting, DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://api.int.dowjones.com/api")]
        public string DashboardServiceBaseUrl
        {
            get { return (string)this["DashboardServiceBaseUrl"]; }
            set { this["DashboardServiceBaseUrl"] = value; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("Dashboard/SearchRequest/1.0/xml")]
        public string SearchRequestServicePath
        {
            get { return (string)this["SearchRequestServicePath"]; }
            set { this["SearchRequestServicePath"] = value; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode]
        [SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("Dashboard/Headlines/1.0/list/xml")]
        public string HeadlinesServicePath
        {
            get { return (string)this["HeadlinesServicePath"]; }
            set { this["HeadlinesServicePath"] = value; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("e73a8e8ffcaa4646b8e6d12ffac23199")]
        public string ThunderBallEntitlementToken
        {
            get { return (string)this["ThunderBallEntitlementToken"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://10.241.32.211/thunderball/ChartService.svc")]
        public string ThunderBallEndpointAddress
        {
            get { return (string)this["ThunderBallEndpointAddress"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://10.241.32.225/dylan2011.webhost/Symbology.svc")]
        public string DylanSymbologyEndpointAddress
        {
            get { return (string)this["DylanSymbologyEndpointAddress"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://10.241.32.225/dylan2011.webhost/Instrument.svc")]
        public string DylanInstrumentEndpointAddress
        {
            get { return (string)this["DylanInstrumentEndpointAddress"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("SIX-Telekurs")]
        public string MarketDataProvider
        {
            get { return (string)this["MarketDataProvider"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://www.SIX-Telekurs.com")]
        public string MarketDataProviderUrl
        {
            get { return (string)this["MarketDataProviderUrl"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("617")]
        public int MaxNumberOfThreads
        {
            get { return (int)this["MaxNumberOfThreads"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://global.factiva.com/FactivaLogos/")]
        public string LogosSiteUrl
        {
            get { return (string)this["LogosSiteUrl"]; } 
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://dataservices.ramp.com/data/factiva/media/?e=")]
        public string RampUri
        {
            get { return (string)this["RampUri"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("http://www.marketwatch.com/mediarss/wsj/allmedia.asp?type=guid&query=")]
        public string MarketWatchUri
        {
            get { return (string)this["MarketWatchUri"]; }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("CORS")]
        public string CrossDomainTransport
        {
            get { return (string)this["CrossDomainTransport"]; }
        }
    }
}