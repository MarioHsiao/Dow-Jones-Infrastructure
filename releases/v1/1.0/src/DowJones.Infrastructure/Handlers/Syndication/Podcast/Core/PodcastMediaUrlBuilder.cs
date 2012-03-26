using System;
using System.Text;
using EMG.Utility.Uri;

namespace EMG.Utility.Handlers.Syndication.Podcast.Core 
{

    /// <summary>
    /// 
    /// </summary>
    public class PodcastMediaUrlBuilder : UrlBuilder
    {
        // private static readonly int m_SegmentSize = 20;
        private static readonly string m_StartSegment = "_SOT";
        private static readonly string m_EndSegment = "_EOT";
        private const string m_DefaultFolderPath = "~/{0}.mp3";

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastMediaUrlBuilder"/> class.
        /// </summary>
        /// <param name="accessionNumber">The accession number.</param>
        /// <param name="token">The token.</param>
        /// <param name="folderPath">The folder path.</param>
        public PodcastMediaUrlBuilder(string accessionNumber, string token, string folderPath)
        {
            string path = m_DefaultFolderPath;
            if (!string.IsNullOrEmpty(folderPath) && !string.IsNullOrEmpty(folderPath.Trim()))
            {
                if (folderPath.EndsWith("/"))
                    path = string.Concat(folderPath, "{0}.mp3");
                else
                    path = string.Concat(folderPath, "/{0}.mp3");
            }

            OutputType = UrlOutputType.Absolute;
            BaseUrl = string.Format(path, Encode(token), accessionNumber);
        }

        public PodcastMediaUrlBuilder(string accessionNumber, string token) : this(accessionNumber,token,string.Empty)
        {
        }

        /// <summary>
        /// Encodes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static string Encode(string token)
        {
            /*StringBuilder sb = new StringBuilder(token);
            string tempToken = Encode(sb);
            StringBuilder tokenEncoder = new StringBuilder();
            if (!string.IsNullOrEmpty(tempToken) && !string.IsNullOrEmpty(tempToken.Trim()))
            {
                tokenEncoder.Append(m_StartSegment).Append("/");
                for (int index = 0; index < tempToken.Length; index = index + m_SegmentSize)
                {
                    if (index + m_SegmentSize < tempToken.Length)
                    {
                        tokenEncoder.Append(tempToken.Substring(index, m_SegmentSize));
                        tokenEncoder.Append("/");
                    }
                    else
                    {
                        tokenEncoder.Append(tempToken.Substring(index));
                    }
                }
                tokenEncoder.Append("/").Append(m_EndSegment);
            }
            return tokenEncoder.ToString();*/
            return Encode(new StringBuilder(token));
        }

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="tokenEncoder">The token decoder.</param>
        /// <returns></returns>
        private static string Encode(StringBuilder tokenEncoder)
        {
            if (tokenEncoder == null || tokenEncoder.Length == 0)
                return string.Empty;

            tokenEncoder.Replace("/", "_2F");
            tokenEncoder.Replace("$", "_24");
            tokenEncoder.Replace("&", "_26");
            tokenEncoder.Replace("+", "_2B");
            tokenEncoder.Replace(",", "_2C");
            tokenEncoder.Replace(":", "_3A");
            tokenEncoder.Replace(";", "_3B");
            tokenEncoder.Replace("=", "_3D");
            tokenEncoder.Replace("?", "_3F");
            tokenEncoder.Replace("@", "_40");
            tokenEncoder.Replace(" ", "_20");
            tokenEncoder.Replace("|", "_7C");

            return tokenEncoder.ToString();
        }


        /// <summary>
        /// Decodes the specified URL.
        /// </summary>
        /// <param name="uri">The URL.</param>
        /// <returns></returns>
        private static string Decode(System.Uri uri)
        {
            StringBuilder tokenDecoder = new StringBuilder();
            bool process = false;
            foreach (string segment in uri.Segments)
            {
                if (segment != "/")
                {
                    if (segment.Equals(m_EndSegment, StringComparison.InvariantCultureIgnoreCase))
                    {
                        process = false;
                    }

                    // skip the start and end segments in the url
                    if (process)
                    {
                        tokenDecoder.Append(segment);
                    }


                    if (segment.Equals(m_StartSegment, StringComparison.InvariantCultureIgnoreCase))
                    {
                        process = true;
                    }
                }
            }
            return Decode(tokenDecoder);
        }

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static string Decode(string token)
        {
            return Decode(new StringBuilder(token));
        }

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="tokenDecoder">The token decoder.</param>
        /// <returns></returns>
        public static string Decode(StringBuilder tokenDecoder)
        {
            if (tokenDecoder == null || tokenDecoder.Length == 0)
                return string.Empty;

            tokenDecoder.Replace("_2F", "/");
            tokenDecoder.Replace("_24", "$");
            tokenDecoder.Replace("_26", "&");
            tokenDecoder.Replace("_2B", "+");
            tokenDecoder.Replace("_2C", ",");
            tokenDecoder.Replace("_3A", ":");
            tokenDecoder.Replace("_3B", ";");
            tokenDecoder.Replace("_3D", "=");
            tokenDecoder.Replace("_3F", "?");
            tokenDecoder.Replace("_40", "@");
            tokenDecoder.Replace("_20", " ");
            tokenDecoder.Replace("_7C", "|");
            return tokenDecoder.ToString();
        }
    }
}
