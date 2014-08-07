// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreServicesSubPrincipal.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CoreServicesSubPrincipal type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Security.Interfaces;
using DowJones.Security.Services;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using System.Linq;
using System.Collections.Generic;

namespace DowJones.Security.SubPrinciples
{
    /// <summary>
    /// Core Services Sub Principal
    /// </summary>
    public class CoreServicesSubPrinciple : ICoreServicesSubPrinciple
    {
        #region Private Variables

        private CIBsService _CIBsService;
        
        private FISPrivateMarketsService _FISPrivateMarketsService;

        private FSInterfaceService _FSInterfaceService;
        
        private RESTService _RESTService;
        
        private AffiliationService _affiliationService;

        private AlertsService _alertsService;

        private AnalyzeService _analyzeService;

        private ArchiveService _archiveService;
       
        private AuthorizationMatrix _authorizationMatrix;

        private ClientMatchService _clientMatchService;

        private CustomerValueDashboardService _customerValueDashboardService;

        private EmailService _emailService;

        private FamilyTreeService _familyTreeService;
       
        private FreeSearchService _freeSearchService;

        private InterfaceService _interfaceService;

        private MarketDataService _marketDataService;

        private MembershipService _membershipService;

        private NewsstandService _newsstandService;

        private NumericDataService _numericDataService;
       
        private PlatformAssetManagementService _platformAssetManagementService;

        private RelationshipMappingService _relationshipMappingService;
        
        private readonly IRuleSet _ruleSet;

        private ScreeningService _screeningService;

        private SearchService _searchService;
       
        private SessionService _sessionService;

        private SnapshotService _snapshotService;

        private SymbologyService _symbologyService;

        private TriggerRepositoryService _triggerRepositoryService;

        private UERService _UERService;

        private CommunicatorService _communicatorService;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreServicesSubPrincipal"/> class.
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <param name="authorizationMatrix"></param>
        internal CoreServicesSubPrinciple(IRuleSet ruleSet, AuthorizationMatrix authorizationMatrix)
        {
            _ruleSet = ruleSet;
            _authorizationMatrix = authorizationMatrix;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the affiliation service.
        /// </summary>
        /// <value>The affiliation service.</value>
        public IAffiliationService AffiliationService
        {
            get
            {
                InitAffiliationService();
                return _affiliationService;
            }
        }

        /// <summary>
        /// Gets the alerts service.
        /// </summary>
        /// <value>The alerts service.</value>
        public IAlertsService AlertsService
        {
            get
            {
                InitAlertsService();
                return _alertsService;
            }
        }

        /// <summary>
        /// Gets the analyze service.
        /// </summary>
        /// <value>The analyze service.</value>
        public IAnalyzeService AnalyzeService
        {
            get
            {
                InitAnalyzeService();
                return _analyzeService;
            }
        }

        /// <summary>
        /// Gets the archive service.
        /// </summary>
        /// <value>The archive service.</value>
        public IArchiveService ArchiveService
        {
            get
            {
                InitArchiveService();
                return _archiveService;
            }
        }

        /// <summary>
        /// Gets the CIB service.
        /// </summary>
        /// <value>The CIB service.</value>
        public ICIBsService CIBsService
        {
            get
            {
                InitCIBsService();
                return _CIBsService;
            }
        }

        /// <summary>
        /// Gets the client match service.
        /// </summary>
        /// <value>The client match service.</value>
        public IClientMatchService ClientMatchService
        {
            get
            {
                InitClientMatchService();
                return _clientMatchService;
            }
        }

        /// <summary>
        /// Gets the customer value dashboard service.
        /// </summary>
        /// <value>The customer value dashboard service.</value>
        public ICustomerValueDashboardService CustomerValueDashboardService
        {
            get
            {
                InitCustomerValueDashboardService();
                return _customerValueDashboardService;
            }
        }

        /// <summary>
        /// Gets the email service.
        /// </summary>
        /// <value>The email service.</value>
        public IEmailService EmailService
        {
            get
            {
                InitEmailService();
                return _emailService;
            }
        }

        /// <summary>
        /// Gets the family tree service.
        /// </summary>
        /// <value>The family tree service.</value>
        public IFamilyTreeService FamilyTreeService
        {
            get
            {
                InitFamilyTreeService();
                return _familyTreeService;
            }
        }

        /// <summary>
        /// Gets the FIS private markets service.
        /// </summary>
        /// <value>The FIS private markets service.</value>
        public IFISPrivateMarketsService FISPrivateMarketsService
        {
            get
            {
                InitFISPrivateMarketsService();
                return _FISPrivateMarketsService;
            }
        }

        /// <summary>
        /// Gets the FS Interface service.
        /// </summary>
        /// <value>The FS Interface service.</value>
        public IFSInterfaceService FSInterfaceService
        {
            get
            {
                InitFSInterfaceService();
                return _FSInterfaceService;
            }
        }


        /// <summary>
        /// Gets the interface service.
        /// </summary>
        /// <value>The interface service.</value>
        public IInterfaceService InterfaceService
        {
            get
            {
                InitInterfaceService();
                return _interfaceService;
            }
        }

        /// <summary>
        /// Gets the market data service.
        /// </summary>
        /// <value>The market data service.</value>
        public IMarketDataService MarketDataService
        {
            get
            {
                InitMarketDataService();
                return _marketDataService;
            }
        }

        /// <summary>
        /// Gets the membership service.
        /// </summary>
        /// <value>The membership service.</value>
        public IMembershipService MembershipService
        {
            get
            {
                InitMembershipService();
                return _membershipService;
            }
        }

        /// <summary>
        /// Gets the newsstand service.
        /// </summary>
        /// <value>The newsstand service.</value>
        public INewsstandService NewsstandService
        {
            get
            {
                InitNewsstandService();
                return _newsstandService;
            }
        }

        /// <summary>
        /// Gets the numeric data service.
        /// </summary>
        /// <value>The numeric data service.</value>
        public INumericDataService NumericDataService
        {
            get
            {
                InitNumericDataService();
                return _numericDataService;
            }
        }

        /// <summary>
        /// Gets the relationship mapping service.
        /// </summary>
        /// <value>The relationship mapping service.</value>
        public IRelationshipMappingService RelationshipMappingService
        {
            get
            {
                InitRelationshipMappingService();
                return _relationshipMappingService;
            }
        }

        /// <summary>
        /// Gets the REST service.
        /// </summary>
        /// <value>The REST service.</value>
        public IRESTService RESTService
        {
            get
            {
                InitRESTService();
                return _RESTService;
            }
        }

        /// <summary>
        /// Gets the screening service.
        /// </summary>
        /// <value>The screening service.</value>
        public IScreeningService ScreeningService
        {
            get
            {
                InitScreeningService();
                return _screeningService;
            }
        }

        /// <summary>
        /// Gets the search service.
        /// </summary>
        /// <value>The search service.</value>
        public ISearchService SearchService
        {
            get
            {
                InitSearchService();
                return _searchService;
            }
        }

        /// <summary>
        /// Gets the snapshot service.
        /// </summary>
        /// <value>The snapshot service.</value>
        public ISnapshotService SnapshotService
        {
            get
            {
                InitSnapshotService();
                return _snapshotService;
            }
        }

        /// <summary>
        /// Gets the symbology service.
        /// </summary>
        /// <value>The symbology service.</value>
        public ISymbologyService SymbologyService
        {
            get
            {
                InitSymbologyService();
                return _symbologyService;
            }
        }

        /// <summary>
        /// Gets the trigger repository service.
        /// </summary>
        /// <value>The trigger repository service.</value>
        public ITriggerRepositoryService TriggerRepositoryService
        {
            get
            {
                InitTriggerRepositoryService();
                return _triggerRepositoryService;
            }
        }

        /// <summary>
        /// Gets the session service.
        /// </summary>
        /// <value>The session service.</value>
        public ISessionService SessionService
        {
            get
            {
                InitSessionService();
                return _sessionService;
            }
        }

        /// <summary>
        /// Gets the platform asset management service.
        /// </summary>
        /// <value>The platform asset management service.</value>
        public IPlatformAssetManagementService PlatformAssetManagementService
        {
            get
            {
                InitPlatformAssetManagementService();
                return _platformAssetManagementService;
            }
        }

        /// <summary>
        /// Gets the core search service.
        /// </summary>
        /// <value>The core search service.</value>
        public IFreeSearchService FreeSearchService
        {
            get
            {
                InitFreeSearchService();
                return _freeSearchService;
            }
        }

        /// <summary>
        /// Gets the UER service.
        /// </summary>
        /// <value>The UER service.</value>
        public IUERService UERService
        {
            get
            {
                InitUERService();
                return _UERService;
            }
        }

        public ICommunicatorService CommunicatorService
        {
            get
            {
                InitCommunicatorService();
                return _communicatorService;
            }
        }

        #endregion

        #region Service Initialization Methods

        private void InitAffiliationService()
        {
            if (_affiliationService != null)
                return;

            _affiliationService = null;
        }

        private void InitAlertsService()
        {
            if (_alertsService != null)
            {
                return;
            }
            _alertsService = new AlertsService(_ruleSet.IsTrackCoreServiceOn, _authorizationMatrix.Track);
        }

        private void InitAnalyzeService()
        {
            if (_analyzeService != null)
            {
                return;
            }
            _analyzeService = null;
        }

        private void InitArchiveService()
        {
            if (_archiveService != null)
            {
                return;
            }
            _archiveService = new ArchiveService(_ruleSet.IsArchiveCoreServiceOn, _authorizationMatrix.Archive);
        }

        private void InitCIBsService()
        {
            if (_CIBsService != null)
            {
                return;
            }
            _CIBsService = new CIBsService(_ruleSet.IsCibsCoreServiceOn, _authorizationMatrix.Cibs);
        }

        private void InitClientMatchService()
        {
            if (_clientMatchService != null)
                return;

            _clientMatchService = null;
        }

        private void InitCustomerValueDashboardService()
        {
            if (_customerValueDashboardService != null)
                return;

            _customerValueDashboardService = null;
        }

        private void InitEmailService()
        {
            if (_emailService != null)
            {
                return;
            }
            _emailService = new EmailService(_ruleSet.IsEmailCoreServiceOn, _authorizationMatrix.Email);
        }

        private void InitFamilyTreeService()
        {
            if (_familyTreeService != null)
                return;

            _familyTreeService = null;
        }

        private void InitFISPrivateMarketsService()
        {
            if (_FISPrivateMarketsService != null)
                return;

            _FISPrivateMarketsService = null;
        }

        private void InitFSInterfaceService()
        {
            if (_FSInterfaceService != null)
            {
                return;
            }
            _FSInterfaceService = new FSInterfaceService(true, _ruleSet.IsOffSet100On, _authorizationMatrix.FinacialServiceInterface);
        }

        private void InitInterfaceService()
        {
            if (_interfaceService != null)
            {
                return;
            }
            var nvp = _authorizationMatrix.AuthMatrixServices.FirstOrDefault(d => d.Key == "interface");
            var authComponents = new Dictionary<string, string>();
            if (nvp.Value != null)
            {
                authComponents = nvp.Value.AuthComponents;
            }
            _interfaceService = new InterfaceService(_ruleSet.IsInterfaceServiceOn, _authorizationMatrix.Interface,_authorizationMatrix.ProductX, authComponents);
        }

        private void InitMarketDataService()
        {
            if (_marketDataService != null)
            {
                return;
            }
            _marketDataService = new MarketDataService(_ruleSet.IsMdsCoreServiceOn, _authorizationMatrix.MarketData); ;
        }

        private void InitMembershipService()
        {
            if (_membershipService != null)
            {
                return;
            }
            _membershipService = new MembershipService(_ruleSet.IsMembershipCoreServiceOn, _authorizationMatrix.Membership); ;
        }

        private void InitNewsstandService()
        {
            if (_newsstandService != null)
                return;

            _newsstandService = null;
        }

        private void InitNumericDataService()
        {
            if (_numericDataService != null)
                return;

            _numericDataService = null;
        }

        private void InitRelationshipMappingService()
        {
            if (_relationshipMappingService != null)
                return;

            _relationshipMappingService = null;
        }

        private void InitRESTService()
        {
            if (_RESTService != null)
                return;

            _RESTService = null;
        }

        private void InitScreeningService()
        {
            if (_screeningService != null)
            {
                return;
            }
            //TODO: Verify the serviceOn
            _screeningService = new ScreeningService(true, _authorizationMatrix.Screening);
        }

        private void InitSearchService()
        {
            if (_searchService != null)
            {
                return;
            }
            _searchService = new SearchService(_ruleSet.IsIndexCoreServiceOn, _authorizationMatrix.Index);
        }

        private void InitSnapshotService()
        {
            if (_snapshotService != null)
            {
                return;
            }
            //TODO: Verify the serviceOn
            _snapshotService = new SnapshotService(true, _authorizationMatrix.Snapshot);
        }

        private void InitSymbologyService()
        {
            if (_symbologyService != null)
                return;

            _symbologyService = null;
        }

        private void InitTriggerRepositoryService()
        {
            if (_triggerRepositoryService != null)
                return;

            _triggerRepositoryService = null;
        }

        private void InitSessionService()
        {
            if (_sessionService != null)
                return;

            _sessionService = null;
        }

        private void InitPlatformAssetManagementService()
        {
            if (_platformAssetManagementService != null)
            {
                return;
            }
            //TODO: TO Verify the Auth Matrix
            _platformAssetManagementService = new PlatformAssetManagementService(_ruleSet.IsPAMServiceOn, _authorizationMatrix.PAM);
        }

        private void InitFreeSearchService()
        {
            if (_freeSearchService != null)
                return;

            _freeSearchService = null;
        }

        private void InitUERService()
        {
            if (_UERService != null)
            {
                return;
            }
            _UERService = new UERService(_ruleSet.IsUerCoreServiceOn, _authorizationMatrix.UER);
        }

        private void InitCommunicatorService()
        {
            if (_communicatorService != null)
            {
                return;
            }
            _communicatorService = new CommunicatorService(_ruleSet.IsCommunicator, _authorizationMatrix.Communicator); 
        }

        #endregion
    }
}
