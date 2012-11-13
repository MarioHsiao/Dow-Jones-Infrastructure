using System.Collections;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Canvas;
using System.Linq;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using System.Collections.Generic;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService;

namespace DowJones.Factiva.Currents.Models
{
    public class NewsstandModel : Module
    {
        public IEnumerable<CurrentsHeadlineModel> CurrentHeadlines { get; set; }
    }

    public class NewsstandModelMapper : TypeMapper<NewsstandNewsPageModuleServiceResult, NewsstandModel>
    {
        public override NewsstandModel Map(NewsstandNewsPageModuleServiceResult newstandSource)
        {
           // var x = newstandSource.PartResults.Where(r => r is NewsstandHeadlinesPackage).SelectMany(ns => ns.NewsstandSections).Select(new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Result)));

            //var x1 = newstandSource.PartResults.Cast<NewsstandHeadlinesPackage>().SelectMany(ns => ns.NewsstandSections).Select(p => new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Result)));

            var x1 = newstandSource.PartResults.Cast<AbstractHeadlinePackage>();
            return new NewsstandModel
                {
                    CurrentHeadlines =newstandSource.PartResults.Cast<NewsstandHeadlinesPackage>().SelectMany(ns => ns.NewsstandSections).Select(p => new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Result)))
                };
        }
    }
}