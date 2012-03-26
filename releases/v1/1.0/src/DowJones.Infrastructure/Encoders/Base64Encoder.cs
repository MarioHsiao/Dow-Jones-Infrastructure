namespace DowJones.Utilities.Encoders
{
    public class Base64Encoder : IEncoder
    {
        #region IEncoder Members

        public string Prefix
        {
            get { return "~"; }
        }
        
        public string Encode(string s)
        {
            return Base64.Encode(s);
        }

        public string Decode(string s)
        {
            return Base64.Decode(s);
        }
        #endregion
    }
}