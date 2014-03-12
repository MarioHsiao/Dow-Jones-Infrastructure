using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "DeleteList", Namespace = "")]
    public class DeleteList
    {
        [DataMember(Name = "id",IsRequired = true)]
        public long ID { get; set; }
    }
}