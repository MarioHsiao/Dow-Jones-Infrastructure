using Factiva.Gateway.Messages.Search.V2_0;

namespace Factiva.Gateway.Messages.CodedNews
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
