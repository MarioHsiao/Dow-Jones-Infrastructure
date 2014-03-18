using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "GetListsPropertiesList", Namespace = "")]
    public class GetListsPropertiesListRequest : PostJsonRestRequest
    {
        [DataMember(Name = "maxResultsToReturn", IsRequired = true)]
        public int MaxResultsToReturn { get; set; }

        [DataMember(Name = "listTypeCollection", IsRequired = true, EmitDefaultValue = false, Order = 1)]
        public ListTypeCollection ListTypeCollection { get; set; }

        [DataMember(Name = "sortBy", IsRequired = true, Order = 2)]
        public ListSortBy SortBy { get; set; }

        [DataMember(Name = "paging", IsRequired = true, EmitDefaultValue = false, Order = 3)]
        public Paging Paging { get; set; }

        [DataMember(Name = "sortOrder", IsRequired = true, Order = 4)]
        public SortOrder SortOrder { get; set; }

        [DataMember(Name = "filterCollection", IsRequired = true, EmitDefaultValue = false, Order = 5)]
        public FilterCollection FilterCollection { get; set; }
    }
}