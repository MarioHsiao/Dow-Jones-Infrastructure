using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using DowJones.Exceptions;
using DowJones.Globalization;
using DowJones.Properties;
using log4net;

namespace DowJones.Managers.AutomatedTranslation.Providers.LanguageWeaver
{
    class ServiceWrapper
    {
        public static LanguagePairCollection GetApprovedLanguagePairs()
        {
            LanguagePairCollection collection;

            HttpWebRequest request = BuildGetApprovedLanguagePairsRequest();
            LogRequest(request);

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                collection = ProcessGetApprovedLanguagePairsRequest(response);

                LogResponse(response, collection);
            }
            catch (WebException webException)
            {
                Dictionary<string, string> errorResult = ProcessErrorResponse(webException.Response);
                LogErrorResponse(webException.Response, errorResult);

                throw GetCorrespondingException(errorResult["message"]);
            }
            catch(Exception exception)
            {
                LogException(exception);

                throw;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return collection;
        }

        public static Dictionary<string, string> SubmitNonBlockingTranslationRequest(string sourceText, TextFormat format, LanguagePair pair)
        {
            Dictionary<string, string> result;

            var request = BuildSubmitNonBlockingTranslationRequest(sourceText, format, pair);

            var context = new Dictionary<string, object>
                              {
                                  {"SourceText", sourceText}
                              };
            LogRequest(request, context);

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                result = ProcessSubmitNonBlockingTranslationResponse(response);

                LogResponse(response, result);
            }
            catch (WebException webException)
            {
                Dictionary<string, string> errorResult = ProcessErrorResponse(webException.Response);
                LogErrorResponse(webException.Response, errorResult);

                throw GetCorrespondingException(errorResult["message"]);
            }
            catch (Exception exception)
            {
                LogException(exception);

                throw;
            }
            finally
            {
                if(response != null)
                    response.Close();
            }
            
            return result;
        }

        public static Dictionary<string, string> QueryNonBlockingTranslationRequest(string uri)
        {
            Dictionary<string, string> result;

            var request = BuildQueryNonBlockingTranslationRequest(uri);
            var context = new Dictionary<string, object> {{"Uri", uri}};

            LogRequest(request, context);

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                result = ProcessQueryNonBlockingTranslationResponse(response);

                LogResponse(response, result);
            }
            catch (WebException webException)
            {
                Dictionary<string, string> errorResult = ProcessErrorResponse(webException.Response);
                LogErrorResponse(webException.Response, errorResult);

                throw GetCorrespondingException(errorResult["message"]);
            }
            catch (Exception exception)
            {
                LogException(exception);

                throw;
            }
            finally
            {
                if(response != null)
                    response.Close();
            }
            return result;
        }

        #region Helpers
        private static HttpWebRequest BuildGetApprovedLanguagePairsRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(s_ServiceUrl + s_UserRequestUri);

            var date = DateTime.Now;
            var signature = GetSignature("GET", date, s_UserRequestUri, s_ApiKey);

            request.Headers.Add("LW-Date", date.ToString());
            request.Headers.Add("Authorization", "LWA:" + s_UserId + ":" + signature);
            request.Method = "GET";
            if (!String.IsNullOrEmpty(s_WebProxy))
            {
                request.Proxy = new WebProxy(s_WebProxy);
            }

            return request;
        }

        private static LanguagePairCollection ProcessGetApprovedLanguagePairsRequest(WebResponse response)
        {
            var collection = new LanguagePairCollection();

            using (var responseStream = response.GetResponseStream())
            {
                using (var xmlTextReader = new XmlTextReader(responseStream))
                {
                    xmlTextReader.MoveToElement();

                    while (xmlTextReader.ReadToFollowing("lpid"))
                    {
                        var languagePairId = int.Parse(xmlTextReader.ReadString());

                        xmlTextReader.ReadToFollowing("source_language");
                        var sourceLanguage = xmlTextReader.ReadString();

                        xmlTextReader.ReadToFollowing("target_language");
                        var targetLanguage = xmlTextReader.ReadString();

                        var pair = new LanguagePair(languagePairId, sourceLanguage, targetLanguage);

                        if (!collection.Contains(pair))
                        {
                            collection.Add(pair);
                        }
                    }
                }
            }

            return collection;
        }

        private static HttpWebRequest BuildSubmitNonBlockingTranslationRequest(string sourceText, TextFormat sourceFormat, LanguagePair pair)
        {
            var uri = s_NonBlockingTranslationUri + pair + "/lpid=" + pair.Id + "/input_format=" + sourceFormat;

            var postDataString = "source_text=" + HttpUtility.UrlEncode(sourceText);
            var data = Encoding.UTF8.GetBytes(postDataString);

            Log.InfoFormat("source text:{0}", data);
            var request = (HttpWebRequest)WebRequest.Create(s_ServiceUrl + uri);

            var date = DateTime.Now;
            var signature = GetSignature("POST", date, uri, s_ApiKey);

            request.Headers.Add("LW-Date", date.ToString());
            request.Headers.Add("Authorization", "LWA:" + s_UserId + ":" + signature);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            if (!String.IsNullOrEmpty(s_WebProxy))
            {
                request.Proxy = new WebProxy(s_WebProxy);
            }

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }

            return request;
        }

        private static Dictionary<string, string> ProcessSubmitNonBlockingTranslationResponse(WebResponse response)
        {
            var result = new Dictionary<string, string>();

            using (var responseStream = response.GetResponseStream())
            {
                using (var xmlTextReader = new XmlTextReader(responseStream))
                {
                    xmlTextReader.MoveToElement();

                    xmlTextReader.ReadToFollowing("retrieval_url");

                    result.Add("uri", xmlTextReader.ReadString()); 
                }
            }

            return result;
        }

        private static HttpWebRequest BuildQueryNonBlockingTranslationRequest(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            var date = DateTime.Now;
            var signature = GetSignature("GET", date, uri.Replace(s_ServiceUrl, ""), s_ApiKey);
            request.Headers.Add("LW-Date", date.ToString());
            request.Headers.Add("Authorization", "LWA:" + s_UserId + ":" + signature);
            request.Method = "GET";
            if (!String.IsNullOrEmpty(s_WebProxy))
            {
                request.Proxy = new WebProxy(s_WebProxy);
            }

            return request;
        }

        private static Dictionary<string, string> ProcessQueryNonBlockingTranslationResponse(WebResponse response)
        {
            var result = new Dictionary<string, string>();

            using (var responseStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    using (var xmlTextReader = new XmlTextReader(reader))
                    {
                        xmlTextReader.MoveToElement();
                        xmlTextReader.ReadToFollowing("state");

                        result.Add("status", xmlTextReader.ReadString());

                        xmlTextReader.ReadToFollowing("translation");
                        result.Add("translatedText", xmlTextReader.ReadString());
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, string> ProcessErrorResponse(WebResponse response)
        {
            var result = new Dictionary<string, string>();
            using (var responseStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    using (var xmlTextReader = new XmlTextReader(reader))
                    {
                        xmlTextReader.MoveToElement();
                        xmlTextReader.ReadToFollowing("error");

                        result["message"] = xmlTextReader.ReadString();
                    }
                }
            }

            return result;
        }

        private static string GetSignature(string requestType, DateTime requestDate, string uri, string apiKey)
        {
            string message = requestType + "\n" + requestDate + "\n" + uri;

            byte[] apiKeyBytes = Encoding.ASCII.GetBytes(apiKey);
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);

            var hmacsha1 = new HMACSHA1(apiKeyBytes);

            return Convert.ToBase64String((hmacsha1.ComputeHash(messageBytes)));
        }

        private static DowJonesUtilitiesException GetCorrespondingException(string errorMessage)
        {
            DowJonesUtilitiesException exception;

            switch(errorMessage)
            {
                case "Invalid API Key":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidApiKeyException);
                    break;
                case "Invalid User":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidUserException);
                    break;
                case "Missing Authorization HTTP Header":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.MissingAuthorizationHttpHeaderexception);
                    break;
                case "Authorization HTTP Header Formatting Error":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.AuthorizationHttpHeaderFormattingException);
                    break;
                case "Authorization Signature Mismatch":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.AuthorizationSignatureMismatchException);
                    break;
                case "Missing LW Date HTTP Header":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.MissingLwDateHttpHeaderException);
                    break;
                case "Authorization Error":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.AuthorizationErrorException);
                    break;
                case "User Not Authorized For This Language Pair":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.UserNotAuthorizedForThisLanguagePairException);
                    break;
                case "Missing Source Text":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.MissingSourceTextException);
                    break;
                case "Missing Source File":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.MissingSourceFileException);
                    break;
                case "Missing Source URL":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.MissingSourceUrlException);
                    break;
                case "Input Source Text Size Greater Than Maximum Allowable Amount":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.InputSourceTextGreaterThanMaximumAllowableAmountException);
                    break;
                case "LPID Not Specified":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.LpidNotSpecifiedException);
                    break;
                case "Invalid Source Language ID":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSourceLanguageIDException);
                    break;
                case "Invalid Target Language ID":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidTargetLanguageIDException);
                    break;
                case "Language Pair Not Found":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.LanguagePairNotFoundException);
                    break;
                case "Invalid Job ID":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidJobIDException);
                    break;
                case "No such job":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.NoSuchJobException);
                    break;
                case "Invalid API Key for Specified Job ID":
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidApiKeyForSpecifiedJobIDException);
                    break;
                default:
                    exception = new DowJonesUtilitiesException(DowJonesUtilitiesException.UnknownTranslationProviderException);
                    break;
            }

            return exception;
        }

        private static void LogRequest(WebRequest request)
        {
            DoLogRequest(request, null);
        }

        private static void LogRequest(WebRequest request, Dictionary<string, object> context)
        {
           DoLogRequest(request, context);
        }

        private static void DoLogRequest(WebRequest request, Dictionary<string, object> context)
        {
            if (!s_Log.IsInfoEnabled)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendFormat("Uri: {0};", request.RequestUri);
            builder.AppendFormat(" Proxy: {0};", request.Proxy);
            builder.AppendFormat(" Method: {0};", request.Method);
            builder.Append(" Headers:");

            var headers = request.Headers;
            foreach (string key in headers.AllKeys)
            {
                builder.AppendFormat("{0}:{1},", key, headers[key]);
            }
            builder.Append(";");

            if (context != null)
            {
                foreach (var keyValuePair in context.Where(keyValuePair => keyValuePair.Value != null))
                {
                    builder.AppendFormat("{0}:{1}", keyValuePair.Key, keyValuePair.Value);
                }
            }

            s_Log.Info(builder.ToString());
        }

        private static void LogResponse(HttpWebResponse response, Dictionary<string, string> result)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Uri: {0};", response.ResponseUri);
            builder.AppendFormat("Status Code: {0};", response.StatusCode);
            builder.AppendFormat("Status Description: {0};", response.StatusDescription);

            foreach (var keyValuePair in result.Where(keyValuePair => keyValuePair.Value != null))
            {
                builder.AppendFormat("{0}:{1};", keyValuePair.Key, keyValuePair.Value);
            }

            if (s_Log.IsInfoEnabled)
            {
                s_Log.Info(builder.ToString());
            }
        }

        private static void LogResponse(HttpWebResponse response, IEnumerable<LanguagePair> collection)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Uri: {0};", response.ResponseUri);
            builder.AppendFormat("Status Code: {0};", response.StatusCode);
            builder.AppendFormat("Status Description: {0};", response.StatusDescription);

            foreach (LanguagePair pair in collection)
            {
                builder.AppendFormat("{0}: {1}.{2}, {3};", pair.GetType().Name, pair.FromLanguage, pair.IntoLanguage, pair.Id);
            }

            if (s_Log.IsInfoEnabled)
            {
                s_Log.Info(builder.ToString());
            }
        }

        private static void LogErrorResponse(WebResponse response, Dictionary<string, string> result)
        {
            if (!s_Log.IsErrorEnabled)
            {
                return;
            }

            var builder = new StringBuilder();

            builder.AppendFormat("Uri: {0};", response.ResponseUri);

            foreach (var keyValuePair in result.Where(keyValuePair => keyValuePair.Value != null))
            {
                builder.AppendFormat("{0}:{1};", keyValuePair.Key, keyValuePair.Value);
            }

            s_Log.Error(builder.ToString());
        }

        private static void LogException(Exception exception)
        {
            if(!s_Log.IsErrorEnabled)
            {
                return;
            }

            s_Log.Error("LW Service Wrappper failed.", exception);
        }

        #endregion

        private static readonly ILog s_Log = LogManager.GetLogger(typeof(ServiceWrapper));

        private static readonly string s_UserId = Settings.Default.LanguageWeaverUserId;
        private static readonly string s_ApiKey = Settings.Default.LanguageWeaverApiKey;
        private static readonly string s_ServiceUrl = Settings.Default.LanguageWeaverServiceUrl;
        private static readonly string s_NonBlockingTranslationUri = Settings.Default.LanguageWeaverNonBlockingTransactonUri;
        private static readonly string s_UserRequestUri = Settings.Default.LanguageWeaverUserRequestUri;
        private static readonly string s_WebProxy = Settings.Default.WebResourcesProxy;

        protected static ILog Log
        {
            get { return s_Log; }
        }
    }
}
