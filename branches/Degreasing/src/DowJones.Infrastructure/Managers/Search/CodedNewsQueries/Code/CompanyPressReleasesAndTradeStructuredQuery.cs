using System;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class CompanyPressReleasesAndTradeStructuredQuery : AbstractCompanyStructuredQuery
    {
        private readonly string sourceCode;

        public CompanyPressReleasesAndTradeStructuredQuery(string sourceCode)
        {
            this.sourceCode = sourceCode;
        }


        protected override void BuildSearchStringList()
        {
           ListSearchString.Add(GetRestrictorFilter(new[] { sourceCode }));

            const string TreditionalBss = @"(({0} and ns=(nrgn and ccat)) or ({0} not ns=nrgn))";
            string companyPart;
            if (CompanyFilter.Status.IsNewsCoded)
            {
                companyPart = "fds=" + CompanyFilter.Fcode;
            }
            else
            {
                var companyName = IsValid(CompanyFilter.NewsSearch) ? CompanyFilter.NewsSearch : (CompanyFilter.Descriptor == null ? string.Empty : CompanyFilter.Descriptor.Value);
                companyPart = string.Format("\"{0}\"", companyName);
            }

            var sr = new SearchString
                         {
                             Id = "LegacyPart", 
                             Type = SearchType.Free, 
                             Value = String.Format(TreditionalBss, companyPart), 
                             Mode = SearchMode.Traditional
                         };
            ListSearchString.Add(sr);
        }
    }
}