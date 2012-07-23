using DowJones.Attributes;
using DowJones.Converters;
using Newtonsoft.Json;

namespace DowJones.Search
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum InclusionFilter
    {
        None = 0,

        [AssignedToken("socialMedia")]
        SocialMedia,
    }
}