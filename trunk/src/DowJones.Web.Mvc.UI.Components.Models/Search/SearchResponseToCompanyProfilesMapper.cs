using System.Linq;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Mvc.UI.Components.Models.CompanySparkline;

namespace DowJones.Web.Mvc.UI.Components.Search.Mappers
{
    public class SearchResponseToCompaniesSparklinesComponentModelMapper : TypeMapper<SearchResponse, CompaniesSparklinesComponentModel>
    {
        public override CompaniesSparklinesComponentModel Map(SearchResponse source)
        {
            if(source == null || source.Navigators == null || source.Navigators.Company == null)
                return new CompaniesSparklinesComponentModel();

            var companyFCodes = source.Navigators.Company.Items.Select(x => x.Value);

            return new CompaniesSparklinesComponentModel(companyFCodes);
        }
    }
}
