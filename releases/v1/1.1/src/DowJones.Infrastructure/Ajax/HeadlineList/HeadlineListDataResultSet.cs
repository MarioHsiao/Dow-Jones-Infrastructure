using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Formatters;
using Factiva.Gateway.Messages.CodedNews;

namespace DowJones.Tools.Ajax.HeadlineList
{
    /// <summary>
    /// 
    /// </summary>
    public class HeadlineListDataResultSet
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
        private WholeNumber __duplicateCount;

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [XmlElement(Type = typeof(WholeNumber), ElementName = "duplicateCount", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public WholeNumber duplicateCount
        {
            get
            {
                if (__duplicateCount == null) __duplicateCount = new WholeNumber(0);
                return __duplicateCount;
            }
            set { __duplicateCount = value; }
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
        private List<HeadlineInfo> __headlines;


        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        /// <value>The first.</value>
        [XmlElement(Type = typeof(HeadlineInfo), ElementName = "headlines", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public List<HeadlineInfo> headlines
        {
            get
            {
                if (__headlines == null) __headlines =  new List<HeadlineInfo>();
                return __headlines;
            }
            set { __headlines = value; }
        }
    }
}
