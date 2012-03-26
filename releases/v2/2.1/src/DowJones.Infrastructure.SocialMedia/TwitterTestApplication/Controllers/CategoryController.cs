// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryController.cs" company="">
//   
// </copyright>
// <summary>
//   The category controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using DowJones.Infrastructure.SocialMedia;

using TwitterTestApplication.ActionFilters;
using TwitterTestApplication.Models;

namespace TwitterTestApplication.Controllers
{
    /// <summary>
    /// The category controller.
    /// </summary>
    public class CategoryController : Controller
    {
        // GET: /Category/
        #region Public Methods

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="selectedCategory">The s Cat.</param>
        /// <returns></returns>         
        [HttpGet]
        public ActionResult Index([Bind(Prefix = "cat")]string selectedCategory = null)
        {
            var model = new CategoryResultsModel();
            model.Categories = this.GetCategories(selectedCategory);

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                var socialClient = new SocialMediaClient();
                model.Tweets = socialClient.GetTweetsByCategory(selectedCategory, 200);
            }

            return this.View(model);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <param name="selectedCategory">The selected category.</param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetCategories(string selectedCategory)
        {
            var collection = new List<SelectListItem>
                {
                    new SelectListItem { Selected = false, Text = "- Select One -", Value = string.Empty, },
                    new SelectListItem { Selected = false, Text = "Political Causes", Value = "all/politics-causes", },
                    new SelectListItem { Selected = false, Text = "Sports", Value = "all/sports", },
                    new SelectListItem { Selected = false, Text = "Baseball", Value = "all/sports/baseball", },
                    new SelectListItem { Selected = false, Text = "Football", Value = "all/sports/football", }
                };

            foreach (var selectListItem in collection.Where(selectListItem => selectListItem.Value == selectedCategory))
            {
                selectListItem.Selected = true;
            }
            return collection;
        }

        #endregion
    }
}