using System.Collections.Generic;
using DowJones.Pages.Modules;

namespace DowJones.Pages
{
    public interface IPageRepository
    {
        int AddModuleToPage(PageReference pageRef, Module module);
        void AddModuleToPage(PageReference pageRef, params int[] moduleIds);

        PageReference CreatePage(Page page);

        void RemoveModulesFromPage(PageReference pageRef, params int[] moduleIds);

        void DeletePage(PageReference pageRef);

        Module GetModule(PageReference pageRef, int moduleId);

        Page GetPage(PageReference pageRef);

        IEnumerable<Page> GetPages(Common.SortBy sort, Common.SortOrder order);

        void UpdateModule(Module module);
        void UpdateModulePositions(PageReference pageRef, IEnumerable<IEnumerable<int>> list);

        void UpdatePage(Page page);

    }
}