namespace DowJones.OperationalData.EntryPoint
{
    public class CoreAlertOperationalData : AbstractOperationalData
    {
        public string AlertID
        {
            get { return Get(ODSConstants.KEY_ALERT_ID); }
            set { Add(ODSConstants.KEY_ALERT_ID, value); }
        }
        public string AlertName
        {
            get { return Get(ODSConstants.KEY_ALERT_NAME); }
            set { Add(ODSConstants.KEY_ALERT_NAME, value); }
        }
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
    }
}
