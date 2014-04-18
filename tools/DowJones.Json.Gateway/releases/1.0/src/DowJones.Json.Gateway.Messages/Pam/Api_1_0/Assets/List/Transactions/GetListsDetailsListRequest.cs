using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/Lists.svc")]
    [DataContract(Name = "GetListsDetailsList", Namespace = "")]
    public class GetListsDetailsListRequest : IPostJsonRestRequest

    {
        [DataMember(Name = "maxResultsToReturn", IsRequired = true)]
        public int MaxResultsToReturn { get; set; }

        [DataMember(Name = "listTypeCollection",IsRequired = true, EmitDefaultValue = false)]
        public ListTypeCollection ListTypeCollection { get; set; }

        [DataMember(Name = "sortBy", EmitDefaultValue = true)]
        public ListSortBy SortBy { get; set; }

        [DataMember(Name = "sortOrder", EmitDefaultValue = true)]
        public SortOrder SortOrder { get; set; }

        [DataMember(Name = "paging", EmitDefaultValue = false)]
        public Paging Paging { get; set; }

        [DataMember(Name = "filterCollection", EmitDefaultValue = false)]
        public FilterCollection FilterCollection { get; set; }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}