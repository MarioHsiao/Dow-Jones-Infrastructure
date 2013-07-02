using System.Linq;
using DowJones.Ajax;
using DowJones.DTO.Web.Request;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Preferences;
using DowJones.Prod.X.Core.Interfaces;
using DowJones.Session;
using DowJones.Url;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Prod.X.Core.Services.Utilities
{
    public class HeadlineUtilityService : IHeadlineUtilityService
    {
        public const string DefaultFileHandlerUrl = "~/DowJones.Web.Handlers.Article.ContentHandler.ashx";
        private readonly IControlData _controlData;
        private readonly IPreferences _preferences;


        public HeadlineUtilityService(IControlData controlData, IPreferences preferences)
        {
            _controlData = controlData;
            _preferences = preferences;
        }

        public string GenerateUrl(ContentHeadline contentHeadline, bool isDuplicate)
        {
            // link to an action and send parameters 
            if (contentHeadline == null || contentHeadline.ContentItems == null)
            {
                return null;
            }

            var contentType = contentHeadline.ContentItems.ContentType.ToLower();
            var contentCategory = GetContentCategory(contentType);

            // look through based on type and provide the correct Uri
            switch (contentCategory)
            {
                case ContentCategory.Website:
                    foreach (
                        var item in contentHeadline.ContentItems.ItemCollection.Where(
                                item =>
                                (item.Type.IsNotEmpty() &&
                                 item.Type.Trim().IsNotEmpty() &&
                                 item.Type.ToLower() == "webpage")))
                    {
                        return item.Ref;
                    }
                    break;
                case ContentCategory.Blog:
                case ContentCategory.Board:
                case ContentCategory.CustomerDoc:
                case ContentCategory.Internal:
                    foreach (var item in contentHeadline.ContentItems.ItemCollection.Where(item => item.Type.ToLower() == "webpage"))
                    {
                        return item.Ref;
                    }
                    break;
                case ContentCategory.Multimedia:
                    break;
                case ContentCategory.Publication:
                    if (contentType == "html" || contentType == "file" || contentType == "pdf")
                    {
                        var tItems = contentHeadline.ContentItems.ItemCollection.Where(tItem => !string.IsNullOrEmpty(tItem.Mimetype)).ToList();

                        foreach (
                            var strHref in
                                tItems.Select(
                                    contentItem =>
                                    GetHandlerUrl(ImageType.Display, contentHeadline.AccessionNo, contentItem))
                                      .Where(strHref => contentType == "pdf"))
                        {
                            return strHref;
                        }
                    }
                    break;
            }
            return string.Empty;
        }

        /// <summary>
        /// The get handler url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="contentItem">The content item.</param>
        /// <param name="isBlob">if set to <c>true</c> [is BLOB].</param>
        /// <returns>
        /// A string representing the .net handler for getting content
        /// </returns>
        public string GetHandlerUrl(ImageType imageType, string accessionNo, ContentItem contentItem, bool isBlob = false)
        {
            var reference = contentItem.Ref;
            var mimeType = contentItem.Mimetype;

            if (DefaultFileHandlerUrl.HasValue())
            {
                var ub = new UrlBuilder(DefaultFileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "ImageType"), Map(imageType));
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "IsBlob"), (isBlob) ? "y" : "");

                if (!_controlData.AccessPointCode.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), _controlData.AccessPointCode);
                }

                if (!_preferences.InterfaceLanguage.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), _preferences.InterfaceLanguage);
                }

                if (!_controlData.ProductID.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), _controlData.ProductID);
                }

                if (!_controlData.AccessPointCodeUsage.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), _controlData.AccessPointCodeUsage);
                }

                if (!_controlData.CacheKey.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "CacheKey"), _controlData.CacheKey);
                }

                if (!_controlData.ClientCode.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), _controlData.ClientCode);
                }

                if (!_controlData.SessionID.IsNullOrEmpty())
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), _controlData.SessionID);
                }
                else if (!_controlData.EncryptedToken.IsNullOrEmpty()) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), _controlData.EncryptedToken);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), _controlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), _controlData.UserPassword);
                }

                return ub.ToString();
            }
            return null;
        }

        private static string Map(ImageType imageType)
        {
            switch (imageType)
            {
                default:
                    return "dispix";
                case ImageType.Fingernail:
                    return "fnail";
                case ImageType.Thumbnail:
                    return "tnail";
            }
        }

        private static ContentCategory GetContentCategory(string contentType)
        {
            switch (contentType)
            {
                case "webpage":
                    return ContentCategory.Website;
                case "file":
                case "article":
                case "pdf":
                case "analyst":
                case "html":
                case "articlewithgraphics":
                    return ContentCategory.Publication;
                case "picture":
                    return ContentCategory.Picture;
                case "multimedia":
                    return ContentCategory.Multimedia;
                case "internal":
                    return ContentCategory.Internal;
                case "summary":
                    return ContentCategory.Summary;
                case "board":
                    return ContentCategory.Board;
                case "blog":
                    return ContentCategory.Blog;
                case "customerdoc":
                    return ContentCategory.CustomerDoc;
                default:
                    return ContentCategory.External;
            }
        }

    }
}
