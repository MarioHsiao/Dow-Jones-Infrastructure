namespace DowJones.OperationalData.AssetActivity
{
    public class BaseAssetActivityOperationalData : AbstractOperationalData
    {
        private BaseCommonRequestOperationalData _commonOperationalData;

        public BaseAssetActivityOperationalData ()
        {
            AssetAction = "View";
        }
        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        /// <value>The asset id.</value>
        public string AssetId
        {
            get { return Get(ODSConstants.KEY_VIEW_ASSET_ID); }
            set { Add(ODSConstants.KEY_VIEW_ASSET_ID, value); }
        }

        /// <summary>
        /// Gets or sets the type of the asset.
        /// </summary>
        /// <value>The type of the asset.</value>
        protected string AssetType
        {
            get { return Get(ODSConstants.KEY_VIEW_ASSET_TYPE); }
            set { Add(ODSConstants.KEY_VIEW_ASSET_TYPE, value); }
        }

        /// <summary>
        /// Gets or sets the asset action.
        /// </summary>
        /// <value>The asset action.</value>
        protected string AssetAction
        {
            get { return Get(ODSConstants.KEY_VIEW_ASSET_ACTION); }
            set { Add(ODSConstants.KEY_VIEW_ASSET_ACTION, value); }
        }


        public BaseCommonRequestOperationalData CommonOperationalData
        {
            get
            {
                if (_commonOperationalData == null)
                {
                    _commonOperationalData = new BaseCommonRequestOperationalData(List);
                }
                return _commonOperationalData;
            }
        }
    }
}
