using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "TradingRegion", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class TradingRegion : object, IExtensibleDataObject
    {
        [DataMember]
        public int Start { get; set; }

        [DataMember]
        public int Stop { get; set; }

        [DataMember]
        public RegionType Type { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}