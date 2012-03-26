
namespace DowJones.Utilities.OperationalData.AssetActivity
{
    public class AlertViewOperationalData : BaseAssetActivityOperationalData
    {

        /// <summary>
        /// Alert Publishing Domain 
        /// </summary>
        public string PublishingDomain
        {
            get { return Get(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN); }
            set { Add(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN, value); }
        }
        
        public AlertViewOperationalData()
        {
            AssetType = "AL";
        }
    }
}
