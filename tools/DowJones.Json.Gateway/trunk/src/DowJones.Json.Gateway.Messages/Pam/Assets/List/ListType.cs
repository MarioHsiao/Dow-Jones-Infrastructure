using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ListType", Namespace = "")]
    public enum ListType
    {
        [EnumMember] AuthorList = 0,

        [EnumMember] IndustryList = 1,

        [EnumMember] RegionList = 2,

        [EnumMember] SubjectList = 3,

        [EnumMember] ExecutiveList = 4,
    }
}