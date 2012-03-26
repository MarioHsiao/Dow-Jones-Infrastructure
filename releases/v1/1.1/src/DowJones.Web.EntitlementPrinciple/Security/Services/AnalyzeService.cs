// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyzeService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AnalyzeService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using DowJones.Web.EntitlementPrinciple.Security.Interfaces;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
{
    /// <summary>
    /// The analyze service.
    /// </summary>
    public class AnalyzeService : AbstractService, IAnalyzeService
    {
        #region Properties

        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether IsOn.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override bool IsOn
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether HasOffset.
        /// </summary>
        public override bool HasOffset
        {
            get { return true; }
        }

        /// <summary>
        /// Gets Offset.
        /// </summary>
        public override int Offset
        {
            get { return 136; }
        }

        #endregion

        #endregion
    }
}