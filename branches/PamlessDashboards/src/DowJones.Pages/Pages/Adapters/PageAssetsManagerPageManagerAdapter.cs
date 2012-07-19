using DowJones.Pages.Modules;
using Gateway = Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Pages
{
    public class PageAssetsManagerPageManagerAdapter : IPageManager
    {
        public IPageAssetsManager PageAssetsManager { get; private set; }

        public PageAssetsManagerPageManagerAdapter(IPageAssetsManager pageAssetsManager)
        {
            PageAssetsManager = pageAssetsManager;
        }

        public string AddModuleToPage(string pageId, Module module)
        {
            var gwModule = Mapper.Map<Gateway.Module>(module);
            return PageAssetsManager.AddModuleToPage(pageId, 1, gwModule, null);
        }

        public void AddModuleToPage(string pageId, params string[] moduleIds)
        {
            PageAssetsManager.AddModuleIdsToEndOfPage(pageId, moduleIds);
        }

        public string CreatePage(Page page)
        {
            return PageAssetsManager.CreatePage(Mapper.Map<Gateway.Page>(page));
        }

        public void DeleteModules(string pageId, params string[] moduleIds)
        {
            PageAssetsManager.DeleteModules(pageId, moduleIds);
        }

        public void DeletePage(string pageId)
        {
            PageAssetsManager.DeletePage(pageId);
        }

        public MetaData GetModuleMetaData(Module module)
        {
            return PageAssetsManager.GetModuleMetaData(Mapper.Map<Gateway.Module>(module));
        }

        public Module GetModuleById(string moduleId)
        {
            var module = PageAssetsManager.GetModuleById(moduleId);
            return Mapper.Map<Module>(module);
        }

        public Module GetModuleById(string pageId, string moduleId)
        {
            var module = PageAssetsManager.GetModuleById(pageId, moduleId);
            return Mapper.Map<Module>(module);
        }

        public Page GetPage(string pageId)
        {
            var page = PageAssetsManager.GetPage(pageId);
            return Mapper.Map<Page>(page);
        }

        public Page GetPage(string pageId, bool cachePage, bool forceCacheRefresh)
        {
            var page = PageAssetsManager.GetPage(pageId, cachePage, forceCacheRefresh);
            return Mapper.Map<Page>(page);
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
    }
}