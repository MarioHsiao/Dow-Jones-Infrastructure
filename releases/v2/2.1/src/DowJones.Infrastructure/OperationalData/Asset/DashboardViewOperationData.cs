using System.Collections.Generic;

namespace DowJones.OperationalData.Asset
{
    public class DashboardViewOperationData : BaseAssetOperationalData
    {
        public string Dashboard
        {
            get { return Get(ODSConstants.KEY_DASHBOARD); }
            set { Add(ODSConstants.KEY_DASHBOARD, value); }
        }

        public DashboardViewOperationData()
        {
        }

        protected DashboardViewOperationData(IDictionary<string, string> list) : base(list) { }
    }
}
