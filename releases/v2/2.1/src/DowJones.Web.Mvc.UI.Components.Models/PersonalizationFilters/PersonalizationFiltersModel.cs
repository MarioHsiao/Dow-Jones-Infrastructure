using System.Collections.Generic;
using System.Linq;
using DowJones.Properties;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.TaxonomySearchBrowse;

namespace DowJones.Web.Mvc.UI.Components.PersonalizationFilters
{
    public enum FilterLevel
    {
        Page,
        Module
    }

    public enum LensType
    {
        None,
        Industry,
        Region
    }


    public class PersonalizationFiltersModel : CompositeComponentModel
    {
        #region Client Properties

        /// <summary>
        /// Gets or sets the SuggestServiceUrl.
        /// </summary>
        [ClientProperty("suggestServiceUrl")]
        public string SuggestServiceUrl { get; set; }

        [ClientProperty("fiiCodeServiceUrl")]
        public string FIICodeServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets whether this is a page or module filter.
        /// </summary>
        [ClientProperty("filterLevel")]
        public FilterLevel FilterLevel { get; set; }

        /// <summary>
        /// Gets or sets whether this is a component is disabled.
        /// </summary>
        [ClientProperty("disabled")]
        public bool Disabled { get; set; }

        #endregion

        #region Client Data

        /// <summary>
        /// Gets or sets the type of lens.
        /// </summary>
        [ClientData("lensType")]
        public LensType LensType { get; set; }

        /// <summary>
        /// Gets or sets the list of industry/region codes for the selected lens.
        /// </summary>
        [ClientData("parentCodes")]
        public IEnumerable<string> ParentCodes
        {
            get
            {
                if (TaxonomySearch == null)
                    return Enumerable.Empty<string>();

                return TaxonomySearch.ParentCodes;
            }
            set
            {
                if (TaxonomySearch == null)
                    return;
                
                TaxonomySearch.ParentCodes = value;
            }
        }

        /// <summary>
        /// Gets or sets the industry filter.
        /// </summary>
        [ClientData("industryFilter")]
        public CodeDesc IndustryFilter { get; set; }

        /// <summary>
        /// Gets or sets the region filter.
        /// </summary>
        [ClientData("regionFilter")]
        public CodeDesc RegionFilter { get; set; }

        /// <summary>
        /// Gets or sets the keyword filter.
        /// </summary>
        [ClientData("keywordFilter")]
        public string KeywordFilter { get; set; }

        #endregion

        internal TaxonomySearchBrowseModel TaxonomySearch { get; set; }


        public PersonalizationFiltersModel()
        {
            SuggestServiceUrl = Settings.Default.SuggestServiceURL;
            TaxonomySearch = new TaxonomySearchBrowseModel();
        }
    }
}
