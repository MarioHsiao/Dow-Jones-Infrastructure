using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "DeleteList", Namespace = "")]
    public class DeleteList
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long ID { get; set; }
    }
}