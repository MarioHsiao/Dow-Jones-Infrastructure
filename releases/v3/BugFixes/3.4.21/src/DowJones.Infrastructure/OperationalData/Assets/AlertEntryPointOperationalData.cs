namespace EMG.Utility.OperationalData
{
    public class AlertOperationalData : BaseAssetOperationalData
    {
        private BaseWidgetOperationalData _baseWidgetOperationalData;
        private BaseRssOperationalData _baseRssOperationalData;
        
        public AlertOperationalData(DisseminationMethod disseminationMethod)
        {
            AssetType = "AL";
            switch(disseminationMethod)
            {
                case OperationalData.DisseminationMethod.PodCast:
                    DisseminationMethod = "PCast";
                    RssOperationalData.RssType = "Private";
                    break;
                case OperationalData.DisseminationMethod.Rss:
                    DisseminationMethod = "RSS";
                    RssOperationalData.RssType = "Private";
                    break;
                case OperationalData.DisseminationMethod.Widget:
                    DisseminationMethod = "WID";
                    break;
                default:
                    break;
            }
        }

        public AlertOperationalData() : this(OperationalData.DisseminationMethod.UnSpecified)
        {
        }


        /// <summary>
        /// Alert type or Folder type, S2.0,FCE,SW,IWE,GBl,...
        /// </summary>
        public string AlertType
        {
            get { return Get(ODSConstants.KEY_ALERT_TYPE); }
            set { Add(ODSConstants.KEY_ALERT_TYPE, value); }
        }
        public string NumberOfItems
        {
            get { return Get(ODSConstants.KEY_NUMBER_OF_ITEMS); }
            set { Add(ODSConstants.KEY_NUMBER_OF_ITEMS, value); }
        }

        public BaseWidgetOperationalData WidgetOperationalData
        {
            get
            {
                if (_baseWidgetOperationalData == null)
                {
                    _baseWidgetOperationalData = new BaseWidgetOperationalData(List);
                }
                return _baseWidgetOperationalData;
            }
        }

        public BaseRssOperationalData RssOperationalData
        {
            get
            {
                if (_baseRssOperationalData == null)
                {
                    _baseRssOperationalData = new BaseRssOperationalData(List);
                }
                return _baseRssOperationalData;
            }
        }
    }
}