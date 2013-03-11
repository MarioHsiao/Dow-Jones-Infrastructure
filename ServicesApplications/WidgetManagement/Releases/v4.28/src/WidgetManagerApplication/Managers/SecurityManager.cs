using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EMG.widgets.ui.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityManager
    {
        /// <summary>
        /// Encodes the specified parts.
        /// </summary>
        /// <param name="parts">The parts.</param>
        /// <returns></returns>
        public static string Encode(params string[] parts)
        {
            string plainText = string.Join("\t", parts);

            SymmetricAlgorithm aes = Rijndael.Create();
            aes.GenerateIV();

            string keyBase64 = "iAvTPBddWuLfSqUUu/d+u1tlnZUa7hzPro38MkGBzNo=";
            byte[] key = Convert.FromBase64String(keyBase64);
            byte[] iv = aes.IV;

            MemoryStream memory = new MemoryStream();
//			memory.Write(iv, 0, iv.Length);

            CryptoStream stream = new CryptoStream(
                memory,
                aes.CreateEncryptor(key, iv),
                CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(plainText);
            writer.Flush();
            stream.FlushFinalBlock();


            string sigBase64 = "FXuR01wClctaocdAf+BQ2jZu2AHnjP3s33oZH10leolR7m891bQewj4xqfvrszGt31TOuJBD7jtGGnm2Es62Tw==";

            HMACSHA1 signer = new HMACSHA1(Convert.FromBase64String(sigBase64));
            byte[] hmac = signer.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            return String.Concat(
                HexEncoding.ToString(iv),
                HexEncoding.ToString(memory.GetBuffer(), 0, (int) memory.Length),
                HexEncoding.ToString(hmac));


//				return  + "_01234567";
        }

        /// <summary>
        /// Decodes the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public static string[] Decode(string cipherText)
        {
            int discarded;
            byte[] cipher = HexEncoding.GetBytes(cipherText, out discarded);


            SymmetricAlgorithm aes = Rijndael.Create();

            string keyBase64 = "iAvTPBddWuLfSqUUu/d+u1tlnZUa7hzPro38MkGBzNo=";
            byte[] key = Convert.FromBase64String(keyBase64);
            byte[] iv = new byte[aes.IV.Length];
            Array.Copy(cipher, 0, iv, 0, iv.Length);

            byte[] hmac = new byte[20];
            Array.Copy(cipher, cipher.Length - hmac.Length, hmac, 0, hmac.Length);

            MemoryStream memory = new MemoryStream(
                cipher, iv.Length, cipher.Length - iv.Length - hmac.Length, false);
            CryptoStream stream = new CryptoStream(
                memory,
                aes.CreateDecryptor(key, iv),
                CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(stream);
            string plainText = reader.ReadToEnd();

            string sigBase64 = "FXuR01wClctaocdAf+BQ2jZu2AHnjP3s33oZH10leolR7m891bQewj4xqfvrszGt31TOuJBD7jtGGnm2Es62Tw==";
            HMACSHA1 signer = new HMACSHA1(Convert.FromBase64String(sigBase64));
            byte[] hmacCalculated = signer.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            for (int index = 0; index != hmacCalculated.Length; ++index)
            {
                if (hmacCalculated[index] != hmac[index])
                    throw new CryptographicException("Parameter signature invalid");
            }

            return plainText.Split('\t');
        }
    }
}