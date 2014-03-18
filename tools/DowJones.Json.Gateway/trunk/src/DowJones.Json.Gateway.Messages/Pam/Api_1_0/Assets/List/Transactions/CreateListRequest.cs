using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/List.svc")]
    [DataContract(Name = "CreateList", Namespace = "")]
    public class CreateListRequest : PostJsonRestRequest
    {
        [DataMember(Name = "list", IsRequired = true, EmitDefaultValue = false)]
        public List List { get; set; }

        public sealed override string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(List);
        }
    }
}