using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "UnSubscribeList", Namespace = "")]
    public class UnSubscribeList
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }
    }
}