using System;
using System.Configuration;
using DowJones.Exceptions;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public partial class CanvasSettings
    {
        public string GetDataServiceUrl(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return null;

            // this allows to override the base URL for settings
            // return the url as is if it doesn't start with a "/"
            return relativeUrl.StartsWith("/") ? DashboardServiceBaseUrl + relativeUrl : relativeUrl;
        }

        public string GetDataServiceUrl(string key, ApplicationSettingsBase settings)
        {
            Guard.IsNotNullOrEmpty(key, "key");

            string url;

            try
            {
                url = settings[key] as string;
            }
            catch (SettingsPropertyNotFoundException settingsException)
            {
                var message = string.Format(
                    "Data Service Url setting was not specified for type {0}.  Consider overriding the {0}.DataServiceUrl property.",
                    key
                    );

                throw new DowJonesUtilitiesException(message, settingsException);
            }

            return GetDataServiceUrl(url);
        }

        public string GetDataServiceUrl(Type type, ApplicationSettingsBase settings)
        {
            Guard.IsNotNull(type, "type");

            string key = type.Name + "ServiceUrl";

            return GetDataServiceUrl(key, settings);
        }
    }
}