using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Formatters;

namespace DowJones.Ajax.HeadlineList
{
    /// <summary>
    /// 
    /// </summary>
    public class HeadlineListDataResult : IListDataResult
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private HeadlineListDataResultSet _headlineListDataResultSet;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private WholeNumber _hitCount;

        [XmlElement(Type = typeof (bool), ElementName = "isTimeInGMT", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public bool isTimeInGMT { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(WholeNumber), ElementName = "hitCount", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public WholeNumber hitCount
        {
            get { return _hitCount ?? (_hitCount = new WholeNumber(0)); }
            set { _hitCount = value; }
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(HeadlineListDataResultSet), ElementName = "headlineListDataResultSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public HeadlineListDataResultSet resultSet
        {
            get { return _headlineListDataResultSet ?? (_headlineListDataResultSet = new HeadlineListDataResultSet()); }
            set { _headlineListDataResultSet = value; }
        }
            }
}
