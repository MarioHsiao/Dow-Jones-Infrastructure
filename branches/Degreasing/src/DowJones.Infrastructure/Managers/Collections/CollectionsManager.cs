using System;
using System.Linq;
using DowJones.Caching;
using DowJones.Managers.Abstract;
using DowJones.Preferences;
using DowJones.Session;
using Factiva.Gateway.Messages.PCM.Search.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Managers.Collections
{
    /// <summary>
    ///   Collections Manager for use with PCM Caching transactions
    /// </summary>
    public class CollectionsManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (CollectionsManager));
        private readonly IPreferences _preferences;


        /// <summary>
        ///   Initializes a new instance of the <see cref = "CollectionsManager" /> class.
        /// </summary>
        /// <param name = "controlData">The control data.</param>
        /// <param name = "preferences">The preferences.</param>
        public CollectionsManager(IControlData controlData, IPreferences preferences) : base(controlData)
        {
            _preferences = preferences;
        }

        #region Overrides of AbstractAggregationManager

        protected override ILog Log { get { return _log; } }

        #endregion

        /// <summary>
        /// Gets the collection headlines by code request.
        /// </summary>
        /// <param name="collectionCode">The collection code.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="forceCacheRefresh">if set to <c>true</c> [force cache refresh].</param>
        /// <returns></returns>
        public PCMGetCollectionHeadlinesByCodeResponse GetCollectionHeadlinesByCodeRequest(string collectionCode, SortBy sortBy, AbstractCacheKey cacheKey = null, bool forceCacheRefresh = false)
        {
            var request = new PCMGetCollectionHeadlinesByCodeRequest
                              {
                                  Version = "2.7",
                                  RequestDTO = new RequestDTO
                                                   {
                                                       ClusterMode = ClusterMode.Off,
                                                       SortBy = sortBy,
                                                       DescriptorControl = new DescriptorControl
                                                                               {
                                                                                   Language = _preferences.InterfaceLanguage,
                                                                                   Mode = DescriptorControlMode.None
                                                                               },
                                                       
                                                       MetaDataController = new MetaDataController
                                                                                {
                                                                                    Mode = CodeNavigatorMode.None,
                                                                                    ReturnCollectionCounts = false,
                                                                                    ReturnKeywordsSet = false,
                                                                                },
                                                   },
                                  SearchServiceType = SearchServiceType.Free,
                                  ReturnContentCatagorizationSet = false,
                                  CollectionCode = collectionCode
                              };
            request.RequestDTO.SearchCollectionCollection.AddRange(Enum.GetValues(typeof(SearchCollection)).Cast<SearchCollection>());
            return GetCollectionHeadlinesByCodeRequest(request, cacheKey, forceCacheRefresh);
        }

        /// <summary>
        ///   Gets the collection headlines by code request.
        /// </summary>
        /// <param name = "request">The request.</param>
        /// <param name = "cacheKey">The cache key.</param>
        /// <param name = "forceCacheRefresh">if set to <c>true</c> [force cache refresh].</param>
        /// <returns></returns>
        public PCMGetCollectionHeadlinesByCodeResponse GetCollectionHeadlinesByCodeRequest(PCMGetCollectionHeadlinesByCodeRequest request, AbstractCacheKey cacheKey = null, bool forceCacheRefresh = false)
        {
            var temp = ControlData;
            if (cacheKey != null)
            {
                temp = ControlDataManager.GetControlDataForTransactionCache(ControlData,
                                                                            cacheKey.Serialize(),
                                                                            cacheKey.CacheScope,
                                                                            cacheKey.CacheExpirationTime,
                                                                            cacheKey.CacheRefreshInterval,
                                                                            cacheKey.CacheExiprationPolicy,
                                                                            forceCacheRefresh);
            }
            return Invoke<PCMGetCollectionHeadlinesByCodeResponse>(request, temp).ObjectResponse;
        }
    }
}