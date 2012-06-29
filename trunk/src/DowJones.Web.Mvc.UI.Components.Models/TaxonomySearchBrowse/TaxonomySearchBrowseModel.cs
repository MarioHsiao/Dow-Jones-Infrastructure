using System.Collections.Generic;
using DowJones.Properties;

namespace DowJones.Web.Mvc.UI.Components.TaxonomySearchBrowse
{
    /*
    Search and Browse lookup component to lookup industry or region if it is not found in autocomplete.
    This is being developed as a requirement for Snapshot Personalization.
    */
    public enum TaxonomyType
    {
        Industry,
        Region
    }

    public class TaxonomySearchBrowseModel : ViewComponentModel
    {
        /// <summary>
        /// Gets or sets the TaxonomyType.
        /// </summary>
        [ClientProperty("taxonomyType")]
        public TaxonomyType TaxonomyType { get; set; }

        /// <summary>
        /// Gets or sets the TaxonomyServiceUrl.
        /// </summary>
        [ClientProperty("taxonomyServiceUrl")]
        public string TaxonomyServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the parent codes.
        /// </summary>
        [ClientProperty("parentCodes")]
        public IEnumerable<string> ParentCodes { get; set; }

        /// <summary>
        /// Gets or sets the text entered by user.
        /// </summary>
        [ClientProperty("lookupText")]
        public string LookupText { get; set; }


        public TaxonomySearchBrowseModel()
        {
            TaxonomyServiceUrl = Settings.Default.TaxonomyServiceUrl;
        }
    }
}
