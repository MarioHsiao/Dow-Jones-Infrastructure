// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AESEncryptionUtility.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DowJones.Utilities.Encoders;
using Utilities.Encryption;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
    public class AESEncryptionUtility
    {
        protected internal const string DefaultKey = "Factiva6969_IPAD";
        private const string KeyValueSeperator = "[#]";
        private const string KeyValueTerminator = "[|]";

        #region Implementation of IEncryption

        /// <summary>
        /// Encrypts the specified name value data.
        /// </summary>
        /// <param name="nameValueData">The name value data.</param>
        /// <param name="secretKey">The secretKey.</param>
        /// <param name="useCustomEncoder">if set to <c>true</c> [use custom encoder].</param>
        /// <returns>
        /// An encrypted string.
        /// </returns>
        public string Encrypt(NameValueCollection nameValueData, string secretKey = DefaultKey, bool useCustomEncoder = true)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < nameValueData.Count; i++)
            {
                sb.Append(string.Concat(nameValueData.Keys[i], KeyValueSeperator, nameValueData[i], KeyValueTerminator));
            }

            if (useCustomEncoder)
            {
                var encoder = new CustomCharacterEncoder();
                return encoder.Encode(AESEncryption.Encrypt(sb.ToString(), secretKey));
            }

            return AESEncryption.Encrypt(sb.ToString(), secretKey);
        }

        /// <summary>
        /// Encrypts the specified name value data.
        /// </summary>
        /// <param name="data">The data object.</param>
        /// <param name="secretKey">The secretKey.</param>
        /// <param name="useCustomEncoder">if set to <c>true</c> [use custom encoder].</param>
        /// <returns>
        /// An encrypted string.
        /// </returns>
        public string Encrypt(string data, string secretKey = DefaultKey, bool useCustomEncoder = true)
        {
            if (useCustomEncoder)
            {
                var encoder = new CustomCharacterEncoder();
                return encoder.Encode(AESEncryption.Encrypt(data, secretKey));
            }

            return AESEncryption.Encrypt(data, secretKey);
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data object.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="useCustomEncoder">if set to <c>true</c> [use custom encoder].</param>
        /// <returns>A name value collection</returns>
        public NameValueCollection DecryptToNameValueCollection(string data, string secretKey = DefaultKey, bool useCustomEncoder = true)
        {
            var nameValueCollection = new NameValueCollection();

            var decryptedData = Decrypt(data, secretKey, useCustomEncoder);

            if (!string.IsNullOrEmpty(decryptedData) && !string.IsNullOrEmpty(decryptedData.Trim()))
            {
                var values = decryptedData.Split(new[] { KeyValueTerminator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var nv in values.Select(t => t.Split(new[] { KeyValueSeperator }, StringSplitOptions.RemoveEmptyEntries)))
                {
                    nameValueCollection.Add(nv[0], nv[1]);
                }
            }

            return nameValueCollection;
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data object.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="useCustomEncoder">if set to <c>true</c> [use custom encoder].</param>
        /// <returns>A string of decrypted data</returns>
        public string Decrypt(string data, string secretKey = DefaultKey, bool useCustomEncoder = true)
        {
            string decryptedData;
            if (useCustomEncoder)
            {
                var encoder = new CustomCharacterEncoder();
                decryptedData = AESEncryption.Decrypt(encoder.Decode(data), secretKey);
            }
            else
            {
                decryptedData = AESEncryption.Decrypt(data, secretKey);
            }

            return decryptedData;
        }

        protected internal string GetStringifiedNameValueCollection(NameValueCollection nameValueData)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < nameValueData.Count; i++)
            {
                sb.Append(string.Concat(nameValueData.Keys[i], KeyValueSeperator, nameValueData[i], KeyValueTerminator));
            }
            return sb.ToString();
        }
        #endregion
    }
}
 