﻿using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "PricePoint", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice")]
    public class PricePoint : object, IExtensibleDataObject
    {
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public double Price { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}