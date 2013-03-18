using DowJones.Ajax;
using DowJones.Formatters.Numerical;

namespace DowJones.Assemblers.RelationshipMapping
{
    public abstract class AbstractRelationshipMappingResultConverter : IListDataResultConverter
    {
        private readonly NumberFormatter m_NumberFormatter = new NumberFormatter();

        protected NumberFormatter NumberFormatter
        {
            get { return m_NumberFormatter; }
        }

        public abstract IListDataResult Process();
    }
}
