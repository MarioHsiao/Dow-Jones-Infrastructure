using System.Collections.Generic;
using DowJones.Utilities.OperationalData.Asset;

namespace DowJones.Utilities.OperationalData.DashboardView
{
    public class SnapshotOperationData : BaseAssetOperationalData
    {

        public string SnapshotType
        {
            get { return Get(ODSConstants.KEY_SNAPSHOT_TYPE); }
            set { Add(ODSConstants.KEY_SNAPSHOT_TYPE, value); }
        }

        public SnapshotOperationData() { }

        protected SnapshotOperationData(IDictionary<string, string> list) : base(list) { }

    }
}
