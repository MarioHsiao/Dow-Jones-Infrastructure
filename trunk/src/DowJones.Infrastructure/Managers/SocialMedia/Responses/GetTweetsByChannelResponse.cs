using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.Managers.SocialMedia
{
    /// <summary>
    /// </summary>
    public class GetTweetsByChannelResponse : List<Tweet>, ISocialMediaResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTweetsByCategoryResponse"/> class.
        /// </summary>
        public GetTweetsByChannelResponse()
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
