using System.Runtime.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// The feed type.
    /// </summary>
    [DataContract(Name = "feedType", Namespace = "")]
    public enum FeedType
    {
        /// <summary>
        /// The rss feed.
        /// </summary>
        [EnumMember]
        RSS, 

        /// <summary>
        /// The atom feed.
        /// </summary>
        [EnumMember]
        Atom
    }
}