using DowJones.Attributes;
using DowJones.Converters;
using Newtonsoft.Json;

namespace DowJones.Search
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum NewsFilterCategory
    {
        Unknown,

        [AssignedToken("companies")]
        Company,

        [AssignedToken("executives")]
        Executive,

        [AssignedToken("authors")]
        Author,

        [AssignedToken("industries")]
        Industry,

        [AssignedToken("subjects")]
        Subject,

        [AssignedToken("regions")]
        Region,

        [AssignedToken("sources")]
        Source,

        [AssignedToken("dateRange")]
        DateRange,

        [AssignedToken("keywords")]
        Keyword,

        [AssignedToken("group")]
        Group,
    }
}