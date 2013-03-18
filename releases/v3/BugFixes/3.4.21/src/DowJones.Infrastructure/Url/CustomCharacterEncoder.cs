using System;
using System.Text;

namespace EMG.Utility.Url
{
    public class CustomCharacterEncoder : IQueryStringEncoder
    {
        #region IQueryStringEncoder Members

        /// <summary>
        /// The recommended prefix is a '~'
        /// </summary>
        public string Prefix
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Encodes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public string Encode(string token)
        {
            return Encode(new StringBuilder(token));
        }

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public string Decode(string token)
        {
            return Decode(new StringBuilder(token));
        }

        #endregion

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="tokenEncoder">The token decoder.</param>
        /// <returns></returns>
        public string Encode(StringBuilder tokenEncoder)
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
