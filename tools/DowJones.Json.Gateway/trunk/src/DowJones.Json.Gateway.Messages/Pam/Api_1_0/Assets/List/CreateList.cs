using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [ServicePath("pamapi/1.0/DJXItems.svc")]
    [DataContract(Name = "CreateList", Namespace = "")]
    public class CreateList : IPostJsonRestRequest
    {
        [DataMember(Name = "list", IsRequired = true, EmitDefaultValue = false)]
        public List List { get; set; }
    }
}