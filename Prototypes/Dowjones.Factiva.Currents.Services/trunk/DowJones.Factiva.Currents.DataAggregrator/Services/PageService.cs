using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DowJones.Factiva.Currents.Common;
using DowJones.Factiva.Currents.Common.ExceptionHandling;
using DowJones.Factiva.Currents.Common.Logging;
using DowJones.Factiva.Currents.Common.Utilities;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Aggregrator.Services
{
    public class PageService:IPageService
    {
        public System.IO.Stream GetPageById(string format,string pageId)
        {
            ApiLog.Logger.Info(ApiLog.LogPrefix.Start);
            ApiLog.Logger.InfoFormat(ApiLog.LogPrefix.Value, string.Format("request={0}",DowJones.Factiva.Currents.Common.Utilities.Web.GetRequestUrl()));
            if (string.IsNullOrWhiteSpace(pageId))
                throw ExceptionHandlerUtility.GetWebFaultByServiceException(ErrorConstants.InvalidPageId, HttpStatusCode.BadRequest);
            string result = Common.GetPageByIdData(pageId, format);
            
            byte[] byteArray = Encoding.Default.GetBytes( result);
            MemoryStream stream = new MemoryStream( byteArray );
            SetResponseHeaders(EnumConverter<RequestFormat>.ConvertStringToEnum(format));
            ApiLog.Logger.Info(ApiLog.LogPrefix.End);
            return stream;
        }

        public System.IO.Stream GetPageList(string format)
        {
          //  ApiLog.Logger.Info(ApiLog.LogPrefix.Start);
            string result = Common.GetPageListData(format);
            byte[] byteArray = Encoding.Default.GetBytes(result);
            MemoryStream stream = new MemoryStream(byteArray);
            SetResponseHeaders(EnumConverter<RequestFormat>.ConvertStringToEnum(format));
          //  ApiLog.Logger.Info(ApiLog.LogPrefix.End);
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
