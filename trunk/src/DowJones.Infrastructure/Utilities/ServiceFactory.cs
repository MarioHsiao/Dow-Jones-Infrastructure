using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Extensions;
using DowJones.PALService;
using ServiceObjects = DowJones.PALService;
using DowJones.Session;
using DowJones.Properties;

namespace DowJones.Utilities
{
    public sealed class ServiceFactory<T> where T : class
    {
        private ServiceFactory() { }

        public static T Create(IControlData controlData)
        {
            var serviceControlData = GetUserCredentialData(controlData);

            var targetObject = (T)Activator.CreateInstance(typeof(T));

            if (targetObject == null)
                throw new ArgumentException("Can't instantiate type " + typeof(T).Name);

            targetObject.SetPropertyValue("ControlDataValue", serviceControlData);

            if (targetObject.GetPropertyValue("ControlDataValue").GetPropertyValue("RoutingData") == null)
            {
                //Using RTS
                var routingData = new RoutingData
                {
                    TransportType = Settings.Default.ProxyPageTransportType,
                    ServiceUrl = targetObject.GetPropertyValue("Url").ToString(),
                    KeyNameExtension = Settings.Default.ProxyPageKeyNameExtension
                };

                targetObject.GetPropertyValue("ControlDataValue").SetPropertyValue("RoutingData", routingData);
                targetObject.SetPropertyValue("Url", Settings.Default.ProxyPageUrl);
            }

            return targetObject;
        }

        private static ServiceObjects.ControlData GetUserCredentialData(IControlData controlData)
        {
            if (string.IsNullOrEmpty(controlData.SessionID))
                throw new ArgumentNullException("control data sessionId is null");

            var tempControlData = ControlDataManager.Clone(controlData);


            var serviceControlData = new ServiceObjects.ControlData
            {
                UserCredentialData = new UserCredentialData
                {
                    SessionId = tempControlData.SessionID,
                }
            };

            if (!string.IsNullOrWhiteSpace(tempControlData.CacheKey))
            {
                serviceControlData.TransactionCacheData = new TransactionCacheData
                {
                    CacheApplication = tempControlData.CacheApplication,
                    CacheExpirationPolicy = StringToEnum<CacheExpirationPolicy>(tempControlData.CacheExpirationPolicy),
                    CacheExpirationPolicySpecified = true,
                    CacheExpirationTime = tempControlData.CacheExpirationTime,
                    CacheKey = tempControlData.CacheKey,
                    CacheRefreshInterval = tempControlData.CacheRefreshInterval,
                    CacheScope = StringToEnum<CacheScope>(tempControlData.CacheScope),
                    CacheScopeSpecified = true,
                    CacheStatus = tempControlData.CacheStatus.ToString(),
                    CacheWait = tempControlData.CacheWait
                };
            }

            return serviceControlData;
        }

        private static TEnum StringToEnum<TEnum>(string name)
        {
            var names = Enum.GetNames(typeof(TEnum));
            if (((IList)names).Contains(name))
            {
                return (TEnum)Enum.Parse(typeof(TEnum), name);
            }

            return default(TEnum);
        }
    }
}
