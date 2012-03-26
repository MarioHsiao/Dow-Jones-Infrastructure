using System;
using System.Linq;
using System.Threading;
using DowJones.Properties;
using DowJones.Utilities.Exceptions;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;
using Factiva.Gateway.V1_0;
using System.Web;
using Factiva.Gateway.Messages.Track.V1_0;
using DowJones.Utilities.Loggers;
using System.Web.Caching;
using System.Collections;

namespace DowJones.Utilities.Managers.Track
{
    public class TrackDeletedFoldersCacheManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TrackDeletedFoldersCacheManager));

        private CacheItemRemovedCallback cacheItemRemovedCallback;
        private static readonly object SyncRoot = new object();
        private const string TrackDeletedFoldersCacheKeyConst = "TrackDeletedFolders";
        private const string TrackDeletedFoldersSchedulerConst = "TrackDeletedFoldersScheduler";

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackDeletedFoldersCacheManager"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLangugage">The interface language.</param>
        public TrackDeletedFoldersCacheManager(string sessionId, string clientTypeCode, string accessPointCode, string interfaceLangugage)
            : base(sessionId, clientTypeCode, accessPointCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackDeletedFoldersCacheManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public TrackDeletedFoldersCacheManager(ControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return _log; }
        }

        public Hashtable Load(Hashtable response)
        {
            var logger = new TransactionLogger(Log);

            Hashtable foldersList = null;

            // check the cache to see if table exists
            if (HttpRuntime.Cache != null)
            {
                foldersList = (Hashtable)HttpRuntime.Cache.Get(TrackDeletedFoldersCacheKeyConst);
            }

            // already have a folder list so check the response for new folders
            if (foldersList != null && foldersList.Count > 0)
            {
                if (response != null && response.Count > 0)
                {
                    // hold new folders
                    var temp = new Hashtable(response);
                    var list = foldersList;
                    foreach (var folderIdToAdd in response.Values.Cast<object>().Where(folderIdToAdd => list.ContainsKey(folderIdToAdd.ToString())))
                    {
                        temp.Remove(folderIdToAdd.ToString());
                    }

                    // handle case where folders are undeleted (i.e. customer service closes and reopens account)
                    if (foldersList.Count > 0 && response.Count > 0 && response.Count < foldersList.Count)
                    {
                        foldersList = new Hashtable(response);
                        UpdateCache(foldersList);
                    }
                    else 
                    {
                        // add all of the new folders
                        foreach (var folderIdToAdd in temp.Values)
                        {
                            lock (SyncRoot)
                            {
                                foldersList.Add(folderIdToAdd.ToString(), folderIdToAdd.ToString());
                            }
                        }
                        UpdateCache(foldersList);
                    }
                }
            }
            else
            {
                // initial load of folder list
                UpdateCache(response);
                if (HttpRuntime.Cache != null)
                {
                    foldersList = (Hashtable) HttpRuntime.Cache.Get(TrackDeletedFoldersCacheKeyConst);
                }
            }
            return foldersList;
        }

        public Hashtable GetDeletedFolders()
        {
            Hashtable temp = null;
            if (HttpRuntime.Cache != null)
            {
                temp = (Hashtable)HttpRuntime.Cache.Get(TrackDeletedFoldersCacheKeyConst);
            }
            return temp;
        }

        private void UpdateCache(Hashtable folderList)
        {
            try
            {
                // check for valid cache object
                if (HttpRuntime.Cache == null)
                    return;

                // check folder list to see data has been added
                if (folderList.Count > 0)
                {
                    HttpRuntime.Cache.Add(TrackDeletedFoldersCacheKeyConst, folderList, null,
                                          Cache.NoAbsoluteExpiration,
                                          Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

                    // if temp object is set to null update the expiration time
                    if (HttpRuntime.Cache[TrackDeletedFoldersCacheKeyConst] == null)
                    {
                        HttpRuntime.Cache.Add(TrackDeletedFoldersCacheKeyConst, new object(), null,
                                              Cache.NoAbsoluteExpiration,
                                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }

                    // success so set the cache to retry in IISCacheExpiryInHours hours
                    if (HttpRuntime.Cache[TrackDeletedFoldersSchedulerConst] == null)
                    {
                        cacheItemRemovedCallback = new CacheItemRemovedCallback(TrackDeletedFoldersScheduler);
                        HttpRuntime.Cache.Add(TrackDeletedFoldersSchedulerConst, new object(), null,
                                              DateTime.Now.AddHours(((double.Parse(Settings.Default.IISCacheExpiryInHours)))), Cache.NoSlidingExpiration, CacheItemPriority.High, cacheItemRemovedCallback);
                    }
                    
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat(string.Format("Successfully added {0} and {1} to IIS Cache", TrackDeletedFoldersCacheKeyConst, TrackDeletedFoldersSchedulerConst));
                    }
                }
                else
                {
                    // failure so set the cache to retry in IISCacheExpiryInMinutes minutes
                    if (HttpRuntime.Cache[TrackDeletedFoldersSchedulerConst] == null)
                    {
                        cacheItemRemovedCallback = new CacheItemRemovedCallback(TrackDeletedFoldersScheduler);
                        HttpRuntime.Cache.Add(TrackDeletedFoldersSchedulerConst, new object(), null,
                                              DateTime.Now.AddMinutes((double.Parse(Settings.Default.IISCacheExpiryInMinutes))), Cache.NoSlidingExpiration, CacheItemPriority.High, cacheItemRemovedCallback);

                        if (Log.IsDebugEnabled)
                        {
                            Log.DebugFormat(string.Format("Deleted Folders Failed. Successfully added {0} to IIS Cache", TrackDeletedFoldersSchedulerConst));
                        }
                    }
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }
        }

        public void TrackDeletedFoldersScheduler(string key, object value, CacheItemRemovedReason reason)
        {
            var t = new Thread(() =>
            {
                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("TrackDeletedFoldersCacheManager >> TrackDeletedFoldersScheduler");
                }

                try
                {
                    ServiceResponse serviceResponse = TrackService.DeletedFolders(ControlDataManager.Clone(ControlData), new DeletedFoldersRequest());

                    DeletedFoldersResponse deletedFoldersResponse;
                    if (serviceResponse.rc == 0)
                    {
                        object responseObj;
                        serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
                        deletedFoldersResponse = (DeletedFoldersResponse)responseObj;

                        // check the object and folder count, if valid then update the cache
                        if (deletedFoldersResponse != null && deletedFoldersResponse.DeletedFolders.Count > 0)
                            UpdateCache(deletedFoldersResponse.DeletedFolders);

                        if (Log.IsDebugEnabled)
                        {
                            Log.DebugFormat("TrackDeletedFoldersCacheManager: Updated Cache");
                        }
                    }
                }
                catch (DowJonesUtilitiesException ex)
                {
                    throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
                }

                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("TrackDeletedFoldersCacheManager << TrackDeletedFoldersScheduler");
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
    }
}
