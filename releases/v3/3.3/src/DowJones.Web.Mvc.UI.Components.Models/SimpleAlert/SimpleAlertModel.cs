namespace DowJones.Web.Mvc.UI.Components.SimpleAlert
{
    public class SimpleAlertModel : ViewComponentModel
    {
        SimpleAlertData _data;

        /// <summary>
        /// Gets or sets the selected Source index.
        /// </summary>
        [ClientProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the enable exclude.
        /// </summary>
        [ClientProperty("enableExclude")]
        public string EnableExclude { get; set; }

        [ClientData]
        public SimpleAlertData Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new SimpleAlertData();
                }
                else
                {
                    SimpleAlertData tempData = new SimpleAlertData();
                    if (_data.EmailFormats == null)
                    {
                        _data.EmailFormats = tempData.EmailFormats;
                        _data.SelectedEmailFormat = tempData.SelectedEmailFormat;
                    }

                    if (_data.Duplicates == null)
                    {
                        _data.Duplicates = tempData.Duplicates;
                        _data.SelectedDuplicate = tempData.SelectedDuplicate;
                    }

                    if (_data.DeliveryTimes == null)
                    {
                        _data.DeliveryTimes = tempData.DeliveryTimes;
                        _data.SelectedDeliveryTime = tempData.SelectedDeliveryTime;
                    }
                }

                return _data;
            }
            set
            {
                _data = value;
            }
        }

        /// <summary>
        /// Gets or sets the client side OnSave event handler.
        /// </summary>
        [ClientEventHandler("dj.SimpleAlert.save")]
        public string OnSave { get; set; }
    }
}
