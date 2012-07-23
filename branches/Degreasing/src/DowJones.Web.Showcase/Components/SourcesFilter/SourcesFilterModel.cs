using System.Collections.Generic;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Showcase.Components.SourcesFilter
{
    public class SourcesFilterModel : CompositeComponentModel
    {
        [ClientData]
        public IEnumerable<string> Sources { get; set; }

        public SourcesFilterModel(IEnumerable<string> sources)
        {
            Sources = sources;
        }
    }
}