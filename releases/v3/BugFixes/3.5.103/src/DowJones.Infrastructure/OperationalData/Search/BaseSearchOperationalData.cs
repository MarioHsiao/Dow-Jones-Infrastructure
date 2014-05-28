using System;
using System.Collections.Generic;
using DowJones.OperationalData.EntryPoint;

namespace DowJones.OperationalData.Search
{
    public class BaseSearchOperationalData : AbstractOperationalData
    {
        private bool searchPageSpecified;
        private bool searchTypeSpecified;
        private bool successIndSpecified;
        
        /// <summary>
        /// Gets or sets the search page. This is required.
        /// </summary>
        /// <value>The search page.</value>
        public SearchPageType SearchPage
        {
            get
            {
                return EnumMapper.MapStringToSearchPageType(Get(ODSConstants.KEY_SEARCH_PAGE));
            }
            set
            {
                Add(ODSConstants.KEY_SEARCH_PAGE, EnumMapper.MapSearchPageTypeToString(value));
                searchPageSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets the auto completed term.
        /// </summary>
        /// <value>The auto completed term.</value>
        public AutoCompletedTerm AutoCompletedTerm
        {
            get
            {
                return EnumMapper.MapAutoCompletedTerm(Get(ODSConstants.KEY_VIEW_AUTO_COMPLETED_TERM));
            }
            set
            {
                Add(ODSConstants.KEY_VIEW_AUTO_COMPLETED_TERM, EnumMapper.MapAutoCompletedTermToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the search type. This is required.
        /// </summary>
        /// <value>The search type.</value>
        public SearchIndicator SearchType
        {
            get
            {
                return EnumMapper.MapStringToSearchIndicator(Get(ODSConstants.KEY_SEARCH_TYPE));
            }
            set
            {
                Add(ODSConstants.KEY_SEARCH_TYPE, EnumMapper.MapSearchIndicatorToString(value));
                searchTypeSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets the flag free text included.
        /// </summary>
        /// <value>Is free text included.</value>
        public bool IsFreeTextIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_FREE_TEXT_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_FREE_TEXT_INCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the search date range.
        /// </summary>
        /// <value>The search date range.</value>
        public EntryPoint.DateRange SearchDateRange
        {
            get
            {
                return EnumMapper.MapStringToDateRange(Get(ODSConstants.KEY_SEARCH_DATE_RANGE));
            }
            set
            {
                Add(ODSConstants.KEY_SEARCH_DATE_RANGE, EnumMapper.MapDateRangeToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the deduplication setting.
        /// </summary>
        /// <value>The deduplication setting.</value>
        public DeduplicationType DeduplicationSetting
        {
            get
            {
                return EnumMapper.MapStringToDeduplicationType(Get(ODSConstants.KEY_DEDUPLICATION_SETTING));
            }
            set
            {
                Add(ODSConstants.KEY_DEDUPLICATION_SETTING, EnumMapper.MapDeduplicationTypeToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the result sort order.
        /// </summary>
        /// <value>The result sort order.</value>
        public PublicationSortOrder ResultSortOrder
        {
            get
            {
                return EnumMapper.MapStringToPublicationSortOrder(Get(ODSConstants.KEY_RESULT_SORT_ORDER));
            }
            set
            {
                Add(ODSConstants.KEY_RESULT_SORT_ORDER, EnumMapper.MapPublicationSortOrderToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the headlines to display per result page.
        /// </summary>
        /// <value>The headlines to display per result page.</value>
        public int ResultHeadlinePerPage
        {
            get { return int.Parse(Get(ODSConstants.KEY_NUMBER_OF_HEADLINE_DISPLAY)); }
            set { Add(ODSConstants.KEY_NUMBER_OF_HEADLINE_DISPLAY, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the flag if the search successful. This is required.
        /// </summary>
        /// <value>Is the search successful.</value>
        public bool IsSuccessful
        {
            get
            {
                string v = Get(ODSConstants.KEY_IS_SUCCESSFUL);
                return (v != null && v == "S");
            }
            set
            {
                string v = "F";
                if (value)
                {
                    v = "S";
                }
                Add(ODSConstants.KEY_IS_SUCCESSFUL, v);
                successIndSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public string ErrorCode
        {
            get { return Get(ODSConstants.KEY_ERROR_CODE); }
            set { Add(ODSConstants.KEY_ERROR_CODE, value); }
        }

        /// <summary>
        /// Gets or sets the total headlines found.
        /// </summary>
        /// <value>The total headlines found.</value>
        public int TotalHeadlinesFound
        {
            get { return int.Parse( Get(ODSConstants.KEY_TOTAL_HEADLINE_FOUND) ); }
            set { Add(ODSConstants.KEY_TOTAL_HEADLINE_FOUND, value.ToString( )); }
        }

        /// <summary>
        /// Gets or sets the number of unique headlines.
        /// </summary>
        /// <value>The number of unique headlines.</value>
        public int NumberUniqueHeadlines
        {
            get { return int.Parse( Get(ODSConstants.KEY_NUMBER_OF_UNIQUE_HEADLINES) ); }
            set { Add(ODSConstants.KEY_NUMBER_OF_UNIQUE_HEADLINES, value.ToString( )); }
        }

        public override IDictionary<string, string> GetKeyValues
        {
            get
            {
                if (!searchPageSpecified)
                {
                    throw new MissingFieldException("Search Page value is not specified");
                }
                if (!searchTypeSpecified)
                {
                    throw new MissingFieldException("Search Type value is not specified");
                }
                if (!successIndSpecified)
                {
                    throw new MissingFieldException("Success or Failure indicator value is not specified");
                }

                return base.GetKeyValues;
            }
        }
    }
}