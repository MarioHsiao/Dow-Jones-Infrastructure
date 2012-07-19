using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules;
using Gateway = Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Pages
{
    public class PageAssetManagerPageSubscriptionManagerAdapter : IPageSubscriptionManager
    {
        public IPageAssetsManager PageAssetsManager { get; private set; }

        public PageAssetManagerPageSubscriptionManagerAdapter(IPageAssetsManager pageAssetsManager)
        {
            PageAssetsManager = pageAssetsManager;
        }

        public void PrivatizeModules(IEnumerable<Module> modules)
        {
            var gwModules = modules.Select(Mapper.Map<Gateway.Module>);
            PageAssetsManager.MakePageModulesPrivate(gwModules);
        }

        public void PublicizeModules(IEnumerable<Module> modules)
        {
            var gwModules = modules.Select(Mapper.Map<Gateway.Module>);
            PageAssetsManager.MakePageModulesPublic(gwModules);
        }

        public void EnablePage(string pageRef, bool enabled = true)
        {
            Gateway.EnableDisable action = enabled ? Gateway.EnableDisable.Enable : Gateway.EnableDisable.Disable;
            PageAssetsManager.EnableDisablePage(pageRef, action);
        }

        public void PublishPage(string pageRef, params int[] personalAlertIds)
        {
            PageAssetsManager.PublishPage(pageRef, personalAlertIds);
        }

        public string SubscribeToPage(string pageId)
        {
            return PageAssetsManager.SubscribeToPage(pageId);
        }

        public string SubscribeToPage(string pageId, int position)
        {
            return PageAssetsManager.SubscribeToPage(pageId, position);
        }

        public void UnpublishPage(string pageRef)
        {
            PageAssetsManager.UnpublishPage(pageRef);
        }

        public void UnsubscribeToPage(string pageId)
        {
            PageAssetsManager.UnsubscribeToPage(pageId);
        }
    }
}