namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class AlertAssetOperationalData : BaseEntryPointOperationalData
    {
        private BaseWidgetOperationalData _baseWidgetOperationalData;
        private BaseRssOperationalData _baseRssOperationalData;
        
        public AlertAssetOperationalData(DisseminationMethod disseminationMethod)
        {
            AssetType = "AL";
            switch(disseminationMethod)
            {
                case EntryPoint.DisseminationMethod.PodCast:
                    DisseminationMethod = "PODCAST";
                    RssOperationalData.RssType = "Private";
                    break;
                case EntryPoint.DisseminationMethod.Rss:
                    DisseminationMethod = "RSS";
                    RssOperationalData.RssType = "Private";
                    break;
                case EntryPoint.DisseminationMethod.Widget:
                    DisseminationMethod = "WID";
                    break;
                default:
                    break;
            }
        }

        public AlertAssetOperationalData()
            : this(EntryPoint.DisseminationMethod.UnSpecified)
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