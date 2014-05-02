
namespace EMG.Utility.OperationalData.EntryPoint.AssetActivity
{
    public class NewsletterViewOperationalData : BaseAssetViewOperationalData
    {

        ///// <summary>
        ///// Mewsletter Publishing Domain 
        ///// </summary>
        //public string PublishingDomain
        //{
        //    get { return Get(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN); }
        //    set { Add(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN, value); }
        //}

        public NewsletterViewOperationalData()
        {
            AssetType = "NL";
            AssetAction = "Publish";
        }

    }
}
