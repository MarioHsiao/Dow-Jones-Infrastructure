using System;
using System.Collections.Generic;
using System.Text;

namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class BaseWidgetOperationalData : AbstractOperationalData
    {

        public string WidgetID
        {
            get { return Get(ODSConstants.KEY_WIDGET_ID); }
            set { Add(ODSConstants.KEY_WIDGET_ID, value); }
        }

        public string WidgetName
        {
            get { return Get(ODSConstants.KEY_WIDGET_NAME); }
            set { Add(ODSConstants.KEY_WIDGET_NAME, value); }
        }

        public string PublisherID
        {
            get { return Get(ODSConstants.KEY_USER_ID); }
            set { Add(ODSConstants.KEY_USER_ID, value); }
        }

        public string PublisherNamespace
        {
            get { return Get(ODSConstants.KEY_USER_NAMESPACE); }
            set { Add(ODSConstants.KEY_USER_NAMESPACE, value); }
        }

        public string PublishingDomain
        {
            get { return Get(ODSConstants.KEY_DOMAIN); }
            set { Add(ODSConstants.KEY_DOMAIN, value); }
        }

        public string AssetCount
        {
            get { return Get(ODSConstants.KEY_ALERT_COUNT); }
            set { Add(ODSConstants.KEY_ALERT_COUNT, value); }
        }

        public string HeadlineFormat
        {
            get { return Get(ODSConstants.KEY_HEADLINE_VIEW); }
            set { Add(ODSConstants.KEY_HEADLINE_VIEW, value); }
        }

        public string NumberOfItems
        {
            get { return Get(ODSConstants.KEY_NUMBER_OF_ITEMS); }
            set { Add(ODSConstants.KEY_NUMBER_OF_ITEMS, value); }
        }


        public BaseWidgetOperationalData(IDictionary<string, string> list) : base(list)
        {

        }
    }
}
