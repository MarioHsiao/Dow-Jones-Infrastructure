using System.Runtime.Serialization;

namespace DowJones.Pages.Modules
{
    /// <remarks>
    /// TODO: Move this to class definitions (or something)
    /// Enums are not extensible
    /// </<remarks>>
    public enum ModuleType
    {
        [EnumMember]
        [IRTCode("malt")]
        AlertsNewspageModule, 

        [EnumMember]
        [IRTCode("mrss")]
        SyndicationNewspageModule, 

        [EnumMember]
        [IRTCode("mnews")]
        NewsstandNewspageModule, 

        [EnumMember]
        [IRTCode("mcus")]
        CustomTopicsNewspageModule, 

        [EnumMember]
        [IRTCode("msrc")]
        SourcesNewspageModule, 

        [EnumMember]
        [IRTCode("mrad")]
        RadarNewspageModule, 

        [EnumMember]
        [IRTCode("mreg")]
        RegionalMapNewspageModule, 

        [EnumMember]
        [IRTCode("mtop")]
        TopNewsNewspageModule, 

        [EnumMember]
        [IRTCode("mtre")]
        TrendingNewsPageModule, 

        [EnumMember]
        [IRTCode("msum")]
        SummaryNewspageModule, 

        [EnumMember]
        [IRTCode("mcom")]
        CompanyOverviewNewspageModule,

        [EnumMember]
        [IRTCode("msoc")]
        SocialMediaNewspageModule,

        [EnumMember]
        CommunicatorAuthorSummaryModule,

        [EnumMember]
        CommunicatorOutletSummaryModule,

        [EnumMember]
        CommunicatorAuthorActivitiesSummaryModule,

        [EnumMember]
        CommunicatorNewsModule,

        [EnumMember]
        CommunicatorNewAuthorAlertsModule,

        [EnumMember]
        CommunicatorChartsModule,

        [EnumMember]
        [IRTCode("mnws")]
        NewsstandSourcesNewspageModule,
    }
}