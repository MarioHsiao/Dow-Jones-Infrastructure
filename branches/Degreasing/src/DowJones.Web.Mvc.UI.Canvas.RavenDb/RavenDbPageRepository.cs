using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Infrastructure;
using DowJones.Pages;
using DowJones.Pages.Common;
using DowJones.Pages.Modules;
using Raven.Client;

namespace DowJones.Web.Mvc.UI.Canvas.RavenDb
{
    // TODO: Make this less chatty by changing all of the .SaveChanges() to unit of work pattern!
    /// <summary>
    /// RavenDB Page Repository implementation
    /// </summary>
    public class RavenDbPageRepository : IPageRepository
    {
        private static readonly Func<Page, DateTime> CreateDatePredicate = x => x.CreatedDate;
        private static readonly Func<Page, DateTime> LastModifiedPredicate = x => x.LastModifiedDate;
        private static readonly Func<Page, string> NamePredicate = x => x.Title;
        private static readonly Func<Page, int> PositionPredicate = x => x.Position;

        private readonly IDocumentSession _session;

        protected IQueryable<Page> Pages
        {
            get { return _session.Query<Page>(); }
        }

        protected IQueryable<Module> Modules
        {
            get
            {
                return 
                    from page in _session.Query<Page>()
                    from module in page.ModuleCollection
                    select module;
            }
        }


        public RavenDbPageRepository(IDocumentSession session)
        {
            _session = session;
        }


        public int AddModuleToPage(PageReference pageRef, Module module)
        {
            var page = GetPage(pageRef);

            if (page == null)
                return 0;

            page.ModuleCollection.Insert(0, module);

            // HACK:  Sub-document ID hack
            string id = _session.Advanced.DocumentStore.Conventions.GenerateDocumentKey(module);
            module.Id = Convert.ToInt32(id.Substring(id.IndexOf('/') + 1));

            page.Layout.AddModule(module.Id);

            _session.SaveChanges();

            return module.Id;
        }

        public void AddModuleToPage(PageReference pageRef, params int[] moduleIds)
        {
            var page = GetPage(pageRef);

            if (page == null)
                return;

            foreach (var module in Modules.Where(x => moduleIds.Reverse().Contains(x.Id)))
            {
                page.ModuleCollection.Insert(0, module);
            }

            _session.SaveChanges();
        }

        public PageReference CreatePage(Page page)
        {
            Guard.IsNotNull(page, "page");

            _session.Store(page, page.ID);
            _session.SaveChanges();
            return _session.Advanced.GetDocumentId(page);
        }

        public void RemoveModulesFromPage(PageReference pageRef, params int[] moduleIds)
        {
            var page = GetPage(pageRef);
            page.ModuleCollection.RemoveAll(x => moduleIds.Contains(x.Id));

            foreach (var moduleId in moduleIds)
            {
                page.Layout.RemoveModule(moduleId);
            }

            _session.SaveChanges();
        }

        public void DeletePage(PageReference pageRef)
        {
            var page = _session.Load<Page>(pageRef.Value);
            
            if (page == null)
                return;

            _session.Delete(page);

            _session.SaveChanges();
        }

        public Module GetModule(PageReference pageRef, int moduleId)
        {
            var page = GetPage(pageRef);

            var module = page.ModuleCollection.FirstOrDefault(x => x.Id == moduleId);

            return module;
        }

        public Page GetPage(PageReference pageRef)
        {
            var page = _session.Load<Page>(pageRef.Value);
            return page;
        }

        public IEnumerable<Page> GetPages(SortBy sort = SortBy.Position, SortOrder order = SortOrder.Ascending)
        {
            switch(sort)
            {
                case (SortBy.CreatedDate):
                {
                    if (order == SortOrder.Ascending)
                        return Pages.OrderBy(CreateDatePredicate);
                    else
                        return Pages.OrderByDescending(CreateDatePredicate);
                }

                case (SortBy.LastModifiedDate):
                {
                    if (order == SortOrder.Ascending)
                        return Pages.OrderBy(LastModifiedPredicate);
                    else
                        return Pages.OrderByDescending(LastModifiedPredicate);
                }

                case (SortBy.Name):
                {
                    if (order == SortOrder.Ascending)
                        return Pages.OrderBy(NamePredicate);
                    else
                        return Pages.OrderByDescending(NamePredicate);
                }

                case (SortBy.Position):
                default:
                {
                    if (order == SortOrder.Ascending)
                        return Pages.OrderBy(PositionPredicate);
                    else
                        return Pages.OrderByDescending(PositionPredicate);
                }
            }
        }

        public void UpdateModule(Module module)
        {
            _session.Store(module);
            _session.SaveChanges();
        }

        public void UpdatePage(Page page)
        {
            _session.Store(page);
            _session.SaveChanges();
        }

        public void UpdatePageLayout(PageReference pageRef, PageLayout layout)
        {
            var page = GetPage(pageRef);
            
            page.Layout = layout;

            _session.SaveChanges();
        }
    }
}
