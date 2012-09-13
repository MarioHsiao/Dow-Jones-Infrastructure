
namespace EMG.Utility.Url
{
    public interface IQueryStringEncoder
    {
        /// <summary>
        /// The recommended prefix is a '~'
        /// </summary>
        string Prefix { get; }

        string Encode(string s);
        string Decode(string s);
    }
}
