using System;

namespace EMG.Utility.Url
{
    public class Base64Encoder : IQueryStringEncoder
    {
        public string Encode(string s)
        {
            return Base64.Encode(s);
        }

        public string Decode(string s)
        {
            return Base64.Decode(s);
        }

        public string Prefix
        {
            get
            {
                return "~";
            }
        }
    }
}
