
namespace EMG.Utility.Url
{
    /// <summary>
    /// Provides no encoding, returns plaintext
    /// </summary>
    public class DefaultEncoder : IQueryStringEncoder
    {
        #region IQueryStringEncoder Members

        public string Encode(string s)
        {
            return s;
        }

        public string Decode(string s)
        {
            return s;
        }

        public string Prefix
        {
            get { return string.Empty; }
        }

        #endregion
    }

}
