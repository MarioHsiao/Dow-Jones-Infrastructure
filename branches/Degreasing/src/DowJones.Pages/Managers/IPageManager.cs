using System.Collections.Generic;
using DowJones.Pages.Modules;

namespace DowJones.Pages
{
    public interface IPageManager
    {
        string AddModuleToPage(PageReference pageRef, Module module);
        void AddModuleToPage(PageReference pageRef, params int[] moduleIds);

        PageReference CreatePage(Page page);

        void RemoveModules(PageReference pageRef, params int[] moduleIds);

        void DeletePage(PageReference pageRef);

        MetaData GetModuleMetaData(Module module);

        Module GetModuleById(int moduleId);
        Module GetModuleById(PageReference pageRef, int moduleId);

        Page GetPage(PageReference pageRef);
        Page GetPage(PageReference pageRef, bool cachePage, bool forceCacheRefresh);

        IEnumerable<Page> GetPages();
        IEnumerable<Page> GetPages(SortBy sort, SortOrder order);

        void UpdateModule(Module module);
        void UpdateModulePositions(PageReference pageRef, IEnumerable<IEnumerable<int>> list);

        void UpdatePage(Page page);

    }
}