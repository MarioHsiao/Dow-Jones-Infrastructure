using System;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Search.Controller
{
	public class FormattingController
	{
        public ResultSortOrder SortOrder;
		public SnippetType SnippetType;
		public DateTime FreshnessDate;
		public ClusterMode ClusterMode;
		public DeduplicationMode DeduplicationMode;
	}
}
