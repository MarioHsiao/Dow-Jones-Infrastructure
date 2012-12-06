using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DowJones.Factiva.Currents.Aggregrator.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContent" in both code and config file together.
    [ServiceContract(Name = "IContentService", Namespace = "")]
    public interface IContent
    {
        [Description("Get Headlines")]
        [WebGet(UriTemplate = "/headlines/{format}?searchContextRef={searchContextRef}", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "GetHeadlines")]
        Stream GetHeadlines(string format, string searchContextRef);

        [Description("Get Headlines By AccessionNumber")]
        [WebGet(UriTemplate = "/headlines/an/{format}?an={accessionNumber}", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "GetHeadlinesByAccessionNumber")]
        Stream GetHeadlinesByAccessionNumber(string accessionNumber, string format);
    }
}
