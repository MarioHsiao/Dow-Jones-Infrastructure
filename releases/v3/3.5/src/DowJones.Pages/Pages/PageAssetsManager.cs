// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageAssetsManager.cs" company="Dow Jones & Co.">
//   
// </copyright>
// <summary>
//   The page assets manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DowJones.Caching;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using DowJones.Loggers;
using DowJones.Managers.Abstract;
using DowJones.Pages.Caching;
using DowJones.Pages.Common;
using DowJones.Pages.DataAccess.Managers;
using DowJones.Pages.Modules;
using DowJones.Preferences;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Assets.V1_0;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using log4net;
using AssetType = Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType;
using CacheScope = Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope;
using FactivaModuleType = Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType;
using GWPage = Factiva.Gateway.Messages.Assets.Pages.V1_0.Page;
using Module = DowJones.Pages.Modules.Module;
using ModuleState = Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleState;
using ModuleType = DowJones.Pages.Modules.ModuleType;
using ShareProperties = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareProperties;
using ShareScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope;
using SortBy = Factiva.Gateway.Messages.Assets.Pages.V1_0.SortBy;
using SortOrder = Factiva.Gateway.Messages.Assets.Common.V2_0.SortOrder;
using GWModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;
using ModuleProperties = Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleProperties;
using QueryFilters = Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryFilters;

namespace DowJones.Pages
{
    /// <summary>
    /// The page assets manager.
    /// </summary>
    public class PageAssetsManager : AbstractAggregationManager, IPageAssetsManager
    {
        /// <summary>
        /// The not specified.
        /// </summary>
        public const string NotSpecified = "Not_Specified";

        /// <summary>
        /// The module types by name.
        /// </summary>
        internal static readonly IDictionary<string, ModuleType> ModuleTypesByName =
            Enum.GetValues(typeof(ModuleType)).Cast<ModuleType>()
                .Select(x => new { Name = Enum.GetName(typeof(ModuleType), x), Type = x })
                .ToDictionary(x => x.Name, y => y.Type);

        /// <summary>
        ///   Internal logger
        /// </summary>
        private static readonly ILog InternalLog = LogManager.GetLogger(typeof(PageAssetsManager));

        /// <summary>
        /// The _interface language.
        /// </summary>
        private readonly string interfaceLanguage;

        /// <summary>
        /// The preferences.
        /// </summary>
        private readonly IPreferences preferences;

        /// <summary>
        /// The product.
        /// </summary>
        private readonly Product product;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageAssetsManager"/> class.
        /// </summary>
        /// <param name="controlData">
        /// The control data.
        /// </param>
        /// <param name="preferences">
        /// The preferences.
        /// </param>
        /// <param name="product">
        /// The product.
        /// </param>
        [Inject("Disambiguating multiple constructors")]
        public PageAssetsManager(IControlData controlData, ITransactionTimer transactionTimer, IPreferences preferences, Product product)
            : base(controlData, transactionTimer)
        {
            Guard.IsNotNull(controlData, "controlData");
            Guard.IsNotNull(preferences, "preferences");

            this.preferences = preferences;
            interfaceLanguage = LanguageUtilityManager.ValidateLanguageCode(preferences.InterfaceLanguage);
            this.product = product;

            //CachingEnabled = CacheKeyConstants.IncludeCacheKeyGeneration && Settings.Default.CachePageItems;
        }

        /// <summary>
        /// Gets InterfaceLanguage.
        /// </summary>
        public string InterfaceLanguage
        {
            get { return interfaceLanguage; }
        }

        /// <summary>
        /// Gets Preferences.
        /// </summary>
        public IPreferences Preferences
        {
            get { return preferences; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether CachingEnabled.
        /// </summary>
        //public bool CachingEnabled { get; set; }

        /// <summary>
        /// Gets Product.
        /// </summary>
        public Product Product
        {
            get { return product; }
        }

        /// <summary>
        /// Gets Log.
        /// </summary>
        protected override ILog Log
        {
            get { return InternalLog; }
        }

        /// <summary>
        /// Adds the module ids to page.
        /// </summary>
        /// <param name="pageRef">The page ref.</param>
        /// <param name="moduleIdWithPositionCollection">The module id with position collection.</param>
        public void AddModuleIdsToPage(string pageRef, IEnumerable<KeyValuePair<int, int>> moduleIdWithPositionCollection)
        {
            var request = new AddModulesToPageRequest
                              {
                                  PageID = ExtractPageId(pageRef),
                                  ModulePositionCollection = new ModulePositionCollection()
                              };

            foreach (var keyValuePair in moduleIdWithPositionCollection.Where(keyValuePair => keyValuePair.Key > 0))
            {
                request.ModulePositionCollection.Add(new ModulePosition
                                                         {
                                                             Id = keyValuePair.Key,
                                                             Position = keyValuePair.Value,
                                                         });
            }

            Process<AddModulesToPageResponse>(request);
        }

        /// <summary>
        /// The add module ids to page.
        /// </summary>
        /// <param name="pageRef">
        /// The page reference.
        /// </param>
        /// <param name="moduleIdCollection">
        /// The module id collection.
        /// </param>
        public void AddModuleIdsToPage(string pageRef, IEnumerable<string> moduleIdCollection)
        {
            if (moduleIdCollection == null || !moduleIdCollection.Any())
            {
                return;
            }

            var module = GetModuleById(moduleIdCollection.First());

            if (module == null)
            {
                return;
            }

            if (module.ShareProperties.AccessControlScope != AccessControlScope.Everyone)
            {
                return;
            }

            var request = new AddModulesToPageRequest
                              {
                                  PageID = ExtractPageId(pageRef),
                                  ModulePositionCollection = new ModulePositionCollection()
                              };

            var i = 0;
            foreach (var modId in moduleIdCollection)
            {
                int moduleId;
                if (int.TryParse(modId, out moduleId))
                {
                    request.ModulePositionCollection.Add(new ModulePosition
                                                             {
                                                                 Id = moduleId,
                                                                 Position = i++
                                                             });
                }
            }

            Process<AddModulesToPageResponse>(request);
        }

        /// <summary>
        /// The add module ids to page.
        /// </summary>
        /// <param name="pageRef">
        /// The page reference.
        /// </param>
        /// <param name="moduleIdCollection">
        /// The module id collection.
        /// </param>
        public void AddModuleIdsToEndOfPage(string pageRef, IEnumerable<string> moduleIdCollection)
        {
            if (moduleIdCollection == null || !moduleIdCollection.Any())
            {
                return;
            }

            var module = GetModuleById(moduleIdCollection.First());

            if (module == null)
            {
                return;
            }

            if (module.ShareProperties.AccessControlScope != AccessControlScope.Everyone)
            {
                return;
            }

            var request = new AddModulesToPageRequest
                              {
                                  PageID = ExtractPageId(pageRef),
                                  ModulePositionCollection = new ModulePositionCollection(),
                              };

            var page = GetPage(pageRef, false, false);

            var max = 0;
            if (page != null && page.ModuleCollection.Count > 0)
            {
                max = page.ModuleCollection.Max(n => n.Position);
                max++;
            }

            var i = max;
            foreach (var modId in moduleIdCollection)
            {
                int moduleId;
                if (int.TryParse(modId, out moduleId))
                {
                    request.ModulePositionCollection.Add(new ModulePosition
                                                             {
                                                                 Id = moduleId,
                                                                 Position = i++
                                                             });
                }
            }

            Process<AddModulesToPageResponse>(request);
        }

        /// <summary>
        /// The add modules to page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="modules">The module collection.</param>
        public void AddModulesToPage(int pageId, IEnumerable<Module> modules)
        {
            var request = new AddModulesToPageRequest
                              {
                                  PageID = pageId
                              };

            if (modules != null)
            {
                request.ModuleCollection = new ModuleCollection();
                request.ModulePositionCollection = new ModulePositionCollection();
                foreach (var module in modules.Where(module => module.Id == 0))
                {
                    var gatewayModule = Mapper.Map<ModuleEx>(module);
                    request.ModuleCollection.Add(gatewayModule);
                }
            }

            Invoke<AddModulesToPageResponse>(request);
        }

        /// <summary>
        /// Adds the module to end of page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="newModule">The new module.</param>
        /// <param name="routine">The routine.</param>
        /// <returns>
        /// A string with the module id
        /// </returns>
        public string AddModuleToEndOfPage(string pageId, GWModule newModule, Action<GWPage, GWModule> routine)
        {
            var page = GetPage(pageId, false, false);
            var i = 0;

            if (page == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.PageIsNull);
            }

            // update the positions of the modules
            if (page.ModuleCollection != null &&
                page.ModuleCollection.Count > 0)
            {
                if (routine != null)
                {
                    routine(page, newModule);
                }

                var updateModulePositionsRequest = new UpdateModulePositionsRequest
                                                       {
                                                           PageID = page.Id
                                                       };

                foreach (var module in page.ModuleCollection)
                {
                    updateModulePositionsRequest.ModulePositions.ModulePositionCollection.Add(
                        new ModulePosition
                            {
                                Id = module.Id,
                                Position = i++
                            });
                }

                if (updateModulePositionsRequest.ModulePositions.ModulePositionCollection.Count > 0)
                {
                    Process<UpdateModulePositionsResponse>(updateModulePositionsRequest);
                }
            }

            // add the module to the proper position
            newModule.Position = i;
            var addModulesToPageRequest = new AddModulesToPageRequest
                                              {
                                                  PageID = page.Id,
                                              };
            addModulesToPageRequest.ModuleCollection.Add(newModule);
            var response = Process<AddModulesToPageResponse>(addModulesToPageRequest);
            newModule.__id = response.AddModulesResult.ModulePositionCollection[0].Id;
            return response.AddModulesResult.ModulePositionCollection[0].Id.ToString();
        }

        /// <summary>
        /// Adds the module to page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="newModule">The new module.</param>
        /// <param name="routine">The routine.</param>
        /// <returns>
        /// A string that represents the newModule Id
        /// </returns>
        public string AddModuleToPage(string pageId, int numberOfColumns, GWModule newModule, Action<GWPage, GWModule> routine)
        {
            var page = GetPage(pageId, false, false);

            if (page == null)
            {
                return string.Empty;
            }

            // update the positions of the modules
            if (page.ModuleCollection != null &&
                page.ModuleCollection.Count > 0)
            {
                if (routine != null)
                {
                    routine(page, newModule);
                }

                var updateModulePositionsRequest = new UpdateModulePositionsRequest
                                                       {
                                                           PageID = page.Id,
                                                       };

                foreach (var module in page.ModuleCollection.Where(module => module.Position % numberOfColumns == 0))
                {
                    updateModulePositionsRequest.ModulePositions.ModulePositionCollection.Add(
                        new ModulePosition
                            {
                                Id = module.Id,
                                Position = module.Position + numberOfColumns
                            });
                }

                if (updateModulePositionsRequest.ModulePositions.ModulePositionCollection.Count > 0)
                {
                    Process<UpdateModulePositionsResponse>(updateModulePositionsRequest);
                }
            }

            // add the module to the proper position
            newModule.Position = 0;

            var addModulesToPageRequest = new AddModulesToPageRequest
                                              {
                                                  PageID = page.Id,
                                              };
            addModulesToPageRequest.ModuleCollection.Add(newModule);

            var response = Process<AddModulesToPageResponse>(addModulesToPageRequest);
            newModule.__id = response.AddModulesResult.ModulePositionCollection[0].Id;

            return newModule.Id.ToString();
        }

        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <param name="page">
        /// The new page object.
        /// </param>
        /// <returns>
        /// A updated page object
        /// </returns>
        public GWPage CreatePage(CreatePageRequest page)
        {
            var createPageResponse = Process<CreatePageResponse>(page);
            return createPageResponse.Id > 0 ? this.GetPage(createPageResponse.Id.ToString()) : null;
        }

        /// <summary>
        /// The create a factiva page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// The create page.
        /// </returns>
        public string CreatePage(GWPage page)
        {
            var request = new CreatePageRequest
                              {
                                  Page = page
                              };

            var createPageResponse = Invoke<CreatePageResponse>(request).ObjectResponse;
            if (createPageResponse.Id > 0)
            {
                if (PageRef.CachingEnabled)
                {
                    var pageRef = new PageRef(createPageResponse.Id, product)
                                      {
                                          ParentId = createPageResponse.Id.ToString(),
                                          PageAccessQualifier = Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Factiva,
                                          RootAccessControlScope = AccessControlScope.Everyone,
                                          PageAccessControlScope = AccessControlScope.Everyone,
                                      };

                    return pageRef.CachedPageId;
                }

                return createPageResponse.Id.ToString();
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UnableToCreatePage);
        }

        /// <summary>
        /// Deletes the modules.
        /// </summary>
        /// <param name="pageId">The canvas id.</param>
        /// <param name="moduleIds">The module ids.</param>
        public void DeleteModules(string pageId, IEnumerable<string> moduleIds)
        {
            var deleteModulesFromPageRequest = new DeleteModulesFromPageRequest
                                                   {
                                                       PageID = ExtractPageId(pageId),
                                                   };

            deleteModulesFromPageRequest.ModuleIDCollection.AddRange(moduleIds.Select(ExtractModuleId));
            Process<DeleteModulesFromPageResponse>(deleteModulesFromPageRequest);
        }

        /// <summary>
        /// Deletes the page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        public void DeletePage(string pageId)
        {
            var request = new DeletePageRequest { Id = ExtractPageId(pageId), };

            Process<DeletePageResponse>(request);
        }

        /// <summary>
        /// Deletes all pages.
        /// </summary>
        /// <param name="pageTypes">
        /// The page types.
        /// </param>
        public void DeleteAllPages(IEnumerable<PageType> pageTypes)
        {
            if (pageTypes.Count() <= 0)
            {
                return;
            }

            var pageListInfoCollection = GetPageListInfoCollection(
                pageTypes,
                SortOrder.Descending,
                SortBy.LastModifiedDate);

            foreach (var page in pageListInfoCollection)
            {
                DeletePage(page.Id.ToString());
            }
        }

        /// <summary>
        /// Deletes all pages.
        /// </summary>
        /// <param name="pageListInfoCollection">
        /// The page list info collection.
        /// </param>
        public void DeleteAllPages(IEnumerable<PageListInfo> pageListInfoCollection)
        {
            if (pageListInfoCollection == null)
            {
                pageListInfoCollection = GetPageListInfoCollection(
                    new[]
                        {
                            PageType.BRI, 
                            PageType.CVD, 
                            PageType.Discovery, 
                            PageType.DJConsultancy, 
                            PageType.MM, 
                            PageType.PNP, 
                            PageType.Prototype
                        },
                    SortOrder.Descending,
                    SortBy.LastModifiedDate);
            }

            foreach (var page in pageListInfoCollection)
            {
                DeletePage(page.Id.ToString());
            }
        }

        /// <summary>
        /// Deletes the page from session cache.
        /// </summary>
        /// <param name="cacheKey">
        /// The cache key.
        /// </param>
        /// <param name="cacheScope">
        /// The cache scope.
        /// </param>
        public void DeletePageFromSessionCache(string cacheKey, CacheScope cacheScope)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                return;
            }

            var request = new DeleteItemRequest
                              {
                                  Key = cacheKey,
                                  Scope = cacheScope
                              };
            Process<DeleteItemResponse>(request);
        }

        public void DeleteItemFromSessionCache(ICacheKey cacheKey)
        {
            if (cacheKey == null)
                return;

            var request = new DeleteItemRequest
            {
                Key = cacheKey.Serialize(),
                Scope = Mapper.Map<CacheScope>(cacheKey.CacheScope)
            };
            Process<DeleteItemResponse>(request);
        }

        /// <summary>
        /// Gets the module by id.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// A module object.
        /// </returns>
        public GWModule GetModuleById(string pageId, string moduleId)
        {
            return pageId != NotSpecified
                ? GetModuleById(this.GetPage(pageId), moduleId)
                : GetModuleById(moduleId);
        }

        public GWModule GetModuleByIdWithPage(string pageId, string moduleId, out GWPage page)
        {
            if (pageId != NotSpecified)
            {
                page = this.GetPage(pageId);
                return GetModuleById(this.GetPage(pageId), moduleId);
            }
            page = null;
            return GetModuleById(moduleId);
        }

        private GWModule GetModuleById(GWPage page, string moduleId)
        {
            return page == null || page.ModuleCollection == null
                ? null
                : page.ModuleCollection.FirstOrDefault(x => x.Id == ExtractModuleId(moduleId));
        }

        /// <summary>
        /// Gets news-pages modules name and descriptions. The interface language is set when creating an instance on
        ///   the Page list manager
        /// </summary>
        /// <param name="codes">
        /// The codes.
        /// </param>
        /// <returns>
        /// List of MetaData items with codes, names and descriptions; if nothing is passed, then all them of the language.
        /// </returns>
        public IEnumerable<MetaData> GetModulesNameAndDescriptions(IEnumerable<string> codes)
        {
            return PageMetadataUtility.GetModulesMetaData(codes, InterfaceLanguage);
        }

        /// <summary>
        /// The get modules by module type.
        /// </summary>
        /// <param name="moduleType">
        /// The module type.
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType)
        {
            var moduleIdByMetadataList = new List<ModuleIdByMetadata>();
            var request = new GetModulesListRequest
                              {
                                  InterfaceLanguage = Mapper.Map<Language>(InterfaceLanguage),
                                  ModuleQualifierCollection = new ModuleQualifierCollection { Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Factiva },
                                  ReturnType = ReturnType.Summary,
                                  TypeCollection = new ModuleTypeCollection { Mapper.Map<FactivaModuleType>(moduleType) },
                                  SortBy = SortBy.Name,
                                  SortOrder = SortOrder.Ascending,
                              };

            var getModuleListResponse = Process<GetModulesListResponse>(request);
            if (getModuleListResponse != null && getModuleListResponse.ModuleListInfoCollection != null
                && getModuleListResponse.ModuleListInfoCollection.Count > 0)
            {
                moduleIdByMetadataList = AssembleModulesListModel(getModuleListResponse.ModuleListInfoCollection);
            }

            return moduleIdByMetadataList;
        }

        public IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType, MetaDataType metaDataType, List<MetaData> metaDataCollection = null)
        {
            var moduleIdByMetadataList = new List<ModuleIdByMetadata>();
            var intLanguage = Mapper.Map<Language>(InterfaceLanguage);
            var request = new GetModulesListRequest
                              {
                                  InterfaceLanguage = intLanguage,
                                  ModuleQualifierCollection = new ModuleQualifierCollection
                                                                  {
                                                                      Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Factiva
                                                                  },
                                  ReturnType = ReturnType.Full,
                                  TypeCollection = new ModuleTypeCollection
                                                       {
                                                           Mapper.Map<FactivaModuleType>(moduleType)
                                                       },
                                  SortBy = SortBy.Name,
                                  SortOrder = SortOrder.Ascending,
                                  SearchFilter = AssembleModuleMetaDataType(metaDataType, metaDataCollection)
                              };

            var getModuleListResponse = Process<GetModulesListResponse>(request);

            if (getModuleListResponse != null && getModuleListResponse.ModuleListInfoCollection != null
                && getModuleListResponse.ModuleListInfoCollection.Count > 0)
            {
                moduleIdByMetadataList = AssembleModulesListModel(getModuleListResponse.ModuleListInfoCollection);
            }

            return moduleIdByMetadataList;
        }

        /// <summary>
        /// The get page name.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>
        /// The get page name.
        /// </returns>
        public string GetPageName(string pageId)
        {
            var request = new GetPageTitleDescriptionsRequest
                              {
                                  PageID = ExtractPageId(pageId)
                              };
            var response = Process<GetPageTitleDescriptionsResponse>(request);

            var titleDescription = response.TitleDescriptionsCollection.FirstOrDefault();

            return titleDescription == null ? null : titleDescription.Title;
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="cachePage">
        /// if set to <c>true</c> [cache page].
        /// </param>
        /// <param name="forceCacheRefresh">
        /// if set to <c>true</c> [force cache refresh].
        /// </param>
        /// <returns>
        /// A page object.
        /// </returns>
        public GWPage GetPage(string pageId, bool cachePage, bool forceCacheRefresh)
        {
            var pageRef = ExtractPageRef(pageId, forceCacheRefresh);

            var getPageByIDRequest = new GetPageByIDRequest
                                         {
                                             Id = pageRef.PageId,
                                             InterfaceLanguage = Mapper.Map<Language>(InterfaceLanguage),
                                         };

            var controlData = ControlData;

            if (PageRef.CachingEnabled && cachePage && pageRef.IsValidCacheKey)
            {
                getPageByIDRequest.Id = pageRef.PageId;
                controlData = pageRef.GetCacheControlData(controlData, Product);
            }

            var page = Process<GetPageByIDResponse>(getPageByIDRequest, controlData).Page;
            if (page != null && page.ModuleCollection != null)
            {
                foreach (var module in page.ModuleCollection.Cast<ModuleEx>().Where(module => module != null))
                {
                    UpdateModuleTitleAndDescription(module);
                }
            }

            return page;
        }

        /// <summary>
        /// Gets the page list info collection.
        /// </summary>
        /// <param name="pageTypes">
        /// The page types.
        /// </param>
        /// <param name="sortOrder">
        /// The sort order.
        /// </param>
        /// <param name="sortBy">
        /// The sort by.
        /// </param>
        /// <returns>
        /// A collection of Page Information
        /// </returns>
        public PageListInfoCollection GetPageListInfoCollection(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy)
        {
            var request = new GetPagesListRequest();
            request.TypeCollection.AddRange(pageTypes);
            request.SortBy = sortBy;
            request.SortOrder = sortOrder;
            return Process<GetPagesListResponse>(request).PageListInfoCollection;
        }

        /// <summary>
        /// The make personal alerts public.
        /// </summary>
        /// <param name="alertIds">
        /// The alert ids.
        /// </param>
        public void MakePersonalAlertsPublic(IEnumerable<int> alertIds)
        {
            if (alertIds == null || !alertIds.Any())
                return;

            var request = new SetFolderSharePropertiesRequest
                              {
                                  folderShareDetails = alertIds.Select(alertId => new FolderShareDetails
                                                                                      {
                                                                                          folderId = alertId,
                                                                                          SharingData = new Factiva.Gateway.Messages.Assets.V1_0.ShareProperties
                                                                                                            {
                                                                                                                accessControlScope = ShareAccessScope.Everyone,
                                                                                                                allowCopy = false,
                                                                                                                assignedScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal,
                                                                                                                externalAccess = ShareAccess.Allow,
                                                                                                                listingScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal,
                                                                                                                sharePromotion = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal
                                                                                                            }
                                                                                      }).ToArray()
                              };

            Process<SetFolderSharePropertiesResponse>(request);
        }


        public void ShareAdditonalPageAssets(IEnumerable<IShareAssets> assetsToShare)
        {
            try
            {
                var taskFactory = new TaskFactory();
                var tasks = (from asset in assetsToShare
                             let task = taskFactory.StartNew(() => asset.Share(), TaskCreationOptions.None)
                             select task).ToList();
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    Log.Error("ShareAddtionalAsset failed with exception : {0}", ex);
                }
                //Throwing only first exception.
                throw ae.Flatten().InnerExceptions[0];
            }
           
        }


        /// <summary>
        /// The make personal alerts public.
        /// </summary>
        /// <param name="alertIds">
        /// The alert ids.
        /// </param>
        public void MakePersonalSavedSearchesPublic(IEnumerable<int> savedSearchIds)
        {
            if (savedSearchIds == null || !savedSearchIds.Any())
                return;


        }


        /// <summary>
        /// Publish the user news page
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="personalAlertIds">
        /// The personal alert ids.
        /// </param>
        public void PublishPage(string pageId, IEnumerable<int> personalAlertIds)
        {
            //var page = GetPage(pageId, false, false);

            //if (page != null && page.ModuleCollection != null)
            //{
            //    //MakePageModulesPublic(page.ModuleCollection);
            //    MakePersonalAlertsPublic(personalAlertIds);
            //}
            //[HD: Updated to reduce number of PAM transactions to share the page]
            MakePersonalAlertsPublic(personalAlertIds);
            SetPageShareProperties(
                pageId,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Account,
                        ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        /* Since the Listing Scope setting was not allowing us to un-publish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                                               and so, the page should be @ Account level to get into the page garden*/
                        AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                    }, 
                true,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Account,
                        ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                    });
        }



        public void PublishPage(string pageId, IEnumerable<IShareAssets> assetsToShare)
        {
            //var page = GetPage(pageId, false, false);

            //if (page != null && page.ModuleCollection != null)
            //{
            //    MakePageModulesPublic(page.ModuleCollection);
            //    ShareAdditonalPageAssets(assetsToShare);
            //    //MakePersonalAlertsPublic(personalAlertIds);
            //}

            //[HD: Updated to reduce number of PAM transactions to share the page]
            ShareAdditonalPageAssets(assetsToShare);
            SetPageShareProperties(
                pageId,
                new ShareProperties
                {
                    AccessControlScope = AccessControlScope.Account,
                    ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    /* Since the Listing Scope setting was not allowing us to un-publish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                                           and so, the page should be @ Account level to get into the page garden*/
                    AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                },
                true,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Account,
                        ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                    });
        }
        /// <summary>
        /// Removes an assigned page from the user's list. This is when user is dismissing a page from his/her list.
        ///   Not an admin action
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <returns>
        /// A Boolean.
        /// </returns>
        public bool RemoveAssignedPage(string pageId)
        {
            return EnableDisable.Disable == EnableDisablePage(pageId, EnableDisable.Disable);
        }

        public virtual string ReplaceModuleOnPage(string pageId, string rootIDOfNewModule, GWModule moduleToReplaceWith, string moduleIdToRemove)
        {
            var replaceModulesOnPageRequest = new ReplaceModuleOnPageRequest
            {
                PageID = ExtractPageId(pageId),
                ModulePositionToReplace = new ModulePosition { Id = ExtractModuleId(moduleIdToRemove) },
                ModuleToReplaceWith = moduleToReplaceWith
            };

            if (!string.IsNullOrWhiteSpace(rootIDOfNewModule))
            {
                replaceModulesOnPageRequest.RootIDOfNewModule = ExtractModuleId(rootIDOfNewModule);
            }

            var response = Process<ReplaceModuleOnPageResponse>(replaceModulesOnPageRequest);
            return response.Id.ToString();
        }

        /// <summary>
        /// subscribe to the page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="pagePosition">
        /// The page position.
        /// </param>
        /// <returns>
        /// The subscribe to page.
        /// </returns>
        public string SubscribeToPage(string pageId, int pagePosition)
        {
            var request = new SubscribePageRequest
                              {
                                  PagePosition = new Factiva.Gateway.Messages.Assets.Pages.V1_0.PagePosition
                                                     {
                                                         Id = ExtractPageId(pageId),
                                                         Position = pagePosition
                                                     }
                              };
            var response = Process<SubscribePageResponse>(request);
            return response.Id.ToString();
        }

        /// <summary>
        /// UnPublish the user news page
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        public void UnpublishPage(string pageId)
        {
            // get the page, and make he modules private...
            //var page = GetPage(pageId, false, false);

            //if (page != null && page.ModuleCollection != null)
            //{
            //    MakePageModulesPrivate(page.ModuleCollection);
            //}

            //[HD: Updated to reduce number of PAM transactions to unshare the page]
            SetPageShareProperties(
                pageId,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Personal,
                        ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                    },
                true,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Personal,
                        ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                    });
        }

        public void UnpublishPage(string pageId, IEnumerable<IShareAssets> assetsToUnShare)
        {
            //// get the page, and make he modules private...
            //var page = GetPage(pageId, false, false);

            //if (page != null && page.ModuleCollection != null)
            //{
            //    MakePageModulesPrivate(page.ModuleCollection);
            //    ShareAdditonalPageAssets(assetsToUnShare);
            //}
            //[HD: Updated to reduce number of PAM transactions to unshare the page]
            ShareAdditonalPageAssets(assetsToUnShare);
            SetPageShareProperties(
                pageId,
                new ShareProperties
                {
                    AccessControlScope = AccessControlScope.Personal,
                    ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                },
                true,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Personal,
                        ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                        SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                    });
        }


        public void SetPageAndModuleShareProperties()
        {
            
        }

        /// <summary>
        /// The un subscribe to page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        public void UnsubscribeToPage(string pageId)
        {
            var request = new UnSubscribePageRequest { Id = ExtractPageId(pageId) };
            Process<UnSubscribePageResponse>(request);
        }

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        public void UpdateModule(string pageId, string moduleId, IEnumerable<Property> properties)
        {
            var module = GetModuleById(pageId, moduleId);
            if (module == null)
            {
                return;
            }

            UpdateModule(module, properties);
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = ExtractPageId(pageId) };

            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Process<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        public void UpdateModule(ModuleEx module)
        {
            Process<UpdateModuleResponse>(new UpdateModuleRequest { Module = module });
        }

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        public void UpdateModule(GWModule module, IEnumerable<Property> properties)
        {
            if (module == null)
            {
                throw new NullReferenceException("module cannot be null");
            }

            foreach (var property in properties)
            {
                var prop = module.GetType().GetProperty(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (prop == null)
                {
                    continue;
                }

                if (prop.CanWrite)
                {
                    prop.SetValue(module, ObjectExtensions.GetObject(property.value, prop.PropertyType), null);
                }
            }
        }

        /// <summary>
        /// The update modules on page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <param name="rootModuleId"></param>
        public void UpdateModulesOnPage(string pageId, GWModule module, string rootModuleId = null)
        {
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = ExtractPageId(pageId) };
            if (!string.IsNullOrWhiteSpace(rootModuleId))
                updateModulesOnPageRequest.RootID = ExtractModuleId(rootModuleId);
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Process<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        /// <summary>
        /// Updates the module positions on page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="zones">
        /// The modules.
        /// </param>
        public void UpdateModulePositionsOnPage(string pageId, IEnumerable<IEnumerable<int>> zones)
        {
            var updateModulePositionsRequest = new UpdateModulePositionsRequest { PageID = ExtractPageId(pageId) };
            var numberOfColums = zones.Count();
            if (numberOfColums <= 0)
            {
                throw new NullReferenceException("module cannot be null");
            }

            var column = 0;
            foreach (var i in zones)
            {
                var row = 0;
                foreach (var item in i)
                {
                    updateModulePositionsRequest.ModulePositions.ModulePositionCollection.Add(
                        new ModulePosition
                        {
                            Id = item,
                            Position = column + (row * numberOfColums)
                        });
                    row++;
                }

                column++;
            }

            Process<UpdateModulePositionsResponse>(updateModulePositionsRequest);
        }

        /// <summary>
        /// The update page positions.
        /// </summary>
        /// <param name="pagePositions">
        /// The page positions.
        /// </param>
        public void UpdatePagePositions(IEnumerable<PagePosition> pagePositions)
        {
            var pagePositionsCollection = AssemblePagePositionsCollection(pagePositions);
            var request = new UpdatePagePositionsRequest
            {
                PagePositions = new PagePositions { PagePositionCollection = pagePositionsCollection }
            };
            Process<UpdatePagePositionsResponse>(request);
        }

        /// <summary>
        /// Updates the state of the module.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void UpdateModuleState(string pageId, string moduleId, ModuleState state)
        {
            var page = GetPage(pageId, false, false);
            if (page == null ||
                page.ModuleCollection == null ||
                page.ModuleCollection.Count <= 0)
            {
                return;
            }

            var extractedModuleId = ExtractModuleId(moduleId);
            var module = page.ModuleCollection.FirstOrDefault(x => x.Id == extractedModuleId);

            if (module == null)
            {
                return;
            }

            module.ModuleState = state;
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = ExtractPageId(pageId) };
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Process<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        /// <summary>
        /// Updates the user news page title and description
        /// </summary>
        /// <param name="page">
        /// A page object.
        /// </param>
        public void UpdateTitleDescription(Page page)
        {
            var request = new UpdatePageTitleRequest
            {
                Id = ExtractPageId(page.ID),
                Title = page.Title,
                Description = page.Description,
                InterfaceLanguage = Mapper.Map<Language>(InterfaceLanguage)
            };

            Process<UpdatePageTitleResponse>(request);
        }

        /// <summary>
        /// The update page.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        public void UpdatePage(GWPage page)
        {
            var request = new UpdatePageRequest
            {
                Page = page,
            };

            Process<UpdatePageResponse>(request);
        }

        /// <summary>
        /// The get admin page owner.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <returns>
        /// </returns>
        public List<KeyValuePair<string, string>> GetAdminPageOwner(int pageId)
        {
            var userInformation = new List<KeyValuePair<string, string>>();
            var request = new GetOwnerByAssetIDRequest
            {
                ID = pageId,
                Type = AssetType.Page
            };
            var response = Process<GetOwnerByAssetIDResponse>(request);
            if (response != null && !string.IsNullOrEmpty(response.UserID) && !string.IsNullOrEmpty(response.Namespace))
            {
                userInformation.Add(new KeyValuePair<string, string>("OwnerUserId", response.UserID));
                userInformation.Add(new KeyValuePair<string, string>("OwnerNamespace", response.Namespace));
            }

            return userInformation;
        }

        /// <summary>
        /// The enable disable page.
        /// </summary>
        /// <param name="pageRef">
        /// The page id.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// </returns>
        public EnableDisable EnableDisablePage(string pageRef, EnableDisable action = EnableDisable.Enable)
        {
            var request = new EnableDisableAssignedPageRequest
            {
                Id = ExtractPageId(pageRef),
                Action = action
            };
            var response = Process<EnableDisableAssignedPageResponse>(request);
            return response.Result;
        }

        /// <summary>
        /// The extract module id.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// The extract module id.
        /// </returns>
        public virtual int ExtractModuleId(string moduleId)
        {
            return int.Parse(moduleId);
        }

        /// <summary>
        /// The extract page id.
        /// </summary>
        /// <param name="pageRef">
        /// The page ref.
        /// </param>
        /// <returns>
        /// The extract page id.
        /// </returns>
        public virtual int ExtractPageId(string pageRef)
        {
            return ExtractPageRef(pageRef).PageId;
        }

        /// <summary>
        /// The extract page ref.
        /// </summary>
        /// <param name="pageRef">
        /// The page ref.
        /// </param>
        /// <param name="forceCacheRefresh">
        /// The force cache refresh.
        /// </param>
        /// <returns>
        /// </returns>
        public virtual PageRef ExtractPageRef(string pageRef, bool forceCacheRefresh = false)
        {
            return new PageRef(pageRef, Product, forceCacheRefresh);
        }

        /// <summary>
        /// The get module by id.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// </returns>
        public GWModule GetModuleById(string moduleId)
        {
            var getModuleByIDRequest = new GetModuleByIDRequest
            {
                ModuleID = ExtractModuleId(moduleId),
                InterfaceLanguage = Mapper.Map<Language>(InterfaceLanguage),
            };

            var module = Process<GetModuleByIDResponse>(getModuleByIDRequest).Module;
            UpdateModuleTitleAndDescription(module as ModuleEx);
            return module;
        }

        public AccessControlScope GetRootAccessControlScope(int pageId, ShareProperties properties, Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier accessQualifier)
        {
            var propertiesResponse = properties as SharePropertiesResponse;

            if (propertiesResponse == null)
                return AccessControlScope.Personal;

            return GetRootAccessControlScope(pageId, propertiesResponse.RootID, properties.AccessControlScope, accessQualifier);
        }

        public PageListInfoExCollection GetSubscribablePages(PageType pageType, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier> accessQualifiers, MetadataFilter metadataFilter)
        {
            var pageQualifiers = new PageQualifierCollection();
            pageQualifiers.AddRange(accessQualifiers);
            var request = new GetSubscribablePagesRequest
            {
                InterfaceLanguage = Mapper.Map<Language>(InterfaceLanguage),
                PageQualifierCollection = pageQualifiers,
                SearchFilter = metadataFilter,
                PageType = pageType,
                ShareScopeCollection = new ShareScopeCollection()
            };
            return Process<GetSubscribablePagesResponse>(request).PageListInfoExCollection;
        }

        public IEnumerable<PageListInfo> GetUserPageList(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, bool forceCacheRefresh = false)
        {
            var request = new GetPagesListRequest
            {
                PageQualifierCollection = new PageQualifierCollection()
            };
            request.TypeCollection.AddRange(pageTypes);
            request.SortBy = sortBy;
            request.SortOrder = sortOrder;

            var tempControlData = ControlData;

            if (PageListCacheKey.CachingEnabled)
            {
                var cacheKey = new PageListCacheKey(Product)
                {
                    CacheForceCacheRefresh = forceCacheRefresh
                };
                tempControlData = cacheKey.GetCacheControlData(ControlData, Product);
            }

            var pageListColl = Invoke<GetPagesListResponse>(request, tempControlData).ObjectResponse.PageListInfoCollection;
            return pageListColl.ToList();
        }

        /// <summary>
        /// Gets the page list info collection.
        /// </summary>
        /// <param name="pageTypes">
        /// The page types.
        /// </param>
        /// <param name="sortOrder">
        /// The sort order.
        /// </param>
        /// <param name="sortBy">
        /// The sort by.
        /// </param>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="pageDefaultedBy">
        /// The page defaulted by.
        /// </param>
        /// <param name="pagePosition">
        /// The page position.
        /// </param>
        /// <returns>
        /// A collection of Page Information
        /// </returns>
        public GetPagesListWithPageResponse GetUserPageListWithDefaultPage(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, int pageId, PageDefaultBy pageDefaultedBy = PageDefaultBy.Position, int pagePosition = 1)
        {
            var request = new GetPagesListWithPageRequest();
            request.TypeCollection.AddRange(pageTypes);
            request.SortBy = sortBy;
            request.SortOrder = sortOrder;
            if (pageDefaultedBy == PageDefaultBy.Position)
            {
                request.ReturnPageAtPosition = pagePosition;
            }

            if (pageDefaultedBy == PageDefaultBy.PageId)
            {
                request.ReturnPageWithID = pageId;
            }

            return Process<GetPagesListWithPageResponse>(request);
        }

        /// <summary>
        /// The make page modules public.
        /// </summary>
        /// <param name="moduleCollection">
        /// The module collection.
        /// </param>
        public void MakePageModulesPublic(IEnumerable<GWModule> moduleCollection)
        {
            SetModuleShareProperties(
                moduleCollection,
                new ShareProperties
                {
                    AccessControlScope = AccessControlScope.Account,
                    ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                }
                );
        }

        /// <summary>
        /// The make page modules private.
        /// </summary>
        /// <param name="moduleCollection">
        /// The module collection.
        /// </param>
        public void MakePageModulesPrivate(IEnumerable<GWModule> moduleCollection)
        {
            SetModuleShareProperties(
                moduleCollection,
                new ShareProperties
                {
                    AccessControlScope = AccessControlScope.Personal,
                    ListingScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    AssignedScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal,
                    SharePromotion = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope.Personal
                }
                );
        }

        public void SetPageShareProperties(string pageRef, ShareProperties shareProperties)
        {
            SetPageShareProperties(pageRef, shareProperties, false, null);
        }

        public void SetPageShareProperties(string pageRef, ShareProperties shareProperties, bool updateModule, ShareProperties moduleShareProperties)
        {
            var request = new SetPageSharePropertiesRequest
            {
                PageID = ExtractPageId(pageRef),
                ShareProperties = shareProperties,
                UpdateModule = updateModule,
                ModuleShareProperties = moduleShareProperties
            };
            Process<SetPageSharePropertiesResponse>(request);
        }

        /// <summary>
        /// The assemble modules list model.
        /// </summary>
        /// <param name="moduleListInfoCollection">
        /// The module list info collection.
        /// </param>
        /// <returns>
        /// </returns>
        protected List<ModuleIdByMetadata> AssembleModulesListModel(IEnumerable moduleListInfoCollection)
        {
            var moduleIdByMetadataList = new List<ModuleIdByMetadata>();

            foreach (ModuleListInfo moduleListInfo in moduleListInfoCollection)
            {
                var moduleIdByMetadata = new ModuleIdByMetadata
                {
                    ModuleId = moduleListInfo.Id,
                    InterfaceLanguage = moduleListInfo.InterfaceLanguage.ToString() // Mapper.Map<string>(moduleListInfo.InterfaceLanguage),
                };


                var moduleEx = moduleListInfo.Module as ModuleEx;
                var moduleProperties = moduleEx != null ? moduleEx.ModuleProperties : moduleListInfo.ModuleProperties;
                if (moduleProperties != null)
                {
                    moduleIdByMetadata.MetaData = AssemblePageModuleMetadata(moduleProperties.ModuleMetaData);
                }

                moduleIdByMetadataList.Add(moduleIdByMetadata);
            }

            return moduleIdByMetadataList;
        }

        protected Filter AssembleModuleMetaDataType(MetaDataType metaDataType, List<MetaData> metaDataCollection)
        {
            MetadataFilterType? metadataFilterType = null;
            switch (metaDataType)
            {
                case MetaDataType.Geographic:
                    metadataFilterType = MetadataFilterType.AllRegion;
                    break;
                case MetaDataType.Industry:
                    metadataFilterType = MetadataFilterType.AllIndustry;
                    break;
                case MetaDataType.Topic:
                    metadataFilterType = MetadataFilterType.AllTopic;
                    break;
            }

            SearchFilterTypeCollection filterTypeCollection;
            if (metadataFilterType.HasValue)
            {
                filterTypeCollection = new SearchFilterTypeCollection
                                           {
                                               metadataFilterType.Value
                                           };
            }
            else
            {
                filterTypeCollection = new SearchFilterTypeCollection
                                           {
                                               MetadataFilterType.AllTopic, 
                                               MetadataFilterType.AllRegion, 
                                               MetadataFilterType.AllIndustry
                                           };
            }

            Metadata mData = null;
            if (metaDataCollection != null && metaDataCollection.Count > 0)
            {
                mData = new Metadata(); 
                foreach (var metadata in metaDataCollection)
                {
                    switch (metadata.MetaDataType)
                    {
                        case MetaDataType.Geographic:
                            mData.RegionCollection.Add(new MetadataField { Text = metadata.MetaDataCode });
                            break;
                        case MetaDataType.Industry:
                            mData.IndustryCollection.Add(new MetadataField { Text = metadata.MetaDataCode });
                            break;
                    }
                }
            }

            return new MetadataFilter
            {
                SearchFilterTypeCollection = filterTypeCollection,
                Filter = mData
            };
        }

        private MetaData AssemblePageModuleMetadata(Metadata metadata)
        {
            var metaData = new MetaData();
            if (metadata != null)
            {
                if (metadata.IndustryCollection != null && metadata.IndustryCollection.Count > 0)
                {
                    metaData.MetaDataType = MetaDataType.Industry;
                    metaData.MetaDataCode = metadata.IndustryCollection[0].Text; // the code
                    metaData.MetaDataDescriptor = PageMetadataUtility.GetMetaDataName(metaData.MetaDataCode, InterfaceLanguage);
                }

                if (metadata.RegionCollection != null && metadata.RegionCollection.Count > 0)
                {
                    metaData.MetaDataType = MetaDataType.Geographic;
                    metaData.MetaDataCode = metadata.RegionCollection[0].Text; // the code
                    metaData.MetaDataDescriptor = PageMetadataUtility.GetMetaDataName(metaData.MetaDataCode, InterfaceLanguage);
                }

                //if (metadata.QueryFilterCollection != null && metadata.QueryFilterCollection.Count > 0)
                //{
                //    metaData.MetaDataType = MetaDataType.Topic;
                //    metaData.MetaDataCode = metadata.QueryFilterCollection[0].Text; // the code
                //    metaData.MetaDataDescriptor = PageMetadataUtility.GetMetaDataName(metaData.MetaDataCode, InterfaceLanguage);
                //}
            }

            return metaData;
        }

        public PagePositionCollection AssemblePagePositionsCollection(IEnumerable<PagePosition> pagePositions)
        {
            var coll = new PagePositionCollection();

            var positions =
                from page in pagePositions
                let pageRef = ExtractPageRef(page.PageId)
                select new Factiva.Gateway.Messages.Assets.Pages.V1_0.PagePosition
                {
                    Id = pageRef.PageId,
                    Position = page.Position,
                };

            coll.AddRange(positions);

            return coll;
        }

        private MetaData GetAffinityMetadata(ModuleEx source)
        {
            string code = null;
            if (source.ModuleProperties.ModuleMetaData.IndustryCollection != null && source.ModuleProperties.ModuleMetaData.IndustryCollection.Count > 0)
            {
                code = source.ModuleProperties.ModuleMetaData.IndustryCollection[0].Text;
            }
            else if (source.ModuleProperties.ModuleMetaData.RegionCollection != null && source.ModuleProperties.ModuleMetaData.RegionCollection.Count > 0)
            {
                code = source.ModuleProperties.ModuleMetaData.RegionCollection[0].Text;
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            var metaData = PageMetadataUtility.GetMetaDataMetaData(code, Preferences.InterfaceLanguage);
            return metaData;

            //return ": " + name;
        }

        public MetaData GetModuleMetaData(GWModule module)
        {
            var moduleName = module.GetType().Name;

            if (!ModuleTypesByName.ContainsKey(moduleName))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleType);
            }

            return GetModuleMetaData(ModuleTypesByName[moduleName]);
        }

        public MetaData GetModuleMetaData(ModuleType moduleType)
        {
            return PageMetadataUtility.GetModuleMetaData(GetIRTCode(moduleType), Preferences.InterfaceLanguage);
        }

        private static string GetIRTCode(ModuleType moduleType)
        {
            try
            {
                var memberInfo = typeof(ModuleType).GetMember(moduleType.ToString())[0];
                var attributes = memberInfo.GetCustomAttributes(typeof(IRTCodeAttribute), false);
                return attributes.Length == 0 ? null : ((IRTCodeAttribute)attributes[0]).Code;
            }
            catch (Exception)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleType);
            }
        }

        public AccessControlScope GetRootAccessControlScope(int pageId, int parentId, AccessControlScope accessControlScope, Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier qualifier)
        {
            if (parentId != pageId && qualifier == Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.User)
            {
                return AccessControlScope.Account;
            }

            if (qualifier == Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Factiva || accessControlScope == AccessControlScope.Everyone)
            {
                return AccessControlScope.Everyone;
            }

            if (qualifier == Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Account || accessControlScope == AccessControlScope.Account)
            {
                return AccessControlScope.Account;
            }

            return AccessControlScope.Personal;
        }

        public void SetModuleShareProperties(IEnumerable<GWModule> modules, ShareProperties shareProperties)
        {
            try
            {
                var taskFactory = new TaskFactory();
                var tasks = (from module in modules
                             let task = taskFactory.StartNew(() => SetModuleShareProperties(module.Id, shareProperties), TaskCreationOptions.None)
                             select task).ToList();
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    Log.Error("ShareAddtionalAsset failed with exception : {0}", ex);
                }
                //Throwing only first exception.
                throw ae.Flatten().InnerExceptions[0];
            }
        }

        public void SetModuleShareProperties(int moduleId, ShareProperties shareProperties)
        {
            try
            {
                var request = new SetModuleSharePropertiesRequest
                {
                    ModuleID = moduleId,
                    ShareProperties = shareProperties
                };
                Process<SetModuleSharePropertiesResponse>(request);
            }
            catch (Exception ex)
            {
                Log.Warn("Error setting module share properties for module " + moduleId, ex);
            }
        }

        private void UpdateModuleTitleAndDescription(ModuleEx module)
        {
            // Only update if it is a Factiva module, or a user module with empty title
            if (module == null)
                return;

            if (module.ModuleQualifier == Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Factiva ||
                string.IsNullOrEmpty(module.Title))
            {
                var moduleMetadata = GetModuleMetaData(module);
                if (moduleMetadata != null)
                {
                    module.Title = moduleMetadata.MetaDataDescriptor;
                    module.Description = moduleMetadata.MetaDataDescription;
                }
                var moduleAffinityMetadata = GetAffinityMetadata(module);
                if (moduleAffinityMetadata != null)
                {
                    if (string.IsNullOrEmpty(module.Title))
                        module.Title = moduleAffinityMetadata.MetaDataDescriptor;
                    else
                        module.Title += ": " + moduleAffinityMetadata.MetaDataDescriptor;
                }
            }
        }

        public void SetRootModuleId(string moduleId, string rootModuleId)
        {
            var request = new SetRootAssetRequest
            {
                ID = int.Parse(moduleId),
                RootID = ExtractModuleId(rootModuleId),
                Type = AssetType.Module
            };

            Process<SetRootAssetResponse>(request);
        }

        public void UpdateQueryFilter(string id, AssetType assetType, QueryFilters queryFilters)
        {
            int idInt;
            switch (assetType)
            {
                case AssetType.Page:
                    idInt = ExtractPageId(id);
                    break;
                case AssetType.Module:
                    idInt = ExtractModuleId(id);
                    break;
                default:
                    throw new NotImplementedException();

            }
            var updateQueryFiltersRequest = new UpdateQueryFiltersRequest { ID = idInt, Type = assetType, QueryFilters = queryFilters };
            Process<UpdateQueryFiltersResponse>(updateQueryFiltersRequest);
        }

        public void DeleteQueryFilter(string id, AssetType assetType)
        {
            int idInt;
            switch (assetType)
            {
                case AssetType.Page:
                    idInt = ExtractPageId(id);
                    break;
                case AssetType.Module:
                    idInt = ExtractModuleId(id);
                    break;
                default:
                    throw new NotImplementedException();

            }
            var deleteQueryFiltersRequest = new DeleteQueryFiltersRequest { ID = idInt, Type = assetType };
            Process<DeleteQueryFiltersResponse>(deleteQueryFiltersRequest);
        }
    }
}