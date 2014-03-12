using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "GetSubscribableLists", Namespace = "")]
    public class GetSubscribableLists
    {
        [DataMember(Name = "listType", IsRequired = true)]
        public ListType ListType { get; set; }

        [DataMember(Name = "shareScopeCollection", IsRequired = true, EmitDefaultValue = false)]
        public ShareScopeCollection ShareScopeCollection { get; set; }
    }
}