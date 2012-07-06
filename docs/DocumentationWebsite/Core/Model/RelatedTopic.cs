using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DowJones.Documentation
{
	public class RelatedTopic
	{
		[DataMember(Name = "name")]
		public string Name
		{
			get { return Page; }
		}

		[DataMember(Name = "category")]
		public string Category { get; set; }

		[DataMember(Name = "page")]
		public string Page { get; set; }

	}
}