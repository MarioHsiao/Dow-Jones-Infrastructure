namespace DowJones.Security.Interfaces
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
        
        int MaxSourceList { get; }
    }


//    public interface IMembershipAssetLimit
//    {
//        int MaxSourceList { get; }
//    }

}