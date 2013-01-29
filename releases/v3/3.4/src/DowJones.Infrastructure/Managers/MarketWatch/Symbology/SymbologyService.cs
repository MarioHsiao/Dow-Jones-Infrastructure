﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DowJones.MarketWatch.Dylan.Core.Schemas;

[assembly: ContractNamespace("http://service.marketwatch.com/ws/2007/05/utility", ClrNamespace = "service.marketwatch.com.ws._2007._05.utility")]
[assembly: ContractNamespace("http://service.marketwatch.com/ws/2007/05/symbology", ClrNamespace = "service.marketwatch.com.ws._2007._05.symbology")]

namespace DowJones.Managers.MarketWatch.Symbology
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "NormalizeResponse", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class NormalizeResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public string Dialect { get; set; }

        [DataMember]
        public NormalizeResult[] NormalizeResults { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "NormalizeResult", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class NormalizeResult : ExtensibleDataObjectBase
    {
        [DataMember]
        public string Symbol { get; set; }

        [DataMember(Order = 1)]
        public DowJones.MarketWatch.Dylan.Core.Symbology.Instrument Instrument { get; set; }

        [DataMember(Order = 2)]
        public bool IsUpdated { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DenormalizeRequest", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class DenormalizeRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public string Dialect { get; set; }

        [DataMember(Order = 1)]
        public DenormalizeItem[] DenormalizeItems { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DenormalizeItem", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class DenormalizeItem : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public DowJones.MarketWatch.Dylan.Core.Symbology.Instrument Instrument { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DenormalizeResponse", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class DenormalizeResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public string Dialect { get; set; }

        [DataMember(Order = 1)]
        public DenormalizeResult[] DenormalizeResults { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DenormalizeResult", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class DenormalizeResult : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public DowJones.MarketWatch.Dylan.Core.Symbology.Instrument Instrument { get; set; }

        [DataMember(Order = 2)]
        public string[] Symbols { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "NormalizeRequest", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class NormalizeRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public string Dialect { get; set; }

        [DataMember]
        public string[] Symbols { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ActiveInstrument", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class ActiveInstrument : object, IExtensibleDataObject
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Ticker { get; set; }

        [DataMember(Order = 2)]
        public string CompanyName { get; set; }

        [DataMember(Order = 3)]
        public string SymbolType { get; set; }

        [DataMember(Order = 4)]
        public string[] AliasSymbols { get; set; }

        [DataMember(Order = 5)]
        public string CountryCode { get; set; }

        [DataMember(Order = 6)]
        public string DoubleMetaphoneValue { get; set; }

        [DataMember(Order = 7)]
        public decimal? VolumeAverage200Day { get; set; }

        [DataMember(Order = 8)]
        public decimal? MarketCapitalization { get; set; }

        [DataMember(Order = 9)]
        public int RelevanceBoost { get; set; }

        [DataMember(Order = 10)]
        public string QuerySymbolType { get; set; }

        [DataMember(Order = 11)]
        public string ExchangeTicker { get; set; }

        [DataMember(Order = 12)]
        public string ExchangeIsoCode { get; set; }

        [DataMember(Order = 13)]
        public string ChartingSymbol { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "FutureFamilyMember", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class FutureFamilyMember : object, IExtensibleDataObject
    {
        [DataMember]
        public DowJones.MarketWatch.Dylan.Core.Symbology.Instrument Instrument { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "TranslateResult", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class TranslateResult : object, IExtensibleDataObject
    {
        [DataMember]
        public string SourceSymbol { get; set; }

        [DataMember(Order = 1)]
        public string[] DestinationSymbols { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "NormalizeFault", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class NormalizeFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "PayloadToLargeFault", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class PayloadToLargeFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DenormalizeFault", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class DenormalizeFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetActiveInstrumentFault", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class GetActiveInstrumentFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetFutureFamilyByInstrumentFault", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class GetFutureFamilyByInstrumentFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetPreIpoInstrumentsFault", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class GetPreIpoInstrumentsFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "TranslateFault", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class TranslateFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", ConfigurationName = "Symbology")]
    public interface ISymbology
    {
        // CODEGEN: Generating message contract since the operation Normalize is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/Normalize", ReplyAction = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/NormalizeResponse")]
        [FaultContract(typeof(NormalizeFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/NormalizeNormalizeF" +
                                                         "aultFault", Name = "NormalizeFault")]
        [FaultContract(typeof(PayloadToLargeFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/NormalizePayloadToL" +
                                                              "argeFaultFault", Name = "PayloadToLargeFault")]
        NormalizeOut Normalize(NormalizeIn request);

        // CODEGEN: Generating message contract since the operation Denormalize is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/Denormalize", ReplyAction = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/DenormalizeResponse" +
                                                                                                                               "")]
        [FaultContract(typeof(PayloadToLargeFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/DenormalizePayloadT" +
                                                              "oLargeFaultFault", Name = "PayloadToLargeFault")]
        [FaultContract(typeof(DenormalizeFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/DenormalizeDenormal" +
                                                           "izeFaultFault", Name = "DenormalizeFault")]
        DenormalizeOut Denormalize(DenormalizeIn request);

        // CODEGEN: Generating message contract since the wrapper name (GetActiveInstrumentBatchRequest) of message GetActiveInstrumentBatchRequest does not match the default value (GetActiveInstrumentBatch)
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetActiveInstrument" +
                                    "Batch", ReplyAction = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetActiveInstrument" +
                                                           "BatchResponse")]
        [FaultContract(typeof(GetActiveInstrumentFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetActiveInstrument" +
                                                                   "BatchGetActiveInstrumentFaultFault", Name = "GetActiveInstrumentFault")]
        GetActiveInstrumentBatchResponse GetActiveInstrumentBatch(GetActiveInstrumentBatchRequest request);

        // CODEGEN: Generating message contract since the wrapper name (GetFutureFamilyByInstrumentRequest) of message GetFutureFamilyByInstrumentRequest does not match the default value (GetFutureFamilyByInstrument)
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetFutureFamilyByIn" +
                                    "strument", ReplyAction = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetFutureFamilyByIn" +
                                                              "strumentResponse")]
        [FaultContract(typeof(GetFutureFamilyByInstrumentFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetFutureFamilyByIn" +
                                                                           "strumentGetFutureFamilyByInstrumentFaultFault", Name = "GetFutureFamilyByInstrumentFault")]
        GetFutureFamilyByInstrumentResponse GetFutureFamilyByInstrument(GetFutureFamilyByInstrumentRequest request);

        // CODEGEN: Generating message contract since the wrapper name (GetPreIpoInstrumentsRequest) of message GetPreIpoInstrumentsRequest does not match the default value (GetPreIpoInstruments)
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetPreIpoInstrument" +
                                    "s", ReplyAction = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetPreIpoInstrument" +
                                                       "sResponse")]
        [FaultContract(typeof(GetPreIpoInstrumentsFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/GetPreIpoInstrument" +
                                                                    "sGetPreIpoInstrumentsFaultFault", Name = "GetPreIpoInstrumentsFault")]
        GetPreIpoInstrumentsResponse GetPreIpoInstruments(GetPreIpoInstrumentsRequest request);

        // CODEGEN: Generating message contract since the wrapper name (TranslateRequest) of message TranslateRequest does not match the default value (Translate)
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/Translate", ReplyAction = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/TranslateResponse")]
        [FaultContract(typeof(TranslateFault), Action = "http://service.marketwatch.com/ws/2007/05/symbology/Symbology/TranslateTranslateF" +
                                                         "aultFault", Name = "TranslateFault")]
        TranslateResponse Translate(TranslateRequest request);
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class NormalizeIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public NormalizeRequest NormalizeRequest;

        public NormalizeIn()
        {
        }

        public NormalizeIn(NormalizeRequest NormalizeRequest)
        {
            this.NormalizeRequest = NormalizeRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class NormalizeOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public NormalizeResponse NormalizeResponse;

        public NormalizeOut()
        {
        }

        public NormalizeOut(NormalizeResponse NormalizeResponse)
        {
            this.NormalizeResponse = NormalizeResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class DenormalizeIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public DenormalizeRequest DenormalizeRequest;

        public DenormalizeIn()
        {
        }

        public DenormalizeIn(DenormalizeRequest DenormalizeRequest)
        {
            this.DenormalizeRequest = DenormalizeRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class DenormalizeOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public DenormalizeResponse DenormalizeResponse;

        public DenormalizeOut()
        {
        }

        public DenormalizeOut(DenormalizeResponse DenormalizeResponse)
        {
            this.DenormalizeResponse = DenormalizeResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetActiveInstrumentBatchRequest", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class GetActiveInstrumentBatchRequest
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public long? LastId;

        public GetActiveInstrumentBatchRequest()
        {
        }

        public GetActiveInstrumentBatchRequest(long? LastId)
        {
            this.LastId = LastId;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetActiveInstrumentBatchResponse", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class GetActiveInstrumentBatchResponse
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public ActiveInstrument[] ActiveInstruments;

        public GetActiveInstrumentBatchResponse()
        {
        }

        public GetActiveInstrumentBatchResponse(ActiveInstrument[] ActiveInstruments)
        {
            this.ActiveInstruments = ActiveInstruments;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetFutureFamilyByInstrumentRequest", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class GetFutureFamilyByInstrumentRequest
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public DowJones.MarketWatch.Dylan.Core.Symbology.Instrument Instrument;

        public GetFutureFamilyByInstrumentRequest()
        {
        }

        public GetFutureFamilyByInstrumentRequest(DowJones.MarketWatch.Dylan.Core.Symbology.Instrument instrument)
        {
            Instrument = instrument;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetFutureFamilyByInstrumentResponse", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class GetFutureFamilyByInstrumentResponse
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public FutureFamilyMember[] FutureFamilyMembers;

        public GetFutureFamilyByInstrumentResponse()
        {
        }

        public GetFutureFamilyByInstrumentResponse(FutureFamilyMember[] FutureFamilyMembers)
        {
            this.FutureFamilyMembers = FutureFamilyMembers;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetPreIpoInstrumentsRequest", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class GetPreIpoInstrumentsRequest
    {
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetPreIpoInstrumentsResponse", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class GetPreIpoInstrumentsResponse
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public ActiveInstrument[] ActiveInstruments;

        public GetPreIpoInstrumentsResponse()
        {
        }

        public GetPreIpoInstrumentsResponse(ActiveInstrument[] ActiveInstruments)
        {
            this.ActiveInstruments = ActiveInstruments;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "TranslateRequest", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class TranslateRequest
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 1)]
        public string DestinationDialect;
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public string SourceDialect;

        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 2)]
        public string[] Symbols;

        public TranslateRequest()
        {
        }

        public TranslateRequest(string SourceDialect, string DestinationDialect, string[] Symbols)
        {
            this.SourceDialect = SourceDialect;
            this.DestinationDialect = DestinationDialect;
            this.Symbols = Symbols;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "TranslateResponse", WrapperNamespace = "http://service.marketwatch.com/ws/2007/05/symbology", IsWrapped = true)]
    public class TranslateResponse
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 1)]
        public string DestinationDialect;
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 0)]
        public string SourceDialect;

        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/05/symbology", Order = 2)]
        public TranslateResult[] TranslateResults;

        public TranslateResponse()
        {
        }

        public TranslateResponse(string SourceDialect, string DestinationDialect, TranslateResult[] TranslateResults)
        {
            this.SourceDialect = SourceDialect;
            this.DestinationDialect = DestinationDialect;
            this.TranslateResults = TranslateResults;
        }
    }

    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface SymbologyChannel : ISymbology, IClientChannel
    {
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class SymbologyClient : ClientBase<ISymbology>, ISymbology
    {
        public SymbologyClient()
        {
        }

        public SymbologyClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public SymbologyClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public SymbologyClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public SymbologyClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        #region Symbology Members

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        NormalizeOut ISymbology.Normalize(NormalizeIn request)
        {
            return Channel.Normalize(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        DenormalizeOut ISymbology.Denormalize(DenormalizeIn request)
        {
            return Channel.Denormalize(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetActiveInstrumentBatchResponse ISymbology.GetActiveInstrumentBatch(GetActiveInstrumentBatchRequest request)
        {
            return Channel.GetActiveInstrumentBatch(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetFutureFamilyByInstrumentResponse ISymbology.GetFutureFamilyByInstrument(GetFutureFamilyByInstrumentRequest request)
        {
            return Channel.GetFutureFamilyByInstrument(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetPreIpoInstrumentsResponse ISymbology.GetPreIpoInstruments(GetPreIpoInstrumentsRequest request)
        {
            return Channel.GetPreIpoInstruments(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        TranslateResponse ISymbology.Translate(TranslateRequest request)
        {
            return Channel.Translate(request);
        }

        #endregion

        public NormalizeResponse Normalize(NormalizeRequest NormalizeRequest)
        {
            var inValue = new NormalizeIn
            {
                NormalizeRequest = NormalizeRequest
            };
            var retVal = ((ISymbology)(this)).Normalize(inValue);
            return retVal.NormalizeResponse;
        }

        public DenormalizeResponse Denormalize(DenormalizeRequest DenormalizeRequest)
        {
            var inValue = new DenormalizeIn
            {
                DenormalizeRequest = DenormalizeRequest
            };
            var retVal = ((ISymbology)(this)).Denormalize(inValue);
            return retVal.DenormalizeResponse;
        }

        public ActiveInstrument[] GetActiveInstrumentBatch(long? LastId)
        {
            var inValue = new GetActiveInstrumentBatchRequest
            {
                LastId = LastId
            };
            var retVal = ((ISymbology)(this)).GetActiveInstrumentBatch(inValue);
            return retVal.ActiveInstruments;
        }

        public FutureFamilyMember[] GetFutureFamilyByInstrument(DowJones.MarketWatch.Dylan.Core.Symbology.Instrument instrument)
        {
            var inValue = new GetFutureFamilyByInstrumentRequest
            {
                Instrument = instrument
            };
            var retVal = ((ISymbology)(this)).GetFutureFamilyByInstrument(inValue);
            return retVal.FutureFamilyMembers;
        }

        public ActiveInstrument[] GetPreIpoInstruments()
        {
            var inValue = new GetPreIpoInstrumentsRequest();
            var retVal = ((ISymbology)(this)).GetPreIpoInstruments(inValue);
            return retVal.ActiveInstruments;
        }

        public TranslateResult[] Translate(ref string SourceDialect, ref string DestinationDialect, string[] Symbols)
        {
            var inValue = new TranslateRequest
            {
                SourceDialect = SourceDialect,
                DestinationDialect = DestinationDialect,
                Symbols = Symbols
            };
            var retVal = ((ISymbology)(this)).Translate(inValue);
            SourceDialect = retVal.SourceDialect;
            DestinationDialect = retVal.DestinationDialect;
            return retVal.TranslateResults;
        }
    }

}
