// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetTweetsByCategoryResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.SocialMedia.Responses
{                          
    /// <summary>
    /// </summary>
    public class GetTweetsByCategoryResponse : List<Tweet>, ISocialMediaResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTweetsByCategoryResponse"/> class.
        /// </summary>
        public GetTweetsByCategoryResponse()
        {
            Audit = new Audit();
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets the Audit.
        /// </summary>
        /// <value>
        /// The audit.
        /// </value>
        public Audit Audit { get; private set; }

        #endregion
    }
}