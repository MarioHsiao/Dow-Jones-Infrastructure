using System;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class IndustryAnalysisAndProfilesStructuredQuery : AbstractIndustryStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var sourceCode = GetSourceByQueryType();
            var searchString = GetRestrictorFilter(new[] {sourceCode});
            searchString.Scope = "sc";
            ListSearchString.Add(searchString);

            searchString = GetIndustryFilter(new[] {Industry.Fcode});
            ListSearchString.Add(searchString);
        }


        private string GetSourceByQueryType()
        {
            switch (QueryType)
            {
                case CodedNewsType.ForresterResearch:
                    return "DMRP";
                case CodedNewsType.FreedoniaSummary:
                    return "RSFG";
                case CodedNewsType.IBIS:
                    return "IBIS";
                case CodedNewsType.MarketResearch:
                    return "MRKRE";
                case CodedNewsType.MergentReport:
                    return "MIRAUS";
                case CodedNewsType.SpSummary:
                    return "SPIND";
                case CodedNewsType.BusinessMonitor:
                    return "EMDN";
                case CodedNewsType.ChinaCoal:
                    return "BCOLEN";

                default:
                    throw new NotSupportedException("Invalid query type");
            }
        }
    }
}