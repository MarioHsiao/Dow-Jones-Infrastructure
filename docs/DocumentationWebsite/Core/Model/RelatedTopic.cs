using System.Runtime.Serialization;

namespace DowJones.Documentation
{
	public class RelatedTopic
	{
	    [DataMember(Name = "category")]
		public string Category { get; set; }

		[DataMember(Name = "page")]
		public string Page { get; set; }

	}
}