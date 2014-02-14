using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Models.Common
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

        [JsonProperty("authorNewsEntities")]
        public ParentNewsEntity AuthorNewsEntities { get; set; }

        [JsonProperty("sourceNewsEntities")]
        public ParentSourceNewsEntity SourceNewsEntities { get; set; }

        [JsonProperty("dateNewsEntities")]
        public ParentDateNewsEntity DateNewsEntities { get; set; }

        [JsonProperty("languageNewsEntities")]
        public ParentNewsEntity LanguageNewsEntities { get; set; }
        //[JsonProperty("organizationNewsEntities")]
        //public ParentNewsEntity OrganizationNewsEntities { get; set; }

        //[JsonProperty("unSpecifiedNewsEntities")]
        //public ParentNewsEntity UnSpecifiedNewsEntities { get; set; }

	}
}
