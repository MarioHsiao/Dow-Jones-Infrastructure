using DowJones.Pages.Modules;

namespace DowJones.Pages
{
    public interface IPageManager
    {
        string AddModuleToPage(string pageRef, Module module);
        void AddModuleToPage(string pageRef, params string[] moduleIds);

        string CreatePage(Page page);
        
        void DeleteModules(string pageRef, params string[] moduleIds);
        
        void DeletePage(string pageRef);

        MetaData GetModuleMetaData(Module module);

        Module GetModuleById(string moduleId);
        Module GetModuleById(string pageRef, string moduleId);

        Page GetPage(string pageRef);
        Page GetPage(string pageRef, bool cachePage, bool forceCacheRefresh);

        void UpdateModule(Module module);

        void UpdatePage(Page page);
    }
}