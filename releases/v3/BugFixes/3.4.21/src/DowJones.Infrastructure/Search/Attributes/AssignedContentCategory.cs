using System;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Search.Attributes
{
    /// <summary>
    /// Summary description for AssignedContentCategory.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AssignedContentCategory : Attribute
    {
        private SearchCollection _contentCategory;

        /// <summary>
        /// Gets or sets the content categories.
        /// </summary>
        /// <value>The content categories.</value>
        public SearchCollection ContentCategory
        {
            get { return _contentCategory; }
            set { _contentCategory = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedContentCategory"/> class.
        /// </summary>
        /// <param name="contentCategory">The content category.</param>
        public AssignedContentCategory(SearchCollection contentCategory)
        {
            _contentCategory = contentCategory;
        }
    }
}
