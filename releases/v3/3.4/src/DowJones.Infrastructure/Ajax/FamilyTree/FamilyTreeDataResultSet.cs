using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Formatters;

namespace DowJones.Ajax.FamilyTree
{
    public class FamilyTreeDataResultSet
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private WholeNumber __count;

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [XmlElement(Type = typeof(WholeNumber), ElementName = "count", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public WholeNumber count
        {
            get
            {
                if (__count == null) __count = new WholeNumber(0);
                return __count;
            }
            set { __count = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private List<FamilyTreeNodeInfo> __familyTreeNodes;
        
        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        /// <value>The first.</value>
        [XmlElement(Type = typeof(FamilyTreeNodeInfo), ElementName = "familyTreeNodes", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public List<FamilyTreeNodeInfo> familyTreeNodes
        {
            get
            {
                if (__familyTreeNodes == null) __familyTreeNodes = new List<FamilyTreeNodeInfo>();
                return __familyTreeNodes;
            }
            set { __familyTreeNodes = value; }
        }

    }
}
