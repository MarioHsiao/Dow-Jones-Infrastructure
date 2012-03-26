using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Infrastructure.Models.SocialMedia;

namespace TwitterTestApplication.Models
{
    /// <summary>
    /// </summary>
    public class CategoryResultsModel
    {
        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public IEnumerable<SelectListItem> Categories
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        /// <value>
        /// The selected value.
        /// </value>
        public List<Tweet> Tweets
        {
            get; 
            set;
        }                
    }
}