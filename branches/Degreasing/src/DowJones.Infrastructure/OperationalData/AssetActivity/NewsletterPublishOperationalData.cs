namespace DowJones.OperationalData.AssetActivity
{
    public class NewsletterPublishOperationalData : BaseAssetActivityOperationalData
    {

        /// <summary>
        ///  Mewsletter Publishing Domain 
        /// </summary>
        public string PublishingDomain
        {
            get { return Get(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN); }
            set { Add(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN, value); }
        }

        public NewsletterPublishOperationalData()
        {
            AssetType = "NL";
            AssetAction = "Publish";
        }
    }
}
