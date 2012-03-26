using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Showcase.Components.CompanyHeadlines;
using DowJones.Web.Showcase.Components.SourcesFilter;
using DowJones.Web.Showcase.Models;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class CompositeComponentController : ControllerBase
    {
        private static readonly IEnumerable<CompanyHeadline> Headlines = new[] {
                new CompanyHeadline("NYT", "Google", new PortalHeadlineInfo { Title = "Google is awesome"}), 
                new CompanyHeadline("WSJ", "Google", new PortalHeadlineInfo { Title = "Google owns the world"}), 
                new CompanyHeadline("WSJ", "Google", new PortalHeadlineInfo { Title = "Google is going bankrupt"}), 
                new CompanyHeadline("FRB", "Microsoft", new PortalHeadlineInfo { Title = "Microsoft is awesome"}), 
                new CompanyHeadline("FRB", "Microsoft", new PortalHeadlineInfo { Title = "Microsoft owns the world"}), 
                new CompanyHeadline("NYT", "Microsoft", new PortalHeadlineInfo { Title = "Microsoft is going bankrupt"}), 
                new CompanyHeadline("FRB", "Apple", new PortalHeadlineInfo { Title = "Apple is awesome"}), 
                new CompanyHeadline("WSJ", "Apple", new PortalHeadlineInfo { Title = "Apple owns the world"}), 
                new CompanyHeadline("NYT", "Apple", new PortalHeadlineInfo { Title = "Apple is going bankrupt"}), 
            };


        public ActionResult Index()
        {
            var viewModel = CompanyHeadlinesModel();

            return View("Index", viewModel);
        }

        public ActionResult DataService(IEnumerable<string> companies, IEnumerable<string> sources)
        {
            var model = CompanyHeadlinesModel(companies, sources);
            return Json(model.Headlines, JsonRequestBehavior.AllowGet);
        }


        private CompositeComponentDemoViewModel CompanyHeadlinesModel(IEnumerable<string> companies = null, IEnumerable<string> sources = null)
        {
            var viewModel = new CompositeComponentDemoViewModel();

            var filteredHeadlines = 
                Headlines
                    .Where(x => (companies == null) || companies.Contains(x.Company))
                    .Where(x => (sources == null) || sources.Contains(x.Source));

            var headlines = new PortalHeadlineListResultSet(filteredHeadlines.Select(x => x.Headline));

            var portalHeadlines = new PortalHeadlineListModel(new PortalHeadlineListDataResult(headlines));
            portalHeadlines.DisplaySnippets = SnippetDisplayType.Hybrid;
            portalHeadlines.MaxNumHeadlinesToShow = 20;

            viewModel.Headlines = new CompanyHeadlinesModel
                       {
                           Companies = filteredHeadlines.Select(x => x.Company).Distinct(),
                           DataServiceUrl = Url.Action("DataService"),
                           Headlines = portalHeadlines,
                           IsClientMode = false,
                       };

            viewModel.Sources = new SourcesFilterModel(filteredHeadlines.Select(x => x.Source).Distinct());

            return viewModel;
        }


        private class CompanyHeadline
        {
            public string Company { get; set; }
            public string Source { get; set; }
            public PortalHeadlineInfo Headline { get; set; }

            public CompanyHeadline(string source, string company, PortalHeadlineInfo headline)
            {
                Source = source;
                Company = company;
                Headline = headline;
            }
        }
    }
}