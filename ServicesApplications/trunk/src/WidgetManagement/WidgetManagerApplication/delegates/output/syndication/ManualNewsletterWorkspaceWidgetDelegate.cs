using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Argotic.Syndication;
using EMG.Utility.Exceptions;
using EMG.Utility.Managers.Search;
using EMG.Utility.Managers.Search.Requests;
using EMG.Utility.Managers.Search.Responses;
using EMG.Utility.OperationalData.AssetActivity;
using EMG.Utility.Url;
using EMG.widgets.Managers;
using EMG.widgets.ui.delegates.core;
using EMG.widgets.ui.delegates.core.manualNewsletterWorkspace;
using EMG.widgets.ui.delegates.interfaces;
using EMG.widgets.ui.delegates.output.literals;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.utility.headline;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.BusinessLayerLogic.Utility;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using factiva.nextgen;
using log4net;
using Encryption = FactivaEncryption.encryption;
using BaseUrlBuilder = EMG.Utility.Uri.UrlBuilder;
using SearchServiceV2_0 = Factiva.Gateway.Services.V2_0;
using SearchManager = EMG.Utility.Managers.Search.SearchManager;
using SortBy=EMG.Utility.Managers.Search.Requests.SortBy;
using WorkspaceItem=Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0.WorkspaceItem;

namespace EMG.widgets.ui.delegates.output.syndication
{
    /// <summary>
    /// 
    /// </summary>
    public class ManualNewsletterWorkspaceWidgetDelegate : AbstractWidgetDelegate, IWidgetPreviewDelegate
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof (ManualNewsletterWorkspaceWidgetDelegate));

        /// <summary>
        /// Data container of the headline Widget Delegate.
        /// </summary>
        public ManualNewsletterWorkspaceWidgetData Data;

        /// <summary>
        /// Definition container of the headline Widget Delegate.
        /// </summary>
        public ManualNewsletterWorkspaceWidgetDefinition Definition;

        /// <summary>
        /// Baseline CopyRight used for tokenization of phrases.
        /// </summary>
        public ManualNewsletterWorkspaceWidgetLiterals Literals;

        private SearchManager m_SearchManager;
        private SessionData m_SessionData;
        private string m_Token;
        private WidgetTokenProperties m_TokenProperties;
        private WorkspaceManager m_WorkspaceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticWorkspaceWidgetDelegate"/> class.
        /// </summary>
        public ManualNewsletterWorkspaceWidgetDelegate()
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

        #region IWidgetPreviewDelegate Members

        /// <summary>
        /// Fills the preview for a specified list of assets
        /// </summary>
        /// <param name="assetIds">The asset ids.</param>
        public void FillPreview(List<int> assetIds)
        {
            using (var logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                m_WorkspaceManager = new WorkspaceManager(SessionData.SessionBasedControlDataEx, SessionData.InterfaceLanguage);
                m_SearchManager = new SearchManager(SessionData.SessionBasedControlDataEx, SessionData.InterfaceLanguage);
                Definition = new ManualNewsletterWorkspaceWidgetDefinition();
                PopulateLiterals();

                ManualWorkspaceWidget manualWorkspaceWidget = null;
                try
                {
                    manualWorkspaceWidget = m_WidgetManager.GetLatestUpdatedManualNewsletterWorkspaceWidget();
                }
                catch (FactivaBusinessLogicException)
                {
                    // Do not show error keep defaults
                }
                catch (Exception ex)
                {
                    new EmgWidgetsUIException("Unable to get the GetLatestUpdatedManualNewsletterWorkspaceWidget", ex);
                    ReturnCode = -1;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    return;
                }

                try
                {
                    if (manualWorkspaceWidget != null)
                    {
                        MergeWidgetDefinition(manualWorkspaceWidget, Definition);
                    }

                    if (assetIds != null && assetIds.Count > 0)
                    {
                        ProcessAssetInfo(assetIds[0], true);
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
        /// Fills the preview for a specified widgetId.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        public void FillPreview(string widgetId)
        {
            using (var logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                m_WorkspaceManager = new WorkspaceManager(SessionData.SessionBasedControlDataEx, SessionData.InterfaceLanguage);
                m_SearchManager = new SearchManager(SessionData.SessionBasedControlDataEx, SessionData.InterfaceLanguage);

                try
                {
                    var widget = m_WidgetManager.GetCachedWidgetById(widgetId);
                    if (widget != null)
                    {
                        var manualWorkspaceWidget = widget as ManualWorkspaceWidget;
                        if (manualWorkspaceWidget != null)
                        {
                            PopulateLiterals();

                            // Populate with ManualWorkspaceWidget
                            PopulateDefinition(manualWorkspaceWidget, m_SessionData.InterfaceLanguage);

                            // Get a list of ids out of the definition
                            if (manualWorkspaceWidget.Component != null &&
                                manualWorkspaceWidget.Component.WorkspacesCollection != null &&
                                manualWorkspaceWidget.Component.WorkspacesCollection.Count > 0)
                            {
                                var assetIds = new List<int>();

                                foreach (var item in manualWorkspaceWidget.Component.WorkspacesCollection)
                                {
                                    int temp;
                                    if (Int32.TryParse(item.ItemId, out temp) &&
                                        !assetIds.Contains(temp))
                                    {
                                        assetIds.Add(temp);
                                    }
                                }

                                if (assetIds.Count >= 1)
                                {
                                    ProcessAssetInfo(assetIds[0], false);
                                }
                            }
                        }
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

        #region Core

        /// <summary>
        /// Populates the literals.
        /// </summary>
        public void PopulateLiterals()
        {
            Literals = new ManualNewsletterWorkspaceWidgetLiterals
                           {
                               CopyRight = string.Format("&copy;&nbsp;{0}&nbsp;{1}", DateTime.Now.Year, ResourceText.GetString("copyRightPhrase")),
                               NoResults = ResourceText.GetString("noResults"),
                               ViewLess = ResourceText.GetString("viewLess"),
                               ViewMore = ResourceText.GetString("viewMore"),
                               ViewAll = ResourceText.GetString("viewAll"),
                               Next = ResourceText.GetString("next"),
                               Previous = ResourceText.GetString("previous"),
                               New = ResourceText.GetString("flagNew"),
                               Hot = ResourceText.GetString("flagHot"),
                               MustRead = ResourceText.GetString("flagMustRead"),
                               MarketingSiteUrl = m_Marketing_Site_Url,
                               MarketingSiteTitle = m_Marketing_Site_Title
                           };

            // Link-out urls

            // Site absolute urls
            var builder = new BaseUrlBuilder("~/img/branding/djicon.gif")
                              {
                                  OutputType = BaseUrlBuilder.UrlOutputType.Absolute
                              };
            Literals.Icon = builder.ToString();

            builder = new BaseUrlBuilder("~/img/branding/djFactiva.gif")
                          {
                              OutputType = BaseUrlBuilder.UrlOutputType.Absolute
                          };
            Literals.BrandingBadge = builder.ToString();

            builder = new BaseUrlBuilder("~/img/syndication/hl/flag.gif")
                          {
                              OutputType = BaseUrlBuilder.UrlOutputType.Absolute
                          };
            Literals.ImportanceFlagUrl = builder.ToString();

        }

        /// <summary>
        /// Processes the asset info.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        private void ProcessAssetInfo(int assetId)
        {
            var workspace = m_WorkspaceManager.GetCachedWorkspaceById(assetId);
            if (workspace == null) return;
            var manualWorkspace = workspace as ManualWorkspace;
            ProcessAssetInfo(assetId, manualWorkspace);
        }

        /// <summary>
        /// Processes the asset info.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="checkIfIsWorkspaceIdAssociatedWithDissemenatedWidget">if set to <c>true</c> [check if is workspace id associated with dissemenated widget].</param>
        private void ProcessAssetInfo(int assetId, bool checkIfIsWorkspaceIdAssociatedWithDissemenatedWidget)
        {
            if (checkIfIsWorkspaceIdAssociatedWithDissemenatedWidget && m_WorkspaceManager.IsWorkspaceIdAssociatedWithDissemenatedWidget(assetId))
            {
                throw new EmgWidgetsUIException(EmgWidgetsUIException.WORKSPACE_HAS_BEEN_DISSEMINATED);
            }

            ManualWorkspace manualWorkspace = m_WorkspaceManager.GetManualWorkspaceById(assetId);
            ProcessAssetInfo(assetId, manualWorkspace);
        }

        /// <summary>
        /// Processes the asset info.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="manualWorkspace">The manual workspace.</param>
        private void ProcessAssetInfo(int assetId, ManualWorkspace manualWorkspace)
        {
            if (manualWorkspace != null)
            {
                Definition.Name = manualWorkspace.Properties.Name;
                Definition.ManualWorkspaceId = assetId;

                // Update the audience Options
                UpdateWidgetDefinitionAudience(manualWorkspace.Properties.Audience, Definition);

                Data = new ManualNewsletterWorkspaceWidgetData();
                // Fill with temp data

                // Check to see if feeds are on
                if (manualWorkspace.Properties.AreFeedsActive)
                {
                    // Fill with temp data
                    Data.ManualNewsletterWorkspaceInfo = GetManualNewsletterWorkspaceData(manualWorkspace);
                }
                else
                {
                    throw new EmgWidgetsUIException(EmgWidgetsUIException.WIDGET_ARE_FEEDS_ACTIVE_SET_TO_OFF);
                }
            }
        }

        private void PopulateDefinition(ManualWorkspaceWidget widget, string overridenInterfaceLanguage)
        {
            if (widget == null) return;

            Definition = new ManualNewsletterWorkspaceWidgetDefinition
                             {
                                 Id = widget.Id,
                                 Name = widget.Properties.Name,
                                 Description = widget.Properties.Description,
                                 Language = widget.Properties.Language
                             };
            MergeWidgetDefinition(widget, Definition);

            if (!string.IsNullOrEmpty(overridenInterfaceLanguage))
            {
                Definition.Language = overridenInterfaceLanguage;
            }
            return;
        }


        /// <summary>
        /// Gets the manual newsletter workspace data for preview.
        /// </summary>
        /// <param name="manualWorkspace">The manual workspace.</param>
        /// <returns></returns>
        private ManualNewsletterWorkspaceInfo GetManualNewsletterWorkspaceData(ManualWorkspace manualWorkspace)
        {
            var workspaceInfo = new ManualNewsletterWorkspaceInfo();
            workspaceInfo.Id = manualWorkspace.Id;
            workspaceInfo.Name = manualWorkspace.Properties.Name;
            try
            {
                var sectionCollection = new List<ManualNewsletterWorkspaceSection>();
                if (manualWorkspace.SectionCollection.Count > 0)
                {
                    var headlineUtility = new HeadlineUtility(m_SessionData, ResourceText);
                    var contentHeadlineStruct = new ContentHeadlineStruct
                                                    {
                                                        distributionType = Definition.DistributionType,
                                                        accountId = m_SessionData.AccountId,
                                                        accountNamespace = m_SessionData.ProductId
                                                    };

                    Dictionary<string, AccessionNumberBasedContentItem> dict = GetManualNewsletterWorkspaceItems(manualWorkspace);

                    foreach (Section section in manualWorkspace.SectionCollection)
                    {
                        var manualNewsletterWorkspaceSection = new ManualNewsletterWorkspaceSection();
                        manualNewsletterWorkspaceSection.Id = section.Id;
                        if (string.IsNullOrEmpty(section.Name)) 
                            section.Name = string.Empty;
                        manualNewsletterWorkspaceSection.Name = section.Name;
                        var headlineInfos = new List<HeadlineInfo>();
                        foreach (var item in section.ItemCollection)
                        {
                            if (item is ArticleItem)
                            {
                                var articleItem = item as ArticleItem;
 
                                // Get AccessNumberBasedContentItem
                                var contentItem = dict[articleItem.AccessionNumber];
                                if (contentItem != null && contentItem.HasBeenFound)
                                {
                                    headlineInfos.Add(GetHeadlineInfoForArticleItem(
                                        headlineUtility,
                                        contentHeadlineStruct,
                                        contentItem,
                                        articleItem));
                                }
                            }
                            else if (!(item is SeparatorItem))
                            {
                                if (item is LinkItem)
                                {
                                    var linkItem = item as LinkItem;
                                    headlineInfos.Add(GetHeadlineInfoForLinkItem(headlineUtility, linkItem));
                                }
                                else if (item is ImageItem)
                                {
                                    var imageItem = item as ImageItem;
                                    headlineInfos.Add(GetHeadlineInfoForImageItem(headlineUtility, imageItem));
                                }
                            }
                        }
                        manualNewsletterWorkspaceSection.Headlines = headlineInfos.ToArray();
                        sectionCollection.Add(manualNewsletterWorkspaceSection);
                    }
                }
                workspaceInfo.Sections = sectionCollection.ToArray();
                workspaceInfo.Count = sectionCollection.Count;
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

            return workspaceInfo;
        }

        /// <summary>
        /// Gets the enhanced article item.
        /// </summary>
        /// <param name="headlineUtility">The headline utility.</param>
        /// <param name="contentHeadlineStruct">The content headline struct.</param>
        /// <param name="contentItem">The content item.</param>
        /// <param name="articleItem">The article item.</param>
        /// <returns></returns>
        private static HeadlineInfo GetHeadlineInfoForArticleItem(HeadlineUtility headlineUtility, ContentHeadlineStruct contentHeadlineStruct, AccessionNumberBasedContentItem contentItem, ArticleItem articleItem)
        {
            contentHeadlineStruct.contentHeadline = contentItem.ContentHeadline;
            HeadlineInfo headlineInfo = headlineUtility.ConvertToHeadlineInfo(contentHeadlineStruct,articleItem);
            return headlineInfo;
        }

        private static HeadlineInfo GetHeadlineInfoForImageItem(HeadlineUtility headlineUtility, ImageItem item)
        {
            HeadlineInfo headlineInfo = headlineUtility.ConvertToHeadlineInfo(item);
            return headlineInfo; 
        }

        private static HeadlineInfo GetHeadlineInfoForLinkItem(HeadlineUtility headlineUtility, LinkItem item)
        {
            HeadlineInfo headlineInfo = headlineUtility.ConvertToHeadlineInfo(item);
            return headlineInfo; 
        }

        /// <summary>
        /// Gets the automatic workspace items.
        /// </summary>
        /// <param name="manualWorkspace">The auto workspace.</param>
        /// <returns></returns>
        private Dictionary<string, AccessionNumberBasedContentItem> GetManualNewsletterWorkspaceItems(ManualWorkspace manualWorkspace)
        {
            Dictionary<string, AccessionNumberBasedContentItem> dictOfAccessionBasedContentItems = new Dictionary<string, AccessionNumberBasedContentItem>();

            AccessionNumberSearchRequestDTO requestDTO = new AccessionNumberSearchRequestDTO();
            requestDTO.AccessionNumbers = GetAccessionNumbers(manualWorkspace).ToArray();
            requestDTO.SortBy = SortBy.LIFO;
            requestDTO.SearchCollectionCollection.Add(SearchCollection.Internal);
            requestDTO.SearchCollectionCollection.Add(SearchCollection.CustomerDoc);
            requestDTO.SearchCollectionCollection.Add(SearchCollection.Blogs);
            requestDTO.SearchCollectionCollection.Add(SearchCollection.Boards);

            if (requestDTO.IsValid())
            {
                AccessionNumberSearchResponse searchResponse = m_SearchManager.PerformAccessionNumberSearch(requestDTO);

                if (searchResponse != null && searchResponse.AccessionNumberBasedContentItemSet != null &&
                    searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection != null &&
                    searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection.Count > 0)
                {
                    // add items
                    foreach (AccessionNumberBasedContentItem item in searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
                    {
                        if (!dictOfAccessionBasedContentItems.ContainsKey(item.AccessionNumber))
                        {
                            dictOfAccessionBasedContentItems.Add(item.AccessionNumber, item);
                        }
                    }
                }
            }

            return dictOfAccessionBasedContentItems;
        }

        private static List<string> GetAccessionNumbers(ManualWorkspace manaualWorkspace)
        {
            List<string> accessionNos = new List<string>();
            foreach (Section section in manaualWorkspace.SectionCollection)
            {
                foreach (Item item in section.ItemCollection)
                {
                    ArticleItem articleItem = item as ArticleItem;
                    if (articleItem != null && !accessionNos.Contains(articleItem.AccessionNumber))
                    {
                        accessionNos.Add(articleItem.AccessionNumber);
                    }
                }
            }
            return accessionNos;
        }

        /// <summary>
        /// Merges the widget definition.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="widgetDefinition">The widget definition.</param>
        private static void MergeWidgetDefinition(ManualWorkspaceWidget widget, ManualNewsletterWorkspaceWidgetDefinition widgetDefinition)
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
                    widgetDefinition.FontFamily = (WidgetFontFamily) Enum.Parse(typeof (WidgetFontFamily), widget.Properties.Font.Name);
                }
            }

            // Update the audience Options
            UpdateWidgetDefinitionAudience(widget.Properties.Audience, widgetDefinition);

            // Update AccentColor, AccentFontColor and HeadlineDisplayType
            if (widget.Component == null || widget.Component.Properties == null) return;

            ManualWorkspaceComponentProperties properties = widget.Component.Properties;

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

            // Update the Widget Template
            widgetDefinition.WidgetTemplate = MapWidgetTemplate(properties.GraphTemplateType);

            // update numOfHeadlines to show
            widgetDefinition.NumItemsPerSection = properties.MaxResultsToReturn;
        }

        #endregion

        #endregion

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

            RssFeed rssFeed = new RssFeed();
            rssFeed.Channel = new RssChannel();
            rssFeed.Channel.Title = Definition.Name;
            rssFeed.Channel.Copyright = Literals.CopyRight;
            rssFeed.Channel.PublicationDate = DateTime.Now;
            rssFeed.Channel.TimeToLive = TIME_TO_LIVE_RSS;

            if (Data != null && Data.ManualNewsletterWorkspaceInfo != null)
            {
                foreach (ManualNewsletterWorkspaceSection section in Data.ManualNewsletterWorkspaceInfo.Sections)
                {
                    RssCategory category = new RssCategory(section.Name);
                    foreach (HeadlineInfo headline in section.Headlines)
                    {
                        RssItem item = new RssItem();
                        item.Title = headline.Text;
                        item.Description = headline.Snippet;
                        item.Link = new EscapedUri(headline.Url);
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
        public override string ToATOM()
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

            if (Data != null && Data.ManualNewsletterWorkspaceInfo != null)
            {
                foreach (ManualNewsletterWorkspaceSection section in Data.ManualNewsletterWorkspaceInfo.Sections)
                {
                    AtomCategory category = new AtomCategory(section.Name);
                    atomFeed.Categories.Add(category);
                    foreach (HeadlineInfo headline in section.Headlines)
                    {
                        EscapedUri link = new EscapedUri(headline.Url);
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
        /// Fills this instance. This is a contract for Command pattern.
        /// </summary>
        /// <param name="token">The widget token.</param>
        /// <param name="integrationTarget">The integration target.</param>
        public override void Fill(string token, IntegrationTarget integrationTarget)
        {
            using (TransactionLogger logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_Token = token;
                m_TokenProperties = new WidgetTokenProperties(token);
                ControlData userControlData = ControlDataManager.AddProxyCredentialsToControlData(ControlDataManagerEx.GetRssFeed1LightWeightUser(), m_TokenProperties.UserId, m_TokenProperties.NameSpace);
                m_WidgetManager = new WidgetManager(userControlData, SessionData.Instance().InterfaceLanguage);
                m_WorkspaceManager = new WorkspaceManager(userControlData, SessionData.Instance().InterfaceLanguage);
                m_SearchManager = new SearchManager(userControlData, SessionData.Instance().InterfaceLanguage);

                try
                {
                    // Get the widget Definition
                    Widget widget = m_WidgetManager.GetCachedWidgetById(m_TokenProperties.WidgetId);
                    if (widget != null)
                    {
                        if (widget is ManualWorkspaceWidget)
                        {
                            ManualWorkspaceWidget manualWorkspaceWidget = (ManualWorkspaceWidget)widget;
                            // Pull out the interfacelanguage
                            string interfaceLanguage = manualWorkspaceWidget.Properties.Language;

                            // Check cache to see if widget exists
                            // See if widget is in Factia cache
                            string cachedData;
                            if (m_WidgetManager.IsWidgetInFactivaCache(m_TokenProperties.WidgetId, interfaceLanguage, out cachedData))
                            {
                                ManualNewsletterWorkspaceWidgetDelegate cachedDelegate =
                                    (ManualNewsletterWorkspaceWidgetDelegate)
                                    Factiva.BusinessLayerLogic.Delegates.Utility.Deserialize(cachedData, GetType());
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
                                    FireMetricsEnvelope(GetOperationData(integrationTarget));
                                    NullOutAuthenticationCredentials();
                                    ElapsedTime = logger.LogTimeSinceInvocation();
                                    return;
                                }
                            }

                            // Set the ui culture on the thread.
                            m_SessionData = new SessionData("b", interfaceLanguage, 0, false);

                            // Populate Literals
                            PopulateLiterals();

                            // Populate with AlertHeadlineWidgetdDelegate
                            PopulateDefinition(manualWorkspaceWidget, null);

                            // Populate with valid Data
                            PopulateHeadlineWidgetData(manualWorkspaceWidget, m_TokenProperties.AccountId, m_TokenProperties.UserId, m_TokenProperties.NameSpace);

                            // Perform Validation
                            Validate();

                            // Put the object into cache
                            SerializeWidgetToCache(m_TokenProperties.WidgetId, interfaceLanguage);

                            // Update Urls 
                            UpdateUrls(m_TokenProperties, integrationTarget, interfaceLanguage);

                            // Update ODS and Metrics data
                            FireMetricsEnvelope(GetOperationData(integrationTarget));

                            // Take care of authentication credentials
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
        /// <returns></returns>
        private WidgetViewOperationalData GetOperationData(IntegrationTarget integrationTarget)
        {
            WidgetViewOperationalData operationalData = new WidgetViewOperationalData();
            operationalData.AssetId = m_TokenProperties.WidgetId;
            operationalData.PublishingDomain = HeadlineUtility.GetHttpReferer(integrationTarget);

            return operationalData;
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
            ManualWorkspaceWidget workspaceWidget = widget as ManualWorkspaceWidget;

            if (Definition == null) return;
            Data = new ManualNewsletterWorkspaceWidgetData();
            int workspaceId;
            if (GetWorkspaceId(workspaceWidget, out workspaceId))
            {
                ProcessAssetInfo(workspaceId);
            }
        }

        private void UpdateUrls(WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget, string interfaceLanguage)
        {
            if (Data.ManualNewsletterWorkspaceInfo == null || Data.ManualNewsletterWorkspaceInfo.Sections == null || Data.ManualNewsletterWorkspaceInfo.Sections.Length == 0) return;
            foreach (ManualNewsletterWorkspaceSection workspaceSection in Data.ManualNewsletterWorkspaceInfo.Sections)
            {
                if (workspaceSection.Headlines != null && workspaceSection.Headlines.Length > 0)
                {
                    foreach (HeadlineInfo headlineInfo in workspaceSection.Headlines)
                    {
                        if (headlineInfo.IsFactivaContent)
                        {
                            headlineInfo.Url = HeadlineUtility.GenerateCycloneManualNewsletterWorkspaceArticleLink(Definition, headlineInfo, Data.ManualNewsletterWorkspaceInfo, tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage);
                        }
                    }
                }
            }
        }

        private static bool GetWorkspaceId(ManualWorkspaceWidget workspaceWidget, out int workspaceId)
        {
            // Get a list of valid alertIds 
            if (workspaceWidget != null &&
                workspaceWidget.Component != null &&
                workspaceWidget.Component.WorkspacesCollection != null &&
                workspaceWidget.Component.WorkspacesCollection.Count > 0)
            {
                int id;
                if (Int32.TryParse(workspaceWidget.Component.WorkspacesCollection[0].ItemId, out id))
                {
                    workspaceId = id;
                    return true;
                }
            }
            workspaceId = 0;
            return false;
        }

        /// <summary>
        /// Nulls the out authentication credentials.
        /// </summary>
        protected override void NullOutAuthenticationCredentials()
        {
            NullOutAuthenticationCredential(Definition);
        }
    }
}