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

        [ClientProperty(Name = "displayTicker")]
        public bool DisplayTicker { get; set; }

        [ClientProperty(Name = "hitFont")]
        public string HitColor { get; set; }

        [ClientProperty(Name = "hitFont")]
        public string HitFont { get; set; }
        
        [ClientProperty(Name = "noMovementColor")]
        public string NoMovementColor { get; set; }

        [ClientProperty(Name = "negativeMovementColor")]
        public string NegativeMovementColor { get; set; }

        [ClientProperty(Name = "positiveMovementColor")]
        public string PositiveMovementColor { get; set; }

        [ClientProperty(Name = "scrollSize")]
        public int ScrollSize { get; set; }
    }
}
