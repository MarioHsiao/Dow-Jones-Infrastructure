// -----------------------------------------------------------------------
// <copyright file="Status.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

//using DowJones.Infrastructure.Models.SocialMedia.Responses;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    /// <summary>
    ///   A list of possible status outcomes associated with a <see cref = "ISocialMediaResponse" />.
    /// </summary>
    public enum Status
    {

        /// <summary>
        ///   The response is unknown to this version of the social media.
        /// </summary>
        Unknown = -1,

        /// <summary>
        ///   The request was successful.
        /// </summary>
        Success = 0,

        /// <summary>
        ///   The request was unsuccessful because of a user error.
        /// </summary>
        UserError,

        /// <summary>
        ///   The request was unsuccessful because of a server error.
        /// </summary>
        ServerError,

        /// <summary>
        ///   Their was an error deserializing the error.
        /// </summary>
        DeserializationError
    }
}
