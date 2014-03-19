using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [DataContract(Name = "SetShareProperties", Namespace = "")]
    [ServicePath("pamapi/1.0/List.svc")]
    public class SetSharePropertiesRequest : IPostJsonRestRequest
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "shareProperties", IsRequired = true, EmitDefaultValue = false)]
        public ShareProperties ShareProperties { get; set; }

        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}