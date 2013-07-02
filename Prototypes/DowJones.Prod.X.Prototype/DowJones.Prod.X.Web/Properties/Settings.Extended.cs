namespace DowJones.Prod.X.Web.Properties
{
    internal sealed partial class Settings
    {

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}://{1}/factivalogin/privacypolicy/privacypolicy_{2}.html?productname={3}")]
        public string PrivacyPolicyUrl
        {
            get
            {
                return ((string)(this["PrivacyPolicyUrl"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.facebook.com/dowjones")]
        public string FacebookUrl
        {
            get
            {
                return ((string)(this["FacebookUrl"]));
            }
        }


        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.twitter.com/dowjones")]
        public string TwitterUrl
        {
            get
            {
                return ((string)(this["TwitterUrl"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.youtube.com/dowjones")]
        public string YouTubeUrl
        {
            get
            {
                return ((string)(this["YouTubeUrl"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.factiva.com")]
        public string MarketingSiteUrl
        {
            get
            {
                return ((string)(this["MarketingSiteUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("djfactivatesting")]
        public string UsageTrackingAccount
        {
            get
            {
                return ((string)this["UsageTrackingAccount"]);
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("true")]
        public bool UsageTrackingOn
        {
            get
            {
                return ((bool)this["UsageTrackingOn"]);
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9FAC029300")]
        public string SkipUsageTrackingAccounts
        {
            get
            {
                return ((string)this["SkipUsageTrackingAccounts"]);
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://customer.int.factiva.com")]
        public string CustomerPortalUrl
        {
            get
            {
                return ((string)this["CustomerPortalUrl"]);
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://suggest.int.dowjones.com/Search/1.0")]
        public string SuggestServiceUrl
        {
            get
            {
                return ((string)this["SuggestServiceUrl"]);
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("75a6c4404d9ffa80a63")]
        public string FlowPlayerKey
        {
            get
            {
                return ((string)(this["FlowPlayerKey"]));
            }
        }
    }
}