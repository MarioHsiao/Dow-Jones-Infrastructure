using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{
    [DataContract(Name = "newRadar")]
    public class NewsRadarModel : ViewComponentModel
    {
        [ClientData]
        [DataMember(Name="parentNewsEntities")]
        public Collection<EntityModel> Data { get; set; }
    }
}
