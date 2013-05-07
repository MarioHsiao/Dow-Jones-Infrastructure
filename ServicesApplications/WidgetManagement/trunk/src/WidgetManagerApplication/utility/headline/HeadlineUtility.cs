using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using EMG.Utility.Attributes;
using EMG.Utility.Formatters;
using EMG.Utility.Formatters.Globalization;
using EMG.Utility.Formatters.Numerical;
using EMG.Utility.OperationalData.EntryPoint;
using EMG.widgets.ui.delegates.core;
using EMG.widgets.ui.delegates.core.alertHeadline;
using EMG.widgets.ui.delegates.core.automaticWorkspace;
using EMG.widgets.ui.delegates.core.manualNewsletterWorkspace;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.Properties;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using factiva.nextgen;
using factiva.nextgen.ui;
using log4net;
using ContentCategory = Factiva.BusinessLayerLogic.ContentCategory;
using ContentHeadline = Factiva.Gateway.Messages.Search.V2_0.ContentHeadline;
using ContentItem=Factiva.Gateway.Messages.Search.V2_0.ContentItem;
using ContentLanguage = Factiva.BusinessLayerLogic.ContentLanguage;
using Encryption = FactivaEncryption.encryption;
using UrlBuilder=EMG.Utility.Uri.UrlBuilder;

namespace EMG.widgets.ui.utility.headline
{
    /// <summary>
    /// 
    /// </summary>
    public struct ContentHeadlineStruct
    {
        // Headline information

        // Account information

        /// <summary>
        /// Account ID
        /// </summary>
        public string accountId;

        /// <summary>
        /// Account Namespace
        /// </summary>
        public string accountNamespace;

        /// <summary>
        /// Content Headline.
        /// </summary>
        public ContentHeadline contentHeadline;

        /// <summary>
        /// Widget Distribution Type
        /// </summary>
        public WidgetDistributionType distributionType;
    }

    /// <summary>
    /// Utility class for Headline process
    /// </summary>
    public class HeadlineUtility
    {
        private const string Eid4TtlProxyKey = "FRGKA8384";
        private const string ExternalReaderPublicKey = "3x4e10e4";
        private const int MaxReferrerSize = 255;


        protected static readonly ILog Log = LogManager.GetLogger(typeof (HeadlineUtility));
        private readonly DateTimeFormatter _dateTimeFormatter;
        private readonly ResourceText _resourceText;
        private readonly NumberFormatter _numberFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlineUtility"/> class.
        /// </summary>
        /// <param name="sessionData">The session data.</param>
        /// <param name="resourceText">The resource text.</param>
        public HeadlineUtility(SessionData sessionData, ResourceText resourceText)
        {
            _resourceText = resourceText;
            _numberFormatter = new NumberFormatter();
            _dateTimeFormatter = new DateTimeFormatter(sessionData.InterfaceLanguage);
        }

        /// <summary>
        /// Converts the specified content headline.
        /// </summary>
        /// <param name="contentHeadlineStruct">The content headline structure.</param>
        /// <returns></returns>
        public AlertHeadlineInfo ConvertToAlertHeadlineInfo(ContentHeadlineStruct contentHeadlineStruct)
        {
            var headlineInfo = new AlertHeadlineInfo();
            Convert(headlineInfo, contentHeadlineStruct);

            headlineInfo.InternalDmmArticle = IsInternalWebPage(contentHeadlineStruct.contentHeadline);
            if (headlineInfo.InternalDmmArticle)
            {
                headlineInfo.PrimaryReference = GetPrimaryReference(contentHeadlineStruct);
            }

            // If there is a valid time update the following fields
            if (contentHeadlineStruct.contentHeadline.PublicationTime > DateTime.MinValue && !headlineInfo.InternalDmmArticle)
            {
                headlineInfo.PublicationDateTime =  DateTimeFormatter.Merge(contentHeadlineStruct.contentHeadline.PublicationDate, contentHeadlineStruct.contentHeadline.PublicationTime);
                headlineInfo.PubDateTime = _dateTimeFormatter.FormatLongDateTime(headlineInfo.PublicationDateTime);
            }
            else
            {
                headlineInfo.PublicationDateTime = contentHeadlineStruct.contentHeadline.PublicationDate;
                headlineInfo.PubDateTime = _dateTimeFormatter.FormatLongDate(contentHeadlineStruct.contentHeadline.PublicationDate);
            }
            return headlineInfo;
        }


        /// <summary>
        /// Converts the specified content headline.
        /// </summary>
        /// <param name="contentHeadlineStruct">The content headline structure.</param>
        /// <returns></returns>
        public HeadlineInfo ConvertToHeadlineInfo(ContentHeadlineStruct contentHeadlineStruct)
        {
            HeadlineInfo headlineInfo = new HeadlineInfo();
            Convert(headlineInfo, contentHeadlineStruct);
            return headlineInfo;
        }

        /// <summary>
        /// Converts the specified content headline.
        /// </summary>
        /// <param name="contentHeadlineStruct">The content headline struct.</param>
        /// <param name="articleItem">The article item.</param>
        /// <returns></returns>
        public HeadlineInfo ConvertToHeadlineInfo(ContentHeadlineStruct contentHeadlineStruct, ArticleItem articleItem)
        {
            HeadlineInfo headlineInfo = new HeadlineInfo();
            Convert(headlineInfo, contentHeadlineStruct, articleItem);
            return headlineInfo;
        }


        /// <summary>
        /// Converts to headline info.
        /// </summary>
        /// <param name="imageItem">The image item.</param>
        /// <returns></returns>
        public HeadlineInfo ConvertToHeadlineInfo(ImageItem imageItem)
        {
            HeadlineInfo headlineInfo = new HeadlineInfo();
            Convert(headlineInfo, imageItem);
            return headlineInfo;
        }


        /// <summary>
        /// Converts to headline info.
        /// </summary>
        /// <param name="linkItem">The link item.</param>
        /// <returns></returns>
        public HeadlineInfo ConvertToHeadlineInfo(LinkItem linkItem)
        {
            var headlineInfo = new HeadlineInfo();
            Convert(headlineInfo, linkItem);
            return headlineInfo;
        }

        /// <summary>
        /// Converts the specified headline info.
        /// </summary>
        /// <param name="headlineInfo">The headline info.</param>
        /// <param name="item">The item.</param>
        protected static void Convert(HeadlineInfo headlineInfo, ImageItem item)
        {
            headlineInfo.PublicationDateTime = item.CreationDate;
            //headlineInfo.PubDateTime = dateTimeFormatter.FormatLongDate(headlineInfo.PublicationDateTime);
            headlineInfo.Text = item.Title;
            headlineInfo.Comment = item.Comment;
            headlineInfo.Importance = item.Importance;
            // empty out the snippet.
            headlineInfo.Snippet = string.Empty;
            headlineInfo.WordCount = string.Empty;
            headlineInfo.Url = item.Uri;
            headlineInfo.IsFactivaContent = false;
        }

        /// <summary>
        /// Converts the specified headline info.
        /// </summary>
        /// <param name="headlineInfo">The headline info.</param>
        /// <param name="item">The item.</param>
        protected static void Convert(HeadlineInfo headlineInfo, LinkItem item)
        {
            headlineInfo.PublicationDateTime = item.PublicationDate.ToUniversalTime;
            headlineInfo.ByLine = item.Author;
            headlineInfo.PubDateTime = dateTimeFormatter.FormatLongDate(headlineInfo.PublicationDateTime);
            headlineInfo.Text = item.Title;
            headlineInfo.Comment = item.Comment;
            headlineInfo.Importance = item.Importance;
            headlineInfo.Lang = item.language;
            headlineInfo.SrcCode = "custom";
            headlineInfo.SrcName = item.sourceName;
            headlineInfo.Snippet = (item.Type == LinkType.RssHeadlineUrl) ? string.Empty : item.Description;
            headlineInfo.Url = item.Uri;
            headlineInfo.IsFactivaContent = false;
        }

        /// <summary>
        /// Converts the specified headline info.
        /// </summary>
        /// <param name="headlineInfo">The headline info.</param>
        /// <param name="contentHeadlineStruct">The content headline struct_v2.</param>
        protected void Convert(HeadlineInfo headlineInfo, ContentHeadlineStruct contentHeadlineStruct)
        {
            Convert(headlineInfo, contentHeadlineStruct, null);
        }

        /// <summary>
        /// Converts the specified headline info.
        /// </summary>
        /// <param name="headlineInfo">The headline info.</param>
        /// <param name="contentHeadlineStruct">The content headline struct.</param>
        /// <param name="articleItem">The article item.</param>
        protected void Convert(HeadlineInfo headlineInfo, ContentHeadlineStruct contentHeadlineStruct, ArticleItem articleItem)
        {
            // Generate cyclone url+
            headlineInfo.AccessionNumber = contentHeadlineStruct.contentHeadline.AccessionNo;
            headlineInfo.Lang = GetLanguageToContentLanguage(contentHeadlineStruct.contentHeadline.BaseLanguage);
            headlineInfo.ContentLanguage = MapLanguage(contentHeadlineStruct.contentHeadline.BaseLanguage);
            if (contentHeadlineStruct.contentHeadline.ContentItems != null &&
                !string.IsNullOrEmpty(contentHeadlineStruct.contentHeadline.ContentItems.ContentType) &&
                !string.IsNullOrEmpty(contentHeadlineStruct.contentHeadline.ContentItems.ContentType.Trim()))
            {
                headlineInfo.ContentType = contentHeadlineStruct.contentHeadline.ContentItems.ContentType;
                headlineInfo.ContentCategory = MapContentType(contentHeadlineStruct.contentHeadline.ContentItems.ContentType);
            }

            headlineInfo.PublicationDateTime = contentHeadlineStruct.contentHeadline.PublicationDate;
            headlineInfo.PubDateTime = _dateTimeFormatter.FormatLongDate(headlineInfo.PublicationDateTime);
            headlineInfo.Snippet = ParseParagraphsV2(contentHeadlineStruct.contentHeadline.Snippet, true).Trim();
            headlineInfo.SrcCode = contentHeadlineStruct.contentHeadline.SourceCode;
            headlineInfo.SrcName = contentHeadlineStruct.contentHeadline.SourceName;
            headlineInfo.Text = ParseParagraphsV2(contentHeadlineStruct.contentHeadline.Headline, false).Trim();
            // NOTE: Removed VP 6/23/09 Deloitte doesnt want it
            //Truncate(headlineInfo.Text, contentHeadlineStruct.contentHeadline.TruncationRules.ExtraSmall);
            headlineInfo.TruncText = null; 
            headlineInfo.WC = contentHeadlineStruct.contentHeadline.WordCount;
            headlineInfo.IconUrl = GetHeadlineIcon(contentHeadlineStruct.contentHeadline);

            if (contentHeadlineStruct.contentHeadline.PublicationTime > DateTime.MinValue)
            {
                headlineInfo.PublicationDateTime = DateTimeFormatter.Merge(contentHeadlineStruct.contentHeadline.PublicationDate, contentHeadlineStruct.contentHeadline.PublicationTime);
                headlineInfo.PubDateTime = _dateTimeFormatter.FormatLongDateTime(headlineInfo.PublicationDateTime);
            }
            
            if (contentHeadlineStruct.contentHeadline.WordCount > 0)
            {
                headlineInfo.WordCount = string.Format("{0} {1}",
                                                       _numberFormatter.Format(contentHeadlineStruct.contentHeadline.WordCount, NumberFormatType.Whole),
                                                       _resourceText.GetString("words"));
            }

            if (articleItem == null)
                return;
            headlineInfo.Comment = articleItem.Comment;
            headlineInfo.Importance = articleItem.Importance;
        }

        public static string GetIcon(HeadlineInfo headlineInfo)
        {
            if (headlineInfo == null || string.IsNullOrEmpty(headlineInfo.ContentType) || string.IsNullOrEmpty(headlineInfo.ContentType.Trim()))
                return null;

            string imgPath;
            if (HttpContext.Current != null)
            {
                imgPath = HttpContext.Current.Request.Url.Scheme + "://"
                    + HttpContext.Current.Request.Url.Authority
                    + HttpContext.Current.Request.ApplicationPath;
            }
            else
            {
                imgPath = "~";
            }

            switch (headlineInfo.ContentType.ToLower())
            {
                
                case "article":
                    imgPath += "/img/syndication/hl/articles.gif";
                    break;
                case "pdf":
                    imgPath += "/img/syndication/hl/pdf.gif";
                    break;
                case "picture":
                    imgPath += "/img/syndication/hl/graphic.gif";
                    break;
                case "multimedia":
                    imgPath += "/img/syndication/hl/multimedia.gif";
                    break;
                default:
                    imgPath += "/img/syndication/hl/html.gif";
                    break;
            }
            if (!string.IsNullOrEmpty(imgPath) && !(string.IsNullOrEmpty(imgPath.Trim())))
            {
                UrlBuilder urlBuilder = new UrlBuilder();
                urlBuilder.BaseUrl = imgPath;
                urlBuilder.OutputType = UrlBuilder.UrlOutputType.Absolute;
                return urlBuilder.ToString();
            }
            return imgPath;
        }

        /// <summary>
        /// Gets the headline icon.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns></returns>
        protected static string GetHeadlineIcon(ContentHeadline contentHeadline)
        {
            string imgPath;
            if (HttpContext.Current != null)
            {
                imgPath = HttpContext.Current.Request.Url.Scheme + "://" 
                    + HttpContext.Current.Request.Url.Authority 
                    + HttpContext.Current.Request.ApplicationPath;
            }
            else
            {
                imgPath = "~";
            }
            
            if (contentHeadline != null && contentHeadline.ContentItems != null &&
                contentHeadline.ContentItems.ContentType != null)
            {
                switch (contentHeadline.ContentItems.ContentType.ToLower())
                {
                    case "analyst":
                    case "pdf":
                        imgPath += "/img/syndication/hl/pdf.gif";
                        break;
                    case "board":
                    case "blog":
                    case "webpage":
                    case "html":
                        imgPath += "/img/syndication/hl/html.gif";
                        break;
                    case "articlewithgraphics":
                    case "internal":
                    case "picture":
                        imgPath += "/img/syndication/hl/graphic.gif";
                        break;
                    case "multimedia":
                        foreach (ContentItem item in contentHeadline.ContentItems.ItemCollection)
                        {
                            switch (item.Type.ToLower())
                            {
                                case "audio":
                                    imgPath += "/img/syndication/hl/audio.gif";
                                    break;
                                case "video":
                                    imgPath += "/img/syndication/hl/video.gif";
                                    break;
                            }
                        }
                        if (string.IsNullOrEmpty(imgPath))
                        {
                            imgPath += "/img/syndication/hl/multimedia.gif";
                        }
                        break;
                    case "article":
                        imgPath += "/img/syndication/hl/articles.gif";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(imgPath) && !(string.IsNullOrEmpty(imgPath.Trim())))
            {
                UrlBuilder urlBuilder = new UrlBuilder();
                urlBuilder.BaseUrl = imgPath;
                urlBuilder.OutputType = UrlBuilder.UrlOutputType.Absolute;
                return urlBuilder.ToString();
            }
            return imgPath;
        }

        /// <summary>
        /// Truncates the specified headline.
        /// </summary>
        /// <param name="headlineStr">The headline.</param>
        /// <param name="size">The size. extraSmall, small, medium, large</param>
        /// <returns>Truncated headline or original headline</returns>
        public static string Truncate(string headlineStr, int size)
        {
            if (String.IsNullOrEmpty(headlineStr))
                return headlineStr;

            if (Log.IsInfoEnabled)
                Log.InfoFormat("Truncate Utility:Truncate with size:{0}", size);

            if (size > 0 && headlineStr.Length >= size)
                headlineStr = String.Format("{0} ...", headlineStr.Substring(0, size).Trim());
            return headlineStr;
        }

        public static string GenerateCycloneAlertViewAllLink(AlertHeadlineWidgetDefinition definition, AlertInfo alertInfo, WidgetTokenProperties tokenProperties, WidgetDistributionType distributionType, IntegrationTarget integrationTarget, string language)
        {
            factiva.nextgen.ui.UrlBuilder ub = new factiva.nextgen.ui.UrlBuilder();
            ub.OutputType = UrlBuilder.UrlOutputType.Absolute;
            
            ub.BaseUrl = Settings.Default.Cyclone_Redirection_URL;
            ub.Append("fid", alertInfo.Id);
            ub.Append("fn", alertInfo.Name);

            // this may have to change
            ub.Append("ft", "g");
            ub.Append("napc", "b");

            // Add ep
            ub.Append("ep", "AL");
            ub.Append("od", GetOperationalDataMemento(definition, alertInfo, tokenProperties, integrationTarget, "va"));

            switch (distributionType)
            {
                case WidgetDistributionType.OnlyUsersInMyAccount:
                    ub.Append("p", "vf");
                    ub.Append("aid", tokenProperties.AccountId);
                    ub.Append("ns", tokenProperties.NameSpace);
                    break;
                case WidgetDistributionType.TTLProxyAccount:
                    ub.Append("p", "vf");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.EncryptedToken))
                    {
                        ub.Append("eid4", GetTTLProxyTokenForViewAll(alertInfo.Id.ToString(), definition.AuthenticationCredentials.EncryptedToken));
                    }
                    break;
                case WidgetDistributionType.ExternalReader:
                    ub.Append("p", "er");
                    ub.Append("f", "s");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.ProfileId))
                    {
                        string externalReaderToken = GetEncryptedExternalReaderToken(definition.AuthenticationCredentials.ProfileId,tokenProperties);
                        ub.Append("erc", externalReaderToken);
                    }
                    break;
                case WidgetDistributionType.UsersOutsideMyAccount:
                    // do nothing here
                    ub.Append("p", "vf");
                    break;
            }
            return ub.ToString(null);
        }


        /// <summary>
        /// Generates the cyclone automatic workspace article link.
        /// </summary>
        /// <returns></returns>
        public static string GenerateCycloneAutomaticWorkspaceArticleLink(AutomaticWorkspaceWidgetDefinition definition, HeadlineInfo headline, AutomaticWorkspaceInfo workspaceInfo, WidgetTokenProperties tokenProperties, WidgetDistributionType distributionType, IntegrationTarget integrationTarget, string language)
        {
            factiva.nextgen.ui.UrlBuilder ub = new factiva.nextgen.ui.UrlBuilder();
            ub.OutputType = UrlBuilder.UrlOutputType.Absolute;
            ub.BaseUrl = Settings.Default.Cyclone_Redirection_URL;
            ub.Append("an", headline.AccessionNumber);
            ub.Append("napc", "WW");

            // Add ep
            ub.Append("ep", "WS");
            ub.Append("od", GetOperationalDataMemento(definition, workspaceInfo, tokenProperties, integrationTarget));

            // map category
            ub.Append("cat", MapContentTypeToCat(headline.ContentType));


            switch (distributionType)
            {
                case WidgetDistributionType.OnlyUsersInMyAccount:
                    ub.Append("aid", tokenProperties.AccountId);
                    ub.Append("ns", tokenProperties.NameSpace);
                    ub.Append("p", "sa");
                    break;
                case WidgetDistributionType.TTLProxyAccount:
                    ub.Append("p", "sa");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.EncryptedToken))
                    {
                        ub.Append("eid4", GetTTLProxyTokenForArticleView(headline.AccessionNumber, definition.AuthenticationCredentials.EncryptedToken));
                    }
                    break;
                case WidgetDistributionType.ExternalReader:
                    ub.Append("p", "er");
                    ub.Append("f", "s");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.ProfileId))
                    {
                        string externalReaderToken = GetEncryptedExternalReaderToken(definition.AuthenticationCredentials.ProfileId,
                                                                                     tokenProperties);
                        ub.Append("erc", externalReaderToken);
                    }
                    break;
                case WidgetDistributionType.UsersOutsideMyAccount:
                    ub.Append("p", "sa");
                    // do nothing here
                    break;
            }
            return ub.ToString(null);
        }

        /// <summary>
        /// Generates the cyclone manual newsletter workspace article link.
        /// </summary>
        /// <returns></returns>
        public static string GenerateCycloneManualNewsletterWorkspaceArticleLink(ManualNewsletterWorkspaceWidgetDefinition definition, HeadlineInfo headline, ManualNewsletterWorkspaceInfo workspaceInfo, WidgetTokenProperties tokenProperties, WidgetDistributionType distributionType, IntegrationTarget integrationTarget, string language)
        {
            factiva.nextgen.ui.UrlBuilder ub = new factiva.nextgen.ui.UrlBuilder();
            ub.OutputType = UrlBuilder.UrlOutputType.Absolute;
            ub.BaseUrl = Settings.Default.Cyclone_Redirection_URL;
            ub.Append("an", headline.AccessionNumber);
            ub.Append("napc", "WN");

            // Add ep
            ub.Append("ep", "NL");
            ub.Append("od", GetOperationalDataMemento(definition, workspaceInfo, tokenProperties, integrationTarget));

            // map category
            ub.Append("cat", MapContentTypeToCat(headline.ContentType));


            switch (distributionType)
            {
                case WidgetDistributionType.OnlyUsersInMyAccount:
                    ub.Append("aid", tokenProperties.AccountId);
                    ub.Append("ns", tokenProperties.NameSpace);
                    ub.Append("p", "sa");
                    break;
                case WidgetDistributionType.TTLProxyAccount:
                    ub.Append("p", "sa");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.EncryptedToken))
                    {
                        ub.Append("eid4", GetTTLProxyTokenForArticleView(headline.AccessionNumber, definition.AuthenticationCredentials.EncryptedToken));
                    }
                    break;
                case WidgetDistributionType.ExternalReader:
                    ub.Append("p", "er");
                    ub.Append("f", "s");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.ProfileId))
                    {
                        string externalReaderToken = GetEncryptedExternalReaderToken(definition.AuthenticationCredentials.ProfileId,
                                                                                     tokenProperties);
                        ub.Append("erc", externalReaderToken);
                    }
                    break;
                case WidgetDistributionType.UsersOutsideMyAccount:
                    ub.Append("p", "sa");
                    // do nothing here
                    break;
            }
            return ub.ToString(null);
        }


        /// <summary>
        /// Generates the cyclone alert article link.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="headline">The info.</param>
        /// <param name="alertInfo">The alert info.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="distributionType">Type of the distribution.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// The fully qualified href valid for cyclone processing
        /// </returns>
        public static string GenerateCycloneAlertArticleLink(AlertHeadlineWidgetDefinition definition, AlertHeadlineInfo headline, AlertInfo alertInfo, WidgetTokenProperties tokenProperties, WidgetDistributionType distributionType, IntegrationTarget integrationTarget, string language)
        {
            if (headline.InternalDmmArticle)
            {
                return headline.PrimaryReference;
            }

            factiva.nextgen.ui.UrlBuilder ub = new factiva.nextgen.ui.UrlBuilder();
            ub.OutputType = UrlBuilder.UrlOutputType.Absolute;
            ub.BaseUrl = Settings.Default.Cyclone_Redirection_URL;
            ub.Append("an", headline.AccessionNumber);
            ub.Append("fid", alertInfo.Id);
            ub.Append("fn", alertInfo.Name);

            // this may have to change
            ub.Append("ft", "g");
            ub.Append("napc", "b");

            // Add ep
            ub.Append("ep", "AL");
            ub.Append("od", GetOperationalDataMemento(definition, alertInfo, tokenProperties, integrationTarget, "sa"));

            // map category
            ub.Append("cat", MapContentTypeToCat(headline.ContentType));


            switch (distributionType)
            {
                case WidgetDistributionType.OnlyUsersInMyAccount:
                    ub.Append("aid", tokenProperties.AccountId);
                    ub.Append("ns", tokenProperties.NameSpace);
                    ub.Append("p", "sta");
                    break;
                case WidgetDistributionType.TTLProxyAccount:
                    ub.Append("p", "sta");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.EncryptedToken))
                    {
                        ub.Append("eid4", GetTTLProxyTokenForArticleView(headline.AccessionNumber, definition.AuthenticationCredentials.EncryptedToken));
                    }
                    break;
                case WidgetDistributionType.ExternalReader:
                    ub.Append("p", "er");
                    ub.Append("f", "s");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.ProfileId))
                    {
                        string externalReaderToken = GetEncryptedExternalReaderToken(definition.AuthenticationCredentials.ProfileId,
                                                                                     tokenProperties);
                        ub.Append("erc", externalReaderToken);
                    }
                    break;
                case WidgetDistributionType.UsersOutsideMyAccount:
                    ub.Append("p", "sta");
                    // do nothing here
                    break;
            }
            return ub.ToString(null);
        }

        /// <summary>
        /// Gets the encrypted external reader token.
        /// </summary>
        /// <param name="profileId">The profile id.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <returns></returns>
        private static string GetEncryptedExternalReaderToken(string profileId, WidgetTokenProperties tokenProperties)
        {
            // Use factiva encription to encode into a token name/value pairs
            Encryption encryption = new Encryption();
            NameValueCollection nvp = new NameValueCollection(3);
            nvp.Add("ppid", tokenProperties.NameSpace);
            nvp.Add("puid", tokenProperties.UserId);
            nvp.Add("cpid", profileId);
            return encryption.encrypt(nvp, ExternalReaderPublicKey);
        }

        /// <summary>
        /// Gets the TTL proxy token for article view.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="TTLProxyToken">The TTL proxy token.</param>
        /// <returns></returns>
        private static string GetTTLProxyTokenForArticleView(string accessionNumber, string TTLProxyToken)
        {
            Encryption encryption = new Encryption();
            NameValueCollection nvp = new NameValueCollection(1);
            nvp.Add("an", accessionNumber);
            nvp.Add("proxyxsid", TTLProxyToken);
            return encryption.encrypt(nvp, Eid4TtlProxyKey);
        }

        /// <summary>
        /// Gets the TTL proxy token for view all.
        /// </summary>
        /// <param name="alertId">The alert id.</param>
        /// <param name="TTLProxyToken">The TTL proxy token.</param>
        /// <returns></returns>
        private static string GetTTLProxyTokenForViewAll(string alertId, string TTLProxyToken)
        {
            Encryption encryption = new Encryption();
            NameValueCollection nvp = new NameValueCollection(1);
            nvp.Add("fid", alertId);
            nvp.Add("proxyxsid", TTLProxyToken);
            return encryption.encrypt(nvp, Eid4TtlProxyKey);
        }

        /// <summary>
        /// Gets the operational data memento.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="alertInfo">The alert info.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <param name="linkType">Type of the link.</param>
        /// <returns></returns>
        private static string GetOperationalDataMemento(AlertHeadlineWidgetDefinition definition, AlertInfo alertInfo, WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget, string linkType)
        {
            AlertAssetOperationalData opData = new AlertAssetOperationalData(DisseminationMethod.Widget);
            opData.AssetId = alertInfo.Id.ToString();
            opData.AssetName = alertInfo.Name;
            opData.AlertType = MapProductType(alertInfo.Type); 
            opData.AudienceOption = MapDistributionTypeToDisseminationOption(definition.DistributionType);
            opData.LinkType = linkType;
            
            opData.WidgetOperationalData.WidgetID = tokenProperties.WidgetId;
            opData.WidgetOperationalData.WidgetName = definition.Name;
            opData.WidgetOperationalData.AssetCount = definition.alertIds.Length.ToString();
            opData.WidgetOperationalData.HeadlineFormat = definition.DisplayType.ToString();
            opData.WidgetOperationalData.NumberOfItems = definition.NumOfHeadlines.ToString();
            opData.WidgetOperationalData.PublisherID = tokenProperties.UserId;
            opData.WidgetOperationalData.PublisherNamespace = tokenProperties.NameSpace;
            opData.WidgetOperationalData.PublishingDomain = GetHttpReferer(integrationTarget);

            return opData.GetMemento;
        }

        /// <summary>
        /// Gets the operational data memento.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="workspaceInfo">The workspace info.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <returns></returns>
        private static string GetOperationalDataMemento(AutomaticWorkspaceWidgetDefinition definition, AutomaticWorkspaceInfo workspaceInfo, WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget)
        {
            WorkspaceAssetOperationalData opData = new WorkspaceAssetOperationalData(DisseminationMethod.Widget);
           opData.AssetId = workspaceInfo.Id.ToString();
           opData.AssetName = workspaceInfo.Name;
           opData.AudienceOption = MapDistributionTypeToDisseminationOption(definition.DistributionType);
           opData.LinkType = "sa";

           opData.WidgetOperationalData.WidgetID = tokenProperties.WidgetId;
           opData.WidgetOperationalData.WidgetName = definition.Name;
           opData.WidgetOperationalData.AssetCount = "1";
           opData.WidgetOperationalData.HeadlineFormat = definition.DisplayType.ToString();
           opData.WidgetOperationalData.NumberOfItems = definition.NumOfHeadlines.ToString();
           opData.WidgetOperationalData.PublisherID = tokenProperties.UserId;
           opData.WidgetOperationalData.PublisherNamespace = tokenProperties.NameSpace;
           opData.WidgetOperationalData.PublishingDomain = GetHttpReferer(integrationTarget);

           return opData.GetMemento;
        }

        /// <summary>
        /// Gets the operational data memento.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="workspaceInfo">The workspace info.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <returns></returns>
        private static string GetOperationalDataMemento(ManualNewsletterWorkspaceWidgetDefinition definition, ManualNewsletterWorkspaceInfo workspaceInfo, WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget)
        {
            NewsletterAssetOperationalData opData = new NewsletterAssetOperationalData(DisseminationMethod.Widget);
            opData.AssetId = workspaceInfo.Id.ToString();
            opData.AssetName = workspaceInfo.Name;
            opData.AudienceOption = MapDistributionTypeToDisseminationOption(definition.DistributionType);
            opData.LinkType = "sa";

            opData.WidgetOperationalData.WidgetID = tokenProperties.WidgetId;
            opData.WidgetOperationalData.WidgetName = definition.Name;
            opData.WidgetOperationalData.AssetCount = "1";
            opData.WidgetOperationalData.HeadlineFormat = WidgetHeadlineDisplayType.HeadlinesWithSnippets.ToString();
            opData.WidgetOperationalData.NumberOfItems = definition.NumItemsPerSection.ToString();
            opData.WidgetOperationalData.PublisherID = tokenProperties.UserId;
            opData.WidgetOperationalData.PublisherNamespace = tokenProperties.NameSpace;
            opData.WidgetOperationalData.PublishingDomain = GetHttpReferer(integrationTarget);

            return opData.GetMemento;
        }

        /// <summary>
        /// Maps the distribution type to dissemination option.
        /// </summary>
        /// <param name="distributionType">Type of the distribution.</param>
        /// <returns></returns>
        private static string MapDistributionTypeToDisseminationOption(WidgetDistributionType distributionType)
        {
            switch (distributionType)
            {
                case WidgetDistributionType.OnlyUsersInMyAccount:
                    return "inacct";
                case WidgetDistributionType.UsersOutsideMyAccount:
                    return "outacct";
                case WidgetDistributionType.TTLProxyAccount:
                    return "ttlt";
                default:
                    return "xrdr";
            }
        }

        /// <summary>
        /// Maps the type of the product.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /// <returns></returns>
        private static string MapProductType(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.FCPCompany:
                case ProductType.FCPExecutive:
                case ProductType.FCPIndustry:
                    return "FCE";
                case ProductType.Iff:
                    return "S20";
                case ProductType.IWE:
                    return "IWE";
                case ProductType.Lexis:
                    return "LEX";
                case ProductType.SelectFullText:
                case ProductType.SelectHeadlines:
                    return "SEL";
                default:
                    return "GBL";
            }
        }

        /// <summary>
        /// Gets the HTTP referer.
        /// </summary>
        /// <param name="integrationTarget">The integration target.</param>
        /// <returns></returns>
        public static string GetHttpReferer(IntegrationTarget integrationTarget)
        {
            switch (integrationTarget)
            {
                    // Return Known integration points
                case IntegrationTarget.Blogger:
                case IntegrationTarget.IGoogle:
                case IntegrationTarget.LiveDotCom:
                case IntegrationTarget.LiveSpaces:
                case IntegrationTarget.Netvibes:
                case IntegrationTarget.PageFlakes:
                    return integrationTarget.ToString();

                    // Return unknown integration points referrer.
                default:
                    HttpContext context = HttpContext.Current;
                    string referrer = (context.Request.UrlReferrer != null) ? context.Request.UrlReferrer.Host : context.Request.Url.Host;
                    if (referrer.Length > 255)
                    {
                        referrer = referrer.Substring(0, MaxReferrerSize);
                    }
                    return referrer;
            }
        }

        /// <summary>
        /// Determines whether [is internal web page] [the specified headline].
        /// </summary>
        /// <param name="headline">The headline.</param>
        /// <returns>
        /// 	<c>true</c> if [is internal web page] [the specified headline]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsInternalWebPage(ContentHeadline headline)
        {
            // if their is no content part assume the headline is
            // DMM internal content.
            if (headline == null ||
                headline.ContentItems == null ||
                string.IsNullOrEmpty(headline.ContentItems.ContentType) ||
                string.IsNullOrEmpty(headline.ContentItems.ContentType.Trim()))
            {
                return false;
            }
            switch (headline.ContentItems.ContentType.ToLower())
            {
                case "internalwebpage":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the primary reference.
        /// </summary>
        /// <param name="contentHeadlineStruct">The content headline struct.</param>
        /// <returns></returns>
        private static string GetPrimaryReference(ContentHeadlineStruct contentHeadlineStruct)
        {
            if (contentHeadlineStruct.contentHeadline != null &&
                contentHeadlineStruct.contentHeadline.ContentItems != null &&
                !string.IsNullOrEmpty(contentHeadlineStruct.contentHeadline.ContentItems.PrimaryRef) &&
                !string.IsNullOrEmpty(contentHeadlineStruct.contentHeadline.ContentItems.PrimaryRef.Trim()))
            {
                return contentHeadlineStruct.contentHeadline.ContentItems.PrimaryRef;
            }
            return null;
        }

        /// <summary>
        /// Maps the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        private static string MapContentTypeToCat(string category)
        {
            switch (category.ToLower())
            {
                case "article":
                    return "a";
                case "webpage":
                case "url":
                    return "w";
                case "pdf":
                    return "p";
                case "file":
                    return "f";
                case "picture":
                    return "i";
                case "multimedia":
                    return "m";
                case "coinfo":
                    return "r";
                case "analyst":
                    return "n";
                case "internal":
                    return "t";
                case "blog":
                    return "b";
                case "board":
                    return "o";
                default:
                    return "a";
            }
        }

        /// <summary>
        /// Parses the paragraphs.
        /// </summary>
        /// <param name="paras">The paras.</param>
        /// <param name="highlight">if set to <c>true</c> [highlight].</param>
        /// <returns></returns>
        private static string ParseParagraphs(IEnumerable<XmlNode> paras, bool highlight)
        {
            if (paras == null)
                return string.Empty;
            var sb = new StringBuilder();
            foreach (var para in paras)
            {
                if (para == null || !para.HasChildNodes)
                    continue;

                foreach (XmlNode node in para.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element && (node.Name == "hlt" || node.Name == "en"))
                    {
                        if (node.PreviousSibling != null && node.PreviousSibling.NodeType == XmlNodeType.Element && (node.PreviousSibling.Name == "hlt" || node.Name == "en"))
                        {
                            sb.Append(" ");
                        }
                        if (highlight)
                        {
                            sb.AppendFormat("<b>{0}</b>", node.InnerText);
                        }
                        else
                        {
                            sb.Append(node.InnerText);
                        }
                    }
                    else if (node.NodeType == XmlNodeType.Text)
                    {
                        sb.Append(node.Value);
                    }
                    sb.Append(" ");
                }
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Parses the paragraphs.
        /// </summary>
        /// <param name="paragraphs">The paragraphs.</param>
        /// <returns></returns>
        protected static string ParseParagraphs(Paragraph[] paragraphs)
        {
            if (paragraphs != null && paragraphs.Length > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Paragraph paragraph in paragraphs)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(paragraph.Value);
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Parses the paragraphs v2.
        /// </summary>
        /// <param name="markup">The markup.</param>
        /// <param name="highlight">if set to <c>true</c> [highlight].</param>
        /// <returns></returns>
        protected static string ParseParagraphsV2(Markup markup, bool highlight)
        {
            return ParseParagraphs(markup.Any, highlight);
        }

        /// <summary>
        /// Maps the language to content language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        private string GetLanguageToContentLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(language.Trim()))
            {
                FieldInfo fieldInfo = typeof (ContentLanguage).GetField(language.ToLower());
                if (fieldInfo != null)
                {
                    AssignedToken assignedToken = (AssignedToken) Attribute.GetCustomAttribute(fieldInfo, typeof (AssignedToken));
                    if (assignedToken != null)
                    {
                        return _resourceText.GetString(assignedToken.Token);
                    }
                }
            }
            return language;
        }

        /// <summary>
        /// Maps the language to content language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        private static ContentLanguage MapLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(language.Trim()))
            {
                ContentLanguage lang = (ContentLanguage) Enum.Parse(typeof (ContentLanguage), language);
                return lang;
            }
            throw new ArgumentNullException("language");
        }

        /// <summary>
        /// Maps the type of the content.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static ContentCategory MapContentType(string type)
        {
            switch (type.ToLower())
            {
                case "article":
                    return ContentCategory.Publications;
                case "webpage":
                    return ContentCategory.WebSites;
                case "pdf":
                    return ContentCategory.Publications;
                case "file":
                    return ContentCategory.Publications;
                case "picture":
                    return ContentCategory.Pictures;
                case "multimedia":
                    return ContentCategory.Multimedia;
                default:
                    return ContentCategory.WebSites;
            }
        }
    }
}