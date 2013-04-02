// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertHeadlineWidgetDelegate.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AlertHeadlineWidgetDelegate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Argotic.Extensions.Core;
using Argotic.Syndication;
using EMG.Tools.Charting;
using EMG.Tools.Charting.Data;
using EMG.Tools.Charting.Discovery;
using EMG.Tools.Managers.Charting;
using EMG.Utility.Exceptions;
using EMG.Utility.Formatters;
using EMG.Utility.Formatters.Numerical;
using EMG.Utility.Handlers.Syndication.Podcast.Core;
using EMG.Utility.OperationalData.AssetActivity;
using EMG.Utility.Url;
using EMG.widgets.Managers;
using EMG.widgets.ui.delegates.core;
using EMG.widgets.ui.delegates.core.alertHeadline;
using EMG.widgets.ui.delegates.core.discovery;
using EMG.widgets.ui.delegates.interfaces;
using EMG.widgets.ui.delegates.output.literals;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.Properties;
using EMG.widgets.ui.utility.discovery;
using EMG.widgets.ui.utility.headline;
using Factiva.BusinessLayerLogic;
using Factiva.BusinessLayerLogic.DataTransferObject;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.BusinessLayerLogic.Utility;
using Factiva.Gateway.Messages.Assets.V1_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using factiva.nextgen;
using log4net;
using BaseUrlBuilder = EMG.Utility.Uri.UrlBuilder;
using Encryption = FactivaEncryption.encryption;
using Navigator = Factiva.Gateway.Messages.Search.V2_0.Navigator;
using Status = EMG.widgets.ui.delegates.core.Status;
using EMG.Utility.Managers.Track;

namespace EMG.widgets.ui.delegates.output.syndication
{
    /// <summary>
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class AlertHeadlineWidgetDelegate : AbstractWidgetDelegate, IWidgetPodcastDelegate, IWidgetPreviewDelegate
    {
        private const int m_Base_Eclosure_Size = 300000;
        private const int m_Chunck_Size = 5;
        private const int m_Delegate_Timeout = 20*1000;
        private const int m_TimeToLive_Podcast_RSS = 60;
        private static readonly ILog m_Log = LogManager.GetLogger(typeof (AlertHeadlineWidgetDelegate));
        private SessionData m_SessionData;
        private WidgetTokenProperties m_TokenProperties;
        private string m_Token;
        private TrackDeletedFoldersCacheManager m_DeletedFoldersManager;
        private readonly object syncObject = new object();

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


        /// <summary>
        /// Initializes a new instance of the <see cref="AlertHeadlineWidgetDelegate"/> class.
        /// </summary>
        public AlertHeadlineWidgetDelegate()
        {
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
                            EscapedUri podcastUri = new EscapedUri(GetPodcastMediaUrl(properties, headline));

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
        /// <param name="assetIds">The alert ids.</param>
        public void FillPreview(List<int> assetIds)
        {
            using (TransactionLogger logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                Definition = new AlertHeadlineWidgetDefinition();
                PopulateLiterals();

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
                    new EmgWidgetsUIException("Unable to get the GetLatestUpdatedAlertWidget", ex);
                    ReturnCode = -1;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    return;
                }

                try
                {
                    if (alertWidget != null)
                    {
                        MergeWidgetDefinition(alertWidget, Definition);
                    }

                    // dacostad[10.27.2008] -- removed functionality due to Knit Picking 
                    // Definition.Name = m_ResourceText.GetString("defaultWidgetName");
                    Definition.Name = string.Empty;

                    if (assetIds != null && assetIds.Count > 0)
                    {
                        ProcessAlertInfos(assetIds);
                        // added http context to determine if ssl retrieval is neccessary
                        //This is not required after switching to HTML chart
                        //GenerateCordaChartUrls(true, true);
                        UpdateIcons();
                    }

                    Validate();
                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
                catch (FactivaBusinessLogicException fex)
                {
                    Data = null;
                    Definition = null;
                    ReturnCode = fex.ReturnCodeFromFactivaService;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
                catch (EmgUtilitiesException emgex)
                {
                    Data = null;
                    Definition = null;
                    ReturnCode = emgex.ReturnCode;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
                catch (Exception ex)
                {
                    // Log the exception 
                    new EmgWidgetsUIException("Unable to render widget.", ex);
                    Data = null;
                    Definition = null;
                    ReturnCode = -1;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
            }
        }

        /// <summary>
        /// Fills the preview.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        public void FillPreview(string widgetId)
        {
            using (TransactionLogger logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                
                try
                {
                    Widget widget = m_WidgetManager.GetCachedWidgetById(widgetId);
                    if (widget != null)
                    {
                        AlertWidget alertWidget = widget as AlertWidget;
                        if (alertWidget != null)
                        {
                            PopulateLiterals();

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

                                // [10.08.2010] Vishal - Modify to check deleted folders from track in IIS, then go to platform cache.
                                m_DeletedFoldersManager = new TrackDeletedFoldersCacheManager(m_SessionData.SessionId,
                                                                              m_SessionData.ClientTypeCode,
                                                                              m_SessionData.AccessPointCode,
                                                                              m_SessionData.InterfaceLanguage);

                                var deletedFolders = m_DeletedFoldersManager.GetDeletedFolders();
                                if (deletedFolders != null && deletedFolders.Count > 0)
                                {

                                    foreach (var deletedFolderId in deletedFolders.Values)
                                    {
                                        int temp;
                                        if (Int32.TryParse(deletedFolderId.ToString().Trim(), out temp) 
                                            && alertIds.Contains(temp))
                                        {
                                            lock (syncObject)
                                            {
                                                alertIds.Remove(temp);
                                                if (m_Log.IsDebugEnabled) m_Log.Debug(string.Format("Removed Alert ID {0} to avoid call to Track.", temp));
                                            }
                                        }
                                    }
                                }

                                if (alertIds.Count > 0)
                                {
                                    ProcessAlertInfos(alertIds);
                                    //This is not required after switching to HTML chart
                                    //GenerateCordaChartUrls(true, true);
                                    UpdateIcons();
                                }
                                
                            }
                            Validate();
                        }

                        ElapsedTime = logger.LogTimeSinceInvocation();
                    }
                }
                catch (FactivaBusinessLogicException fex)
                {
                    Data = null;
                    Definition = null;
                    ReturnCode = fex.ReturnCodeFromFactivaService;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
                catch (Exception ex)
                {
                    // Log the exception 
                    new EmgWidgetsUIException("Unable to render widget.", ex);
                    Data = null;
                    Definition = null;
                    ReturnCode = -1;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    ElapsedTime = logger.LogTimeSinceInvocation();
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
        public override void Fill(string token, IntegrationTarget integrationTarget)
        {
            using (var logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_Token = token;
                m_TokenProperties = new WidgetTokenProperties(token);
                var userControlData = ControlDataManager.AddProxyCredentialsToControlData(ControlDataManagerEx.GetRssFeed1LightWeightUser(), m_TokenProperties.UserId, m_TokenProperties.NameSpace);
                m_WidgetManager = new WidgetManager(userControlData, SessionData.Instance().InterfaceLanguage);

                try
                {
                    // Get the widget Definition
                    var widget = m_WidgetManager.GetCachedWidgetById(m_TokenProperties.WidgetId);
                    if (widget != null)
                    {
                        if (widget is AlertWidget)
                        {
                            var alertWidget = (AlertWidget) widget;
                            
                            // Pull out the interfacelanguage
                            var interfaceLanguage = alertWidget.Properties.Language;

                            // Check cache to see if widget exists
                            // See if widget is in Factia cache
                            string cachedData;
                            if (m_WidgetManager.IsWidgetInFactivaCache(m_TokenProperties.WidgetId, interfaceLanguage, out cachedData))
                            {
                                var cachedDelegate = (AlertHeadlineWidgetDelegate) Factiva.BusinessLayerLogic.Delegates.Utility.Deserialize(cachedData, GetType());
                                
                                // Make sure a valid item was saved in cache
                                if (cachedDelegate != null)
                                {
                                    // ignore return code.
                                    Data = cachedDelegate.Data;
                                    Definition = cachedDelegate.Definition;
                                    ReturnCode = cachedDelegate.ReturnCode;
                                    StatusMessage = cachedDelegate.StatusMessage;
                                    //Literals = cachedDelegate.Literals;
                                    PopulateLiterals();

                                    
                                    // Update Urls based ResponseFormat()
                                    UpdateUrls(m_TokenProperties, integrationTarget, interfaceLanguage);
                                    UpdateDiscoveryChartUrls(m_TokenProperties, integrationTarget, interfaceLanguage);
                                    //This is not required after switching to HTML chart
                                    //GenerateCordaChartUrls(false, true);
                                    FireMetricsEnvelope(GetOperationData(integrationTarget));
                                    NullOutAuthenticationCredentials();
                                    ElapsedTime = logger.LogTimeSinceInvocation();
                                    return;
                                }
                            }
                            
                            // Set the ui culture on the thread.
                            m_SessionData = new SessionData("b", interfaceLanguage, 0, false);
                            PopulateLiterals();

                            // Populate with AlertHeadlineWidgetdDelegate
                            PopulateAlertHeadlineWidgetDefinition(alertWidget, null);
                            PopulateHeadlineWidgetData(alertWidget, m_TokenProperties.AccountId, m_TokenProperties.UserId, m_TokenProperties.NameSpace);
                            Validate();
                            SerializeWidgetToCache(m_TokenProperties.WidgetId, interfaceLanguage);

                            // Update Urls based ResponseFormat()
                            UpdateUrls(m_TokenProperties, integrationTarget, interfaceLanguage);
                            UpdateDiscoveryChartUrls(m_TokenProperties, integrationTarget, interfaceLanguage);

                            //This is not required after switching to HTML chart
                            //GenerateCordaChartUrls(false, true);

                            FireMetricsEnvelope(GetOperationData(integrationTarget));
                            NullOutAuthenticationCredentials();
                            ElapsedTime = logger.LogTimeSinceInvocation();
                            return;
                        }
                    }
                    else
                    {
                        throw new EmgWidgetsUIException(FactivaBusinessLogicException.INVALID_WIDGET);
                    }

                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
                catch (FactivaBusinessLogicException fex)
                {
                    Data = null;
                    Definition = null;
                    ReturnCode = fex.ReturnCodeFromFactivaService;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
                catch (Exception ex)
                {
                    // Log the exception 
                    new EmgWidgetsUIException("Unable to render widget.", ex);
                    Data = null;
                    Definition = null;
                    ReturnCode = -1;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    ElapsedTime = logger.LogTimeSinceInvocation();
                }
            }
        }

        /// <summary>
        /// Gets the operation data.
        /// </summary>
        /// <returns>The operation data.</returns>
        private WidgetViewOperationalData GetOperationData(IntegrationTarget integrationTarget)
        {
            var operationalData = new WidgetViewOperationalData {AssetId = m_TokenProperties.WidgetId, PublishingDomain = HeadlineUtility.GetHttpReferer(integrationTarget)};

            return operationalData;
        }

        /// <summary>
        /// Deserializes to RSS.
        /// </summary>
        /// <returns></returns>
        public override string ToRSS()
        {
            if (Definition == null)
            {
                return string.Empty;
            }

            var rssFeed = new RssFeed
                              {
                                  Channel = new RssChannel
                                                {
                                                    Title = Definition.Name, 
                                                    Copyright = Literals.CopyRight, 
                                                    PublicationDate = DateTime.Now, 
                                                    TimeToLive = TIME_TO_LIVE_RSS
                                                }
                              };

            if (Data != null && Data.Count > 0)
            {
                foreach (var info in Data.Alerts)
                {
                    var category = new RssCategory(info.Name);
                    foreach (var headline in info.Headlines)
                    {
                        var item = new RssItem
                                       {
                                           Title = headline.Text, 
                                           Description = headline.Snippet, 
                                           Link = new EscapedUri(headline.Url)
                                       }; // new RssItem(headline.Text, headline.Snippet, new Uri(headline.Url));

                        item.Categories.Add(category);
                        item.PublicationDate = headline.PublicationDateTime;
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
        public override string ToATOM()
        {
            if (Definition == null)
            {
                return string.Empty;
            }

            var atomFeed = new AtomFeed
                               {
                                   Title = new AtomTextConstruct(Definition.Name), 
                                   UpdatedOn = DateTime.Now, 
                                   Rights = new AtomTextConstruct(Literals.CopyRight)
                               };

            atomFeed.UpdatedOn = DateTime.Now;

            if (Data != null && Data.Count > 0)
            {
                foreach (AlertInfo info in Data.Alerts)
                {
                    AtomCategory category = new AtomCategory(info.Name);
                    atomFeed.Categories.Add(category);
                    foreach (AlertHeadlineInfo headline in info.Headlines)
                    {
                        var link = new EscapedUri(headline.Url);
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
               
        #endregion

        /// <summary>
        /// Processes the alert infos.
        /// </summary>
        /// <param name="alertIds">The alert ids.</param>
        private void ProcessAlertInfos(List<int> alertIds)
        {
            var alerts = GetHeadlineWidgetDataForPreview(alertIds);
            if (alerts.Count <= 0)
            {
                return;
            }

            Definition.alertIds = alertIds.ToArray();
            Data = new AlertWidgetData
                       {
                           Alerts = alerts.ToArray(), 
                           Count = alerts.Count
                       };
        }

        /// <summary>
        /// Updates the urls.
        /// </summary>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        private void UpdateUrls(WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget, string interfaceLanguage)
        {
            if (Data.Alerts == null || Data.Alerts.Length <= 0)
            {
                return;
            }

            foreach (var alertInfo in Data.Alerts)
            {
                alertInfo.ViewAllUri = HeadlineUtility.GenerateCycloneAlertViewAllLink(Definition, alertInfo, tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage);
                if (alertInfo.Headlines == null || alertInfo.Headlines.Length <= 0)
                {
                    continue;
                }

                foreach (var headline in alertInfo.Headlines)
                {
                    headline.IconUrl = HeadlineUtility.GetIcon(headline);
                    headline.Url = HeadlineUtility.GenerateCycloneAlertArticleLink(Definition, headline, alertInfo, tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage);
                }
            }
        }

        /// <summary>
        /// Updates the discovery chart urls.
        /// </summary>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        private void UpdateDiscoveryChartUrls(WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget, string interfaceLanguage)
        {
            if (Data.Alerts == null || Data.Alerts.Length <= 0)
            {
                return;
            }

            for (var i = 0; i < Data.Alerts.Length; i++)
            {
                var getAlertUrl = DiscoveryUtility.GenerateCycloneDiscoveryGetAlertLink(Definition, Data.Alerts[i], tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage);
                if (Data.Alerts[i].CompaniesChart != null)
                {
                    Data.Alerts[i].CompaniesChart.GetThisAlertUrl = getAlertUrl;

                    if (Data.Alerts[i].CompaniesChart.Chart != null)
                    {
                        // NOTE: fsr - use "fsd" for companies rather than "co"
                        for (int j = 0; j < Data.Alerts[i].CompaniesChart.Chart.data.Count; j++)
                        {
                            Data.Alerts[i].CompaniesChart.Chart.data[j].uri =
                                DiscoveryUtility.GenerateCycloneDiscoveryChartItemLink(Definition, Data.Alerts[i],
                                                                                       tokenProperties,
                                                                                       Definition.DistributionType,
                                                                                       integrationTarget,
                                                                                       interfaceLanguage,
                                                                                       "fds|" +
                                                                                       Data.Alerts[i].CompaniesChart.
                                                                                           Chart.data[j].code, "cod");
                        }
                    }
                }

                if (Data.Alerts[i].ExecutivesChart != null)
                {
                    Data.Alerts[i].ExecutivesChart.GetThisAlertUrl = getAlertUrl;

                    if (Data.Alerts[i].ExecutivesChart.Chart != null)
                    {
                        for (var j = 0; j < Data.Alerts[i].ExecutivesChart.Chart.data.Count; j++)
                        {
                            Data.Alerts[i].ExecutivesChart.Chart.data[j].uri =
                                DiscoveryUtility.GenerateCycloneDiscoveryChartItemLink(Definition, Data.Alerts[i],
                                                                                       tokenProperties,
                                                                                       Definition.DistributionType,
                                                                                       integrationTarget,
                                                                                       interfaceLanguage,
                                                                                       "pe|" +
                                                                                       Data.Alerts[i].ExecutivesChart.
                                                                                           Chart.data[j].code, "exd");
                        }
                    }
                }

                if (Data.Alerts[i].IndustriesChart != null)
                {
                    Data.Alerts[i].IndustriesChart.GetThisAlertUrl = getAlertUrl;

                    if (Data.Alerts[i].IndustriesChart.Chart != null)
                    {
                        for (var j = 0; j < Data.Alerts[i].IndustriesChart.Chart.data.Count; j++)
                        {
                            Data.Alerts[i].IndustriesChart.Chart.data[j].uri =
                                DiscoveryUtility.GenerateCycloneDiscoveryChartItemLink(Definition, Data.Alerts[i],
                                                                                       tokenProperties,
                                                                                       Definition.DistributionType,
                                                                                       integrationTarget,
                                                                                       interfaceLanguage,
                                                                                       "in|" +
                                                                                       Data.Alerts[i].IndustriesChart.
                                                                                           Chart.data[j].code, "ind");
                        }
                    }
                }

                if (Data.Alerts[i].RegionsChart != null)
                {
                    Data.Alerts[i].RegionsChart.GetThisAlertUrl = getAlertUrl;
                    if (Data.Alerts[i].RegionsChart.Chart != null)
                    {
                        for (var j = 0; j < Data.Alerts[i].RegionsChart.Chart.data.Count; j++)
                        {
                            Data.Alerts[i].RegionsChart.Chart.data[j].uri =
                                DiscoveryUtility.GenerateCycloneDiscoveryChartItemLink(Definition, Data.Alerts[i],
                                                                                       tokenProperties,
                                                                                       Definition.DistributionType,
                                                                                       integrationTarget,
                                                                                       interfaceLanguage,
                                                                                       "re|" +
                                                                                       Data.Alerts[i].RegionsChart.Chart
                                                                                           .data[j].code, "red");
                        }
                    }
                }

                if (Data.Alerts[i].SubjectsChart == null)
                {
                    continue;
                }

                Data.Alerts[i].SubjectsChart.GetThisAlertUrl = getAlertUrl;

                if (Data.Alerts[i].SubjectsChart.Chart == null){continue;}

                for (var j = 0; j < Data.Alerts[i].SubjectsChart.Chart.data.Count; j++)
                {
                    Data.Alerts[i].SubjectsChart.Chart.data[j].uri = DiscoveryUtility.GenerateCycloneDiscoveryChartItemLink(Definition, Data.Alerts[i], tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage, "ns|" + Data.Alerts[i].SubjectsChart.Chart.data[j].code, "nsd");
                }
            }
        }

        /// <summary>
        /// Generates the discovery chart urls.
        /// </summary>
        private void GenerateCordaChartUrls(bool isSample, bool useCache)
        {
            if (Data.Alerts == null || Data.Alerts.Length <= 0) return;

            for (int i = 0; i < Data.Alerts.Length; i++)
            {
                DiscoveryChartGenerator companiesGenerator = new DiscoveryChartGenerator();
                companiesGenerator.OutputChartType = OutputChartType.FLASH;
                companiesGenerator.IsSampleChart = isSample;
                companiesGenerator.UseCache = useCache;
                companiesGenerator.BarColor = ColorTranslator.FromHtml(Definition.ChartBarColor);
                companiesGenerator.ChartType = DiscoveryChartType.Companies;

                DiscoveryChartGenerator executivesGenerator = new DiscoveryChartGenerator();
                executivesGenerator.OutputChartType = OutputChartType.FLASH;
                executivesGenerator.IsSampleChart = isSample;
                executivesGenerator.UseCache = useCache;
                executivesGenerator.BarColor = ColorTranslator.FromHtml(Definition.ChartBarColor);
                executivesGenerator.ChartType = DiscoveryChartType.Executives;

                DiscoveryChartGenerator industriesGenerator = new DiscoveryChartGenerator();
                industriesGenerator.OutputChartType = OutputChartType.FLASH;
                industriesGenerator.IsSampleChart = isSample;
                industriesGenerator.UseCache = useCache;
                industriesGenerator.BarColor = ColorTranslator.FromHtml(Definition.ChartBarColor);
                industriesGenerator.ChartType = DiscoveryChartType.Industries;

                DiscoveryChartGenerator regionsGenerator = new DiscoveryChartGenerator();
                regionsGenerator.OutputChartType = OutputChartType.FLASH;
                regionsGenerator.IsSampleChart = isSample;
                regionsGenerator.UseCache = useCache;
                regionsGenerator.BarColor = ColorTranslator.FromHtml(Definition.ChartBarColor);
                regionsGenerator.ChartType = DiscoveryChartType.Regions;

                DiscoveryChartGenerator subjectsGenerator = new DiscoveryChartGenerator();
                subjectsGenerator.OutputChartType = OutputChartType.FLASH;
                subjectsGenerator.IsSampleChart = isSample;
                subjectsGenerator.UseCache = useCache;
                subjectsGenerator.BarColor = ColorTranslator.FromHtml(Definition.ChartBarColor);
                subjectsGenerator.ChartType = DiscoveryChartType.NewsSubjects;

                if (Data.Alerts[i].CompaniesChart != null && Data.Alerts[i].CompaniesChart.Chart != null)
                {
                    for (int j = 0; j < Data.Alerts[i].CompaniesChart.Chart.data.Count; j++)
                    {
                        if (Data.Alerts[i].CompaniesChart.Chart.data[j].uri != null)
                        {
                            // set the data for the current item in the chart
                            DataItem dataItem = new DataItem();
                            dataItem.Drilldown = string.Format(
                                "javascript:void FactivaWidgetRenderManager.getInstance().xWinOpen('{0}');"
                                , Data.Alerts[i].CompaniesChart.Chart.data[j].uri);
                            dataItem.Hover = Data.Alerts[i].CompaniesChart.Chart.data[j].name;
                            dataItem.Value = Convert.ToDouble(Data.Alerts[i].CompaniesChart.Chart.data[j].value);

                            companiesGenerator.DataSet.Items.Add(dataItem);
                        }
                    }

                    IUriResponse companiesChartUri = companiesGenerator.GetUri();
                    Data.Alerts[i].CompaniesChart.Chart.chartUri = CheckContext(companiesChartUri.Uri);
                    Data.Alerts[i].CompaniesChart.Chart.height = companiesChartUri.Height;
                    Data.Alerts[i].CompaniesChart.Chart.width = companiesChartUri.Width;

                }

                if (Data.Alerts[i].ExecutivesChart != null && Data.Alerts[i].ExecutivesChart.Chart != null)
                {
                    for (int j = 0; j < Data.Alerts[i].ExecutivesChart.Chart.data.Count; j++)
                    {
                        if (Data.Alerts[i].ExecutivesChart.Chart.data[j].uri != null && Data.Alerts[i].ExecutivesChart.Chart.data[j].name != null)
                        {
                            // set the data for the current item in the chart
                            DataItem dataItem = new DataItem();
                            dataItem.Drilldown = string.Format(
                                "javascript:void FactivaWidgetRenderManager.getInstance().xWinOpen('{0}');"
                                , Data.Alerts[i].ExecutivesChart.Chart.data[j].uri);
                            dataItem.Hover = Data.Alerts[i].ExecutivesChart.Chart.data[j].name;
                            dataItem.Value = Convert.ToDouble(Data.Alerts[i].ExecutivesChart.Chart.data[j].value);

                            executivesGenerator.DataSet.Items.Add(dataItem);
                        }
                    }

                    IUriResponse executivesChartUri = executivesGenerator.GetUri();
                    Data.Alerts[i].ExecutivesChart.Chart.chartUri = CheckContext(executivesChartUri.Uri);
                    Data.Alerts[i].ExecutivesChart.Chart.height = executivesChartUri.Height;
                    Data.Alerts[i].ExecutivesChart.Chart.width = executivesChartUri.Width;

                }

                if (Data.Alerts[i].IndustriesChart != null && Data.Alerts[i].IndustriesChart.Chart != null)
                {
                    for (int j = 0; j < Data.Alerts[i].IndustriesChart.Chart.data.Count; j++)
                    {
                        if (Data.Alerts[i].IndustriesChart.Chart.data[j].uri!=null)
                        {
                            // set the data for the current item in the chart
                            DataItem dataItem = new DataItem();
                            dataItem.Drilldown =
                                string.Format(
                                    "javascript:void FactivaWidgetRenderManager.getInstance().xWinOpen('{0}');",
                                    Data.Alerts[i].IndustriesChart.Chart.data[j].uri);
                            dataItem.Hover = Data.Alerts[i].IndustriesChart.Chart.data[j].name;
                            dataItem.Value = Convert.ToDouble(Data.Alerts[i].IndustriesChart.Chart.data[j].value);

                            industriesGenerator.DataSet.Items.Add(dataItem);
                        }
                    }

                    IUriResponse industriesChartUri = industriesGenerator.GetUri();
                    Data.Alerts[i].IndustriesChart.Chart.chartUri = CheckContext(industriesChartUri.Uri);
                    Data.Alerts[i].IndustriesChart.Chart.height = industriesChartUri.Height;
                    Data.Alerts[i].IndustriesChart.Chart.width = industriesChartUri.Width;

                }

                if (Data.Alerts[i].RegionsChart != null && Data.Alerts[i].RegionsChart.Chart != null)
                {
                    for (int j = 0; j < Data.Alerts[i].RegionsChart.Chart.data.Count; j++)
                    {
                        if (Data.Alerts[i].RegionsChart.Chart.data[j].uri != null && Data.Alerts[i].RegionsChart.Chart.data[j].name != null)
                        {
                            // set the data for the current item in the chart
                            DataItem dataItem = new DataItem();
                            dataItem.Drilldown = string.Format(
                                "javascript:void FactivaWidgetRenderManager.getInstance().xWinOpen('{0}');"
                                , Data.Alerts[i].RegionsChart.Chart.data[j].uri);
                            dataItem.Hover = Data.Alerts[i].RegionsChart.Chart.data[j].name;
                            dataItem.Value = Convert.ToDouble(Data.Alerts[i].RegionsChart.Chart.data[j].value);

                            regionsGenerator.DataSet.Items.Add(dataItem);
                        }
                    }

                    IUriResponse regionsChartUri = regionsGenerator.GetUri();
                    Data.Alerts[i].RegionsChart.Chart.chartUri = CheckContext(regionsChartUri.Uri);
                    Data.Alerts[i].RegionsChart.Chart.height = regionsChartUri.Height;
                    Data.Alerts[i].RegionsChart.Chart.width = regionsChartUri.Width;

                }

                if (Data.Alerts[i].SubjectsChart != null && Data.Alerts[i].SubjectsChart.Chart != null)
                {
                    for (int j = 0; j < Data.Alerts[i].SubjectsChart.Chart.data.Count; j++)
                    {
                        if (Data.Alerts[i].SubjectsChart.Chart.data[j].uri != null)
                        {
                            // set the data for the current item in the chart
                            DataItem dataItem = new DataItem();
                            dataItem.Drilldown =
                                string.Format(
                                    "javascript:void FactivaWidgetRenderManager.getInstance().xWinOpen('{0}');",
                                    Data.Alerts[i].SubjectsChart.Chart.data[j].uri);
                            dataItem.Hover = Data.Alerts[i].SubjectsChart.Chart.data[j].name;
                            dataItem.Value = Convert.ToDouble(Data.Alerts[i].SubjectsChart.Chart.data[j].value);

                            subjectsGenerator.DataSet.Items.Add(dataItem);
                        }
                    }

                    IUriResponse subjectsChartUri = subjectsGenerator.GetUri();
                    Data.Alerts[i].SubjectsChart.Chart.chartUri = CheckContext(subjectsChartUri.Uri);
                    Data.Alerts[i].SubjectsChart.Chart.height = subjectsChartUri.Height;
                    Data.Alerts[i].SubjectsChart.Chart.width = subjectsChartUri.Width;

                }
            }
        }


        /// <summary>
        /// Gets the rounded hit count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private static string GetRoundedHitCount(int count)
        {
            if (count < 10000)
            {
                return new NumberFormatter().Format(count, NumberFormatType.Whole);
            }
            if (count <= 99999)
            {
                return String.Format("{0:##.0K}", Math.Round((double)count / 1000, 1));
            }
            if (count <= 999999)
            {
                return String.Format("{0:###K}", Math.Round((double)count / 1000));
            }
            return String.Format("{0}M", new NumberFormatter().Format(Math.Round((double)count / 1000000, 1), NumberFormatType.Precision));
        }

        /// <summary>
        /// Retrieves the chart over SSL if neccessary.
        /// </summary>
        /// <param name="chartUri">The chart URI.</param>
        /// <returns>chartUri with http or https prefix</returns>
        private string CheckContext(string chartUri)
        {
            bool isSecureContext = (HttpContext.Current.Request.Url.Scheme == "https");

            if (isSecureContext)
            {
                return chartUri.Insert(4, "s");
            }

            return chartUri;
        }
        
        /// <summary>
        /// Updates the icons.
        /// </summary>
        private void UpdateIcons()
        {
            if (Data == null || Data.Alerts == null || Data.Alerts.Length <= 0) return;

            for (int i = 0; i < Data.Alerts.Length; i++)
            {
                if (Data.Alerts[i].Headlines == null || Data.Alerts[i].HeadlineCount == 0)
                    break;
                foreach (AlertHeadlineInfo headline in Data.Alerts[i].Headlines)
                {
                    headline.IconUrl = HeadlineUtility.GetIcon(headline);
                }
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        protected override void Validate()
        {
            // Make sure you have a valid alert folders
            bool atLeastOneGoodAlertFolder = false;
            if (Data != null && Data.Count > 0)
            {
                foreach (AlertInfo info in Data.Alerts)
                {
                    if (info.Status.Code != 0) continue;
                    atLeastOneGoodAlertFolder = true;
                    break;
                }
            }
            if (atLeastOneGoodAlertFolder) return;

            ReturnCode = EmgWidgetsUIException.NO_VALID_FOLDERS;
            StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
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

        /// <summary>
        /// Determines whether [is content in A podcast enable language] [the specified language].
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>
        /// 	<c>true</c> if [is content in A podcast enable language] [the specified language]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Gets the podcast media URL.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="info">The info.</param>
        /// <returns></returns>
        private static string GetPodcastMediaUrl(WidgetTokenProperties properties, HeadlineInfo info)
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
        
        /// <summary>
        /// Populates the literals.
        /// </summary>
        public void PopulateLiterals()
        {
            Literals = new AlertHeadlineWidgetLiterals();
            Literals.CopyRight = string.Format("&copy;&nbsp;{0}&nbsp;{1}", DateTime.Now.Year, ResourceText.GetString("copyRightPhrase"));
            Literals.NoResults = ResourceText.GetString("noResults");
            Literals.ViewLess = ResourceText.GetString("viewLess");
            Literals.ViewMore = ResourceText.GetString("viewMore");
            Literals.ViewAll = ResourceText.GetString("viewAll");
            Literals.Next = ResourceText.GetString("next");
            Literals.Previous = ResourceText.GetString("previous");
            Literals.GetThisAlert = ResourceText.GetString("getAlert");
            Literals.Headlines = ResourceText.GetString("headlines");
            Literals.Companies = ResourceText.GetString("companies");
            Literals.Industries = ResourceText.GetString("industries");
            Literals.Subjects = ResourceText.GetString("subjects");
            Literals.Executives = ResourceText.GetString("executives");
            Literals.Regions = ResourceText.GetString("regions");

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
            builder.BaseUrl = "~/img/tabs/carouselLeft.png";
            Literals.CarouselLeft = builder.ToString();

            builder = new BaseUrlBuilder();
            builder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            builder.BaseUrl = "~/img/tabs/carouselRight.png";
            Literals.CarouselRight = builder.ToString();

            builder = new BaseUrlBuilder();
            builder.OutputType = BaseUrlBuilder.UrlOutputType.Absolute;
            builder.BaseUrl = "~/img/plotdot.png";
            Literals.DiscoveryChartImage = builder.ToString();
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
            FactivaBusinessLogicException fex = null;
            Exception bex = null;
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
                    catch (FactivaBusinessLogicException fbex)
                    {
                        fex = fbex;
                        m_Log.Error("unable to retrieve preview headlines", fbex);
                    }
                    catch (Exception ex)
                    {
                        bex = ex;
                        m_Log.Error("unable to retrieve preview headlines", ex);
                    }
                }
                if (asyncResults.Count == 1)
                {
                    if (fex != null)
                    {
                        throw fex;
                    }
                    if (bex != null)
                    {
                        throw bex;
                    }
                }
            }
         
            return buffer;
        }

        /// <summary>
        /// Gets the group folders.
        /// </summary>
        /// <returns></returns>
        private List<long> GetGroupFolders()
        {
            List<long> groupFolders = new List<long>();

            GetItemsByClassIDRequest request = new GetItemsByClassIDRequest();
            request.ClassID = new PreferenceClassID[1];
            request.ClassID[0] = PreferenceClassID.GroupFolder;
            
            try
            {
                PreferenceResponse preferenceResponse = PreferenceService.GetItemsByClassID(ControlDataManager.Clone(m_SessionData.SessionBasedControlDataEx), request);
                if (preferenceResponse.rc == 0)
                {
                    foreach (GroupFolderPreferenceItem item in preferenceResponse.GroupFolder)
                    {
                        if (!item.ItemValue.IsNullOrEmpty())
                        {
                            groupFolders.Add(Convert.ToInt64(item.ItemValue));
                        }
                    }
                }    
            }
            catch (Exception ex)
            {
                // Instantiate FactivaBusinessLogicException to write messages to log.
                FactivaBusinessLogicException fbe = new FactivaBusinessLogicException(ex, -1);
                ReturnCode = fbe.ReturnCodeFromFactivaService;
                StatusMessage = ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
            }

            return groupFolders;
        }

        /// <summary>
        /// Gets the group folders.
        /// </summary>
        /// <returns></returns>
        private List<int> GetGroupFoldersForFill()
        {
            var userControlData = ControlDataManager.AddProxyCredentialsToControlData(ControlDataManagerEx.GetRssFeed1LightWeightUser(), m_TokenProperties.UserId, m_TokenProperties.NameSpace);
            var groupFolders = new List<int>();
            var request = new GetItemsByClassIDNoCacheRequest
                                                   {
                                                       ClassID = new PreferenceClassID[1]
                                                   };

            request.ClassID[0] = PreferenceClassID.GroupFolder;

            try
            {
                var preferenceResponse = PreferenceService.GetItemsByClassIDNoCache(userControlData, request);
                if (preferenceResponse.rc == 0)
                {
                    foreach (GroupFolderPreferenceItem item in preferenceResponse.GroupFolder)
                    {
                        if (item.ItemValue != null)
                        {
                            groupFolders.Add(Convert.ToInt32(item.ItemValue));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Instantiate FactivaBusinessLogicException to write messages to log.
                var fbe = new FactivaBusinessLogicException(ex, -1);
                ReturnCode = fbe.ReturnCodeFromFactivaService;
                StatusMessage = ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
                throw fbe;
            }

            return groupFolders;
        }

        /// <summary>
        /// Gets the folders.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="dictExternalHasKeys">The dict external has keys.</param>
        /// <param name="proxyUserId">The proxy user id.</param>
        /// <param name="proxyNamespace">The proxy namespace.</param>
        /// <returns></returns>
        private List<Folder> GetFolders(List<int> ids, Dictionary<int, string> dictExternalHasKeys, string proxyUserId, string proxyNamespace)
        {
            var buffer = new List<Folder>();
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
                var trackManager = new TrackManager(ControlDataManager.GetLightWeightUserControlData("RSSFEED1", "RSSFEED1", "16"), m_SessionData.InterfaceLanguage);

                // Fire transaction to get the response
                GetAlertHeadlinesForRss2Response response = null;
                try
                {
                    response = trackManager.GetMultipleAlertRSSHeadlines(ids, 20, exHashKeys, proxyUserId, proxyNamespace, true);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    ReturnCode = fbe.ReturnCodeFromFactivaService;
                    StatusMessage = ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
                }
                catch (Exception ex)
                {
                    // Instantiate FactivaBusinessLogicException to write messages to log.
                    var fbe = new FactivaBusinessLogicException(ex, -1);
                    ReturnCode = fbe.ReturnCodeFromFactivaService;
                    StatusMessage = ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
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

        /// <summary>
        /// Processes the navigator.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <returns></returns>
        private static DiscoveryChartInfo PopulateDiscoveryChartData(Navigator navigator)
        {
            if (navigator == null || navigator.Count == 0) return null;
            
            DiscoveryChartInfo discoveryChartInfo = new DiscoveryChartInfo();
            discoveryChartInfo.chartType = OutputChartType.FLASH;
            discoveryChartInfo.version = Settings.Default.ChartingFlashImage_TargetVersion;

            foreach (Bucket bucket in navigator.BucketCollection)
            {
                // Add the Name, Value, Empty Cyclone Url, and Code to the ChartDataItem
                discoveryChartInfo.data.Add(new ChartDataItem(bucket.Value, GetRoundedHitCount(bucket.HitCount), string.Empty, bucket.Id));
            }
            
            return discoveryChartInfo;
        }

        /// <summary>
        /// Populates the headline widget data for preview.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        private List<AlertInfo> PopulateHeadlineWidgetDataForPreview(List<int> ids)
        {
            var buffer = new List<AlertInfo>();
            // because we are in a delegate change thread culture to match.
            SetThreadCulture(m_SessionData.InterfaceLanguage);
            if (ids.Count > 0)
            {
                var trackManager = new TrackManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                var alertInfos = new Dictionary<int, AlertInfo>();

                var exSearchDTO = new ExtendedSearchDTO
                                      {
                                          alertIds = ids.ToArray(),
                                          maxResultsToReturn = 20,
                                          contentCategory = ContentCategory.None,
                                          // The following data is not used
                                          returnTimelineNavigatorSet = false,
                                          returnNavigatorBuckets = false,
                                          returnKeyWordsSet = false,
                                          returnClusterSet = false
                                      };

                // Fire transaction to get group folders
                var groupFolders = GetGroupFolders();

                // Fire transaction to get the response
                var response = trackManager.GetAlertHeadlinesWithSearch20Response(exSearchDTO, true);

                if (response != null &&
                    response.folderHeadlinesResponse != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.count > 0 &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.folder != null &&
                    response.folderHeadlinesResponse.folderHeadlinesResult.folder.Length > 0)
                {
                    var headlineUtility = new HeadlineUtility(m_SessionData, ResourceText);
                    foreach (var folder in response.folderHeadlinesResponse.folderHeadlinesResult.folder)
                    {
                        var alertInfo = new AlertInfo
                                            {
                                                Status = new Status(),
                                                Id = folder.folderID,
                                                IsGroupFolder = false
                                            };

                        if (folder.folderSharing != null && folder.folderSharing.sharingData != null)
                        {
                            if (folder.folderSharing.sharingData.assignedScope != ShareScope.Personal ||
                                groupFolders.Contains(alertInfo.Id))
                            {
                                alertInfo.IsGroupFolder = true;
                            }
                            else
                            {
                                alertInfo.IsGroupFolder = false;
                            }
                        }

                        var contentHeadlineStruct = new ContentHeadlineStruct
                                                        {
                                                            distributionType = Definition.DistributionType,
                                                            accountId = m_SessionData.AccountId,
                                                            accountNamespace = m_SessionData.ProductId
                                                        };

                        if (folder.status == 0)
                        {
                            // Add the Name
                            alertInfo.Name = folder.folderName;
                            alertInfo.ExternalAccessToken = folder.folderSharing.sharingData.externalHashKey;

                            if (folder.PerformContentSearchResponse != null)
                            {
                                var searchResponse = new PerformContentSearchResponse().Deserialize((folder.PerformContentSearchResponse).Serialize());
                                if (searchResponse != null && searchResponse.ContentSearchResult!= null &&
                                    searchResponse.ContentSearchResult.ContentHeadlineResultSet!= null)
                                {
                                    if (searchResponse.ContentSearchResult.HitCount > 0 &&
                                        searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection != null)
                                    {
                                        var headlineInfos = new List<AlertHeadlineInfo>(10);
                                        var duplicateIndices = GetDuplicateIndices(searchResponse);
                                        for (var index = 0; index < searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection.Count; index++)
                                        {
                                            if (duplicateIndices.Contains(index))
                                                continue;
                                            contentHeadlineStruct.contentHeadline = searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection[index];
                                            headlineInfos.Add(headlineUtility.ConvertToAlertHeadlineInfo(contentHeadlineStruct));
                                        }
                                        if (headlineInfos.Count > 0)
                                        {
                                            alertInfo.HeadlineCount = headlineInfos.Count;
                                            alertInfo.Headlines = headlineInfos.ToArray();
                                        }
                                        if (searchResponse.ContentSearchResult.CodeNavigatorSet != null &&
                                            searchResponse.ContentSearchResult.CodeNavigatorSet.Count > 0)
                                        {
                                            // Populate Discovery Data
                                            foreach (var navigator in searchResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection)
                                            {
                                                switch (navigator.Id.ToLower())
                                                {
                                                    case "in":
                                                        alertInfo.IndustriesChart = new DiscoveryInfo
                                                        {
                                                            Chart = PopulateDiscoveryChartData(navigator),
                                                            GetThisAlertUrl = string.Empty
                                                        };
                                                        break;
                                                    case "co":
                                                        alertInfo.CompaniesChart = new DiscoveryInfo
                                                        {
                                                            Chart = PopulateDiscoveryChartData(navigator),
                                                            GetThisAlertUrl = string.Empty
                                                        };
                                                        break;
                                                    case "pe":
                                                        alertInfo.ExecutivesChart = new DiscoveryInfo
                                                        {
                                                            Chart = PopulateDiscoveryChartData(navigator),
                                                            GetThisAlertUrl = string.Empty
                                                        };
                                                        break;
                                                    case "ns":
                                                        alertInfo.SubjectsChart = new DiscoveryInfo
                                                        {
                                                            Chart = PopulateDiscoveryChartData(navigator),
                                                            GetThisAlertUrl = string.Empty
                                                        };
                                                        break;
                                                    case "re":
                                                        alertInfo.RegionsChart = new DiscoveryInfo
                                                        {
                                                            Chart = PopulateDiscoveryChartData(navigator),
                                                            GetThisAlertUrl = string.Empty
                                                        };
                                                        break;
                                                }
                                            }
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
                                    alertInfo.Status = GetStatus(new EmgWidgetsUIException(EmgWidgetsUIException.UNABLE_TO_RETRIEVE_CONTENT_HEADLINES_RESULTSET));
                                }
                            }
                            if (folder.folderSharing.sharingData.externalAccess == ShareAccess.Deny)
                            {
                                alertInfo.Status = GetStatus(new EmgWidgetsUIException(EmgWidgetsUIException.SHARE_PROPERTIES_ON_FOLDER_SET_TO_DENY));
                            }
                        }
                        else
                        {
                            alertInfo.Status = GetStatus(new EmgWidgetsUIException(folder.status));
                        }
                        alertInfos.Add(alertInfo.Id, alertInfo);
                    }
                }
                if (alertInfos.Count > 0)
                {
                    foreach (var i in ids)
                    {
                        if (alertInfos.ContainsKey(i))
                        {
                            buffer.Add(alertInfos[i]);
                        }
                        else
                        {
                            buffer.Add(new AlertInfo
                            {
                                Id = i,
                                Status = GetStatus(new EmgWidgetsUIException(EmgWidgetsUIException.INVALID_FOLDER_ID))
                            });
                        }
                    }
                }
            }
            return buffer;
        }


        /// <summary>
        /// Populates the headline widget data.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="proxyUserId">The proxy user id.</param>
        /// <param name="proxyNamespace">The proxy namespace.</param>
        protected override void PopulateHeadlineWidgetData(Widget widget, string accountId, string proxyUserId, string proxyNamespace)
        {
            var alertWidget = widget as AlertWidget;

            if (Definition == null)
            {
                return;
            }

            Data = new AlertWidgetData();
            var ids = new List<int>();
            var exHashKeys = new Dictionary<int, string>();

            // Get a list of valid alertIds 
            if (alertWidget != null &&
                alertWidget.Component != null &&
                alertWidget.Component.AlertCollection != null &&
                alertWidget.Component.AlertCollection.Count > 0)
            {
                foreach (var item in alertWidget.Component.AlertCollection)
                {
                    int aID;
                    Int32.TryParse(item.ItemId, out aID);
                    if (ids.Contains(aID) || aID <= 0)
                    {
                        continue;
                    }

                    ids.Add(aID);
                    exHashKeys.Add(aID, item.ExternalToken);
                }
            }

            // Update the list of ids
            var requestIds = new List<int>(ids);
            Definition.alertIds = requestIds.ToArray();

            // [10.08.2010] Vishal - Modify to check deleted folders from track in IIS
            // , then go to platform cache if not found in IIS cache.
            m_DeletedFoldersManager = new TrackDeletedFoldersCacheManager(m_SessionData.SessionId,
                                                                              m_SessionData.ClientTypeCode,
                                                                              m_SessionData.AccessPointCode,
                                                                              m_SessionData.InterfaceLanguage);
            // [10.08.10] Vishal - Get folders from IIS cache.
            var deletedFolders = m_DeletedFoldersManager.GetDeletedFolders();
            // [10.08.10] Vishal - Need to add folders deleted from track to platform cache regardless
            // Check to see if folders are in platform cache
            var folderDict = m_WidgetManager.GetAlertsWithErrorStatusesFromCache(ids, proxyUserId, proxyNamespace);


            if (deletedFolders != null && deletedFolders.Count > 0)
            {
                // remove values found in IIS Cache
                foreach (var deletedFolderId in deletedFolders.Values)
                {
                    int temp;
                    if (Int32.TryParse(deletedFolderId.ToString().Trim(), out temp)
                        && requestIds.Contains(temp))
                    {
                        lock (syncObject)
                        {
                            requestIds.Remove(temp);
                            if (m_Log.IsDebugEnabled) m_Log.Debug(string.Format("Removed Alert ID {0} to avoid call to Track.", temp));
                        }
                    }
                }
            }
            else
            {
                // remove keys you have data for.
                foreach (var pair in folderDict)
                {
                    if (requestIds.Contains(pair.Key))
                    {
                        requestIds.Remove(pair.Key);
                    }
                }
            }

            if (requestIds.Count > 0)
            {
                var buffer = GetFoldersForFill(requestIds, exHashKeys, proxyUserId, proxyNamespace);

                var itemsToSendToCache = new List<Folder>();
                foreach (var folder in buffer)
                {
                    if (!folderDict.ContainsKey(folder.folderID))
                    {
                        folderDict.Add(folder.folderID, folder);
                    }

                    // Note: Add deleted folders to cache
                    if (folder.status == 0)
                    {
                        continue;
                    }

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
            var alertInfos = GetAlertInfos(ids, folderDict, accountId, proxyNamespace);
            var output = new List<AlertInfo>();
            if (alertInfos.Count > 0)
            {
                foreach (var i in ids)
                {
                    if (alertInfos.ContainsKey(i))
                    {
                        output.Add(alertInfos[i]);
                    }
                    else
                    {
                        var alertInfo = new AlertInfo
                                            {
                                                Id = i, 
                                                Status = GetStatus(new EmgWidgetsUIException(EmgWidgetsUIException.INVALID_FOLDER_ID))
                                            };
                        output.Add(alertInfo);
                    }
                }
            }

            if (output.Count <= 0)
            {
                return;
            }

            Data = new AlertWidgetData
                       {
                           Alerts = output.ToArray(), 
                           Count = output.Count
                       };
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
            HeadlineUtility headlineUtility = new HeadlineUtility(m_SessionData, ResourceText);

            // Fire transaction to get group folders
            List<int> groupFolders = GetGroupFoldersForFill();
            
            if (folderDictionary.Count > 0)
            {
                foreach (int i in alertIds)
                {
                    if (!folderDictionary.ContainsKey(i))
                    {
                        continue;
                    }

                    var folder = folderDictionary[i];
                    var alertInfo = new AlertInfo();
                    alertInfo.Status = new Status();
                    alertInfo.Id = folder.folderID;
                    alertInfo.IsGroupFolder = false;

                    if (folder.folderSharing != null && folder.folderSharing.sharingData != null) 
                    {  
                        if (folder.folderSharing.sharingData.assignedScope != ShareScope.Personal || groupFolders.Contains(alertInfo.Id))
                        {
                            alertInfo.IsGroupFolder = true;
                        }
                        else
                        {
                            alertInfo.IsGroupFolder = false;
                        }
                    }

                    if (folder.status == 0)
                    {
                        // Add the Name
                        alertInfo.Name = folder.folderName;
                        alertInfo.Type = folder.productType;
                        if (folder.folderSharing != null && folder.folderSharing.sharingData != null)
                        {
                            alertInfo.ExternalAccessToken = folder.folderSharing.sharingData.externalHashKey;
                        }

                        var contentHeadlineStruct = new ContentHeadlineStruct
                                                        {
                                                            distributionType = Definition.DistributionType,
                                                            accountId = accountId,
                                                            accountNamespace = proxyNamespace
                                                        };

                        if (folder.PerformContentSearchResponse != null)
                        {
                            var searchResponse = new PerformContentSearchResponse().Deserialize(folder.PerformContentSearchResponse.Serialize());
                            if (searchResponse != null && searchResponse.ContentSearchResult != null &&
                                searchResponse.ContentSearchResult.ContentHeadlineResultSet != null)
                            {
                                // NN - Get indices of duplcates
                                if (searchResponse.ContentSearchResult.HitCount > 0 &&
                                    searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection != null)
                                {
                                    var headlineInfos = new List<AlertHeadlineInfo>(10);
                                    var duplicateIndices = GetDuplicateIndices(searchResponse);
                                    for (var index = 0; index < searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection.Count; index++)
                                    {
                                        if (duplicateIndices.Contains(index))
                                            continue;
                                        contentHeadlineStruct.contentHeadline = searchResponse.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection[index];
                                        headlineInfos.Add(headlineUtility.ConvertToAlertHeadlineInfo(contentHeadlineStruct));
                                    }

                                    if (headlineInfos.Count > 0)
                                    {
                                        alertInfo.HeadlineCount = headlineInfos.Count;
                                        alertInfo.Headlines = headlineInfos.ToArray();
                                    }

                                    if (searchResponse.ContentSearchResult.CodeNavigatorSet != null &&
                                        searchResponse.ContentSearchResult.CodeNavigatorSet.Count > 0)
                                    {
                                        // Populate Discovery Data
                                        foreach (var navigator in searchResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection)
                                        {
                                            switch (navigator.Id.ToLower())
                                            {
                                                case "in":
                                                    alertInfo.IndustriesChart = new DiscoveryInfo
                                                                                    {
                                                                                        Chart = PopulateDiscoveryChartData(navigator), 
                                                                                        GetThisAlertUrl = string.Empty
                                                                                    };
                                                    break;
                                                case "co":
                                                    alertInfo.CompaniesChart = new DiscoveryInfo
                                                                                   {
                                                                                       Chart = PopulateDiscoveryChartData(navigator), 
                                                                                       GetThisAlertUrl = string.Empty
                                                                                   };
                                                    break;
                                                case "pe":
                                                    alertInfo.ExecutivesChart = new DiscoveryInfo
                                                                                    {
                                                                                        Chart = PopulateDiscoveryChartData(navigator), 
                                                                                        GetThisAlertUrl = string.Empty
                                                                                    };
                                                    break;
                                                case "ns":
                                                    alertInfo.SubjectsChart = new DiscoveryInfo
                                                                                  {
                                                                                      Chart = PopulateDiscoveryChartData(navigator), 
                                                                                      GetThisAlertUrl = string.Empty
                                                                                  };
                                                    break;
                                                case "re":
                                                    alertInfo.RegionsChart = new DiscoveryInfo
                                                    {
                                                        Chart = PopulateDiscoveryChartData(navigator),
                                                        GetThisAlertUrl = string.Empty
                                                    };
                                                    break;
                                            }
                                        }
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
                                alertInfo.Status = GetStatus(new FactivaBusinessLogicException(EmgWidgetsUIException.UNABLE_TO_RETRIEVE_CONTENT_HEADLINES_RESULTSET));
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

        private List<int> GetDuplicateIndices(PerformContentSearchResponse searchResponse)
        {
            var duplicateIndices = new List<int>();
            if (searchResponse.ContentSearchResult.DeduplicatedHeadlineSet != null &&
                searchResponse.ContentSearchResult.DeduplicatedHeadlineSet.HeadlineRefCollection != null)
            {
                foreach (var headlineRef in searchResponse.ContentSearchResult.DeduplicatedHeadlineSet.HeadlineRefCollection)
                {
                    if (headlineRef.Duplicates != null && headlineRef.Duplicates.DuplicateRefCollection != null)
                    {
                        foreach (var duplicateRef in headlineRef.Duplicates.DuplicateRefCollection)
                            duplicateIndices.Add(duplicateRef.Index);
                    }
                }
            }
            return duplicateIndices;
        }

        /// <summary>
        /// Populates the alert headline widget definition.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="overridenInterfaceLanguage">The overriden interface language.</param>
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
        private static void MergeWidgetDefinition(AlertWidget widget, AlertHeadlineWidgetDefinition widgetDefinition)
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
            //widgetDefinition.ViewType = widget.Properties.ViewType;

            // Update the audience Options
            UpdateWidgetDefinitionAudience(widget.Properties.Audience, widgetDefinition);

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
            if (properties.AccentFont != null && properties.AccentFont.Color != null &&
                !string.IsNullOrEmpty(properties.AccentFont.Color.Value))
            {
                widgetDefinition.AccentFontColor = properties.AccentFont.Color.Value;
            }

            // Update ChartBarColor,ChartTemplate
            if (properties.GraphBarColor != null && !string.IsNullOrEmpty(properties.GraphBarColor.Value))
            {
                widgetDefinition.ChartBarColor = properties.GraphBarColor.Value;
            }

            // Update the Widget Template
            widgetDefinition.WidgetTemplate = MapWidgetTemplate(properties.GraphTemplateType);

            if (properties.Discovery.TabCollection != null && properties.Discovery.TabCollection.Count > 0)
            {
                widgetDefinition.DiscoveryTabs = MapDiscoveryTabs(properties.Discovery.TabCollection, widgetDefinition.DiscoveryTabs);
            }
            else if (properties.Discovery.TabCollection == null)
            {
                widgetDefinition.DiscoveryTabs = GetDefaultDiscoveryTabs();
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

        /// <summary>
        /// Nulls the out authentication credentials.
        /// </summary>
        protected override void NullOutAuthenticationCredentials()
        {
            NullOutAuthenticationCredential(Definition);
        }
    }
}