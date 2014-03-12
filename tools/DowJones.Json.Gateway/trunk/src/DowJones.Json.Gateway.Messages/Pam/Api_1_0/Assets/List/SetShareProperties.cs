using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "SetShareProperties", Namespace = "")]
    public class SetShareProperties
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "shareProperties", IsRequired = true, EmitDefaultValue = false)]
        public ShareProperties ShareProperties { get; set; }
    }
}