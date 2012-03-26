using System;
using System.Text;
using Factiva.Gateway.Messages.Search.V2_0;

namespace Factiva.Gateway.Messages.CodedNews.CodedNewsQueries
{
    public class CompanyAnalysisStructuredQuery : AbstractCompanyStructuredQuery
    {
        private const string SOURCE_CODE = "DATMON";

        protected override void BuildSearchStringList()
        {
            if (QueryType == CodedNewsType.ReportAll)
            {
                var eachReportFormat = String.Format( "(sc={0} and fds={1}", SOURCE_CODE , CompanyFilter.Fcode) + " and hd=({0}))";
                var sb = new StringBuilder();
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonBusinessDescription));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonCompanyLocations));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonCompanyOverview));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonHistory));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonKeyEmployees));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonKeyFacts));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonMajorProducts));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonSWOTAnalysis));
                sb.Append(" or ");
                sb.AppendFormat(eachReportFormat, GetValue(CodedNewsType.DataMonTopCompetitors));
                
                var sr = new SearchString
                             {
                                 Id = "LegacyPart", 
                                 Type = SearchType.Free, 
                                 Value = sb.ToString(), 
                                 Mode = SearchMode.Traditional
                             };
                ListSearchString.Add(sr);

            }
            else
            {
                var sr = GetRestrictorFilter(new [] {SOURCE_CODE});
                sr.Scope = "sc";
                ListSearchString.Add(sr);

                sr = GetCompanyFilter(new [] {CompanyFilter.Fcode}, false);
                ListSearchString.Add(sr);

                sr = new SearchString
                         {
                             Scope = "hd", 
                             Value = GetValue(QueryType), 
                             Mode = SearchMode.Phrase, 
                             Type = SearchType.Free
                         };
                ListSearchString.Add(sr);
            }
        }

        private static string GetValue(CodedNewsType codedNewsType)
        {
            switch (codedNewsType)
            {
                case CodedNewsType.DataMonBusinessDescription:
                    return "Business Description";
                case CodedNewsType.DataMonCompanyLocations:
                    return "company locations";
                case CodedNewsType.DataMonCompanyOverview:
                    return "company overview";
                case CodedNewsType.DataMonHistory:
                    return "History";
                case CodedNewsType.DataMonKeyEmployees:
                    return "Key Employees";
                case CodedNewsType.DataMonKeyFacts:
                    return "Key Facts";
                case CodedNewsType.DataMonMajorProducts:
                    return "Major Products";
                case CodedNewsType.DataMonSWOTAnalysis:
                    return "SWOT Analysis";
                case CodedNewsType.DataMonTopCompetitors:
                    return "Top Competitors";
                default:
                    throw new NotImplementedException(codedNewsType.ToString());
            }
        }
    }
}