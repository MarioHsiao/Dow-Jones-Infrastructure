﻿using System.Collections.Generic;
using System.Linq;
using DowJones.Pages;
using DowJones.Pages.Modules;

namespace DowJones.DegreasedDashboards
{
    public class InMemoryPageRepository : IPageRepository
    {
        private readonly IList<Page> _pages;

        protected IEnumerable<Module> Modules
        {
            get { return _pages.Where(x => x != null).SelectMany(x => x.ModuleCollection ?? Enumerable.Empty<Module>()).Where(x => x != null); }
        }


        public InMemoryPageRepository(IEnumerable<Page> pages = null)
        {
            _pages = new List<Page>(pages ?? Enumerable.Empty<Page>());
        }


        public int AddModuleToPage(PageReference pageRef, Module module)
        {
            var page = GetPage(pageRef);

            AddModulesToPage(page, module);

            return module.Id;
        }

        public void AddModuleToPage(PageReference pageRef, params int[] moduleIds)
        {
            var page = GetPage(pageRef);
            AddModulesToPage(page, moduleIds.Select(x => new Module() { Id = x }).ToArray());
        }

        protected void AddModulesToPage(Page page, params Module[] modules)
        {
            foreach (var module in modules.Reverse())
            {
                if (module.Id == default(int))
                {
                    if(Modules.Any())
                        module.Id = Modules.Max(x => x.Id) + 1;
                    else
                        module.Id = 1;
                }

                page.ModuleCollection.Insert(0, module);
            }
        }

        public PageReference CreatePage(Page page)
        {
            _pages.Add(page);

            if (string.IsNullOrEmpty(page.ID))
                page.ID = (_pages.Max(x => ((PageReference) x.ID).PageId) + 1).ToString();
            
            return page.ID;
        }

        public void RemoveModulesFromPage(PageReference pageRef, params int[] moduleIds)
        {
            var page = GetPage(pageRef);
            page.ModuleCollection.RemoveAll(x => moduleIds.Contains(x.Id));
        }

        public void DeletePage(PageReference pageRef)
        {
            var page = GetPage(pageRef);
            _pages.Remove(page);
        }

        public Module GetModule(PageReference pageRef, int moduleId)
        {
            var page = GetPage(pageRef);
            return page.ModuleCollection.FirstOrDefault(x => x.Id == moduleId);
        }

        public Page GetPage(PageReference pageRef)
        {
            return _pages.FirstOrDefault(x => x.ID == pageRef.Value);
        }

        public IEnumerable<Page> GetPages(DowJones.Pages.Common.SortBy sort, DowJones.Pages.Common.SortOrder order)
        {
            return _pages;
        }

        public void UpdateModule(Module module)
        {
            foreach (var page in _pages)
            {
                var existing = page.ModuleCollection.FirstOrDefault(x => x.Id == module.Id);
                if (existing == null)
                    continue;

                var index = page.ModuleCollection.IndexOf(existing);
                page.ModuleCollection.Remove(existing);
                page.ModuleCollection.Insert(index, module);
            }
        }

        public void UpdatePage(Page page)
        {
            var existing = _pages.FirstOrDefault(x => x.ID == page.ID);

            if (existing == null)
                return;

            var index = _pages.IndexOf(existing);
            _pages.Remove(existing);
            _pages.Insert(index, page);
        }

        public void UpdatePageLayout(PageReference pageRef, PageLayout layout)
        {
            var page = GetPage(pageRef);
            page.Layout = layout;
        }
    }
}