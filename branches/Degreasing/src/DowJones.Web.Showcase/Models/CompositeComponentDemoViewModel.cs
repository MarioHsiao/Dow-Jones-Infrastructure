using DowJones.Web.Showcase.Components.CompanyHeadlines;
using DowJones.Web.Showcase.Components.SourcesFilter;

namespace DowJones.Web.Showcase.Models
{
    public class CompositeComponentDemoViewModel
    {

        public CompanyHeadlinesModel Headlines { get; set; }

        public SourcesFilterModel Sources { get; set; }

    }
}