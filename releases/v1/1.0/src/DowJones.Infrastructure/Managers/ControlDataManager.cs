// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlDataManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Properties;
using DowJones.Session;
using DowJones.Utilities.Configuration;
using Factiva.Gateway.Utils.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Utilities.Managers
{
    #region Enumerations

    #region CacheExiprationPolicy enum

    public enum CacheExiprationPolicy
    {
        None = 0,
        Absolute,
        Sliding,
    }

    #endregion

    #region CacheScope enum

    public enum CacheScope
    {
        Session = 0,
        User,
        Account,
        All
    }

    #endregion

    public class TransactionCacheControlData
    {
        public TransactionCacheControlData()
        {
            ExiprationPolicy = CacheExiprationPolicy.None;
            Scope = CacheScope.User;
        }

        public string Key { get; set; }
        public CacheScope Scope { get; set; }
        public int ExpirationTime { get; set; }
        public int RefreshInterval { get; set; }
        public CacheExiprationPolicy ExiprationPolicy { get; set; }
        public bool ForceCacheRefresh { get; set; }
        public bool Wait { get; set; }
    }


    #endregion

    #region CacheStatus enum
    /// <summary>
    /// CacheStatus comes back in the response and will be
    /// </summary>
   

    #endregion

    /// <summary>
    /// Summary description for ControlDataManager.
    /// </summary>
    public class ControlDataManager : Factiva.Gateway.Managers.ControlDataManager
    {
        public static ControlData GetControlDataForTransactionCache(ControlData controlData, TransactionCacheControlData transControlData)
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
        public static ControlData GetControlDataForTransactionCache(ControlData sessionUserControlData, string cacheKey, Managers.CacheScope cacheScope, int cacheExpirationTime, int cacheRefreshInterval, Managers.CacheExiprationPolicy cacheExiprationPolicy, bool forceCacheRefresh)
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
        public static ControlData GetControlDataForTransactionCache(ControlData sessionUserControlData, string cacheKey, Managers.CacheScope cacheScope, int cacheExpirationTime, int cacheRefreshInterval, Managers.CacheExiprationPolicy cacheExiprationPolicy, bool forceCacheRefresh, bool cacheWait)
        {
            var controlData = Clone(sessionUserControlData, false, Settings.Default.ByPassSessionServerCache);

            controlData.CacheKey = cacheKey;
            controlData.CacheScope = cacheScope.ToString();
            controlData.CacheExpirationTime = cacheExpirationTime.ToString();
            controlData.CacheRefreshInterval = cacheRefreshInterval.ToString();
            if (cacheExiprationPolicy != Managers.CacheExiprationPolicy.None)
            {
                controlData.CacheExpirationPolicy = cacheExiprationPolicy.ToString();
            }

            controlData.ForceCacheRefresh = forceCacheRefresh ? "True" : "False";
            controlData.CacheWait = cacheWait ? "True" : "False";
            return controlData;
        }

        /// <summary>
        /// Clones the "control data" to add additional information, that is transaction specific.
        /// Does not copy the content server address by default and forces a 0. In need of content server address, 
        /// please use clone with copyContentServerAddress overridden method.
        /// </summary>
        /// <param name="controlData"></param>
        /// <returns></returns>
        public new static ControlData Clone(ControlData controlData)
        {
            return Clone(controlData, false, Settings.Default.ByPassSessionServerCache);
        }

        /// <summary>
        /// Clones the specified control data. 
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="copyContentServerAddress">if set to <c>true</c> [copy content server address].</param>
        /// <returns></returns>
        public new static ControlData Clone(ControlData controlData, bool copyContentServerAddress)
        {
            return Clone(controlData, copyContentServerAddress, Settings.Default.ByPassSessionServerCache);
        }

        /// <summary>        
        /// Gets the light weight user control data.
        /// </summary>
        /// <param name="lightWeightUser">The light weight user.</param>
        /// <returns></returns>
        public static ControlData GetLightWeightUserControlData(LightWeightUser lightWeightUser)
        {
            return GetLightWeightUserControlData(lightWeightUser.userId, lightWeightUser.userPassword, lightWeightUser.productId, lightWeightUser.clientCodeType, lightWeightUser.accessPointCode, lightWeightUser.contentServerAddress, lightWeightUser.ipAddress);
        }

        public static ControlData Convert(IControlData source)
        {
            var controlData = new ControlData
                           {
                               UserID = source.UserID, 
                               AccessPointCode = source.AccessPointCode, 
                               AccessPointCodeUsage = source.AccessPointCodeUsage, 
                               CacheKey = source.CacheKey, 
                               ClientCode = source.ClientCodeType, 
                               EncryptedLogin = source.EncryptedToken, 
                               ProductID = source.ProductID, 
                               SessionID = source.SessionID,
                               ByPassClientBilling = source.BypassClientBilling,
                               ContentServerAddress = UInt16.Parse(source.ContentServerAddress),
                               IPAddress = source.IpAddress,
                               UserPassword = source.UserPassword,
                           };
            return controlData;
        }

        public static IControlData Convert(ControlData source)
        {
            var controlData = new Session.ControlData
                           {
                               UserID = source.UserID, 
                               AccessPointCode = source.AccessPointCode, 
                               AccessPointCodeUsage = source.AccessPointCodeUsage, 
                               CacheKey = source.CacheKey,
                               ClientCodeType = source.ClientCode,
                               EncryptedToken = source.EncryptedLogin, 
                               ProductID = source.ProductID, 
                               SessionID = source.SessionID,
                               BypassClientBilling = source.ByPassClientBilling,
                               ContentServerAddress = source.ContentServerAddress.ToString(),
                               IpAddress = source.IPAddress,
                               UserPassword = source.UserPassword,
                           };
            return controlData;
        }

        internal new static ControlData GetLightWeightUserControlData(string userId, string password, string productId)
        {
            throw new System.NotImplementedException();
        }

        internal static ControlData UpdateProxyCredentials(ControlData controldata, LightWeightUser lightWeightUser)
        {
            var temp = Clone(controldata);
            temp.ProxyUserID = lightWeightUser.userId;
            temp.ProxyUserNamespace = lightWeightUser.productId;
            return temp;
        }
    }
}