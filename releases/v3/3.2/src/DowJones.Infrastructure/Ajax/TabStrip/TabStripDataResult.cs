using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DowJones.Ajax.TabStrip
{
    public class TabStripDataResult : IListDataResult
    {
        /// <summary>
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private TabStripDataResultSet _tabStripDataResultSet;

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(TabStripDataResultSet), ElementName = "tabStripDataResultSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public TabStripDataResultSet resultSet
        {
            get
            {
                if (_tabStripDataResultSet == null) _tabStripDataResultSet = new TabStripDataResultSet();
                return _tabStripDataResultSet;
            }
            set { _tabStripDataResultSet = value; }
        }
    }
}
