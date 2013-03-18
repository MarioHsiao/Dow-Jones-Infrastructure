using System.ComponentModel;
using System.Linq;

namespace DowJones.Web
{
    public enum ClientResourceKind
    {
        [Description(KnownMimeTypes.Content)]
        Content,

        [Description(KnownMimeTypes.JavaScript)]
        Script,

        [Description(KnownMimeTypes.Stylesheet)]
        Stylesheet,

        [Description(KnownMimeTypes.Html)]
        ClientTemplate
    }

    public static class ClientResourceKindExtensions
    {
        public static string GetMimeType(this ClientResourceKind resourceKind)
        {
            string contentType = KnownMimeTypes.Content;

            DescriptionAttribute contentTypeAttribute =
                typeof(ClientResourceKind)
                    .GetField(resourceKind.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .Cast<DescriptionAttribute>()
                    .FirstOrDefault();

            if (contentTypeAttribute != null && !string.IsNullOrWhiteSpace(contentTypeAttribute.Description))
                contentType = contentTypeAttribute.Description;

            return contentType;
        }
    }
}