using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemsDetailsList")]
    public class GetItemsDetailsList
    {
        public GetItemsDetailsList()
        {
            Qualifier = new List<AccessQualifier>();
            Filter = new List<Filter>();
            Paging = new Paging();
            Type = new List<ItemType>();
            MaxResultsToReturn = 0;
        }

        [DataMember(Name = "maxResultsToReturn")]
        public int MaxResultsToReturn { get; set; }

        [DataMember(Name = "type")]
        public List<ItemType> Type { get; set; }

        [DataMember(Name = "resultType")]
        public ResultType ResultType { get; set; }

        [DataMember(Name = "sortBy")]
        public ItemSortBy SortBy { get; set; }

        [DataMember(Name = "sortOrder")]
        public SortOrder SortOrder { get; set; }

        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }

        [DataMember(Name = "filter")]
        public List<Filter> Filter { get; set; }

        [DataMember(Name = "qualifier")]
        public List<AccessQualifier> Qualifier { get; set; }
    }
}