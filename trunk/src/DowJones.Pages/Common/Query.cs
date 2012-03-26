using System.Runtime.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// The query.
    /// </summary>
    [DataContract(Name = "query", Namespace = "")]
    public class Query
    {
        /// <summary>
        /// Gets or sets SearchMode.
        /// </summary>
        [DataMember(Name = "searchMode")]
        public SearchMode SearchMode { get; set; }

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}