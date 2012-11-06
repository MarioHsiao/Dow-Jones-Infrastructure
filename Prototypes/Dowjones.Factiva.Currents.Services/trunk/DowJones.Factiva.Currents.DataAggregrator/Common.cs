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
            List<string> pageModuleList = new List<string>();
            pageModuleList.Add(GetData(url));
            GetModuleData(pageModuleList, pageId);
            return GetSerializedPagesModules(pageModuleList);
        }

        

        public static string GetPageListData()
        {
            //List<string> pageModuleList = new List<string>();

            string url =
                string.Format(
                    "{0}/Pages/1.0/list/json?encryptedToken={1}",
                    basePath,
                    enToken
                  );

            //dynamic pageList = JsonConvert.DeserializeObject(GetData(url));
            //if (pageList.package != null && pageList.package.newsPages != null && pageList.package.newsPages.Count > 0)
            //{
            //    foreach (dynamic newsPage in pageList.package.newsPages)
            //    {
            //        string pageId = newsPage.id.Value;
            //        GetPageByIdData(pageId);
            //       // GetModuleData(pageModuleList, pageId);
            //    }
            //}
            return GetData(url);
        }

        private static string GetSerializedPagesModules(List<string> pageModuleList)
        {
            object[] arr = new object[pageModuleList.Count];
            int index = 0;
            foreach (string str in pageModuleList)
            {
                arr[index] = JsonConvert.DeserializeObject(str);
                index++;
            }
            string serializedObject = JsonConvert.SerializeObject(arr);
            return serializedObject;
        }

        private static void GetModuleData(List<string> pageModuleList, string pageId)
        {
            int firstResultToReturn = 0;
            int firstPartToReturn = 0;
            
            string parts = string.Empty;
            string url = string.Empty;
            dynamic pageById = JsonConvert.DeserializeObject(pageModuleList[0]);
            if (pageById.package != null && pageById.package.newsPage != null &&
                pageById.package.newsPage.modules != null && pageById.package.newsPage.modules.Count > 0)
            {
                int maxResultsToReturn = pageById.package.newsPage.modules.Count;
                int maxPartsToReturn = maxResultsToReturn;
                string timeFrame = "lastweek";
                foreach (dynamic module in pageById.package.newsPage.modules)
                {
                    if (module != null && module.__type != null)
                    {
                        string type = module.__type.Value;
                        long moduleId = module.id.Value;
                        switch (type)
                        {
                            case "summaryNewspageModule":
                                int maxEntitiesToReturn = maxResultsToReturn;
                                parts = "Chart|RecentArticles|RecentVideos|RegionalMap|Trending";
                                url =
                                   string.Format(
                                       "{0}/Modules/Summary/1.0/data/json?pageId={1}&moduleId={2}&firstResultToReturn={3}&maxResultsToReturn={4}&maxEntitiesToReturn={5}&parts={6}&encryptedToken={7}",
                                       basePath,
                                       pageId,
                                       moduleId,
                                       firstResultToReturn,
                                       maxResultsToReturn,
                                       maxEntitiesToReturn,
                                       parts,
                                       enToken
                                     );
                                pageModuleList.Add(GetData(url));
                                break;
                            case "topNewsNewspageModule":
                                 parts = "EditorsChoice|VideoAndAudio|OpinionAndAnalysis";
                                 url =
                                   string.Format(
                                       "{0}/Modules/TopNews/1.0/data/json?pageId={1}&moduleId={2}&firstResultToReturn={3}&maxResultsToReturn={4}&parts={5}&encryptedToken={6}",
                                       basePath,
                                       pageId,
                                       moduleId,
                                       firstResultToReturn,
                                       maxResultsToReturn,
                                       parts,
                                       enToken
                                     );
                                pageModuleList.Add(GetData(url));
                                break;
                            case "sourceNewspageModule":
                                 
                                 parts = "EditorsChoice|VideoAndAudio|OpinionAndAnalysis";
                                 url =
                                   string.Format(
                                       "{0}/Modules/Sources/1.0/data/json?pageid={1}&moduleId={2}&firstPartToReturn={3}&maxPartsToReturn={4}&firstResultToReturn={5}&maxResultsToReturn={6}&encryptedToken={7}",
                                       basePath,
                                       pageId,
                                       moduleId,
                                       firstPartToReturn,
                                       maxPartsToReturn,
                                       firstResultToReturn,
                                       maxResultsToReturn,
                                       enToken
                                     );
                                pageModuleList.Add(GetData(url));
                                break;
                            case "companyOverviewNewspageModule":
                                 parts = "SnapShot|Chart|RecentArticles|Trending";
                                 url =
                                   string.Format(
                                       "{0}/Modules/CompanyOverview/1.0/data/json?pageid={1}&moduleId={2}&firstResultToReturn={3}&maxResultsToReturn={4}&parts={5}&encryptedToken={6}",
                                       basePath,
                                       pageId,
                                       moduleId,
                                       firstResultToReturn,
                                       maxResultsToReturn,
                                       parts,
                                       enToken
                                     );
                                pageModuleList.Add(GetData(url));
                                break;
                            case "radarNewspageModule":
                               
                                url =
                                  string.Format(
                                      "{0}/Modules/Radar/1.0/data/json?pageid={1}&moduleId={2}&timeFrame={3}&encryptedToken={4}",
                                      basePath,
                                      pageId,
                                      moduleId,
                                     timeFrame,
                                      enToken
                                    );
                                pageModuleList.Add(GetData(url));
                                break;
                            case "customTopicsNewspageModule":
                                url =
                                  string.Format(
                                      "{0}/Modules/CustomTopics/1.0/data/json?pageid={1}&moduleId={2}&firstPartToReturn={3}&maxPartsToReturn={4}&firstResultToReturn={5}&maxResultsToReturn={6}&encryptedToken={7}",
                                      basePath,
                                      pageId,
                                      moduleId,
                                       firstPartToReturn,
                                       maxPartsToReturn,
                                       firstResultToReturn,
                                       maxResultsToReturn,
                                      enToken
                                    );
                                pageModuleList.Add(GetData(url));
                                break;
                        }
                    }

                }
            }
        }

        public static string GetData(string url)
        {
            string result = string.Empty;
            try
            {
                var webRequest = WebRequest.Create(new Uri(url));
                webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
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
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Something more serious happened
                // like for example you don't have network access
                // we cannot talk about a server exception here as
                // the server probably was never reached
            }
            
            return result;
        }
    }
}
