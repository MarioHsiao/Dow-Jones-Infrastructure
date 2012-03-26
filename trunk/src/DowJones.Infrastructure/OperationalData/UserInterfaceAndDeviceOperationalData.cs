using System.Collections.Generic;

namespace DowJones.OperationalData
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInterfaceAndDeviceOperationalData : AbstractOperationalData
    {

        public string InterfaceLanguage
        {
            get { return Get("LNG"); }
            set { Add("LNG",value); }
        }

        public string Device
        {
            get { return Get("DEVICE"); }
            set { Add("DEVICE", value); }
        }

        public string DestinationProduct
        {
            get { return Get("DESTPROD"); }
            set { Add("DESTPROD", value); }
        }

        public string Browser
        {
            get { return Get("BROWSER"); }
            set { Add("BROWSER", value); }
        }

        public string BrowserVersion
        {
            get { return Get("BRWVER"); }
            set { Add("BRWVER", value); }
        }

        public string Platform
        {
            get { return Get("PLATFORM"); }
            set { Add("PLATFORM", value); }
        }

        public string UserAgent
        {
            get { return Get("USRAGT"); }
            set { Add("USRAGT", value); }
        }

        public UserInterfaceAndDeviceOperationalData()
        {
            
        }
        protected UserInterfaceAndDeviceOperationalData(IDictionary<string, string> list) : base(list)
        {
        }
    }
}
