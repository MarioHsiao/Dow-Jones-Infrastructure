// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TalksAbout.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Infrastructure.Models.SocialMedia
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TalksAbout : List<string>
    {
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Count > 0 ? string.Join(",", this.ToArray()) : string.Empty;
        }
    }
}
