namespace DowJones.Globalization
{

    public class CountryInfo
    {
        private string m_CountryISOCode;
        private string m_RegionSrcs;
        private string m_RegionName;
        private int m_RegionId;


        public string CountryISOCode
        {
            get { return m_CountryISOCode; }
            set { m_CountryISOCode = value; }
        }

        public string RegionalSources
        {
            get { return m_RegionSrcs; }
            set { m_RegionSrcs = value; }
        }
        public string RegionName
        {
            get { return m_RegionName; }
            set { m_RegionName = value; }
        }
        public int RegionId
        {
            get { return m_RegionId; }
            set { m_RegionId = value; }
        }
    }
}
