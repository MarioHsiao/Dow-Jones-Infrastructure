using System;
using System.Text;
using EMG.Utility.Exceptions;

namespace EMG.Utility.Url
{
    public class Base64
    {
        public static string Encode(string value)
        {
            try
            {
                byte[] bytes;
                bytes = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                throw new EMGUtilitiesException(e, EMGUtilitiesException.ENCODING_UTILITIES_ENCODING_EXCEPTION);
            }
        }

        public static string Decode(string value)
        {
            try
            {
                Decoder decoder = new UTF8Encoding().GetDecoder();

                byte[] bytes = Convert.FromBase64String(value);
                int charCount = decoder.GetCharCount(bytes, 0, bytes.Length);

                char[] chars = new char[charCount];
                decoder.GetChars(bytes, 0, bytes.Length, chars, 0);

                return new String(chars);
            }
            catch (Exception e)
            {
                throw new EMGUtilitiesException(e, EMGUtilitiesException.ENCODING_UTILITIES_DECODING_EXCEPTION);
            }
        }
    }
}