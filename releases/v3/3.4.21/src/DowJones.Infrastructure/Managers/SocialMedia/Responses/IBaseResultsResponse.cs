using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Managers.SocialMedia.Responses
{
    /// <summary>
    /// The interface for base results response.
    /// </summary>
    public interface IBaseResultsResponse
    {
        /// <summary>
        ///   Gets or sets the current page number.
        /// </summary>
        /// <value>
        ///   The page.
        /// </value>
        int Page { get; set; }

        /// <summary>
        ///   Gets or sets the total number of pages.
        /// </summary>
        /// <value>
        ///   The page count.
        /// </value>
        int PageCount { get; set; }

        /// <summary>
        ///   Gets or sets the The number of users per page.
        /// </summary>
        /// <value>
        ///   The lists per page.
        /// </value>
        int ResultsPerPage { get; set; }

        /// <summary>
        ///   Gets or sets the total number of lists matching.
        /// </summary>
        /// <value>
        ///   The total count.
        /// </value>
        int TotalCount { get; set; }
    }
}
