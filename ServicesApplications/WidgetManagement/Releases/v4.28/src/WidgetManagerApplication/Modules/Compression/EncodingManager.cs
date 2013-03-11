// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EncodingManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The encoding manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace EMG.widgets.ui.Modules.Compression
{
    /// <summary>
    /// The encoding manager.
    /// </summary>
    public sealed class EncodingManager
    {
        /// <summary>
        /// The _current context.
        /// </summary>
        private readonly HttpContext currentContext;

        /// <summary>
        /// The preferred encoding.
        /// </summary>
        private string preferredEncoding = string.Empty;

        /// <summary>
        /// The _preferred encoding type.
        /// </summary>
        private EncodingType preferredEncodingType = EncodingType.None;

        /// <summary>
        /// The _request header.
        /// </summary>
        private string requestHeader = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingManager"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public EncodingManager(HttpContext context)
        {
            currentContext = context;
            if (!string.IsNullOrEmpty(context.Request.Headers["Accept-encoding"]))
            {
                RequestHeader = context.Request.Headers["Accept-encoding"].Replace(" ", string.Empty);
            }

            ParsePreferrdedEncoding();
        }

        #region ..:: EncodingType enum ::..

        /// <summary>
        /// The encoding type.
        /// </summary>
        public enum EncodingType
        {
            /// <summary>
            /// No Encoding.
            /// </summary>
            None = 0,

            /// <summary>
            /// Gzip Encoding.
            /// </summary>
            Gzip = 1,

            /// <summary>
            /// Deflate encoding.
            /// </summary>
            Deflate = 2
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets RequestHeader.
        /// </summary>
        public string RequestHeader
        {
            get { return requestHeader; }
            private set { requestHeader = value; }
        }

        /// <summary>
        /// Gets or sets PreferredEncodingType.
        /// </summary>
        public EncodingType PreferredEncodingType
        {
            get
            {
                return preferredEncodingType;
            }

            set
            {
                preferredEncodingType = value;
                switch (value)
                {
                    case EncodingType.None:
                        preferredEncoding = string.Empty;
                        break;
                    case EncodingType.Gzip:
                        preferredEncoding = "gzip";
                        break;
                    case EncodingType.Deflate:
                        preferredEncoding = "deflate";
                        break;
                }
            }
        }

        /// <summary>
        /// Gets PreferredEncoding.
        /// </summary>
        public string PreferredEncoding
        {
            get { return preferredEncoding; }
            private set { preferredEncoding = value; }
        }

        /// <summary>
        /// Gets a value indicating whether IsEncodingEnabled.
        /// </summary>
        public bool IsEncodingEnabled
        {
            get { return PreferredEncodingType != EncodingType.None; }
        }

        #endregion

        #region Compression methods

        /// <summary>
        /// Add a compression filter to the response
        /// </summary>
        public void CompressResponse()
        {
            if (preferredEncodingType == EncodingType.Deflate)
            {
                currentContext.Response.Filter = new DeflateStream(currentContext.Response.Filter, CompressionMode.Compress);
            }
            else if (preferredEncodingType == EncodingType.Gzip)
            {
                currentContext.Response.Filter = new GZipStream(currentContext.Response.Filter, CompressionMode.Compress);
            }
        }

        /// <summary>
        /// Compress a given string using the preffered algorithm
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// A <see cref="T:System.Byte"/> array that contains the compressed string.
        /// </returns>
        public byte[] CompressString(string input)
        {
            return string.IsNullOrEmpty(input) ? null : CompressBytes(Util.StringToBytes(input));
        }

        /// <summary>
        /// Compress a given byte[] the preferred algorithm
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// A <see cref="T:System.Byte"/>   array that contains the compressed bytes.
        /// </returns>
        public byte[] CompressBytes(byte[] buffer)
        {
            if (!IsEncodingEnabled)
            {
                throw new NotSupportedException(ServiceResources.GetString(ServiceResources.COMPRESSION_NOT_SUPPORTED));
            }

            if (buffer != null && buffer.Length > 0)
            {
                using (var memStream = new MemoryStream())
                {
                    if (PreferredEncodingType == EncodingType.Gzip)
                    {
                        using (var compressStream = new GZipStream(memStream, CompressionMode.Compress))
                        {
                            compressStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        using (var compressStream = new DeflateStream(memStream, CompressionMode.Compress))
                        {
                            compressStream.Write(buffer, 0, buffer.Length);
                        }
                    }

                    return memStream.ToArray();
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Set the encoding type for the current response
        /// </summary>
        public void SetResponseEncodingType()
        {
            if (IsEncodingEnabled)
            {
                currentContext.Response.AppendHeader("Content-encoding", PreferredEncoding);
            }
        }

        #region Parsing methods methods

        /// <summary>
        /// Get the QValue from the string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The parse q value.</returns>
        private static float ParseQValue(string value)
        {
            var index = value.IndexOf("q=", StringComparison.OrdinalIgnoreCase);

            if (index < 0)
            {
                return 1;
            }

            float result;
            return float.TryParse(value.Substring(index + 2), out result) ? result : 0;
        }

        /// <summary>
        /// Check if the browser support compression
        /// </summary>
        /// <returns>The is compression supported.</returns>
        private bool IsCompressionSupported()
        {
            if (currentContext.Request.Browser == null)
            {
                return false;
            }

            // Check if the browser is not IE. If it is not, it safe to say it support compression
            if (!currentContext.Request.Browser.IsBrowser("IE"))
            {
                return true;
            }

            // If we are here, it means the client have IE. Sometimes IE have problems with proxys that using old protocol
            // 1.0, but still sending 'Accept-encoding' header as compressible.
            return currentContext.Request.Params["SERVER_PROTOCOL"] != null && currentContext.Request.Params["SERVER_PROTOCOL"].Contains("1.1");
        }

        /// <summary>
        /// Find the preferred encoding
        /// </summary>
        private void ParsePreferrdedEncoding()
        {
            var gzipIndex = -1;
            var deflateIndex = -1;
            float gzipQValue = 0;
            float deflateQValue = 0;
            float starQValue = 0;
            float identityQValue = 0;

            if (string.IsNullOrEmpty(requestHeader))
            {
                PreferredEncoding = string.Empty;
                preferredEncodingType = EncodingType.None;
                return;
            }

            var headersParts = requestHeader.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < headersParts.Length; i++)
            {
                if (gzipIndex < 0 && headersParts[i].Equals("gzip", StringComparison.OrdinalIgnoreCase))
                {
                    gzipIndex = i;
                    gzipQValue = ParseQValue(headersParts[i]);

                    // We found gzip QValue = 1, skip the rest...
                    if (gzipQValue == 1)
                    {
                        if (IsCompressionSupported())
                        {
                            PreferredEncoding = "gzip";
                            preferredEncodingType = EncodingType.Gzip;
                        }
                        else
                        {
                            PreferredEncoding = string.Empty;
                            preferredEncodingType = EncodingType.None;
                        }

                        return;
                    }
                }
                else if (deflateIndex < 0 && headersParts[i].Equals("deflate", StringComparison.OrdinalIgnoreCase))
                {
                    deflateIndex = i;
                    deflateQValue = ParseQValue(headersParts[i]);

                    // We found deflate QValue = 1, skip the rest...
                    if (deflateQValue == 1)
                    {
                        if (IsCompressionSupported())
                        {
                            PreferredEncoding = "deflate";
                            preferredEncodingType = EncodingType.Deflate;
                        }
                        else
                        {
                            PreferredEncoding = string.Empty;
                            preferredEncodingType = EncodingType.None;
                        }

                        return;
                    }
                }
                else if (headersParts[i].Equals("identity", StringComparison.OrdinalIgnoreCase))
                {
                    identityQValue = ParseQValue(headersParts[i]);
                }
                else if (headersParts[i].Equals("*", StringComparison.OrdinalIgnoreCase))
                {
                    starQValue = ParseQValue(headersParts[i]);
                }
            }

            // gzip if preferred
            if (gzipQValue > deflateQValue && gzipQValue > identityQValue)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = "gzip";
                    preferredEncodingType = EncodingType.Gzip;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    preferredEncodingType = EncodingType.None;
                }

                return;
            }

            // deflate is preferred
            if (deflateQValue > gzipQValue && deflateQValue > identityQValue)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = "deflate";
                    preferredEncodingType = EncodingType.Deflate;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    preferredEncodingType = EncodingType.None;
                }

                return;
            }

            // identity (no compression) is preferred
            if (identityQValue > gzipQValue && identityQValue > deflateQValue)
            {
                PreferredEncoding = string.Empty;
                preferredEncodingType = EncodingType.None;
                return;
            }

            // They both have the same QValue that bogger than 0, so the preferred is the first one to apear
            if (gzipQValue == deflateQValue && gzipQValue > 0)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = gzipIndex < deflateIndex ? "gzip" : "deflate";
                    preferredEncodingType = gzipIndex < deflateIndex ? EncodingType.Gzip : EncodingType.Deflate;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    preferredEncodingType = EncodingType.None;
                }

                return;
            }

            // Any encoding is accepteble. We will use deflate as the default
            if (starQValue > 0)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = "deflate";
                    preferredEncodingType = EncodingType.Deflate;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    preferredEncodingType = EncodingType.None;
                }

                return;
            }

            // No encoding is supported
            PreferredEncoding = string.Empty;
            preferredEncodingType = EncodingType.None;
            return;
        }

        #endregion
    }
}
