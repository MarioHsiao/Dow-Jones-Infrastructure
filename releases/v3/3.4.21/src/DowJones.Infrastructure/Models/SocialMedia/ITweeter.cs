using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    /// <summary>
    /// The ITwitter Interface.
    /// </summary>
    public interface ITweeter
    {
        #region Properties

        /// <summary>
        /// Gets ProfileImageUrl.
        /// </summary>
        string ProfileImageUrl { get; }

        /// <summary>
        /// Gets ScreenName.
        /// </summary>
        string ScreenName { get; }

        #endregion
    }
}
