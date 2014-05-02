// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractAggregationManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Loggers;
using DowJones.Session;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using Factiva.Gateway.Messages.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Managers.Abstract
{
    /// <summary>
    /// Base class for AbstractAggregationManager.
    /// </summary>
    public abstract class AbstractAggregationManager : IAggregationManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAggregationManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="transactionTimer"></param>
        protected AbstractAggregationManager(IControlData controlData, ITransactionTimer transactionTimer = null)
        {
            ControlData = controlData;
            TransactionTimer = transactionTimer ?? new BasicTransactionTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAggregationManager"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="transactionTimer"></param>
        protected AbstractAggregationManager(string sessionId, string clientTypeCode, string accessPointCode, ITransactionTimer transactionTimer)
            : this(new ControlData{SessionID = sessionId, ClientType = clientTypeCode, AccessPointCode = accessPointCode}, transactionTimer)
        {
        }
        
        public IControlData LastTransactionControlData { get; private set; }

        public string LastRawResponse { get; private set; }

        /// <summary>
        /// Gets or sets the control data.
        /// </summary>
        /// <value>The control data.</value>
        public IControlData ControlData { get; protected internal set; }

        public ITransactionTimer TransactionTimer { get; protected internal set; }
        
        /*/// <summary>
        /// Gets or sets the Transaction logger.
        /// </summary>
        /// <value>
        /// The transaction logger.
        /// </value>
        [Inject("Injected to avoid a base constructor call in derived classes")]
        internal TransactionLogger TransactionLogger { get; set; }
*/
        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log associated w/object.</value>
        protected abstract ILog Log { get; }

        #region protected internal methods

        /// <summary>
        /// Performs the specified request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <param name="baseControlData">The cache enable control data.</param>
        /// <param name="copyContentServerAddress">if set to <c>true</c> [copy content server address].</param>
        /// <returns></returns>
        public ServiceResponse<T> Invoke<T>(object request, IControlData baseControlData = null, bool copyContentServerAddress = false)
        {
            using (TransactionTimer.Start(Log, MethodBase.GetCurrentMethod()))
            {
                if (Log.IsInfoEnabled)
                {
                    Log.InfoFormat("Request Name: {0}", request.GetType().FullName);
                }

                var controlData = ControlData;
                if (baseControlData != null)
                {
                    controlData = baseControlData;
                }

                var response = FactivaServices.Invoke<T>(ControlDataManager.Convert(controlData, copyContentServerAddress), request);

                if (response == null)
                {
                    throw new DowJonesUtilitiesException(new NullReferenceException("ServiceResponse is null"), -1);
                }

                var responseControlData = response.GetControlData();
                LastRawResponse = response.RawResponse as string;
                LastTransactionControlData = ControlDataManager.Convert(responseControlData);

                if (response.rc != 0)
                {
                    DeleteFromCache(responseControlData.CacheKey, responseControlData.CacheScope, controlData);
                    var message = string.Format("\n   ReturnCode: {0}\n   RequestType: {1}\n   ResponseControlData.TransactionType: {2}",
                                                response.ReturnCode,
                                                request.GetType().FullName,
                                                responseControlData.TransactionType);
                    throw new DowJonesUtilitiesException(message, response.rc);
                }
                return response;
            }
        }
        

        /// <summary>
        /// Performs the specified request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <param name="baseControlData">The cache enable control data.</param>
        /// <param name="copyContentServerAddress">if set to <c>true</c> [copy content server address].</param>
        /// <returns></returns>
        public ServiceResponse<T> Invoke<T>(GatewayMessage request, IControlData baseControlData = null, bool copyContentServerAddress = false)
        {
            return Invoke<T>((object)request, baseControlData, copyContentServerAddress);
        }

        /// <summary>
        /// The process.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject">The requestObject.</param>
        /// <param name="controlData">The control data.</param>
        /// <param name="copyContentServerAddress">The copy content server address.</param>
        /// <returns></returns>
        /// <exception cref="DowJonesUtilitiesException" />
        protected internal T Process<T>(object requestObject, IControlData controlData = null, bool copyContentServerAddress = true)
        {
            if (controlData == null)
            {
                controlData = ControlData;
            }

            try
            {
                var sr = Invoke<T>(requestObject, controlData, copyContentServerAddress);
                return sr.ObjectResponse;
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }
        }

        #endregion

        /// <summary>
        /// Serializes the specified requestObject.
        /// </summary>
        /// <param name="obj">The requestObject.</param>
        /// <returns>
        /// The serialize.
        /// </returns>
        protected internal static string Serialize(object obj)
        {
            // Serialization
            var serializer = new XmlSerializer(obj.GetType());
            var stringWriter = new StringWriter();

            // create an xml-writer and then write nothing to this to fake and remove xml declaration
            var xw = new XmlTextWriter(stringWriter) { Formatting = Formatting.None };

            // serialize the request data.
            serializer.Serialize(xw, obj);
            return stringWriter.ToString();
        }


        /// <summary>
        /// Serializes the with no XML declaration.
        /// </summary>
        /// <param name="obj">The requestObject.</param>
        /// <returns>
        /// The serialize with no xml declaration.
        /// </returns>
        protected internal static string SerializeWithNoXmlDeclaration(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var stringWriter = new StringWriter();

            // create an xml-writer and then write nothing to this to fake and remove xml declaration
            var xw = new XmlTextWriter(stringWriter) { Formatting = Formatting.None };
            xw.WriteRaw(string.Empty);

            // serialize the request data.
            serializer.Serialize(xw, obj);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Deletes failed data from session cache.
        /// </summary>
        /// <param name="cacheScope"></param>
        /// <param name="controlData">The control data.</param>
        /// <param name="cacheKey"></param>
        protected internal void DeleteFromCache(string cacheKey, string cacheScope, IControlData controlData)
        {
            try
            {
                if (string.IsNullOrEmpty(cacheKey))
                {
                    return;
                }

                var tempControlData = ControlDataManager.Clone(controlData);
                tempControlData.CacheExpirationPolicy = string.Empty;
                tempControlData.CacheExpirationTime = string.Empty;
                tempControlData.CacheKey = string.Empty;
                tempControlData.CacheRefreshInterval = string.Empty;
                tempControlData.CacheStatus = CacheStatus.NotSpecified;
                tempControlData.CacheWait = String.Empty;

                var request = new DeleteItemRequest
                {
                    Key = cacheKey,
                    Scope = (CacheScope)Enum.Parse(typeof(CacheScope), cacheScope)
                };
                FactivaServices.Invoke<DeleteItemResponse>(ControlDataManager.Convert(tempControlData), request);
            }
            catch (Exception ex)
            {
                if (Log != null && Log.IsErrorEnabled)
                {
                    Log.Error(ex.StackTrace);
                }
            }
        }


        /// <summary>
        /// De-serializes the specified XML request.
        /// </summary>
        /// <param name="xmlRequest">The XML request.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// The de-serialized object.
        /// </returns>
        protected internal static object Deserialize(string xmlRequest, Type objectType)
        {
            // System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequest);
            if (xmlDoc.DocumentElement != null)
            {
                var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);
                var xs = new XmlSerializer(objectType);
                var obj = xs.Deserialize(xmlReader);
                return obj;
            }

            return null;
        }
    }
}
