namespace DowJones.Utilities.Encoders
{
    /// <summary>
    /// Provides no encoding, returns plaintext
    /// </summary>
    public class DefaultEncoder : IEncoder
    {
        #region IEncoder Members

        /// <summary>
        /// The recommended prefix is a '~'
        /// </summary>
        /// <value></value>
        public string Prefix
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Encodes the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public string Encode(string s)
        {
            return s;
        }

        /// <summary>
        /// Decodes the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public string Decode(string s)
        {
            return s;
        }

        #endregion
    }
}