using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Pages;
using DowJones.Pages.Modules;

namespace DowJones.DegreasedDashboards
{
    public class InMemoryPageManager : IPageManager
    {
        private readonly IList<Page> _pages;

        protected IEnumerable<Module> Modules
        {
            get { return _pages.SelectMany(x => x.ModuleCollection); }
        }


        public InMemoryPageManager(IEnumerable<Page> pages = null)
        {
            _pages = new List<Page>(pages ?? Enumerable.Empty<Page>());
        }


        public string AddModuleToPage(PageReference pageRef, Module module)
        {
            var page = GetPage(pageRef);

            page.ModuleCollection.Add(module);
            
            if (module.Id == default(int))
                module.Id = Modules.Max(x => x.Id) + 1;

            return module.Id.ToString();
        }

        public void AddModuleToPage(PageReference pageRef, params string[] moduleIds)
        {
            var page = GetPage(pageRef);

            foreach (var moduleId in moduleIds)
            {
                page.ModuleCollection.Add(new Module { Id = Convert.ToInt32(moduleId) });
            }
        }

        public PageReference CreatePage(Page page)
        {
            _pages.Add(page);

            if (string.IsNullOrEmpty(page.ID))
                page.ID = (_pages.Max(x => ((PageReference) x.ID).PageId) + 1).ToString();
            
            return page.ID;
        }

        public void DeleteModules(PageReference pageRef, params string[] moduleIds)
        {
            var intModuleIds = moduleIds.Select(int.Parse);
            
            var page = GetPage(pageRef);
            page.ModuleCollection.RemoveAll(x => intModuleIds.Contains(x.Id));
        }

        public void DeletePage(PageReference pageRef)
        {
            var page = GetPage(pageRef);
            _pages.Remove(page);
        }

        public MetaData GetModuleMetaData(Module module)
        {
            return new MetaData();
        }

        public Module GetModuleById(string moduleId)
        {
            return Modules.FirstOrDefault(x => x.Id.ToString() == moduleId);
        }

        public Module GetModuleById(PageReference pageRef, string moduleId)
        {
            var page = GetPage(pageRef);
            return page.ModuleCollection.FirstOrDefault(x => x.Id.ToString() == moduleId);
        }

        public Page GetPage(PageReference pageRef)
        {
            return _pages.FirstOrDefault(x => x.ID == pageRef.Value);
        }

        public Page GetPage(PageReference pageRef, bool cachePage, bool forceCacheRefresh)
        {
            return GetPage(pageRef);
        }

        public IEnumerable<Page> GetPages()
        {
            return _pages;
        }

        public IEnumerable<Page> GetPages(SortBy sort, SortOrder order)
        {
            return GetPages();
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


        public void UpdateModulePositions(PageReference pageRef, IEnumerable<IEnumerable<int>> list)
        {
            var page = GetPage(pageRef);

            var reorderedModules =
                (
                    from moduleId in list.SelectMany(x => x)
                    join module in page.ModuleCollection on moduleId equals module.Id
                    select module
                ).ToArray();

            for (int i = 0; i < reorderedModules.Length; i++)
                reorderedModules[i].Position = i;
        }
    }
}
