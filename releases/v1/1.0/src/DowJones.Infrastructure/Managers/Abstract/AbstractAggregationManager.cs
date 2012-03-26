// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractAggregationManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DowJones.DependencyInjection;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using Factiva.Gateway.Messages.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Utilities.Managers
{
    /// <summary>
    /// Base class for AbstractAggregationManager.
    /// </summary>
    public abstract class AbstractAggregationManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAggregationManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        protected AbstractAggregationManager(ControlData controlData)
        {
            ControlData = controlData;
            
            if (Log != null)
            {
                TransactionLogger = new TransactionLogger(Log);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAggregationManager"/> class.
        /// </summary>
        /// <param name="sessionID">The session id.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        protected AbstractAggregationManager(string sessionID, string clientTypeCode, string accessPointCode)
            : this(Factiva.Gateway.Managers.ControlDataManager.GetSessionUserControlData(sessionID, clientTypeCode, accessPointCode))
        {
        }

        protected AbstractAggregationManager()
        {
            throw new NotImplementedException();
        }

        public ushort LastContentServerAddress { get; private set; }

        public ControlData LastTransactionControlData { get; private set; }

        public string LastRawResponse { get; private set; }

        /// <summary>
        /// Gets or sets the control data.
        /// </summary>
        /// <value>The control data.</value>
        public ControlData ControlData { get; protected internal set; }

        /// <summary>
        /// Gets or sets the content server address.
        /// </summary>
        /// <value>The content server address.</value>
        public ushort ContentServerAddress
        {
            get
            {
                return ControlData != null ? LastContentServerAddress : (ushort)0;
            }

            set
            {
                if (ControlData != null)
                {
                    ControlData.ContentServerAddress = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Transaction logger.
        /// </summary>
        /// <value>
        /// The transaction logger.
        /// </value>
        [Inject("Injected to avoid a base constructor call in derived classes")]
        internal TransactionLogger TransactionLogger { get; set; }

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
        protected internal ServiceResponse<T> Invoke<T>(object request, ControlData baseControlData = null,  bool copyContentServerAddress = false)
        {
            TransactionLogger.Reset();
            var controlData = ControlData;
            if (baseControlData != null)
            {
                controlData = baseControlData;
            }

            var response = FactivaServices.Invoke<T>(ControlDataManager.Clone(controlData, copyContentServerAddress), request);

            if (response == null)
            {
                throw new DowJonesUtilitiesException(new NullReferenceException("ServiceResponse is null"), -1);
            }

            var responseControlData = response.GetControlData();
            LastRawResponse = response.RawResponse as string;
            LastContentServerAddress = responseControlData.ContentServerAddress;
            LastTransactionControlData = responseControlData;

            if (response.rc != 0)
            {
                DeleteFromCache(responseControlData, ControlDataManager.Clone(controlData, copyContentServerAddress));
                var message = string.Format("\n   RequestType: {0}\n   ResponseControlData.TransactionType: {1}",
                                            request.GetType().FullName,
                                            responseControlData.TransactionType);
                throw new DowJonesUtilitiesException(message, response.rc);
            }

            TransactionLogger.LogTimeSpentSinceReset("Invoke");
            return response;
        }
        

        /// <summary>
        /// Performs the specified request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <param name="baseControlData">The cache enable control data.</param>
        /// <param name="copyContentServerAddress">if set to <c>true</c> [copy content server address].</param>
        /// <returns></returns>
        protected internal ServiceResponse<T> Invoke<T>(GatewayMessage request, ControlData baseControlData = null, bool copyContentServerAddress = false)
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
        protected internal T Process<T>(object requestObject, ControlData controlData = null, bool copyContentServerAddress = true)
        {
            if (controlData == null)
            {
                controlData = ControlData;
            }

            ServiceResponse<T> sr;
            try
            {
                sr = Invoke<T>(requestObject, controlData, copyContentServerAddress);
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
        /// <param name="failedControlData">The failed control data.</param>
        /// <param name="controlData">The control data.</param>
        protected internal void DeleteFromCache(ControlData failedControlData, ControlData controlData)
        {
            if (string.IsNullOrEmpty(failedControlData.CacheKey))
            {
                return;
            }

            controlData.CacheExpirationPolicy = string.Empty;
            controlData.CacheExpirationTime = string.Empty;
            controlData.CacheKey = string.Empty;
            controlData.CacheRefreshInterval = string.Empty;
            controlData.CacheStatus = CacheStatus.NotSpecified;
            controlData.CacheWait = String.Empty;

            var request = new DeleteItemRequest
            {
                Key = failedControlData.CacheKey,
                Scope = (Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope)Enum.Parse(typeof(Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope), failedControlData.CacheScope)
            };
            try
            {
                FactivaServices.Invoke<DeleteItemResponse>(controlData, request);
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled)
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
