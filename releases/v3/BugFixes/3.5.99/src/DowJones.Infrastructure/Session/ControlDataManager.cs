// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlDataManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Configuration;

namespace DowJones.Session
{
    /// <summary>
    /// Summary description for ControlDataManager.
    /// </summary>
    public class ControlDataManager
    {
        public static IControlData GetControlDataForTransactionCache(IControlData controlData, TransactionCacheControlData transControlData)
        {
            return GetControlDataForTransactionCache(controlData,
                transControlData.Key,
                transControlData.Scope,
                transControlData.ExpirationTime,
                transControlData.RefreshInterval,
                transControlData.ExiprationPolicy,
                transControlData.ForceCacheRefresh,
                transControlData.Wait);
        }

        /// <summary>
        /// Gets the control data for transaction cache.
        /// </summary>
        /// <param name="sessionUserControlData">session-user controlData.</param>
        /// <param name="cacheKey">CacheKey is a string with a maximum length of 64 characters. 
        /// The valid characters are [A..Z],[a..z],[0..9] and the underscore character.  Transactions 
        /// with a CacheKey that do not meet this specification will return an error. </param>
        /// <param name="cacheScope">CacheScope indicates how a Cache Item is shared by different 
        /// Sessions.If CacheScope is ‘Session’, the Cache Item is not shared and is only relevant 
        /// for the Session in which it was created. If CacheScope is ‘Account’, the Cache Item is shared by all 
        /// Sessions created by users in the same account.  If CacheScope is ‘All’, the Cache Item 
        /// is shared by all Sessions.  If the CacheScope parameter is not supplied or the value 
        /// of CacheScope is not one of the valid values described above, an error will be 
        /// returned for this transaction.</param>
        /// <param name="cacheExpirationTime">CacheExpirationTime is a value expressed in minutes 
        /// that indicates how long Session Server should save a Cache Item.  Session Server will remove 
        /// a Cache Item when the number of minutes specified by CacheExpirationTime has elapsed from 
        /// either the Cache Item creation time or last access time (see optional CacheExpirationPolicy 
        /// below). For Cache Items with a Cache Scope of ‘Session’, the Cache Item will be removed when 
        /// the Session is terminated by either a user logout or a Session timeout even if the Cache 
        /// Expiration Time has not been reached. If CacheExpirationTime is not supplied, an error will be 
        /// returned for this transaction.</param>
        /// <param name="cacheRefreshInterval">CacheRefreshInterval is a value expressed in minutes that 
        /// indicates how often  Session Server should perform backend transactions in order to refresh 
        /// the contents of the Cache Item.  The backend transaction performed will be the Request 
        /// that was saved when the Cache Item was created. If CacheRefreshInterval is not supplied, an 
        /// error will be returned for this transaction. </param>
        /// <param name="cacheExiprationPolicy">CacheExpirationPolicy determines how the CacheExpirationTime 
        /// value will be construed. If CacheExpirationPolicy is ‘Absolute’, the Cache Item will be removed 
        /// when the number of minutes specified by CacheExpirationTime has elapsed from the time the Cache Item 
        /// was created.  If CacheExpirationPolicy is ‘Sliding’, the Cache Item will be removed 
        /// when the number of minutes specified by CacheExpirationTime has elapsed from the last 
        /// time the Cache Item was accessed.  The default value for CacheExpirationPolicy is 
        /// ‘Absolute’.  For Cache Items with CacheScope set to ‘Session’, the Cache Item will be 
        /// removed when the Session is terminated by either a user logout or a Session timeout 
        /// even if the CacheExpirationTime has not been reached. If the value of CacheExpirationPolicy 
        /// is not one of the valid values described above, an error will be returned for this transaction.</param>
        /// <param name="forceCacheRefresh">ForceCacheRefresh is a Boolean parameter that if set to ‘True’,
        /// triggers Session Server to send this transaction to the backend and update the Cache Item 
        /// associated with the supplied CacheKey and CacheScope.  Session Server will replace the saved Request, 
        /// CacheExpirationTime, CacheRefreshInterval and CacheExpirationPolicy with those supplied 
        /// with this transaction.</param>
        /// <returns></returns>
        public static IControlData GetControlDataForTransactionCache(IControlData sessionUserControlData, string cacheKey, Caching.CacheScope cacheScope, int cacheExpirationTime, int cacheRefreshInterval, Caching.CacheExiprationPolicy cacheExiprationPolicy, bool forceCacheRefresh)
        {
            return GetControlDataForTransactionCache(sessionUserControlData, cacheKey, cacheScope, cacheExpirationTime, cacheRefreshInterval, cacheExiprationPolicy, forceCacheRefresh, true);
        }

        /// <summary>
        /// Gets the control data for transaction cache.
        /// </summary>
        /// <param name="sessionUserControlData">session-user controlData.</param>
        /// <param name="cacheKey">CacheKey is a string with a maximum length of 64 characters. 
        /// The valid characters are [A..Z],[a..z],[0..9] and the underscore character.  Transactions 
        /// with a CacheKey that do not meet this specification will return an error. </param>
        /// <param name="cacheScope">CacheScope indicates how a Cache Item is shared by different 
        /// Sessions.If CacheScope is ‘Session’, the Cache Item is not shared and is only relevant 
        /// for the Session in which it was created. If CacheScope is ‘Account’, the Cache Item is shared by all 
        /// Sessions created by users in the same account.  If CacheScope is ‘All’, the Cache Item 
        /// is shared by all Sessions.  If the CacheScope parameter is not supplied or the value 
        /// of CacheScope is not one of the valid values described above, an error will be 
        /// returned for this transaction.</param>
        /// <param name="cacheExpirationTime">CacheExpirationTime is a value expressed in minutes 
        /// that indicates how long Session Server should save a Cache Item.  Session Server will remove 
        /// a Cache Item when the number of minutes specified by CacheExpirationTime has elapsed from 
        /// either the Cache Item creation time or last access time (see optional CacheExpirationPolicy 
        /// below). For Cache Items with a Cache Scope of ‘Session’, the Cache Item will be removed when 
        /// the Session is terminated by either a user logout or a Session timeout even if the Cache 
        /// Expiration Time has not been reached. If CacheExpirationTime is not supplied, an error will be 
        /// returned for this transaction.</param>
        /// <param name="cacheRefreshInterval">CacheRefreshInterval is a value expressed in minutes that 
        /// indicates how often  Session Server should perform backend transactions in order to refresh 
        /// the contents of the Cache Item.  The backend transaction performed will be the Request 
        /// that was saved when the Cache Item was created. If CacheRefreshInterval is not supplied, an 
        /// error will be returned for this transaction. </param>
        /// <param name="cacheExiprationPolicy">CacheExpirationPolicy determines how the CacheExpirationTime 
        /// value will be construed. If CacheExpirationPolicy is ‘Absolute’, the Cache Item will be removed 
        /// when the number of minutes specified by CacheExpirationTime has elapsed from the time the Cache Item 
        /// was created.  If CacheExpirationPolicy is ‘Sliding’, the Cache Item will be removed 
        /// when the number of minutes specified by CacheExpirationTime has elapsed from the last 
        /// time the Cache Item was accessed.  The default value for CacheExpirationPolicy is 
        /// ‘Absolute’.  For Cache Items with CacheScope set to ‘Session’, the Cache Item will be 
        /// removed when the Session is terminated by either a user logout or a Session timeout 
        /// even if the CacheExpirationTime has not been reached. If the value of CacheExpirationPolicy 
        /// is not one of the valid values described above, an error will be returned for this transaction.</param>
        /// <param name="forceCacheRefresh">ForceCacheRefresh is a Boolean parameter that if set to ‘True’,
        /// triggers Session Server to send this transaction to the backend and update the Cache Item 
        /// associated with the supplied CacheKey and CacheScope.  Session Server will replace the saved Request, 
        /// CacheExpirationTime, CacheRefreshInterval and CacheExpirationPolicy with those supplied 
        /// with this transaction.</param>
        /// <param name="cacheWait">CacheWait is a enumeration parameter that if set to ‘True’,
        /// triggers Session Server to send this transaction to the backend and update the Cache Item 
        /// associated with the supplied CacheKey and CacheScope.  Session Server will replace the saved Request, 
        /// CacheExpirationTime, CacheRefreshInterval and CacheExpirationPolicy with those supplied 
        /// with this transaction.</param> 
        /// <returns></returns>
        public static IControlData GetControlDataForTransactionCache(IControlData sessionUserControlData, string cacheKey, Caching.CacheScope cacheScope, int cacheExpirationTime, int cacheRefreshInterval, Caching.CacheExiprationPolicy cacheExiprationPolicy, bool forceCacheRefresh, bool cacheWait)
        {
            var controlData = Clone(sessionUserControlData);

            controlData.CacheKey = cacheKey;
            controlData.CacheScope = cacheScope.ToString();
            controlData.CacheExpirationTime = cacheExpirationTime.ToString();
            controlData.CacheRefreshInterval = cacheRefreshInterval.ToString();
            if (cacheExiprationPolicy != Caching.CacheExiprationPolicy.None)
            {
                controlData.CacheExpirationPolicy = cacheExiprationPolicy.ToString();
            }

            controlData.ForceCacheRefresh = forceCacheRefresh ? "True" : "False";
            controlData.CacheWait = cacheWait ? "True" : "False";
            return controlData;
        }


        public static Factiva.Gateway.Utils.V1_0.ControlData Convert(IControlData source, bool copyContentServerAddress = false)
        {
            if (source == null)
                return null;

            var controlData = new Factiva.Gateway.Utils.V1_0.ControlData
            {
                UserID = source.UserID,
                AccessPointCode = source.AccessPointCode,
                AccessPointCodeUsage = source.AccessPointCodeUsage,
                CacheKey = source.CacheKey,
                ClientCode = source.ClientCode,
                ClientType = source.ClientType,
                EncryptedLogin = source.EncryptedToken,
                ProductID = source.ProductID,
                SessionID = source.SessionID,
                ByPassClientBilling = source.BypassClientBilling,
                ContentServerAddress = copyContentServerAddress ? source.ContentServerAddress : (ushort)0,
                IPAddress = source.IpAddress,
                UserPassword = source.UserPassword,
                ProxyUserID = source.ProxyUserId,
                ProxyUserNamespace = source.ProxyProductId,

                CacheApplication = source.CacheApplication,
                CacheExpirationPolicy = source.CacheExpirationPolicy,
                CacheExpirationTime = source.CacheExpirationTime,
                CacheRefreshInterval = source.CacheRefreshInterval,
                CacheStatus = Map(source.CacheStatus),
                CacheWait = source.CacheWait,
                CacheScope = source.CacheScope,
                ForceCacheRefresh = source.ForceCacheRefresh,
                AccountID = source.AccountId,

                KeepSessionAlive = source.KeepSessionAlive
            };

            controlData.AddRange(source.Metrics);

            return controlData;
        }

        public static IControlData Convert(Factiva.Gateway.Utils.V1_0.ControlData source, bool copyContentServerAddress = false)
        {
            if (source == null)
                return null;

            var controlData = new ControlData
            {
                UserID = source.UserID,
                AccessPointCode = source.AccessPointCode,
                AccessPointCodeUsage = source.AccessPointCodeUsage,
                CacheKey = source.CacheKey,
                ClientCode = source.ClientCode,
                ClientType = source.ClientType,
                EncryptedToken = source.EncryptedLogin,
                ProductID = source.ProductID,
                SessionID = source.SessionID,
                BypassClientBilling = source.ByPassClientBilling,
                ContentServerAddress = copyContentServerAddress ? source.ContentServerAddress : (ushort)0,
                IpAddress = source.IPAddress,
                UserPassword = source.UserPassword,
                ProxyUserId = source.ProxyUserID,
                ProxyProductId = source.ProxyUserNamespace,

                CacheApplication = source.CacheApplication,
                CacheExpirationPolicy = source.CacheExpirationPolicy,
                CacheExpirationTime = source.CacheExpirationTime,
                CacheRefreshInterval = source.CacheRefreshInterval,
                CacheStatus = Map(source.CacheStatus),
                CacheWait = source.CacheWait,
                CacheScope = source.CacheScope,
                ForceCacheRefresh = source.ForceCacheRefresh,
                AccountId = source.AccountID,
                KeepSessionAlive = source.KeepSessionAlive
            };
            return controlData;
        }

        private static CacheStatus Map(Factiva.Gateway.Utils.V1_0.CacheStatus cacheStatus)
        {
            switch (cacheStatus)
            {
                case Factiva.Gateway.Utils.V1_0.CacheStatus.Hit:
                    return CacheStatus.Hit;
                case Factiva.Gateway.Utils.V1_0.CacheStatus.Miss:
                    return CacheStatus.Miss;
                case Factiva.Gateway.Utils.V1_0.CacheStatus.NotSpecified:
                    return CacheStatus.NotSpecified;
                default:
                    return CacheStatus.Unknown;
            }
        }

        private static Factiva.Gateway.Utils.V1_0.CacheStatus Map(CacheStatus cacheStatus)
        {
            switch (cacheStatus)
            {
                case CacheStatus.Hit:
                    return Factiva.Gateway.Utils.V1_0.CacheStatus.Hit;
                case CacheStatus.Miss:
                    return Factiva.Gateway.Utils.V1_0.CacheStatus.Miss;
                case CacheStatus.NotSpecified:
                    return Factiva.Gateway.Utils.V1_0.CacheStatus.NotSpecified;
                default:
                    return Factiva.Gateway.Utils.V1_0.CacheStatus.Unknown;
            }
        }

        public static IControlData GetLightWeightUserControlData(string userId, string userPassword, string productId)
        {
            return GetLightWeightUserControlData(userId, userPassword, productId, "D", "p", 0, "127.0.0.1");
        }

        public static ControlData GetLightWeightUserControlData(string userId, string userPassword, string productId, string clientTypeCode, string accessPointCode, ushort contentServerAddress, string ipAddress)
        {
            return new ControlData
            {
                UserID = userId,
                UserPassword = userPassword,
                ProductID = productId,
                ClientType = clientTypeCode,
                AccessPointCode = accessPointCode,
                ContentServerAddress = contentServerAddress,
                IpAddress = (!string.IsNullOrEmpty(ipAddress)) ? ipAddress : "127.0.0.1"
            };
        }

        public static IControlData GetSessionUserControlData(string sessionId, string clientType, string accessPointCode)
        {
            return new ControlData
            {
                SessionID = sessionId,
                ClientType = clientType,
                AccessPointCode = accessPointCode,
            };
        }

        public static IControlData Clone(IControlData source, bool copyContentServerAddress = false)
        {
            if (source == null)
                return null;

            var controlData = new ControlData
            {
                UserID = source.UserID,
                AccessPointCode = source.AccessPointCode,
                AccessPointCodeUsage = source.AccessPointCodeUsage,
                CacheKey = source.CacheKey,
                ClientCode = source.ClientCode,
                ClientType = source.ClientType,
                EncryptedToken = source.EncryptedToken,
                ProductID = source.ProductID,
                SessionID = source.SessionID,
                BypassClientBilling = source.BypassClientBilling,
                ContentServerAddress = copyContentServerAddress ? source.ContentServerAddress : (ushort)0,
                IpAddress = source.IpAddress,
                UserPassword = source.UserPassword,
                ProxyUserId = source.ProxyUserId,
                ProxyProductId = source.ProxyProductId,
                Debug = source.Debug,

                CacheApplication = source.CacheApplication,
                CacheExpirationPolicy = source.CacheExpirationPolicy,
                CacheExpirationTime = source.CacheExpirationTime,
                CacheRefreshInterval = source.CacheRefreshInterval,
                CacheStatus = source.CacheStatus,
                CacheWait = source.CacheWait,
                CacheScope = source.CacheScope,
                ForceCacheRefresh = source.ForceCacheRefresh,
                AccountId = source.AccountId
            };
            return controlData;
        }

        public static IControlData GetLightWeightUserControlData(LightWeightUser lightWeightUser)
        {
            if (lightWeightUser == null)
                return null;
            return new ControlData
                       {
                           UserID = lightWeightUser.userId,
                           UserPassword = lightWeightUser.userPassword,
                           ProductID = lightWeightUser.productId,
                           ClientCode = lightWeightUser.clientCodeType,
                           AccessPointCode = lightWeightUser.accessPointCode,
                           ContentServerAddress = UInt16.Parse(lightWeightUser.contentServerAddress),
                           IpAddress = lightWeightUser.ipAddress
                       };
        }
    }
}