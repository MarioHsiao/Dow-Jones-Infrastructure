using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/Lists.svc")]
    [DataContract(Name = "DeleteList", Namespace = "")]
    public class DeleteListRequest : IDeleteJsonRestRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(Id);
        }
    }
}