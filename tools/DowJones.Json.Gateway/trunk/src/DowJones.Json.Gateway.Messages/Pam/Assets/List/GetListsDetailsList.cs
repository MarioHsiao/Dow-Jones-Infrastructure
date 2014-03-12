using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "GetListsDetailsList", Namespace = "")]
    public class GetListsDetailsList
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