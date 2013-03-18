namespace DowJones.OperationalData.EntryPoint
{
    public class HomePageOperationalData : AbstractOperationalData
    {
        public string HomePage
        {
            get { return Get(ODSConstants.KEY_HOMEPAGE); }
            set { Add(ODSConstants.KEY_HOMEPAGE, value); }
        }
    }
}