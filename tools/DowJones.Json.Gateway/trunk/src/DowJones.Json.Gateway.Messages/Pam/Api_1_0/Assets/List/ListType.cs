using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ListType", Namespace = "")]
    public enum ListType
    {
        [EnumMember(Value = "AuthorList")] AuthorList = 0,

        [EnumMember(Value = "IndustryList")] IndustryList = 1,

        [EnumMember(Value = "RegionList")] RegionList = 2,

        [EnumMember(Value = "SubjectList")] SubjectList = 3,

        [EnumMember(Value = "ExecutiveList")] ExecutiveList = 4,
    }
}