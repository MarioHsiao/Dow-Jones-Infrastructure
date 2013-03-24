// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPageAssetsManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Managers.Abstract;
using DowJones.Pages.Caching;
using DowJones.Pages.Common;
using DowJones.Pages.Modules;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AssetType = Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType;
using GWModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;
using ModuleState = Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleState;
using SortBy = Factiva.Gateway.Messages.Assets.Pages.V1_0.SortBy;
using GWPage = Factiva.Gateway.Messages.Assets.Pages.V1_0.Page;
using QueryFilters = Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryFilters;
using SortOrder = Factiva.Gateway.Messages.Assets.Common.V2_0.SortOrder;

namespace DowJones.Pages
{
    public interface IPageAssetsManager : IAggregationManager
    {
        MetaData GetModuleMetaData(GWModule module);
        MetaData GetModuleMetaData(Modules.ModuleType moduleType);
        IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(Modules.ModuleType moduleType);
        IEnumerable<ModuleIdByMetadata> GetModulesByModuleType(Modules.ModuleType moduleType, MetaDataType metaDataType, List<MetaData> metaDataCollection = null);
        IEnumerable<PageListInfo> GetUserPageList(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, bool forceCacheRefresh = false);
        GetPagesListWithPageResponse GetUserPageListWithDefaultPage(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, int pageId, PageDefaultBy pageDefaultedBy = PageDefaultBy.Position, int pagePosition = 1);
        int ExtractPageId(string pageRef);
        int ExtractModuleId(string moduleId);
        PageListInfoExCollection GetSubscribablePages(PageType pageType, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier> accessQualifiers, MetadataFilter metadataFilter);
        void SetPageShareProperties(string pageRef, ShareProperties shareProperties);
        void SetPageShareProperties(string pageRef, ShareProperties shareProperties, bool updateModule, ShareProperties moduleShareProperties);
        void MakePageModulesPublic(IEnumerable<GWModule> moduleCollection);
        void MakePageModulesPrivate(IEnumerable<GWModule> moduleCollection);
        AccessControlScope GetRootAccessControlScope(int pageId, ShareProperties properties, Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier accessQualifier);
        PageRef ExtractPageRef(string pageRef, bool forceCacheRefresh = false);
        EnableDisable EnableDisablePage(string pageRef, EnableDisable action = EnableDisable.Enable);
        List<KeyValuePair<string, string>> GetAdminPageOwner(int pageId);
        GWModule GetModuleById(string moduleId);
        void UpdatePage(GWPage page);
        void AddModuleIdsToPage(string pageRef, IEnumerable<string> moduleIds);
        void AddModuleIdsToEndOfPage(string pageRef, IEnumerable<string> moduleIds);
        void AddModuleIdsToPage(string pageRef, IEnumerable<KeyValuePair<int, int>> moduleIdWithPositionCollection);
        void AddModulesToPage(int pageId, IEnumerable<Modules.Module> modules);
        string AddModuleToEndOfPage(string pageId, GWModule newModule, Action<GWPage, GWModule> routine);
        string AddModuleToPage(string pageId, int numberOfColumns, GWModule newModule, Action<GWPage, GWModule> routine);
        string CreatePage(GWPage newsPage);
        GWPage CreatePage(CreatePageRequest page);
        void DeleteAllPages(IEnumerable<PageType> pageTypes);
        void DeleteAllPages(IEnumerable<PageListInfo> pageListInfoCollection);
        void DeleteModules(string pageId, IEnumerable<string> moduleIds);
        void DeletePage(string pageId);
        void DeletePageFromSessionCache(string cacheKey, Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope cacheScope);
        GWModule GetModuleById(string pageId, string moduleId);
        GWModule GetModuleByIdWithPage(string pageId, string moduleId, out GWPage page);
        IEnumerable<MetaData> GetModulesNameAndDescriptions(IEnumerable<string> codes);
        GWPage GetPage(string pageId, bool cachePage, bool forceCacheRefresh);
        string GetPageName(string pageId);
        PageListInfoCollection GetPageListInfoCollection(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy);
        void MakePersonalAlertsPublic(IEnumerable<int> alertIds);
        void PublishPage(string pageId, IEnumerable<int> personalAlertIds);
        void PublishPage(string pageId, IEnumerable<IShareAssets> assetsToShare);
        bool RemoveAssignedPage(string pageId);
        string ReplaceModuleOnPage(string pageId, string rootIDOfNewModule, GWModule moduleToReplaceWith, string moduleIdToRemove);
        string SubscribeToPage(string pageId, int pagePosition);
        void UpdateModule(string pageId, string moduleId, IEnumerable<Property> properties);
        void UpdateModule(ModuleEx module);
        void UpdateModulePositionsOnPage(string pageId, IEnumerable<IEnumerable<int>> zones);
        void UpdateModulesOnPage(string pageId, GWModule module, string rootModuleId = null);
        void UpdateModuleState(string pageId, string moduleId, ModuleState state);
        void UpdateModule(GWModule module, IEnumerable<Property> properties);
        void UpdatePagePositions(IEnumerable<PagePosition> pagePositions);
        void UpdateTitleDescription(Page page);
        void UnpublishPage(string pageId);
        void UnsubscribeToPage(string pageId);
        void SetRootModuleId(string moduleId, string rootModuleId);
        void UpdateQueryFilter(string id, AssetType assetType, QueryFilters queryFilters);
        void DeleteQueryFilter(string id, AssetType assetType);
    }

    public static class IPageAssetsManagerExtensions
    {
        public static void DeletePage(this IPageAssetsManager manager, int pageId)
        {
            manager.DeletePage(pageId.ToString());
        }

        public static GWPage GetPage(this IPageAssetsManager manager, string pageId, bool cachePage = true)
        {
            return manager.GetPage(pageId, cachePage, false);
        }

        public static GWPage GetPage(this IPageAssetsManager manager, int pageId, bool cachePage = true, bool forceCacheRefresh = false)
        {
            return manager.GetPage(pageId.ToString(), cachePage, forceCacheRefresh);
        }

        public static string SubscribeToPage(this IPageAssetsManager manager, string pageId, int pagePosition = 1)
        {
            return manager.SubscribeToPage(pageId, pagePosition);
        }

        public static void UnpublishPage(this IPageAssetsManager manager, int pageId)
        {
            manager.UnpublishPage(pageId.ToString());
        }
    }
}