using DowJones.OperationalData.EntryPoint;

namespace DowJones.OperationalData.Search
{
    public class BaseCoreSearchBuilderOperationalData : BaseCoreSearchOperationalData
    {

        /// <summary>
        /// Gets or sets the flag saved search.
        /// </summary>
        /// <value>The flag saved search.</value>
        public bool IsSavedSearch
        {
            get
            {
                string v = Get(ODSConstants.KEY_SAVED_SEARCH);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_SAVED_SEARCH, v);
            }
        }
        
        /// <summary>
        /// Gets or sets the search for free text area.
        /// </summary>
        /// <value>The search for free text area.</value>
        public SearchFreeTextArea SearchFreeText
        {
            get
            {
                return EnumMapper.MapStringToSearchFreeText(Get(ODSConstants.KEY_SEARCH_FREE_TEXT));
            }
            set
            {
                Add(ODSConstants.KEY_SEARCH_FREE_TEXT, EnumMapper.MapSearchFreeTextToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the flag republished news excluded.
        /// </summary>
        /// <value>Is republished news excluded.</value>
        public bool IsRepublishedNewsExcluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_REPUBLISHED_NEWS_EXCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_REPUBLISHED_NEWS_EXCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag recurring pricing and market data excluded.
        /// </summary>
        /// <value>Is recurring pricing and market data excluded.</value>
        public bool IsRecurringPricingMarketDataExcluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_RECURRING_PRICING_MARKET_DATA_EXCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_RECURRING_PRICING_MARKET_DATA_EXCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag obituaries, sport and calendars excluded.
        /// </summary>
        /// <value>Is obituaries, sport and calendars excluded.</value>
        public bool IsObituariesSportCalendarsExcluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_OBITUARIES_SPORT_CALENDAR_EXCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_OBITUARIES_SPORT_CALENDAR_EXCLUDED, v);
            }
        }
    }
}