using System.Collections.Generic;
using DowJones.Pages.Modules;

namespace DowJones.Pages
{
    public interface IPageRepository
    {
        int AddModuleToPage(PageReference pageRef, Module module);
        void AddModuleToPage(PageReference pageRef, params int[] moduleIds);

        PageReference CreatePage(Page page);

        void DeletePage(PageReference pageRef);

        Module GetModule(PageReference pageRef, int moduleId);

        Page GetPage(PageReference pageRef);

        IEnumerable<Page> GetPages(Common.SortBy sort, Common.SortOrder order);

        void RemoveModulesFromPage(PageReference pageRef, params int[] moduleIds);

        void UpdateModule(Module module);

        void UpdatePage(Page page);

        void UpdatePageLayout(PageReference pageRef, PageLayout layout);
    }
}