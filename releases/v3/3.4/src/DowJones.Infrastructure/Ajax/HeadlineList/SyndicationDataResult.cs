using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DowJones.Ajax.HeadlineList
{
    public class SyndicationDataResult : IListDataResult
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private string _feedTitle;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private HeadlineListDataResult _headlineListDataResult;

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(HeadlineListDataResult), ElementName = "headlineListDataResult", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public HeadlineListDataResult result
        {
            get { return _headlineListDataResult ?? (_headlineListDataResult = new HeadlineListDataResult()); }
            set { _headlineListDataResult = value; }
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(string), ElementName = "feedTitle", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public string feedTitle
        {
            get { return _feedTitle; }
            set { _feedTitle = value; }
        }
    }
}