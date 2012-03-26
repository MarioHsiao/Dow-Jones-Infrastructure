using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Utilities.OperationalData.Asset;

namespace DowJones.Utilities.OperationalData.CompanySnapshotView
{
    public class CompanySnapshotViewOperationalData : BaseAssetOperationalData
    {

        /// <summary>
        /// Gets or sets the type of the company.
        /// </summary>
        /// <value>The type of the company.</value>
        public CompanyType CompType
        {
            get
            {
                return MapStringToCompanyType(Get(ODSConstants.FCS_OD_COMPANY_TYPE));
            }
            set
            {
                Add(ODSConstants.FCS_OD_COMPANY_TYPE, MapCompanyTypeToString(value));
            }
        }
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompName
        {
            get { return Get(ODSConstants.FCS_OD_COMPANY_NAME); }
            set { Add(ODSConstants.FCS_OD_COMPANY_NAME, value); }
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

        public CompanySnapshotViewOperationalData() { }

        protected CompanySnapshotViewOperationalData(IDictionary<string, string> list) : base(list) { }

        #region EnumRegion
        public enum CompanyType
        {
            Private,
            Public,
            NotApplicable
        }
        #endregion

        #region MapperRegion
        public static CompanyType MapStringToCompanyType(string type)
        {
            switch (type)
            {
                case "Private":
                    return CompanyType.Private;
                case "Public":
                    return CompanyType.Public;
                default:
                    return CompanyType.NotApplicable;

            }
        }
        public static string MapCompanyTypeToString(CompanyType type)
        {
            switch (type)
            {
                case CompanyType.Private:
                    return "Private";
                case CompanyType.Public:
                    return "Public";
                default:
                    return "";
            }
        }
        #endregion
    }
}
