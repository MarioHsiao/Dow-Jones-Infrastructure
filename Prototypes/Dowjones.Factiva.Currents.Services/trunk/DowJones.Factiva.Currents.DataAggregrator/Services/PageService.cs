using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Aggregrator.Services
{
    public class PageService:IPageService
    {
        public System.IO.Stream GetPageById(string format,string pageId)
        {
            string result = Common.GetPageByIdData(pageId);
            
            byte[] byteArray = Encoding.ASCII.GetBytes( result);
            MemoryStream stream = new MemoryStream( byteArray );
            SetResponseHeaders(RequestFormat.Json);
            return stream;
        }

        public System.IO.Stream GetPageList(string format)
        {
            string result = Common.GetPageListData();
            byte[] byteArray = Encoding.ASCII.GetBytes(result);
            MemoryStream stream = new MemoryStream(byteArray);
            SetResponseHeaders(RequestFormat.Json);
            return stream;
        }

        public static void SetResponseHeaders(RequestFormat format)
        {
            if (WebOperationContext.Current != null && OperationContext.Current != null)
            {
                OutgoingWebResponseContext outgoingResponse = WebOperationContext.Current.OutgoingResponse;

                if (string.IsNullOrWhiteSpace(outgoingResponse.Headers["Access-Control-Allow-Origin"]))
                    outgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

                // Ovveriding Http Status
                //if (Utilities.Web.OverrideHttpStatus())
                //    outgoingResponse.StatusCode = HttpStatusCode.OK;

                //Set format and content type for the OutgoingResponse of the WebOperationContext
                switch (format)
                {
                    case RequestFormat.Json:
                        string callBack = Common.GetQueryStringRequest(Constants.CallbackParam);
                        outgoingResponse.Format = WebMessageFormat.Json;
                        outgoingResponse.ContentType = !string.IsNullOrWhiteSpace(callBack) ? Constants.JsonContentWithCallbackType : Constants.JsonContentType;
                        break;
                    case RequestFormat.Xml:
                        outgoingResponse.Format = WebMessageFormat.Xml;
                        outgoingResponse.ContentType = Constants.XmlContentType;
                        break;
                }
            }
        }

        public enum RequestFormat
        {
            Xml,
            Json,
            Soap
        }
    }
}
