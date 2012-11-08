using System.Runtime.Serialization;
using System.Xml.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Sources.Results
{
    [DataContract(Name = "sourcesNewsPageModuleServiceResult", Namespace = "")]
    public class SourcesNewsPageModuleServiceResult :
        AbstractModuleServiceResult<SourcesNewsPageServicePartResult<SourcePackage>, SourcePackage, SourcesNewspageModule>
    {
		[DataMember(Name = "sources")]
		[XmlElement(ElementName = "sources")]
		public SourceCollection SourceList { get; set; }
    }
}