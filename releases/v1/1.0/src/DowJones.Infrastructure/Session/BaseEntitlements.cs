using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Tools.Session
{
    public class BaseEntitlements
    {
        public string RuleSet;
        public string UserRegistrationIndustryCode = string.Empty;

        public bool IsAccountAdministrator;
        public bool IsDotComDisplayServiceOn;
        public bool IsNewsViewsAdministratorOn;
        public bool IsPersonalizationOn;
        public bool IsTrackCoreServiceOn;

        // mt 2008 Q2 Company occurence coding
        public bool IsFcpDisplayServiceOn;
        public bool IsFcpCompanyTabDisplayServiceOn;
        public bool IsFcpIndustryTabDisplayServiceOn;
        public bool IsDotComCompanyQuickSearchDisplayServiceOn;
        public bool IsDotComWorkspaceDisplayServiceOn;
        public bool IsDotComNewsletterDisplayServiceOn;
        public bool IsDotComQuoteDisplayServiceOn;

        //removing blocking 6/2/08 part of Q3 2008
        //public bool IsMultimediaContentOn;
        public bool IsSharingOn;
        public bool IsUserAllowedToSavePersistentCookie; //sm_3/19/07
        public bool IsUserAllowedToChangePassword;//sm_3/19/07
        public bool IsUserAllowedToViewWSJCOMLink; // sm_05/14/07 for wsj link.

        // public EmailValidationLevel UserEmailValidationLevel = EmailValidationLevel.NoEmailAddressHasBeenStoredInPrefereneces;
        public UserType CurrentUserType = UserType.Unspecified;
        public EmailLoginConversionAllowed UserEmailLoginConversionState = EmailLoginConversionAllowed.NotAllowed; //sm_3/19/07
        public EmailLoginState UserEmailLoginState = EmailLoginState.Unspecified; //sm_3/19/07

        public bool IsDotComTrackDisplayServiceOn;
        //public bool isAdminBlockingAlertEmailDelivery; //for Q4 2007

        // mt 2008 Q2 - DJI Access
        public bool IsDowJonesInsider = true;
        //public bool IsNttXtractionPOCOn;
        public bool IsProjectVisibleAdsOn = true;

        // mt 2008 Q4 - RIC Replacement
        public bool IsGrandFatheredUser;

        // dd 2008 Q4 - Widgets TTL/Proxy and ExternalReader
        public bool IsAllowedToSetExternalReaderProfiles;
        public bool IsAllowedToSetTTLProxyCredentials;
        public bool IsAllowedToRetriveSharePointWebPart;

        /*03202009 - to user interface/ac6 to decide the branding override for dotocm and newsletter builder */
        public bool IsSegmentationBrandOverride;
        public string SegmentationPrdType = "";//making this is a string as I am not sure what all are possible.ADVISOR-for WM..

        public string InterfaceAc8;

        public bool IsMyDJFactivaOn = true;

        public string IndexAC1 = string.Empty;
        public string IndexAC2 = string.Empty;
        public string ArchiveAC2 = string.Empty;

    }
}
