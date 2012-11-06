using System;
using System.Collections.Generic;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.Messages.PIM.V1_0;
using System.Runtime.Remoting.Messaging;
using DowJones.API.Common.Logging;
using DowJones.API.Common.ExceptionHandling;
using System.ServiceModel.Web;
using System.Net;
using System.Threading;
using Factiva.Gateway.Services.V1_0;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Text;
using DowJones.DependencyInjection;
using System.ServiceModel;
using DowJones.Factiva.Currents.Aggregrator;


namespace DowJones.API.Common.Utilities
{
    public static class PimUtility
    {
        public static void PublishToPim(DateTime requestStartTime, IServiceResponse response, bool succeeded, IRequest request = null, ControlData controlData = null)
        {
            var incomingRequest = WebOperationContext.Current.IncomingRequest;
            var outgoingResponse = WebOperationContext.Current.OutgoingResponse;
            string apiType = "PLATFORM_REST";

            // Setting Audit Log
            DowJones.API.Common.Utilities.Web.SetAuditLog(response);
            DowJones.API.Common.Utilities.Web.SetARMValues(response);

            if (!ApiFramework.PimLoggingEnabled)
                return;

            try
            {
                LogEntry logEntry = null;
                DateTime responseTime = DateTime.Now;
                object objTrans = CallContext.GetData(Constants.TransactionLog);

                if (objTrans != null)
                    logEntry = (LogEntry)objTrans;
                else
                    logEntry = new LogEntry();

                logEntry.Status = succeeded ? TransactionStatus.Succeeded : TransactionStatus.Failed;

                CallContext.SetData(Constants.TransactionLog, null);

                logEntry.Certification = false;// indicates whether the transaction is marked for certification
                logEntry.Version = Constants.ApiVersion;
                logEntry.ClientAddress = Utilities.Web.GetRemoteAddr();
                logEntry.ServerAddress = Environment.MachineName;

                if (logEntry.ControlData.SessionId == null && logEntry.ControlData.EncryptedId == null)
                {
                    logEntry.ControlData = new LogEntryControlData
                    {
                        AccountId = controlData.AccountID,
                        EncryptedId = controlData.EncryptedLogin,
                        UserId = controlData.UserID,
                        Namespace = controlData.ProductID,
                        SessionId = controlData.SessionID
                    };
                }

                logEntry.TransactionType = TransactionType.General;
                
                //Commenting
                //if (incomingRequest.UriTemplateMatch != null)
                //{
                //    logEntry.TransactionName = GetTransactionName(incomingRequest.UriTemplateMatch.RequestUri.AbsolutePath, logEntry.Version);
                //    logEntry.RequestUri = incomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri;

                //    if (logEntry.RequestUri.Contains("/modules/", StringComparison.CurrentCultureIgnoreCase))
                //        apiType = "MODULE_REST";
                //}

                // API type that is processing the transaction.  Acceptable values at this time are:
                //   FDK_SOAP - Traditional FDK
                //   FDK_REST - Simple API for FDK
                //   PLATFORM_REST - New REST API
                //   DASHBOARD_REST - Dashboard API
                logEntry.ApiType = apiType;

                logEntry.RequestHeader = incomingRequest.Headers.ToString();
                logEntry.RequestMethod = GetHttpMethodType().ToString();
                logEntry.RequestTime = requestStartTime;

                if (request != null)
                    logEntry.RequestBody = Serialization.Serialize(request);
                else
                    logEntry.RequestBody = OperationContext.Current.RequestContext.RequestMessage.ToString();

                if (string.IsNullOrWhiteSpace(logEntry.RequestBody))
                    logEntry.RequestBody = logEntry.RequestUri;

                logEntry.ResponseContentType = outgoingResponse.ContentType;
                logEntry.ResponseBody = Serialization.Serialize(response);
                logEntry.ResponseTime = DateTime.Now;

                // PIM service should be always invoked asynchronously, 
                // as not to delay client-side procesing.

                ThreadPool.QueueUserWorkItem(delegate
                {
                    PIMService.BeginPublishLogEntry(controlData,
                                                     new PublishLogEntryRequest { LogEntry = logEntry },
                                                     null,
                                                     null);
                });
            }
            catch (Exception ex)
            {
                ApiLog.Logger.ErrorFormat("***************  ERROR IN PIM logging **********************");
                ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, ex.Message);
            }

        }

        //public static void PublishToPimXML(DateTime requestStartTime, BaseXMLResponse response, bool succeeded, IRequest request = null, ControlData controlData = null)
        //{
        //    var incomingRequest = WebOperationContext.Current.IncomingRequest;
        //    var outgoingResponse = WebOperationContext.Current.OutgoingResponse;
        //    string apiType = "PLATFORM_REST";

        //    // Setting Audit Log
        //    DowJones.API.Common.Utilities.Web.SetAuditLog(response);
        //    DowJones.API.Common.Utilities.Web.SetARMValues(response);

        //    if (!ApiFramework.PimLoggingEnabled)
        //        return;

        //    try
        //    {
        //        LogEntry logEntry = null;
        //        DateTime responseTime = DateTime.Now;
        //        object objTrans = CallContext.GetData(Constants.TransactionLog);

        //        if (objTrans != null)
        //            logEntry = (LogEntry)objTrans;
        //        else
        //            logEntry = new LogEntry();

        //        logEntry.Status = succeeded ? TransactionStatus.Succeeded : TransactionStatus.Failed;

        //        CallContext.SetData(Constants.TransactionLog, null);

        //        if (controlData == null)
        //            controlData = ControlDataUtility.InitControlData(ServiceLocator.Resolve<Credentials>());

        //        logEntry.Certification = false;// indicates whether the transaction is marked for certification
        //        logEntry.Version = Constants.ApiVersion;
        //        logEntry.ClientAddress = Utilities.Web.GetRemoteAddr();
        //        logEntry.ServerAddress = Environment.MachineName;

        //        if (logEntry.ControlData.SessionId == null && logEntry.ControlData.EncryptedId == null)
        //        {
        //            logEntry.ControlData = new LogEntryControlData
        //            {
        //                AccountId = controlData.AccountID,
        //                EncryptedId = controlData.EncryptedLogin,
        //                UserId = controlData.UserID,
        //                Namespace = controlData.ProductID,
        //                SessionId = controlData.SessionID
        //            };
        //        }

        //        logEntry.TransactionType = TransactionType.General;

        //        if (incomingRequest.UriTemplateMatch != null)
        //        {
        //            logEntry.TransactionName = GetTransactionName(incomingRequest.UriTemplateMatch.RequestUri.AbsolutePath, logEntry.Version);
        //            logEntry.RequestUri = incomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri;

        //            if (logEntry.RequestUri.Contains("/modules/", StringComparison.CurrentCultureIgnoreCase))
        //                apiType = "MODULE_REST";
        //        }

        //        // API type that is processing the transaction.  Acceptable values at this time are:
        //        //   FDK_SOAP - Traditional FDK
        //        //   FDK_REST - Simple API for FDK
        //        //   PLATFORM_REST - New REST API
        //        //   DASHBOARD_REST - Dashboard API
        //        logEntry.ApiType = apiType;

        //        logEntry.RequestHeader = incomingRequest.Headers.ToString();
        //        logEntry.RequestMethod = GetHttpMethodType().ToString();
        //        logEntry.RequestTime = requestStartTime;

        //        if (request != null)
        //            logEntry.RequestBody = Serialization.Serialize(request);
        //        else
        //            logEntry.RequestBody = OperationContext.Current.RequestContext.RequestMessage.ToString();

        //        if (string.IsNullOrWhiteSpace(logEntry.RequestBody))
        //            logEntry.RequestBody = logEntry.RequestUri;

        //        logEntry.ResponseContentType = outgoingResponse.ContentType;
        //        logEntry.ResponseBody = Serialization.Serialize(response);
        //        logEntry.ResponseTime = DateTime.Now;

        //        // PIM service should be always invoked asynchronously, 
        //        // as not to delay client-side procesing.

        //        ThreadPool.QueueUserWorkItem(delegate
        //        {
        //            PIMService.BeginPublishLogEntry(controlData,
        //                                             new PublishLogEntryRequest { LogEntry = logEntry },
        //                                             null,
        //                                             null);
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        ApiLog.Logger.ErrorFormat("***************  ERROR IN PIM logging **********************");
        //        ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, ex.Message);
        //    }

        //}

        public static HttpMethodType GetHttpMethodType()
        {
            if (WebOperationContext.Current == null || string.IsNullOrEmpty(WebOperationContext.Current.IncomingRequest.Method))
                return HttpMethodType.Get;

            switch (WebOperationContext.Current.IncomingRequest.Method.ToLower())
            {
                case "post":
                    return HttpMethodType.Post;
                case "delete":
                    return HttpMethodType.Delete;
                case "put":
                    return HttpMethodType.Put;
                default:
                    return HttpMethodType.Get;
            }
        }

        private static string GetTransactionName(string absolutePath, string version)
        {
            string transactionName = absolutePath;

            if (transactionName.Contains(version + "/"))
            {
                transactionName = transactionName.Substring(transactionName.IndexOf(version + "/") + version.Length + 1);
                transactionName = transactionName.Replace(".svc", "");
            }
            if (transactionName.EndsWith("/xml", StringComparison.CurrentCultureIgnoreCase)
                || transactionName.EndsWith("/json", StringComparison.CurrentCultureIgnoreCase))
            {
                transactionName = transactionName.Remove(transactionName.LastIndexOf("/"));
            }

            return transactionName + "_" + GetHttpMethodType();
        }

        public static void AddTraceTransactionToCurrentThread(DateTime requestTime, string transactionName, long status, ControlData controlData)
        {
            if (ApiFramework.AuthenticationEnabled && ApiFramework.PimLoggingEnabled)
            {
                try
                {
                    LogEntry objPimLog = null;
                    DateTime responseTime = DateTime.Now;
                    object objTrans = CallContext.GetData(Constants.TransactionLog);

                    if (objTrans != null)
                        objPimLog = (LogEntry)objTrans;
                    else
                        objPimLog = new LogEntry();

                    if (objPimLog.Trace == null)
                        objPimLog.Trace = new List<LogEntryTrace>();

                    LogEntryTrace transaction = new LogEntryTrace();
                    transaction.TransactionName = transactionName;
                    transaction.RequestTime = requestTime;
                    transaction.ResponseTime = responseTime;

                    if (controlData != null)
                    {
                        if (objPimLog.ControlData.SessionId == null && objPimLog.ControlData.EncryptedId == null)
                            objPimLog.ControlData = new LogEntryControlData
                            {
                                AccountId = controlData.AccountID,
                                EncryptedId = controlData.EncryptedLogin,
                                UserId = controlData.UserID,
                                Namespace = controlData.ProductID,
                                SessionId = controlData.SessionID
                            };
                    }

                    transaction.Status = Convert.ToInt32(status);

                    //if (status == 0)
                    //{
                    //    transaction.IsSuccess = true;
                    //}
                    //else
                    //{
                    //    transaction.IsSuccess = false;
                    //    Error objError = new Error();
                    //    objError.Code = status;
                    //    transaction.ErrorMessage = new ErrorResponse(objError);
                    //}
                    objPimLog.Trace.Add(transaction);
                    CallContext.SetData(Constants.TransactionLog, objPimLog);
                }
                catch (Exception Ex)
                {
                    ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, Ex.Message);
                }
            }
        }

        public static void AddTraceTransactionToCurrentThread(DateTime requestTime, string transactionName, long status, ControlData controlData, object requestObject, object responseObject, string details)
        {
            try
            {
                if (ApiFramework.AuthenticationEnabled)
                {
                    if (ApiFramework.PimLoggingEnabled)
                    {
                        AddTraceTransactionToCurrentThread(requestTime, transactionName, status, controlData);
                    }
                    if (Web.IsDebugOn())
                    {
                        AddTraceTransactionToAudit(requestTime, transactionName, status, controlData, requestObject, responseObject, details);
                    }
                }
            }
            catch (Exception Ex)
            {
                ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, Ex.Message);
            }
        }

        //public static void AddTraceTransactionToCurrentThread(DateTime requestTime, string transactionName, long status, Credentials credentials, object requestObject, object responseObject)
        //{
        //    try
        //    {
                //if (ApiFramework.AuthenticationEnabled)
                //{
                //    if (ApiFramework.PimLoggingEnabled)
                //    {
                //        AddTraceTransactionToCurrentThread(requestTime, transactionName, status, ControlDataUtility.InitControlData(credentials));
                //    }
                //    if (Web.IsDebugOn())
                //    {
                //        AddTraceTransactionToAudit(requestTime, transactionName, status, ControlDataUtility.InitControlData(credentials), requestObject, responseObject, string.Empty);
                //    }
                //}
        //    }
        //    catch (Exception Ex)
        //    {
        //        ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, Ex.Message);
        //    }
        //}

        /// <summary>
        /// Adds the trace transaction to audit.
        /// </summary>
        /// <param name="requestStartTime">The request start time.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        /// <param name="status">The status.</param>
        /// <param name="controlData">The control data.</param>
        /// <param name="requestObject">The request object.</param>
        /// <param name="responseObject">The response object.</param>
        /// <param name="details">The details.</param>
        public static void AddTraceTransactionToAudit(DateTime requestStartTime, string transactionName, long status, ControlData controlData, object requestObject, object responseObject, string details)
        {
            if (ApiFramework.AuthenticationEnabled && Web.IsDebugOn())
            {
                try
                {
                    AuditLog auditLog = null;
                    DateTime responseTime = DateTime.Now;

                    object objTrans = CallContext.GetData(Constants.AuditLog);

                    if (objTrans != null)
                        auditLog = (AuditLog)objTrans;
                    else
                        auditLog = new AuditLog();

                    if (auditLog.AuditTransactionTrace == null)
                        auditLog.AuditTransactionTrace = new List<AuditTransaction>();

                    AuditTransaction transaction = new AuditTransaction();
                    transaction.Name = transactionName;
                    transaction.RequestDateTime = requestStartTime;
                    transaction.ResponseDateTime = responseTime;
                    transaction.Details = details;
                    transaction.ElapsedTime = Convert.ToInt32((responseTime - requestStartTime).TotalMilliseconds);
                    transaction.ReturnCode = status;

                    if (requestObject != null)
                        transaction.RawRequest = Serialization.SerializeObjectToString(requestObject); //Serialization.Serialize(requestObject);

                    if (controlData != null)
                    {
                        object fcsControlData = controlData.ToFCSControlData();
                        transaction.ControlData = Serialization.Serialize(fcsControlData);
                    }

                    if (status == 0)
                    {
                        transaction.RawResponse = Serialization.SerializeObjectToString(responseObject);
                    }
                    else
                    {
                        transaction.RawResponse = (string)responseObject;
                    }

                    auditLog.AuditTransactionTrace.Add(transaction);

                    CallContext.SetData(Constants.AuditLog, auditLog);
                }
                catch (Exception Ex)
                {
                    ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, Ex.Message);
                }
            }
        }
    }
}
