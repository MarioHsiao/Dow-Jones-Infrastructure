using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using System.Linq;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using System.Collections.Generic;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages;

namespace DowJones.Factiva.Currents.Models
{
	public class CurrentNewsStandModel : CompositeComponentModel
	{
		public IEnumerable<NewsStandSource> CurrentSources { get; set; }
	}

	public class NewsstandModelMapper : TypeMapper<NewsstandNewsPageModuleServiceResult, CurrentNewsStandModel>
	{
		public override CurrentNewsStandModel Map(NewsstandNewsPageModuleServiceResult newstandSource)
		{
            return new CurrentNewsStandModel
                {
                    CurrentSources = newstandSource.PartResults
                                            .Where(pr => pr.Package is NewsstandHeadlinesPackage)
                                            .Select(p => p.Package)
                                            .Cast<NewsstandHeadlinesPackage>()
                                            .SelectMany(p => p.NewsstandSections)
                                            .Select(p => new NewsStandSource
                                                {
                                                    LogoUrl = p.SourceLogoUrl,
                                                    Title = p.SourceTitle,
                                                    CurrentHeadline = new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Result)) { ShowSource = false}
                                                }
                                            )
                };
		}
	}

	public class NewsStandSource
	{
		public string LogoUrl { get; set; }
		public string Title { get; set; }
		public CurrentsHeadlineModel CurrentHeadline { get; set; }

	}
}