using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Utilities.Formatters;

namespace DowJones.Tools.Ajax.TriggerList
{
    /// <summary>
    /// 
    /// </summary>
    public class TriggerListDataResult : IListDataResult
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private WholeNumber _hitCount;

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(WholeNumber), ElementName = "hitCount", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public WholeNumber hitCount
        {
            get
            {
                if (_hitCount == null) _hitCount = new WholeNumber(0);
                return _hitCount;
            }
            set { _hitCount = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private TriggerListDataResultSet _triggerListDataResultSet;

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(TriggerListDataResultSet), ElementName = "triggerListDataResultSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public TriggerListDataResultSet resultSet
        {
            get
            {
                if (_triggerListDataResultSet == null) _triggerListDataResultSet = new TriggerListDataResultSet();
                return _triggerListDataResultSet;
            }
            set { _triggerListDataResultSet = value; }
        }
    }
}
