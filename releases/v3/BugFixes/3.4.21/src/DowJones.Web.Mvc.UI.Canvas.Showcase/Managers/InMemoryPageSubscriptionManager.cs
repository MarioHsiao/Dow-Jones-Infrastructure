using System.Collections.Generic;
using DowJones.Pages;
using DowJones.Pages.Modules;

namespace DowJones.DegreasedDashboards
{
    public class InMemoryPageSubscriptionManager : IPageSubscriptionManager
    {
        public void PrivatizeModules(IEnumerable<Module> modules)
        {
        }

        public void PublicizeModules(IEnumerable<Module> modules)
        {
        }

        public void EnablePage(PageReference pageRef, bool enabled = true)
        {
        }

        public void PublishPage(PageReference pageRef, params int[] personalAlertIds)
        {
        }

        public string SubscribeToPage(PageReference pageRef)
        {
            return pageRef;
        }

        public string SubscribeToPage(PageReference pageRef, int position)
        {
            return pageRef;
        }

        public void UnpublishPage(PageReference pageRef)
        {
        }

        public void UnsubscribeToPage(PageReference pageRef)
        {
        }
    }
}