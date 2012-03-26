using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace EMG.Utility.Managers.Security
{
          /// <summary>
      /// Summary description for Roles.
      /// </summary>
      public interface IPrincipal
      {
            bool IsAjaxEnabled { get; }

            /// <summary>
            /// Determines whether the user is a group administrator
            /// </summary>
            /// <returns></returns>
            bool IsGroupAdministrator { get; }

            /// <summary>
            /// Determines whether the user is an account administrator
            /// </summary>
            /// <returns></returns>
            bool IsAccountAdministrator { get; }

            /// <summary>
            /// Determines whether the user is a track administrator
            /// </summary>
            bool IsTrackAdministrator { get; }

            /// <summary>
            /// Determines whether the user is a DMM user
            /// </summary>
            bool IsDMMUser { get; }

            /// <summary>
            /// Determines whether the user is a Select-FullArticle user
            /// </summary>
            bool IsSelectFullUser { get; }

            /// <summary>
            /// Determines whether the user is a Select-Headlines user
            /// </summary>
            bool IsSelectHeadlinesUser { get; }

            /// <summary>
            /// Determines whether the user is a Alerts user
            /// </summary>
            bool IsAlertsUser { get; }

            /// <summary>
            /// Gets the maximum number of items in the specified class
            /// </summary>
            /// <param name="classID"></param>
            /// <returns></returns>
            int GetMaxItems(PreferenceClassID classID);

            /// <summary>
            /// Gets the RuleSet
            /// </summary>
            RuleSet RuleSet { get; }

            bool IsMembershipAdmin { get; }

            bool IsUsageReport { get; }
            bool IsUiInfo { get; }
            bool IsNewsViews { get; }

            int MaxFolderForGlobal { get; }
            int MaxFolderForSelectHeadline { get; }
            int MaxFolderForSelectFullText { get; }
            int MaxFolderForFastTrack { get; }
            int MaxFolderForFcpAndIwe { get; }
            int MaxFolderForDmm { get; }

            bool CanMakeFolderSubscribable { get; }
            bool CanCreateFactivaRootGroupItem { get; }
            bool CanGetUsageReport { get; }
            bool IsNewsDigest { get; }

            bool IsHistoricalQuote { get; }
            bool IsTreadlineQuote { get; }
            bool IsReuterInvestorQuote { get; }
            bool IsCurrentQuote { get; }
            bool IsQuickQuote { get; }
            bool IsFullQuote { get; }
            bool IsFceNavigation { get; }
            bool IsSalesWorksNavigation { get; }
        bool IsSmbNavigation { get; }
        bool IsSalesWorkLiteNavigation { get; }
            bool IsCreditcardUser { get; }
            bool IsCorporateUser { get; }
            bool IsCustomerServiceUser { get; }
            bool IsIndividualUser { get; }
            bool CanSwitchUser { get; }
            bool IsRedirects { get; }
            bool IsManageRedirects { get; }

            bool IsThomson { get; }
            bool IsBvd { get; }
            bool Is508User { get; }
            bool IsBackgroundDataFromReuters { get; }

            bool IsDnb { get; }
            bool IsDnbSnapshotOnly { get; }
            bool IsDnbReportsWithFactivaBill { get; }
            bool IsDnbReportsWithCreditcard { get; }

            int MaximumFoldersPerEmailSetup { get; }
            int MaximumPersonalFoldersEmailDelivery{ get; }

            string UserId { get; }
            string AccountId { get; }
            string ProductId { get; }

            bool IsFcpLinkInDotCom {get;}
            /// <summary>
            /// Maximum date range value for index search.
            /// Used in fce/salesworks news channel
            /// </summary>
            /// <created>
            ///         <bucket>Q107</bucket>
            ///         <date>17-12-2007</date>
            ///         <id>NP</id>
            ///</created>
            int MaximumExtendedContentSearchInMonth{get;}


        string EmailLoginStatus { get;}

        string AccountAuthOption { get;}

        string ArchiveAc3 { get;}

        string ArchiveAc2 { get;}

        string ArchiveAc9 { get;}

        bool IsNewsLetterUser { get;}

        bool IsSharing { get;}

        bool IsMultimedia { get;}

        bool IsTimeToLiveToken { get; }

        bool IsPermittedReaderView { get;}

        bool IsExternalReader { get;}

        bool IsDowJonesInsider { get;}

        bool IsInsightUser { get;}

        bool IsPersonalization { get;}

        bool IsPodcastability { get;}

        bool IsProjectVisibleAdsOn { get;}

        bool IsPremiumSnapshotOn { get; }

        bool IsRICForHistoricalOn  { get; }

        bool IsResearchCenterOn { get; }

        bool IsSearchAssistanceOn { get; }

        bool IsRecentCompanySearches { get; }

        bool IsTrackCoreServiceOn { get;}

        //TODO: SM_this has to change once we have the correct ac value;;;
        bool IsSelectDedupeOn { get;}

        string SalesworksPartner { get;}

        bool IsHttps { get; }

        //TODO: SM_this has to change once we have the correct ac value;;;
        bool IsSetMobileCookieOn { get;}
    }
}
