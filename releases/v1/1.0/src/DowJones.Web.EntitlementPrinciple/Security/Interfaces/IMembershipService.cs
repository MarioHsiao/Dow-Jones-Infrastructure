using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IMembershipService
    {
        bool CanMakeFolderSubscribable { get; }
        string MembershipAC2 { get; }
        string MembershipAC3 { get; }
        string MembershipAC4 { get; }
        bool IsPersonalization { get; }
        string SharingDA { get; }
        string NewsletterDA { get; }
        bool IsTimeToLiveToken { get; }
        bool IsNewsViewsRenderOn { get; }
        bool IsNewsViewsAdministratorOn { get; }
    }
}