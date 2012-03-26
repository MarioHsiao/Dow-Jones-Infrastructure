
namespace DowJones.Utilities.OperationalData.AssetActivity
{
    public class WidgetViewOperationalData : BaseAssetActivityOperationalData
    {
        /// <summary>
        /// Widget Publishing Domain 
        /// </summary>
        public string PublishingDomain
        {
            get { return Get(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN); }
            set { Add(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN, value); }
        }


        public WidgetViewOperationalData()
        {
            AssetType = "WD";
        }
    }
}
