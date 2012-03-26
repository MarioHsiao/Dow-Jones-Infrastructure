﻿////------------------------------------------------------------------------------
//// <auto-generated>
////     This code was generated by a tool.
////     Runtime Version:2.0.50727.3607
////
////     Changes to this file may cause incorrect behavior and will be lost if
////     the code is regenerated.
//// </auto-generated>
////------------------------------------------------------------------------------

//using System;
//using System.CodeDom.Compiler;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Runtime.Serialization;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using DowJones.Thunderball.Library.Charting;

//[assembly:
//    ContractNamespace( "http://service.marketwatch.com/ws/2009/03/chartservice",
//        ClrNamespace = "service.marketwatch.com.ws._2009._03.chartservice" )]

//namespace DowJones.Thunderball.Library
//{
//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "GetChartRequest", Namespace = "http://schemas.datacontract.org/2004/07/Thunderball.Protocol" )]
//    public class GetChartRequest : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public DateTime? EndDate
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string EntitlementToken
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string Freq
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int Height
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string RequestId
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public DateTime? StartDate
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string[] Symbol
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string Time
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int Width
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int XAxisPadding
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int YAxisPadding
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "SessionUnit", Namespace = "http://schemas.datacontract.org/2004/07/Thunderball.Protocol" )]
//    public enum SessionUnit
//    {
//        [EnumMember]
//        Minute = 0,

//        [EnumMember]
//        Hour = 1,

//        [EnumMember]
//        Day = 2,

//        [EnumMember]
//        Month = 3,

//        [EnumMember]
//        Quarter = 4,

//        [EnumMember]
//        Year = 5,
//    }

//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "RegionType", Namespace = "http://schemas.datacontract.org/2004/07/Thunderball.Protocol" )]
//    public enum RegionType
//    {
//        [EnumMember]
//        Premarket = 0,

//        [EnumMember]
//        Postmarket = 1,

//        [EnumMember]
//        Intersession = 2,

//        [EnumMember]
//        EndOfDay = 3,

//        [EnumMember]
//        Market = 4,
//    }
//}

//namespace DowJones.Thunderball.Library.Charting
//{
//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "ChartDataResponse", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class ChartDataResponse : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public double BarFactor
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int BarSize
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public bool CustomDateRange
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public Dictionary<string, SessionInfo> Data
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string Error
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public XAxisLabelInfo xAxisLabelInfo
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public XAxisDataMembers xAxisLabelsObj
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "ChartVMLResponse", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class ChartVMLResponse : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public string Error
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string Vml
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "XAxisLabelInfo", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class XAxisLabelInfo : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public SessionUnit chartUnit
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public SessionUnit displayUnit
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "XAxisDataMembers", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class XAxisDataMembers : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public AxisLabel[] ElementList
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public double[] timeLines
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "SessionInfo", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class SessionInfo : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public string BlueGrassChannel
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int IsIndex
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string Name
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public Session[] Sessions
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "Session", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class Session : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public PricePoint High
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public PricePoint Low
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public NewsLink[] News
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public double? PreviousClose
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public TradingRegion[] Regions
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public DateTime Start
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public DateTime Stop
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public double?[] Trades
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int Weight
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "PricePoint", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class PricePoint : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public int Index
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public double Price
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "NewsLink", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class NewsLink : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public int At
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string Text
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public string Url
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "TradingRegion", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class TradingRegion : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public int Start
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public int Stop
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public RegionType Type
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }

//    [DebuggerStepThrough]
//    [GeneratedCode( "System.Runtime.Serialization", "3.0.0.0" )]
//    [DataContract( Name = "AxisLabel", Namespace = "http://service.marketwatch.com/ws/2009/03/chartservice" )]
//    public class AxisLabel : object, IExtensibleDataObject
//    {
//        [DataMember]
//        public string element
//        {
//            get;
//            set;
//        }

//        [DataMember]
//        public double x
//        {
//            get;
//            set;
//        }

//        #region IExtensibleDataObject Members

//        public ExtensionDataObject ExtensionData
//        {
//            get;
//            set;
//        }

//        #endregion
//    }
//}

//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//[ServiceContract( ConfigurationName = "IChartService" )]
//public interface IChartService
//{
//    // CODEGEN: Generating message contract since the wrapper namespace (http://service.marketwatch.com/ws/2009/03/chartservice) of message GetChartRequests does not match the default value (http://tempuri.org/)
//    [OperationContract( Action = "http://tempuri.org/IChartService/GetChart",
//        ReplyAction = "http://tempuri.org/IChartService/GetChartResponse" )]
//    GetChartResponse GetChart( GetChartRequests request );

//    // CODEGEN: Generating message contract since the wrapper namespace (http://service.marketwatch.com/ws/2009/03/chartservice) of message GetChartRequest does not match the default value (http://tempuri.org/)
//    [OperationContract( Action = "http://tempuri.org/IChartService/GetVMLChart",
//        ReplyAction = "http://tempuri.org/IChartService/GetVMLChartResponse" )]
//    GetVMLChartResponse GetVMLChart( GetChartRequest request );

//    // CODEGEN: Generating message contract since the wrapper namespace (http://service.marketwatch.com/ws/2009/03/chartservice) of message GetChartStyleRequest does not match the default value (http://tempuri.org/)
//    [OperationContract( Action = "http://tempuri.org/IChartService/GetChartStyle",
//        ReplyAction = "http://tempuri.org/IChartService/GetChartStyleResponse" )]
//    GetChartStyleResponse GetChartStyle( GetChartStyleRequest request );
//}

//[DebuggerStepThrough]
//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//[MessageContract( WrapperName = "GetChartRequests",
//    WrapperNamespace = "http://service.marketwatch.com/ws/2009/03/chartservice", IsWrapped = true )]
//public class GetChartRequests
//{
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 0 )]
//    public DowJones.Thunderball.Library.GetChartRequest[]
//        Requests;

//    public GetChartRequests()
//    {
//    }

//    public GetChartRequests( DowJones.Thunderball.Library.GetChartRequest[] Requests )
//    {
//        this.Requests = Requests;
//    }
//}

//[DebuggerStepThrough]
//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//[MessageContract( WrapperName = "GetChartResponse",
//    WrapperNamespace = "http://service.marketwatch.com/ws/2009/03/chartservice", IsWrapped = true )]
//public class GetChartResponse
//{
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 0 )]
//    public Dictionary<string, ChartDataResponse>
//        ChartData;

//    public GetChartResponse()
//    {
//    }

//    public GetChartResponse( Dictionary<string, ChartDataResponse> ChartData )
//    {
//        this.ChartData = ChartData;
//    }
//}

//[DebuggerStepThrough]
//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//[MessageContract( WrapperName = "GetChartRequest",
//    WrapperNamespace = "http://service.marketwatch.com/ws/2009/03/chartservice", IsWrapped = true )]
//public class GetChartRequest
//{
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 8 )]
//    public DateTime? EndDate;
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 0 )]
//    public string EntitlementToken;
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 5 )]
//    public string Freq;
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 10 )]
//    public int Height;

//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 1 )]
//    public string RequestId;
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 7 )]
//    public DateTime? StartDate;

//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 4 )]
//    public string[] Symbol;

//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 6 )]
//    public string Time;

//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 9 )]
//    public int Width;
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 2 )]
//    public int XAxisPadding;

//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 3 )]
//    public int YAxisPadding;

//    public GetChartRequest()
//    {
//    }

//    public GetChartRequest( string EntitlementToken, string RequestId, int XAxisPadding, int YAxisPadding,
//                           string[] Symbol, string Freq, string Time, DateTime? StartDate, DateTime? EndDate, int Width,
//                           int Height )
//    {
//        this.EntitlementToken = EntitlementToken;
//        this.RequestId = RequestId;
//        this.XAxisPadding = XAxisPadding;
//        this.YAxisPadding = YAxisPadding;
//        this.Symbol = Symbol;
//        this.Freq = Freq;
//        this.Time = Time;
//        this.StartDate = StartDate;
//        this.EndDate = EndDate;
//        this.Width = Width;
//        this.Height = Height;
//    }
//}

//[DebuggerStepThrough]
//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//[MessageContract( WrapperName = "GetVMLChartResponse",
//    WrapperNamespace = "http://service.marketwatch.com/ws/2009/03/chartservice", IsWrapped = true )]
//public class GetVMLChartResponse
//{
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 0 )]
//    public ChartVMLResponse VMLChartData;

//    public GetVMLChartResponse()
//    {
//    }

//    public GetVMLChartResponse( ChartVMLResponse VMLChartData )
//    {
//        this.VMLChartData = VMLChartData;
//    }
//}

//[DebuggerStepThrough]
//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//[MessageContract( WrapperName = "GetChartStyleRequest",
//    WrapperNamespace = "http://service.marketwatch.com/ws/2009/03/chartservice", IsWrapped = true )]
//public class GetChartStyleRequest
//{
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 0 )]
//    public string StyleName;

//    public GetChartStyleRequest()
//    {
//    }

//    public GetChartStyleRequest( string StyleName )
//    {
//        this.StyleName = StyleName;
//    }
//}

//[DebuggerStepThrough]
//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//[MessageContract( WrapperName = "GetChartStyleResponse",
//    WrapperNamespace = "http://service.marketwatch.com/ws/2009/03/chartservice", IsWrapped = true )]
//public class GetChartStyleResponse
//{
//    [MessageBodyMember( Namespace = "http://tempuri.org/", Order = 0 )]
//    public string StyleData;

//    public GetChartStyleResponse()
//    {
//    }

//    public GetChartStyleResponse( string StyleData )
//    {
//        this.StyleData = StyleData;
//    }
//}

//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//public interface IChartServiceChannel : IChartService, IClientChannel
//{
//}

//[DebuggerStepThrough]
//[GeneratedCode( "System.ServiceModel", "3.0.0.0" )]
//public class ChartServiceClient : ClientBase<IChartService>, IChartService
//{
//    public ChartServiceClient()
//    {
//    }

//    public ChartServiceClient( string endpointConfigurationName ) :
//        base( endpointConfigurationName )
//    {
//    }

//    public ChartServiceClient( string endpointConfigurationName, string remoteAddress ) :
//        base( endpointConfigurationName, remoteAddress )
//    {
//    }

//    public ChartServiceClient( string endpointConfigurationName, EndpointAddress remoteAddress ) :
//        base( endpointConfigurationName, remoteAddress )
//    {
//    }

//    public ChartServiceClient( Binding binding, EndpointAddress remoteAddress ) :
//        base( binding, remoteAddress )
//    {
//    }

//    #region IChartService Members

//    [EditorBrowsable( EditorBrowsableState.Advanced )]
//    GetChartResponse IChartService.GetChart( GetChartRequests request )
//    {
//        return Channel.GetChart( request );
//    }

//    [EditorBrowsable( EditorBrowsableState.Advanced )]
//    GetVMLChartResponse IChartService.GetVMLChart( GetChartRequest request )
//    {
//        return Channel.GetVMLChart( request );
//    }

//    [EditorBrowsable( EditorBrowsableState.Advanced )]
//    GetChartStyleResponse IChartService.GetChartStyle( GetChartStyleRequest request )
//    {
//        return Channel.GetChartStyle( request );
//    }

//    #endregion

//    public Dictionary<string, ChartDataResponse> GetChart( DowJones.Thunderball.Library.GetChartRequest[] Requests )
//    {
//        var inValue = new GetChartRequests
//                          {
//                              Requests = Requests
//                          };
//        GetChartResponse retVal = ( ( IChartService ) ( this ) ).GetChart( inValue );
//        return retVal.ChartData;
//    }

//    public ChartVMLResponse GetVMLChart( string EntitlementToken, string RequestId, int XAxisPadding, int YAxisPadding,
//                                        string[] Symbol, string Freq, string Time, DateTime? StartDate,
//                                        DateTime? EndDate, int Width, int Height )
//    {
//        var inValue = new GetChartRequest
//                          {
//                              EntitlementToken = EntitlementToken, 
//                              RequestId = RequestId, 
//                              XAxisPadding = XAxisPadding, 
//                              YAxisPadding = YAxisPadding, 
//                              Symbol = Symbol, 
//                              Freq = Freq, 
//                              Time = Time, 
//                              StartDate = StartDate, 
//                              EndDate = EndDate, 
//                              Width = Width, 
//                              Height = Height
//                          };
//        var retVal = ( ( IChartService ) ( this ) ).GetVMLChart( inValue );
//        return retVal.VMLChartData;
//    }

//    public string GetChartStyle( string StyleName )
//    {
//        var inValue = new GetChartStyleRequest
//                          {
//                              StyleName = StyleName
//                          };
//        var retVal = ( ( IChartService ) ( this ) ).GetChartStyle( inValue );
//        return retVal.StyleData;
//    }
//}