using System.Collections.Generic;
using DowJones.Pages.Modules;

namespace DowJones.Pages
{
    public interface IPageManager
    {
        string AddModuleToPage(PageReference pageRef, Module module);
        void AddModuleToPage(PageReference pageRef, params string[] moduleIds);

        PageReference CreatePage(Page page);

        void DeleteModules(PageReference pageRef, params string[] moduleIds);

        void DeletePage(PageReference pageRef);

        MetaData GetModuleMetaData(Module module);

        Module GetModuleById(string moduleId);
        Module GetModuleById(PageReference pageRef, string moduleId);

        Page GetPage(PageReference pageRef);
        Page GetPage(PageReference pageRef, bool cachePage, bool forceCacheRefresh);

        IEnumerable<Page> GetPages();
        IEnumerable<Page> GetPages(SortBy sort, SortOrder order);

        void UpdateModule(Module module);

        void UpdatePage(Page page);
    }
}