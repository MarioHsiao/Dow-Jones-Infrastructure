// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntitlementsPrincipal.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using DowJones.Utilities.Managers.Core;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Utilities.Security
{
    /// <summary>
    /// The entitlements principal.
    /// </summary>
    public class EntitlementsPrincipal : IPrincipal
    {
        /// <summary>
        /// The _ac 6.
        /// </summary>
        private readonly Hashtable _ac6 = new Hashtable();

        /// <summary>
        /// The _djce sub principal.
        /// </summary>
        private DJCESubPrincipal _djceSubPrincipal;

        /// <summary>
        /// Interface AC10 - used to decide if the dedup options can be displayed for Alert Select Folders(only)
        /// </summary>
        /// <returns></returns>
        private string _interfaceAc10;

        /// <summary>
        /// The _is account administrator.
        /// </summary>
        private bool _isAccountAdministrator;

        /// <summary>
        /// The _is alert administrator.
        /// </summary>
        private bool _isAlertAdministrator;

        /// <summary>
        /// The _is alerts user.
        /// </summary>
        private bool _isAlertsUser;

        /// <summary>
        /// The _is dmm user.
        /// </summary>
        private bool _isDmmUser;

        /// <summary>
        /// The _is group administrator.
        /// </summary>
        private bool _isGroupAdministrator;

        /// <summary>
        /// The _is https.
        /// </summary>
        private bool _isHttps;

        /// <summary>
        /// The _is membership administrator.
        /// </summary>
        private bool _isMembershipAdministrator;

        /// <summary>
        /// The _is news views.
        /// </summary>
        private bool _isNewsViews;

        /// <summary>
        /// Looks for interfac10 value and accordingly sets the  boolean
        /// </summary>
        private bool _isSelectDedupeOn;

        /// <summary>
        /// The _is select full user.
        /// </summary>
        private bool _isSelectFullUser;

        /// <summary>
        /// The _is select headlines user.
        /// </summary>
        private bool _isSelectHeadlinesUser;

        /// <summary>
        /// The _is set mobile cookie on.
        /// </summary>
        private bool _isSetMobileCookieOn;

        /// <summary>
        /// The _is ui info.
        /// </summary>
        private bool _isUiInfo;

        /// <summary>
        /// The _is usage report.
        /// </summary>
        private bool _isUsageReport;

        /// <summary>
        /// The _matrix.
        /// </summary>
        private AuthorizationMatrix _matrix;

        /// <summary>
        /// The _rule set.
        /// </summary>
        private RuleSet _ruleSet;

        /// <summary>
        /// The _segmentation sub principal.
        /// </summary>
        private SegmentationSubPrincipal _segmentationSubPrincipal;

        /// <summary>
        /// The _user authorization response cache.
        /// </summary>
        private GetUserAuthorizationsResponse _userAuthorizationResponseCache;

        /// <summary>
        /// The _maximum extended content search in month.
        /// </summary>
        private int _maximumExtendedContentSearchInMonth;

        /// <summary>
        /// The _maximum folders per email setup.
        /// </summary>
        private int _maximumFoldersPerEmailSetup;

        /// <summary>
        /// The _maximum personal folders email delivery.
        /// </summary>
        private int _maximumPersonalFoldersEmailDelivery;

        /// <summary>
        /// The _product id.
        /// </summary>
        private string _productId;

        /// <summary>
        /// The _salesworks partner.
        /// </summary>
        private string _salesworksPartner;

        /// <summary>
        /// The _user id.
        /// </summary>
        private string _userId;

        /// <summary>
        /// The _account auth option.
        /// </summary>
        private string _accountAuthOption;

        /// <summary>
        /// The _account id.
        /// </summary>
        private string _accountId;

        /// <summary>
        /// The _archive ac 2.
        /// </summary>
        private string _archiveAc2;

        /// <summary>
        /// The _archive ac 3.
        /// </summary>
        private string _archiveAc3;

        /// <summary>
        /// The _archive ac 9.
        /// </summary>
        private string _archiveAc9;

        /// <summary>
        /// The _email login status.
        /// </summary>
        private string _emailLoginStatus;

        /// <summary>
        /// The _can create factiva root group item.
        /// </summary>
        private bool _canCreateFactivaRootGroupItem;

        /// <summary>
        /// The _can get usage report.
        /// </summary>
        private bool _canGetUsageReport;

        /// <summary>
        /// The _can make folder subscribable.
        /// </summary>
        private bool _canMakeFolderSubscribable;

        /// <summary>
        /// The _can switch user.
        /// </summary>
        private bool _canSwitchUser;

        /// <summary>
        /// The _is 508 user.
        /// </summary>
        private bool _is508User;

        /// <summary>
        /// The _is ajax enabled.
        /// </summary>
        private bool _isAjaxEnabled;

        /// <summary>
        /// The _is alert core service on.
        /// </summary>
        private bool _isAlertCoreServiceOn;

        /// <summary>
        /// The _is background data from reuters.
        /// </summary>
        private bool _isBackgroundDataFromReuters;

        /// <summary>
        /// The _is bvd.
        /// </summary>
        private bool _isBvd;

        /// <summary>
        /// The _is corporate user.
        /// </summary>
        private bool _isCorporateUser;

        /// <summary>
        /// The _is creditcard user.
        /// </summary>
        private bool _isCreditcardUser;

        /// <summary>
        /// The _is current quote.
        /// </summary>
        private bool _isCurrentQuote;

        /// <summary>
        /// The _is customer service user.
        /// </summary>
        private bool _isCustomerServiceUser;

        /// <summary>
        /// The _is dnb.
        /// </summary>
        private bool _isDNB;

        /// <summary>
        /// The _is dnb reports with creditcard.
        /// </summary>
        private bool _isDNBReportsWithCreditcard;

        /// <summary>
        /// The _is dnb reports with factiva bill.
        /// </summary>
        private bool _isDNBReportsWithFactivaBill;

        /// <summary>
        /// The _is dnb snapshot only.
        /// </summary>
        private bool _isDNBSnapshotOnly;

        /// <summary>
        /// The _is dow jones insider.
        /// </summary>
        private bool _isDowJonesInsider = true;

        /// <summary>
        /// The _is external reader.
        /// </summary>
        private bool _isExternalReader;

        /// <summary>
        /// The _is fce navigation.
        /// </summary>
        private bool _isFceNavigation;

        /// <summary>
        /// The _is fcp link in dot com.
        /// </summary>
        private bool _isFcpLinkInDotCom = true;

        /// <summary>
        /// The _is full quote.
        /// </summary>
        private bool _isFullQuote;

        /// <summary>
        /// The _is historical quote.
        /// </summary>
        private bool _isHistoricalQuote;

        /// <summary>
        /// The _is individual user.
        /// </summary>
        private bool _isIndividualUser;

        /// <summary>
        /// The _is insight user.
        /// </summary>
        private bool _isInsightUser;

        /// <summary>
        /// The _is interface ac 5 on.
        /// </summary>
        private bool _isInterfaceAc5On = true;

        /// <summary>
        /// The _is manage redirects.
        /// </summary>
        private bool _isManageRedirects;

        /// <summary>
        /// The _is multimedia.
        /// </summary>
        private bool _isMultimedia;

        /// <summary>
        /// The _is news digest.
        /// </summary>
        private bool _isNewsDigest;

        /// <summary>
        /// The _is news letter user.
        /// </summary>
        private bool _isNewsLetterUser;

        /// <summary>
        /// The _is permitted reader view.
        /// </summary>
        private bool _isPermittedReaderView;

        /// <summary>
        /// The _is personalization.
        /// </summary>
        private bool _isPersonalization = true;

        /// <summary>
        /// The _is podcastability.
        /// </summary>
        private bool _isPodcastability = true;

        /// <summary>
        /// The _is premium snapshot on.
        /// </summary>
        private bool _isPremiumSnapshotOn;

        /// <summary>
        /// The _is project visible ads on.
        /// </summary>
        private bool _isProjectVisibleAdsOn = true;

        /// <summary>
        /// The _is quick quote.
        /// </summary>
        private bool _isQuickQuote;

        /// <summary>
        /// The _is recent company searches.
        /// </summary>
        private bool _isRecentCompanySearches = true;

        /// <summary>
        /// The _is redirects.
        /// </summary>
        private bool _isRedirects;

        /// <summary>
        /// The _is research center on.
        /// </summary>
        private bool _isResearchCenterOn;

        /// <summary>
        /// The _is reuter investor quote.
        /// </summary>
        private bool _isReuterInvestorQuote;

        /// <summary>
        /// The _is ric for historical on.
        /// </summary>
        private bool _isRicForHistoricalOn;

        /// <summary>
        /// The _is sales work lite navigation.
        /// </summary>
        private bool _isSalesWorkLiteNavigation;

        /// <summary>
        /// The _is sales works navigation.
        /// </summary>
        private bool _isSalesWorksNavigation;

        /// <summary>
        /// The _is search assistance on.
        /// </summary>
        private bool _isSearchAssistanceOn;

        /// <summary>
        /// The _is share point dissemination point on.
        /// </summary>
        private bool _isSharePointDisseminationPointOn;

        /// <summary>
        /// The _is sharing.
        /// </summary>
        private bool _isSharing;

        /// <summary>
        /// The _is smb navigation.
        /// </summary>
        private bool _isSmbNavigation;

        /// <summary>
        /// The _is thomson.
        /// </summary>
        private bool _isThomson;

        /// <summary>
        /// The _is time to live token.
        /// </summary>
        private bool _isTimeToLiveToken;

        /// <summary>
        /// The _is treadline quote.
        /// </summary>
        private bool _isTreadlineQuote;

        /*
        /// <summary>
        /// The _is page monitoring.
        /// </summary>
        private bool _isPageMonitoring;

        /// <summary>
        /// The _is sub domaining static resources.
        /// </summary>
        private bool _isSubDomainingStaticResources;
        */

        /// <summary>
        /// Gets SegmentationSubPrincipal.
        /// </summary>
        public SegmentationSubPrincipal SegmentationSubPrincipal
        {
            get { return _segmentationSubPrincipal ?? new SegmentationSubPrincipal(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is share point dissemination point on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is share point dissemination point on; otherwise, <c>false</c>.
        /// </value>
        /// <remarks/>
        public bool IsSharePointDisseminationPointOn
        {
            get { return _isSharePointDisseminationPointOn; }
        }

        /// <summary>
        /// Get Interface AC10 - used to decide if the dedup options can be displayed for Alert Select Folders(only)
        /// </summary>
        /// <returns></returns>
        public string InterfaceAC10
        {
            get { return _interfaceAc10; }
        }

        /// <summary>
        /// Gets DjceSubPrincipal.
        /// </summary>
        public DJCESubPrincipal DjceSubPrincipal
        {
            get { return _djceSubPrincipal ?? new DJCESubPrincipal(); }
        }

        #region IPrincipal Members

        /// <summary>
        /// Gets a value indicating whether IsHttps.
        /// </summary>
        public bool IsHttps
        {
            get { return _isHttps; }
        }

        /// <summary>
        /// Gets a value indicating whether IsGroupAdministrator.
        /// </summary>
        public bool IsGroupAdministrator
        {
            get { return _isGroupAdministrator; }
        }

        /// <summary>
        /// Gets a value indicating whether IsAccountAdministrator.
        /// </summary>
        public bool IsAccountAdministrator
        {
            get { return _isAccountAdministrator; }
        }

        /// <summary>
        /// Gets a value indicating whether IsMembershipAdministrator.
        /// </summary>
        public bool IsMembershipAdministrator
        {
            get { return _isMembershipAdministrator; }
        }

        /// <summary>
        /// Gets whether the user is on an old firewall which doesn't support ajax
        /// </summary>
        public bool IsAjaxEnabled
        {
            get { return _isAjaxEnabled; }
        }


        /// <summary>
        /// Gets whether the user is a DMM user
        /// </summary>
        public bool IsDMMUser
        {
            get { return _isDmmUser; }
        }

        /// <summary>
        /// Gets whether the user is a Select-FullArticle user
        /// </summary>
        public bool IsSelectFullUser
        {
            get { return _isSelectFullUser; }
        }

        /// <summary>
        /// Determines whether the user is a Select-Headlines user
        /// </summary>
        public bool IsSelectHeadlinesUser
        {
            get { return _isSelectHeadlinesUser; }
        }

        /// <summary>
        /// Determines whether the user is a Alerts user
        /// </summary>
        public bool IsAlertsUser
        {
            get { return _isAlertsUser; }
        }

        /// <summary>
        /// Determines whether the user is a Alert administrator
        /// </summary>
        public bool IsAlertAdministrator
        {
            get { return _isAlertAdministrator; }
        }

        /// <summary>
        /// Gets the maximum number of items in the specified class
        /// </summary>
        /// <param name="classId">
        /// </param>
        /// <returns>
        /// The get max items.
        /// </returns>
        public int GetMaxItems(PreferenceClassID classId)
        {
            if (_ac6.Contains(classId))
            {
                return (int) _ac6[classId];
            }

            return 25;
        }

        /// <summary>
        /// Gets the RuleSet
        /// </summary>
        public RuleSet RuleSet
        {
            get { return _ruleSet; }
        }

        /// <summary>
        /// Gets a value indicating whether IsUsageReport.
        /// </summary>
        public bool IsUsageReport
        {
            get { return _isUsageReport; }
        }

        /// <summary>
        /// Gets a value indicating whether IsUiInfo.
        /// </summary>
        public bool IsUiInfo
        {
            get { return _isUiInfo; }
        }

        /// <summary>
        /// Gets a value indicating whether IsNewsViews.
        /// </summary>
        public bool IsNewsViews
        {
            get { return _isNewsViews; }
        }

        /// <summary>
        /// Gets MaxFolderForGlobal.
        /// </summary>
        public int MaxFolderForGlobal
        {
            get { return GetNumberOfAlertFolders("G"); }
        }

        /// <summary>
        /// Gets MaxFolderForSelectHeadline.
        /// </summary>
        public int MaxFolderForSelectHeadline
        {
            get { return GetNumberOfAlertFolders("H"); }
        }

        /// <summary>
        /// Gets MaxFolderForSelectFullText.
        /// </summary>
        public int MaxFolderForSelectFullText
        {
            get { return GetNumberOfAlertFolders("T"); }
        }

        /// <summary>
        /// Gets MaxFolderForFastAlert.
        /// </summary>
        public int MaxFolderForFastAlert
        {
            get { return GetNumberOfAlertFolders("F"); }
        }

        /// <summary>
        /// Gets MaxFolderForFcpAndIwe.
        /// </summary>
        public int MaxFolderForFcpAndIwe
        {
            get { return GetNumberOfAlertFolders("P"); }
        }

        /// <summary>
        /// Gets MaxFolderForDmm.
        /// </summary>
        public int MaxFolderForDmm
        {
            get { return GetNumberOfAlertFolders("M"); }
        }

        /// <summary>
        /// Gets a value indicating whether CanGetUsageReport.
        /// </summary>
        public bool CanGetUsageReport
        {
            get { return _canGetUsageReport; }
        }

        /// <summary>
        /// Gets a value indicating whether CanMakeFolderSubscribable.
        /// </summary>
        public bool CanMakeFolderSubscribable
        {
            get { return _canMakeFolderSubscribable; }
        }

        /// <summary>
        /// Gets a value indicating whether CanCreateFactivaRootGroupItem.
        /// </summary>
        public bool CanCreateFactivaRootGroupItem
        {
            get { return _canCreateFactivaRootGroupItem; }
        }


        /// <summary>
        /// Gets a value indicating whether IsNewsDigest.
        /// </summary>
        public bool IsNewsDigest
        {
            get { return _isNewsDigest; }
        }

        /// <summary>
        /// Gets a value indicating whether IsHistoricalQuote.
        /// </summary>
        public bool IsHistoricalQuote
        {
            get { return _isHistoricalQuote; }
        }

        /// <summary>
        /// Gets a value indicating whether IsTreadlineQuote.
        /// </summary>
        public bool IsTreadlineQuote
        {
            get { return _isTreadlineQuote; }
        }

        /// <summary>
        /// Gets a value indicating whether IsReuterInvestorQuote.
        /// </summary>
        public bool IsReuterInvestorQuote
        {
            get { return _isReuterInvestorQuote; }
        }

        /// <summary>
        /// Gets a value indicating whether IsCurrentQuote.
        /// </summary>
        public bool IsCurrentQuote
        {
            get { return _isCurrentQuote; }
        }

        /// <summary>
        /// Gets a value indicating whether IsQuickQuote.
        /// </summary>
        public bool IsQuickQuote
        {
            get { return _isQuickQuote; }
        }

        /// <summary>
        /// Gets a value indicating whether IsFullQuote.
        /// </summary>
        public bool IsFullQuote
        {
            get { return _isFullQuote; }
        }


        /// <summary>
        /// Gets a value indicating whether IsFceNavigation.
        /// </summary>
        public bool IsFceNavigation
        {
            get { return _isFceNavigation; }
        }

        /// <summary>
        /// Gets a value indicating whether IsSalesWorksNavigation.
        /// </summary>
        public bool IsSalesWorksNavigation
        {
            get { return _isSalesWorksNavigation; }
        }

        /// <summary>
        /// Gets a value indicating whether IsSmbNavigation.
        /// </summary>
        public bool IsSmbNavigation
        {
            get { return _isSmbNavigation; }
        }

        /// <summary>
        /// Gets a value indicating whether IsCreditcardUser.
        /// </summary>
        public bool IsCreditcardUser
        {
            get { return _isCreditcardUser; }
        }

        /// <summary>
        /// Gets a value indicating whether IsCorporateUser.
        /// </summary>
        public bool IsCorporateUser
        {
            get { return _isCorporateUser; }
        }

        /// <summary>
        /// Gets a value indicating whether IsCustomerServiceUser.
        /// </summary>
        public bool IsCustomerServiceUser
        {
            get { return _isCustomerServiceUser; }
        }

        /// <summary>
        /// Gets a value indicating whether IsIndividualUser.
        /// </summary>
        public bool IsIndividualUser
        {
            get { return _isIndividualUser; }
        }

        /// <summary>
        /// Gets a value indicating whether CanSwitchUser.
        /// </summary>
        public bool CanSwitchUser
        {
            get { return _canSwitchUser; }
        }

        /// <summary>
        /// Gets a value indicating whether IsRedirects.
        /// </summary>
        public bool IsRedirects
        {
            get { return _isRedirects; }
        }

        /// <summary>
        /// Gets a value indicating whether IsManageRedirects.
        /// </summary>
        public bool IsManageRedirects
        {
            get { return _isManageRedirects; }
        }

        /// <summary>
        /// Gets a value indicating whether IsThomson.
        /// </summary>
        public bool IsThomson
        {
            get { return _isThomson; }
        }

        /// <summary>
        /// Gets a value indicating whether IsBvd.
        /// </summary>
        public bool IsBvd
        {
            get { return _isBvd; }
        }

        /// <summary>
        /// Gets a value indicating whether Is508User.
        /// </summary>
        public bool Is508User
        {
            get { return _is508User; }
        }

        /// <summary>
        /// Gets a value indicating whether IsBackgroundDataFromReuters.
        /// </summary>
        public bool IsBackgroundDataFromReuters
        {
            get { return _isBackgroundDataFromReuters; }
        }

        /// <summary>
        /// Gets a value indicating whether IsDNB.
        /// </summary>
        public bool IsDNB
        {
            get { return _isDNB; }
        }

        /// <summary>
        /// Gets a value indicating whether IsDnbSnapshotOnly.
        /// </summary>
        public bool IsDnbSnapshotOnly
        {
            get { return _isDNBSnapshotOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether IsDnbReportsWithFactivaBill.
        /// </summary>
        public bool IsDnbReportsWithFactivaBill
        {
            get { return _isDNBReportsWithFactivaBill; }
        }

        /// <summary>
        /// Gets a value indicating whether IsDnbReportsWithCreditcard.
        /// </summary>
        public bool IsDnbReportsWithCreditcard
        {
            get { return _isDNBReportsWithCreditcard; }
        }

        /// <summary>
        /// Gets MaximumFoldersPerEmailSetup.
        /// </summary>
        public int MaximumFoldersPerEmailSetup
        {
            get { return _maximumFoldersPerEmailSetup; }
        }

        /// <summary>
        /// Gets MaximumPersonalFoldersEmailDelivery.
        /// </summary>
        public int MaximumPersonalFoldersEmailDelivery
        {
            get { return _maximumPersonalFoldersEmailDelivery; }
        }

        /// <summary>
        /// Gets UserId.
        /// </summary>
        public string UserId
        {
            get { return _userId; }
        }

        /// <summary>
        /// Gets AccountId.
        /// </summary>
        public string AccountId
        {
            get { return _accountId; }
        }

        /// <summary>
        /// Gets ProductId.
        /// </summary>
        public string ProductId
        {
            get { return _productId; }
        }

        /// <summary>
        /// Gets a value indicating whether IsFcpLinkInDotCom.
        /// </summary>
        public bool IsFcpLinkInDotCom
        {
            get { return _isFcpLinkInDotCom; }
        }

        /// <summary>
        /// Gets MaximumExtendedContentSearchInMonth.
        /// </summary>
        public int MaximumExtendedContentSearchInMonth
        {
            get { return _maximumExtendedContentSearchInMonth; }
        }

        /// <summary>
        /// Gets EmailLoginStatus.
        /// </summary>
        public string EmailLoginStatus
        {
            get { return _emailLoginStatus; }
        }

        /// <summary>
        /// Gets AccountAuthOption.
        /// </summary>
        public string AccountAuthOption
        {
            get { return _accountAuthOption; }
        }

        /// <summary>
        /// Gets ArchiveAc3.
        /// </summary>
        public string ArchiveAc3
        {
            get { return _archiveAc3; }
        }

        /// <summary>
        /// Gets ArchiveAc2.
        /// </summary>
        public string ArchiveAc2
        {
            get { return _archiveAc2; }
        }


        /// <summary>
        /// Gets ArchiveAc9.
        /// </summary>
        public string ArchiveAc9
        {
            get { return _archiveAc9; }
        }

        /// <summary>
        /// Gets a value indicating whether IsNewsLetterUser.
        /// </summary>
        public bool IsNewsLetterUser
        {
            get { return _isNewsLetterUser; }
        }

        /// <summary>
        /// Gets a value indicating whether IsSharing.
        /// </summary>
        public bool IsSharing
        {
            get { return _isSharing; }
        }

        /// <summary>
        /// Gets a value indicating whether IsMultimedia.
        /// </summary>
        public bool IsMultimedia
        {
            get { return _isMultimedia; }
        }

        /// <summary>
        /// Gets a value indicating whether IsTimeToLiveToken.
        /// </summary>
        public bool IsTimeToLiveToken
        {
            get { return _isTimeToLiveToken; }
        }

        /// <summary>
        /// Gets a value indicating whether IsPermittedReaderView.
        /// </summary>
        public bool IsPermittedReaderView
        {
            get { return _isPermittedReaderView; }
        }

        /// <summary>
        /// Gets a value indicating whether IsExternalReader.
        /// </summary>
        public bool IsExternalReader
        {
            get { return _isExternalReader; }
        }

        /// <summary>
        /// Gets a value indicating whether IsDowJonesInsider.
        /// </summary>
        public bool IsDowJonesInsider
        {
            get { return _isDowJonesInsider; }
        }

        /// <summary>
        /// Gets a value indicating whether IsInsightUser.
        /// </summary>
        public bool IsInsightUser
        {
            get { return _isInsightUser; }
        }

        /// <summary>
        /// Gets a value indicating whether IsPersonalization.
        /// </summary>
        public bool IsPersonalization
        {
            get { return _isPersonalization; }
        }

        /// <summary>
        /// Gets a value indicating whether IsPodcastability.
        /// </summary>
        public bool IsPodcastability
        {
            get { return _isPodcastability; }
        }

        /// <summary>
        /// Gets a value indicating whether IsProjectVisibleAdsOn.
        /// </summary>
        public bool IsProjectVisibleAdsOn
        {
            get { return _isProjectVisibleAdsOn; }
        }

        /// <summary>
        /// Gets a value indicating whether IsPremiumSnapshotOn.
        /// </summary>
        public bool IsPremiumSnapshotOn
        {
            get { return _isPremiumSnapshotOn; }
        }

        /// <summary>
        /// Gets a value indicating whether IsRICForHistoricalOn.
        /// </summary>
        public bool IsRICForHistoricalOn
        {
            get { return _isRicForHistoricalOn; }
        }

        /// <summary>
        /// Gets a value indicating whether IsResearchCenterOn.
        /// </summary>
        public bool IsResearchCenterOn
        {
            get { return _isResearchCenterOn; }
        }


        /// <summary>
        /// Gets a value indicating whether IsSalesWorkLiteNavigation.
        /// </summary>
        public bool IsSalesWorkLiteNavigation
        {
            get { return _isSalesWorkLiteNavigation; }
        }

        /// <summary>
        /// Gets a value indicating whether IsSearchAssistanceOn.
        /// </summary>
        public bool IsSearchAssistanceOn
        {
            get { return _isSearchAssistanceOn; }
        }

        /// <summary>
        /// Gets a value indicating whether IsRecentCompanySearches.
        /// </summary>
        public bool IsRecentCompanySearches
        {
            get { return _isRecentCompanySearches; }
        }

        /// <summary>
        /// Gets a value indicating whether IsAlertCoreServiceOn.
        /// </summary>
        public bool IsAlertCoreServiceOn
        {
            get { return _isAlertCoreServiceOn; }
        }

        /// <summary>
        /// Gets a value indicating whether IsSelectDedupeOn.
        /// </summary>
        public bool IsSelectDedupeOn
        {
            get { return _isSelectDedupeOn; }
        }

        /// <summary>
        /// Gets SalesworksPartner.
        /// </summary>
        public string SalesworksPartner
        {
            get { return _salesworksPartner; }
        }

        /// <summary>
        /// Controls of the user can see the Set Mobile cookie link in dotcom tools. based on offset 18 in the rule set.
        /// </summary>
        public bool IsSetMobileCookieOn
        {
            get { return _isSetMobileCookieOn; }
        }

        #endregion

        /// <summary>
        /// loads the UserAuthorizationResponse into the cache if it is not loaded
        /// </summary>
        /// <param name="getUserAuthorizationsResponse">
        /// The get User Authorizations Response.
        /// </param>
        public void Load(GetUserAuthorizationsResponse getUserAuthorizationsResponse)
        {
            // Only allow one to be set per instance;
            if (getUserAuthorizationsResponse == null || _userAuthorizationResponseCache != null)
                return;
            _userAuthorizationResponseCache = getUserAuthorizationsResponse;

            _isGroupAdministrator = getUserAuthorizationsResponse.administratorFlag == AdministratorFlag.GroupAdministrator;
            _isAccountAdministrator = getUserAuthorizationsResponse.administratorFlag == AdministratorFlag.AccountAdministrator;
            _isMembershipAdministrator = _isAccountAdministrator || _isGroupAdministrator;
            _userId = getUserAuthorizationsResponse.UserId;
            _accountId = getUserAuthorizationsResponse.AccountId;
            _productId = getUserAuthorizationsResponse.ProductId;

            // Parse membership.ac6
            if (getUserAuthorizationsResponse.AuthorizationMatrix.Membership.ac6 != null && getUserAuthorizationsResponse.AuthorizationMatrix.Membership.ac6.Count > 0)
            {
                var ac6Serialized = "<AC6>" + getUserAuthorizationsResponse.AuthorizationMatrix.Membership.ac6[0] + "</AC6>";
                _ac6.Clear();
                var ac6Serializer = new XmlSerializer(typeof (AC6));
                var ac6 = (AC6) ac6Serializer.Deserialize(new StringReader(ac6Serialized));
                if (ac6.ITEM != null)
                {
                    foreach (var item in ac6.ITEM)
                    {
                        _ac6.Add((PreferenceClassID) item.@class, item.count);
                    }
                }
            }

            // parse Alert.ac7
            var ac7 = string.Empty;
            if (getUserAuthorizationsResponse.AuthorizationMatrix.Track != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Track.ac7 != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Track.ac7.Count == 1)
            {
                ac7 = getUserAuthorizationsResponse.AuthorizationMatrix.Track.ac7[0] ?? string.Empty;
                if (ac7 == "-1")
                {
                    _isAlertAdministrator = true;
                }
            }


            var da7 = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.da7;
            _isAjaxEnabled = da7 != "1";

            if (getUserAuthorizationsResponse.AuthorizationMatrix.Interface != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac7 != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac7.Count == 1)
            {
                ac7 = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac7[0] ?? string.Empty;
            }

            _isPodcastability = ac7 != null && ac7.Trim().ToUpper() == "ON";


            if (getUserAuthorizationsResponse.AuthorizationMatrix.Interface != null)
            {
                var da5 = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.da5;
                if (da5 != null && da5.Trim().Length > 0)
                {
                    _maximumExtendedContentSearchInMonth = int.Parse(da5);
                }
            }


            // parse Alert.ac8
            var ac8 = string.Empty;
            if (getUserAuthorizationsResponse.AuthorizationMatrix.Track != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Track.ac8 != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Track.ac8.Count == 1)
            {
                ac8 = getUserAuthorizationsResponse.AuthorizationMatrix.Track.ac8[0] ?? string.Empty;
            }

            _isDmmUser = ac8.ToLower().StartsWith("m");
            _isSelectFullUser = ac8.ToLower().StartsWith("t");
            _isSelectHeadlinesUser = ac8.ToLower().StartsWith("h");
            _isAlertsUser = ac8.ToLower().StartsWith("f");

            // parse ruleset
            var sessionRuleSet = getUserAuthorizationsResponse.RuleSet;
            _ruleSet = new RuleSet(sessionRuleSet);

            _matrix = getUserAuthorizationsResponse.AuthorizationMatrix;
            string membershipAc4 = null;
            string cibsAc2 = null;
            string membershipAc2 = null;
            string membershipAc3 = null;
            string mdsAc1 = null;
            string mdsAc2 = null;
            string mdsAc3 = null;
            string uerAc1 = null;
            string uerAc2 = null;
            string emailAc3 = null;
            string alertAc3 = null;
            var isAcademicUser = false;
            if (_matrix != null)
            {
                // Segmentation Node
                _segmentationSubPrincipal = new SegmentationSubPrincipal(_matrix.FinacialServiceInterface);

                // Dow Jones Companies and Executives Node
                _djceSubPrincipal = new DJCESubPrincipal(_matrix.Interface);

                if (_matrix.Cibs != null)
                {
                    if (_matrix.Cibs.ac1 != null &&
                        _matrix.Cibs.ac1.Count == 1)
                    {
                        _isUsageReport = _matrix.Cibs.ac1[0] != "N";
                    }

                    if (_matrix.Cibs.ac2 != null &&
                        _matrix.Cibs.ac2.Count == 1)
                    {
                        cibsAc2 = _matrix.Cibs.ac2[0];
                    }
                }

                if (_matrix.MarketData != null)
                {
                    if (_matrix.MarketData.ac1 != null &&
                        _matrix.MarketData.ac1.Count == 1)
                    {
                        mdsAc1 = _matrix.MarketData.ac1[0];
                    }

                    if (_matrix.MarketData.ac2 != null &&
                        _matrix.MarketData.ac2.Count == 1)
                    {
                        mdsAc2 = _matrix.MarketData.ac2[0];
                    }

                    if (_matrix.MarketData.ac3 != null &&
                        _matrix.MarketData.ac3.Count == 1)
                    {
                        mdsAc3 = _matrix.MarketData.ac3[0];
                    }
                }

                if (_matrix.Membership != null)
                {
                    if (_matrix.Membership.ac1 != null &&
                        _matrix.Membership.ac1.Count == 1)
                    {
                        _canMakeFolderSubscribable = _matrix.Membership.ac1[0] == "A";
                    }

                    if (_matrix.Membership.ac2 != null &&
                        _matrix.Membership.ac2.Count == 1)
                    {
                        membershipAc2 = _matrix.Membership.ac2[0];
                    }

                    if (_matrix.Membership.ac3 != null &&
                        _matrix.Membership.ac3.Count == 1)
                    {
                        membershipAc3 = _matrix.Membership.ac3[0];
                    }

                    if (_matrix.Membership.ac4 != null &&
                        _matrix.Membership.ac4.Count == 1)
                    {
                        membershipAc4 = _matrix.Membership.ac4[0];
                    }
                }

                if (_matrix.Interface != null)
                {
                    if (_matrix.Interface.ac1 != null &&
                        _matrix.Interface.ac1.Count == 1)
                    {
                        _isUiInfo = _matrix.Interface.ac1[0] == "W" || _matrix.Interface.ac1[0] == "R";
                    }

                    if (_matrix.Interface.ac2 != null &&
                        _matrix.Interface.ac2.Count == 1)
                    {
                        _is508User = _matrix.Interface.ac2[0] == "Y";
                    }

                    if (_matrix.Interface.ac3 != null &&
                        _matrix.Interface.ac3.Count == 1)
                    {
                        _isFceNavigation = _matrix.Interface.ac3[0] == "4";
                        _isSalesWorksNavigation = _matrix.Interface.ac3[0] == "3";
                        _isSmbNavigation = _matrix.Interface.ac3[0] == "5";
                        _isSalesWorkLiteNavigation = _matrix.Interface.ac3[0] == "6";
                    }

                    if (_matrix.Interface.ac6 != null &&
                        _matrix.Interface.ac6.Count == 1)
                    {
                        _isPremiumSnapshotOn = _matrix.Interface.ac6[0] == "PREMIUM";
                    }

                    if (_matrix.Interface.ac9 != null &&
                        _matrix.Interface.ac9.Count == 1 &&
                        _matrix.Interface.ac9[0] != null &&
                        _matrix.Interface.ac9[0].Trim() == "A")
                    {
                        _isFcpLinkInDotCom = false;
                        isAcademicUser = true;
                    }

                    if (_matrix.Interface.insider != null && _matrix.Interface.insider.Trim() == "OFF")
                    {
                        _isDowJonesInsider = false;
                    }

                    if (_matrix.Interface.allowSharePointWidget != null && _matrix.Interface.allowSharePointWidget.Trim() == "ON")
                    {
                        _isSharePointDisseminationPointOn = true;
                    }
                }

                if (_matrix.UER != null)
                {
                    if (_matrix.UER.ac1 != null &&
                        _matrix.UER.ac1.Count == 1)
                    {
                        uerAc1 = _matrix.UER.ac1[0];
                    }

                    if (_matrix.UER.ac2 != null &&
                        _matrix.UER.ac2.Count == 1)
                    {
                        uerAc2 = _matrix.UER.ac2[0];
                    }
                }

                if (_matrix.Email != null &&
                    _matrix.Email.ac3 != null &&
                    _matrix.Email.ac3.Count == 1)
                {
                    emailAc3 = _matrix.Email.ac3[0];
                }

                if (_matrix.Track != null &&
                    _matrix.Track.ac3 != null &&
                    _matrix.Track.ac3.Count == 1)
                {
                    alertAc3 = _matrix.Track.ac3[0];
                }

                if (_matrix.Archive != null)
                {
                    if (_matrix.Archive.ac3 != null &&
                        _matrix.Archive.ac3.Count == 1)
                    {
                        _archiveAc3 = _matrix.Archive.ac3[0];
                    }

                    if (_matrix.Archive.ac2 != null &&
                        _matrix.Archive.ac2.Count == 1)
                    {
                        _archiveAc2 = _matrix.Archive.ac2[0];
                    }

                    if (_matrix.Archive.ac9 != null &&
                        _matrix.Archive.ac9.Count == 1)
                    {
                        _archiveAc9 = _matrix.Archive.ac9[0];
                    }
                }

                _isNewsDigest = _matrix.Email != null &&
                                _matrix.Email.ac1 != null &&
                                _matrix.Email.ac1.Count == 1 &&
                                _matrix.Email.ac1[0] == "Y";
            }

            _canCreateFactivaRootGroupItem = membershipAc4 == "W";
            _canGetUsageReport = cibsAc2 == "Y";
            if (mdsAc1 == null)
            {
                mdsAc1 = String.Empty;
            }

            if (mdsAc2 == null)
            {
                mdsAc2 = String.Empty;
            }

            mdsAc1 = mdsAc1.ToUpper();
            mdsAc2 = mdsAc2.ToUpper();
            _isHistoricalQuote = mdsAc2 != "OFF";
            if (mdsAc2.IndexOf("TLINE") != -1)
            {
                _isTreadlineQuote = true;
                _isReuterInvestorQuote = true;
            }
            else if (mdsAc2.IndexOf("RIVST") != -1)
            {
                _isReuterInvestorQuote = true;
            }

            _isCurrentQuote = mdsAc1 != "OFF";
            _isQuickQuote = mdsAc1 == "QUICK";
            _isFullQuote = mdsAc1 == "FULL";

            _isCreditcardUser = getUserAuthorizationsResponse.UserType == UserType.Creditcard;
            _isCorporateUser = getUserAuthorizationsResponse.UserType == UserType.Corporate;
            _isCustomerServiceUser = getUserAuthorizationsResponse.UserType == UserType.CustomerService;
            _isIndividualUser = getUserAuthorizationsResponse.UserType == UserType.Individual;

            _canSwitchUser = membershipAc3 == "Y";
            _isRedirects = membershipAc2 != null && membershipAc2.ToUpper().IndexOf("R") != -1;
            _isManageRedirects = membershipAc2 != null && membershipAc2.ToUpper().IndexOf("W") != -1;

            if (uerAc1 == null)
            {
                uerAc1 = String.Empty;
            }

            if (uerAc2 == null)
            {
                uerAc2 = String.Empty;
            }

            _isThomson = uerAc2.ToUpper().IndexOf("SEC=1") != -1;
            _isBvd = uerAc2.ToUpper().IndexOf("BVD=1") != -1;
            _isBackgroundDataFromReuters = mdsAc3 != "OFF";
            _isDNB = uerAc1 != "0";
            _isDNBSnapshotOnly = uerAc1 == "1";
            _isDNBReportsWithFactivaBill = uerAc1 == "3";
            _isDNBReportsWithCreditcard = uerAc1 == string.Empty || uerAc1 == "2";

            if (emailAc3 != null && emailAc3.Trim().Length > 0)
            {
                _maximumFoldersPerEmailSetup = int.Parse(emailAc3.Trim());
            }

            if (alertAc3 == null)
            {
                _maximumPersonalFoldersEmailDelivery = 50;
            }
            else if (alertAc3.Trim().Length > 0)
            {
                _maximumPersonalFoldersEmailDelivery = int.Parse(alertAc3.Trim());
            }

            _emailLoginStatus = getUserAuthorizationsResponse.emailLogin;
            _accountAuthOption = getUserAuthorizationsResponse.lwrFlag;

            _isNewsLetterUser = true;
            var personalization = getUserAuthorizationsResponse.AuthorizationMatrix.Membership.personalization ?? string.Empty;

            // if not off then On, even if blank
            if (getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac5 != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac5.Count == 1 &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac5[0] != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac5[0].Trim().ToUpper() == "OFF")
            {
                _isInterfaceAc5On = false;
            }

            _isPersonalization = personalization.Trim().ToUpper() != "OFF";
            if (
/*!_ruleSet.IsDotComBriefcaseDisplayServiceOn ||*/ // sm 1/8/09 and adde the _isInterfaceAc5On
                !_isInterfaceAc5On ||
                _isSelectFullUser ||
                _isSelectHeadlinesUser ||
                (
                    (!_isMembershipAdministrator)
                    &&
                    (
                        isAcademicUser
                        || _isCreditcardUser
                        || !_isPersonalization
                    )
                )
                )
            {
                _isNewsLetterUser = false;
            }

            _isSharing = true;
            var sharingDa = getUserAuthorizationsResponse.AuthorizationMatrix.Membership.sharing ?? string.Empty;
            ac8 = (getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac8 != null &&
                   getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac8.Count == 1 &&
                   getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac8[0] != null) ? getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac8[0] : string.Empty;

            _isPermittedReaderView = ac8.Trim().ToUpper() == "PRV";
            if (isAcademicUser || personalization.Trim().ToUpper() == "OFF" || sharingDa.Trim().ToUpper() == "OFF" || _isPermittedReaderView)
            {
                _isSharing = false;
            }

            _isMultimedia = true;

// Removing check for content blocking - NP 
            // valueHolder = _userAuthorizationResponseCache.authorizationMatrix.membership.multimedia;
            // if (valueHolder != null && valueHolder.Trim().ToUpper() == "OFF")
            // {
            // isMultimedia = false;
            // }
            var valueHolder = getUserAuthorizationsResponse.AuthorizationMatrix.Membership.newsletterDA;
            if (valueHolder != null && valueHolder.Trim().ToUpper() == "TTLT")
            {
                _isTimeToLiveToken = true;
            }

            _isExternalReader = getUserAuthorizationsResponse.externalReaderFlag;

            _isInsightUser = _ruleSet.IsInsightUser;

            var projectVisibleAdsOnDA = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ads;

            if (projectVisibleAdsOnDA != null && projectVisibleAdsOnDA.Trim().ToUpper() == "OFF")
            {
                _isProjectVisibleAdsOn = false;
            }

            if (_archiveAc9 != null && _archiveAc9.ToUpper() == "RIC=ON")
            {
                _isRicForHistoricalOn = true;
            }

            var researchCenterDA = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.rCenter;
            if (researchCenterDA != null && researchCenterDA.Trim().ToUpper() == "ON")
            {
                _isResearchCenterOn = true;
            }

            var searchAssistanceDA = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.searchAssist;
            if (searchAssistanceDA != null && searchAssistanceDA.Trim().ToUpper() == "ON")
            {
                _isSearchAssistanceOn = true;
            }

// AC4 can contain "ssl" or "rcs" or both separated by ',' (HTTPS or RecentCompanySearches)
            if (getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac4 != null &&
                getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac4.Count == 1)
            {
                valueHolder = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.ac4[0];
            }

            if (!String.IsNullOrEmpty(valueHolder) && valueHolder.IndexOf("rcs") != -1)
            {
                _isRecentCompanySearches = false;
            }

            if (!String.IsNullOrEmpty(valueHolder) && valueHolder.ToLower().IndexOf("ssl") != -1)
            {
                _isHttps = true;
            }

            _isAlertCoreServiceOn = _ruleSet.IsTrackCoreServiceOn;

            if (_matrix.Interface != null &&
                _matrix.Interface.ac10 != null &&
                _matrix.Interface.ac10.Count == 1)
            {
                _interfaceAc10 = _matrix.Interface.ac10[0];
            }

            if (_interfaceAc10 != null && _interfaceAc10.Trim().ToUpper() == "ON")
            {
                _isSelectDedupeOn = true;
            }

            var newsViewsPermissions = new NewsViewsPermissions(getUserAuthorizationsResponse);

            _isNewsViews = newsViewsPermissions.IsNewsViewsAdministratorOn;

            var salesworksPartnerDA = getUserAuthorizationsResponse.AuthorizationMatrix.Interface.salesworksPartner;
            if (!String.IsNullOrEmpty(salesworksPartnerDA))
            {
                _salesworksPartner = salesworksPartnerDA;
            }

            _isSetMobileCookieOn = _ruleSet.IsSetMobileCookieOn;
        }

        /// <summary>
        /// The get number of alert folders.
        /// </summary>
        /// <param name="productCode">
        /// The product code.
        /// </param>
        /// <returns>
        /// The get number of alert folders.
        /// </returns>
        private int GetNumberOfAlertFolders(string productCode)
        {
            if (!_ruleSet.IsTrackCoreServiceOn)
            {
                return 0;
            }

            string ac4 = null;
            if (_userAuthorizationResponseCache.AuthorizationMatrix != null &&
                _userAuthorizationResponseCache.AuthorizationMatrix.Track != null &&
                _userAuthorizationResponseCache.AuthorizationMatrix.Track.ac4 != null &&
                _userAuthorizationResponseCache.AuthorizationMatrix.Track.ac4.Count == 1)
            {
                ac4 = _userAuthorizationResponseCache.AuthorizationMatrix.Track.ac4[0];
            }

            if (ac4 == null)
            {
                ac4 = String.Empty;
            }

            if (ac4.Trim() == string.Empty)
            {
                return 25;
            }

            if (StringUtilitiesManager.IsNumeric(ac4))
            {
                return int.Parse(ac4);
            }

            // Parse ac4
            string number = null;
            var parts = ac4.Split(new[] {';'});
            foreach (var part in parts)
            {
                if (part.IndexOf(productCode + ":") != -1)
                {
                    number = part.Split(new[] {':'})[1];
                }
            }

            if (number == null)
            {
                number = parts[0];
            }

            return StringUtilitiesManager.IsNumeric(number) ? int.Parse(number) : 0;
        }

        /// <summary>
        /// The get quotes access level.
        /// </summary>
        /// <param name="mdsAc1">
        /// The mds ac 1.
        /// </param>
        /// <param name="mdsAc2">
        /// The mds ac 2.
        /// </param>
        /// <returns>
        /// The get quotes access level.
        /// </returns>
        protected static int GetQuotesAccessLevel(string mdsAc1, string mdsAc2)
        {
            switch (mdsAc1)
            {
                case "OFF":
                    switch (mdsAc2)
                    {
                        case "OFF":
                            return 0;
                        case "RIVST":
                            return 1;
                        default:
                            return 2;
                    }

                case "QUICK":
                    switch (mdsAc2)
                    {
                        case "OFF":
                            return 3;
                        case "RIVST":
                            return 4;
                        default:
                            return 5;
                    }

                default:
                    switch (mdsAc2)
                    {
                        case "OFF":
                            return 6;
                        case "RIVST":
                            return 7;
                        default:
                            return 8;
                    }
            }
        }
    }
}