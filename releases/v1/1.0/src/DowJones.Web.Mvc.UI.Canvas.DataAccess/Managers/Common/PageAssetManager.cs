// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageAssetManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DowJones.DependencyInjection;
using DowJones.Managers.PAM;
using DowJones.Session;
using DowJones.Utilities.Ajax.Canvas;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using log4net;
using CacheScope = Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using Module = Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;
using Product = DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Product;
using SortOrder = Factiva.Gateway.Messages.Assets.Common.V2_0.SortOrder;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common
{
    public class PageAssetsManager : AbstractAggregationManager, IPageAssetsManager
    {
        public const string NotSpecified = "Not_Specified";

        private readonly string interfaceLanguage;
        private readonly IPreferences preferences;

        /// <summary>
        /// Internal logger
        /// </summary>
        private static readonly ILog InternalLog = LogManager.GetLogger(typeof(PageAssetsManager));

        #region ..:: Constructor ::..

        /// <summary>
        /// Initializes a new instance of the <see cref="PageAssetsManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public PageAssetsManager(ControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
            this.interfaceLanguage = LanguageUtilityManager.ValidateLanguageCode(interfaceLanguage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageAssetsManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="preferences">The preferences.</param>
        [Inject("Disambiguating multiple constructors")]
        public PageAssetsManager(IControlData controlData, IPreferences preferences)
            : this(ControlDataManager.Convert(controlData), preferences.InterfaceLanguage)
        {
            this.preferences = preferences;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageAssetsManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="preferences">The preferences.</param>
        public PageAssetsManager(ControlData controlData, IPreferences preferences)
            : this(controlData, preferences.InterfaceLanguage)
        {
            this.preferences = preferences;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageAssetsManager"/> class.
        /// </summary>
        /// <param name="sessionID">The session ID.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public PageAssetsManager(string sessionID, string clientTypeCode, string accessPointCode, string interfaceLanguage)
            : base(sessionID, clientTypeCode, accessPointCode)
        {
            this.interfaceLanguage = LanguageUtilityManager.ValidateLanguageCode(interfaceLanguage);
        }

        #endregion

        #region ..:: Overrides of AbstractAggregationManager ::..

        public string InterfaceLanguage
        {
            get { return interfaceLanguage; }
        }

        public IPreferences Preferences
        {
            get { return preferences; }
        }

        protected override ILog Log
        {
            get { return InternalLog; }
        }

        #endregion

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="properties">The properties.</param>
        public static void UpdateModule(Module module, ICollection<Property> properties)
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
        /// Gets the module by id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <returns>A module object.</returns>
        public Module GetModuleById(string pageId, string moduleId)
        {
            if (pageId != NotSpecified)
            {
                var page = GetPage(pageId);

                if (page.ModuleCollection == null || page.ModuleCollection.Count <= 0)
                {
                    return null;
                }

                return FindModule(page.ModuleCollection, moduleId);
            }

            return GetModuleById(moduleId);
        }

        /// <summary>
        /// Deletes the modules.
        /// </summary>
        /// <param name="pageId">The canvas id.</param>
        /// <param name="moduleIds">The module ids.</param>
        public void DeleteModules(string pageId, List<string> moduleIds)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;

            var deleteModulesFromPageRequest = new DeleteModulesFromPageRequest
            {
                PageID = pId,
            };

            deleteModulesFromPageRequest.ModuleIDCollection.AddRange(moduleIds.ConvertAll(ExtractModuleId));
            Process<DeleteModulesFromPageResponse>(deleteModulesFromPageRequest);
        }
        
        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <param name="page">The new page object.</param>
        /// <returns>A updated page object</returns>
        public Page CreatePage(CreatePageRequest page)
        {
            var createPageResponse = Process<CreatePageResponse>(page);
            return createPageResponse.Id > 0 ? GetPage(createPageResponse.Id.ToString()) : null;
        }

        /// <summary>
        /// Adds the module to page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="newModule">The new module.</param>
        /// <param name="routine">The routine.</param>
        /// <returns>A string that represents the newModule Id</returns>
        public string AddModuleToPage(string pageId, int numberOfColumns, Module newModule, ValidationRoutine routine)
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
        /// Adds the module to end of page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="newModule">The new module.</param>
        /// <param name="routine">The routine.</param>
        /// <returns>A string with the module id</returns>
        public string AddModuleToEndOfPage(string pageId, Module newModule, ValidationRoutine routine)
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
        /// Updates the module positions on page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="zones">The modules.</param>
        public void UpdateModulePositionsOnPage(string pageId, IEnumerable<IEnumerable<int>> zones)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var updateModulePositionsRequest = new UpdateModulePositionsRequest { PageID = pId };
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
        /// Updates the state of the module.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="state">The state.</param>
        public void UpdateModuleState(string pageId, string moduleId, ModuleState state)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var page = GetPage(pageId, false, false);

            if (page == null ||
                page.ModuleCollection == null ||
                page.ModuleCollection.Count <= 0)
            {
                return;
            }

            var module = FindModule(page.ModuleCollection, moduleId);

            if (module == null)
            {
                return;
            }

            module.ModuleState = state;
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = pId };
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Process<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>
        /// A page object.
        /// </returns>
        public Page GetPage(string pageId)
        {
            return GetPage(pageId, true);
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="cachePage">if set to <c>true</c> [cache page].</param>
        /// <returns>A <see cref="Page"/> object.</returns>
        public Page GetPage(string pageId, bool cachePage = true)
        {
            return GetPage(pageId, false, cachePage);
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="forceCacheRefresh">if set to <c>true</c> [force cache refresh].</param>
        /// <param name="cachePage">if set to <c>true</c> [cache page].</param>
        /// <returns>
        /// A page object.
        /// </returns>
        public Page GetPage(string pageId, bool forceCacheRefresh = false, bool cachePage = true)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;

            var getPageByIDRequest = new GetPageByIDRequest
                                            {
                                                Id = pId,
                                                InterfaceLanguage = MapInterfaceLanguage(InterfaceLanguage),
                                            };

            var controlData = ControlData;

            if (Settings.Default.IncludeCacheKeyGeneration &&
                Settings.Default.CachePageItems &&
                cachePage &&
                dissectedPageRef.IsValid)
            {
                dissectedPageRef.CacheKeyGenerator.CacheForceCacheRefresh = forceCacheRefresh;
                getPageByIDRequest.Id = dissectedPageRef.CacheKeyGenerator.GetPageId();
                controlData = dissectedPageRef.CacheKeyGenerator.GetCacheControlData(controlData);
            }

            var page = Process<GetPageByIDResponse>(getPageByIDRequest, controlData).Page;
            if (page != null && page.ModuleCollection != null)
            {
                foreach (var module in page.ModuleCollection.Cast<ModuleEx>().Where(module => module != null))
                {
                    UpdateModuleTitle(module);
                }
            }

            return page;
        }

        private void UpdateModuleTitle(ModuleEx module)
        {
            if (module == null)
            {
                return;
            }

            module.Title = string.IsNullOrWhiteSpace(module.Title) ? GetModuleTitle(module) + GetMetadataAffinity(module) : module.Title;
        }

        /// <summary>
        /// Deletes the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        public void DeletePage(string pageId)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var request = new DeletePageRequest
            {
                Id = pId,
            };

            Process<DeletePageResponse>(request);
        }

        /// <summary>
        /// Gets the page list info collection.
        /// </summary>
        /// <param name="pageTypes">The page types.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <returns>A collection of Page Information</returns>
        public PageListInfoCollection GetPageListInfoCollection(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy)
        {
            var request = new GetPagesListRequest();
            request.TypeCollection.AddRange(pageTypes);
            request.SortBy = sortBy;
            request.SortOrder = sortOrder;
            return Process<GetPagesListResponse>(request).PageListInfoCollection;
        }

        /// <summary>
        /// Deletes all pages.
        /// </summary>
        /// <param name="pageTypes">The page types.</param>
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
        /// <param name="pageListInfoCollection">The page list info collection.</param>
        public void DeleteAllPages(PageListInfoCollection pageListInfoCollection)
        {
            if (pageListInfoCollection == null)
            {
                pageListInfoCollection = GetPageListInfoCollection(
                                            new[] { PageType.BRI, PageType.CVD, PageType.Discovery, PageType.DJConsultancy, PageType.MM, PageType.PNP, PageType.Prototype },
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
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheScope">The cache scope.</param>
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

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="properties">The properties.</param>
        public void UpdateModule(string pageId, string moduleId, ICollection<Property> properties)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var module = GetModuleById(pageId, moduleId);
            if (module == null)
            {
                return;
            }

            UpdateModule(module, properties);
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = pId };
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Process<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        public void UpdateModulesOnPage(string pageId, Module module)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = pId };
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Process<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }
        
        protected internal static int ExtractModuleId(string moduleId)
        {
            return Int32.Parse(moduleId);
        }
        
        protected internal Module GetModuleById(string moduleId)
        {
            var getModuleByIDRequest = new GetModuleByIDRequest
            {
                ModuleID = ExtractModuleId(moduleId),
                InterfaceLanguage = MapInterfaceLanguage(InterfaceLanguage),
            };

            var module = Process<GetModuleByIDResponse>(getModuleByIDRequest).Module;
            UpdateModuleTitle(module as ModuleEx);
            return module;
        }

        protected internal DissectedPageRef ExtractPageId(string pageId)
        {
            var pageRef = new DissectedPageRef();
            try
            {
                int retNum;
                pageRef.IsNumeric = Int32.TryParse(pageId, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
                if (pageRef.IsNumeric)
                {
                    pageRef.IsValid = false;
                    pageRef.ExtractedId = retNum;
                    return pageRef;
                }

                pageRef.CacheKeyGenerator = new PageCacheKeyGenerator(pageId);
                int outId;
                if (Int32.TryParse(pageRef.CacheKeyGenerator.PageId, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out outId))
                 {
                    pageRef.ExtractedId = outId;
                }
                else
                {
                    pageRef.IsValid = false;
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.InvalidCacheKey);
            }

            return pageRef;
        }

        /// <summary>
        /// The map interface language.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <returns>A language code.</returns>
        protected static Language MapInterfaceLanguage(string interfaceLanguage)
        {
            switch (interfaceLanguage.ToLower())
            {
                case "en":
                    return Language.en;
                case "de":
                    return Language.de;
                case "fr":
                    return Language.fr;
                case "es":
                    return Language.es;
                case "it":
                    return Language.it;
                case "ru":
                    return Language.ru;
                case "ja":
                    return Language.ja;
                case "zhcn":
                    return Language.zhcn;
                case "zhtw":
                    return Language.zhtw;
            }

            return Language.en;
        }

        /// <summary>
        /// The map language enum to string.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <returns>
        /// The map language to string.
        /// </returns>
        protected static string MapLanguageToString(Language interfaceLanguage)
        {
            switch (interfaceLanguage)
            {
                case Language.en:
                case Language.es:
                case Language.fr:
                case Language.de:
                case Language.it:
                case Language.ja:
                case Language.ru:
                case Language.zhcn:
                case Language.zhtw:
                    return interfaceLanguage.ToString();
            }

            return "en";
        }
        
        /// <summary>
        /// Finds the module.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="moduleId">The module id.</param>
        /// <returns>A module object.</returns>
        protected static Module FindModule(ICollection<Module> collection, string moduleId)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }

            return collection.FirstOrDefault(module => module.Id == ExtractModuleId(moduleId));
        }

        private string GetMetadataAffinity(ModuleEx source)
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
                return string.Empty;
            }

            var name = NewspageMetadataUtility.GetMetaDataName(code, Preferences.InterfaceLanguage);
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            return ": " + name;
        }

        private string GetModuleTitle(Module factivaModule)
        {
            return NewspageMetadataUtility.GetModuleName(ModuleTypeHelper.GetModuleType(factivaModule).GetIRTCode(), Preferences.InterfaceLanguage);
        }

        protected internal class DissectedPageRef
        {
            public DissectedPageRef(Product product = Product.Np)
            {
                CacheKeyGenerator = new PageCacheKeyGenerator(product);
                IsValid = true;
            }   

            public bool IsNumeric { get; set; }
            
            public int ExtractedId { get; set; }

            public bool IsValid { get; set; }

            public PageCacheKeyGenerator CacheKeyGenerator { get; set; }
        }

        /// <summary>
        /// Adds the module ids to page.
        /// </summary>
        /// <param name="pageRef">The page ref.</param>
        /// <param name="moduleIdWithPositionCollection">The module id with position collection.</param>
        public void AddModuleIdsToPage(string pageRef, List<KeyValuePair<int, int>> moduleIdWithPositionCollection)
        {
            var dissectedPageRef = ExtractPageId(pageRef);
            var pId = dissectedPageRef.ExtractedId;

            var request = new AddModulesToPageRequest
            {
                PageID = pId,
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
        /// <param name="pageRef">The page reference.</param>
        /// <param name="moduleIdCollection">The module id collection.</param>
        public void AddModuleIdsToPage(string pageRef, List<string> moduleIdCollection)
        {
            var dissectedPageRef = ExtractPageId(pageRef);
            var pId = dissectedPageRef.ExtractedId;

            if (moduleIdCollection == null || moduleIdCollection.Count <= 0)
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
                                  PageID = pId,
                                  ModulePositionCollection = new ModulePositionCollection()
                              };

            var i = 0;
            foreach (var modId in moduleIdCollection)
            {
                int moduleId;
                if (Int32.TryParse(modId, out moduleId))
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
        /// <param name="pageRef">The page reference.</param>
        /// <param name="moduleIdCollection">The module id collection.</param>
        public void AddModuleIdsToEndOfPage(string pageRef, List<string> moduleIdCollection)
        {
            var dissectedPageRef = ExtractPageId(pageRef);
            var pId = dissectedPageRef.ExtractedId;

            if (moduleIdCollection == null || moduleIdCollection.Count <= 0)
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
                PageID = pId,
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
                if (Int32.TryParse(modId, out moduleId))
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
    }
}
