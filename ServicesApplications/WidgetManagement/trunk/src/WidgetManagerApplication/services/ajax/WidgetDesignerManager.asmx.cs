// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetDesignerManager.asmx.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Summary description for GetHeadlines
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using EMG.Utility.Syndication.RSS;
using EMG.widgets.ui.delegates.output;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.exception;
using EMG.widgets.ui.services.ajax;
using EMG.widgets.ui.services.web;
using EMG.widgets.ui.syndication.integration;
using EMG.widgets.ui.syndication.integration.portalEndPoints;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.BusinessLayerLogic.Utility;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.ajaxDelegates;
using log4net;
using Encryption = FactivaEncryption.encryption;
using WidgetDelegate = EMG.widgets.ui.delegates.output.WidgetDelegate;
using WidgetSortBy = EMG.widgets.ui.dto.WidgetSortBy;
using WidgetType = EMG.widgets.ui.dto.WidgetType;

namespace EMG.widgets.services
{
    /// <summary>
    /// Widget Designer Manager Web Serverice
    /// </summary>
    [WebService(Namespace = "http://EMG.widgets.ws/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public partial class WidgetDesignerManager : BaseWebService
    {
        private const int MaxWidgetItemsToReturn = 500;
        private static readonly ILog Log = LogManager.GetLogger(typeof(WidgetDesignerManager));

        /// <summary>
        /// Dycrypts the widget token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        [WebMethod]
        public TokenItem[] DycryptWidgetToken(string token)
        {
            try
            {
                var encryption = new Encryption();
                var collection = encryption.decrypt(
                                                token,
                                                RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
                if (collection == null || collection.Count == 0)
                {
                    token = HttpUtility.UrlDecode(token);
                    collection = encryption.decrypt(token, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
                }

                var list = new List<TokenItem>(collection.Keys.Count);
                for (var i = 0; i < collection.Keys.Count; i++)
                {
                    list.Add(new TokenItem(collection.Keys[i], collection[collection.Keys[i]]));
                }

                return list.ToArray();
            }
            catch (EmgWidgetsUIException)
            {
                throw new EmgWidgetsUIException("Unable to decrypt token");
            }
        }

        private static void UpdateAjaxDelegate(FactivaBusinessLogicException fbe, IAjaxDelegate ajaxDelegate)
        {
            ajaxDelegate.ReturnCode = fbe.ReturnCodeFromFactivaService;
            ajaxDelegate.StatusMessage = ResourceText.GetInstance.GetErrorMessage(ajaxDelegate.ReturnCode.ToString());
        }

        /// <summary>
        /// Gets the integration end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        protected static IntegrationEndPoint GetIntegrationEndPoint(AbstractWidgetPortalEndPoint endPoint)
        {
            var integrationEndPoint = new IntegrationEndPoint
                                          {
                                              HasIcon = endPoint.HasIconImage, 
                                              Icon = QuerystringManager.GetLocalUrl(endPoint.IconVirtualPath), 
                                              Title = endPoint.Title, 
                                              Badge = endPoint.GetBadge(), 
                                              IntegrationUrl = endPoint.GetIntegrationUrl()
                                          };
            return integrationEndPoint;
        }

        /// <summary>
        /// Gets the integration end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <returns>A Integration Point Object.</returns>
        protected static IntegrationEndPoint GetIntegrationEndPoint(IRssPortalEndPoint endPoint)
        {
            var integrationEndPoint = new IntegrationEndPoint();
            var embeddedBadgeResource = endPoint as IEmbeddedBadgeResource;
            if (embeddedBadgeResource != null)
            {
                integrationEndPoint.Badge = embeddedBadgeResource.GetEmbeddedBadgetUrl();
            }

            integrationEndPoint.Title = ResourceText.GetInstance.GetString("add" + endPoint.GetType().Name.Replace("RssPortalEndPoint", string.Empty));
            integrationEndPoint.IntegrationUrl = endPoint.GetIntegrationUrl();
            return integrationEndPoint;
        }

        /// <summary>
        /// Gets the unique asset ids.
        /// </summary>
        /// <param name="assetIds">The asset ids.</param>
        /// <returns>A list of integers</returns>
        private static List<int> GetUniqueAssetIds(ICollection<int> assetIds)
        {
            var validIds = new List<int>();
            if (assetIds == null)
            {
                return validIds;
            }

            if (assetIds.Count == 1)
            {
                return new List<int>(assetIds);
            }

            // Make sure the list is unique and maintain order.
            if (assetIds.Count <= 1)
            {
                return validIds;
            }

            foreach (var i in assetIds.Where(i => !validIds.Contains(i)))
            {
                validIds.Add(i);
            }

            return validIds;
        }

        /// <summary>
        /// Deletes the alert widget.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overridden product prefix.</param>
        /// <returns>A Delete Widget Delegate.</returns>
        [WebMethod]
        [ScriptMethod]
        public DeleteWidgetDelegate DeleteWidget(string widgetId, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                var deleteWidgetDelegate = new DeleteWidgetDelegate();
                
                // Initialize SessionData.
                try
                {
                    var sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    var manager = new WidgetManager(sessionData.SessionBasedControlDataEx, sessionData.InterfaceLanguage);
                    manager.DeleteWidget(widgetId);
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    UpdateAjaxDelegate(fbe, deleteWidgetDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), deleteWidgetDelegate);
                }

                return deleteWidgetDelegate;
            }
        }

        /// <summary>
        /// Gets the widget.
        /// </summary>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overridden product prefix.</param>
        /// <returns>A Get Widget List Delegate</returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(WidgetType))]
        [GenerateScriptType(typeof(GetWidgetListDelegate))]
        [GenerateScriptType(typeof(WidgetDelegate))]
        public GetWidgetListDelegate GetWidgetList(WidgetSortBy sortBy, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            var widgetListDelegate = new GetWidgetListDelegate();
            using (var transLogger = new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                try
                {
                    var sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                    var widgetManager = new WidgetManager(SessionData.Instance().SessionBasedControlDataEx, SessionData.Instance().InterfaceLanguage);
                    transLogger.Reset();
                    var by = (sortBy == WidgetSortBy.Date) ? Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0.WidgetSortBy.LastModified : Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0.WidgetSortBy.Name;
                    var order = (sortBy == WidgetSortBy.Date) ? SortOrder.Descending : SortOrder.Ascending;
                    var widgets = widgetManager.GetWidgetList(by, order, MaxWidgetItemsToReturn, WidgetResponseElement.All);
                    transLogger.LogTimeSpentSinceReset("Fired GetWidgetList Transaction");
                    transLogger.Reset();
                    if (widgets != null && widgets.Length > 0)
                    {
                        var widgetDelegates = new List<WidgetDelegate>(widgets.Length);
                        var mappingDictionary = new Dictionary<string, string>();
                        foreach (var widget in widgets)
                        {
                            var item = new WidgetDelegate();
                            if (widget is AlertWidget)
                            {
                                item.Name = widget.Properties.Name;
                                item.Type = WidgetType.AlertHeadlineWidget;
                            }
                            else if (widget is AutomaticWorkspaceWidget)
                            {
                                var automaticWorkspaceWidget = widget as AutomaticWorkspaceWidget;
                                item.Type = WidgetType.AutomaticWorkspaceWidget;
                                if (automaticWorkspaceWidget.Component.WorkspacesCollection.Count > 0)
                                {
                                    mappingDictionary.Add(automaticWorkspaceWidget.Id, automaticWorkspaceWidget.Component.WorkspacesCollection[0].ItemId);
                                }
                            }
                            else if (widget is ManualWorkspaceWidget)
                            {
                                var manualWorkspaceWidget = widget as ManualWorkspaceWidget;
                                item.Type = WidgetType.ManualNewsletterWorkspaceWidget;
                                if (manualWorkspaceWidget.Component.WorkspacesCollection.Count > 0)
                                {
                                    mappingDictionary.Add(manualWorkspaceWidget.Id, manualWorkspaceWidget.Component.WorkspacesCollection[0].ItemId);
                                }
                            }

                            item.Id = widget.Id;
                            item.LastModifiedDateTime = sessionData.DateTimeFormatter.FormatDateTime(widget.Properties.LastAccessedDate);
                            item.CreationDateTime = sessionData.DateTimeFormatter.FormatDateTime(widget.Properties.CreationDate);
                            //// item.LastModifiedDate = sessionData.DateTimeFormatter.Format(widget.Properties.LastAccessedDate);
                            //// item.CreationDate = sessionData.DateTimeFormatter.Format(widget.Properties.CreationDate);
                            item.LastModifiedTicks = widget.Properties.LastAccessedDate.Ticks;
                            widgetDelegates.Add(item);
                        }

                        if (mappingDictionary.Count > 0)
                        {
                            MarryWorkspaceNames(widgetDelegates, mappingDictionary);
                        }

                        if (widgetDelegates.Count > 0)
                        {
                            widgetListDelegate.Widgets = widgetDelegates.ToArray();
                        }
                    }

                    transLogger.LogTimeSpentSinceReset("Processing of widget objects");
                }
                catch (FactivaBusinessLogicException fbe)
                {
                    // Suppress this error in getting the list of users
                    if (fbe.ReturnCodeFromFactivaService != 573012)
                    {
                        UpdateAjaxDelegate(fbe, widgetListDelegate);
                    }
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), widgetListDelegate);
                }
            }

            return widgetListDelegate;
        }

        private static void MarryWorkspaceNames(ICollection<WidgetDelegate> widgetDelegates,  IDictionary<string, string> mappingDictionary)
        {
            if (widgetDelegates.Count <= 0 || mappingDictionary.Count <= 0)
            {
                return;
            }

            var manager = new WorkspaceManager(SessionData.Instance().SessionBasedControlDataEx, SessionData.Instance().InterfaceLanguage);
            var workspaceItems = manager.GetWorkspaceList();

            if (workspaceItems.Count <= 0)
            {
                return;
            }

            foreach (var widgetDelegate in widgetDelegates)
            {
                // Check workspaceID
                if (!mappingDictionary.ContainsKey(widgetDelegate.Id))
                {
                    continue;
                }

                var workspaceId = mappingDictionary[widgetDelegate.Id];
                if (workspaceItems.ContainsKey(workspaceId))
                {
                    widgetDelegate.Name = workspaceItems[workspaceId].Properties.Name;
                }
            }
        }

        /// <summary>
        /// Gets the alert headline widget script code.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The overridden product prefix.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        [GenerateScriptType(typeof(GetWidgetCodeResponseDelegate))]
        public GetWidgetCodeResponseDelegate GetWidgetScriptCode(string widgetId, string accessPointCode, string interfaceLanguage, string productPrefix)
        {
            using (new TransactionLogger(Log, MethodBase.GetCurrentMethod()))
            {
                var getWidgetResponseDelegate = new GetWidgetCodeResponseDelegate();
                new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);

                if (!string.IsNullOrEmpty(widgetId) && !string.IsNullOrEmpty(widgetId.Trim()))
                {
                    try
                    {
                        var widgetManager = new WidgetManager(SessionData.Instance().SessionBasedControlDataEx, SessionData.Instance().InterfaceLanguage);
                        var type = GetWidgetType(widgetManager.GetCachedWidgetById(widgetId));
                        
                        // use factiva encryption to encode into a token name/value pairs
                        var encryption = new Encryption();

                        var nameValueCollection = new NameValueCollection
                                                      {
                                                          { RenderWidgetEncryptionConfiguration.WIDGET_ID, widgetId }, 
                                                          { RenderWidgetEncryptionConfiguration.USER_ID, SessionData.Instance().UserId }, 
                                                          { RenderWidgetEncryptionConfiguration.ACCOUNT_ID, SessionData.Instance().AccountId }, 
                                                          { RenderWidgetEncryptionConfiguration.NAMESPACE, SessionData.Instance().ProductId }
                                                      };

                        // Set the easy parameters
                        getWidgetResponseDelegate.Token = encryption.encrypt(nameValueCollection, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
                        getWidgetResponseDelegate.JavaSciptWidgetCodeSnippet = CodeSnippetManager.GetBaseJavaScriptWidgetCodeSnippet(getWidgetResponseDelegate.Token, type, true);
                        getWidgetResponseDelegate.JavaScriptCodeBaseUrl = CodeSnippetManager.GetBaseWidgetIntegrationUrl(getWidgetResponseDelegate.Token, type, true, IntegrationTarget.JavaScriptCode);

                        var renderWidgetDTO = new RenderWidgetDTO
                                                  {
                                                      token = getWidgetResponseDelegate.Token, 
                                                      type = type
                                                  };

                        // Initialize w/sharepoint webpart 
                        var endPoints = new List<IntegrationEndPoint>
                                        {
                                            GetIntegrationEndPoint(new SharePointWebPart(renderWidgetDTO))
                                        };

                        getWidgetResponseDelegate.IntegrationEndPoints = endPoints.ToArray();
                    }
                    catch (FactivaBusinessLogicException fbe)
                    {
                        UpdateAjaxDelegate(fbe, getWidgetResponseDelegate);
                    }
                    catch (Exception exception)
                    {
                        UpdateAjaxDelegate(new FactivaBusinessLogicException(exception, -1), getWidgetResponseDelegate);
                    }
                }
                else
                {
                    UpdateAjaxDelegate(new FactivaBusinessLogicException(EmgWidgetsUIException.INVALID_INPUT_AJAX_DELEGATE), getWidgetResponseDelegate);
                }

                return getWidgetResponseDelegate;
            }
        }

        /// <summary>
        /// Gets the type of the widget.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <returns></returns>
        private static WidgetType GetWidgetType(Widget widget)
        {
            switch (widget.GetType().Name)
            {
                case "AutomaticWorkspaceWidget":
                    return WidgetType.AutomaticWorkspaceWidget;
                case "ManualWorkspaceWidget":
                    return WidgetType.ManualNewsletterWorkspaceWidget;
                default:
                    return WidgetType.AlertHeadlineWidget;
            }
        }
    }
}