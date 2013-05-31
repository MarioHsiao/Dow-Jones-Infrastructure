using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.Managers.SocialMedia.Responses
{
    /// <summary>
    /// The IUsers response.
    /// </summary>
    public interface IUsersResponse
    {
        #region Properties

        /// <summary>
        ///   Gets or sets Lists.
        /// </summary>
        List<TwitterUser> Users { get; set; }

        #endregion
    }
}
