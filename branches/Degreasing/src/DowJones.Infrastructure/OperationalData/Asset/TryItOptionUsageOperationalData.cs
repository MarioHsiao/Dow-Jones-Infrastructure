using System.Collections.Generic;

namespace DowJones.OperationalData.Asset
{
    public class TryItOptionUsageOperationalData : BaseAssetOperationalData
    {
        /// <summary>
        /// Gets or sets the requestor IP.
        /// </summary>
        /// <value>The requestor IP.</value>
        public string RequestorIP
        {
            get { return Get(ODSConstants.FCS_OD_REQUESTOR_IP); }
            set { Add(ODSConstants.FCS_OD_REQUESTOR_IP, value); }
        }

        public TryItOptionUsageOperationalData() { }

        protected TryItOptionUsageOperationalData(IDictionary<string, string> list) : base(list) { }

        
    }
}
