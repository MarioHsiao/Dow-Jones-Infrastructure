using System.Collections.Generic;

namespace DowJones.OperationalData.Asset
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
