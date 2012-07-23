using System.Collections.Generic;
using System.Runtime.Serialization;
using GWQueryFilters = Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryFilters;
using DowJones.Mapping;

namespace DowJones.Pages.Common
{
    [DataContract(Name = "queryFilterSet", Namespace = "")]
    public class QueryFilterSet
    {
        [DataMember(Name = "inherit")]
        public bool Inherit { get; set; }

        [DataMember(Name = "queryFilters")]
        public QueryFilters QueryFilters { get; set; }
    }

    public class GWQueryFiltersQueryFiltersMapper : TypeMapper<GWQueryFilters, QueryFilterSet>
    {
        public override QueryFilterSet Map(GWQueryFilters source)
        {
            if (source == null)
                return null;
            var QF = new QueryFilterSet
            {
                Inherit = source.Inherit,
                QueryFilters = new QueryFilters()
            };

            if (source.QueryFilterCollection != null && source.QueryFilterCollection.Count > 0)
            {
                foreach (Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryFilter QueryFilterItem in source.QueryFilterCollection)
                {
                    QF.QueryFilters.Add(new QueryFilter { Text = QueryFilterItem.Text, Type = Mapper.Map<FilterType>(QueryFilterItem.Type) });
                }
            }
            return QF;
        }
    }

    public class QueryFiltersGWQueryFiltersMapper : TypeMapper<QueryFilterSet, GWQueryFilters>
    {
        public override GWQueryFilters Map(QueryFilterSet source)
        {
            if (source == null)
                return null;
            var QF = new GWQueryFilters
            {
                Inherit = source.Inherit,
                QueryFilterCollection = new Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryFilterCollection()
            };

            if (source.QueryFilters != null && source.QueryFilters.Count > 0)
            {
                foreach (QueryFilter QueryFilterItem in source.QueryFilters)
                {
                    QF.QueryFilterCollection.Add(new Factiva.Gateway.Messages.Assets.Pages.V1_0.QueryFilter { Text = QueryFilterItem.Text, Type = Mapper.Map<Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType>(QueryFilterItem.Type) });
                }
            }
            return QF;
        }
    }
}
