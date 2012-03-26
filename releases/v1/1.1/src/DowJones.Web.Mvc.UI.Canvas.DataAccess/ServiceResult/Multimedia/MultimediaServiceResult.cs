using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Multimedia;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.Multimedia;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Multimedia;
using Factiva.Gateway.Managers;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Services.V2_0;
using Factiva.Gateway.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;



namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Multimedia
{
    [DataContract(Name="multimediaServiceResult", Namespace="")]
    public class MultimediaServiceResult : AbstractServiceResult, IPopulate<VideoRequest>
    {
        private XmlNode rampNode; 

        [DataMember(Name = "result")]
        public MultimediaPackage Result { get; set; }

        public void Populate(ControlData controlData, VideoRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   if (request == null || !request.IsValid())
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                   }

                   GetData(controlData, request);
               },
               preferences);
        }

        private void GetData(ControlData controlData,VideoRequest request)
        {
            string episodeId = GetEpisodeId(controlData,request.AccessionNo);
            Audit.AdditionalInfo = "EpisodeId:" + episodeId;
            MediaContents mediaContents = null;
            MediaContent mediaContent = null;
            MustPlayFromSource mustPlay;
            
            var node = ProcessEpisodeID(episodeId);
            var htmlUrlValue = node.GetAttribute("html-url") ?? "";

            string guid = null;

            if (node.Attributes != null && node.Attributes["guid"] == null)
            {
                if (node.Attributes["html-url"]==null)
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.MultimediaHtmlUrlNotFound);

                mustPlay = new MustPlayFromSource {Status = true, Url = htmlUrlValue};

            }
            //throw new DowJonesUtilitiesException(DowJonesUtilitiesException.MultimediaEpisodeGuidNotFound);
            else
            {
                mustPlay = new MustPlayFromSource {Status = false, Url = htmlUrlValue};
                guid = node.GetAttribute("guid");


                if (node.GetAttribute("media-type") == "audio")
                {
                    mediaContent = new MediaContent
                                       {
                                           Medium = "audio",
                                           Duration = node.GetAttribute("duration"),
                                           Type = "audio/x-" + Path.GetExtension(node.GetAttribute("media-url")).Replace(".", ""),
                                           Url = node.GetAttribute("media-url")
                                       };

                    mediaContents = new MediaContents {mediaContent};
                }
                else
                {
                    var nodes = ProcessGuid(guid);
                    if (nodes.Count == 0)
                    {
                        Result = new MultimediaPackage
                                     {
                                         MustPlayFromSource = new MustPlayFromSource {Status = false, Url = node.GetAttribute("media-url")}
                                     };
                        return;
                    }
                    else
                    {
                        mediaContents = new MediaContents();
                        foreach (XmlNode mnode in nodes)
                        {
                            mediaContent = new MediaContent
                                               {
                                                   BitRate = mnode.GetAttribute("bitrate"),
                                                   FrameRate = mnode.GetAttribute("framerate"),
                                                   Medium = mnode.GetAttribute("medium"),
                                                   Duration = mnode.GetAttribute("duration"),
                                                   Type = mnode.GetAttribute("type"),
                                                   Width = mnode.GetAttribute("width"),
                                                   Height = mnode.GetAttribute("height"),
                                                   Url = mnode.GetAttribute("url"),
                                               };
                            if (mnode.GetAttribute("type") == "video/x-flv")
                            {
                                PopulateStreamerFile(mediaContent);
                            }
                            mediaContents.Add(mediaContent);
                        }
                    }

                }
            }

            
            Result = new MultimediaPackage
            {
                Guid = guid,
                MediaContents = mediaContents,
                MustPlayFromSource = mustPlay
            };

        }
        private static void PopulateStreamerFile(MediaContent mediacontent)
        { 
            int index = mediacontent.Url.IndexOf("/video");
            mediacontent.Streamer = mediacontent.Url.Substring(0, index);
            mediacontent.File = mediacontent.Url.Substring(index + 1);
        }
        private string GetEpisodeId(ControlData controlData,string accessionNo) {

            string episodeID;
            var multimediaArticleUrlRequest = new GetMultimediaArticleUrlRequest {accessionNumbers = new[] {accessionNo}};
            ServiceResponse serviceResponse = null;
            
            RecordTransaction(
                "ArchiveService.GetMultimediaArticleUrl",
                null,
                () =>
                    {
                        serviceResponse = ArchiveService.GetMultimediaArticleUrl(ControlDataManager.Clone(controlData), multimediaArticleUrlRequest);
                    });

            if (serviceResponse.rc == 0)
            {
                object responseObj;
                long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
                var getMultimediaArticleUrlResponse = (GetMultimediaArticleUrlResponse) responseObj;
                var multimediaArticle = getMultimediaArticleUrlResponse.multimediaArticleResultSet.multimediaArticle[0];
                if (multimediaArticle.status ==0)
                {
                    episodeID = FindEpisodeID(multimediaArticle.properties).value;
                }
                else
                {
                    throw new DowJonesUtilitiesException(multimediaArticle.status);
                }
            }
            else
            {
                throw new DowJonesUtilitiesException(serviceResponse.rc);
            }

            return episodeID;
        }

        private static ArticleProperty FindEpisodeID(ICollection<ArticleProperty> collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }

            return collection.FirstOrDefault(articleProperty => articleProperty.name == "ipdocid");
        }

        //private string GetBitRateFromUrl(string url)
        //{
        //    string bitrate = string.Empty;
        //    string endStr = "k" + Path.GetExtension(url);
        //    if (url.EndsWith(endStr))
        //    {
        //        bitrate = url.Substring(url.LastIndexOf("_") + 1, url.IndexOf(endStr) - url.LastIndexOf("_") - 1);
        //    }
        //    return bitrate;
        //}

        private XmlNode ProcessEpisodeID(string episodeID)
        {
            var ramUri = new Uri(Settings.Default.RAMUri + episodeID);
            var rampResponse = GetHttpWebResponse(ramUri, DowJonesUtilitiesException.MultimediaRampFailed);
            var stream = rampResponse.GetResponseStream();
            var doc = new XmlDocument();
            doc.Load(stream);
            rampNode = doc.SelectSingleNode("/EveryzingPlayerDoc/Content/FeaturedEpisode/ResultSet/Results/CompleteResult/EpisodeMetaData");
            return rampNode;
        }

        private HttpWebResponse GetHttpWebResponse(Uri uri,long errorCode)
        {
            HttpWebResponse response = null;
            RecordTransaction(
                                    uri.ToString(),
                                    null,
                                    () =>
                                        {
                                            try
                                            {
                                                var request = (HttpWebRequest) HttpWebRequest.Create(uri);
                                                request.Method = "GET";
                                                response = (HttpWebResponse) request.GetResponse();
                                            }
                                            catch 
                                            {
                                                throw new DowJonesUtilitiesException(errorCode);
                                            }

                                        });
            return response;
        }
        private XmlNodeList ProcessGuid(string guid)
        {
            var uri = new Uri(Settings.Default.MarketWatchUri + guid);
            var response = GetHttpWebResponse(uri,DowJonesUtilitiesException.MultimediaMarketWatchFailed);
            var stream = response.GetResponseStream();
            var doc = new XmlDocument();
            doc.Load(stream);
            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("media", "http://search.yahoo.com/mrss/");
            var nodes = doc.SelectNodes("/rss/channel/item/media:group/media:content",ns);
            return nodes;
        }
    }
}
