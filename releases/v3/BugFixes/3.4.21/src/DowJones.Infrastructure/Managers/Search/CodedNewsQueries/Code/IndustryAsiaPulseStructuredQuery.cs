using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class IndustryAsiaPulseStructuredQuery : AbstractIndustryStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var searchString = new SearchString
                                   {
                                       Id = "rstFilter", 
                                       Mode = SearchMode.Simple, 
                                       Type = SearchType.Controlled, 
                                       Value = "sc:APULSE hd:briefing"
                                   };

            ListSearchString.Add(searchString);

            searchString = GetIndustryFilter(new [] { Industry.Fcode });
            ListSearchString.Add(searchString);
        }
    }
}

