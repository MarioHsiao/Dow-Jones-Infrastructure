// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Encryption.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Summary description for encryption.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DowJones.Encoders;
using DowJones.Exceptions;
using DowJones.Properties;

namespace DowJones.Security
{
    /// <summary>
    /// Summary description for encryption.
    /// </summary>
    internal interface IEncryption
    {
        string Encrypt(NameValueCollection nameValueData, string secretKey);

        NameValueCollection Decrypt(string data, string key);
    }

    public class Encryption : IEncryption
    {
       public const string EncryptionVersion = "2";
       private const char Delimiter = '|';

        public string Encrypt(NameValueCollection nameValueData, string secretKey)
        {
            string encryptedData;
            switch (EncryptionVersion)
            {
                case "2":
                    var e = new Encryption_2_0();
                    encryptedData = e.Encrypt(nameValueData, secretKey);
                    //// add version at the end
                    encryptedData = encryptedData + "|" + EncryptionVersion;
                    break;
            }

            return encryptedData;
        }

        public NameValueCollection Decrypt(string data, string key)
        {
            return Decrypt(data, key, null);
        }

        public NameValueCollection Decrypt(string data, string key, NameValueCollection nameValueInputData)
        {
            var tmpData = EncryptionUtilities.Reverse(data);
            var version = EncryptionUtilities.Left(tmpData, tmpData.IndexOf(Delimiter));
            tmpData = EncryptionUtilities.Right(tmpData, tmpData.Length - (tmpData.IndexOf(Delimiter) + 1));
            data = EncryptionUtilities.Reverse(tmpData);

            var nameValueCollection = new NameValueCollection();
            switch (version)
            {
                case "2":
                    var e = new Encryption_2_0();
                    if ((nameValueInputData == null) || (nameValueInputData.Keys.Count == 0))
                    {
                        nameValueCollection = e.Decrypt(data, key);
                    }
                    else
                    {
                        nameValueCollection = e.Decrypt(data, key);
                        if (!Validate(nameValueCollection, nameValueInputData))
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.EncryptionUtilitiesValidationException);
                        }
                    }

                    break;
            }

            return nameValueCollection;
        }

        /// <summary>
        /// Validates the specified name/value decrypted input.
        /// </summary>
        /// <param name="namValueDecryptedInput">The name/value decrypted input.</param>
        /// <param name="nameValueInputData">The name/value input data.</param>
        /// <returns>A Boolean value.</returns>
        public bool Validate(NameValueCollection namValueDecryptedInput, NameValueCollection nameValueInputData)
        {
            foreach (string key in nameValueInputData.Keys)
            {
                if (nameValueInputData[key] == null)
                {
                    return false;
                }

                if (namValueDecryptedInput[key] == null)
                {
                    return false;
                }

                if (nameValueInputData[key] != namValueDecryptedInput[key])
                {
                    return false;
                }
            }

            return true;
        }
    }

    internal class Encryption_2_0 : IEncryption
    {
        private static readonly CustomCharacterEncoder Encoder = new CustomCharacterEncoder();

        public string Encrypt(NameValueCollection nameValueData, string secretKey)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < nameValueData.Count; i++)
            {
                sb.Append(nameValueData.Keys[i] + "#" + nameValueData[i] + "|");
            }

            return EncryptData(sb.ToString(), secretKey);
        }

        public NameValueCollection Decrypt(string data, string key)
        {
            var nameValueCollection = new NameValueCollection();

            var tempData = data.Replace(" ", "+");
            var decryptedData = DecryptData(tempData, key);
            if (!string.IsNullOrEmpty(decryptedData) && !string.IsNullOrEmpty(decryptedData.Trim()))
            {
                var values = decryptedData.Split('|');

                for (var i = 0; i < values.Length - 1; i++)
                {
                    var nv = values[i].Split('#');
                    nameValueCollection.Add(nv[0], nv[1]);
                }
            }

            return nameValueCollection;
        }

        private static string EncryptData(string data, string key)
        {
            key = "!$?" + key + "#$a54?3?&$908";
            byte[] iv = { 10, 20, 30, 40, 50, 60, 70, 80 };

            try
            {
                var encryptionKey = Encoding.UTF8.GetBytes(key.Substring(0, 16));
                var des = new TripleDESCryptoServiceProvider
                              {
                                  Mode = CipherMode.CBC
                              };

                var inputByteArray = Encoding.UTF8.GetBytes(data); // Convert.ToByte(stringToEncrypt.Length)
                byte[] cipherTextBytes;
                using (var encryptor = des.CreateEncryptor(encryptionKey, iv))
                {
                    using (var memStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memStream.ToArray();
                            memStream.Close();
                            cryptoStream.Close();
                        }
                    }
                }

                des.Clear();
                return Settings.Default.Encryption_UseCustomEncoding ? Encoder.Encode(Convert.ToBase64String(cipherTextBytes)) : Convert.ToBase64String(cipherTextBytes); 
             }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.EncryptionUtilitiesEncryptingException);
            }
        }

        private static string DecryptData(string cipherText, string key)
        {
            byte[] iv = { 10, 20, 30, 40, 50, 60, 70, 80 };
            key = "!$?" + key + "#$a54?3?&$908";

            try
            {
                var decryptionKey = Encoding.UTF8.GetBytes(key.Substring(0, 16));
                var des = new TripleDESCryptoServiceProvider
                              {
                                  Mode = CipherMode.CBC
                              };

                byte[] cipherTextBytes;
                try
                {
                    cipherTextBytes = Convert.FromBase64String(Settings.Default.Encryption_UseCustomEncoding ? Encoder.Decode(cipherText) : cipherText);
                }
                catch (Exception)
                {
                    if (Settings.Default.Encryption_UseCustomEncoding)
                    {
                        cipherTextBytes = Convert.FromBase64String(cipherText);
                    }
                    else
                    {
                        throw;
                    }
                }

                var plainTextBytes = new byte[cipherText.Length];
                int byteCount;

                using (var decryptor = des.CreateDecryptor(decryptionKey, iv))
                {
                    using (var memStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                        {
                            byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            memStream.Close();
                            cryptoStream.Close();
                        }
                    }
                }

                des.Clear();
                return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.EncryptionUtilitiesDecryptingException);
            }
        }
    }

    internal class EncryptionUtilities
    {
        public static string Left(string str, int length)
        {
            try
            {
                return length > 0 ? str.Substring(0, length) : str;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.EncryptionUtilitiesDecryptingException);
            }
        }

        /// <summary>
        /// Rights the specified string parameter.
        /// </summary>
        /// <param name="str">The string parameter.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string object</returns>
        public static string Right(string str, int length)
        {
            try
            {
                return length > 0 ? str.Substring(str.Length - length, length) : str;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.EncryptionUtilitiesDecryptingException);
            }
        }

        // Function to Reverse the String
        public static string Reverse(string str)
        {
            if (str.Length == 1)
            {
                return str;
            }

            return Reverse(str.Substring(1)) + str.Substring(0, 1);
        }
    }
}