using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "XAxisDataMembers", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class XAxisDataMembers : object, IExtensibleDataObject
    {
        [DataMember]
        public AxisLabel[] ElementList { get; set; }

        [DataMember]
        public double[] timeLines { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}