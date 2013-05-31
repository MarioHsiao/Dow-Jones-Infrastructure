using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "XAxisLabelInfo", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class XAxisLabelInfo : object, IExtensibleDataObject
    {
        [DataMember]
        public SessionUnit chartUnit { get; set; }

        [DataMember]
        public SessionUnit displayUnit { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}