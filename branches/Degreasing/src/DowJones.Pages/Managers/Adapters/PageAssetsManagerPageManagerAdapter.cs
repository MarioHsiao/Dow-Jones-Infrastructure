using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules;
using Gateway = Factiva.Gateway.Messages.Assets.Pages.V1_0;
using GatewayCommon = Factiva.Gateway.Messages.Assets.Common.V2_0;

namespace DowJones.Pages
{
    public class PageAssetsManagerPageManagerAdapter : IPageManager
    {
        protected readonly Gateway.PageType PageType;

        public IPageAssetsManager PageAssetsManager { get; private set; }

        public PageAssetsManagerPageManagerAdapter(IPageAssetsManager pageAssetsManager, Gateway.PageType pageType)
        {
            PageAssetsManager = pageAssetsManager;
            PageType = pageType;
        }

        public string AddModuleToPage(PageReference pageRef, Module module)
        {
            var gwModule = Mapper.Map<Gateway.Module>(module);
            return PageAssetsManager.AddModuleToPage(pageRef, 1, gwModule, null);
        }

        public void AddModuleToPage(PageReference pageRef, params string[] moduleIds)
        {
            PageAssetsManager.AddModuleIdsToEndOfPage(pageRef, moduleIds);
        }

        public PageReference CreatePage(Page page)
        {
            return PageAssetsManager.CreatePage(Mapper.Map<Gateway.Page>(page));
        }

        public void DeleteModules(PageReference pageRef, params string[] moduleIds)
        {
            PageAssetsManager.DeleteModules(pageRef, moduleIds);
        }

        public void DeletePage(PageReference pageRef)
        {
            PageAssetsManager.DeletePage(pageRef);
        }

        public MetaData GetModuleMetaData(Module module)
        {
            return PageAssetsManager.GetModuleMetaData(Mapper.Map<Gateway.Module>(module));
        }

        public Module GetModuleById(string moduleId)
        {
            var module = PageAssetsManager.GetModuleById(moduleId);
            return Mapper.Map<Module>(module);
        }

        public Module GetModuleById(PageReference pageRef, string moduleId)
        {
            var module = PageAssetsManager.GetModuleById(pageRef, moduleId);
            return Mapper.Map<Module>(module);
        }

        public Page GetPage(PageReference pageRef)
        {
            var page = PageAssetsManager.GetPage(pageRef);
            return Mapper.Map<Page>(page);
        }

        public Page GetPage(PageReference pageRef, bool cachePage, bool forceCacheRefresh)
        {
            var page = PageAssetsManager.GetPage(pageRef, cachePage, forceCacheRefresh);
            return Mapper.Map<Page>(page);
        }

        public IEnumerable<Page> GetPages()
        {
            return GetPages(SortBy.Position, SortOrder.Ascending);
        }

        public IEnumerable<Page> GetPages(SortBy sort, SortOrder order)
        {
            var gwOrder = Mapper.Map<GatewayCommon.SortOrder>(order);
            var gwSortBy = Mapper.Map<Gateway.SortBy>(sort);

            var pages = PageAssetsManager.GetPageListInfoCollection(new[] { PageType }, gwOrder, gwSortBy);
            return pages.Select(Mapper.Map<Page>);
        }

        public void UpdateModule(Module module)
        {
            var gwModule = Mapper.Map<Gateway.ModuleEx>(module);
            PageAssetsManager.UpdateModule(gwModule);
        }

        public void UpdatePage(Page page)
        {
            var gwPage = Mapper.Map<Gateway.Page>(page);
            PageAssetsManager.UpdatePage(gwPage);
        }

        public void UpdateModulePositions(PageReference pageRef, IEnumerable<IEnumerable<int>> list)
        {
            PageAssetsManager.UpdateModulePositionsOnPage(pageRef, list);
        }
    }
}