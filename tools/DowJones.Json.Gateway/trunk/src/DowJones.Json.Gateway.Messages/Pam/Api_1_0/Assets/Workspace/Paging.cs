using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [DataContract(Name = "Paging", Namespace = "")]
    public class Paging
    {
        [DataMember(Name = "startIndex", IsRequired = true)]
        public int StartIndex { get; set; }

        [DataMember(Name = "maxResultsToReturn", IsRequired = true, Order = 1)]
        public int MaxResultsToReturn { get; set; }
    }
}
