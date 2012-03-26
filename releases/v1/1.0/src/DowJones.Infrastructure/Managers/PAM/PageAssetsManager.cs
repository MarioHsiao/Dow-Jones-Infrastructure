// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageAssetsManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the PageAssetsManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DowJones.Session;
using DowJones.Utilities.Ajax.Canvas;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using log4net;
using CacheScope = Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using Module = Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;
using SortOrder = Factiva.Gateway.Messages.Assets.Common.V2_0.SortOrder;

namespace DowJones.Managers.PAM
{
    public delegate void ValidationRoutine(Page page, Module module);

    public class PageAssetsManager : AbstractAggregationManager, IPageAssetsManager
    {
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
            this.interfaceLanguage = interfaceLanguage;
        }

        #endregion

        #region ..:: Overrides of AbstractAggregationManager ::..

        public string InterfaceLangugage
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
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);
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
            var getPageByIDRequest = new GetPageByIDRequest { Id = ExtractPageId(pageId) };
            
            var serviceResponse = Invoke<GetPageByIDResponse>(getPageByIDRequest);
            if (serviceResponse.ObjectResponse == null ||
                serviceResponse.ObjectResponse.Page == null ||
                serviceResponse.ObjectResponse.Page.ModuleCollection == null ||
                serviceResponse.ObjectResponse.Page.ModuleCollection.Count <= 0)
            {
                return null;
            }

            return FindModule(serviceResponse.ObjectResponse.Page.ModuleCollection, moduleId);
        }

        /// <summary>
        /// Deletes the modules.
        /// </summary>
        /// <param name="pageId">The canvas id.</param>
        /// <param name="moduleIds">The module ids.</param>
        public void DeleteModules(string pageId, List<string> moduleIds)
        {
            var deleteModulesFromPageRequest = new DeleteModulesFromPageRequest
                                                   {
                                                       PageID = ExtractPageId(pageId),
                                                   };

            deleteModulesFromPageRequest.ModuleIDCollection.AddRange(moduleIds.ConvertAll(ExtractModuleId));
            Invoke<DeleteModulesFromPageResponse>(deleteModulesFromPageRequest);
        }
        
        /// <summary>
        /// Deletes the modules for media monitor.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleIds">The module ids.</param>
        public void DeleteModulesForMediaMonitor(string pageId, List<string> moduleIds)
        {
            var pId = ExtractPageId(pageId);

            if (moduleIds.Count != 1)
            {
                return;
            }

            // Must delete the queryId;
            var getPageByIDRequest = new GetPageByIDRequest
                                         {
                                             Id = pId,
                                         };
            var serviceResponse = Invoke<GetPageByIDResponse>(getPageByIDRequest);

            if (serviceResponse.ObjectResponse == null ||
                serviceResponse.ObjectResponse.Page == null ||
                serviceResponse.ObjectResponse.Page.ModuleCollection == null ||
                serviceResponse.ObjectResponse.Page.ModuleCollection.Count <= 0)
            {
                return;
            }

            // check to see if we have a help module
            var hasHelpModule = serviceResponse.ObjectResponse.Page.ModuleCollection.OfType<HelpModule>().Any();

            var module = FindModule(serviceResponse.ObjectResponse.Page.ModuleCollection, moduleIds[0]);
            if (module == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);
            }

            if (!(module is MediaMonitorQueryModule))
            {
                return;
            }

            // Delete the Query Asset
            var deleteQueryRequest = new DeleteQueryRequest
                                         {
                                             Id = ((MediaMonitorQueryModule)module).QueryID
                                         };
            try
            {
                Invoke<DeleteQueryResponse>(deleteQueryRequest);
            }
            catch (DowJonesUtilitiesException ex)
            {
                if (ex.ReturnCode != 578005)
                {
                    throw;
                }
            }

            var deleteModulesFromPageRequest = new DeleteModulesFromPageRequest
                                                   {
                                                       PageID = ExtractPageId(pageId)
                                                   };

            deleteModulesFromPageRequest.ModuleIDCollection.AddRange(moduleIds.ConvertAll(ExtractModuleId));
            Invoke<DeleteModulesFromPageResponse>(deleteModulesFromPageRequest);

            if (serviceResponse.ObjectResponse.Page.ModuleCollection.Count != 3)
            {
                return;
            }

            // If there is no help module add it to the end;
            if (!hasHelpModule)
            {
                AddModuleToEndOfPage(pageId, new HelpModule(), null);
            }
        }

        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <param name="page">The new page object.</param>
        /// <returns>A updated page object</returns>
        public Page CreatePage(CreatePageRequest page)
        {
            var createPageSr = Invoke<CreatePageResponse>(page);

            if (createPageSr.ObjectResponse.Id > 0)
            {
                var getPageByIDRequest = new GetPageByIDRequest
                                             {
                                                 Id = createPageSr.ObjectResponse.Id
                                             };
                var getPageByIdResponse = Invoke<GetPageByIDResponse>(getPageByIDRequest);
                return getPageByIdResponse.ObjectResponse.Page;
            }

            return null;
        }

        /// <summary>
        /// The add module ids to page.
        /// </summary>
        /// <param name="pageRef">The page reference.</param>
        /// <param name="moduleIdCollection">The module id with position collection.</param>
        public void AddModuleIdsToPage(string pageRef, List<string> moduleIdCollection)
        {
            var request = new AddModulesToPageRequest
            {
                PageID = ExtractPageId(pageRef),
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
            throw new NotImplementedException("This method has not been implementd");
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
            var pId = ExtractPageId(pageId);
            var getPageByIDRequest = new GetPageByIDRequest
                                         {
                                             Id = pId,
                                         };
            var serviceResponse = Invoke<GetPageByIDResponse>(getPageByIDRequest);

            if (serviceResponse.ObjectResponse == null ||
                serviceResponse.ObjectResponse.Page == null)
            {
                return string.Empty;
            }

            // update the positions of the modules
            if (serviceResponse.ObjectResponse.Page.ModuleCollection != null &&
                serviceResponse.ObjectResponse.Page.ModuleCollection.Count > 0)
            {
                if (routine != null)
                {
                    routine(serviceResponse.ObjectResponse.Page, newModule);
                }

                var updateModulePositionsRequest = new UpdateModulePositionsRequest
                                                       {
                                                           PageID = pId,
                                                       };
                foreach (var module in serviceResponse.ObjectResponse.Page.ModuleCollection.Where(module => module.Position % numberOfColumns == 0))
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
                    Invoke<UpdateModulePositionsResponse>(updateModulePositionsRequest);
                }
            }

            // add the module to the proper position
            newModule.Position = 0;
            var addModulesToPageRequest = new AddModulesToPageRequest
                                              {
                                                  PageID = pId,
                                              };
            addModulesToPageRequest.ModuleCollection.Add(newModule);
            var response = Invoke<AddModulesToPageResponse>(addModulesToPageRequest);
            newModule.__id = response.ObjectResponse.AddModulesResult.ModulePositionCollection[0].Id;
            return newModule.Id.ToString();
        }

        /// <summary>
        /// Adds the module to end of page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="newModule">The new module.</param>
        /// <param name="routine">The routine.</param>
        public string AddModuleToEndOfPage(string pageId, Module newModule, ValidationRoutine routine)
        {
            var pId = ExtractPageId(pageId);
            var getPageByIDRequest = new GetPageByIDRequest
                                         {
                                             Id = pId,
                                         };
            var serviceResponse = Invoke<GetPageByIDResponse>(getPageByIDRequest);
            var i = 0;
            if (serviceResponse.ObjectResponse == null ||
                serviceResponse.ObjectResponse.Page == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.PageIsNull);
            }

            // update the positions of the modules
            if (serviceResponse.ObjectResponse.Page.ModuleCollection != null &&
                serviceResponse.ObjectResponse.Page.ModuleCollection.Count > 0)
            {
                if (routine != null)
                {
                    routine(serviceResponse.ObjectResponse.Page, newModule);
                }

                var updateModulePositionsRequest = new UpdateModulePositionsRequest
                                                       {
                                                           PageID = pId
                                                       };

                foreach (var module in serviceResponse.ObjectResponse.Page.ModuleCollection)
                {
                    updateModulePositionsRequest.ModulePositions.ModulePositionCollection.Add(
                        new ModulePosition
                            {
                                Id = module.Id, Position = i++
                            });
                }

                if (updateModulePositionsRequest.ModulePositions.ModulePositionCollection.Count > 0)
                {
                    Invoke<UpdateModulePositionsResponse>(updateModulePositionsRequest);
                }
            }

            // add the module to the proper position
            newModule.Position = i;
            var addModulesToPageRequest = new AddModulesToPageRequest
                                              {
                                                  PageID = pId,
                                              };
            addModulesToPageRequest.ModuleCollection.Add(newModule);
            var response = Invoke<AddModulesToPageResponse>(addModulesToPageRequest);
            return response.ObjectResponse.AddModulesResult.ModulePositionCollection[0].Id.ToString();
        }

        /// <summary>
        /// Updates the module positions on page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="zones">The modules.</param>
        public void UpdateModulePositionsOnPage(string pageId, IEnumerable<IEnumerable<int>> zones)
        {
            var pId = ExtractPageId(pageId);
            
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
                                Id = item, Position = column + (row * numberOfColums)
                            });
                    row++;
                }

                column++;
            }
            
            Invoke<UpdateModulePositionsResponse>(updateModulePositionsRequest);
        }

        /// <summary>
        /// Updates the state of the module.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="state">The state.</param>
        public void UpdateModuleState(string pageId, string moduleId, ModuleState state)
        {
            var pId = ExtractPageId(pageId);
            var getPageByIDRequest = new GetPageByIDRequest { Id = pId };
            var serviceResponse = Invoke<GetPageByIDResponse>(getPageByIDRequest);

            if (serviceResponse.ObjectResponse == null ||
                serviceResponse.ObjectResponse.Page == null ||
                serviceResponse.ObjectResponse.Page.ModuleCollection == null ||
                serviceResponse.ObjectResponse.Page.ModuleCollection.Count <= 0)
            {
                return;
            }

            var module = FindModule(serviceResponse.ObjectResponse.Page.ModuleCollection, moduleId);

            if (module == null)
            {
                return;
            }

            module.ModuleState = state;
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = pId };
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Invoke<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>A page object.</returns>
        public Page GetPage(string pageId)
        {
            var pId = ExtractPageId(pageId);
            var request = new GetPageByIDRequest { Id = pId };
            return Invoke<GetPageByIDResponse>(request).ObjectResponse.Page;
        }

        /// <summary>
        /// Deletes the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        public void DeletePage(string pageId)
        {
            var request = new DeletePageRequest
            {
                Id = ExtractPageId(pageId),
            };

            Invoke<DeletePageResponse>(request);
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
            return Invoke<GetPagesListResponse>(request).ObjectResponse.PageListInfoCollection;
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
            Invoke<DeleteItemResponse>(request);
        }

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="properties">The properties.</param>
        public void UpdateModule(string pageId, string moduleId, ICollection<Property> properties)
        {
            var pId = ExtractPageId(pageId);
            var module = GetModuleById(pageId, moduleId);
            if (module == null)
            {
                return;
            }

            UpdateModule(module, properties);
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = pId };
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Invoke<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        public void UpdateModulesOnPage(string pageId, Module module)
        {
            var pId = ExtractPageId(pageId);
            var updateModulesOnPageRequest = new UpdateModulesOnPageRequest { PageID = pId };
            updateModulesOnPageRequest.ModuleCollection.Add(module);
            Invoke<UpdateModulesOnPageResponse>(updateModulesOnPageRequest);
        }

        #region ..:: Static Methods ::.. 
        
        protected internal virtual int ExtractPageId(string pageId)
        {
            int retNum;
            if (Int32.TryParse(pageId, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum))
            {
                return retNum;
            }
            
            throw new DowJonesUtilitiesException(string.Empty, -1);
        }

        protected internal static int ExtractModuleId(string moduleId)
        {
            return Int32.Parse(moduleId);
        }

        /// <summary>
        /// Finds the module.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="moduleId">The module id.</param>
        /// <returns>A module object.</returns>
        private static Module FindModule(ICollection<Module> collection, string moduleId)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }

            return collection.FirstOrDefault(module => module.Id == ExtractModuleId(moduleId));
        }

        #endregion
    }
}
