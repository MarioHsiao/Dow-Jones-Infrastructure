// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBaseResultsResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The interface for base results response.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.SocialMedia.Responses
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

    /// <summary>
    /// The i users response.
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

    /// <summary>
    /// </summary>
    public interface IBaseListsResponse
    { 
        /// <summary>
        /// Gets or sets Lists.
        /// </summary>
        List<TwitterList> Lists { get; set; }
    }
    
    /// <summary>
    /// The i base users response.
    /// </summary>
    public interface IBaseCategoriesResponse
    {
        /// <summary>
        ///   Gets or sets the categories.
        /// </summary>
        /// <value>
        ///   The categories.
        /// </value>
        List<Category> Categories { get; set; }
    }    
}
