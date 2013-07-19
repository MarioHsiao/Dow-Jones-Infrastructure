using System.Collections.Generic;
using DowJones.Search;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.Search.UI.Components.DidYouMean
{
	public class DidYouMean
	{
		public IEnumerable<DidYouMeanGroup> Groups { get; set; }

		public bool HideEntities { get; set; }

		public string Context { get; set; }
	}

	public class DidYouMeanGroup : List<DidYouMeanEntity>
	{
		public NewsFilterCategory Category { get; set; }

		public string Name { get; set; }

		public DidYouMeanGroup()
		{
		}

		public DidYouMeanGroup(IEnumerable<DidYouMeanEntity> entities)
			: base(entities)
		{
		}
	}

	public class DidYouMeanEntity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string SearchTerm { get; set; }
	}
}