using System;
using System.Linq;
using System.Web.Mvc;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Managers.QueryUtility;
using DowJones.Web.Mvc.UI.Components.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using Track = Factiva.Gateway.Messages.Track.V1_0;
using DowJones.Globalization;
using DowJones.Utilities.Search.Core;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Attributes;
using SearchDateRange = DowJones.Search.SearchDateRange;
using SortOrder = DowJones.Search.SortOrder;

namespace DowJones.Web.Mvc.UI.Components.Models
{

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SearchBuilderData
    {
        private List<CodeDesc> _exclusionsList = SearchBuilderModel.GetCodeValueFromEnum<ExclusionFilter>();

        [JsonProperty("freeText")]
        public string FreeText { get; set; }

        [JsonProperty("topicId")]
        public string TopicId { get; set; }

        [JsonProperty("alertId")]
        public string AlertId { get; set; }

        [JsonProperty("filters")]
        public SearchChannelFilters Filters { get; set; }

        [JsonProperty("newsFilters")]
        public SearchNewsFilters NewsFilters { get; set; }

        [JsonModelBinder]
        [JsonProperty("contentLanguages")]
        public IEnumerable<string> ContentLanguages { get; set; }

        [JsonProperty("searchIn")]
        public SearchFreeTextArea SearchIn { get; set; }

        [JsonProperty("sortBy")]
        public SortOrder SortBy { get; set; }

        [JsonProperty("dateRange")]
        public SearchDateRange DateRange { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("exclusionFilter")]
        public IEnumerable<ExclusionFilter> ExclusionFilter { get; set; }

        [JsonProperty("duplicates")]
        public bool Duplicates { get; set; }

        [JsonProperty("socialMedia")]
        public bool IncludeSocialMedia { get; set; }

        /// <summary>
        /// Gets or Sets the Exclusions list
        /// </summary>
        [JsonProperty("exclusionsList")]
        public List<CodeDesc> ExclusionsList
        {
            get { return _exclusionsList; }
            set { _exclusionsList = value; }
        }

        /// <summary>
        /// Gets or Sets the SourceGroup
        /// </summary>
        [JsonProperty("sourceGroup")]
        public List<SourceGroupItem> SourceGroup { get; set; }
    }

    public class SearchBuilderModel : CompositeComponentModel
    {
        #region ..:: Private Members ::..
        private SearchBuilderData _data;
        private int _searchQueryMaxLength = 2048;
        private List<CodeDesc> _searchFreeTextArea = GetCodeValueFromEnum<SearchFreeTextArea>();
        private List<CodeDesc> _sortBy = GetSortBy();
        private List<CodeDesc> _dateRange = DateRangeHelper.GetDateRange(true, true).Select(d => new CodeDesc { Code = d.Key, Desc = d.Value }).ToList();

        private static List<CodeDesc> GetSortBy()
        {
            var lstCodeDesc = new List<CodeDesc>();
            string token;
            var sortEnums = new[] {SortOrder.PublicationDateMostRecentFirst, SortOrder.PublicationDateOldestFirst, SortOrder.Relevance};
            foreach (SortOrder sortOrder in sortEnums)
            {
                token = ((AssignedToken)Attribute.GetCustomAttribute(typeof(SortOrder).GetField(sortOrder.ToString()), typeof(AssignedToken))).Token;
                lstCodeDesc.Add(new CodeDesc { Code = ((int)Enum.Parse(typeof(SortOrder), sortOrder.ToString())).ToString(), Desc = ResourceTextManager.Instance.GetString(token) });
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
        #endregion

        #region ..:: Public Members ::..
        /// <summary>
        /// Gets or sets the sortBy dropdown list.
        /// </summary>
        public List<CodeDesc> SearchFreeTextArea
        {
            get { return _searchFreeTextArea; }
            //set { _sortBy = value; }
        }

        /// <summary>
        /// Gets or sets the sortBy dropdown list.
        /// </summary>
        public List<CodeDesc> SortBy
        {
            get { return _sortBy; }
            //set { _sortBy = value; }
        }

        /// <summary>
        /// Gets or sets the date range dropdown list.
        /// </summary>
        public List<CodeDesc> DateRange
        {
            get { return _dateRange; }
            //set { _dateRange = value; }
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
            get { return ResourceTextManager.Instance.GetString(((AssignedToken)Attribute.GetCustomAttribute(typeof(DateFormat).GetField(DateFormat.ToString()), typeof(AssignedToken))).Token); }
        }

        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        [ClientData]
        public SearchBuilderData Data
        {
            get { return _data ?? (_data = new SearchBuilderData()); }
            set { _data = value; }
        }

        /// <summary>
        /// Gets or sets the SessionId.
        /// </summary>
        [ClientProperty("searchQueryMaxLength")]
        public int SearchQueryMaxLength { get { return _searchQueryMaxLength; } set { _searchQueryMaxLength = value; } }

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

        public static List<CodeDesc> GetCodeValueFromEnum<T>()
        {
            var lstCodeDesc = new List<CodeDesc>();
            string token;
            foreach (T exclusion in Enum.GetValues(typeof(T)))
            {
                token = ((AssignedToken)Attribute.GetCustomAttribute(typeof(T).GetField(exclusion.ToString()), typeof(AssignedToken))).Token;
                lstCodeDesc.Add(new CodeDesc { Code = ((int)Enum.Parse(typeof(T), exclusion.ToString())).ToString(), Desc = ResourceTextManager.Instance.GetString(token) });
            }
            return lstCodeDesc;
        }
        #endregion

        #region ..:: Constructor ::..

        public SearchBuilderModel()
        {
        }

        #endregion
    }
}