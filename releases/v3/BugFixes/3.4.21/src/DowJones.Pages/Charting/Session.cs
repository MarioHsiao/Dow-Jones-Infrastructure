using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "Session", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class Session : object, IExtensibleDataObject
    {
        [DataMember]
        public PricePoint High { get; set; }

        [DataMember]
        public PricePoint Low { get; set; }

        [DataMember]
        public NewsLink[] News { get; set; }

        [DataMember]
        public double? PreviousClose { get; set; }

        [DataMember]
        public TradingRegion[] Regions { get; set; }

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public DateTime Stop { get; set; }

        [DataMember]
        public double?[] Trades { get; set; }

        [DataMember]
        public int Weight { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}