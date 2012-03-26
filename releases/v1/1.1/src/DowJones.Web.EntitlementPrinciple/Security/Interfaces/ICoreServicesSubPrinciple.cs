using DowJones.Web.EntitlementPrinciple.Security.Services;

namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface ICoreServicesSubPrinciple
    {
        /// <summary>
        /// Gets the affiliation service.
        /// </summary>
        /// <value>The affiliation service.</value>
        IAffiliationService AffiliationService { get; }

        /// <summary>
        /// Gets the alerts service.
        /// </summary>
        /// <value>The alerts service.</value>
        IAlertsService AlertsService { get; }

        /// <summary>
        /// Gets the analyze service.
        /// </summary>
        /// <value>The analyze service.</value>
        IAnalyzeService AnalyzeService { get; }

        /// <summary>
        /// Gets the archive service.
        /// </summary>
        /// <value>The archive service.</value>
        IArchiveService ArchiveService { get; }

        /// <summary>
        /// Gets the CIB service.
        /// </summary>
        /// <value>The CIB service.</value>
        ICIBsService CIBsService { get; }

        /// <summary>
        /// Gets the client match service.
        /// </summary>
        /// <value>The client match service.</value>
        IClientMatchService ClientMatchService { get; }

        /// <summary>
        /// Gets the customer value dashboard service.
        /// </summary>
        /// <value>The customer value dashboard service.</value>
        ICustomerValueDashboardService CustomerValueDashboardService { get; }

        /// <summary>
        /// Gets the email service.
        /// </summary>
        /// <value>The email service.</value>
        IEmailService EmailService { get; }

        /// <summary>
        /// Gets the family tree service.
        /// </summary>
        /// <value>The family tree service.</value>
        IFamilyTreeService FamilyTreeService { get; }

        /// <summary>
        /// Gets the FIS private markets service.
        /// </summary>
        /// <value>The FIS private markets service.</value>
        IFISPrivateMarketsService FISPrivateMarketsService { get; }

        /// <summary>
        /// Gets the interface service.
        /// </summary>
        /// <value>The interface service.</value>
        IInterfaceService InterfaceService { get; }

        /// <summary>
        /// Gets the market data service.
        /// </summary>
        /// <value>The market data service.</value>
        IMarketDataService MarketDataService { get; }

        /// <summary>
        /// Get the membership service
        /// </summary>
        IMembershipService MembershipService { get; }

        /// <summary>
        /// Gets the newsstand service.
        /// </summary>
        /// <value>The newsstand service.</value>
        INewsstandService NewsstandService { get; }

        /// <summary>
        /// Gets the numeric data service.
        /// </summary>
        /// <value>The numeric data service.</value>
        INumericDataService NumericDataService { get; }

        /// <summary>
        /// Gets the relationship mapping service.
        /// </summary>
        /// <value>The relationship mapping service.</value>
        IRelationshipMappingService RelationshipMappingService { get; }

        /// <summary>
        /// Gets the REST service.
        /// </summary>
        /// <value>The REST service.</value>
        IRESTService RESTService { get; }

        /// <summary>
        /// Gets the screening service.
        /// </summary>
        /// <value>The screening service.</value>
        IScreeningService ScreeningService { get; }

        /// <summary>
        /// Gets the search service.
        /// </summary>
        /// <value>The search service.</value>
        ISearchService SearchService { get; }

        /// <summary>
        /// Gets the snapshot service.
        /// </summary>
        /// <value>The snapshot service.</value>
        ISnapshotService SnapshotService { get; }

        /// <summary>
        /// Gets the symbology service.
        /// </summary>
        /// <value>The symbology service.</value>
        ISymbologyService SymbologyService { get; }

        /// <summary>
        /// Gets the trigger repository service.
        /// </summary>
        /// <value>The trigger repository service.</value>
        ITriggerRepositoryService TriggerRepositoryService { get; }

        /// <summary>
        /// Gets the session service.
        /// </summary>
        /// <value>The session service.</value>
        ISessionService SessionService { get; }

        /// <summary>
        /// Gets the platform asset management service.
        /// </summary>
        /// <value>The platform asset management service.</value>
        IPlatformAssetManagementService PlatformAssetManagementService { get; }

        /// <summary>
        /// Gets the core search service.
        /// </summary>
        /// <value>The core search service.</value>
        IFreeSearchService FreeSearchService { get; }


        /// <summary>
        /// 
        /// </summary>
        IFSInterfaceService FSInterfaceService { get; }

        /// <summary>
        /// 
        /// </summary>
        IUERService UERService { get; }
    }
}