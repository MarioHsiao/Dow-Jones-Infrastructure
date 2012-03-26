// -----------------------------------------------------------------------
// <copyright file="WordCloud.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DowJones.Infrastructure.Models.SocialMedia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// </summary>
    public class WordCloud : List<Tag>
    {

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (Count > 0)
            {
                var temp = this.Select(item => string.Format("{0}:{1}", item.Term, item.Weight)).ToArray();
                return string.Join(",", temp);
            }
            return string.Empty;     
        }
    }

    /// <summary>
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets Term.
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets Weight.
        /// </summary>
        public int Weight { get; set; }
    }
}
