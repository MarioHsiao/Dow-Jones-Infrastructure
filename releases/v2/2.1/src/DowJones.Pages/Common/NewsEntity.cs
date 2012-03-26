using System.Runtime.Serialization;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;

namespace DowJones.Pages
{
    /// <summary>
    /// The trending entity. Contains the code, volume the descriptor in the requested interface language
    /// </summary>
    /// <remarks></remarks>
    [DataContract(Name = "newsEntity", Namespace = "")]
    public class NewsEntity
    {
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        /// What we probably need is Bucket
        /// <remarks></remarks>
        [DataMember(Name = "type")]
        public EntityType Type { get; set; }

        /// <summary>
        /// Gets or sets the news volume.
        /// </summary>
        /// <value>The news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "currentTimeFrameNewsVolume")]
        public WholeNumber CurrentTimeFrameNewsVolume { get; set; }

        /// <summary>
        /// Gets or sets the entity factiva code.
        /// </summary>
        /// <value>The entity factiva code.</value>
        /// <remarks></remarks>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the entity descriptor.
        /// </summary>
        /// <value>The entity descriptor.</value>
        /// <remarks></remarks>
        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }

        [DataMember(Name = "searchContextRef")]
        public string SearchContextRef { get; set; }
    }
}