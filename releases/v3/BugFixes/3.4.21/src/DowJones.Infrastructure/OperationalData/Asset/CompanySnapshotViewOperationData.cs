using System.Collections.Generic;
using System.Xml.Serialization;
using EMG.Utility.OperationalData.Asset;

namespace EMG.Utility.OperationalData.CompanySnapshotView
{
    public class CompanySnapshotViewOperationData : BaseAssetOperationalData
    {
       

        /// <summary>
        /// Gets or sets the article origin (from where the article was viewed). This is required.
        /// </summary>
        /// <value>The article origin.</value>
        public CompanyType CompType
        {
            get
            {
                return MapCompanyType(Get(ODSConstants.FCS_OD_COMPANY_TYPE));
            }
            set
            {
                Add(ODSConstants.FCS_OD_COMPANY_TYPE, MapCompanyType(value));
            }
        }
        public string CompName
        {
            get { return Get(ODSConstants.FCS_OD_COMPANY_NAME); }
            set { Add(ODSConstants.FCS_OD_COMPANY_NAME, value); }
        }

        public string FromExtendedUniverse
        {
            get { return Get(ODSConstants.FCS_OD_FROM_EXTENDED_UNIVERSE); }
            set { Add(ODSConstants.FCS_OD_FROM_EXTENDED_UNIVERSE, value); }
        }

        public string RequestorIP
        {
            get { return Get(ODSConstants.FCS_OD_REQUESTOR_IP); }
            set { Add(ODSConstants.FCS_OD_REQUESTOR_IP, value); }
        }

        public CompanySnapshotViewOperationData() { }

        protected CompanySnapshotViewOperationData(IDictionary<string, string> list) : base(list) { }

        #region EnumRegion
        public enum CompanyType
        {
            Private,
            Public,
            NotApplicable
        }
        #endregion

        #region MapperRegion
        public static CompanyType MapCompanyType(string type)
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
        public static string MapCompanyType(CompanyType type)
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
