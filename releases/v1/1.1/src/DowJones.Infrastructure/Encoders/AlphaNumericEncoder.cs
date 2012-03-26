// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlphaNumericEncoder.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AlphaNumericEncoder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Utilities.Encoders
{
    public class AlphaNumericEncoder : IEncoder
    {
        public string Prefix
        {
            get { throw new NotImplementedException(); }
        }

        public string Encode(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            return ByteArrayToHexString(bytes);
        }

        public string Decode(string s)
        {
            var decoder = new UTF8Encoding().GetDecoder();

            var bytes = HexStringToByteArray(s);
            var charCount = decoder.GetCharCount(bytes, 0, bytes.Length);

            var chars = new char[charCount];
            decoder.GetChars(bytes, 0, bytes.Length, chars, 0);

            return new string(chars);
        }

        private static string ByteArrayToHexString(IEnumerable<byte> byteArray)
        {
            return byteArray.Aggregate(string.Empty, (current, outputByte) => current + outputByte.ToString("x2"));
        }

        private static byte[] HexStringToByteArray(string hexString)
        {
            var stringLength = hexString.Length;
            var bytes = new byte[stringLength / 2];

            for (var i = 0; i < stringLength; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
