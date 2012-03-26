using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Utilities.OperationalData.Asset;

namespace DowJones.Utilities.OperationalData.IndustrySnapshotView
{
    public class IndustrySnapshotViewOperationalData : BaseAssetOperationalData
    {
       
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string IndustryName
        {
            get { return Get(ODSConstants.FCS_OD_INDUSTRY_NAME); }
            set { Add(ODSConstants.FCS_OD_INDUSTRY_NAME, value); }
        }

        // <summary>
        /// Gets or sets the requestor IP.
        /// </summary>
        /// <value>The requestor IP.</value>
        public string RequestorIP
        {
            get { return Get(ODSConstants.FCS_OD_REQUESTOR_IP); }
            set { Add(ODSConstants.FCS_OD_REQUESTOR_IP, value); }
        }

        public IndustrySnapshotViewOperationalData() { }

        protected IndustrySnapshotViewOperationalData(IDictionary<string, string> list) : base(list) { }

        
    }
}
