using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{
    [DataContract(Name = "entity")]
    public class EntityModel
    {
        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "ownershipType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OwnershipType OwnershipType { get; set; }

        [DataMember(Name = "totalNewsItems")]
        public Collection<NewsVolumeModel> TotalNewsItems { get; set; }

        [DataMember(Name = "maxNewsItems")]
        public Collection<NewsVolumeModel> MaxNewsItems { get; set; }

        [DataMember(Name = "newsEntities")]
        public Collection<NewsEntityModel> NewsEntities { get; set; }

        [DataMember(Name = "instrumentReference")]
        public InstrumentReferenceModel InstrumentReference { get; set; }

        [DataMember(Name = "isNewsCoded")]
        public bool IsNewsCoded { get; set; }

        [DataMember(Name = "newsSearch")]
        public string NewsSearch { get; set; }

    }
}
