using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class CompanyStructuredQuery : AbstractCompanyStructuredQuery
    {
        public bool IncludeNiweFilter { get; set; }

        protected override void BuildSearchStringList()
        {
            // Default iwe filter
            if (IncludeNiweFilter)
            {
                ListSearchString.Add(GetNiweFilter());
            }

            // Selected news subject based on query type.
            var newsSubjects = CompanyKeyDevelopmentStructuredQuery.GetNewsSubjectsCode(QueryType);
            var temp = GetNewsSubjectFilter(newsSubjects);
            if (temp != null)
            {
                temp.Id = "nsCat";
                temp.Mode = SearchMode.Any;
                ListSearchString.Add(temp);
            }

            // Current company
            if (OccurrenceSearch)
            {
                temp = GetOccurrenceCompanyFilter(new[] { CompanyFilter.Fcode });
            }
            else if (CompanyFilter.Status.IsOccurrenceCoded)
            {
                temp = GetOccurrenceOnlyCompanyFilter(new[] { CompanyFilter.Fcode });
            }
            else if (CompanyFilter.Status.IsNewsCoded)
            {
                temp = GetAboutOnlyCompanyFilter(new[] { CompanyFilter.Fcode });
            }
            else
            {
                var companyName = IsValid(CompanyFilter.NewsSearch) ? CompanyFilter.NewsSearch : (CompanyFilter.Descriptor == null ? string.Empty : CompanyFilter.Descriptor.Value);
                companyName = string.Format("\"{0}\"", companyName);
                temp = GetCompanyNameFilter(companyName);
            }

            temp.Id = "fdsCat";

            ListSearchString.Add(temp);
        }
    }
}