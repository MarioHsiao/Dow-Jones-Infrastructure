using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.Search.UI.Components.Filters
{
    public class SearchNavigatorNode : List<SearchNavigatorNode>
    {
        public string Ref { get; set; }

        public string DisplayName { get; set; }

        public string GroupCode { get; set; }

        /// <summary>
        /// The formatted Group Code for use in CSS
        /// selectors, etc.
        /// e.g. SG_NMEDIA => nmedia
        /// </summary>
        public string GroupTag { get; set; }

        public bool HasChildren
        {
            get { return Count > 0; }
        }

        public bool HasCount
        {
            get { return ResultCount > 0; }
        }
        
        public int ResultCount { get; set; }

        public bool IsSecondaryGroup { get; set; }

        public bool IsSelectable { get; set; }

        public bool IsSelected
        {
            get { return _isSelected || this.Any(x => x.IsSelected); }
            set { _isSelected = value; }
        }
        private bool _isSelected;


        public SearchNavigatorNode()
            : this(Enumerable.Empty<SearchNavigatorNode>())
        {
        }

        public SearchNavigatorNode(IEnumerable<SearchNavigatorNode> children)
            : base(children)
        {
            IsSelectable = true;
        }
    }
}