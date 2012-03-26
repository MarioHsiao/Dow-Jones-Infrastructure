using System.Configuration;
using System.Diagnostics;
using DowJones.Configuration;

namespace DowJones.Pages
{
    public partial class Settings
    {
        [ApplicationScopedSetting, DebuggerNonUserCode, SettingsSerializeAs(SettingsSerializeAs.String)]
        [DefaultSettingValue("<lightWeightUser><userId>snap_proxy</userId><userPassword>pa55w0rd</userPassword><productId>16</productId><clientCodeType>D</clientCodeType><accessPointCodeUsage>SC</accessPointCodeUsage></lightWeightUser>")]
        public LightWeightUser PageMetadataProxyUser
        {
            get { return (LightWeightUser)this["PageMetadataProxyUser"]; }
            set { this["PageMetadataProxyUser"] = value; }
        }
    }
}
