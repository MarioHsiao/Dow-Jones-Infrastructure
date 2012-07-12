using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.UI.Components.AuthorList
{
    public class AuthorListModel : ViewComponentModel
    {
        /// <summary>
        /// Gets and sets author list.
        /// </summary>
        public IEnumerable<AuthorModel> Authors { get; set; }

        /// <summary>
        /// Gets and sets first author index.
        /// Used in table to show a serial number.
        /// </summary>
        public uint FirstRecordIndex { get; set; }

        /// <summary>
        /// Gets and sets tokens to be used in AuthorList component.
        /// </summary>
        public AuthorListTokens Tokens { get; set; }

        /// <summary>
        /// Gets and sets column by which the author list will be sorted.
        /// </summary>
        public AuthorListSortColumns SortBy { get; set; }

        /// <summary>
        /// Gets and sets order direction for sorting the author list.
        /// </summary>
        public OrderDirections SortOrder { get; set; }

        [ClientProperty("pageSize")]
        public int PageSize { get; set; }

        [ClientProperty("totalResultCount")]
        public int TotalResultCount { get; set; }

        public bool ShowDeleteAction { get; set; }

        public List<AuthorListColumns> DisplayedColumns { get; set; }

        public IEnumerable<SelectListItem> Actions
        {
            get
            {
                IEnumerable<SelectListItem> all = GetActionMenuItems();
                if (this.ShowDeleteAction)
                {
                    return all;
                }
                else
                {
                    var woDelete = from a in all
                                   where a.Value.Equals("delete") == false
                                   select a;
                    return woDelete;
                }
            }
        }

        public bool AnyOutletRelatedColumn
        {
            get
            {
                bool retval = this.DisplayedColumns.Contains(AuthorListColumns.OutletName)
                    || this.DisplayedColumns.Contains(AuthorListColumns.OutletType)
                    || this.DisplayedColumns.Contains(AuthorListColumns.OutletFrequency)
                    || this.DisplayedColumns.Contains(AuthorListColumns.OutletCirculation)
                    || this.DisplayedColumns.Contains(AuthorListColumns.EmploymentType);

                return retval;
            }
        }

        [ClientData("selectedAuthorIds")]
        public string SelectedAuthorIds { get; set; }

        /// <summary>
        /// Create an instanse of AuthorList class.
        /// </summary>
        public AuthorListModel()
        {
            this.Authors = Enumerable.Empty<AuthorModel>();
            this.Tokens = new AuthorListTokens();
            this.ShowDeleteAction = false;
            this.SortBy = AuthorListSortColumns.ContactName;
            this.SortOrder = OrderDirections.Ascending;
            this.DisplayedColumns = new List<AuthorListColumns>();
        }

        private IEnumerable<SelectListItem> GetActionMenuItems()
        {
            // Define the action menu;
            return new[] {
				new SelectListItem { Text = this.Tokens.AddToContactList, Value = "contact-list" },
				new SelectListItem { Text = this.Tokens.CreateActivity, Value = "create-activity" },
				new SelectListItem { Text = this.Tokens.CreateAlert, Value = "create-alert" },
				new SelectListItem { Text = this.Tokens.CreateBriefingBook, Value = "create-briefing-book" },
				new SelectListItem { Text = this.Tokens.Print, Value = "print" },
				new SelectListItem { Text = this.Tokens.Export, Value = "export" },
				new SelectListItem { Text = this.Tokens.ExportAll, Value = "export-all" },
				new SelectListItem { Text = this.Tokens.Delete, Value = "delete" },
				new SelectListItem { Text = this.Tokens.Email, Value = "email" },
				new SelectListItem { Text = this.Tokens.UnselectAll, Value = "unselect-all" }
			};
        }
    }

    /// <summary>
    /// Author's sortable columns enumerator.
    /// </summary>
    public enum AuthorListSortColumns
    {
        None = 0,

        /// <summary>
        /// Sort by author name;
        /// </summary>
        [Description("contact")]
        ContactName = 1,

        /// <summary>
        /// Sort by outlet name;
        /// </summary>
        [Description("outlet")]
        OutletName = 2,

        /// <summary>
        /// Sort by outlet type;
        /// </summary>
        [Description("outlet-type")]
        OutletType = 3,

        /// <summary>
        /// Sort by outlet frequency;
        /// </summary>
        [Description("outlet-frequency")]
        OutletFrequency = 4,

        /// <summary>
        /// Sort by circulation;
        /// </summary>
        [Description("circulation")]
        Circulation = 5,

        /// <summary>
        /// Sort by author's country;
        /// </summary>
        [Description("country")]
        Country = 6,

        /// <summary>
        /// Sort by author's job title;
        /// </summary>
        [Description("job-title")]
        JobTitle = 7,

        /// <summary>
        /// Sort by author's employment type;
        /// </summary>
        [Description("employment-type")]
        EmploymentType = 8,

        /// <summary>
        /// Sort by outlet country;
        /// </summary>
        [Description("outlet-country")]
        OutletCountry = 9,

        /// <summary>
        /// Sort by outlet state;
        /// </summary>
        [Description("outlet-state")]
        OutletState = 10,

        /// <summary>
        /// Sort by outlet city;
        /// </summary>
        [Description("outlet-city")]
        OutletCity = 11,

        /// <summary>
        /// Sort by author's state;
        /// </summary>
        [Description("state")]
        State = 12,

        /// <summary>
        /// Sort by author's city;
        /// </summary>
        [Description("city")]
        City = 13,
    }

    public enum AuthorListColumns
    {
        None = 0,
        OutletName,
        OutletType,
        OutletFrequency,
        OutletCirculation,
        Country,
        EmailAddresses,
        UserInfo,
        Phones,
        JobTitle,
        BeatsIndustries,
        BeatsSubjects,
        BeatsRegions,
        EmploymentType,
        PreferedContactMethod,
        RelatedMediaContacts,
        OutletOriginCountry,
        OutletCountry,
        OutletState,
        OutletCity,
        State,
        City
    }

    /// <summary>
    /// Sort order enumerator.
    /// </summary>
    public enum OrderDirections
    {
        None = 0,

        /// <summary>
        /// Order ascending;
        /// </summary>
        [Description("asc")]
        Ascending = 1,

        /// <summary>
        /// Order descending;
        /// </summary>
        [Description("desc")]
        Descending = 3
    }
}