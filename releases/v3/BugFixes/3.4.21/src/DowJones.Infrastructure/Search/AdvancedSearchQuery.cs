using System.Collections.Generic;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Search
{
    public abstract class AdvancedSearchQuery : AbstractSearchQuery
    {
        public CompoundQueryFilter Author { get; set; }

        public CompoundQueryFilter Company { get; set; }
        
        public IEnumerable<ExclusionFilter> Exclusions { get; set; }

        public CompoundQueryFilter Executive { get; set; }

        public CompoundQueryFilter Industry { get; set; }

        public CompoundQueryFilter Subject { get; set; }

        public CompoundQueryFilter Region { get; set; }

        public CompoundQueryFilter Source { get; set; }

        public override SearchSetupScreen GetSearchSetupScreen()
        {
            return SearchSetupScreen.AdvancedSearch;
        }
    }
}
