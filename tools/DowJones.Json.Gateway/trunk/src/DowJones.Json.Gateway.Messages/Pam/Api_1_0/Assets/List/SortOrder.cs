using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "SortOrder", Namespace = "")]
    public enum SortOrder
    {
        [EnumMember] Ascending = 0,

        [EnumMember] Descending = 1,
    }
}