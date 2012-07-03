using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class StylesheetResourceImageUrlResolver : IClientResourceProcessor
    {
        private static readonly Regex UrlExpression =
            new Regex(@"url\(['""]?((?!https?://)[^)'""]*)['""]?\)",
                      RegexOptions.Compiled | RegexOptions.IgnoreCase |
                      RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        internal static string ApplicationPath
        {
            get { return applicationPath ?? HttpContext.Current.Request.ApplicationPath; }
            set { applicationPath = value; }
        }
        private static string applicationPath;


        public ClientResourceProcessorOrder? Order
        {
            get { return ClientResourceProcessorOrder.Last; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Preprocessor; }
        }

        public void Process(ProcessedClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");

            if (!resource.HasContent)
                return;

            if (resource.ClientResource.ResourceKind != ClientResourceKind.Stylesheet)
                return;

            if(resource.ClientResource is EmbeddedClientResource)
                return;

            string basePath = VirtualPathUtility.GetDirectory(resource.ClientResource.Url);
            
            if (basePath.StartsWith("~"))
                basePath = basePath.Substring(1);

            resource.Content =
                UrlExpression.Replace(
                    resource.Content,
                    match => ReplaceTokenMatch(match, basePath)
                );
        }

        private static string ReplaceTokenMatch(Match match, string basePath)
        {
            var styleSheetRelativePath = match.Groups[1].Value.Trim();

            if (styleSheetRelativePath.StartsWith("/"))
                styleSheetRelativePath = styleSheetRelativePath.Substring(1);

            var combinedPath = Path.Combine(basePath, styleSheetRelativePath);
            combinedPath = combinedPath.Replace('\\', '/');
            var relativePath = VirtualPathUtility.ToAbsolute(combinedPath, ApplicationPath);

            return string.Format("url('{0}')", relativePath);
        }
    }
}
