using System.Linq;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Components.CompanySparkline;

namespace DowJones.Web.Mvc.UI.Components.Search
{
    public class SearchResponseToCompaniesSparklinesComponentModelMapper : TypeMapper<SearchResponse, CompaniesSparklinesModel>
    {
        public override CompaniesSparklinesModel Map(SearchResponse source)
        {
            if(source == null || source.Navigators == null || source.Navigators.Company == null)
                return new CompaniesSparklinesModel();

            var companyFCodes = source.Navigators.Company.Items.Select(x => x.Value);

            return new CompaniesSparklinesModel(companyFCodes);
        }
    }
}
