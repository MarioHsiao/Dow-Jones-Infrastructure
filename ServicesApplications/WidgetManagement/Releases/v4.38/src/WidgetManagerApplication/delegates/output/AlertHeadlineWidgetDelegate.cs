using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Argotic.Common;
using Argotic.Extensions.Core;
using Argotic.Syndication;
using EMG.Utility.Handlers.Syndication.Podcast.Core;
using EMG.widgets.Managers;
using EMG.widgets.ui.delegates.abstractClasses;
using EMG.widgets.ui.delegates.core;
using EMG.widgets.ui.delegates.interfaces;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.utility.headline;
using Factiva.BusinessLayerLogic;
using Factiva.BusinessLayerLogic.DataTransferObject;
using Factiva.BusinessLayerLogic.DataTransferObject.Widget;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.BusinessLayerLogic.Utility;
using Factiva.BusinessLayerLogic.Utility.Xml;
using Factiva.Gateway.Messages.Assets.V1_0;
using Factiva.Gateway.Messages.Assets.WebWidgets.V1_0;
using Factiva.Gateway.Messages.Search.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using factiva.nextgen;
using factiva.nextgen.ui;
using log4net;
using Encryption = FactivaEncryption.encryption;
using BaseUrlBuilder = EMG.Utility.Uri.UrlBuilder;
using ContentHeadline=Factiva.Gateway.Messages.Search.V1_0.ContentHeadline;
using ContentLanguage=Factiva.BusinessLayerLogic.ContentLanguage;
using ControlData=Factiva.Gateway.Utils.V1_0.ControlData;
using Folder=Factiva.Gateway.Messages.Track.V1_0.Folder;
using Status=EMG.widgets.ui.delegates.core.Status;

namespace EMG.widgets.ui.delegates.output
{
    /// <summary>
    /// Stucture that contains translated strings
    /// </summary>
    public struct AlertHeadlineWidgetLiterals
    {
        /// <summary>
        /// Url for Branding Image
        /// </summary>
        public string BrandingBadge;

        /// <summary>
        /// CopyRight token
        /// </summary>
        public string CopyRight;

        /// <summary>
        /// Url for the flash player movie url.
        /// </summary>
        public string DewplayerFlashUrl;

        /// <summary>
        /// Url for Icon Image
        /// </summary>
        public string Icon;

        /// <summary>
        /// Url for the Marketing Site title
        /// </summary>
        public string MarketingSiteTitle;

        /// <summary>
        /// Url for the Marketing Site
        /// </summary>
        public string MarketingSiteUrl;

        /// <summary>
        /// No results token.
        /// </summary>
        public string NoResults;

        /// <summary>
        /// Speaker Icon Url.
        /// </summary>
        public string SpeakerIconUrl;

        /// <summary>
        /// View Less token.
        /// </summary>
        public string ViewLess;

        /// <summary>
        /// View More token.
        /// </summary>
        public string ViewMore;

        /// <summary>
        /// View More token.
        /// </summary>
        public string ViewAll;
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class AlertHeadlineWidgetDelegate : AbstractHeadlineWidgetDelegate, IWidgetSyndicationDelegate, IWidgetPodcastDelegate, IWidgetPreviewDelegate
    {
        private const int m_Base_Eclosure_Size = 300000;
        private const int m_Chunck_Size = 5;
        private const int m_Delegate_Timeout = 20*1000;
        private const string m_Marketing_Site_Title = "Dow Jones Factiva";
        private const string m_Marketing_Site_Url = "http://www.factiva.com";
        private const int m_TimeToLive_Podcast_RSS = 60;
        private const int m_TimeToLive_RSS = 15;
        private static readonly ILog m_Log = LogManager.GetLogger(typeof (AlertHeadlineWidgetDelegate));
        private readonly ResourceText m_ResourceText;

        /// <summary>
        /// Data container of the headline Widget Delegate.
        /// </summary>
        public AlertWidgetData Data;

        /// <summary>
        /// Definition container of the headline Widget Delegate.
        /// </summary>
        public AlertHeadlineWidgetDefinition Definition;

        /// <summary>
        /// Baseline CopyRight used for tokenization of phrases.
        /// </summary>
        public AlertHeadlineWidgetLiterals Literals;

        private SessionData m_SessionData;
        private string m_Token;
        private WidgetManager m_WidgetManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertHeadlineWidgetDelegate"/> class.
        /// </summary>
        public AlertHeadlineWidgetDelegate()
        {
            if (ResourceText.GetInstance != null)
            {
                m_ResourceText = ResourceText.GetInstance;
            }
            if (SessionData.Instance() != null)
            {
                m_SessionData = SessionData.Instance();
            }
        }


        /// <summary>
        /// Gets or sets the session data.
        /// </summary>
        /// <value>The session data.</value>
        [XmlIgnore]
        [ScriptIgnore]
        public SessionData SessionData
        {
            get { return m_SessionData; }
            set { m_SessionData = value; }
        }

        #region IWidgetPodcastDelegate Members

        /// <summary>
        /// Deserializes to a  Podcast RSS.
        /// </summary>
        /// <returns></returns>
        public string ToPodcastRSS()
        {
            if (string.IsNullOrEmpty(m_Token) || string.IsNullOrEmpty(m_Token.Trim()))
                return string.Empty;
            if (Definition == null)
            {
                return string.Empty;
            }
            WidgetTokenProperties properties = new WidgetTokenProperties(m_Token);

            RssFeed rssFeed = new RssFeed();
            rssFeed.Channel = new RssChannel();
            rssFeed.Channel.Title = Definition.Name;
            rssFeed.Channel.Copyright = Literals.CopyRight;
            rssFeed.Channel.PublicationDate = DateTime.Now;
            rssFeed.Channel.TimeToLive = m_TimeToLive_Podcast_RSS;

            // Add the ITunes Syndication Extention 
            ITunesSyndicationExtension iTunesExtension = new ITunesSyndicationExtension();
            iTunesExtension.Context.Summary = string.Empty;
            iTunesExtension.Context.Author = Literals.CopyRight;
            iTunesExtension.Context.Subtitle = string.Empty;
            rssFeed.AddExtension(iTunesExtension);

            YahooMediaSyndicationExtension yahooMediaSyndicationExtension = new YahooMediaSyndicationExtension();
            rssFeed.AddExtension(yahooMediaSyndicationExtension);

            if (Data != null && Data.Count > 0)
            {
                foreach (AlertInfo info in Data.Alerts)
                {
                    RssCategory category = new RssCategory(info.Name);
                    foreach (AlertHeadlineInfo headline in info.Headlines)
                    {
                        if (IsContentInAPodcastEnableLanguage(headline.ContentLanguage) &&
                            headline.WC <= 5000 && headline.ContentCategory == ContentCategory.Publications)
                        {
                            // Add rss main piece
                            Uri podcastUri = new Uri(GetPodcastMediaUrl(properties, headline), UriKind.Absolute);

                            RssItem item = new RssItem();
                            item.Title = headline.Text;
                            item.Description = GetDescription(headline.Snippet, podcastUri.ToString());
                            item.Link = new Uri(headline.Url);
                            item.Author = headline.SrcName;
                            item.PublicationDate = headline.PublicationDateTime;
                            item.Guid = new RssGuid(item.Link.ToString(), false);
                            /*item.Enclosures.Add(
                                new RssEnclosure(m_Base_Eclosure_Size,
                                    "audio/mpeg",
                                    podcastUri
                                    )
                                );*/
                            RssEnclosure enclosure = new RssEnclosure();
                            enclosure.ContentType = "audio/mpeg";
                            enclosure.Length = m_Base_Eclosure_Size;
                            enclosure.Url = podcastUri;

                            item.Enclosures.Add(enclosure);

                            // Add itunes piece
                            ITunesSyndicationExtension itemITunesExtension = new ITunesSyndicationExtension();
                            itemITunesExtension.Context.Summary = string.Empty;
                            itemITunesExtension.Context.Author = headline.SrcName;
                            itemITunesExtension.Context.Subtitle = string.Empty;
                            item.AddExtension(itemITunesExtension);

                            // add category
                            item.Categories.Add(category);
                            rssFeed.Channel.AddItem(item);
                        }
                    }
                }
            }
            return Serialize(rssFeed, false);
        }

        #endregion

        #region IWidgetPreviewDelegate Members

        /// <summary>
        /// Fills the preview.
        /// </summary>
        public void FillPreview(List<int> alertIds)
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);

                Definition = new AlertHeadlineWidgetDefinition();
                PopulateAlertHeadlineWidgetLiterals();

                // Go to PAM and get last create widget and update select parameters.
                // This acts like preferences. 
                AlertWidget alertWidget = null;
                try
                {
                    alertWidget = m_WidgetManager.GetLatestUpdatedAlertWidget();
                }
                catch (FactivaBusinessLogicException)
                {
                    // Do not show error keep defaults
                }
                catch (Exception ex)
                {
                    new FactivaWidgetsUIException("Unable to get the GetLatestUpdatedAlertWidget", ex);
                    ReturnCode = -1;
                    StatusMessage = m_ResourceText.GetErrorMessage(ReturnCode.ToString());
                    return;
                }

                if (alertWidget != null)
                {
                    MergeWidgetDefinition(alertWidget, Definition);
                }

                // dacostad[10.27.2008] -- removed functionality due to Knit Picking 
                // Definition.Name = m_ResourceText.GetString("defaultWidgetName");
                Definition.Name = string.Empty;
                
                if (alertIds != null && alertIds.Count > 0)
                {
                    ProcessAlertInfos(alertIds);
                }
            }
        }

        /// <summary>
        /// Fills the preview.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        public void FillPreview(string widgetId)
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);


                try
                {
                    Widget widget = m_WidgetManager.GetWidgetById(widgetId);
                    if (widget != null)
                    {
                        AlertWidget alertWidget = widget as AlertWidget;
                        if (alertWidget != null)
                        {
                            PopulateAlertHeadlineWidgetLiterals();

                            // Populate with AlertHeadlineWidgetdDelegate
                            PopulateAlertHeadlineWidgetDefinition(alertWidget, m_SessionData.InterfaceLanguage);

                            // Get a list of ids out of the definition
                            if (alertWidget.Component != null &&
                                alertWidget.Component.AlertCollection != null &&
                                alertWidget.Component.AlertCollection.Count > 0)
                            {
                                List<int> alertIds = new List<int>();

                                foreach (AlertItem item in alertWidget.Component.AlertCollection)
                                {
                                    int temp;
                                    if (Int32.TryParse(item.ItemId, out temp) &&
                                        !alertIds.Contains(temp))
                                    {
                                        alertIds.Add(temp);
                                    }
                                }

                                if (alertIds.Count > 0)
                                {
                                    ProcessAlertInfos(alertIds);
                                }
                            }
                        }
                    }
                }
                catch (FactivaBusinessLogicException fex)
                {
                    Data = null;
                    Definition = null;
                    ReturnCode = fex.ReturnCodeFromFactivaService;
                    StatusMessage = m_ResourceText.GetErrorMessage(ReturnCode.ToString());
                }
                catch (Exception ex)
                {
                    // Log the exception 
                    new FactivaWidgetsUIException("Unable to render widget.", ex);
                    Data = null;
                    Definition = null;
                    ReturnCode = -1;
                    StatusMessage = m_ResourceText.GetErrorMessage(ReturnCode.ToString());
                }
            }
        }

        #endregion

        #region IWidgetSyndicationDelegate Members

        /// <summary>
        /// Fills this instance.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="integrationTarget">Type of the integration.</param>
        public void Fill(string token, IntegrationTarget integrationTarget)
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_Token = token;
                WidgetTokenProperties tokenProperties = new WidgetTokenProperties(token);
                ControlData userControlData = ControlDataManager.AddProxyCredentialsToControlData(ControlDataManagerEx.GetRssFeed1LightWeightUser(), tokenProperties.UserId, tokenProperties.NameSpace);
                m_WidgetManager = new WidgetManager(userControlData, SessionData.Instance().InterfaceLanguage);

                try
                {
                    // Get the widget Definition
                    Widget widget = m_WidgetManager.GetWidgetById(tokenProperties.WidgetId);
                    if (widget != null)
                    {
                        if (widget is AlertWidget)
                        {
                            AlertWidget alertWidget = (AlertWidget) widget;
                            // Pull out the interfacelanguage
                            string interfaceLanguage = alertWidget.Properties.Language;

                            // Check cache to see if widget exists
                            // See if widget is in Factia cache
                            string cachedData;
                            if (m_WidgetManager.IsWidgetInFactivaCache(tokenProperties.WidgetId, interfaceLanguage , out cachedData))
                            {
                                AlertHeadlineWidgetDelegate cachedDelegate =
                                    (AlertHeadlineWidgetDelegate)
                                    Factiva.BusinessLayerLogic.Delegates.Utility.Deserialize(cachedData, GetType());
                                // Make sure a valid item was saved in cache
                                if (cachedDelegate != null)
                                {
                                    // ignore return code.
                                    Data = cachedDelegate.Data;
                                    Definition = cachedDelegate.Definition;
                                    ReturnCode = cachedDelegate.ReturnCode;
                                    StatusMessage = cachedDelegate.StatusMessage;
                                    Literals = cachedDelegate.Literals;
                                    // Update Urls based ResponseFormat()
                                    UpdateUrls(tokenProperties, integrationTarget, interfaceLanguage);
                                    FireMetricsEnvelope(tokenProperties.WidgetId);
                                    return;
                                }
                            }
                            
                            // Set the ui culture on the thread.
                            m_SessionData = new SessionData("b", interfaceLanguage, 0, false);
                            PopulateAlertHeadlineWidgetLiterals();

                            // Populate with AlertHeadlineWidgetdDelegate
                            PopulateAlertHeadlineWidgetDefinition(alertWidget, null);
                            PopulateHeadlineWidgetData(alertWidget, tokenProperties.AccountId, tokenProperties.UserId,
                                                       tokenProperties.NameSpace);
                            ValidateAllFolders();
                            SerializeWidgetToCache(tokenProperties.WidgetId, interfaceLanguage);
                            // Update Urls based ResponseFormat()
                            UpdateUrls(tokenProperties, integrationTarget, interfaceLanguage);
                            FireMetricsEnvelope(tokenProperties.WidgetId);
                            return;
                        }
                    }
                    else
                    {
                        throw new FactivaWidgetsUIException(FactivaBusinessLogicException.INVALID_ALERT_WIDGET);
                    }
                    
                }
                catch (FactivaBusinessLogicException fex)
                {
                    Data = null;
                    Definition = null;
                    ReturnCode = fex.ReturnCodeFromFactivaService;
                    StatusMessage = m_ResourceText.GetErrorMessage(ReturnCode.ToString());
                }
                catch (Exception ex)
                {
                    // Log the exception 
                    new FactivaWidgetsUIException("Unable to render widget.", ex);
                    Data = null;
                    Definition = null;
                    ReturnCode = -1;
                    StatusMessage = m_ResourceText.GetErrorMessage(ReturnCode.ToString());
                }
            }
        }

        /// <summary>
        /// Deserializes to RSS.
        /// </summary>
        /// <returns></returns>
        public string ToRSS()
        {
            if (Definition == null)
            {
                return string.Empty;
            }

            RssFeed rssFeed = new RssFeed();
            rssFeed.Channel = new RssChannel();
            rssFeed.Channel.Title = Definition.Name;
            rssFeed.Channel.Copyright = Literals.CopyRight;
            rssFeed.Channel.PublicationDate = DateTime.Now;
            rssFeed.Channel.TimeToLive = m_TimeToLive_RSS;


            if (Data != null && Data.Count > 0)
            {
                foreach (AlertInfo info in Data.Alerts)
                {
                    RssCategory category = new RssCategory(info.Name);
                    foreach (AlertHeadlineInfo headline in info.Headlines)
                    {
                        RssItem item = new RssItem(); //new RssItem(headline.Text, headline.Snippet, new Uri(headline.Url));
                        item.Title = headline.Text;
                        item.Description = headline.Snippet;
                        item.Link = new Uri(headline.Url);
                        item.Categories.Add(category);
                        rssFeed.Channel.AddItem(item);
                    }
                }
            }
            return Serialize(rssFeed);
        }

        /// <summary>
        /// Deserializes to ATOM.
        /// </summary>
        /// <returns></returns>
        public string ToATOM()
        {
            if (Definition == null)
            {
                return string.Empty;
            }
            AtomFeed atomFeed = new AtomFeed();
            atomFeed.Title = new AtomTextConstruct(Definition.Name);
            atomFeed.UpdatedOn = DateTime.Now;
            atomFeed.Rights = new AtomTextConstruct(Literals.CopyRight);
            atomFeed.UpdatedOn = DateTime.Now;

            if (Data != null && Data.Count > 0)
            {
                foreach (AlertInfo info in Data.Alerts)
                {
                    AtomCategory category = new AtomCategory(info.Name);
                    atomFeed.Categories.Add(category);
                    foreach (AlertHeadlineInfo headline in info.Headlines)
                    {
                        Uri link = new Uri(headline.Url);
                        AtomEntry item = new AtomEntry(new AtomId(link), new AtomTextConstruct(headline.Text), DateTime.Now);
                        item.Categories.Add(category);
                        AtomLink aLink = new AtomLink(link);
                        aLink.ContentType = "text/html";
                        aLink.Language = MapLanguageToCultureInfo(headline.ContentLanguage);
                        aLink.Relation = "Alternate";
                        item.Links.Add(aLink);
                        item.Summary = new AtomTextConstruct(headline.Snippet);
                        item.PublishedOn = headline.PublicationDateTime;
                        atomFeed.AddEntry(item);
                    }
                }
            }
            return Serialize(atomFeed);
        }

        /// <summary>
        /// Deserializes to JSON.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

        /// <summary>
        /// Deserializes to XML.
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            XmlSerializer serializer = new XmlSerializer(GetType());
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (NonXsiTextWriter xw = new NonXsiTextWriter(sw))
                {
                    xw.Formatting = Formatting.None;
                    xw.WriteRaw("");
                    serializer.Serialize(xw, this);
                    sw.Flush();
                }
            }
            return sb.Insert(0, "<?xml version=\"1.0\" encoding=\"utf-8\"?>").ToString();
        }

        /// <summary>
        /// Deserializes to JSONP.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string ToJSONP(string callback, params string[] args)
        {
            if (string.IsNullOrEmpty(callback))
            {
                return ToJSON();
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(callback);
            sb.Append("(");
            sb.Append(ToJSON());
            if (args != null && args.Length > 0)
            {
                sb.Append(",");
                string[] temp = Array.ConvertAll<string, string>(args, delegate(string s) { return string.Format("\"{0}\"", s.Replace("\"", "&quot;")); });
                sb.Append(string.Join(",", temp));
            }
            sb.Append(");");
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// Processes the alert infos.
        /// </summary>
        /// <param name="alertIds">The alert ids.</param>
        private void ProcessAlertInfos(List<int> alertIds)
        {
            List<AlertInfo> alerts = GetHeadlineWidgetDataForPreview(alertIds);
            if (alerts.Count <= 0) return;
            Definition.alertIds = alertIds.ToArray();
            Data = new AlertWidgetData();
            Data.Alerts = alerts.ToArray();
            Data.Count = alerts.Count;
            ValidateAllFolders();
        }

        private void UpdateUrls(WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget, string interfaceLanguage)
        {
            if (Data.Alerts == null || Data.Alerts.Length <= 0) return;

            for (int i = 0; i < Data.Alerts.Length; i++)
            {
                Data.Alerts[i].ViewAllUri = HeadlineUtility.GenerateCycloneAlertViewAllLink(Definition, Data.Alerts[i], tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage);
                foreach (AlertHeadlineInfo headline in Data.Alerts[i].Headlines)
                {
                    headline.Url = HeadlineUtility.GenerateCycloneAlertArticleLink(Definition, headline, Data.Alerts[i], tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage);
                }
            }
        }

        private void SerializeWidgetToCache(string widgetId, string interfaceLanguage)
        {
            // Serialize the widget to Factiva cache
            if (ReturnCode == 0)
            {
                m_WidgetManager.CacheWidget(widgetId, interfaceLanguage, Factiva.BusinessLayerLogic.Delegates.Utility.Serialize(this));
            }
        }

        private void ValidateAllFolders()
        {
            // Make sure you have a valid alert folders
            bool atLeastOneGoodAlertFolder = false;
            if (Data.Count > 0)
            {
                foreach (AlertInfo info in Data.Alerts)
                {
                    if (info.Status.Code != 0) continue;
                    atLeastOneGoodAlertFolder = true;
                    break;
                }
            }
            if (atLeastOneGoodAlertFolder) return;
            ReturnCode = FactivaWidgetsUIException.NO_VALID_FOLDERS;
            StatusMessage = m_ResourceText.GetErrorMessage(ReturnCode.ToString());
        }

        /// <summary>
        /// Fires the metrics envelope.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        private void FireMetricsEnvelope(string widgetId)
        {
            try
            {
                m_WidgetManager.FireUpdateRenderCountOnWidget(widgetId);
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="snippet">The snippet.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private static string GetDescription(string snippet, string url)
        {
            return string.Format("{0}<br/><a href=\"{1}\" type=\"audio/mpeg\">MP3</a>", snippet, url);
        }

        private static bool IsContentInAPodcastEnableLanguage(ContentLanguage language)
        {
            switch (language)
            {
                case ContentLanguage.en:
                case ContentLanguage.es:
                case ContentLanguage.de:
                case ContentLanguage.it:
                case ContentLanguage.fr:
                    return true;
                default:
                    return false;
            }
        }

        private static string GetPodcastMediaUrl(WidgetTokenProperties properties, AlertHeadlineInfo info)
        {
            AudioMediaUrlBuilder urlBuilder = new AudioMediaUrlBuilder();
            urlBuilder.AccessionNumber = info.AccessionNumber;
            urlBuilder.AccountId = properties.AccountId;
            urlBuilder.ContentCategory = info.ContentCategory.ToString();
            urlBuilder.ContentLanguage = info.ContentLanguage.ToString();
            urlBuilder.IncludeMarketingMessage = false;
            urlBuilder.NameSpace = properties.NameSpace;
            urlBuilder.UserId = properties.UserId;
            return urlBuilder.ToString();
        }

        private static CultureInfo MapLanguageToCultureInfo(ContentLanguage language)
        {
            switch (language)
            {
                default:
                    return CultureInfo.CreateSpecificCulture(language.ToString());
                case ContentLanguage.zhcn:
                    return CultureInfo.CreateSpecificCulture("zh-cn");
                case ContentLanguage.zhtw:
                    return CultureInfo.CreateSpecificCulture("zh-tw");
            }
        }


        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="useStylesheet">if set to <c>true</c> [use stylesheet].</param>
        /// <returns></returns>
        private static string Serialize(ISyndicationResource feed, bool useStylesheet)
        {
            //create an xmlwriter and then write nothing to this to fake and remove xml decl
            StringBuilder sb = new StringBuilder();
            using (StringWriterWithEncodingClass sw = new StringWriterWithEncodingClass(sb, Encoding.UTF8))
            {
                using (XmlTextWriter writer = new XmlTextWriter(sw))
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
        private static string Serialize(ISyndicationResource feed)
        {
            return Serialize(feed, false);
        }


        /// <summary>
        /// Gets the alert headline widget tokens.
        /// </summary>
        private void PopulateAlertHeadlineWidgetLiterals()
        {
            Literals = new AlertHeadlineWidgetLiterals();
            Literals.CopyRight = string.Format("&copy;&nbsp;{0}&nbsp;{1}", DateTime.Now.Year, m_ResourceText.GetString("copyRightPhrase"));
            Literals.NoResults = m_ResourceText.GetString("noResults");
            Literals.ViewLess = m_ResourceText.GetString("viewLess");
            Literals.ViewMore = m_ResourceText.GetString("viewMore");
            Literals.ViewAll = m_ResourceText.GetString("viewAll");
            // Link-out urls
            Literals.MarketingSiteUrl = m_Marketing_Site_Url;
            Literals.MarketingSiteTitle = m_Marketing_Site_Title;

            // Site absolute urls
            BaseUrlBuilder builder = new BaseUrlBuilder();
            builder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            builder.BaseUrl = "~/img/branding/djicon.gif";
            Literals.Icon = builder.ToString();

            builder = new BaseUrlBuilder();
            builder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            builder.BaseUrl = "~/img/branding/djFactiva.gif";
            Literals.BrandingBadge = builder.ToString();

            builder = new BaseUrlBuilder();
            builder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            builder.BaseUrl = "~/syndication/podcast/players/dewplayer.swf";
            Literals.DewplayerFlashUrl = builder.ToString();

            builder = new BaseUrlBuilder();
            builder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            builder.BaseUrl = "~/img/speaker.gif";
            Literals.SpeakerIconUrl = builder.ToString();
        }

        /// <summary>
        /// Gets the folders for preview.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dictExternalHasKeys">The dict external has keys.</param>
        /// <param name="proxyUserId">The proxy user id.</param>
        /// <param name="proxyNamespace">The proxy namespace.</param>
        /// <returns></returns>
        private List<Folder> GetFoldersForFill(List<int> ids, Dictionary<int, string> dictExternalHasKeys, string proxyUserId, string proxyNamespace)
        {
            List<Folder> buffer = new List<Folder>();
            if (ids != null && ids.Count > 0)
            {
                GetFoldersDelegate foldersDelegate = GetFolders;
                List<IAsyncResult> asyncResults = new List<IAsyncResult>();
                for (int index = 0; index < ids.Count; index = index + m_Chunck_Size)
                {
                    List<int> chunckedIds = index + m_Chunck_Size > ids.Count ? ids.GetRange(index, ids.Count - index) : ids.GetRange(index, 5);
                    if (chunckedIds.Count > 0)
                    {
                        asyncResults.Add(foldersDelegate.BeginInvoke(chunckedIds, dictExternalHasKeys, proxyUserId, proxyNamespace, null, null));
                    }
                }
                foreach (IAsyncResult asyncResult in asyncResults)
                {
                    if (!asyncResult.AsyncWaitHandle.WaitOne(m_Delegate_Timeout, true)) // Blocks util thread is completed/Timeouts.
                    {
                        m_Log.Debug("Delegate Timeout.");
                    }
                    try
                    {
                        List<Folder> temp = new List<Folder>(foldersDelegate.EndInvoke(asyncResult));
                        if (temp.Count > 0)
                        {
                            buffer.AddRange(temp);
                        }
                    }
                    catch (Exception ex)
                    {
                        m_Log.Error("unable to retrieve preview headlines", ex);
                    }
                }
            }
            return buffer;
        }

        /// <summary>
        /// Gets the headline widget data for preview. This is a asynchronous transaction that breaks the list into
        /// chuncks and retrieves it for speed purposes.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        private List<AlertInfo> GetHeadlineWidgetDataForPreview(List<int> ids)
        {
            List<AlertInfo> buffer = new List<AlertInfo>();
            if (ids != null && ids.Count > 0)
            {
                GetWidgetDataDelegate dataDelegate = PopulateHeadlineWidgetDataForPreview;
                List<IAsyncResult> asyncResults = new List<IAsyncResult>();
                for (int index = 0; index < ids.Count; index = index + m_Chunck_Size)
                {
                    List<int> chunckedIds = index + m_Chunck_Size > ids.Count ? ids.GetRange(index, ids.Count - index) : ids.GetRange(index, 5);
                    if (chunckedIds.Count > 0)
                    {
                        asyncResults.Add(dataDelegate.BeginInvoke(chunckedIds, null, null));
                    }
                }
                foreach (IAsyncResult asyncResult in asyncResults)
                {
                    if (!asyncResult.AsyncWaitHandle.WaitOne(m_Delegate_Timeout, true)) // Blocks util thread is completed/Timeouts.
                    {
                        m_Log.Debug("Delegate Timeout.");
                    }
                    try
                    {
                        List<AlertInfo> temp = new List<AlertInfo>(dataDelegate.EndInvoke(asyncResult));
                        if (temp.Count > 0)
                        {
                            buffer.AddRange(temp);
                        }
                    }
                    catch (Exception ex)
                    {
                        m_Log.Error("unable to retrieve preview headlines", ex);
                    }
                }
            }
            return buffer;
        }

        private List<Folder> GetFolders(List<int> ids, Dictionary<int, string> dictExternalHasKeys, string proxyUserId, string proxyNamespace)
        {
            List<Folder> buffer = new List<Folder>();
            if (ids.Count > 0)
            {
                List<string> exHashKeys = new List<string>();
                foreach (int folderId in ids)
                {
                    // find external hash keys
                    if (dictExternalHasKeys.ContainsKey(folderId))
                    {
                        exHashKeys.Add(dictExternalHasKeys[folderId]);
                    }
                }
                TrackManager trackManager = new TrackManager(ControlDataManager.GetLightWeightUserControlData("RSSFEED1", "RSSFEED1", "16"), m_SessionData.InterfaceLanguage);

                // Fire transaction to get the response
                GetAlertHeadlinesForRssResponse response = null;
                try
                {
                    response = trackManager.GetMultipleAlertRSSHeadlines(ids, 20, exHashKeys, proxyUserId, proxyNamespace, true);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    ReturnCode = fbe.ReturnCodeFromFactivaService;
                    StatusMessage = m_ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
                }
                catch (Exception ex)
                {
                    // Instantiate FactivaBusinessLogicException to write messages to log.
                    FactivaBusinessLogicException fbe = new FactivaBusinessLogicException(ex, -1);
                    ReturnCode = fbe.ReturnCodeFromFactivaService;
                    StatusMessage = m_ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
                }

                if (response != null &&
                    response.folderHeadlinesResponse != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.count > 0 &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.folder != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.folder.Length > 0)
                {
                    foreach (Folder folder in response.folderHeadlinesResponse.folderHeadlinesResult.folder)
                    {
                        buffer.Add(folder);
                    }
                }
            }
            return buffer;
        }

        private List<AlertInfo> PopulateHeadlineWidgetDataForPreview(List<int> ids)
        {
            List<AlertInfo> buffer = new List<AlertInfo>();
            // because we are in a delegate change thread culture to match.
            SetThreadCulture(m_SessionData.InterfaceLanguage);
            if (ids.Count > 0)
            {
                TrackManager trackManager = new TrackManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                Dictionary<int, AlertInfo> alertInfos = new Dictionary<int, AlertInfo>();

                ExtendedSearchDTO exSearchDTO = new ExtendedSearchDTO();
                exSearchDTO.alertIds = ids.ToArray();
                exSearchDTO.maxResultsToReturn = 20;
                exSearchDTO.contentCategory = ContentCategory.None;

                // The following data is not used
                exSearchDTO.returnTimelineNavigatorSet = false;
                exSearchDTO.returnNavigatorBuckets = false;
                exSearchDTO.returnKeyWordsSet = false;
                exSearchDTO.returnClusterSet = false;

                // Fire transaction to get the response
                GetMultipleFolderHeadlinesResponse response = null;
                try
                {
                    response = trackManager.GetAlertHeadlinesWithSearch20Response(exSearchDTO, true);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    ReturnCode = fbe.ReturnCodeFromFactivaService;
                    StatusMessage = m_ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
                }
                catch (Exception ex)
                {
                    // Instantiate FactivaBusinessLogicException to write messages to log.
                    FactivaBusinessLogicException fbe = new FactivaBusinessLogicException(ex, -1);
                    ReturnCode = fbe.ReturnCodeFromFactivaService;
                    StatusMessage = m_ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
                }

                if (response != null &&
                    response.folderHeadlinesResponse != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.count > 0 &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.folder != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.folder.Length > 0)
                {
                    HeadlineUtility headlineUtility = new HeadlineUtility(m_SessionData, m_ResourceText);
                    foreach (Folder folder in response.folderHeadlinesResponse.folderHeadlinesResult.folder)
                    {
                        AlertInfo alertInfo = new AlertInfo();
                        alertInfo.Status = new Status();
                        alertInfo.Id = folder.folderID;

                        ContentHeadlineStruct contentHeadlineStruct = new ContentHeadlineStruct();
                        contentHeadlineStruct.folderId = folder.folderID;
                        contentHeadlineStruct.folderName = folder.folderName;
                        contentHeadlineStruct.distributionType = Definition.DistributionType;
                        contentHeadlineStruct.accountId = m_SessionData.AccountId;
                        contentHeadlineStruct.accountNamespace = m_SessionData.ProductId;

                        if (folder.status == 0)
                        {
                            // Add the Name
                            alertInfo.Name = folder.folderName;
                            alertInfo.ExternalAccessToken = folder.folderSharing.sharingData.externalHashKey;

                            if (folder.PerformContentSearchResponse != null)
                            {
                                PerformContentSearchResponse searchResponse = new PerformContentSearchResponse().Deserialize((folder.PerformContentSearchResponse).Serialize());
                                if (searchResponse != null && searchResponse.contentSearchResult != null &&
                                    searchResponse.contentSearchResult.contentHeadlinesResultSet != null)
                                {
                                    if (searchResponse.contentSearchResult.hitCount > 0 &&
                                        searchResponse.contentSearchResult.contentHeadlinesResultSet.contentHeadline != null)
                                    {
                                        List<AlertHeadlineInfo> headlineInfos = new List<AlertHeadlineInfo>(10);
                                        foreach (ContentHeadline contentHeadline in searchResponse.contentSearchResult.contentHeadlinesResultSet.contentHeadline)
                                        {
                                            contentHeadlineStruct.contentHeadline = contentHeadline;
                                            headlineInfos.Add(headlineUtility.Convert(contentHeadlineStruct));
                                        }
                                        if (headlineInfos.Count > 0)
                                        {
                                            alertInfo.HeadlineCount = headlineInfos.Count;
                                            alertInfo.Headlines = headlineInfos.ToArray();
                                        }
                                    }
                                    else
                                    {
                                        alertInfo.HeadlineCount = 0;
                                        alertInfo.Headlines = new AlertHeadlineInfo[0];
                                    }
                                }
                                else
                                {
                                    alertInfo.Status = GetStatus(new FactivaWidgetsUIException(FactivaWidgetsUIException.UNABLE_TO_RETRIEVE_CONTENT_HEADLINES_RESULTSET));
                                }
                            }
                            if (folder.folderSharing.sharingData.externalAccess == ShareAccess.Deny)
                            {
                                alertInfo.Status = GetStatus(new FactivaWidgetsUIException(FactivaWidgetsUIException.SHARE_PROPERTIES_ON_FOLDER_SET_TO_DENY));
                            }
                        }
                        else
                        {
                            alertInfo.Status = GetStatus(new FactivaWidgetsUIException(folder.status));
                        }
                        alertInfos.Add(alertInfo.Id, alertInfo);
                    }
                }
                if (alertInfos.Count > 0)
                {
                    foreach (int i in ids)
                    {
                        if (alertInfos.ContainsKey(i))
                        {
                            buffer.Add(alertInfos[i]);
                        }
                        else
                        {
                            AlertInfo alertInfo = new AlertInfo();
                            alertInfo.Id = i;
                            alertInfo.Status = GetStatus(new FactivaWidgetsUIException(FactivaWidgetsUIException.INVALID_FOLDER_ID));
                            buffer.Add(alertInfo);
                        }
                    }
                }
            }
            return buffer;
        }

        /// <summary>
        /// Populates the headline widget data.
        /// </summary>
        /// <param name="alertWidget">The alert widget.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="proxyUserId">The proxy user id.</param>
        /// <param name="proxyNamespace">The proxy namespace.</param>
        private void PopulateHeadlineWidgetData(AlertWidget alertWidget, string accountId, string proxyUserId, string proxyNamespace)
        {
            if (Definition == null) return;
            Data = new AlertWidgetData();
            List<int> ids = new List<int>();
            Dictionary<int, string> exHashKeys = new Dictionary<int, string>();

            // Get a list of valid alertIds 
            if (alertWidget != null &&
                alertWidget.Component != null &&
                alertWidget.Component.AlertCollection != null &&
                alertWidget.Component.AlertCollection.Count > 0)
            {
                foreach (AlertItem item in alertWidget.Component.AlertCollection)
                {
                    int aID;
                    Int32.TryParse(item.ItemId, out aID);
                    if (ids.Contains(aID) || aID <= 0) continue;
                    ids.Add(aID);
                    exHashKeys.Add(aID, item.ExternalToken);
                }
            }

            // Check to see if folders are in cache
            Dictionary<int, Folder> folderDict = m_WidgetManager.GetAlertsWithErrorStatusesFromCache(ids, proxyUserId, proxyNamespace);

            // Update the list of ids
            List<int> requestIds = new List<int>(ids);
            Definition.alertIds = requestIds.ToArray();

            // remove keys you have data for.
            foreach (KeyValuePair<int, Folder> pair in folderDict)
            {
                if (requestIds.Contains(pair.Key))
                    requestIds.Remove(pair.Key);
            }

            if (requestIds.Count > 0)
            {
                List<Folder> buffer = GetFoldersForFill(requestIds, exHashKeys, proxyUserId, proxyNamespace);

                List<Folder> itemsToSendToCache = new List<Folder>();
                foreach (Folder folder in buffer)
                {
                    if (!folderDict.ContainsKey(folder.folderID))
                    {
                        folderDict.Add(folder.folderID, folder);
                    }
                    // Note: Add deleted folders to cache
                    if (folder.status == 0) continue;
                    if (!itemsToSendToCache.Contains(folder))
                    {
                        itemsToSendToCache.Add(folder);
                    }
                }
                if (itemsToSendToCache.Count > 0)
                {
                    // Note: update cache
                    m_WidgetManager.AddAlertsWithErrorStatusesToCache(itemsToSendToCache, proxyUserId, proxyNamespace);
                }
            }

            // Prepare to output the keys.
            Dictionary<int, AlertInfo> alertInfos = GetAlertInfos(ids, folderDict, accountId, proxyNamespace);
            List<AlertInfo> output = new List<AlertInfo>();
            if (alertInfos.Count > 0)
            {
                foreach (int i in ids)
                {
                    if (alertInfos.ContainsKey(i))
                    {
                        output.Add(alertInfos[i]);
                    }
                    else
                    {
                        AlertInfo alertInfo = new AlertInfo();
                        alertInfo.Id = i;
                        alertInfo.Status = GetStatus(new FactivaWidgetsUIException(FactivaWidgetsUIException.INVALID_FOLDER_ID));
                        output.Add(alertInfo);
                    }
                }
            }
            if (output.Count <= 0) return;
            Data = new AlertWidgetData();
            Data.Alerts = output.ToArray();
            Data.Count = output.Count;
        }

        /// <summary>
        /// Gets the alert infos.
        /// </summary>
        /// <param name="alertIds">The alert ids.</param>
        /// <param name="folderDictionary">The folder dictionary.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="proxyNamespace">The proxy namespace.</param>
        /// <returns></returns>
        private Dictionary<int, AlertInfo> GetAlertInfos(IEnumerable<int> alertIds, IDictionary<int, Folder> folderDictionary, string accountId, string proxyNamespace)
        {
            Dictionary<int, AlertInfo> alertInfos = new Dictionary<int, AlertInfo>();
            HeadlineUtility headlineUtility = new HeadlineUtility(m_SessionData, m_ResourceText);
            if (folderDictionary.Count > 0)
            {
                foreach (int i in alertIds)
                {
                    if (!folderDictionary.ContainsKey(i)) continue;

                    Folder folder = folderDictionary[i];
                    AlertInfo alertInfo = new AlertInfo();
                    alertInfo.Status = new Status();
                    alertInfo.Id = folder.folderID;

                    if (folder.status == 0)
                    {
                        // Add the Name
                        alertInfo.Name = folder.folderName;
                        alertInfo.Type = folder.productType;
                        alertInfo.ExternalAccessToken = folder.folderSharing.sharingData.externalHashKey;

                        ContentHeadlineStruct contentHeadlineStruct = new ContentHeadlineStruct();
                        contentHeadlineStruct.folderId = folder.folderID;
                        contentHeadlineStruct.folderName = folder.folderName;
                        contentHeadlineStruct.distributionType = Definition.DistributionType;
                        contentHeadlineStruct.accountId = accountId;
                        contentHeadlineStruct.accountNamespace = proxyNamespace;

                        if (folder.PerformContentSearchResponse != null)
                        {
                            PerformContentSearchResponse searchResponse = new PerformContentSearchResponse().Deserialize((folder.PerformContentSearchResponse).Serialize());
                            if (searchResponse != null && searchResponse.contentSearchResult != null &&
                                searchResponse.contentSearchResult.contentHeadlinesResultSet != null)
                            {
                                if (searchResponse.contentSearchResult.hitCount > 0 &&
                                    searchResponse.contentSearchResult.contentHeadlinesResultSet.contentHeadline != null)
                                {
                                    List<AlertHeadlineInfo> headlineInfos = new List<AlertHeadlineInfo>(10);
                                    foreach (ContentHeadline contentHeadline in searchResponse.contentSearchResult.contentHeadlinesResultSet.contentHeadline)
                                    {
                                        contentHeadlineStruct.contentHeadline = contentHeadline;
                                        headlineInfos.Add(headlineUtility.Convert(contentHeadlineStruct));
                                    }
                                    if (headlineInfos.Count > 0)
                                    {
                                        alertInfo.HeadlineCount = headlineInfos.Count;
                                        alertInfo.Headlines = headlineInfos.ToArray();
                                    }
                                }
                                else
                                {
                                    alertInfo.HeadlineCount = 0;
                                    alertInfo.Headlines = new AlertHeadlineInfo[0];
                                }
                            }
                            else
                            {
                                alertInfo.Status = GetStatus(new FactivaBusinessLogicException(FactivaWidgetsUIException.UNABLE_TO_RETRIEVE_CONTENT_HEADLINES_RESULTSET));
                            }
                        }
                    }
                    else
                    {
                        alertInfo.Status = GetStatus(new FactivaBusinessLogicException(folder.status));
                    }
                    alertInfos.Add(alertInfo.Id, alertInfo);
                }
            }
            return alertInfos;
        }

        /// <param name="alertSnippetType">Type of the alert snippet.</param>
        /// <returns></returns>
        private static WidgetHeadlineDisplayType MapWidgetHeadlineDisplayType(AlertSnippetType alertSnippetType)
        {
            switch (alertSnippetType)
            {
                case AlertSnippetType.Fixed:
                    return WidgetHeadlineDisplayType.HeadlinesWithSnippets;
                default:
                    return WidgetHeadlineDisplayType.HeadlinesOnly;
            }
        }

        /// <summary>
        /// Maps the size of the widget font.
        /// </summary>
        /// <param name="fontSize">Size of the font.</param>
        /// <returns></returns>
        private static WidgetFontSize MapWidgetFontSize(FontSize fontSize)
        {
            switch (fontSize)
            {
                case FontSize.ExtraExtraSmall:
                    return WidgetFontSize.XX_Small;
                case FontSize.ExtraSmall:
                    return WidgetFontSize.X_Small;
                case FontSize.Small:
                    return WidgetFontSize.Small;
                case FontSize.Medium:
                    return WidgetFontSize.Medium;
                case FontSize.Large:
                    return WidgetFontSize.Large;
                case FontSize.ExtraLarge:
                    return WidgetFontSize.X_Large;
                case FontSize.ExtraExtraLarge:
                    return WidgetFontSize.XX_Large;
                default:
                    return WidgetFontSize.Medium;
            }
        }

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="fbe">The fbe.</param>
        /// <returns></returns>
        private Status GetStatus(FactivaBusinessLogicException fbe)
        {
            Status status = new Status();
            status.Code = fbe.ReturnCodeFromFactivaService;
            switch (status.Code)
            {
                case FactivaBusinessLogicException.SHARING_VIOLATION_FOLDER_MARKED_PRIVATE:
                    status.Message = m_ResourceText.GetString("folderIsMakedAsPrivate");
                    break;
                default:
                    status.Message = m_ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
                    break;
            }

            return status;
        }

        private void PopulateAlertHeadlineWidgetDefinition(AlertWidget widget, string overridenInterfaceLanguage)
        {
            
            if (widget == null) return;

            Definition = new AlertHeadlineWidgetDefinition();
            Definition.Id = widget.Id;
            Definition.Name = widget.Properties.Name;
            Definition.Description = widget.Properties.Description;
            Definition.Language = widget.Properties.Language;
            MergeWidgetDefinition(widget, Definition);

            if (!string.IsNullOrEmpty(overridenInterfaceLanguage))
            {
                Definition.Language = overridenInterfaceLanguage;
            }

        }

        /// <summary>
        /// Merges the widget definition.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="widgetDefinition">The widget definition.</param>
        private static void MergeWidgetDefinition(AlertWidget widget, HeadlineWidgetDefinition widgetDefinition)
        {
            if (widget == null) return;

            // Update MainColor
            if (widget.Properties.Color != null && !string.IsNullOrEmpty(widget.Properties.Color.Value))
            {
                widgetDefinition.MainColor = widget.Properties.Color.Value;
            }

            // Update MainFontColor, FontFamily, FontSize
            if (widget.Properties.Font != null)
            {
                if (widget.Properties.Font.Color != null &&
                    !string.IsNullOrEmpty(widget.Properties.Font.Color.Value))
                {
                    widgetDefinition.MainFontColor = widget.Properties.Font.Color.Value;
                }

                // Update FontSize
                widgetDefinition.FontSize = MapWidgetFontSize(widget.Properties.Font.Size);

                // Update FontName or FontFamily
                if (!string.IsNullOrEmpty(widget.Properties.Font.Name))
                {
                    widgetDefinition.FontFamily = (WidgetFontFamily)Enum.Parse(typeof(WidgetFontFamily), widget.Properties.Font.Name);
                }
            }

            // Update the view
            widgetDefinition.ViewType = widget.Properties.ViewType;

            // Update Distribution values
            widgetDefinition.DistributionType = WidgetDistributionType.OnlyUsersInMyAccount;
            widgetDefinition.AuthenticationCredentials = new AuthenticationCredentials();
            if (widget.Properties.Audience != null)
            {
                switch(widget.Properties.Audience.AudienceOptions)
                {
                    case AudienceOptions.InternalAccount:
                        widgetDefinition.DistributionType = WidgetDistributionType.OnlyUsersInMyAccount;
                        break;
                    case AudienceOptions.OutsideAccount:
                        widgetDefinition.DistributionType = WidgetDistributionType.UsersOutsideMyAccount;
                        break;
                    case AudienceOptions.TimeToLive_Proxy:
                        widgetDefinition.DistributionType = WidgetDistributionType.TTLProxyAccount;
                        break;
                    case AudienceOptions.ExternalReader:
                        widgetDefinition.DistributionType = WidgetDistributionType.ExternalReader;
                        break;
                }
                // update the profileId;
                widgetDefinition.AuthenticationCredentials.ProfileId = widget.Properties.Audience.ProfileId; 
                
                // update the authentication credentials
                if (widget.Properties.Audience.ProxyCredentials != null)
                {
                    switch (widget.Properties.Audience.ProxyCredentials.AuthenticationScheme)
                    {
                        case AuthenticationScheme.Email:
                            widgetDefinition.AuthenticationCredentials.AuthenticationScheme = WidgetAuthenticationScheme.EmailAddress;
                            break;
                        default:
                            widgetDefinition.AuthenticationCredentials.AuthenticationScheme = WidgetAuthenticationScheme.UserId;
                            break;
                    }
                    widgetDefinition.AuthenticationCredentials.EncryptedToken = widget.Properties.Audience.ProxyCredentials.EncrytedToken;
                    widgetDefinition.AuthenticationCredentials.ProxyUserId = widget.Properties.Audience.ProxyCredentials.UserId;
                    widgetDefinition.AuthenticationCredentials.ProxyEmailAddress = widget.Properties.Audience.ProxyCredentials.EmailAddress;
                    widgetDefinition.AuthenticationCredentials.ProxyPassword = widget.Properties.Audience.ProxyCredentials.Password;
                    widgetDefinition.AuthenticationCredentials.ProxyNamespace = widget.Properties.Audience.ProxyCredentials.NameSpace;
                }
            }

            // Update AccentColor, AccentFontColor and HeadlineDisplayType
            if (widget.Component == null || widget.Component.Properties == null) return;

            AlertComponentProperties properties = widget.Component.Properties;

            // Update HeadlineDisplayType
            widgetDefinition.DisplayType = MapWidgetHeadlineDisplayType(properties.SnippetType);

            // Update AccentColor
            if (properties.AccentColor != null && !string.IsNullOrEmpty(properties.AccentColor.Value))
            {
                widgetDefinition.AccentColor = properties.AccentColor.Value;
            }

            // update AccentFontColor
            if (properties.AlertFont != null && properties.AlertFont.Color != null &&
                !string.IsNullOrEmpty(properties.AlertFont.Color.Value))
            {
                widgetDefinition.AccentFontColor = properties.AlertFont.Color.Value;
            }

            // update numOfHeadlines to show
            widgetDefinition.NumOfHeadlines = properties.MaxResultsToReturn;
        }

        #region Nested type: GetFoldersDelegate

        private delegate List<Folder> GetFoldersDelegate(List<int> ids, Dictionary<int, string> dictExternalHasKeys, string proxyUserId, string proxyNamespace);

        #endregion

        #region Nested type: GetWidgetDataDelegate

        private delegate List<AlertInfo> GetWidgetDataDelegate(List<int> ids);

        #endregion
    }
}