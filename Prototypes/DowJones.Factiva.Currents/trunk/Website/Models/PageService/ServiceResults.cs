using System.Runtime.Serialization;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AbstractSummaryPackage = DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Packages.AbstractSummaryPackage;
using AbstractTopNewsPackage = DowJones.Factiva.Currents.Website.Models.PageService.Modules.TopNews.Packages.AbstractTopNewsPackage;
using CustomTopicsPackage = DowJones.Factiva.Currents.Website.Models.PageService.Modules.CustomTopics.Packages.CustomTopicsPackage;
using RadarPackage = DowJones.Factiva.Currents.Website.Models.PageService.Modules.Radar.Packages.RadarPackage;
using SourcePackage = DowJones.Factiva.Currents.Website.Models.PageService.Modules.Sources.Packages.SourcePackage;

namespace DowJones.Factiva.Currents.Website.Models.PageService
{
	[DataContract(Name = "topNewsNewspageModuleServiceResult", Namespace = "")]
	public class TopNewsNewsPageModuleServiceResult :
		AbstractModuleServiceResult
			<Modules.TopNews.Results.TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>, AbstractTopNewsPackage, TopNewsNewspageModule>
	{
	}


	[DataContract(Name = "sourcesNewspageModuleServiceResult", Namespace = "")]
	public class SourcesNewsPageModuleServiceResult :
		AbstractModuleServiceResult<Modules.Sources.Results.SourcesNewsPageServicePartResult<SourcePackage>, SourcePackage, SourcesNewspageModule>
	{
	}

	[DataContract(Name = "summaryNewspageModuleServiceResult", Namespace = "")]
	public class SummaryNewsPageModuleServiceResult :
		AbstractModuleServiceResult
			<Modules.Summary.Results.SummaryNewsPageServicePartResult<AbstractSummaryPackage>, AbstractSummaryPackage, SummaryNewspageModule>
	{
	}

	[DataContract(Name = "radarNewspageModuleServiceResult", Namespace = "")]
	public class RadarNewsPageModuleServiceResult :
		AbstractModuleServiceResult<Modules.Radar.Results.RadarNewsPageServicePartResult<RadarPackage>, RadarPackage, RadarNewspageModule>
	{
	}


	[DataContract(Name = "customTopicsNewspageModuleServiceResult", Namespace = "")]
	public class CustomTopicsNewsPageModuleServiceResult :
		AbstractModuleServiceResult<Modules.CustomTopics.Results.CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>, CustomTopicsPackage, CustomTopicsNewspageModule>
	{
	}
}