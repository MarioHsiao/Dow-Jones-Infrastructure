using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService
{
    [DataContract(Name = "newsPagesListServiceResult", Namespace = "")]
    public class NewsPagesListServiceResult : AbstractServiceResult
    {

		
        [DataMember(Name = "package")]
        public NewsPagesListPackage Package { get; set; }

        
    }
}
