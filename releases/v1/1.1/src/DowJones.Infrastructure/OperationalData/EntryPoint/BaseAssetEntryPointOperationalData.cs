namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class BaseEntryPointOperationalData : AbstractOperationalData
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        /// <value>The asset id.</value>
        public string AssetId
        {
            get { return Get(ODSConstants.KEY_ASSET_ID); }
            set { Add(ODSConstants.KEY_ASSET_ID, value); }
        }

        /// <summary>
        /// Gets or sets the type of the asset.
        /// </summary>
        /// <value>The type of the asset.</value>
        public string AssetType
        {
            get { return Get(ODSConstants.KEY_ASSET_TYPE); }
            set { Add(ODSConstants.KEY_ASSET_TYPE, value); }
        }

        /// <summary>
        /// Gets or sets the asset action.
        /// </summary>
        /// <value>The asset action.</value>
        public string AssetName
        {
            get { return Get(ODSConstants.KEY_ASSET_NAME); }
            set { Add(ODSConstants.KEY_ASSET_NAME, value); }
        }

        /// <summary>
        /// Show Article, View Folder, or Manage Alert (this is assumed by the product page adding these values when not provided)
        /// Values: sa, va or ma
        /// </summary>
        public string LinkType
        {
            get { return Get(ODSConstants.KEY_LINK_TYPE); }
            set { Add(ODSConstants.KEY_LINK_TYPE, value); }
        }

        /// <summary>
        /// Dissemination Option * xrdr requires additional parameters, see next heading
        /// inacct, outacct, ttlt, xrdr
        /// </summary>
        protected string DisseminationMethod
        {
            get { return Get(ODSConstants.KEY_DISSEMINATION_METHOD); }
            set { Add(ODSConstants.KEY_DISSEMINATION_METHOD, value); }
        }


        public string AudienceOption
        {
            get { return Get(ODSConstants.KEY_AUDIENCE_OPTION); }
            set { Add(ODSConstants.KEY_AUDIENCE_OPTION, value); }
        }
    }
}