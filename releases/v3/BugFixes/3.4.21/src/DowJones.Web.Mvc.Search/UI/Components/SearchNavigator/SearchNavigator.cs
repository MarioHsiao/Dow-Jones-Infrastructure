using System.Collections.Generic;
using System.Linq;
using DowJones.Web.Mvc.Search.Results;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Search.UI.Components.Filters
{
    public class SearchNavigator : CompositeComponentModel
    {
        public SearchNavigatorNode AdditionalFilters
        {
            get
            {
                if (SecondarySourceGroups == null)
                    return new SearchNavigatorNode(EntityFilters);

                return new SearchNavigatorNode(new[] {SecondarySourceGroups}.Union(EntityFilters));
            }
        }

        public bool HasAdditionalFilters
        {
            get { return AdditionalFilters != null && AdditionalFilters.Any(); }
        }

        public IEnumerable<SearchNavigatorNode> EntityFilters { get; set; }

        [ClientData("primaryGroupId")]
        public string PrimaryGroupId { get; set; }

        public IEnumerable<SearchNavigatorNode> PrimarySourceGroups { get; set; }

        [ClientData("secondaryGroupId")]
        public string SecondaryGroupId { get; set; }

        [ClientData("layout")]   
        public ResultsLayout Layout { get; set; }

        public SearchNavigatorNode SecondarySourceGroups { get; set; }


        public SearchNavigator()
        {
            EntityFilters = Enumerable.Empty<SearchNavigatorNode>();
            DataServiceUrl = "/search/SearchNavigator";
            PrimarySourceGroups = Enumerable.Empty<SearchNavigatorNode>();
            SecondarySourceGroups = new SearchNavigatorNode();
        }
    }
}