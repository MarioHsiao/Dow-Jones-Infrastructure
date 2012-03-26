// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The interface service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Web.EntitlementPrinciple.Security.Interfaces;
using DowJones.Web.EntitlementPrinciple.Utility;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
{
    /// <summary>
    /// The interface service.
    /// </summary>
    public class InterfaceService : AbstractService, IInterfaceService
    {
        #region Private Variables

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixInterfaceService _matrixInterfaceService;

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceService"/> class.
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <param name="matrixInterfaceService"></param>
        internal InterfaceService(bool isInterfaceServiceOn, MatrixInterfaceService matrixInterfaceService)
        {
            _isOn = isInterfaceServiceOn;
            _matrixInterfaceService = matrixInterfaceService;
            Initialize();
            //financialMatrixFsService = authorizationMatrix.FinacialServiceInterface;
            //DJCESubService = new DJCESubService(authorizationMatrix.Interface);
            //SegmentationSubService = new SegmentationSubService(authorizationMatrix.FinacialServiceInterface);
            
            
        }

        #endregion

        #region Public Properties

        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether IsOn.
        /// </summary>
        public override bool IsOn
        {
            get { return _isOn; }
        }

        /// <summary>
        /// Gets a value indicating whether HasOffset.
        /// </summary>
        public override bool HasOffset
        {
            get { return true; }
        }

        /// <summary>
        /// Gets Offset.
        /// </summary>
        public override int Offset
        {
            //TODO: Need to verfity 
            get { return 114; }
        }

        #endregion

        public bool IsAjaxEnabled { get; private set; }
        
        public bool IsPodcastability { get; private set; }

        public int MaximumExtendedContentSearchInMonth { get; private set; }

        public bool ShowDeduplicationControlsForSelectFolders { get; private set; }

        public bool IsUiInfo { get; private set; }

        public bool Is508User { get; private set; }

        public bool IsFceNavigation { get; private set; }

        public bool IsSalesWorksNavigation { get; private set; }

        public bool IsSmbNavigation { get; private set; }

        public bool IsSalesWorkLiteNavigation { get; private set; }

        public bool IsDJCESalesUser { get; private set; }

        public bool IsEnhancedDJCEUser { get; private set; }

        public bool IsWMSegmentationBrandOverride{ get; private set; }

        public bool IsWMSegmentationPrdCO{ get; private set; }
        
        public bool IsWMSegmentationPrdFAE{ get; private set; }
        
        public bool IsWMSegmentationPrdFA{ get; private set; }
        
        public bool IsFcpLinkInDotCom{ get; private set; }
        
        public bool IsAcademicUser{ get; private set; }
        
        public bool IsDowJonesInsider{ get; private set; }
        
        public bool IsMyDowJonesFactiva{ get; private set; }
        
        public bool IsAllowMobileCookieEmailLinkDA{ get; private set; }
        
        public bool IsDULinkEnabled{ get; private set; }
        
        public bool IsTranslateArticleAllowed{ get; private set; }
        
        public bool IsPageMonitoring{ get; private set; }
        
        public bool IsSubDomainingStaticResources{ get; private set; }
        
        public bool IsBynoteMonitoring{ get; private set; }
        
        public bool IsInterfaceAC5On{ get; private set; }
        
        public bool IsPermittedReaderView{ get; private set; }

        public string Personalization { get; private set; }
        
        public string SharingDA { get; private set; }
        
        public bool IsSharing{ get; private set; }
        
        public bool IsProjectVisibleAdsOn{ get; private set; }
        
        public bool IsResearchCenterOn{ get; private set; }
        
        public bool IsCvdLinkOn{ get; private set; }
        
        public bool IsSearchAssistanceOn{ get; private set; }
        
        public bool IsSs_promotion_popupOn{ get; private set; }
        
        public bool IsBlogDAOn{ get; private set; }
        
        public bool IsRecentCompanySearches{ get; private set; }
        
        public bool IsSelectDedupeOn{ get; private set; }
        
        public string SalesworksPartner{ get; private set; }
        
        public bool IsTestExecMarkupInArticleUser { get; private set; }

        #endregion

        #region SubServices

        /// <summary>
        /// Gets the DJCE sub service.
        /// </summary>
        /// <value>The DJCE sub service.</value>
        //public DJCESubService DJCESubService { get; private set; }

        //public SegmentationSubService SegmentationSubService { get; private set; }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            //TODO: Verify these logic
            if (_matrixInterfaceService == null || _matrixInterfaceService.ac10 == null || _matrixInterfaceService.ac10.Count <= 0)
            {
                return;
            }

            string ac10 = ListUtility.FindAnyValue(_matrixInterfaceService.ac10);
            if (!string.IsNullOrEmpty(ac10) && !string.IsNullOrEmpty(ac10.Trim()))
            {
                ShowDeduplicationControlsForSelectFolders = ac10.Trim().ToUpper() == "ON";
            }

            //AC's
            IsUiInfo = ListUtility.FindInList(_matrixInterfaceService.ac1, "W") ||
                       ListUtility.FindInList(_matrixInterfaceService.ac1, "R");
            Is508User = ListUtility.FindInList(_matrixInterfaceService.ac2, "Y");
            IsFceNavigation = ListUtility.FindInList(_matrixInterfaceService.ac3, "4") ||
                              ListUtility.FindInList(_matrixInterfaceService.ac3, "3");
            
            IsSalesWorksNavigation = false;
            IsSmbNavigation = ListUtility.FindInList(_matrixInterfaceService.ac3, "5");
            IsSalesWorkLiteNavigation = ListUtility.FindInList(_matrixInterfaceService.ac3, "6");
            IsDJCESalesUser = ListUtility.FindInList(_matrixInterfaceService.ac3, "7");
            IsEnhancedDJCEUser = ListUtility.FindInList(_matrixInterfaceService.ac3, "8");
            string rcs = ListUtility.FindAnyValue(_matrixInterfaceService.ac4);
            IsRecentCompanySearches = rcs.IndexOf("rcs") == -1;
            IsInterfaceAC5On = !ListUtility.FindInList(_matrixInterfaceService.ac5, "OFF"); 
            IsWMSegmentationPrdCO = ListUtility.FindInList(_matrixInterfaceService.ac6, "ADVISOR-CO");
            IsWMSegmentationPrdFAE = ListUtility.FindInList(_matrixInterfaceService.ac6, "ADVISOR-FAE") || 
                                     ListUtility.FindInList(_matrixInterfaceService.ac6, "ADVISOR");
            IsWMSegmentationPrdFA = ListUtility.FindInList(_matrixInterfaceService.ac6, "ADVISOR-FA");
            if (IsWMSegmentationPrdCO || IsWMSegmentationPrdFAE || IsWMSegmentationPrdFA)
            {
                IsWMSegmentationBrandOverride = true;
            }
            IsPodcastability = ListUtility.FindInList(_matrixInterfaceService.ac7, "ON");
            IsPermittedReaderView = ListUtility.FindInList(_matrixInterfaceService.ac8, "PRV");
            IsAcademicUser = ListUtility.FindInList(_matrixInterfaceService.ac9, "A");
            IsFcpLinkInDotCom = !IsAcademicUser;
            Personalization = _matrixInterfaceService.personalization;
            SharingDA = _matrixInterfaceService.sharingDA;
            IsSharing = (IsAcademicUser || Personalization == "OFF" || SharingDA == "OFF" || IsPermittedReaderView);
            IsSelectDedupeOn = ListUtility.FindInList(_matrixInterfaceService.ac10, "ON");
            

            //DA's
            if (_matrixInterfaceService.da5 != null && _matrixInterfaceService.da5.Trim().Length > 0)
            {
                MaximumExtendedContentSearchInMonth = int.Parse(_matrixInterfaceService.da5);
            }
            IsAjaxEnabled = _matrixInterfaceService.da7 != "1";
            IsDowJonesInsider = _matrixInterfaceService.insider != "OFF";
            IsMyDowJonesFactiva = _matrixInterfaceService.IsMyDJFactivaEnabled;
            IsAllowMobileCookieEmailLinkDA = _matrixInterfaceService.mcemail != "OFF";
            //TODO: NO duLinkBuilder IN MATRIXINTERFACESERVICE
            //IsDULinkEnabled = _matrixInterfaceService.duLinkBuilder == "ON";
            IsTranslateArticleAllowed = _matrixInterfaceService.IsTranslateDAEnabled;
            //TODO: NO pageMonitor IN MATRIXINTERFACESERVICE
            //IsPageMonitoring = _matrixInterfaceService.pageMonitor == "ON";
            //TODO: NO subDomianing IN MATRIXINTERFACESERVICE
            //IsSubDomainingStaticResources = _matrixInterfaceService.subDomianing == "ON";
            //TODO: NO baynote IN MATRIXINTERFACESERVICE
            //IsBynoteMonitoring = _matrixInterfaceService.baynote == "ON";
            IsProjectVisibleAdsOn = _matrixInterfaceService.ads != "OFF";
            IsResearchCenterOn = _matrixInterfaceService.rCenter == "ON";
            IsCvdLinkOn = _matrixInterfaceService.CVD;
            IsSearchAssistanceOn = _matrixInterfaceService.searchAssist != "OFF";
            //TODO: NO ss_promotion_popup IN MATRIXINTERFACESERVICE
            //IsSs_promotion_popupOn = _matrixInterfaceService.ss_promotion_popupDA != "OFF";
            //TODO: NO blogDA IN MATRIXINTERFACESERVICE
            //IsBlogDAOn = _matrixInterfaceService.blogDA == "ON";
            SalesworksPartner = _matrixInterfaceService.salesworksPartner;
            //TODO: NO execDQTest IN MATRIXINTERFACESERVICE
            //IsTestExecMarkupInArticleUser = _matrixInterfaceService.execDQTest == "ON";

        }

        #endregion
    }
}