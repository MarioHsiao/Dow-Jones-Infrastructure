
namespace EMG.Utility.OperationalData.EntryPoint.AssetActivity
{
    public class WorkspaceViewOperationalData : BaseAssetViewOperationalData
    {

        ///// <summary>
        ///// Workspace Publishing Domain 
        ///// </summary>
        //public string PublishingDomain
        //{
        //    get { return Get(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN); }
        //    set { Add(ODSConstants.KEY_VIEW_PUBLISHING_DOMAIN, value); }
        //}

        public WorkspaceViewOperationalData()
        {
            AssetType = "WS";
            AssetAction = "Publish";
        }

    }
}
