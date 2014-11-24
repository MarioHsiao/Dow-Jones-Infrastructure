using System.Collections.Specialized;
using System.Web;
using EMG.Utility.OperationalData.EntryPoint;
using EMG.Utility.Uri;
using EMG.widgets.ui.delegates.core.alertHeadline;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.Properties;
using EMG.widgets.ui.utility.headline;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;
using Factiva.Gateway.Messages.Track.V1_0;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.utility.discovery
{
    /// <summary>
    /// 
    /// </summary>
    public class DiscoveryUtility
    {
        private const int MAX_REFERRER_SIZE = 255;
        private const string EID4_TTL_PROXY_KEY = "FRGKA8384";
        private const string EXTERNAL_READER_PUBLIC_KEY = "3x4e10e4";
        private const string DISCOVERY_REDIRECTION_URL = "~/utility/discovery/redir.ashx";

        /// <summary>
        /// Generates the cyclone discovery view alert link.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="alertInfo">The alert info.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="distributionType">Type of the distribution.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static string GenerateCycloneDiscoveryGetAlertLink(AlertHeadlineWidgetDefinition definition, AlertInfo alertInfo, WidgetTokenProperties tokenProperties, WidgetDistributionType distributionType, IntegrationTarget integrationTarget, string language)
        {
            factiva.nextgen.ui.UrlBuilder ub = new factiva.nextgen.ui.UrlBuilder();
            ub.OutputType = UrlBuilder.UrlOutputType.Absolute;
            ub.BaseUrl = DISCOVERY_REDIRECTION_URL;
            ub.Append("p", "sl");
            ub.Append("ep", "SMAIL");
            ub.Append("fid", alertInfo.Id);
            ub.Append("od", GetOperationalDataMemento(definition, alertInfo, tokenProperties, integrationTarget));

            switch (distributionType)
            {
                case WidgetDistributionType.OnlyUsersInMyAccount:
                    ub.Append("aid", tokenProperties.AccountId);
                    ub.Append("ns", tokenProperties.NameSpace);
                    break;
                case WidgetDistributionType.UsersOutsideMyAccount:
                    // do nothing here
                    break;
            }

            return ub.ToString(null);
        }

        /// <summary>
        /// Generates the cyclone discovery chart item link.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="alertInfo">The alert info.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="distributionType">Type of the distribution.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <param name="language">The language.</param>
        /// <param name="fsr">The FSR.</param>
        /// <param name="linkType">Type of the link.</param>
        /// <returns></returns>
        public static string GenerateCycloneDiscoveryChartItemLink(AlertHeadlineWidgetDefinition definition, AlertInfo alertInfo, WidgetTokenProperties tokenProperties, WidgetDistributionType distributionType, IntegrationTarget integrationTarget, string language, string fsr, string linkType)
        {
            factiva.nextgen.ui.UrlBuilder ub = new factiva.nextgen.ui.UrlBuilder();
            ub.OutputType = UrlBuilder.UrlOutputType.Absolute;
            ub.BaseUrl = DISCOVERY_REDIRECTION_URL; //change to redirection handler .ashx
            ub.Append("fid", alertInfo.Id);
            ub.Append("fsr", fsr);
            ub.Append("od", GetOperationalDataMemento(definition, alertInfo, tokenProperties, integrationTarget, linkType));
            ub.Append("view", "a");
            ub.Append(AlertHeadlineUtility.AlertMetadataQuerystringName, AlertHeadlineUtility.GetAlertMetadata(alertInfo));

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
                        ub.Append("eid4", GetTTLProxyTokenForChartItemLink(alertInfo.Id.ToString(), fsr, definition.AuthenticationCredentials.EncryptedToken));
                    }
                    break;
                case WidgetDistributionType.ExternalReader:
                    ub.Append("p", "er");
                    ub.Append("f", "s");
                    if (definition.AuthenticationCredentials != null && !string.IsNullOrEmpty(definition.AuthenticationCredentials.ProfileId))
                    {
                        string externalReaderToken = GetEncryptedExternalReaderToken(definition.AuthenticationCredentials.ProfileId, tokenProperties);
                        ub.Append("erc", externalReaderToken);
                    }
                    break;
                case WidgetDistributionType.UsersOutsideMyAccount:
                    ub.Append("p", "vf");
                    // do nothing here
                    break;
            }
            return ub.ToString(null);
        }

        /// <summary>
        /// Gets the operational data memento.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="alertInfo">The alert info.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <param name="integrationTarget">The integration target.</param>
        /// <returns></returns>
        private static string GetOperationalDataMemento(AlertHeadlineWidgetDefinition definition, AlertInfo alertInfo, WidgetTokenProperties tokenProperties, IntegrationTarget integrationTarget)
        {
            AlertAssetOperationalData opData = new AlertAssetOperationalData(DisseminationMethod.Widget);
            opData.AssetId = alertInfo.Id.ToString();
            opData.AssetName = alertInfo.Name;
            opData.AlertType = MapProductType(alertInfo.Type);
            opData.AudienceOption = MapDistributionTypeToDisseminationOption(definition.DistributionType);

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
                        referrer = referrer.Substring(0, MAX_REFERRER_SIZE);
                    }
                    return referrer;
            }
        }

        /// <summary>
        /// Gets the TTL proxy token for view all.
        /// </summary>
        /// <param name="alertId">The alert id.</param>
        /// <param name="fsr">The FSR.</param>
        /// <param name="TTLProxyToken">The TTL proxy token.</param>
        /// <returns></returns>
        private static string GetTTLProxyTokenForChartItemLink(string alertId, string fsr, string TTLProxyToken)
        {
            Encryption encryption = new Encryption();
            NameValueCollection nvp = new NameValueCollection(2);
            nvp.Add("fid", alertId);
            nvp.Add("proxyxsid", TTLProxyToken);
            return encryption.encrypt(nvp, EID4_TTL_PROXY_KEY);
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
            return encryption.encrypt(nvp, EXTERNAL_READER_PUBLIC_KEY);
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

    }
}