using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Argotic.Common;
using EMG.Utility.OperationalData.AssetActivity;
using EMG.widgets.ui.delegates.core;
using EMG.widgets.ui.delegates.interfaces;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.exception;
using Factiva.BusinessLayerLogic;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.BusinessLayerLogic.Utility;
using Factiva.BusinessLayerLogic.Utility.Xml;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using factiva.nextgen.ui;
using factiva.nextgen.ui.ajaxDelegates;

namespace EMG.widgets.ui.delegates.output.syndication
{
    /// <summary>
    /// Ajax Headline Widget Delegate class that adds functionality to AjaxDelegate
    /// </summary>
    public abstract class AbstractWidgetDelegate : AjaxDelegate, IWidgetSyndicationDelegate
    {

        /// <summary>
        /// 
        /// </summary>
        protected const int TIME_TO_LIVE_RSS = 15;

        private string m_CultureInfoCode;
        private string m_InterfaceLanguage;
        private readonly ResourceText m_ResourceText;

        /// <summary>
        /// 
        /// </summary>
        protected WidgetManager m_WidgetManager;
        /// <summary>
        /// 
        /// </summary>
        protected const string m_Marketing_Site_Title = "Dow Jones Factiva";
        /// <summary>
        /// 
        /// </summary>
        protected const string m_Marketing_Site_Url = "http://www.factiva.com";

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWidgetDelegate"/> class.
        /// </summary>
        protected AbstractWidgetDelegate()
        {
            if (ResourceText.GetInstance != null)
            {
                m_ResourceText = ResourceText.GetInstance;
            }
        }

        /// <summary>
        /// Gets the resource text.
        /// </summary>
        /// <value>The resource text.</value>
        protected ResourceText ResourceText
        {
            get { return m_ResourceText; }
        }

        /// <summary>
        /// Fires the metrics envelope.
        /// </summary>
        /// <param name="operationalData">The operational data.</param>
        protected void FireMetricsEnvelope(WidgetViewOperationalData operationalData)
        {
            try
            {
                if (ReturnCode == 0 && m_WidgetManager != null)
                {
                    m_WidgetManager.FireUpdateRenderCountOnWidget(operationalData);
                }
                
            }
            catch (Exception)
            {
            }
        }



        /// <summary>
        /// Maps the language to culture info.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        protected static CultureInfo MapLanguageToCultureInfo(ContentLanguage language)
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
        /// Populates the headline widget data.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="proxyUserId">The proxy user id.</param>
        /// <param name="proxyNamespace">The proxy namespace.</param>
        protected abstract void PopulateHeadlineWidgetData(Widget widget, string accountId, string proxyUserId, string proxyNamespace);

        /// <summary>
        /// Nulls the out authentication credentials.
        /// </summary>
        protected abstract void NullOutAuthenticationCredentials();

        /// <summary>
        /// Nulls the out authentication credential.
        /// </summary>
        /// <param name="definition">The definition.</param>
        protected static void NullOutAuthenticationCredential(WidgetDefinition definition)
        {
            // null out the Credentials portion of the definition
            if (definition != null && definition.AuthenticationCredentials != null)
            {
                definition.AuthenticationCredentials = null;
            }
        }

        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <returns></returns>
        protected static string Serialize(ISyndicationResource feed)
        {
            return Serialize(feed, false);
        }

        /// <summary>
        /// Serializes the specified feed.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="useStylesheet">if set to <c>true</c> [use stylesheet].</param>
        /// <returns></returns>
        protected static string Serialize(ISyndicationResource feed, bool useStylesheet)
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
        /// Validates this instance.
        /// </summary>
        protected virtual void Validate()
        {
            
        }

        /// <summary>
        /// Serializes the widget to cache.
        /// </summary>
        /// <param name="widgetId">The widget id.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        protected void SerializeWidgetToCache(string widgetId, string interfaceLanguage)
        {
            // Serialize the widget to Factiva cache
            if (ReturnCode == 0 && m_WidgetManager != null)
            {
                m_WidgetManager.CacheWidget(widgetId, interfaceLanguage, Factiva.BusinessLayerLogic.Delegates.Utility.Serialize(this));
            }
        }
        /// <summary>
        /// Sets the thread culture.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        protected void SetThreadCulture(string interfaceLanguage)
        {
            // Set interface lanaguage
            m_InterfaceLanguage = LanguageUtility.ValidateLanguageCode(interfaceLanguage);
            m_CultureInfoCode = LanguageUtility.GetCultureInfoLanguageCode(m_InterfaceLanguage, true);

            try
            {
                switch (m_InterfaceLanguage)
                {
                    default:
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(m_CultureInfoCode);
                        break;

                    case "zh":
                    case "zh-cn":
                    case "zhcn":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("zh-cn");
                        break;
                    case "zh-tw":
                    case "zhtw":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("zh-tw");
                        break;

                    case "template":
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                        m_InterfaceLanguage = "en";
                        m_CultureInfoCode = "en";
                        break;
                }
            }
            catch (Exception ex)
            {
                new EmgWidgetsUIException(ex, -1);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                m_InterfaceLanguage = "en";
                m_CultureInfoCode = "en";
            }
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="fbe">The fbe.</param>
        /// <returns></returns>
        protected Status GetStatus(FactivaBusinessLogicException fbe)
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

        /// <summary>
        /// Maps the display type of the widget headline.
        /// </summary>
        /// <param name="SnippetType">Type of the snippet.</param>
        /// <returns></returns>
        protected static WidgetHeadlineDisplayType MapWidgetHeadlineDisplayType(SnippetType SnippetType)
        {
            switch (SnippetType)
            {
                case SnippetType.Fixed:
                    return WidgetHeadlineDisplayType.HeadlinesWithSnippets;
                default:
                    return WidgetHeadlineDisplayType.HeadlinesOnly;
            }
        }

        /// <summary>
        /// Gets the default discovery tabs.
        /// </summary>
        /// <returns></returns>
        protected static WidgetDiscoveryTab[] GetDefaultDiscoveryTabs()
        {
            WidgetDiscoveryTabCollection widgetDiscoveryTabCollection = new WidgetDiscoveryTabCollection();
            
            WidgetDiscoveryTab widgetDiscoveryTab = new WidgetDiscoveryTab();
            widgetDiscoveryTab.Active = true;
            widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("headlines");
            widgetDiscoveryTab.Id = "headlines";
            widgetDiscoveryTab.DisplayCheckbox = false;
            widgetDiscoveryTabCollection.Insert(0, widgetDiscoveryTab);

            widgetDiscoveryTab.Active = true;
            widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("companies");
            widgetDiscoveryTab.Id = "companies";
            widgetDiscoveryTab.DisplayCheckbox = true;
            widgetDiscoveryTabCollection.Insert(1, widgetDiscoveryTab);

            widgetDiscoveryTab.Active = true;
            widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("executives");
            widgetDiscoveryTab.Id = "executives";
            widgetDiscoveryTab.DisplayCheckbox = true;
            widgetDiscoveryTabCollection.Insert(2, widgetDiscoveryTab);

            widgetDiscoveryTab.Active = true;
            widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("industries");
            widgetDiscoveryTab.Id = "industries";
            widgetDiscoveryTab.DisplayCheckbox = true;
            widgetDiscoveryTabCollection.Insert(3, widgetDiscoveryTab);

            widgetDiscoveryTab.Active = true;
            widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("regions");
            widgetDiscoveryTab.Id = "regions";
            widgetDiscoveryTab.DisplayCheckbox = true;
            widgetDiscoveryTabCollection.Insert(4, widgetDiscoveryTab);

            widgetDiscoveryTab.Active = true;
            widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("newsSubjects");
            widgetDiscoveryTab.Id = "subjects";
            widgetDiscoveryTab.DisplayCheckbox = true;
            widgetDiscoveryTabCollection.Insert(5, widgetDiscoveryTab);


            return widgetDiscoveryTabCollection.ToArray();
        }

        /// <summary>
        /// Maps the discovery tabs.
        /// </summary>
        /// <param name="tabCollection">The tab collection.</param>
        /// <param name="discoveryTabs">discovery Tabs.</param>
        /// <returns></returns>
        protected static WidgetDiscoveryTab[] MapDiscoveryTabs(TabCollection tabCollection, WidgetDiscoveryTab[] discoveryTabs)
        {
            WidgetDiscoveryTabCollection widgetDiscoveryTabCollection = new WidgetDiscoveryTabCollection();

            if (tabCollection != null && tabCollection.Count > 0)
            {
                var foundHeadline = false;
                foreach (Tab tab in tabCollection)
                {
                    if (tab != null)
                    {
                        //Bug fix for possible incorrect data where Headlines tab comes in twice.
                        if (tab.Type.Equals(TabType.Headlines))
                        {
                            if (foundHeadline)
                            {
                                tab.Type = TabType.Regions;
                            }
                            else
                            {
                                foundHeadline = true;
                            }
                        }
                        //End of Bug fix

                        // Insert the tab at the position specified by the Tab object
                        widgetDiscoveryTabCollection.Insert(tab.Position, MapDiscoveryTab(tab));
                    }
                }
            }
            if (widgetDiscoveryTabCollection.Count < discoveryTabs.Length)
            {
                //Append the missing discovery tabs
                foreach (WidgetDiscoveryTab discoveryTab in discoveryTabs)
                {
                    bool found = false;
                    foreach (WidgetDiscoveryTab widgetDiscoveryTab in widgetDiscoveryTabCollection)
                    {
                        if (widgetDiscoveryTab.Id == discoveryTab.Id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        discoveryTab.Active = false;
                        widgetDiscoveryTabCollection.Insert(widgetDiscoveryTabCollection.Count, discoveryTab);
                    }

                }
            }
            return widgetDiscoveryTabCollection.ToArray();
        }

        /// <summary>
        /// Maps the discovery tab.
        /// </summary>
        /// <param name="tabCollection">The tab collection.</param>
        /// <returns></returns>
        protected static WidgetDiscoveryTab MapDiscoveryTab(Tab tab)
        {
            WidgetDiscoveryTab widgetDiscoveryTab = new WidgetDiscoveryTab();
            widgetDiscoveryTab.Active = Convert.ToBoolean(tab.Active);
            switch (tab.Type)
            {
                case TabType.Headlines:
                    widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("headlines");
                    widgetDiscoveryTab.Id = "headlines";
                    widgetDiscoveryTab.DisplayCheckbox = false;
                    break;
                case TabType.Company:
                    widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("companies");
                    widgetDiscoveryTab.Id = "companies";
                    widgetDiscoveryTab.DisplayCheckbox = true;
                    break;
                case TabType.Executives:
                    widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("executives");
                    widgetDiscoveryTab.Id = "executives";
                    widgetDiscoveryTab.DisplayCheckbox = true;
                    break;
                case TabType.Industry:
                    widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("industries");
                    widgetDiscoveryTab.Id = "industries";
                    widgetDiscoveryTab.DisplayCheckbox = true;
                    break;
                case TabType.Regions:
                    widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("regions");
                    widgetDiscoveryTab.Id = "regions";
                    widgetDiscoveryTab.DisplayCheckbox = true;
                    break;
                case TabType.NewSubjects:
                    widgetDiscoveryTab.Text = ResourceText.GetInstance.GetString("newsSubjects");
                    widgetDiscoveryTab.Id = "subjects";
                    widgetDiscoveryTab.DisplayCheckbox = true;
                    break;
                default:
                    break;
            }
            return widgetDiscoveryTab;
        }

        /// <summary>
        /// Maps the size of the widget font.
        /// </summary>
        /// <param name="fontSize">Size of the font.</param>
        /// <returns></returns>
        protected static WidgetFontSize MapWidgetFontSize(FontSize fontSize)
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
        /// Maps the widget template.
        /// </summary>
        /// <param name="graphTemplateType">Type of the graph template.</param>
        /// <returns></returns>
        protected static WidgetTemplate MapWidgetTemplate(GraphTemplateType graphTemplateType)
        {
            switch (graphTemplateType)
            {
                case GraphTemplateType.EditDesign:
                    return WidgetTemplate.EditDesign;
                case GraphTemplateType.Corporate:
                    return WidgetTemplate.Corporate;
                case GraphTemplateType.DarkBlue:
                    return WidgetTemplate.DarkBlue;
                case GraphTemplateType.Green:
                    return WidgetTemplate.Green;
                case GraphTemplateType.Grey:
                    return WidgetTemplate.Grey;
                case GraphTemplateType.Red:
                    return WidgetTemplate.Red;
                default:
                    return WidgetTemplate.EditDesign;
            }
        }

        /// <summary>
        /// Updates the audience.
        /// </summary>
        /// <param name="audience">The audience.</param>
        /// <param name="widgetDefinition">The widget definition.</param>
        protected static void UpdateWidgetDefinitionAudience(Audience audience, WidgetDefinition widgetDefinition)
        {
            // Update Distribution values
            widgetDefinition.AuthenticationCredentials = new AuthenticationCredentials();
            if (audience != null)
            {
                switch (audience.AudienceOptions)
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
                widgetDefinition.AuthenticationCredentials.ProfileId = audience.ProfileId;

                // update the authentication credentials
                if (audience.ProxyCredentials != null)
                {
                    switch (audience.ProxyCredentials.AuthenticationScheme)
                    {
                        case AuthenticationScheme.Email:
                            widgetDefinition.AuthenticationCredentials.AuthenticationScheme = WidgetAuthenticationScheme.EmailAddress;
                            break;
                        default:
                            widgetDefinition.AuthenticationCredentials.AuthenticationScheme = WidgetAuthenticationScheme.UserId;
                            break;
                    }
                    widgetDefinition.AuthenticationCredentials.EncryptedToken = audience.ProxyCredentials.EncryptedToken;
                    widgetDefinition.AuthenticationCredentials.ProxyUserId = audience.ProxyCredentials.UserId;
                    widgetDefinition.AuthenticationCredentials.ProxyEmailAddress = audience.ProxyCredentials.EmailAddress;
                    widgetDefinition.AuthenticationCredentials.ProxyPassword = audience.ProxyCredentials.Password;
                    widgetDefinition.AuthenticationCredentials.ProxyNamespace = audience.ProxyCredentials.Namespace;
                }
            }
        }

        #region IWidgetSyndicationDelegate Members

        /// <summary>
        /// Deserializes to RSS.
        /// </summary>
        /// <returns></returns>
        public abstract string ToRSS();

        /// <summary>
        /// Deserializes to ATOM.
        /// </summary>
        /// <returns></returns>
        public abstract string ToATOM();

        /// <summary>
        /// Deserializes to JSON.
        /// </summary>
        /// <returns></returns>
        public virtual string ToJSON()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

        /// <summary>
        /// Deserializes to XML.
        /// </summary>
        /// <returns></returns>
        public virtual string ToXML()
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
        /// <param name="callback">The callback string.</param>
        /// <param name="args">The arguments params.</param>
        /// <returns></returns>
        public virtual string ToJSONP(string callback, params string[] args)
        {
            if (string.IsNullOrEmpty(callback))
            {
                return ToJSON();
            }

            return ConvertToJSONP(ToJSON(), callback, args);
        }

        /// <summary>
        /// Converts a JSON string to a JSONP.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="callback">The callback string.</param>
        /// <param name="args">The string array of args.</param>
        /// <returns>A string of JavaScript representing a JSONP string</returns>
        public static string ConvertToJSONP(string json, string callback, params string[] args)
        {
            if (json == null)
            {
                throw new ArgumentNullException("json");
            }

            if (string.IsNullOrEmpty(callback))
            {
                return json;
            }

            var sb = new StringBuilder();
            sb.AppendFormat("if(typeof {0} === 'function') {0} (", callback);
            sb.Append(json);
            if (args != null && args.Length > 0)
            {
                sb.Append(",");
                var temp = Array.ConvertAll(args, s => string.Format("\"{0}\"", s.Replace("\"", "&quot;")));
                sb.Append(string.Join(",", temp));
            }

            sb.Append(");");
            return sb.ToString();
        }

        /// <summary>
        /// Fills this instance. This is a contract for Command pattern.
        /// </summary>
        /// <param name="token">The widget token.</param>
        /// <param name="integrationTarget">The integration target.</param>
        public abstract void Fill(string token, IntegrationTarget integrationTarget);

        #endregion
    }
}