using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Argotic.Common;
using Argotic.Extensions.Core;
using Argotic.Syndication;
using Data;
using EMG.Utility.Formatters.Globalization;
using EMG.Utility.Handlers.Syndication.Podcast.Core;
using EMG.Utility.Managers.Assets;
using EMG.Utility.OperationalData.EntryPoint;
using EMG.Utility.Uri;
using EMG.Utility.Url;
using Factiva.BusinessLayerLogic;
using Factiva.BusinessLayerLogic.Utility.Xml;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using utils = Data.utils;

namespace FactivaRssManager
{

    /// <summary>
    /// Summary description for RssGeneratorBase.
    /// </summary>
    public class RssGenerator
    {
        private string xmlTopLevelNode = "";
        private string[] fldTitles;
        private string fldTitleConnector;
        private string[] fldDescriptions;
        private string fldDescriptionConnector;
        private string[] fldSources;
        private string fldSourceConnector;
        private string[] fldDates;
        private string fldDateConnector;
        private string[] fldDirectUrls;
        private string[] fldEnclosures;
        RssFeed rssFeed = new RssFeed();

        private NameValueCollection fldArticleURL = new NameValueCollection();

        public RssGenerator()
        {
            ChannelAuthor = "";
            Channelttl = "";
            ChannelRssChannelImage = "";
            ChannelImageWidth = "";
            ChannelImageHeight = "";
            ChannelManagingEditor = "";
            ChannelDocs = "";
            ChannelCopyright = "";
            ChannelLanguage = "";
            ChannelLastBuildDate = "";
            ChannelUrlParam = "";
            ChannelId = "";
            ChannelLink = "";
            ChannelDesc = "";
            ChannelTitle = "";
            ArticleUrl = "";
            TranName = "";
        }

        public string TranName { get; set; }

        public string ArticleUrl { get; set; }

        public string ChannelTitle { get; set; }

        public string ChannelDesc { get; set; }

        public string ChannelLink { get; set; }

        public string ChannelId { get; set; }

        public string ChannelUrlParam { get; set; }

        public string ChannelLastBuildDate { get; set; }

        public string ChannelLanguage { get; set; }

        public string ChannelCopyright { get; set; }

        public string ChannelDocs { get; set; }

        public string ChannelManagingEditor { get; set; }

        public string ChannelImageHeight { get; set; }

        public string ChannelImageWidth { get; set; }

        public string ChannelRssChannelImage { get; set; }

        public string Channelttl { get; set; }

        public string ChannelAuthor { get; set; }

        public bool IsPodCast { get; set; }

        public virtual string Convert(XmlDocument xmlHeadlines, ConfigData configData, InputData inputData)
        {
            try
            {
                var xmlNS = utils.CreateNsMgr(xmlHeadlines, configData.getItem("//rssDetail/TopLevelNode/@nsPrefix"), configData.getItem("//rssDetail/TopLevelNode/@nsURI"));
                var headlinesData = new XmlData(xmlHeadlines, xmlNS);
                try
                {
                    BuildRssDetailFields(configData, inputData);
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: buildRssDetailFields (82883- 9239993) " + ex);
                    throw;
                }

                string rss;
                try
                {
                    rss = BuildRss(headlinesData, inputData);
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: buildRss (82883- 9239994) " + ex);
                    return "";
                    //throw ex;
                }

                return rss;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: Convert (82883-737734) " + ex);
                return "";
                //throw ex;
            }
        }

        protected virtual void BuildRssDetailFields(ConfigData configData, InputData inputData)
        {
            try
            {
                IsPodCast = false;
                xmlTopLevelNode = configData.getItem("//rssDetail/TopLevelNode");
                //setting properties 
                fldTitles = configData.getItemValues("//rssDetail/rssMapping/title/params/param");
                fldTitleConnector = configData.getItem("//rssDetail/rssMapping/title/@connector");

                fldSources = configData.getItemValues("//rssDetail/rssMapping/source/params/param");
                fldSourceConnector = configData.getItem("//rssDetail/rssMapping/source/@connector");

                fldDescriptions = configData.getItemValues("//rssDetail/rssMapping/description/params/param");
                fldDescriptionConnector = configData.getItem("//rssDetail/rssMapping/description/@connector");

                fldDates = configData.getItemValues("//rssDetail/rssMapping/date/params/param");
                fldDateConnector = configData.getItem("//rssDetail/rssMapping/date/@connector");

                if (!string.IsNullOrEmpty(inputData.getItem("app")) && inputData.getItem("app").ToUpper() == "WSJ")
                {
                    fldArticleURL = configData.getItems("//rssDetail/wsjArticleURL/params/param", "urlName", "fldName");
                    fldDirectUrls = configData.getItemValues("//rssDetail/rssMapping/url/params/param");
                }
                else
                {
                    fldArticleURL = configData.getItems("//rssDetail/articleURL/params/param", "urlName", "fldName");
                    fldDirectUrls = configData.getItemValues("//rssDetail/rssMapping/url/params/param");
                }


                if (inputData.getItem("from") == null && inputData.getItem("type") == null)
                {
                    return;
                }

                var strPodCast = (inputData.getItem("type") != null ? inputData.getItem("type").ToLower() : inputData.getItem("from").ToLower());
                if (configData.getNode(string.Format("//Custom/params/param[@name='{0}']", strPodCast)) == null)
                {
                    return;
                }

                IsPodCast = true;
                Logger.Log(Logger.Level.INFO, string.Format("validateNode::Validating PodCast Type[09239-003] ... {0}", strPodCast));
                fldEnclosures = configData.getItemValues("//rssDetail/rssMapping/enclosure/params/param");
                //fldEnclosureConnector = configData.getItem("//rssDetail/rssMapping/enclosure/@connector");
                Logger.Log(Logger.Level.INFO, string.Format("PodCast fldEnclosures :- {0} ", fldEnclosures));
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: buildRssDetailFields (82883-99394) " + ex);
                //throw ex;
            }
        }

        protected virtual string BuildRss(XmlData headlinesData, InputData inputData)
        {
            CreateChannelHeader();
            return CreateDetail(headlinesData, inputData);
        }

        protected void CreateChannelHeader()
        {
            rssFeed.Channel = new RssChannel();
            try
            {
                rssFeed.Channel.Title = ChannelTitle;
                rssFeed.Channel.Copyright = ChannelCopyright;
                rssFeed.Channel.Link = new Uri(ChannelLink);
                rssFeed.Channel.Description = ChannelDesc;
                var image = new RssImage(new Uri(ChannelLink), ChannelTitle, new Uri(ChannelRssChannelImage))
                                {
                                    Description = ChannelDesc,
                                    Height = int.Parse(ChannelImageHeight),
                                    Width = int.Parse(ChannelImageWidth)
                                };
                rssFeed.Channel.Image = image;
                rssFeed.Channel.LastBuildDate = DateTime.Parse(ChannelLastBuildDate);
                rssFeed.Channel.Language = ((CultureInfo.GetCultureInfoByIetfLanguageTag(ChannelLanguage)));
                if (!string.IsNullOrEmpty(Channelttl))
                    rssFeed.Channel.TimeToLive = int.Parse(Channelttl);
                rssFeed.Channel.Generator = string.Empty;
                if (IsPodCast)
                {
                    var itemITunesExtension = new ITunesSyndicationExtension { Context = { Author = ChannelAuthor } };
                    rssFeed.Channel.AddExtension(itemITunesExtension);
                }

                //YahooMediaSyndicationExtension yahooMediaSyndicationExtension = new YahooMediaSyndicationExtension();
                //yahooMediaSyndicationExtension.Context.Copyright.Text = channelCopyright;
                //rssFeed.Channel.AddExtension(itemITunesExtension);
                //rssFeed.Channel.AddExtension(yahooMediaSyndicationExtension);
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: createChannelHeader (82883-73664) " + ex);
                throw;
            }
        }

        private static string MapDistributionTypeToDisseminationOption(AudienceOptions audienceOptions)
        {
            switch (audienceOptions)
            {
                case AudienceOptions.InternalAccount:
                    return "inacct";
                case AudienceOptions.OutsideAccount:
                    return "outacct";
                case AudienceOptions.TimeToLive_Proxy:
                    return "ttlt";
                default:
                    return "xrdr";
            }
        }

        private static string GetWorkspaceOperationalDataMemento(string workspaceID, string workspaceName, Audience audience, bool isPodcast)
        {
            var disseminationMethod = isPodcast ? DisseminationMethod.PodCast : DisseminationMethod.Rss;

            var opData = new WorkspaceAssetOperationalData(disseminationMethod)
                             {
                                 AssetId = workspaceID,
                                 AssetName = workspaceName,
                                 AudienceOption = MapDistributionTypeToDisseminationOption(audience.AudienceOptions),
                                 LinkType = "sa"
                             };
            opData.RssOperationalData.RssType = "Private";
            return opData.GetMemento;
        }

        private static string GetNewsletterOperationalDataMemento(string newsletterID, string newsletterName, Audience audience, bool isPodcast)
        {
            var disseminationMethod = isPodcast ? DisseminationMethod.PodCast : DisseminationMethod.Rss;

            var opData = new NewsletterAssetOperationalData(disseminationMethod)
                             {
                                 AssetId = newsletterID,
                                 AssetName = newsletterName,
                                 AudienceOption = MapDistributionTypeToDisseminationOption(audience.AudienceOptions),
                                 LinkType = "sa"
                             };
            opData.RssOperationalData.RssType = "Private";
            return opData.GetMemento;
        }

        protected virtual string CreateDetail(XmlData headlinesData, InputData inputData)
        {
            var operationalDataMemento = string.Empty;
            //var rss = new StringBuilder();

            // Get Newsletter Details token containing Newsletter ID
            var _type = string.Empty;
            var nldtl = string.Empty;

            var isMct = false;

            if (null != inputData)
            {
                bool.TryParse(inputData.getItem("isMCT"), out isMct);

                if (!string.IsNullOrEmpty(inputData.getItem("from")))
                {
                    switch (inputData.getItem("from").ToLower())
                    {
                        case "ws1":
                        case "ws1pcast":
                            _type = "WS";
                            if (!string.IsNullOrEmpty(inputData.getItem("workspaceID")))
                            {
                                nldtl = GetNLDTLToken(inputData.getItem("workspaceID"), _type);
                            }
                            break;
                        case "nl1":
                        case "nl2":
                        case "nl2pcast":
                            _type = isMct ? "MCT" : "NL";
                            if (!string.IsNullOrEmpty(inputData.getItem("newsletterID")))
                            {
                                nldtl = GetNLDTLToken(inputData.getItem("newsletterID"), _type);
                            }
                            break;
                    }
                }
            }

            var headlineNodes = headlinesData.getNodeList("//" + xmlTopLevelNode);
            foreach (XmlNode headlineNode in headlineNodes)
            {
                var title = "";
                var source = "";
                var description = "";
                var date = "";
                var enclosure = "";
                var counter = 1;
                foreach (var fldTitle in fldTitles)
                {
                    title += headlinesData.getItem(fldTitle, headlineNode);
                    if (title == "")
                        headlinesData.getItem(fldTitle);
                    if (counter < fldTitles.Length)
                        title += fldTitleConnector;
                    counter++;
                }
                counter = 1;
                foreach (var fldSource in fldSources)
                {
                    source += headlinesData.getItem(fldSource, headlineNode);
                    if (counter < fldSources.Length)
                        source += fldSourceConnector;
                    counter++;
                }
                counter = 1;
                foreach (var fldDescription in fldDescriptions)
                {
                    description += headlinesData.getItem(fldDescription, headlineNode);
                    if (counter < fldDescriptions.Length)
                        description += fldDescriptionConnector;
                    counter++;
                }
                counter = 1;
                foreach (var fldDate in fldDates)
                {
                    date += headlinesData.getItem(fldDate, headlineNode);
                    if (counter < fldDates.Length)
                        date += fldDateConnector;
                    counter++;
                }
                if (IsPodCast)
                {
                    counter = 1;
                    foreach (var fldEnclosure in fldEnclosures)
                    {
                        enclosure += headlinesData.getItem(fldEnclosure, headlineNode);
                        //if (counter < fldEnclosures.Length)
                        //    enclosure += fldEnclosureConnector;
                        counter++;
                    }
                }

                string strArticleURLParamFromData = "", strCompleteURL = "";
                string AdocTOC;
                string category;
                string AdocTOCref;
                var accessionNumber = "";
                var AdocTOCURL = "";

                var product = inputData.getItem("from").ToLower();
                switch (product)
                {
                    case "nl2pcast":
                    case "ws1pcast":
                    case "nl2":
                    case "nl1":
                    case "ws1":
                        #region << Workspace RSS & Podcasts >>
                        var audience = (Audience)Deserialize(inputData.getItem("audience"), typeof(Audience));
                        // If it's an external weblink or insight chart link build the compelete url .
                        if (fldDirectUrls.Length > 0)
                        {
                            foreach (var fldDirectUrl in fldDirectUrls)
                            {
                                strCompleteURL = headlinesData.getItem(fldDirectUrl, headlineNode);
                            }
                        }

                        if (strCompleteURL == "")
                        {
                            AdocTOC = headlinesData.getItem("category", headlineNode);

                            for (var i = 0; i < fldArticleURL.Count; i++)
                                accessionNumber = headlinesData.getItem(fldArticleURL[i], headlineNode);

                            var urlBuilder = new UrlBuilder(ArticleUrl);

                            var properties = new UserProperties(inputData.getItem("accountid"),
                                isMct ? audience.ProxyCredentials.Namespace : inputData.getItem("ns"),
                                isMct ? audience.ProxyCredentials.UserId : inputData.getItem("userid"));

                            var audienceParams = AudienceManager.GetDictionaryForNonTrackArticleCycloneLink(accessionNumber, audience, properties);

                            if (isMct)
                            {
                                if (null != audienceParams)
                                {
                                    if (audienceParams.ContainsKey("f"))
                                        audienceParams["f"] = "nv";
                                    else
                                        audienceParams.Add("f", "nv");
                                }
                                if (null != urlBuilder && null != urlBuilder.QueryString && urlBuilder.QueryString.ContainsKey("napc")) { urlBuilder.QueryString["napc"] = "MC"; }
                            }

                            urlBuilder.Add(audienceParams);

                            urlBuilder.Append("AN", accessionNumber);

                            //CAT=I|picture, CAT=W|webpage, CAT=A |Publications(is the default)
                            switch (AdocTOC)
                            {
                                case "articlewithgraphics":
                                case "article":
                                    AdocTOCURL = "a";
                                    break;
                                case "webpage":
                                    AdocTOCURL = "w";
                                    break;
                                case "picture":
                                    AdocTOCURL = "i";
                                    break;
                                case "multimedia":
                                    AdocTOCURL = "m";
                                    break;
                                case "pdf":
                                    AdocTOCURL = "p";
                                    break;
                                case "file":
                                    AdocTOCURL = "f";
                                    break;
                                case "blog":
                                    AdocTOCURL = "b";
                                    break;
                                case "board":
                                    AdocTOCURL = "o";
                                    break;
                                case "chartImageItem":
                                    AdocTOCURL = "ci";
                                    break;
                            }
                            urlBuilder.Append(AdocTOC.Equals("chartImageItem") ? "ct" : "cat", AdocTOCURL);
                            urlBuilder.Append("nldtl", nldtl);

                            switch (product)
                            {
                                case "ws1":
                                    operationalDataMemento = GetWorkspaceOperationalDataMemento(inputData.getItem("workspaceID"), inputData.getItem("workspaceName"), audience, false);
                                    urlBuilder.Append("mod", "workspace_rss");
                                    break;
                                case "ws1pcast":
                                    operationalDataMemento = GetWorkspaceOperationalDataMemento(inputData.getItem("workspaceID"), inputData.getItem("workspaceName"), audience, true);
                                    urlBuilder.Append("mod", "workspace_rss");
                                    break;
                                case "nl2":
                                case "nl1":
                                    operationalDataMemento = GetNewsletterOperationalDataMemento(inputData.getItem("newsletterID"), inputData.getItem("newsletterName"), audience, false);
                                    urlBuilder.Append("mod", "newsletter_rss");
                                    break;
                                case "nl2pcast":
                                    operationalDataMemento = GetNewsletterOperationalDataMemento(inputData.getItem("newsletterID"), inputData.getItem("newsletterName"), audience, true);
                                    urlBuilder.Append("mod", "newsletter_rss");
                                    break;
                            }
                            urlBuilder.Append("od", HttpContext.Current.Server.UrlEncode(operationalDataMemento));

                            strCompleteURL = urlBuilder.ToString();
                        }
                        #endregion
                        break;
                    case "g1":
                    case "g2":
                    case "fce1":
                    case "fdk":
                        #region Search2Headlines
                        if (fldDirectUrls.Length > 0)
                        {
                            foreach (var fldDirectUrl in fldDirectUrls)
                            {
                                strCompleteURL = headlinesData.getItem(fldDirectUrl, headlineNode);
                            }
                        }
                        if (strCompleteURL == "")
                        {
                            AdocTOC = headlinesData.getItem("category", headlineNode);
                            var app = inputData.getItem("app");
                            if (!string.IsNullOrEmpty(app) && app.ToUpper() == "WSJ")  //app is WSJ
                            {
                                strCompleteURL = ArticleUrl;
                                if (!string.IsNullOrEmpty(headlinesData.getItem(fldArticleURL[0], headlineNode)))
                                {
                                    strCompleteURL = strCompleteURL + (headlinesData.getItem(fldArticleURL[0], headlineNode).Split(" ").Length > 1 ? headlinesData.getItem(fldArticleURL[0], headlineNode).Split(" ")[1] : headlinesData.getItem(fldArticleURL[0], headlineNode).Split(" ")[0]) + ".html";
                                }
                            }
                            else
                            {
                                //other applications
                                for (var i = 0; i < fldArticleURL.Count; i++)
                                {
                                    strArticleURLParamFromData += ArticleUrl.IndexOf("?") > 0 ? "&" : "?";
                                    strArticleURLParamFromData += fldArticleURL.Keys[i] + "=" + headlinesData.getItem(fldArticleURL[i], headlineNode);
                                }
                                //CAT=I|picture, CAT=W|webpage, CAT=A |Publications(is the default)
                                switch (AdocTOC)
                                {
                                    case "article":
                                        strCompleteURL = ArticleUrl + "&cat=a" + strArticleURLParamFromData;
                                        break;
                                    case "webpage":
                                        strCompleteURL = ArticleUrl + "&cat=w" + strArticleURLParamFromData;
                                        break;
                                    case "picture":
                                        strCompleteURL = ArticleUrl + "&cat=i" + strArticleURLParamFromData;
                                        break;
                                    case "multimedia":
                                        strCompleteURL = ArticleUrl + "&cat=m" + strArticleURLParamFromData;
                                        break;
                                    case "analyst":
                                        AdocTOCref = HttpContext.Current.Server.UrlEncode(headlinesData.getItem("reference", headlineNode));
                                        strCompleteURL = ArticleUrl + strArticleURLParamFromData + "&cat=n&r=" + AdocTOCref;
                                        break;
                                    case "pdf":
                                        strCompleteURL = ArticleUrl + "&cat=p" + strArticleURLParamFromData;
                                        break;
                                    case "file":
                                        strCompleteURL = ArticleUrl + "&cat=f" + strArticleURLParamFromData;
                                        break;
                                    case "blog":
                                        strCompleteURL = ArticleUrl + "&cat=b" + strArticleURLParamFromData;
                                        break;
                                    case "board":
                                        strCompleteURL = ArticleUrl + "&cat=o" + strArticleURLParamFromData;
                                        break;
                                    case "tfreg":
                                    case "tfprx":
                                    case "tf80k":
                                    case "tf40k":
                                    case "tf20k":
                                    case "tf10q":
                                    case "tf10k":
                                    case "tfino":
                                    case "tfins":
                                    case "ttfil":
                                    case "coinfo":
                                    case "eolfil":
                                        AdocTOCref = HttpContext.Current.Server.UrlEncode(headlinesData.getItem("reference", headlineNode));
                                        strCompleteURL = ArticleUrl + strArticleURLParamFromData + "&cat=r&r=" + AdocTOCref;
                                        break;
                                    default:
                                        category = AdocTOC;
                                        strCompleteURL = ArticleUrl + strArticleURLParamFromData + "&cat=" + category;
                                        AdocTOCref = HttpContext.Current.Server.UrlEncode(headlinesData.getItem("reference", headlineNode));
                                        if (AdocTOCref != "")
                                            strCompleteURL = strCompleteURL + "&r=" + AdocTOCref;
                                        break;
                                }
                            }
                            strCompleteURL = strCompleteURL.Replace("?&", "?");
                        }
                        #endregion
                        break;
                    // similar to above
                    default:
                        if (fldDirectUrls.Length > 0)
                        {
                            foreach (var fldDirectUrl in fldDirectUrls)
                            {
                                strCompleteURL = headlinesData.getItem(fldDirectUrl, headlineNode);
                            }
                        }
                        if (strCompleteURL == "")
                        {
                            AdocTOC = headlinesData.getItem(@"AdocTOC/@adoctype", headlineNode);

                            if (!string.IsNullOrEmpty(inputData.getItem("app")) && inputData.getItem("app").ToUpper() == "WSJ")  //app is WSJ
                            {
                                strCompleteURL = ArticleUrl;
                                if (!string.IsNullOrEmpty(headlinesData.getItem(fldArticleURL[0], headlineNode)))
                                {
                                    strCompleteURL = strCompleteURL + (headlinesData.getItem(fldArticleURL[0], headlineNode).Split(" ").Length > 1 ? headlinesData.getItem(fldArticleURL[0], headlineNode).Split(" ")[1] : headlinesData.getItem(fldArticleURL[0], headlineNode).Split(" ")[0]) + ".html";
                                }
                            }
                            else
                            {
                                //other applications
                                for (var i = 0; i < fldArticleURL.Count; i++)
                                {
                                    strArticleURLParamFromData += ArticleUrl.IndexOf("?") > 0 ? "&" : "?";
                                    strArticleURLParamFromData += fldArticleURL.Keys[i] + "=" +
                                                                  headlinesData.getItem(fldArticleURL[i], headlineNode);
                                }
                                //CAT=I|picture, CAT=W|webpage, CAT=A |Publications(is the default)
                                switch (AdocTOC)
                                {
                                    case "article":
                                        strCompleteURL = ArticleUrl + "&cat=a" + strArticleURLParamFromData;
                                        break;
                                    case "webpage":
                                        strCompleteURL = ArticleUrl + "&cat=w" + strArticleURLParamFromData;
                                        break;
                                    case "picture":
                                        strCompleteURL = ArticleUrl + "&cat=i" + strArticleURLParamFromData;
                                        break;
                                    case "multimedia":
                                        strCompleteURL = ArticleUrl + "&cat=m" + strArticleURLParamFromData;
                                        break;
                                    case "analyst":
                                        AdocTOCref = HttpContext.Current.Server.UrlEncode(headlinesData.getItem(@"AdocTOC/Item/@ref", headlineNode));
                                        strCompleteURL = ArticleUrl + strArticleURLParamFromData + "&cat=n&r=" + AdocTOCref;
                                        break;
                                    case "pdf":
                                        strCompleteURL = ArticleUrl + "&cat=p" + strArticleURLParamFromData;
                                        break;
                                    case "file":
                                        strCompleteURL = ArticleUrl + "&cat=f" + strArticleURLParamFromData;
                                        break;
                                    case "tfreg":
                                    case "tfprx":
                                    case "tf80k":
                                    case "tf40k":
                                    case "tf20k":
                                    case "tf10q":
                                    case "tf10k":
                                    case "tfino":
                                    case "tfins":
                                    case "ttfil":
                                    case "coinfo":
                                    case "eolfil":
                                        AdocTOCref = HttpContext.Current.Server.UrlEncode(headlinesData.getItem(@"AdocTOC/Item/@ref", headlineNode));
                                        strCompleteURL = ArticleUrl + strArticleURLParamFromData + "&cat=r&r=" + AdocTOCref;
                                        break;
                                    default:
                                        // unrecognized category, do not pass cat=
                                        //category = AdocTOC;
                                        strCompleteURL = ArticleUrl + strArticleURLParamFromData;// +"&cat=" + category;
                                        //AdocTOCref = HttpContext.Current.Server.UrlEncode(headlinesData.getItem(@"AdocTOC/Item/@ref", headlineNode));
                                        //if (AdocTOCref != "")
                                        //    strCompleteURL = strCompleteURL + "&r=" + AdocTOCref;
                                        break;
                                }
                            }
                            strCompleteURL = strCompleteURL.Replace("?&", "?");
                        }
                        break;
                }

                rssFeed.Channel.AddItem(CreateItem(title, description, date, source, strCompleteURL, strCompleteURL, enclosure, inputData, operationalDataMemento));
            }
            return Serialize(rssFeed, false);
        }

        protected virtual string CreateItem(string title, string description, string date, string source, string articleURL, string guid)
        {
            var data = date.Split(',');
            var item = new StringBuilder();

            item.Append("<item>");

            item.Append("<title><![CDATA[");
            item.Append(title);
            item.Append("]]></title>");

            item.Append("<description><![CDATA[");
            item.Append(description);
            item.Append("]]></description>");


            item.Append("<link>");
            item.Append(articleURL);
            item.Append("</link>");

            item.Append("<author><![CDATA[");
            item.Append(source);
            item.Append("]]></author>");

            item.Append("<pubDate>");
            if (!string.IsNullOrEmpty(data[0]) && !string.IsNullOrEmpty(data[1]))
                item.Append(FormatDate(data[0], data[1]));
            if (data.Length > 2)
                item.Append(FormatDate(data[2], data[3]));
            item.Append("</pubDate>");

            item.Append("<guid>");
            item.Append(articleURL);
            item.Append("</guid>");

            item.Append("</item>");

            return item.ToString();
        }

        protected virtual RssItem CreateItem(string title, string description, string date, string source, string articleURL, string guid, string an, InputData inputData, string operationalDataMemento)
        {
            //var xmlDoc = new XmlDocument();
            var uid = string.Empty;
            var accountid = string.Empty;
            var _namespace = string.Empty;
            var item = new RssItem();
            var data = date.Split(',');

            try
            {
                // Paramaters required for creating podcast.

                if (!string.IsNullOrEmpty(inputData.getItem("uid")))
                    uid = inputData.getItem("uid");

                if (!string.IsNullOrEmpty(inputData.getItem("userID")))
                    uid = inputData.getItem("userID");

                if (!string.IsNullOrEmpty(inputData.getItem("namespace")))
                    _namespace = inputData.getItem("namespace");

                if (!string.IsNullOrEmpty(inputData.getItem("ns")))
                    _namespace = inputData.getItem("ns");

                if (!string.IsNullOrEmpty(inputData.getItem("accountid")))
                    accountid = inputData.getItem("accountid");

                if (!string.IsNullOrEmpty(inputData.getItem("aid")))
                    accountid = inputData.getItem("aid");


                item.Title = title;
                item.Description = description;

                if (!string.IsNullOrEmpty(articleURL))
                {
                    if (!articleURL.StartsWith("https://"))
                    {
                        if (articleURL.StartsWith("//"))
                        {
                            articleURL = string.Concat("https:", articleURL);
                        }
                        /*else if (articleURL.StartsWith("http:"))
                        {
                            articleURL = articleURL.Replace("http:", "https:");
                        }*/
                    }
                }

                item.Link = new EscapedUri(articleURL);
                item.Author = source;

                try
                {
                    DateTime temp;
                    if (DateTimeFormatter.ParseDate(date, out temp))
                    {
                        item.PublicationDate = temp;
                    }
                }
                catch (Exception _ex) { }

                try
                {
                    item.PublicationDate = DateTime.Parse(FormatDate(data[0], data[1]));
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: Publication Date- Time  [5500050] " + ex);
                }
                try
                {
                    if (!string.IsNullOrEmpty(data[2]))
                        item.PublicationDate = DateTime.Parse(FormatDate(data[2], data[3]));
                }
                catch (Exception ex)
                {
                }


                item.Guid = new RssGuid(guid, false);

                if (IsPodCast)
                {
                    var integrationType = IntegrationType.PodcastWorkspace;
                    if ((inputData.getItem("from").ToLower() == "ws1pcast") || (inputData.getItem("from").ToLower() == "nl2pcast"))
                    {
                        switch (inputData.getItem("from").ToLower())
                        {
                            default:
                            case "ws1pcast":
                                integrationType = IntegrationType.PodcastWorkspace;
                                break;
                            case "nl2pcast":
                                integrationType = IntegrationType.PodcastNewsletter;
                                break;
                        }
                    }

                    var podcastUriStandardPlayer = new EscapedUri(GetPodcastMediaUrl(uid, accountid, _namespace, an, ContentLanguage.en, Factiva.BusinessLayerLogic.ContentCategory.Publications, string.Empty, MediaRedirectionType.StandardPlayer, integrationType));
                    var podcastUriForFile = new EscapedUri(GetPodcastMediaUrl(uid, accountid, _namespace, an, ContentLanguage.en, Factiva.BusinessLayerLogic.ContentCategory.Publications, string.Empty, MediaRedirectionType.UrlToSoundFile, integrationType));
                    var enclosure = new RssEnclosure
                                        {
                                            ContentType = "audio/mpeg",
                                            Length = 300000,
                                            Url = podcastUriForFile
                                        };

                    item.Enclosures.Add(enclosure);

                    // Add itunes piece
                    var itemITunesExtension = new ITunesSyndicationExtension
                                                  {
                                                      Context =
                                                          {
                                                              Summary = title,
                                                              Author = source,
                                                              Subtitle = string.Empty,
                                                              ExplicitMaterial = ITunesExplicitMaterial.No
                                                          }
                                                  };

                    var yahooMediaCredit = new YahooMediaCredit
                                               {
                                                   Role = "author",
                                                   Entity = source.Length > 0 ? source : "Unknown Author"
                                               };

                    var yahooMediaContent = new YahooMediaContent
                                                {
                                                    Url = podcastUriForFile,
                                                    FileSize = 300000,
                                                    ContentType = "audio/mpeg"
                                                };

                    var yahooMediaSyndicationExtension = new YahooMediaSyndicationExtension();
                    yahooMediaSyndicationExtension.Context.Credits.Add(yahooMediaCredit);
                    yahooMediaSyndicationExtension.Context.AddContent(yahooMediaContent);

                    item.AddExtension(itemITunesExtension);
                    item.AddExtension(yahooMediaSyndicationExtension);
                    item.Description = string.Format("{0}<BR><a href=\"{1}\">MP3</a>", description, podcastUriStandardPlayer);
                }
            }
            catch (Exception ex)
            {
                //Logger.Log(Logger.Level.ERROR, string.Format("CreatePodCast (7273663-884) {0}", ex));
                throw ex;
            }
            return item;
        }

        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="useStylesheet">if set to <c>true</c> [use stylesheet].</param>
        /// <returns></returns>
        protected virtual string Serialize(ISyndicationResource feed, bool useStylesheet)
        {
            //create an xmlwriter and then write nothing to this to fake and remove xml decl
            var sb = new StringBuilder();
            using (var sw = new StringWriterWithEncodingClass(sb, Encoding.UTF8))
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.None;
                    feed.Save(writer);
                    sw.Flush();
                    sw.Close();
                }
            }
            if (useStylesheet)
            {
                sb.Insert(0, "<?xml-stylesheet type=\"text/xsl\" href=\"/syndication/podcast/default.xsl\"?>");
            }
            return sb.Insert(0, "<?xml version=\"1.0\" encoding=\"utf-8\"?>").ToString();
        }

        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <returns></returns>
        protected virtual string Serialize(ISyndicationResource feed)
        {
            return Serialize(feed, false);
        }

        private static string GetPodcastMediaUrl(string userId, string accountId, string productId, string accessionNumber, ContentLanguage contentLanguage, Factiva.BusinessLayerLogic.ContentCategory contentCategory, string operationalDataMemento, MediaRedirectionType mediaRedirectionType, IntegrationType integrationType)
        {
            try
            {
                //Logger.Log(Logger.Level.INFO, string.Format("GetPodcastMediaUrl:: userId :- {0} | accountId :-{1} | productId :-{2} | accessionNumber :-{3} | contentLanguage :-{4} | contentCategory :-{5}",
                //                            userId,
                //                            accountId,
                //                            productId,
                //                            accessionNumber,
                //                            contentLanguage,
                //                            contentCategory
                //                  ));

                var urlBuilder = new AudioMediaUrlBuilder(mediaRedirectionType)
                                     {
                                         AccessionNumber = accessionNumber,
                                         UserId = userId,
                                         AccountId = accountId,
                                         NameSpace = productId,
                                         ContentCategory = contentCategory.ToString(),
                                         ContentLanguage = contentLanguage.ToString(),
                                         IntegrationType = integrationType
                                     };
                //urlBuilder.OperationalDataMemento = operationalDataMemento;
                return urlBuilder.ToString();
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: " + ex);
                return null;
            }
        }

        protected virtual string FormatDate(string pubDate, string pubTime)
        {
            var finalPubDate = "";
            var tmpDate = "";
            var tmpTime = "";
            try
            {
                if (pubDate.Length == 8)
                {
                    tmpDate = Left(pubDate, 4) + "/" + (pubDate.Substring(4, 2)) + "/" + Right(pubDate, 2);
                }

                if (pubTime != "")
                {
                    tmpTime = Left(pubTime, 2) + ":" + (pubTime.Substring(2, 2)); // + " GMT";
                }

                finalPubDate = tmpDate + " " + tmpTime;
                if (finalPubDate.Trim() != "")
                {
                    finalPubDate = System.Convert.ToString(System.Convert.ToDateTime(finalPubDate).ToString("R"));
                }

                //if time is blank then remove 00:00:00 GMT
                if (pubTime == "")
                {
                    if (finalPubDate.IndexOf("00:00:00 GMT", StringComparison.Ordinal) != -1)
                    {
                        finalPubDate = Left(finalPubDate, finalPubDate.IndexOf("00:00:00 GMT", StringComparison.Ordinal));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: formatDate[5500051] " + ex);
            }
            return finalPubDate;

        }

        public static String Left(String strParam, int iLen)
        {
            try
            {
                return iLen > 0 ? strParam.Substring(0, iLen) : strParam;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: Left [5500052] " + ex);
            }
            return strParam;
        }

        //Function to get string from end
        public static String Right(String strParam, int iLen)
        {
            try
            {
                return iLen > 0 ? strParam.Substring(strParam.Length - iLen, iLen) : strParam;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Level.ERROR, "RequestGeneratorBase.cs :: Right [5500053] " + ex);
            }
            return strParam;
        }

        public static object Deserialize(string xmlRequest, Type objectType)
        {
            //System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequest);
            var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);

            var xs = new XmlSerializer(objectType);
            var obj = xs.Deserialize(xmlReader);
            return obj;
        }

        private static string GetNLDTLToken(string newsletterId, string _type)
        {
            const string ercPubKey = "3x4e10e4";
            var enc = new FactivaEncryption.encryption();
            var nvp = new NameValueCollection(1) { { "nlId", newsletterId }, { "nt", _type } };

            return HttpUtility.UrlEncode(enc.encrypt(nvp, ercPubKey));
        }
    }
}
