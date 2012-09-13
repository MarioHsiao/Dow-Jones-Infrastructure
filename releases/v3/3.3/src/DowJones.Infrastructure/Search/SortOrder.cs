using DowJones.Converters;
using Newtonsoft.Json;
using DowJones.Attributes;

namespace DowJones.Search
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum SortOrder
    {
        [AssignedToken("publicationDateMostRecentFirst")]
        [JsonProperty("recent")]
        PublicationDateMostRecentFirst = 0,

        [AssignedToken("publicationDateOldestFirst")]
        [JsonProperty("oldest")]
        PublicationDateOldestFirst,
        
        [AssignedToken("relevance")]
        [JsonProperty("relevance")]
        Relevance,

        [AssignedToken("arrivalTime")]
        ArrivalTime,
    }
}