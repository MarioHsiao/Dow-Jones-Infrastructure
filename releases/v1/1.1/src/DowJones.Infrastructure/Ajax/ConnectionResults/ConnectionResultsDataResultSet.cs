
namespace DowJones.Utilities.Ajax.ConnectionResults
{
    using System.Collections.Generic;
    using Formatters;

    public class ConnectionResultsDataResultSet
    {
        private WholeNumber m_First;
        private WholeNumber m_Count;
        private List<RelationshipInfo> m_Relationships;


        public ConnectionInfo SourceConnection { get; set; }

        public ConnectionInfo TargetConnection { get; set; }

        public WholeNumber First
        {
            get
            {
                if (m_First == null)
                    m_First = new WholeNumber(0);

                return m_First;
            }
            set { m_First = value; }
        }

        public WholeNumber Count
        {
            get
            {
                if (m_Count == null)
                    m_Count = new WholeNumber(0);

                return m_Count;
            }
            set { m_Count = value; }
        }

        public List<RelationshipInfo> Relationships
        {
            get
            {
                if (m_Relationships == null) m_Relationships = new List<RelationshipInfo>();
                return m_Relationships;
            }
            set { m_Relationships = value; }
        }
    }
}
