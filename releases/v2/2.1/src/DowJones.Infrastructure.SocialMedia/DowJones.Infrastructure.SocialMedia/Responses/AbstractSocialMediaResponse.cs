// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractSocialMediaResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Abstract Social Media Response base class
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.SocialMedia.Responses
{   
    /// <summary>
    /// Abstract Sulia Response base class
    /// </summary>
    public abstract class AbstractSocialMediaResponse : ISocialMediaResponse
    {      
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSocialMediaResponse"/> class.
        /// </summary>
        protected AbstractSocialMediaResponse()
        {
            Audit = new Audit();
        }

        #endregion
        
        #region Properties

        /// <summary>
        ///   Gets or sets the the message from the API.  
        ///   In the event of an error, this message may contain helpful text.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///   Gets or sets the status outcome of the response.
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