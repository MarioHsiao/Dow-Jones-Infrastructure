using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ListSortBy", Namespace = "")]
    public enum ListSortBy
    {
        [EnumMember(Value = "CreatedDate")] CreatedDate = 0,

        [EnumMember(Value = "LastModifiedDate")] LastModifiedDate = 1,

        [EnumMember(Value = "Name")] Name = 2,
    }
}