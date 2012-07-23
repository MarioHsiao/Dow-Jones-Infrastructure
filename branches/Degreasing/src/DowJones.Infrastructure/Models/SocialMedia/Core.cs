// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

namespace DowJones.Infrastructure.Models.SocialMedia
{           
    /// <summary>
    ///  The publication status of the list (currently, either public or private)
    /// </summary>
    public enum PublicationStatus
    {
        /// <summary>
        /// Public status
        /// </summary>
        [JsonProperty("public")]
        Public,

        /// <summary>
        /// Private status
        /// </summary>
        [JsonProperty("private")]
        Private
    }

}
