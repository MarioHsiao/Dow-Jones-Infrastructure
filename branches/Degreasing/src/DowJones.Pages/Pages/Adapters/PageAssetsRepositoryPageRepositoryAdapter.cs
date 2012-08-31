using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules;
using Gateway = Factiva.Gateway.Messages.Assets.Pages.V1_0;
using GatewayCommon = Factiva.Gateway.Messages.Assets.Common.V2_0;

namespace DowJones.Pages
{
    public class PageAssetsRepositoryPageRepositoryAdapter : IPageRepository
    {
        protected readonly Gateway.PageType PageType;

        public IPageAssetsManager PageAssetsManager { get; private set; }

        public PageAssetsRepositoryPageRepositoryAdapter(IPageAssetsManager pageAssetsManager, Gateway.PageType pageType)
        {
            PageAssetsManager = pageAssetsManager;
            PageType = pageType;
        }

        public int AddModuleToPage(PageReference pageRef, Module module)
        {
            var gwModule = Mapper.Map<Gateway.Module>(module);
            return int.Parse(PageAssetsManager.AddModuleToPage(pageRef, 1, gwModule, null));
        }

        public void AddModuleToPage(PageReference pageRef, params int[] moduleIds)
        {
            PageAssetsManager.AddModuleIdsToEndOfPage(pageRef, moduleIds.Select(x => x.ToString()).ToArray());
        }

        public PageReference CreatePage(Page page)
        {
            return PageAssetsManager.CreatePage(Mapper.Map<Gateway.Page>(page));
        }

        public void RemoveModulesFromPage(PageReference pageRef, params int[] moduleIds)
        {
            PageAssetsManager.DeleteModules(pageRef, moduleIds.Select(x => x.ToString()).ToArray());
        }

        public void DeletePage(PageReference pageRef)
        {
            PageAssetsManager.DeletePage(pageRef);
        }

        public Module GetModule(PageReference pageRef, int moduleId)
        {
            var module = PageAssetsManager.GetModuleById(pageRef.Value, moduleId.ToString());
            return Mapper.Map<Module>(module);
        }

        public Page GetPage(PageReference pageRef)
        {
            var page = PageAssetsManager.GetPage(pageRef);
            return Mapper.Map<Page>(page);
        }

        public IEnumerable<Page> GetPages(Common.SortBy sort, Common.SortOrder order)
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

        public void UpdatePageLayout(PageReference pageRef, PageLayout layout)
        {
            if(!(layout is ZonePageLayout))
                throw new NotSupportedException("Only the Grouped Layout is supported");

            var groupedLayout = (ZonePageLayout)layout;

            if (groupedLayout == null)
                return;

            var zones = groupedLayout.Zones.Select(x => x);

            PageAssetsManager.UpdateModulePositionsOnPage(pageRef, zones);
        }
    }
}