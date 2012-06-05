using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{
    [DataContract(Name = "entity")]
    public class NewsEntityModel
    {
        [DataMember(Name = "entityType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType EntityType { get; set; }

        [DataMember(Name = "newsVolumes")]
        public Collection<NewsVolumeModel> NewsVolumes { get; set; }

        [DataMember(Name = "subjectCode")]
        public string SubjectCode { get; set; }

        [DataMember(Name = "searchQuery")]
        public SearchQueryModel SearchQuery { get; set; }
    }
}
