using System.Collections.Generic;
using System.Linq;
using DowJones.Pages;
using DowJones.Pages.Modules;
using Raven.Client;

namespace DowJones.DegreasedDashboards
{
    public class RavenDbPageManager : IPageManager
    {
        private readonly IDocumentSession _session;

        protected IQueryable<Page> Pages
        {
            get { return _session.Query<Page>(); }
        }

        public RavenDbPageManager(IDocumentSession session)
        {
            _session = session;
        }

        protected IEnumerable<Module> Modules
        {
            get { return Pages.SelectMany(x => x.ModuleCollection ?? Enumerable.Empty<Module>()).Where(x => x != null); }
        }

        public void SaveChanges()
        {
            _session.SaveChanges();
        }

        public string AddModuleToPage(PageReference pageRef, Module module)
        {
            var page = GetPage(pageRef);

            page.ModuleCollection.Add(module);

            if (module.Id == default(int))
                module.Id = Modules.Max(x => x.Id) + 1;

            return module.Id.ToString();
        }

        public void AddModuleToPage(PageReference pageRef, params int[] moduleIds)
        {
            var page = GetPage(pageRef);

            foreach (var moduleId in moduleIds)
            {
                page.ModuleCollection.Add(new Module { Id = moduleId });
            }
        }

        public PageReference CreatePage(Page page)
        {
            _session.Store(page);

            if (string.IsNullOrEmpty(page.ID))
                page.ID = (Pages.Max(x => ((PageReference)x.ID).PageId) + 1).ToString();

            return page.ID;
        }

        public void RemoveModules(PageReference pageRef, params int[] moduleIds)
        {
            var page = GetPage(pageRef);
            page.ModuleCollection.RemoveAll(x => moduleIds.Contains(x.Id));
        }

        public void DeletePage(PageReference pageRef)
        {
            var page = GetPage(pageRef);
            
            if (page != null)
                _session.Delete(page);
        }

        public MetaData GetModuleMetaData(Module module)
        {
            return new MetaData();
        }

        public Module GetModuleById(int moduleId)
        {
            return Modules.FirstOrDefault(x => x.Id == moduleId);
        }

        public Module GetModuleById(PageReference pageRef, int moduleId)
        {
            var page = GetPage(pageRef);
            return page.ModuleCollection.FirstOrDefault(x => x.Id == moduleId);
        }

        public Page GetPage(PageReference pageRef)
        {
            return _session.Load<Page>(pageRef.PageId);
        }

        public Page GetPage(PageReference pageRef, bool cachePage, bool forceCacheRefresh)
        {
            return GetPage(pageRef);
        }

        public IEnumerable<Page> GetPages()
        {
            return Pages;
        }

        public IEnumerable<Page> GetPages(SortBy sort, SortOrder order)
        {
            return GetPages();
        }

        public void UpdateModule(Module module)
        {
            foreach (var page in Pages)
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
            _session.Store(page, page.ID);
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
