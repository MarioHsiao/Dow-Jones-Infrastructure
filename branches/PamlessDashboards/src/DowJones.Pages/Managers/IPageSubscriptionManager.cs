using System.Collections.Generic;
using DowJones.Pages.Modules;

namespace DowJones.Pages
{
    public interface IPageSubscriptionManager
    {
        void PrivatizeModules(IEnumerable<Module> modules);

        void PublicizeModules(IEnumerable<Module> modules);

        void EnablePage(PageReference pageRef, bool enabled = true);

        void PublishPage(PageReference pageRef, params int[] personalAlertIds);

        string SubscribeToPage(PageReference pageRef);
        string SubscribeToPage(PageReference pageRef, int position);

        void UnpublishPage(PageReference pageRef);

        void UnsubscribeToPage(PageReference pageRef);
    }
}