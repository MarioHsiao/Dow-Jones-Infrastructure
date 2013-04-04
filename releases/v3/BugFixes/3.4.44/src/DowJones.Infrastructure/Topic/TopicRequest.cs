using System;
using DowJones.Search;
using DowJones.Search.Filters;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Newtonsoft.Json;

namespace DowJones.Topic
{
    public class TopicRequest: TopicProperties
    {
        public CompoundQueryFilter Author { get; set; }

        public CompoundQueryFilter Company { get; set; }

        public CompoundQueryFilter Executive { get; set; }

        public CompoundQueryFilter Industry { get; set; }

        public CompoundQueryFilter Subject { get; set; }

        public CompoundQueryFilter Region { get; set; }

        public CompoundQueryFilter Source { get; set; }

        public SearchQueryFilters Filters { get; set; }
    }
}
