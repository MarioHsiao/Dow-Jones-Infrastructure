

namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class WorkspaceAssetOperationalData : BaseEntryPointOperationalData
    {
        private BaseWidgetOperationalData _baseWidgetOperationalData;
        private BaseRssOperationalData _baseRssOperationalData;


        public WorkspaceAssetOperationalData(): this(OperationalData.EntryPoint.DisseminationMethod.UnSpecified)
        {
        }

        public WorkspaceAssetOperationalData(DisseminationMethod disseminationMethod) 
        {
            AssetType = "WS";
            switch(disseminationMethod)
            {
                case OperationalData.EntryPoint.DisseminationMethod.PodCast:
                    DisseminationMethod = "PODCAST";
                    RssOperationalData.RssType = "Private";
                    break;
                case OperationalData.EntryPoint.DisseminationMethod.Rss:
                    DisseminationMethod = "RSS";
                    RssOperationalData.RssType = "Private";
                    break;
                case OperationalData.EntryPoint.DisseminationMethod.Widget:
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
