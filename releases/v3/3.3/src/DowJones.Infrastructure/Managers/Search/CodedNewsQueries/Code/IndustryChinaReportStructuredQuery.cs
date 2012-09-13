namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class IndustryChinaReportStructuredQuery : AbstractIndustryStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var searchString = GetRestrictorFilter(new[] {"BCIROS", "BCIRDT", "BCIREN"});
            searchString.Scope = "sc";

            ListSearchString.Add(searchString);

            searchString = GetIndustryFilter(new[] {Industry.Fcode});
            ListSearchString.Add(searchString);
        }
    }
}