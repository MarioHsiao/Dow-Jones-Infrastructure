// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using DowJones.DependencyInjection;
using DowJones.Managers.PAM;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using Factiva.Gateway.Utils.V1_0;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common
{
    public abstract class BaseManager
    {
        protected BaseManager()
        {
            ChacheEnabledPageAssetsManagerFactory = new ChacheEnabledPageAssetsManagerFactory();    
        }

        // [Ninject.Inject]
        protected ILog Logger { get; set; }

        //[Ninject.Inject]
        protected Interfaces.IChacheEnabledPageAssetsManagerFactory ChacheEnabledPageAssetsManagerFactory { get; set; }

        protected static void UpdateServiceResult(DowJonesUtilitiesException emgEx, IServiceResult serviceResult)
        {
            UpdateServiceResult(emgEx, serviceResult, null);
        }

        protected static void UpdateServiceResult(DowJonesUtilitiesException emgEx, IServiceResult serviceResult, TransactionLogger transactionLogger)
        {
            serviceResult.ReturnCode = emgEx.ReturnCode;
            serviceResult.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(serviceResult.ReturnCode.ToString());
            if (transactionLogger != null)
            {
                serviceResult.ElapsedTime = transactionLogger.ElapsedTimeSinceInvocation;
            }
        }

        protected virtual void ProcessRequest(MethodBase method, IServiceResult serviceResult, ControlData controlData, IPreferences preferences, Action<IServiceResult, IPageAssetsManager> processingFunction)
        {
            using (new TransactionLogger(Logger, method))
            {
                try
                {
                    var manager = ChacheEnabledPageAssetsManagerFactory.CreateManager(controlData, preferences.InterfaceLanguage);
                    processingFunction.Invoke(serviceResult, manager);
                }
                catch (DowJonesUtilitiesException rEx)
                {
                    UpdateServiceResult(rEx, serviceResult);
                }
                catch (Exception exception)
                {
                    UpdateServiceResult(new DowJonesUtilitiesException(exception, -1), serviceResult);
                }
            }
        }
    }
}
