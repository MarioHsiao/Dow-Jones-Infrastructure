// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Extensions;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Generic
{
    [DataContract(Namespace = "")]
    public abstract class AbstractServiceResult<TServicePartResult, TPackage> : AbstractServiceResult, IServicePartResults<TServicePartResult, TPackage>
        where TServicePartResult : IServicePartResult<TPackage>
        where TPackage : IPackage
    {
        protected AbstractServiceResult()
        {
            PageAssetsManagerFactory = new ChacheEnabledPageAssetsManagerFactory();
        }

        #region Implementation of IServicePartResults

        [DataMember(Name = "partResults")]
        public virtual IEnumerable<TServicePartResult> PartResults { get; set; }

        [DataMember(Name = "maxPartsAvailable")]
        public int MaxPartsAvailable { get; set; }

        #endregion

        protected static IEnumerable<T> GetPage<T>(IEnumerable<T> list, NewsPageHeadlineModuleDataRequest request)
        {
            var lowerBound = request.FirstPartToReturn;
            var upperBound = lowerBound + request.MaxPartsToReturn;

            if (upperBound <= list.Count())
            {
                return list.ToArray().Slice(lowerBound, upperBound);
            }

            if (lowerBound < list.Count())
            {
                var tempArray = list.ToArray();
                var len = tempArray.Length;
                return tempArray.Slice(lowerBound, len);
            }

            return new T[0];
        }

        protected static void UpdateServiceResult(DowJonesUtilitiesException emgEx, IServicePartResult<TPackage> servicePartResult)
        {
            servicePartResult.ReturnCode = emgEx.ReturnCode;
            servicePartResult.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(servicePartResult.ReturnCode.ToString());
        }

        protected virtual void ProcessServicePartResult<TPackageType>(MethodBase method, IServicePartResult<TPackage> servicePartResult, Action processingFunction, IPreferences preferences)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                LanguageUtilityManager.SetThreadCulture(preferences.InterfaceLanguage);
                servicePartResult.PackageType = typeof(TPackageType).GetName();
                processingFunction.Invoke();
            }
            catch (WebException wex)
            {
                // throw new DowJonesUtilitiesException(wex, 58700);
                UpdateServiceResult(new DowJonesUtilitiesException(new DowJonesUtilitiesException(wex, 58700), -1), servicePartResult);
            }
            catch (DowJonesUtilitiesException dex)
            {
                UpdateServiceResult(dex, servicePartResult);
            }
            catch (AggregateException ex)
            {
                UpdateServiceResult(ExtractAggregateException(ex), servicePartResult);
            }
            catch (Exception exception)
            {
                UpdateServiceResult(new DowJonesUtilitiesException(exception, -1), servicePartResult);
            }

            stopwatch.Stop();
            servicePartResult.ElapsedTime = stopwatch.ElapsedMilliseconds;
        }
    }
}
