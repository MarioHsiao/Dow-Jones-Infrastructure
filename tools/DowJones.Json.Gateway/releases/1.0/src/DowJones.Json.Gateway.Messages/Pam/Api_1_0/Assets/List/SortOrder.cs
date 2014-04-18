using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "SortOrder", Namespace = "")]
    public enum SortOrder
    {
        [EnumMember(Value = "Ascending")] Ascending = 0,

        [EnumMember(Value = "Descending")] Descending = 1,
    }
}