using DowJones.OperationalData.EntryPoint;

namespace DowJones.OperationalData.Search
{
    public class BaseCoreSearchOperationalData : BaseSearchOperationalData
    {

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
        /// Gets or sets the flag subject included.
        /// </summary>
        /// <value>Is subject included.</value>
        public bool IsSubjectIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_SUBJECT_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_SUBJECT_INCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag industry included.
        /// </summary>
        /// <value>Is industry included.</value>
        public bool IsIndustryIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_INDUSTRY_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_INDUSTRY_INCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag region included.
        /// </summary>
        /// <value>Is region included.</value>
        public bool IsRegionIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_REGION_INCLUDED);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_REGION_INCLUDED, v);
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
        /// Gets or sets the flag Author included.
        /// </summary>
        /// <value>Is Author included.</value>
        public bool IsAuthorIncluded
        {
            get
            {
                string v = Get(ODSConstants.KEY_AUTHOR_INCLUDED);
                return (v != null && v == "1");
            }

            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_AUTHOR_INCLUDED, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag “User Consultant Lens” .
        /// </summary>
        /// <value>Is Author included.</value>
        public bool IsUserConsultantLensUsed
        {
            get
            {
                string v = Get(ODSConstants.KEY_USER_CONSULTANT_LENS);
                return (v != null && v == "1");
            }

            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_USER_CONSULTANT_LENS, v);
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

        /// <summary>
        /// Gets or sets the flag discovery filter date clicked.
        /// </summary>
        /// <value>Is discovery filter date clicked.</value>
        public bool IsDiscoveryFilterDateClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_DATE);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_DATE, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter company clicked.
        /// </summary>
        /// <value>Is discovery filter company clicked.</value>
        public bool IsDiscoveryFilterCompanyClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_COMPANY);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_COMPANY, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter source clicked.
        /// </summary>
        /// <value>Is discovery filter source clicked.</value>
        public bool IsDiscoveryFilterSourceClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_SOURCE);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_SOURCE, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter subject clicked.
        /// </summary>
        /// <value>Is discovery filter subject clicked.</value>
        public bool IsDiscoveryFilterSubjectClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_SUBJECT);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_SUBJECT, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter industry clicked.
        /// </summary>
        /// <value>Is discovery filter industry clicked.</value>
        public bool IsDiscoveryFilterIndustryClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_INDUSTRY);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_INDUSTRY, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter news cluster clicked.
        /// </summary>
        /// <value>Is discovery filter news cluster clicked.</value>
        public bool IsDiscoveryFilterNewsClusterClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_NEWSCLUSTER);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_NEWSCLUSTER, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter keyword clicked.
        /// </summary>
        /// <value>Is discovery filter keyword clicked.</value>
        public bool IsDiscoveryFilterKeywordClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_KEYWORD);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_KEYWORD, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter executive clicked.
        /// </summary>
        /// <value>Is discovery filter executive clicked.</value>
        public bool IsDiscoveryFilterExecutiveClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_EXECUTIVE);
                return (v != null && v == "1");
            }
            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_EXECUTIVE, v);
            }
        }

        /// <summary>
        /// Gets or sets the flag discovery filter author clicked.
        /// </summary>
        /// <value>Is discovery filter author clicked.</value>
        public bool IsDiscoveryFilterAuthorClicked
        {
            get
            {
                string v = Get(ODSConstants.KEY_DISCOVERY_FILTER_AUTHOR);
                return (v != null && v == "1");
            }

            set
            {
                string v = "0";
                if (value)
                {
                    v = "1";
                }
                Add(ODSConstants.KEY_DISCOVERY_FILTER_AUTHOR, v);
            }
        }


        /// <summary>
        /// Gets or sets the number of duplicate headlines returned per page.
        /// </summary>
        /// <value>The number of duplicate headlines returned per page.</value>
        public int DuplicateHeadlinePerPage
        {
            get { return int.Parse( Get(ODSConstants.KEY_NUMBER_OF_DUPLICATE_HEADLINES) ); }
            set { Add(ODSConstants.KEY_NUMBER_OF_DUPLICATE_HEADLINES, value.ToString( )); }
        }
    }
}