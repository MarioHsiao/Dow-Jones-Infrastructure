using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Aggregrator
{
    public class Common
    {
        static string enToken = System.Configuration.ConfigurationManager.AppSettings["EncryptedToken"];
        static string basePath = System.Configuration.ConfigurationManager.AppSettings["DasboardApiBasePath"];

        /// <summary>
        /// Gets the query string request.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string GetQueryStringRequest(string name, WebOperationContext webContext = null)
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

        public static string GetPageByIdData(string pageId)
        {
            string url =
                string.Format(
                    "{0}/Pages/1.0/id/json?pageId={1}&encryptedToken={2}",                   
                    basePath,
                    pageId,
                    enToken
                  );
            return GetData(url);
        }

        

        public static string GetPageListData()
        {
            string url =
                string.Format(
                    "{0}/Pages/1.0/list/json?encryptedToken={1}",
                    basePath,
                    enToken
                  );
            return GetData(url);
        }

        public static string GetData(string url)
        {
            var webRequest = WebRequest.Create(new Uri(url));
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.Method = "GET";
            string result = string.Empty;
            using (var httpResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }
            return result;
        }

        public static Stream SerializeObjectToStream(object obj, DowJones.Factiva.Currents.Aggregrator.Services.PageService.RequestFormat format, bool wrapCallback = true)
        {
            string OPNAME = "SerializeObjectToStream";
           // ApiLog.Logger.InfoFormat(ApiLog.LogPrefix.Value, string.Format("Start:{0} - format:{1}, obj:{2}", OPNAME, System.Enum.GetName(format.GetType(), format), obj.GetType().ToString()));

            MemoryStream stream = new MemoryStream();

            ////Add ARM values to the response object if returnARM=true
            //ARMUtility.GetARMValuesFromContext(obj);

            //switch (format)
            //{
              //  case RequestFormat.Json:
                    JsonSerializer(stream, obj);

            //        break;
            //    case RequestFormat.Xml:
            //        new DataContractSerializer(obj.GetType()).WriteObject(stream, obj);
            //        break;
            //    default:
            //        new DataContractSerializer(obj.GetType()).WriteObject(stream, obj);
            //        break;
            //}

            stream.Position = 0;
           // ApiLog.Logger.InfoFormat(ApiLog.LogPrefix.Value, string.Format("End:{0} - format:{1}, obj:{2}", OPNAME, System.Enum.GetName(format.GetType(), format), obj.GetType().ToString()));

            // Handling Callback
            if (format == DowJones.Factiva.Currents.Aggregrator.Services.PageService.RequestFormat.Json && wrapCallback)
            {
                string callback = GetRequest("callback");
                if (!string.IsNullOrEmpty(callback))
                    stream = WrapStreamWithCallback(stream, callback);
            }
            return stream;
        }

        public static MemoryStream WrapStreamWithCallback(MemoryStream stream, string callback)
        {
            MemoryStream ms = new MemoryStream();
           // ApiLog.Logger.Info(ApiLog.LogPrefix.Start);

            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            string data = sr.ReadToEnd();
            string callbackString = string.Format("{0}({1});", callback, data);
            UTF8Encoding enc = new UTF8Encoding();
            Byte[] b = enc.GetBytes(callbackString);
            ms.Write(b, 0, b.Length);
            ms.Position = 0;
           // ApiLog.Logger.Info(ApiLog.LogPrefix.End);

            return ms;
        }

        public static void JsonSerializer(Stream stream, object obj)
        {
            Newtonsoft.Json.JsonSerializer json = new JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            json.DefaultValueHandling = DefaultValueHandling.Ignore;
            json.ObjectCreationHandling = ObjectCreationHandling.Replace;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //json.TypeNameHandling = TypeNameHandling.Objects;  //Not Needed
            //TODO: Check, Dave (Decorate infrastructure with attributes and remove following line) 
            json.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            //Serialize Dates in ISO Format
            json.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());

            //json.Converters.Add(new JavaScriptDateTimeConverter());
            StringWriter sw = new StringWriter();
            Newtonsoft.Json.JsonTextWriter writer = new JsonTextWriter(sw);
            writer.QuoteChar = '"';
            json.Serialize(writer, obj);
            string output = sw.ToString();
            writer.Close();
            sw.Close();

            // Changing encoding to fix serialization issue 
            //MemoryStream ms = new MemoryStream(ASCIIEncoding.GetEncoding(0).GetBytes(output));
            MemoryStream ms = new MemoryStream(ASCIIEncoding.GetEncoding("utf-8").GetBytes(output));
            ms.Position = 0;
            stream.Position = 0;
            ms.WriteTo(stream);
            ms.Close();
        }

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
    }
}
