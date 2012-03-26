using System;
using System.Text;
using DowJones.Utilities.Exceptions;

namespace DowJones.Utilities.Encoders
{
    public class Base64
    {
        public static string Encode(string value)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                throw new DowJonesUtilitiesException(e, DowJonesUtilitiesException.EncodingUtilitiesEncodingException);
            }
        }

        public static string Decode(string value)
        {
            try
            {
                var decoder = new UTF8Encoding().GetDecoder();

                var bytes = Convert.FromBase64String(value);
                var charCount = decoder.GetCharCount(bytes, 0, bytes.Length);

                var chars = new char[charCount];
                decoder.GetChars(bytes, 0, bytes.Length, chars, 0);

                return new string(chars);
            }
            catch (Exception e)
            {
                throw new DowJonesUtilitiesException(e, DowJonesUtilitiesException.EncodingUtilitiesDecodingException);
            }
        }
    }
}