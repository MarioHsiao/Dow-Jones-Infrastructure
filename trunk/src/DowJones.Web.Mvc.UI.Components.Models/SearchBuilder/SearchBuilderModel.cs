using System;
using System.Linq;
using DowJones.Managers.QueryUtility;
using System.Collections.Generic;
using DowJones.Globalization;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Attributes;
using DowJones.Web.Mvc.UI.Components.Search;

namespace DowJones.Web.Mvc.UI.Components.SearchBuilder
{
    public class SearchBuilderModel : CompositeComponentModel
    {
        private readonly IResourceTextManager _resources;
        private SearchBuilderData _data;
        private readonly List<CodeDesc> _searchFreeTextArea;
        private readonly List<CodeDesc> _sortBy;
        private readonly List<CodeDesc> _dateRange;

        private static List<CodeDesc> GetSortBy(IResourceTextManager resources)
        {
            var lstCodeDesc = new List<CodeDesc>();
            var sortEnums = new[] {SortOrder.PublicationDateMostRecentFirst, SortOrder.PublicationDateOldestFirst, SortOrder.Relevance};
            foreach (SortOrder sortOrder in sortEnums)
            {
                string token = ((AssignedToken)Attribute.GetCustomAttribute(typeof(SortOrder).GetField(sortOrder.ToString()), typeof(AssignedToken))).Token;
                lstCodeDesc.Add(new CodeDesc { Code = ((int)Enum.Parse(typeof(SortOrder), sortOrder.ToString())).ToString(), Desc = resources.GetString(token) });
            }
            return lstCodeDesc;
        }

        private string GetFormattedDate(string dateInYMD)
        {
            if (!string.IsNullOrEmpty(dateInYMD))
            {
                var sep = (!string.IsNullOrEmpty(DateFormatDisplay) && DateFormatDisplay.IndexOf('-') != -1) ? "-" : "/";
                string format;
                switch (DateFormat)
                {
                    case DateFormat.DDMMCCYY:
                        format =  "dd" + sep + "MM" + sep + "yyyy";
                        break;
                    case DateFormat.CCYYMMDD://No seperator for ISO format
                        format = "yyyyMMdd";
                        break;
                    default:
                        format = "MM" + sep + "dd" + sep + "yyyy";
                        break;
                }
                var date = dateInYMD.Trim();
                return (new DateTime(Convert.ToInt32(date.Substring(0, 4)), Convert.ToInt32(date.Substring(4, 2)), Convert.ToInt32(date.Substring(6, 2)))).ToString(format);
            }
            return null;
        }

        /// <summary>
        /// Gets or sets the sortBy dropdown list.
        /// </summary>
        public List<CodeDesc> SearchFreeTextArea
        {
            get { return _searchFreeTextArea; }
        }

        /// <summary>
        /// Gets or sets the sortBy dropdown list.
        /// </summary>
        public List<CodeDesc> SortBy
        {
            get { return _sortBy; }
        }

        /// <summary>
        /// Gets or sets the date range dropdown list.
        /// </summary>
        public List<CodeDesc> DateRange
        {
            get { return _dateRange; }
        }

        /// <summary>
        /// Gets or sets the SuggestServiceUrl.
        /// </summary>
        [ClientProperty("preferences")]
        public IPreferences Preferences { get; set; }

        /// <summary>
        /// Gets or sets the SuggestServiceUrl.
        /// </summary>
        [ClientProperty("suggestServiceUrl")]
        public string SuggestServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the TaxanomyServiceUrl.
        /// </summary>
        [ClientProperty("taxonomyServiceUrl")]
        public string TaxonomyServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the QueriesServiceUrl.
        /// </summary>
        [ClientProperty("queriesServiceUrl")]
        public string QueriesServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the DateFormat.
        /// </summary>
        [ClientProperty("dateFormat")]
        public DateFormat DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the DateFormatDisplay.
        /// </summary>
        [ClientProperty("dateFormatDisplay")]
        public string DateFormatDisplay
        {
            get { return _resources.GetString(((AssignedToken)Attribute.GetCustomAttribute(typeof(DateFormat).GetField(DateFormat.ToString()), typeof(AssignedToken))).Token); }
        }

        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        [ClientData]
        public SearchBuilderData Data
        {
            get { return _data ?? (_data = new SearchBuilderData(_resources)); }
            set { _data = value; }
        }

        /// <summary>
        /// Gets or sets the SessionId.
        /// </summary>
        [ClientProperty("searchQueryMaxLength")]
        public int SearchQueryMaxLength { get; set; }

        /// <summary>
        /// Gets or sets the ProductId.
        /// </summary>
        [ClientProperty("productId")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets the formatted Start Date
        /// </summary>
        public string FormattedStartDate
        {
            get { return GetFormattedDate(Data.StartDate); }
        }

        /// <summary>
        /// Gets the formatted End Date
        /// </summary>
        public string FormattedEndDate
        {
            get { return GetFormattedDate(Data.EndDate); }
        }

        public static List<CodeDesc> GetCodeValueFromEnum<T>(IResourceTextManager resources)
        {
            var lstCodeDesc = new List<CodeDesc>();
            foreach (T exclusion in Enum.GetValues(typeof(T)))
            {
                string token = ((AssignedToken)Attribute.GetCustomAttribute(typeof(T).GetField(exclusion.ToString()), typeof(AssignedToken))).Token;
                lstCodeDesc.Add(new CodeDesc { Code = ((int)Enum.Parse(typeof(T), exclusion.ToString())).ToString(), Desc = resources.GetString(token) });
            }
            return lstCodeDesc;
        }

        public SearchBuilderModel(IResourceTextManager resources = null)
        {
            _resources = resources ?? DowJones.DependencyInjection.ServiceLocator.Resolve<IResourceTextManager>();
            SearchQueryMaxLength = 2048;
            _searchFreeTextArea = GetCodeValueFromEnum<SearchFreeTextArea>(_resources);
            _sortBy = GetSortBy(_resources);
            _dateRange = DateRangeHelper.GetDateRange(true, true).Select(d => new CodeDesc { Code = d.Key, Desc = d.Value }).ToList();
        }
    }
}