using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using EMG.Utility.Managers.Core;
using EMG.Utility.Managers.Security;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace EMG.Utility.Security
{
    public class EntitlementsPrincipal : IPrincipal
    {
        private GetUserAuthorizationsResponse userAuthorizationResponseCache;

        private bool isMembershipAdmin;
        private bool isGroupAdministrator;
        private bool isAccountAdministrator;
        private bool isTrackAdministrator;
        private bool isDMMUser;
        private bool isSelectFullUser;
        private bool isSelectHeadlinesUser;
        private bool isAlertsUser;
        private readonly Hashtable ac6 = new Hashtable();
        private RuleSet ruleSet;

        private bool isUsageReport;
        private bool isUiInfo;
        private bool isNewsViews;

        private bool canMakeFolderSubscribable;
        private bool canCreateFactivaRootGroupItem;
        private bool canGetUsageReport;


        private bool isNewsDigest;
        private bool isAjaxEnabled;

        private bool isHistoricalQuote;
        private bool isTreadlineQuote;
        private bool isReuterInvestorQuote;
        private bool isCurrentQuote;
        private bool isQuickQuote;
        private bool isFullQuote;
        private bool isFceNavigation;
        private bool isSalesWorksNavigation;
        private bool isSmbNavigation;
        private bool isCreditcardUser;
        private bool isCorporateUser;
        private bool isCustomerServiceUser;
        private bool isIndividualUser;
        private bool canSwitchUser;
        private bool isRedirects;
        private bool isManageRedirects;
        private bool isThomson;
        private bool isBvd;
        private bool is508User;
        private bool isBackgroundDataFromReuters;
        private bool isDNB;
        private bool isDNBSnapshotOnly;
        private bool isDNBReportsWithFactivaBill;
        private bool isDNBReportsWithCreditcard;

        private int maximumFoldersPerEmailSetup = 0;
        private int maximumPersonalFoldersEmailDelivery = 0;

        private string userId;
        private string accountId;
        private string productId;
        private bool isFcpLinkInDotCom = true;
        private int maximumExtendedContentSearchInMonth;
        private string emailLoginStatus;
        private string accountAuthOption;
        private string archiveAc3;
        private string archiveAc2;
        private string archiveAc9;
        private string salesworksPartner;
        private bool isNewsLetterUser;
        private bool isSharing;
        private bool isMultimedia;
        private bool isTimeToLiveToken;
        private bool isPermittedReaderView;
        private bool isExternalReader;
        private bool isDowJonesInsider = true;
        private bool isInsightUser;
        private bool isPersonalization = true;
        private bool isPodcastability = true;
        private bool isProjectVisibleAdsOn = true;
        private bool isPremiumSnapshotOn = false;
        private bool isRICForHistoricalOn = false;
        private bool isResearchCenterOn = false;
        private bool isSalesWorkLiteNavigation = false;
        private bool isSearchAssistanceOn = false;
        private bool isRecentCompanySearches = true;
        private bool isTrackCoreServiceOn = false;
        private bool isInterfaceAC5On = true;

        public bool isSetMobileCookieOn = false;
        /// <summary>
        /// Interface AC10 - used to decide if the dedup options can be displayed for Track Select Folders(only)
        /// </summary>
        /// <returns></returns>
        private string interfaceac10;

        /// <summary>
        /// Looks for interfac10 value and accordingly sets the  boolean
        /// </summary>
        private bool isSelectDedupeOn = false;

        private bool isHttps;
               
        
        public  bool IsHttps
        {
            get
            {
                return isHttps;
            }
        }

        public bool IsGroupAdministrator
        {
            get
            {
                return isGroupAdministrator;
            }
        }

        public bool IsAccountAdministrator
        {
            get
            {
                return isAccountAdministrator;
            }
        }

        public bool IsMembershipAdmin
        {
            get
            {
                return isMembershipAdmin;
            }
        }

        /// <summary>
        /// Determines whether the user is on an old firewall which doesn't support ajax
        /// </summary>
        public bool IsAjaxEnabled
        {
            get
            {
                return isAjaxEnabled;
            }
        }


        /// <summary>
        /// Determines whether the user is a DMM user
        /// </summary>
        public bool IsDMMUser
        {
            get
            {
                return isDMMUser;
            }
        }

        /// <summary>
        /// Determines whether the user is a Select-FullArticle user
        /// </summary>
        public bool IsSelectFullUser
        {
            get
            {
                return isSelectFullUser;
            }
        }

        /// <summary>
        /// Determines whether the user is a Select-Headlines user
        /// </summary>
        public bool IsSelectHeadlinesUser
        {
            get
            {
                return isSelectHeadlinesUser;
            }
        }

        /// <summary>
        /// Determines whether the user is a Alerts user
        /// </summary>
        public bool IsAlertsUser
        {
            get
            {
                return isAlertsUser;
            }
        }

        /// <summary>
        /// Determines whether the user is a track administrator
        /// </summary>
        public bool IsTrackAdministrator
        {
            get
            {
                return isTrackAdministrator;
            }
        }

        /// <summary>
        /// Gets the maximum number of items in the specified class
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public int GetMaxItems(PreferenceClassID classId)
        {
            if (ac6.Contains(classId))
            {
                return (int)ac6[classId];
            }
            else
            {
                return 25;
            }
        }

        /// <summary>
        /// Gets the RuleSet
        /// </summary>
        public RuleSet RuleSet
        {
            get
            {
                return ruleSet;
            }
        }

        public bool IsUsageReport
        {
            get
            {
                return isUsageReport;
            }
        }

        public bool IsUiInfo
        {
            get
            {
                return isUiInfo;
            }
        }

        public bool IsNewsViews
        {
            get
            {
                return isNewsViews;
            }
        }

        public int MaxFolderForGlobal
        {
            get
            {
                return GetNumberOfTrackFolders("G");
            }
        }

        public int MaxFolderForSelectHeadline
        {
            get
            {
                return GetNumberOfTrackFolders("H");
            }
        }

        public int MaxFolderForSelectFullText
        {
            get
            {
                return GetNumberOfTrackFolders("T");
            }
        }

        public int MaxFolderForFastTrack
        {
            get
            {
                return GetNumberOfTrackFolders("F");
            }
        }

        public int MaxFolderForFcpAndIwe
        {
            get
            {
                return GetNumberOfTrackFolders("P");
            }
        }

        public int MaxFolderForDmm
        {
            get
            {
                return GetNumberOfTrackFolders("M");
            }
        }

        public bool CanGetUsageReport
        {
            get
            {
                return canGetUsageReport;
            }
        }

        public bool CanMakeFolderSubscribable
        {
            get
            {
                return canMakeFolderSubscribable;
            }
        }

        public bool CanCreateFactivaRootGroupItem
        {
            get
            {
                return canCreateFactivaRootGroupItem;
            }
        }


        public bool IsNewsDigest
        {
            get
            {
                return isNewsDigest;
            }
        }

        public bool IsHistoricalQuote
        {
            get
            {
                return isHistoricalQuote;
            }
        }

        public bool IsTreadlineQuote
        {
            get
            {
                return isTreadlineQuote;
            }
        }

        public bool IsReuterInvestorQuote
        {
            get
            {
                return isReuterInvestorQuote;
            }
        }

        public bool IsCurrentQuote
        {
            get
            {
                return isCurrentQuote;
            }
        }

        public bool IsQuickQuote
        {
            get
            {
                return isQuickQuote;
            }
        }

        public bool IsFullQuote
        {
            get
            {
                return isFullQuote;
            }
        }


        public bool IsFceNavigation
        {
            get
            {
                return isFceNavigation;
            }
        }

        public bool IsSalesWorksNavigation
        {
            get
            {
                return isSalesWorksNavigation;
            }
        }

        public bool IsSmbNavigation
        {
            get
            {
                return isSmbNavigation;
            }
        }

        public bool IsCreditcardUser
        {
            get
            {
                return isCreditcardUser;
            }
        }

        public bool IsCorporateUser
        {
            get
            {
                return isCorporateUser;
            }
        }

        public bool IsCustomerServiceUser
        {
            get
            {
                return isCustomerServiceUser;
            }
        }

        public bool IsIndividualUser
        {
            get
            {
                return isIndividualUser;
            }
        }

        public bool CanSwitchUser
        {
            get
            {
                return canSwitchUser;
            }
        }

        public bool IsRedirects
        {
            get
            {
                return isRedirects;
            }
        }

        public bool IsManageRedirects
        {
            get
            {
                return isManageRedirects;
            }
        }

        public bool IsThomson
        {
            get
            {
                return isThomson;
            }
        }

        public bool IsBvd
        {
            get
            {
                return isBvd;
            }
        }

        public bool Is508User
        {
            get
            {
                return is508User;
            }
        }

        public bool IsBackgroundDataFromReuters
        {
            get
            {
                return isBackgroundDataFromReuters;
            }
        }

        public bool IsDnb
        {
            get
            {
                return isDNB;
            }
        }

        public bool IsDnbSnapshotOnly
        {
            get
            {
                return isDNBSnapshotOnly;
            }
        }

        public bool IsDnbReportsWithFactivaBill
        {
            get
            {
                return isDNBReportsWithFactivaBill;
            }
        }

        public bool IsDnbReportsWithCreditcard
        {
            get
            {
                return isDNBReportsWithCreditcard;
            }
        }

        public int MaximumFoldersPerEmailSetup
        {
            get
            {
                return maximumFoldersPerEmailSetup;
            }
        }

        public int MaximumPersonalFoldersEmailDelivery
        {
            get
            {
                return maximumPersonalFoldersEmailDelivery;
            }
        }

        public string UserId
        {
            get
            {
                return userId;
            }
        }

        public string AccountId
        {
            get
            {
                return accountId;
            }
        }

        public string ProductId
        {
            get
            {
                return productId;
            }
        }

        public bool IsFcpLinkInDotCom
        {
            get
            {
                return isFcpLinkInDotCom;

            }
        }

        public int MaximumExtendedContentSearchInMonth
        {
            get
            {
                return maximumExtendedContentSearchInMonth;
            }
        }

        public string EmailLoginStatus
        {
            get
            {
                return emailLoginStatus;
            }
        }

        public string AccountAuthOption
        {
            get
            {
                return accountAuthOption;
            }
        }

        public string ArchiveAc3
        {
            get
            {
                return archiveAc3;
            }
        }
        public string ArchiveAc2
        {
            get
            {
                return archiveAc2;
            }
        }


        public string ArchiveAc9
        {
            get
            {
                return archiveAc9;
            }
        }

        public bool IsNewsLetterUser
        {
            get
            {
                return isNewsLetterUser;
            }
        }

        public bool IsSharing
        {
            get
            {
                return isSharing;
            }
        }
        public bool IsMultimedia
        {
            get
            {
                return isMultimedia;
            }
        }

        public bool IsTimeToLiveToken
        {
            get
            {
                return isTimeToLiveToken;
            }
        }
        public bool IsPermittedReaderView
        {
            get
            {
                return isPermittedReaderView;
            }
        }

        public bool IsExternalReader
        {
            get
            {
                return isExternalReader;
            }
        }

        public bool IsDowJonesInsider
        {
            get
            {
                return isDowJonesInsider;
            }
        }

        public bool IsInsightUser
        {
            get
            {
                return isInsightUser;
            }
        }

        public bool IsPersonalization
        {
            get
            {
                return isPersonalization;
            }
        }

        public bool IsPodcastability
        {
            get
            {
                return isPodcastability;
            }
        }

        public bool IsProjectVisibleAdsOn
        {
            get
            {
                return isProjectVisibleAdsOn;
            }
        }

        public bool IsPremiumSnapshotOn
        {
            get
            {
                return isPremiumSnapshotOn;
            }
        }

        public bool IsRICForHistoricalOn
        {
            get
            {
                return isRICForHistoricalOn;
            }
        }

        public bool IsResearchCenterOn
        {
            get
            {
                return isResearchCenterOn;
            }
        }


        public bool IsSalesWorkLiteNavigation
        {
            get
            {
                return isSalesWorkLiteNavigation;
            }
        }

        public bool IsSearchAssistanceOn
        {
            get
            {
                return isSearchAssistanceOn;
            }
        }

        public bool IsRecentCompanySearches
        {
            get
            {
                return isRecentCompanySearches;
            }
        }

        public bool IsTrackCoreServiceOn
        {
            get
            {
                return isTrackCoreServiceOn;
            }
        }

        /// <summary>
        /// Interface AC10 - used to decide if the dedup options can be displayed for Track Select Folders(only)
        /// </summary>
        /// <returns></returns>
        public string InterfaceAC10
        {
            get
            {
                return interfaceac10;
            }
        }
        
        public bool IsSelectDedupeOn
        {
            get
            {
                return isSelectDedupeOn;
            }
        }

        public string SalesworksPartner
        {
            get
            {
                return salesworksPartner;
            }
        }
        /// <summary>
        /// Controls of the user can see the Set Mobile cookie link in dotcom tools. based on offset 18 in the rule set.
        /// </summary>
        public bool IsSetMobileCookieOn
        {
            get
            {
                return isSetMobileCookieOn;
            }
            
        }
        
        /// <summary>
        /// loads the UserAuthorizationResponse into the cache if it is not loaded
        /// </summary>
        /// <returns></returns>
        internal void Load(GetUserAuthorizationsResponse getUserAuthorizationsResponse)
        {
            // Only allow one to be set per instance;
            if (getUserAuthorizationsResponse != null && userAuthorizationResponseCache == null)
                {
                    userAuthorizationResponseCache = getUserAuthorizationsResponse;

                    isGroupAdministrator = getUserAuthorizationsResponse.administratorFlag == AdministratorFlag.GroupAdministrator;
                    isAccountAdministrator = getUserAuthorizationsResponse.administratorFlag == AdministratorFlag.AccountAdministrator;
                    isMembershipAdmin = (isAccountAdministrator || isGroupAdministrator);
                    userId = getUserAuthorizationsResponse.UserId;
                    accountId = getUserAuthorizationsResponse.AccountId;
                    productId = getUserAuthorizationsResponse.ProductId;

                    // Parse membership.ac6
                    string ac6Serialized = "<AC6>" + getUserAuthorizationsResponse.AuthorizationMatrix.membership.ac6 + "</AC6>";
                    ac6.Clear();
                    XmlSerializer ac6Serializer = new XmlSerializer(typeof (AC6));
                    AC6 _ac6 = (AC6) ac6Serializer.Deserialize(new StringReader(ac6Serialized));
                    if (_ac6.ITEM != null)
                    {
                        foreach (AC6ITEM item in _ac6.ITEM)
                        {
                            ac6.Add((PreferenceClassID) item.@class, item.count);
                        }
                    }

                    // parse track.ac7
                    string ac7 = getUserAuthorizationsResponse.AuthorizationMatrix.track.ac7;
                    if (ac7 == "-1")
                    {
                        isTrackAdministrator = true;
                    }

                    string da7 = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.da7;
                    if (da7 == "1")
                    {
                        isAjaxEnabled = false;
                    }
                    else
                    {
                        isAjaxEnabled = true;
                    }
                    ac7 = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.ac7;
                    isPodcastability = (ac7 != null && ac7.Trim().ToUpper() == "ON");


                    string da5 = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.da5;
                    if (da5 != null && da5.Trim().Length > 0)
                    {
                        maximumExtendedContentSearchInMonth = int.Parse(da5);
                    }


                    // parse track.ac8
                    string ac8 = getUserAuthorizationsResponse.AuthorizationMatrix.track.ac8;
                    if (ac8 == null)
                    {
                        ac8 = string.Empty;
                    }
                    isDMMUser = ac8.ToLower().StartsWith("m");
                    isSelectFullUser = ac8.ToLower().StartsWith("t");
                    isSelectHeadlinesUser = ac8.ToLower().StartsWith("h");
                    isAlertsUser = ac8.ToLower().StartsWith("f");

                    // parse ruleset
                    string sessionRuleSet = getUserAuthorizationsResponse.RuleSet;
                    ruleSet = new RuleSet(sessionRuleSet);

                    AuthorizationMatrix matrix = getUserAuthorizationsResponse.AuthorizationMatrix;
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
                    string trackAc3 = null;
                    bool isAcademicUser = false;
                    if (matrix != null)
                    {
                        if (matrix.cibs != null)
                        {
                            isUsageReport = matrix.cibs.ac1 != "N";
                            cibsAc2 = matrix.cibs.ac2;
                        }
                        if (matrix.mds != null)
                        {
                            mdsAc1 = matrix.mds.ac1;
                            mdsAc2 = matrix.mds.ac2;
                            mdsAc3 = matrix.mds.ac3;
                        }

                        if (matrix.membership != null)
                        {
                            canMakeFolderSubscribable = (matrix.membership.ac1 == "A");
                            membershipAc2 = matrix.membership.ac2;
                            membershipAc3 = matrix.membership.ac3;
                            membershipAc4 = matrix.membership.ac4;
                        }
                        if (matrix.@interface != null)
                        {
                            isUiInfo = (matrix.@interface.ac1 == "W" || matrix.@interface.ac1 == "R");
                            is508User = matrix.@interface.ac2 == "Y";
                            isFceNavigation = matrix.@interface.ac3 == "4";
                            isSalesWorksNavigation = matrix.@interface.ac3 == "3";
                            isSmbNavigation = matrix.@interface.ac3 == "5";
                            isSalesWorkLiteNavigation = matrix.@interface.ac3 == "6";
                            isPremiumSnapshotOn = matrix.@interface.ac6 == "PREMIUM";
                            if (matrix.@interface.ac9 != null && matrix.@interface.ac9.Trim() == "A")
                            {
                                isFcpLinkInDotCom = false;
                                isAcademicUser = true;
                            }
                            if (matrix.@interface.insider != null && matrix.@interface.insider.Trim() == "OFF")
                            {
                                isDowJonesInsider = false;
                            }
                        }
                        if (matrix.uer != null)
                        {
                            uerAc1 = matrix.uer.ac1;
                            uerAc2 = matrix.uer.ac2;
                        }
                        if (matrix.email != null)
                        {
                            emailAc3 = matrix.email.ac3;
                        }
                        if (matrix.track != null)
                        {
                            trackAc3 = matrix.track.ac3;
                        }
                        if (matrix.archive != null)
                        {
                            archiveAc3 = matrix.archive.ac3;
                            archiveAc2 = matrix.archive.ac2;
                            archiveAc9 = matrix.archive.ac9;
                        }
                        isNewsDigest = (matrix.email != null && matrix.email.ac1 == "Y");
                    }
                    canCreateFactivaRootGroupItem = (membershipAc4 == "W");
                    canGetUsageReport = (cibsAc2 == "Y");
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
                    isHistoricalQuote = (mdsAc2 != "OFF");
                    if (mdsAc2.IndexOf("TLINE") != -1)
                    {
                        isTreadlineQuote = true;
                        isReuterInvestorQuote = true;
                    }
                    else if (mdsAc2.IndexOf("RIVST") != -1)
                    {
                        isReuterInvestorQuote = true;
                    }
                    isCurrentQuote = (mdsAc1 != "OFF");
                    isQuickQuote = (mdsAc1 == "QUICK");
                    isFullQuote = (mdsAc1 == "FULL");

                    isCreditcardUser = getUserAuthorizationsResponse.UserType == UserType.Creditcard;
                    isCorporateUser = getUserAuthorizationsResponse.UserType == UserType.Corporate;
                    isCustomerServiceUser = getUserAuthorizationsResponse.UserType == UserType.CustomerService;
                    isIndividualUser = getUserAuthorizationsResponse.UserType == UserType.Individual;

                    canSwitchUser = membershipAc3 == "Y";
                    isRedirects = membershipAc2 != null && membershipAc2.ToUpper().IndexOf("R") != -1;
                    isManageRedirects = membershipAc2 != null && membershipAc2.ToUpper().IndexOf("W") != -1;

                    if (uerAc1 == null)
                    {
                        uerAc1 = String.Empty;
                    }
                    if (uerAc2 == null)
                    {
                        uerAc2 = String.Empty;
                    }
                    isThomson = uerAc2.ToUpper().IndexOf("SEC=1") != -1;
                    isBvd = uerAc2.ToUpper().IndexOf("BVD=1") != -1;
                    isBackgroundDataFromReuters = (mdsAc3 != "OFF");
                    isDNB = uerAc1 != "0";
                    isDNBSnapshotOnly = uerAc1 == "1";
                    isDNBReportsWithFactivaBill = uerAc1 == "3";
                    isDNBReportsWithCreditcard = (uerAc1 == "" || uerAc1 == "2");

                    if (emailAc3 != null && emailAc3.Trim().Length > 0)
                    {
                        maximumFoldersPerEmailSetup = int.Parse(emailAc3.Trim());
                    }
                    if (trackAc3 == null)
                    {
                        maximumPersonalFoldersEmailDelivery = 50;
                    }
                    else if (trackAc3.Trim().Length > 0)
                    {
                        maximumPersonalFoldersEmailDelivery = int.Parse(trackAc3.Trim());
                    }
                    emailLoginStatus = getUserAuthorizationsResponse.emailLogin;
                    accountAuthOption = getUserAuthorizationsResponse.lwrFlag;
                    //TODO..

                    isNewsLetterUser = true;
                    string personalization = getUserAuthorizationsResponse.AuthorizationMatrix.membership.personalization;
                    if (personalization == null)
                    {
                        personalization = "";
                    }
                    //if not off then On, even if blank
                    if (getUserAuthorizationsResponse.AuthorizationMatrix.@interface.ac5 != null
                        && getUserAuthorizationsResponse.AuthorizationMatrix.@interface.ac5.Trim().ToUpper() == "OFF")
                    {
                        isInterfaceAC5On = false;
                    }

                    isPersonalization = personalization.Trim().ToUpper() != "OFF";
                    if (
                        /*!ruleSet.IsDotComBriefcaseDisplayServiceOn ||*/ //sm 1/8/09 and adde the isInterfaceAC5On
                        !isInterfaceAC5On ||
                        isSelectFullUser ||
                        isSelectHeadlinesUser ||
                        (
                            (!isMembershipAdmin)
                            &&
                            (
                                isAcademicUser
                                || isCreditcardUser
                                || !isPersonalization
                            )
                        )
                        )
                    {
                        isNewsLetterUser = false;
                    }
                    isSharing = true;
                    string sharingDa = getUserAuthorizationsResponse.AuthorizationMatrix.membership.sharing;
                    if (sharingDa == null)
                    {
                        sharingDa = "";
                    }
                    ac8 = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.ac8;
                    if (ac8 == null)
                    {
                        ac8 = "";
                    }
                    isPermittedReaderView = ac8.Trim().ToUpper() == "PRV";
                    if (isAcademicUser || personalization.Trim().ToUpper() == "OFF" || sharingDa.Trim().ToUpper() == "OFF" || isPermittedReaderView)
                    {
                        isSharing = false;
                    }
                    isMultimedia = true;
                    string valueHolder;
                    //                    Removing check for content blocking - NP 
                    //                    valueHolder = userAuthorizationResponseCache.authorizationMatrix.membership.multimedia;
                    //                    if (valueHolder != null && valueHolder.Trim().ToUpper() == "OFF")
                    //                    {
                    //                        isMultimedia = false;
                    //                    }
                    valueHolder = getUserAuthorizationsResponse.AuthorizationMatrix.membership.newsletterDA;
                    if (valueHolder != null && valueHolder.Trim().ToUpper() == "TTLT")
                    {
                        isTimeToLiveToken = true;
                    }
                    isExternalReader = (getUserAuthorizationsResponse.erFlag == "Y");

                    isInsightUser = ruleSet.IsInsightUser;

                    string projectVisibleAdsOnDA = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.ads;
                    ;
                    if (projectVisibleAdsOnDA != null && projectVisibleAdsOnDA.Trim().ToUpper() == "OFF")
                    {
                        isProjectVisibleAdsOn = false;
                    }

                    if (archiveAc9 != null && archiveAc9.ToUpper() == "RIC=ON")
                    {
                        isRICForHistoricalOn = true;
                    }

                    string researchCenterDA = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.rCenter;
                    if (researchCenterDA != null && researchCenterDA.Trim().ToUpper() == "ON")
                    {
                        isResearchCenterOn = true;
                    }

                    string searchAssistanceDA = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.searchAssist;
                    if (searchAssistanceDA != null && searchAssistanceDA.Trim().ToUpper() == "ON")
                    {
                        isSearchAssistanceOn = true;
                    }
                    //AC4 can contain "ssl" or "rcs" or both separated by ',' (HTTPS or RecentCompanySearches)
                    valueHolder = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.ac4;
                    if (!String.IsNullOrEmpty(valueHolder) && valueHolder.IndexOf("rcs") != -1)
                    {
                        isRecentCompanySearches = false;
                    }

                    if (!String.IsNullOrEmpty(valueHolder) && valueHolder.ToLower().IndexOf("ssl") != -1)
                    {
                        isHttps = true;
                    }

                    isTrackCoreServiceOn = ruleSet.IsTrackCoreServiceOn;
                    //TODO: SM_this has to change once we have the correct ac value;;;
                    if (matrix.@interface != null && matrix.@interface.ac10 != null)
                    {
                        interfaceac10 = matrix.@interface.ac10;
                    }
                    if (interfaceac10 != null && interfaceac10.Trim().ToUpper() == "ON")
                    {
                        isSelectDedupeOn = true;
                    }

                    NewsViewsPermissions _newsViewsPermissions =
                        new NewsViewsPermissions(getUserAuthorizationsResponse);

                    isNewsViews = _newsViewsPermissions.IsNewsViewsAdministratorOn;

                    string salesworksPartnerDA = getUserAuthorizationsResponse.AuthorizationMatrix.@interface.salesworksPartner;
                    if (!String.IsNullOrEmpty(salesworksPartnerDA))
                    {
                        salesworksPartner = salesworksPartnerDA;
                    }
                    isSetMobileCookieOn = ruleSet.IsSetMobileCookieOn;
                }
        }

        private int GetNumberOfTrackFolders(string productCode)
        {
            if (!ruleSet.IsTrackCoreServiceOn)
            {
                return 0;
            }
            string ac4 = null;
            if (userAuthorizationResponseCache.AuthorizationMatrix != null
                && userAuthorizationResponseCache.AuthorizationMatrix.track != null)
            {
                ac4 = userAuthorizationResponseCache.AuthorizationMatrix.track.ac4;
            }
            if (ac4 == null)
            {
                ac4 = String.Empty;
            }
            if (ac4.Trim() == "")
            {
                return 25;
            }
            if (StringUtilitiesManger.IsNumeric(ac4))
            {
                return int.Parse(ac4);
            }

            //Parse ac4
            string number = null;
            string[] parts = ac4.Split(new char[] { ';' });
            foreach (string part in parts)
            {
                if (part.IndexOf(productCode + ":") != -1)
                {
                    number = part.Split(new char[] { ':' })[1];
                }
            }
            if (number == null)
            {
                number = parts[0];
            }
            if (StringUtilitiesManger.IsNumeric(number))
            {
                return int.Parse(number);
            }
            return 0;
        }

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

    /// <remarks/>
    internal class NewsViewsPermissions
    {
        private readonly bool bAccess;
        private readonly string strNewsViewsAdmin;
        private readonly AuthorizationMatrix authorization;
        private readonly static Regex regex_NVPermission = new Regex(@"I\s*c=""(?<x>\d{3})""\s*g=""(?<y>\d*)", RegexOptions.Compiled);

        /// <remarks/>
        public NewsViewsPermissions(GetUserAuthorizationsResponse auth)
        {
            authorization = auth.AuthorizationMatrix;
            string strGroupNewsViews = authorization.membership.ac5;
            strNewsViewsAdmin = authorization.membership.gripAdmin;
            string strGripDefault = authorization.membership.gripDefault;
            if (strGroupNewsViews == null || strGroupNewsViews.Length == 0)
            {
                // if AC5 value doesn't exist then use Grip Default values.
                if (strGripDefault != null && strGripDefault.Length > 0)
                {
                    bAccess = CheckNewsViewsPermissions(strGripDefault);
                }
            }
            else
            {
                bAccess = CheckNewsViewsPermissions(strGroupNewsViews);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorization"></param>
        public NewsViewsPermissions(AuthorizationMatrix authorization)
        {
            string strGroupNewsViews = authorization.membership.ac5;
            strNewsViewsAdmin = authorization.membership.gripAdmin;
            string strGripDefault = authorization.membership.gripDefault;
            if (strGroupNewsViews == null || strGroupNewsViews.Length == 0)
            {
                // if AC5 value doesn't exist then use Grip Default values.
                if (strGripDefault != null && strGripDefault.Length > 0)
                {
                    bAccess = CheckNewsViewsPermissions(strGripDefault);
                }
            }
            else
            {
                bAccess = CheckNewsViewsPermissions(strGroupNewsViews);
            }
        }

        private static bool CheckNewsViewsPermissions(string strAccess)
        {
            bool bFlag = false;
            Match m = regex_NVPermission.Match(strAccess);
            while (m.Success)
            {
                string strClassID = m.Groups["x"].ToString();
                string strCount = m.Groups["y"].ToString();
                // Class ID = 139 is the class ID for NewsViews class.
                if (strClassID.CompareTo("139") == 0)
                {
                    if (Convert.ToInt32(strCount) > 0)
                    {
                        bFlag = true;
                    }
                }
                m = m.NextMatch();
            }
            return bFlag;
        }

        /// <remarks/>
        public bool IsNewsViewsRenderOn
        {
            get { return bAccess; }
        }

        /// <remarks/>
        public bool IsNewsViewsAdministratorOn
        {
            get
            {
                return bAccess && strNewsViewsAdmin == "Y";
            }
        }

        
    }

}
