using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "GetListByIDResponse", Namespace = "")]
    public class GetListByIdResponse
    {
        [DataMember(Name="list", IsRequired = true, EmitDefaultValue = false)]
        public List List { get; set; }
    }
}