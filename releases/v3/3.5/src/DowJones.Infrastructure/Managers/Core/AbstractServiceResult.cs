using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Globalization;
using DowJones.Utilities;
using log4net;

namespace DowJones.Managers.Core
{
    [DataContract(Namespace = "")]
    public abstract class AbstractServiceResult<TServicePartResult, TPackage> : AbstractServiceResult, IServicePartResults<TServicePartResult, TPackage>
        where TServicePartResult : IServicePartResult<TPackage>
        where TPackage : IPackage
    {

        [DataMember(Name = "partResults")]
        public virtual IEnumerable<TServicePartResult> PartResults { get; set; }

        [DataMember(Name = "maxPartsAvailable")]
        public int MaxPartsAvailable { get; set; }

        protected virtual void ProcessServicePartResult<TPackageType>(MethodBase method, IServicePartResult<TPackage> servicePartResult, Action processingFunction)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            DowJonesUtilitiesException djException = null;
            try
            {
                servicePartResult.PackageType = typeof(TPackageType).GetName();
                processingFunction.Invoke();
            }
            catch (SoapException ex)
            {
                long errorCode;
                if (!long.TryParse(ex.Message, out errorCode))
                    errorCode = -1;
                djException = new DowJonesUtilitiesException(ex, errorCode);
            }
            catch (WebException wex)
            {
                djException = new DowJonesUtilitiesException(wex, 58700);
            }
            catch (DowJonesUtilitiesException dex)
            {
                djException = dex;
            }
            catch (AggregateException ex)
            {
                djException = ExtractAggregateException(ex);
            }
            catch (Exception exception)
            {
                djException = new DowJonesUtilitiesException(exception, -1);
            }

            if (djException != null)
            {
                servicePartResult.ReturnCode = djException.ReturnCode;
                servicePartResult.StatusMessage = Resources.GetErrorMessage(djException.ReturnCode.ToString());
            }

            stopwatch.Stop();
            servicePartResult.ElapsedTime = stopwatch.ElapsedMilliseconds;
        }
    }

    [DataContract(Namespace = "")]
    public abstract class AbstractServiceResult : IServiceResult
    {
        private ILog _log;
        private TaskFactory _taskFactory;
        private IResourceTextManager _resources;
        private volatile ConcurrentQueue<Transaction> transactions = new ConcurrentQueue<Transaction>();

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        [DataMember(Name = "returnCode")]
        //// [XmlElement(Type = typeof (int), ElementName = "returnCode", IsNullable = false, Form = XmlSchemaForm.Qualified,
        //    Namespace = "")]
        [XmlElement(ElementName = "returnCode")]
        public long ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        [DataMember(Name = "statusMessage")]
        [XmlElement(ElementName = "statusMessage")]
        //// [XmlElement(Type = typeof (string), ElementName = "statusMessage", IsNullable = false,
            //    Form = XmlSchemaForm.Qualified, Namespace = "")]
            public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        [DataMember(Name = "elapsedTime")]
        public long ElapsedTime { get; set; }

        [DataMember(Name = "audit", EmitDefaultValue = false)]
        public Audit Audit { get; set; }

        public ILog Logger
        {
            get { return _log ?? (_log = LogManager.GetLogger(GetType())); }
            set { _log = value; }
        }

        public IResourceTextManager Resources
        {
            get { return _resources = _resources ?? ServiceLocator.Resolve<IResourceTextManager>(); }
            set { _resources = value; }
        }

        protected TaskFactory TaskFactory
        {
            get { return _taskFactory ?? TaskFactoryManager.Instance.GetDefaultTaskFactory(); }
            set { _taskFactory = value; }
        }

        
        protected virtual void ProcessServiceResult(MethodBase method, Action processingFunction)
        {
            var stopwatch = Stopwatch.StartNew();
            Audit = new Audit();
            try
            {
                processingFunction.Invoke();
            }
            catch (SoapException ex)
            {
                long errorCode;
                if (!long.TryParse(ex.Message, out errorCode))
                    errorCode = -1;
                UpdateServiceResult(new DowJonesUtilitiesException(ex, errorCode));
            }
            catch (WebException wex)
            {
                UpdateServiceResult(new DowJonesUtilitiesException(wex, 58700));
            }
            catch (DowJonesUtilitiesException dex)
            {
                UpdateServiceResult(dex);
            }
            catch (AggregateException ex)
            {
                UpdateServiceResult(ExtractAggregateException(ex));
            }
            catch (Exception exception)
            {
                UpdateServiceResult(new DowJonesUtilitiesException(exception, -1));
            }
            finally
            {
                stopwatch.Stop();
                Audit.Name = GetType().Name;
                ElapsedTime = Audit.ElapsedTime = stopwatch.ElapsedMilliseconds;
                Audit.TransactionCollection = new TransactionCollection(transactions);
                transactions.Clear();

                if (ReturnCode != 0)
                {
                    Audit.ReturnCode = ReturnCode;
                }
            }
        }


        protected internal static DowJonesUtilitiesException ExtractAggregateException(AggregateException aggregateException)
        {
            // just extract the first non-aggregate exception
            // return it if it is DowJonesUtilitiesException
            // or return generic -1 exception
            // TODO: maybe go down the innerexceptions tree to find first DowJonesUtilitiesException to return
            foreach (var exception in aggregateException.InnerExceptions)
            {
                if (exception is AggregateException)
                {
                    return ExtractAggregateException((AggregateException)exception);
                }

                if (exception is DowJonesUtilitiesException)
                {
                    return (DowJonesUtilitiesException)exception;
                }
            }

            return new DowJonesUtilitiesException(aggregateException, -1);
        }

        protected void UpdateServiceResult(DowJonesUtilitiesException emgEx)
        {
            ReturnCode = emgEx.ReturnCode;
            StatusMessage = Resources.GetErrorMessage(ReturnCode.ToString());
        }

        protected virtual void RecordTransaction(string name, string detail, Action processingFunction)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var transaction = new Transaction(name, detail);
            try
            {
                processingFunction.Invoke();
            }
            catch (DowJonesUtilitiesException ex)
            {
                transaction.ReturnCode = ex.ReturnCode;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                transaction.ElapsedTime = stopwatch.ElapsedMilliseconds;
                transactions.Enqueue(transaction);
            }
        }

    }
}