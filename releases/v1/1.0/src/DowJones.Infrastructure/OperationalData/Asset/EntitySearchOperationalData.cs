using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Utilities.OperationalData.Asset;

namespace DowJones.Utilities.OperationalData.EntitySearch
{
    public class EntitySearchOperationalData : BaseAssetOperationalData
    {
       
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string EntityType
        {
            get { return Get(ODSConstants.FCS_OD_ENTITY_TYPE); }
            set { Add(ODSConstants.FCS_OD_ENTITY_TYPE, value); }
        }

        /// <summary>
        /// Gets or sets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        public string SearchType
        {
            get { return Get(ODSConstants.FCS_OD_SEARCH_TYPE); }
            set { Add(ODSConstants.FCS_OD_SEARCH_TYPE, value); }
        }

        /// <summary>
        /// Gets or sets from extended universe.
        /// </summary>
        /// <value>From extended universe.</value>
        public FromExtendedUniverseFlag FromExtendedUniverse
        {
            get { return MapStringToFromExtendedUniverseFlag(Get(ODSConstants.FCS_OD_FROM_EXTENDED_UNIVERSE)); }
            set { Add(ODSConstants.FCS_OD_FROM_EXTENDED_UNIVERSE, MapFromExtendedUniverseFlagToString(value)); }
        }

        /// <summary>
        /// Gets or sets the requestor IP.
        /// </summary>
        /// <value>The requestor IP.</value>
        public string RequestorIP
        {
            get { return Get(ODSConstants.FCS_OD_REQUESTOR_IP); }
            set { Add(ODSConstants.FCS_OD_REQUESTOR_IP, value); }
        }

        public EntitySearchOperationalData() { }

        protected EntitySearchOperationalData(IDictionary<string, string> list) : base(list) { }

        
    }
}
