using System.Collections.Generic;
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
    public class TriggerListDataResultSet
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private WholeNumber __first;

        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        /// <value>The first.</value>
        [XmlElement(Type = typeof(WholeNumber), ElementName = "first", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public WholeNumber first
        {
            get
            {
                if (__first == null) __first = new WholeNumber(0);
                return __first;
            }
            set { __first = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private List<TriggerInfo> __triggers;


        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        /// <value>The first.</value>
        [XmlElement(Type = typeof(TriggerInfo), ElementName = "triggers", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public List<TriggerInfo> triggers
        {
            get
            {
                if (__triggers == null) __triggers = new List<TriggerInfo>();
                return __triggers;
            }
            set { __triggers = value; }
        }

    }
}
