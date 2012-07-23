using System.Collections.Generic;
using DowJones.Exceptions;
using DowJones.OperationalData.EntryPoint;

namespace DowJones.OperationalData.Search
{
    /// <summary>
    /// Represents the OperationalData for the search builder page (SB) "Free Text Search" tab in dotcom product.
    /// </summary>
    public class SearchBuilderOperationalData : BaseCoreSearchBuilderOperationalData
    {
        public SearchBuilderOperationalData()
        {
            SearchPage = SearchPageType.SearchBuilder;
            IsSavedSearch = false;
        }

        /// <summary>
        /// Gets or sets the flag concept explorer included.
        /// </summary>
        /// <value>Is concept explorer included.</value>
        public bool IsConceptExplorerIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_CONCEPT_EXPLORER_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_CONCEPT_EXPLORER_INCLUDED, v);
            }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the saved searches page (mss) from search builder in dotcom product.
    /// </summary>
    public class SavedSearchBuilderOperationalData : BaseCoreSearchBuilderOperationalData
    {
        public SavedSearchBuilderOperationalData()
        {
            SearchPage = SearchPageType.SearchBuilder;
            IsSavedSearch = true;
        }

        /// <summary>
        /// Gets or sets the flag personal or group.
        /// </summary>
        /// <value>Is personal or group.</value>
        public SavedSearches IsPersonalOrGroup
        {
            get
            {
                return EnumMapper.MapStringToSavedSearches(Get(ODSConstants.KEY_PERSONAL_OR_GROUP));
            }
            set
            {
                Add(ODSConstants.KEY_PERSONAL_OR_GROUP, EnumMapper.MapSavedSearchesToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the share type (for group saved searches only).
        /// </summary>
        /// <value>The share type.</value>
        public ShareTypeOption ShareType
        {
            get
            {
                return EnumMapper.MapStringToShareTypeOption(Get(ODSConstants.KEY_SHARE_TYPE));
            }
            set
            {
                Add(ODSConstants.KEY_SHARE_TYPE, EnumMapper.MapShareTypeOptionToString(value));
            }
        }

        public override IDictionary<string, string> GetKeyValues
        {
            get
            {
                if (IsPersonalOrGroup != SavedSearches.Group && ShareType == ShareTypeOption.Assigned)
                {
                    throw new DowJonesUtilitiesException("ShareType = Assigned applicable only if IsPersonalOrGroup = Group.");
                }

                return base.GetKeyValues;
            }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the search builder page (SB) "Search Form" tab in dotcom product.
    /// </summary>
    public class SearchFormOperationalData : BaseCoreSearchBuilderOperationalData
    {
        public SearchFormOperationalData()
        {
            SearchPage = SearchPageType.SearchForm;
            IsSavedSearch = false;
        }
    }

    /// <summary>
    /// Represents the OperationalData for the saved searches page (mss) from search form in dotcom product.
    /// </summary>
    public class SavedSearchFormOperationalData : BaseCoreSearchBuilderOperationalData
    {
        public SavedSearchFormOperationalData()
        {
            SearchPage = SearchPageType.SearchForm;
            IsSavedSearch = true;
        }

        /// <summary>
        /// Gets or sets the flag personal or group.
        /// </summary>
        /// <value>Is personal or group.</value>
        public SavedSearches IsPersonalOrGroup
        {
            get
            {
                return EnumMapper.MapStringToSavedSearches(Get(ODSConstants.KEY_PERSONAL_OR_GROUP));
            }
            set
            {
                Add(ODSConstants.KEY_PERSONAL_OR_GROUP, EnumMapper.MapSavedSearchesToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the share type (for group saved searches only).
        /// </summary>
        /// <value>The share type.</value>
        public ShareTypeOption ShareType
        {
            get
            {
                return EnumMapper.MapStringToShareTypeOption(Get(ODSConstants.KEY_SHARE_TYPE));
            }
            set
            {
                Add(ODSConstants.KEY_SHARE_TYPE, EnumMapper.MapShareTypeOptionToString(value));
            }
        }

        public override IDictionary<string, string> GetKeyValues
        {
            get
            {
                if (IsPersonalOrGroup != SavedSearches.Group && ShareType == ShareTypeOption.Assigned)
                {
                    throw new DowJonesUtilitiesException("ShareType = Assigned applicable only if IsPersonalOrGroup = Group.");
                }

                return base.GetKeyValues;
            }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the simple search page (SB) in dotcom product.
    /// </summary>
    public class SimpleSearchOperationalData : BaseCoreSearchOperationalData
    {
        public SimpleSearchOperationalData()
        {
            SearchPage = SearchPageType.SimpleSearch;
            IsSavedSearch = false;
        }

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
    }

    /// <summary>
    /// Represents the OperationalData for the saved simple search page (SB) "Simple search history" in dotcom product.
    /// </summary>
    public class SavedSimpleSearchOperationalData : BaseCoreSearchOperationalData
    {
        public SavedSimpleSearchOperationalData()
        {
            SearchPage = SearchPageType.SimpleSearch;
            IsSavedSearch = true;
        }

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
    }

    /// <summary>
    /// Represents the OperationalData for the iWorks page (PHP) in iWorks/dotcom product.
    /// </summary>
    [System.Obsolete("use SimpleSearchOperationalData", true)]
    public class iWorksOperationalData : BaseCoreSearchOperationalData
    {
        public iWorksOperationalData()
        {
            SearchPage = SearchPageType.iWorks;
        }
    }

    /// <summary>
    /// Represents the OperationalData for the Newsstand page (NP) in dotcom product.
    /// </summary>
    public class NewsstandOperationalData : BaseSearchOperationalData
    {
        public NewsstandOperationalData()
        {
            SearchPage = SearchPageType.NewsstandSearch;
        }

        /// <summary>
        /// Gets or sets the lead sentence display option.
        /// </summary>
        /// <value>The lead sentence display option.</value>
        public LeadSentenceDisplayOption LeadSentenceDisplay
        {
            get
            {
                return EnumMapper.MapStringToLeadSentenceDisplayOption(Get(ODSConstants.KEY_LEAD_SENTENCE_DISPLAY));
            }
            set
            {
                Add(ODSConstants.KEY_LEAD_SENTENCE_DISPLAY, EnumMapper.MapLeadSentenceDisplayOptionToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the number of duplicate headlines returned per page.
        /// </summary>
        /// <value>The number of duplicate headlines returned per page.</value>
        public string DuplicateHeadlinePerPage
        {
            get { return Get(ODSConstants.KEY_NUMBER_OF_DUPLICATE_HEADLINES); }
            set { Add(ODSConstants.KEY_NUMBER_OF_DUPLICATE_HEADLINES, value); }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the Factiva Mobile product.
    /// </summary>
    [System.Obsolete("use SimpleSearchOperationalData", true)]
    public class FactivaMobileOperationalData : BaseSearchOperationalData
    {
        public FactivaMobileOperationalData()
        {
            SearchPage = SearchPageType.FactivaMobile;
        }

        /// <summary>
        /// Gets or sets the flag language included.
        /// </summary>
        /// <value>Is language included.</value>
        public bool IsLanguageIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_LANGUAGE_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_LANGUAGE_INCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the number of duplicate headlines returned per page.
        /// </summary>
        /// <value>The number of duplicate headlines returned per page.</value>
        public string DuplicateHeadlinePerPage
        {
            get { return Get(ODSConstants.KEY_NUMBER_OF_DUPLICATE_HEADLINES); }
            set { Add(ODSConstants.KEY_NUMBER_OF_DUPLICATE_HEADLINES, value); }
        }

    }

    /// <summary>
    /// Represents the OperationalData for the simple search pages (News) in DJCE product.
    /// </summary>
    [System.Obsolete("use SimpleSearchOperationalData", true)]
    public class DJCESimpleOperationalData : BaseSearchOperationalData
    {
        public DJCESimpleOperationalData()
        {
            SearchPage = SearchPageType.DJCE_SimpleSearch;
        }

        /// <summary>
        /// Gets or sets the flag language included.
        /// </summary>
        /// <value>Is language included.</value>
        public bool IsLanguageIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_LANGUAGE_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_LANGUAGE_INCLUDED, v);
            }
        }
        
        /// <summary>
        /// Gets or sets the lead sentence display option.
        /// </summary>
        /// <value>The lead sentence display option.</value>
        public LeadSentenceDisplayOption LeadSentenceDisplay
        {
            get
            {
                return EnumMapper.MapStringToLeadSentenceDisplayOption(Get(ODSConstants.KEY_LEAD_SENTENCE_DISPLAY));
            }
            set
            {
                Add(ODSConstants.KEY_LEAD_SENTENCE_DISPLAY, EnumMapper.MapLeadSentenceDisplayOptionToString(value));
            }
        }
    }

    /// <summary>
    /// Represents the OperationalData for the advanced search pages (News sa) in DJCE product.
    /// </summary>
    [System.Obsolete("use SearchFormOperationalData", true)]
    public class DJCEAdvancedOperationalData : BaseSearchOperationalData
    {
        public DJCEAdvancedOperationalData()
        {
            SearchPage = SearchPageType.DJCE_AdvanceSearch;
        }

        /// <summary>
        /// Gets or sets the sources included.
        /// </summary>
        /// <value>The sources included.</value>
        public SourceList SourcesIncluded
        {
            get
            {
                return EnumMapper.MapStringToSourceList(Get(ODSConstants.KEY_SOURCES_INCLUDED));
            }
            set
            {
                Add(ODSConstants.KEY_SOURCES_INCLUDED, EnumMapper.MapSourceListToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the companies included.
        /// </summary>
        /// <value>The companies included.</value>
        public CompanyList CompaniesIncluded
        {
            get
            {
                return EnumMapper.MapStringToCompanyList(Get(ODSConstants.KEY_COMPANIES_INCLUDED));
            }
            set
            {
                Add(ODSConstants.KEY_COMPANIES_INCLUDED, EnumMapper.MapCompanyListToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the flag language included.
        /// </summary>
        /// <value>Is language included.</value>
        public bool IsLanguageIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_LANGUAGE_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_LANGUAGE_INCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the lead sentence display option.
        /// </summary>
        /// <value>The lead sentence display option.</value>
        public LeadSentenceDisplayOption LeadSentenceDisplay
        {
            get
            {
                return EnumMapper.MapStringToLeadSentenceDisplayOption(Get(ODSConstants.KEY_LEAD_SENTENCE_DISPLAY));
            }
            set
            {
                Add(ODSConstants.KEY_LEAD_SENTENCE_DISPLAY, EnumMapper.MapLeadSentenceDisplayOptionToString(value));
            }
        }
    }

}