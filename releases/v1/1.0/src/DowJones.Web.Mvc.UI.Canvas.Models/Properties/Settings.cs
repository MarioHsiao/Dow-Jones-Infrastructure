using System;
using System.Configuration;
using DowJones.Utilities.Exceptions;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public sealed partial class Settings
    {
        public string GetDataServiceUrl(string key)
        {
            Guard.IsNotNullOrEmpty(key, "key");

            string url;

            try
            {
                url = Default[key] as string;
            }
            catch (SettingsPropertyNotFoundException settingsException)
            {
                var message = string.Format(
                    "Data Service Url setting was not specified for type {0}.  Consider overriding the {0}.DataServiceUrl property.",
                    key
                );

                throw new DowJonesUtilitiesException(message, settingsException);
            }

            if (string.IsNullOrWhiteSpace(url))
                return null;

            // this allows to override the base URL for settings
            // return the url as is if it doesn't start with a "/"
            return url.StartsWith("/") ? DashboardServiceBaseUrl + url : url;
        }
        
        public string GetDataServiceUrl(Type type)
        {
            Guard.IsNotNull(type, "type");

            string key = type.Name + "ServiceUrl";

            return GetDataServiceUrl(key);
            
        }

    }
}
