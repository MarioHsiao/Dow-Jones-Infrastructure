using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Tools.WebServices.Suggest;
using DowJones.Tools.ServiceLayer.WebServices;

namespace DowJones.Tools.Ajax.Suggest
{
    public struct Declarations
    {
        public const string SCHEMA_VERSION = "urn:factiva:fcp:v1_0";
    }

    public class SuggestCompanyResponseDelegate : AbstractAjaxResponseDelegate
    {
        [XmlElement(Type = typeof(SugestedCompanyList), ElementName = "SuggestCompanyResult", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        public SugestedCompanyList __suggestCompanyResult;

        [XmlIgnore]
        public SugestedCompanyList Result
        {
            get
            {
                if (__suggestCompanyResult == null) __suggestCompanyResult = new SugestedCompanyList();
                return __suggestCompanyResult;
            }
            set { __suggestCompanyResult = value; }
        }
    }


}
