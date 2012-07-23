using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "AxisLabel", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class AxisLabel : object, IExtensibleDataObject
    {
        [DataMember]
        public string element { get; set; }

        [DataMember]
        public double x { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}