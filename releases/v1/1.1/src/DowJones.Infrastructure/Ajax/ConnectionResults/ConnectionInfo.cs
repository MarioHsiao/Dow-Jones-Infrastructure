
namespace DowJones.Utilities.Ajax.ConnectionResults
{
    using System.Collections.Generic;
    using Formatters;

    public class ConnectionInfo
    {
        public string Name { get; set; }
        public string FCode { get; set; }
        public EntityType EntityType { get; set; }
        public ConnectionType Type { get; set; }
        public string NavigateUrl { get; set; }

        private List<JobInfo> m_Jobs;
        public List<JobInfo> Jobs
        {
            get
            {
                if (m_Jobs == null)
                {
                    m_Jobs = new List<JobInfo>();
                }
                return m_Jobs;
            }
            set
            {
                m_Jobs = value;
            }
        }

        private WholeNumber m_Strength;

        public WholeNumber Strength
        {
            get
            {
                if (m_Strength == null) m_Strength = new WholeNumber(0);
                return m_Strength;
            }
            set { m_Strength = value; }
        }

    }
}
