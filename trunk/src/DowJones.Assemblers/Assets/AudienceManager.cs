// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AudienceManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AudienceManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Specialized;
using DowJones.Managers.Abstract;
using DowJones.Managers.Assets;
using DowJones.Properties;
using DowJones.Session;
using DowJones.Url;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Utils.V1_0;
using FactivaEncryption;
using log4net;

namespace DowJones.Assemblers.Assets
{
    /// <summary>
    /// 
    /// </summary>
    public class AudienceManager : AbstractAggregationManager
    {
        private static readonly ILog ILog = LogManager.GetLogger(typeof(AudienceManager));
        private const string EID4_TTL_PROXY_KEY = "FRGKA8384";
        private const string ExternalReaderPublicKey = "3x4e10e4";

        /// <summary>
        /// Initializes a new instance of the <see cref="AudienceManager"/> class.
        /// </summary>
        /// <param name="sessionID">The session ID.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLangugage">The interface langugage.</param>
        public AudienceManager(string sessionID, string clientTypeCode, string accessPointCode, string interfaceLangugage) : base(sessionID, clientTypeCode, accessPointCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudienceManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public AudienceManager(IControlData controlData, string interfaceLanguage) : base(controlData)
        {
        }
           
        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return ILog; }
        }

        /// <summary>
        /// Cyclone Info
        /// </summary>
        public class CycloneInfo
        {
            /// <summary>
            /// Accession Number
            /// </summary>
            public string AccessionNumber;

            /// <summary>
            /// Folder Id
            /// </summary>
            public int FolderId;

            /// <summary>
            /// Folder Name
            /// </summary>
            public string FolderName;

            /// <summary>
            /// Content Type
            /// </summary>
            public ContentType ContentType;

            /// <summary>
            /// </summary>
            /// NAPC value
            public string AccessPointCode;

        }

        /// <summary>
        /// ContentType Enumeration
        /// </summary>
        public enum ContentType
        {
            /// <summary>
            /// Article
            /// </summary>
            Article,

            /// <summary>
            /// Webpage
            /// </summary>
            Webpage,

            /// <summary>
            /// Url
            /// </summary>
            Url,

            /// <summary>
            /// Pdf
            /// </summary>
            Pdf,

            /// <summary>
            /// File
            /// </summary>
            File,

            /// <summary>
            /// Picture
            /// </summary>
            Picture,

            /// <summary>
            /// Multimedia
            /// </summary>
            Multimedia,

            /// <summary>
            /// CoInfo
            /// </summary>
            CoInfo,

            /// <summary>
            /// Analyst
            /// </summary>
            Analyst,

            /// <summary>
            /// Internal
            /// </summary>
            Internal,

            /// <summary>
            /// Blog
            /// </summary>
            Blog,

            /// <summary>
            /// Board
            /// </summary>
            Board,
        }

        /// <summary>
        /// Gets the dictionary for cyclone article view.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="audience">The audience.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <returns></returns>
        public static QueryStringDictionary GetDictionaryForTrackArticleCycloneLink(string accessionNumber, Audience audience, UserProperties tokenProperties)
        {
            var qsd = new QueryStringDictionary();
            switch (audience.AudienceOptions)
            {
                case AudienceOptions.InternalAccount:
                    qsd.Append("aid", tokenProperties.AccountId);
                    qsd.Append("ns", tokenProperties.NameSpace);
                    qsd.Append("p", "sta");
                    break;
                case AudienceOptions.TimeToLive_Proxy:
                    qsd.Append("p", "sta");
                    if (audience.ProxyCredentials != null && !string.IsNullOrEmpty(audience.ProxyCredentials.EncryptedToken))
                    {
                        qsd.Append("eid4", GetTTLProxyTokenForArticleView(accessionNumber, audience.ProxyCredentials.EncryptedToken));
                    }
                    break;
                case AudienceOptions.ExternalReader:
                    qsd.Append("p", "er");
                    qsd.Append("f", "s");
                    if (!string.IsNullOrEmpty(audience.ProfileId))
                    {
                        string externalReaderToken = GetEncryptedExternalReaderToken(audience.ProfileId, tokenProperties);
                        qsd.Append("erc", externalReaderToken);
                    }
                    break;
                case AudienceOptions.OutsideAccount:
                    qsd.Append("p", "sta");
                    // do nothing here
                    break;
            }
            return qsd;
        }

        /// <summary>
        /// Gets the dictionary for cyclone article view.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="audience">The audience.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <returns></returns>
        public static QueryStringDictionary GetDictionaryForNonTrackArticleCycloneLink(string accessionNumber, Audience audience, UserProperties tokenProperties)
        {
            var qsd = new QueryStringDictionary();
            switch (audience.AudienceOptions)
            {
                case AudienceOptions.InternalAccount:
                    qsd.Append("aid", tokenProperties.AccountId);
                    qsd.Append("ns", tokenProperties.NameSpace);
                    qsd.Append("p", "sa");
                    break;
                case AudienceOptions.TimeToLive_Proxy:
                    qsd.Append("p", "sa");
                    if (audience.ProxyCredentials != null && !string.IsNullOrEmpty(audience.ProxyCredentials.EncryptedToken))
                    {
                        qsd.Append("eid4", GetTTLProxyTokenForArticleView(accessionNumber, audience.ProxyCredentials.EncryptedToken));
                    }
                    break;
                case AudienceOptions.ExternalReader:
                    qsd.Append("p", "er");
                    qsd.Append("f", "s");
                    if (!string.IsNullOrEmpty(audience.ProfileId))
                    {
                        var externalReaderToken = GetEncryptedExternalReaderToken(audience.ProfileId, tokenProperties);
                        qsd.Append("erc", externalReaderToken);
                    }
                    break;
                case AudienceOptions.OutsideAccount:
                    qsd.Append("p", "sa");
                    // do nothing here
                    break;
            }
            return qsd;
        }

        /// <summary>
        /// Gets the dictionary for TTL cyclone link.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="encryptedCredentials">The encrypted credentials.</param>
        /// <returns></returns>
        public static QueryStringDictionary GetDictionaryForTTLCycloneLink(string accessionNumber, string encryptedCredentials)
        {
            var qsd = new QueryStringDictionary();
            qsd.Append("p", "sa");
            qsd.Append("eid4", GetTTLProxyTokenForArticleView(accessionNumber, encryptedCredentials));
            return qsd;
        }

        /// <summary>
        /// Gets the dictionary for alert view all cyclone link.
        /// </summary>
        /// <param name="alertId">The alert id.</param>
        /// <param name="audience">The audience.</param>
        /// <param name="tokenProperties">The token properties.</param>
        /// <returns></returns>
        public static QueryStringDictionary GetDictionaryForAlertViewAllCycloneLink(string alertId, Audience audience, UserProperties tokenProperties)
        {
            var qsd = new QueryStringDictionary();
            switch (audience.AudienceOptions)
            {
                case AudienceOptions.InternalAccount:
                    qsd.Append("p", "vf");
                    qsd.Append("aid", tokenProperties.AccountId);
                    qsd.Append("ns", tokenProperties.NameSpace);
                    break;
                case AudienceOptions.TimeToLive_Proxy:
                    qsd.Append("p", "vf");
                    if (audience.ProxyCredentials != null && !string.IsNullOrEmpty(audience.ProxyCredentials.EncryptedToken))
                    {
                        qsd.Append("eid4", GetTTLProxyTokenForAlertViewAll(alertId, audience.ProxyCredentials.EncryptedToken));
                    }
                    break;
                case AudienceOptions.ExternalReader:
                    qsd.Append("p", "er");
                    qsd.Append("f", "s");
                    if (!string.IsNullOrEmpty(audience.ProfileId))
                    {
                        string externalReaderToken = GetEncryptedExternalReaderToken(audience.ProfileId, tokenProperties);
                        qsd.Append("erc", externalReaderToken);
                    }
                    break;
                case AudienceOptions.OutsideAccount:
                    // do nothing here
                    qsd.Append("p", "vf");
                    break;
            }
            return qsd;
        }

        /// <summary>
        /// Generates the cyclone URL for alert view all.
        /// </summary>
        /// <param name="cycloneInfo">The cyclone info.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="audienceOptionDictionary">The audience option dictionary.</param>
        /// <returns></returns>
        public static string GenerateCycloneUrlForAlertViewAllLink(CycloneInfo cycloneInfo, string accessPointCode, QueryStringDictionary audienceOptionDictionary)
        {
            var ub = new UrlBuilder
                         {
                             OutputType = UrlBuilder.UrlOutputType.Absolute,
                             BaseUrl = Settings.Default.BaseCycloneRedirectionURL,
                             BasicUtf8UrlEncoding = true
                         };
            ub.Append("fid", cycloneInfo.FolderId);
            ub.Append("fn", cycloneInfo.FolderName);
            ub.Append("ft", "g");
            ub.Append("napc", accessPointCode);

            // Add ep
            ub.Append("ep", "AL");
            // ub.Append("od", GetOperationalDataMemento(definition, alertInfo, tokenProperties, integrationTarget, "sa"));

            ub.Add(audienceOptionDictionary);

            return ub.ToString("p");
        }

        /// <summary>
        /// Generates the cyclone automatic workspace article link.
        /// </summary>
        /// <param name="cycloneInfo">The cyclone info.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="audienceOptionDictionary">The audience option dictionary.</param>
        /// <returns></returns>
        public static string GenerateCycloneUrlForArticleLink(CycloneInfo cycloneInfo, string accessPointCode, QueryStringDictionary audienceOptionDictionary)
        {
            var ub = new UrlBuilder
                         {
                             OutputType = UrlBuilder.UrlOutputType.Absolute,
                             BaseUrl = Settings.Default.BaseCycloneRedirectionURL,
                             BasicUtf8UrlEncoding = true
                         };

            ub.Append("an", cycloneInfo.AccessionNumber);
            ub.Append("napc", cycloneInfo.AccessPointCode);

            // Add ep
            //ub.Append("ep", "AL");
            // ub.Append("od", GetOperationalDataMemento(definition, workspaceInfo, tokenProperties, integrationTarget));

            // map category
            ub.Append("cat", MapContentTypeToCat(cycloneInfo.ContentType));

            ub.Add(audienceOptionDictionary);

            return ub.ToString("p");
        }

        /// <summary>
        /// Gets the encrypted external reader token.
        /// </summary>
        /// <param name="profileId">The profile id.</param>
        /// <param name="props">The props.</param>
        /// <returns></returns>
        private static string GetEncryptedExternalReaderToken(string profileId, UserProperties props)
        {
            // Use factiva encription to encode into a token name/value pairs
            var encryption = new encryption();
            var nvp = new NameValueCollection
                          {
                              { "ppid", props.NameSpace },
                              { "puid", props.UserId },
                              { "cpid", profileId }
                          };
            return encryption.encrypt(nvp, ExternalReaderPublicKey);
        }

        /// <summary>
        /// Gets the TTL proxy token for article view.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="ttlProxyToken">The TTL proxy token.</param>
        /// <returns>A well qualified url</returns>
        private static string GetTTLProxyTokenForArticleView(string accessionNumber, string ttlProxyToken)
        {
            var encryption = new encryption();
            var nvp = new NameValueCollection { { "an", accessionNumber }, { "proxyxsid", ttlProxyToken } };
            return encryption.encrypt(nvp, EID4_TTL_PROXY_KEY);
        }

        /// <summary>
        /// Gets the TTL proxy token for alert view all.
        /// </summary>
        /// <param name="alertId">The alert id.</param>
        /// <param name="ttlProxyToken">The TTL proxy token.</param>
        /// <returns></returns>
        private static string GetTTLProxyTokenForAlertViewAll(string alertId, string ttlProxyToken)
        {
            var encryption = new encryption();
            var nvp = new NameValueCollection { { "fid", alertId }, { "proxyxsid", ttlProxyToken }};
            return encryption.encrypt(nvp, EID4_TTL_PROXY_KEY);
        }

        /// <summary>
        /// Maps the content type to the content category.
        /// </summary>
        /// <param name="type">The content type enumeration.</param>
        /// <returns></returns>
        private static string MapContentTypeToCat(ContentType type)
        {
            switch (type.ToString().ToLower())
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
    }
}
