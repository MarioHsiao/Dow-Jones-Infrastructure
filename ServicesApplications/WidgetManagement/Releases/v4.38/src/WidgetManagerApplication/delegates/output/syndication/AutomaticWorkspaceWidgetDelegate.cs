using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Argotic.Syndication;
using EMG.Utility.Exceptions;
using EMG.Utility.Managers.Search.Requests;
using EMG.Utility.Managers.Search.Responses;
using EMG.Utility.OperationalData.AssetActivity;
using EMG.Utility.Url;
using EMG.widgets.Managers;
using EMG.widgets.ui.delegates.core;
using EMG.widgets.ui.delegates.core.automaticWorkspace;
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
using ContentItem=Factiva.Gateway.Messages.Assets.Common.V2_0.ContentItem;
using SearchServiceV2_0 = Factiva.Gateway.Services.V2_0;
using SearchManager = EMG.Utility.Managers.Search.SearchManager;
using SortBy=EMG.Utility.Managers.Search.Requests.SortBy;
using WorkspaceItem=Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0.WorkspaceItem;

namespace EMG.widgets.ui.delegates.output.syndication
{
    /// <summary>
    /// 
    /// </summary>
    public class AutomaticWorkspaceWidgetDelegate : AbstractWidgetDelegate, IWidgetPreviewDelegate
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof (AlertHeadlineWidgetDelegate));

        /// <summary>
        /// Data container of the headline Widget Delegate.
        /// </summary>
        public AutomaticWorkspaceWidgetData Data;

        /// <summary>
        /// Definition container of the headline Widget Delegate.
        /// </summary>
        public AutomaticWorkspaceWidgetDefinition Definition;

        /// <summary>
        /// Baseline CopyRight used for tokenization of phrases.
        /// </summary>
        public AutomaticWorkspaceWidgetLiterals Literals;

        private SearchManager m_SearchManager;
        private SessionData m_SessionData;
        private string m_Token;
        private WidgetTokenProperties m_TokenProperties;
        private WorkspaceManager m_WorkspaceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticWorkspaceWidgetDelegate"/> class.
        /// </summary>
        public AutomaticWorkspaceWidgetDelegate()
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
            using (TransactionLogger logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                m_WorkspaceManager = new WorkspaceManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                m_SearchManager = new SearchManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage); 
                
                Definition = new AutomaticWorkspaceWidgetDefinition();
                PopulateLiterals();

                AutomaticWorkspaceWidget automaticWorkspaceWidget = null;
                try
                {
                    automaticWorkspaceWidget = m_WidgetManager.GetLatestUpdatedAutomaticWorkspaceWidget();
                }
                catch (FactivaBusinessLogicException)
                {
                    // Do not show error keep defaults
                }
                catch (Exception ex)
                {
                    new EmgWidgetsUIException("Unable to get the Get Latest Updated Widget", ex);
                    ReturnCode = -1;
                    StatusMessage = ResourceText.GetErrorMessage(ReturnCode.ToString());
                    return;
                }

                try
                {
                    if (automaticWorkspaceWidget != null)
                    {
                        MergeWidgetDefinition(automaticWorkspaceWidget, Definition);
                    }

                    if (assetIds != null && assetIds.Count > 0)
                    {
                        ProcessAssetInfoForPreview(assetIds[0], true);
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
            using (TransactionLogger logger = new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_WidgetManager = new WidgetManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                m_WorkspaceManager = new WorkspaceManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                m_SearchManager = new SearchManager(m_SessionData.SessionBasedControlDataEx, m_SessionData.InterfaceLanguage);
                try
                {
                    Widget widget = m_WidgetManager.GetWidgetById(widgetId);
                    if (widget != null)
                    {
                        AutomaticWorkspaceWidget automaticWorkspaceWidget = widget as AutomaticWorkspaceWidget;
                        if (automaticWorkspaceWidget != null)
                        {
                            PopulateLiterals();

                            // Populate with AlertHeadlineWidgetdDelegate
                            PopulateDefinition(automaticWorkspaceWidget, m_SessionData.InterfaceLanguage);

                            // Get a list of ids out of the definition
                            if (automaticWorkspaceWidget.Component != null &&
                                automaticWorkspaceWidget.Component.WorkspacesCollection != null &&
                                automaticWorkspaceWidget.Component.WorkspacesCollection.Count > 0)
                            {
                                List<int> assetIds = new List<int>();

                                foreach (WorkspaceItem item in automaticWorkspaceWidget.Component.WorkspacesCollection)
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
                                    ProcessAssetInfoForPreview(assetIds[0], false);
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
            Literals = new AutomaticWorkspaceWidgetLiterals();
            Literals.CopyRight = string.Format("&copy;&nbsp;{0}&nbsp;{1}", DateTime.Now.Year, ResourceText.GetString("copyRightPhrase"));
            Literals.NoResults = ResourceText.GetString("noResults");
            Literals.ViewLess = ResourceText.GetString("viewLess");
            Literals.ViewMore = ResourceText.GetString("viewMore");
            Literals.ViewAll = ResourceText.GetString("viewAll");
            Literals.Next = ResourceText.GetString("next");
            Literals.Previous = ResourceText.GetString("previous");

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
        }

        private void PopulateDefinition(AutomaticWorkspaceWidget widget, string overridenInterfaceLanguage)
        {
            if (widget == null) return;

            Definition = new AutomaticWorkspaceWidgetDefinition();
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
        /// Processes the asset info.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="CheckIfIsWorkspaceIdAssociatedWithDissemenatedWidget">if set to <c>true</c> [check if is workspace id associated with dissemenated widget].</param>
        private void ProcessAssetInfoForPreview(int assetId, bool CheckIfIsWorkspaceIdAssociatedWithDissemenatedWidget)
        {
            if (CheckIfIsWorkspaceIdAssociatedWithDissemenatedWidget && m_WorkspaceManager.IsWorkspaceIdAssociatedWithDissemenatedWidget(assetId))
            {
                throw new EmgWidgetsUIException(EmgWidgetsUIException.WORKSPACE_HAS_BEEN_DISSEMINATED);
            }

            AutomaticWorkspace automaticWorkspace = m_WorkspaceManager.GetAutomaticWorkspaceById(assetId);
            ProcessAssetInfo(assetId, automaticWorkspace);
        }

        /// <summary>
        /// Processes the asset info.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        private void ProcessAssetInfo(int assetId)
        {
            Workspace workspace = m_WorkspaceManager.GetCachedWorkspaceById(assetId);
            if (workspace != null)
            {
                AutomaticWorkspace automaticWorkspace = workspace as AutomaticWorkspace;
                ProcessAssetInfo(assetId, automaticWorkspace);
            }
        }

        /// <summary>
        /// Processes the asset info.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="automaticWorkspace">The automatic workspace.</param>
        private void ProcessAssetInfo(int assetId, AutomaticWorkspace automaticWorkspace)
        {
            if (automaticWorkspace != null)
            {
                Definition.Name = automaticWorkspace.Properties.Name;
                Definition.AutomaticWorkspaceId = assetId;

                // Update the audience Options
                UpdateWidgetDefinitionAudience(automaticWorkspace.Properties.Audience, Definition);

                Data = new AutomaticWorkspaceWidgetData();

                // Check to see if feeds are on
                if (automaticWorkspace.Properties.AreFeedsActive)
                {
                    // Fill with temp data
                    Data.AutomaticWorkspaceInfo = GetAutomaticWorkspaceData(automaticWorkspace);
                }
                else
                {
                    throw new EmgWidgetsUIException(EmgWidgetsUIException.WIDGET_ARE_FEEDS_ACTIVE_SET_TO_OFF);
                }
            }
        }

        /// <summary>
        /// Gets the automatic workspace data for preview.
        /// </summary>
        /// <param name="autoWorkspace">The auto workspace.</param>
        /// <returns></returns>
        private AutomaticWorkspaceInfo GetAutomaticWorkspaceData(AutomaticWorkspace autoWorkspace)
        {
            AutomaticWorkspaceInfo workspaceInfo = new AutomaticWorkspaceInfo();
            workspaceInfo.Id = autoWorkspace.Id;
            workspaceInfo.Name = autoWorkspace.Properties.Name;
            try
            {
                HeadlineUtility headlineUtility = new HeadlineUtility(m_SessionData, ResourceText);
                ContentHeadlineStruct contentHeadlineStruct = new ContentHeadlineStruct();
                contentHeadlineStruct.distributionType = Definition.DistributionType;
                contentHeadlineStruct.accountId = m_SessionData.AccountId;
                contentHeadlineStruct.accountNamespace = m_SessionData.ProductId;

                AccessionNumberSearchResponse searchResponse = GetAutomaticWorkspaceItems(autoWorkspace);
                if (searchResponse != null && searchResponse.AccessionNumberBasedContentItemSet != null &&
                    searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection != null)
                {
                    if (searchResponse.AccessionNumberBasedContentItemSet.Count > 0)
                    {
                        List<HeadlineInfo> headlineInfos = new List<HeadlineInfo>(10);
                        foreach (AccessionNumberBasedContentItem contentItem in searchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
                        {
                            if (contentItem.HasBeenFound)
                            {
                                contentHeadlineStruct.contentHeadline = contentItem.ContentHeadline;
                                headlineInfos.Add(headlineUtility.ConvertToHeadlineInfo(contentHeadlineStruct));
                            }
                        }
                        if (headlineInfos.Count > 0)
                        {
                            workspaceInfo.Count = headlineInfos.Count;
                            workspaceInfo.Headlines = headlineInfos.ToArray();
                        }
                    }
                    else
                    {
                        workspaceInfo.Count = 0;
                        workspaceInfo.Headlines = new HeadlineInfo[0];
                    }
                }
                else
                {
                    workspaceInfo.Status = GetStatus(new EmgWidgetsUIException(EmgWidgetsUIException.UNABLE_TO_RETRIEVE_CONTENT_HEADLINES_RESULTSET));
                }
            }
            catch (FactivaBusinessLogicException fbe)
            {
                ReturnCode = fbe.ReturnCodeFromFactivaService;
                StatusMessage = ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
            }
            catch (Exception ex)
            {
                // Instantiate FactivaBusinessLogicException to write messages to log.
                FactivaBusinessLogicException fbe = new FactivaBusinessLogicException(ex, -1);
                ReturnCode = fbe.ReturnCodeFromFactivaService;
                StatusMessage = ResourceText.GetErrorMessage(fbe.ReturnCodeFromFactivaService.ToString());
            }

            return workspaceInfo;
        }


        private void UpdateUrls(WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget, string interfaceLanguage)
        {
            if (Data.AutomaticWorkspaceInfo == null || Data.AutomaticWorkspaceInfo.Headlines == null) return;

            foreach (HeadlineInfo headline in Data.AutomaticWorkspaceInfo.Headlines)
            {
                headline.Url = HeadlineUtility.GenerateCycloneAutomaticWorkspaceArticleLink(Definition, headline, Data.AutomaticWorkspaceInfo, tokenProperties, Definition.DistributionType, integrationTarget, interfaceLanguage);
            }
        }


        /// <summary>
        /// Gets the automatic workspace items.
        /// </summary>
        /// <param name="autoWorkspace">The auto workspace.</param>
        /// <returns></returns>
        private AccessionNumberSearchResponse GetAutomaticWorkspaceItems(AutomaticWorkspace autoWorkspace)
        {
            AccessionNumberSearchRequestDTO requestDTO = new AccessionNumberSearchRequestDTO();
            requestDTO.AccessionNumbers = GetAccessionNumbers(autoWorkspace).ToArray();
            requestDTO.SortBy = SortBy.LIFO;
            requestDTO.SearchCollectionCollection.Add(SearchCollection.Internal);       // internal is specific to dowjones domain
            requestDTO.SearchCollectionCollection.Add(SearchCollection.CustomerDoc);    // customer doc is internal
            requestDTO.SearchCollectionCollection.Add(SearchCollection.Blogs);
            requestDTO.SearchCollectionCollection.Add(SearchCollection.Boards);

            if (requestDTO.IsValid())
            {
                return m_SearchManager.PerformAccessionNumberSearch(requestDTO);
            }
            return null;
        }

        private static List<string> GetAccessionNumbers(AutomaticWorkspace autoWorkspace)
        {
            List<string> accessionNos = new List<string>();
            foreach (ContentItem item in autoWorkspace.ItemsCollection)
            {
                ArticleItem articleItem = item as ArticleItem;
                if (articleItem != null)
                {
                    if ( !accessionNos.Contains(articleItem.AccessionNumber) )
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
        private static void MergeWidgetDefinition(AutomaticWorkspaceWidget widget, AutomaticWorkspaceWidgetDefinition widgetDefinition)
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

            AutomaticWorkspaceComponentProperties properties = widget.Component.Properties;

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
            widgetDefinition.NumOfHeadlines = properties.MaxResultsToReturn;
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

            if (Data != null && Data.AutomaticWorkspaceInfo != null)
            {
                RssCategory category = new RssCategory(Data.AutomaticWorkspaceInfo.Name);
                foreach (HeadlineInfo headline in Data.AutomaticWorkspaceInfo.Headlines)
                {
                    RssItem item = new RssItem();
                    item.Title = headline.Text;
                    item.Description = headline.Snippet;
                    item.Link = new EscapedUri(headline.Url);
                    item.Categories.Add(category);
                    rssFeed.Channel.AddItem(item);
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

            if (Data != null && Data.AutomaticWorkspaceInfo != null)
            {
                AtomCategory category = new AtomCategory(Data.AutomaticWorkspaceInfo.Name);
                atomFeed.Categories.Add(category);
                foreach (HeadlineInfo headline in Data.AutomaticWorkspaceInfo.Headlines)
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
                        if (widget is AutomaticWorkspaceWidget)
                        {
                            AutomaticWorkspaceWidget automaticWorkspaceWidget = (AutomaticWorkspaceWidget) widget;

                            // Pull out the interfacelanguage
                            string interfaceLanguage = automaticWorkspaceWidget.Properties.Language;

                            // Check cache to see if widget exists
                            // See if widget is in Factia cache
                            string cachedData;
                            if (m_WidgetManager.IsWidgetInFactivaCache(m_TokenProperties.WidgetId, interfaceLanguage, out cachedData))
                            {
                                AutomaticWorkspaceWidgetDelegate cachedDelegate = (AutomaticWorkspaceWidgetDelegate) Factiva.BusinessLayerLogic.Delegates.Utility.Deserialize(cachedData, GetType());
                                
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
                            PopulateDefinition(automaticWorkspaceWidget, null);

                            // Populate with valid Data
                            PopulateHeadlineWidgetData(automaticWorkspaceWidget, m_TokenProperties.AccountId, m_TokenProperties.UserId, m_TokenProperties.NameSpace);

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
            AutomaticWorkspaceWidget workspaceWidget = widget as AutomaticWorkspaceWidget;

            if (Definition == null) return;
            Data = new AutomaticWorkspaceWidgetData();
            int workspaceId;
            if (GetWorkspaceId(workspaceWidget, out workspaceId))
            {
                ProcessAssetInfo(workspaceId);
            }
        }

        private static bool GetWorkspaceId(AutomaticWorkspaceWidget workspaceWidget, out int workspaceId)
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