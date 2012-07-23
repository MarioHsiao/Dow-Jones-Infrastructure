// -----------------------------------------------------------------------
// <copyright file="RelatedConcepts.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Models.Search
{    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract(Name = "term", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "term")]
    public class Term
    {
        [JsonProperty("text")]
        [DataMember(Name = "text")]
        public string Text { get; set; }

        [JsonProperty("weight")]
        [DataMember(Name = "weight")]
        public float Weight { get; set; }
    }

    [DataContract(Name = "relatedConceptsResults", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "relatedConceptsResults")]
    public class RelatedConceptsDataResult
    {
        /// <summary>
        ///   Gets or sets DateItems.
        /// </summary>
        [JsonProperty("terms")]
        [DataMember(Name = "terms")]
        public IList<Term> Terms { get; set; }
    }
}
