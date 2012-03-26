namespace DowJones.Utilities.Encoders
{
    public interface IEncoder
    {
        /// <summary>
        /// Gets the recommended prefix
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Encodes the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        string Encode(string s);

        /// <summary>
        /// Decodes the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        string Decode(string s);
    }
}