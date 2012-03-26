using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Properties;
using DowJones.Utilities.Exceptions;
using Factiva.Gateway.Utils.V1_0;
using FactivaCaching_v_1_1;
using Factiva.Gateway.Messages.Cache.PlatformCache.V1_0;
using Factiva.Gateway.Messages.V1_0;
using Factiva.Gateway.Services.V1_0;
using log4net;

namespace DowJones.Utilities.Managers.CacheService
{
    public enum CacheItemDuration
    {
        Default = 0,
        Minute,
        FifthteenMinutes,
        OneHour,
        SixHours,
        TwelveHours,
        TwentyFourHours,
        TwelveOhFiveMorningNextDay,
        Week,
        Month,
        SixMonths,
        Year,
        Unlimited,
    }

    public enum CacheNameSpace
    {
        /// <summary>
        /// Widget's Namespace
        /// </summary>
        [XmlEnum("Search20")]
        Search = 2,
        [XmlEnum("EditorChoiceRSS")]
        EditorChoice = 3,
        [XmlEnum("NewsLetter")]
        NewsLetter = 4,
        [XmlEnum("WidgetsFactivaUI")]
        Widgets = 5,
        [XmlEnum("TrackFolder")]
        TrackFolder = 6,
        [XmlEnum("ProjectVisible")]
        ProjectVisible = 7,
        [XmlEnum("PodCast")]
        PodCast = 8,
        [XmlEnum("TrackFolder")]
        QueryAsset = 9,
    }

    public class CacheManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CacheManager));
        private bool throwErrors;


        /// <summary>
        /// Gets or sets a value indicating whether [throw errors].
        /// </summary>
        /// <value><c>true</c> if [throw errors]; otherwise, <c>false</c>.</value>
        public bool ThrowErrors
        {
            get { return throwErrors; }
            set { throwErrors = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="throwErrors">if set to <c>true</c> [throw errors].</param>
        public CacheManager(bool throwErrors)
        {
            this.throwErrors = throwErrors;
        }

        public CacheManager()
        {
        }

        /// <summary>
        /// Adds the item to cache.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="itemValue">The item value.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns></returns>
        public bool AddItem(string itemID, string itemValue, CacheNameSpace nameSpace)
        {
            return AddItem(itemID, itemValue, nameSpace, CacheItemDuration.Default);
        }

        /// <summary>
        /// Adds an Item to the cache
        /// </summary>
        /// <param name="itemID">String ID of the Item</param>
        /// <param name="itemValue">String value of the item</param>
        /// <param name="nameSpace">The name space.</param>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        public bool AddItem(string itemID, string itemValue, CacheNameSpace nameSpace, CacheItemDuration duration)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info(itemValue.Replace("\n", string.Empty).Replace("\r", string.Empty));
            }
            var item = new GenericCacheItem
                           {
                               ID = itemID, 
                               Value = itemValue, 
                               NameSpace = (int) nameSpace, 
                               MoreInfo1 = String.Empty, 
                               MoreInfo2 = String.Empty, 
                               expiresAfter = ExpireAfter(duration)
                           };
            //newer version of caching dll, 2 more new data items to pass - sm 7/26
            try
            {
                if (Settings.Default.UsePlatformCache)
                {
                    long status;
                    if ((status = PlatformCache.AddItem(itemID, itemValue, nameSpace, duration)) != 0)
                        throw new Exception(string.Format("Failed with error code {0}", status));
                }
                
                return true;
            }
            catch (Exception ex)
            {
                if (throwErrors)
                {
                    throw new DowJonesUtilitiesException(ex.InnerException, DowJonesUtilitiesException.CacheManagerUnableToAddItem);
                }
                if (Log.IsErrorEnabled)
                {
                    Log.Error(string.Format("Exception while Adding to Cache:{0} \nMessage: {1}\nStackTrace: {2}", itemID, ex.Message, ex.StackTrace), ex);
                }
            }
            return false;
        }

        /// <summary>
        /// Adds the items to cache.
        /// </summary>
        /// <param name="cacheItems">The cache items.</param>
        /// <param name="nameSpace">The name space.</param>
        public void AddItems(Dictionary<string, string> cacheItems, CacheNameSpace nameSpace)
        {
            AddItems(cacheItems, nameSpace, CacheItemDuration.Default);
        }

        /// <summary>
        /// Adds the items to cache.
        /// </summary>
        /// <param name="cacheItems">The cache items.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <param name="duration">The duration.</param>
        public void AddItems(Dictionary<string, string> cacheItems, CacheNameSpace nameSpace, CacheItemDuration duration)
        {
            foreach (var pair in cacheItems)
            {
                AddItem(pair.Key, pair.Value, nameSpace, duration);
            }
        }

        /// <summary>
        /// Gets a single Item from the Factiva Cache based on the ID
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>String value of the Item</returns>
        public string GetItem(string itemID, CacheNameSpace nameSpace)
        {
            var itemVal = string.Empty;
            try
            {
                if (Settings.Default.UsePlatformCache)
                {
                    itemVal = PlatformCache.GetItem(itemID, nameSpace);
                }
            }
            catch (Exception ex)
            {
                if (throwErrors)
                {
                    throw new DowJonesUtilitiesException(ex.InnerException, DowJonesUtilitiesException.CacheManagerUnableToGetItem);
                }
                if (Log.IsErrorEnabled)
                {
                    Log.Info(string.Format("Exception while getting from Cache:{0}", itemID), ex);
                }
            }
            if (Log.IsInfoEnabled)
            {
                Log.Info(string.Format("Cached ITEM,ID-{0},value-{1}", itemID, itemVal));
            }
            return itemVal;
        }

        /// <summary>
        /// Returns the values for a string array of keys provided.
        /// </summary>
        /// <param name="cacheItemIds">Array of Keys</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>
        /// Namevaluecollection corresponding to each inputted key ID
        /// </returns>
        public Dictionary<string, string> GetItems(string[] cacheItemIds, CacheNameSpace nameSpace)
        {
            var collection = new Dictionary<string, string>();
            try
            {
                if (Settings.Default.UsePlatformCache)
                {
                    foreach (var itemID in cacheItemIds)
                    {
                        collection.Add(itemID, GetItem(itemID, nameSpace));
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwErrors)
                {
                    throw new DowJonesUtilitiesException(ex.InnerException, DowJonesUtilitiesException.CacheManagerUnableToGetItem);
                }
                if (Log.IsErrorEnabled)
                {
                    Log.Error("Exception while getting from Cache:method:GetItems", ex);
                }

            }
            return collection;
        }

        /// <summary>
        /// Deletes from cache.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns></returns>
        public bool Delete(string itemID, CacheNameSpace nameSpace)
        {
            try
            {
                if (Settings.Default.UsePlatformCache)
                {
                    long status;
                    if ((status = PlatformCache.DeleteItem(itemID, nameSpace)) != 0)
                        throw new Exception(string.Format("Failed with error code {0}", status));
                }
                return true;
            }
            catch (Exception ex)
            {
                if (throwErrors)
                {
                    throw new DowJonesUtilitiesException(ex.InnerException, DowJonesUtilitiesException.CacheManagerUnableToUpdateItem);
                }
                if (Log.IsErrorEnabled)
                {
                    Log.ErrorFormat(string.Format("Exception while Updating to Cache:{0}", itemID), ex);
                }
            }
            return false;
        }

        /// <summary>
        /// Updates the cache.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="itemValue">The item value.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns></returns>
        public bool Update(string itemID, string itemValue, CacheNameSpace nameSpace)
        {
            return Update(itemID, itemValue, nameSpace, CacheItemDuration.Default);
        }

        /// <summary>
        /// Updates the cache.  
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="itemValue">The item value.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        public bool Update(string itemID, string itemValue, CacheNameSpace nameSpace, CacheItemDuration duration)
        {
            var item = new GenericCacheItem
                           {
                               ID = itemID, 
                               Value = itemValue, 
                               NameSpace = (int) nameSpace, 
                               MoreInfo1 = String.Empty, 
                               MoreInfo2 = String.Empty, 
                               expiresAfter = ExpireAfter(duration)
                           };
            //newer version of caching dll, 2 more new data items to pass - sm 7/26
            try
            {
                if (Settings.Default.UsePlatformCache)
                {
                    long status ;
                    if ((status = PlatformCache.UpdateItem(itemID, itemValue, nameSpace, duration)) != 0)
                        throw new Exception(string.Format("Failed with error code {0}", status));
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (throwErrors)
                {
                    throw new DowJonesUtilitiesException(ex.InnerException, DowJonesUtilitiesException.CacheManagerUnableToUpdateItem);
                }
                if (Log.IsErrorEnabled)
                {
                    Log.ErrorFormat(string.Format("Exception while Updating to Cache:{0}", itemID), ex);
                }
            }
            return false;
        }

        public static int ExpireAfter(CacheItemDuration duration)
        {
            switch (duration)
            {
                case CacheItemDuration.Minute:
                    return 1;
                case CacheItemDuration.FifthteenMinutes:
                    return 15;
                case CacheItemDuration.OneHour:
                    return 60;
                case CacheItemDuration.SixHours:
                    return 60 * 6;
                case CacheItemDuration.TwelveHours:
                    return 60 * 12;
                case CacheItemDuration.TwentyFourHours:
                    return 60 * 24;
                case CacheItemDuration.TwelveOhFiveMorningNextDay:
                    DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1).AddMinutes(5);
                    return temp.Subtract(DateTime.Now).Minutes;
                case CacheItemDuration.Week:
                    return 60 * 24 * 7;
                case CacheItemDuration.Month:
                    return 60 * 24 * 30;
                case CacheItemDuration.SixMonths:
                    return 60 * 24 * 30 * 6;
                case CacheItemDuration.Year:
                    return 60 * 24 * 30 * 12;
                case CacheItemDuration.Unlimited:
                    return DateTime.MaxValue.Subtract(DateTime.Now).Minutes;
                default:
                    return Settings.Default.CacheExpiryInMinutes;

            }
        }

        public static int ExpireAfterSeconds(CacheItemDuration duration)
        {
            return ExpireAfter(duration) * 60;
        }

       

    }

    internal class PlatformCache
    {
        public static long AddItem(string key, string value, CacheNameSpace nameSpace, CacheItemDuration duration)
        {
            long rc;
            var request = new StoreItemRequest();
            var item = new CacheItem { Key = key, Namespace = GatewayMessage.GetXmlEnumName<CacheNameSpace>(nameSpace.ToString()), StringData = value, ExpirationPolicy = ExpirationPolicy.Absolute, ExpiryInterval = (uint)CacheManager.ExpireAfter(duration) };

            request.Item = item;

            var response = PlatformCacheService.StoreItem(new ControlData(), request);
            if ((rc = response.rc) == 0)
            {
                var cacheResponse = response.ObjectResponse;
                rc = cacheResponse.Status;
            }

            return rc;

        }

        public static string GetItem(string key, CacheNameSpace nameSpace)
        {
            var retVal = string.Empty;
            var request = new GetItemRequest
            {
                Key = key,
                Namespace = GatewayMessage.GetXmlEnumName<CacheNameSpace>(nameSpace.ToString())
            };

            var response = PlatformCacheService.GetItem(new ControlData(), request);
            if (response.rc == 0)
            {
                var cacheResponse = response.ObjectResponse;
                retVal = cacheResponse.Item.StringData;
            }

            return retVal;

        }

        public static long UpdateItem(string key, string value, CacheNameSpace nameSpace, CacheItemDuration duration)
        {
            return AddItem(key, value, nameSpace, duration);
        }

        public static long DeleteItem(string key, CacheNameSpace nameSpace)
        {
            long rc;
            var request = new DeleteItemRequest
            {
                Key = key,
                Namespace = GatewayMessage.GetXmlEnumName<CacheNameSpace>(nameSpace.ToString())
            };

            var response = PlatformCacheService.DeleteItem(new ControlData(), request);
            if ((rc = response.rc) == 0)
            {
                var cacheResponse = response.ObjectResponse;
                rc = cacheResponse.Status;
            }

            return rc;

        }
    }
}