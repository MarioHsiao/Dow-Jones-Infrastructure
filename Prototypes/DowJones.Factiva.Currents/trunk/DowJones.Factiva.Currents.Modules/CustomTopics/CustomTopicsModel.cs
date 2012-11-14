using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using System.Linq;
using System.Collections.Generic;

namespace DowJones.Factiva.Currents.Models
{
	public class CustomTopicsModel : CompositeComponentModel
    {
        public IEnumerable<CurrentsHeadlineModel> CurrentsHeadlines { get; set; }
    }

    public class CustomTopicsModelMapper : TypeMapper<CustomTopicsNewsPageModuleServiceResult, CustomTopicsModel>
    {
        public override CustomTopicsModel Map(CustomTopicsNewsPageModuleServiceResult customTopicSource)
        {
            return new CustomTopicsModel
            {
                CurrentsHeadlines = customTopicSource.PartResults
                                        .Select(p => new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Package.Result)))
            };
        }
    }
}