namespace DowJones.Utilities.Ajax.ConnectionResults.Converters
{
    using Formatters.Numerical;
    using Tools.Ajax;

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
