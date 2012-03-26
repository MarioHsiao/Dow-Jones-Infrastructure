namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class EmailOperationalData : CoreAlertOperationalData
    {
        /// <summary>
        /// Email format ht, pt, wf(html, plain text, wireless friendly)
        /// </summary>
        public string EmailFormat
        {
            get { return Get(ODSConstants.KEY_FORMAT); }
            set { Add(ODSConstants.KEY_FORMAT, value); }
        }

        /// <summary>
        /// Delivery type con, bdl,sch, ode (continuous, scheduled, bundled, on-demand)
        /// </summary>
        public string DeliveryType
        {
            get { return Get(ODSConstants.KEY_DELIVERY_TYPE); }
            set { Add(ODSConstants.KEY_DELIVERY_TYPE, value); }
        }

        /// <summary>
        /// Email disposition: att or inl (attachment or inline)
        /// </summary>
        public string Disposition
        {
            get { return Get(ODSConstants.KEY_DISPOSITION); }
            set { Add(ODSConstants.KEY_DISPOSITION, value); }
        }

        /// <summary>
        /// Show Article, View Folder, or Manage Alert (this is assumed by the product page adding these values when not provided)
        /// Values: sa, vf or ma
        /// </summary>
        public string LinkType
        {
            get { return Get(ODSConstants.KEY_LINK_TYPE); }
            set { Add(ODSConstants.KEY_LINK_TYPE, value); }
        }

        /// <summary>
        /// Radar alert ID
        /// </summary>
        public string RadarAlertID
        {
            get { return Get(ODSConstants.KEY_RADAR_ALERT_ID); }
            set { Add(ODSConstants.KEY_RADAR_ALERT_ID, value); }
        }

        /// <summary>
        /// Radar view ID
        /// </summary>
        public string RadarViewID
        {
            get { return Get(ODSConstants.KEY_VIEW_ID); }
            set { Add(ODSConstants.KEY_VIEW_ID, value); }
        }

        /// <summary>
        /// Radar list ID(Company or screening)
        /// </summary>
        public string RadarListID
        {
            get { return Get(ODSConstants.KEY_LIST_ID); }
            set { Add(ODSConstants.KEY_LIST_ID, value); }
        }

        /// <summary>
        /// List type associated with SLID
        /// CompanyList, SharedCompanyList, ScreeningCriteria, MarketIndex
        /// </summary>
        public string RadarListType
        {
            get { return Get(ODSConstants.KEY_LIST_TYPE); }
            set { Add(ODSConstants.KEY_LIST_TYPE, value); }
        }
 
   }
}