// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerValueDashboardService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CustomerValueDashboardService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using DowJones.Security.Interfaces;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The customer value dashboard service.
    /// </summary>
    public class CustomerValueDashboardService : AbstractService, ICustomerValueDashboardService
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
            get { return 134; }
        }

        #endregion

        #endregion
    }
}