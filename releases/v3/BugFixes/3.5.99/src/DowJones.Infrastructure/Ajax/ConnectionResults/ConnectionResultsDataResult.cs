
using DowJones.Formatters;

namespace DowJones.Ajax.ConnectionResults
{
    public class ConnectionResultsDataResult : IListDataResult
    {
        private WholeNumber m_HitCount;

        public WholeNumber HitCount
        {
            get
            {
                if (m_HitCount == null) m_HitCount = new WholeNumber(0);
                return m_HitCount;
            }
            set { m_HitCount = value; }
        }

        private ConnectionResultsDataResultSet m_ConnectionResultsDataResultSet;

        public ConnectionResultsDataResultSet ResultSet
        {
            get
            {
                if (m_ConnectionResultsDataResultSet == null) m_ConnectionResultsDataResultSet = new ConnectionResultsDataResultSet();
                return m_ConnectionResultsDataResultSet;
            }
            set { m_ConnectionResultsDataResultSet = value; }
        }
    }
}
