using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "Paging")]
    public class Paging
    {
        [DataMember(Name = "startIndex")]
        public int StartIndex { get; set; }

        [DataMember(Name = "maxResultsToReturn")]
        public int MaxResultsToReturn { get; set; }
    }
}