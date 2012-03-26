
namespace DowJones.Utilities.Ajax.ConnectionResults
{
    using System.Collections.Generic;
    using Formatters;

    public class RelationshipInfo
    {
        public ConnectionInfo SourceConnection { get; set; }

        public ConnectionInfo TargetConnection { get; set; }

        public RelationshipType Type { get; set; }

        private WholeNumber m_Strength;
        public WholeNumber Strength
        {
            get
            {
                if (m_Strength == null)
                {
                    m_Strength = new WholeNumber(0);
                }

                return m_Strength;
            }
            set
            {
                m_Strength = value;
            }
        }

        private List<ConnectionInfo> m_Connections;
        public List<ConnectionInfo> Connections
        {
            get
            {
                if (m_Connections == null)
                {
                    m_Connections = new List<ConnectionInfo>();
                }

                return m_Connections;
            }
            set
            {
                m_Connections = value;
            }

        }

    }
}
