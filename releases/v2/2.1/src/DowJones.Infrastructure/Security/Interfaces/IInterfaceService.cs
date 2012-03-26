namespace DowJones.Security.Interfaces
{
    public interface IInterfaceService
    {
        bool IsAjaxEnabled { get; }
        bool IsPodcastability { get; }
        int MaximumExtendedContentSearchInMonth { get; }
        bool ShowDeduplicationControlsForSelectFolders { get; }
        bool IsUiInfo { get; }
        bool Is508User { get; }
        bool IsFceNavigation { get; }
        bool IsSalesWorksNavigation { get; }
        bool IsSmbNavigation { get; }
        bool IsSalesWorkLiteNavigation { get; }
        bool IsDJCESalesUser { get; }
        bool IsEnhancedDJCEUser { get; }
        bool IsWMSegmentationBrandOverride { get; }
        bool IsWMSegmentationPrdCO { get; }
        bool IsWMSegmentationPrdFAE { get; }
        bool IsWMSegmentationPrdFA { get; }
        bool IsFcpLinkInDotCom { get; }
        bool IsAcademicUser { get; }
        bool IsDowJonesInsider { get; }
        bool IsMyDowJonesFactiva { get; }
        bool IsAllowMobileCookieEmailLinkDA { get; }
        bool IsDULinkEnabled { get; }
        bool IsTranslateArticleAllowed { get; }
        bool IsPageMonitoring { get; }
        bool IsSubDomainingStaticResources { get; }
        bool IsBynoteMonitoring { get; }
        bool IsInterfaceAC5On { get; }
        bool IsPermittedReaderView { get; }
        string Personalization { get; }
        string SharingDA { get; }
        bool IsSharing { get; }
        bool IsProjectVisibleAdsOn { get; }
        bool IsResearchCenterOn { get; }
        bool IsCvdLinkOn { get; }
        bool IsSearchAssistanceOn { get; }
        bool IsSs_promotion_popupOn { get; }
        bool IsBlogDAOn { get; }
        bool IsRecentCompanySearches { get; }
        bool IsSelectDedupeOn { get; }
        string SalesworksPartner { get; }
        bool IsTestExecMarkupInArticleUser { get; }
    }
}