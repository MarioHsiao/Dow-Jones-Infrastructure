using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "GetListByID", Namespace = "")]
    public class GetListById : IGetJsonRestRequest
    {
        [DataMember(Name="id", IsRequired = true)]
        public long Id { get; set; }
    }
}