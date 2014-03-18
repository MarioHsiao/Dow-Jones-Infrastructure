using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "SetShareProperties", Namespace = "")]
    public class SetSharePropertiesRequest : PostJsonRestRequest
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "shareProperties", IsRequired = true, EmitDefaultValue = false)]
        public ShareProperties ShareProperties { get; set; }
    }
}