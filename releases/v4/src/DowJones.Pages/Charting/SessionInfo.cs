using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "SessionInfo", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class SessionInfo : object, IExtensibleDataObject
    {
        [DataMember]
        public string BlueGrassChannel { get; set; }

        [DataMember]
        public int IsIndex { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Session[] Sessions { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}