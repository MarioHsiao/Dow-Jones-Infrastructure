namespace DowJones.OperationalData.EntryPoint
{
    public class NewsletterAssetOperationalData : BaseEntryPointOperationalData
    {
        private BaseWidgetOperationalData _baseWidgetOperationalData;
        private BaseRssOperationalData _baseRssOperationalData;
        private EdtionOperationalData _baseEditionOperationalData;


        public NewsletterAssetOperationalData() : this(OperationalData.EntryPoint.DisseminationMethod.UnSpecified)
        {
           
        }

        public NewsletterAssetOperationalData(DisseminationMethod disseminationMethod) 
        {
            AssetType = "NL";
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
                case OperationalData.EntryPoint.DisseminationMethod.Edition:
                    DisseminationMethod = "ED";
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

        public EdtionOperationalData EdtionOperationalData
        {
            get
            {
                if (_baseEditionOperationalData == null)
                {
                    _baseEditionOperationalData = new EdtionOperationalData(List);
                }
                return _baseEditionOperationalData;
            }
        }

    }
}