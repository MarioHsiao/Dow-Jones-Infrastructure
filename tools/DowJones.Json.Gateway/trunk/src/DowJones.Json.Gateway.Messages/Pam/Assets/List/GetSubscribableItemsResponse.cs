using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "GetSubscribableItemsResponse", Namespace = "")]
    public class GetSubscribableItemsResponse
    {
        [DataMember(Name = "listPropertiesItemCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListPropertiesItemCollection ListPropertiesItemCollection { get; set; }
    }
}