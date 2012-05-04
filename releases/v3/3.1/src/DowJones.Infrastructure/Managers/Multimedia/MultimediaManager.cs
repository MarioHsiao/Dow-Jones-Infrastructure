using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Managers.Abstract;
using DowJones.Session;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Sources.V1_0;
using log4net;

namespace DowJones.Managers.Multimedia
{
    public class MultimediaManager : AbstractAggregationManager
    {
        #region Private variables

        private readonly ILog _log = LogManager.GetLogger(typeof(MultimediaManager));
        private const string DefaultRampRedirect = "http://factiva.ramp.com/redirect/?e=";

        private readonly string _rampUri = Properties.Settings.Default.RampUri;
        private readonly string _marketWatchUri = Properties.Settings.Default.MarketWatchUri;

        #endregion

        #region Constructors

        public MultimediaManager(IControlData controlData)
            : base(controlData)
        {
        }

        #endregion

        #region Abstractclass overrides

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return _log; }
        }

        #endregion

        #region Public methods

        public MultimediaResponse GetMultiMediaResult(MultimediaRequest objRequest)
        {
            return GetMultiMediaResult(objRequest.AccessionNumber, false, null);

        }


        public MultimediaResponse GetMultiMediaResult(string accessionNumber, bool flashOnly)
        {
            return GetMultiMediaResult(accessionNumber, flashOnly, null);
        }

        public MultimediaResponse GetMultiMediaResult(string accessionNumber, bool flashOnly, List<string> mustPlayFromSourceList)
        {
            return GetMultiMediaResult(GetEpisodeId(accessionNumber), GetSourceCodeForArticleId(accessionNumber), flashOnly,
                                       mustPlayFromSourceList);
        }

        public MultimediaResponse GetMultiMediaResult(string episodeId, string sourceCode, bool flashOnly)
        {
            return GetMultiMediaResult(episodeId, sourceCode, flashOnly, null);
        }

        public MultimediaResponse GetMultiMediaResult(string episodeId, string sourceCode, bool flashOnly, List<string> mustPlayFromSourceList)
        {
            MediaContents mediaContents = null;
            var mustPlay = new MustPlayFromSource { Status = false, Url = DefaultRampRedirect + episodeId };
            var sourceList = mustPlayFromSourceList;
            var response = new MultimediaResponse();
            
            try
            {
                if (sourceList == null)
                {
                    sourceList = GetOptInSourceList();
                }
                if (sourceList != null && !sourceList.Contains(sourceCode))
                {
                    mustPlay.Status = true;
                }

                if (mustPlay.Status)
                {
                    response.multimediaResult = new MultimediaPackage { MustPlayFromSource = mustPlay };
                    response.status = 0;
                    return response;
                }
                var node = ProcessEpisodeID(episodeId);

                if (node.Attributes != null && node.Attributes["html-url"] != null)
                {
                    mustPlay.Url = node.GetAttribute("html-url");

                    if (node.Attributes != null && node.Attributes["guid"] != null)
                    {
                        var guid = node.GetAttribute("guid");
                        var mediaType = node.GetAttribute("media-type");

                        MediaContent mediaContent;
                        if (mediaType.Equals("audio"))
                        {
                            mediaContent = new MediaContent
                                               {
                                                   Medium = "audio",
                                                   Duration = node.GetAttribute("duration"),
                                                   Url = node.GetAttribute("media-url"), Type = string.Format("audio/x-{0}", Path.GetExtension(node.GetAttribute("media-url")).Replace(".", ""))
                                               };

                            mediaContents = new MediaContents {mediaContent};
                        }
                        else if(!flashOnly)
                        {
                            var nodes = ProcessGUID(guid);
                            if (nodes.Count == 0)
                            {
                                response.multimediaResult = new MultimediaPackage
                                                                {
                                                                    MustPlayFromSource =
                                                                        new MustPlayFromSource
                                                                            {
                                                                                Status = false,
                                                                                Url = node.GetAttribute("media-url")
                                                                            }
                                                                };

                                return response;
                            }

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

                        var result = new MultimediaPackage
                                         {
                                             Guid = guid,
                                             MediaContents = mediaContents,
                                             MustPlayFromSource =
                                                 new MustPlayFromSource { Status = mustPlay.Status, Url = mustPlay.Url}
                                         };

                        response.multimediaResult = result;
                    }
                }
                else
                {
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.MULTIMEDIA_HTML_URL_NOT_FOUND);
                }
            }
            catch (DowJonesUtilitiesException emgEx)
            {
                response.status = emgEx.ReturnCode;
                _log.Error(string.Format("Code {0} - {1}", emgEx.ReturnCode, emgEx.Message));
                
            }
            catch(Exception ex)
            {
                response.status = DowJonesUtilitiesException.MULTIMEDIA_UNKNOWN_EXCEPTION;
                _log.Error(ex.Message);
            }

            return response;
                        
           
                
        }


        #endregion

        #region Internal Methods

        internal List<string> GetOptInSourceList()
        {
            var lstSources = new List<string>();

            var performSourceSearchRequest = new PerformSourceSearchRequest
                                                 {
                                                     searchString = "rst=toptin and ila=en", 
                                                     resultType = ResultType.Sources, 
                                                     responseType = ResponseType.Brief
                                                 };


            var performSourceSearchResponse = Process<PerformSourceSearchResponse>(performSourceSearchRequest);
            if (performSourceSearchResponse.sourceSearchResponse != null && performSourceSearchResponse.sourceSearchResponse.sourceSearchResult != null)
            foreach (Document document in performSourceSearchResponse.sourceSearchResponse.sourceSearchResult.sourcesResultSet)
            {
                lstSources.Add(((SourceDoc)document).sourceCode.ToUpper());

            }


            return lstSources;
        }

        /// <summary>
        /// Gets the source code of teh given article id
        /// </summary>
        /// <param name="accessionNumber"></param>
        /// <returns></returns>
        internal string GetSourceCodeForArticleId(string accessionNumber)
        {
            var partOfId = accessionNumber.Substring(0, 8);

            if (partOfId.EndsWith("0"))
                accessionNumber = partOfId.TrimEnd(new[] { '0' });

            return accessionNumber.ToUpper();
        }

        internal string GetEpisodeId(string accessionNo)
        {

            string episodeID;
            var multimediaArticleUrlRequest = new GetMultimediaArticleUrlRequest
                                                  {
                                                      accessionNumbers = new[] {accessionNo}
                                                  };
            
           
            var response = Process<GetMultimediaArticleUrlResponse>(multimediaArticleUrlRequest);
            var multimediaArticle = response.multimediaArticleResultSet.multimediaArticle[0];
            if (multimediaArticle.status == 0)
            {
                if (string.Equals(CheckDocType(multimediaArticle.properties).value.Trim(), "multimedia", StringComparison.InvariantCultureIgnoreCase))
                    episodeID = FindEpisodeID(multimediaArticle.properties).value;
                else
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.INVALID_MULTIMEDIA_ARTICLE);
            }
            else
            {

                throw new DowJonesUtilitiesException(multimediaArticle.status);
            }

            return episodeID;
        }

        internal static ArticleProperty FindEpisodeID(ICollection<ArticleProperty> collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }

            return collection.FirstOrDefault(articleProperty => articleProperty.name == "ipdocid");
        }

        internal static ArticleProperty CheckDocType(ICollection<ArticleProperty> collection)
        {
            if (collection == null || collection.Count <= 0)
            {
                return null;
            }

            return collection.FirstOrDefault(articleProperty => articleProperty.name == "doctype");
        }

        internal XmlNode ProcessEpisodeID(string episodeID)
        {
            var ramUri = new Uri(_rampUri + episodeID);
            var rampResponse = GetHttpWebResponse(ramUri, DowJonesUtilitiesException.MULTIMEDIA_RAMP_FAILED);
            var stream = rampResponse.GetResponseStream();
            var doc = new XmlDocument();
            if (stream != null) doc.Load(stream);
            var rampNode = doc.SelectSingleNode("/EveryzingPlayerDoc/Content/FeaturedEpisode/ResultSet/Results/CompleteResult/EpisodeMetaData");
            return rampNode;
        }

        internal HttpWebResponse GetHttpWebResponse(Uri uri, long errorCode)
        {
            HttpWebResponse response;
           
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                throw new DowJonesUtilitiesException(errorCode);
            }

                                   
            return response;
        }

        internal XmlNodeList ProcessGUID(string guid)
        {
            var uri = new Uri(_marketWatchUri + guid);
            var response = GetHttpWebResponse(uri, DowJonesUtilitiesException.MULTIMEDIA_MARKET_WATCH_FAILED);
            var stream = response.GetResponseStream();
            var doc = new XmlDocument();
            if (stream != null) doc.Load(stream);
            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("media", "http://search.yahoo.com/mrss/");
            var nodes = doc.SelectNodes("/rss/channel/item/media:group/media:content", ns);
            return nodes;
        }

        internal static void PopulateStreamerFile(MediaContent mediacontent)
        {
            var index = mediacontent.Url.IndexOf("/video");
            mediacontent.Streamer = mediacontent.Url.Substring(0, index);
            mediacontent.File = mediacontent.Url.Substring(index + 1);
        }

        #endregion
    }
}
