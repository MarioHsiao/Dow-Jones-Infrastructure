using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DowJones.Ajax.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Aggregrator.Services
{
    [ServiceContract(Name="IPageService",Namespace="")]
    public interface IPageService
    {
        [Description("Get Page By Id")]
        [WebGet(UriTemplate = "/Id/{format}?pageId={pageId}", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "GetPageById")]
        //[FaultContract(typeof(ErrorResponse))]
        Stream GetPageById(string format,string pageId);

        [Description("Get Page By Id")]
        [WebGet(UriTemplate = "/{format}?", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "GetPageList")]
        //[FaultContract(typeof(ErrorResponse))]
        Stream GetPageList(string format);

        [Description("Get Headlines")]
        [WebGet(UriTemplate = "/headlines/{format}?searchContextRef={searchContextRef}", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "GetHeadlines")]
        Stream GetHeadlines(string format, string searchContextRef);
    }
}
