// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DowJones.DependencyInjection;
using DowJones.Session;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Utilities.Configuration;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Namespace = "")]
    public abstract class AbstractServiceResult : IServiceResult
    {
        public const TruncationType DefaultTruncationType = TruncationType.Medium;
        private readonly LightWeightUser proxyUser;
        private ILog log;
        private volatile ConcurrentQueue<Transaction> transactions = new ConcurrentQueue<Transaction>();

        protected AbstractServiceResult()
        {
            PageAssetsManagerFactory = new ChacheEnabledPageAssetsManagerFactory();
            proxyUser = SnapshotProxyUserManager.Instance.GetProxyUser();
        }

        public delegate ControlData ControlDataAction();

        #region IServiceResult Members

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

        #endregion

        protected internal LightWeightUser ProxyUser
        {
            get { return proxyUser; }
        }

        protected IChacheEnabledPageAssetsManagerFactory PageAssetsManagerFactory { get; set; }

        [Inject("Injected to avoid constructor injection requirement in derived classes")]
        protected ILog Logger
        {
            get { return log ?? (log = LogManager.GetLogger(GetType())); }
        }

        [Inject("Injected to avoid constructor injection requirement in derived classes")]
        protected TaskFactory TaskFactory
        {
            get { return TaskFactoryManager.Instance.GetDefaultTaskFactory(); }
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
            StatusMessage = ResourceTextManager.Instance.GetErrorMessage(ReturnCode.ToString());
        }

       
        protected virtual void RecordTransaction(string name, string detail, Action processingFunction, ControlData controlData = null)
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
                transaction.ControlData = controlData;
                transaction.ElapsedTime = stopwatch.ElapsedMilliseconds;
                transactions.Enqueue(transaction);
            }
        }

        protected virtual void RecordTransaction(string name, string detail, Action<PageListManager> processingFunction, PageListManager manager)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var transaction = new Transaction(name, detail);
            try
            {
                processingFunction.Invoke(manager);
            }
            catch (DowJonesUtilitiesException ex)
            {
                transaction.ReturnCode = ex.ReturnCode;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                transaction.ControlData = manager.LastTransactionControlData;
                transaction.RawResponse = manager.LastRawResponse;
                transaction.ElapsedTime = stopwatch.ElapsedMilliseconds;
                transactions.Enqueue(transaction);
            }
        }

        protected virtual void RecordTransaction(string name, string detail, Action<AbstractAggregationManager> processingFunction, AbstractAggregationManager manager)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var transaction = new Transaction(name, detail);
            try
            {
                processingFunction.Invoke(manager);
            }
            catch (DowJonesUtilitiesException ex)
            {
                transaction.ReturnCode = ex.ReturnCode;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                transaction.ControlData = manager.LastTransactionControlData;
                transaction.RawResponse = manager.LastRawResponse;
                transaction.ElapsedTime = stopwatch.ElapsedMilliseconds;
                transactions.Enqueue(transaction);
            }
        }

        protected virtual void RecordTransaction(string name, string detail, Action<SearchManager> processingFunction, SearchManager manager) 
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var transaction = new Transaction(name, detail);
            try
            {
                processingFunction.Invoke(manager);
            }
            catch (DowJonesUtilitiesException ex)
            {
                transaction.ReturnCode = ex.ReturnCode;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                transaction.ControlData = manager.LastTransactionControlData;
                transaction.RawResponse = manager.LastRawResponse;
                transaction.ElapsedTime = stopwatch.ElapsedMilliseconds;
                transactions.Enqueue(transaction);
            }
        }

        protected virtual void ProcessServiceResult(MethodBase method, Action processingFunction, IPreferences preferences)
        {
            var stopwatch = Stopwatch.StartNew();
            Audit = new Audit();
            try
            {
                LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);
                processingFunction.Invoke();
            }
            catch (WebException wex)
            {
                throw new DowJonesUtilitiesException(wex, 58700);
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

        protected TModule GetModule<TModule>(IModuleRequest request, ControlData controlData, IPreferences preferences)
            where TModule : Factiva.Gateway.Messages.Assets.Pages.V1_0.Module
        {
            var module = GetModule(request.PageId, request.ModuleId, controlData, preferences);

            if (module is TModule)
            {
                return module as TModule;
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleTypeNotExpected);
        }

        protected Factiva.Gateway.Messages.Assets.Pages.V1_0.Module GetModule(string pageId, string moduleId, ControlData controlData, IPreferences preferences)
        {
            Factiva.Gateway.Messages.Assets.Pages.V1_0.Module module = null;
            RecordTransaction(
                "PageAssetsManager.GetModuleById",
                null,
                manager =>
                {
                    module = manager.GetModuleById(pageId, moduleId);
                },
                PageAssetsManagerFactory.CreateManager(controlData, preferences.InterfaceLanguage) as PageListManager);

            if (module == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleIsNull);
            }
            return module;
        }

        protected BasePreferences GetPreferences(ControlData controlData)
        {
            BasePreferences preferences = null;
            RecordTransaction(
                               "PreferencesUtilites.GetBasePreferences",
                               null,
                               () =>
                               {
                                   preferences = PreferencesUtilites.GetBasePreferences(controlData);
                               });

            return preferences;
        }


    }
}