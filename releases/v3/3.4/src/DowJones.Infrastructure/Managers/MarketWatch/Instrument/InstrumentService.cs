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
using System.Xml;
using DowJones.MarketWatch.Dylan.Core.Financialdata;

[assembly: ContractNamespace("http://service.marketwatch.com/ws/2007/05/symbology", ClrNamespace = "service.marketwatch.com.ws._2007._05.symbology")]
[assembly: ContractNamespace("http://service.marketwatch.com/ws/2007/05/utility", ClrNamespace = "service.marketwatch.com.ws._2007._05.utility")]
[assembly: ContractNamespace("http://service.marketwatch.com/ws/2007/10/financialdata", ClrNamespace = "service.marketwatch.com.ws._2007._10.financialdata")]

namespace DowJones.Managers.MarketWatch.Instrument
{
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", ConfigurationName = "Instrument")]
    public interface IInstrument
    {
        // CODEGEN: Generating message contract since the operation GetInstrument is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetInstrument", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetInstrumentR" +
                                                                                                                                      "esponse")]
        [FaultContract(typeof (GetInstrumentFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetInstrumentG" +
                                                             "etInstrumentFaultFault", Name = "GetInstrumentFault")]
        [FaultContract(typeof (PayloadToLargeFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetInstrumentP" +
                                                              "ayloadToLargeFaultFault", Name = "PayloadToLargeFault")]
        GetInstrumentOut GetInstrument(GetInstrumentIn request);

        // CODEGEN: Generating message contract since the operation GetEntity is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetEntity", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetEntityRespo" +
                                                                                                                                  "nse")]
        [FaultContract(typeof (GetEntityFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetEntityGetEn" +
                                                         "tityFaultFault", Name = "GetEntityFault")]
        GetEntityOut GetEntity(GetEntityIn request);

        // CODEGEN: Generating message contract since the operation GetExchangeSummary is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetExchangeSum" +
                                    "mary", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetExchangeSum" +
                                                          "maryResponse")]
        [FaultContract(typeof (GetExchangeSummaryFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetExchangeSum" +
                                                                  "maryGetExchangeSummaryFaultFault", Name = "GetExchangeSummaryFault")]
        GetExchangeSummaryOut GetExchangeSummary(GetExchangeSummaryIn request);

        // CODEGEN: Generating message contract since the operation PhoneticSearch is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/PhoneticSearch" +
                                    "", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/PhoneticSearch" +
                                                      "Response")]
        [FaultContract(typeof (PhoneticSearchFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/PhoneticSearch" +
                                                              "PhoneticSearchFaultFault", Name = "PhoneticSearchFault")]
        PhoneticSearchOut PhoneticSearch(PhoneticSearchIn request);

        // CODEGEN: Generating message contract since the operation GetOptionChain is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetOptionChain" +
                                    "", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetOptionChain" +
                                                      "Response")]
        [FaultContract(typeof (GetOptionChainFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetOptionChain" +
                                                              "GetOptionChainFaultFault", Name = "GetOptionChainFault")]
        GetOptionChainOut GetOptionChain(GetOptionChainIn request);

        // CODEGEN: Generating message contract since the operation GetIndexComponents is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetIndexCompon" +
                                    "ents", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetIndexCompon" +
                                                          "entsResponse")]
        [FaultContract(typeof (GetIndexComponentsFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetIndexCompon" +
                                                                  "entsGetIndexComponentsFaultFault", Name = "GetIndexComponentsFault")]
        GetIndexComponentsOut GetIndexComponents(GetIndexComponentsIn request);

        // CODEGEN: Generating message contract since the operation GetCurrencyInstruments is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetCurrencyIns" +
                                    "truments", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetCurrencyIns" +
                                                              "trumentsResponse")]
        [FaultContract(typeof (GetCurrencyInstrumentsFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetCurrencyIns" +
                                                                      "trumentsGetCurrencyInstrumentsFaultFault", Name = "GetCurrencyInstrumentsFault")]
        GetCurrencyInstrumentsOut GetCurrencyInstruments(GetCurrencyInstrumentsIn request);

        // CODEGEN: Generating message contract since the operation ConvertCurrency is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/ConvertCurrenc" +
                                    "y", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/ConvertCurrenc" +
                                                       "yResponse")]
        [FaultContract(typeof (ConvertCurrencyFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/ConvertCurrenc" +
                                                               "yConvertCurrencyFaultFault", Name = "ConvertCurrencyFault")]
        ConvertCurrencyOut ConvertCurrency(ConvertCurrencyIn request);

        // CODEGEN: Generating message contract since the wrapper name (GetInstrumentByDialectRequest) of message GetInstrumentByDialectRequest does not match the default value (GetInstrumentByDialect)
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetInstrumentB" +
                                    "yDialect", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetInstrumentB" +
                                                              "yDialectResponse")]
        [FaultContract(typeof (GetInstrumentByDialectFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetInstrumentB" +
                                                                      "yDialectGetInstrumentByDialectFaultFault", Name = "GetInstrumentByDialectFault")]
        GetInstrumentByDialectResponse GetInstrumentByDialect(GetInstrumentByDialectRequest request);

        // CODEGEN: Generating message contract since the operation GetIndexes is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetIndexes", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetIndexesResp" +
                                                                                                                                   "onse")]
        [FaultContract(typeof (GetIndexesFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetIndexesGetI" +
                                                          "ndexesFaultFault", Name = "GetIndexesFault")]
        GetIndexesOut GetIndexes(GetIndexesIn request);

        // CODEGEN: Generating message contract since the operation GetOptionMonths is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetOptionMonth" +
                                    "s", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetOptionMonth" +
                                                       "sResponse")]
        [FaultContract(typeof (GetOptionMonthsFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetOptionMonth" +
                                                               "sGetOptionMonthsFaultFault", Name = "GetOptionMonthsFault")]
        GetOptionMonthsOut GetOptionMonths(GetOptionMonthsIn request);

        // CODEGEN: Generating message contract since the operation GetFutureInstruments is neither RPC nor document wrapped.
        [OperationContract(Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetFutureInstr" +
                                    "uments", ReplyAction = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetFutureInstr" +
                                                            "umentsResponse")]
        [FaultContract(typeof (GetFutureInstrumentsFault), Action = "http://service.marketwatch.com/ws/2007/10/financialdata/Instrument/GetFutureInstr" +
                                                                    "umentsGetFutureInstrumentsFaultFault", Name = "GetFutureInstrumentsFault")]
        GetFutureInstrumentsOut GetFutureInstruments(GetFutureInstrumentsIn request);
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetInstrumentIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetInstrumentRequest GetInstrumentRequest;

        public GetInstrumentIn()
        {
        }

        public GetInstrumentIn(GetInstrumentRequest getInstrumentRequest)
        {
            GetInstrumentRequest = getInstrumentRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetInstrumentOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetInstrumentResponse GetInstrumentResponse;

        public GetInstrumentOut()
        {
        }

        public GetInstrumentOut(GetInstrumentResponse GetInstrumentResponse)
        {
            this.GetInstrumentResponse = GetInstrumentResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetEntityIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetEntityRequest GetEntityRequest;

        public GetEntityIn()
        {
        }

        public GetEntityIn(GetEntityRequest GetEntityRequest)
        {
            this.GetEntityRequest = GetEntityRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetEntityOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetEntityResponse GetEntityResponse;

        public GetEntityOut()
        {
        }

        public GetEntityOut(GetEntityResponse GetEntityResponse)
        {
            this.GetEntityResponse = GetEntityResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetExchangeSummaryIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetExchangeSummaryRequest GetExchangeSummaryRequest;

        public GetExchangeSummaryIn()
        {
        }

        public GetExchangeSummaryIn(GetExchangeSummaryRequest GetExchangeSummaryRequest)
        {
            this.GetExchangeSummaryRequest = GetExchangeSummaryRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetExchangeSummaryOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetExchangeSummaryResponse GetExchangeSummaryResponse;

        public GetExchangeSummaryOut()
        {
        }

        public GetExchangeSummaryOut(GetExchangeSummaryResponse GetExchangeSummaryResponse)
        {
            this.GetExchangeSummaryResponse = GetExchangeSummaryResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class PhoneticSearchIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public PhoneticSearchRequest PhoneticSearchRequest;

        public PhoneticSearchIn()
        {
        }

        public PhoneticSearchIn(PhoneticSearchRequest PhoneticSearchRequest)
        {
            this.PhoneticSearchRequest = PhoneticSearchRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class PhoneticSearchOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public PhoneticSearchResponse PhoneticSearchResponse;

        public PhoneticSearchOut()
        {
        }

        public PhoneticSearchOut(PhoneticSearchResponse PhoneticSearchResponse)
        {
            this.PhoneticSearchResponse = PhoneticSearchResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetOptionChainIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetOptionChainRequest GetOptionChainRequest;

        public GetOptionChainIn()
        {
        }

        public GetOptionChainIn(GetOptionChainRequest GetOptionChainRequest)
        {
            this.GetOptionChainRequest = GetOptionChainRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetOptionChainOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetOptionChainResponse GetOptionChainResponse;

        public GetOptionChainOut()
        {
        }

        public GetOptionChainOut(GetOptionChainResponse GetOptionChainResponse)
        {
            this.GetOptionChainResponse = GetOptionChainResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetIndexComponentsIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetIndexComponentsRequest GetIndexComponentsRequest;

        public GetIndexComponentsIn()
        {
        }

        public GetIndexComponentsIn(GetIndexComponentsRequest GetIndexComponentsRequest)
        {
            this.GetIndexComponentsRequest = GetIndexComponentsRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetIndexComponentsOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetIndexComponentsResponse GetIndexComponentsResponse;

        public GetIndexComponentsOut()
        {
        }

        public GetIndexComponentsOut(GetIndexComponentsResponse GetIndexComponentsResponse)
        {
            this.GetIndexComponentsResponse = GetIndexComponentsResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetCurrencyInstrumentsIn
    {
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetCurrencyInstrumentsOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetCurrencyInstrumentsResponse GetCurrencyInstrumentsResponse;

        public GetCurrencyInstrumentsOut()
        {
        }

        public GetCurrencyInstrumentsOut(GetCurrencyInstrumentsResponse GetCurrencyInstrumentsResponse)
        {
            this.GetCurrencyInstrumentsResponse = GetCurrencyInstrumentsResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class ConvertCurrencyIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public ConvertCurrencyRequest ConvertCurrencyRequest;

        public ConvertCurrencyIn()
        {
        }

        public ConvertCurrencyIn(ConvertCurrencyRequest ConvertCurrencyRequest)
        {
            this.ConvertCurrencyRequest = ConvertCurrencyRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class ConvertCurrencyOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public ConvertCurrencyResponse ConvertCurrencyResponse;

        public ConvertCurrencyOut()
        {
        }

        public ConvertCurrencyOut(ConvertCurrencyResponse ConvertCurrencyResponse)
        {
            this.ConvertCurrencyResponse = ConvertCurrencyResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetInstrumentByDialectRequest", WrapperNamespace = "http://service.marketwatch.com/ws/2007/10/financialdata", IsWrapped = true)]
    public class GetInstrumentByDialectRequest
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 2)] public string Dialect;
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public InstrumentByDialectRequest[] InstrumentRequests;

        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 3)] public int? MaxInstrumentMatches;
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 1)] public XmlQualifiedName[] Needed;

        public GetInstrumentByDialectRequest()
        {
        }

        public GetInstrumentByDialectRequest(InstrumentByDialectRequest[] instrumentRequests, XmlQualifiedName[] needed, string dialect, int? maxInstrumentMatches)
        {
            InstrumentRequests = instrumentRequests;
            Needed = needed;
            Dialect = dialect;
            MaxInstrumentMatches = maxInstrumentMatches;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(WrapperName = "GetInstrumentByDialectResponse", WrapperNamespace = "http://service.marketwatch.com/ws/2007/10/financialdata", IsWrapped = true)]
    public class GetInstrumentByDialectResponse
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public InstrumentByDialectResponse[] InstrumentResponses;

        public GetInstrumentByDialectResponse()
        {
        }

        public GetInstrumentByDialectResponse(InstrumentByDialectResponse[] instrumentResponses)
        {
            this.InstrumentResponses = instrumentResponses;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetIndexesIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetIndexesRequest GetIndexesRequest;

        public GetIndexesIn()
        {
        }

        public GetIndexesIn(GetIndexesRequest GetIndexesRequest)
        {
            this.GetIndexesRequest = GetIndexesRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetIndexesOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetIndexesResponse GetIndexesResponse;

        public GetIndexesOut()
        {
        }

        public GetIndexesOut(GetIndexesResponse GetIndexesResponse)
        {
            this.GetIndexesResponse = GetIndexesResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetOptionMonthsIn
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetOptionMonthsRequest GetOptionMonthsRequest;

        public GetOptionMonthsIn()
        {
        }

        public GetOptionMonthsIn(GetOptionMonthsRequest GetOptionMonthsRequest)
        {
            this.GetOptionMonthsRequest = GetOptionMonthsRequest;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetOptionMonthsOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetOptionMonthsResponse GetOptionMonthsResponse;

        public GetOptionMonthsOut()
        {
        }

        public GetOptionMonthsOut(GetOptionMonthsResponse GetOptionMonthsResponse)
        {
            this.GetOptionMonthsResponse = GetOptionMonthsResponse;
        }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetFutureInstrumentsIn
    {
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [MessageContract(IsWrapped = false)]
    public class GetFutureInstrumentsOut
    {
        [MessageBodyMember(Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata", Order = 0)] public GetFutureInstrumentsResponse GetFutureInstrumentsResponse;

        public GetFutureInstrumentsOut()
        {
        }

        public GetFutureInstrumentsOut(GetFutureInstrumentsResponse GetFutureInstrumentsResponse)
        {
            this.GetFutureInstrumentsResponse = GetFutureInstrumentsResponse;
        }
    }

    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface IInstrumentChannel : IInstrument, IClientChannel
    {
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class InstrumentClient : ClientBase<IInstrument>, IInstrument
    {
        public InstrumentClient()
        {
        }

        public InstrumentClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public InstrumentClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public InstrumentClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public InstrumentClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        #region Instrument Members

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetInstrumentOut IInstrument.GetInstrument(GetInstrumentIn request)
        {
            return Channel.GetInstrument(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetEntityOut IInstrument.GetEntity(GetEntityIn request)
        {
            return Channel.GetEntity(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetExchangeSummaryOut IInstrument.GetExchangeSummary(GetExchangeSummaryIn request)
        {
            return Channel.GetExchangeSummary(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PhoneticSearchOut IInstrument.PhoneticSearch(PhoneticSearchIn request)
        {
            return Channel.PhoneticSearch(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetOptionChainOut IInstrument.GetOptionChain(GetOptionChainIn request)
        {
            return Channel.GetOptionChain(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetIndexComponentsOut IInstrument.GetIndexComponents(GetIndexComponentsIn request)
        {
            return Channel.GetIndexComponents(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetCurrencyInstrumentsOut IInstrument.GetCurrencyInstruments(GetCurrencyInstrumentsIn request)
        {
            return Channel.GetCurrencyInstruments(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        ConvertCurrencyOut IInstrument.ConvertCurrency(ConvertCurrencyIn request)
        {
            return Channel.ConvertCurrency(request);
        }

        GetInstrumentByDialectResponse IInstrument.GetInstrumentByDialect(GetInstrumentByDialectRequest request)
        {
            return Channel.GetInstrumentByDialect(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetIndexesOut IInstrument.GetIndexes(GetIndexesIn request)
        {
            return Channel.GetIndexes(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetOptionMonthsOut IInstrument.GetOptionMonths(GetOptionMonthsIn request)
        {
            return Channel.GetOptionMonths(request);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        GetFutureInstrumentsOut IInstrument.GetFutureInstruments(GetFutureInstrumentsIn request)
        {
            return Channel.GetFutureInstruments(request);
        }

        #endregion

        public GetInstrumentResponse GetInstrument(GetInstrumentRequest getInstrumentRequest)
        {
            var inValue = new GetInstrumentIn
                              {
                                  GetInstrumentRequest = getInstrumentRequest
                              };
            var retVal = ((IInstrument) (this)).GetInstrument(inValue);
            return retVal.GetInstrumentResponse;
        }

        public GetEntityResponse GetEntity(GetEntityRequest getEntityRequest)
        {
            var inValue = new GetEntityIn
                              {
                                  GetEntityRequest = getEntityRequest
                              };
            var retVal = ((IInstrument) (this)).GetEntity(inValue);
            return retVal.GetEntityResponse;
        }

        public GetExchangeSummaryResponse GetExchangeSummary(GetExchangeSummaryRequest getExchangeSummaryRequest)
        {
            var inValue = new GetExchangeSummaryIn
                              {
                                  GetExchangeSummaryRequest = getExchangeSummaryRequest
                              };
            var retVal = ((IInstrument) (this)).GetExchangeSummary(inValue);
            return retVal.GetExchangeSummaryResponse;
        }

        public PhoneticSearchResponse PhoneticSearch(PhoneticSearchRequest phoneticSearchRequest)
        {
            var inValue = new PhoneticSearchIn
                              {
                                  PhoneticSearchRequest = phoneticSearchRequest
                              };
            var retVal = ((IInstrument) (this)).PhoneticSearch(inValue);
            return retVal.PhoneticSearchResponse;
        }

        public GetOptionChainResponse GetOptionChain(GetOptionChainRequest getOptionChainRequest)
        {
            var inValue = new GetOptionChainIn
                              {
                                  GetOptionChainRequest = getOptionChainRequest
                              };
            var retVal = ((IInstrument) (this)).GetOptionChain(inValue);
            return retVal.GetOptionChainResponse;
        }

        public GetIndexComponentsResponse GetIndexComponents(GetIndexComponentsRequest getIndexComponentsRequest)
        {
            var inValue = new GetIndexComponentsIn
                              {
                                  GetIndexComponentsRequest = getIndexComponentsRequest
                              };
            var retVal = ((IInstrument) (this)).GetIndexComponents(inValue);
            return retVal.GetIndexComponentsResponse;
        }

        public GetCurrencyInstrumentsResponse GetCurrencyInstruments()
        {
            var inValue = new GetCurrencyInstrumentsIn();
            var retVal = ((IInstrument) (this)).GetCurrencyInstruments(inValue);
            return retVal.GetCurrencyInstrumentsResponse;
        }

        public ConvertCurrencyResponse ConvertCurrency(ConvertCurrencyRequest convertCurrencyRequest)
        {
            var inValue = new ConvertCurrencyIn
                              {
                                  ConvertCurrencyRequest = convertCurrencyRequest
                              };
            var retVal = ((IInstrument) (this)).ConvertCurrency(inValue);
            return retVal.ConvertCurrencyResponse;
        }

        public InstrumentByDialectResponse[] GetInstrumentByDialect(InstrumentByDialectRequest[] instrumentRequests, XmlQualifiedName[] needed, string dialect, int? maxInstrumentMatches)
        {
            var inValue = new GetInstrumentByDialectRequest
                              {
                                  InstrumentRequests = instrumentRequests,
                                  Needed = needed,
                                  Dialect = dialect,
                                  MaxInstrumentMatches = maxInstrumentMatches
                              };
            var retVal = ((IInstrument) (this)).GetInstrumentByDialect(inValue);
            return retVal.InstrumentResponses;
        }

        public GetIndexesResponse GetIndexes(GetIndexesRequest getIndexesRequest)
        {
            var inValue = new GetIndexesIn
                              {
                                  GetIndexesRequest = getIndexesRequest
                              };
            var retVal = ((IInstrument) (this)).GetIndexes(inValue);
            return retVal.GetIndexesResponse;
        }

        public GetOptionMonthsResponse GetOptionMonths(GetOptionMonthsRequest getOptionMonthsRequest)
        {
            var inValue = new GetOptionMonthsIn {GetOptionMonthsRequest = getOptionMonthsRequest};
            var retVal = ((IInstrument) (this)).GetOptionMonths(inValue);
            return retVal.GetOptionMonthsResponse;
        }

        public GetFutureInstrumentsResponse GetFutureInstruments()
        {
            var inValue = new GetFutureInstrumentsIn();
            var retVal = ((IInstrument) (this)).GetFutureInstruments(inValue);
            return retVal.GetFutureInstrumentsResponse;
        }
    }
}