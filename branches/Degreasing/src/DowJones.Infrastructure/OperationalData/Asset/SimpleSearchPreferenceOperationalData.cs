using System.Collections.Generic;

namespace DowJones.OperationalData.Asset
{
    public class SimpleSearchPreferenceOperationalData : BaseAssetOperationalData
    {
        /// <summary>
        /// Gets or sets the preference value.
        /// </summary>
        /// <value>The preference value.</value>
        public PreferenceType PreferenceValue
        {
            get
            {
                return MapStringToPreferenceType(Get(ODSConstants.FCS_OD_PREFERENCE_VALUE));
            }
            set
            {
                Add(ODSConstants.FCS_OD_PREFERENCE_VALUE, MapPreferenceTypeToString(value));
            }
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

        public SimpleSearchPreferenceOperationalData() { }

        protected SimpleSearchPreferenceOperationalData(IDictionary<string, string> list) : base(list) { }

        #region EnumRegion
        public enum PreferenceType
        {
            HomePage,
            DefaultSearch,
            NotApplicable
        }
        #endregion

        #region MapperRegion
        public static PreferenceType MapStringToPreferenceType(string type)
        {
            switch (type)
            {
                case "HomePage":
                    return PreferenceType.HomePage;
                case "DefaultSearch":
                    return PreferenceType.DefaultSearch;
                default:
                    return PreferenceType.NotApplicable;

            }
        }
        public static string MapPreferenceTypeToString(PreferenceType type)
        {
            switch (type)
            {
                case PreferenceType.HomePage:
                    return "HomePage";
                case PreferenceType.DefaultSearch:
                    return "DefaultSearch";
                default:
                    return "";
            }
        }
        #endregion
        
    }
}
