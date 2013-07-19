using DowJones.Attributes;
using DowJones.Converters;
using Newtonsoft.Json;

namespace DowJones.Search
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum SearchFreeTextArea
    {
        [AssignedToken("fullArticle")]
        [AssignedString("")]
        FullArticle,

        [AssignedToken("headlineLeadPara")]
        [AssignedString("HLP")]
        HeadlineAndLeadParagraph,

        [AssignedToken("headline")]
        [AssignedString("HL")]
        Headline,

        [AssignedToken("cmAuthor")]
        [AssignedString("BY")]
        Author,
    }
}