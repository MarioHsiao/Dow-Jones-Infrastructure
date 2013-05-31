using DowJones.Managers.Search.CodedNewsQueries.Government;
using DowJones.Session;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public interface IStructuredQuery
    {
        IControlData ControlData { get; set; }

        CodedNewsType QueryType { get; set; }

        NewsFiltersExtended Filters { get; set; }

        bool OccurrenceSearch { set; get; }

        string[] LanguagePreference { set; get; }

        StructuredQuery Query { get; }

        StructuredQuery ExpandQuery { get; }
    }

    public interface ICompanyQuery : IStructuredQuery
    {
        CompanyFilter CompanyFilter { get; set; }
    }

    public interface IExecutiveQuery : IStructuredQuery
    {
        ExecutiveFilter Executives { get; set; }
    }

    public interface IIndustryQuery : IStructuredQuery
    {
        Industry Industry { get; set; }
    }

    public interface IGovernmentQuery : IStructuredQuery
    {
        GovernmentOfficial Official { get; set; }
        Organization Organization { get; set; }
    }
}