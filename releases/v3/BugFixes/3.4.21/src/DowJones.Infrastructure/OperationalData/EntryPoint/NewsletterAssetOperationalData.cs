namespace DowJones.OperationalData.EntryPoint
{
    public class NewsletterAssetOperationalData : BaseEntryPointOperationalData
    {
        private BaseWidgetOperationalData _baseWidgetOperationalData;
        private BaseRssOperationalData _baseRssOperationalData;
        private EdtionOperationalData _baseEditionOperationalData;


        public NewsletterAssetOperationalData() : this(EntryPoint.DisseminationMethod.UnSpecified)
        {
           
        }

        public NewsletterAssetOperationalData(DisseminationMethod disseminationMethod) 
        {
            AssetType = "NL";
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
                case EntryPoint.DisseminationMethod.Edition:
                    DisseminationMethod = "ED";
                    break;
            }
        }


        public BaseWidgetOperationalData WidgetOperationalData
        {
            get { return _baseWidgetOperationalData ?? (_baseWidgetOperationalData = new BaseWidgetOperationalData(List)); }
        }

        public BaseRssOperationalData RssOperationalData
        {
            get { return _baseRssOperationalData ?? (_baseRssOperationalData = new BaseRssOperationalData(List)); }
        }

        public EdtionOperationalData EdtionOperationalData
        {
            get { return _baseEditionOperationalData ?? (_baseEditionOperationalData = new EdtionOperationalData(List)); }
        }

    }
}