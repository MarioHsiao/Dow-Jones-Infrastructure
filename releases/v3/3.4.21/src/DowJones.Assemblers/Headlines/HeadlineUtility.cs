using System.Linq;
using DowJones.Ajax.HeadlineList;
using DowJones.DTO.Web.Request;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Preferences;
using DowJones.Session;
using DowJones.Url;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.Headlines
{
    public class HeadlineUtility
    {
        private readonly IControlData _controlData;
        private readonly IPreferences _preferences;
        public const string DefaultFileHandlerUrl = "~/DowJones.Web.Handlers.Article.ContentHandler.ashx";

        public ImageType ImageType { get; set; }

        /// <summary>
        /// Gets or sets the FileHandlerUrl.
        /// </summary>
        /// <value>
        /// The file handler URL.
        /// </value>
        public string FileHandlerUrl
        {
            get;
            set;
        }                                                                                                 

        public HeadlineUtility(IControlData controlData, IPreferences preferences)
        {
            _controlData = controlData;
            _preferences = preferences;
            FileHandlerUrl = DefaultFileHandlerUrl;
        }

        public void GetThumbNail(ThumbnailImage image, ContentHeadline contentHeadline)
        {
            if (contentHeadline.ContentItems.ItemCollection == null || contentHeadline.ContentItems.ItemCollection.Count <= 0) return;
            foreach( var item in contentHeadline.ContentItems.ItemCollection.Where( item => ( string.IsNullOrEmpty( item.Mimetype ) && item.Type.ToLower() == "dispix" && item.Size.ToLower() == "0" && item.Subtype.ToLower() == "primary" && !string.IsNullOrEmpty( item.Ref ) ) ) )
            {
                image.URI = item.Ref;
                image.SRC = GetHandlerUrl(ImageType, contentHeadline.AccessionNo, item, true );
                return;
            }
        }


        /// <summary>
        /// The get handler url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="contentItem">The content item.</param>
        /// <param name="isBlob">if set to <c>true</c> [is BLOB].</param>
        /// <returns>
        /// A string representing the Handerl Url
        /// </returns>
        private string GetHandlerUrl( ImageType imageType, string accessionNo, ContentItem contentItem, bool isBlob = false )
        {
            var reference = contentItem.Ref;
            var mimeType = contentItem.Mimetype;

            if( FileHandlerUrl.HasValue() )
            {
                var ub = new UrlBuilder( FileHandlerUrl );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "AccessionNumber" ), accessionNo );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "Reference" ), reference );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "MimeType" ), mimeType );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "ImageType" ), Map(imageType));
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "IsBlob" ), ( isBlob ) ? "y" : "" );

                if( !string.IsNullOrEmpty( _controlData.AccessPointCode ) )
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "AccessPointCode" ), _controlData.AccessPointCode );
                }

                if( !string.IsNullOrEmpty( _preferences.InterfaceLanguage ) )
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "InterfaceLanguage" ), _preferences.InterfaceLanguage );
                }

                if( !string.IsNullOrEmpty( _controlData.ProductID ) )
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "ProductID" ), _controlData.ProductID );
                }

                if( !string.IsNullOrEmpty( _controlData.SessionID ) )
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "SessionID" ), _controlData.SessionID );
                }
                else if( !string.IsNullOrEmpty( _controlData.EncryptedToken ) ) // assume this is a lightweight user
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "EncryptedToken" ), _controlData.EncryptedToken );
                }
                else
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "UserID" ), _controlData.UserID );
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "Password" ), _controlData.UserPassword );
                }

                if( !string.IsNullOrEmpty( _controlData.AccessPointCodeUsage ) )
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "AccessPointCodeUsage" ), _controlData.AccessPointCodeUsage );
                }

                if( !string.IsNullOrEmpty( _controlData.CacheKey ) )
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "CacheKey" ), _controlData.CacheKey );
                }

                if( !string.IsNullOrEmpty( _controlData.ClientCode ) )
                {
                    ub.Append( UrlBuilder.GetParameterName( typeof( SessionRequestDTO ), "ClientCodeType" ), _controlData.ClientCode );
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
    }
}