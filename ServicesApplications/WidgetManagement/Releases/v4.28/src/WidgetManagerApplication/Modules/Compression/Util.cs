// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Util.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The util.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

namespace EMG.widgets.ui.Modules.Compression
{
    /// <summary>
    /// The utility class for compression.
    /// </summary>
    internal class Util
    {
        /// <summary>
        /// The _decrypt string.
        /// </summary>
        private static MethodInfo _decryptString;

        /// <summary>
        /// The _get method lock.
        /// </summary>
        private static readonly object _getMethodLock = new object();

        /// <summary>
        /// The _compressible types.
        /// </summary>
        private static readonly Dictionary<string, sbyte> _compressibleTypes = new Dictionary<string, sbyte>(5, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The _ ui page types.
        /// </summary>
        private static readonly Dictionary<string, sbyte> _UIPageTypes = new Dictionary<string, sbyte>(2, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes static members of the <see cref="Util"/> class.
        /// </summary>
        static Util()
        {
            if (_compressibleTypes.Count < 1)
            {
                _compressibleTypes.Add("text/css", 0);
                _compressibleTypes.Add("application/x-javascript", 0);
                _compressibleTypes.Add("text/javascript", 0);
                _compressibleTypes.Add("text/html", 0);
                _compressibleTypes.Add("text/plain", 0);
            }

            if (_UIPageTypes.Count >= 1)
            {
                return;
            }

            _UIPageTypes.Add("text/html", 0);
            _UIPageTypes.Add("text/plain", 0);
        }

        /// <summary>
        /// Check if the current request is an AsyncCall by MS-AJAX framework
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The is ms ajax request.</returns>
        internal static bool IsMsAjaxRequest(HttpContext context)
        {
            return context.Request.Headers["X-MicrosoftAjax"] != null;
        }

        /// <summary>
        /// Get the current "System.Web.Extensions" assembly version.
        /// </summary>
        /// <returns>
        /// <para>
        /// 0.0 - the assembly was not loaded
        /// </para>
        /// <para>
        /// bigger that 0.0 - the assembly version that loaded
        /// </para>
        /// </returns>
        internal static double GetMsAjaxVersion()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName == null)
                {
                    continue;
                }

                if (!asm.FullName.StartsWith("System.Web.Extensions", StringComparison.Ordinal))
                {
                    continue;
                }

                var versionIndex = asm.FullName.IndexOf("Version=", StringComparison.OrdinalIgnoreCase);

                // Not suppose to happen, but just to be sure..
                if (versionIndex < 0)
                {
                    return 0.0;
                }

                versionIndex += "Version=".Length;
                var commaIndex = asm.FullName.IndexOf(',', versionIndex);
                var ver = asm.FullName.Substring(versionIndex, commaIndex - versionIndex);
                while (ver.IndexOf('.') < ver.LastIndexOf('.'))
                {
                    ver = ver.Remove(ver.LastIndexOf('.'), 1);
                }

                return Convert.ToDouble(ver);
            }

            return 0.0;
        }

        /// <summary>
        /// Check if a specific content type is compressible
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The is content type compressible.</returns>
        public static bool IsContentTypeCompressible(string contentType)
        {
            return _compressibleTypes.ContainsKey(contentType);
        }

        /// <summary>
        /// Check if the specified content type is System.Web.UI.Page
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The is ui page content type.</returns>
        public static bool IsUIPageContentType(string contentType)
        {
            return _UIPageTypes.ContainsKey(contentType);
        }

        /// <summary>
        /// Decript a string using MachineKey
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The decrypt string.</returns>
        [ReflectionPermission(SecurityAction.Assert, Unrestricted = true),
         SecurityCritical,
         SecurityTreatAsSafe]
        internal static string DecryptString(string input)
        {
            try
            {
                if (_decryptString == null)
                {
                    lock (_getMethodLock)
                    {
                        if (_decryptString == null)
                        {
                            _decryptString = typeof(Page).GetMethod("DecryptString", BindingFlags.Static | BindingFlags.NonPublic);
                        }
                    }
                }

                return (string)_decryptString.Invoke(null, new object[] { input });
            }
            catch (MethodAccessException) 
            {
                // Reflection is not allowded!
                return EmptyMembership.Instance.DecryptString(input);
            }
            catch (TargetInvocationException)
            {
                return EmptyMembership.Instance.DecryptString(input);
            }
        }

        /// <summary>
        /// copy one stream to another
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        internal static void StreamCopy(Stream input, Stream output)
        {
            var buffer = new byte[1024];
            int read;
            do
            {
                read = input.Read(buffer, 0, buffer.Length);
                output.Write(buffer, 0, read);
            }
            while (read > 0);
        }

        /// <summary>
        /// Combine two hash codes (From class: 'HashCodeCombiner' in the assembly: 'System.Web.Util')
        /// </summary>
        /// <param name="hash1">The hash1.</param>
        /// <param name="hash2">The hash2.</param>
        /// <returns>The combine hash codes.</returns>
        internal static int CombineHashCodes(int hash1, int hash2)
        {
            if (hash2 == 0)
            {
                return hash1;
            }

            return ((hash1 << 5) + hash1) ^ hash2;
        }

        /// <summary>
        /// Get the file name from a path string
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <returns>
        /// The get file name.
        /// </returns>
        internal static string GetFileName(string url)
        {
            return Path.GetFileName(url);
        }


        /// <summary>
        /// Get the extension of the specified filename
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <returns>
        /// The get file extension.
        /// </returns>
        internal static string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        /// <summary>
        /// Convert string to byte[]
        /// <para>
        /// Faster than the built-in method, and prevent encoding problems
        /// </para>
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] StringToBytes(string value)
        {
            var length = value.Length;
            var resultBytes = new byte[length];
            for (var i = 0; i < length; i++)
            {
                resultBytes[i] = (byte)value[i];
            }

            return resultBytes;
        }


        /// <summary>
        /// Get the current folder of the current request
        /// </summary>
        /// <param name="context">
        /// </param>
        /// <returns>
        /// The get current path.
        /// </returns>
        internal static string GetCurrentPath(HttpContext context)
        {
            var index = context.Request.CurrentExecutionFilePath.LastIndexOf('/');
            if (index > -1)
            {
                return context.Request.CurrentExecutionFilePath.Substring(0, index + 1);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Encode giben string for using in the querystring
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// The url encode.
        /// </returns>
        internal static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }
    }
}
