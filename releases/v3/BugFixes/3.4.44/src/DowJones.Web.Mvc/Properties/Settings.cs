using System;

namespace DowJones.Web.Mvc.Properties
{
    partial class Settings
    {
        [Obsolete("Use DowJones.Properties.Settings.LanguageRequestParameter instead")]
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("FCPIL")]
        public string LanguageRequestParameter
        {
            get
            {
                return ((string)(this["LanguageRequestParameter"]));
            }
        }
    }
}