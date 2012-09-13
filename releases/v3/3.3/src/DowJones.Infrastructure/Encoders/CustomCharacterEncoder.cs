// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomCharacterEncoder.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CustomCharacterEncoder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;

namespace DowJones.Encoders
{
    public class CustomCharacterEncoder : IEncoder
    {
        /// <summary>
        /// Gets the recommended prefix is a '~'
        /// </summary>
        public string Prefix
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="tokenDecoder">The token decoder.</param>
        /// <returns>A encoded string.</returns>
        public static string Decode(StringBuilder tokenDecoder)
        {
            if (tokenDecoder == null || tokenDecoder.Length == 0)
            {
                return string.Empty;
            }

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

        /// <summary>
        /// Encodes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>An encoded string</returns>
        public string Encode(string token)
        {
            return Encode(new StringBuilder(token));
        }

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>A decoded string</returns>
        public string Decode(string token)
        {
            return Decode(new StringBuilder(token));
        }

        /// <summary>
        /// Decodes the specified token.
        /// </summary>
        /// <param name="tokenEncoder">The token decoder.</param>
        /// <returns>A encoded string.</returns>
        public string Encode(StringBuilder tokenEncoder)
        {
            if (tokenEncoder == null || tokenEncoder.Length == 0)
            {
                return string.Empty;
            }

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
    }
}