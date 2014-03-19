using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/List.svc")]
    [DataContract(Name = "UpdateListName", Namespace = "")]
    public class UpdateListNameRequest : IPostJsonRestRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = false)]
        public string Name { get; set; }

        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}