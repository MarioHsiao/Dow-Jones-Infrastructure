using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ListType", Namespace = "")]
    public enum ListType
    {
        [EnumMember(Value = "AuthorList")] AuthorList = 1,

        [EnumMember(Value = "IndustryList")] IndustryList = 2,

        [EnumMember(Value = "RegionList")] RegionList = 3,

        [EnumMember(Value = "SubjectList")] SubjectList = 4,

        [EnumMember(Value = "ExecutiveList")] ExecutiveList = 5,
    }
}