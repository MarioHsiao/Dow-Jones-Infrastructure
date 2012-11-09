using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using DowJones.Factiva.Currents.Common;
using DowJones.Factiva.Currents.Common.ExceptionHandling;
using DowJones.Factiva.Currents.Common.Logging;
using CurrentUtilities = DowJones.Factiva.Currents.Common.Utilities;
using Newtonsoft.Json;
using DowJones.Factiva.Currents.Common.Utilities;

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
            byte[] byteArray = Encoding.UTF8.GetBytes(result);
            MemoryStream stream = new MemoryStream(byteArray);
            CurrentUtilities.Web.SetResponseHeaders(EnumConverter<RequestFormat>.ConvertStringToEnum(format));
            ApiLog.Logger.Info(ApiLog.LogPrefix.End);
            return stream;
        }

        public System.IO.Stream GetPageList(string format)
        {
          //  ApiLog.Logger.Info(ApiLog.LogPrefix.Start);
            string result = Common.GetPageListData(format);
            byte[] byteArray = Encoding.Default.GetBytes(result);
            MemoryStream stream = new MemoryStream(byteArray);
            CurrentUtilities.Web.SetResponseHeaders(EnumConverter<RequestFormat>.ConvertStringToEnum(format));
          //  ApiLog.Logger.Info(ApiLog.LogPrefix.End);
            return stream;
        }
    }
}
