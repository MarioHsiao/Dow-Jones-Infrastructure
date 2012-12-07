using System.Collections.Generic;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.TopNews.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using System.Linq;

namespace DowJones.Factiva.Currents.Models
{
    public class CurrentTopNewsModel : CompositeComponentModel
    {
        public IEnumerable<CurrentTopNews> CurrentsHeadlines { get; set; }
    }

    public class TopNewsModelMapper : TypeMapper<TopNewsNewsPageModuleServiceResult, CurrentTopNewsModel>
    {
        public override CurrentTopNewsModel Map(TopNewsNewsPageModuleServiceResult topNewsResult)
        {
            return new CurrentTopNewsModel
            {
                CurrentsHeadlines = topNewsResult.PartResults
                                          .Select(p =>
                                              new CurrentTopNews()
                                              {
                                                  Title = p.Package != null ? p.Package.Title : string.Empty,
                                                  CurrentHeadline = p.Package != null ? new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Package.Result) { ShowAuthor = false }) : null
                                              })

            };
        }
    }

    public class CurrentTopNews
    {
        public string Title { get; set; }

        public CurrentsHeadlineModel CurrentHeadline { get; set; }
    }
}