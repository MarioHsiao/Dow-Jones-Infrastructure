using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DowJones.Ajax.FamilyTree
{
    public class FamilyTreeDataResult : IListDataResult
    {
        /// <summary>
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private FamilyTreeDataResultSet _familyTreeDataResultSet;

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(FamilyTreeDataResultSet), ElementName = "familyTreeDataResultSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public FamilyTreeDataResultSet resultSet
        {
            get
            {
                if (_familyTreeDataResultSet == null) _familyTreeDataResultSet = new FamilyTreeDataResultSet();
                return _familyTreeDataResultSet;
            }
            set { _familyTreeDataResultSet = value; }
        }
    }
}
