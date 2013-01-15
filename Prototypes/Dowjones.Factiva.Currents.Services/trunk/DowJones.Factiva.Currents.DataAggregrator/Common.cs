using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using DowJones.Factiva.Currents.Common;
using DowJones.Factiva.Currents.Common.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DowJones.Factiva.Currents.Aggregrator
{
    public class Common
    {
        static CacheManager cacheManager = new CacheManager();
        static string enToken = System.Configuration.ConfigurationManager.AppSettings["EncryptedToken"];
        static string basePath = System.Configuration.ConfigurationManager.AppSettings["DasboardApiBasePath"];
        static string restAPIBasePath = System.Configuration.ConfigurationManager.AppSettings["RestApiBasePath"];
        static int maxResultsToReturn = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxResultsToReturn"]);
        static int maxHeadlinesToReturn = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxHeadlinesToReturn"]);
        
        static int maxEntitiesToReturn = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxEntitiesToReturn"]);
        static int maxPartsToReturn = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxPartsToReturn"]);
        static string timeFrame = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TimeFrame"]);
        static string entityType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityType"]);
        static string sourceGenre = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SourceGenre"]);
        static string daysRange = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DaysRange"]);

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

        public static string GetPageByIdData(string pageId, string format)
        {
            try
            {
                string key = pageId + "_" + format + "_" + enToken;
                string cahceData = cacheManager.GetCache(key) != null ? cacheManager.GetCache(key).ToString() : null;
                if (!string.IsNullOrEmpty(cahceData))
                {
                    return cahceData;
                }

                string url =
                    string.Format(
                        "{0}/Pages/1.0/id/" + format + "?pageId={1}&encryptedToken={2}",
                        basePath,
                        pageId,
                        enToken
                      );

                Dictionary<string, string> pageModuleList = new Dictionary<string, string>();
                string pageData = GetData(url);
                pageModuleList.Add("newsPage", pageData);
                string data = string.Empty;
                if (format.Equals(RequestFormat.Json.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    GetModuleDataForJson(pageModuleList, pageId);
                    data =  GetSerializedPagesModulesForJson(pageModuleList);
                    cacheManager.Add(key,data);
                   
                }
                else
                {
                    GetModuleDataForXml(pageModuleList, pageId);
                    data = GetSerializedPagesModulesForXml(pageModuleList);
                    cacheManager.Add(key, data);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetPageListData(string format)
        {
            try
            {
                string key = format + "_" + enToken;
                string cahceData = cacheManager.GetCache(key) != null ? cacheManager.GetCache(key).ToString() : null;
                if (!string.IsNullOrEmpty(cahceData))
                {
                    return cahceData;
                }
                string url =
                    string.Format(
                        "{0}/Pages/1.0/list/" + format + "?encryptedToken={1}",
                        basePath,
                        enToken
                      );
                string data = GetData(url);
                cacheManager.Add(key, data);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        public static string GetHeadlines(string format, string searchContextRef)
        {
            try
            {
                int firstResultToReturn = 0;
                string key = format + "_" + enToken + "_" + searchContextRef;
                string cahceData = cacheManager.GetCache(key) != null ? cacheManager.GetCache(key).ToString() : null;
                if (!string.IsNullOrEmpty(cahceData))
                {
                    return cahceData;
                }
                string url =
                    string.Format(
                        "{0}/Headlines/1.0/list/" + format + "?encryptedToken={1}&searchContextRef={2}&firstResultToReturn={3}&maxResultsToReturn={4}",
                        basePath,
                        enToken,
                        searchContextRef,
                        firstResultToReturn,
                        maxHeadlinesToReturn
                      );
                string data = GetData(url,true);
                cacheManager.Add(key, data);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetHeadlinesByAccessionNumber(string accessionNumber, string format)
        {
            try
            {
                string key = format + "_" + enToken + "_" + accessionNumber;
                string cahceData = cacheManager.GetCache(key) != null ? cacheManager.GetCache(key).ToString() : null;
                if (!string.IsNullOrEmpty(cahceData))
                {
                    return cahceData;
                }
                string url =
                    string.Format(
                        "{0}/Content/headlines/" + format + "?searchString={1}&encryptedToken={2}&searchmode={3}&sourceGenre={4}&daysRange={5}",
                        restAPIBasePath,
                        "an="+accessionNumber,
                        enToken,
                        "traditional",
                         sourceGenre,
                         daysRange
                      );
                string data = GetData(url, true);
                cacheManager.Add(key, data);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetSerializedPagesModulesForJson(Dictionary<string, string> pageModuleList)
        {
            if (pageModuleList.Count > 0)
            {
                object[] arr = new object[pageModuleList.Count];
                int itemIndex = 0;
                for (int index = 0; index < pageModuleList.Keys.Count; index++)
                {
                    string s = JsonConvert.SerializeObject(pageModuleList.Keys.ElementAt(index) + "ServiceResult");
                    arr[itemIndex] = @"{" + s + ":" + pageModuleList.Values.ElementAt(index) + "}";
                    itemIndex++;
                }
                return Cascade(arr);
            }
            return string.Empty;
        }

        private static string GetSerializedPagesModulesForXml(Dictionary<string, string> pageModuleList)
        {
            if (pageModuleList.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pageModuleList.Values.ElementAt(0));
                XmlNode node = doc.DocumentElement.SelectSingleNode("/newsPageServiceResult");
                if (node != null)
                {
                    node.RemoveAll();
                    XmlDocument doc1 = new XmlDocument();
                    for (int index = 0; index < pageModuleList.Keys.Count; index++)
                    {
                        doc1.LoadXml(pageModuleList.Values.ElementAt(index));
                        node.AppendChild(doc.ImportNode(doc1.DocumentElement, true));
                    }
                }
                return doc.OuterXml;
            }
            return string.Empty;
        }

        private static void GetModuleDataForJson(Dictionary<string, string> pageModuleList, string pageId)
        {
            dynamic pageById = JsonConvert.DeserializeObject(pageModuleList["newsPage"]);
            if (pageById.package != null && pageById.package.newsPage != null &&
                pageById.package.newsPage.modules != null && pageById.package.newsPage.modules.Count > 0)
            {
                int maxResultsToReturn = pageById.package.newsPage.modules.Count;
                int maxPartsToReturn = maxResultsToReturn;
              
                foreach (dynamic module in pageById.package.newsPage.modules)
                {
                    if (module != null && module.__type != null)
                    {
                        long moduleId = module.id.Value;
                        string type = module.__type.Value;
                        GetPageModuleByPageId(pageModuleList, pageId, "Json", moduleId.ToString(), type);
                    }

                }
            }
        }

        private static void GetModuleDataForXml(Dictionary<string, string> pageModuleList, string pageId)
        {
            XDocument xDoc =XDocument.Parse(pageModuleList["newsPage"]);
            IEnumerable<XElement> modules = xDoc.Descendants("modules");
            if (modules != null && modules.Count() > 0)
            {
                IEnumerable<XElement> newsPageModules = modules.First().Descendants("newsPageModule");
                int maxResultsToReturn = newsPageModules.Count();
                int maxPartsToReturn = maxResultsToReturn;
                foreach (XElement module in newsPageModules)
                {
                    if (module != null && module.FirstAttribute != null && module.FirstAttribute.Value != null)
                    {
                        string type = module.FirstAttribute.Value;
                        string moduleId = module.Element("id").Value;
                        GetPageModuleByPageId(pageModuleList, pageId, "Xml", moduleId, type);
                    }
                }
            }
        }

        private static void GetPageModuleByPageId(Dictionary<string, string> pageModuleList, string pageId, string format, string moduleId, string type)
        {
            int firstResultToReturn = 0;
            int firstPartToReturn = 0;
            string parts = string.Empty;
            string url = string.Empty;

            switch (type)
            {
                case "summaryNewspageModule":
                    parts = "Chart|RecentArticles|RecentVideos|RegionalMap|Trending";
                    url =
                       string.Format(
                           "{0}/Modules/Summary/1.0/data/" + format + "?pageId={1}&moduleId={2}&firstResultToReturn={3}&maxResultsToReturn={4}&maxEntitiesToReturn={5}&parts={6}&encryptedToken={7}",
                           basePath,
                           pageId,
                           moduleId,
                           firstResultToReturn,
                           maxResultsToReturn,
                           maxEntitiesToReturn,
                           parts,
                           enToken
                         );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "topNewsNewspageModule":
                    parts = "EditorsChoice|VideoAndAudio|OpinionAndAnalysis";
                    url =
                      string.Format(
                          "{0}/Modules/TopNews/1.0/data/" + format + "?pageId={1}&moduleId={2}&firstResultToReturn={3}&maxResultsToReturn={4}&parts={5}&encryptedToken={6}",
                          basePath,
                          pageId,
                          moduleId,
                          firstResultToReturn,
                          maxResultsToReturn,
                          parts,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "sourcesNewspageModule":
                    parts = "EditorsChoice|VideoAndAudio|OpinionAndAnalysis";
                    url =
                      string.Format(
                          "{0}/Modules/Sources/1.0/data/" + format + "?pageid={1}&moduleId={2}&firstPartToReturn={3}&maxPartsToReturn={4}&firstResultToReturn={5}&maxResultsToReturn={6}&encryptedToken={7}",
                          basePath,
                          pageId,
                          moduleId,
                          firstPartToReturn,
                          maxPartsToReturn,
                          firstResultToReturn,
                          maxResultsToReturn,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "companyOverviewNewspageModule":
                    parts = "SnapShot|Chart|RecentArticles|Trending";
                    url =
                      string.Format(
                          "{0}/Modules/CompanyOverview/1.0/data/" + format + "?pageid={1}&moduleId={2}&firstResultToReturn={3}&maxResultsToReturn={4}&parts={5}&encryptedToken={6}",
                          basePath,
                          pageId,
                          moduleId,
                          firstResultToReturn,
                          maxResultsToReturn,
                          parts,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "radarNewspageModule":
                    url =
                      string.Format(
                          "{0}/Modules/Radar/1.0/data/" + format + "?pageid={1}&moduleId={2}&timeFrame={3}&encryptedToken={4}",
                          basePath,
                          pageId,
                          moduleId,
                         timeFrame,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "customTopicsNewspageModule":
                    url =
                      string.Format(
                          "{0}/Modules/CustomTopics/1.0/data/" + format + "?pageid={1}&moduleId={2}&firstPartToReturn={3}&maxPartsToReturn={4}&firstResultToReturn={5}&maxResultsToReturn={6}&encryptedToken={7}",
                          basePath,
                          pageId,
                          moduleId,
                           firstPartToReturn,
                           maxPartsToReturn,
                           firstResultToReturn,
                           maxResultsToReturn,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "regionalMapNewspageModule":
                    url =
                      string.Format(
                          "{0}/Modules/RegionalMap/1.0/data/" + format + "?pageid={1}&moduleId={2}&timeFrame={3}&encryptedToken={4}",
                          basePath,
                          pageId,
                          moduleId,
                          timeFrame,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "trendingNewsPageModule":
                    parts = "TopEntities|TrendingDown|TrendingUp";
                    url =
                      string.Format(
                          "{0}/Modules/Trending/1.0/data/" + format + "?pageid={1}&moduleId={2}&parts={3}&timeFrame={4}&entityType={5}&encryptedToken={6}",
                          basePath,
                          pageId,
                          moduleId,
                          parts,
                          timeFrame,
                          entityType,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "newsstandSourcesNewspageModule":
                    parts = "Headlines|Counts|DiscoveredEntities";
                    url =
                      string.Format(
                          "{0}/Modules/NewsstandSources/1.0/data/" + format + "?pageid={1}&moduleId={2}&parts={3}&firstResultToReturn={4}&maxResultsToReturn={5}&firstPartToReturn={6}&maxPartsToReturn={7}&encryptedToken={8}",
                          basePath,
                          pageId,
                          moduleId,
                          parts,
                          firstResultToReturn,
                          maxResultsToReturn,
                          firstPartToReturn,
                          maxPartsToReturn,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
                case "alertsNewspageModule":
                    url =
                      string.Format(
                          "{0}/Modules/Alerts/1.0/data/" + format + "?pageid={1}&moduleId={2}&firstResultToReturn={3}&maxResultsToReturn={4}&firstPartToReturn={5}&maxPartsToReturn={6}&encryptedToken={7}",
                          basePath,
                          pageId,
                          moduleId,
                          firstResultToReturn,
                          maxResultsToReturn,
                          firstPartToReturn,
                          maxPartsToReturn,
                          enToken
                        );
                    pageModuleList.Add(type, GetData(url));
                    break;
            }
        }

        public static string Cascade(params object[] jsonArray)
        {
            JObject result = new JObject();
            foreach (string json in jsonArray)
            {
                JObject parsed = JObject.Parse(json);
                Merge(result, parsed);
            }
            return result.ToString();
        }

        private static void Merge(JObject receiver, JObject donor)
        {
            foreach (var property in donor)
            {
                JObject receiverValue = receiver[property.Key] as JObject;
                JObject donorValue = property.Value as JObject;
                if (receiverValue != null && donorValue != null)
                    Merge(receiverValue, donorValue);
                else
                    receiver[property.Key] = property.Value;
            }
        }

        public static string GetData(string url,bool addPreference = true)
        {
            string result = string.Empty;
            try
            {
                var webRequest = WebRequest.Create(new Uri(url));
                webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                if (addPreference)
                {
                    webRequest.Headers.Add("preferences", "{\"clockType\":1,\"interfaceLanguage\":\"en\",\"contentLanguages\":[\"en\"],\"timeZone\":\"on, -05:00|1, on\"}");
                }

                
                webRequest.Method = "GET";
                
                WebResponse response = webRequest.GetResponse();
                using (var httpResponse = (HttpWebResponse)response)
                {
                    using (var responseStream = httpResponse.GetResponseStream())
                    {
                        using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            result = readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
               // isError = true;
                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            result += reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    result = ex.Status +" "+ ex.Message;
                }
            }
            catch (Exception ex)
            {
               // isError = true;
                throw ex;
            }
            
            return result;
        }
    }
}
