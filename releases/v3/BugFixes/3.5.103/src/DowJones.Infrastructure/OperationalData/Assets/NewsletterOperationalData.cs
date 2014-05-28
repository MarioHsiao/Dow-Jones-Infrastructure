using EMG.Utility.OperationalData.Assets;

namespace EMG.Utility.OperationalData
{
    public class NewsletterOperationalData : BaseAssetOperationalData
    {
        private BaseWidgetOperationalData _baseWidgetOperationalData;
        private BaseRssOperationalData _baseRssOperationalData;
        private EdtionOperationalData _baseEditionOperationalData;


        public NewsletterOperationalData() : this(OperationalData.DisseminationMethod.UnSpecified)
        {
           
        }

        public NewsletterOperationalData(DisseminationMethod disseminationMethod) 
        {
            AssetType = "NL";
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
                case OperationalData.DisseminationMethod.Edition:
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