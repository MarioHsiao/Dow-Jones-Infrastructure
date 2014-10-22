using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using EMG.widgets.ui.delegates.core;
using EMG.widgets.ui.delegates.interfaces;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.exception;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Utility;
using Factiva.BusinessLayerLogic.Utility.Xml;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Web.Widgets.V2_0;
using factiva.nextgen.ui;
using factiva.nextgen.ui.ajaxDelegates;

namespace EMG.widgets.ui.delegates.abstractClasses
{
    /// <summary>
    /// Ajax Headline Widget Delegate class that adds functionality to AjaxDelegate
    /// </summary>
    public abstract class AbstractWidgetDelegate : AjaxDelegate, IWidgetSyndicationDelegate
    {
        private string m_CultureInfoCode;
        private string m_InterfaceLanguage;
        private readonly ResourceText m_ResourceText;

        protected const string m_Marketing_Site_Title = "Dow Jones Factiva";
        protected const string m_Marketing_Site_Url = "http://www.factiva.com";

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWidgetDelegate"/> class.
        /// </summary>
        public AbstractWidgetDelegate()
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
                new EMG_WidgetsUIException(ex, -1);
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
        /// Updates the audience.
        /// </summary>
        /// <param name="audience">The audience.</param>
        /// <param name="widgetDefinition">The widget definition.</param>
        protected static void UpdateWidgetDefinitionAudience(Audience audience, WidgetDefinition widgetDefinition)
        {
            // Update Distribution values
            widgetDefinition.DistributionType = WidgetDistributionType.OnlyUsersInMyAccount;
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
                    widgetDefinition.AuthenticationCredentials.EncryptedToken = audience.ProxyCredentials.EncrytedToken;
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
        /// <param name="callback"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual string ToJSONP(string callback, params string[] args)
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

        /// <summary>
        /// Fills this instance. This is a contract for Command pattern.
        /// </summary>
        /// <param name="token">The widget token.</param>
        /// <param name="integrationTarget">The integration target.</param>
        public abstract void Fill(string token, IntegrationTarget integrationTarget);

        #endregion
    }
}