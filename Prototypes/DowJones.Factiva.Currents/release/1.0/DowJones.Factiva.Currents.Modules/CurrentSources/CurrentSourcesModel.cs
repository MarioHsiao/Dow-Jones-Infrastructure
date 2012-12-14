using System.Collections.Generic;
using System.Linq;
using DowJones.Factiva.Currents.Components.CurrentsHeadline;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Results;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Models
{
	public class CurrentSourcesModel : CompositeComponentModel 
	{
        public IEnumerable<CurrentSource> CurrentsHeadlines { get; set; }
	}

	public class CurrentSourcesModelMapper : TypeMapper<SourcesNewsPageModuleServiceResult, CurrentSourcesModel>
	{
		public override CurrentSourcesModel Map(SourcesNewsPageModuleServiceResult source)
		{

            return new CurrentSourcesModel
                {
                    CurrentsHeadlines = source.PartResults
                                              .Select(p =>
                                                  new CurrentSource()
                                                  {
                                                      LogoUrl = p.Package.SourceLogoUrl,
                                                      Title = p.Package.SourceName,
                                                      CurrentHeadline = new CurrentsHeadlineModel(new PortalHeadlineListModel(p.Package.Result) { ShowSource = false })
                                                  })
                };
		}
	}

    public class CurrentSource
    {
        public string LogoUrl {get;set;}

        public string Title { get; set; }

        public CurrentsHeadlineModel CurrentHeadline { get; set; }
    }
}
