// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.PerformAccountHealthCheck.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The social media client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Hammock;
using Hammock.Web;

namespace DowJones.Infrastructure.SocialMedia
{
    using DowJones.SocialMedia.Responses;

    /// <summary>
    /// The social media client.
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Public Methods

        /// <summary>
        /// Determines whether [is account healthy].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [is account healthy]; otherwise, <c>false</c>.
        /// </returns>
        public PerformAccountHealthCheckResponse PerformAccountHealthCheck()
        {
            var request = this.GetHealthCheckRequest();
            return TryGetSuliaResponseImplementation<PerformAccountHealthCheckResponse>(request);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the health check request.
        /// </summary>
        /// <returns>
        /// A <see cref="RestResponse"/> object.
        /// </returns>
        private RestRequest GetHealthCheckRequest()
        {
            var request = new RestRequest
                {
                   Path = "account/health_check.json", 
                   Method = WebMethod.Get, 
                   Serializer = Serializer 
                };

            this.SetSocialMediaMeta(request);
            return request;
        }

        #endregion
    }
}