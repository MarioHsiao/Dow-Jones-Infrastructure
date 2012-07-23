using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Formatters;

namespace DowJones.Ajax.TabStrip
{
    public class TabStripDataResultSet
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
        private List<Tab> __tab;

        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        /// <value>The first.</value>
        [XmlElement(Type = typeof(Tab), ElementName = "tab", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public List<Tab> tab
        {
            get
            {
                if (__tab == null) __tab = new List<Tab>();
                return __tab;
            }
            set { __tab = value; }
        }
    }
}
