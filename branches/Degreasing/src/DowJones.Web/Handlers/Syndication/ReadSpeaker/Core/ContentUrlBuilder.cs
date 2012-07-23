using DowJones.Properties;
using DowJones.Url;

namespace DowJones.Web.Handlers.Syndication.ReadSpeaker.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentUrlBuilder : UrlBuilder
    {
        internal const string QsParamToken = "tkn";
        private static readonly string ReadSpeakerContentHandler = Settings.Default.ReadSpeaker_ContentHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentUrlBuilder"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ContentUrlBuilder(string token) : base(ReadSpeakerContentHandler)
        {
            OutputType = UrlOutputType.Absolute;
            Append(QsParamToken, token);
        }
    }
}
