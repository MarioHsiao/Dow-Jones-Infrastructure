// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FISPrivateMarketsService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the FISPrivateMarketsService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using DowJones.Security.Interfaces;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The fis private markets service.
    /// </summary>
    public class FISPrivateMarketsService : AbstractService, IFISPrivateMarketsService
    {
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
            get { return 108; }
        }

        #endregion
    }
}