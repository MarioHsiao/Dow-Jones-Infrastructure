using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "NewsLink", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class NewsLink : object, IExtensibleDataObject
    {
        [DataMember]
        public int At { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string Url { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}