// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaviconUrlBuilder.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Uri;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
    internal class FaviconUrlBuilder : UrlBuilder
    {
        internal const string SizeParamToken = "size";
        internal const string UrlParamToken = "url";

        private static readonly string BaseFaviconHandler = Settings.Default.FaviconConverstionUrl;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FaviconUrlBuilder"/> class.
        /// </summary>
        /// <param name="size">The size of the image.</param>
        /// <param name="targetUrl">The target URL.</param>
        public FaviconUrlBuilder(IconSize size, string targetUrl)
            : base(BaseFaviconHandler)
        {
            OutputType = UrlOutputType.Absolute;
            Append(SizeParamToken, size.ToString().ToLowerInvariant());
            Append(UrlParamToken, targetUrl);
        }

        internal enum IconSize
        {
            /// <summary>
            /// Small icons
            /// </summary>
            Small,

            /// <summary>
            /// Medium icons
            /// </summary>
            Medium,

            /// <summary>
            /// Large Icons
            /// </summary>
            Large,
        }
    }
}
