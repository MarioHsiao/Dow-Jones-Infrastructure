namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IRuleSet
    {
        bool IsSearchModulesOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is investext gateway on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is investext gateway on; otherwise, <c>false</c>.
        /// </value>
        bool IsInvestextGatewayOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP FDK offset on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP FDK offset on; otherwise, <c>false</c>.
        /// </value>
        bool IsFcpFdkOffsetOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is information worker display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is information worker display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsInformationWorkerDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM UI info display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM UI info display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComUiInfoDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is client billing display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is client billing display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsClientBillingDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is archive core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archive core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsArchiveCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is cibs core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is cibs core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsCibsCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is track core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is track core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsTrackCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is email core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is email core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsEmailCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is index core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is index core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsIndexCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is MDS core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is MDS core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsMdsCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is NDS core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is NDS core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsNdsCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is symbology service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is symbology service on; otherwise, <c>false</c>.
        /// </value>
        bool IsSymbologyServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is membership core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is membership core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsMembershipCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is uer core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is uer core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsUerCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is migration core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is migration core service on; otherwise, <c>false</c>.
        /// </value>
        bool IsMigrationCoreServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM track display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM track display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComTrackDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM news page display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM news page display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComNewsPageDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM company quick search display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM company quick search display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComCompanyQuickSearchDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM quote list display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM quote list display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComQuoteListDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM company screening display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM company screening display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComCompanyScreeningDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM company list display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM company list display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComCompanyListDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM quote display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM quote display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComQuoteDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM preferences display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM preferences display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComPreferencesDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM charting display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM charting display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComChartingDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM saved searches display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM saved searches display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComSavedSearchesDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM industry reports display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM industry reports display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComIndustryReportsDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is offset22 on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is offset22 on; otherwise, <c>false</c>.
        /// </value>
        bool IsOffset22On { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is interface service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is interface service on; otherwise, <c>false</c>.
        /// </value>
        bool IsInterfaceServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is offset23 on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is offset23 on; otherwise, <c>false</c>.
        /// </value>
        bool IsOffset23On { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM briefcase display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM briefcase display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsDotComBriefcaseDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is free text indexing display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is free text indexing display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsFreeTextIndexingDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsFcpDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP industry tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP industry tab display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsFcpIndustryTabDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP company tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP company tab display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsFcpCompanyTabDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP executive tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP executive tab display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsFcpExecutiveTabDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is government tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is government tab display service on; otherwise, <c>false</c>.
        /// </value>
        bool IsGovernmentTabDisplayServiceOn { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is insight user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is insight user; otherwise, <c>false</c>.
        /// </value>
        bool IsInsightUser { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is set mobile cookie on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is set mobile cookie on; otherwise, <c>false</c>.
        /// </value>
        bool IsSetMobileCookieOn { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsOffSet100On { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsMediaMonitor { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsMRM { get; }

        bool IsPAMServiceOn { get; }
    }
}