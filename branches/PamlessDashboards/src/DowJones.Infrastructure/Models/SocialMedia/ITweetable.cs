using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Managers.SocialMedia.Twitter;


namespace DowJones.Infrastructure.Models.SocialMedia
{
    /// <summary>
    /// </summary>
    public interface ITweetable
    {
        #region Properties

        /// <summary>
        /// Gets Author.
        /// </summary>
        ITweeter Author { get; }

        /// <summary>
        /// Gets CreatedDate.
        /// </summary>
        DateTime CreatedDate { get; }

        /// <summary>
        /// Gets Entities.
        /// </summary>
        TwitterEntities Entities { get; }

        /// <summary>
        /// Gets Id.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Gets Text.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets TextAsHtml.
        /// </summary>
        string TextAsHtml { get; }

        #endregion
    }

}
