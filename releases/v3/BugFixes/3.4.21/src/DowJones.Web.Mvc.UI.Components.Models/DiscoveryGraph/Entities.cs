using DowJones.Models.Common;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.DiscoveryGraph
{
    public class Entities
    {
		[JsonProperty("companyNewsEntities")]
		public ParentNewsEntity CompanyNewsEntities { get; set; }

		[JsonProperty("industryNewsEntities")]
		public ParentNewsEntity IndustryNewsEntities { get; set; }

		[JsonProperty("personNewsEntities")]
		public ParentNewsEntity PersonNewsEntities { get; set; }

		[JsonProperty("regionNewsEntities")]
		public ParentNewsEntity RegionNewsEntities { get; set; }

		[JsonProperty("subjectNewsEntities")]
		public ParentNewsEntity SubjectNewsEntities { get; set; }
    }
}
