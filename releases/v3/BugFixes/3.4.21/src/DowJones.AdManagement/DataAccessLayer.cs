using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using DowJones.Properties;
using log4net;

namespace DowJones.AdManagement
{
    /// <summary>
    /// Retrieve data from marketing site
    /// </summary>
    public class DataAccessLayer
    {
        //PRDID_GRPTYPE_PRDSUBTYPE_IL_SIZE_SERVERNAME 
        //PROJECT VISIBLE NAMESPACE IN GLOBAL FACTIVA CACHE SERVER = 7
        private const string CacheKeyForMarketing = "";
        private const ushort CacheNamespace = 7;
        private const int ResponseTimeoutFromMarketingSite = 1500;
        private static readonly ILog Log = LogManager.GetLogger(typeof (DataAccessLayer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketingRequest"></param>
        /// <returns></returns>
        public MarketingResponse GetResponse(MarketingRequest marketingRequest)
        {
            /*
            * 1. Try getting it from cache.
            * 2. if found return data.
            * 3. if not get it from source server.
            */
            var marketingResponse = GetCacheResponse(marketingRequest);

            if (marketingResponse == null)
            {
                marketingResponse = GetServerResponse(marketingRequest);
                AddAdToCache(marketingResponse, marketingRequest);
            }

            return marketingResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketingRequest"></param>
        /// <returns></returns>
        private static MarketingResponse GetCacheResponse(MarketingRequest marketingRequest)
        {
            var key = GetCacheKey(marketingRequest);

            MarketingResponse marketingResponse = null;

            try
            {
                //cc: made changes to use the emg.utility cache manager instead of genericCache

                var cm = new CacheManager();

                var _itemValue = cm.GetItem(key, CacheNameSpace.ProjectVisible);

                if (!string.IsNullOrEmpty(_itemValue))
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(_itemValue);
                    using (var r = new XmlNodeReader(doc))
                    {
                        var xs = new XmlSerializer(typeof (MarketingResponse));
                        marketingResponse = (MarketingResponse) xs.Deserialize(r);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(
                    string.Format("Exception while Getting Ad from Cache:{0} \nMessage: {1}\nStackTrace: {2}",
                                  key, ex.Message, ex.StackTrace), ex);
                return null;
            }
            return marketingResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketingRequest"></param>
        /// <returns></returns>
        private static MarketingResponse GetServerResponse(MarketingRequest marketingRequest)
        {
            Log.Debug("Getting data from marketing site");
            var url = Settings.Default.MarketingDataURL;

            var queryStrings = new NameValueCollection
                                   {
                                       {"productid", marketingRequest.ProductID},
                                       {"grouptype", marketingRequest.GroupType},
                                       {"productsubtype", marketingRequest.ProductSubType.ToString()},
                                       {"interfacelanguage", marketingRequest.InterfaceLanguage},
                                       {"size", MapSize(marketingRequest.Size)},
                                       {"cc", marketingRequest.SourceCountryCode}
                                   };

            //added code for Q2-2009-regional ads adding country code to request object

            if (marketingRequest.ResponseType.ToString() == "All")
            {
                queryStrings.Add("responsetype", marketingRequest.ResponseType.ToString());
            }
            else
            {
                queryStrings.Add("elementstoreturn", marketingRequest.ElementsToReturn.ToString());
            }

            var sb = new StringBuilder();
            foreach (var s in queryStrings.AllKeys)
            {
                sb.Append(s).Append("=").Append(HttpUtility.UrlEncode(queryStrings[s]));
                sb.Append("&");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            Log.Debug("Requested NVP: " + sb);

            var byte1 = Encoding.UTF8.GetBytes(sb.ToString());

            var doc = new XmlDocument();
            var WReq = WebRequest.Create(url);
            WReq.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            WReq.Method = "POST";
            WReq.ContentType = "application/x-www-form-urlencoded";
            WReq.Headers.Add(queryStrings);
            WReq.ContentLength = byte1.Length;
            WReq.GetRequestStream().Write(byte1, 0, byte1.Length);
            WReq.Timeout = ResponseTimeoutFromMarketingSite;

            Log.Debug("Begin Marketing site WebRequest:" + DateTime.Now);
            try
            {
                using (var WResp = WReq.GetResponse())
                {
                    Log.Debug("End Marketing site WebRequest:" + DateTime.Now);
                    using (var sr = new StreamReader(WResp.GetResponseStream(), Encoding.UTF8))
                    {
                        doc.LoadXml(sr.ReadToEnd());
                        Log.Debug("Got response xml:" + doc.OuterXml);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(
                    string.Format(
                        "Exception while connecting to Marketing Data URL:{0} \nMessage: {1}\nStackTrace: {2}",
                        url, ex.Message, ex.StackTrace), ex);
                return null;
            }
            try
            {
                using (var r = new XmlNodeReader(doc))
                {
                    var xs = new XmlSerializer(typeof(MarketingResponse));
                    var resp = (MarketingResponse) xs.Deserialize(r);
                    Log.Debug("Deserialized from marketing response");
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Log.Error(
                    string.Format(
                        "Exception while Serializing Response from Marketing Server:{0} \nMessage: {1}\nStackTrace: {2}",
                        doc.InnerText, ex.Message, ex.StackTrace), ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketingRequest"></param>
        /// <returns></returns>
        private static string GetCacheKey(MarketingRequest marketingRequest)
        {
            //PRDID_GRPTYPE_PRDSUBTYPE_IL_SIZE_SERVERNAME 
            //modified cache key to include Country code Q2-2009 - 03/30/09 - Project visible regional ads
            //new key - PRDID_GRPTYPE_PRDSUBTYPE_IL_SIZE_COUNTRYCODE_SERVERNAME 

            Log.Debug("Marketing Request getCacheKey() key code:" + CacheKeyForMarketing + "" +
                        marketingRequest.ProductID + "_" +
                        marketingRequest.ProductSubType + "_" +
                        marketingRequest.GroupType + "_" +
                        marketingRequest.InterfaceLanguage + "_" +
                        marketingRequest.Size + "_" +
                        marketingRequest.SourceCountryCode + "_" +
                        Environment.MachineName);

            //added country code in cache key for Q2-2009-regional ads
            return CacheKeyForMarketing + "" +
                   marketingRequest.ProductID + "_" +
                   marketingRequest.ProductSubType + "_" +
                   marketingRequest.GroupType + "_" +
                   marketingRequest.InterfaceLanguage + "_" +
                   marketingRequest.Size + "_" +
                   marketingRequest.SourceCountryCode + "_" +
                   Environment.MachineName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketingResponse"></param>
        /// <param name="marketingRequest"></param>
        private static void AddAdToCache(MarketingResponse marketingResponse, MarketingRequest marketingRequest)
        {
            if (marketingResponse == null)
            {
                return;
            }
            var key = GetCacheKey(marketingRequest);

            try
            {
                //cc: made changes to use the emg.utility cache manager instead of genericCache
                var cm = new CacheManager();

                var ser = new XmlSerializer(typeof (MarketingResponse));
                using (var sw = new StringWriter())
                {
                    ser.Serialize(sw, marketingResponse);
                    cm.AddItem(key, sw.ToString(), CacheNameSpace.ProjectVisible);
                }
            }
            catch (Exception ex)
            {
                Log.Error(
                    string.Format("Exception while adding to Cache:{0} \nMessage: {1}\nStackTrace: {2}",
                                  CacheNamespace, ex.Message, ex.StackTrace), ex);
            }
        }

        private static string MapSize(ImageSize size)
        {
            switch (size)
            {
                case ImageSize.Mini:
                    return "mn";
                case ImageSize.Small:
                    return "sm";
                case ImageSize.Large:
                    return "lg";
                case ImageSize.Custom:
                    return "cu";
                case ImageSize.SmallVertical:
                    return "smv";
                case ImageSize.SmallLowHorizontal:
                    return "smlowh";

            }
            return string.Empty;
        }
    }
}