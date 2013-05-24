// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AESEncryption.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DowJones.Security
{
    /// <summary>
    /// Utility class that handles encryption
    /// </summary>
    public static class AESEncryption
    {
        /// <summary>
        /// Encrypts the sourceString, returns this result as an AES encrypted, BASE64 encoded string
        /// </summary>
        /// <param name="plainSourceStringToEncrypt">a plain, Framework string (UTF-8, null terminated)</param>
        /// <param name="passPhrase">The pass phrase.</param>
        /// <returns>
        /// returns an AES encrypted, BASE64 encoded string
        /// </returns>
        public static string Encrypt(string plainSourceStringToEncrypt, string passPhrase)
        {
            // Set up the encryption objects
            using (var acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                var sourceBytes = Encoding.UTF8.GetBytes(plainSourceStringToEncrypt);
                var ictE = acsp.CreateEncryptor();

                byte[] encryptedBytes; 

                // Set up stream to contain the encryption
                using (var msS = new MemoryStream())
                {
                    // Perform the encryption, storing output into the stream
                    using (var csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write))
                    {
                        csS.Write(sourceBytes, 0, sourceBytes.Length);
                        csS.FlushFinalBlock();
                        
                        // sourceBytes are now encrypted as an array of secure bytes
                        encryptedBytes = msS.ToArray();  // .ToArray() is important, don't mess with the buffer
                    }
                }

                // return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Decrypts a BASE64 encoded string of encrypted data, returns a plain string
        /// </summary>
        /// <param name="base64StringToDecrypt">an AES encrypted AND base64 encoded string</param>
        /// <param name="passphrase">The passphrase.</param>
        /// <returns>returns a plain string</returns>
        public static string Decrypt(string base64StringToDecrypt, string passphrase)
        {
            // Set up the encryption objects
            using (var acsp = GetProvider(Encoding.UTF8.GetBytes(passphrase)))
            {
                var rawBytes = Convert.FromBase64String(base64StringToDecrypt);
                var ictD = acsp.CreateDecryptor();

                // RawBytes now contains original byte array, still in Encrypted state
                string decryptedData;

                // Decrypt into stream
                using (var memoryStream = new MemoryStream(rawBytes, 0, rawBytes.Length))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, ictD, CryptoStreamMode.Read))
                    {
                        decryptedData = (new StreamReader(cryptoStream)).ReadToEnd();
                    }
                }

                return decryptedData;
            }
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            var result = new AesCryptoServiceProvider
                             {
                                 BlockSize = 128, KeySize = 128, 
                                 Mode = CipherMode.CBC, 
                                 Padding = PaddingMode.PKCS7
                             };

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var realKey = GetKey(key, result);
            result.Key = realKey;
            return result;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            var rawKey = suggestedKey;
            var rawList = new List<byte>();

            for (var i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                rawList.Add(rawKey[(i / 8) % rawKey.Length]);
            }

            return rawList.ToArray();
        }
    }
}