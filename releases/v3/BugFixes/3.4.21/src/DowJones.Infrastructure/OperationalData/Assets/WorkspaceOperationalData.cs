

namespace EMG.Utility.OperationalData
{
    public class WorkspaceOperationalData : BaseAssetOperationalData
    {
        private BaseWidgetOperationalData _baseWidgetOperationalData;
        private BaseRssOperationalData _baseRssOperationalData;


        public WorkspaceOperationalData(): this(OperationalData.DisseminationMethod.UnSpecified)
        {
        }

        public WorkspaceOperationalData(DisseminationMethod disseminationMethod) 
        {
            AssetType = "WS";
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
