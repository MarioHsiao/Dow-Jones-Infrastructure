using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "GetListsDetailsList", Namespace = "")]
    public class GetListsDetailsListRequest : IPostJsonRestRequest

    {
        [DataMember(Name = "maxResultsToReturn", IsRequired = true)]
        public int MaxResultsToReturn { get; set; }

        [DataMember(Name = "listTypeCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListTypeCollection ListTypeCollection { get; set; }

        [DataMember(Name = "sortBy", IsRequired = true)]
        public ListSortBy SortBy { get; set; }

        [DataMember(Name = "sortOrder", IsRequired = true)]
        public SortOrder SortOrder { get; set; }

        [DataMember(Name = "paging", IsRequired = true, EmitDefaultValue = false)]
        public Paging Paging { get; set; }

        [DataMember(Name = "filterCollection", IsRequired = true, EmitDefaultValue = false)]
        public FilterCollection FilterCollection { get; set; }
    }
}