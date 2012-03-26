using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Multimedia;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.Multimedia
{
    [DataContract(Name="multimediaPackage", Namespace="")]
    public class MultimediaPackage : IPackage
    {
        //private MediaContents mediaContents;

        [DataMember(Name = "guid",EmitDefaultValue = false)]
        public string Guid;

        [DataMember(Name = "mustPlayFromSource", EmitDefaultValue = false)]
        public MustPlayFromSource MustPlayFromSource { get; set; }

        [DataMember(Name = "mediaContents", EmitDefaultValue = false)]
        public MediaContents MediaContents { get; set; }
        
    }
}
