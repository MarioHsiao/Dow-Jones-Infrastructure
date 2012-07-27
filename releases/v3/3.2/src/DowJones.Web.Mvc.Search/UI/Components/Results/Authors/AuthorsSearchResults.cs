﻿using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Mvc.UI.Components.RelatedConcepts;

namespace DowJones.Web.Mvc.Search.UI.Components.Results.Authors
{
    public class AuthorsSearchResults : SearchResults
    {
        const int PAGE_SIZE_DEFAULT_VALUE = 20;

        public CompositeAuthorModel Authors { get; set; }
        public RelatedConceptsComponentModel RelatedConcepts { get; set; }
        public bool HideRelatedConcepts { get; set; }

        private int pageSize = 0;

        [ClientProperty("pageSize")]
        public int PageSize
        {
            get
            {
                if (this.pageSize == 0)
                {
                    this.pageSize = PAGE_SIZE_DEFAULT_VALUE;
                }

                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }

        }


        public AuthorsSearchResults()
        {
            this.Authors = new CompositeAuthorModel();
        }

        public override bool HasResults
        {
            get { return true; }
        }
    }
}