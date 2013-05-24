using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.Managers.SocialMedia
{
    /// <summary>
    /// The Interface for a AbstractSocialMediaResponse
    /// </summary>
    public interface ISocialMediaResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the message from the API.
        /// In the event of an error, this message may contain helpful text.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the status outcome of the response.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        Status Status { get; set; }

        /// <summary>
        /// Gets or sets Audit.
        /// </summary>
        /// <value>
        /// The audit.
        /// </value>
        Audit Audit { get; }
        #endregion
    }
}
