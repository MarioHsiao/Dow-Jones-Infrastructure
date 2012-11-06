using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Net;
using System.ServiceModel.Web;
using System.Web;
using System.Xml.Serialization;
using System.ServiceModel;
using DowJones.API.Common.ExceptionHandling;
using DowJones.API.Common.Logging;
using System.Runtime.Remoting.Messaging;
using System.IO;
using System.Collections.Generic;
//using DowJones.API.Common.Configuration;
using System.Configuration;
using System.Compat.Web;
using DowJones.Infrastructure;
using DowJones.Factiva.Currents.Aggregrator;

namespace DowJones.API.Common.Utilities
{
    [DataContract(Name = "HttpMethodType", Namespace = "")]
    [XmlRoot(ElementName = "HttpMethodType")]
    public enum HttpMethodType
    {
        Get,
        Post,
        Put,
        Delete
    }

    /// <summary>
    /// Utility class to help the Host application
    /// </summary>
    public static class Web
    {
        private static Dictionary<string, CategoryDictionary> collectionCategoryDictionary = null;
        private static Dictionary<string, CategoryDictionary> collectionCodesDictionary = null;
       // private static ProviderDetailsList sourceAttributes = null;

        /// <summary>
        /// gets the local path
        /// </summary>
        /// <returns>returns the base directory</returns>
        static public string GetLocalPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Overrides the HTTP status.
        /// </summary>
        /// <returns></returns>
        public static bool OverrideHttpStatus()
        {
            var overrideHttpStatus = ApiFramework.OverrideHttpStatus;

            var steOverrideHttpStatus = GetQueryStringRequest(Constants.OverrideHttpStatus);

            if (!String.IsNullOrWhiteSpace(steOverrideHttpStatus) && steOverrideHttpStatus.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                overrideHttpStatus = true;

            return overrideHttpStatus;
        }

        /// <summary>
        /// gets the domain path
        /// </summary>
        /// <returns>returns the domain path</returns>
        static public string GetDomainPath(WebOperationContext webContext = null)
        {
            string strBaseURL = string.Empty;
            string strServiceName = string.Empty;
            string requestedHost = string.Empty;

            try
            {
                WebOperationContext context = null;

                if (WebOperationContext.Current != null)
                    context = WebOperationContext.Current;
                if (context == null && webContext != null)
                    context = webContext;

                if (context != null)
                {
                    if (context.IncomingRequest != null)
                    {
                        strServiceName = context.IncomingRequest.UriTemplateMatch.BaseUri.ToString();
                        requestedHost = context.IncomingRequest.Headers["Host"].ToString();
                        strBaseURL = strServiceName.Replace(context.IncomingRequest.UriTemplateMatch.BaseUri.Host, requestedHost);
                    }
                    // strBaseURL = strBaseURL.Remove(strBaseURL.IndexOf("2.0"));
                    // strBaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
                }
            }
            catch (Exception ex)
            {
                ApiLog.Logger.Error(ApiLog.FormatMemberInfoMessage(ex));
            }
            return strBaseURL;
        }

        /// <summary>
        /// gets the domain path
        /// </summary>
        /// <returns>returns the domain path</returns>
        static public string GetDomainPathRemoveServicePath(ref string serviceName, WebOperationContext webContext = null)
        {
            string strBaseURL = string.Empty;
            string requestedHost = string.Empty;

            try
            {
                WebOperationContext context = null;

                if (WebOperationContext.Current != null)
                    context = WebOperationContext.Current;
                if (context == null && webContext != null)
                    context = webContext;

                if (context != null)
                {
                    if (context.IncomingRequest != null)
                    {
                        serviceName = context.IncomingRequest.UriTemplateMatch.BaseUri.ToString();
                        requestedHost = context.IncomingRequest.Headers["Host"].ToString();
                        strBaseURL = serviceName.Replace(context.IncomingRequest.UriTemplateMatch.BaseUri.Host, requestedHost);
                    }
                    if (!string.IsNullOrWhiteSpace(strBaseURL) && strBaseURL.Contains("2.0"))
                        strBaseURL = strBaseURL.Remove(strBaseURL.IndexOf("2.0") + 3);
                    // strBaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
                }
            }
            catch (Exception ex)
            {
                ApiLog.Logger.Error(ApiLog.FormatMemberInfoMessage(ex));
            }
            return strBaseURL;
        }

        /// <summary>
        /// Gets the format value from the request context
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public static string GetFormatfromRequestContext(RequestContext requestContext)
        {
            string strFormat;
            string strAbsolutePath;

            try
            {
                var request = requestContext.RequestMessage;
                var requestProp = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

                strAbsolutePath = requestContext.RequestMessage.Headers.To.AbsolutePath;

                strFormat = strAbsolutePath.Substring(strAbsolutePath.LastIndexOf("/") + 1);

                //if (requestProp.Method.ToLower() == Constants.Post || requestProp.Method.ToLower() == Constants.Put)
                //{
                //    string receivedMessage = ((System.ServiceModel.Channels.Message)(request)).ToString().ToLower();
                //    if (receivedMessage.Contains(Constants.FormatStartTag))
                //    {
                //        int firstIndex = receivedMessage.IndexOf(Constants.FormatStartTag) + Constants.FormatStartTag.Length;
                //        int lastIndex = receivedMessage.IndexOf(Constants.FormatEndTag);
                //        strFormat = receivedMessage.Substring(firstIndex, lastIndex - firstIndex);
                //    }
                //}
                //else
                //{
                //    strFormat = strAbsolutePath.Substring(strAbsolutePath.LastIndexOf("/") + 1);
                //}
            }
            catch
            {
                strFormat = Constants.XmlFormat;
            }

            return strFormat;
        }

        /// <summary>
        /// This operation will retrieve a querystring value based on the input name.
        /// If no value is found then an empty string will be returned.
        /// </summary>
        /// <remarks>All values returned will be decoded</remarks>
        /// <param name="name">the name portion of the name/value pair used to find the value</param>
        /// <returns>string containing the value otherwise an empty string</returns>
        static public string GetRequest(string name)
        {
            string value = string.Empty;

            if (WebOperationContext.Current != null && OperationContext.Current != null)
            {
                if (OperationContext.Current.IncomingMessageHeaders != null)
                {
                    MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
                    HttpRequestMessageProperty request = (HttpRequestMessageProperty)properties[HttpRequestMessageProperty.Name];

                    if (request.QueryString.Length > 0)
                    {
                        char[] namValSeperator = new char[] { '=' };
                        string[] pair = request.QueryString.Split(new char[] { '&' });
                        foreach (string s in pair)
                        {
                            string[] namval = s.Split(namValSeperator);
                            if (namval[0].Equals(name, StringComparison.OrdinalIgnoreCase))
                            {
                                //APILog.Logger.Info(APILog.LogPrefix.END);
                                return Uri.UnescapeDataString(namval[1].Trim());
                            }
                        }
                    }
                }
            }
            //APILog.Logger.Info(APILog.LogPrefix.END);
            return value;
        }

        /// <summary>
        /// Gets the remote addr.
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteAddr()
        {
            if (WebOperationContext.Current != null && OperationContext.Current != null)
            {
                if (OperationContext.Current.IncomingMessageHeaders != null)
                {
                    var properties = OperationContext.Current.IncomingMessageProperties;
                    var remote = (RemoteEndpointMessageProperty)properties[RemoteEndpointMessageProperty.Name];
                    return remote.Address;
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string GetHeader(string name, WebOperationContext webContext = null)
        {
            var value = String.Empty;

            try
            {
                WebOperationContext context = null;

                if (WebOperationContext.Current != null)
                    context = WebOperationContext.Current;
                if (context == null && webContext != null)
                    context = webContext;

                if (context != null)
                {
                    if (context.IncomingRequest.Headers != null)
                    {
                        value = context.IncomingRequest.Headers.Get(name);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {

            }
            return value;
        }

        /// <summary>
        /// gets the domain path
        /// </summary>
        /// <returns>returns the domain path</returns>
        static public RequestFormat GetRequestFormat(WebOperationContext webContext=null)
        {
            string strRequestUrl;
            var requestFormat = RequestFormat.Xml;
            try
            {
                WebOperationContext context = null;

                if (WebOperationContext.Current != null)
                    context = WebOperationContext.Current;
                if (context == null && webContext != null)
                    context = webContext;

                if (context != null)
                {
                    if (context.IncomingRequest != null)
                    {
                        strRequestUrl = context.IncomingRequest.UriTemplateMatch.RequestUri.AbsolutePath;

                        if (!string.IsNullOrWhiteSpace(strRequestUrl) && strRequestUrl.EndsWith("json", StringComparison.CurrentCultureIgnoreCase))
                            requestFormat = RequestFormat.Json;
                    }
                }
            }
            catch (Exception ex)
            {
                ApiLog.Logger.Error(ApiLog.FormatMemberInfoMessage(ex));
            }
            return requestFormat;
        }

        /// <summary>
        /// gets the domain path
        /// </summary>
        /// <returns>returns the domain path</returns>
        static public string GetRequestUrl(WebOperationContext webContext = null)
        {
            var strRequestUrl = String.Empty;
            try
            {
                WebOperationContext context = null;

                if (WebOperationContext.Current != null)
                    context = WebOperationContext.Current;
                if (context == null && webContext != null)
                    context = webContext;

                if (context != null)
                {
                    if (context.IncomingRequest != null)
                    {
                        strRequestUrl = context.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ApiLog.Logger.Error(ApiLog.FormatMemberInfoMessage(ex));
            }
            return strRequestUrl;
        }

        /// <summary>
        /// gets the domain path
        /// </summary>
        /// <returns>returns the domain path</returns>
        static public string GetRequestUrlWithoutHost(WebOperationContext webContext = null)
        {
            var strRequestUrl = String.Empty;
            try
            {
                WebOperationContext context = null;

                if (WebOperationContext.Current != null)
                    context = WebOperationContext.Current;
                if (context == null && webContext != null)
                    context = webContext;

                if (context != null)
                {
                    if (context.IncomingRequest != null)
                    {
                        strRequestUrl = context.IncomingRequest.UriTemplateMatch.BaseUri.Scheme + "://" + 
                                        context.IncomingRequest.UriTemplateMatch.BaseUri.Host + 
                                        context.IncomingRequest.UriTemplateMatch.RequestUri.AbsolutePath + 
                                        context.IncomingRequest.UriTemplateMatch.RequestUri.Query;
                        //strRequestUrl = context.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ApiLog.Logger.Error(ApiLog.FormatMemberInfoMessage(ex));
            }
            return strRequestUrl;
        }

        /// <summary>
        /// Gets the query string request.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string GetQueryStringRequest(string name, WebOperationContext webContext=null)
        {
            var value = String.Empty;

            WebOperationContext context = null;

            if (WebOperationContext.Current != null)
                context = WebOperationContext.Current;
            if (context == null && webContext != null)
                context = webContext;

            if (context != null)
            {
                if (context.IncomingRequest.UriTemplateMatch != null)
                {
                    value = context.IncomingRequest.UriTemplateMatch.QueryParameters.Get(name);
                }
            }
            return value;
        }

        public static string GetQueryStringOrHeaderRequest(string name)
        {
            var value = GetQueryStringRequest(name);
            if (!String.IsNullOrWhiteSpace(value))
                return value;

            value = GetHeader(name);

            return value;
        }

        public static bool IsDebugOn()
        {
            var isDebugStr = GetQueryStringRequest("dbg");
            if (!String.IsNullOrWhiteSpace(isDebugStr) && isDebugStr.ToLower() == "true")
                return true;

            isDebugStr = GetHeader("dbg");
            if (!String.IsNullOrWhiteSpace(isDebugStr) && isDebugStr.ToLower() == "true")
                return true;

            return false;
        }

        public static bool IsReturnARMOn()
        {
            var isReturnArmStr = GetQueryStringRequest("returnarm");
            if (!String.IsNullOrWhiteSpace(isReturnArmStr) && isReturnArmStr.ToLower() == "true")
                return true;

            isReturnArmStr = GetHeader("returnarm");
            if (!String.IsNullOrWhiteSpace(isReturnArmStr) && isReturnArmStr.ToLower() == "true")
                return true;

            return false;
        }

        public static void SetAuditLog(IServiceResponse result)
        {
            if (IsDebugOn())
            {
                object objTrans = CallContext.GetData(Constants.AuditLog);
                // Use Audit Log
                AuditLog objAuditLog = (AuditLog)objTrans;
                result.AuditLog = (AuditLog)objTrans;
                // Clear data from Thread Context                
                CallContext.SetData(Constants.AuditLog, null);
            }
        }

        public static void SetARMValues(IServiceResponse result)
        {
            if (IsReturnARMOn())
            {
                object objARM = CallContext.GetData(Constants.ARMValues);
                // Use Audit Log
                ARMValues objARMValues = (ARMValues)objARM;
                result.ARMValues = (ARMValues)objARMValues;
                // Clear data from Thread Context                
                CallContext.SetData(Constants.ARMValues, null);
            }
        }

        //public static void SetAuditLog(BaseXMLResponse result)
        //{
        //    if (IsDebugOn())
        //    {
        //        object objTrans = CallContext.GetData(Constants.AuditLog);
        //        // Use Audit Log
        //        AuditLog objAuditLog = (AuditLog)objTrans;
        //        result.AuditLog = (AuditLog)objTrans;
        //        // Clear data from Thread Context                
        //        CallContext.SetData(Constants.AuditLog, null);
        //    }
        //}

        //public static void SetARMValues(BaseXMLResponse result)
        //{
        //    if (IsReturnARMOn())
        //    {
        //        object objARM = CallContext.GetData(Constants.ARMValues);
        //        // Use Audit Log
        //        ARMValues objARMValues = (ARMValues)objARM;
        //        result.ARMValues = (ARMValues)objARMValues;
        //        // Clear data from Thread Context                
        //        CallContext.SetData(Constants.ARMValues, null);
        //    }
        //}

        public static string GetCacheStateString()
        {
            var cacheState = GetQueryStringRequest("cacheState");
            if (!String.IsNullOrWhiteSpace(cacheState))
                return cacheState;

            cacheState = GetHeader("cacheState");
            if (!String.IsNullOrWhiteSpace(cacheState))
                return cacheState;

            return null;
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
                        string callBack = DowJones.API.Common.Utilities.Web.GetQueryStringRequest(Constants.CallbackParam);
                        outgoingResponse.Format = WebMessageFormat.Json;
                        outgoingResponse.ContentType = !string.IsNullOrWhiteSpace(callBack) ? ApiFramework.JsonContentWithCallbackType : ApiFramework.JsonContentType;
                        break;
                    case RequestFormat.Xml:
                        outgoingResponse.Format = WebMessageFormat.Xml;
                        outgoingResponse.ContentType = ApiFramework.XmlContentType;
                        break;
                }
            }
        }

        public static void SetResponseHeaders()
        {
            RequestFormat format = GetRequestFormat();
            SetResponseHeaders(format);
        }


        public static bool IsCacheEnabled()
        {
            var cacheEnabled = GetQueryStringRequest("cacheenabled");
            if (!String.IsNullOrWhiteSpace(cacheEnabled) && cacheEnabled.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                return false;
            return true;
        }

        public static bool ForceCacheRefresh()
        {
            var forceCacheRefresh = GetQueryStringRequest("ForceCacheRefresh");
            if (!String.IsNullOrWhiteSpace(forceCacheRefresh) && forceCacheRefresh.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                return true;
            return false;
        }

    }

    public class CategoryDictionary : Dictionary<string, string> { }
}
