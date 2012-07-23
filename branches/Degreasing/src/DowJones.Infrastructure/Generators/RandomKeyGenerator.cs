// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Summary description for Guid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Security.Cryptography;

namespace DowJones.Generators
{
    /// <summary>
    /// Summary description for Guid.
    /// </summary>
    public class RandomKeyGenerator
    {
        /// <summary>
        /// The alpha.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        /// <summary>
        /// The numeric.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private const string Numeric = "0123456789";

        /// <summary>
        /// The alph a_ numeric.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private const string AlphaNumeric = Alpha + Numeric;

        public enum CharacterSet
        {
            Alpha,
            Numeric,
            AlphaNumeric,
        }

        /// <summary>
        /// Gets the random key.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="characterSet">The character set.</param>
        /// <returns>A random key string</returns>
        public static string GetRandomKey(int length, CharacterSet characterSet)
        {
            var randomData = string.Empty;
            var data = new byte[length];
            string internalCharacterSet;
            switch (characterSet)
            {
                case CharacterSet.AlphaNumeric:
                    internalCharacterSet = AlphaNumeric;
                    break;
                case CharacterSet.Numeric:
                    internalCharacterSet = Numeric;
                    break;
                default:
                    internalCharacterSet = Alpha;
                    break;
            }
            var characterSetLength = internalCharacterSet.Length;
            var random = RandomNumberGenerator.Create();
            random.GetBytes(data);

            for (var index = 0; index < length; index++)
            {
                int position = data[index];
                position = position % characterSetLength;
                randomData = randomData + internalCharacterSet.Substring(position, 1);
            }

            return randomData;
        }

        /// <summary>
        /// Gets the random key.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="characterSet">The character set.</param>
        /// <returns></returns>
        public static string GetRandomKey(int length, string characterSet)
        {
            var randomData = string.Empty;
            var data = new byte[length];
            var internalCharacterSet = characterSet;
            var characterSetLength = internalCharacterSet.Length;
            var random = RandomNumberGenerator.Create();
            random.GetBytes(data);
            for (var index = 0; index < length; index++)
            {
                int position = data[index];
                position = position % characterSetLength;
                randomData = randomData + internalCharacterSet.Substring(position, 1);
            }

            return randomData;
        }
    }
}
