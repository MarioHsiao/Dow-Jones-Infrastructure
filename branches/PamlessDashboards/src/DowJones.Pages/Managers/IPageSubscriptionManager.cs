using System.Collections.Generic;
using DowJones.Pages.Modules;

namespace DowJones.Pages
{
    public interface IPageSubscriptionManager
    {
        void PrivatizeModules(IEnumerable<Module> modules);

        void PublicizeModules(IEnumerable<Module> modules);

        void EnablePage(string pageRef, bool enabled = true);

        void PublishPage(string pageRef, params int[] personalAlertIds);

        string SubscribeToPage(string pageId);
        string SubscribeToPage(string pageId, int position);

        void UnpublishPage(string pageRef);

        void UnsubscribeToPage(string pageId);
    }
}