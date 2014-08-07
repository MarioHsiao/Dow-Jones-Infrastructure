// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The interface service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The interface service.
    /// </summary>
    public class InterfaceService : AbstractService, IInterfaceService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixInterfaceService _matrixInterfaceService;
        private readonly MatrixProductXService _matrixProductxService;

        private readonly Dictionary<string, string> _authComponents;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceService"/> class.
        /// </summary>
        internal InterfaceService(bool isInterfaceServiceOn, MatrixInterfaceService matrixInterfaceService,MatrixProductXService matrixProductxService, Dictionary<string, string> authComponents)
        {
            _isOn = isInterfaceServiceOn;
            _matrixInterfaceService = matrixInterfaceService;
            _matrixProductxService = matrixProductxService;
            _authComponents = authComponents;
            Initialize();
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

        public bool IsWMSegmentationBrandOverride { get; private set; }

        public bool IsWMSegmentationPrdCO { get; private set; }

        public bool IsWMSegmentationPrdFAE { get; private set; }

        public bool IsWMSegmentationPrdFA { get; private set; }

        public bool IsFcpLinkInDotCom { get; private set; }

        public bool IsAcademicUser { get; private set; }

        public bool IsDowJonesInsider { get; private set; }

        public bool IsMyDowJonesFactiva { get; private set; }

        public bool IsMctUser { get; private set; }

        public bool IsAllowMobileCookieEmailLinkDA { get; private set; }

        public bool IsDULinkEnabled { get; private set; }

        public bool IsTranslateArticleAllowed { get; private set; }

        public bool IsPageMonitoring { get; private set; }

        public bool IsSubDomainingStaticResources { get; private set; }

        public bool IsBynoteMonitoring { get; private set; }

        public bool IsInterfaceAC5On { get; private set; }

        public bool IsPermittedReaderView { get; private set; }

        public string Personalization { get; private set; }

        public string SharingDA { get; private set; }

        public bool IsSharing { get; private set; }

        public bool IsProjectVisibleAdsOn { get; private set; }

        public bool IsResearchCenterOn { get; private set; }

        public bool IsCvdLinkOn { get; private set; }

        public bool IsSearchAssistanceOn { get; private set; }

        public bool IsSs_promotion_popupOn { get; private set; }

        public bool IsBlogDAOn { get; private set; }

        public bool IsRecentCompanySearches { get; private set; }

        public bool IsSelectDedupeOn { get; private set; }

        public string SalesworksPartner { get; private set; }

        public bool IsTestExecMarkupInArticleUser { get; private set; }

        public bool IsDowJonesTabEnabled { get; private set; }

        public bool IsDJRC { get; private set; }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            //TODO: Verify these logic
            if (_matrixInterfaceService == null)
            {
                return;
            }

            string ac10 = _matrixInterfaceService.ac10.FirstOrDefault(x => x.HasValue());
            if (!string.IsNullOrEmpty(ac10) && !string.IsNullOrEmpty(ac10.Trim()))
            {
                ShowDeduplicationControlsForSelectFolders = ac10.Trim().ToUpper() == "ON";
            }

            //AC's
            IsUiInfo = _matrixInterfaceService.ac1.ContainsAtAnyIndex("W") ||
                       _matrixInterfaceService.ac1.ContainsAtAnyIndex("R");
            Is508User = _matrixInterfaceService.ac2.ContainsAtAnyIndex("Y");
            IsFceNavigation = _matrixInterfaceService.ac3.ContainsAtAnyIndex("4") ||
                              _matrixInterfaceService.ac3.ContainsAtAnyIndex("3");

            IsSalesWorksNavigation = false;
            IsSmbNavigation = _matrixInterfaceService.ac3.ContainsAtAnyIndex("5");
            IsSalesWorkLiteNavigation = _matrixInterfaceService.ac3.ContainsAtAnyIndex("6");
            IsDJCESalesUser = _matrixInterfaceService.ac3.ContainsAtAnyIndex("7");
            IsEnhancedDJCEUser = _matrixInterfaceService.ac3.ContainsAtAnyIndex("8");
            var rcs = _matrixInterfaceService.ac4.FirstOrDefault(x => x.HasValue());
            if(!string.IsNullOrEmpty(rcs))
            IsRecentCompanySearches = rcs.IndexOf("rcs") == -1;
            IsInterfaceAC5On = !_matrixInterfaceService.ac5.ContainsAtAnyIndex("OFF");
            IsWMSegmentationPrdCO = _matrixInterfaceService.ac6.ContainsAtAnyIndex("ADVISOR-CO");
            IsWMSegmentationPrdFAE = _matrixInterfaceService.ac6.ContainsAtAnyIndex("ADVISOR-FAE") ||
                                     _matrixInterfaceService.ac6.ContainsAtAnyIndex("ADVISOR");
            IsWMSegmentationPrdFA = _matrixInterfaceService.ac6.ContainsAtAnyIndex("ADVISOR-FA");
            if (IsWMSegmentationPrdCO || IsWMSegmentationPrdFAE || IsWMSegmentationPrdFA)
            {
                IsWMSegmentationBrandOverride = true;
            }
            IsPodcastability = _matrixInterfaceService.ac7.ContainsAtAnyIndex("ON");
            IsPermittedReaderView = _matrixInterfaceService.ac8.ContainsAtAnyIndex("PRV");
            IsAcademicUser = _matrixInterfaceService.ac9.ContainsAtAnyIndex("A");
            IsFcpLinkInDotCom = !IsAcademicUser;
            Personalization = _matrixInterfaceService.personalization;
            SharingDA = _matrixInterfaceService.sharingDA;
            IsSharing = (IsAcademicUser || Personalization == "OFF" || SharingDA == "OFF" || IsPermittedReaderView);
            IsSelectDedupeOn = _matrixInterfaceService.ac10.ContainsAtAnyIndex("ON");


            //DA's
            if (_matrixInterfaceService.da5 != null && _matrixInterfaceService.da5.Trim().Length > 0)
            {
                MaximumExtendedContentSearchInMonth = int.Parse(_matrixInterfaceService.da5);
            }
            IsAjaxEnabled = _matrixInterfaceService.da7 != "1";
            IsDowJonesInsider = _matrixInterfaceService.insider != "OFF";
            IsMyDowJonesFactiva = _matrixInterfaceService.IsMyDJFactivaEnabled;
            IsMctUser = _matrixInterfaceService.ac5.Contains("MCT");
            IsAllowMobileCookieEmailLinkDA = _matrixInterfaceService.mcemail != "OFF";
            IsDULinkEnabled = _matrixInterfaceService.duLink != "OFF";
            IsTranslateArticleAllowed = _matrixInterfaceService.IsTranslateDAEnabled;
            //TODO: NO pageMonitor IN MATRIXINTERFACESERVICE
            //TODO: NO subDomianing IN MATRIXINTERFACESERVICE
            //TODO: NO baynote IN MATRIXINTERFACESERVICE
            IsProjectVisibleAdsOn = _matrixInterfaceService.ads != "OFF";
            IsResearchCenterOn = _matrixInterfaceService.rCenter == "ON";
            IsCvdLinkOn = _matrixInterfaceService.CVD;
            IsSearchAssistanceOn = _matrixInterfaceService.searchAssist != "OFF";
            //TODO: NO ss_promotion_popup IN MATRIXINTERFACESERVICE
            //TODO: NO blogDA IN MATRIXINTERFACESERVICE
            SalesworksPartner = _matrixInterfaceService.salesworksPartner;
            //TODO: NO execDQTest IN MATRIXINTERFACESERVICE

            string strValue = null;
            if (_authComponents.ContainsKey("DJTAB")){
                strValue = _authComponents["DJTAB"];
            }
            IsDowJonesTabEnabled = ((string.IsNullOrEmpty(strValue)) || string.Equals(strValue.Trim(), "ON", StringComparison.InvariantCultureIgnoreCase));

            //Check DJRC 
            IsDJRC = IsPermittedReaderView && (_matrixProductxService != null && _matrixProductxService.ac2 != null && _matrixProductxService.ac2.ContainsAtAnyIndex("REG=DJRC", StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion


        
    }
}