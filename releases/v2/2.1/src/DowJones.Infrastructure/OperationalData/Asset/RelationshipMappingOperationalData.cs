using System.Collections.Generic;

namespace DowJones.OperationalData.Asset
{
    public class RelationshipMappingOperationalData : BaseAssetOperationalData
    {

        public string RelationFrom
        {
            get { return Get(ODSConstants.KEY_RELATION_FROM); }
            set { Add(ODSConstants.KEY_RELATION_FROM, value); }
        }

        public string RelationTo
        {
            get { return Get(ODSConstants.KEY_RELATION_TO); }
            set { Add(ODSConstants.KEY_RELATION_TO, value); }
        }


        public RelationshipMappingOperationalData()
        {
        }

        protected RelationshipMappingOperationalData(IDictionary<string, string> list) : base(list) { }
    }
}
