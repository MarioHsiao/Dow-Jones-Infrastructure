using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Pages;
using DowJones.Pages.Caching;
using DowJones.Pages.Modules;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AccessQualifier = DowJones.Pages.AccessQualifier;
using CacheScope = Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope;
using Module = Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;
using ModuleState = Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleState;
using ModuleType = DowJones.Pages.Modules.ModuleType;
using Page = Factiva.Gateway.Messages.Assets.Pages.V1_0.Page;
using PagePosition = DowJones.Pages.PagePosition;
using SortBy = Factiva.Gateway.Messages.Assets.Pages.V1_0.SortBy;
using DowJones.Preferences;

namespace DowJones.Web.Showcase.Mocks
{
    public class MockPageAssetsManagerFactory : PageAssetsManagerFactory
    {
        private static MockPageAssetsManager _manager;

        public override IPageAssetsManager CreateManager(IControlData controlData, IPreferences preferences)
        {
            return _manager = _manager ?? new MockPageAssetsManager();
        }
    }

    public class MockPageAssetsManager : IPageAssetsManager
    {
        static IEnumerable<Module> _modules = new Module[] {
                new SocialMediaNewspageModule {
                    Position = 11,
                    __id = 2102012,
                    Title = "Social Media",
                    Description = "Social Media Mock Module",
                    ChannelMapCollection = new ChannelMapCollection{ new ChannelMap() { Code = "iacc", CodeScheme = CodeScheme.Industry}}
                }
        };

        public Module GetModuleById(string pageId, string moduleId)
        {
            return _modules.OrderBy(x => x.Position).Single(x => x.Id == int.Parse(moduleId));
        }

        public Module GetModuleByIdWithPage(string pageId, string moduleId, out Page page)
        {
            throw new NotImplementedException();
        }

        public Page GetPage(string pageId, bool cachePage, bool forceCacheRefresh)
        {
            if (pageId == (-1).ToString())
                return new MockPage(1, Enumerable.Empty<Module>());

            return new MockPage(int.Parse(pageId), _modules.OrderBy(x => x.Position));
        }

        public void DeleteModules(string pageId, List<string> moduleIds)
        {
            _modules = _modules.Where(x => !moduleIds.Contains(x.Id.ToString())).ToArray();
        }

        public void DeleteModulesForMediaMonitor(string pageId, List<string> moduleIds)
        {
            DeleteModules(pageId, moduleIds);
        }

        public void AddModuleIdsToEndOfPage(string pageRef, IEnumerable<string> moduleIds)
        {
            throw new NotImplementedException();
        }

        public Page CreatePage(CreatePageRequest page)
        {
            throw new NotImplementedException();
        }

        public string AddModuleToPage(string pageId, int numberOfColumns, Module newModule, Action<Page, Module> routine)
        {
            AddModuleToEndOfPage(pageId, newModule, routine);
            return newModule.Id.ToString();
        }

        public string AddModuleToEndOfPage(string pageId, Module newModule, Action<Page, Module> routine)
        {
            newModule.Position = _modules.Max(x => x.Position) + 1;
            _modules = _modules.Union(new[] { newModule }).ToArray();
            return 3.ToString();
        }

        public AccessControlScope GetRootAccessControlScope(int pageId, ShareProperties properties, Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier accessQualifier)
        {
            throw new NotImplementedException();
        }

        public PageRef ExtractPageRef(string pageRef, bool forceCacheRefresh)
        {
            throw new NotImplementedException();
        }

        public EnableDisable EnableDisablePage(string pageRef, EnableDisable action)
        {
            throw new NotImplementedException();
        }

        public List<KeyValuePair<string, string>> GetAdminPageOwner(int pageId)
        {
            throw new NotImplementedException();
        }

        public Module GetModuleById(string moduleId)
        {
            throw new NotImplementedException();
        }

        public void UpdatePage(Page page)
        {
            throw new NotImplementedException();
        }

        public void AddModuleIdsToPage(string pageRef, IEnumerable<string> moduleIds)
        {
            throw new NotImplementedException();
        }

        public void UpdateModule(string pageId, string moduleId, IEnumerable<Property> properties)
        {
            throw new NotImplementedException();
        }

        public void UpdateModulePositionsOnPage(string pageId, IEnumerable<IEnumerable<int>> zones)
        {
            var orderedModules =
                zones.First().Select(moduleId => _modules.Single(x => x.Id == moduleId));

            int position = 0;

            foreach (var module in orderedModules)
                module.Position = position++;
        }

        public void UpdateModuleState(string pageId, string moduleId, ModuleState state)
        {
            _modules.Single(x => x.Id == int.Parse(moduleId)).ModuleState = state;
        }

        public Page GetPage(string pageId)
        {
            if (pageId == (-1).ToString())
                return new MockPage(1, Enumerable.Empty<Module>());

            return new MockPage(int.Parse(pageId), _modules.OrderBy(x => x.Position));
        }

        public void DeleteModules(string pageId, IEnumerable<string> moduleIds)
        {
            throw new NotImplementedException();
        }

        public void DeletePage(string pageId)
        {
            throw new NotImplementedException();
        }

        public PageListInfoCollection GetPageListInfoCollection(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllPages(IEnumerable<PageType> pageTypes)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllPages(IEnumerable<PageListInfo> pageListInfoCollection)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllPages(PageListInfoCollection pageListInfoCollection)
        {
            throw new NotImplementedException();
        }

        public void DeletePageFromSessionCache(string cacheKey, CacheScope cacheScope)
        {
            // Ok...  it's deleted!
        }

        public void UpdateModule(string pageId, string moduleId, ICollection<Property> properties)
        {
            var module = _modules.Single(x => x.Id.ToString() == moduleId);

            foreach (var property in properties)
            {
                module.SetPropertyValue(property.name, property.value);
            }
        }

        public void UpdateModulesOnPage(string pageId, Module module, string rootModuleId = null)
        {
            throw new NotImplementedException();
        }

        public void AddModuleIdsToPage(string pageRef, IEnumerable<KeyValuePair<int, int>> moduleIdWithPositionCollection)
        {
            throw new NotImplementedException();
        }

        public void AddModulesToPage(int pageId, IEnumerable<Pages.Modules.Module> modules)
        {
            throw new NotImplementedException();
        }

        public string CreatePage(Pages.Page newsPage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MetaData> GetModulesNameAndDescriptions(IEnumerable<string> codes)
        {
            throw new NotImplementedException();
        }

        public string GetModuleTitle(Module module)
        {
            throw new NotImplementedException();
        }

        public string GetModuleTitle(ModuleType moduleType)
        {
            throw new NotImplementedException();
        }

        public MetaData GetModuleMetaData(Module module)
        {
            throw new NotImplementedException();
        }

        public MetaData GetModuleMetaData(ModuleType moduleType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType, MetaDataType metaDataType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PageListInfo> GetUserPageList(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, bool forceCacheRefresh)
        {
            throw new NotImplementedException();
        }

        public GetPagesListWithPageResponse GetUserPageListWithDefaultPage(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, int pageId, PageDefaultBy pageDefaultedBy, int pagePosition)
        {
            throw new NotImplementedException();
        }

        public int ExtractPageId(string pageRef)
        {
            throw new NotImplementedException();
        }

        public int ExtractModuleId(string moduleId)
        {
            throw new NotImplementedException();
        }

        public void SetPageShareProperties(string pageRef, ShareProperties shareProperties)
        {
            throw new NotImplementedException();
        }

        public void SetPageShareProperties(string pageRef, ShareProperties shareProperties, bool updateModule, ShareProperties moduleShareProperties)
        {
            throw new NotImplementedException();
        }

        public void MakePageModulesPublic(IEnumerable<Module> moduleCollection)
        {
            throw new NotImplementedException();
        }

        public void MakePageModulesPrivate(IEnumerable<Module> moduleCollection)
        {
            throw new NotImplementedException();
        }

        public bool CachingEnabled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public PageListInfoExCollection GetSubscribablePages(PageType pageType, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier> accessQualifiers, MetadataFilter metadataFilter)
        {
            throw new NotImplementedException();
        }

        public string GetPageName(string pageId)
        {
            return pageId;
        }

        public void MakePersonalAlertsPublic(IEnumerable<int> alertIds)
        {
            throw new NotImplementedException();
        }

        public AccessQualifier MapAccessQualifier(Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier accessQualifier)
        {
            throw new NotImplementedException();
        }

        public void PublishPage(string pageId, IEnumerable<int> personalAlertIds)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAssignedPage(string pageId)
        {
            throw new NotImplementedException();
        }

        public string ReplaceModuleOnPage(string pageId, string moduleToAdd, string moduleToRemove)
        {
            throw new NotImplementedException();
        }

        public string SubscribeToPage(string pageId, int pagePosition)
        {
            throw new NotImplementedException();
        }

        public void UnpublishPage(string pageId)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeToPage(string pageId)
        {
            throw new NotImplementedException();
        }

        public void SetRootModuleId(string moduleId, string rootModuleId)
        {
            throw new NotImplementedException();
        }

        public void UpdateModule(Module module, IEnumerable<Property> properties)
        {
            throw new NotImplementedException();
        }

        public void UpdateModule(ModuleEx module)
        {
            throw new NotImplementedException();
        }

        public void UpdatePagePositions(IEnumerable<PagePosition> pagePositions)
        {
            throw new NotImplementedException();
        }

        public void UpdateTitleDescription(Pages.Page page)
        {
            throw new NotImplementedException();
        }

        public void AddModuleIdsToPage(string pageRef, List<string> moduleIdWithPositionCollection)
        {
            throw new NotImplementedException();
        }

        public void AddModuleIdsToEndOfPage(string pageRef, List<string> moduleIdWithPositionCollection)
        {
            throw new NotImplementedException();
        }

        public void CreateModule(ModuleEx module)
        {
            throw new NotImplementedException();
        }


        protected class MockPage : Page
        {
            public MockPage(int id, IEnumerable<Module> modules)
            {
                ModuleCollection = new ModuleCollection();
                ModuleCollection.AddRange(modules);
                __id = id;
            }
        }


        #region IPageAssetsManager Members


        public string CreatePage(Page newsPage)
        {
            throw new NotImplementedException();
        }

        #endregion

        public ushort LastContentServerAddress
        {
            get { return 0; }
        }

        public IControlData LastTransactionControlData
        {
            get { return null; }
        }

        public string LastRawResponse
        {
            get { return null; }
        }

        #region IPageAssetsManager Members


        public string ReplaceModuleOnPage(string pageId, string rootIDOfNewModule, Module moduleToReplaceWith, string moduleToRemove)
        {
            throw new NotImplementedException();
        }

        public void UpdateQueryFilter(string id, Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType assetType, Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryFilters queryFilters)
        {
            throw new NotImplementedException();
        }

        public void DeleteQueryFilter(string id, Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType assetType)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IPageAssetsManager Members


        public IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType, MetaDataType metaDataType, Metadata metaData = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPageAssetsManager Members


        public IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType, MetaDataType metaDataType, List<MetaData> metaDataCollection = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
