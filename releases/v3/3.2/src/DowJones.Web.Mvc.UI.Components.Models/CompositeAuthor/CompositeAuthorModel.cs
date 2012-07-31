using System;
using DowJones.Web.Mvc.UI.Components.AuthorList;
namespace DowJones.Web.Mvc.UI.Components.CompositeAuthor
{
    public class CompositeAuthorModel : CompositeComponentModel
    {
        public readonly int GOOGLE_PAGER_SIZE = 3;
        public readonly int GOOGLE_PAGER_HALF_SIZE = 1;

        /// <summary>
        /// Gets or sets the first index of the result.
        /// </summary>
        [ClientProperty("firstResultIndex")]
        public int FirstResultIndex { get; set; }

        /// <summary>
        /// Gets or sets the last index of the result.
        /// </summary>
        [ClientProperty("lastResultIndex")]
        public int LastResultIndex { get; set; }

        /// <summary>
        /// Gets or sets the total result count.
        /// </summary>
        [ClientProperty("totalResultCount")]
        public int TotalResultCount { get; set; }

        [ClientProperty("pageSize")]
        public int PageSize { get; set; }

        private int currentPage = 0;
        [ClientProperty("currentPage")]
        public int CurrentPage
        {
            get
            {
                if (this.currentPage == 0)
                {
                    this.currentPage = (this.FirstResultIndex / this.PageSize) + 1;
                }

                return this.currentPage;
            }
            set { this.currentPage = value; }
        }

        private int totalPages = 0;
        [ClientProperty("totalPages")]
        public int TotalPages
        {
            set { this.totalPages = value; }
            get
            {
                if (this.totalPages == 0)
                {
                    int temp = this.TotalResultCount / this.PageSize;
                    if (this.TotalResultCount % this.PageSize > 0)
                    {
                        temp++;
                    }

                    this.totalPages = temp;
                }

                return totalPages;
            }
        }

        [ClientProperty("allAuthorsSelector")]
        public AuthorSelector AllAuthorsSelector { get; set; }

        public string Title { get; set; }

        public string PagerLikeGoogle()
        {
            string pagerString = String.Empty;
            if (this.TotalPages == 1)
            {
                pagerString = "<li class='current'>1</li>";
            }
            else
            {
                int numberPages = this.TotalPages;
                if (numberPages > GOOGLE_PAGER_SIZE)
                {
                    numberPages = GOOGLE_PAGER_SIZE;
                }

                int beginPage = 1;
                if (this.CurrentPage > GOOGLE_PAGER_HALF_SIZE && this.TotalPages > GOOGLE_PAGER_SIZE)
                {
                    beginPage = this.CurrentPage - GOOGLE_PAGER_HALF_SIZE;
                }

                int lastPage = beginPage + numberPages;
                if (lastPage > this.TotalPages)
                {
                    beginPage = this.TotalPages - numberPages + 1;
                    lastPage = this.TotalPages + 1;
                }
                for (int i = beginPage; i < lastPage; i++)
                {
                    if (i == this.CurrentPage)
                    {
                        pagerString += "<li class='current'>" + i + "</li>";
                    }
                    else
                    {
                        pagerString += "<li><span class='entity-list-pager'>" + i + "</span></li>";
                    }
                }
            }

            return pagerString;
        }

        /// <summary>
        /// Gets or sets the author list.
        /// </summary>
        public AuthorListModel AuthorList { get; set; }

        public CompositeAuthorModel()
        {
            this.PageSize = 25;
            this.AuthorList = new AuthorListModel();
        }
    }

    public class AuthorSelector
    {
        public AuthorSelectorType Type { get; set; }
        public string Value { get; set; }
    }

    public enum AuthorSelectorType
    {
        None = 0,
        HeadlineSearch,
        Search,
        List,
        Related
    }
}