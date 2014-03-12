using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ListSortBy", Namespace = "")]
    public enum ListSortBy
    {
        [EnumMember] CreatedDate = 0,

        [EnumMember] LastModifiedDate = 1,

        [EnumMember] Name = 2,
    }
}