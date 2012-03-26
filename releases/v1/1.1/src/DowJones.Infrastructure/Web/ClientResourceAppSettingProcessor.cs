using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DowJones.Web
{
    public class ClientResourceAppSettingProcessor : IClientResourceProcessor
    {
        private static readonly Regex AppSettingExpression =
            new Regex(@"\<%\s*=\s*AppSetting\s*\(\s*""(?<Name>.+?)""\s*(,\s*""(?<Default>.+?)""\s*)?\)\s*%>",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);


        protected internal virtual NameValueCollection AppSettings
        {
            get { return _appSettings ?? ConfigurationManager.AppSettings; }
            set { _appSettings = value; }
        }
        private NameValueCollection _appSettings;


        public ClientResourceProcessorOrder? Order
        {
            get { return null; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Preprocessor; }
        }


        public void Process(ProcessedClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");

            if (!resource.HasContent)
                return;

            resource.Content = AppSettingExpression.Replace(resource.Content, ReplaceAppSettingMatch);
        }

        private string ReplaceAppSettingMatch(Match match)
        {
            var appSettingName = match.Groups["Name"].Value.Trim();
            var defaultValue = match.Groups["Default"].Value.Trim();

            var appSettingValue = AppSettings[appSettingName];

            if(string.IsNullOrWhiteSpace(appSettingValue))
                return defaultValue;

            return appSettingValue;
        }
    }
}