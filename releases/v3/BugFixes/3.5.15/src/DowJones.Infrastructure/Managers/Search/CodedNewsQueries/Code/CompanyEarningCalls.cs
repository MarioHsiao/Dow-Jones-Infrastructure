namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class CompanyEarningCalls : AbstractCompanyStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var searchString = GetRestrictorFilter(new [] { "WC40943" });
            searchString.Scope = "sc";
            ListSearchString.Add(searchString);
        }
    }
}
