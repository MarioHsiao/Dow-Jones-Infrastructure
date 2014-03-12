using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "GetListsPropertiesListResponse", Namespace = "")]
    public class GetListsPropertiesListResponse
    {
        [DataMember(Name = "listPropertiesItemCollection", IsRequired = true, EmitDefaultValue = false)]
        public ListPropertiesItemCollection ListPropertiesItemCollection { get; set; }
    }
}