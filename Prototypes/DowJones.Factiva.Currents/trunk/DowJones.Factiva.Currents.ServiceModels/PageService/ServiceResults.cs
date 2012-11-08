using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Radar.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Radar.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Results;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.TopNews.Packages;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.TopNews.Results;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
{
	[DataContract(Name = "topNewsNewspageModuleServiceResult", Namespace = "")]
	public class TopNewsNewsPageModuleServiceResult :
		AbstractModuleServiceResult
			<TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>, AbstractTopNewsPackage, TopNewsNewspageModule>
	{
	}


	[DataContract(Name = "sourcesNewspageModuleServiceResult", Namespace = "")]
	public class SourcesNewsPageModuleServiceResult :
		AbstractModuleServiceResult<SourcesNewsPageServicePartResult<SourcePackage>, SourcePackage, SourcesNewspageModule>
	{
	}

	[DataContract(Name = "summaryNewspageModuleServiceResult", Namespace = "")]
	public class SummaryNewsPageModuleServiceResult :
		AbstractModuleServiceResult
			<SummaryNewsPageServicePartResult<AbstractSummaryPackage>, AbstractSummaryPackage, SummaryNewspageModule>
	{
	}

	[DataContract(Name = "radarNewspageModuleServiceResult", Namespace = "")]
	public class RadarNewsPageModuleServiceResult :
		AbstractModuleServiceResult<RadarNewsPageServicePartResult<RadarPackage>, RadarPackage, RadarNewspageModule>
	{
	}


	[DataContract(Name = "customTopicsNewspageModuleServiceResult", Namespace = "")]
	public class CustomTopicsNewsPageModuleServiceResult :
		AbstractModuleServiceResult<CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>, CustomTopicsPackage, CustomTopicsNewspageModule>
	{
	}
}